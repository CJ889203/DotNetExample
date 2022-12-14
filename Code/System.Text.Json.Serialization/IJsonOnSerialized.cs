// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.IJsonOnSerialized
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json.Serialization
{
  /// <summary>Specifies that the type should have its <see cref="M:System.Text.Json.Serialization.IJsonOnSerialized.OnSerialized" /> method called after serialization occurs.</summary>
  public interface IJsonOnSerialized
  {
    /// <summary>The method that is called after serialization.</summary>
    void OnSerialized();
  }
}
