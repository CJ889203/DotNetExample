// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonConverterAttribute
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>When placed on a property or type, specifies the converter type to use.</summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false)]
  public class JsonConverterAttribute : JsonAttribute
  {
    /// <summary>Initializes a new instance of <see cref="T:System.Text.Json.Serialization.JsonConverterAttribute" /> with the specified converter type.</summary>
    /// <param name="converterType">The type of the converter.</param>
    public JsonConverterAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type converterType) => this.ConverterType = converterType;

    /// <summary>Initializes a new instance of <see cref="T:System.Text.Json.Serialization.JsonConverterAttribute" />.</summary>
    protected JsonConverterAttribute()
    {
    }

    /// <summary>Gets the type of the <see cref="T:System.Text.Json.Serialization.JsonConverterAttribute" />, or <see langword="null" /> if it was created without a type.</summary>
    /// <returns>The type of the <see cref="T:System.Text.Json.Serialization.JsonConverterAttribute" />, or <see langword="null" /> if it was created without a type.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public Type? ConverterType { get; private set; }

    /// <summary>When overridden in a derived class and <see cref="P:System.Text.Json.Serialization.JsonConverterAttribute.ConverterType" /> is <see langword="null" />, allows the derived class to create a <see cref="T:System.Text.Json.Serialization.JsonConverter" /> in order to pass additional state.</summary>
    /// <param name="typeToConvert">The type of the converter.</param>
    /// <returns>The custom converter.</returns>
    public virtual JsonConverter? CreateConverter(Type typeToConvert) => (JsonConverter) null;
  }
}
