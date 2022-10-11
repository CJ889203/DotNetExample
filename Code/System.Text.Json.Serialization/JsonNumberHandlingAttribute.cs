// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonNumberHandlingAttribute
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>When placed on a type, property, or field, indicates what <see cref="T:System.Text.Json.Serialization.JsonNumberHandling" /> settings should be used when serializing or deserializing numbers.</summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public sealed class JsonNumberHandlingAttribute : JsonAttribute
  {
    /// <summary>Indicates what settings should be used when serializing or deserializing numbers.</summary>
    /// <returns>An object that determines the number serialization and deserialization settings.</returns>
    public JsonNumberHandling Handling { get; }

    /// <summary>Initializes a new instance of <see cref="T:System.Text.Json.Serialization.JsonNumberHandlingAttribute" />.</summary>
    /// <param name="handling">A bitwise combination of the enumeration values that specify how number types should be handled when serializing or deserializing.</param>
    public JsonNumberHandlingAttribute(JsonNumberHandling handling) => this.Handling = JsonSerializer.IsValidNumberHandlingValue(handling) ? handling : throw new ArgumentOutOfRangeException(nameof (handling));
  }
}
