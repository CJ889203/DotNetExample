// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyCultureAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Specifies which culture the assembly supports.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyCultureAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyCultureAttribute" /> class with the culture supported by the assembly being attributed.</summary>
    /// <param name="culture">The culture supported by the attributed assembly.</param>
    public AssemblyCultureAttribute(string culture) => this.Culture = culture;

    /// <summary>Gets the supported culture of the attributed assembly.</summary>
    /// <returns>A string containing the name of the supported culture.</returns>
    public string Culture { get; }
  }
}
