// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonSourceGenerationOptionsAttribute
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>Instructs the System.Text.Json source generator to assume the specified options will be used at run time via <see cref="T:System.Text.Json.JsonSerializerOptions" />.</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class JsonSourceGenerationOptionsAttribute : JsonAttribute
  {
    /// <summary>Gets or sets the default ignore condition.</summary>
    public JsonIgnoreCondition DefaultIgnoreCondition { get; set; }

    /// <summary>Gets or sets a value that indicates whether to ignore read-only fields.</summary>
    public bool IgnoreReadOnlyFields { get; set; }

    /// <summary>Gets or sets a value that indicates whether to ignore read-only properties.</summary>
    public bool IgnoreReadOnlyProperties { get; set; }

    /// <summary>Gets or sets a value that indicates whether to include fields for serialization and deserialization.</summary>
    public bool IncludeFields { get; set; }

    /// <summary>Gets or sets a built-in naming policy to convert JSON property names with.</summary>
    public JsonKnownNamingPolicy PropertyNamingPolicy { get; set; }

    /// <summary>Gets or sets a value that indicates whether JSON output should be pretty-printed.</summary>
    public bool WriteIndented { get; set; }

    /// <summary>Gets or sets the source generation mode for types that don't explicitly set the mode with <see cref="P:System.Text.Json.Serialization.JsonSerializableAttribute.GenerationMode" />.</summary>
    public JsonSourceGenerationMode GenerationMode { get; set; }
  }
}
