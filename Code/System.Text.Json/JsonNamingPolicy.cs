// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonNamingPolicy
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml


#nullable enable
namespace System.Text.Json
{
  /// <summary>Determines the naming policy used to convert a string-based name to another format, such as a camel-casing format.</summary>
  public abstract class JsonNamingPolicy
  {
    /// <summary>Gets the naming policy for camel-casing.</summary>
    /// <returns>The naming policy for camel-casing.</returns>
    public static JsonNamingPolicy CamelCase { get; } = (JsonNamingPolicy) new JsonCamelCaseNamingPolicy();

    /// <summary>When overridden in a derived class, converts the specified name according to the policy.</summary>
    /// <param name="name">The name to convert.</param>
    /// <returns>The converted name.</returns>
    public abstract string ConvertName(string name);
  }
}
