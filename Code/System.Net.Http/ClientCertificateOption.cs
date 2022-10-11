// Decompiled with JetBrains decompiler
// Type: System.Net.Http.ClientCertificateOption
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

namespace System.Net.Http
{
  /// <summary>Specifies how client certificates are provided.</summary>
  public enum ClientCertificateOption
  {
    /// <summary>The application manually provides the client certificates to the <see cref="T:System.Net.Http.WebRequestHandler" />. This value is the default.</summary>
    Manual,
    /// <summary>The <see cref="T:System.Net.Http.HttpClientHandler" /> will attempt to provide  all available client certificates  automatically.</summary>
    Automatic,
  }
}
