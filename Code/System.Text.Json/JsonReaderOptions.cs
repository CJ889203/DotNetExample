// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonReaderOptions
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json
{
  /// <summary>Provides the ability for the user to define custom behavior when reading JSON.</summary>
  public struct JsonReaderOptions
  {
    private int _maxDepth;
    private JsonCommentHandling _commentHandling;

    /// <summary>Gets or sets a value that determines how the <see cref="T:System.Text.Json.Utf8JsonReader" /> handles comments when reading through the JSON data.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is not a member of the <see cref="T:System.Text.Json.JsonCommentHandling" /> enumeration.</exception>
    /// <returns>One of the enumeration values that indicates how comments are handled.</returns>
    public JsonCommentHandling CommentHandling
    {
      readonly get => this._commentHandling;
      set => this._commentHandling = value <= JsonCommentHandling.Allow ? value : throw ThrowHelper.GetArgumentOutOfRangeException_CommentEnumMustBeInRange(nameof (value));
    }

    /// <summary>Gets or sets the maximum depth allowed when reading JSON, with the default (that is, 0) indicating a maximum depth of 64.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The maximum depth is being set to a negative value.</exception>
    /// <returns>The maximum depth allowed when reading JSON.</returns>
    public int MaxDepth
    {
      readonly get => this._maxDepth;
      set => this._maxDepth = value >= 0 ? value : throw ThrowHelper.GetArgumentOutOfRangeException_MaxDepthMustBePositive(nameof (value));
    }

    /// <summary>Gets or sets a value that defines whether an extra comma at the end of a list of JSON values in an object or array is allowed (and ignored) within the JSON payload being read.</summary>
    /// <returns>
    /// <see langword="true" /> if an extra comma is allowed; otherwise, <see langword="false" />.</returns>
    public bool AllowTrailingCommas { get; set; }
  }
}
