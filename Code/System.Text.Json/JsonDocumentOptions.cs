// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonDocumentOptions
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json
{
  /// <summary>Provides the ability for the user to define custom behavior when parsing JSON to create a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
  public struct JsonDocumentOptions
  {
    private int _maxDepth;
    private JsonCommentHandling _commentHandling;

    /// <summary>Gets or sets a value that determines how the <see cref="T:System.Text.Json.JsonDocument" /> handles comments when reading through the JSON data.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The comment handling enum is set to a value that is not supported (or not within the <see cref="T:System.Text.Json.JsonCommentHandling" /> enum range).</exception>
    /// <returns>One of the enumeration values that indicates how comments are handled.</returns>
    public JsonCommentHandling CommentHandling
    {
      readonly get => this._commentHandling;
      set => this._commentHandling = value <= JsonCommentHandling.Skip ? value : throw new ArgumentOutOfRangeException(nameof (value), SR.JsonDocumentDoesNotSupportComments);
    }

    /// <summary>Gets or sets the maximum depth allowed when parsing JSON data, with the default (that is, 0) indicating a maximum depth of 64.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The max depth is set to a negative value.</exception>
    /// <returns>The maximum depth allowed when parsing JSON data.</returns>
    public int MaxDepth
    {
      readonly get => this._maxDepth;
      set => this._maxDepth = value >= 0 ? value : throw ThrowHelper.GetArgumentOutOfRangeException_MaxDepthMustBePositive(nameof (value));
    }

    /// <summary>Gets or sets a value that indicates whether an extra comma at the end of a list of JSON values in an object or array is allowed (and ignored) within the JSON payload being read.</summary>
    /// <returns>
    /// <see langword="true" /> if an extra comma at the end of a list of JSON values in an object or array is allowed; otherwise, <see langword="false" />. Default is <see langword="false" /></returns>
    public bool AllowTrailingCommas { get; set; }

    internal JsonReaderOptions GetReaderOptions() => new JsonReaderOptions()
    {
      AllowTrailingCommas = this.AllowTrailingCommas,
      CommentHandling = this.CommentHandling,
      MaxDepth = this.MaxDepth
    };
  }
}
