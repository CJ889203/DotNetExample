// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.ReferenceHandler
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>This class defines how the <see cref="T:System.Text.Json.JsonSerializer" /> deals with references on serialization and deserialization.</summary>
  public abstract class ReferenceHandler
  {
    internal ReferenceHandlingStrategy HandlingStrategy = ReferenceHandlingStrategy.Preserve;

    /// <summary>Metadata properties will be honored when deserializing JSON objects and arrays into reference types and written when serializing reference types. This is necessary to create round-trippable JSON from objects that contain cycles or duplicate references.</summary>
    public static ReferenceHandler Preserve { get; } = (ReferenceHandler) new PreserveReferenceHandler();

    /// <summary>Ignores an object when a reference cycle is detected during serialization.</summary>
    public static ReferenceHandler IgnoreCycles { get; } = (ReferenceHandler) new IgnoreReferenceHandler();

    /// <summary>Returns the <see cref="T:System.Text.Json.Serialization.ReferenceResolver" /> used for each serialization call.</summary>
    /// <returns>The resolver to use for serialization and deserialization.</returns>
    public abstract ReferenceResolver CreateResolver();


    #nullable disable
    internal virtual ReferenceResolver CreateResolver(bool writing) => this.CreateResolver();
  }
}
