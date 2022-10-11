// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyMetadataAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Defines a key/value metadata pair for the decorated assembly.</summary>
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
  public sealed class AssemblyMetadataAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyMetadataAttribute" /> class by using the specified metadata key and value.</summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    public AssemblyMetadataAttribute(string key, string? value)
    {
      this.Key = key;
      this.Value = value;
    }

    /// <summary>Gets the metadata key.</summary>
    /// <returns>The metadata key.</returns>
    public string Key { get; }

    /// <summary>Gets the metadata value.</summary>
    /// <returns>The metadata value.</returns>
    public string? Value { get; }
  }
}
