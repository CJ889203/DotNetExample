// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonPropertyOrderAttribute
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>Specifies the property order that is present in the JSON when serializing. Lower values are serialized first.
  /// If the attribute is not specified, the default value is 0.</summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public sealed class JsonPropertyOrderAttribute : JsonAttribute
  {
    /// <summary>Initializes a new instance of <see cref="T:System.Text.Json.Serialization.JsonPropertyNameAttribute" /> with the specified order.</summary>
    /// <param name="order">The order of the property.</param>
    public JsonPropertyOrderAttribute(int order) => this.Order = order;

    /// <summary>Gets the serialization order of the property.</summary>
    public int Order { get; }
  }
}
