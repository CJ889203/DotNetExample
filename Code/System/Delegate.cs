// Decompiled with JetBrains decompiler
// Type: System.Delegate
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;


#nullable enable
namespace System
{
  /// <summary>Represents a delegate, which is a data structure that refers to a static method or to a class instance and an instance method of that class.</summary>
  [ClassInterface(ClassInterfaceType.None)]
  [ComVisible(true)]
  public abstract class Delegate : ICloneable, ISerializable
  {

    #nullable disable
    internal object _target;
    internal object _methodBase;
    internal IntPtr _methodPtr;
    internal IntPtr _methodPtrAux;


    #nullable enable
    /// <summary>Initializes a delegate that invokes the specified instance method on the specified class instance.</summary>
    /// <param name="target">The class instance on which the delegate invokes <paramref name="method" />.</param>
    /// <param name="method">The name of the instance method that the delegate represents.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="target" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">There was an error binding to the target method.</exception>
    [RequiresUnreferencedCode("The target method might be removed")]
    protected Delegate(object target, string method)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (method == null)
        throw new ArgumentNullException(nameof (method));
      if (!this.BindToMethodName(target, (RuntimeType) target.GetType(), method, DelegateBindingFlags.InstanceMethodOnly | DelegateBindingFlags.ClosedDelegateOnly))
        throw new ArgumentException(SR.Arg_DlgtTargMeth);
    }

    /// <summary>Initializes a delegate that invokes the specified static method from the specified class.</summary>
    /// <param name="target">The <see cref="T:System.Type" /> representing the class that defines <paramref name="method" />.</param>
    /// <param name="method">The name of the static method that the delegate represents.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="target" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="target" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> represents an open generic type.</exception>
    protected Delegate([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target, string method)
    {
      if (target == (Type) null)
        throw new ArgumentNullException(nameof (target));
      if (target.ContainsGenericParameters)
        throw new ArgumentException(SR.Arg_UnboundGenParam, nameof (target));
      if (method == null)
        throw new ArgumentNullException(nameof (method));
      if (!(target is RuntimeType methodType))
        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (target));
      this.BindToMethodName((object) null, methodType, method, DelegateBindingFlags.StaticMethodOnly | DelegateBindingFlags.OpenDelegateOnly | DelegateBindingFlags.CaselessMatching);
    }

    /// <summary>Dynamically invokes (late-bound) the method represented by the current delegate.</summary>
    /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.
    /// 
    /// -or-
    /// 
    /// <see langword="null" />, if the method represented by the current delegate does not require arguments.</param>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).
    /// 
    /// -or-
    /// 
    /// The number, order, or type of parameters listed in <paramref name="args" /> is invalid.</exception>
    /// <exception cref="T:System.ArgumentException">The method represented by the delegate is invoked on an object or a class that does not support it.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The method represented by the delegate is an instance method and the target object is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One of the encapsulated methods throws an exception.</exception>
    /// <returns>The object returned by the method represented by the delegate.</returns>
    protected virtual object? DynamicInvokeImpl(object?[]? args)
    {
      RuntimeMethodHandleInternal methodHandle = new RuntimeMethodHandleInternal(this.GetInvokeMethod());
      return RuntimeType.GetMethodBase((RuntimeType) this.GetType(), methodHandle).Invoke((object) this, BindingFlags.Default, (Binder) null, args, (CultureInfo) null);
    }

    /// <summary>Determines whether the specified object and the current delegate are of the same type and share the same targets, methods, and invocation list.</summary>
    /// <param name="obj">The object to compare with the current delegate.</param>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> and the current delegate have the same targets, methods, and invocation list; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
      if (obj == null || !Delegate.InternalEqualTypes((object) this, obj))
        return false;
      Delegate right = (Delegate) obj;
      if (this._target == right._target && this._methodPtr == right._methodPtr && this._methodPtrAux == right._methodPtrAux)
        return true;
      if (this._methodPtrAux == IntPtr.Zero)
      {
        if (right._methodPtrAux != IntPtr.Zero || this._target != right._target)
          return false;
      }
      else
      {
        if (right._methodPtrAux == IntPtr.Zero)
          return false;
        if (this._methodPtrAux == right._methodPtrAux)
          return true;
      }
      return this._methodBase == null || right._methodBase == null || (object) (this._methodBase as MethodInfo) == null || (object) (right._methodBase as MethodInfo) == null ? Delegate.InternalEqualMethodHandles(this, right) : this._methodBase.Equals(right._methodBase);
    }

    /// <summary>Returns a hash code for the delegate.</summary>
    /// <returns>A hash code for the delegate.</returns>
    public override int GetHashCode() => this._methodPtrAux == IntPtr.Zero ? (this._target != null ? RuntimeHelpers.GetHashCode(this._target) * 33 : 0) + this.GetType().GetHashCode() : this.GetType().GetHashCode();

    /// <summary>Gets the static method represented by the current delegate.</summary>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> describing the static method represented by the current delegate.</returns>
    protected virtual MethodInfo GetMethodImpl()
    {
      if (this._methodBase == null || (object) (this._methodBase as MethodInfo) == null)
      {
        IRuntimeMethodInfo methodHandle = this.FindMethodHandle();
        RuntimeType runtimeType = RuntimeMethodHandle.GetDeclaringType(methodHandle);
        if ((RuntimeTypeHandle.IsGenericTypeDefinition(runtimeType) || RuntimeTypeHandle.HasInstantiation(runtimeType)) && (RuntimeMethodHandle.GetAttributes(methodHandle) & MethodAttributes.Static) == 0)
        {
          if (this._methodPtrAux == IntPtr.Zero)
          {
            Type type = this._target.GetType();
            Type genericTypeDefinition = runtimeType.GetGenericTypeDefinition();
            for (; type != (Type) null; type = type.BaseType)
            {
              if (type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition)
              {
                runtimeType = type as RuntimeType;
                break;
              }
            }
          }
          else
            runtimeType = (RuntimeType) this.GetType().GetMethod("Invoke").GetParameters()[0].ParameterType;
        }
        this._methodBase = (object) (MethodInfo) RuntimeType.GetMethodBase(runtimeType, methodHandle);
      }
      return (MethodInfo) this._methodBase;
    }

    /// <summary>Gets the class instance on which the current delegate invokes the instance method.</summary>
    /// <returns>The object on which the current delegate invokes the instance method, if the delegate represents an instance method; <see langword="null" /> if the delegate represents a static method.</returns>
    public object? Target => this.GetTarget();

    /// <summary>Creates a delegate of the specified type that represents the specified instance method to invoke on the specified class instance, with the specified case-sensitivity and the specified behavior on failure to bind.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="target">The class instance on which <paramref name="method" /> is invoked.</param>
    /// <param name="method">The name of the instance method that the delegate is to represent.</param>
    /// <param name="ignoreCase">A Boolean indicating whether to ignore the case when comparing the name of the method.</param>
    /// <param name="throwOnBindFailure">
    /// <see langword="true" /> to throw an exception if <paramref name="method" /> cannot be bound; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not an instance method.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound, for example because it cannot be found, and <paramref name="throwOnBindFailure" /> is <see langword="true" />.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type that represents the specified instance method to invoke on the specified class instance.</returns>
    [RequiresUnreferencedCode("The target method might be removed")]
    public static Delegate? CreateDelegate(
      Type type,
      object target,
      string method,
      bool ignoreCase,
      bool throwOnBindFailure)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (method == null)
        throw new ArgumentNullException(nameof (method));
      if (!(type is RuntimeType type1))
        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (type));
      Delegate @delegate = type1.IsDelegate() ? (Delegate) Delegate.InternalAlloc(type1) : throw new ArgumentException(SR.Arg_MustBeDelegate, nameof (type));
      if (@delegate.BindToMethodName(target, (RuntimeType) target.GetType(), method, (DelegateBindingFlags) (26 | (ignoreCase ? 32 : 0))))
        return @delegate;
      if (throwOnBindFailure)
        throw new ArgumentException(SR.Arg_DlgtTargMeth);
      return (Delegate) null;
    }

    /// <summary>Creates a delegate of the specified type that represents the specified static method of the specified class, with the specified case-sensitivity and the specified behavior on failure to bind.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="target">The <see cref="T:System.Type" /> representing the class that implements <paramref name="method" />.</param>
    /// <param name="method">The name of the static method that the delegate is to represent.</param>
    /// <param name="ignoreCase">A Boolean indicating whether to ignore the case when comparing the name of the method.</param>
    /// <param name="throwOnBindFailure">
    /// <see langword="true" /> to throw an exception if <paramref name="method" /> cannot be bound; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is an open generic type. That is, its <see cref="P:System.Type.ContainsGenericParameters" /> property is <see langword="true" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not a <see langword="static" /> method (<see langword="Shared" /> method in Visual Basic).
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound, for example because it cannot be found, and <paramref name="throwOnBindFailure" /> is <see langword="true" />.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type that represents the specified static method of the specified class.</returns>
    public static Delegate? CreateDelegate(
      Type type,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target,
      string method,
      bool ignoreCase,
      bool throwOnBindFailure)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (target == (Type) null)
        throw new ArgumentNullException(nameof (target));
      if (target.ContainsGenericParameters)
        throw new ArgumentException(SR.Arg_UnboundGenParam, nameof (target));
      if (method == null)
        throw new ArgumentNullException(nameof (method));
      if (!(type is RuntimeType type1))
        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (type));
      if (!(target is RuntimeType methodType))
        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (target));
      Delegate delegate1 = type1.IsDelegate() ? (Delegate) Delegate.InternalAlloc(type1) : throw new ArgumentException(SR.Arg_MustBeDelegate, nameof (type));
      Delegate delegate2 = delegate1;
      string method1 = method;
      int flags = 5 | (ignoreCase ? 32 : 0);
      if (delegate2.BindToMethodName((object) null, methodType, method1, (DelegateBindingFlags) flags))
        return delegate1;
      if (throwOnBindFailure)
        throw new ArgumentException(SR.Arg_DlgtTargMeth);
      return (Delegate) null;
    }

    /// <summary>Creates a delegate of the specified type to represent the specified static method, with the specified behavior on failure to bind.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> describing the static or instance method the delegate is to represent.</param>
    /// <param name="throwOnBindFailure">
    /// <see langword="true" /> to throw an exception if <paramref name="method" /> cannot be bound; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound, and <paramref name="throwOnBindFailure" /> is <see langword="true" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not a <see langword="RuntimeMethodInfo" />. See Runtime Types in Reflection.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type to represent the specified static method.</returns>
    public static Delegate? CreateDelegate(
      Type type,
      MethodInfo method,
      bool throwOnBindFailure)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (method == (MethodInfo) null)
        throw new ArgumentNullException(nameof (method));
      if (!(type is RuntimeType rtType))
        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (type));
      if (!(method is RuntimeMethodInfo rtMethod))
        throw new ArgumentException(SR.Argument_MustBeRuntimeMethodInfo, nameof (method));
      Delegate @delegate = rtType.IsDelegate() ? Delegate.CreateDelegateInternal(rtType, rtMethod, (object) null, DelegateBindingFlags.OpenDelegateOnly | DelegateBindingFlags.RelaxedSignature) : throw new ArgumentException(SR.Arg_MustBeDelegate, nameof (type));
      return !((object) @delegate == null & throwOnBindFailure) ? @delegate : throw new ArgumentException(SR.Arg_DlgtTargMeth);
    }

    /// <summary>Creates a delegate of the specified type that represents the specified static or instance method, with the specified first argument and the specified behavior on failure to bind.</summary>
    /// <param name="type">A <see cref="T:System.Type" /> representing the type of delegate to create.</param>
    /// <param name="firstArgument">An <see cref="T:System.Object" /> that is the first argument of the method the delegate represents. For instance methods, it must be compatible with the instance type.</param>
    /// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> describing the static or instance method the delegate is to represent.</param>
    /// <param name="throwOnBindFailure">
    /// <see langword="true" /> to throw an exception if <paramref name="method" /> cannot be bound; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound, and <paramref name="throwOnBindFailure" /> is <see langword="true" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not a <see langword="RuntimeMethodInfo" />. See Runtime Types in Reflection.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type that represents the specified static or instance method, or <see langword="null" /> if <paramref name="throwOnBindFailure" /> is <see langword="false" /> and the delegate cannot be bound to <paramref name="method" />.</returns>
    public static Delegate? CreateDelegate(
      Type type,
      object? firstArgument,
      MethodInfo method,
      bool throwOnBindFailure)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (method == (MethodInfo) null)
        throw new ArgumentNullException(nameof (method));
      if (!(type is RuntimeType rtType))
        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (type));
      if (!(method is RuntimeMethodInfo rtMethod))
        throw new ArgumentException(SR.Argument_MustBeRuntimeMethodInfo, nameof (method));
      if (!rtType.IsDelegate())
        throw new ArgumentException(SR.Arg_MustBeDelegate, nameof (type));
      Delegate delegateInternal = Delegate.CreateDelegateInternal(rtType, rtMethod, firstArgument, DelegateBindingFlags.RelaxedSignature);
      return !((object) delegateInternal == null & throwOnBindFailure) ? delegateInternal : throw new ArgumentException(SR.Arg_DlgtTargMeth);
    }


    #nullable disable
    internal static Delegate CreateDelegateNoSecurityCheck(
      Type type,
      object target,
      RuntimeMethodHandle method)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (method.IsNullHandle())
        throw new ArgumentNullException(nameof (method));
      if (!(type is RuntimeType type1))
        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (type));
      Delegate delegateNoSecurityCheck = type1.IsDelegate() ? (Delegate) Delegate.InternalAlloc(type1) : throw new ArgumentException(SR.Arg_MustBeDelegate, nameof (type));
      if (!delegateNoSecurityCheck.BindToMethodInfo(target, method.GetMethodInfo(), RuntimeMethodHandle.GetDeclaringType(method.GetMethodInfo()), DelegateBindingFlags.RelaxedSignature))
        throw new ArgumentException(SR.Arg_DlgtTargMeth);
      return delegateNoSecurityCheck;
    }

    internal static Delegate CreateDelegateInternal(
      RuntimeType rtType,
      RuntimeMethodInfo rtMethod,
      object firstArgument,
      DelegateBindingFlags flags)
    {
      Delegate @delegate = (Delegate) Delegate.InternalAlloc(rtType);
      return @delegate.BindToMethodInfo(firstArgument, (IRuntimeMethodInfo) rtMethod, rtMethod.GetDeclaringTypeInternal(), flags) ? @delegate : (Delegate) null;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool BindToMethodName(
      object target,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] RuntimeType methodType,
      string method,
      DelegateBindingFlags flags);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool BindToMethodInfo(
      object target,
      IRuntimeMethodInfo method,
      RuntimeType methodType,
      DelegateBindingFlags flags);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern MulticastDelegate InternalAlloc(RuntimeType type);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern MulticastDelegate InternalAllocLike(Delegate d);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalEqualTypes(object a, object b);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void DelegateConstruct(object target, IntPtr slot);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern IntPtr GetMulticastInvoke();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern IntPtr GetInvokeMethod();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern IRuntimeMethodInfo FindMethodHandle();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalEqualMethodHandles(Delegate left, Delegate right);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern IntPtr AdjustTarget(object target, IntPtr methodPtr);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern IntPtr GetCallStub(IntPtr methodPtr);

    internal virtual object GetTarget() => !(this._methodPtrAux == IntPtr.Zero) ? (object) null : this._target;

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CompareUnmanagedFunctionPtrs(Delegate d1, Delegate d2);


    #nullable enable
    /// <summary>Creates a shallow copy of the delegate.</summary>
    /// <returns>A shallow copy of the delegate.</returns>
    public virtual object Clone() => this.MemberwiseClone();

    /// <summary>Concatenates the invocation lists of two delegates.</summary>
    /// <param name="a">The delegate whose invocation list comes first.</param>
    /// <param name="b">The delegate whose invocation list comes last.</param>
    /// <exception cref="T:System.ArgumentException">Both <paramref name="a" /> and <paramref name="b" /> are not <see langword="null" />, and <paramref name="a" /> and <paramref name="b" /> are not instances of the same delegate type.</exception>
    /// <returns>A new delegate with an invocation list that concatenates the invocation lists of <paramref name="a" /> and <paramref name="b" /> in that order. Returns <paramref name="a" /> if <paramref name="b" /> is <see langword="null" />, returns <paramref name="b" /> if <paramref name="a" /> is a null reference, and returns a null reference if both <paramref name="a" /> and <paramref name="b" /> are null references.</returns>
    [return: NotNullIfNotNull("a")]
    [return: NotNullIfNotNull("b")]
    public static Delegate? Combine(Delegate? a, Delegate? b) => (object) a == null ? b : a.CombineImpl(b);

    /// <summary>Concatenates the invocation lists of an array of delegates.</summary>
    /// <param name="delegates">The array of delegates to combine.</param>
    /// <exception cref="T:System.ArgumentException">Not all the non-null entries in <paramref name="delegates" /> are instances of the same delegate type.</exception>
    /// <returns>A new delegate with an invocation list that concatenates the invocation lists of the delegates in the <paramref name="delegates" /> array. Returns <see langword="null" /> if <paramref name="delegates" /> is <see langword="null" />, if <paramref name="delegates" /> contains zero elements, or if every entry in <paramref name="delegates" /> is <see langword="null" />.</returns>
    public static Delegate? Combine(params Delegate?[]? delegates)
    {
      if (delegates == null || delegates.Length == 0)
        return (Delegate) null;
      Delegate a = delegates[0];
      for (int index = 1; index < delegates.Length; ++index)
        a = Delegate.Combine(a, delegates[index]);
      return a;
    }

    /// <summary>Creates a delegate of the specified type that represents the specified static or instance method, with the specified first argument.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="firstArgument">The object to which the delegate is bound, or <see langword="null" /> to treat <paramref name="method" /> as <see langword="static" /> (<see langword="Shared" /> in Visual Basic).</param>
    /// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> describing the static or instance method the delegate is to represent.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not a <see langword="RuntimeMethodInfo" />. See Runtime Types in Reflection.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type that represents the specified static or instance method.</returns>
    public static Delegate CreateDelegate(
      Type type,
      object? firstArgument,
      MethodInfo method)
    {
      return Delegate.CreateDelegate(type, firstArgument, method, true);
    }

    /// <summary>Creates a delegate of the specified type to represent the specified static method.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> describing the static or instance method the delegate is to represent. Only static methods are supported in the .NET Framework version 1.0 and 1.1.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not a static method, and the .NET Framework version is 1.0 or 1.1.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not a <see langword="RuntimeMethodInfo" />. See Runtime Types in Reflection.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type to represent the specified static method.</returns>
    public static Delegate CreateDelegate(Type type, MethodInfo method) => Delegate.CreateDelegate(type, method, true);

    /// <summary>Creates a delegate of the specified type that represents the specified instance method to invoke on the specified class instance.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="target">The class instance on which <paramref name="method" /> is invoked.</param>
    /// <param name="method">The name of the instance method that the delegate is to represent.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not an instance method.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound, for example because it cannot be found.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type that represents the specified instance method to invoke on the specified class instance.</returns>
    [RequiresUnreferencedCode("The target method might be removed")]
    public static Delegate CreateDelegate(Type type, object target, string method) => Delegate.CreateDelegate(type, target, method, false, true);

    /// <summary>Creates a delegate of the specified type that represents the specified instance method to invoke on the specified class instance with the specified case-sensitivity.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="target">The class instance on which <paramref name="method" /> is invoked.</param>
    /// <param name="method">The name of the instance method that the delegate is to represent.</param>
    /// <param name="ignoreCase">A Boolean indicating whether to ignore the case when comparing the name of the method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not an instance method.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound, for example because it cannot be found.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type that represents the specified instance method to invoke on the specified class instance.</returns>
    [RequiresUnreferencedCode("The target method might be removed")]
    public static Delegate CreateDelegate(
      Type type,
      object target,
      string method,
      bool ignoreCase)
    {
      return Delegate.CreateDelegate(type, target, method, ignoreCase, true);
    }

    /// <summary>Creates a delegate of the specified type that represents the specified static method of the specified class.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="target">The <see cref="T:System.Type" /> representing the class that implements <paramref name="method" />.</param>
    /// <param name="method">The name of the static method that the delegate is to represent.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is an open generic type. That is, its <see cref="P:System.Type.ContainsGenericParameters" /> property is <see langword="true" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not a <see langword="static" /> method (<see langword="Shared" /> method in Visual Basic).
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound, for example because it cannot be found, and <paramref name="throwOnBindFailure" /> is <see langword="true" />.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type that represents the specified static method of the specified class.</returns>
    public static Delegate CreateDelegate(Type type, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target, string method) => Delegate.CreateDelegate(type, target, method, false, true);

    /// <summary>Creates a delegate of the specified type that represents the specified static method of the specified class, with the specified case-sensitivity.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> of delegate to create.</param>
    /// <param name="target">The <see cref="T:System.Type" /> representing the class that implements <paramref name="method" />.</param>
    /// <param name="method">The name of the static method that the delegate is to represent.</param>
    /// <param name="ignoreCase">A Boolean indicating whether to ignore the case when comparing the name of the method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />. See Runtime Types in Reflection.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is an open generic type. That is, its <see cref="P:System.Type.ContainsGenericParameters" /> property is <see langword="true" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> is not a <see langword="static" /> method (<see langword="Shared" /> method in Visual Basic).
    /// 
    /// -or-
    /// 
    /// <paramref name="method" /> cannot be bound, for example because it cannot be found.</exception>
    /// <exception cref="T:System.MissingMethodException">The <see langword="Invoke" /> method of <paramref name="type" /> is not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />.</exception>
    /// <returns>A delegate of the specified type that represents the specified static method of the specified class.</returns>
    public static Delegate CreateDelegate(
      Type type,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type target,
      string method,
      bool ignoreCase)
    {
      return Delegate.CreateDelegate(type, target, method, ignoreCase, true);
    }

    /// <summary>Concatenates the invocation lists of the specified multicast (combinable) delegate and the current multicast (combinable) delegate.</summary>
    /// <param name="d">The multicast (combinable) delegate whose invocation list to append to the end of the invocation list of the current multicast (combinable) delegate.</param>
    /// <exception cref="T:System.MulticastNotSupportedException">Always thrown.</exception>
    /// <returns>A new multicast (combinable) delegate with an invocation list that concatenates the invocation list of the current multicast (combinable) delegate and the invocation list of <paramref name="d" />, or the current multicast (combinable) delegate if <paramref name="d" /> is <see langword="null" />.</returns>
    protected virtual Delegate CombineImpl(Delegate? d) => throw new MulticastNotSupportedException(SR.Multicast_Combine);

    /// <summary>Removes the invocation list of a delegate from the invocation list of another delegate.</summary>
    /// <param name="d">The delegate that supplies the invocation list to remove from the invocation list of the current delegate.</param>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).</exception>
    /// <returns>A new delegate with an invocation list formed by taking the invocation list of the current delegate and removing the invocation list of <paramref name="value" />, if the invocation list of <paramref name="value" /> is found within the current delegate's invocation list. Returns the current delegate if <paramref name="value" /> is <see langword="null" /> or if the invocation list of <paramref name="value" /> is not found within the current delegate's invocation list. Returns <see langword="null" /> if the invocation list of <paramref name="value" /> is equal to the current delegate's invocation list.</returns>
    protected virtual Delegate? RemoveImpl(Delegate d) => !d.Equals((object) this) ? this : (Delegate) null;

    /// <summary>Returns the invocation list of the delegate.</summary>
    /// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
    public virtual Delegate[] GetInvocationList() => new Delegate[1]
    {
      this
    };

    /// <summary>Dynamically invokes (late-bound) the method represented by the current delegate.</summary>
    /// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.
    /// 
    /// -or-
    /// 
    /// <see langword="null" />, if the method represented by the current delegate does not require arguments.</param>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).
    /// 
    /// -or-
    /// 
    /// The number, order, or type of parameters listed in <paramref name="args" /> is invalid.</exception>
    /// <exception cref="T:System.ArgumentException">The method represented by the delegate is invoked on an object or a class that does not support it.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The method represented by the delegate is an instance method and the target object is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One of the encapsulated methods throws an exception.</exception>
    /// <returns>The object returned by the method represented by the delegate.</returns>
    public object? DynamicInvoke(params object?[]? args) => this.DynamicInvokeImpl(args);

    /// <summary>Not supported.</summary>
    /// <param name="info">Not supported.</param>
    /// <param name="context">Not supported.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    /// <summary>Gets the method represented by the delegate.</summary>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> describing the method represented by the delegate.</returns>
    public MethodInfo Method => this.GetMethodImpl();

    /// <summary>Removes the last occurrence of the invocation list of a delegate from the invocation list of another delegate.</summary>
    /// <param name="source">The delegate from which to remove the invocation list of <paramref name="value" />.</param>
    /// <param name="value">The delegate that supplies the invocation list to remove from the invocation list of <paramref name="source" />.</param>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).</exception>
    /// <exception cref="T:System.ArgumentException">The delegate types do not match.</exception>
    /// <returns>A new delegate with an invocation list formed by taking the invocation list of <paramref name="source" /> and removing the last occurrence of the invocation list of <paramref name="value" />, if the invocation list of <paramref name="value" /> is found within the invocation list of <paramref name="source" />. Returns <paramref name="source" /> if <paramref name="value" /> is <see langword="null" /> or if the invocation list of <paramref name="value" /> is not found within the invocation list of <paramref name="source" />. Returns a null reference if the invocation list of <paramref name="value" /> is equal to the invocation list of <paramref name="source" /> or if <paramref name="source" /> is a null reference.</returns>
    public static Delegate? Remove(Delegate? source, Delegate? value)
    {
      if ((object) source == null)
        return (Delegate) null;
      if ((object) value == null)
        return source;
      return Delegate.InternalEqualTypes((object) source, (object) value) ? source.RemoveImpl(value) : throw new ArgumentException(SR.Arg_DlgtTypeMis);
    }

    /// <summary>Removes all occurrences of the invocation list of a delegate from the invocation list of another delegate.</summary>
    /// <param name="source">The delegate from which to remove the invocation list of <paramref name="value" />.</param>
    /// <param name="value">The delegate that supplies the invocation list to remove from the invocation list of <paramref name="source" />.</param>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).</exception>
    /// <exception cref="T:System.ArgumentException">The delegate types do not match.</exception>
    /// <returns>A new delegate with an invocation list formed by taking the invocation list of <paramref name="source" /> and removing all occurrences of the invocation list of <paramref name="value" />, if the invocation list of <paramref name="value" /> is found within the invocation list of <paramref name="source" />. Returns <paramref name="source" /> if <paramref name="value" /> is <see langword="null" /> or if the invocation list of <paramref name="value" /> is not found within the invocation list of <paramref name="source" />. Returns a null reference if the invocation list of <paramref name="value" /> is equal to the invocation list of <paramref name="source" />, if <paramref name="source" /> contains only a series of invocation lists that are equal to the invocation list of <paramref name="value" />, or if <paramref name="source" /> is a null reference.</returns>
    public static Delegate? RemoveAll(Delegate? source, Delegate? value)
    {
      Delegate @delegate;
      do
      {
        @delegate = source;
        source = Delegate.Remove(source, value);
      }
      while (@delegate != source);
      return @delegate;
    }

    /// <summary>Determines whether the specified delegates are equal.</summary>
    /// <param name="d1">The first delegate to compare.</param>
    /// <param name="d2">The second delegate to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> is equal to <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Delegate? d1, Delegate? d2) => (object) d2 == null ? (object) d1 == null : (object) d2 == (object) d1 || d2.Equals((object) d1);

    /// <summary>Determines whether the specified delegates are not equal.</summary>
    /// <param name="d1">The first delegate to compare.</param>
    /// <param name="d2">The second delegate to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> is not equal to <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Delegate? d1, Delegate? d2) => (object) d2 == null ? (object) d1 != null : (object) d2 != (object) d1 && !d2.Equals((object) d1);
  }
}
