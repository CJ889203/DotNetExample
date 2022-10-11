// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonConverter
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Metadata;


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>Converts an object or value to or from JSON.</summary>
  public abstract class JsonConverter
  {
    internal bool IsInternalConverterForNumberType;

    internal JsonConverter()
    {
    }

    /// <summary>When overridden in a derived class, determines whether the converter instance can convert the specified object type.</summary>
    /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
    /// <returns>
    /// <see langword="true" /> if the instance can convert the specified object type; otherwise, <see langword="false" />.</returns>
    public abstract bool CanConvert(Type typeToConvert);

    internal abstract ConverterStrategy ConverterStrategy { get; }

    internal bool CanUseDirectReadOrWrite { get; set; }

    internal virtual bool CanHaveIdMetadata => false;

    internal bool CanBePolymorphic { get; set; }


    #nullable disable
    internal virtual object CreateObject(JsonSerializerOptions options) => throw new InvalidOperationException(SR.NodeJsonObjectCustomConverterNotAllowedOnExtensionProperty);

    internal virtual void ReadElementAndSetProperty(
      object obj,
      string propertyName,
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state)
    {
      throw new InvalidOperationException(SR.NodeJsonObjectCustomConverterNotAllowedOnExtensionProperty);
    }

    internal abstract JsonPropertyInfo CreateJsonPropertyInfo();

    internal abstract JsonParameterInfo CreateJsonParameterInfo();


    #nullable enable
    internal abstract Type? ElementType { get; }

    internal abstract Type? KeyType { get; }

    internal bool IsValueType { get; set; }

    internal bool IsInternalConverter { get; set; }


    #nullable disable
    internal abstract object ReadCoreAsObject(
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state);


    #nullable enable
    internal virtual Type RuntimeType => this.TypeToConvert;


    #nullable disable
    internal bool ShouldFlush(Utf8JsonWriter writer, ref WriteStack state) => state.FlushThreshold > 0 && writer.BytesPending > state.FlushThreshold;


    #nullable enable
    internal abstract Type TypeToConvert { get; }


    #nullable disable
    internal abstract bool TryReadAsObject(
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state,
      out object value);

    internal abstract bool TryWriteAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      ref WriteStack state);

    internal abstract bool WriteCoreAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      ref WriteStack state);

    internal abstract void WriteAsPropertyNameCoreAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      bool isWritingExtensionDataProperty);

    internal virtual bool ConstructorIsParameterized { get; }


    #nullable enable
    internal ConstructorInfo? ConstructorInfo { get; set; }

    internal virtual bool RequiresDynamicMemberAccessors { get; }


    #nullable disable
    internal virtual void Initialize(JsonSerializerOptions options, JsonTypeInfo jsonTypeInfo = null)
    {
    }

    internal virtual void CreateInstanceForReferenceResolver(
      ref Utf8JsonReader reader,
      ref ReadStack state,
      JsonSerializerOptions options)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool SingleValueReadWithReadAhead(
      ConverterStrategy converterStrategy,
      ref Utf8JsonReader reader,
      ref ReadStack state)
    {
      return !state.ReadAhead || converterStrategy != ConverterStrategy.Value ? reader.Read() : JsonConverter.DoSingleValueReadWithReadAhead(ref reader, ref state);
    }

    internal static bool DoSingleValueReadWithReadAhead(
      ref Utf8JsonReader reader,
      ref ReadStack state)
    {
      JsonReaderState currentState = reader.CurrentState;
      long bytesConsumed = reader.BytesConsumed;
      if (!reader.Read())
        return false;
      switch (reader.TokenType)
      {
        case JsonTokenType.StartObject:
        case JsonTokenType.StartArray:
          bool flag = reader.TrySkip();
          reader = new Utf8JsonReader(reader.OriginalSpan.Slice(checked ((int) bytesConsumed)), reader.IsFinalBlock, currentState);
          state.BytesConsumed += bytesConsumed;
          if (!flag)
            return false;
          reader.ReadWithVerify();
          break;
      }
      return true;
    }
  }
}
