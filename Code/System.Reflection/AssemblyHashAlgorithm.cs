// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyHashAlgorithm
// Assembly: System.Reflection.Metadata, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: FDD13CB9-4DB5-4759-8B88-2D188C369E68
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.Metadata.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.Metadata.xml

namespace System.Reflection
{
  /// <summary>Specifies the hash algorithms used for hashing assembly files and for generating the strong name.</summary>
  public enum AssemblyHashAlgorithm
  {
    /// <summary>
    ///   <para>A mask indicating that there is no hash algorithm.</para>
    ///   <para>If you specify <see cref="F:System.Reflection.AssemblyHashAlgorithm.None" /> for a multi-module assembly, the common language runtime defaults to the SHA1 algorithm, since multi-module assemblies need to generate a hash.</para>
    /// </summary>
    None = 0,
    /// <summary>
    ///   <para>Retrieves the MD5 message-digest algorithm.</para>
    ///   <para>Due to collision problems with MD5, Microsoft recommends SHA256.</para>
    ///   <para>MD5 was developed by Rivest in 1991. It is basically MD4 with safety-belts and, while it is slightly slower than MD4, it helps provide more security. The algorithm consists of four distinct rounds, which has a slightly different design from that of MD4. Message-digest size, as well as padding requirements, remain the same.</para>
    /// </summary>
    MD5 = 32771, // 0x00008003
    /// <summary>
    ///   <para>Retrieves a revision of the Secure Hash Algorithm that corrects an unpublished flaw in SHA.</para>
    ///   <para>Due to collision problems with SHA1, Microsoft recommends SHA256.</para>
    /// </summary>
    Sha1 = 32772, // 0x00008004
    /// <summary>Retrieves a version of the Secure Hash Algorithm with a hash size of 256 bits.</summary>
    Sha256 = 32780, // 0x0000800C
    /// <summary>Retrieves a version of the Secure Hash Algorithm with a hash size of 384 bits.</summary>
    Sha384 = 32781, // 0x0000800D
    /// <summary>Retrieves a version of the Secure Hash Algorithm with a hash size of 512 bits.</summary>
    Sha512 = 32782, // 0x0000800E
  }
}
