// Decompiled with JetBrains decompiler
// Type: System.Reflection.EventInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Reflection
{
  /// <summary>Discovers the attributes of an event and provides access to event metadata.</summary>
  public abstract class EventInfo : MemberInfo
  {
    /// <summary>Gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is an event.</summary>
    /// <returns>A <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is an event.</returns>
    public override MemberTypes MemberType => MemberTypes.Event;

    /// <summary>Gets the attributes for this event.</summary>
    /// <returns>The read-only attributes for this event.</returns>
    public abstract EventAttributes Attributes { get; }

    /// <summary>Gets a value indicating whether the <see langword="EventInfo" /> has a name with a special meaning.</summary>
    /// <returns>
    /// <see langword="true" /> if this event has a special name; otherwise, <see langword="false" />.</returns>
    public bool IsSpecialName => (this.Attributes & EventAttributes.SpecialName) != 0;

    /// <summary>Returns the public methods that have been associated with an event in metadata using the <see langword=".other" /> directive.</summary>
    /// <returns>An array representing the public methods that have been associated with the event in metadata by using the <see langword=".other" /> directive. If there are no such public methods, an empty array is returned.</returns>
    public MethodInfo[] GetOtherMethods() => this.GetOtherMethods(false);

    /// <summary>Returns the methods that have been associated with the event in metadata using the <see langword=".other" /> directive, specifying whether to include non-public methods.</summary>
    /// <param name="nonPublic">
    /// <see langword="true" /> to include non-public methods; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.NotImplementedException">This method is not implemented.</exception>
    /// <returns>An array representing methods that have been associated with an event in metadata by using the <see langword=".other" /> directive. If there are no methods matching the specification, an empty array is returned.</returns>
    public virtual MethodInfo[] GetOtherMethods(bool nonPublic) => throw NotImplemented.ByDesign;

    /// <summary>Gets the <see cref="T:System.Reflection.MethodInfo" /> object for the <see cref="M:System.Reflection.EventInfo.AddEventHandler(System.Object,System.Delegate)" /> method of the event, including non-public methods.</summary>
    /// <returns>The <see cref="T:System.Reflection.MethodInfo" /> object for the <see cref="M:System.Reflection.EventInfo.AddEventHandler(System.Object,System.Delegate)" /> method.</returns>
    public virtual MethodInfo? AddMethod => this.GetAddMethod(true);

    /// <summary>Gets the <see langword="MethodInfo" /> object for removing a method of the event, including non-public methods.</summary>
    /// <returns>The <see langword="MethodInfo" /> object for removing a method of the event.</returns>
    public virtual MethodInfo? RemoveMethod => this.GetRemoveMethod(true);

    /// <summary>Gets the method that is called when the event is raised, including non-public methods.</summary>
    /// <returns>The method that is called when the event is raised.</returns>
    public virtual MethodInfo? RaiseMethod => this.GetRaiseMethod(true);

    /// <summary>Returns the method used to add an event handler delegate to the event source.</summary>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object representing the method used to add an event handler delegate to the event source.</returns>
    public MethodInfo? GetAddMethod() => this.GetAddMethod(false);

    /// <summary>Returns the method used to remove an event handler delegate from the event source.</summary>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object representing the method used to remove an event handler delegate from the event source.</returns>
    public MethodInfo? GetRemoveMethod() => this.GetRemoveMethod(false);

    /// <summary>Returns the method that is called when the event is raised.</summary>
    /// <returns>The method that is called when the event is raised.</returns>
    public MethodInfo? GetRaiseMethod() => this.GetRaiseMethod(false);

    /// <summary>When overridden in a derived class, retrieves the <see langword="MethodInfo" /> object for the <see cref="M:System.Reflection.EventInfo.AddEventHandler(System.Object,System.Delegate)" /> method of the event, specifying whether to return non-public methods.</summary>
    /// <param name="nonPublic">
    /// <see langword="true" /> if non-public methods can be returned; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.MethodAccessException">
    /// <paramref name="nonPublic" /> is <see langword="true" />, the method used to add an event handler delegate is non-public, and the caller does not have permission to reflect on non-public methods.</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object representing the method used to add an event handler delegate to the event source.</returns>
    public abstract MethodInfo? GetAddMethod(bool nonPublic);

    /// <summary>When overridden in a derived class, retrieves the <see langword="MethodInfo" /> object for removing a method of the event, specifying whether to return non-public methods.</summary>
    /// <param name="nonPublic">
    /// <see langword="true" /> if non-public methods can be returned; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.MethodAccessException">
    /// <paramref name="nonPublic" /> is <see langword="true" />, the method used to add an event handler delegate is non-public, and the caller does not have permission to reflect on non-public methods.</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object representing the method used to remove an event handler delegate from the event source.</returns>
    public abstract MethodInfo? GetRemoveMethod(bool nonPublic);

    /// <summary>When overridden in a derived class, returns the method that is called when the event is raised, specifying whether to return non-public methods.</summary>
    /// <param name="nonPublic">
    /// <see langword="true" /> if non-public methods can be returned; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.MethodAccessException">
    /// <paramref name="nonPublic" /> is <see langword="true" />, the method used to add an event handler delegate is non-public, and the caller does not have permission to reflect on non-public methods.</exception>
    /// <returns>A <see langword="MethodInfo" /> object that was called when the event was raised.</returns>
    public abstract MethodInfo? GetRaiseMethod(bool nonPublic);

    /// <summary>Gets a value indicating whether the event is multicast.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>
    /// <see langword="true" /> if the delegate is an instance of a multicast delegate; otherwise, <see langword="false" />.</returns>
    public virtual bool IsMulticast => typeof (MulticastDelegate).IsAssignableFrom(this.EventHandlerType);

    /// <summary>Gets the <see langword="Type" /> object of the underlying event-handler delegate associated with this event.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A read-only <see langword="Type" /> object representing the delegate event handler.</returns>
    public virtual Type? EventHandlerType
    {
      get
      {
        ParameterInfo[] parametersNoCopy = this.GetAddMethod(true).GetParametersNoCopy();
        Type c = typeof (Delegate);
        for (int index = 0; index < parametersNoCopy.Length; ++index)
        {
          Type parameterType = parametersNoCopy[index].ParameterType;
          if (parameterType.IsSubclassOf(c))
            return parameterType;
        }
        return (Type) null;
      }
    }

    /// <summary>Adds an event handler to an event source.</summary>
    /// <param name="target">The event source.</param>
    /// <param name="handler">Encapsulates a method or methods to be invoked when the event is raised by the target.</param>
    /// <exception cref="T:System.InvalidOperationException">The event does not have a public <see langword="add" /> accessor.</exception>
    /// <exception cref="T:System.ArgumentException">The handler that was passed in cannot be used.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have access permission to the member.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The <paramref name="target" /> parameter is <see langword="null" /> and the event is not static.
    /// 
    ///  -or-
    /// 
    ///  The <see cref="T:System.Reflection.EventInfo" /> is not declared on the target.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.</exception>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public virtual void AddEventHandler(object? target, Delegate? handler)
    {
      MethodInfo addMethod = this.GetAddMethod(false);
      if (addMethod == (MethodInfo) null)
        throw new InvalidOperationException(SR.InvalidOperation_NoPublicAddMethod);
      addMethod.Invoke(target, new object[1]
      {
        (object) handler
      });
    }

    /// <summary>Removes an event handler from an event source.</summary>
    /// <param name="target">The event source.</param>
    /// <param name="handler">The delegate to be disassociated from the events raised by target.</param>
    /// <exception cref="T:System.InvalidOperationException">The event does not have a public <see langword="remove" /> accessor.</exception>
    /// <exception cref="T:System.ArgumentException">The handler that was passed in cannot be used.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The <paramref name="target" /> parameter is <see langword="null" /> and the event is not static.
    /// 
    ///  -or-
    /// 
    ///  The <see cref="T:System.Reflection.EventInfo" /> is not declared on the target.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have access permission to the member.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public virtual void RemoveEventHandler(object? target, Delegate? handler)
    {
      MethodInfo removeMethod = this.GetRemoveMethod(false);
      if (removeMethod == (MethodInfo) null)
        throw new InvalidOperationException(SR.InvalidOperation_NoPublicRemoveMethod);
      removeMethod.Invoke(target, new object[1]
      {
        (object) handler
      });
    }

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Indicates whether two <see cref="T:System.Reflection.EventInfo" /> objects are equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(EventInfo? left, EventInfo? right)
    {
      if ((object) right == null)
        return (object) left == null;
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>Indicates whether two <see cref="T:System.Reflection.EventInfo" /> objects are not equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(EventInfo? left, EventInfo? right) => !(left == right);
  }
}
