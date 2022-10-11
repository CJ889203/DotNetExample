// Decompiled with JetBrains decompiler
// Type: System.IO.Directory
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Enumeration;


#nullable enable
namespace System.IO
{
  /// <summary>Exposes static methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.</summary>
  public static class Directory
  {
    /// <summary>Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
    /// <param name="path">The path for which to retrieve the parent directory.</param>
    /// <exception cref="T:System.IO.IOException">The directory specified by <paramref name="path" /> is read-only.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For more information, see the <see cref="T:System.IO.PathTooLongException" /> topic.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">.NET Framework only: The caller does not have the required permissions.</exception>
    /// <returns>The parent directory, or <see langword="null" /> if <paramref name="path" /> is the root directory, including the root of a UNC server or share name.</returns>
    public static DirectoryInfo? GetParent(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_PathEmpty, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          string directoryName = Path.GetDirectoryName(Path.GetFullPath(path));
          return directoryName == null ? (DirectoryInfo) null : new DirectoryInfo(directoryName);
      }
    }

    /// <summary>Creates all directories and subdirectories in the specified path unless they already exist.</summary>
    /// <param name="path">The directory to create.</param>
    /// <exception cref="T:System.IO.IOException">The directory specified by <paramref name="path" /> is a file.
    /// 
    /// -or-
    /// 
    /// The network name is not known.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> is prefixed with, or contains, only a colon character (:).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> contains a colon character (:) that is not part of a drive label ("C:\").</exception>
    /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
    public static DirectoryInfo CreateDirectory(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_PathEmpty, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          string fullPath = Path.GetFullPath(path);
          FileSystem.CreateDirectory(fullPath);
          return new DirectoryInfo(path, fullPath, isNormalized: true);
      }
    }

    /// <summary>Determines whether the given path refers to an existing directory on disk.</summary>
    /// <param name="path">The path to test.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="path" /> refers to an existing directory; <see langword="false" /> if the directory does not exist or an error occurs when trying to determine if the specified directory exists.</returns>
    public static bool Exists([NotNullWhen(true)] string? path)
    {
      try
      {
        switch (path)
        {
          case "":
            return false;
          case null:
            return false;
          default:
            return FileSystem.DirectoryExists(Path.GetFullPath(path));
        }
      }
      catch (ArgumentException ex)
      {
      }
      catch (IOException ex)
      {
      }
      catch (UnauthorizedAccessException ex)
      {
      }
      return false;
    }

    /// <summary>Sets the creation date and time for the specified file or directory.</summary>
    /// <param name="path">The file or directory for which to set the creation date and time information.</param>
    /// <param name="creationTime">The date and time the file or directory was last written to. This value is expressed in local time.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    public static void SetCreationTime(string path, DateTime creationTime) => FileSystem.SetCreationTime(Path.GetFullPath(path), (DateTimeOffset) creationTime, true);

    /// <summary>Sets the creation date and time, in Coordinated Universal Time (UTC) format, for the specified file or directory.</summary>
    /// <param name="path">The file or directory for which to set the creation date and time information.</param>
    /// <param name="creationTimeUtc">The date and time the directory or file was created. This value is expressed in local time.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc) => FileSystem.SetCreationTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(creationTimeUtc), true);

    /// <summary>Gets the creation date and time of a directory.</summary>
    /// <param name="path">The path of the directory.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <returns>A structure that is set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
    public static DateTime GetCreationTime(string path) => File.GetCreationTime(path);

    /// <summary>Gets the creation date and time, in Coordinated Universal Time (UTC) format, of a directory.</summary>
    /// <param name="path">The path of the directory.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <returns>A structure that is set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
    public static DateTime GetCreationTimeUtc(string path) => File.GetCreationTimeUtc(path);

    /// <summary>Sets the date and time a directory was last written to.</summary>
    /// <param name="path">The path of the directory.</param>
    /// <param name="lastWriteTime">The date and time the directory was last written to. This value is expressed in local time.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="path" /> was not found (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> was not found (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="lastWriteTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    public static void SetLastWriteTime(string path, DateTime lastWriteTime) => FileSystem.SetLastWriteTime(Path.GetFullPath(path), (DateTimeOffset) lastWriteTime, true);

    /// <summary>Sets the date and time, in Coordinated Universal Time (UTC) format, that a directory was last written to.</summary>
    /// <param name="path">The path of the directory.</param>
    /// <param name="lastWriteTimeUtc">The date and time the directory was last written to. This value is expressed in UTC time.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="path" /> was not found (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> was not found (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="lastWriteTimeUtc" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc) => FileSystem.SetLastWriteTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastWriteTimeUtc), true);

    /// <summary>Returns the date and time the specified file or directory was last written to.</summary>
    /// <param name="path">The file or directory for which to obtain modification date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <returns>A structure that is set to the date and time the specified file or directory was last written to. This value is expressed in local time.</returns>
    public static DateTime GetLastWriteTime(string path) => File.GetLastWriteTime(path);

    /// <summary>Returns the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last written to.</summary>
    /// <param name="path">The file or directory for which to obtain modification date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <returns>A structure that is set to the date and time the specified file or directory was last written to. This value is expressed in UTC time.</returns>
    public static DateTime GetLastWriteTimeUtc(string path) => File.GetLastWriteTimeUtc(path);

    /// <summary>Sets the date and time the specified file or directory was last accessed.</summary>
    /// <param name="path">The file or directory for which to set the access date and time information.</param>
    /// <param name="lastAccessTime">An object that contains the value to set for the access date and time of <paramref name="path" />. This value is expressed in local time.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="lastAccessTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    public static void SetLastAccessTime(string path, DateTime lastAccessTime) => FileSystem.SetLastAccessTime(Path.GetFullPath(path), (DateTimeOffset) lastAccessTime, true);

    /// <summary>Sets the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last accessed.</summary>
    /// <param name="path">The file or directory for which to set the access date and time information.</param>
    /// <param name="lastAccessTimeUtc">An object that  contains the value to set for the access date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="lastAccessTimeUtc" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc) => FileSystem.SetLastAccessTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastAccessTimeUtc), true);

    /// <summary>Returns the date and time the specified file or directory was last accessed.</summary>
    /// <param name="path">The file or directory for which to obtain access date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">The <paramref name="path" /> parameter is in an invalid format.</exception>
    /// <returns>A structure that is set to the date and time the specified file or directory was last accessed. This value is expressed in local time.</returns>
    public static DateTime GetLastAccessTime(string path) => File.GetLastAccessTime(path);

    /// <summary>Returns the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last accessed.</summary>
    /// <param name="path">The file or directory for which to obtain access date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">The <paramref name="path" /> parameter is in an invalid format.</exception>
    /// <returns>A structure that is set to the date and time the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
    public static DateTime GetLastAccessTimeUtc(string path) => File.GetLastAccessTimeUtc(path);

    /// <summary>Returns the names of files (including their paths) in the specified directory.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="path" /> is a file name.
    /// 
    /// -or-
    /// 
    /// A network error has occurred.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is not found or is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of the full names (including paths) for the files in the specified directory, or an empty array if no files are found.</returns>
    public static string[] GetFiles(string path) => Directory.GetFiles(path, "*", EnumerationOptions.Compatible);

    /// <summary>Returns the names of files (including their paths) that match the specified search pattern in the specified directory.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="path" /> is a file name.
    /// 
    /// -or-
    /// 
    /// A network error has occurred.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using <see cref="M:System.IO.Path.GetInvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> doesn't contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is not found or is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern, or an empty array if no files are found.</returns>
    public static string[] GetFiles(string path, string searchPattern) => Directory.GetFiles(path, searchPattern, EnumerationOptions.Compatible);

    /// <summary>Returns the names of files (including their paths) that match the specified search pattern in the specified directory, using a value to determine whether to search subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchpattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is not found or is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="path" /> is a file name.
    /// 
    /// -or-
    /// 
    /// A network error has occurred.</exception>
    /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern and option, or an empty array if no files are found.</returns>
    public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption) => Directory.GetFiles(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));

    /// <summary>Returns the names of files (including their paths) that match the specified search pattern and enumeration options in the specified directory.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="path" /> is a file name.
    /// 
    /// -or-
    /// 
    /// A network error has occurred.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using <see cref="M:System.IO.Path.GetInvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> doesn't contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is not found or is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern and enumeration options, or an empty array if no files are found.</returns>
    public static string[] GetFiles(
      string path,
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return new List<string>(Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Files, enumerationOptions)).ToArray();
    }

    /// <summary>Returns the names of subdirectories (including their paths) in the specified directory.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of the full names (including paths) of subdirectories in the specified path, or an empty array if no directories are found.</returns>
    public static string[] GetDirectories(string path) => Directory.GetDirectories(path, "*", EnumerationOptions.Compatible);

    /// <summary>Returns the names of subdirectories (including their paths) that match the specified search pattern in the specified directory.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using <see cref="M:System.IO.Path.GetInvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> doesn't contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of the full names (including paths) of the subdirectories that match the search pattern in the specified directory, or an empty array if no directories are found.</returns>
    public static string[] GetDirectories(string path, string searchPattern) => Directory.GetDirectories(path, searchPattern, EnumerationOptions.Compatible);

    /// <summary>Returns the names of the subdirectories (including their paths) that match the specified search pattern in the specified directory, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of the full names (including paths) of the subdirectories that match the specified criteria, or an empty array if no directories are found.</returns>
    public static string[] GetDirectories(
      string path,
      string searchPattern,
      SearchOption searchOption)
    {
      return Directory.GetDirectories(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns the names of subdirectories (including their paths) that match the specified search pattern and enumeration options in the specified directory.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using <see cref="M:System.IO.Path.GetInvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> doesn't contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of the full names (including paths) of the subdirectories that match the search pattern and enumeration options in the specified directory, or an empty array if no directories are found.</returns>
    public static string[] GetDirectories(
      string path,
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return new List<string>(Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Directories, enumerationOptions)).ToArray();
    }

    /// <summary>Returns the names of all files and subdirectories in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of the names of files and subdirectories in the specified directory, or an empty array if no files or subdirectories are found.</returns>
    public static string[] GetFileSystemEntries(string path) => Directory.GetFileSystemEntries(path, "*", EnumerationOptions.Compatible);

    /// <summary>Returns an array of file names and directory names that match a search pattern in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of file and directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of file names and directory names that match the specified search criteria, or an empty array if no files or directories are found.</returns>
    public static string[] GetFileSystemEntries(string path, string searchPattern) => Directory.GetFileSystemEntries(path, searchPattern, EnumerationOptions.Compatible);

    /// <summary>Returns an array of all the file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files and directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An array of file the file names and directory names that match the specified search criteria, or an empty array if no files or directories are found.</returns>
    public static string[] GetFileSystemEntries(
      string path,
      string searchPattern,
      SearchOption searchOption)
    {
      return Directory.GetFileSystemEntries(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns an array of file names and directory names that match a search pattern and enumeration options in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <returns>An array of file names and directory names that match the specified search pattern and enumeration options, or an empty array if no files or directories are found.</returns>
    public static string[] GetFileSystemEntries(
      string path,
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return new List<string>(Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Both, enumerationOptions)).ToArray();
    }


    #nullable disable
    internal static IEnumerable<string> InternalEnumeratePaths(
      string path,
      string searchPattern,
      SearchTarget searchTarget,
      EnumerationOptions options)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (searchPattern == null)
        throw new ArgumentNullException(nameof (searchPattern));
      FileSystemEnumerableFactory.NormalizeInputs(ref path, ref searchPattern, options.MatchType);
      switch (searchTarget)
      {
        case SearchTarget.Files:
          return FileSystemEnumerableFactory.UserFiles(path, searchPattern, options);
        case SearchTarget.Directories:
          return FileSystemEnumerableFactory.UserDirectories(path, searchPattern, options);
        case SearchTarget.Both:
          return FileSystemEnumerableFactory.UserEntries(path, searchPattern, options);
        default:
          throw new ArgumentOutOfRangeException(nameof (searchTarget));
      }
    }


    #nullable enable
    /// <summary>Returns an enumerable collection of directory full names in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" />.</returns>
    public static IEnumerable<string> EnumerateDirectories(string path) => Directory.EnumerateDirectories(path, "*", EnumerationOptions.Compatible);

    /// <summary>Returns an enumerable collection of directory full names that match a search pattern in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" /> and that match the specified search pattern.</returns>
    public static IEnumerable<string> EnumerateDirectories(
      string path,
      string searchPattern)
    {
      return Directory.EnumerateDirectories(path, searchPattern, EnumerationOptions.Compatible);
    }

    /// <summary>Returns an enumerable collection of directory full names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" /> and that match the specified search pattern and search option.</returns>
    public static IEnumerable<string> EnumerateDirectories(
      string path,
      string searchPattern,
      SearchOption searchOption)
    {
      return Directory.EnumerateDirectories(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns an enumerable collection of the directory full names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" /> and that match the specified search pattern and enumeration options.</returns>
    public static IEnumerable<string> EnumerateDirectories(
      string path,
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Directories, enumerationOptions);
    }

    /// <summary>Returns an enumerable collection of full file names in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" />.</returns>
    public static IEnumerable<string> EnumerateFiles(string path) => Directory.EnumerateFiles(path, "*", EnumerationOptions.Compatible);

    /// <summary>Returns an enumerable collection of full file names that match a search pattern in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" /> and that match the specified search pattern.</returns>
    public static IEnumerable<string> EnumerateFiles(string path, string searchPattern) => Directory.EnumerateFiles(path, searchPattern, EnumerationOptions.Compatible);

    /// <summary>Returns an enumerable collection of full file names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" /> and that match the specified search pattern and search option.</returns>
    public static IEnumerable<string> EnumerateFiles(
      string path,
      string searchPattern,
      SearchOption searchOption)
    {
      return Directory.EnumerateFiles(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns an enumerable collection of full file names that match a search pattern and enumeration options in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" /> and that match the specified search pattern and enumeration options.</returns>
    public static IEnumerable<string> EnumerateFiles(
      string path,
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Files, enumerationOptions);
    }

    /// <summary>Returns an enumerable collection of file names and directory names in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path" />.</returns>
    public static IEnumerable<string> EnumerateFileSystemEntries(string path) => Directory.EnumerateFileSystemEntries(path, "*", EnumerationOptions.Compatible);

    /// <summary>Returns an enumerable collection of file names and directory names that match a search pattern in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of file-system entries in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path" /> and that match the specified search pattern.</returns>
    public static IEnumerable<string> EnumerateFileSystemEntries(
      string path,
      string searchPattern)
    {
      return Directory.EnumerateFileSystemEntries(path, searchPattern, EnumerationOptions.Compatible);
    }

    /// <summary>Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against file-system entries in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values  that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path" /> and that match the specified search pattern and option.</returns>
    public static IEnumerable<string> EnumerateFileSystemEntries(
      string path,
      string searchPattern,
      SearchOption searchOption)
    {
      return Directory.EnumerateFileSystemEntries(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
    }

    /// <summary>Returns an enumerable collection of file names and directory names that match a search pattern and enumeration options in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> does not contain a valid pattern.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="path" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="searchPattern" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="searchOption" /> is not a valid <see cref="T:System.IO.SearchOption" /> value.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid, such as referring to an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> is a file name.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path" />, that match the specified search pattern and the specified enumeration options.</returns>
    public static IEnumerable<string> EnumerateFileSystemEntries(
      string path,
      string searchPattern,
      EnumerationOptions enumerationOptions)
    {
      return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Both, enumerationOptions);
    }

    /// <summary>Returns the volume information, root information, or both for the specified path.</summary>
    /// <param name="path">The path of a file or directory.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <returns>A string that contains the volume information, root information, or both for the specified path.</returns>
    public static string GetDirectoryRoot(string path) => path != null ? Path.GetPathRoot(Path.GetFullPath(path)) : throw new ArgumentNullException(nameof (path));

    /// <summary>Gets the current working directory of the application.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">The operating system is Windows CE, which does not have current directory functionality.
    /// 
    /// This method is available in the .NET Compact Framework, but is not currently supported.</exception>
    /// <returns>A string that contains the absolute path of the current working directory, and does not end with a backslash (\).</returns>
    public static string GetCurrentDirectory() => Environment.CurrentDirectory;

    /// <summary>Sets the application's current working directory to the specified directory.</summary>
    /// <param name="path">The path to which the current working directory is set.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to access unmanaged code.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified directory was not found.</exception>
    public static void SetCurrentDirectory(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_PathEmpty, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          Environment.CurrentDirectory = Path.GetFullPath(path);
          break;
      }
    }

    /// <summary>Moves a file or a directory and its contents to a new location.</summary>
    /// <param name="sourceDirName">The path of the file or directory to move.</param>
    /// <param name="destDirName">The path to the new location for <paramref name="sourceDirName" />. If <paramref name="sourceDirName" /> is a file, then <paramref name="destDirName" /> must also be a file name.</param>
    /// <exception cref="T:System.IO.IOException">An attempt was made to move a directory to a different volume.
    /// 
    /// -or-
    /// 
    /// <paramref name="destDirName" /> already exists. See the Note in the Remarks section.
    /// 
    /// -or-
    /// 
    /// The <paramref name="sourceDirName" /> and <paramref name="destDirName" /> parameters refer to the same file or directory.
    /// 
    /// -or-
    /// 
    /// The directory or a file within it is being used by another process.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="sourceDirName" /> or <paramref name="destDirName" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters with the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceDirName" /> or <paramref name="destDirName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified by <paramref name="sourceDirName" /> is invalid (for example, it is on an unmapped drive).</exception>
    public static void Move(string sourceDirName, string destDirName)
    {
      switch (sourceDirName)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyFileName, nameof (sourceDirName));
        case null:
          throw new ArgumentNullException(nameof (sourceDirName));
        default:
          switch (destDirName)
          {
            case "":
              throw new ArgumentException(SR.Argument_EmptyFileName, nameof (destDirName));
            case null:
              throw new ArgumentNullException(nameof (destDirName));
            default:
              string fullPath1 = Path.GetFullPath(sourceDirName);
              string str1 = PathInternal.EnsureTrailingSeparator(fullPath1);
              string fullPath2 = Path.GetFullPath(destDirName);
              string str2 = PathInternal.EnsureTrailingSeparator(fullPath2);
              ReadOnlySpan<char> fileName1 = Path.GetFileName(fullPath1.AsSpan());
              ReadOnlySpan<char> fileName2 = Path.GetFileName(fullPath2.AsSpan());
              StringComparison stringComparison = PathInternal.StringComparison;
              bool flag = !fileName1.SequenceEqual<char>(fileName2) && MemoryExtensions.Equals(fileName1, fileName2, StringComparison.OrdinalIgnoreCase) && MemoryExtensions.Equals(fileName2, fileName1, stringComparison);
              if (!flag && string.Equals(str1, str2, stringComparison))
                throw new IOException(SR.IO_SourceDestMustBeDifferent);
              if (!MemoryExtensions.Equals(Path.GetPathRoot(str1.AsSpan()), Path.GetPathRoot(str2.AsSpan()), StringComparison.OrdinalIgnoreCase))
                throw new IOException(SR.IO_SourceDestMustHaveSameRoot);
              if (!FileSystem.DirectoryExists(fullPath1) && !FileSystem.FileExists(fullPath1))
                throw new DirectoryNotFoundException(SR.Format(SR.IO_PathNotFound_Path, (object) fullPath1));
              if (!flag && FileSystem.DirectoryExists(fullPath2))
                throw new IOException(SR.Format(SR.IO_AlreadyExists_Name, (object) fullPath2));
              if (!flag && Directory.Exists(fullPath2))
                throw new IOException(SR.Format(SR.IO_AlreadyExists_Name, (object) fullPath2));
              FileSystem.MoveDirectory(fullPath1, fullPath2);
              return;
          }
      }
    }

    /// <summary>Deletes an empty directory from a specified path.</summary>
    /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
    /// <exception cref="T:System.IO.IOException">A file with the same name and location specified by <paramref name="path" /> exists.
    /// 
    /// -or-
    /// 
    /// The directory is the application's current working directory.
    /// 
    /// -or-
    /// 
    /// The directory specified by <paramref name="path" /> is not empty.
    /// 
    /// -or-
    /// 
    /// The directory is read-only or contains a read-only file.
    /// 
    /// -or-
    /// 
    /// The directory is being used by another process.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    ///        <paramref name="path" /> does not exist or could not be found.
    /// 
    /// -or-
    /// 
    /// The specified path is invalid (for example, it is on an unmapped drive).</exception>
    public static void Delete(string path) => FileSystem.RemoveDirectory(Path.GetFullPath(path), false);

    /// <summary>Deletes the specified directory and, if indicated, any subdirectories and files in the directory.</summary>
    /// <param name="path">The name of the directory to remove.</param>
    /// <param name="recursive">
    /// <see langword="true" /> to remove directories, subdirectories, and files in <paramref name="path" />; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.IO.IOException">A file with the same name and location specified by <paramref name="path" /> exists.
    /// 
    /// -or-
    /// 
    /// The directory specified by <paramref name="path" /> is read-only, or <paramref name="recursive" /> is <see langword="false" /> and <paramref name="path" /> is not an empty directory.
    /// 
    /// -or-
    /// 
    /// The directory is the application's current working directory.
    /// 
    /// -or-
    /// 
    /// The directory contains a read-only file.
    /// 
    /// -or-
    /// 
    /// The directory is being used by another process.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    ///        <paramref name="path" /> does not exist or could not be found.
    /// 
    /// -or-
    /// 
    /// The specified path is invalid (for example, it is on an unmapped drive).</exception>
    public static void Delete(string path, bool recursive) => FileSystem.RemoveDirectory(Path.GetFullPath(path), recursive);

    /// <summary>Retrieves the names of the logical drives on this computer in the form "&lt;drive letter&gt;:\".</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>The logical drives on this computer.</returns>
    public static string[] GetLogicalDrives() => FileSystem.GetLogicalDrives();

    /// <summary>Creates a directory symbolic link identified by <paramref name="path" /> that points to <paramref name="pathToTarget" />.</summary>
    /// <param name="path">The path where the symbolic link should be created.</param>
    /// <param name="pathToTarget">The target directory of the symbolic link.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="pathToTarget" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="path" /> or <paramref name="pathToTarget" /> is empty.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> or <paramref name="pathToTarget" /> contains invalid path characters.</exception>
    /// <exception cref="T:System.IO.IOException">A file or directory already exists in the location of <paramref name="path" />.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred.</exception>
    /// <returns>A <see cref="T:System.IO.DirectoryInfo" /> instance that wraps the newly created directory symbolic link.</returns>
    public static FileSystemInfo CreateSymbolicLink(string path, string pathToTarget)
    {
      string fullPath = Path.GetFullPath(path);
      FileSystem.VerifyValidPath(pathToTarget, nameof (pathToTarget));
      FileSystem.CreateSymbolicLink(path, pathToTarget, true);
      return (FileSystemInfo) new DirectoryInfo(path, fullPath, isNormalized: true);
    }

    /// <summary>Gets the target of the specified directory link.</summary>
    /// <param name="linkPath">The path of the directory link.</param>
    /// <param name="returnFinalTarget">
    /// <see langword="true" /> to follow links to the final target; <see langword="false" /> to return the immediate next link.</param>
    /// <exception cref="T:System.IO.IOException">The directory on <paramref name="linkPath" /> does not exist.
    /// 
    /// -or-
    /// 
    /// There are too many levels of symbolic links.</exception>
    /// <returns>A <see cref="T:System.IO.DirectoryInfo" /> instance if <paramref name="linkPath" /> exists, independently if the target exists or not. <see langword="null" /> if <paramref name="linkPath" /> is not a link.</returns>
    public static FileSystemInfo? ResolveLinkTarget(
      string linkPath,
      bool returnFinalTarget)
    {
      FileSystem.VerifyValidPath(linkPath, nameof (linkPath));
      return FileSystem.ResolveLinkTarget(linkPath, returnFinalTarget, true);
    }
  }
}
