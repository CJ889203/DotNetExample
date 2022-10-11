// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyVersionAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Specifies the version of the assembly being attributed.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyVersionAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see langword="AssemblyVersionAttribute" /> class with the version number of the assembly being attributed.</summary>
    /// <param name="version">The version number of the attributed assembly.</param>
    public AssemblyVersionAttribute(string version) => this.Version = version;

    /// <summary>Gets the version number of the attributed assembly.</summary>
    /// <returns>A string containing the assembly version number.</returns>
    public string Version { get; }
  }
}
