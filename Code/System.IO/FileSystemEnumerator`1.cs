// Decompiled with JetBrains decompiler
// Type: System.IO.Enumeration.FileSystemEnumerator`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;


#nullable enable
namespace System.IO.Enumeration
{
  /// <summary>Enumerates the file system elements of the provided type that are being searched and filtered by a <see cref="T:System.IO.Enumeration.FileSystemEnumerable`1" />.</summary>
  /// <typeparam name="TResult">The type of the result produced by this file system enumerator.</typeparam>
  public abstract class FileSystemEnumerator<TResult> : 
    CriticalFinalizerObject,
    IEnumerator<TResult>,
    IDisposable,
    IEnumerator
  {
    private int _remainingRecursionDepth;

    #nullable disable
    private readonly string _originalRootDirectory;
    private readonly string _rootDirectory;
    private readonly EnumerationOptions _options;
    private readonly object _lock = new object();
    private unsafe Interop.NtDll.FILE_FULL_DIR_INFORMATION* _entry;
    private TResult _current;
    private IntPtr _buffer;
    private int _bufferLength;
    private IntPtr _directoryHandle;
    private string _currentPath;
    private bool _lastEntryFound;
    private Queue<(IntPtr Handle, string Path, int RemainingDepth)> _pending;


    #nullable enable
    /// <summary>Encapsulates a find operation.</summary>
    /// <param name="directory">The directory to search in.</param>
    /// <param name="options">Enumeration options to use.</param>
    public FileSystemEnumerator(string directory, EnumerationOptions? options = null)
      : this(directory, false, options)
    {
    }


    #nullable disable
    internal FileSystemEnumerator(string directory, bool isNormalized, EnumerationOptions options = null)
    {
      this._originalRootDirectory = directory ?? throw new ArgumentNullException(nameof (directory));
      this._rootDirectory = Path.TrimEndingDirectorySeparator(isNormalized ? directory : Path.GetFullPath(directory));
      this._options = options ?? EnumerationOptions.Default;
      this._remainingRecursionDepth = this._options.MaxRecursionDepth;
      this.Init();
    }


    #nullable enable
    /// <summary>When overridden in a derived class, determines whether the specified file system entry should be included in the results.</summary>
    /// <param name="entry">A file system entry reference.</param>
    /// <returns>
    /// <see langword="true" /> if the specified file system entry should be included in the results; otherwise, <see langword="false" />.</returns>
    protected virtual bool ShouldIncludeEntry(ref FileSystemEntry entry) => true;

    /// <summary>When overridden in a derived class, determines whether the specified file system entry should be recursed.</summary>
    /// <param name="entry">A file system entry reference.</param>
    /// <returns>
    /// <see langword="true" /> if the specified directory entry should be recursed into; otherwise, <see langword="false" />.</returns>
    protected virtual bool ShouldRecurseIntoEntry(ref FileSystemEntry entry) => true;

    /// <summary>When overridden in a derived class, generates the result type from the current entry.</summary>
    /// <param name="entry">A file system entry reference.</param>
    /// <returns>The result type from the current entry.</returns>
    protected abstract TResult TransformEntry(ref FileSystemEntry entry);

    /// <summary>When overriden in a derived class, this method is called whenever the end of a directory is reached.</summary>
    /// <param name="directory">The directory path as a read-only span.</param>
    protected virtual void OnDirectoryFinished(ReadOnlySpan<char> directory)
    {
    }

    /// <summary>When overriden in a derived class, returns a value that indicates whether to continue execution or throw the default exception.</summary>
    /// <param name="error">The native error code.</param>
    /// <returns>
    /// <see langword="true" /> to continue; <see langword="false" /> to throw the default exception for the given error.</returns>
    protected virtual bool ContinueOnError(int error) => false;

    /// <summary>Gets the currently visited element.</summary>
    /// <returns>The currently visited element.</returns>
    public TResult Current => this._current;

    /// <summary>Gets the currently visited object.</summary>
    /// <returns>The currently visited object.</returns>
    object? IEnumerator.Current => (object) this.Current;

    private unsafe void DirectoryFinished()
    {
      this._entry = (Interop.NtDll.FILE_FULL_DIR_INFORMATION*) null;
      this.CloseDirectoryHandle();
      this.OnDirectoryFinished(this._currentPath.AsSpan());
      if (!this.DequeueNextDirectory())
        this._lastEntryFound = true;
      else
        this.FindNextEntry();
    }

    /// <summary>Always throws <see cref="T:System.NotSupportedException" />.</summary>
    public void Reset() => throw new NotSupportedException();

    /// <summary>Releases the resources used by the current instance of the <see cref="T:System.IO.Enumeration.FileSystemEnumerator`1" /> class.</summary>
    public void Dispose()
    {
      this.InternalDispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>When overridden in a derived class, releases the unmanaged resources used by the <see cref="T:System.IO.Enumeration.FileSystemEnumerator`1" /> class and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    ~FileSystemEnumerator() => this.InternalDispose(false);

    private void Init()
    {
      using (new DisableMediaInsertionPrompt())
      {
        this._directoryHandle = this.CreateDirectoryHandle(this._rootDirectory);
        if (this._directoryHandle == IntPtr.Zero)
          this._lastEntryFound = true;
      }
      this._currentPath = this._rootDirectory;
      int bufferSize = this._options.BufferSize;
      this._bufferLength = bufferSize <= 0 ? 4096 : Math.Max(1024, bufferSize);
      try
      {
        this._buffer = Marshal.AllocHGlobal(this._bufferLength);
      }
      catch
      {
        this.CloseDirectoryHandle();
        throw;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe bool GetData()
    {
      Interop.NtDll.IO_STATUS_BLOCK ioStatusBlock;
      int Status = Interop.NtDll.NtQueryDirectoryFile(this._directoryHandle, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, &ioStatusBlock, this._buffer, (uint) this._bufferLength, Interop.NtDll.FILE_INFORMATION_CLASS.FileFullDirectoryInformation, Interop.BOOLEAN.FALSE, (Interop.UNICODE_STRING*) null, Interop.BOOLEAN.FALSE);
      switch ((uint) Status)
      {
        case 0:
          return true;
        case 2147483654:
          this.DirectoryFinished();
          return false;
        case 3221225487:
          this.DirectoryFinished();
          return false;
        default:
          int dosError = (int) Interop.NtDll.RtlNtStatusToDosError(Status);
          if ((dosError != 5 || !this._options.IgnoreInaccessible) && !this.ContinueOnError(dosError))
            throw Win32Marshal.GetExceptionForWin32Error(dosError, this._currentPath);
          this.DirectoryFinished();
          return false;
      }
    }


    #nullable disable
    private IntPtr CreateRelativeDirectoryHandle(
      ReadOnlySpan<char> relativePath,
      string fullPath)
    {
      (uint num, IntPtr handle) = Interop.NtDll.CreateFile(relativePath, this._directoryHandle, Interop.NtDll.CreateDisposition.FILE_OPEN, Interop.NtDll.DesiredAccess.FILE_READ_DATA | Interop.NtDll.DesiredAccess.SYNCHRONIZE, createOptions: (Interop.NtDll.CreateOptions.FILE_DIRECTORY_FILE | Interop.NtDll.CreateOptions.FILE_SYNCHRONOUS_IO_NONALERT | Interop.NtDll.CreateOptions.FILE_OPEN_FOR_BACKUP_INTENT));
      if (num == 0U)
        return handle;
      int dosError = (int) Interop.NtDll.RtlNtStatusToDosError((int) num);
      if (this.ContinueOnDirectoryError(dosError, true))
        return IntPtr.Zero;
      throw Win32Marshal.GetExceptionForWin32Error(dosError, fullPath);
    }

    private void CloseDirectoryHandle()
    {
      IntPtr handle = Interlocked.Exchange(ref this._directoryHandle, IntPtr.Zero);
      if (!(handle != IntPtr.Zero))
        return;
      Interop.Kernel32.CloseHandle(handle);
    }

    private IntPtr CreateDirectoryHandle(string path, bool ignoreNotFound = false)
    {
      IntPtr fileIntPtr = Interop.Kernel32.CreateFile_IntPtr(path, 1, FileShare.ReadWrite | FileShare.Delete, FileMode.Open, 33554432);
      if (!(fileIntPtr == IntPtr.Zero) && !(fileIntPtr == (IntPtr) -1))
        return fileIntPtr;
      int num = Marshal.GetLastWin32Error();
      if (this.ContinueOnDirectoryError(num, ignoreNotFound))
        return IntPtr.Zero;
      num = num == 2 ? 3 : throw Win32Marshal.GetExceptionForWin32Error(num, path);
    }

    private bool ContinueOnDirectoryError(int error, bool ignoreNotFound) => ignoreNotFound && (error == 2 || error == 3 || error == 267) || error == 5 && this._options.IgnoreInaccessible || this.ContinueOnError(error);

    /// <summary>Advances the enumerator to the next item of the <see cref="T:System.IO.Enumeration.FileSystemEnumerator`1" />.</summary>
    /// <returns>
    /// <see langword="true" /> if the enumerator successfully advanced to the next item; <see langword="false" /> if the end of the enumerator has been passed.</returns>
    public unsafe bool MoveNext()
    {
      if (this._lastEntryFound)
        return false;
      FileSystemEntry entry = new FileSystemEntry();
      lock (this._lock)
      {
        if (this._lastEntryFound)
          return false;
        do
        {
          do
          {
            this.FindNextEntry();
            if (this._lastEntryFound)
              return false;
            FileSystemEntry.Initialize(ref entry, this._entry, this._currentPath.AsSpan(), this._rootDirectory.AsSpan(), this._originalRootDirectory.AsSpan());
          }
          while ((this._entry->FileAttributes & this._options.AttributesToSkip) != (FileAttributes) 0);
          if ((this._entry->FileAttributes & FileAttributes.Directory) != (FileAttributes) 0)
          {
            ReadOnlySpan<char> fileName = this._entry->FileName;
            if (fileName.Length <= 2)
            {
              fileName = this._entry->FileName;
              if (fileName[0] == '.')
              {
                fileName = this._entry->FileName;
                if (fileName.Length == 2)
                {
                  fileName = this._entry->FileName;
                  if (fileName[1] != '.')
                    goto label_14;
                }
                if (this._options.ReturnSpecialDirectories)
                  goto label_20;
                else
                  continue;
              }
            }
label_14:
            if (this._options.RecurseSubdirectories && this._remainingRecursionDepth > 0 && this.ShouldRecurseIntoEntry(ref entry))
            {
              string fullPath = Path.Join(this._currentPath.AsSpan(), this._entry->FileName);
              IntPtr relativeDirectoryHandle = this.CreateRelativeDirectoryHandle(this._entry->FileName, fullPath);
              if (relativeDirectoryHandle != IntPtr.Zero)
              {
                try
                {
                  if (this._pending == null)
                    this._pending = new Queue<(IntPtr, string, int)>();
                  this._pending.Enqueue((relativeDirectoryHandle, fullPath, this._remainingRecursionDepth - 1));
                }
                catch
                {
                  Interop.Kernel32.CloseHandle(relativeDirectoryHandle);
                  throw;
                }
              }
            }
          }
label_20:;
        }
        while (!this.ShouldIncludeEntry(ref entry));
        this._current = this.TransformEntry(ref entry);
        return true;
      }
    }

    private unsafe void FindNextEntry()
    {
      this._entry = Interop.NtDll.FILE_FULL_DIR_INFORMATION.GetNextInfo(this._entry);
      if ((IntPtr) this._entry != IntPtr.Zero || !this.GetData())
        return;
      this._entry = (Interop.NtDll.FILE_FULL_DIR_INFORMATION*) (void*) this._buffer;
    }

    private bool DequeueNextDirectory()
    {
      if (this._pending == null || this._pending.Count == 0)
        return false;
      (this._directoryHandle, this._currentPath, this._remainingRecursionDepth) = this._pending.Dequeue();
      return true;
    }

    private void InternalDispose(bool disposing)
    {
      if (this._lock != null)
      {
        lock (this._lock)
        {
          this._lastEntryFound = true;
          this.CloseDirectoryHandle();
          if (this._pending != null)
          {
            while (this._pending.Count > 0)
              Interop.Kernel32.CloseHandle(this._pending.Dequeue().Handle);
            this._pending = (Queue<(IntPtr, string, int)>) null;
          }
          if (this._buffer != IntPtr.Zero)
            Marshal.FreeHGlobal(this._buffer);
          this._buffer = new IntPtr();
        }
      }
      this.Dispose(disposing);
    }
  }
}
