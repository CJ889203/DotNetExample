// Decompiled with JetBrains decompiler
// Type: System.IO.File
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Microsoft.Win32.SafeHandles;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Strategies;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Provides static methods for the creation, copying, deletion, moving, and opening of a single file, and aids in the creation of <see cref="T:System.IO.FileStream" /> objects.</summary>
  public static class File
  {

    #nullable disable
    private static Encoding s_UTF8NoBOM;


    #nullable enable
    /// <summary>Opens an existing UTF-8 encoded text file for reading.</summary>
    /// <param name="path">The file to be opened for reading.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.IO.StreamReader" /> on the specified path.</returns>
    public static StreamReader OpenText(string path) => path != null ? new StreamReader(path) : throw new ArgumentNullException(nameof (path));

    /// <summary>Creates or opens a file for writing UTF-8 encoded text. If the file already exists, its contents are overwritten.</summary>
    /// <param name="path">The file to be opened for writing.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.IO.StreamWriter" /> that writes to the specified file using UTF-8 encoding.</returns>
    public static StreamWriter CreateText(string path) => path != null ? new StreamWriter(path, false) : throw new ArgumentNullException(nameof (path));

    /// <summary>Creates a <see cref="T:System.IO.StreamWriter" /> that appends UTF-8 encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
    /// <param name="path">The path to the file to append to.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A stream writer that appends UTF-8 encoded text to the specified file or to a new file.</returns>
    public static StreamWriter AppendText(string path) => path != null ? new StreamWriter(path, true) : throw new ArgumentNullException(nameof (path));

    /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
    /// <param name="sourceFileName">The file to copy.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> specifies a directory.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="destFileName" /> exists.
    /// 
    /// -or-
    /// 
    /// An I/O error has occurred.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is in an invalid format.</exception>
    public static void Copy(string sourceFileName, string destFileName) => File.Copy(sourceFileName, destFileName, false);

    /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
    /// <param name="sourceFileName">The file to copy.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
    /// <param name="overwrite">
    /// <see langword="true" /> if the destination file can be overwritten; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="destFileName" /> is read-only.
    /// 
    ///  -or-
    /// 
    /// <paramref name="overwrite" /> is <see langword="true" />, <paramref name="destFileName" /> exists and is hidden, but <paramref name="sourceFileName" /> is not hidden.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> specifies a directory.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="destFileName" /> exists and <paramref name="overwrite" /> is <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// An I/O error has occurred.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is in an invalid format.</exception>
    public static void Copy(string sourceFileName, string destFileName, bool overwrite)
    {
      if (sourceFileName == null)
        throw new ArgumentNullException(nameof (sourceFileName), SR.ArgumentNull_FileName);
      if (destFileName == null)
        throw new ArgumentNullException(nameof (destFileName), SR.ArgumentNull_FileName);
      if (sourceFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyFileName, nameof (sourceFileName));
      if (destFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyFileName, nameof (destFileName));
      FileSystem.CopyFile(Path.GetFullPath(sourceFileName), Path.GetFullPath(destFileName), overwrite);
    }

    /// <summary>Creates or overwrites a file in the specified path.</summary>
    /// <param name="path">The path and name of the file to create.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while creating the file.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.IO.FileStream" /> that provides read/write access to the file specified in <paramref name="path" />.</returns>
    public static FileStream Create(string path) => File.Create(path, 4096);

    /// <summary>Creates or overwrites a file in the specified path, specifying a buffer size.</summary>
    /// <param name="path">The path and name of the file to create.</param>
    /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while creating the file.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.IO.FileStream" /> with the specified buffer size that provides read/write access to the file specified in <paramref name="path" />.</returns>
    public static FileStream Create(string path, int bufferSize) => new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize);

    /// <summary>Creates or overwrites a file in the specified path, specifying a buffer size and options that describe how to create or overwrite the file.</summary>
    /// <param name="path">The path and name of the file to create.</param>
    /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
    /// <param name="options">One of the <see cref="T:System.IO.FileOptions" /> values that describes how to create or overwrite the file.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while creating the file.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A new file with the specified buffer size.</returns>
    public static FileStream Create(string path, int bufferSize, FileOptions options) => new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);

    /// <summary>Deletes the specified file.</summary>
    /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">The specified file is in use.
    /// 
    /// -or-
    /// 
    /// There is an open handle on the file, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and files. For more information, see How to: Enumerate Directories and Files.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// The file is an executable file that is in use.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> is a directory.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a read-only file.</exception>
    public static void Delete(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      FileSystem.DeleteFile(Path.GetFullPath(path));
    }

    /// <summary>Determines whether the specified file exists.</summary>
    /// <param name="path">The file to check.</param>
    /// <returns>
    /// <see langword="true" /> if the caller has the required permissions and <paramref name="path" /> contains the name of an existing file; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="path" /> is <see langword="null" />, an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <see langword="false" /> regardless of the existence of <paramref name="path" />.</returns>
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
            path = Path.GetFullPath(path);
            return (path.Length <= 0 || !PathInternal.IsDirectorySeparator(path[path.Length - 1])) && FileSystem.FileExists(path);
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

    /// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path with read/write access with no sharing.</summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> specified an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.IO.FileStream" /> opened in the specified mode and path, with read/write access and not shared.</returns>
    public static FileStream Open(string path, FileMode mode) => File.Open(path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);

    /// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path, with the specified mode and access with no sharing.</summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">A <see cref="T:System.IO.FileAccess" /> value that specifies the operations that can be performed on the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> specified <see langword="Read" /> and <paramref name="mode" /> specified <see langword="Create" />, <see langword="CreateNew" />, <see langword="Truncate" />, or <see langword="Append" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only and <paramref name="access" /> is not <see langword="Read" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> or <paramref name="access" /> specified an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>An unshared <see cref="T:System.IO.FileStream" /> that provides access to the specified file, with the specified mode and access.</returns>
    public static FileStream Open(string path, FileMode mode, FileAccess access) => File.Open(path, mode, access, FileShare.None);

    /// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">A <see cref="T:System.IO.FileAccess" /> value that specifies the operations that can be performed on the file.</param>
    /// <param name="share">A <see cref="T:System.IO.FileShare" /> value specifying the type of access other threads have to the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> specified <see langword="Read" /> and <paramref name="mode" /> specified <see langword="Create" />, <see langword="CreateNew" />, <see langword="Truncate" />, or <see langword="Append" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only and <paramref name="access" /> is not <see langword="Read" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" />, <paramref name="access" />, or <paramref name="share" /> specified an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.IO.FileStream" /> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
    public static FileStream Open(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share)
    {
      return new FileStream(path, mode, access, share);
    }

    internal static DateTimeOffset GetUtcDateTimeOffset(DateTime dateTime) => dateTime.Kind == DateTimeKind.Unspecified ? (DateTimeOffset) DateTime.SpecifyKind(dateTime, DateTimeKind.Utc) : (DateTimeOffset) dateTime.ToUniversalTime();

    /// <summary>Sets the date and time the file was created.</summary>
    /// <param name="path">The file for which to set the creation date and time information.</param>
    /// <param name="creationTime">A <see cref="T:System.DateTime" /> containing the value to set for the creation date and time of <paramref name="path" />. This value is expressed in local time.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while performing the operation.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationTime" /> specifies a value outside the range of dates, times, or both permitted for this operation.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    public static void SetCreationTime(string path, DateTime creationTime) => FileSystem.SetCreationTime(Path.GetFullPath(path), (DateTimeOffset) creationTime, false);

    /// <summary>Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
    /// <param name="path">The file for which to set the creation date and time information.</param>
    /// <param name="creationTimeUtc">A <see cref="T:System.DateTime" /> containing the value to set for the creation date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while performing the operation.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationTime" /> specifies a value outside the range of dates, times, or both permitted for this operation.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc) => FileSystem.SetCreationTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(creationTimeUtc), false);

    /// <summary>Returns the creation date and time of the specified file or directory.</summary>
    /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.DateTime" /> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.</returns>
    public static DateTime GetCreationTime(string path) => FileSystem.GetCreationTime(Path.GetFullPath(path)).LocalDateTime;

    /// <summary>Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory.</summary>
    /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.DateTime" /> structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.</returns>
    public static DateTime GetCreationTimeUtc(string path) => FileSystem.GetCreationTime(Path.GetFullPath(path)).UtcDateTime;

    /// <summary>Sets the date and time the specified file was last accessed.</summary>
    /// <param name="path">The file for which to set the access date and time information.</param>
    /// <param name="lastAccessTime">A <see cref="T:System.DateTime" /> containing the value to set for the last access date and time of <paramref name="path" />. This value is expressed in local time.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="lastAccessTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    public static void SetLastAccessTime(string path, DateTime lastAccessTime) => FileSystem.SetLastAccessTime(Path.GetFullPath(path), (DateTimeOffset) lastAccessTime, false);

    /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
    /// <param name="path">The file for which to set the access date and time information.</param>
    /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime" /> containing the value to set for the last access date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="lastAccessTimeUtc" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc) => FileSystem.SetLastAccessTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastAccessTimeUtc), false);

    /// <summary>Returns the date and time the specified file or directory was last accessed.</summary>
    /// <param name="path">The file or directory for which to obtain access date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
    public static DateTime GetLastAccessTime(string path) => FileSystem.GetLastAccessTime(Path.GetFullPath(path)).LocalDateTime;

    /// <summary>Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed.</summary>
    /// <param name="path">The file or directory for which to obtain access date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
    public static DateTime GetLastAccessTimeUtc(string path) => FileSystem.GetLastAccessTime(Path.GetFullPath(path)).UtcDateTime;

    /// <summary>Sets the date and time that the specified file was last written to.</summary>
    /// <param name="path">The file for which to set the date and time information.</param>
    /// <param name="lastWriteTime">A <see cref="T:System.DateTime" /> containing the value to set for the last write date and time of <paramref name="path" />. This value is expressed in local time.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="lastWriteTime" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    public static void SetLastWriteTime(string path, DateTime lastWriteTime) => FileSystem.SetLastWriteTime(Path.GetFullPath(path), (DateTimeOffset) lastWriteTime, false);

    /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
    /// <param name="path">The file for which to set the date and time information.</param>
    /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime" /> containing the value to set for the last write date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The specified path was not found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="lastWriteTimeUtc" /> specifies a value outside the range of dates or times permitted for this operation.</exception>
    public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc) => FileSystem.SetLastWriteTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastWriteTimeUtc), false);

    /// <summary>Returns the date and time the specified file or directory was last written to.</summary>
    /// <param name="path">The file or directory for which to obtain write date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
    public static DateTime GetLastWriteTime(string path) => FileSystem.GetLastWriteTime(Path.GetFullPath(path)).LocalDateTime;

    /// <summary>Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.</summary>
    /// <param name="path">The file or directory for which to obtain write date and time information.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>A <see cref="T:System.DateTime" /> structure set to the date and time that the specified file or directory was last written to. This value is expressed in UTC time.</returns>
    public static DateTime GetLastWriteTimeUtc(string path) => FileSystem.GetLastWriteTime(Path.GetFullPath(path)).UtcDateTime;

    /// <summary>Gets the <see cref="T:System.IO.FileAttributes" /> of the file on the path.</summary>
    /// <param name="path">The path to the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is empty, contains only white spaces, or contains invalid characters.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="path" /> represents a file and is invalid, such as being on an unmapped drive, or the file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> represents a directory and is invalid, such as being on an unmapped drive, or the directory cannot be found.</exception>
    /// <exception cref="T:System.IO.IOException">This file is being used by another process.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>The <see cref="T:System.IO.FileAttributes" /> of the file on the path.</returns>
    public static FileAttributes GetAttributes(string path) => FileSystem.GetAttributes(Path.GetFullPath(path));

    /// <summary>Sets the specified <see cref="T:System.IO.FileAttributes" /> of the file on the specified path.</summary>
    /// <param name="path">The path to the file.</param>
    /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is empty, contains only white spaces, contains invalid characters, or the file attribute is invalid.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    public static void SetAttributes(string path, FileAttributes fileAttributes) => FileSystem.SetAttributes(Path.GetFullPath(path), fileAttributes);

    /// <summary>Opens an existing file for reading.</summary>
    /// <param name="path">The file to be opened for reading.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <returns>A read-only <see cref="T:System.IO.FileStream" /> on the specified path.</returns>
    public static FileStream OpenRead(string path) => new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

    /// <summary>Opens an existing file or creates a new file for writing.</summary>
    /// <param name="path">The file to be opened for writing.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a read-only file or directory.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <returns>An unshared <see cref="T:System.IO.FileStream" /> object on the specified path with <see cref="F:System.IO.FileAccess.Write" /> access.</returns>
    public static FileStream OpenWrite(string path) => new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

    /// <summary>Opens a text file, reads all the text in the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A string containing all the text in the file.</returns>
    public static string ReadAllText(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          return File.InternalReadAllText(path, Encoding.UTF8);
      }
    }

    /// <summary>Opens a file, reads all text in the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A string containing all text in the file.</returns>
    public static string ReadAllText(string path, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      return path.Length != 0 ? File.InternalReadAllText(path, encoding) : throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
    }


    #nullable disable
    private static string InternalReadAllText(string path, Encoding encoding)
    {
      using (StreamReader streamReader = new StreamReader(path, encoding, true))
        return streamReader.ReadToEnd();
    }


    #nullable enable
    /// <summary>Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void WriteAllText(string path, string? contents)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          using (StreamWriter streamWriter = new StreamWriter(path))
          {
            streamWriter.Write(contents);
            break;
          }
      }
    }

    /// <summary>Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <param name="encoding">The encoding to apply to the string.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void WriteAllText(string path, string? contents, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
        streamWriter.Write(contents);
    }

    /// <summary>Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A byte array containing the contents of the file.</returns>
    public static byte[] ReadAllBytes(string path)
    {
      using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1, FileOptions.SequentialScan))
      {
        long num1 = 0;
        if (fs.CanSeek && (num1 = fs.Length) > (long) int.MaxValue)
          throw new IOException(SR.IO_FileTooLong2GB);
        if (num1 == 0L)
          return File.ReadAllBytesUnknownLength(fs);
        int offset = 0;
        int count = (int) num1;
        byte[] buffer = new byte[count];
        int num2;
        for (; count > 0; count -= num2)
        {
          num2 = fs.Read(buffer, offset, count);
          if (num2 == 0)
            ThrowHelper.ThrowEndOfFileException();
          offset += num2;
        }
        return buffer;
      }
    }

    /// <summary>Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" /> or the byte array is empty.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void WriteAllBytes(string path, byte[] bytes)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path), SR.ArgumentNull_Path);
        default:
          if (bytes == null)
            throw new ArgumentNullException(nameof (bytes));
          using (SafeFileHandle handle = File.OpenHandle(path, FileMode.Create, FileAccess.Write))
          {
            RandomAccess.WriteAtOffset(handle, (ReadOnlySpan<byte>) bytes, 0L);
            break;
          }
      }
    }

    /// <summary>Opens a text file, reads all lines of the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A string array containing all lines of the file.</returns>
    public static string[] ReadAllLines(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          return File.InternalReadAllLines(path, Encoding.UTF8);
      }
    }

    /// <summary>Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A string array containing all lines of the file.</returns>
    public static string[] ReadAllLines(string path, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      return path.Length != 0 ? File.InternalReadAllLines(path, encoding) : throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
    }


    #nullable disable
    private static string[] InternalReadAllLines(string path, Encoding encoding)
    {
      List<string> stringList = new List<string>();
      using (StreamReader streamReader = new StreamReader(path, encoding))
      {
        string str;
        while ((str = streamReader.ReadLine()) != null)
          stringList.Add(str);
      }
      return stringList.ToArray();
    }


    #nullable enable
    /// <summary>Reads the lines of a file.</summary>
    /// <param name="path">The file to read.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specifies a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> is a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
    public static IEnumerable<string> ReadLines(string path)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          return (IEnumerable<string>) ReadLinesIterator.CreateIterator(path, Encoding.UTF8);
      }
    }

    /// <summary>Read the lines of a file that has a specified encoding.</summary>
    /// <param name="path">The file to read.</param>
    /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specifies a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> is a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
    public static IEnumerable<string> ReadLines(string path, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      return path.Length != 0 ? (IEnumerable<string>) ReadLinesIterator.CreateIterator(path, encoding) : throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
    }

    /// <summary>Creates a new file, write the specified string array to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string array to write to the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" /> or <paramref name="contents" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void WriteAllLines(string path, string[] contents) => File.WriteAllLines(path, (IEnumerable<string>) contents);

    /// <summary>Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" /> or <paramref name="contents" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> is a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    public static void WriteAllLines(string path, IEnumerable<string> contents)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (contents == null)
        throw new ArgumentNullException(nameof (contents));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      File.InternalWriteAllLines((TextWriter) new StreamWriter(path), contents);
    }

    /// <summary>Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string array to write to the file.</param>
    /// <param name="encoding">An <see cref="T:System.Text.Encoding" /> object that represents the character encoding applied to the string array.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" /> or <paramref name="contents" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void WriteAllLines(string path, string[] contents, Encoding encoding) => File.WriteAllLines(path, (IEnumerable<string>) contents, encoding);

    /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" />, <paramref name="contents" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a file that is hidden.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> is a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (contents == null)
        throw new ArgumentNullException(nameof (contents));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      File.InternalWriteAllLines((TextWriter) new StreamWriter(path, false, encoding), contents);
    }


    #nullable disable
    private static void InternalWriteAllLines(TextWriter writer, IEnumerable<string> contents)
    {
      using (writer)
      {
        foreach (string content in contents)
          writer.WriteLine(content);
      }
    }


    #nullable enable
    /// <summary>Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.</summary>
    /// <param name="path">The file to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void AppendAllText(string path, string? contents)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          using (StreamWriter streamWriter = new StreamWriter(path, true))
          {
            streamWriter.Write(contents);
            break;
          }
      }
    }

    /// <summary>Appends the specified string to the file using the specified encoding, creating the file if it does not already exist.</summary>
    /// <param name="path">The file to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void AppendAllText(string path, string? contents, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      using (StreamWriter streamWriter = new StreamWriter(path, true, encoding))
        streamWriter.Write(contents);
    }

    /// <summary>Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is a zero-length string, contains only white space, or contains one more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" /> or <paramref name="contents" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have permission to write to the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specifies a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> is a directory.</exception>
    public static void AppendAllLines(string path, IEnumerable<string> contents)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (contents == null)
        throw new ArgumentNullException(nameof (contents));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      File.InternalWriteAllLines((TextWriter) new StreamWriter(path, true), contents);
    }

    /// <summary>Appends lines to a file by using a specified encoding, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is a zero-length string, contains only white space, or contains one more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">Either <paramref name="path" />, <paramref name="contents" />, or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid (for example, the directory doesn't exist or it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path" /> was not found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="path" /> specifies a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> is a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (contents == null)
        throw new ArgumentNullException(nameof (contents));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      File.InternalWriteAllLines((TextWriter) new StreamWriter(path, true, encoding), contents);
    }

    /// <summary>Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of the replaced file.</summary>
    /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName" />.</param>
    /// <param name="destinationFileName">The name of the file being replaced.</param>
    /// <param name="destinationBackupFileName">The name of the backup file.</param>
    /// <exception cref="T:System.ArgumentException">The path described by the <paramref name="destinationFileName" /> parameter was not of a legal form.
    /// 
    /// -or-
    /// 
    /// The path described by the <paramref name="destinationBackupFileName" /> parameter was not of a legal form.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationFileName" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.
    /// 
    /// -or-
    /// 
    /// The file described by the <paramref name="destinationBackupFileName" /> parameter could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="sourceFileName" /> and <paramref name="destinationFileName" /> parameters specify the same file.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> parameter specifies a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// Source or destination parameters specify a directory instead of a file.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    public static void Replace(
      string sourceFileName,
      string destinationFileName,
      string? destinationBackupFileName)
    {
      File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, false);
    }

    /// <summary>Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of the replaced file and optionally ignores merge errors.</summary>
    /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName" />.</param>
    /// <param name="destinationFileName">The name of the file being replaced.</param>
    /// <param name="destinationBackupFileName">The name of the backup file.</param>
    /// <param name="ignoreMetadataErrors">
    /// <see langword="true" /> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the replacement file; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">The path described by the <paramref name="destinationFileName" /> parameter was not of a legal form.
    /// 
    /// -or-
    /// 
    /// The path described by the <paramref name="destinationBackupFileName" /> parameter was not of a legal form.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationFileName" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo" /> object could not be found.
    /// 
    /// -or-
    /// 
    /// The file described by the <paramref name="destinationBackupFileName" /> parameter could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="sourceFileName" /> and <paramref name="destinationFileName" /> parameters specify the same file.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="sourceFileName" /> or <paramref name="destinationFileName" /> parameter specifies a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// Source or destination parameters specify a directory instead of a file.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    public static void Replace(
      string sourceFileName,
      string destinationFileName,
      string? destinationBackupFileName,
      bool ignoreMetadataErrors)
    {
      if (sourceFileName == null)
        throw new ArgumentNullException(nameof (sourceFileName));
      if (destinationFileName == null)
        throw new ArgumentNullException(nameof (destinationFileName));
      FileSystem.ReplaceFile(Path.GetFullPath(sourceFileName), Path.GetFullPath(destinationFileName), destinationBackupFileName != null ? Path.GetFullPath(destinationBackupFileName) : (string) null, ignoreMetadataErrors);
    }

    /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
    /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path.</param>
    /// <param name="destFileName">The new path and name for the file.</param>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="destFileName" /> already exists.
    /// 
    ///   -or-
    /// 
    ///   An I/O error has occurred, e.g. while copying the file across disk volumes.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is in an invalid format.</exception>
    public static void Move(string sourceFileName, string destFileName) => File.Move(sourceFileName, destFileName, false);

    /// <summary>Moves a specified file to a new location, providing the options to specify a new file name and to overwrite the destination file if it already exists.</summary>
    /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path.</param>
    /// <param name="destFileName">The new path and name for the file.</param>
    /// <param name="overwrite">
    /// <see langword="true" /> to overwrite the destination file if it already exists; <see langword="false" /> otherwise.</param>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="destFileName" /> already exists and <paramref name="overwrite" /> is <see langword="false" />.
    /// 
    ///   -or-
    /// 
    ///   An I/O error has occurred, e.g. while copying the file across disk volumes.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceFileName" /> was not found.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is a zero-length string, contains only white space, or contains invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="sourceFileName" /> or <paramref name="destFileName" /> is in an invalid format.</exception>
    public static void Move(string sourceFileName, string destFileName, bool overwrite)
    {
      if (sourceFileName == null)
        throw new ArgumentNullException(nameof (sourceFileName), SR.ArgumentNull_FileName);
      if (destFileName == null)
        throw new ArgumentNullException(nameof (destFileName), SR.ArgumentNull_FileName);
      if (sourceFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyFileName, nameof (sourceFileName));
      if (destFileName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyFileName, nameof (destFileName));
      string fullPath1 = Path.GetFullPath(sourceFileName);
      string fullPath2 = Path.GetFullPath(destFileName);
      if (!FileSystem.FileExists(fullPath1))
        throw new FileNotFoundException(SR.Format(SR.IO_FileNotFound_FileName, (object) fullPath1), fullPath1);
      FileSystem.MoveFile(fullPath1, fullPath2, overwrite);
    }

    /// <summary>Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
    /// <param name="path">A path that describes a file to encrypt.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: The <paramref name="path" /> parameter is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the <paramref name="path" /> parameter could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="path" /> parameter specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// The <paramref name="path" /> parameter specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    [SupportedOSPlatform("windows")]
    public static void Encrypt(string path) => FileSystem.Encrypt(path ?? throw new ArgumentNullException(nameof (path)));

    /// <summary>Decrypts a file that was encrypted by the current account using the <see cref="M:System.IO.File.Encrypt(System.String)" /> method.</summary>
    /// <param name="path">A path that describes a file to decrypt.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: The <paramref name="path" /> parameter is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the <see cref="M:System.IO.Path.GetInvalidPathChars" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file described by the <paramref name="path" /> parameter could not be found.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. For example, the encrypted file is already open.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception>
    /// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="path" /> parameter specified a file that is read-only.
    /// 
    /// -or-
    /// 
    /// This operation is not supported on the current platform.
    /// 
    /// -or-
    /// 
    /// The <paramref name="path" /> parameter specified a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission.</exception>
    [SupportedOSPlatform("windows")]
    public static void Decrypt(string path) => FileSystem.Decrypt(path ?? throw new ArgumentNullException(nameof (path)));

    private static Encoding UTF8NoBOM => File.s_UTF8NoBOM ?? (File.s_UTF8NoBOM = (Encoding) new UTF8Encoding(false, true));


    #nullable disable
    private static StreamReader AsyncStreamReader(string path, Encoding encoding) => new StreamReader((Stream) new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan), encoding, true);

    private static StreamWriter AsyncStreamWriter(
      string path,
      Encoding encoding,
      bool append)
    {
      return new StreamWriter((Stream) new FileStream(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan), encoding);
    }


    #nullable enable
    /// <summary>Asynchronously opens a text file, reads all the text in the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string containing all text in the file.</returns>
    public static Task<string> ReadAllTextAsync(
      string path,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return File.ReadAllTextAsync(path, Encoding.UTF8, cancellationToken);
    }

    /// <summary>Asynchronously opens a text file, reads all text in the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string containing all text in the file.</returns>
    public static Task<string> ReadAllTextAsync(
      string path,
      Encoding encoding,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      return !cancellationToken.IsCancellationRequested ? File.InternalReadAllTextAsync(path, encoding, cancellationToken) : Task.FromCanceled<string>(cancellationToken);
    }


    #nullable disable
    private static async Task<string> InternalReadAllTextAsync(
      string path,
      Encoding encoding,
      CancellationToken cancellationToken)
    {
      char[] buffer = (char[]) null;
      StreamReader sr = File.AsyncStreamReader(path, encoding);
      string str;
      try
      {
        cancellationToken.ThrowIfCancellationRequested();
        buffer = ArrayPool<char>.Shared.Rent(sr.CurrentEncoding.GetMaxCharCount(4096));
        StringBuilder sb = new StringBuilder();
        while (true)
        {
          int charCount = await sr.ReadAsync(new Memory<char>(buffer), cancellationToken).ConfigureAwait(false);
          if (charCount != 0)
            sb.Append(buffer, 0, charCount);
          else
            break;
        }
        str = sb.ToString();
      }
      finally
      {
        sr.Dispose();
        if (buffer != null)
          ArrayPool<char>.Shared.Return(buffer);
      }
      buffer = (char[]) null;
      sr = (StreamReader) null;
      return str;
    }


    #nullable enable
    /// <summary>Asynchronously creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAllTextAsync(
      string path,
      string? contents,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return File.WriteAllTextAsync(path, contents, File.UTF8NoBOM, cancellationToken);
    }

    /// <summary>Asynchronously creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <param name="encoding">The encoding to apply to the string.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAllTextAsync(
      string path,
      string? contents,
      Encoding encoding,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (!string.IsNullOrEmpty(contents))
        return File.InternalWriteAllTextAsync(File.AsyncStreamWriter(path, encoding, false), contents, cancellationToken);
      new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read).Dispose();
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the byte array containing the contents of the file.</returns>
    public static Task<byte[]> ReadAllBytesAsync(
      string path,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled<byte[]>(cancellationToken);
      FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1, FileOptions.Asynchronous | FileOptions.SequentialScan);
      bool flag = false;
      try
      {
        long count = 0;
        if (fs.CanSeek && (count = fs.Length) > (long) int.MaxValue)
        {
          IOException source = new IOException(SR.IO_FileTooLong2GB);
          ExceptionDispatchInfo.SetCurrentStackTrace((Exception) source);
          return Task.FromException<byte[]>((Exception) source);
        }
        flag = true;
        return count > 0L ? File.InternalReadAllBytesAsync(fs, (int) count, cancellationToken) : File.InternalReadAllBytesUnknownLengthAsync(fs, cancellationToken);
      }
      finally
      {
        if (!flag)
          fs.Dispose();
      }
    }


    #nullable disable
    private static async Task<byte[]> InternalReadAllBytesAsync(
      FileStream fs,
      int count,
      CancellationToken cancellationToken)
    {
      byte[] numArray;
      using (fs)
      {
        int index = 0;
        byte[] bytes = new byte[count];
        do
        {
          int num = await fs.ReadAsync(new Memory<byte>(bytes, index, count - index), cancellationToken).ConfigureAwait(false);
          if (num == 0)
            ThrowHelper.ThrowEndOfFileException();
          index += num;
        }
        while (index < count);
        numArray = bytes;
      }
      return numArray;
    }

    private static async Task<byte[]> InternalReadAllBytesUnknownLengthAsync(
      FileStream fs,
      CancellationToken cancellationToken)
    {
      byte[] rentedArray = ArrayPool<byte>.Shared.Rent(512);
      byte[] array1;
      try
      {
        int bytesRead = 0;
        while (true)
        {
          if (bytesRead == rentedArray.Length)
          {
            uint minimumLength = (uint) (rentedArray.Length * 2);
            if (minimumLength > 2147483591U)
              minimumLength = (uint) Math.Max(2147483591, rentedArray.Length + 1);
            byte[] dst = ArrayPool<byte>.Shared.Rent((int) minimumLength);
            Buffer.BlockCopy((Array) rentedArray, 0, (Array) dst, 0, bytesRead);
            byte[] array2 = rentedArray;
            rentedArray = dst;
            ArrayPool<byte>.Shared.Return(array2);
          }
          int num = await fs.ReadAsync(rentedArray.AsMemory<byte>(bytesRead), cancellationToken).ConfigureAwait(false);
          if (num != 0)
            bytesRead += num;
          else
            break;
        }
        array1 = rentedArray.AsSpan<byte>(0, bytesRead).ToArray();
      }
      finally
      {
        fs.Dispose();
        ArrayPool<byte>.Shared.Return(rentedArray);
      }
      rentedArray = (byte[]) null;
      return array1;
    }


    #nullable enable
    /// <summary>Asynchronously creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAllBytesAsync(
      string path,
      byte[] bytes,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path), SR.ArgumentNull_Path);
        default:
          if (bytes == null)
            throw new ArgumentNullException(nameof (bytes));
          return !cancellationToken.IsCancellationRequested ? Core(path, bytes, cancellationToken) : Task.FromCanceled(cancellationToken);
      }


      #nullable disable
      static async Task Core(string path, byte[] bytes, CancellationToken cancellationToken)
      {
        using (SafeFileHandle sfh = File.OpenHandle(path, FileMode.Create, FileAccess.Write, options: (FileOptions.Asynchronous | FileOptions.SequentialScan)))
          await RandomAccess.WriteAtOffsetAsync(sfh, (ReadOnlyMemory<byte>) bytes, 0L, cancellationToken).ConfigureAwait(false);
      }
    }


    #nullable enable
    /// <summary>Asynchronously opens a text file, reads all lines of the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string array containing all lines of the file.</returns>
    public static Task<string[]> ReadAllLinesAsync(
      string path,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return File.ReadAllLinesAsync(path, Encoding.UTF8, cancellationToken);
    }

    /// <summary>Asynchronously opens a text file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string array containing all lines of the file.</returns>
    public static Task<string[]> ReadAllLinesAsync(
      string path,
      Encoding encoding,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      return !cancellationToken.IsCancellationRequested ? File.InternalReadAllLinesAsync(path, encoding, cancellationToken) : Task.FromCanceled<string[]>(cancellationToken);
    }


    #nullable disable
    private static async Task<string[]> InternalReadAllLinesAsync(
      string path,
      Encoding encoding,
      CancellationToken cancellationToken)
    {
      string[] array;
      using (StreamReader sr = File.AsyncStreamReader(path, encoding))
      {
        cancellationToken.ThrowIfCancellationRequested();
        List<string> lines = new List<string>();
        while (true)
        {
          string str;
          if ((str = await sr.ReadLineAsync().ConfigureAwait(false)) != null)
          {
            lines.Add(str);
            cancellationToken.ThrowIfCancellationRequested();
          }
          else
            break;
        }
        array = lines.ToArray();
      }
      return array;
    }


    #nullable enable
    /// <summary>Asynchronously creates a new file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAllLinesAsync(
      string path,
      IEnumerable<string> contents,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return File.WriteAllLinesAsync(path, contents, File.UTF8NoBOM, cancellationToken);
    }

    /// <summary>Asynchronously creates a new file, write the specified lines to the file by using the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAllLinesAsync(
      string path,
      IEnumerable<string> contents,
      Encoding encoding,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (contents == null)
        throw new ArgumentNullException(nameof (contents));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      return !cancellationToken.IsCancellationRequested ? File.InternalWriteAllLinesAsync((TextWriter) File.AsyncStreamWriter(path, encoding, false), contents, cancellationToken) : Task.FromCanceled(cancellationToken);
    }


    #nullable disable
    private static async Task InternalWriteAllLinesAsync(
      TextWriter writer,
      IEnumerable<string> contents,
      CancellationToken cancellationToken)
    {
      using (writer)
      {
        foreach (string content in contents)
        {
          cancellationToken.ThrowIfCancellationRequested();
          await writer.WriteLineAsync(content).ConfigureAwait(false);
        }
        cancellationToken.ThrowIfCancellationRequested();
        await writer.FlushAsync().ConfigureAwait(false);
      }
    }

    private static async Task InternalWriteAllTextAsync(
      StreamWriter sw,
      string contents,
      CancellationToken cancellationToken)
    {
      using (sw)
      {
        await sw.WriteAsync(contents.AsMemory(), cancellationToken).ConfigureAwait(false);
        await sw.FlushAsync().ConfigureAwait(false);
      }
    }


    #nullable enable
    /// <summary>Asynchronously opens a file or creates a file if it does not already exist, appends the specified string to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    public static Task AppendAllTextAsync(
      string path,
      string? contents,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return File.AppendAllTextAsync(path, contents, File.UTF8NoBOM, cancellationToken);
    }

    /// <summary>Asynchronously opens a file or creates the file if it does not already exist, appends the specified string to the file using the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    public static Task AppendAllTextAsync(
      string path,
      string? contents,
      Encoding encoding,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (!string.IsNullOrEmpty(contents))
        return File.InternalWriteAllTextAsync(File.AsyncStreamWriter(path, encoding, true), contents, cancellationToken);
      new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read).Dispose();
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    public static Task AppendAllLinesAsync(
      string path,
      IEnumerable<string> contents,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return File.AppendAllLinesAsync(path, contents, File.UTF8NoBOM, cancellationToken);
    }

    /// <summary>Asynchronously appends lines to a file by using a specified encoding, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    public static Task AppendAllLinesAsync(
      string path,
      IEnumerable<string> contents,
      Encoding encoding,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (contents == null)
        throw new ArgumentNullException(nameof (contents));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
      return !cancellationToken.IsCancellationRequested ? File.InternalWriteAllLinesAsync((TextWriter) File.AsyncStreamWriter(path, encoding, true), contents, cancellationToken) : Task.FromCanceled(cancellationToken);
    }

    /// <summary>Creates a file symbolic link identified by <paramref name="path" /> that points to <paramref name="pathToTarget" />.</summary>
    /// <param name="path">The path where the symbolic link should be created.</param>
    /// <param name="pathToTarget">The path of the target to which the symbolic link points.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="pathToTarget" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="path" /> or <paramref name="pathToTarget" /> is empty.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> or <paramref name="pathToTarget" /> contains a <see langword="null" /> character.</exception>
    /// <exception cref="T:System.IO.IOException">A file or directory already exists in the location of <paramref name="path" />.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred.</exception>
    /// <returns>A <see cref="T:System.IO.FileInfo" /> instance that wraps the newly created file symbolic link.</returns>
    public static FileSystemInfo CreateSymbolicLink(string path, string pathToTarget)
    {
      string fullPath = Path.GetFullPath(path);
      FileSystem.VerifyValidPath(pathToTarget, nameof (pathToTarget));
      FileSystem.CreateSymbolicLink(path, pathToTarget, false);
      return (FileSystemInfo) new FileInfo(path, fullPath, isNormalized: true);
    }

    /// <summary>Gets the target of the specified file link.</summary>
    /// <param name="linkPath">The path of the file link.</param>
    /// <param name="returnFinalTarget">
    /// <see langword="true" /> to follow links to the final target; <see langword="false" /> to return the immediate next link.</param>
    /// <exception cref="T:System.IO.IOException">The file on <paramref name="linkPath" /> does not exist.
    /// 
    /// -or-
    /// 
    /// There are too many levels of symbolic links.</exception>
    /// <returns>A <see cref="T:System.IO.FileInfo" /> instance if <paramref name="linkPath" /> exists, independently if the target exists or not. <see langword="null" /> if <paramref name="linkPath" /> is not a link.</returns>
    public static FileSystemInfo? ResolveLinkTarget(
      string linkPath,
      bool returnFinalTarget)
    {
      FileSystem.VerifyValidPath(linkPath, nameof (linkPath));
      return FileSystem.ResolveLinkTarget(linkPath, returnFinalTarget, false);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified path, creation mode, read/write and sharing permission, the access other FileStreams can have to the same file, the buffer size, additional file options and the allocation size.</summary>
    /// <param name="path">The path of the file to open.</param>
    /// <param name="options">An object that describes optional <see cref="T:System.IO.FileStream" /> parameters to use.</param>
    /// <returns>A <see cref="T:System.IO.FileStream" /> instance that wraps the opened file.</returns>
    public static FileStream Open(string path, FileStreamOptions options) => new FileStream(path, options);

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.SafeFileHandle" /> class with the specified path, creation mode, read/write and sharing permission, the access other SafeFileHandles can have to the same file, additional file options and the allocation size.</summary>
    /// <param name="path">A relative or absolute path for the file that the current <see cref="T:Microsoft.Win32.SafeHandles.SafeFileHandle" /> instance will encapsulate.</param>
    /// <param name="mode">One of the enumeration values that determines how to open or create the file. The default value is <see cref="F:System.IO.FileMode.Open" /></param>
    /// <param name="access">A bitwise combination of the enumeration values that determines how the file can be accessed. The default value is <see cref="F:System.IO.FileAccess.Read" /></param>
    /// <param name="share">A bitwise combination of the enumeration values that determines how the file will be shared by processes. The default value is <see cref="F:System.IO.FileShare.Read" />.</param>
    /// <param name="options">An object that describes optional <see cref="T:Microsoft.Win32.SafeHandles.SafeFileHandle" /> parameters to use.</param>
    /// <param name="preallocationSize">The initial allocation size in bytes for the file. A positive value is effective only when a regular file is being created, overwritten, or replaced.
    /// Negative values are not allowed. In other cases (including the default 0 value), it's ignored.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string (""), contains only white space, or contains one or more invalid characters.
    ///     -or- <paramref name="path" /> refers to a non-file device, such as <c>CON:</c>, <c>COM1:</c>, <c>LPT1:</c>, etc. in an NTFS environment.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> refers to a non-file device, such as <c>CON:</c>, <c>COM1:</c>, <c>LPT1:</c>, etc. in a non-NTFS environment.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="preallocationSize" /> is negative.
    ///     -or- <paramref name="mode" />, <paramref name="access" />, or <paramref name="share" /> contain an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Truncate" /> or <see cref="F:System.IO.FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as specifying <see cref="F:System.IO.FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.
    /// -or- The disk was full (when <paramref name="preallocationSize" /> was provided and <paramref name="path" /> was pointing to a regular file).
    /// -or- The file was too large (when <paramref name="preallocationSize" /> was provided and <paramref name="path" /> was pointing to a regular file).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified <paramref name="path" />, such as when <paramref name="access" />  is <see cref="F:System.IO.FileAccess.Write" /> or <see cref="F:System.IO.FileAccess.ReadWrite" /> and the file or directory is set for read-only access.
    /// -or- <see cref="F:System.IO.FileOptions.Encrypted" /> is specified for <paramref name="options" />, but file encryption is not supported on the current platform.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <returns>A <see cref="T:Microsoft.Win32.SafeHandles.SafeFileHandle" /> instance.</returns>
    public static SafeFileHandle OpenHandle(
      string path,
      FileMode mode = FileMode.Open,
      FileAccess access = FileAccess.Read,
      FileShare share = FileShare.Read,
      FileOptions options = FileOptions.None,
      long preallocationSize = 0)
    {
      FileStreamHelpers.ValidateArguments(path, mode, access, share, 0, options, preallocationSize);
      return SafeFileHandle.Open(Path.GetFullPath(path), mode, access, share, options, preallocationSize);
    }


    #nullable disable
    private static unsafe byte[] ReadAllBytesUnknownLength(FileStream fs)
    {
      byte[] array1 = (byte[]) null;
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(512)), 512);
      try
      {
        int num1 = 0;
        while (true)
        {
          if (num1 == span.Length)
          {
            uint minimumLength = (uint) (span.Length * 2);
            if ((long) minimumLength > (long) Array.MaxLength)
              minimumLength = (uint) Math.Max(Array.MaxLength, span.Length + 1);
            byte[] destination = ArrayPool<byte>.Shared.Rent((int) minimumLength);
            span.CopyTo((Span<byte>) destination);
            byte[] array2 = array1;
            span = (Span<byte>) (array1 = destination);
            if (array2 != null)
              ArrayPool<byte>.Shared.Return(array2);
          }
          int num2 = fs.Read(span.Slice(num1));
          if (num2 != 0)
            num1 += num2;
          else
            break;
        }
        return span.Slice(0, num1).ToArray();
      }
      finally
      {
        if (array1 != null)
          ArrayPool<byte>.Shared.Return(array1);
      }
    }
  }
}
