// Decompiled with JetBrains decompiler
// Type: System.Reflection.EventAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Specifies the attributes of an event.</summary>
  [Flags]
  public enum EventAttributes
  {
    /// <summary>Specifies that the event has no attributes.</summary>
    None = 0,
    /// <summary>Specifies that the event is special in a way described by the name.</summary>
    SpecialName = 512, // 0x00000200
    /// <summary>Specifies that the common language runtime should check name encoding.</summary>
    RTSpecialName = 1024, // 0x00000400
    /// <summary>Specifies a reserved flag for common language runtime use only.</summary>
    ReservedMask = RTSpecialName, // 0x00000400
  }
}
