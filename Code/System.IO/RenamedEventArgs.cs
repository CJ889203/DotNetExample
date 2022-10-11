// Decompiled with JetBrains decompiler
// Type: System.IO.RenamedEventArgs
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml


#nullable enable
namespace System.IO
{
  /// <summary>Provides data for the <see cref="E:System.IO.FileSystemWatcher.Renamed" /> event.</summary>
  public class RenamedEventArgs : FileSystemEventArgs
  {

    #nullable disable
    private readonly string _oldName;
    private readonly string _oldFullPath;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.RenamedEventArgs" /> class.</summary>
    /// <param name="changeType">One of the <see cref="T:System.IO.WatcherChangeTypes" /> values.</param>
    /// <param name="directory">The name of the affected file or directory.</param>
    /// <param name="name">The name of the affected file or directory.</param>
    /// <param name="oldName">The old name of the affected file or directory.</param>
    public RenamedEventArgs(
      WatcherChangeTypes changeType,
      string directory,
      string? name,
      string? oldName)
      : base(changeType, directory, name)
    {
      this._oldName = oldName;
      this._oldFullPath = FileSystemEventArgs.Combine(directory, oldName);
    }

    /// <summary>Gets the previous fully qualified path of the affected file or directory.</summary>
    /// <returns>The previous fully qualified path of the affected file or directory.</returns>
    public string OldFullPath => this._oldFullPath;

    /// <summary>Gets the old name of the affected file or directory.</summary>
    /// <returns>The previous name of the affected file or directory.</returns>
    public string? OldName => this._oldName;
  }
}
