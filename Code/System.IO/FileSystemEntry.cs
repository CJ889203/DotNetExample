// Decompiled with JetBrains decompiler
// Type: System.IO.Enumeration.FileSystemEntry
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.IO.Enumeration
{
  /// <summary>Provides a lower level view of <see cref="T:System.IO.FileSystemInfo" /> to help process and filter find results.</summary>
  public ref struct FileSystemEntry
  {

    #nullable disable
    internal unsafe Interop.NtDll.FILE_FULL_DIR_INFORMATION* _info;


    #nullable enable
    /// <summary>Returns the full path for the find results, based on the initially provided path.</summary>
    /// <returns>A string representing the full path.</returns>
    public string ToSpecifiedFullPath()
    {
      ReadOnlySpan<char> readOnlySpan = this.Directory.Slice(this.RootDirectory.Length);
      if (Path.EndsInDirectorySeparator(this.OriginalRootDirectory) && PathInternal.StartsWithDirectorySeparator(readOnlySpan))
        readOnlySpan = readOnlySpan.Slice(1);
      return Path.Join(this.OriginalRootDirectory, readOnlySpan, this.FileName);
    }


    #nullable disable
    internal static unsafe void Initialize(
      ref FileSystemEntry entry,
      Interop.NtDll.FILE_FULL_DIR_INFORMATION* info,
      ReadOnlySpan<char> directory,
      ReadOnlySpan<char> rootDirectory,
      ReadOnlySpan<char> originalRootDirectory)
    {
      entry._info = info;
      entry.Directory = directory;
      entry.RootDirectory = rootDirectory;
      entry.OriginalRootDirectory = originalRootDirectory;
    }


    #nullable enable
    /// <summary>Gets the full path of the directory this entry resides in.</summary>
    /// <returns>The full path of this entry's directory.</returns>
    public ReadOnlySpan<char> Directory { get; private set; }

    /// <summary>Gets the full path of the root directory used for the enumeration.</summary>
    /// <returns>The root directory.</returns>
    public ReadOnlySpan<char> RootDirectory { get; private set; }

    /// <summary>Gets the root directory for the enumeration as specified in the constructor.</summary>
    /// <returns>The original root directory.</returns>
    public ReadOnlySpan<char> OriginalRootDirectory { get; private set; }

    /// <summary>Gets the file name for this entry.</summary>
    /// <returns>This entry's file name.</returns>
    public unsafe ReadOnlySpan<char> FileName => this._info->FileName;

    /// <summary>Gets the attributes for this entry.</summary>
    /// <returns>The attributes for this entry.</returns>
    public unsafe FileAttributes Attributes => this._info->FileAttributes;

    /// <summary>Gets the length of the file, in bytes.</summary>
    /// <returns>The file length in bytes.</returns>
    public unsafe long Length => this._info->EndOfFile;

    /// <summary>Gets the creation time for the entry or the oldest available time stamp if the operating system does not support creation time stamps.</summary>
    /// <returns>The creation time for the entry.</returns>
    public unsafe DateTimeOffset CreationTimeUtc => this._info->CreationTime.ToDateTimeOffset();

    /// <summary>Gets a datetime offset that represents the last access time in UTC.</summary>
    /// <returns>The last access time in UTC.</returns>
    public unsafe DateTimeOffset LastAccessTimeUtc => this._info->LastAccessTime.ToDateTimeOffset();

    /// <summary>Gets a datetime offset that represents the last write time in UTC.</summary>
    /// <returns>The last write time in UTC.</returns>
    public unsafe DateTimeOffset LastWriteTimeUtc => this._info->LastWriteTime.ToDateTimeOffset();

    /// <summary>Gets a value that indicates whether this entry is a directory.</summary>
    /// <returns>
    /// <see langword="true" /> if the entry is a directory; otherwise, <see langword="false" />.</returns>
    public bool IsDirectory => (this.Attributes & FileAttributes.Directory) != 0;

    /// <summary>Gets a value that indicates whether the file has the hidden attribute.</summary>
    /// <returns>
    /// <see langword="true" /> if the file has the hidden attribute; otherwise, <see langword="false" />.</returns>
    public bool IsHidden => (this.Attributes & FileAttributes.Hidden) != 0;

    /// <summary>Converts the value of this instance to a <see cref="T:System.IO.FileSystemInfo" />.</summary>
    /// <returns>The value of this instance as a <see cref="T:System.IO.FileSystemInfo" />.</returns>
    public FileSystemInfo ToFileSystemInfo() => FileSystemInfo.Create(Path.Join(this.Directory, this.FileName), ref this);

    /// <summary>Returns the full path of the find result.</summary>
    /// <returns>A string representing the full path.</returns>
    public string ToFullPath() => Path.Join(this.Directory, this.FileName);
  }
}
