// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyName
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Configuration.Assemblies;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;


#nullable enable
namespace System.Reflection
{
  /// <summary>Describes an assembly's unique identity in full.</summary>
  public sealed class AssemblyName : ICloneable, IDeserializationCallback, ISerializable
  {

    #nullable disable
    private string _name;
    private byte[] _publicKey;
    private byte[] _publicKeyToken;
    private CultureInfo _cultureInfo;
    private string _codeBase;
    private Version _version;
    private AssemblyHashAlgorithm _hashAlgorithm;
    private AssemblyVersionCompatibility _versionCompatibility;
    private AssemblyNameFlags _flags;
    internal const char c_DummyChar = '\uFFFF';
    private const short c_MaxAsciiCharsReallocate = 40;
    private const short c_MaxUnicodeCharsReallocate = 40;
    private const short c_MaxUTF_8BytesPerUnicodeChar = 4;
    private const short c_EncodedCharsPerByte = 3;
    private const string RFC3986ReservedMarks = ":/?#[]@!$&'()*+,;=";
    private const string RFC3986UnreservedMarks = "-._~";


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyName" /> class with the specified display name.</summary>
    /// <param name="assemblyName">The display name of the assembly, as returned by the <see cref="P:System.Reflection.AssemblyName.FullName" /> property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="assemblyName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.IO.FileLoadException">The referenced assembly could not be found, or could not be loaded.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.IO.IOException" />, instead.</exception>
    public AssemblyName(string assemblyName)
    {
      switch (assemblyName)
      {
        case "":
          throw new ArgumentException(SR.Format_StringZeroLength);
        case null:
          throw new ArgumentNullException(nameof (assemblyName));
        default:
          if (assemblyName[0] != char.MinValue)
          {
            this._name = assemblyName;
            this.nInit();
            break;
          }
          goto case "";
      }
    }


    #nullable disable
    internal AssemblyName(
      string name,
      byte[] publicKey,
      byte[] publicKeyToken,
      Version version,
      CultureInfo cultureInfo,
      AssemblyHashAlgorithm hashAlgorithm,
      AssemblyVersionCompatibility versionCompatibility,
      string codeBase,
      AssemblyNameFlags flags)
    {
      this._name = name;
      this._publicKey = publicKey;
      this._publicKeyToken = publicKeyToken;
      this._version = version;
      this._cultureInfo = cultureInfo;
      this._hashAlgorithm = hashAlgorithm;
      this._versionCompatibility = versionCompatibility;
      this._codeBase = codeBase;
      this._flags = flags;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void nInit();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AssemblyName nGetFileInformation(string s);

    internal static AssemblyName GetFileInformationCore(string assemblyFile) => AssemblyName.nGetFileInformation(Path.GetFullPath(assemblyFile));

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern byte[] ComputePublicKeyToken();

    internal void SetProcArchIndex(PortableExecutableKinds pek, ImageFileMachine ifm) => this.ProcessorArchitecture = AssemblyName.CalculateProcArchIndex(pek, ifm, this._flags);

    internal static ProcessorArchitecture CalculateProcArchIndex(
      PortableExecutableKinds pek,
      ImageFileMachine ifm,
      AssemblyNameFlags flags)
    {
      if ((flags & (AssemblyNameFlags) 240) == (AssemblyNameFlags) 112)
        return ProcessorArchitecture.None;
      if ((pek & PortableExecutableKinds.PE32Plus) == PortableExecutableKinds.PE32Plus)
      {
        switch (ifm)
        {
          case ImageFileMachine.I386:
            if ((pek & PortableExecutableKinds.ILOnly) == PortableExecutableKinds.ILOnly)
              return ProcessorArchitecture.MSIL;
            break;
          case ImageFileMachine.IA64:
            return ProcessorArchitecture.IA64;
          case ImageFileMachine.AMD64:
            return ProcessorArchitecture.Amd64;
        }
      }
      else
      {
        switch (ifm)
        {
          case ImageFileMachine.I386:
            return (pek & PortableExecutableKinds.Required32Bit) == PortableExecutableKinds.Required32Bit || (pek & PortableExecutableKinds.ILOnly) != PortableExecutableKinds.ILOnly ? ProcessorArchitecture.X86 : ProcessorArchitecture.MSIL;
          case ImageFileMachine.ARM:
            return ProcessorArchitecture.Arm;
        }
      }
      return ProcessorArchitecture.None;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyName" /> class.</summary>
    public AssemblyName() => this._versionCompatibility = AssemblyVersionCompatibility.SameMachine;


    #nullable enable
    /// <summary>Gets or sets the simple name of the assembly. This is usually, but not necessarily, the file name of the manifest file of the assembly, minus its extension.</summary>
    /// <returns>The simple name of the assembly.</returns>
    public string? Name
    {
      get => this._name;
      set => this._name = value;
    }

    /// <summary>Gets or sets the major, minor, build, and revision numbers of the assembly.</summary>
    /// <returns>An object that represents the major, minor, build, and revision numbers of the assembly.</returns>
    public Version? Version
    {
      get => this._version;
      set => this._version = value;
    }

    /// <summary>Gets or sets the culture supported by the assembly.</summary>
    /// <returns>An object that represents the culture supported by the assembly.</returns>
    public CultureInfo? CultureInfo
    {
      get => this._cultureInfo;
      set => this._cultureInfo = value;
    }

    /// <summary>Gets or sets the name of the culture associated with the assembly.</summary>
    /// <returns>The culture name.</returns>
    public string? CultureName
    {
      get => this._cultureInfo?.Name;
      set => this._cultureInfo = value == null ? (CultureInfo) null : new CultureInfo(value);
    }

    /// <summary>Gets or sets the location of the assembly as a URL.</summary>
    /// <returns>A string that is the URL location of the assembly.</returns>
    public string? CodeBase
    {
      [RequiresAssemblyFiles("The code will return an empty string for assemblies embedded in a single-file app")] get => this._codeBase;
      set => this._codeBase = value;
    }

    /// <summary>Gets the URI, including escape characters, that represents the codebase.</summary>
    /// <returns>A URI with escape characters.</returns>
    [RequiresAssemblyFiles("The code will return an empty string for assemblies embedded in a single-file app")]
    public string? EscapedCodeBase => this._codeBase == null ? (string) null : AssemblyName.EscapeCodeBase(this._codeBase);

    /// <summary>Gets or sets a value that identifies the processor and bits-per-word of the platform targeted by an executable.</summary>
    /// <returns>One of the enumeration values that identifies the processor and bits-per-word of the platform targeted by an executable.</returns>
    public ProcessorArchitecture ProcessorArchitecture
    {
      get
      {
        int processorArchitecture = (int) (this._flags & (AssemblyNameFlags) 112) >> 4;
        if (processorArchitecture > 5)
          processorArchitecture = 0;
        return (ProcessorArchitecture) processorArchitecture;
      }
      set
      {
        int num = (int) (value & (ProcessorArchitecture.IA64 | ProcessorArchitecture.Amd64));
        if (num > 5)
          return;
        this._flags = (AssemblyNameFlags) ((long) this._flags & 4294967055L);
        this._flags |= (AssemblyNameFlags) (num << 4);
      }
    }

    /// <summary>Gets or sets a value that indicates what type of content the assembly contains.</summary>
    /// <returns>A value that indicates what type of content the assembly contains.</returns>
    public AssemblyContentType ContentType
    {
      get
      {
        int contentType = (int) (this._flags & (AssemblyNameFlags) 3584) >> 9;
        if (contentType > 1)
          contentType = 0;
        return (AssemblyContentType) contentType;
      }
      set
      {
        int num = (int) (value & (AssemblyContentType) 7);
        if (num > 1)
          return;
        this._flags = (AssemblyNameFlags) ((long) this._flags & 4294963711L);
        this._flags |= (AssemblyNameFlags) (num << 9);
      }
    }

    /// <summary>Makes a copy of this <see cref="T:System.Reflection.AssemblyName" /> object.</summary>
    /// <returns>An object that is a copy of this <see cref="T:System.Reflection.AssemblyName" /> object.</returns>
    public object Clone() => (object) new AssemblyName()
    {
      _name = this._name,
      _publicKey = (byte[]) this._publicKey?.Clone(),
      _publicKeyToken = (byte[]) this._publicKeyToken?.Clone(),
      _cultureInfo = this._cultureInfo,
      _version = this._version,
      _flags = this._flags,
      _codeBase = this._codeBase,
      _hashAlgorithm = this._hashAlgorithm,
      _versionCompatibility = this._versionCompatibility
    };

    /// <summary>Gets the <see cref="T:System.Reflection.AssemblyName" /> for a given file.</summary>
    /// <param name="assemblyFile">The path for the assembly whose <see cref="T:System.Reflection.AssemblyName" /> is to be returned.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="assemblyFile" /> is invalid, such as an assembly with an invalid culture.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have path discovery permission.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    /// <paramref name="assemblyFile" /> is not a valid assembly.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different sets of evidence.</exception>
    /// <returns>An object that represents the given assembly file.</returns>
    public static AssemblyName GetAssemblyName(string assemblyFile) => assemblyFile != null ? AssemblyName.GetFileInformationCore(assemblyFile) : throw new ArgumentNullException(nameof (assemblyFile));

    /// <summary>Gets the public key of the assembly.</summary>
    /// <exception cref="T:System.Security.SecurityException">A public key was provided (for example, by using the <see cref="M:System.Reflection.AssemblyName.SetPublicKey(System.Byte[])" /> method), but no public key token was provided.</exception>
    /// <returns>A byte array that contains the public key of the assembly.</returns>
    public byte[]? GetPublicKey() => this._publicKey;

    /// <summary>Sets the public key identifying the assembly.</summary>
    /// <param name="publicKey">A byte array containing the public key of the assembly.</param>
    public void SetPublicKey(byte[]? publicKey)
    {
      this._publicKey = publicKey;
      if (publicKey == null)
        this._flags &= ~AssemblyNameFlags.PublicKey;
      else
        this._flags |= AssemblyNameFlags.PublicKey;
    }

    /// <summary>Gets the public key token, which is the last 8 bytes of the SHA-1 hash of the public key under which the application or assembly is signed.</summary>
    /// <returns>A byte array that contains the public key token.</returns>
    public byte[]? GetPublicKeyToken() => this._publicKeyToken ?? (this._publicKeyToken = this.ComputePublicKeyToken());

    /// <summary>Sets the public key token, which is the last 8 bytes of the SHA-1 hash of the public key under which the application or assembly is signed.</summary>
    /// <param name="publicKeyToken">A byte array containing the public key token of the assembly.</param>
    public void SetPublicKeyToken(byte[]? publicKeyToken) => this._publicKeyToken = publicKeyToken;

    /// <summary>Gets or sets the attributes of the assembly.</summary>
    /// <returns>A value that represents the attributes of the assembly.</returns>
    public AssemblyNameFlags Flags
    {
      get => this._flags & (AssemblyNameFlags) -3825;
      set
      {
        this._flags &= (AssemblyNameFlags) 3824;
        this._flags |= value & (AssemblyNameFlags) -3825;
      }
    }

    /// <summary>Gets or sets the hash algorithm used by the assembly manifest.</summary>
    /// <returns>The hash algorithm used by the assembly manifest.</returns>
    public AssemblyHashAlgorithm HashAlgorithm
    {
      get => this._hashAlgorithm;
      set => this._hashAlgorithm = value;
    }

    /// <summary>Gets or sets the information related to the assembly's compatibility with other assemblies.</summary>
    /// <returns>A value that represents information about the assembly's compatibility with other assemblies.</returns>
    public AssemblyVersionCompatibility VersionCompatibility
    {
      get => this._versionCompatibility;
      set => this._versionCompatibility = value;
    }

    /// <summary>Gets or sets the public and private cryptographic key pair that is used to create a strong name signature for the assembly.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET 6+ only: In all cases.</exception>
    /// <returns>The public and private cryptographic key pair to be used to create a strong name for the assembly.</returns>
    [Obsolete("Strong name signing is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0017", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public StrongNameKeyPair? KeyPair
    {
      get => throw new PlatformNotSupportedException(SR.PlatformNotSupported_StrongNameSigning);
      set => throw new PlatformNotSupportedException(SR.PlatformNotSupported_StrongNameSigning);
    }

    /// <summary>Gets the full name of the assembly, also known as the display name.</summary>
    /// <returns>A string that is the full name of the assembly, also known as the display name.</returns>
    public string FullName => string.IsNullOrEmpty(this.Name) ? string.Empty : AssemblyNameFormatter.ComputeDisplayName(this.Name, this.Version, this.CultureName, this._publicKeyToken ?? this.ComputePublicKeyToken(), this.Flags, this.ContentType);

    /// <summary>Returns the full name of the assembly, also known as the display name.</summary>
    /// <returns>The full name of the assembly, or the class name if the full name cannot be determined.</returns>
    public override string ToString() => this.FullName ?? base.ToString();

    /// <summary>Gets serialization information with all the data needed to recreate an instance of this <see langword="AssemblyName" />.</summary>
    /// <param name="info">The object to be populated with serialization information.</param>
    /// <param name="context">The destination context of the serialization.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and is called back by the deserialization event when deserialization is complete.</summary>
    /// <param name="sender">The source of the deserialization event.</param>
    public void OnDeserialization(object? sender) => throw new PlatformNotSupportedException();

    /// <summary>Returns a value indicating whether two assembly names are the same. The comparison is based on the simple assembly names.</summary>
    /// <param name="reference">The reference assembly name.</param>
    /// <param name="definition">The assembly name that is compared to the reference assembly.</param>
    /// <returns>
    /// <see langword="true" /> if the simple assembly names are the same; otherwise, <see langword="false" />.</returns>
    public static bool ReferenceMatchesDefinition(AssemblyName? reference, AssemblyName? definition)
    {
      if (reference == definition)
        return true;
      if (reference == null)
        throw new ArgumentNullException(nameof (reference));
      if (definition == null)
        throw new ArgumentNullException(nameof (definition));
      return (reference.Name ?? string.Empty).Equals(definition.Name ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }


    #nullable disable
    [RequiresAssemblyFiles("The code will return an empty string for assemblies embedded in a single-file app")]
    internal static string EscapeCodeBase(string codebase)
    {
      if (codebase == null)
        return string.Empty;
      int destPos = 0;
      char[] chArray = AssemblyName.EscapeString(codebase, 0, codebase.Length, (char[]) null, ref destPos, true, char.MaxValue, char.MaxValue, char.MaxValue);
      return chArray == null ? codebase : new string(chArray, 0, destPos);
    }

    internal static unsafe char[] EscapeString(
      string input,
      int start,
      int end,
      char[] dest,
      ref int destPos,
      bool isUriString,
      char force1,
      char force2,
      char rsvd)
    {
      int currentInputPos = start;
      int prevInputPos = start;
      byte* bytes1 = stackalloc byte[160];
      IntPtr num1;
      if (input == null)
      {
        num1 = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &input.GetPinnableReference())
          num1 = (IntPtr) chPtr;
      }
      char* pStr = (char*) num1;
      for (; currentInputPos < end; ++currentInputPos)
      {
        char ch = pStr[currentInputPos];
        if (ch > '\u007F')
        {
          short num2 = (short) Math.Min(end - currentInputPos, 39);
          short charCount = 1;
          while ((int) charCount < (int) num2 && pStr[currentInputPos + (int) charCount] > '\u007F')
            ++charCount;
          if (pStr[currentInputPos + (int) charCount - 1] >= '\uD800' && pStr[currentInputPos + (int) charCount - 1] <= '\uDBFF')
          {
            if (charCount == (short) 1 || (int) charCount == end - currentInputPos)
              throw new FormatException(SR.Arg_FormatException);
            ++charCount;
          }
          dest = AssemblyName.EnsureDestinationSize(pStr, dest, currentInputPos, (short) ((int) charCount * 4 * 3), (short) 480, ref destPos, prevInputPos);
          short bytes2 = (short) Encoding.UTF8.GetBytes(pStr + currentInputPos, (int) charCount, bytes1, 160);
          if (bytes2 == (short) 0)
            throw new FormatException(SR.Arg_FormatException);
          currentInputPos += (int) charCount - 1;
          for (short index = 0; (int) index < (int) bytes2; ++index)
            AssemblyName.EscapeAsciiChar((char) bytes1[(int) index], dest, ref destPos);
          prevInputPos = currentInputPos + 1;
        }
        else if (ch == '%' && rsvd == '%')
        {
          dest = AssemblyName.EnsureDestinationSize(pStr, dest, currentInputPos, (short) 3, (short) 120, ref destPos, prevInputPos);
          if (currentInputPos + 2 < end && HexConverter.IsHexChar((int) pStr[currentInputPos + 1]) && HexConverter.IsHexChar((int) pStr[currentInputPos + 2]))
          {
            dest[destPos++] = '%';
            dest[destPos++] = pStr[currentInputPos + 1];
            dest[destPos++] = pStr[currentInputPos + 2];
            currentInputPos += 2;
          }
          else
            AssemblyName.EscapeAsciiChar('%', dest, ref destPos);
          prevInputPos = currentInputPos + 1;
        }
        else if ((int) ch == (int) force1 || (int) ch == (int) force2 || (int) ch != (int) rsvd && (isUriString ? (!AssemblyName.IsReservedUnreservedOrHash(ch) ? 1 : 0) : (!AssemblyName.IsUnreserved(ch) ? 1 : 0)) != 0)
        {
          dest = AssemblyName.EnsureDestinationSize(pStr, dest, currentInputPos, (short) 3, (short) 120, ref destPos, prevInputPos);
          AssemblyName.EscapeAsciiChar(ch, dest, ref destPos);
          prevInputPos = currentInputPos + 1;
        }
      }
      if (prevInputPos != currentInputPos && (prevInputPos != start || dest != null))
        dest = AssemblyName.EnsureDestinationSize(pStr, dest, currentInputPos, (short) 0, (short) 0, ref destPos, prevInputPos);
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(chPtr);
      return dest;
    }

    private static unsafe char[] EnsureDestinationSize(
      char* pStr,
      char[] dest,
      int currentInputPos,
      short charsToAdd,
      short minReallocateChars,
      ref int destPos,
      int prevInputPos)
    {
      if (dest == null || dest.Length < destPos + (currentInputPos - prevInputPos) + (int) charsToAdd)
      {
        char[] dst = new char[destPos + (currentInputPos - prevInputPos) + (int) minReallocateChars];
        if (dest != null && destPos != 0)
          Buffer.BlockCopy((Array) dest, 0, (Array) dst, 0, destPos << 1);
        dest = dst;
      }
      while (prevInputPos != currentInputPos)
        dest[destPos++] = pStr[prevInputPos++];
      return dest;
    }

    internal static void EscapeAsciiChar(char ch, char[] to, ref int pos)
    {
      to[pos++] = '%';
      to[pos++] = HexConverter.ToCharUpper((int) ch >> 4);
      to[pos++] = HexConverter.ToCharUpper((int) ch);
    }

    private static bool IsReservedUnreservedOrHash(char c) => AssemblyName.IsUnreserved(c) || ":/?#[]@!$&'()*+,;=".Contains(c);

    internal static bool IsUnreserved(char c) => AssemblyName.IsAsciiLetterOrDigit(c) || "-._~".Contains(c);

    internal static bool IsAsciiLetter(char character)
    {
      if (character >= 'a' && character <= 'z')
        return true;
      return character >= 'A' && character <= 'Z';
    }

    internal static bool IsAsciiLetterOrDigit(char character)
    {
      if (AssemblyName.IsAsciiLetter(character))
        return true;
      return character >= '0' && character <= '9';
    }
  }
}
