// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.ZipFileExtensions
// Assembly: System.IO.Compression.ZipFile, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0994478F-354B-401F-8B34-44FF869CF3C3
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.ZipFile.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.ZipFile.xml

using System.ComponentModel;


#nullable enable
namespace System.IO.Compression
{
  /// <summary>Provides extension methods for the <see cref="T:System.IO.Compression.ZipArchive" /> and <see cref="T:System.IO.Compression.ZipArchiveEntry" /> classes.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class ZipFileExtensions
  {
    /// <summary>Archives a file by compressing it and adding it to the zip archive.</summary>
    /// <param name="destination">The zip archive to add the file to.</param>
    /// <param name="sourceFileName">The path to the file to be archived. You can specify either a relative or an absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="entryName">The name of the entry to create in the zip archive.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="sourceFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.
    /// 
    /// -or-
    /// 
    /// <paramref name="entryName" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="entryName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="sourceFileName" />, the specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="sourceFileName" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">The file specified by <paramref name="sourceFileName" /> cannot be opened, or is too large to be updated (current limit is <see cref="F:System.Int32.MaxValue" />).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="sourceFileName" /> specifies a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission to access the file specified by <paramref name="sourceFileName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="sourceFileName" /> is not found.</exception>
    /// <exception cref="T:System.NotSupportedException">The <paramref name="sourceFileName" /> parameter is in an invalid format.
    /// 
    /// -or-
    /// 
    /// The zip archive does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
    /// <returns>A wrapper for the new entry in the zip archive.</returns>
    public static ZipArchiveEntry CreateEntryFromFile(
      this ZipArchive destination,
      string sourceFileName,
      string entryName)
    {
      return destination.DoCreateEntryFromFile(sourceFileName, entryName, new CompressionLevel?());
    }

    /// <summary>Archives a file by compressing it using the specified compression level and adding it to the zip archive.</summary>
    /// <param name="destination">The zip archive to add the file to.</param>
    /// <param name="sourceFileName">The path to the file to be archived. You can specify either a relative or an absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="entryName">The name of the entry to create in the zip archive.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression effectiveness when creating the entry.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="sourceFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.
    /// 
    /// -or-
    /// 
    /// <paramref name="entryName" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFileName" /> or <paramref name="entryName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="sourceFileName" /> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="sourceFileName" />, the specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">The file specified by <paramref name="sourceFileName" /> cannot be opened, or is too large to be updated (current limit is <see cref="F:System.Int32.MaxValue" />).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    ///        <paramref name="sourceFileName" /> specifies a directory.
    /// 
    /// -or-
    /// 
    /// The caller does not have the required permission to access the file specified by <paramref name="sourceFileName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="sourceFileName" /> is not found.</exception>
    /// <exception cref="T:System.NotSupportedException">The <paramref name="sourceFileName" /> parameter is in an invalid format.
    /// 
    /// -or-
    /// 
    /// The zip archive does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
    /// <returns>A wrapper for the new entry in the zip archive.</returns>
    public static ZipArchiveEntry CreateEntryFromFile(
      this ZipArchive destination,
      string sourceFileName,
      string entryName,
      CompressionLevel compressionLevel)
    {
      return destination.DoCreateEntryFromFile(sourceFileName, entryName, new CompressionLevel?(compressionLevel));
    }


    #nullable disable
    internal static ZipArchiveEntry DoCreateEntryFromFile(
      this ZipArchive destination,
      string sourceFileName,
      string entryName,
      CompressionLevel? compressionLevel)
    {
      if (destination == null)
        throw new ArgumentNullException(nameof (destination));
      if (sourceFileName == null)
        throw new ArgumentNullException(nameof (sourceFileName));
      if (entryName == null)
        throw new ArgumentNullException(nameof (entryName));
      using (FileStream fileStream = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false))
      {
        ZipArchiveEntry entryFromFile = compressionLevel.HasValue ? destination.CreateEntry(entryName, compressionLevel.Value) : destination.CreateEntry(entryName);
        DateTime dateTime = File.GetLastWriteTime(sourceFileName);
        if (dateTime.Year < 1980 || dateTime.Year > 2107)
          dateTime = new DateTime(1980, 1, 1, 0, 0, 0);
        entryFromFile.LastWriteTime = (DateTimeOffset) dateTime;
        using (Stream destination1 = entryFromFile.Open())
          fileStream.CopyTo(destination1);
        return entryFromFile;
      }
    }


    #nullable enable
    /// <summary>Extracts all the files in the zip archive to a directory on the file system.</summary>
    /// <param name="source">The zip archive to extract files from.</param>
    /// <param name="destinationDirectoryName">The path to the directory to place the extracted files in. You can specify either a relative or an absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="destinationDirectoryName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destinationDirectoryName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">The name of an entry in the archive is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.
    /// 
    /// -or-
    /// 
    /// Extracting an entry from the archive would create a file that is outside the directory specified by <paramref name="destinationDirectoryName" />. (For example, this might happen if the entry name contains parent directory accessors.)
    /// 
    /// -or-
    /// 
    /// Two or more entries in the archive have the same name.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission to write to the destination directory.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destinationDirectoryName" /> contains an invalid format.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">An archive entry cannot be found or is corrupt.
    /// 
    /// -or-
    /// 
    /// An archive entry was compressed by using a compression method that is not supported.</exception>
    public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName) => source.ExtractToDirectory(destinationDirectoryName, false);

    /// <summary>Extracts all of the files in the archive to a directory on the file system.</summary>
    /// <param name="source">The <see cref="T:System.IO.Compression.ZipArchive" /> to extract.</param>
    /// <param name="destinationDirectoryName">The path to the destination directory on the file system. The path can be relative or absolute. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="overwriteFiles">
    /// <see langword="true" /> to overwrite existing files; <see langword="false" /> otherwise.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="destinationArchiveFileName" /> is a zero-length string, contains only whitespace,
    ///     or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destinationArchiveFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">The name of a <see cref="T:System.IO.Compression.ZipArchiveEntry" /> is zero-length, contains only whitespace, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// Extracting a <see cref="T:System.IO.Compression.ZipArchiveEntry" /> would have resulted in a destination file that is outside <paramref name="destinationArchiveFileName" /> (for example, if the entry name contains parent directory accessors).
    /// 
    /// -or-
    /// 
    /// A <see cref="T:System.IO.Compression.ZipArchiveEntry" /> has the same name as an already extracted entry from the same archive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destinationArchiveFileName" /> is in an invalid format.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">A <see cref="T:System.IO.Compression.ZipArchiveEntry" /> was not found or was corrupt.
    /// 
    /// -or-
    /// 
    /// A <see cref="T:System.IO.Compression.ZipArchiveEntry" /> has been compressed using a compression method that is not supported.</exception>
    public static void ExtractToDirectory(
      this ZipArchive source,
      string destinationDirectoryName,
      bool overwriteFiles)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (destinationDirectoryName == null)
        throw new ArgumentNullException(nameof (destinationDirectoryName));
      foreach (ZipArchiveEntry entry in source.Entries)
        entry.ExtractRelativeToDirectory(destinationDirectoryName, overwriteFiles);
    }

    /// <summary>Extracts an entry in the zip archive to a file.</summary>
    /// <param name="source">The zip archive entry to extract a file from.</param>
    /// <param name="destinationFileName">The path of the file to create from the contents of the entry. You can  specify either a relative or an absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="destinationFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationFileName" /> specifies a directory.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destinationFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="destinationFileName" /> already exists.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred.
    /// 
    /// -or-
    /// 
    /// The entry is currently open for writing.
    /// 
    /// -or-
    /// 
    /// The entry has been deleted from the archive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission to create the new file.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The entry is missing from the archive, or is corrupt and cannot be read.
    /// 
    /// -or-
    /// 
    /// The entry has been compressed by using a compression method that is not supported.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive that this entry belongs to has been disposed.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="destinationFileName" /> is in an invalid format.
    /// 
    /// -or-
    /// 
    /// The zip archive for this entry was opened in <see cref="F:System.IO.Compression.ZipArchiveMode.Create" /> mode, which does not permit the retrieval of entries.</exception>
    public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName) => source.ExtractToFile(destinationFileName, false);

    /// <summary>Extracts an entry in the zip archive to a file, and optionally overwrites an existing file that has the same name.</summary>
    /// <param name="source">The zip archive entry to extract a file from.</param>
    /// <param name="destinationFileName">The path of the file to create from the contents of the entry. You can specify either a relative or an absolute path. A relative path is interpreted as relative to the current working directory.</param>
    /// <param name="overwrite">
    /// <see langword="true" /> to overwrite an existing file that has the same name as the destination file; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="destinationFileName" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationFileName" /> specifies a directory.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destinationFileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="destinationFileName" /> already exists and <paramref name="overwrite" /> is <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred.
    /// 
    /// -or-
    /// 
    /// The entry is currently open for writing.
    /// 
    /// -or-
    /// 
    /// The entry has been deleted from the archive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission to create the new file.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The entry is missing from the archive or is corrupt and cannot be read.
    /// 
    /// -or-
    /// 
    /// The entry has been compressed by using a compression method that is not supported.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive that this entry belongs to has been disposed.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="destinationFileName" /> is in an invalid format.
    /// 
    /// -or-
    /// 
    /// The zip archive for this entry was opened in <see cref="F:System.IO.Compression.ZipArchiveMode.Create" /> mode, which does not permit the retrieval of entries.</exception>
    public static void ExtractToFile(
      this ZipArchiveEntry source,
      string destinationFileName,
      bool overwrite)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (destinationFileName == null)
        throw new ArgumentNullException(nameof (destinationFileName));
      FileMode mode = overwrite ? FileMode.Create : FileMode.CreateNew;
      using (FileStream destination = new FileStream(destinationFileName, mode, FileAccess.Write, FileShare.None, 4096, false))
      {
        using (Stream stream = source.Open())
          stream.CopyTo((Stream) destination);
      }
      try
      {
        File.SetLastWriteTime(destinationFileName, source.LastWriteTime.DateTime);
      }
      catch
      {
      }
    }


    #nullable disable
    internal static void ExtractRelativeToDirectory(
      this ZipArchiveEntry source,
      string destinationDirectoryName,
      bool overwrite)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      string path1 = destinationDirectoryName != null ? Directory.CreateDirectory(destinationDirectoryName).FullName : throw new ArgumentNullException(nameof (destinationDirectoryName));
      if (!path1.EndsWith(Path.DirectorySeparatorChar))
        path1 += Path.DirectorySeparatorChar.ToString();
      string fullPath = Path.GetFullPath(Path.Combine(path1, source.FullName));
      if (!fullPath.StartsWith(path1, PathInternal.StringComparison))
        throw new IOException(SR.IO_ExtractingResultsInOutside);
      if (Path.GetFileName(fullPath).Length == 0)
      {
        if (source.Length != 0L)
          throw new IOException(SR.IO_DirectoryNameWithData);
        Directory.CreateDirectory(fullPath);
      }
      else
      {
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        source.ExtractToFile(fullPath, overwrite);
      }
    }
  }
}
