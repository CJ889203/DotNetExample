// Decompiled with JetBrains decompiler
// Type: System.IO.WatcherChangeTypes
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml

namespace System.IO
{
  /// <summary>Changes that might occur to a file or directory.</summary>
  [Flags]
  public enum WatcherChangeTypes
  {
    /// <summary>The creation of a file or folder.</summary>
    Created = 1,
    /// <summary>The deletion of a file or folder.</summary>
    Deleted = 2,
    /// <summary>The change of a file or folder. The types of changes include: changes to size, attributes, security settings, last write, and last access time.</summary>
    Changed = 4,
    /// <summary>The renaming of a file or folder.</summary>
    Renamed = 8,
    /// <summary>The creation, deletion, change, or renaming of a file or folder.</summary>
    All = Renamed | Changed | Deleted | Created, // 0x0000000F
  }
}
