// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonSerializableAttribute
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>Instructs the System.Text.Json source generator to generate source code to help optimize performance when serializing and deserializing instances of the specified type and types in its object graph.</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public sealed class JsonSerializableAttribute : JsonAttribute
  {
    /// <summary>Initializes a new instance of <see cref="T:System.Text.Json.Serialization.JsonSerializableAttribute" /> with the specified type.</summary>
    /// <param name="type">The type to generate source code for.</param>
    public JsonSerializableAttribute(Type type)
    {
    }

    /// <summary>The name of the property for the generated <see cref="T:System.Text.Json.Serialization.Metadata.JsonTypeInfo`1" /> for the type on the generated, derived <see cref="T:System.Text.Json.Serialization.JsonSerializerContext" /> type.</summary>
    public string? TypeInfoPropertyName { get; set; }

    /// <summary>Determines what the source generator should generate for the type. If the value is <see cref="F:System.Text.Json.Serialization.JsonSourceGenerationMode.Default" />, then the setting specified on <see cref="P:System.Text.Json.Serialization.JsonSourceGenerationOptionsAttribute.GenerationMode" /> will be used.</summary>
    public JsonSourceGenerationMode GenerationMode { get; set; }
  }
}
