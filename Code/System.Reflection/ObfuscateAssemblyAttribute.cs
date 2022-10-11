// Decompiled with JetBrains decompiler
// Type: System.Reflection.ObfuscateAssemblyAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Instructs obfuscation tools to use their standard obfuscation rules for the appropriate assembly type.</summary>
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
  public sealed class ObfuscateAssemblyAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ObfuscateAssemblyAttribute" /> class, specifying whether the assembly to be obfuscated is public or private.</summary>
    /// <param name="assemblyIsPrivate">
    /// <see langword="true" /> if the assembly is used within the scope of one application; otherwise, <see langword="false" />.</param>
    public ObfuscateAssemblyAttribute(bool assemblyIsPrivate) => this.AssemblyIsPrivate = assemblyIsPrivate;

    /// <summary>Gets a <see cref="T:System.Boolean" /> value indicating whether the assembly was marked private.</summary>
    /// <returns>
    /// <see langword="true" /> if the assembly was marked private; otherwise, <see langword="false" />.</returns>
    public bool AssemblyIsPrivate { get; }

    /// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the obfuscation tool should remove the attribute after processing.</summary>
    /// <returns>
    /// <see langword="true" /> if the obfuscation tool should remove the attribute after processing; otherwise, <see langword="false" />. The default value for this property is <see langword="true" />.</returns>
    public bool StripAfterObfuscation { get; set; } = true;
  }
}
