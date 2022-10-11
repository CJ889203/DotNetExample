// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyAlgorithmIdAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Configuration.Assemblies;

namespace System.Reflection
{
  /// <summary>Specifies an algorithm to hash all files in an assembly. This class cannot be inherited.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyAlgorithmIdAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyAlgorithmIdAttribute" /> class with the specified hash algorithm, using one of the members of <see cref="T:System.Configuration.Assemblies.AssemblyHashAlgorithm" /> to represent the hash algorithm.</summary>
    /// <param name="algorithmId">A member of <see langword="AssemblyHashAlgorithm" /> that represents the hash algorithm.</param>
    public AssemblyAlgorithmIdAttribute(AssemblyHashAlgorithm algorithmId) => this.AlgorithmId = (uint) algorithmId;

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyAlgorithmIdAttribute" /> class with the specified hash algorithm, using an unsigned integer to represent the hash algorithm.</summary>
    /// <param name="algorithmId">An unsigned integer representing the hash algorithm.</param>
    [CLSCompliant(false)]
    public AssemblyAlgorithmIdAttribute(uint algorithmId) => this.AlgorithmId = algorithmId;

    /// <summary>Gets the hash algorithm of an assembly manifest's contents.</summary>
    /// <returns>An unsigned integer representing the assembly hash algorithm.</returns>
    [CLSCompliant(false)]
    public uint AlgorithmId { get; }
  }
}
