// Decompiled with JetBrains decompiler
// Type: System.Reflection.TypeAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Specifies type attributes.</summary>
  [Flags]
  public enum TypeAttributes
  {
    /// <summary>Specifies type visibility information.</summary>
    VisibilityMask = 7,
    /// <summary>Specifies that the class is not public.</summary>
    NotPublic = 0,
    /// <summary>Specifies that the class is public.</summary>
    Public = 1,
    /// <summary>Specifies that the class is nested with public visibility.</summary>
    NestedPublic = 2,
    /// <summary>Specifies that the class is nested with private visibility.</summary>
    NestedPrivate = NestedPublic | Public, // 0x00000003
    /// <summary>Specifies that the class is nested with family visibility, and is thus accessible only by methods within its own type and any derived types.</summary>
    NestedFamily = 4,
    /// <summary>Specifies that the class is nested with assembly visibility, and is thus accessible only by methods within its assembly.</summary>
    NestedAssembly = NestedFamily | Public, // 0x00000005
    /// <summary>Specifies that the class is nested with assembly and family visibility, and is thus accessible only by methods lying in the intersection of its family and assembly.</summary>
    NestedFamANDAssem = NestedFamily | NestedPublic, // 0x00000006
    /// <summary>Specifies that the class is nested with family or assembly visibility, and is thus accessible only by methods lying in the union of its family and assembly.</summary>
    NestedFamORAssem = NestedFamANDAssem | Public, // 0x00000007
    /// <summary>Specifies class layout information.</summary>
    LayoutMask = 24, // 0x00000018
    /// <summary>Specifies that class fields are automatically laid out by the common language runtime.</summary>
    AutoLayout = 0,
    /// <summary>Specifies that class fields are laid out sequentially, in the order that the fields were emitted to the metadata.</summary>
    SequentialLayout = 8,
    /// <summary>Specifies that class fields are laid out at the specified offsets.</summary>
    ExplicitLayout = 16, // 0x00000010
    /// <summary>Specifies class semantics information; the current class is contextful (else agile).</summary>
    ClassSemanticsMask = 32, // 0x00000020
    /// <summary>Specifies that the type is a class.</summary>
    Class = 0,
    /// <summary>Specifies that the type is an interface.</summary>
    Interface = ClassSemanticsMask, // 0x00000020
    /// <summary>Specifies that the type is abstract.</summary>
    Abstract = 128, // 0x00000080
    /// <summary>Specifies that the class is concrete and cannot be extended.</summary>
    Sealed = 256, // 0x00000100
    /// <summary>Specifies that the class is special in a way denoted by the name.</summary>
    SpecialName = 1024, // 0x00000400
    /// <summary>Specifies that the class or interface is imported from another module.</summary>
    Import = 4096, // 0x00001000
    /// <summary>Specifies that the class can be serialized.</summary>
    Serializable = 8192, // 0x00002000
    /// <summary>Specifies a Windows Runtime type.</summary>
    WindowsRuntime = 16384, // 0x00004000
    /// <summary>Used to retrieve string information for native interoperability.</summary>
    StringFormatMask = 196608, // 0x00030000
    /// <summary>LPTSTR is interpreted as ANSI.</summary>
    AnsiClass = 0,
    /// <summary>LPTSTR is interpreted as UNICODE.</summary>
    UnicodeClass = 65536, // 0x00010000
    /// <summary>LPTSTR is interpreted automatically.</summary>
    AutoClass = 131072, // 0x00020000
    /// <summary>LPSTR is interpreted by some implementation-specific means, which includes the possibility of throwing a <see cref="T:System.NotSupportedException" />. Not used in the Microsoft implementation of the .NET Framework.</summary>
    CustomFormatClass = AutoClass | UnicodeClass, // 0x00030000
    /// <summary>Used to retrieve non-standard encoding information for native interop. The meaning of the values of these 2 bits is unspecified. Not used in the Microsoft implementation of the .NET Framework.</summary>
    CustomFormatMask = 12582912, // 0x00C00000
    /// <summary>Specifies that calling static methods of the type does not force the system to initialize the type.</summary>
    BeforeFieldInit = 1048576, // 0x00100000
    /// <summary>Runtime should check name encoding.</summary>
    RTSpecialName = 2048, // 0x00000800
    /// <summary>Type has security associate with it.</summary>
    HasSecurity = 262144, // 0x00040000
    /// <summary>Attributes reserved for runtime use.</summary>
    ReservedMask = HasSecurity | RTSpecialName, // 0x00040800
  }
}
