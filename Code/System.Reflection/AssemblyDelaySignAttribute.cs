// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyDelaySignAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Specifies that the assembly is not fully signed when created.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyDelaySignAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyDelaySignAttribute" /> class.</summary>
    /// <param name="delaySign">
    /// <see langword="true" /> if the feature this attribute represents is activated; otherwise, <see langword="false" />.</param>
    public AssemblyDelaySignAttribute(bool delaySign) => this.DelaySign = delaySign;

    /// <summary>Gets a value indicating the state of the attribute.</summary>
    /// <returns>
    /// <see langword="true" /> if this assembly has been built as delay-signed; otherwise, <see langword="false" />.</returns>
    public bool DelaySign { get; }
  }
}
