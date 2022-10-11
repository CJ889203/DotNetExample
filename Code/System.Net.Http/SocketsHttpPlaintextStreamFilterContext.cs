// Decompiled with JetBrains decompiler
// Type: System.Net.Http.SocketsHttpPlaintextStreamFilterContext
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.IO;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Represents the context passed to the PlaintextStreamFilter for a SocketsHttpHandler instance.</summary>
  public sealed class SocketsHttpPlaintextStreamFilterContext
  {

    #nullable disable
    private readonly Stream _plaintextStream;
    private readonly Version _negotiatedHttpVersion;
    private readonly HttpRequestMessage _initialRequestMessage;

    internal SocketsHttpPlaintextStreamFilterContext(
      Stream plaintextStream,
      Version negotiatedHttpVersion,
      HttpRequestMessage initialRequestMessage)
    {
      this._plaintextStream = plaintextStream;
      this._negotiatedHttpVersion = negotiatedHttpVersion;
      this._initialRequestMessage = initialRequestMessage;
    }


    #nullable enable
    /// <summary>Gets the plaintext Stream that will be used for HTTP protocol requests and responses.</summary>
    /// <returns>The plaintext stream that will be used for HTTP protocol requests and responses.</returns>
    public Stream PlaintextStream => this._plaintextStream;

    /// <summary>Gets the version of HTTP in use for this stream.</summary>
    /// <returns>The version of HTTP in use for this stream.</returns>
    public Version NegotiatedHttpVersion => this._negotiatedHttpVersion;

    /// <summary>Gets the initial HttpRequestMessage that is causing the stream to be used.</summary>
    /// <returns>The HTTP request message that is causing the stream to be used.</returns>
    public HttpRequestMessage InitialRequestMessage => this._initialRequestMessage;
  }
}
