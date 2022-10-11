// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonPropertyNameAttribute
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>Specifies the property name that is present in the JSON when serializing and deserializing. This overrides any naming policy specified by <see cref="T:System.Text.Json.JsonNamingPolicy" />.</summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public sealed class JsonPropertyNameAttribute : JsonAttribute
  {
    /// <summary>Initializes a new instance of <see cref="T:System.Text.Json.Serialization.JsonPropertyNameAttribute" /> with the specified property name.</summary>
    /// <param name="name">The name of the property.</param>
    public JsonPropertyNameAttribute(string name) => this.Name = name;

    /// <summary>Gets the name of the property.</summary>
    /// <returns>The name of the property.</returns>
    public string Name { get; }
  }
}
