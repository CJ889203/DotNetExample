// Decompiled with JetBrains decompiler
// Type: System.IO.DirectoryInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.IO.Enumeration;


#nullable enable
namespace System.IO
{
  /// <summary>Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.</summary>
  public sealed class DirectoryInfo : FileSystemInfo
  {
    private bool _isNormalized;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.DirectoryInfo" /> class on the specified path.</summary>
    /// <param name="path">A string specifying the path on which to create the <see langword="DirectoryInfo" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> contains invalid characters such as ", &lt;, &gt;, or |.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    public DirectoryInfo(string path) => this.Init(path, Path.GetFullPath(path), isNormalized: true);


    #nullable disable
    internal DirectoryInfo(
      string originalPath,
      string fullPath = null,
      string fileName = null,
      bool isNormalized = false)
    {
      this.Init(originalPath, fullPath, fileName, isNormalized);
    }

    private void Init(string originalPath, string fullPath = null, string fileName = null, bool isNormalized = false)
    {
      this.OriginalPath = originalPath ?? throw new ArgumentNullException(nameof (originalPath));
      fullPath = fullPath ?? originalPath;
      fullPath = isNormalized ? fullPath : Path.GetFullPath(fullPath);
      this._name = fileName ?? (PathInternal.IsRoot(fullPath.AsSpan()) ? fullPath.AsSpan() : Path.GetFileName(Path.TrimEndingDirectorySeparator(fullPath.AsSpan()))).ToString();
      this.FullPath = fullPath;
      this._isNormalized = isNormalized;
    }


    #nullable enable
    /// <summary>Gets the parent directory of a specified subdirectory.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>The parent directory, or <see langword="null" /> if the path is null or if the file path denotes a root (such as \, C:\, or \\server\share).</returns>
    public DirectoryInfo? Parent
    {
      get
      {
        string directoryName = Path.GetDirectoryName(PathInternal.IsRoot(this.FullPath.AsSpan()) ? this.FullPath : Path.TrimEndingDirectorySeparator(this.FullPath));
        return directoryName == null ? (DirectoryInfo) null : new DirectoryInfo(directoryName, isNormalized: true);
      }
    }

    /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:System.IO.DirectoryInfo" /> class.</summary>
    /// <param name="path">The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> does not specify a valid file path or contains invalid <see langword="DirectoryInfo" /> characters.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">The subdirectory cannot be created.
    /// 
    /// -or-
    /// 
    /// A file or directory already has the name specified by <paramref name="path" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have code access permission to create the directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have code access permission to read the directory described by the returned <see cref="T:System.IO.DirectoryInfo" /> object.  This can occur when the <paramref name="path" /> parameter describes an existing directory.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> contains a colon character (:) that is not part of a drive label ("C:\").</exception>
    /// <returns>The last directory specified in <paramref name="path" />.</returns>
    public DirectoryInfo CreateSubdirectory(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (PathInternal.IsEffectivelyEmpty(path.AsSpan()))
        throw new ArgumentException(SR.Argument_PathEmpty, nameof (path));
      string str = !Path.IsPathRooted(path) ? Path.GetFullPath(Path.Combine(this.FullPath, path)) : throw new ArgumentException(SR.Arg_Path2IsRooted, nameof (path));
      ReadOnlySpan<char> span = Path.TrimEndingDirectorySeparator(str.AsSpan());
      ReadOnlySpan<char> readOnlySpan = Path.TrimEndingDirectorySeparator(this.FullPath.AsSpan());
      if (!span.StartsWith(readOnlySpan, PathInternal.StringComparison) || span.Length != readOnlySpan.Length && !PathInternal.IsDirectorySeparator(str[readOnlySpan.Length]))
        throw new ArgumentException(SR.Format(SR.Argument_InvalidSubPath, (object) path, (object) this.FullPath), nameof (path));
      FileSystem.CreateDirectory(str);
      return new DirectoryInfo(str);
    }

    /// <summary>Creates a directory.</summary>
    /// <exception cref="T:System.IO.IOException">The directory cannot be created.</exception>
    public void Create()
    {
      FileSystem.CreateDirectory(this.FullPath);
      this.Invalidate();
    }

    /// <summary>Returns a file list from the current directory.</summary>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid, such as being on an unmapped drive.</exception>
    /// <returns>An array of type <see cref="T:System.IO.FileInfo" />.</returns>
    public FileInfo[] GetFiles() => this.GetFiles("*", EnumerationOptions.Compatible);

    /// <summary>Returns a file list from the current directory matching the given search pattern.</summary>
    /// <param name="searchPattern">The search string to match against the names of files.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An array of type <see cref="T:System.IO.FileInfo" />.</returns>
    public FileInfo[] GetFiles(string searchPattern) => this.GetFiles(searchPattern, EnumerationOptions.Compatible);

    /// <summary>Returns a file list from the current directory matching the given search pattern and using a value to determine whether to search subdirectories.</summary>
    /// <param name="searchPattern">The search string to match against the names of files.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An array of type <see cref="T:System.IO.FileInfo" />.</returns>
    public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption) => this.GetFiles(searchPattern, EnumerationOptions.FromSearchOption(searchOption));

    /// <summary>Returns a file list from the current directory matching the specified search pattern and enumeration options.</summary>
    /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An array of strongly typed <see cref="T:System.IO.FileInfo" /> objects that match <paramref name="searchPattern" /> and <paramref name="enumerationOptions" />.</returns>
    public FileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions) => new List<FileInfo>((IEnumerable<FileInfo>) this.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Files, enumerationOptions)).ToArray();

    /// <summary>Returns an array of strongly typed <see cref="T:System.IO.FileSystemInfo" /> entries representing all the files and subdirectories in a directory.</summary>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of strongly typed <see cref="T:System.IO.FileSystemInfo" /> entries.</returns>
    public FileSystemInfo[] GetFileSystemInfos() => this.GetFileSystemInfos("*", EnumerationOptions.Compatible);

    /// <summary>Retrieves an array of strongly typed <see cref="T:System.IO.FileSystemInfo" /> objects representing the files and subdirectories that match the specified search criteria.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories and files.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An array of strongly typed <see langword="FileSystemInfo" /> objects matching the search criteria.</returns>
    public FileSystemInfo[] GetFileSystemInfos(string searchPattern) => this.GetFileSystemInfos(searchPattern, EnumerationOptions.Compatible);

    /// <summary>Retrieves an array of <see cref="T:System.IO.FileSystemInfo" /> objects that represent the files and subdirectories matching the specified search criteria.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories and files.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An array of file system entries that match the search criteria.</returns>
    public FileSystemInfo[] GetFileSystemInfos(
      string searchPattern,
      SearchOption searchOption)
    {
      return this.GetFileSystemInfos(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Retrieves an array of strongly typed <see cref="T:System.IO.FileSystemInfo" /> objects representing the files and subdirectories that match the specified search pattern and enumeration options.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories and files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An array of strongly typed <see langword="FileSystemInfo" /> objects matching <paramref name="searchPattern" /> and <paramref name="enumerationOptions" />.</returns>
    public FileSystemInfo[] GetFileSystemInfos(
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return new List<FileSystemInfo>(this.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Both, enumerationOptions)).ToArray();
    }

    /// <summary>Returns the subdirectories of the current directory.</summary>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An array of <see cref="T:System.IO.DirectoryInfo" /> objects.</returns>
    public DirectoryInfo[] GetDirectories() => this.GetDirectories("*", EnumerationOptions.Compatible);

    /// <summary>Returns an array of directories in the current <see cref="T:System.IO.DirectoryInfo" /> matching the given search criteria.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see langword="DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An array of type <see langword="DirectoryInfo" /> matching <paramref name="searchPattern" />.</returns>
    public DirectoryInfo[] GetDirectories(string searchPattern) => this.GetDirectories(searchPattern, EnumerationOptions.Compatible);

    /// <summary>Returns an array of directories in the current <see cref="T:System.IO.DirectoryInfo" /> matching the given search criteria and using a value to determine whether to search subdirectories.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see langword="DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An array of type <see langword="DirectoryInfo" /> matching <paramref name="searchPattern" />.</returns>
    public DirectoryInfo[] GetDirectories(
      string searchPattern,
      SearchOption searchOption)
    {
      return this.GetDirectories(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns an array of directories in the current <see cref="T:System.IO.DirectoryInfo" /> matching the specified search pattern and enumeration options.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="searchPattern" /> contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see langword="DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An array of type <see langword="DirectoryInfo" /> matching <paramref name="searchPattern" /> and <paramref name="enumerationOptions" />.</returns>
    public DirectoryInfo[] GetDirectories(
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return new List<DirectoryInfo>((IEnumerable<DirectoryInfo>) this.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Directories, enumerationOptions)).ToArray();
    }

    /// <summary>Returns an enumerable collection of directory information in the current directory.</summary>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of directories in the current directory.</returns>
    public IEnumerable<DirectoryInfo> EnumerateDirectories() => this.EnumerateDirectories("*", EnumerationOptions.Compatible);

    /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern" />.</returns>
    public IEnumerable<DirectoryInfo> EnumerateDirectories(
      string searchPattern)
    {
      return this.EnumerateDirectories(searchPattern, EnumerationOptions.Compatible);
    }

    /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
    public IEnumerable<DirectoryInfo> EnumerateDirectories(
      string searchPattern,
      SearchOption searchOption)
    {
      return this.EnumerateDirectories(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns an enumerable collection of directory information that matches the specified search pattern and enumeration options.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern" /> and <paramref name="enumerationOptions" />.</returns>
    public IEnumerable<DirectoryInfo> EnumerateDirectories(
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return (IEnumerable<DirectoryInfo>) this.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Directories, enumerationOptions);
    }

    /// <summary>Returns an enumerable collection of file information in the current directory.</summary>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the files in the current directory.</returns>
    public IEnumerable<FileInfo> EnumerateFiles() => this.EnumerateFiles("*", EnumerationOptions.Compatible);

    /// <summary>Returns an enumerable collection of file information that matches a search pattern.</summary>
    /// <param name="searchPattern">The search string to match against the names of files.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of files that matches <paramref name="searchPattern" />.</returns>
    public IEnumerable<FileInfo> EnumerateFiles(string searchPattern) => this.EnumerateFiles(searchPattern, EnumerationOptions.Compatible);

    /// <summary>Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.</summary>
    /// <param name="searchPattern">The search string to match against the names of files.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of files that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
    public IEnumerable<FileInfo> EnumerateFiles(
      string searchPattern,
      SearchOption searchOption)
    {
      return this.EnumerateFiles(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns an enumerable collection of file information that matches the specified search pattern and enumeration options.</summary>
    /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of files that matches <paramref name="searchPattern" /> and <paramref name="enumerationOptions" />.</returns>
    public IEnumerable<FileInfo> EnumerateFiles(
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return (IEnumerable<FileInfo>) this.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Files, enumerationOptions);
    }

    /// <summary>Returns an enumerable collection of file system information in the current directory.</summary>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of file system information in the current directory.</returns>
    public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos() => this.EnumerateFileSystemInfos("*", EnumerationOptions.Compatible);

    /// <summary>Returns an enumerable collection of file system information that matches a specified search pattern.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern" />.</returns>
    public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(
      string searchPattern)
    {
      return this.EnumerateFileSystemInfos(searchPattern, EnumerationOptions.Compatible);
    }

    /// <summary>Returns an enumerable collection of file system information that matches a specified search pattern and search subdirectory option.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
    public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(
      string searchPattern,
      SearchOption searchOption)
    {
      return this.EnumerateFileSystemInfos(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns an enumerable collection of file system information that matches the specified search pattern and enumeration options.</summary>
    /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern" /> and <paramref name="enumerationOptions" />.</returns>
    public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return this.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Both, enumerationOptions);
    }


    #nullable disable
    private IEnumerable<FileSystemInfo> InternalEnumerateInfos(
      string path,
      string searchPattern,
      SearchTarget searchTarget,
      EnumerationOptions options)
    {
      if (searchPattern == null)
        throw new ArgumentNullException(nameof (searchPattern));
      this._isNormalized &= FileSystemEnumerableFactory.NormalizeInputs(ref path, ref searchPattern, options.MatchType);
      switch (searchTarget)
      {
        case SearchTarget.Files:
          return (IEnumerable<FileSystemInfo>) FileSystemEnumerableFactory.FileInfos(path, searchPattern, options, this._isNormalized);
        case SearchTarget.Directories:
          return (IEnumerable<FileSystemInfo>) FileSystemEnumerableFactory.DirectoryInfos(path, searchPattern, options, this._isNormalized);
        case SearchTarget.Both:
          return FileSystemEnumerableFactory.FileSystemInfos(path, searchPattern, options, this._isNormalized);
        default:
          throw new ArgumentException(SR.ArgumentOutOfRange_Enum, nameof (searchTarget));
      }
    }


    #nullable enable
    /// <summary>Gets the root portion of the directory.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An object that represents the root of the directory.</returns>
    public DirectoryInfo Root => new DirectoryInfo(Path.GetPathRoot(this.FullPath));

    /// <summary>Moves a <see cref="T:System.IO.DirectoryInfo" /> instance and its contents to a new path.</summary>
    /// <param name="destDirName">The name and path to which to move this directory. The destination cannot be another disk volume or a directory with the identical name. It can be an existing directory to which you want to add this directory as a subdirectory.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destDirName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="destDirName" /> is an empty string (''").</exception>
    /// <exception cref="T:System.IO.IOException">An attempt was made to move a directory to a different volume.
    /// 
    /// -or-
    /// 
    /// <paramref name="destDirName" /> already exists.
    /// 
    /// -or-
    /// 
    /// You are not authorized to access this path.
    /// 
    /// -or-
    /// 
    /// The directory being moved and the destination directory have the same name.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The destination directory cannot be found.</exception>
    public void MoveTo(string destDirName)
    {
      switch (destDirName)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyFileName, nameof (destDirName));
        case null:
          throw new ArgumentNullException(nameof (destDirName));
        default:
          string fullPath = Path.GetFullPath(destDirName);
          string str1 = PathInternal.EnsureTrailingSeparator(fullPath);
          string str2 = PathInternal.EnsureTrailingSeparator(this.FullPath);
          if (string.Equals(str2, str1, PathInternal.StringComparison))
            throw new IOException(SR.IO_SourceDestMustBeDifferent);
          if (!string.Equals(Path.GetPathRoot(str2), Path.GetPathRoot(str1), PathInternal.StringComparison))
            throw new IOException(SR.IO_SourceDestMustHaveSameRoot);
          if (!this.Exists && !FileSystem.FileExists(this.FullPath))
            throw new DirectoryNotFoundException(SR.Format(SR.IO_PathNotFound_Path, (object) this.FullPath));
          if (FileSystem.DirectoryExists(fullPath))
            throw new IOException(SR.Format(SR.IO_AlreadyExists_Name, (object) str1));
          FileSystem.MoveDirectory(this.FullPath, fullPath);
          this.Init(destDirName, str1, isNormalized: true);
          this.Invalidate();
          break;
      }
    }

    /// <summary>Deletes this <see cref="T:System.IO.DirectoryInfo" /> if it is empty.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">The directory contains a read-only file.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory described by this <see cref="T:System.IO.DirectoryInfo" /> object does not exist or could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">The directory is not empty.
    /// 
    /// -or-
    /// 
    /// The directory is the application's current working directory.
    /// 
    /// -or-
    /// 
    /// There is an open handle on the directory, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories. For more information, see How to: Enumerate Directories and Files.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public override void Delete() => this.Delete(false);

    /// <summary>Deletes this instance of a <see cref="T:System.IO.DirectoryInfo" />, specifying whether to delete subdirectories and files.</summary>
    /// <param name="recursive">
    /// <see langword="true" /> to delete this directory, its subdirectories, and all files; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The directory contains a read-only file.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory described by this <see cref="T:System.IO.DirectoryInfo" /> object does not exist or could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">The directory is read-only.
    /// 
    /// -or-
    /// 
    /// The directory contains one or more files or subdirectories and <paramref name="recursive" /> is <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// The directory is the application's current working directory.
    /// 
    /// -or-
    /// 
    /// There is an open handle on the directory or on one of its files, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and files. For more information, see How to: Enumerate Directories and Files.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public void Delete(bool recursive)
    {
      FileSystem.RemoveDirectory(this.FullPath, recursive);
      this.Invalidate();
    }
  }
}
