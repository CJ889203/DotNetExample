// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonCommentHandling
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json
{
  /// <summary>Defines how the <see cref="T:System.Text.Json.Utf8JsonReader" /> struct handles comments.</summary>
  public enum JsonCommentHandling : byte
  {
    /// <summary>Doesn't allow comments within the JSON input. Comments are treated as invalid JSON if found, and a <see cref="T:System.Text.Json.JsonException" /> is thrown. This is the default value.</summary>
    Disallow,
    /// <summary>Allows comments within the JSON input and ignores them. The <see cref="T:System.Text.Json.Utf8JsonReader" /> behaves as if no comments are present.</summary>
    Skip,
    /// <summary>Allows comments within the JSON input and treats them as valid tokens. While reading, the caller can access the comment values.</summary>
    Allow,
  }
}
