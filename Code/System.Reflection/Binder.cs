// Decompiled with JetBrains decompiler
// Type: System.Reflection.Binder
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Globalization;


#nullable enable
namespace System.Reflection
{
  /// <summary>Selects a member from a list of candidates, and performs type conversion from actual argument type to formal argument type.</summary>
  public abstract class Binder
  {
    /// <summary>Selects a field from the given set of fields, based on the specified criteria.</summary>
    /// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values.</param>
    /// <param name="match">The set of fields that are candidates for matching. For example, when a <see cref="T:System.Reflection.Binder" /> object is used by <see cref="Overload:System.Type.InvokeMember" />, this parameter specifies the set of fields that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by <see cref="P:System.Type.DefaultBinder" /> changes the order of this array.</param>
    /// <param name="value">The field value used to locate a matching field.</param>
    /// <param name="culture">An instance of <see cref="T:System.Globalization.CultureInfo" /> that is used to control the coercion of data types, in binder implementations that coerce types. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">For the default binder, <paramref name="bindingAttr" /> includes <see cref="F:System.Reflection.BindingFlags.SetField" />, and <paramref name="match" /> contains multiple fields that are equally good matches for <paramref name="value" />. For example, <paramref name="value" /> contains a <c>MyClass</c> object that implements the <c>IMyClass</c> interface, and <paramref name="match" /> contains a field of type <c>MyClass</c> and a field of type <c>IMyClass</c>.</exception>
    /// <exception cref="T:System.MissingFieldException">For the default binder, <paramref name="bindingAttr" /> includes <see cref="F:System.Reflection.BindingFlags.SetField" />, and <paramref name="match" /> contains no fields that can accept <paramref name="value" />.</exception>
    /// <exception cref="T:System.NullReferenceException">For the default binder, <paramref name="bindingAttr" /> includes <see cref="F:System.Reflection.BindingFlags.SetField" />, and <paramref name="match" /> is <see langword="null" /> or an empty array.
    /// 
    /// -or-
    /// 
    /// <paramref name="bindingAttr" /> includes <see cref="F:System.Reflection.BindingFlags.SetField" />, and <paramref name="value" /> is <see langword="null" />.</exception>
    /// <returns>The matching field.</returns>
    public abstract FieldInfo BindToField(
      BindingFlags bindingAttr,
      FieldInfo[] match,
      object value,
      CultureInfo? culture);

    /// <summary>Selects a method to invoke from the given set of methods, based on the supplied arguments.</summary>
    /// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values.</param>
    /// <param name="match">The set of methods that are candidates for matching. For example, when a <see cref="T:System.Reflection.Binder" /> object is used by <see cref="Overload:System.Type.InvokeMember" />, this parameter specifies the set of methods that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by <see cref="P:System.Type.DefaultBinder" /> changes the order of this array.</param>
    /// <param name="args">The arguments that are passed in. The binder can change the order of the arguments in this array; for example, the default binder changes the order of arguments if the <paramref name="names" /> parameter is used to specify an order other than positional order. If a binder implementation coerces argument types, the types and values of the arguments can be changed as well.</param>
    /// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified. The default binder implementation does not use this parameter.</param>
    /// <param name="culture">An instance of <see cref="T:System.Globalization.CultureInfo" /> that is used to control the coercion of data types, in binder implementations that coerce types. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <param name="names">The parameter names, if parameter names are to be considered when matching, or <see langword="null" /> if arguments are to be treated as purely positional. For example, parameter names must be used if arguments are not supplied in positional order.</param>
    /// <param name="state">After the method returns, <paramref name="state" /> contains a binder-provided object that keeps track of argument reordering. The binder creates this object, and the binder is the sole consumer of this object. If <paramref name="state" /> is not <see langword="null" /> when <see langword="BindToMethod" /> returns, you must pass <paramref name="state" /> to the <see cref="M:System.Reflection.Binder.ReorderArgumentArray(System.Object[]@,System.Object)" /> method if you want to restore <paramref name="args" /> to its original order, for example, so that you can retrieve the values of <see langword="ref" /> parameters (<see langword="ByRef" /> parameters in Visual Basic).</param>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">For the default binder, <paramref name="match" /> contains multiple methods that are equally good matches for <paramref name="args" />. For example, <paramref name="args" /> contains a <c>MyClass</c> object that implements the <c>IMyClass</c> interface, and <paramref name="match" /> contains a method that takes <c>MyClass</c> and a method that takes <c>IMyClass</c>.</exception>
    /// <exception cref="T:System.MissingMethodException">For the default binder, <paramref name="match" /> contains no methods that can accept the arguments supplied in <paramref name="args" />.</exception>
    /// <exception cref="T:System.ArgumentException">For the default binder, <paramref name="match" /> is <see langword="null" /> or an empty array.</exception>
    /// <returns>The matching method.</returns>
    public abstract MethodBase BindToMethod(
      BindingFlags bindingAttr,
      MethodBase[] match,
      ref object?[] args,
      ParameterModifier[]? modifiers,
      CultureInfo? culture,
      string[]? names,
      out object? state);

    /// <summary>Changes the type of the given <see langword="Object" /> to the given <see langword="Type" />.</summary>
    /// <param name="value">The object to change into a new <see langword="Type" />.</param>
    /// <param name="type">The new <see langword="Type" /> that <paramref name="value" /> will become.</param>
    /// <param name="culture">An instance of <see cref="T:System.Globalization.CultureInfo" /> that is used to control the coercion of data types. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <returns>An object that contains the given value as the new type.</returns>
    public abstract object ChangeType(object value, Type type, CultureInfo? culture);

    /// <summary>Upon returning from <see cref="M:System.Reflection.Binder.BindToMethod(System.Reflection.BindingFlags,System.Reflection.MethodBase[],System.Object[]@,System.Reflection.ParameterModifier[],System.Globalization.CultureInfo,System.String[],System.Object@)" />, restores the <paramref name="args" /> argument to what it was when it came from <see langword="BindToMethod" />.</summary>
    /// <param name="args">The actual arguments that are passed in. Both the types and values of the arguments can be changed.</param>
    /// <param name="state">A binder-provided object that keeps track of argument reordering.</param>
    public abstract void ReorderArgumentArray(ref object?[] args, object state);

    /// <summary>Selects a method from the given set of methods, based on the argument type.</summary>
    /// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values.</param>
    /// <param name="match">The set of methods that are candidates for matching. For example, when a <see cref="T:System.Reflection.Binder" /> object is used by <see cref="Overload:System.Type.InvokeMember" />, this parameter specifies the set of methods that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by <see cref="P:System.Type.DefaultBinder" /> changes the order of this array.</param>
    /// <param name="types">The parameter types used to locate a matching method.</param>
    /// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified.</param>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">For the default binder, <paramref name="match" /> contains multiple methods that are equally good matches for the parameter types described by <paramref name="types" />. For example, the array in <paramref name="types" /> contains a <see cref="T:System.Type" /> object for <c>MyClass</c> and the array in <paramref name="match" /> contains a method that takes a base class of <c>MyClass</c> and a method that takes an interface that <c>MyClass</c> implements.</exception>
    /// <exception cref="T:System.ArgumentException">For the default binder, <paramref name="match" /> is <see langword="null" /> or an empty array.
    /// 
    /// -or-
    /// 
    /// An element of <paramref name="types" /> derives from <see cref="T:System.Type" />, but is not of type <see langword="RuntimeType" />.</exception>
    /// <returns>The matching method, if found; otherwise, <see langword="null" />.</returns>
    public abstract MethodBase? SelectMethod(
      BindingFlags bindingAttr,
      MethodBase[] match,
      Type[] types,
      ParameterModifier[]? modifiers);

    /// <summary>Selects a property from the given set of properties, based on the specified criteria.</summary>
    /// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values.</param>
    /// <param name="match">The set of properties that are candidates for matching. For example, when a <see cref="T:System.Reflection.Binder" /> object is used by <see cref="Overload:System.Type.InvokeMember" />, this parameter specifies the set of properties that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by <see cref="P:System.Type.DefaultBinder" /> changes the order of this array.</param>
    /// <param name="returnType">The return value the matching property must have.</param>
    /// <param name="indexes">The index types of the property being searched for. Used for index properties such as the indexer for a class.</param>
    /// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified.</param>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">For the default binder, <paramref name="match" /> contains multiple properties that are equally good matches for <paramref name="returnType" /> and <paramref name="indexes" />.</exception>
    /// <exception cref="T:System.ArgumentException">For the default binder, <paramref name="match" /> is <see langword="null" /> or an empty array.</exception>
    /// <returns>The matching property.</returns>
    public abstract PropertyInfo? SelectProperty(
      BindingFlags bindingAttr,
      PropertyInfo[] match,
      Type? returnType,
      Type[]? indexes,
      ParameterModifier[]? modifiers);
  }
}
