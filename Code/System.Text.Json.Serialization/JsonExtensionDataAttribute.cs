// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.JsonExtensionDataAttribute
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>When placed on a property of type <see cref="T:System.Collections.Generic.IDictionary`2" />, any properties that do not have a matching member are added to that dictionary during deserialization and written during serialization.</summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public sealed class JsonExtensionDataAttribute : JsonAttribute
  {
  }
}
