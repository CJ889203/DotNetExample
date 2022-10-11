// Decompiled with JetBrains decompiler
// Type: System.IO.FileSystemEventArgs
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml


#nullable enable
namespace System.IO
{
  /// <summary>Provides data for the directory events: <see cref="E:System.IO.FileSystemWatcher.Changed" />, <see cref="E:System.IO.FileSystemWatcher.Created" />, <see cref="E:System.IO.FileSystemWatcher.Deleted" />.</summary>
  public class FileSystemEventArgs : EventArgs
  {
    private readonly WatcherChangeTypes _changeType;

    #nullable disable
    private readonly string _name;
    private readonly string _fullPath;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemEventArgs" /> class.</summary>
    /// <param name="changeType">One of the <see cref="T:System.IO.WatcherChangeTypes" /> values, which represents the kind of change detected in the file system.</param>
    /// <param name="directory">The root directory of the affected file or directory.</param>
    /// <param name="name">The name of the affected file or directory.</param>
    public FileSystemEventArgs(WatcherChangeTypes changeType, string directory, string? name)
    {
      this._changeType = changeType;
      this._name = name;
      this._fullPath = FileSystemEventArgs.Combine(directory, name);
    }


    #nullable disable
    internal static string Combine(string directoryPath, string name)
    {
      bool flag = false;
      if (directoryPath.Length > 0)
      {
        char ch = directoryPath[directoryPath.Length - 1];
        flag = (int) ch == (int) Path.DirectorySeparatorChar || (int) ch == (int) Path.AltDirectorySeparatorChar;
      }
      return !flag ? directoryPath + Path.DirectorySeparatorChar.ToString() + name : directoryPath + name;
    }

    /// <summary>Gets the type of directory event that occurred.</summary>
    /// <returns>One of the <see cref="T:System.IO.WatcherChangeTypes" /> values that represents the kind of change detected in the file system.</returns>
    public WatcherChangeTypes ChangeType => this._changeType;


    #nullable enable
    /// <summary>Gets the fully qualified path of the affected file or directory.</summary>
    /// <returns>The path of the affected file or directory.</returns>
    public string FullPath => this._fullPath;

    /// <summary>Gets the name of the affected file or directory.</summary>
    /// <returns>The name of the affected file or directory.</returns>
    public string? Name => this._name;
  }
}
