// Decompiled with JetBrains decompiler
// Type: System.UriFormat
// Assembly: System.Private.Uri, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: EFC59026-8404-447E-A976-365A5080B26A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Uri.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System
{
  /// <summary>Controls how URI information is escaped.</summary>
  public enum UriFormat
  {
    /// <summary>Escaping is performed according to the rules in RFC 2396.</summary>
    UriEscaped = 1,
    /// <summary>No escaping is performed.</summary>
    Unescaped = 2,
    /// <summary>Characters that have a reserved meaning in the requested URI components remain escaped. All others are not escaped.</summary>
    SafeUnescaped = 3,
  }
}
