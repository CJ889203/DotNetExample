// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpRequestOptionsKey`1
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml


#nullable enable
namespace System.Net.Http
{
  /// <summary>Represents a key in the options collection for an HTTP request.</summary>
  /// <typeparam name="TValue">The type of the value of the option.</typeparam>
  public readonly struct HttpRequestOptionsKey<TValue>
  {
    /// <summary>Gets the name of the option.</summary>
    public string Key { get; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpRequestOptionsKey`1" /> class using the specified key name.</summary>
    /// <param name="key">Name of the HTTP request option.</param>
    public HttpRequestOptionsKey(string key) => this.Key = key;
  }
}
