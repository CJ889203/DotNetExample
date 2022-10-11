// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonSerializerOptions
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;
using System.Text.Json.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Metadata;
using System.Threading;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Provides options to be used with <see cref="T:System.Text.Json.JsonSerializer" />.</summary>
  public sealed class JsonSerializerOptions
  {

    #nullable disable
    private static Dictionary<Type, JsonConverter> s_defaultSimpleConverters;
    private static JsonConverter[] s_defaultFactoryConverters;
    private readonly ConcurrentDictionary<Type, JsonConverter> _converters = new ConcurrentDictionary<Type, JsonConverter>();
    internal static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions();
    private readonly ConcurrentDictionary<Type, JsonTypeInfo> _classes = new ConcurrentDictionary<Type, JsonTypeInfo>();
    internal JsonSerializerContext _context;
    private Func<Type, JsonSerializerOptions, JsonTypeInfo> _typeInfoCreationFunc;
    private MemberAccessor _memberAccessorStrategy;
    private JsonNamingPolicy _dictionaryKeyPolicy;
    private JsonNamingPolicy _jsonPropertyNamingPolicy;
    private JsonCommentHandling _readCommentHandling;
    private ReferenceHandler _referenceHandler;
    private JavaScriptEncoder _encoder;
    private JsonIgnoreCondition _defaultIgnoreCondition;
    private JsonNumberHandling _numberHandling;
    private JsonUnknownTypeHandling _unknownTypeHandling;
    private int _defaultBufferSize = 16384;
    private int _maxDepth;
    private bool _allowTrailingCommas;
    private bool _haveTypesBeenCreated;
    private bool _ignoreNullValues;
    private bool _ignoreReadOnlyProperties;
    private bool _ignoreReadonlyFields;
    private bool _includeFields;
    private bool _propertyNameCaseInsensitive;
    private bool _writeIndented;
    internal ReferenceHandlingStrategy ReferenceHandlingStrategy;

    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    private void RootBuiltInConverters()
    {
      if (Volatile.Read<JsonConverter[]>(ref JsonSerializerOptions.s_defaultFactoryConverters) != null)
        return;
      JsonSerializerOptions.s_defaultSimpleConverters = JsonSerializerOptions.GetDefaultSimpleConverters();
      Volatile.Write<JsonConverter[]>(ref JsonSerializerOptions.s_defaultFactoryConverters, new JsonConverter[8]
      {
        (JsonConverter) new UnsupportedTypeConverterFactory(),
        (JsonConverter) new NullableConverterFactory(),
        (JsonConverter) new EnumConverterFactory(),
        (JsonConverter) new JsonNodeConverterFactory(),
        (JsonConverter) new FSharpTypeConverterFactory(),
        (JsonConverter) new IAsyncEnumerableConverterFactory(),
        (JsonConverter) new IEnumerableConverterFactory(),
        (JsonConverter) new ObjectConverterFactory()
      });
    }

    private static Dictionary<Type, JsonConverter> GetDefaultSimpleConverters()
    {
      Dictionary<Type, JsonConverter> converters = new Dictionary<Type, JsonConverter>(24);
      Add((JsonConverter) JsonMetadataServices.BooleanConverter);
      Add((JsonConverter) JsonMetadataServices.ByteConverter);
      Add((JsonConverter) JsonMetadataServices.ByteArrayConverter);
      Add((JsonConverter) JsonMetadataServices.CharConverter);
      Add((JsonConverter) JsonMetadataServices.DateTimeConverter);
      Add((JsonConverter) JsonMetadataServices.DateTimeOffsetConverter);
      Add((JsonConverter) JsonMetadataServices.DoubleConverter);
      Add((JsonConverter) JsonMetadataServices.DecimalConverter);
      Add((JsonConverter) JsonMetadataServices.GuidConverter);
      Add((JsonConverter) JsonMetadataServices.Int16Converter);
      Add((JsonConverter) JsonMetadataServices.Int32Converter);
      Add((JsonConverter) JsonMetadataServices.Int64Converter);
      Add((JsonConverter) new JsonElementConverter());
      Add((JsonConverter) new JsonDocumentConverter());
      Add((JsonConverter) JsonMetadataServices.ObjectConverter);
      Add((JsonConverter) JsonMetadataServices.SByteConverter);
      Add((JsonConverter) JsonMetadataServices.SingleConverter);
      Add((JsonConverter) JsonMetadataServices.StringConverter);
      Add((JsonConverter) JsonMetadataServices.TimeSpanConverter);
      Add((JsonConverter) JsonMetadataServices.UInt16Converter);
      Add((JsonConverter) JsonMetadataServices.UInt32Converter);
      Add((JsonConverter) JsonMetadataServices.UInt64Converter);
      Add((JsonConverter) JsonMetadataServices.UriConverter);
      Add((JsonConverter) JsonMetadataServices.VersionConverter);
      return converters;

      void Add(JsonConverter converter) => converters.Add(converter.TypeToConvert, converter);
    }


    #nullable enable
    /// <summary>Gets the list of user-defined converters that were registered.</summary>
    /// <returns>The list of custom converters.</returns>
    public IList<JsonConverter> Converters { get; }


    #nullable disable
    internal JsonConverter DetermineConverter(
      Type parentClassType,
      Type runtimePropertyType,
      MemberInfo memberInfo)
    {
      JsonConverter jsonConverter = (JsonConverter) null;
      if (memberInfo != (MemberInfo) null)
      {
        JsonConverterAttribute thatCanHaveMultiple = (JsonConverterAttribute) JsonSerializerOptions.GetAttributeThatCanHaveMultiple(parentClassType, typeof (JsonConverterAttribute), memberInfo);
        if (thatCanHaveMultiple != null)
          jsonConverter = this.GetConverterFromAttribute(thatCanHaveMultiple, runtimePropertyType, parentClassType, memberInfo);
      }
      if (jsonConverter == null)
        jsonConverter = this.GetConverterInternal(runtimePropertyType);
      if (jsonConverter is JsonConverterFactory converterFactory)
        jsonConverter = converterFactory.GetConverterInternal(runtimePropertyType, this);
      if (runtimePropertyType.IsValueType && jsonConverter.IsValueType && runtimePropertyType.IsNullableOfT() ^ jsonConverter.TypeToConvert.IsNullableOfT())
        ThrowHelper.ThrowInvalidOperationException_ConverterCanConvertMultipleTypes(runtimePropertyType, jsonConverter);
      return jsonConverter;
    }


    #nullable enable
    /// <summary>Returns the converter for the specified type.</summary>
    /// <param name="typeToConvert">The type to return a converter for.</param>
    /// <exception cref="T:System.InvalidOperationException">The configured <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="typeToConvert" /> returned an invalid converter.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="typeToConvert" /> or its serializable members.</exception>
    /// <returns>The first converter that supports the given type, or <see langword="null" /> if there is no converter.</returns>
    [RequiresUnreferencedCode("Getting a converter for a type may require reflection which depends on unreferenced code.")]
    public JsonConverter GetConverter(Type typeToConvert)
    {
      if (typeToConvert == (Type) null)
        throw new ArgumentNullException(nameof (typeToConvert));
      this.RootBuiltInConverters();
      return this.GetConverterInternal(typeToConvert);
    }


    #nullable disable
    internal JsonConverter GetConverterInternal(Type typeToConvert)
    {
      JsonConverter converterInternal1;
      if (this._converters.TryGetValue(typeToConvert, out converterInternal1))
        return converterInternal1;
      JsonConverter converterInternal2 = this._context?.GetTypeInfo(typeToConvert)?.PropertyInfoForTypeInfo?.ConverterBase;
      foreach (JsonConverter converter in (IEnumerable<JsonConverter>) this.Converters)
      {
        if (converter.CanConvert(typeToConvert))
        {
          converterInternal2 = converter;
          break;
        }
      }
      if (converterInternal2 == null)
      {
        JsonConverterAttribute thatCanHaveMultiple = (JsonConverterAttribute) JsonSerializerOptions.GetAttributeThatCanHaveMultiple(typeToConvert, typeof (JsonConverterAttribute));
        if (thatCanHaveMultiple != null)
          converterInternal2 = this.GetConverterFromAttribute(thatCanHaveMultiple, typeToConvert, typeToConvert, (MemberInfo) null);
      }
      if (converterInternal2 == null)
      {
        if (JsonSerializerOptions.s_defaultSimpleConverters == null || JsonSerializerOptions.s_defaultFactoryConverters == null)
        {
          ThrowHelper.ThrowNotSupportedException_BuiltInConvertersNotRooted(typeToConvert);
          return (JsonConverter) null;
        }
        JsonConverter jsonConverter;
        if (JsonSerializerOptions.s_defaultSimpleConverters.TryGetValue(typeToConvert, out jsonConverter))
        {
          converterInternal2 = jsonConverter;
        }
        else
        {
          foreach (JsonConverter factoryConverter in JsonSerializerOptions.s_defaultFactoryConverters)
          {
            if (factoryConverter.CanConvert(typeToConvert))
            {
              converterInternal2 = factoryConverter;
              break;
            }
          }
        }
      }
      if (converterInternal2 is JsonConverterFactory converterFactory)
        converterInternal2 = converterFactory.GetConverterInternal(typeToConvert, this);
      Type typeToConvert1 = converterInternal2.TypeToConvert;
      if (!typeToConvert1.IsAssignableFromInternal(typeToConvert) && !typeToConvert.IsAssignableFromInternal(typeToConvert1))
        ThrowHelper.ThrowInvalidOperationException_SerializationConverterNotCompatible(converterInternal2.GetType(), typeToConvert);
      if (this._haveTypesBeenCreated)
        this._converters.TryAdd(typeToConvert, converterInternal2);
      return converterInternal2;
    }

    private JsonConverter GetConverterFromAttribute(
      JsonConverterAttribute converterAttribute,
      Type typeToConvert,
      Type classTypeAttributeIsOn,
      MemberInfo memberInfo)
    {
      Type converterType = converterAttribute.ConverterType;
      JsonConverter valueConverter;
      if (converterType == (Type) null)
      {
        valueConverter = converterAttribute.CreateConverter(typeToConvert);
        if (valueConverter == null)
          ThrowHelper.ThrowInvalidOperationException_SerializationConverterOnAttributeNotCompatible(classTypeAttributeIsOn, memberInfo, typeToConvert);
      }
      else
      {
        ConstructorInfo constructor = converterType.GetConstructor(Type.EmptyTypes);
        if (!typeof (JsonConverter).IsAssignableFrom(converterType) || constructor == (ConstructorInfo) null || !constructor.IsPublic)
          ThrowHelper.ThrowInvalidOperationException_SerializationConverterOnAttributeInvalid(classTypeAttributeIsOn, memberInfo);
        valueConverter = (JsonConverter) Activator.CreateInstance(converterType);
      }
      if (!valueConverter.CanConvert(typeToConvert))
      {
        Type underlyingType = Nullable.GetUnderlyingType(typeToConvert);
        if (underlyingType != (Type) null && valueConverter.CanConvert(underlyingType))
        {
          if (valueConverter is JsonConverterFactory converterFactory)
            valueConverter = converterFactory.GetConverterInternal(underlyingType, this);
          return NullableConverterFactory.CreateValueConverter(underlyingType, valueConverter);
        }
        ThrowHelper.ThrowInvalidOperationException_SerializationConverterOnAttributeNotCompatible(classTypeAttributeIsOn, memberInfo, typeToConvert);
      }
      return valueConverter;
    }

    internal bool TryGetDefaultSimpleConverter(Type typeToConvert, [NotNullWhen(true)] out JsonConverter converter)
    {
      if (this._context == null && JsonSerializerOptions.s_defaultSimpleConverters != null && JsonSerializerOptions.s_defaultSimpleConverters.TryGetValue(typeToConvert, out converter))
        return true;
      converter = (JsonConverter) null;
      return false;
    }

    private static Attribute GetAttributeThatCanHaveMultiple(
      Type classType,
      Type attributeType,
      MemberInfo memberInfo)
    {
      object[] customAttributes = memberInfo.GetCustomAttributes(attributeType, false);
      return JsonSerializerOptions.GetAttributeThatCanHaveMultiple(attributeType, classType, memberInfo, customAttributes);
    }

    internal static Attribute GetAttributeThatCanHaveMultiple(
      Type classType,
      Type attributeType)
    {
      object[] customAttributes = classType.GetCustomAttributes(attributeType, false);
      return JsonSerializerOptions.GetAttributeThatCanHaveMultiple(attributeType, classType, (MemberInfo) null, customAttributes);
    }

    private static Attribute GetAttributeThatCanHaveMultiple(
      Type attributeType,
      Type classType,
      MemberInfo memberInfo,
      object[] attributes)
    {
      if (attributes.Length == 0)
        return (Attribute) null;
      if (attributes.Length == 1)
        return (Attribute) attributes[0];
      ThrowHelper.ThrowInvalidOperationException_SerializationDuplicateAttribute(attributeType, classType, memberInfo);
      return (Attribute) null;
    }


    #nullable enable
    private JsonTypeInfo? _lastClass { get; set; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.JsonSerializerOptions" /> class.</summary>
    public JsonSerializerOptions()
    {
      this.Converters = (IList<JsonConverter>) new ConverterList(this);
      JsonSerializerOptions.TrackOptionsInstance(this);
    }

    /// <summary>Copies the options from a <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance to a new instance.</summary>
    /// <param name="options">The options instance to copy options from.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="options" /> is <see langword="null" />.</exception>
    public JsonSerializerOptions(JsonSerializerOptions options)
    {
      this._memberAccessorStrategy = options != null ? options._memberAccessorStrategy : throw new ArgumentNullException(nameof (options));
      this._dictionaryKeyPolicy = options._dictionaryKeyPolicy;
      this._jsonPropertyNamingPolicy = options._jsonPropertyNamingPolicy;
      this._readCommentHandling = options._readCommentHandling;
      this._referenceHandler = options._referenceHandler;
      this._encoder = options._encoder;
      this._defaultIgnoreCondition = options._defaultIgnoreCondition;
      this._numberHandling = options._numberHandling;
      this._unknownTypeHandling = options._unknownTypeHandling;
      this._defaultBufferSize = options._defaultBufferSize;
      this._maxDepth = options._maxDepth;
      this._allowTrailingCommas = options._allowTrailingCommas;
      this._ignoreNullValues = options._ignoreNullValues;
      this._ignoreReadOnlyProperties = options._ignoreReadOnlyProperties;
      this._ignoreReadonlyFields = options._ignoreReadonlyFields;
      this._includeFields = options._includeFields;
      this._propertyNameCaseInsensitive = options._propertyNameCaseInsensitive;
      this._writeIndented = options._writeIndented;
      this.Converters = (IList<JsonConverter>) new ConverterList(this, (ConverterList) options.Converters);
      this.EffectiveMaxDepth = options.EffectiveMaxDepth;
      this.ReferenceHandlingStrategy = options.ReferenceHandlingStrategy;
      JsonSerializerOptions.TrackOptionsInstance(this);
    }


    #nullable disable
    private static void TrackOptionsInstance(JsonSerializerOptions options) => JsonSerializerOptions.TrackedOptionsInstances.All.Add(options, (object) null);

    /// <summary>Constructs a new <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance with a predefined set of options determined by the specified <see cref="T:System.Text.Json.JsonSerializerDefaults" />.</summary>
    /// <param name="defaults">The <see cref="T:System.Text.Json.JsonSerializerDefaults" /> to reason about.</param>
    public JsonSerializerOptions(JsonSerializerDefaults defaults)
      : this()
    {
      if (defaults == JsonSerializerDefaults.Web)
      {
        this._propertyNameCaseInsensitive = true;
        this._jsonPropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        this._numberHandling = JsonNumberHandling.AllowReadingFromString;
      }
      else if (defaults != JsonSerializerDefaults.General)
        throw new ArgumentOutOfRangeException(nameof (defaults));
    }


    #nullable enable
    /// <summary>Binds current <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance with a new instance of the specified <see cref="T:System.Text.Json.Serialization.JsonSerializerContext" /> type.</summary>
    /// <typeparam name="TContext">The generic definition of the specified context type.</typeparam>
    public void AddContext<TContext>() where TContext : JsonSerializerContext, new()
    {
      if (this._context != null)
        ThrowHelper.ThrowInvalidOperationException_JsonSerializerOptionsAlreadyBoundToContext();
      TContext context = new TContext();
      this._context = (JsonSerializerContext) context;
      context._options = this;
    }

    /// <summary>Get or sets a value that indicates whether an extra comma at the end of a list of JSON values in an object or array is allowed (and ignored) within the JSON payload being deserialized.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property was set after serialization or deserialization has occurred.</exception>
    /// <returns>
    /// <see langword="true" /> if an extra comma at the end of a list of JSON values in an object or array is allowed (and ignored); <see langword="false" /> otherwise.</returns>
    public bool AllowTrailingCommas
    {
      get => this._allowTrailingCommas;
      set
      {
        this.VerifyMutable();
        this._allowTrailingCommas = value;
      }
    }

    /// <summary>Gets or sets the default buffer size, in bytes, to use when creating temporary buffers.</summary>
    /// <exception cref="T:System.ArgumentException">The buffer size is less than 1.</exception>
    /// <exception cref="T:System.InvalidOperationException">This property was set after serialization or deserialization has occurred.</exception>
    /// <returns>The default buffer size in bytes.</returns>
    public int DefaultBufferSize
    {
      get => this._defaultBufferSize;
      set
      {
        this.VerifyMutable();
        this._defaultBufferSize = value >= 1 ? value : throw new ArgumentException(SR.SerializationInvalidBufferSize);
      }
    }

    /// <summary>Gets or sets the encoder to use when escaping strings, or <see langword="null" /> to use the default encoder.</summary>
    /// <returns>The JavaScript character encoding.</returns>
    public JavaScriptEncoder? Encoder
    {
      get => this._encoder;
      set
      {
        this.VerifyMutable();
        this._encoder = value;
      }
    }

    /// <summary>Gets or sets the policy used to convert a <see cref="T:System.Collections.IDictionary" /> key's name to another format, such as camel-casing.</summary>
    /// <returns>The policy used to convert a <see cref="T:System.Collections.IDictionary" /> key's name to another format.</returns>
    public JsonNamingPolicy? DictionaryKeyPolicy
    {
      get => this._dictionaryKeyPolicy;
      set
      {
        this.VerifyMutable();
        this._dictionaryKeyPolicy = value;
      }
    }

    /// <summary>Gets or sets a value that determines whether <see langword="null" /> values are ignored during serialization and deserialization. The default value is <see langword="false" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property was set after serialization or deserialization has occurred.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Text.Json.JsonSerializerOptions.DefaultIgnoreCondition" /> has been set to a non-default value. These properties cannot be used together.</exception>
    /// <returns>
    /// <see langword="true" /> to ignore null values during serialization and deserialization; otherwise, <see langword="false" />.</returns>
    [Obsolete("JsonSerializerOptions.IgnoreNullValues is obsolete. To ignore null values when serializing, set DefaultIgnoreCondition to JsonIgnoreCondition.WhenWritingNull.", DiagnosticId = "SYSLIB0020", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool IgnoreNullValues
    {
      get => this._ignoreNullValues;
      set
      {
        this.VerifyMutable();
        this._ignoreNullValues = !value || this._defaultIgnoreCondition == JsonIgnoreCondition.Never ? value : throw new InvalidOperationException(SR.DefaultIgnoreConditionAlreadySpecified);
      }
    }

    /// <summary>Specifies a condition to determine when properties with default values are ignored during serialization or deserialization.
    /// The default value is <see cref="F:System.Text.Json.Serialization.JsonIgnoreCondition.Never" />.</summary>
    /// <exception cref="T:System.ArgumentException">This property is set to <see cref="F:System.Text.Json.Serialization.JsonIgnoreCondition.Always" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">This property is set after serialization or deserialization has occurred.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Text.Json.JsonSerializerOptions.IgnoreNullValues" /> has been set to <see langword="true" />. These properties cannot be used together.</exception>
    public JsonIgnoreCondition DefaultIgnoreCondition
    {
      get => this._defaultIgnoreCondition;
      set
      {
        this.VerifyMutable();
        if (value == JsonIgnoreCondition.Always)
          throw new ArgumentException(SR.DefaultIgnoreConditionInvalid);
        this._defaultIgnoreCondition = value == JsonIgnoreCondition.Never || !this._ignoreNullValues ? value : throw new InvalidOperationException(SR.DefaultIgnoreConditionAlreadySpecified);
      }
    }

    /// <summary>Specifies how number types should be handled when serializing or deserializing.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    public JsonNumberHandling NumberHandling
    {
      get => this._numberHandling;
      set
      {
        this.VerifyMutable();
        this._numberHandling = JsonSerializer.IsValidNumberHandlingValue(value) ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    /// <summary>Gets a value that determines whether read-only properties are ignored during serialization. The default value is <see langword="false" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property was set after serialization or deserialization has occurred.</exception>
    /// <returns>
    /// <see langword="true" /> to ignore read-only properties during serialization; otherwise, <see langword="false" />.</returns>
    public bool IgnoreReadOnlyProperties
    {
      get => this._ignoreReadOnlyProperties;
      set
      {
        this.VerifyMutable();
        this._ignoreReadOnlyProperties = value;
      }
    }

    /// <summary>Determines whether read-only fields are ignored during serialization. A field is read-only if it is marked with the <see langword="readonly" /> keyword. The default value is <see langword="false" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    /// <returns>
    /// <see langword="true" /> if read-only fields should be ignored during serialization; <see langword="false" /> otherwise.</returns>
    public bool IgnoreReadOnlyFields
    {
      get => this._ignoreReadonlyFields;
      set
      {
        this.VerifyMutable();
        this._ignoreReadonlyFields = value;
      }
    }

    /// <summary>Determines whether fields are handled during serialization and deserialization.
    /// The default value is <see langword="false" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property is set after serialization or deserialization has occurred.</exception>
    public bool IncludeFields
    {
      get => this._includeFields;
      set
      {
        this.VerifyMutable();
        this._includeFields = value;
      }
    }

    /// <summary>Gets or sets the maximum depth allowed when serializing or deserializing JSON, with the default value of 0 indicating a maximum depth of 64.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property was set after serialization or deserialization has occurred.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The max depth is set to a negative value.</exception>
    /// <returns>The maximum depth allowed when serializing or deserializing JSON.</returns>
    public int MaxDepth
    {
      get => this._maxDepth;
      set
      {
        this.VerifyMutable();
        this._maxDepth = value >= 0 ? value : throw ThrowHelper.GetArgumentOutOfRangeException_MaxDepthMustBePositive(nameof (value));
        this.EffectiveMaxDepth = value == 0 ? 64 : value;
      }
    }

    internal int EffectiveMaxDepth { get; private set; } = 64;

    /// <summary>Gets or sets a value that specifies the policy used to convert a property's name on an object to another format, such as camel-casing, or <see langword="null" /> to leave property names unchanged.</summary>
    /// <returns>A property naming policy, or <see langword="null" /> to leave property names unchanged.</returns>
    public JsonNamingPolicy? PropertyNamingPolicy
    {
      get => this._jsonPropertyNamingPolicy;
      set
      {
        this.VerifyMutable();
        this._jsonPropertyNamingPolicy = value;
      }
    }

    /// <summary>Gets or sets a value that determines whether a property's name uses a case-insensitive comparison during deserialization. The default value is <see langword="false" />.</summary>
    /// <returns>
    /// <see langword="true" /> to compare property names using case-insensitive comparison; otherwise, <see langword="false" />.</returns>
    public bool PropertyNameCaseInsensitive
    {
      get => this._propertyNameCaseInsensitive;
      set
      {
        this.VerifyMutable();
        this._propertyNameCaseInsensitive = value;
      }
    }

    /// <summary>Gets or sets a value that defines how comments are handled during deserialization.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property was set after serialization or deserialization has occurred.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The comment handling enum is set to a value that is not supported (or not within the <see cref="T:System.Text.Json.JsonCommentHandling" /> enum range).</exception>
    /// <returns>A value that indicates whether comments are allowed, disallowed, or skipped.</returns>
    public JsonCommentHandling ReadCommentHandling
    {
      get => this._readCommentHandling;
      set
      {
        this.VerifyMutable();
        this._readCommentHandling = value <= JsonCommentHandling.Skip ? value : throw new ArgumentOutOfRangeException(nameof (value), SR.JsonSerializerDoesNotSupportComments);
      }
    }

    /// <summary>Defines how deserializing a type declared as an <see cref="T:System.Object" /> is handled during deserialization.</summary>
    public JsonUnknownTypeHandling UnknownTypeHandling
    {
      get => this._unknownTypeHandling;
      set
      {
        this.VerifyMutable();
        this._unknownTypeHandling = value;
      }
    }

    /// <summary>Gets or sets a value that defines whether JSON should use pretty printing. By default, JSON is serialized without any extra white space.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property was set after serialization or deserialization has occurred.</exception>
    /// <returns>
    /// <see langword="true" /> if JSON should pretty print on serialization; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
    public bool WriteIndented
    {
      get => this._writeIndented;
      set
      {
        this.VerifyMutable();
        this._writeIndented = value;
      }
    }

    /// <summary>Configures how object references are handled when reading and writing JSON.</summary>
    public ReferenceHandler? ReferenceHandler
    {
      get => this._referenceHandler;
      set
      {
        this.VerifyMutable();
        this._referenceHandler = value;
        this.ReferenceHandlingStrategy = value != null ? value.HandlingStrategy : ReferenceHandlingStrategy.None;
      }
    }

    internal MemberAccessor MemberAccessorStrategy
    {
      get
      {
        if (this._memberAccessorStrategy == null)
          this._memberAccessorStrategy = RuntimeFeature.IsDynamicCodeSupported ? (MemberAccessor) new ReflectionEmitMemberAccessor() : (MemberAccessor) new ReflectionMemberAccessor();
        return this._memberAccessorStrategy;
      }
    }

    internal bool IsInitializedForReflectionSerializer { get; set; }

    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    internal void InitializeForReflectionSerializer()
    {
      this.RootBuiltInConverters();
      this._typeInfoCreationFunc = new Func<Type, JsonSerializerOptions, JsonTypeInfo>(CreateJsonTypeInfo);
      this.IsInitializedForReflectionSerializer = true;


      #nullable disable
      [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
      static JsonTypeInfo CreateJsonTypeInfo(Type type, JsonSerializerOptions options) => new JsonTypeInfo(type, options);
    }

    internal JsonTypeInfo GetOrAddClass(Type type)
    {
      this._haveTypesBeenCreated = true;
      JsonTypeInfo jsonTypeInfo;
      if (!this.TryGetClass(type, out jsonTypeInfo))
        jsonTypeInfo = this._classes.GetOrAdd(type, this.GetClassFromContextOrCreate(type));
      return jsonTypeInfo;
    }

    internal JsonTypeInfo GetClassFromContextOrCreate(Type type)
    {
      JsonTypeInfo typeInfo = this._context?.GetTypeInfo(type);
      if (typeInfo != null)
        return typeInfo;
      if (this._typeInfoCreationFunc != null)
        return this._typeInfoCreationFunc(type, this);
      ThrowHelper.ThrowNotSupportedException_NoMetadataForType(type);
      return (JsonTypeInfo) null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal JsonTypeInfo GetOrAddClassForRootType(Type type)
    {
      JsonTypeInfo classForRootType = this._lastClass;
      if (classForRootType?.Type != type)
      {
        classForRootType = this.GetOrAddClass(type);
        this._lastClass = classForRootType;
      }
      return classForRootType;
    }

    internal bool TryGetClass(Type type, [NotNullWhen(true)] out JsonTypeInfo jsonTypeInfo)
    {
      JsonTypeInfo jsonTypeInfo1;
      if (!this._classes.TryGetValue(type, out jsonTypeInfo1))
      {
        jsonTypeInfo = (JsonTypeInfo) null;
        return false;
      }
      jsonTypeInfo = jsonTypeInfo1;
      return true;
    }

    internal bool TypeIsCached(Type type) => this._classes.ContainsKey(type);

    internal void ClearClasses()
    {
      this._classes.Clear();
      this._lastClass = (JsonTypeInfo) null;
    }

    internal JsonDocumentOptions GetDocumentOptions() => new JsonDocumentOptions()
    {
      AllowTrailingCommas = this.AllowTrailingCommas,
      CommentHandling = this.ReadCommentHandling,
      MaxDepth = this.MaxDepth
    };

    internal JsonNodeOptions GetNodeOptions() => new JsonNodeOptions()
    {
      PropertyNameCaseInsensitive = this.PropertyNameCaseInsensitive
    };

    internal JsonReaderOptions GetReaderOptions() => new JsonReaderOptions()
    {
      AllowTrailingCommas = this.AllowTrailingCommas,
      CommentHandling = this.ReadCommentHandling,
      MaxDepth = this.MaxDepth
    };

    internal JsonWriterOptions GetWriterOptions() => new JsonWriterOptions()
    {
      Encoder = this.Encoder,
      Indented = this.WriteIndented,
      SkipValidation = true
    };

    internal void VerifyMutable()
    {
      if (!this._haveTypesBeenCreated && this._context == null)
        return;
      ThrowHelper.ThrowInvalidOperationException_SerializerOptionsImmutable(this._context);
    }

    internal static class TrackedOptionsInstances
    {
      public static ConditionalWeakTable<JsonSerializerOptions, object> All { get; } = new ConditionalWeakTable<JsonSerializerOptions, object>();
    }
  }
}
