// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonConverter`1
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Metadata;


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>Converts an object or value to or from JSON.</summary>
  /// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
  public abstract class JsonConverter<T> : JsonConverter
  {

    #nullable disable
    internal override sealed object ReadCoreAsObject(
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state)
    {
      return (object) this.ReadCore(ref reader, options, ref state);
    }

    internal T ReadCore(
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state)
    {
      try
      {
        if (!state.IsContinuation)
        {
          if (!JsonConverter.SingleValueReadWithReadAhead(this.ConverterStrategy, ref reader, ref state))
          {
            if (state.SupportContinuation)
            {
              state.BytesConsumed += reader.BytesConsumed;
              return state.Current.ReturnValue == null ? default (T) : (T) state.Current.ReturnValue;
            }
            state.BytesConsumed += reader.BytesConsumed;
            return default (T);
          }
        }
        else if (!JsonConverter.SingleValueReadWithReadAhead(ConverterStrategy.Value, ref reader, ref state))
        {
          state.BytesConsumed += reader.BytesConsumed;
          return default (T);
        }
        JsonPropertyInfo propertyInfoForTypeInfo = state.Current.JsonTypeInfo.PropertyInfoForTypeInfo;
        T obj;
        if (this.TryRead(ref reader, propertyInfoForTypeInfo.RuntimePropertyType, options, ref state, out obj) && !reader.Read() && !reader.IsFinalBlock)
          state.Current.ReturnValue = (object) obj;
        state.BytesConsumed += reader.BytesConsumed;
        return obj;
      }
      catch (JsonReaderException ex)
      {
        ThrowHelper.ReThrowWithPath(ref state, ex);
        return default (T);
      }
      catch (FormatException ex) when (ex.Source == "System.Text.Json.Rethrowable")
      {
        ThrowHelper.ReThrowWithPath(ref state, in reader, (Exception) ex);
        return default (T);
      }
      catch (InvalidOperationException ex) when (ex.Source == "System.Text.Json.Rethrowable")
      {
        ThrowHelper.ReThrowWithPath(ref state, in reader, (Exception) ex);
        return default (T);
      }
      catch (JsonException ex) when (ex.Path == null)
      {
        ThrowHelper.AddJsonExceptionInformation(ref state, in reader, ex);
        throw;
      }
      catch (NotSupportedException ex)
      {
        if (ex.Message.Contains(" Path: "))
        {
          throw;
        }
        else
        {
          ThrowHelper.ThrowNotSupportedException(ref state, in reader, ex);
          return default (T);
        }
      }
    }

    internal override sealed bool WriteCoreAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      if (this.IsValueType)
      {
        if (value == null && Nullable.GetUnderlyingType(this.TypeToConvert) == (Type) null)
          ThrowHelper.ThrowJsonException_DeserializeUnableToConvertValue(this.TypeToConvert);
        if (options.ReferenceHandlingStrategy == ReferenceHandlingStrategy.IgnoreCycles && value != null)
          state.ReferenceResolver.PushReferenceForCycleDetection(value);
      }
      T obj = (T) value;
      return this.WriteCore(writer, in obj, options, ref state);
    }

    internal bool WriteCore(
      Utf8JsonWriter writer,
      in T value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      try
      {
        return this.TryWrite(writer, in value, options, ref state);
      }
      catch (InvalidOperationException ex) when (ex.Source == "System.Text.Json.Rethrowable")
      {
        ThrowHelper.ReThrowWithPath(ref state, (Exception) ex);
        throw;
      }
      catch (JsonException ex) when (ex.Path == null)
      {
        ThrowHelper.AddJsonExceptionInformation(ref state, ex);
        throw;
      }
      catch (NotSupportedException ex)
      {
        if (ex.Message.Contains(" Path: "))
        {
          throw;
        }
        else
        {
          ThrowHelper.ThrowNotSupportedException(ref state, ex);
          return false;
        }
      }
    }

    /// <summary>Initializes a new <see cref="T:System.Text.Json.Serialization.JsonConverter`1" /> instance.</summary>
    protected internal JsonConverter()
    {
      this.IsInternalConverter = this.GetType().Assembly == typeof (JsonConverter).Assembly;
      this.CanBePolymorphic = this.IsInternalConverter && this.TypeToConvert == JsonTypeInfo.ObjectType;
      this.IsValueType = this.TypeToConvert.IsValueType;
      this.CanBeNull = (object) default (T) == null;
      if (this.HandleNull)
      {
        this.HandleNullOnRead = true;
        this.HandleNullOnWrite = true;
      }
      this.CanUseDirectReadOrWrite = !this.CanBePolymorphic && this.IsInternalConverter && this.ConverterStrategy == ConverterStrategy.Value;
    }


    #nullable enable
    /// <summary>Determines whether the specified type can be converted.</summary>
    /// <param name="typeToConvert">The type to compare against.</param>
    /// <returns>
    /// <see langword="true" /> if the type can be converted; otherwise, <see langword="false" />.</returns>
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof (T);

    internal override ConverterStrategy ConverterStrategy => ConverterStrategy.Value;


    #nullable disable
    internal override sealed JsonPropertyInfo CreateJsonPropertyInfo() => (JsonPropertyInfo) new JsonPropertyInfo<T>();

    internal override sealed JsonParameterInfo CreateJsonParameterInfo() => (JsonParameterInfo) new JsonParameterInfo<T>();


    #nullable enable
    internal override Type? KeyType => (Type) null;

    internal override Type? ElementType => (Type) null;

    /// <summary>Indicates whether <see langword="null" /> should be passed to the converter on serialization, and whether <see cref="F:System.Text.Json.JsonTokenType.Null" /> should be passed on deserialization.</summary>
    public virtual bool HandleNull
    {
      get
      {
        this.HandleNullOnRead = !this.CanBeNull;
        this.HandleNullOnWrite = false;
        return false;
      }
    }

    internal bool HandleNullOnRead { get; private set; }

    internal bool HandleNullOnWrite { get; private set; }

    internal bool CanBeNull { get; }


    #nullable disable
    internal override sealed bool TryWriteAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      T obj = (T) value;
      return this.TryWrite(writer, in obj, options, ref state);
    }

    internal virtual bool OnTryWrite(
      Utf8JsonWriter writer,
      T value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      this.Write(writer, value, options);
      return true;
    }

    internal virtual bool OnTryRead(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options,
      ref ReadStack state,
      out T value)
    {
      value = this.Read(ref reader, typeToConvert, options);
      return true;
    }


    #nullable enable
    /// <summary>Reads and converts the JSON to type <typeparamref name="T" />.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public abstract T? Read(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options);


    #nullable disable
    internal bool TryRead(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options,
      ref ReadStack state,
      out T value)
    {
      if (this.ConverterStrategy == ConverterStrategy.Value)
      {
        if (reader.TokenType == JsonTokenType.Null && !this.HandleNullOnRead)
        {
          if (!this.CanBeNull)
            ThrowHelper.ThrowJsonException_DeserializeUnableToConvertValue(this.TypeToConvert);
          value = default (T);
          return true;
        }
        if (this.IsInternalConverter)
        {
          value = !state.Current.NumberHandling.HasValue || !this.IsInternalConverterForNumberType ? this.Read(ref reader, typeToConvert, options) : this.ReadNumberWithCustomHandling(ref reader, state.Current.NumberHandling.Value, options);
        }
        else
        {
          JsonTokenType tokenType = reader.TokenType;
          int currentDepth = reader.CurrentDepth;
          long bytesConsumed = reader.BytesConsumed;
          value = !state.Current.NumberHandling.HasValue || !this.IsInternalConverterForNumberType ? this.Read(ref reader, typeToConvert, options) : this.ReadNumberWithCustomHandling(ref reader, state.Current.NumberHandling.Value, options);
          this.VerifyRead(tokenType, currentDepth, bytesConsumed, true, ref reader);
        }
        object referenceValue;
        if (options.ReferenceHandlingStrategy == ReferenceHandlingStrategy.Preserve && this.CanBePolymorphic && value is JsonElement element && JsonSerializer.TryGetReferenceFromJsonElement(ref state, element, out referenceValue))
          value = (T) referenceValue;
        return true;
      }
      bool isContinuation = state.IsContinuation;
      state.Push();
      bool success;
      if (this.IsInternalConverter)
      {
        if (reader.TokenType == JsonTokenType.Null && !this.HandleNullOnRead && !isContinuation)
        {
          if (!this.CanBeNull)
            ThrowHelper.ThrowJsonException_DeserializeUnableToConvertValue(this.TypeToConvert);
          value = default (T);
          success = true;
        }
        else
          success = this.OnTryRead(ref reader, typeToConvert, options, ref state, out value);
      }
      else
      {
        if (!isContinuation)
        {
          if (reader.TokenType == JsonTokenType.Null && !this.HandleNullOnRead)
          {
            if (!this.CanBeNull)
              ThrowHelper.ThrowJsonException_DeserializeUnableToConvertValue(this.TypeToConvert);
            value = default (T);
            state.Pop(true);
            return true;
          }
          state.Current.OriginalTokenType = reader.TokenType;
          state.Current.OriginalDepth = reader.CurrentDepth;
        }
        success = this.OnTryRead(ref reader, typeToConvert, options, ref state, out value);
        if (success)
        {
          if (state.IsContinuation)
            ThrowHelper.ThrowJsonException_SerializationConverterRead((JsonConverter) this);
          this.VerifyRead(state.Current.OriginalTokenType, state.Current.OriginalDepth, 0L, false, ref reader);
        }
      }
      state.Pop(success);
      return success;
    }

    internal override sealed bool TryReadAsObject(
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state,
      out object value)
    {
      T obj;
      bool flag = this.TryRead(ref reader, this.TypeToConvert, options, ref state, out obj);
      value = (object) obj;
      return flag;
    }

    private static bool IsNull(T value) => (object) value == null;

    internal bool TryWrite(
      Utf8JsonWriter writer,
      in T value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      if (writer.CurrentDepth >= options.EffectiveMaxDepth)
        ThrowHelper.ThrowJsonException_SerializerCycleDetected(options.EffectiveMaxDepth);
      if ((object) default (T) == null && !this.HandleNullOnWrite && JsonConverter<T>.IsNull(value))
      {
        writer.WriteNullValue();
        return true;
      }
      bool flag1 = false;
      if (!typeof (T).IsValueType && (object) value != null)
      {
        if (options.ReferenceHandlingStrategy == ReferenceHandlingStrategy.IgnoreCycles && this.ConverterStrategy != ConverterStrategy.Value)
        {
          ReferenceResolver referenceResolver = state.ReferenceResolver;
          if (referenceResolver.ContainsReferenceForCycleDetection((object) value))
          {
            writer.WriteNullValue();
            return true;
          }
          referenceResolver.PushReferenceForCycleDetection((object) value);
          flag1 = true;
        }
        if (this.CanBePolymorphic)
        {
          Type type = value.GetType();
          if (type != this.TypeToConvert)
          {
            JsonConverter jsonConverter = state.Current.InitializeReEntry(type, options);
            if (jsonConverter.IsValueType)
            {
              switch (options.ReferenceHandlingStrategy)
              {
                case ReferenceHandlingStrategy.Preserve:
                  if (jsonConverter.CanHaveIdMetadata && !state.IsContinuation && JsonSerializer.TryWriteReferenceForBoxedStruct((object) value, ref state, writer))
                    return true;
                  break;
                case ReferenceHandlingStrategy.IgnoreCycles:
                  state.ReferenceResolver.PushReferenceForCycleDetection((object) value);
                  flag1 = true;
                  break;
              }
            }
            bool flag2 = jsonConverter.TryWriteAsObject(writer, (object) value, options, ref state);
            if (flag1)
              state.ReferenceResolver.PopReferenceForCycleDetection();
            return flag2;
          }
        }
      }
      if (this.ConverterStrategy == ConverterStrategy.Value)
      {
        int currentDepth = writer.CurrentDepth;
        if (state.Current.NumberHandling.HasValue && this.IsInternalConverterForNumberType)
          this.WriteNumberWithCustomHandling(writer, value, state.Current.NumberHandling.Value);
        else
          this.Write(writer, value, options);
        this.VerifyWrite(currentDepth, writer);
        if (!typeof (T).IsValueType & flag1)
          state.ReferenceResolver.PopReferenceForCycleDetection();
        return true;
      }
      bool isContinuation = state.IsContinuation;
      state.Push();
      if (!isContinuation)
        state.Current.OriginalDepth = writer.CurrentDepth;
      bool success = this.OnTryWrite(writer, value, options, ref state);
      if (success)
        this.VerifyWrite(state.Current.OriginalDepth, writer);
      state.Pop(success);
      if (flag1)
        state.ReferenceResolver.PopReferenceForCycleDetection();
      return success;
    }

    internal bool TryWriteDataExtensionProperty(
      Utf8JsonWriter writer,
      T value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      if (!this.IsInternalConverter)
        return this.TryWrite(writer, in value, options, ref state);
      JsonConverter<T> jsonConverter;
      switch (this)
      {
        case JsonDictionaryConverter<T> dictionaryConverter2:
label_6:
          JsonDictionaryConverter<T> dictionaryConverter1 = dictionaryConverter2;
          if (dictionaryConverter1 == null)
            return this.TryWrite(writer, in value, options, ref state);
          if (writer.CurrentDepth >= options.EffectiveMaxDepth)
            ThrowHelper.ThrowJsonException_SerializerCycleDetected(options.EffectiveMaxDepth);
          bool isContinuation = state.IsContinuation;
          state.Push();
          if (!isContinuation)
            state.Current.OriginalDepth = writer.CurrentDepth;
          state.Current.IsWritingExtensionDataProperty = true;
          state.Current.DeclaredJsonPropertyInfo = state.Current.JsonTypeInfo.ElementTypeInfo.PropertyInfoForTypeInfo;
          bool success = dictionaryConverter1.OnWriteResume(writer, value, options, ref state);
          if (success)
            this.VerifyWrite(state.Current.OriginalDepth, writer);
          state.Pop(success);
          return success;
        case JsonMetadataServicesConverter<T> servicesConverter:
          jsonConverter = servicesConverter.Converter;
          break;
        default:
          jsonConverter = (JsonConverter<T>) null;
          break;
      }
      dictionaryConverter2 = jsonConverter as JsonDictionaryConverter<T>;
      goto label_6;
    }


    #nullable enable
    internal override sealed Type TypeToConvert => typeof (T);


    #nullable disable
    internal void VerifyRead(
      JsonTokenType tokenType,
      int depth,
      long bytesConsumed,
      bool isValueConverter,
      ref Utf8JsonReader reader)
    {
      switch (tokenType)
      {
        case JsonTokenType.StartObject:
          if (reader.TokenType != JsonTokenType.EndObject)
          {
            ThrowHelper.ThrowJsonException_SerializationConverterRead((JsonConverter) this);
            break;
          }
          if (depth == reader.CurrentDepth)
            break;
          ThrowHelper.ThrowJsonException_SerializationConverterRead((JsonConverter) this);
          break;
        case JsonTokenType.StartArray:
          if (reader.TokenType != JsonTokenType.EndArray)
          {
            ThrowHelper.ThrowJsonException_SerializationConverterRead((JsonConverter) this);
            break;
          }
          if (depth == reader.CurrentDepth)
            break;
          ThrowHelper.ThrowJsonException_SerializationConverterRead((JsonConverter) this);
          break;
        default:
          if (!isValueConverter)
          {
            if (this.HandleNullOnRead && tokenType == JsonTokenType.Null)
              break;
            ThrowHelper.ThrowJsonException_SerializationConverterRead((JsonConverter) this);
            break;
          }
          if (reader.BytesConsumed == bytesConsumed)
            break;
          ThrowHelper.ThrowJsonException_SerializationConverterRead((JsonConverter) this);
          break;
      }
    }

    internal void VerifyWrite(int originalDepth, Utf8JsonWriter writer)
    {
      if (originalDepth == writer.CurrentDepth)
        return;
      ThrowHelper.ThrowJsonException_SerializationConverterWrite((JsonConverter) this);
    }


    #nullable enable
    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public abstract void Write(Utf8JsonWriter writer, 
    #nullable disable
    T value, 
    #nullable enable
    JsonSerializerOptions options);

    /// <summary>Reads a dictionary key from a JSON property name.</summary>
    /// <param name="reader">The <see cref="T:System.Text.Json.Utf8JsonReader" /> to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The options to use when reading the value.</param>
    /// <returns>The value that was converted.</returns>
    public virtual T ReadAsPropertyName(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options)
    {
      JsonConverter converter;
      if (!this.IsInternalConverter && options.TryGetDefaultSimpleConverter(this.TypeToConvert, out converter))
        return ((JsonConverter<T>) converter).ReadAsPropertyNameCore(ref reader, this.TypeToConvert, options);
      ThrowHelper.ThrowNotSupportedException_DictionaryKeyTypeNotSupported(this.TypeToConvert, (JsonConverter) this);
      return default (T);
    }


    #nullable disable
    internal virtual T ReadAsPropertyNameCore(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options)
    {
      long bytesConsumed = reader.BytesConsumed;
      T obj = this.ReadAsPropertyName(ref reader, typeToConvert, options);
      if (reader.BytesConsumed != bytesConsumed)
        ThrowHelper.ThrowJsonException_SerializationConverterRead((JsonConverter) this);
      return obj;
    }


    #nullable enable
    /// <summary>Writes a dictionary key as a JSON property name.</summary>
    /// <param name="writer">The <see cref="T:System.Text.Json.Utf8JsonWriter" /> to write to.</param>
    /// <param name="value">The value to convert. The value of <see cref="P:System.Text.Json.Serialization.JsonConverter`1.HandleNull" /> determines if the converter handles <see langword="null" /> values.</param>
    /// <param name="options">The options to use when writing the value.</param>
    public virtual void WriteAsPropertyName(
      Utf8JsonWriter writer,
      T value,
      JsonSerializerOptions options)
    {
      JsonConverter converter;
      if (!this.IsInternalConverter && options.TryGetDefaultSimpleConverter(this.TypeToConvert, out converter))
        ((JsonConverter<T>) converter).WriteAsPropertyNameCore(writer, value, options, false);
      else
        ThrowHelper.ThrowNotSupportedException_DictionaryKeyTypeNotSupported(this.TypeToConvert, (JsonConverter) this);
    }


    #nullable disable
    internal virtual void WriteAsPropertyNameCore(
      Utf8JsonWriter writer,
      T value,
      JsonSerializerOptions options,
      bool isWritingExtensionDataProperty)
    {
      if (isWritingExtensionDataProperty)
      {
        writer.WritePropertyName((string) (object) value);
      }
      else
      {
        int currentDepth = writer.CurrentDepth;
        this.WriteAsPropertyName(writer, value, options);
        if (currentDepth == writer.CurrentDepth && writer.TokenType == JsonTokenType.PropertyName)
          return;
        ThrowHelper.ThrowJsonException_SerializationConverterWrite((JsonConverter) this);
      }
    }

    internal override sealed void WriteAsPropertyNameCoreAsObject(
      Utf8JsonWriter writer,
      object value,
      JsonSerializerOptions options,
      bool isWritingExtensionDataProperty)
    {
      this.WriteAsPropertyNameCore(writer, (T) value, options, isWritingExtensionDataProperty);
    }

    internal virtual T ReadNumberWithCustomHandling(
      ref Utf8JsonReader reader,
      JsonNumberHandling handling,
      JsonSerializerOptions options)
    {
      throw new InvalidOperationException();
    }

    internal virtual void WriteNumberWithCustomHandling(
      Utf8JsonWriter writer,
      T value,
      JsonNumberHandling handling)
    {
      throw new InvalidOperationException();
    }
  }
}
