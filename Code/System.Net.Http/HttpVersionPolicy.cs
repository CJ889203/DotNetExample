// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpVersionPolicy
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

namespace System.Net.Http
{
  /// <summary>Specifies behaviors for selecting and negotiating the HTTP version for a request.</summary>
  public enum HttpVersionPolicy
  {
    /// <summary>
    ///   <para>Use the requested version or downgrade to a lower one. This is the default behavior.</para>
    ///   <para>If the server supports the requested version, either negotiated via ALPN (H2) or advertised via Alt-Svc (H3), and a secure connection is being requested, the result is the <see cref="System.Net.Http.HttpRequestMessage.Version" />. Otherwise, the version downgrades to HTTP/1.1. This option does not allow use of a prenegotiated clear text connection, for example, H2C.</para>
    /// </summary>
    RequestVersionOrLower,
    /// <summary>
    ///   <para>Use the highest available version, downgrading only to the requested version but not below.</para>
    ///   <para>If the server supports a higher version than the requested version (either negotiated via ALPN (H2) or advertised via Alt-Svc (H3)) and a secure connection is requested, the result is the highest available version. Otherwise, the version downgrades to <see cref="System.Net.Http.HttpRequestMessage.Version" />. This option allows use of a prenegotiated clear text connection for the requested version but not for a higher version.</para>
    /// </summary>
    RequestVersionOrHigher,
    /// <summary>
    ///   <para>Only use the requested version.</para>
    ///   <para>This option allows for use of a prenegotiated clear text connection for the requested version.</para>
    /// </summary>
    RequestVersionExact,
  }
}
