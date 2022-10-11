// Decompiled with JetBrains decompiler
// Type: System.Reflection.StrongNameKeyPair
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.IO;
using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>Encapsulates access to a public or private key pair used to sign strong name assemblies.</summary>
  [Obsolete("Strong name signing is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0017", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
  public class StrongNameKeyPair : IDeserializationCallback, ISerializable
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.StrongNameKeyPair" /> class, building the key pair from a <see langword="FileStream" />.</summary>
    /// <param name="keyPairFile">A <see langword="FileStream" /> containing the key pair.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keyPairFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET 6+ only: In all cases.</exception>
    public StrongNameKeyPair(FileStream keyPairFile) => throw new PlatformNotSupportedException(SR.PlatformNotSupported_StrongNameSigning);

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.StrongNameKeyPair" /> class, building the key pair from a <see langword="byte" /> array.</summary>
    /// <param name="keyPairArray">An array of type <see langword="byte" /> containing the key pair.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keyPairArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET 6+ only: In all cases.</exception>
    public StrongNameKeyPair(byte[] keyPairArray) => throw new PlatformNotSupportedException(SR.PlatformNotSupported_StrongNameSigning);

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.StrongNameKeyPair" /> class, building the key pair from serialized data.</summary>
    /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that holds the serialized object data.</param>
    /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains contextual information about the source or destination.</param>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    protected StrongNameKeyPair(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.StrongNameKeyPair" /> class, building the key pair from a <see langword="String" />.</summary>
    /// <param name="keyPairContainer">A string containing the key pair.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keyPairContainer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    public StrongNameKeyPair(string keyPairContainer) => throw new PlatformNotSupportedException(SR.PlatformNotSupported_StrongNameSigning);

    /// <summary>Gets the public part of the public key or public key token of the key pair.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <returns>An array of type <see langword="byte" /> containing the public key or public key token of the key pair.</returns>
    public byte[] PublicKey => throw new PlatformNotSupportedException(SR.PlatformNotSupported_StrongNameSigning);


    #nullable disable
    /// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with all the data required to reinstantiate the current <see cref="T:System.Reflection.StrongNameKeyPair" /> object.</summary>
    /// <param name="info">The object to be populated with serialization information.</param>
    /// <param name="context">The destination context of the serialization.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    /// <summary>Runs when the entire object graph has been deserialized.</summary>
    /// <param name="sender">The object that initiated the callback.</param>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    void IDeserializationCallback.OnDeserialization(object sender) => throw new PlatformNotSupportedException();
  }
}
