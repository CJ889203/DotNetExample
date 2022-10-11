// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonIgnoreCondition
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>Controls how the <see cref="T:System.Text.Json.Serialization.JsonIgnoreAttribute" /> ignores properties on serialization and deserialization.</summary>
  public enum JsonIgnoreCondition
  {
    /// <summary>Property will always be serialized and deserialized, regardless of <see cref="P:System.Text.Json.JsonSerializerOptions.IgnoreNullValues" /> configuration.</summary>
    Never,
    /// <summary>Property will always be ignored.</summary>
    Always,
    /// <summary>Property will only be ignored if it is <see langword="null" />.</summary>
    WhenWritingDefault,
    /// <summary>If the value is <see langword="null" />, the property is ignored during serialization. This is applied only to reference-type properties and fields.</summary>
    WhenWritingNull,
  }
}
