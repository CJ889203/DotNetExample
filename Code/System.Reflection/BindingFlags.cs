// Decompiled with JetBrains decompiler
// Type: System.Reflection.BindingFlags
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Specifies flags that control binding and the way in which the search for members and types is conducted by reflection.</summary>
  [Flags]
  public enum BindingFlags
  {
    /// <summary>Specifies that no binding flags are defined.</summary>
    Default = 0,
    /// <summary>Specifies that the case of the member name should not be considered when binding.</summary>
    IgnoreCase = 1,
    /// <summary>Specifies that only members declared at the level of the supplied type's hierarchy should be considered. Inherited members are not considered.</summary>
    DeclaredOnly = 2,
    /// <summary>Specifies that instance members are to be included in the search.</summary>
    Instance = 4,
    /// <summary>Specifies that static members are to be included in the search.</summary>
    Static = 8,
    /// <summary>Specifies that public members are to be included in the search.</summary>
    Public = 16, // 0x00000010
    /// <summary>Specifies that non-public members are to be included in the search.</summary>
    NonPublic = 32, // 0x00000020
    /// <summary>Specifies that public and protected static members up the hierarchy should be returned. Private static members in inherited classes are not returned. Static members include fields, methods, events, and properties. Nested types are not returned.</summary>
    FlattenHierarchy = 64, // 0x00000040
    /// <summary>Specifies that a method is to be invoked. This must not be a constructor or a type initializer.
    /// 
    /// This flag is passed to an <see langword="InvokeMember" /> method to invoke a method.</summary>
    InvokeMethod = 256, // 0x00000100
    /// <summary>Specifies that reflection should create an instance of the specified type. Calls the constructor that matches the given arguments. The supplied member name is ignored. If the type of lookup is not specified, (Instance | Public) will apply. It is not possible to call a type initializer.
    /// 
    /// This flag is passed to an <see langword="InvokeMember" /> method to invoke a constructor.</summary>
    CreateInstance = 512, // 0x00000200
    /// <summary>Specifies that the value of the specified field should be returned.
    /// 
    /// This flag is passed to an <see langword="InvokeMember" /> method to get a field value.</summary>
    GetField = 1024, // 0x00000400
    /// <summary>Specifies that the value of the specified field should be set.
    /// 
    /// This flag is passed to an <see langword="InvokeMember" /> method to set a field value.</summary>
    SetField = 2048, // 0x00000800
    /// <summary>Specifies that the value of the specified property should be returned.
    /// 
    /// This flag is passed to an <see langword="InvokeMember" /> method to invoke a property getter.</summary>
    GetProperty = 4096, // 0x00001000
    /// <summary>Specifies that the value of the specified property should be set. For COM properties, specifying this binding flag is equivalent to specifying <see langword="PutDispProperty" /> and <see langword="PutRefDispProperty" />.
    /// 
    /// This flag is passed to an <see langword="InvokeMember" /> method to invoke a property setter.</summary>
    SetProperty = 8192, // 0x00002000
    /// <summary>Specifies that the <see langword="PROPPUT" /> member on a COM object should be invoked. <see langword="PROPPUT" /> specifies a property-setting function that uses a value. Use <see langword="PutDispProperty" /> if a property has both <see langword="PROPPUT" /> and <see langword="PROPPUTREF" /> and you need to distinguish which one is called.</summary>
    PutDispProperty = 16384, // 0x00004000
    /// <summary>Specifies that the <see langword="PROPPUTREF" /> member on a COM object should be invoked. <see langword="PROPPUTREF" /> specifies a property-setting function that uses a reference instead of a value. Use <see langword="PutRefDispProperty" /> if a property has both <see langword="PROPPUT" /> and <see langword="PROPPUTREF" /> and you need to distinguish which one is called.</summary>
    PutRefDispProperty = 32768, // 0x00008000
    /// <summary>Specifies that types of the supplied arguments must exactly match the types of the corresponding formal parameters. Reflection throws an exception if the caller supplies a non-null <see langword="Binder" /> object, since that implies that the caller is supplying <see langword="BindToXXX" /> implementations that will pick the appropriate method. The default binder ignores this flag, while custom binders can implement the semantics of this flag.</summary>
    ExactBinding = 65536, // 0x00010000
    /// <summary>Not implemented.</summary>
    SuppressChangeType = 131072, // 0x00020000
    /// <summary>Returns the set of members whose parameter count matches the number of supplied arguments. This binding flag is used for methods with parameters that have default values and methods with variable arguments (varargs). This flag should only be used with <see cref="M:System.Type.InvokeMember(System.String,System.Reflection.BindingFlags,System.Reflection.Binder,System.Object,System.Object[],System.Reflection.ParameterModifier[],System.Globalization.CultureInfo,System.String[])" />.<br /><br />Parameters with default values are used only in calls where trailing arguments are omitted. They must be the last arguments.</summary>
    OptionalParamBinding = 262144, // 0x00040000
    /// <summary>Used in COM interop to specify that the return value of the member can be ignored.</summary>
    IgnoreReturn = 16777216, // 0x01000000
    DoNotWrapExceptions = 33554432, // 0x02000000
  }
}
