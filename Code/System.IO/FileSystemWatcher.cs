// Decompiled with JetBrains decompiler
// Type: System.IO.FileSystemWatcher
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml

using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO.Enumeration;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Listens to the file system change notifications and raises events when a directory, or file in a directory, changes.</summary>
  public class FileSystemWatcher : Component, ISupportInitialize
  {

    #nullable disable
    private readonly FileSystemWatcher.NormalizedFilterCollection _filters = new FileSystemWatcher.NormalizedFilterCollection();
    private string _directory;
    private NotifyFilters _notifyFilters = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
    private bool _includeSubdirectories;
    private bool _enabled;
    private bool _initializing;
    private uint _internalBufferSize = 8192;
    private bool _disposed;
    private FileSystemEventHandler _onChangedHandler;
    private FileSystemEventHandler _onCreatedHandler;
    private FileSystemEventHandler _onDeletedHandler;
    private RenamedEventHandler _onRenamedHandler;
    private ErrorEventHandler _onErrorHandler;
    private int _currentSession;
    private SafeFileHandle _directoryHandle;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemWatcher" /> class.</summary>
    public FileSystemWatcher() => this._directory = string.Empty;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemWatcher" /> class, given the specified directory to monitor.</summary>
    /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> parameter is an empty string ("").
    /// 
    /// -or-
    /// 
    /// The path specified through the <paramref name="path" /> parameter does not exist.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> is too long.</exception>
    public FileSystemWatcher(string path)
    {
      FileSystemWatcher.CheckPathValidity(path);
      this._directory = path;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemWatcher" /> class, given the specified directory and type of files to monitor.</summary>
    /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
    /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="filter" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> parameter is an empty string ("").
    /// 
    /// -or-
    /// 
    /// The path specified through the <paramref name="path" /> parameter does not exist.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> is too long.</exception>
    public FileSystemWatcher(string path, string filter)
    {
      FileSystemWatcher.CheckPathValidity(path);
      this._directory = path;
      this.Filter = filter ?? throw new ArgumentNullException(nameof (filter));
    }

    /// <summary>Gets or sets the type of changes to watch for.</summary>
    /// <exception cref="T:System.ArgumentException">The value is not a valid bitwise OR combination of the <see cref="T:System.IO.NotifyFilters" /> values.</exception>
    /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value that is being set is not valid.</exception>
    /// <returns>One of the <see cref="T:System.IO.NotifyFilters" /> values. The default is the bitwise OR combination of <see langword="LastWrite" />, <see langword="FileName" />, and <see langword="DirectoryName" />.</returns>
    public NotifyFilters NotifyFilter
    {
      get => this._notifyFilters;
      set
      {
        if ((value & ~(NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime | NotifyFilters.Security)) != (NotifyFilters) 0)
          throw new ArgumentException(SR.Format(SR.InvalidEnumArgument, (object) nameof (value), (object) (int) value, (object) "NotifyFilters"));
        if (this._notifyFilters == value)
          return;
        this._notifyFilters = value;
        this.Restart();
      }
    }

    /// <summary>Gets the collection of all the filters used to determine what files are monitored in a directory.</summary>
    /// <returns>A filter collection.</returns>
    public Collection<string> Filters => (Collection<string>) this._filters;

    /// <summary>Gets or sets a value indicating whether the component is enabled.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.FileSystemWatcher" /> object has been disposed.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The directory specified in <see cref="P:System.IO.FileSystemWatcher.Path" /> could not be found.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <see cref="P:System.IO.FileSystemWatcher.Path" /> has not been set or is invalid.</exception>
    /// <returns>
    /// <see langword="true" /> if the component is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />. If you are using the component on a designer in Visual Studio 2005, the default is <see langword="true" />.</returns>
    public bool EnableRaisingEvents
    {
      get => this._enabled;
      set
      {
        if (this._enabled == value)
          return;
        if (this.IsSuspended())
          this._enabled = value;
        else if (value)
          this.StartRaisingEventsIfNotDisposed();
        else
          this.StopRaisingEvents();
      }
    }

    /// <summary>Gets or sets the filter string used to determine what files are monitored in a directory.</summary>
    /// <returns>The filter string. The default is "*.*" (Watches all files.)</returns>
    public string Filter
    {
      get => this.Filters.Count != 0 ? this.Filters[0] : "*";
      set
      {
        this.Filters.Clear();
        this.Filters.Add(value);
      }
    }

    /// <summary>Gets or sets a value indicating whether subdirectories within the specified path should be monitored.</summary>
    /// <returns>
    /// <see langword="true" /> if you want to monitor subdirectories; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
    public bool IncludeSubdirectories
    {
      get => this._includeSubdirectories;
      set
      {
        if (this._includeSubdirectories == value)
          return;
        this._includeSubdirectories = value;
        this.Restart();
      }
    }

    /// <summary>Gets or sets the size (in bytes) of the internal buffer.</summary>
    /// <returns>The internal buffer size in bytes. The default is 8192 (8 KB).</returns>
    public int InternalBufferSize
    {
      get => (int) this._internalBufferSize;
      set
      {
        if ((long) this._internalBufferSize == (long) value)
          return;
        this._internalBufferSize = value >= 4096 ? (uint) value : 4096U;
        this.Restart();
      }
    }


    #nullable disable
    private byte[] AllocateBuffer()
    {
      try
      {
        return new byte[(int) this._internalBufferSize];
      }
      catch (OutOfMemoryException ex)
      {
        throw new OutOfMemoryException(SR.Format(SR.BufferSizeTooLarge, (object) this._internalBufferSize));
      }
    }


    #nullable enable
    /// <summary>Gets or sets the path of the directory to watch.</summary>
    /// <exception cref="T:System.ArgumentException">The specified path does not exist or could not be found.
    /// 
    /// -or-
    /// 
    /// The specified path contains wildcard characters.
    /// 
    /// -or-
    /// 
    /// The specified path contains invalid path characters.</exception>
    /// <returns>The path to monitor. The default is an empty string ("").</returns>
    [Editor("System.Diagnostics.Design.FSWPathEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public string Path
    {
      get => this._directory;
      set
      {
        value = value == null ? string.Empty : value;
        if (string.Equals(this._directory, value, PathInternal.StringComparison))
          return;
        if (value.Length == 0)
          throw new ArgumentException(SR.Format(SR.InvalidDirName, (object) value), nameof (Path));
        this._directory = Directory.Exists(value) ? value : throw new ArgumentException(SR.Format(SR.InvalidDirName_NotExists, (object) value), nameof (Path));
        this.Restart();
      }
    }

    /// <summary>Occurs when a file or directory in the specified <see cref="P:System.IO.FileSystemWatcher.Path" /> is changed.</summary>
    public event FileSystemEventHandler? Changed
    {
      add => this._onChangedHandler += value;
      remove => this._onChangedHandler -= value;
    }

    /// <summary>Occurs when a file or directory in the specified <see cref="P:System.IO.FileSystemWatcher.Path" /> is created.</summary>
    public event FileSystemEventHandler? Created
    {
      add => this._onCreatedHandler += value;
      remove => this._onCreatedHandler -= value;
    }

    /// <summary>Occurs when a file or directory in the specified <see cref="P:System.IO.FileSystemWatcher.Path" /> is deleted.</summary>
    public event FileSystemEventHandler? Deleted
    {
      add => this._onDeletedHandler += value;
      remove => this._onDeletedHandler -= value;
    }

    /// <summary>Occurs when the instance of <see cref="T:System.IO.FileSystemWatcher" /> is unable to continue monitoring changes or when the internal buffer overflows.</summary>
    public event ErrorEventHandler? Error
    {
      add => this._onErrorHandler += value;
      remove => this._onErrorHandler -= value;
    }

    /// <summary>Occurs when a file or directory in the specified <see cref="P:System.IO.FileSystemWatcher.Path" /> is renamed.</summary>
    public event RenamedEventHandler? Renamed
    {
      add => this._onRenamedHandler += value;
      remove => this._onRenamedHandler -= value;
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.FileSystemWatcher" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (disposing)
        {
          this.StopRaisingEvents();
          this._onChangedHandler = (FileSystemEventHandler) null;
          this._onCreatedHandler = (FileSystemEventHandler) null;
          this._onDeletedHandler = (FileSystemEventHandler) null;
          this._onRenamedHandler = (RenamedEventHandler) null;
          this._onErrorHandler = (ErrorEventHandler) null;
        }
        else
          this.FinalizeDispose();
      }
      finally
      {
        this._disposed = true;
        base.Dispose(disposing);
      }
    }


    #nullable disable
    private static void CheckPathValidity(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Format(SR.InvalidDirName, (object) path), nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          if (Directory.Exists(path))
            break;
          throw new ArgumentException(SR.Format(SR.InvalidDirName_NotExists, (object) path), nameof (path));
      }
    }

    private bool MatchPattern(ReadOnlySpan<char> relativePath)
    {
      ReadOnlySpan<char> fileName = System.IO.Path.GetFileName(relativePath);
      if (fileName.Length == 0)
        return false;
      string[] filters = this._filters.GetFilters();
      if (filters.Length == 0)
        return true;
      foreach (string expression in filters)
      {
        if (FileSystemName.MatchesSimpleExpression((ReadOnlySpan<char>) expression, fileName, !PathInternal.IsCaseSensitive))
          return true;
      }
      return false;
    }

    private void NotifyInternalBufferOverflowEvent()
    {
      if (this._onErrorHandler == null)
        return;
      this.OnError(new ErrorEventArgs((Exception) new InternalBufferOverflowException(SR.Format(SR.FSW_BufferOverflow, (object) this._directory))));
    }

    private void NotifyRenameEventArgs(
      WatcherChangeTypes action,
      ReadOnlySpan<char> name,
      ReadOnlySpan<char> oldName)
    {
      if (this._onRenamedHandler == null || !this.MatchPattern(name) && !this.MatchPattern(oldName))
        return;
      this.OnRenamed(new RenamedEventArgs(action, this._directory, name.IsEmpty ? (string) null : name.ToString(), oldName.IsEmpty ? (string) null : oldName.ToString()));
    }

    private FileSystemEventHandler GetHandler(WatcherChangeTypes changeType)
    {
      switch (changeType)
      {
        case WatcherChangeTypes.Created:
          return this._onCreatedHandler;
        case WatcherChangeTypes.Deleted:
          return this._onDeletedHandler;
        case WatcherChangeTypes.Changed:
          return this._onChangedHandler;
        default:
          return (FileSystemEventHandler) null;
      }
    }

    private void NotifyFileSystemEventArgs(WatcherChangeTypes changeType, ReadOnlySpan<char> name)
    {
      FileSystemEventHandler handler = this.GetHandler(changeType);
      if (handler == null || !this.MatchPattern(name.IsEmpty ? (ReadOnlySpan<char>) this._directory : name))
        return;
      this.InvokeOn(new FileSystemEventArgs(changeType, this._directory, name.IsEmpty ? (string) null : name.ToString()), handler);
    }


    #nullable enable
    /// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Changed" /> event.</summary>
    /// <param name="e">A <see cref="T:System.IO.FileSystemEventArgs" /> that contains the event data.</param>
    protected void OnChanged(FileSystemEventArgs e) => this.InvokeOn(e, this._onChangedHandler);

    /// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Created" /> event.</summary>
    /// <param name="e">A <see cref="T:System.IO.FileSystemEventArgs" /> that contains the event data.</param>
    protected void OnCreated(FileSystemEventArgs e) => this.InvokeOn(e, this._onCreatedHandler);

    /// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Deleted" /> event.</summary>
    /// <param name="e">A <see cref="T:System.IO.FileSystemEventArgs" /> that contains the event data.</param>
    protected void OnDeleted(FileSystemEventArgs e) => this.InvokeOn(e, this._onDeletedHandler);


    #nullable disable
    private void InvokeOn(FileSystemEventArgs e, FileSystemEventHandler handler)
    {
      if (handler == null)
        return;
      ISynchronizeInvoke synchronizingObject = this.SynchronizingObject;
      if (synchronizingObject != null && synchronizingObject.InvokeRequired)
        synchronizingObject.BeginInvoke((Delegate) handler, new object[2]
        {
          (object) this,
          (object) e
        });
      else
        handler((object) this, e);
    }


    #nullable enable
    /// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Error" /> event.</summary>
    /// <param name="e">An <see cref="T:System.IO.ErrorEventArgs" /> that contains the event data.</param>
    protected void OnError(ErrorEventArgs e)
    {
      ErrorEventHandler onErrorHandler = this._onErrorHandler;
      if (onErrorHandler == null)
        return;
      ISynchronizeInvoke synchronizingObject = this.SynchronizingObject;
      if (synchronizingObject != null && synchronizingObject.InvokeRequired)
        synchronizingObject.BeginInvoke((Delegate) onErrorHandler, new object[2]
        {
          (object) this,
          (object) e
        });
      else
        onErrorHandler((object) this, e);
    }

    /// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Renamed" /> event.</summary>
    /// <param name="e">A <see cref="T:System.IO.RenamedEventArgs" /> that contains the event data.</param>
    protected void OnRenamed(RenamedEventArgs e)
    {
      RenamedEventHandler onRenamedHandler = this._onRenamedHandler;
      if (onRenamedHandler == null)
        return;
      ISynchronizeInvoke synchronizingObject = this.SynchronizingObject;
      if (synchronizingObject != null && synchronizingObject.InvokeRequired)
        synchronizingObject.BeginInvoke((Delegate) onRenamedHandler, new object[2]
        {
          (object) this,
          (object) e
        });
      else
        onRenamedHandler((object) this, e);
    }

    /// <summary>A synchronous method that returns a structure that contains specific information on the change that occurred, given the type of change you want to monitor.</summary>
    /// <param name="changeType">The <see cref="T:System.IO.WatcherChangeTypes" /> to watch for.</param>
    /// <returns>A <see cref="T:System.IO.WaitForChangedResult" /> that contains specific information on the change that occurred.</returns>
    public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType) => this.WaitForChanged(changeType, -1);

    /// <summary>A synchronous method that returns a structure that contains specific information on the change that occurred, given the type of change you want to monitor and the time (in milliseconds) to wait before timing out.</summary>
    /// <param name="changeType">The <see cref="T:System.IO.WatcherChangeTypes" /> to watch for.</param>
    /// <param name="timeout">The time (in milliseconds) to wait before timing out.</param>
    /// <returns>A <see cref="T:System.IO.WaitForChangedResult" /> that contains specific information on the change that occurred.</returns>
    public WaitForChangedResult WaitForChanged(
      WatcherChangeTypes changeType,
      int timeout)
    {
      TaskCompletionSource<WaitForChangedResult> tcs = new TaskCompletionSource<WaitForChangedResult>();
      FileSystemEventHandler systemEventHandler = (FileSystemEventHandler) null;
      RenamedEventHandler renamedEventHandler = (RenamedEventHandler) null;
      if ((changeType & (WatcherChangeTypes.Created | WatcherChangeTypes.Deleted | WatcherChangeTypes.Changed)) != (WatcherChangeTypes) 0)
      {
        systemEventHandler = (FileSystemEventHandler) ((s, e) =>
        {
          if ((e.ChangeType & changeType) == (WatcherChangeTypes) 0)
            return;
          tcs.TrySetResult(new WaitForChangedResult(e.ChangeType, e.Name, (string) null, false));
        });
        if ((changeType & WatcherChangeTypes.Created) != (WatcherChangeTypes) 0)
          this.Created += systemEventHandler;
        if ((changeType & WatcherChangeTypes.Deleted) != (WatcherChangeTypes) 0)
          this.Deleted += systemEventHandler;
        if ((changeType & WatcherChangeTypes.Changed) != (WatcherChangeTypes) 0)
          this.Changed += systemEventHandler;
      }
      if ((changeType & WatcherChangeTypes.Renamed) != (WatcherChangeTypes) 0)
      {
        renamedEventHandler = (RenamedEventHandler) ((s, e) =>
        {
          if ((e.ChangeType & changeType) == (WatcherChangeTypes) 0)
            return;
          tcs.TrySetResult(new WaitForChangedResult(e.ChangeType, e.Name, e.OldName, false));
        });
        this.Renamed += renamedEventHandler;
      }
      try
      {
        bool enableRaisingEvents = this.EnableRaisingEvents;
        if (!enableRaisingEvents)
          this.EnableRaisingEvents = true;
        tcs.Task.Wait(timeout);
        this.EnableRaisingEvents = enableRaisingEvents;
      }
      finally
      {
        if (renamedEventHandler != null)
          this.Renamed -= renamedEventHandler;
        if (systemEventHandler != null)
        {
          if ((changeType & WatcherChangeTypes.Changed) != (WatcherChangeTypes) 0)
            this.Changed -= systemEventHandler;
          if ((changeType & WatcherChangeTypes.Deleted) != (WatcherChangeTypes) 0)
            this.Deleted -= systemEventHandler;
          if ((changeType & WatcherChangeTypes.Created) != (WatcherChangeTypes) 0)
            this.Created -= systemEventHandler;
        }
      }
      return !tcs.Task.IsCompletedSuccessfully ? WaitForChangedResult.TimedOutResult : tcs.Task.Result;
    }

    private void Restart()
    {
      if (this.IsSuspended() || !this._enabled)
        return;
      this.StopRaisingEvents();
      this.StartRaisingEventsIfNotDisposed();
    }

    private void StartRaisingEventsIfNotDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(this.GetType().Name);
      this.StartRaisingEvents();
    }

    /// <summary>Gets or sets an <see cref="T:System.ComponentModel.ISite" /> for the <see cref="T:System.IO.FileSystemWatcher" />.</summary>
    /// <returns>An <see cref="T:System.ComponentModel.ISite" /> for the <see cref="T:System.IO.FileSystemWatcher" />.</returns>
    public override ISite? Site
    {
      get => base.Site;
      set
      {
        base.Site = value;
        if (this.Site == null || !this.Site.DesignMode)
          return;
        this.EnableRaisingEvents = true;
      }
    }

    /// <summary>Gets or sets the object used to marshal the event handler calls issued as a result of a directory change.</summary>
    /// <returns>The <see cref="T:System.ComponentModel.ISynchronizeInvoke" /> that represents the object used to marshal the event handler calls issued as a result of a directory change. The default is <see langword="null" />.</returns>
    public ISynchronizeInvoke? SynchronizingObject { get; set; }

    /// <summary>Begins the initialization of a <see cref="T:System.IO.FileSystemWatcher" /> used on a form or used by another component. The initialization occurs at run time.</summary>
    public void BeginInit()
    {
      bool enabled = this._enabled;
      this.StopRaisingEvents();
      this._enabled = enabled;
      this._initializing = true;
    }

    /// <summary>Ends the initialization of a <see cref="T:System.IO.FileSystemWatcher" /> used on a form or used by another component. The initialization occurs at run time.</summary>
    public void EndInit()
    {
      this._initializing = false;
      if (this._directory.Length == 0 || !this._enabled)
        return;
      this.StartRaisingEvents();
    }

    private bool IsSuspended() => this._initializing || this.DesignMode;

    private unsafe void StartRaisingEvents()
    {
      if (this.IsSuspended())
      {
        this._enabled = true;
      }
      else
      {
        if (!FileSystemWatcher.IsHandleInvalid(this._directoryHandle))
          return;
        this._directoryHandle = Interop.Kernel32.CreateFile(this._directory, 1, FileShare.ReadWrite | FileShare.Delete, FileMode.Open, 1107296256);
        if (FileSystemWatcher.IsHandleInvalid(this._directoryHandle))
        {
          this._directoryHandle = (SafeFileHandle) null;
          throw new FileNotFoundException(SR.Format(SR.FSW_IOError, (object) this._directory));
        }
        FileSystemWatcher.AsyncReadState state;
        try
        {
          int session = Interlocked.Increment(ref this._currentSession);
          byte[] numArray = this.AllocateBuffer();
          state = new FileSystemWatcher.AsyncReadState(session, numArray, this._directoryHandle, ThreadPoolBoundHandle.BindHandle((SafeHandle) this._directoryHandle), this);
          state.PreAllocatedOverlapped = new PreAllocatedOverlapped((IOCompletionCallback) ((errorCode, numBytes, overlappedPointer) =>
          {
            FileSystemWatcher.AsyncReadState nativeOverlappedState = (FileSystemWatcher.AsyncReadState) ThreadPoolBoundHandle.GetNativeOverlappedState(overlappedPointer);
            nativeOverlappedState.ThreadPoolBinding.FreeNativeOverlapped(overlappedPointer);
            FileSystemWatcher target;
            if (!nativeOverlappedState.WeakWatcher.TryGetTarget(out target))
              return;
            target.ReadDirectoryChangesCallback(errorCode, numBytes, nativeOverlappedState);
          }), (object) state, (object) numArray);
        }
        catch
        {
          this._directoryHandle.Dispose();
          this._directoryHandle = (SafeFileHandle) null;
          throw;
        }
        this._enabled = true;
        this.Monitor(state);
      }
    }

    private void StopRaisingEvents()
    {
      this._enabled = false;
      if (this.IsSuspended() || FileSystemWatcher.IsHandleInvalid(this._directoryHandle))
        return;
      Interlocked.Increment(ref this._currentSession);
      this._directoryHandle.Dispose();
      this._directoryHandle = (SafeFileHandle) null;
    }

    private void FinalizeDispose()
    {
      if (FileSystemWatcher.IsHandleInvalid(this._directoryHandle))
        return;
      this._directoryHandle.Dispose();
    }


    #nullable disable
    private static bool IsHandleInvalid([NotNullWhen(false)] SafeFileHandle handle) => handle == null || handle.IsInvalid || handle.IsClosed;

    private unsafe void Monitor(FileSystemWatcher.AsyncReadState state)
    {
      NativeOverlapped* nativeOverlappedPtr = (NativeOverlapped*) null;
      bool flag = false;
      try
      {
        if (!this._enabled || FileSystemWatcher.IsHandleInvalid(state.DirectoryHandle))
          return;
        nativeOverlappedPtr = state.ThreadPoolBinding.AllocateNativeOverlapped(state.PreAllocatedOverlapped);
        flag = Interop.Kernel32.ReadDirectoryChangesW(state.DirectoryHandle, state.Buffer, this._internalBufferSize, this._includeSubdirectories, (uint) this._notifyFilters, (uint*) null, nativeOverlappedPtr, (void*) null);
      }
      catch (ObjectDisposedException ex)
      {
      }
      catch (ArgumentNullException ex)
      {
      }
      finally
      {
        if (!flag)
        {
          if ((IntPtr) nativeOverlappedPtr != IntPtr.Zero)
            state.ThreadPoolBinding.FreeNativeOverlapped(nativeOverlappedPtr);
          state.PreAllocatedOverlapped.Dispose();
          state.ThreadPoolBinding.Dispose();
          if (!FileSystemWatcher.IsHandleInvalid(state.DirectoryHandle))
            this.OnError(new ErrorEventArgs((Exception) new Win32Exception()));
        }
      }
    }

    private void ReadDirectoryChangesCallback(
      uint errorCode,
      uint numBytes,
      FileSystemWatcher.AsyncReadState state)
    {
      try
      {
        if (FileSystemWatcher.IsHandleInvalid(state.DirectoryHandle))
          return;
        switch (errorCode)
        {
          case 0:
            if (state.Session != Volatile.Read(ref this._currentSession))
              break;
            if (numBytes == 0U)
            {
              this.NotifyInternalBufferOverflowEvent();
              break;
            }
            this.ParseEventBufferAndNotifyForEach(new ReadOnlySpan<byte>(state.Buffer, 0, (int) numBytes));
            break;
          case 995:
            break;
          default:
            this.OnError(new ErrorEventArgs((Exception) new Win32Exception((int) errorCode)));
            this.EnableRaisingEvents = false;
            break;
        }
      }
      finally
      {
        this.Monitor(state);
      }
    }

    private void ParseEventBufferAndNotifyForEach(ReadOnlySpan<byte> buffer)
    {
      ReadOnlySpan<char> oldName = ReadOnlySpan<char>.Empty;
      // ISSUE: variable of a reference type
      Interop.Kernel32.FILE_NOTIFY_INFORMATION& local;
      for (; (long) sizeof (Interop.Kernel32.FILE_NOTIFY_INFORMATION) <= (long) (uint) buffer.Length; buffer = buffer.Slice((int) local.NextEntryOffset))
      {
        local = ref MemoryMarshal.AsRef<Interop.Kernel32.FILE_NOTIFY_INFORMATION>(buffer);
        if ((long) local.FileNameLength <= (long) (uint) buffer.Length - (long) sizeof (Interop.Kernel32.FILE_NOTIFY_INFORMATION))
        {
          ReadOnlySpan<char> name = MemoryMarshal.Cast<byte, char>(buffer.Slice(sizeof (Interop.Kernel32.FILE_NOTIFY_INFORMATION), (int) local.FileNameLength));
          switch (local.Action)
          {
            case Interop.Kernel32.FileAction.FILE_ACTION_RENAMED_OLD_NAME:
              oldName = name;
              break;
            case Interop.Kernel32.FileAction.FILE_ACTION_RENAMED_NEW_NAME:
              this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, name, oldName);
              oldName = ReadOnlySpan<char>.Empty;
              break;
            default:
              if (!oldName.IsEmpty)
              {
                this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, ReadOnlySpan<char>.Empty, oldName);
                oldName = ReadOnlySpan<char>.Empty;
              }
              switch (local.Action)
              {
                case Interop.Kernel32.FileAction.FILE_ACTION_ADDED:
                  this.NotifyFileSystemEventArgs(WatcherChangeTypes.Created, name);
                  break;
                case Interop.Kernel32.FileAction.FILE_ACTION_REMOVED:
                  this.NotifyFileSystemEventArgs(WatcherChangeTypes.Deleted, name);
                  break;
                case Interop.Kernel32.FileAction.FILE_ACTION_MODIFIED:
                  this.NotifyFileSystemEventArgs(WatcherChangeTypes.Changed, name);
                  break;
              }
              break;
          }
          if (local.NextEntryOffset == 0U || local.NextEntryOffset > (uint) buffer.Length)
            break;
        }
        else
          break;
      }
      if (oldName.IsEmpty)
        return;
      this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, ReadOnlySpan<char>.Empty, oldName);
    }

    private sealed class NormalizedFilterCollection : Collection<string>
    {
      internal NormalizedFilterCollection()
        : base((IList<string>) new FileSystemWatcher.NormalizedFilterCollection.ImmutableStringList())
      {
      }

      protected override void InsertItem(int index, string item) => base.InsertItem(index, string.IsNullOrEmpty(item) || item == "*.*" ? "*" : item);

      protected override void SetItem(int index, string item) => base.SetItem(index, string.IsNullOrEmpty(item) || item == "*.*" ? "*" : item);

      internal string[] GetFilters() => ((FileSystemWatcher.NormalizedFilterCollection.ImmutableStringList) this.Items).Items;

      private sealed class ImmutableStringList : 
        IList<string>,
        ICollection<string>,
        IEnumerable<string>,
        IEnumerable
      {
        public string[] Items = Array.Empty<string>();

        public string this[int index]
        {
          get
          {
            string[] items = this.Items;
            if ((uint) index >= (uint) items.Length)
              throw new ArgumentOutOfRangeException(nameof (index));
            return items[index];
          }
          set
          {
            string[] strArray = (string[]) this.Items.Clone();
            strArray[index] = value;
            this.Items = strArray;
          }
        }

        public int Count => this.Items.Length;

        public bool IsReadOnly => false;

        public void Add(string item) => throw new NotSupportedException();

        public void Clear() => this.Items = Array.Empty<string>();

        public bool Contains(string item) => Array.IndexOf<string>(this.Items, item) != -1;

        public void CopyTo(string[] array, int arrayIndex) => this.Items.CopyTo((Array) array, arrayIndex);

        public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>) this.Items).GetEnumerator();

        public int IndexOf(string item) => Array.IndexOf<string>(this.Items, item);

        public void Insert(int index, string item)
        {
          string[] items = this.Items;
          string[] strArray = new string[items.Length + 1];
          items.AsSpan<string>(0, index).CopyTo((Span<string>) strArray);
          items.AsSpan<string>(index).CopyTo(strArray.AsSpan<string>(index + 1));
          strArray[index] = item;
          this.Items = strArray;
        }

        public bool Remove(string item) => throw new NotSupportedException();

        public void RemoveAt(int index)
        {
          string[] items = this.Items;
          string[] strArray = new string[items.Length - 1];
          items.AsSpan<string>(0, index).CopyTo((Span<string>) strArray);
          items.AsSpan<string>(index + 1).CopyTo(strArray.AsSpan<string>(index));
          this.Items = strArray;
        }

        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
      }
    }

    private sealed class AsyncReadState
    {
      internal AsyncReadState(
        int session,
        byte[] buffer,
        SafeFileHandle handle,
        ThreadPoolBoundHandle binding,
        FileSystemWatcher parent)
      {
        this.Session = session;
        this.Buffer = buffer;
        this.DirectoryHandle = handle;
        this.ThreadPoolBinding = binding;
        this.WeakWatcher = new WeakReference<FileSystemWatcher>(parent);
      }

      internal int Session { get; }

      internal byte[] Buffer { get; }

      internal SafeFileHandle DirectoryHandle { get; }

      internal ThreadPoolBoundHandle ThreadPoolBinding { get; }

      internal PreAllocatedOverlapped PreAllocatedOverlapped { get; set; }

      internal WeakReference<FileSystemWatcher> WeakWatcher { get; }
    }
  }
}
