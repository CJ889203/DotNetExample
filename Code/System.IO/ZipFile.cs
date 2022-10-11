// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.ZipFile
// Assembly: System.IO.Compression.ZipFile, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0994478F-354B-401F-8B34-44FF869CF3C3
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.ZipFile.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.ZipFile.xml

using System.Buffers;
using System.Text;


#nullable enable
namespace System.IO.Compression
{
  /// <summary>Provides static methods for creating, extracting, and opening zip archives.</summary>
  public static class ZipFile
  {
    /// <summary>Opens a zip archive for reading at the specified path.</summary>
    /// <param name="archiveFileName">The path to the archive to open, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="archiveFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="archiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="archiveFileName" />, the specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="archiveFileName" /> is invalid or does not exist (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="archiveFileName" /> could not be opened.
    /// 
    /// -or-
    /// 
    /// An unspecified I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="archiveFileName" /> specifies a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission to access the file specified in <paramref name="archiveFileName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="archiveFileName" /> is not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="archiveFileName" /> contains an invalid format.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">
    /// <paramref name="archiveFileName" /> could not be interpreted as a zip archive.</exception>
    /// <returns>The opened zip archive.</returns>
    public static ZipArchive OpenRead(string archiveFileName) => ZipFile.Open(archiveFileName, ZipArchiveMode.Read);

    /// <summary>Opens a zip archive at the specified path and in the specified mode.</summary>
    /// <param name="archiveFileName">The path to the archive to open, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="mode">One of the enumeration values that specifies the actions which are allowed on the entries in the opened archive.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="archiveFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="archiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="archiveFileName" />, the specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="archiveFileName" /> is invalid or does not exist (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="archiveFileName" /> could not be opened.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="mode" /> is set to <see cref="F:System.IO.Compression.ZipArchiveMode.Create" />, but the file specified in <paramref name="archiveFileName" /> already exists.
    /// 
    /// -or-
    /// 
    /// An unspecified I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="archiveFileName" /> specifies a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission to access the file specified in <paramref name="archiveFileName" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> specifies an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="mode" /> is set to <see cref="F:System.IO.Compression.ZipArchiveMode.Read" />, but the file specified in <paramref name="archiveFileName" /> is not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="archiveFileName" /> contains an invalid format.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">
    ///        <paramref name="archiveFileName" /> could not be interpreted as a zip archive.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" />, but an entry is missing or corrupt and cannot be read.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" />, but an entry is too large to fit into memory.</exception>
    /// <returns>The opened zip archive.</returns>
    public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode) => ZipFile.Open(archiveFileName, mode, (Encoding) null);

    /// <summary>Opens a zip archive at the specified path, in the specified mode, and by using the specified character encoding for entry names.</summary>
    /// <param name="archiveFileName">The path to the archive to open, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="mode">One of the enumeration values that specifies the actions that are allowed on the entries in the opened archive.</param>
    /// <param name="entryNameEncoding">The encoding to use when reading or writing entry names in this archive. Specify a value for this parameter only when an encoding is required for interoperability with zip archive tools and libraries that do not support UTF-8 encoding for entry names.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="archiveFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.
    /// 
    /// -or-
    /// 
    /// <paramref name="entryNameEncoding" /> is set to a Unicode encoding other than UTF-8.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="archiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="archiveFileName" />, the specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="archiveFileName" /> is invalid or does not exist (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="archiveFileName" /> could not be opened.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="mode" /> is set to <see cref="F:System.IO.Compression.ZipArchiveMode.Create" />, but the file specified in <paramref name="archiveFileName" /> already exists.
    /// 
    /// -or-
    /// 
    /// An unspecified I/O error occurred while opening the file.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="archiveFileName" /> specifies a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission to access the file specified in <paramref name="archiveFileName" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> specifies an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="mode" /> is set to <see cref="F:System.IO.Compression.ZipArchiveMode.Read" />, but the file specified in <paramref name="archiveFileName" /> is not found.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="archiveFileName" /> contains an invalid format.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">
    ///        <paramref name="archiveFileName" /> could not be interpreted as a zip archive.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" />, but an entry is missing or corrupt and cannot be read.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" />, but an entry is too large to fit into memory.</exception>
    /// <returns>The opened zip archive.</returns>
    public static ZipArchive Open(
      string archiveFileName,
      ZipArchiveMode mode,
      Encoding? entryNameEncoding)
    {
      FileMode mode1;
      FileAccess access;
      FileShare share;
      switch (mode)
      {
        case ZipArchiveMode.Read:
          mode1 = FileMode.Open;
          access = FileAccess.Read;
          share = FileShare.Read;
          break;
        case ZipArchiveMode.Create:
          mode1 = FileMode.CreateNew;
          access = FileAccess.Write;
          share = FileShare.None;
          break;
        case ZipArchiveMode.Update:
          mode1 = FileMode.OpenOrCreate;
          access = FileAccess.ReadWrite;
          share = FileShare.None;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (mode));
      }
      FileStream fileStream = new FileStream(archiveFileName, mode1, access, share, 4096, false);
      try
      {
        return new ZipArchive((Stream) fileStream, mode, false, entryNameEncoding);
      }
      catch
      {
        fileStream.Dispose();
        throw;
      }
    }

    /// <summary>Creates a zip archive that contains the files and directories from the specified directory.</summary>
    /// <param name="sourceDirectoryName">The path to the directory to be archived, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="destinationArchiveFileName">The path of the archive to be created, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" />, the specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="sourceDirectoryName" /> is invalid or does not exist (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="destinationArchiveFileName" /> already exists.
    /// 
    ///  -or-
    /// 
    ///  A file in the specified directory could not be opened.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred while opening a file to be archived.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="destinationArchiveFileName" /> specifies a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission to access the directory specified in <paramref name="sourceDirectoryName" /> or the file specified in <paramref name="destinationArchiveFileName" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> contains an invalid format.
    /// 
    /// -or-
    /// 
    /// The zip archive does not support writing.</exception>
    public static void CreateFromDirectory(
      string sourceDirectoryName,
      string destinationArchiveFileName)
    {
      ZipFile.DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, new CompressionLevel?(), false, (Encoding) null);
    }

    /// <summary>Creates a zip archive that contains the files and directories from the specified directory, uses the specified compression level, and optionally includes the base directory.</summary>
    /// <param name="sourceDirectoryName">The path to the directory to be archived, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="destinationArchiveFileName">The path of the archive to be created, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression effectiveness when creating the entry.</param>
    /// <param name="includeBaseDirectory">
    /// <see langword="true" /> to include the directory name from <paramref name="sourceDirectoryName" /> at the root of the archive; <see langword="false" /> to include only the contents of the directory.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" />, the specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="sourceDirectoryName" /> is invalid or does not exist (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="destinationArchiveFileName" /> already exists.
    /// 
    ///  -or-
    /// 
    ///  A file in the specified directory could not be opened.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred while opening a file to be archived.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="destinationArchiveFileName" /> specifies a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission to access the directory specified in <paramref name="sourceDirectoryName" /> or the file specified in <paramref name="destinationArchiveFileName" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> contains an invalid format.
    /// 
    /// -or-
    /// 
    /// The zip archive does not support writing.</exception>
    public static void CreateFromDirectory(
      string sourceDirectoryName,
      string destinationArchiveFileName,
      CompressionLevel compressionLevel,
      bool includeBaseDirectory)
    {
      ZipFile.DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, new CompressionLevel?(compressionLevel), includeBaseDirectory, (Encoding) null);
    }

    /// <summary>Creates a zip archive that contains the files and directories from the specified directory, uses the specified compression level and character encoding for entry names, and optionally includes the base directory.</summary>
    /// <param name="sourceDirectoryName">The path to the directory to be archived, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="destinationArchiveFileName">The path of the archive to be created, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression effectiveness when creating the entry.</param>
    /// <param name="includeBaseDirectory">
    /// <see langword="true" /> to include the directory name from <paramref name="sourceDirectoryName" /> at the root of the archive; <see langword="false" /> to include only the contents of the directory.</param>
    /// <param name="entryNameEncoding">The encoding to use when reading or writing entry names in this archive. Specify a value for this parameter only when an encoding is required for interoperability with zip archive tools and libraries that do not support UTF-8 encoding for entry names.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.
    /// 
    /// -or-
    /// 
    /// <paramref name="entryNameEncoding" /> is set to a Unicode encoding other than UTF-8.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" />, the specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="sourceDirectoryName" /> is invalid or does not exist (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="destinationArchiveFileName" /> already exists.
    /// 
    ///  -or-
    /// 
    ///  A file in the specified directory could not be opened.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred while opening a file to be archived.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="destinationArchiveFileName" /> specifies a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission to access the directory specified in <paramref name="sourceDirectoryName" /> or the file specified in <paramref name="destinationArchiveFileName" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="sourceDirectoryName" /> or <paramref name="destinationArchiveFileName" /> contains an invalid format.
    /// 
    /// -or-
    /// 
    /// The zip archive does not support writing.</exception>
    public static void CreateFromDirectory(
      string sourceDirectoryName,
      string destinationArchiveFileName,
      CompressionLevel compressionLevel,
      bool includeBaseDirectory,
      Encoding? entryNameEncoding)
    {
      ZipFile.DoCreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, new CompressionLevel?(compressionLevel), includeBaseDirectory, entryNameEncoding);
    }


    #nullable disable
    private static void DoCreateFromDirectory(
      string sourceDirectoryName,
      string destinationArchiveFileName,
      CompressionLevel? compressionLevel,
      bool includeBaseDirectory,
      Encoding entryNameEncoding)
    {
      sourceDirectoryName = Path.GetFullPath(sourceDirectoryName);
      destinationArchiveFileName = Path.GetFullPath(destinationArchiveFileName);
      using (ZipArchive destination = ZipFile.Open(destinationArchiveFileName, ZipArchiveMode.Create, entryNameEncoding))
      {
        bool flag = true;
        DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectoryName);
        string fullName = directoryInfo.FullName;
        if (includeBaseDirectory && directoryInfo.Parent != null)
          fullName = directoryInfo.Parent.FullName;
        char[] buffer = ArrayPool<char>.Shared.Rent(260);
        try
        {
          foreach (FileSystemInfo enumerateFileSystemInfo in directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
          {
            flag = false;
            int length = enumerateFileSystemInfo.FullName.Length - fullName.Length;
            if (enumerateFileSystemInfo is FileInfo)
            {
              string entryName = ZipFileUtils.EntryFromPath(enumerateFileSystemInfo.FullName, fullName.Length, length, ref buffer);
              destination.DoCreateEntryFromFile(enumerateFileSystemInfo.FullName, entryName, compressionLevel);
            }
            else if (enumerateFileSystemInfo is DirectoryInfo possiblyEmptyDir && ZipFileUtils.IsDirEmpty(possiblyEmptyDir))
            {
              string entryName = ZipFileUtils.EntryFromPath(enumerateFileSystemInfo.FullName, fullName.Length, length, ref buffer, true);
              destination.CreateEntry(entryName);
            }
          }
          if (!(includeBaseDirectory & flag))
            return;
          destination.CreateEntry(ZipFileUtils.EntryFromPath(directoryInfo.Name, 0, directoryInfo.Name.Length, ref buffer, true));
        }
        finally
        {
          ArrayPool<char>.Shared.Return(buffer);
        }
      }
    }


    #nullable enable
    /// <summary>Extracts all the files in the specified zip archive to a directory on the file system.</summary>
    /// <param name="sourceArchiveFileName">The path to the archive that is to be extracted.</param>
    /// <param name="destinationDirectoryName">The path to the directory in which to place the extracted files, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="destinationDirectoryName" /> or <paramref name="sourceArchiveFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destinationDirectoryName" /> or <paramref name="sourceArchiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path in <paramref name="destinationDirectoryName" /> or <paramref name="sourceArchiveFileName" /> exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">The name of an entry in the archive is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.
    /// 
    /// -or-
    /// 
    /// Extracting an archive entry would create a file that is outside the directory specified by <paramref name="destinationDirectoryName" />. (For example, this might happen if the entry name contains parent directory accessors.)
    /// 
    /// -or-
    /// 
    /// An archive entry to extract has the same name as an entry that has already been extracted or that exists in <paramref name="destinationDirectoryName" />.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission to access the archive or the destination directory.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destinationDirectoryName" /> or <paramref name="sourceArchiveFileName" /> contains an invalid format.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceArchiveFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The archive specified by <paramref name="sourceArchiveFileName" /> is not a valid zip archive.
    /// 
    /// -or-
    /// 
    /// An archive entry was not found or was corrupt.
    /// 
    /// -or-
    /// 
    /// An archive entry was compressed by using a compression method that is not supported.</exception>
    public static void ExtractToDirectory(
      string sourceArchiveFileName,
      string destinationDirectoryName)
    {
      ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, (Encoding) null, false);
    }

    /// <summary>Extracts all of the files in the specified archive to a directory on the file system.</summary>
    /// <param name="sourceArchiveFileName">The path on the file system to the archive that is to be extracted.</param>
    /// <param name="destinationDirectoryName">The path to the destination directory on the file system.</param>
    /// <param name="overwriteFiles">
    /// <see langword="true" /> to overwrite files; <see langword="false" /> otherwise.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> is a zero-length string, contains only whitespace, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> specifies a path, a file name, or both that exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified by <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="overwriteFiles" /> is <see langword="false" /> and <paramref name="destinationDirectoryName" /> already contains a file with the same name as a file that's being extracted.
    /// 
    /// -or-
    /// 
    /// An I/O error has occurred.
    /// 
    /// -or-
    /// 
    /// The name of a <see cref="T:System.IO.Compression.ZipArchiveEntry" /> is zero-length, contains only whitespace, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// Extracting a <see cref="T:System.IO.Compression.ZipArchiveEntry" /> would result in a file destination that is outside the destination directory (for example, because of parent directory accessors).
    /// 
    /// -or-
    /// 
    /// A <see cref="T:System.IO.Compression.ZipArchiveEntry" /> has the same name as an entry from the same archive that's already been extracted.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> is in an invalid format.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceArchiveFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The archive specified by <paramref name="sourceArchiveFileName" /> is not a valid <see cref="T:System.IO.Compression.ZipArchive" />.
    /// 
    /// -or-
    /// 
    /// A <see cref="T:System.IO.Compression.ZipArchiveEntry" /> was not found or was corrupt.
    /// 
    /// -or-
    /// 
    /// A <see cref="T:System.IO.Compression.ZipArchiveEntry" /> has been compressed using a compression method that is not supported.</exception>
    public static void ExtractToDirectory(
      string sourceArchiveFileName,
      string destinationDirectoryName,
      bool overwriteFiles)
    {
      ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, (Encoding) null, overwriteFiles);
    }

    /// <summary>Extracts all the files in the specified zip archive to a directory on the file system and uses the specified character encoding for entry names.</summary>
    /// <param name="sourceArchiveFileName">The path to the archive that is to be extracted.</param>
    /// <param name="destinationDirectoryName">The path to the directory in which to place the extracted files, specified as a relative or absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="entryNameEncoding">The encoding to use when reading or writing entry names in this archive. Specify a value for this parameter only when an encoding is required for interoperability with zip archive tools and libraries that do not support UTF-8 encoding for entry names.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="destinationDirectoryName" /> or <paramref name="sourceArchiveFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.
    /// 
    /// -or-
    /// 
    /// <paramref name="entryNameEncoding" /> is set to a Unicode encoding other than UTF-8.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destinationDirectoryName" /> or <paramref name="sourceArchiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path in <paramref name="destinationDirectoryName" /> or <paramref name="sourceArchiveFileName" /> exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">The name of an entry in the archive is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.
    /// 
    /// -or-
    /// 
    /// Extracting an archive entry would create a file that is outside the directory specified by <paramref name="destinationDirectoryName" />. (For example, this might happen if the entry name contains parent directory accessors.)
    /// 
    /// -or-
    /// 
    /// An archive entry to extract has the same name as an entry that has already been extracted or that exists in <paramref name="destinationDirectoryName" />.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission to access the archive or the destination directory.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destinationDirectoryName" /> or <paramref name="sourceArchiveFileName" /> contains an invalid format.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceArchiveFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The archive specified by <paramref name="sourceArchiveFileName" /> is not a valid zip archive.
    /// 
    /// -or-
    /// 
    /// An archive entry was not found or was corrupt.
    /// 
    /// -or-
    /// 
    /// An archive entry was compressed by using a compression method that is not supported.</exception>
    public static void ExtractToDirectory(
      string sourceArchiveFileName,
      string destinationDirectoryName,
      Encoding? entryNameEncoding)
    {
      ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, entryNameEncoding, false);
    }

    /// <summary>Extracts all of the files in the specified archive to a directory on the file system.</summary>
    /// <param name="sourceArchiveFileName">The path on the file system to the archive that is to be extracted.</param>
    /// <param name="destinationDirectoryName">The path to the destination directory on the file system.</param>
    /// <param name="entryNameEncoding">The encoding to use when reading entry names in this <see cref="T:System.IO.Compression.ZipArchive" />.</param>
    /// <param name="overwriteFiles">
    /// <see langword="true" /> to overwrite files; <see langword="false" /> otherwise.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> is a zero-length string, contains only whitespace, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="entryNameEncoding" /> is set to a Unicode encoding other than UTF-8.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> specifies a path, a file name, or both that exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified by <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="overwriteFiles" /> is <see langword="false" /> and an archive entry to extract has the same name as a file that already exists in <paramref name="destinationDirectoryName" />.
    /// 
    /// -or-
    /// 
    /// An I/O error has occurred.
    /// 
    /// -or-
    /// 
    /// The name of a <see cref="T:System.IO.Compression.ZipArchiveEntry" /> is zero-length, contains only whitespace, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// Extracting a <see cref="T:System.IO.Compression.ZipArchiveEntry" /> would result in a file destination that is outside the destination directory (for example, because of parent directory accessors).
    /// 
    /// -or-
    /// 
    /// A <see cref="T:System.IO.Compression.ZipArchiveEntry" /> has the same name as an already extracted entry from the same archive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="sourceArchiveFileName" /> or <paramref name="destinationDirectoryName" /> is in an invalid format.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceArchiveFileName" /> was not found.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The archive specified by <paramref name="sourceArchiveFileName" /> is not a valid <see cref="T:System.IO.Compression.ZipArchive" />.
    /// 
    /// -or-
    /// 
    /// An archive entry was not found or was corrupt.
    /// 
    /// -or-
    /// 
    /// An archive entry has been compressed using a compression method that is not supported.</exception>
    public static void ExtractToDirectory(
      string sourceArchiveFileName,
      string destinationDirectoryName,
      Encoding? entryNameEncoding,
      bool overwriteFiles)
    {
      if (sourceArchiveFileName == null)
        throw new ArgumentNullException(nameof (sourceArchiveFileName));
      using (ZipArchive source = ZipFile.Open(sourceArchiveFileName, ZipArchiveMode.Read, entryNameEncoding))
        source.ExtractToDirectory(destinationDirectoryName, overwriteFiles);
    }
  }
}
