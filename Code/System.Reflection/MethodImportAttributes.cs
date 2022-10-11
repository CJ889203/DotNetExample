// Decompiled with JetBrains decompiler
// Type: System.Reflection.MethodImportAttributes
// Assembly: System.Reflection.Metadata, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: FDD13CB9-4DB5-4759-8B88-2D188C369E68
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.Metadata.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.Metadata.xml

namespace System.Reflection
{
  /// <summary>Specifies flags for the unmanaged method import attributes.</summary>
  [Flags]
  public enum MethodImportAttributes : short
  {
    /// <summary>Specifies default method import attributes.</summary>
    None = 0,
    /// <summary>Specifies that the Common Language Runtime should not try an entry-point names with charset-specific suffixes when searching for the imported method.</summary>
    ExactSpelling = 1,
    /// <summary>Specifies that the best-fit mapping behavior when converting Unicode characters to ANSI characters is disabled.</summary>
    BestFitMappingDisable = 32, // 0x0020
    /// <summary>Specifies that the best-fit mapping behavior when converting Unicode characters to ANSI characters is enabled.</summary>
    BestFitMappingEnable = 16, // 0x0010
    /// <summary>Specifies whether the best-fit mapping behavior when converting Unicode characters to ANSI characters is enabled or disabled.</summary>
    BestFitMappingMask = BestFitMappingEnable | BestFitMappingDisable, // 0x0030
    /// <summary>Specifies that strings are marshalled as multiple-byte character strings: the system default Windows (ANSI) code page on Windows, and UTF-8 on Unix.</summary>
    CharSetAnsi = 2,
    /// <summary>Specifies that strings are marshalled as Unicode 2-byte character strings.</summary>
    CharSetUnicode = 4,
    /// <summary>Specifies that the character set is chosen automatically. See Charsets and marshaling for details.</summary>
    CharSetAuto = CharSetUnicode | CharSetAnsi, // 0x0006
    /// <summary>Specifies the character set used for string marshalling.</summary>
    CharSetMask = CharSetAuto, // 0x0006
    /// <summary>Specifies that an exception should be thrown when an unmappable Unicode character is converted to an ANSI character.</summary>
    ThrowOnUnmappableCharEnable = 4096, // 0x1000
    /// <summary>Specifies that an exception should not be thrown when an unmappable Unicode character is converted to an ANSI character.</summary>
    ThrowOnUnmappableCharDisable = 8192, // 0x2000
    /// <summary>Specifies whether an exception should be thrown when an unmappable Unicode character is converted to an ANSI character.</summary>
    ThrowOnUnmappableCharMask = ThrowOnUnmappableCharDisable | ThrowOnUnmappableCharEnable, // 0x3000
    /// <summary>Specifies that the imported method calls the SetLastError Windows API function before returning.</summary>
    SetLastError = 64, // 0x0040
    /// <summary>Specifies that the default platform calling convention is used (StdCall on Windows x86, CDecl on Linux x86).</summary>
    CallingConventionWinApi = 256, // 0x0100
    /// <summary>Specifies that the calling convention is CDecl.</summary>
    CallingConventionCDecl = 512, // 0x0200
    /// <summary>Specifies that the calling convention is StdCall.</summary>
    CallingConventionStdCall = CallingConventionCDecl | CallingConventionWinApi, // 0x0300
    /// <summary>Specifies that the calling convention is ThisCall.</summary>
    CallingConventionThisCall = 1024, // 0x0400
    /// <summary>Specifies that the calling convention is FastCall.</summary>
    CallingConventionFastCall = CallingConventionThisCall | CallingConventionWinApi, // 0x0500
    /// <summary>Specifies the calling convention.</summary>
    CallingConventionMask = CallingConventionFastCall | CallingConventionCDecl, // 0x0700
  }
}
