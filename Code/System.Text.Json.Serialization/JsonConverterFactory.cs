// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonConverterFactory
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Text.Json.Serialization.Metadata;


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>Supports converting several types by using a factory pattern.</summary>
  public abstract class JsonConverterFactory : JsonConverter
  {
    internal override sealed ConverterStrategy ConverterStrategy => ConverterStrategy.None;

    /// <summary>Creates a converter for a specified type.</summary>
    /// <param name="typeToConvert">The type handled by the converter.</param>
    /// <param name="options">The serialization options to use.</param>
    /// <returns>A converter for which <typeparamref name="T" /> is compatible with <paramref name="typeToConvert" />.</returns>
    public abstract JsonConverter? CreateConverter(
      Type typeToConvert,
      JsonSerializerOptions options);


    #nullable disable
    internal override JsonPropertyInfo CreateJsonPropertyInfo() => throw new InvalidOperationException();

    internal override JsonParameterInfo CreateJsonParameterInfo() => throw new InvalidOperationException();


    #nullable enable
    internal override sealed Type? KeyType => (Type) null;

    internal override sealed Type? ElementType => (Type) null;


    #nullable disable
    internal JsonConverter GetConverterInternal(
      Type typeToConvert,
      JsonSerializerOptions options)
    {
      JsonConverter converter = this.CreateConverter(typeToConvert, options);
      if (converter == null)
        ThrowHelper.ThrowInvalidOperationException_SerializerConverterFactoryReturnsNull(this.GetType());
      if (converter is JsonConverterFactory)
        ThrowHelper.ThrowInvalidOperationException_SerializerConverterFactoryReturnsJsonConverterFactorty(this.GetType());
      return converter;
    }

    internal override sealed object ReadCoreAsObject(
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state)
    {
      throw new InvalidOperationException();
    }

    internal override sealed bool TryReadAsObject(
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state,
      out object value)
    {
      throw new InvalidOperationException();
    }

    internal override sealed bool TryWriteAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      throw new InvalidOperationException();
    }


    #nullable enable
    internal override sealed Type TypeToConvert => (Type) null;


    #nullable disable
    internal override sealed bool WriteCoreAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      throw new InvalidOperationException();
    }

    internal override sealed void WriteAsPropertyNameCoreAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      bool isWritingExtensionDataProperty)
    {
      throw new InvalidOperationException();
    }
  }
}
