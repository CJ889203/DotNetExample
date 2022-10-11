// Decompiled with JetBrains decompiler
// Type: System.Reflection.MethodAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Specifies flags for method attributes. These flags are defined in the corhdr.h file.</summary>
  [Flags]
  public enum MethodAttributes
  {
    /// <summary>Retrieves accessibility information.</summary>
    MemberAccessMask = 7,
    /// <summary>Indicates that the member cannot be referenced.</summary>
    PrivateScope = 0,
    /// <summary>Indicates that the method is accessible only to the current class.</summary>
    Private = 1,
    /// <summary>Indicates that the method is accessible to members of this type and its derived types that are in this assembly only.</summary>
    FamANDAssem = 2,
    /// <summary>Indicates that the method is accessible to any class of this assembly.</summary>
    Assembly = FamANDAssem | Private, // 0x00000003
    /// <summary>Indicates that the method is accessible only to members of this class and its derived classes.</summary>
    Family = 4,
    /// <summary>Indicates that the method is accessible to derived classes anywhere, as well as to any class in the assembly.</summary>
    FamORAssem = Family | Private, // 0x00000005
    /// <summary>Indicates that the method is accessible to any object for which this object is in scope.</summary>
    Public = Family | FamANDAssem, // 0x00000006
    /// <summary>Indicates that the method is defined on the type; otherwise, it is defined per instance.</summary>
    Static = 16, // 0x00000010
    /// <summary>Indicates that the method cannot be overridden.</summary>
    Final = 32, // 0x00000020
    /// <summary>Indicates that the method is virtual.</summary>
    Virtual = 64, // 0x00000040
    /// <summary>Indicates that the method hides by name and signature; otherwise, by name only.</summary>
    HideBySig = 128, // 0x00000080
    /// <summary>Indicates that the method can only be overridden when it is also accessible.</summary>
    CheckAccessOnOverride = 512, // 0x00000200
    /// <summary>Retrieves vtable attributes.</summary>
    VtableLayoutMask = 256, // 0x00000100
    /// <summary>Indicates that the method will reuse an existing slot in the vtable. This is the default behavior.</summary>
    ReuseSlot = 0,
    /// <summary>Indicates that the method always gets a new slot in the vtable.</summary>
    NewSlot = VtableLayoutMask, // 0x00000100
    /// <summary>Indicates that the class does not provide an implementation of this method.</summary>
    Abstract = 1024, // 0x00000400
    /// <summary>Indicates that the method is special. The name describes how this method is special.</summary>
    SpecialName = 2048, // 0x00000800
    /// <summary>Indicates that the method implementation is forwarded through PInvoke (Platform Invocation Services).</summary>
    PinvokeImpl = 8192, // 0x00002000
    /// <summary>Indicates that the managed method is exported by thunk to unmanaged code.</summary>
    UnmanagedExport = 8,
    /// <summary>Indicates that the common language runtime checks the name encoding.</summary>
    RTSpecialName = 4096, // 0x00001000
    /// <summary>Indicates that the method has security associated with it. Reserved flag for runtime use only.</summary>
    HasSecurity = 16384, // 0x00004000
    /// <summary>Indicates that the method calls another method containing security code. Reserved flag for runtime use only.</summary>
    RequireSecObject = 32768, // 0x00008000
    /// <summary>Indicates a reserved flag for runtime use only.</summary>
    ReservedMask = RequireSecObject | HasSecurity | RTSpecialName, // 0x0000D000
  }
}
