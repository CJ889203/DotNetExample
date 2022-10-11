// Decompiled with JetBrains decompiler
// Type: System.Reflection.MethodBase
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides information about methods and constructors.</summary>
  public abstract class MethodBase : MemberInfo
  {
    /// <summary>Gets method information by using the method's internal metadata representation (handle).</summary>
    /// <param name="handle">The method's handle.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <returns>A <see langword="MethodBase" /> containing information about the method.</returns>
    public static MethodBase? GetMethodFromHandle(RuntimeMethodHandle handle)
    {
      MethodBase p1 = !handle.IsNullHandle() ? RuntimeType.GetMethodBase(handle.GetMethodInfo()) : throw new ArgumentException(SR.Argument_InvalidHandle);
      Type declaringType = p1?.DeclaringType;
      if (declaringType != (Type) null && declaringType.IsGenericType)
        throw new ArgumentException(SR.Format(SR.Argument_MethodDeclaringTypeGeneric, (object) p1, (object) declaringType.GetGenericTypeDefinition()));
      return p1;
    }

    /// <summary>Gets a <see cref="T:System.Reflection.MethodBase" /> object for the constructor or method represented by the specified handle, for the specified generic type.</summary>
    /// <param name="handle">A handle to the internal metadata representation of a constructor or method.</param>
    /// <param name="declaringType">A handle to the generic type that defines the constructor or method.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodBase" /> object representing the method or constructor specified by <paramref name="handle" />, in the generic type specified by <paramref name="declaringType" />.</returns>
    public static MethodBase? GetMethodFromHandle(
      RuntimeMethodHandle handle,
      RuntimeTypeHandle declaringType)
    {
      return !handle.IsNullHandle() ? RuntimeType.GetMethodBase(declaringType.GetRuntimeType(), handle.GetMethodInfo()) : throw new ArgumentException(SR.Argument_InvalidHandle);
    }

    /// <summary>Returns a <see langword="MethodBase" /> object representing the currently executing method.</summary>
    /// <exception cref="T:System.Reflection.TargetException">This member was invoked with a late-binding mechanism.</exception>
    /// <returns>
    ///        <see cref="M:System.Reflection.MethodBase.GetCurrentMethod" /> is a static method that is called from within an executing method and that returns information about that method.
    /// 
    /// A <see langword="MethodBase" /> object representing the currently executing method.</returns>
    public static MethodBase? GetCurrentMethod()
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return RuntimeMethodInfo.InternalGetCurrentMethod(ref stackMark);
    }

    private IntPtr GetMethodDesc() => this.MethodHandle.Value;


    #nullable disable
    internal virtual ParameterInfo[] GetParametersNoCopy() => this.GetParameters();

    internal virtual Type[] GetParameterTypes()
    {
      ParameterInfo[] parametersNoCopy = this.GetParametersNoCopy();
      Type[] parameterTypes = new Type[parametersNoCopy.Length];
      for (int index = 0; index < parametersNoCopy.Length; ++index)
        parameterTypes[index] = parametersNoCopy[index].ParameterType;
      return parameterTypes;
    }

    private protected Span<object> CheckArguments(
      ref MethodBase.StackAllocedArguments stackArgs,
      ReadOnlySpan<object> parameters,
      Binder binder,
      BindingFlags invokeAttr,
      CultureInfo culture,
      Signature sig)
    {
      Span<object> span = parameters.Length <= 4 ? MemoryMarshal.CreateSpan<object>(ref stackArgs._arg0, parameters.Length) : new Span<object>(new object[parameters.Length]);
      ParameterInfo[] parameterInfoArray = (ParameterInfo[]) null;
      for (int index = 0; index < parameters.Length; ++index)
      {
        object obj = parameters[index];
        RuntimeType runtimeType = sig.Arguments[index];
        if (obj == Type.Missing)
        {
          if (parameterInfoArray == null)
            parameterInfoArray = this.GetParametersNoCopy();
          if (parameterInfoArray[index].DefaultValue == DBNull.Value)
            throw new ArgumentException(SR.Arg_VarMissNull, nameof (parameters));
          obj = parameterInfoArray[index].DefaultValue;
        }
        span[index] = runtimeType.CheckValue(obj, binder, culture, invokeAttr);
      }
      return span;
    }


    #nullable enable
    /// <summary>When overridden in a derived class, gets the parameters of the specified method or constructor.</summary>
    /// <returns>An array of type <see langword="ParameterInfo" /> containing information that matches the signature of the method (or constructor) reflected by this <see langword="MethodBase" /> instance.</returns>
    public abstract ParameterInfo[] GetParameters();

    /// <summary>Gets the attributes associated with this method.</summary>
    /// <returns>One of the <see cref="T:System.Reflection.MethodAttributes" /> values.</returns>
    public abstract MethodAttributes Attributes { get; }

    /// <summary>Gets the <see cref="T:System.Reflection.MethodImplAttributes" /> flags that specify the attributes of a method implementation.</summary>
    /// <returns>The method implementation flags.</returns>
    public virtual MethodImplAttributes MethodImplementationFlags => this.GetMethodImplementationFlags();

    /// <summary>When overridden in a derived class, returns the <see cref="T:System.Reflection.MethodImplAttributes" /> flags.</summary>
    /// <returns>The <see langword="MethodImplAttributes" /> flags.</returns>
    public abstract MethodImplAttributes GetMethodImplementationFlags();

    /// <summary>When overridden in a derived class, gets a <see cref="T:System.Reflection.MethodBody" /> object that provides access to the MSIL stream, local variables, and exceptions for the current method.</summary>
    /// <exception cref="T:System.InvalidOperationException">This method is invalid unless overridden in a derived class.</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodBody" /> object that provides access to the MSIL stream, local variables, and exceptions for the current method.</returns>
    [RequiresUnreferencedCode("Trimming may change method bodies. For example it can change some instructions, remove branches or local variables.")]
    public virtual MethodBody? GetMethodBody() => throw new InvalidOperationException();

    /// <summary>Gets a value indicating the calling conventions for this method.</summary>
    /// <returns>The <see cref="T:System.Reflection.CallingConventions" /> for this method.</returns>
    public virtual CallingConventions CallingConvention => CallingConventions.Standard;

    /// <summary>Gets a value indicating whether the method is abstract.</summary>
    /// <returns>
    /// <see langword="true" /> if the method is abstract; otherwise, <see langword="false" />.</returns>
    public bool IsAbstract => (this.Attributes & MethodAttributes.Abstract) != 0;

    /// <summary>Gets a value indicating whether the method is a constructor.</summary>
    /// <returns>
    /// <see langword="true" /> if this method is a constructor represented by a <see cref="T:System.Reflection.ConstructorInfo" /> object (see note in Remarks about <see cref="T:System.Reflection.Emit.ConstructorBuilder" /> objects); otherwise, <see langword="false" />.</returns>
    public bool IsConstructor => (object) (this as ConstructorInfo) != null && !this.IsStatic && (this.Attributes & MethodAttributes.RTSpecialName) == MethodAttributes.RTSpecialName;

    /// <summary>Gets a value indicating whether this method is <see langword="final" />.</summary>
    /// <returns>
    /// <see langword="true" /> if this method is <see langword="final" />; otherwise, <see langword="false" />.</returns>
    public bool IsFinal => (this.Attributes & MethodAttributes.Final) != 0;

    /// <summary>Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class.</summary>
    /// <returns>
    /// <see langword="true" /> if the member is hidden by signature; otherwise, <see langword="false" />.</returns>
    public bool IsHideBySig => (this.Attributes & MethodAttributes.HideBySig) != 0;

    /// <summary>Gets a value indicating whether this method has a special name.</summary>
    /// <returns>
    /// <see langword="true" /> if this method has a special name; otherwise, <see langword="false" />.</returns>
    public bool IsSpecialName => (this.Attributes & MethodAttributes.SpecialName) != 0;

    /// <summary>Gets a value indicating whether the method is <see langword="static" />.</summary>
    /// <returns>
    /// <see langword="true" /> if this method is <see langword="static" />; otherwise, <see langword="false" />.</returns>
    public bool IsStatic => (this.Attributes & MethodAttributes.Static) != 0;

    /// <summary>Gets a value indicating whether the method is <see langword="virtual" />.</summary>
    /// <returns>
    /// <see langword="true" /> if this method is <see langword="virtual" />; otherwise, <see langword="false" />.</returns>
    public bool IsVirtual => (this.Attributes & MethodAttributes.Virtual) != 0;

    /// <summary>Gets a value indicating whether the potential visibility of this method or constructor is described by <see cref="F:System.Reflection.MethodAttributes.Assembly" />; that is, the method or constructor is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.</summary>
    /// <returns>
    /// <see langword="true" /> if the visibility of this method or constructor is exactly described by <see cref="F:System.Reflection.MethodAttributes.Assembly" />; otherwise, <see langword="false" />.</returns>
    public bool IsAssembly => (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;

    /// <summary>Gets a value indicating whether the visibility of this method or constructor is described by <see cref="F:System.Reflection.MethodAttributes.Family" />; that is, the method or constructor is visible only within its class and derived classes.</summary>
    /// <returns>
    /// <see langword="true" /> if access to this method or constructor is exactly described by <see cref="F:System.Reflection.MethodAttributes.Family" />; otherwise, <see langword="false" />.</returns>
    public bool IsFamily => (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;

    /// <summary>Gets a value indicating whether the visibility of this method or constructor is described by <see cref="F:System.Reflection.MethodAttributes.FamANDAssem" />; that is, the method or constructor can be called by derived classes, but only if they are in the same assembly.</summary>
    /// <returns>
    /// <see langword="true" /> if access to this method or constructor is exactly described by <see cref="F:System.Reflection.MethodAttributes.FamANDAssem" />; otherwise, <see langword="false" />.</returns>
    public bool IsFamilyAndAssembly => (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;

    /// <summary>Gets a value indicating whether the potential visibility of this method or constructor is described by <see cref="F:System.Reflection.MethodAttributes.FamORAssem" />; that is, the method or constructor can be called by derived classes wherever they are, and by classes in the same assembly.</summary>
    /// <returns>
    /// <see langword="true" /> if access to this method or constructor is exactly described by <see cref="F:System.Reflection.MethodAttributes.FamORAssem" />; otherwise, <see langword="false" />.</returns>
    public bool IsFamilyOrAssembly => (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;

    /// <summary>Gets a value indicating whether this member is private.</summary>
    /// <returns>
    /// <see langword="true" /> if access to this method is restricted to other members of the class itself; otherwise, <see langword="false" />.</returns>
    public bool IsPrivate => (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;

    /// <summary>Gets a value indicating whether this is a public method.</summary>
    /// <returns>
    /// <see langword="true" /> if this method is public; otherwise, <see langword="false" />.</returns>
    public bool IsPublic => (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;

    public virtual bool IsConstructedGenericMethod => this.IsGenericMethod && !this.IsGenericMethodDefinition;

    /// <summary>Gets a value indicating whether the method is generic.</summary>
    /// <returns>
    /// <see langword="true" /> if the current <see cref="T:System.Reflection.MethodBase" /> represents a generic method; otherwise, <see langword="false" />.</returns>
    public virtual bool IsGenericMethod => false;

    /// <summary>Gets a value indicating whether the method is a generic method definition.</summary>
    /// <returns>
    /// <see langword="true" /> if the current <see cref="T:System.Reflection.MethodBase" /> object represents the definition of a generic method; otherwise, <see langword="false" />.</returns>
    public virtual bool IsGenericMethodDefinition => false;

    /// <summary>Returns an array of <see cref="T:System.Type" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition.</summary>
    /// <exception cref="T:System.NotSupportedException">The current object is a <see cref="T:System.Reflection.ConstructorInfo" />. Generic constructors are not supported in the .NET Framework version 2.0. This exception is the default behavior if this method is not overridden in a derived class.</exception>
    /// <returns>An array of <see cref="T:System.Type" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
    public virtual Type[] GetGenericArguments() => throw new NotSupportedException(SR.NotSupported_SubclassOverride);

    /// <summary>Gets a value indicating whether the generic method contains unassigned generic type parameters.</summary>
    /// <returns>
    /// <see langword="true" /> if the current <see cref="T:System.Reflection.MethodBase" /> object represents a generic method that contains unassigned generic type parameters; otherwise, <see langword="false" />.</returns>
    public virtual bool ContainsGenericParameters => false;

    /// <summary>Invokes the method or constructor represented by the current instance, using the specified parameters.</summary>
    /// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be <see langword="null" /> or an instance of the class that defines the constructor.</param>
    /// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, <paramref name="parameters" /> should be <see langword="null" />.
    /// 
    /// If the method or constructor represented by this instance takes a <see langword="ref" /> parameter (<see langword="ByRef" /> in Visual Basic), no special attribute is required for that parameter in order to invoke the method or constructor using this function. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is <see langword="null" />. For value-type elements, this value is 0, 0.0, or <see langword="false" />, depending on the specific element type.</param>
    /// <exception cref="T:System.Reflection.TargetException">The <paramref name="obj" /> parameter is <see langword="null" /> and the method is not static.
    /// 
    ///  -or-
    /// 
    ///  The method is not declared or inherited by the class of <paramref name="obj" />.
    /// 
    ///  -or-
    /// 
    ///  A static constructor is invoked, and <paramref name="obj" /> is neither <see langword="null" /> nor an instance of the class that declared the constructor.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.</exception>
    /// <exception cref="T:System.ArgumentException">The elements of the <paramref name="parameters" /> array do not match the signature of the method or constructor reflected by this instance.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The invoked method or constructor throws an exception.
    /// 
    /// -or-
    /// 
    /// The current instance is a <see cref="T:System.Reflection.Emit.DynamicMethod" /> that contains unverifiable code. See the "Verification" section in Remarks for <see cref="T:System.Reflection.Emit.DynamicMethod" />.</exception>
    /// <exception cref="T:System.Reflection.TargetParameterCountException">The <paramref name="parameters" /> array does not have the correct number of arguments.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to execute the method or constructor that is represented by the current instance.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.InvalidOperationException">The type that declares the method is an open generic type. That is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" /> for the declaring type.</exception>
    /// <exception cref="T:System.NotSupportedException">The current instance is a <see cref="T:System.Reflection.Emit.MethodBuilder" />.</exception>
    /// <returns>An object containing the return value of the invoked method, or <see langword="null" /> in the case of a constructor.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public object? Invoke(object? obj, object?[]? parameters) => this.Invoke(obj, BindingFlags.Default, (Binder) null, parameters, (CultureInfo) null);

    /// <summary>When overridden in a derived class, invokes the reflected method or constructor with the given parameters.</summary>
    /// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be <see langword="null" /> or an instance of the class that defines the constructor.</param>
    /// <param name="invokeAttr">A bitmask that is a combination of 0 or more bit flags from <see cref="T:System.Reflection.BindingFlags" />. If <paramref name="binder" /> is <see langword="null" />, this parameter is assigned the value <see cref="F:System.Reflection.BindingFlags.Default" />; thus, whatever you pass in is ignored.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see langword="MemberInfo" /> objects via reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, this should be <see langword="null" />.
    /// 
    /// If the method or constructor represented by this instance takes a ByRef parameter, there is no special attribute required for that parameter in order to invoke the method or constructor using this function. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is <see langword="null" />. For value-type elements, this value is 0, 0.0, or <see langword="false" />, depending on the specific element type.</param>
    /// <param name="culture">An instance of <see langword="CultureInfo" /> used to govern the coercion of types. If this is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. (This is necessary to convert a string that represents 1000 to a <see cref="T:System.Double" /> value, for example, since 1000 is represented differently by different cultures.)</param>
    /// <exception cref="T:System.Reflection.TargetException">The <paramref name="obj" /> parameter is <see langword="null" /> and the method is not static.
    /// 
    /// -or-
    /// 
    /// The method is not declared or inherited by the class of <paramref name="obj" />.
    /// 
    /// -or-
    /// 
    /// A static constructor is invoked, and <paramref name="obj" /> is neither <see langword="null" /> nor an instance of the class that declared the constructor.</exception>
    /// <exception cref="T:System.ArgumentException">The type of the <paramref name="parameters" /> parameter does not match the signature of the method or constructor reflected by this instance.</exception>
    /// <exception cref="T:System.Reflection.TargetParameterCountException">The <paramref name="parameters" /> array does not have the correct number of arguments.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The invoked method or constructor throws an exception.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to execute the method or constructor that is represented by the current instance.</exception>
    /// <exception cref="T:System.InvalidOperationException">The type that declares the method is an open generic type. That is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" /> for the declaring type.</exception>
    /// <returns>An <see langword="Object" /> containing the return value of the invoked method, or <see langword="null" /> in the case of a constructor, or <see langword="null" /> if the method's return type is <see langword="void" />. Before calling the method or constructor, <see langword="Invoke" /> checks to see if the user has access permission and verifies that the parameters are valid.</returns>
    public abstract object? Invoke(
      object? obj,
      BindingFlags invokeAttr,
      Binder? binder,
      object?[]? parameters,
      CultureInfo? culture);

    /// <summary>Gets a handle to the internal metadata representation of a method.</summary>
    /// <returns>A <see cref="T:System.RuntimeMethodHandle" /> object.</returns>
    public abstract RuntimeMethodHandle MethodHandle { get; }

    /// <summary>Gets a value that indicates whether the current method or constructor is security-critical or security-safe-critical at the current trust level, and therefore can perform critical operations.</summary>
    /// <returns>
    /// <see langword="true" /> if the current method or constructor is security-critical or security-safe-critical at the current trust level; <see langword="false" /> if it is transparent.</returns>
    public virtual bool IsSecurityCritical => throw NotImplemented.ByDesign;

    /// <summary>Gets a value that indicates whether the current method or constructor is security-safe-critical at the current trust level; that is, whether it can perform critical operations and can be accessed by transparent code.</summary>
    /// <returns>
    /// <see langword="true" /> if the method or constructor is security-safe-critical at the current trust level; <see langword="false" /> if it is security-critical or transparent.</returns>
    public virtual bool IsSecuritySafeCritical => throw NotImplemented.ByDesign;

    /// <summary>Gets a value that indicates whether the current method or constructor is transparent at the current trust level, and therefore cannot perform critical operations.</summary>
    /// <returns>
    /// <see langword="true" /> if the method or constructor is security-transparent at the current trust level; otherwise, <see langword="false" />.</returns>
    public virtual bool IsSecurityTransparent => throw NotImplemented.ByDesign;

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Indicates whether two <see cref="T:System.Reflection.MethodBase" /> objects are equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(MethodBase? left, MethodBase? right)
    {
      if ((object) right == null)
        return (object) left == null;
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>Indicates whether two <see cref="T:System.Reflection.MethodBase" /> objects are not equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(MethodBase? left, MethodBase? right) => !(left == right);


    #nullable disable
    internal static void AppendParameters(
      ref ValueStringBuilder sbParamList,
      Type[] parameterTypes,
      CallingConventions callingConvention)
    {
      string s = "";
      for (int index = 0; index < parameterTypes.Length; ++index)
      {
        Type parameterType = parameterTypes[index];
        sbParamList.Append(s);
        string str = parameterType.FormatTypeName();
        if (parameterType.IsByRef)
        {
          sbParamList.Append(str.AsSpan().TrimEnd('&'));
          sbParamList.Append(" ByRef");
        }
        else
          sbParamList.Append(str);
        s = ", ";
      }
      if ((callingConvention & CallingConventions.VarArgs) != CallingConventions.VarArgs)
        return;
      sbParamList.Append(s);
      sbParamList.Append("...");
    }

    private protected struct StackAllocedArguments
    {
      internal object _arg0;
      private object _arg1;
      private object _arg2;
      private object _arg3;
    }
  }
}
