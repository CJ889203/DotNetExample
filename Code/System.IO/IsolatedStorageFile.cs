// Decompiled with JetBrains decompiler
// Type: System.IO.IsolatedStorage.IsolatedStorageFile
// Assembly: System.IO.IsolatedStorage, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 87FE0B2F-0A44-4572-BEFC-C86F7165516A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.IsolatedStorage.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.IsolatedStorage.xml

using System.Collections;
using System.Linq;
using System.Text;


#nullable enable
namespace System.IO.IsolatedStorage
{
  /// <summary>Represents an isolated storage area containing files and directories.</summary>
  public sealed class IsolatedStorageFile : System.IO.IsolatedStorage.IsolatedStorage, IDisposable
  {
    private bool _disposed;
    private bool _closed;

    #nullable disable
    private readonly object _internalLock = new object();
    private readonly string _rootDirectory;

    internal IsolatedStorageFile(IsolatedStorageScope scope)
    {
      this.InitStore(scope, (Type) null, (Type) null);
      StringBuilder stringBuilder = new StringBuilder(Helper.GetRootDirectory(scope));
      stringBuilder.Append(this.SeparatorExternal);
      stringBuilder.Append(this.IdentityHash);
      stringBuilder.Append(this.SeparatorExternal);
      if (Helper.IsApplication(scope))
        stringBuilder.Append("AppFiles");
      else if (Helper.IsDomain(scope))
        stringBuilder.Append("Files");
      else
        stringBuilder.Append("AssemFiles");
      stringBuilder.Append(this.SeparatorExternal);
      this._rootDirectory = stringBuilder.ToString();
      Helper.CreateDirectory(this._rootDirectory, scope);
    }


    #nullable enable
    private string RootDirectory => this._rootDirectory;

    internal bool Disposed => this._disposed;

    internal bool IsDeleted
    {
      get
      {
        try
        {
          return !Directory.Exists(this.RootDirectory);
        }
        catch (IOException ex)
        {
          return true;
        }
        catch (UnauthorizedAccessException ex)
        {
          return true;
        }
      }
    }

    /// <summary>Closes a store previously opened with <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFile.GetStore(System.IO.IsolatedStorage.IsolatedStorageScope,System.Type,System.Type)" />, <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly" />, or <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForDomain" />.</summary>
    public void Close()
    {
      if (Helper.IsRoaming(this.Scope))
        return;
      lock (this._internalLock)
      {
        if (this._closed)
          return;
        this._closed = true;
        GC.SuppressFinalize((object) this);
      }
    }

    /// <summary>Deletes a file in the isolated storage scope.</summary>
    /// <param name="file">The relative path of the file to delete within the isolated storage scope.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The target file is open or the path is incorrect.</exception>
    /// <exception cref="T:System.ArgumentNullException">The file path is <see langword="null" />.</exception>
    public void DeleteFile(string file)
    {
      if (file == null)
        throw new ArgumentNullException(nameof (file));
      this.EnsureStoreIsValid();
      try
      {
        File.Delete(this.GetFullPath(file));
      }
      catch (Exception ex)
      {
        throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_DeleteFile, ex);
      }
    }

    /// <summary>Determines whether the specified path refers to an existing file in the isolated store.</summary>
    /// <param name="path">The path and file name to test.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store is closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="path" /> refers to an existing file in the isolated store and is not <see langword="null" />; otherwise, <see langword="false" />.</returns>
    public bool FileExists(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      this.EnsureStoreIsValid();
      return File.Exists(this.GetFullPath(path));
    }

    /// <summary>Determines whether the specified path refers to an existing directory in the isolated store.</summary>
    /// <param name="path">The path to test.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store is closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="path" /> refers to an existing directory in the isolated store and is not <see langword="null" />; otherwise, <see langword="false" />.</returns>
    public bool DirectoryExists(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      this.EnsureStoreIsValid();
      return Directory.Exists(this.GetFullPath(path));
    }

    /// <summary>Creates a directory in the isolated storage scope.</summary>
    /// <param name="dir">The relative path of the directory to create within the isolated storage scope.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The current code has insufficient permissions to create isolated storage directory.</exception>
    /// <exception cref="T:System.ArgumentNullException">The directory path is <see langword="null" />.</exception>
    public void CreateDirectory(string dir)
    {
      if (dir == null)
        throw new ArgumentNullException(nameof (dir));
      this.EnsureStoreIsValid();
      string fullPath = this.GetFullPath(dir);
      if (Directory.Exists(fullPath))
        return;
      try
      {
        Directory.CreateDirectory(fullPath);
      }
      catch (Exception ex)
      {
        throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_CreateDirectory, ex);
      }
    }

    /// <summary>Deletes a directory in the isolated storage scope.</summary>
    /// <param name="dir">The relative path of the directory to delete within the isolated storage scope.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The directory could not be deleted.</exception>
    /// <exception cref="T:System.ArgumentNullException">The directory path was <see langword="null" />.</exception>
    public void DeleteDirectory(string dir)
    {
      if (dir == null)
        throw new ArgumentNullException(nameof (dir));
      this.EnsureStoreIsValid();
      try
      {
        Directory.Delete(this.GetFullPath(dir), false);
      }
      catch (Exception ex)
      {
        throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_DeleteDirectory, ex);
      }
    }

    /// <summary>Enumerates the file names at the root of an isolated store.</summary>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">File paths from the isolated store root cannot be determined.</exception>
    /// <returns>An array of relative paths of files at the root of the isolated store.  A zero-length array specifies that there are no files at the root.</returns>
    public string[] GetFileNames() => this.GetFileNames("*");

    /// <summary>Gets the file names that match a search pattern.</summary>
    /// <param name="searchPattern">A search pattern. Both single-character ("?") and multi-character ("*") wildcards are supported.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The file path specified by <paramref name="searchPattern" /> cannot be found.</exception>
    /// <returns>An array of relative paths of files in the isolated storage scope that match <paramref name="searchPattern" />. A zero-length array specifies that there are no files that match.</returns>
    public string[] GetFileNames(string searchPattern)
    {
      if (searchPattern == null)
        throw new ArgumentNullException(nameof (searchPattern));
      this.EnsureStoreIsValid();
      try
      {
        return Directory.EnumerateFiles(this.RootDirectory, searchPattern).Select<string, string>((Func<string, string>) (f => Path.GetFileName(f))).ToArray<string>();
      }
      catch (UnauthorizedAccessException ex)
      {
        throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_Operation, (Exception) ex);
      }
    }

    /// <summary>Enumerates the directories at the root of an isolated store.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store is closed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Caller does not have permission to enumerate directories.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">One or more directories are not found.</exception>
    /// <returns>An array of relative paths of directories at the root of the isolated store. A zero-length array specifies that there are no directories at the root.</returns>
    public string[] GetDirectoryNames() => this.GetDirectoryNames("*");

    /// <summary>Enumerates the directories in an isolated storage scope that match a given search pattern.</summary>
    /// <param name="searchPattern">A search pattern. Both single-character ("?") and multi-character ("*") wildcards are supported.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store is closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Caller does not have permission to enumerate directories resolved from <paramref name="searchPattern" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory or directories specified by <paramref name="searchPattern" /> are not found.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.</exception>
    /// <returns>An array of the relative paths of directories in the isolated storage scope that match <paramref name="searchPattern" />. A zero-length array specifies that there are no directories that match.</returns>
    public string[] GetDirectoryNames(string searchPattern)
    {
      if (searchPattern == null)
        throw new ArgumentNullException(nameof (searchPattern));
      this.EnsureStoreIsValid();
      try
      {
        return Directory.EnumerateDirectories(this.RootDirectory, searchPattern).Select<string, string>((Func<string, string>) (m => m.Substring(Path.GetDirectoryName(m).Length + 1))).ToArray<string>();
      }
      catch (UnauthorizedAccessException ex)
      {
        throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_Operation, (Exception) ex);
      }
    }

    /// <summary>Opens a file in the specified mode.</summary>
    /// <param name="path">The relative path of the file within the isolated store.</param>
    /// <param name="mode">One of the enumeration values that specifies how to open the file.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is malformed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory in <paramref name="path" /> does not exist.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <returns>A file that is opened in the specified mode, with read/write access, and is unshared.</returns>
    public IsolatedStorageFileStream OpenFile(string path, FileMode mode)
    {
      this.EnsureStoreIsValid();
      return new IsolatedStorageFileStream(path, mode, this);
    }

    /// <summary>Opens a file in the specified mode with the specified read/write access.</summary>
    /// <param name="path">The relative path of the file within the isolated store.</param>
    /// <param name="mode">One of the enumeration values that specifies how to open the file.</param>
    /// <param name="access">One of the enumeration values that specifies whether the file will be opened with read, write, or read/write access.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is malformed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory in <paramref name="path" /> does not exist.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <returns>A file that is opened in the specified mode and access, and is unshared.</returns>
    public IsolatedStorageFileStream OpenFile(
      string path,
      FileMode mode,
      FileAccess access)
    {
      this.EnsureStoreIsValid();
      return new IsolatedStorageFileStream(path, mode, access, this);
    }

    /// <summary>Opens a file in the specified mode, with the specified read/write access and sharing permission.</summary>
    /// <param name="path">The relative path of the file within the isolated store.</param>
    /// <param name="mode">One of the enumeration values that specifies how to open or create the file.</param>
    /// <param name="access">One of the enumeration values that specifies whether the file will be opened with read, write, or read/write access.</param>
    /// <param name="share">A bitwise combination of enumeration values that specify the type of access other <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> objects have to this file.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is malformed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory in <paramref name="path" /> does not exist.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="M:System.IO.FileInfo.Open(System.IO.FileMode)" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <returns>A file that is opened in the specified mode and access, and with the specified sharing options.</returns>
    public IsolatedStorageFileStream OpenFile(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share)
    {
      this.EnsureStoreIsValid();
      return new IsolatedStorageFileStream(path, mode, access, share, this);
    }

    /// <summary>Creates a file in the isolated store.</summary>
    /// <param name="path">The relative path of the file to create.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is malformed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory in <paramref name="path" /> does not exist.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <returns>A new isolated storage file.</returns>
    public IsolatedStorageFileStream CreateFile(string path)
    {
      this.EnsureStoreIsValid();
      return new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, this);
    }

    /// <summary>Returns the creation date and time of a specified file or directory.</summary>
    /// <param name="path">The path to the file or directory for which to obtain creation date and time information.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <returns>The creation date and time for the specified file or directory. This value is expressed in local time.</returns>
    public DateTimeOffset GetCreationTime(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          this.EnsureStoreIsValid();
          try
          {
            return new DateTimeOffset(File.GetCreationTime(this.GetFullPath(path)));
          }
          catch (UnauthorizedAccessException ex)
          {
            return new DateTimeOffset(1601, 1, 1, 0, 0, 0, TimeSpan.Zero).ToLocalTime();
          }
      }
    }

    /// <summary>Returns the date and time a specified file or directory was last accessed.</summary>
    /// <param name="path">The path to the file or directory for which to obtain last access date and time information.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <returns>The date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
    public DateTimeOffset GetLastAccessTime(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          this.EnsureStoreIsValid();
          try
          {
            return new DateTimeOffset(File.GetLastAccessTime(this.GetFullPath(path)));
          }
          catch (UnauthorizedAccessException ex)
          {
            return new DateTimeOffset(1601, 1, 1, 0, 0, 0, TimeSpan.Zero).ToLocalTime();
          }
      }
    }

    /// <summary>Returns the date and time a specified file or directory was last written to.</summary>
    /// <param name="path">The path to the file or directory for which to obtain last write date and time information.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <returns>The date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
    public DateTimeOffset GetLastWriteTime(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          this.EnsureStoreIsValid();
          try
          {
            return new DateTimeOffset(File.GetLastWriteTime(this.GetFullPath(path)));
          }
          catch (UnauthorizedAccessException ex)
          {
            return new DateTimeOffset(1601, 1, 1, 0, 0, 0, TimeSpan.Zero).ToLocalTime();
          }
      }
    }

    /// <summary>Copies an existing file to a new file.</summary>
    /// <param name="sourceFileName">The name of the file to copy.</param>
    /// <param name="destinationFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationFileName" /> exists.
    /// 
    /// -or-
    /// 
    /// An I/O error has occurred.</exception>
    public void CopyFile(string sourceFileName, string destinationFileName)
    {
      if (sourceFileName == null)
        throw new ArgumentNullException(nameof (sourceFileName));
      if (destinationFileName == null)
        throw new ArgumentNullException(nameof (destinationFileName));
      if (sourceFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (sourceFileName));
      if (destinationFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (destinationFileName));
      this.CopyFile(sourceFileName, destinationFileName, false);
    }

    /// <summary>Copies an existing file to a new file, and optionally overwrites an existing file.</summary>
    /// <param name="sourceFileName">The name of the file to copy.</param>
    /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
    /// <param name="overwrite">
    /// <see langword="true" /> if the destination file can be overwritten; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.
    /// 
    /// -or-
    /// 
    /// An I/O error has occurred.</exception>
    public void CopyFile(string sourceFileName, string destinationFileName, bool overwrite)
    {
      if (sourceFileName == null)
        throw new ArgumentNullException(nameof (sourceFileName));
      if (destinationFileName == null)
        throw new ArgumentNullException(nameof (destinationFileName));
      if (sourceFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (sourceFileName));
      if (destinationFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (destinationFileName));
      this.EnsureStoreIsValid();
      string fullPath1 = this.GetFullPath(sourceFileName);
      string fullPath2 = this.GetFullPath(destinationFileName);
      try
      {
        File.Copy(fullPath1, fullPath2, overwrite);
      }
      catch (FileNotFoundException ex)
      {
        throw new FileNotFoundException(SR.Format(SR.PathNotFound_Path, (object) sourceFileName));
      }
      catch (PathTooLongException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_Operation, ex);
      }
    }

    /// <summary>Moves a specified file to a new location, and optionally lets you specify a new file name.</summary>
    /// <param name="sourceFileName">The name of the file to move.</param>
    /// <param name="destinationFileName">The path to the new location for the file. If a file name is included, the moved file will have that name.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    public void MoveFile(string sourceFileName, string destinationFileName)
    {
      if (sourceFileName == null)
        throw new ArgumentNullException(nameof (sourceFileName));
      if (destinationFileName == null)
        throw new ArgumentNullException(nameof (destinationFileName));
      if (sourceFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (sourceFileName));
      if (destinationFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (destinationFileName));
      this.EnsureStoreIsValid();
      string fullPath1 = this.GetFullPath(sourceFileName);
      string fullPath2 = this.GetFullPath(destinationFileName);
      try
      {
        File.Move(fullPath1, fullPath2);
      }
      catch (FileNotFoundException ex)
      {
        throw new FileNotFoundException(SR.Format(SR.PathNotFound_Path, (object) sourceFileName));
      }
      catch (PathTooLongException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_Operation, ex);
      }
    }

    /// <summary>Moves a specified directory and its contents to a new location.</summary>
    /// <param name="sourceDirectoryName">The name of the directory to move.</param>
    /// <param name="destinationDirectoryName">The path to the new location for <paramref name="sourceDirectoryName" />. This cannot be the path to an existing directory.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="sourceDirectoryName" /> does not exist.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationDirectoryName" /> already exists.
    /// 
    /// -or-
    /// 
    /// <paramref name="sourceDirectoryName" /> and <paramref name="destinationDirectoryName" /> refer to the same directory.</exception>
    public void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
    {
      if (sourceDirectoryName == null)
        throw new ArgumentNullException(nameof (sourceDirectoryName));
      if (destinationDirectoryName == null)
        throw new ArgumentNullException(nameof (destinationDirectoryName));
      if (sourceDirectoryName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (sourceDirectoryName));
      if (destinationDirectoryName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (destinationDirectoryName));
      this.EnsureStoreIsValid();
      string fullPath1 = this.GetFullPath(sourceDirectoryName);
      string fullPath2 = this.GetFullPath(destinationDirectoryName);
      try
      {
        Directory.Move(fullPath1, fullPath2);
      }
      catch (DirectoryNotFoundException ex)
      {
        throw new DirectoryNotFoundException(SR.Format(SR.PathNotFound_Path, (object) sourceDirectoryName));
      }
      catch (PathTooLongException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_Operation, ex);
      }
    }

    /// <summary>Gets the enumerator for the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> stores within an isolated storage scope.</summary>
    /// <param name="scope">Represents the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> for which to return isolated stores. <see langword="User" /> and <see langword="User|Roaming" /> are the only <see langword="IsolatedStorageScope" /> combinations supported.</param>
    /// <returns>Enumerator for the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> stores within the specified isolated storage scope.</returns>
    public static IEnumerator GetEnumerator(IsolatedStorageScope scope) => (IEnumerator) new IsolatedStorageFile.IsolatedStorageFileEnumerator();

    /// <summary>Gets a value that represents the amount of free space available for isolated storage.</summary>
    /// <exception cref="T:System.InvalidOperationException">The isolated store is closed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <returns>The available free space for isolated storage, in bytes.</returns>
    public override long AvailableFreeSpace => this.Quota - this.UsedSize;

    /// <summary>Gets a value representing the maximum amount of space available for isolated storage within the limits established by the quota.</summary>
    /// <exception cref="T:System.InvalidOperationException">The property is unavailable. <see cref="P:System.IO.IsolatedStorage.IsolatedStorageFile.MaximumSize" /> cannot be determined without evidence from the assembly's creation. The evidence could not be determined when the object was created.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage error occurred.</exception>
    /// <returns>The limit of isolated storage space in bytes.</returns>
    [CLSCompliant(false)]
    [Obsolete("IsolatedStorage.MaximumSize has been deprecated because it is not CLS Compliant. To get the maximum size use IsolatedStorage.Quota instead.")]
    public override ulong MaximumSize => (ulong) long.MaxValue;

    /// <summary>Gets a value that represents the maximum amount of space available for isolated storage.</summary>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <returns>The limit of isolated storage space, in bytes.</returns>
    public override long Quota => long.MaxValue;

    /// <summary>Gets a value that represents the amount of the space used for isolated storage.</summary>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <returns>The used isolated storage space, in bytes.</returns>
    public override long UsedSize => 0;

    /// <summary>Gets the current size of the isolated storage.</summary>
    /// <exception cref="T:System.InvalidOperationException">The property is unavailable. The current store has a roaming scope or is not open.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current object size is undefined.</exception>
    /// <returns>The total number of bytes of storage currently in use within the isolated storage scope.</returns>
    [CLSCompliant(false)]
    [Obsolete("IsolatedStorage.CurrentSize has been deprecated because it is not CLS Compliant. To get the current size use IsolatedStorage.UsedSize instead.")]
    public override ulong CurrentSize => 0;

    /// <summary>Obtains user-scoped isolated storage corresponding to the calling code's application identity.</summary>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.
    /// 
    /// -or-
    /// 
    /// The application identity of the caller cannot be determined, because the <see cref="P:System.AppDomain.ActivationContext" /> property returned <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The permissions for the application domain cannot be determined.</exception>
    /// <returns>An object corresponding to the isolated storage scope based on the calling code's assembly identity.</returns>
    public static IsolatedStorageFile GetUserStoreForApplication() => IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Application);

    /// <summary>Obtains user-scoped isolated storage corresponding to the calling code's assembly identity.</summary>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.
    /// 
    /// -or-
    /// 
    /// The permissions for the calling assembly cannot be determined.</exception>
    /// <returns>An object corresponding to the isolated storage scope based on the calling code's assembly identity.</returns>
    public static IsolatedStorageFile GetUserStoreForAssembly() => IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly);

    /// <summary>Obtains user-scoped isolated storage corresponding to the application domain identity and assembly identity.</summary>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The store failed to open.
    /// 
    /// -or-
    /// 
    /// The assembly specified has insufficient permissions to create isolated stores.
    /// 
    /// -or-
    /// 
    /// An isolated storage location cannot be initialized.
    /// 
    /// -or-
    /// 
    /// The permissions for the application domain cannot be determined.</exception>
    /// <returns>An object corresponding to the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />, based on a combination of the application domain identity and the assembly identity.</returns>
    public static IsolatedStorageFile GetUserStoreForDomain() => IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly);

    /// <summary>Obtains a user-scoped isolated store for use by applications in a virtual host domain.</summary>
    /// <returns>The isolated storage file that corresponds to the isolated storage scope based on the calling code's application identity.</returns>
    public static IsolatedStorageFile GetUserStoreForSite() => throw new NotSupportedException(SR.IsolatedStorage_NotValidOnDesktop);

    /// <summary>Obtains machine-scoped isolated storage corresponding to the calling code's application identity.</summary>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The application identity of the caller could not be determined.
    /// 
    /// -or-
    /// 
    /// The granted permission set for the application domain could not be determined.
    /// 
    /// -or-
    /// 
    /// An isolated storage location cannot be initialized.</exception>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <returns>An object corresponding to the isolated storage scope based on the calling code's application identity.</returns>
    public static IsolatedStorageFile GetMachineStoreForApplication() => IsolatedStorageFile.GetStore(IsolatedStorageScope.Machine | IsolatedStorageScope.Application);

    /// <summary>Obtains machine-scoped isolated storage corresponding to the calling code's assembly identity.</summary>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.</exception>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <returns>An object corresponding to the isolated storage scope based on the calling code's assembly identity.</returns>
    public static IsolatedStorageFile GetMachineStoreForAssembly() => IsolatedStorageFile.GetStore(IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine);

    /// <summary>Obtains machine-scoped isolated storage corresponding to the application domain identity and the assembly identity.</summary>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The store failed to open.
    /// 
    /// -or-
    /// 
    /// The assembly specified has insufficient permissions to create isolated stores.
    /// 
    /// -or-
    /// 
    /// The permissions for the application domain cannot be determined.
    /// 
    /// -or-
    /// 
    /// An isolated storage location cannot be initialized.</exception>
    /// <returns>An object corresponding to the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />, based on a combination of the application domain identity and the assembly identity.</returns>
    public static IsolatedStorageFile GetMachineStoreForDomain() => IsolatedStorageFile.GetStore(IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine);


    #nullable disable
    private static IsolatedStorageFile GetStore(IsolatedStorageScope scope) => new IsolatedStorageFile(scope);


    #nullable enable
    /// <summary>Obtains isolated storage corresponding to the isolation scope and the application identity object.</summary>
    /// <param name="scope">A bitwise combination of the enumeration values.</param>
    /// <param name="applicationEvidenceType">An object that contains the application identity.</param>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <exception cref="T:System.ArgumentNullException">The   <paramref name="applicationEvidence" /> identity has not been passed in.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the enumeration value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Application" />, but the application identity of the caller cannot be determined, because the <see cref="P:System.AppDomain.ActivationContext" /> for  the current application domain returned <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Domain" />, but the permissions for the application domain cannot be determined.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Assembly" />, but the permissions for the calling assembly cannot be determined.</exception>
    /// <returns>An object that represents the parameters.</returns>
    public static IsolatedStorageFile GetStore(
      IsolatedStorageScope scope,
      Type? applicationEvidenceType)
    {
      if (!(applicationEvidenceType == (Type) null))
        throw new PlatformNotSupportedException(SR.PlatformNotSupported_CAS);
      return IsolatedStorageFile.GetStore(scope);
    }

    /// <summary>Obtains isolated storage corresponding to the given application identity.</summary>
    /// <param name="scope">A bitwise combination of the enumeration values.</param>
    /// <param name="applicationIdentity">An object that contains evidence for the application identity.</param>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <exception cref="T:System.ArgumentNullException">The  <paramref name="applicationIdentity" /> identity has not been passed in.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the enumeration value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Application" />, but the application identity of the caller cannot be determined,because the <see cref="P:System.AppDomain.ActivationContext" /> for  the current application domain returned <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Domain" />, but the permissions for the application domain cannot be determined.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Assembly" />, but the permissions for the calling assembly cannot be determined.</exception>
    /// <returns>An object that represents the parameters.</returns>
    public static IsolatedStorageFile GetStore(
      IsolatedStorageScope scope,
      object? applicationIdentity)
    {
      if (applicationIdentity != null)
        throw new PlatformNotSupportedException(SR.PlatformNotSupported_CAS);
      return IsolatedStorageFile.GetStore(scope);
    }

    /// <summary>Obtains isolated storage corresponding to the isolated storage scope given the application domain and assembly evidence types.</summary>
    /// <param name="scope">A bitwise combination of the enumeration values.</param>
    /// <param name="domainEvidenceType">The type of the <see cref="T:System.Security.Policy.Evidence" /> that you can chose from the list of <see cref="T:System.Security.Policy.Evidence" /> present in the domain of the calling application. <see langword="null" /> lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence.</param>
    /// <param name="assemblyEvidenceType">The type of the <see cref="T:System.Security.Policy.Evidence" /> that you can chose from the list of <see cref="T:System.Security.Policy.Evidence" /> present in the domain of the calling application. <see langword="null" /> lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence.</param>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The evidence type provided is missing in the assembly evidence list.
    /// 
    /// -or-
    /// 
    /// An isolated storage location cannot be initialized.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the enumeration value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Application" />, but the application identity of the caller cannot be determined, because the <see cref="P:System.AppDomain.ActivationContext" /> for  the current application domain returned <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Domain" />, but the permissions for the application domain cannot be determined.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Assembly" />, but the permissions for the calling assembly cannot be determined.</exception>
    /// <returns>An object that represents the parameters.</returns>
    public static IsolatedStorageFile GetStore(
      IsolatedStorageScope scope,
      Type? domainEvidenceType,
      Type? assemblyEvidenceType)
    {
      if (!(domainEvidenceType == (Type) null) || !(assemblyEvidenceType == (Type) null))
        throw new PlatformNotSupportedException(SR.PlatformNotSupported_CAS);
      return IsolatedStorageFile.GetStore(scope);
    }

    /// <summary>Obtains the isolated storage corresponding to the given application domain and assembly evidence objects.</summary>
    /// <param name="scope">A bitwise combination of the enumeration values.</param>
    /// <param name="domainIdentity">An object that contains evidence for the application domain identity.</param>
    /// <param name="assemblyIdentity">An object that contains evidence for the code assembly identity.</param>
    /// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted.</exception>
    /// <exception cref="T:System.ArgumentNullException">Neither <paramref name="domainIdentity" /> nor <paramref name="assemblyIdentity" /> has been passed in. This verifies that the correct constructor is being used.
    /// 
    /// -or-
    /// 
    /// Either <paramref name="domainIdentity" /> or <paramref name="assemblyIdentity" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the enumeration value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Application" />, but the application identity of the caller cannot be determined, because the <see cref="P:System.AppDomain.ActivationContext" /> for  the current application domain returned <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Domain" />, but the permissions for the application domain cannot be determined.
    /// 
    /// -or-
    /// 
    /// <paramref name="scope" /> contains the value <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Assembly" />, but the permissions for the calling assembly cannot be determined.</exception>
    /// <returns>An object that represents the parameters.</returns>
    public static IsolatedStorageFile GetStore(
      IsolatedStorageScope scope,
      object? domainIdentity,
      object? assemblyIdentity)
    {
      if (domainIdentity != null || assemblyIdentity != null)
        throw new PlatformNotSupportedException(SR.PlatformNotSupported_CAS);
      return IsolatedStorageFile.GetStore(scope);
    }


    #nullable disable
    internal string GetFullPath(string partialPath)
    {
      int num = 0;
      while (num < partialPath.Length && ((int) partialPath[num] == (int) Path.DirectorySeparatorChar || (int) partialPath[num] == (int) Path.AltDirectorySeparatorChar))
        ++num;
      partialPath = partialPath.Substring(num);
      return Path.Combine(this.RootDirectory, partialPath);
    }

    internal void EnsureStoreIsValid()
    {
      if (this.Disposed)
        throw new ObjectDisposedException((string) null, SR.IsolatedStorage_StoreNotOpen);
      if (this._closed || this.IsDeleted)
        throw new InvalidOperationException(SR.IsolatedStorage_StoreNotOpen);
    }

    /// <summary>Releases all resources used by the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" />.</summary>
    public void Dispose()
    {
      this.Close();
      this._disposed = true;
    }

    internal static Exception GetIsolatedStorageException(
      string exceptionMsg,
      Exception rootCause)
    {
      return (Exception) new IsolatedStorageException(exceptionMsg, rootCause)
      {
        _underlyingException = rootCause
      };
    }

    /// <summary>Enables an application to explicitly request a larger quota size, in bytes.</summary>
    /// <param name="newQuotaSize">The requested size, in bytes.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="newQuotaSize" /> is less than current quota size.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="newQuotaSize" /> is less than zero, or less than or equal to the current quota size.</exception>
    /// <exception cref="T:System.InvalidOperationException">The isolated store has been closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current scope is not for an application user.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed.
    /// 
    /// -or-
    /// 
    /// Isolated storage is disabled.</exception>
    /// <returns>
    /// <see langword="true" /> if the new quota is accepted; otherwise, <see langword="false" />.</returns>
    public override bool IncreaseQuotaTo(long newQuotaSize) => true;

    /// <summary>Removes the isolated storage scope and all its contents.</summary>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store cannot be deleted.</exception>
    public override void Remove()
    {
      try
      {
        Directory.Delete(this.RootDirectory, true);
      }
      catch
      {
        throw new IsolatedStorageException(SR.IsolatedStorage_DeleteDirectories);
      }
      this.Close();
      string directoryName1 = Path.GetDirectoryName(this.RootDirectory.TrimEnd(Path.DirectorySeparatorChar));
      if (this.ContainsUnknownFiles(directoryName1))
        return;
      try
      {
        Directory.Delete(directoryName1, true);
      }
      catch
      {
        return;
      }
      if (!Helper.IsDomain(this.Scope))
        return;
      string directoryName2 = Path.GetDirectoryName(directoryName1);
      if (this.ContainsUnknownFiles(directoryName2))
        return;
      try
      {
        Directory.Delete(directoryName2, true);
      }
      catch
      {
      }
    }

    /// <summary>Removes the specified isolated storage scope for all identities.</summary>
    /// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store cannot be removed.</exception>
    public static void Remove(IsolatedStorageScope scope)
    {
      IsolatedStorageFile.VerifyGlobalScope(scope);
      string rootDirectory = Helper.GetRootDirectory(scope);
      try
      {
        Directory.Delete(rootDirectory, true);
        Directory.CreateDirectory(rootDirectory);
      }
      catch
      {
        throw new IsolatedStorageException(SR.IsolatedStorage_DeleteDirectories);
      }
    }

    /// <summary>Gets a value that indicates whether isolated storage is enabled.</summary>
    /// <returns>
    /// <see langword="true" /> in all cases.</returns>
    public static bool IsEnabled => true;

    private static void VerifyGlobalScope(IsolatedStorageScope scope)
    {
      if (scope != IsolatedStorageScope.User && scope != (IsolatedStorageScope.User | IsolatedStorageScope.Roaming) && scope != IsolatedStorageScope.Machine)
        throw new ArgumentException(SR.IsolatedStorage_Scope_U_R_M);
    }

    private bool ContainsUnknownFiles(string directory)
    {
      string[] files;
      string[] directories;
      try
      {
        files = Directory.GetFiles(directory);
        directories = Directory.GetDirectories(directory);
      }
      catch
      {
        throw new IsolatedStorageException(SR.IsolatedStorage_DeleteDirectories);
      }
      if (directories.Length > 1 || directories.Length != 0 && !this.IsMatchingScopeDirectory(directories[0]))
        return true;
      if (files.Length == 0)
        return false;
      if (Helper.IsRoaming(this.Scope))
        return files.Length > 1 || !IsolatedStorageFile.IsIdFile(files[0]);
      if (files.Length > 2 || !IsolatedStorageFile.IsIdFile(files[0]) && !IsolatedStorageFile.IsInfoFile(files[0]))
        return true;
      return files.Length == 2 && !IsolatedStorageFile.IsIdFile(files[1]) && !IsolatedStorageFile.IsInfoFile(files[1]);
    }

    private bool IsMatchingScopeDirectory(string directory)
    {
      string fileName = Path.GetFileName(directory);
      if (Helper.IsApplication(this.Scope) && string.Equals(fileName, "AppFiles", StringComparison.Ordinal) || Helper.IsAssembly(this.Scope) && string.Equals(fileName, "AssemFiles", StringComparison.Ordinal))
        return true;
      return Helper.IsDomain(this.Scope) && string.Equals(fileName, "Files", StringComparison.Ordinal);
    }

    private static bool IsIdFile(string file) => string.Equals(Path.GetFileName(file), "identity.dat");

    private static bool IsInfoFile(string file) => string.Equals(Path.GetFileName(file), "info.dat");

    internal sealed class IsolatedStorageFileEnumerator : IEnumerator
    {
      public object Current => throw new InvalidOperationException();

      public bool MoveNext() => false;

      public void Reset()
      {
      }
    }
  }
}
