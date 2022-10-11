// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonProperty
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml


#nullable enable
namespace System.Text.Json
{
  /// <summary>Represents a single property for a JSON object.</summary>
  [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
  public readonly struct JsonProperty
  {
    /// <summary>Gets the value of this property.</summary>
    /// <returns>The value of this property.</returns>
    public JsonElement Value { get; }

    private string? _name { get; }


    #nullable disable
    internal JsonProperty(JsonElement value, string name = null)
    {
      this.Value = value;
      this._name = name;
    }


    #nullable enable
    /// <summary>Gets the name of this property.</summary>
    /// <returns>The name of this property.</returns>
    public string Name => this._name ?? this.Value.GetPropertyName();

    /// <summary>Compares the specified string to the name of this property.</summary>
    /// <param name="text">The text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="T:System.Type" /> is not <see cref="F:System.Text.Json.JsonTokenType.PropertyName" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the name of this property matches <paramref name="text" />; otherwise <see langword="false" />.</returns>
    public bool NameEquals(string? text) => this.NameEquals(text.AsSpan());

    /// <summary>Compares the specified UTF-8 encoded text to the name of this property.</summary>
    /// <param name="utf8Text">The UTF-8 encoded text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="T:System.Type" /> is not <see cref="F:System.Text.Json.JsonTokenType.PropertyName" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the name of this property has the same UTF-8 encoding as <paramref name="utf8Text" />; otherwise, <see langword="false" />.</returns>
    public bool NameEquals(ReadOnlySpan<byte> utf8Text) => this.Value.TextEqualsHelper(utf8Text, true, true);

    /// <summary>Compares the specified text as a character span to the name of this property.</summary>
    /// <param name="text">The text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="T:System.Type" /> is not <see cref="F:System.Text.Json.JsonTokenType.PropertyName" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the name of this property matches <paramref name="text" />; otherwise, <see langword="false" />.</returns>
    public bool NameEquals(ReadOnlySpan<char> text) => this.Value.TextEqualsHelper(text, true);


    #nullable disable
    internal bool EscapedNameEquals(ReadOnlySpan<byte> utf8Text) => this.Value.TextEqualsHelper(utf8Text, true, false);


    #nullable enable
    /// <summary>Writes the property to the provided writer as a named JSON object property.</summary>
    /// <param name="writer">The writer to which to write the property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="writer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <see cref="P:System.Text.Json.JsonProperty.Name" /> is too large to be a JSON object property.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Text.Json.JsonElement.ValueKind" /> of this JSON property's <see cref="P:System.Text.Json.JsonProperty.Value" /> would result in invalid JSON.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    public void WriteTo(Utf8JsonWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WritePropertyName(this.Name);
      this.Value.WriteTo(writer);
    }

    /// <summary>Provides a string representation of the property for debugging purposes.</summary>
    /// <returns>A string containing the uninterpreted value of the property, beginning at the declaring open-quote and ending at the last character that is part of the value.</returns>
    public override string ToString() => this.Value.GetPropertyRawText();

    private string DebuggerDisplay => this.Value.ValueKind != JsonValueKind.Undefined ? "\"" + this.ToString() + "\"" : "<Undefined>";
  }
}
