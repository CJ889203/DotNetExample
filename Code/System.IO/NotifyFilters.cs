// Decompiled with JetBrains decompiler
// Type: System.IO.NotifyFilters
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml

namespace System.IO
{
  /// <summary>Specifies changes to watch for in a file or folder.</summary>
  [Flags]
  public enum NotifyFilters
  {
    /// <summary>The name of the file.</summary>
    FileName = 1,
    /// <summary>The name of the directory.</summary>
    DirectoryName = 2,
    /// <summary>The attributes of the file or folder.</summary>
    Attributes = 4,
    /// <summary>The size of the file or folder.</summary>
    Size = 8,
    /// <summary>The date the file or folder last had anything written to it.</summary>
    LastWrite = 16, // 0x00000010
    /// <summary>The date the file or folder was last opened.</summary>
    LastAccess = 32, // 0x00000020
    /// <summary>The time the file or folder was created.</summary>
    CreationTime = 64, // 0x00000040
    /// <summary>The security settings of the file or folder.</summary>
    Security = 256, // 0x00000100
  }
}
