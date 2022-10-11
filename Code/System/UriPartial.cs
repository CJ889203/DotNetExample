// Decompiled with JetBrains decompiler
// Type: System.UriPartial
// Assembly: System.Private.Uri, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: EFC59026-8404-447E-A976-365A5080B26A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Uri.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System
{
  /// <summary>Defines the parts of a URI for the <see cref="M:System.Uri.GetLeftPart(System.UriPartial)" /> method.</summary>
  public enum UriPartial
  {
    /// <summary>The scheme segment of the URI.</summary>
    Scheme,
    /// <summary>The scheme and authority segments of the URI.</summary>
    Authority,
    /// <summary>The scheme, authority, and path segments of the URI.</summary>
    Path,
    /// <summary>The scheme, authority, path, and query segments of the URI.</summary>
    Query,
  }
}
