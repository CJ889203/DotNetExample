// Decompiled with JetBrains decompiler
// Type: System.Net.Http.SocketsHttpConnectionContext
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml


#nullable enable
namespace System.Net.Http
{
  /// <summary>Represents the context passed to the <see cref="P:System.Net.Http.SocketsHttpHandler.ConnectCallback" /> for a <see cref="T:System.Net.Http.SocketsHttpHandler" /> instance. .</summary>
  public sealed class SocketsHttpConnectionContext
  {

    #nullable disable
    private readonly DnsEndPoint _dnsEndPoint;
    private readonly HttpRequestMessage _initialRequestMessage;

    internal SocketsHttpConnectionContext(
      DnsEndPoint dnsEndPoint,
      HttpRequestMessage initialRequestMessage)
    {
      this._dnsEndPoint = dnsEndPoint;
      this._initialRequestMessage = initialRequestMessage;
    }


    #nullable enable
    /// <summary>Gets the DNS endpoint to be used by the <see cref="P:System.Net.Http.SocketsHttpHandler.ConnectCallback" /> to establish the connection.</summary>
    public DnsEndPoint DnsEndPoint => this._dnsEndPoint;

    /// <summary>Gets the initial HttpRequestMessage that is causing the connection to be created.</summary>
    /// <returns>The request message that's causing the connection to be created.</returns>
    public HttpRequestMessage InitialRequestMessage => this._initialRequestMessage;
  }
}
