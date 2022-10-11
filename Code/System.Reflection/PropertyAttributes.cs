// Decompiled with JetBrains decompiler
// Type: System.Reflection.PropertyAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Defines the attributes that can be associated with a property. These attribute values are defined in corhdr.h.</summary>
  [Flags]
  public enum PropertyAttributes
  {
    /// <summary>Specifies that no attributes are associated with a property.</summary>
    None = 0,
    /// <summary>Specifies that the property is special, with the name describing how the property is special.</summary>
    SpecialName = 512, // 0x00000200
    /// <summary>Specifies that the metadata internal APIs check the name encoding.</summary>
    RTSpecialName = 1024, // 0x00000400
    /// <summary>Specifies that the property has a default value.</summary>
    HasDefault = 4096, // 0x00001000
    /// <summary>Reserved.</summary>
    Reserved2 = 8192, // 0x00002000
    /// <summary>Reserved.</summary>
    Reserved3 = 16384, // 0x00004000
    /// <summary>Reserved.</summary>
    Reserved4 = 32768, // 0x00008000
    /// <summary>Specifies a flag reserved for runtime use only.</summary>
    ReservedMask = Reserved4 | Reserved3 | Reserved2 | HasDefault | RTSpecialName, // 0x0000F400
  }
}
