// Decompiled with JetBrains decompiler
// Type: System.Reflection.ManifestResourceAttributes
// Assembly: System.Reflection.Metadata, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: FDD13CB9-4DB5-4759-8B88-2D188C369E68
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.Metadata.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.Metadata.xml

namespace System.Reflection
{
  [Flags]
  public enum ManifestResourceAttributes
  {
    /// <summary>The resource is exported from the assembly.</summary>
    Public = 1,
    /// <summary>The resource is not exported from the assembly.</summary>
    Private = 2,
    /// <summary>Masks just the visibility-related attributes.</summary>
    VisibilityMask = 7,
  }
}
