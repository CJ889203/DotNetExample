// Decompiled with JetBrains decompiler
// Type: System.Reflection.ProcessorArchitecture
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Identifies the processor and bits-per-word of the platform targeted by an executable.</summary>
  public enum ProcessorArchitecture
  {
    /// <summary>An unknown or unspecified combination of processor and bits-per-word.</summary>
    None,
    /// <summary>Neutral with respect to processor and bits-per-word.</summary>
    MSIL,
    /// <summary>A 32-bit Intel processor, either native or in the Windows on Windows environment on a 64-bit platform (WOW64).</summary>
    X86,
    /// <summary>A 64-bit Intel Itanium processor only.</summary>
    IA64,
    /// <summary>A 64-bit processor based on the x64 architecture.</summary>
    Amd64,
    /// <summary>An ARM processor.</summary>
    Arm,
  }
}
