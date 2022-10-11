﻿// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblySignatureKeyAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides migration from an older, simpler strong name key to a larger key with a stronger hashing algorithm.</summary>
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
  public sealed class AssemblySignatureKeyAttribute : Attribute
  {
    /// <summary>Creates a new instance of the <see cref="T:System.Reflection.AssemblySignatureKeyAttribute" /> class by using the specified public key and countersignature.</summary>
    /// <param name="publicKey">The public or identity key.</param>
    /// <param name="countersignature">The countersignature, which is the signature key portion of the strong-name key.</param>
    public AssemblySignatureKeyAttribute(string publicKey, string countersignature)
    {
      this.PublicKey = publicKey;
      this.Countersignature = countersignature;
    }

    /// <summary>Gets the public key for the strong name used to sign the assembly.</summary>
    /// <returns>The public key for this assembly.</returns>
    public string PublicKey { get; }

    /// <summary>Gets the countersignature for the strong name for this assembly.</summary>
    /// <returns>The countersignature for this signature key.</returns>
    public string Countersignature { get; }
  }
}
