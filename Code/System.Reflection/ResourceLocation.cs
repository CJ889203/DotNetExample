// Decompiled with JetBrains decompiler
// Type: System.Reflection.ResourceLocation
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Specifies the resource location.</summary>
  [Flags]
  public enum ResourceLocation
  {
    /// <summary>Specifies that the resource is contained in another assembly.</summary>
    ContainedInAnotherAssembly = 2,
    /// <summary>Specifies that the resource is contained in the manifest file.</summary>
    ContainedInManifestFile = 4,
    /// <summary>Specifies an embedded (that is, non-linked) resource.</summary>
    Embedded = 1,
  }
}
