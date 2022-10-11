// Decompiled with JetBrains decompiler
// Type: System.Reflection.ParameterAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Defines the attributes that can be associated with a parameter. These are defined in CorHdr.h.</summary>
  [Flags]
  public enum ParameterAttributes
  {
    /// <summary>Specifies that there is no parameter attribute.</summary>
    None = 0,
    /// <summary>Specifies that the parameter is an input parameter.</summary>
    In = 1,
    /// <summary>Specifies that the parameter is an output parameter.</summary>
    Out = 2,
    /// <summary>Specifies that the parameter is a locale identifier (lcid).</summary>
    Lcid = 4,
    /// <summary>Specifies that the parameter is a return value.</summary>
    Retval = 8,
    /// <summary>Specifies that the parameter is optional.</summary>
    Optional = 16, // 0x00000010
    /// <summary>Specifies that the parameter has a default value.</summary>
    HasDefault = 4096, // 0x00001000
    /// <summary>Specifies that the parameter has field marshaling information.</summary>
    HasFieldMarshal = 8192, // 0x00002000
    /// <summary>Reserved.</summary>
    Reserved3 = 16384, // 0x00004000
    /// <summary>Reserved.</summary>
    Reserved4 = 32768, // 0x00008000
    /// <summary>Specifies that the parameter is reserved.</summary>
    ReservedMask = Reserved4 | Reserved3 | HasFieldMarshal | HasDefault, // 0x0000F000
  }
}
