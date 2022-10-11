// Decompiled with JetBrains decompiler
// Type: System.IO.FileSystemInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.IO.Enumeration;
using System.Runtime.Serialization;


#nullable enable
namespace System.IO
{
  /// <summary>Provides the base class for both <see cref="T:System.IO.FileInfo" /> and <see cref="T:System.IO.DirectoryInfo" /> objects.</summary>
  public abstract class FileSystemInfo : MarshalByRefObject, ISerializable
  {
    /// <summary>Represents the fully qualified path of the directory or file.</summary>
    /// <exception cref="T:System.IO.PathTooLongException">The fully qualified path exceeds the system-defined maximum length.</exception>
    protected string FullPath;
    /// <summary>The path originally specified by the user, whether relative or absolute.</summary>
    protected string OriginalPath;

    #nullable disable
    internal string _name;
    private string _linkTarget;
    private bool _linkTargetIsValid;
    private Interop.Kernel32.WIN32_FILE_ATTRIBUTE_DATA _data;
    private int _dataInitialized = -1;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemInfo" /> class with serialized data.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    /// <exception cref="T:System.ArgumentNullException">The specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> is null.</exception>
    protected FileSystemInfo(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    internal void Invalidate()
    {
      this._linkTargetIsValid = false;
      this.InvalidateCore();
    }

    /// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name and additional exception information.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    /// <summary>Gets the full path of the directory or file.</summary>
    /// <exception cref="T:System.IO.PathTooLongException">The fully qualified path and file name exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A string containing the full path.</returns>
    public virtual string FullName => this.FullPath;

    /// <summary>Gets the extension part of the file name, including the leading dot <c>.</c> even if it is the entire file name, or an empty string if no extension is present.</summary>
    /// <returns>A string containing the <see cref="T:System.IO.FileSystemInfo" /> extension.</returns>
    public string Extension
    {
      get
      {
        int length = this.FullPath.Length;
        int num = length;
        while (--num >= 0)
        {
          char c = this.FullPath[num];
          if (c == '.')
            return this.FullPath.Substring(num, length - num);
          if (PathInternal.IsDirectorySeparator(c) || (int) c == (int) Path.VolumeSeparatorChar)
            break;
        }
        return string.Empty;
      }
    }

    /// <summary>For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the <see langword="Name" /> property gets the name of the directory.</summary>
    /// <returns>A string that is the name of the parent directory, the name of the last directory in the hierarchy, or the name of a file, including the file name extension.</returns>
    public virtual string Name => this._name;

    /// <summary>Gets a value indicating whether the file or directory exists.</summary>
    /// <returns>
    /// <see langword="true" /> if the file or directory exists; otherwise, <see langword="false" />.</returns>
    public virtual bool Exists
    {
      get
      {
        try
        {
          return this.ExistsCore;
        }
        catch
        {
          return false;
        }
      }
    }

    /// <summary>Deletes a file or directory.</summary>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">There is an open handle on the file or directory, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and files. For more information, see How to: Enumerate Directories and Files.</exception>
    public abstract void Delete();

    /// <summary>Gets or sets the creation time of the current file or directory.</summary>
    /// <exception cref="T:System.IO.IOException">
    /// <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The caller attempts to set an invalid creation time.</exception>
    /// <returns>The creation date and time of the current <see cref="T:System.IO.FileSystemInfo" /> object.</returns>
    public DateTime CreationTime
    {
      get => this.CreationTimeUtc.ToLocalTime();
      set => this.CreationTimeUtc = value.ToUniversalTime();
    }

    /// <summary>Gets or sets the creation time, in coordinated universal time (UTC), of the current file or directory.</summary>
    /// <exception cref="T:System.IO.IOException">
    /// <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The caller attempts to set an invalid access time.</exception>
    /// <returns>The creation date and time in UTC format of the current <see cref="T:System.IO.FileSystemInfo" /> object.</returns>
    public DateTime CreationTimeUtc
    {
      get => this.CreationTimeCore.UtcDateTime;
      set => this.CreationTimeCore = File.GetUtcDateTimeOffset(value);
    }

    /// <summary>Gets or sets the time the current file or directory was last accessed.</summary>
    /// <exception cref="T:System.IO.IOException">
    /// <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The caller attempts to set an invalid access time</exception>
    /// <returns>The time that the current file or directory was last accessed.</returns>
    public DateTime LastAccessTime
    {
      get => this.LastAccessTimeUtc.ToLocalTime();
      set => this.LastAccessTimeUtc = value.ToUniversalTime();
    }

    /// <summary>Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed.</summary>
    /// <exception cref="T:System.IO.IOException">
    /// <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The caller attempts to set an invalid access time.</exception>
    /// <returns>The UTC time that the current file or directory was last accessed.</returns>
    public DateTime LastAccessTimeUtc
    {
      get => this.LastAccessTimeCore.UtcDateTime;
      set => this.LastAccessTimeCore = File.GetUtcDateTimeOffset(value);
    }

    /// <summary>Gets or sets the time when the current file or directory was last written to.</summary>
    /// <exception cref="T:System.IO.IOException">
    /// <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The caller attempts to set an invalid write time.</exception>
    /// <returns>The time the current file was last written.</returns>
    public DateTime LastWriteTime
    {
      get => this.LastWriteTimeUtc.ToLocalTime();
      set => this.LastWriteTimeUtc = value.ToUniversalTime();
    }

    /// <summary>Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.</summary>
    /// <exception cref="T:System.IO.IOException">
    /// <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The caller attempts to set an invalid write time.</exception>
    /// <returns>The UTC time when the current file was last written to.</returns>
    public DateTime LastWriteTimeUtc
    {
      get => this.LastWriteTimeCore.UtcDateTime;
      set => this.LastWriteTimeCore = File.GetUtcDateTimeOffset(value);
    }

    /// <summary>If this <see cref="T:System.IO.FileSystemInfo" /> instance represents a link, returns the link target's path.
    /// If a link does not exist in <see cref="P:System.IO.FileSystemInfo.FullName" />, or this instance does not represent a link, returns <see langword="null" />.</summary>
    public string? LinkTarget
    {
      get
      {
        if (this._linkTargetIsValid)
          return this._linkTarget;
        this._linkTarget = FileSystem.GetLinkTarget(this.FullPath, this is DirectoryInfo);
        this._linkTargetIsValid = true;
        return this._linkTarget;
      }
    }

    /// <summary>Creates a symbolic link located in <see cref="P:System.IO.FileSystemInfo.FullName" /> that points to the specified <paramref name="pathToTarget" />.</summary>
    /// <param name="pathToTarget">The path of the symbolic link target.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pathToTarget" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="pathToTarget" /> is empty.
    /// 
    /// -or-
    /// 
    /// <paramref name="pathToTarget" /> contains invalid path characters.</exception>
    /// <exception cref="T:System.IO.IOException">A file or directory already exists in the location of <see cref="P:System.IO.FileSystemInfo.FullName" />.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred.</exception>
    public void CreateAsSymbolicLink(string pathToTarget)
    {
      FileSystem.VerifyValidPath(pathToTarget, nameof (pathToTarget));
      FileSystem.CreateSymbolicLink(this.OriginalPath, pathToTarget, this is DirectoryInfo);
      this.Invalidate();
    }

    /// <summary>Gets the target of the specified link.</summary>
    /// <param name="returnFinalTarget">
    /// <see langword="true" /> to follow links to the final target; <see langword="false" /> to return the immediate next link.</param>
    /// <exception cref="T:System.IO.IOException">The file or directory does not exist.
    /// 
    /// -or-
    /// 
    /// There are too many levels of symbolic links.</exception>
    /// <returns>A <see cref="T:System.IO.FileSystemInfo" /> instance if the link exists, independently if the target exists or not; <see langword="null" /> if this file or directory is not a link.</returns>
    public FileSystemInfo? ResolveLinkTarget(bool returnFinalTarget) => FileSystem.ResolveLinkTarget(this.FullPath, returnFinalTarget, this is DirectoryInfo);

    /// <summary>Returns the original path. Use the <see cref="P:System.IO.FileSystemInfo.FullName" /> or <see cref="P:System.IO.FileSystemInfo.Name" /> properties for the full path or file/directory name.</summary>
    /// <returns>A string with the original path.</returns>
    public override string ToString() => this.OriginalPath ?? string.Empty;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemInfo" /> class.</summary>
    protected FileSystemInfo()
    {
    }


    #nullable disable
    internal static unsafe FileSystemInfo Create(
      string fullPath,
      ref FileSystemEntry findData)
    {
      FileSystemInfo fileSystemInfo = findData.IsDirectory ? (FileSystemInfo) new DirectoryInfo(fullPath, fileName: findData.FileName.ToString(), isNormalized: true) : (FileSystemInfo) new FileInfo(fullPath, fileName: findData.FileName.ToString(), isNormalized: true);
      fileSystemInfo.Init(findData._info);
      return fileSystemInfo;
    }

    internal void InvalidateCore() => this._dataInitialized = -1;

    internal unsafe void Init(Interop.NtDll.FILE_FULL_DIR_INFORMATION* info)
    {
      this._data.dwFileAttributes = (int) info->FileAttributes;
      this._data.ftCreationTime = *(Interop.Kernel32.FILE_TIME*) &info->CreationTime;
      this._data.ftLastAccessTime = *(Interop.Kernel32.FILE_TIME*) &info->LastAccessTime;
      this._data.ftLastWriteTime = *(Interop.Kernel32.FILE_TIME*) &info->LastWriteTime;
      this._data.nFileSizeHigh = (uint) (info->EndOfFile >> 32);
      this._data.nFileSizeLow = (uint) info->EndOfFile;
      this._dataInitialized = 0;
    }

    /// <summary>Gets or sets the attributes for the current file or directory.</summary>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified file doesn't exist. Only thrown when setting the property value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid. For example, it's on an unmapped drive. Only thrown when setting the property value.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller doesn't have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">.NET Core and .NET 5+ only: The user attempts to set an attribute value but doesn't have write permission.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.ArgumentException">The caller attempts to set an invalid file attribute.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: The user attempts to set an attribute value but doesn't have write permission.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot initialize the data.</exception>
    /// <returns>
    /// <see cref="T:System.IO.FileAttributes" /> of the current <see cref="T:System.IO.FileSystemInfo" />.</returns>
    public FileAttributes Attributes
    {
      get
      {
        this.EnsureDataInitialized();
        return (FileAttributes) this._data.dwFileAttributes;
      }
      set
      {
        FileSystem.SetAttributes(this.FullPath, value);
        this._dataInitialized = -1;
      }
    }

    internal bool ExistsCore
    {
      get
      {
        if (this._dataInitialized == -1)
          this.RefreshCore();
        return this._dataInitialized == 0 && this._data.dwFileAttributes != -1 && this is DirectoryInfo == ((this._data.dwFileAttributes & 16) == 16);
      }
    }

    internal DateTimeOffset CreationTimeCore
    {
      get
      {
        this.EnsureDataInitialized();
        return this._data.ftCreationTime.ToDateTimeOffset();
      }
      set
      {
        FileSystem.SetCreationTime(this.FullPath, value, this is DirectoryInfo);
        this._dataInitialized = -1;
      }
    }

    internal DateTimeOffset LastAccessTimeCore
    {
      get
      {
        this.EnsureDataInitialized();
        return this._data.ftLastAccessTime.ToDateTimeOffset();
      }
      set
      {
        FileSystem.SetLastAccessTime(this.FullPath, value, this is DirectoryInfo);
        this._dataInitialized = -1;
      }
    }

    internal DateTimeOffset LastWriteTimeCore
    {
      get
      {
        this.EnsureDataInitialized();
        return this._data.ftLastWriteTime.ToDateTimeOffset();
      }
      set
      {
        FileSystem.SetLastWriteTime(this.FullPath, value, this is DirectoryInfo);
        this._dataInitialized = -1;
      }
    }

    internal long LengthCore
    {
      get
      {
        this.EnsureDataInitialized();
        return (long) this._data.nFileSizeHigh << 32 | (long) this._data.nFileSizeLow & (long) uint.MaxValue;
      }
    }

    private void EnsureDataInitialized()
    {
      if (this._dataInitialized == -1)
      {
        this._data = new Interop.Kernel32.WIN32_FILE_ATTRIBUTE_DATA();
        this.RefreshCore();
      }
      if (this._dataInitialized != 0)
        throw Win32Marshal.GetExceptionForWin32Error(this._dataInitialized, this.FullPath);
    }

    /// <summary>Refreshes the state of the object.</summary>
    /// <exception cref="T:System.IO.IOException">A device such as a disk drive is not ready.</exception>
    public void Refresh()
    {
      this._linkTargetIsValid = false;
      this.RefreshCore();
    }

    private void RefreshCore() => this._dataInitialized = FileSystem.FillAttributeInfo(this.FullPath, ref this._data, false);


    #nullable enable
    internal string NormalizedPath => !PathInternal.EndsWithPeriodOrSpace(this.FullPath) ? this.FullPath : PathInternal.EnsureExtendedPrefix(this.FullPath);
  }
}
