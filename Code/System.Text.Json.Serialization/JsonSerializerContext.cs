// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonSerializerContext
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.ComponentModel;
using System.Text.Json.Serialization.Metadata;


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>Provides metadata about a set of types that is relevant to JSON serialization.</summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public abstract class JsonSerializerContext
  {

    #nullable disable
    private bool? _canUseSerializationLogic;
    internal JsonSerializerOptions _options;


    #nullable enable
    /// <summary>Gets the run-time specified options of the context. If no options were passed when instanciating the context, then a new instance is bound and returned.</summary>
    public JsonSerializerOptions Options
    {
      get
      {
        if (this._options == null)
        {
          this._options = new JsonSerializerOptions();
          this._options._context = this;
        }
        return this._options;
      }
    }

    internal bool CanUseSerializationLogic
    {
      get
      {
        if (!this._canUseSerializationLogic.HasValue)
          this._canUseSerializationLogic = this.GeneratedSerializerOptions != null ? new bool?(this.Options.Converters.Count == 0 && this.Options.Encoder == null && (this.Options.NumberHandling & (JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowNamedFloatingPointLiterals)) == JsonNumberHandling.Strict && this.Options.ReferenceHandlingStrategy == ReferenceHandlingStrategy.None && !this.Options.IgnoreNullValues && this.Options.DefaultIgnoreCondition == this.GeneratedSerializerOptions.DefaultIgnoreCondition && this.Options.IgnoreReadOnlyFields == this.GeneratedSerializerOptions.IgnoreReadOnlyFields && this.Options.IgnoreReadOnlyProperties == this.GeneratedSerializerOptions.IgnoreReadOnlyProperties && this.Options.IncludeFields == this.GeneratedSerializerOptions.IncludeFields && this.Options.PropertyNamingPolicy == this.GeneratedSerializerOptions.PropertyNamingPolicy && this.Options.DictionaryKeyPolicy == this.GeneratedSerializerOptions.DictionaryKeyPolicy && this.Options.WriteIndented == this.GeneratedSerializerOptions.WriteIndented) : new bool?(false);
        return this._canUseSerializationLogic.Value;
      }
    }

    protected abstract JsonSerializerOptions? GeneratedSerializerOptions { get; }

    protected JsonSerializerContext(JsonSerializerOptions? options)
    {
      if (options == null)
        return;
      if (options._context != null)
        ThrowHelper.ThrowInvalidOperationException_JsonSerializerOptionsAlreadyBoundToContext();
      this._options = options;
      options._context = this;
    }

    /// <summary>Returns a <see cref="T:System.Text.Json.Serialization.Metadata.JsonTypeInfo" /> instance representing the given type.</summary>
    /// <param name="type">The type to fetch metadata about.</param>
    /// <returns>The metadata for the specified type, or <see langword="null" /> if the context has no metadata for the type.</returns>
    public abstract JsonTypeInfo? GetTypeInfo(Type type);
  }
}
