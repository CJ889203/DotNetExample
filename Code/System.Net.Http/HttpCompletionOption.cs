// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpCompletionOption
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

namespace System.Net.Http
{
  /// <summary>Indicates if <see cref="T:System.Net.Http.HttpClient" /> operations should be considered completed either as soon as a response is available, or after reading the entire response message including the content.</summary>
  public enum HttpCompletionOption
  {
    /// <summary>The operation should complete after reading the entire response including the content.</summary>
    ResponseContentRead,
    /// <summary>The operation should complete as soon as a response is available and headers are read. The content is not read yet.</summary>
    ResponseHeadersRead,
  }
}
