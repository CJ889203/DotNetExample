// Decompiled with JetBrains decompiler
// Type: System.Threading.LazyInitializer
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Threading
{
  /// <summary>Provides lazy initialization routines.</summary>
  public static class LazyInitializer
  {
    /// <summary>Initializes a target reference type with the type's parameterless constructor if it hasn't already been initialized.</summary>
    /// <param name="target">A reference to initialize if it has not already been initialized. If it is <see langword="null" />, it is considered not initialized; otherwise, it's considered initialized.</param>
    /// <typeparam name="T">The type of the reference to be initialized.</typeparam>
    /// <exception cref="T:System.MemberAccessException">Permissions to access the constructor of type <paramref name="T" /> were missing.</exception>
    /// <exception cref="T:System.MissingMemberException">Type <paramref name="T" /> does not have a parameterless constructor.</exception>
    /// <returns>The initialized object.</returns>
    public static T EnsureInitialized<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(
      [NotNull] ref T? target)
      where T : class
    {
      return Volatile.Read<T>(ref target) ?? LazyInitializer.EnsureInitializedCore<T>(ref target);
    }


    #nullable disable
    private static T EnsureInitializedCore<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(
      [NotNull] ref T target)
      where T : class
    {
      try
      {
        Interlocked.CompareExchange<T>(ref target, Activator.CreateInstance<T>(), default (T));
      }
      catch (MissingMethodException ex)
      {
        throw new MissingMemberException(SR.Lazy_CreateValue_NoParameterlessCtorForT);
      }
      return target;
    }


    #nullable enable
    /// <summary>Initializes a target reference type by using a specified function if it hasn't already been initialized.</summary>
    /// <param name="target">The reference to initialize if it hasn't already been initialized.</param>
    /// <param name="valueFactory">The function that is called to initialize the reference.</param>
    /// <typeparam name="T">The type of the reference to be initialized.</typeparam>
    /// <exception cref="T:System.MissingMemberException">Type <paramref name="T" /> does not have a parameterless constructor.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="valueFactory" /> returned null (Nothing in Visual Basic).</exception>
    /// <returns>The initialized object.</returns>
    public static T EnsureInitialized<T>([NotNull] ref T? target, Func<T> valueFactory) where T : class => Volatile.Read<T>(ref target) ?? LazyInitializer.EnsureInitializedCore<T>(ref target, valueFactory);


    #nullable disable
    private static T EnsureInitializedCore<T>([NotNull] ref T target, Func<T> valueFactory) where T : class
    {
      Interlocked.CompareExchange<T>(ref target, valueFactory() ?? throw new InvalidOperationException(SR.Lazy_StaticInit_InvalidOperation), default (T));
      return target;
    }


    #nullable enable
    /// <summary>Initializes a target reference or value type with its parameterless constructor if it hasn't already been initialized.</summary>
    /// <param name="target">A reference or value of type <c>T</c> to initialize if it hasn't already been initialized.</param>
    /// <param name="initialized">A reference to a Boolean value that determines whether the target has already been initialized.</param>
    /// <param name="syncLock">A reference to an object used as the mutually exclusive lock for initializing <paramref name="target" />. If <paramref name="syncLock" /> is <see langword="null" />, a new object will be instantiated.</param>
    /// <typeparam name="T">The type of the reference to be initialized.</typeparam>
    /// <exception cref="T:System.MemberAccessException">Permissions to access the constructor of type <paramref name="T" /> were missing.</exception>
    /// <exception cref="T:System.MissingMemberException">Type <paramref name="T" /> does not have a parameterless constructor.</exception>
    /// <returns>The initialized object.</returns>
    public static T EnsureInitialized<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(
      [AllowNull] ref T target,
      ref bool initialized,
      [NotNullIfNotNull("syncLock")] ref object? syncLock)
    {
      return Volatile.Read(ref initialized) ? target : LazyInitializer.EnsureInitializedCore<T>(ref target, ref initialized, ref syncLock);
    }


    #nullable disable
    private static T EnsureInitializedCore<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(
      [AllowNull] ref T target,
      ref bool initialized,
      [NotNull] ref object syncLock)
    {
      lock (LazyInitializer.EnsureLockInitialized(ref syncLock))
      {
        if (!Volatile.Read(ref initialized))
        {
          try
          {
            target = Activator.CreateInstance<T>();
          }
          catch (MissingMethodException ex)
          {
            throw new MissingMemberException(SR.Lazy_CreateValue_NoParameterlessCtorForT);
          }
          Volatile.Write(ref initialized, true);
        }
      }
      return target;
    }


    #nullable enable
    /// <summary>Initializes a target reference or value type by using a specified function if it hasn't already been initialized.</summary>
    /// <param name="target">A reference or value of type <c>T</c> to initialize if it hasn't already been initialized.</param>
    /// <param name="initialized">A reference to a Boolean value that determines whether the target has already been initialized.</param>
    /// <param name="syncLock">A reference to an object used as the mutually exclusive lock for initializing <paramref name="target" />. If <paramref name="syncLock" /> is <see langword="null" />, a new object will be instantiated.</param>
    /// <param name="valueFactory">The function that is called to initialize the reference or value.</param>
    /// <typeparam name="T">The type of the reference to be initialized.</typeparam>
    /// <exception cref="T:System.MemberAccessException">Permissions to access the constructor of type <paramref name="T" /> were missing.</exception>
    /// <exception cref="T:System.MissingMemberException">Type <paramref name="T" /> does not have a parameterless constructor.</exception>
    /// <returns>The initialized object.</returns>
    public static T EnsureInitialized<T>(
      [AllowNull] ref T target,
      ref bool initialized,
      [NotNullIfNotNull("syncLock")] ref object? syncLock,
      Func<T> valueFactory)
    {
      return Volatile.Read(ref initialized) ? target : LazyInitializer.EnsureInitializedCore<T>(ref target, ref initialized, ref syncLock, valueFactory);
    }


    #nullable disable
    private static T EnsureInitializedCore<T>(
      [AllowNull] ref T target,
      ref bool initialized,
      [NotNull] ref object syncLock,
      Func<T> valueFactory)
    {
      lock (LazyInitializer.EnsureLockInitialized(ref syncLock))
      {
        if (!Volatile.Read(ref initialized))
        {
          target = valueFactory();
          Volatile.Write(ref initialized, true);
        }
      }
      return target;
    }


    #nullable enable
    /// <summary>Initializes a target reference type with a specified function if it has not already been initialized.</summary>
    /// <param name="target">A reference to initialize if it has not already been initialized. If it is <see langword="null" />, it is considered not initialized; otherwise, it's considered initialized.</param>
    /// <param name="syncLock">A reference to an object used as the mutually exclusive lock for initializing
    /// <paramref name="target" />. If <paramref name="syncLock" /> is <see langword="null" />, a new object will be instantiated.</param>
    /// <param name="valueFactory">The method to invoke to initialize <paramref name="target" />.</param>
    /// <typeparam name="T">The type of the reference to be initialized.</typeparam>
    /// <returns>The initialized object.</returns>
    public static T EnsureInitialized<T>([NotNull] ref T? target, [NotNullIfNotNull("syncLock")] ref object? syncLock, Func<T> valueFactory) where T : class => Volatile.Read<T>(ref target) ?? LazyInitializer.EnsureInitializedCore<T>(ref target, ref syncLock, valueFactory);


    #nullable disable
    private static T EnsureInitializedCore<T>(
      [NotNull] ref T target,
      [NotNull] ref object syncLock,
      Func<T> valueFactory)
      where T : class
    {
      lock (LazyInitializer.EnsureLockInitialized(ref syncLock))
      {
        if ((object) Volatile.Read<T>(ref target) == null)
        {
          Volatile.Write<T>(ref target, valueFactory());
          if ((object) target == null)
            throw new InvalidOperationException(SR.Lazy_StaticInit_InvalidOperation);
        }
      }
      return target;
    }

    private static object EnsureLockInitialized([NotNull] ref object syncLock) => syncLock ?? Interlocked.CompareExchange(ref syncLock, new object(), (object) null) ?? syncLock;
  }
}
