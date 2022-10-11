// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonKnownNamingPolicy
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>The <see cref="T:System.Text.Json.JsonNamingPolicy" /> to be used at run time.</summary>
  public enum JsonKnownNamingPolicy
  {
    /// <summary>Specifies that JSON property names should not be converted.</summary>
    Unspecified,
    /// <summary>Specifies that the built-in <see cref="P:System.Text.Json.JsonNamingPolicy.CamelCase" /> be used to convert JSON property names.</summary>
    CamelCase,
  }
}
