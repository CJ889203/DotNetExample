// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpKeepAlivePingPolicy
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

namespace System.Net.Http
{
  /// <summary>Specifies when the HTTP/2 ping frame is sent on an idle connection.</summary>
  public enum HttpKeepAlivePingPolicy
  {
    /// <summary>Sends a keep alive ping only when there are active streams on the connection.</summary>
    WithActiveRequests,
    /// <summary>Sends a keep alive ping for the whole lifetime of the connection.</summary>
    Always,
  }
}
