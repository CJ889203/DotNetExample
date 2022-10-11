// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonUnknownTypeHandling
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>Defines how deserializing a type declared as an <see cref="T:System.Object" /> is handled during deserialization.</summary>
  public enum JsonUnknownTypeHandling
  {
    /// <summary>A type declared as <see cref="T:System.Object" /> is deserialized as a <see cref="F:System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonElement" />.</summary>
    JsonElement,
    /// <summary>A type declared as <see cref="T:System.Object" /> is deserialized as a <see cref="F:System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonNode" />.</summary>
    JsonNode,
  }
}
