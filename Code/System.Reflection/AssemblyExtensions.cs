// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyExtensions
// Assembly: System.Reflection.TypeExtensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 640AC10B-88E0-451A-B5D0-A4B0F7E22777
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.TypeExtensions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.TypeExtensions.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Reflection
{
  public static class AssemblyExtensions
  {
    /// <param name="assembly" />
    [RequiresUnreferencedCode("Types might be removed")]
    public static Type[] GetExportedTypes(this Assembly assembly)
    {
      ArgumentNullException.ThrowIfNull((object) assembly, nameof (assembly));
      return assembly.GetExportedTypes();
    }

    /// <param name="assembly" />
    public static Module[] GetModules(this Assembly assembly)
    {
      ArgumentNullException.ThrowIfNull((object) assembly, nameof (assembly));
      return assembly.GetModules();
    }

    /// <param name="assembly" />
    [RequiresUnreferencedCode("Types might be removed")]
    public static Type[] GetTypes(this Assembly assembly)
    {
      ArgumentNullException.ThrowIfNull((object) assembly, nameof (assembly));
      return assembly.GetTypes();
    }
  }
}
