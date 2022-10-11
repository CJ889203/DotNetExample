// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonValueKind
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json
{
  /// <summary>Specifies the data type of a JSON value.</summary>
  public enum JsonValueKind : byte
  {
    /// <summary>There is no value (as distinct from <see cref="F:System.Text.Json.JsonValueKind.Null" />).</summary>
    Undefined,
    /// <summary>A JSON object.</summary>
    Object,
    /// <summary>A JSON array.</summary>
    Array,
    /// <summary>A JSON string.</summary>
    String,
    /// <summary>A JSON number.</summary>
    Number,
    /// <summary>The JSON value true.</summary>
    True,
    /// <summary>The JSON value false.</summary>
    False,
    /// <summary>The JSON value null.</summary>
    Null,
  }
}
