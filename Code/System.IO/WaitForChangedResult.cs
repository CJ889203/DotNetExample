// Decompiled with JetBrains decompiler
// Type: System.IO.WaitForChangedResult
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml


#nullable enable
namespace System.IO
{
  /// <summary>Contains information on the change that occurred.</summary>
  public struct WaitForChangedResult
  {
    internal static readonly WaitForChangedResult TimedOutResult = new WaitForChangedResult((WatcherChangeTypes) 0, (string) null, (string) null, true);


    #nullable disable
    internal WaitForChangedResult(
      WatcherChangeTypes changeType,
      string name,
      string oldName,
      bool timedOut)
    {
      this.ChangeType = changeType;
      this.Name = name;
      this.OldName = oldName;
      this.TimedOut = timedOut;
    }

    /// <summary>Gets or sets the type of change that occurred.</summary>
    /// <returns>One of the <see cref="T:System.IO.WatcherChangeTypes" /> values.</returns>
    public WatcherChangeTypes ChangeType { get; set; }


    #nullable enable
    /// <summary>Gets or sets the name of the file or directory that changed.</summary>
    /// <returns>The name of the file or directory that changed.</returns>
    public string? Name { get; set; }

    /// <summary>Gets or sets the original name of the file or directory that was renamed.</summary>
    /// <returns>The original name of the file or directory that was renamed.</returns>
    public string? OldName { get; set; }

    /// <summary>Gets or sets a value indicating whether the wait operation timed out.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="M:System.IO.FileSystemWatcher.WaitForChanged(System.IO.WatcherChangeTypes)" /> method timed out; otherwise, <see langword="false" />.</returns>
    public bool TimedOut { get; set; }
  }
}
