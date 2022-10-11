// Decompiled with JetBrains decompiler
// Type: System.Reflection.MemberTypes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Marks each type of member that is defined as a derived class of <see cref="T:System.Reflection.MemberInfo" />.</summary>
  [Flags]
  public enum MemberTypes
  {
    /// <summary>Specifies that the member is a constructor.</summary>
    Constructor = 1,
    /// <summary>Specifies that the member is an event.</summary>
    Event = 2,
    /// <summary>Specifies that the member is a field.</summary>
    Field = 4,
    /// <summary>Specifies that the member is a method.</summary>
    Method = 8,
    /// <summary>Specifies that the member is a property.</summary>
    Property = 16, // 0x00000010
    /// <summary>Specifies that the member is a type.</summary>
    TypeInfo = 32, // 0x00000020
    /// <summary>Specifies that the member is a custom member type.</summary>
    Custom = 64, // 0x00000040
    /// <summary>Specifies that the member is a nested type.</summary>
    NestedType = 128, // 0x00000080
    /// <summary>Specifies all member types.</summary>
    All = NestedType | TypeInfo | Property | Method | Field | Event | Constructor, // 0x000000BF
  }
}
