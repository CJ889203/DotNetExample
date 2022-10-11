// Decompiled with JetBrains decompiler
// Type: System.Reflection.ManifestResourceInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides access to manifest resources, which are XML files that describe application dependencies.</summary>
  public class ManifestResourceInfo
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ManifestResourceInfo" /> class for a resource that is contained by the specified assembly and file, and that has the specified location.</summary>
    /// <param name="containingAssembly">The assembly that contains the manifest resource.</param>
    /// <param name="containingFileName">The name of the file that contains the manifest resource, if the file is not the same as the manifest file.</param>
    /// <param name="resourceLocation">A bitwise combination of enumeration values that provides information about the location of the manifest resource.</param>
    public ManifestResourceInfo(
      Assembly? containingAssembly,
      string? containingFileName,
      ResourceLocation resourceLocation)
    {
      this.ReferencedAssembly = containingAssembly;
      this.FileName = containingFileName;
      this.ResourceLocation = resourceLocation;
    }

    /// <summary>Gets the containing assembly for the manifest resource.</summary>
    /// <returns>The manifest resource's containing assembly.</returns>
    public virtual Assembly? ReferencedAssembly { get; }

    /// <summary>Gets the name of the file that contains the manifest resource, if it is not the same as the manifest file.</summary>
    /// <returns>The manifest resource's file name.</returns>
    public virtual string? FileName { get; }

    /// <summary>Gets the manifest resource's location.</summary>
    /// <returns>A bitwise combination of <see cref="T:System.Reflection.ResourceLocation" /> flags that indicates the location of the manifest resource.</returns>
    public virtual ResourceLocation ResourceLocation { get; }
  }
}
