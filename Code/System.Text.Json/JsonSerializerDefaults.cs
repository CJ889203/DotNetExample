// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonSerializerDefaults
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json
{
  /// <summary>Specifies scenario-based default serialization options that can be used to construct a <see cref="T:System.Text.Json.JsonSerializerOptions" /> instance.</summary>
  public enum JsonSerializerDefaults
  {
    /// <summary>
    ///   <para>General-purpose option values. These are the same settings that are applied if a <see cref="T:System.Text.Json.JsonSerializerDefaults" /> member isn't specified.</para>
    ///   <para>For information about the default property values that are applied, see JsonSerializerOptions properties.</para>
    /// </summary>
    General,
    /// <summary>
    ///   <para>Option values appropriate to Web-based scenarios.</para>
    ///   <para>This member implies that:</para>
    ///   <para>- Property names are treated as case-insensitive.</para>
    ///   <para>- "camelCase" name formatting should be employed.</para>
    ///   <para>- Quoted numbers (JSON strings for number properties) are allowed.</para>
    /// </summary>
    Web,
  }
}
