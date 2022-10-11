// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyTrademarkAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Defines a trademark custom attribute for an assembly manifest.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyTrademarkAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyTrademarkAttribute" /> class.</summary>
    /// <param name="trademark">The trademark information.</param>
    public AssemblyTrademarkAttribute(string trademark) => this.Trademark = trademark;

    /// <summary>Gets trademark information.</summary>
    /// <returns>A <see langword="String" /> containing trademark information.</returns>
    public string Trademark { get; }
  }
}
