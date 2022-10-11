// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyNameProxy
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides a remotable version of the <see langword="AssemblyName" />.</summary>
  public class AssemblyNameProxy : MarshalByRefObject
  {
    /// <summary>Gets the <see langword="AssemblyName" /> for a given file.</summary>
    /// <param name="assemblyFile">The assembly file for which to get the <see langword="AssemblyName" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="assemblyFile" /> is empty.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    /// <paramref name="assemblyFile" /> is not a valid assembly.</exception>
    /// <returns>An <see langword="AssemblyName" /> object representing the given file.</returns>
    public AssemblyName GetAssemblyName(string assemblyFile) => AssemblyName.GetAssemblyName(assemblyFile);
  }
}
