// Decompiled with JetBrains decompiler
// Type: System.Reflection.MethodInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Reflection
{
  /// <summary>Discovers the attributes of a method and provides access to method metadata.</summary>
  public abstract class MethodInfo : MethodBase
  {
    /// <summary>Gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a method.</summary>
    /// <returns>A <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a method.</returns>
    public override MemberTypes MemberType => MemberTypes.Method;

    /// <summary>Gets a <see cref="T:System.Reflection.ParameterInfo" /> object that contains information about the return type of the method, such as whether the return type has custom modifiers.</summary>
    /// <exception cref="T:System.NotImplementedException">This method is not implemented.</exception>
    /// <returns>A <see cref="T:System.Reflection.ParameterInfo" /> object that contains information about the return type.</returns>
    public virtual ParameterInfo ReturnParameter => throw NotImplemented.ByDesign;

    /// <summary>Gets the return type of this method.</summary>
    /// <returns>The return type of this method.</returns>
    public virtual Type ReturnType => throw NotImplemented.ByDesign;

    /// <summary>Returns an array of <see cref="T:System.Type" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition.</summary>
    /// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
    /// <returns>An array of <see cref="T:System.Type" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
    public override Type[] GetGenericArguments() => throw new NotSupportedException(SR.NotSupported_SubclassOverride);

    /// <summary>Returns a <see cref="T:System.Reflection.MethodInfo" /> object that represents a generic method definition from which the current method can be constructed.</summary>
    /// <exception cref="T:System.InvalidOperationException">The current method is not a generic method. That is, <see cref="P:System.Reflection.MethodBase.IsGenericMethod" /> returns <see langword="false" />.</exception>
    /// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object representing a generic method definition from which the current method can be constructed.</returns>
    public virtual MethodInfo GetGenericMethodDefinition() => throw new NotSupportedException(SR.NotSupported_SubclassOverride);

    /// <summary>Substitutes the elements of an array of types for the type parameters of the current generic method definition, and returns a <see cref="T:System.Reflection.MethodInfo" /> object representing the resulting constructed method.</summary>
    /// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic method definition.</param>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Reflection.MethodInfo" /> does not represent a generic method definition. That is, <see cref="P:System.Reflection.MethodBase.IsGenericMethodDefinition" /> returns <see langword="false" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="typeArguments" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// Any element of <paramref name="typeArguments" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in <paramref name="typeArguments" /> is not the same as the number of type parameters of the current generic method definition.
    /// 
    /// -or-
    /// 
    /// An element of <paramref name="typeArguments" /> does not satisfy the constraints specified for the corresponding type parameter of the current generic method definition.</exception>
    /// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object that represents the constructed method formed by substituting the elements of <paramref name="typeArguments" /> for the type parameters of the current generic method definition.</returns>
    [RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can't validate that the requirements of those annotations are met.")]
    public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments) => throw new NotSupportedException(SR.NotSupported_SubclassOverride);

    /// <summary>When overridden in a derived class, returns the <see cref="T:System.Reflection.MethodInfo" /> object for the method on the direct or indirect base class in which the method represented by this instance was first declared.</summary>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object for the first implementation of this method.</returns>
    public abstract MethodInfo GetBaseDefinition();

    /// <summary>Gets the custom attributes for the return type.</summary>
    /// <returns>An <see langword="ICustomAttributeProvider" /> object representing the custom attributes for the return type.</returns>
    public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

    /// <summary>Creates a delegate of the specified type from this method.</summary>
    /// <param name="delegateType">The type of the delegate to create.</param>
    /// <returns>The delegate for this method.</returns>
    public virtual Delegate CreateDelegate(Type delegateType) => throw new NotSupportedException(SR.NotSupported_SubclassOverride);

    /// <summary>Creates a delegate of the specified type with the specified target from this method.</summary>
    /// <param name="delegateType">The type of the delegate to create.</param>
    /// <param name="target">The object targeted by the delegate.</param>
    /// <returns>The delegate for this method.</returns>
    public virtual Delegate CreateDelegate(Type delegateType, object? target) => throw new NotSupportedException(SR.NotSupported_SubclassOverride);

    /// <summary>Creates a delegate of type <typeparamref name="T" /> from this method.</summary>
    /// <typeparam name="T">The type of the delegate to create.</typeparam>
    /// <returns>The delegate for this method.</returns>
    public T CreateDelegate<T>() where T : Delegate => (T) this.CreateDelegate(typeof (T));

    /// <summary>Creates a delegate of type <typeparamref name="T" /> with the specified target from this method.</summary>
    /// <param name="target">The object targeted by the delegate.</param>
    /// <typeparam name="T">The type of the delegate to create.</typeparam>
    /// <returns>The delegate for this method.</returns>
    public T CreateDelegate<T>(object? target) where T : Delegate => (T) this.CreateDelegate(typeof (T), target);

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Indicates whether two <see cref="T:System.Reflection.MethodInfo" /> objects are equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(MethodInfo? left, MethodInfo? right)
    {
      if ((object) right == null)
        return (object) left == null;
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>Indicates whether two <see cref="T:System.Reflection.MethodInfo" /> objects are not equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(MethodInfo? left, MethodInfo? right) => !(left == right);

    internal virtual int GenericParameterCount => this.GetGenericArguments().Length;
  }
}
