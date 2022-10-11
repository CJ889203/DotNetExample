// Decompiled with JetBrains decompiler
// Type: System.IO.ErrorEventArgs
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml


#nullable enable
namespace System.IO
{
  /// <summary>Provides data for the <see cref="E:System.IO.FileSystemWatcher.Error" /> event.</summary>
  public class ErrorEventArgs : EventArgs
  {

    #nullable disable
    private readonly Exception _exception;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.ErrorEventArgs" /> class.</summary>
    /// <param name="exception">An <see cref="T:System.Exception" /> that represents the error that occurred.</param>
    public ErrorEventArgs(Exception exception) => this._exception = exception;

    /// <summary>Gets the <see cref="T:System.Exception" /> that represents the error that occurred.</summary>
    /// <returns>An <see cref="T:System.Exception" /> that represents the error that occurred.</returns>
    public virtual Exception GetException() => this._exception;
  }
}
