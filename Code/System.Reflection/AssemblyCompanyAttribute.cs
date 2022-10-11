// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyCompanyAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Defines a company name custom attribute for an assembly manifest.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyCompanyAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyCompanyAttribute" /> class.</summary>
    /// <param name="company">The company name information.</param>
    public AssemblyCompanyAttribute(string company) => this.Company = company;

    /// <summary>Gets company name information.</summary>
    /// <returns>A string containing the company name.</returns>
    public string Company { get; }
  }
}
