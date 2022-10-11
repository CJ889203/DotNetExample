// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyKeyFileAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Specifies the name of a file containing the key pair used to generate a strong name.</summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblyKeyFileAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see langword="AssemblyKeyFileAttribute" /> class with the name of the file containing the key pair to generate a strong name for the assembly being attributed.</summary>
    /// <param name="keyFile">The name of the file containing the key pair.</param>
    public AssemblyKeyFileAttribute(string keyFile) => this.KeyFile = keyFile;

    /// <summary>Gets the name of the file containing the key pair used to generate a strong name for the attributed assembly.</summary>
    /// <returns>A string containing the name of the file that contains the key pair.</returns>
    public string KeyFile { get; }
  }
}
