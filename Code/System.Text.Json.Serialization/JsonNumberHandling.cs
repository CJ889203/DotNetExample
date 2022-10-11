// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonNumberHandling
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>Determines how <see cref="T:System.Text.Json.JsonSerializer" /> handles numbers when serializing and deserializing.</summary>
  [Flags]
  public enum JsonNumberHandling
  {
    /// <summary>Numbers will only be read from <see cref="F:System.Text.Json.JsonTokenType.Number" /> tokens and will only be written as JSON numbers (without quotes).</summary>
    Strict = 0,
    /// <summary>Numbers can be read from <see cref="F:System.Text.Json.JsonTokenType.String" /> tokens. Does not prevent numbers from being read from <see cref="F:System.Text.Json.JsonTokenType.Number" /> token.</summary>
    AllowReadingFromString = 1,
    /// <summary>Numbers will be written as JSON strings (with quotes), not as JSON numbers.</summary>
    WriteAsString = 2,
    /// <summary>The "NaN", "Infinity", and "-Infinity" <see cref="F:System.Text.Json.JsonTokenType.String" /> tokens can be read as floating-point constants, and the <see cref="T:System.Single" /> and <see cref="T:System.Double" /> values for these constants will be written as their corresponding JSON string representations.</summary>
    AllowNamedFloatingPointLiterals = 4,
  }
}
