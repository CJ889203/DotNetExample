// Decompiled with JetBrains decompiler
// Type: System.IO.FileAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.IO
{
  /// <summary>Provides attributes for files and directories.</summary>
  [Flags]
  public enum FileAttributes
  {
    /// <summary>The file is read-only. <see langword="ReadOnly" /> is supported on Windows, Linux, and macOS. On Linux and macOS, changing the <see langword="ReadOnly" /> flag is a permissions operation.</summary>
    ReadOnly = 1,
    /// <summary>The file is hidden, and thus is not included in an ordinary directory listing. <see langword="Hidden" /> is supported on Windows, Linux, and macOS.</summary>
    Hidden = 2,
    /// <summary>The file is a system file. That is, the file is part of the operating system or is used exclusively by the operating system.</summary>
    System = 4,
    /// <summary>The file is a directory. <see langword="Directory" /> is supported on Windows, Linux, and macOS.</summary>
    Directory = 16, // 0x00000010
    /// <summary>This file is marked to be included in incremental backup operation. Windows sets this attribute whenever the file is modified, and backup software should clear it when processing the file during incremental backup.</summary>
    Archive = 32, // 0x00000020
    /// <summary>Reserved for future use.</summary>
    Device = 64, // 0x00000040
    /// <summary>The file is a standard file that has no special attributes. This attribute is valid only if it is used alone. <see langword="Normal" /> is supported on Windows, Linux, and macOS.</summary>
    Normal = 128, // 0x00000080
    /// <summary>The file is temporary. A temporary file contains data that is needed while an application is executing but is not needed after the application is finished. File systems try to keep all the data in memory for quicker access rather than flushing the data back to mass storage. A temporary file should be deleted by the application as soon as it is no longer needed.</summary>
    Temporary = 256, // 0x00000100
    /// <summary>The file is a sparse file. Sparse files are typically large files whose data consists of mostly zeros.</summary>
    SparseFile = 512, // 0x00000200
    /// <summary>The file contains a reparse point, which is a block of user-defined data associated with a file or a directory. <see langword="ReparsePoint" /> is supported on Windows, Linux, and macOS.</summary>
    ReparsePoint = 1024, // 0x00000400
    /// <summary>The file is compressed.</summary>
    Compressed = 2048, // 0x00000800
    /// <summary>The file is offline. The data of the file is not immediately available.</summary>
    Offline = 4096, // 0x00001000
    /// <summary>The file will not be indexed by the operating system's content indexing service.</summary>
    NotContentIndexed = 8192, // 0x00002000
    /// <summary>The file or directory is encrypted. For a file, this means that all data in the file is encrypted. For a directory, this means that encryption is the default for newly created files and directories.</summary>
    Encrypted = 16384, // 0x00004000
    /// <summary>The file or directory includes data integrity support. When this value is applied to a file, all data streams in the file have integrity support. When this value is applied to a directory, all new files and subdirectories within that directory, by default, include integrity support.</summary>
    IntegrityStream = 32768, // 0x00008000
    /// <summary>The file or directory is excluded from the data integrity scan. When this value is applied to a directory, by default, all new files and subdirectories within that directory are excluded from data integrity.</summary>
    NoScrubData = 131072, // 0x00020000
  }
}
