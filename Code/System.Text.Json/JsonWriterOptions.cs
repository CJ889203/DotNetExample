// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonWriterOptions
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Text.Encodings.Web;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Allows the user to define custom behavior when writing JSON using the <see cref="T:System.Text.Json.Utf8JsonWriter" />.</summary>
  public struct JsonWriterOptions
  {
    private int _optionsMask;

    /// <summary>Gets or sets the encoder to use when escaping strings, or <see langword="null" /> to use the default encoder.</summary>
    /// <returns>The JavaScript character encoder used to override the escaping behavior.</returns>
    public JavaScriptEncoder? Encoder { get; set; }

    /// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Text.Json.Utf8JsonWriter" /> should format the JSON output, which includes indenting nested JSON tokens, adding new lines, and adding white space between property names and values.</summary>
    /// <returns>
    /// <see langword="true" /> to format the JSON output; <see langword="false" /> to write without any extra white space. The default is <see langword="false" />.</returns>
    public bool Indented
    {
      get => (this._optionsMask & 1) != 0;
      set
      {
        if (value)
          this._optionsMask |= 1;
        else
          this._optionsMask &= -2;
      }
    }

    /// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Text.Json.Utf8JsonWriter" /> should skip structural validation and allow the user to write invalid JSON.</summary>
    /// <returns>
    /// <see langword="true" /> to skip structural validation and allow invalid JSON; <see langword="false" /> to throw an <see cref="T:System.InvalidOperationException" /> on any attempt to write invalid JSON.</returns>
    public bool SkipValidation
    {
      get => (this._optionsMask & 2) != 0;
      set
      {
        if (value)
          this._optionsMask |= 2;
        else
          this._optionsMask &= -3;
      }
    }

    internal bool IndentedOrNotSkipValidation => this._optionsMask != 2;
  }
}
