// Decompiled with JetBrains decompiler
// Type: System.Reflection.GenericParameterAttributes
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Describes the constraints on a generic type parameter of a generic type or method.</summary>
  [Flags]
  public enum GenericParameterAttributes
  {
    /// <summary>There are no special flags.</summary>
    None = 0,
    /// <summary>Selects the combination of all variance flags. This value is the result of using logical OR to combine the following flags: <see cref="F:System.Reflection.GenericParameterAttributes.Contravariant" /> and <see cref="F:System.Reflection.GenericParameterAttributes.Covariant" />.</summary>
    VarianceMask = 3,
    /// <summary>The generic type parameter is covariant. A covariant type parameter can appear as the result type of a method, the type of a read-only field, a declared base type, or an implemented interface.</summary>
    Covariant = 1,
    /// <summary>The generic type parameter is contravariant. A contravariant type parameter can appear as a parameter type in method signatures.</summary>
    Contravariant = 2,
    /// <summary>Selects the combination of all special constraint flags. This value is the result of using logical OR to combine the following flags: <see cref="F:System.Reflection.GenericParameterAttributes.DefaultConstructorConstraint" />, <see cref="F:System.Reflection.GenericParameterAttributes.ReferenceTypeConstraint" />, and <see cref="F:System.Reflection.GenericParameterAttributes.NotNullableValueTypeConstraint" />.</summary>
    SpecialConstraintMask = 28, // 0x0000001C
    /// <summary>A type can be substituted for the generic type parameter only if it is a reference type.</summary>
    ReferenceTypeConstraint = 4,
    /// <summary>A type can be substituted for the generic type parameter only if it is a value type and is not nullable.</summary>
    NotNullableValueTypeConstraint = 8,
    /// <summary>A type can be substituted for the generic type parameter only if it has a parameterless constructor.</summary>
    DefaultConstructorConstraint = 16, // 0x00000010
  }
}
