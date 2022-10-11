﻿// Decompiled with JetBrains decompiler
// Type: System.Reflection.MethodImplAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Specifies flags for the attributes of a method implementation.</summary>
  public enum MethodImplAttributes
  {
    /// <summary>Specifies that the method implementation is in Microsoft intermediate language (MSIL).</summary>
    IL = 0,
    /// <summary>Specifies that the method is implemented in managed code.</summary>
    Managed = 0,
    /// <summary>Specifies that the method implementation is native.</summary>
    Native = 1,
    /// <summary>Specifies that the method implementation is in Optimized Intermediate Language (OPTIL).</summary>
    OPTIL = 2,
    /// <summary>Specifies flags about code type.</summary>
    CodeTypeMask = 3,
    /// <summary>Specifies that the method implementation is provided by the runtime.</summary>
    Runtime = 3,
    /// <summary>Specifies whether the method is implemented in managed or unmanaged code.</summary>
    ManagedMask = 4,
    /// <summary>Specifies that the method is implemented in unmanaged code.</summary>
    Unmanaged = 4,
    /// <summary>Specifies that the method cannot be inlined.</summary>
    NoInlining = 8,
    /// <summary>Specifies that the method is not defined.</summary>
    ForwardRef = 16, // 0x00000010
    /// <summary>Specifies that the method is single-threaded through the body. Static methods (<see langword="Shared" /> in Visual Basic) lock on the type, whereas instance methods lock on the instance. You can also use the C# lock statement or the Visual Basic SyncLock statement for this purpose.</summary>
    Synchronized = 32, // 0x00000020
    /// <summary>Specifies that the method is not optimized by the just-in-time (JIT) compiler or by native code generation (see Ngen.exe) when debugging possible code generation problems.</summary>
    NoOptimization = 64, // 0x00000040
    /// <summary>Specifies that the method signature is exported exactly as declared.</summary>
    PreserveSig = 128, // 0x00000080
    /// <summary>Specifies that the method should be inlined wherever possible.</summary>
    AggressiveInlining = 256, // 0x00000100
    /// <summary>Specifies that the method should be optimized whenever possible.</summary>
    AggressiveOptimization = 512, // 0x00000200
    /// <summary>Specifies an internal call.</summary>
    InternalCall = 4096, // 0x00001000
    /// <summary>Specifies a range check value.</summary>
    MaxMethodImplVal = 65535, // 0x0000FFFF
  }
}
