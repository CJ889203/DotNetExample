// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.ReferenceHandler`1
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>This class defines how the <see cref="T:System.Text.Json.JsonSerializer" /> deals with references on serialization and deserialization.</summary>
  /// <typeparam name="T">The type of the <see cref="T:System.Text.Json.Serialization.ReferenceResolver" /> to create on each serialization or deserialization call.</typeparam>
  public sealed class ReferenceHandler<T> : ReferenceHandler where T : ReferenceResolver, new()
  {
    /// <summary>Creates a new <see cref="T:System.Text.Json.Serialization.ReferenceResolver" /> of type <typeparamref name="T" /> used for each serialization call.</summary>
    /// <returns>The new resolver to use for serialization and deserialization.</returns>
    public override ReferenceResolver CreateResolver() => (ReferenceResolver) new T();
  }
}
