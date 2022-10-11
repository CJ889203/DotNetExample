// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonSourceGenerationMode
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>The generation mode for the System.Text.Json source generator.</summary>
  [Flags]
  public enum JsonSourceGenerationMode
  {
    /// <summary>When specified on <see cref="P:System.Text.Json.Serialization.JsonSourceGenerationOptionsAttribute.GenerationMode" />, indicates that both type-metadata initialization logic and optimized serialization logic should be generated for all types. When specified on <see cref="P:System.Text.Json.Serialization.JsonSerializableAttribute.GenerationMode" />, indicates that the setting on <see cref="P:System.Text.Json.Serialization.JsonSourceGenerationOptionsAttribute.GenerationMode" /> should be used.</summary>
    Default = 0,
    /// <summary>Instructs the JSON source generator to generate type-metadata initialization logic.</summary>
    Metadata = 1,
    /// <summary>Instructs the JSON source generator to generate optimized serialization logic.</summary>
    Serialization = 2,
  }
}
