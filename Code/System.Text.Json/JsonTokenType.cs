// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonTokenType
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json
{
  /// <summary>Defines the various JSON tokens that make up a JSON text.</summary>
  public enum JsonTokenType : byte
  {
    /// <summary>There is no value (as distinct from <see cref="F:System.Text.Json.JsonTokenType.Null" />). This is the default token type if no data has been read by the <see cref="T:System.Text.Json.Utf8JsonReader" />.</summary>
    None,
    /// <summary>The token type is the start of a JSON object.</summary>
    StartObject,
    /// <summary>The token type is the end of a JSON object.</summary>
    EndObject,
    /// <summary>The token type is the start of a JSON array.</summary>
    StartArray,
    /// <summary>The token type is the end of a JSON array.</summary>
    EndArray,
    /// <summary>The token type is a JSON property name.</summary>
    PropertyName,
    /// <summary>The token type is a comment string.</summary>
    Comment,
    /// <summary>The token type is a JSON string.</summary>
    String,
    /// <summary>The token type is a JSON number.</summary>
    Number,
    /// <summary>The token type is the JSON literal true.</summary>
    True,
    /// <summary>The token type is the JSON literal false.</summary>
    False,
    /// <summary>The token type is the JSON literal null.</summary>
    Null,
  }
}
