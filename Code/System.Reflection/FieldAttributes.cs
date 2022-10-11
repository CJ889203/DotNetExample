// Decompiled with JetBrains decompiler
// Type: System.Reflection.FieldAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Specifies flags that describe the attributes of a field.</summary>
  [Flags]
  public enum FieldAttributes
  {
    /// <summary>Specifies the access level of a given field.</summary>
    FieldAccessMask = 7,
    /// <summary>Specifies that the field cannot be referenced.</summary>
    PrivateScope = 0,
    /// <summary>Specifies that the field is accessible only by the parent type.</summary>
    Private = 1,
    /// <summary>Specifies that the field is accessible only by subtypes in this assembly.</summary>
    FamANDAssem = 2,
    /// <summary>Specifies that the field is accessible throughout the assembly.</summary>
    Assembly = FamANDAssem | Private, // 0x00000003
    /// <summary>Specifies that the field is accessible only by type and subtypes.</summary>
    Family = 4,
    /// <summary>Specifies that the field is accessible by subtypes anywhere, as well as throughout this assembly.</summary>
    FamORAssem = Family | Private, // 0x00000005
    /// <summary>Specifies that the field is accessible by any member for whom this scope is visible.</summary>
    Public = Family | FamANDAssem, // 0x00000006
    /// <summary>Specifies that the field represents the defined type, or else it is per-instance.</summary>
    Static = 16, // 0x00000010
    /// <summary>Specifies that the field is initialized only, and can be set only in the body of a constructor.</summary>
    InitOnly = 32, // 0x00000020
    /// <summary>Specifies that the field's value is a compile-time (static or early bound) constant. Any attempt to set it throws a <see cref="T:System.FieldAccessException" />.</summary>
    Literal = 64, // 0x00000040
    /// <summary>Specifies that the field does not have to be serialized when the type is remoted.</summary>
    NotSerialized = 128, // 0x00000080
    /// <summary>Specifies a special method, with the name describing how the method is special.</summary>
    SpecialName = 512, // 0x00000200
    /// <summary>Reserved for future use.</summary>
    PinvokeImpl = 8192, // 0x00002000
    /// <summary>Specifies that the common language runtime (metadata internal APIs) should check the name encoding.</summary>
    RTSpecialName = 1024, // 0x00000400
    /// <summary>Specifies that the field has marshaling information.</summary>
    HasFieldMarshal = 4096, // 0x00001000
    /// <summary>Specifies that the field has a default value.</summary>
    HasDefault = 32768, // 0x00008000
    /// <summary>Specifies that the field has a relative virtual address (RVA). The RVA is the location of the method body in the current image, as an address relative to the start of the image file in which it is located.</summary>
    HasFieldRVA = 256, // 0x00000100
    /// <summary>Reserved.</summary>
    ReservedMask = HasFieldRVA | HasDefault | HasFieldMarshal | RTSpecialName, // 0x00009500
  }
}
