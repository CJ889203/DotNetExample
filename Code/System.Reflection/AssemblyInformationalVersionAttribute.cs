// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyInformationalVersionAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Defines additional version information for an assembly manifest.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyInformationalVersionAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyInformationalVersionAttribute" /> class.</summary>
    /// <param name="informationalVersion">The assembly version information.</param>
    public AssemblyInformationalVersionAttribute(string informationalVersion) => this.InformationalVersion = informationalVersion;

    /// <summary>Gets version information.</summary>
    /// <returns>A string containing the version information.</returns>
    public string InformationalVersion { get; }
  }
}
