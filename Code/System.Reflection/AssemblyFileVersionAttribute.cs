// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyFileVersionAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Instructs a compiler to use a specific version number for the Win32 file version resource. The Win32 file version is not required to be the same as the assembly's version number.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyFileVersionAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyFileVersionAttribute" /> class, specifying the file version.</summary>
    /// <param name="version">The file version.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="version" /> is <see langword="null" />.</exception>
    public AssemblyFileVersionAttribute(string version) => this.Version = version ?? throw new ArgumentNullException(nameof (version));

    /// <summary>Gets the Win32 file version resource name.</summary>
    /// <returns>A string containing the file version resource name.</returns>
    public string Version { get; }
  }
}
