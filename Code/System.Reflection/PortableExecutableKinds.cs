// Decompiled with JetBrains decompiler
// Type: System.Reflection.PortableExecutableKinds
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Identifies the nature of the code in an executable file.</summary>
  [Flags]
  public enum PortableExecutableKinds
  {
    /// <summary>The file is not in portable executable (PE) file format.</summary>
    NotAPortableExecutableImage = 0,
    /// <summary>The executable contains only Microsoft intermediate language (MSIL), and is therefore neutral with respect to 32-bit or 64-bit platforms.</summary>
    ILOnly = 1,
    /// <summary>The executable can be run on a 32-bit platform, or in the 32-bit Windows on Windows (WOW) environment on a 64-bit platform.</summary>
    Required32Bit = 2,
    /// <summary>The executable requires a 64-bit platform.</summary>
    PE32Plus = 4,
    /// <summary>The executable contains pure unmanaged code.</summary>
    Unmanaged32Bit = 8,
    /// <summary>The executable is platform-agnostic but should be run on a 32-bit platform whenever possible.</summary>
    Preferred32Bit = 16, // 0x00000010
  }
}
