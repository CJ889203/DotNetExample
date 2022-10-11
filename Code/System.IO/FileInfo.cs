// Decompiled with JetBrains decompiler
// Type: System.IO.FileInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.Versioning;
using System.Text;


#nullable enable
namespace System.IO
{
  /// <summary>Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="T:System.IO.FileStream" /> objects. This class cannot be inherited.</summary>
  public sealed class FileInfo : FileSystemInfo
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileInfo" /> class, which acts as a wrapper for a file path.</summary>
    /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="fileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: The file name is empty, contains only white spaces, or contains invalid characters.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to <paramref name="fileName" /> is denied.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="fileName" /> contains a colon (:) in the middle of the string.</exception>
    public FileInfo(string fileName)
      : this(fileName, (string) null, (string) null, false)
    {
    }


    #nullable disable
    internal FileInfo(string originalPath, string fullPath = null, string fileName = null, bool isNormalized = false)
    {
      this.OriginalPath = originalPath ?? throw new ArgumentNullException(nameof (fileName));
      fullPath = fullPath ?? originalPath;
      this.FullPath = isNormalized ? fullPath ?? originalPath : Path.GetFullPath(fullPath);
      this._name = fileName ?? Path.GetFileName(originalPath);
    }

    /// <summary>Gets the size, in bytes, of the current file.</summary>
    /// <exception cref="T:System.IO.IOException">
    /// <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot update the state of the file or directory.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file does not exist.
    /// 
    /// -or-
    /// 
    /// The <see langword="Length" /> property is called for a directory.</exception>
    /// <returns>The size of the current file in bytes.</returns>
    public long Length
    {
      get
      {
        if ((this.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
          throw new FileNotFoundException(SR.Format(SR.IO_FileNotFound_FileName, (object) this.FullPath), this.FullPath);
        return this.LengthCore;
      }
    }


    #nullable enable
    /// <summary>Gets a string representing the directory's full path.</summary>
    /// <exception cref="T:System.ArgumentNullException">
    /// <see langword="null" /> was passed in for the directory name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The fully qualified path name exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A string representing the directory's full path.</returns>
    public string? DirectoryName => Path.GetDirectoryName(this.FullPath);

    /// <summary>Gets an instance of the parent directory.</summary>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A <see cref="T:System.IO.DirectoryInfo" /> object representing the parent directory of this file.</returns>
    public DirectoryInfo? Directory
    {
      get
      {
        string directoryName = this.DirectoryName;
        return directoryName == null ? (DirectoryInfo) null : new DirectoryInfo(directoryName);
      }
    }

    /// <summary>Gets or sets a value that determines if the current file is read only.</summary>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">The user does not have write permission, but attempted to set this property to <see langword="false" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the current file is read only; otherwise, <see langword="false" />.</returns>
    public bool IsReadOnly
    {
      get => (this.Attributes & FileAttributes.ReadOnly) != 0;
      set
      {
        if (value)
          this.Attributes |= FileAttributes.ReadOnly;
        else
          this.Attributes &= ~FileAttributes.ReadOnly;
      }
    }

    /// <summary>Creates a <see cref="T:System.IO.StreamReader" /> with UTF8 encoding that reads from an existing text file.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <see cref="P:System.IO.FileInfo.Name" /> is read-only or is a directory.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <returns>A new <see langword="StreamReader" /> with UTF8 encoding.</returns>
    public StreamReader OpenText() => new StreamReader(this.NormalizedPath, Encoding.UTF8, true);

    /// <summary>Creates a <see cref="T:System.IO.StreamWriter" /> that writes a new text file.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">The file name is a directory.</exception>
    /// <exception cref="T:System.IO.IOException">The disk is read-only.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A new <see langword="StreamWriter" />.</returns>
    public StreamWriter CreateText() => new StreamWriter(this.NormalizedPath, false);

    /// <summary>Creates a <see cref="T:System.IO.StreamWriter" /> that appends text to the file represented by this instance of the <see cref="T:System.IO.FileInfo" />.</summary>
    /// <returns>A new <see langword="StreamWriter" />.</returns>
    public StreamWriter AppendText() => new StreamWriter(this.NormalizedPath, true);

    /// <summary>Copies an existing file to a new file, disallowing the overwriting of an existing file.</summary>
    /// <param name="destFileName">The name of the new file to copy to.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
    /// <exception cref="T:System.IO.IOException">An error occurs, or the destination file already exists.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">A directory path is passed in, or the file is being moved to a different drive.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory specified in <paramref name="destFileName" /> does not exist.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destFileName" /> contains a colon (:) within the string but does not specify the volume.</exception>
    /// <returns>A new file with a fully qualified path.</returns>
    public FileInfo CopyTo(string destFileName) => this.CopyTo(destFileName, false);

    /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
    /// <param name="destFileName">The name of the new file to copy to.</param>
    /// <param name="overwrite">
    /// <see langword="true" /> to allow an existing file to be overwritten; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
    /// <exception cref="T:System.IO.IOException">An error occurs, or the destination file already exists and <paramref name="overwrite" /> is <see langword="false" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory specified in <paramref name="destFileName" /> does not exist.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">A directory path is passed in, or the file is being moved to a different drive.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destFileName" /> contains a colon (:) in the middle of the string.</exception>
    /// <returns>A new file, or an overwrite of an existing file if <paramref name="overwrite" /> is <see langword="true" />. If the file exists and <paramref name="overwrite" /> is <see langword="false" />, an <see cref="T:System.IO.IOException" /> is thrown.</returns>
    public FileInfo CopyTo(string destFileName, bool overwrite)
    {
      switch (destFileName)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyFileName, nameof (destFileName));
        case null:
          throw new ArgumentNullException(nameof (destFileName), SR.ArgumentNull_FileName);
        default:
          string fullPath = Path.GetFullPath(destFileName);
          FileSystem.CopyFile(this.FullPath, fullPath, overwrite);
          return new FileInfo(fullPath, isNormalized: true);
      }
    }

    /// <summary>Creates a file.</summary>
    /// <returns>A new file.</returns>
    public FileStream Create()
    {
      FileStream fileStream = File.Create(this.NormalizedPath);
      this.Invalidate();
      return fileStream;
    }

    /// <summary>Permanently deletes a file.</summary>
    /// <exception cref="T:System.IO.IOException">The target file is open or memory-mapped on a computer running Microsoft Windows NT.
    /// 
    /// -or-
    /// 
    /// There is an open handle on the file, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and files. For more information, see How to: Enumerate Directories and Files.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The path is a directory.</exception>
    public override void Delete()
    {
      FileSystem.DeleteFile(this.FullPath);
      this.Invalidate();
    }

    /// <summary>Opens a file in the specified mode.</summary>
    /// <param name="mode">A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, <see langword="Open" /> or <see langword="Append" />) in which to open the file.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The file is read-only or is a directory.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">The file is already open.</exception>
    /// <returns>A file opened in the specified mode, with read/write access and unshared.</returns>
    public FileStream Open(FileMode mode) => this.Open(mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);

    /// <summary>Opens a file in the specified mode with read, write, or read/write access.</summary>
    /// <param name="mode">A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, <see langword="Open" /> or <see langword="Append" />) in which to open the file.</param>
    /// <param name="access">A <see cref="T:System.IO.FileAccess" /> constant specifying whether to open the file with <see langword="Read" />, <see langword="Write" />, or <see langword="ReadWrite" /> file access.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <see cref="P:System.IO.FileInfo.Name" /> is read-only or is a directory.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">The file is already open.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <see cref="P:System.IO.FileInfo.Name" /> is empty or contains only white spaces.</exception>
    /// <exception cref="T:System.ArgumentNullException">One or more arguments is null.</exception>
    /// <returns>A <see cref="T:System.IO.FileStream" /> object opened in the specified mode and access, and unshared.</returns>
    public FileStream Open(FileMode mode, FileAccess access) => this.Open(mode, access, FileShare.None);

    /// <summary>Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
    /// <param name="mode">A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, <see langword="Open" /> or <see langword="Append" />) in which to open the file.</param>
    /// <param name="access">A <see cref="T:System.IO.FileAccess" /> constant specifying whether to open the file with <see langword="Read" />, <see langword="Write" />, or <see langword="ReadWrite" /> file access.</param>
    /// <param name="share">A <see cref="T:System.IO.FileShare" /> constant specifying the type of access other <see langword="FileStream" /> objects have to this file.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <see cref="P:System.IO.FileInfo.Name" /> is read-only or is a directory.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">The file is already open.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <see cref="P:System.IO.FileInfo.Name" /> is empty or contains only white spaces.</exception>
    /// <exception cref="T:System.ArgumentNullException">One or more arguments is null.</exception>
    /// <returns>A <see cref="T:System.IO.FileStream" /> object opened with the specified mode, access, and sharing options.</returns>
    public FileStream Open(FileMode mode, FileAccess access, FileShare share) => new FileStream(this.NormalizedPath, mode, access, share);

    /// <summary>Creates a read-only <see cref="T:System.IO.FileStream" />.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <see cref="P:System.IO.FileInfo.Name" /> is read-only or is a directory.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">The file is already open.</exception>
    /// <returns>A new read-only <see cref="T:System.IO.FileStream" /> object.</returns>
    public FileStream OpenRead() => new FileStream(this.NormalizedPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false);

    /// <summary>Creates a write-only <see cref="T:System.IO.FileStream" />.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">The path specified when creating an instance of the <see cref="T:System.IO.FileInfo" /> object is read-only or is a directory.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified when creating an instance of the <see cref="T:System.IO.FileInfo" /> object is invalid, such as being on an unmapped drive.</exception>
    /// <returns>A write-only unshared <see cref="T:System.IO.FileStream" /> object for a new or existing file.</returns>
    public FileStream OpenWrite() => new FileStream(this.NormalizedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

    /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
    /// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the destination file already exists or the destination device is not ready.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="destFileName" /> is read-only or is a directory.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destFileName" /> contains a colon (:) in the middle of the string.</exception>
    public void MoveTo(string destFileName) => this.MoveTo(destFileName, false);

    /// <summary>Moves a specified file to a new location, providing the options to specify a new file name and to overwrite the destination file if it already exists.</summary>
    /// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
    /// <param name="overwrite">
    /// <see langword="true" /> to overwrite the destination file if it already exists; <see langword="false" /> otherwise.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred, such as the destination device is not ready.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="destFileName" /> is read-only or is a directory.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destFileName" /> contains a colon (:) in the middle of the string.</exception>
    public void MoveTo(string destFileName, bool overwrite)
    {
      switch (destFileName)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyFileName, nameof (destFileName));
        case null:
          throw new ArgumentNullException(nameof (destFileName));
        default:
          string fullPath = Path.GetFullPath(destFileName);
          if (!new DirectoryInfo(Path.GetDirectoryName(this.FullName)).Exists)
            throw new DirectoryNotFoundException(SR.Format(SR.IO_PathNotFound_Path, (object) this.FullName));
          if (!this.Exists)
            throw new FileNotFoundException(SR.Format(SR.IO_FileNotFound_FileName, (object) this.FullName), this.FullName);
          FileSystem.MoveFile(this.FullPath, fullPath, overwrite);
          this.FullPath = fullPath;
          this.OriginalPath = destFileName;
          this._name = Path.GetFileName(fullPath);
          this.Invalidate();
          break;
      }
    }

    /// <summary>Replaces the contents of a specified file with the file described by the current <see cref="T:System.IO.FileInfo" /> object, deleting the original file, and creating a backup of the replaced file.</summary>
    /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
    /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destFileName" /> parameter.</param>
    /// <exception cref="T:System.ArgumentException">The path described by the <paramref name="destFileName" /> parameter was not of a legal form.
    /// 
    /// -or-
    /// 
    /// The path described by the <paramref name="destBackupFileName" /> parameter was not of a legal form.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="destFileName" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.
    /// 
    /// -or-
    /// 
    /// The file described by the <paramref name="destinationFileName" /> parameter could not be found.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
    /// <returns>A <see cref="T:System.IO.FileInfo" /> object that encapsulates information about the file described by the <paramref name="destFileName" /> parameter.</returns>
    public FileInfo Replace(string destinationFileName, string? destinationBackupFileName) => this.Replace(destinationFileName, destinationBackupFileName, false);

    /// <summary>Replaces the contents of a specified file with the file described by the current <see cref="T:System.IO.FileInfo" /> object, deleting the original file, and creating a backup of the replaced file.  Also specifies whether to ignore merge errors.</summary>
    /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
    /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destFileName" /> parameter.</param>
    /// <param name="ignoreMetadataErrors">
    /// <see langword="true" /> to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">The path described by the <paramref name="destFileName" /> parameter was not of a legal form.
    /// 
    /// -or-
    /// 
    /// The path described by the <paramref name="destBackupFileName" /> parameter was not of a legal form.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="destFileName" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.
    /// 
    /// -or-
    /// 
    /// The file described by the <paramref name="destinationFileName" /> parameter could not be found.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
    /// <returns>A <see cref="T:System.IO.FileInfo" /> object that encapsulates information about the file described by the <paramref name="destFileName" /> parameter.</returns>
    public FileInfo Replace(
      string destinationFileName,
      string? destinationBackupFileName,
      bool ignoreMetadataErrors)
    {
      if (destinationFileName == null)
        throw new ArgumentNullException(nameof (destinationFileName));
      FileSystem.ReplaceFile(this.FullPath, Path.GetFullPath(destinationFileName), destinationBackupFileName != null ? Path.GetFullPath(destinationBackupFileName) : (string) null, ignoreMetadataErrors);
      return new FileInfo(destinationFileName);
    }

    /// <summary>Decrypts a file that was encrypted by the current account using the <see cref="M:System.IO.FileInfo.Encrypt" /> method.</summary>
    /// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The file described by the current <see cref="T:System.IO.FileInfo" /> object is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    [SupportedOSPlatform("windows")]
    public void Decrypt() => File.Decrypt(this.FullPath);

    /// <summary>Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
    /// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The file described by the current <see cref="T:System.IO.FileInfo" /> object is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    [SupportedOSPlatform("windows")]
    public void Encrypt() => File.Encrypt(this.FullPath);

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified creation mode, read/write and sharing permission, the access other FileStreams can have to the same file, the buffer size, additional file options and the allocation size.</summary>
    /// <param name="options">An object that describes optional <see cref="T:System.IO.FileStream" /> parameters to use.</param>
    /// <returns>A <see cref="T:System.IO.FileStream" /> that wraps the opened file.</returns>
    public FileStream Open(FileStreamOptions options) => File.Open(this.NormalizedPath, options);
  }
}
