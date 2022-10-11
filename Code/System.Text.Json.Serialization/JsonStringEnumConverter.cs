// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonStringEnumConverter
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Text.Json.Serialization.Converters;


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>Converts enumeration values to and from strings.</summary>
  public class JsonStringEnumConverter : JsonConverterFactory
  {

    #nullable disable
    private readonly JsonNamingPolicy _namingPolicy;
    private readonly EnumConverterOptions _converterOptions;

    /// <summary>Initializes an instance of the <see cref="T:System.Text.Json.Serialization.JsonStringEnumConverter" /> class with the default naming policy that allows integer values.</summary>
    public JsonStringEnumConverter()
      : this((JsonNamingPolicy) null, true)
    {
    }


    #nullable enable
    /// <summary>Initializes an instance of the <see cref="T:System.Text.Json.Serialization.JsonStringEnumConverter" /> class with a specified naming policy and a value that indicates whether undefined enumeration values are allowed.</summary>
    /// <param name="namingPolicy">The optional naming policy for writing enum values.</param>
    /// <param name="allowIntegerValues">
    /// <see langword="true" /> to allow undefined enum values; otherwise, <see langword="false" />. When <see langword="true" />, if an enum value isn't defined, it will output as a number rather than a string.</param>
    public JsonStringEnumConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true)
    {
      this._namingPolicy = namingPolicy;
      this._converterOptions = allowIntegerValues ? EnumConverterOptions.AllowStrings | EnumConverterOptions.AllowNumbers : EnumConverterOptions.AllowStrings;
    }

    /// <summary>Determines whether the specified type can be converted to an enum.</summary>
    /// <param name="typeToConvert">The type to be checked.</param>
    /// <returns>
    /// <see langword="true" /> if the type can be converted; otherwise, <see langword="false" />.</returns>
    public override sealed bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    /// <summary>Creates a converter for the specified type.</summary>
    /// <param name="typeToConvert">The type handled by the converter.</param>
    /// <param name="options">The serialization options to use.</param>
    /// <returns>A converter for which <typeparamref name="T" /> is compatible with <paramref name="typeToConvert" />.</returns>
    public override sealed JsonConverter CreateConverter(
      Type typeToConvert,
      JsonSerializerOptions options)
    {
      return EnumConverterFactory.Create(typeToConvert, this._converterOptions, this._namingPolicy, options);
    }
  }
}
