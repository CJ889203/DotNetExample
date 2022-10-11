// Decompiled with JetBrains decompiler
// Type: System.Lazy`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Provides support for lazy initialization.</summary>
  /// <typeparam name="T">The type of object that is being lazily initialized.</typeparam>
  [DebuggerTypeProxy(typeof (LazyDebugView<>))]
  [DebuggerDisplay("ThreadSafetyMode={Mode}, IsValueCreated={IsValueCreated}, IsValueFaulted={IsValueFaulted}, Value={ValueForDebugDisplay}")]
  public class Lazy<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>
  {

    #nullable disable
    private volatile LazyHelper _state;
    private Func<T> _factory;
    private T _value;

    private static T CreateViaDefaultConstructor() => LazyHelper.CreateViaDefaultConstructor<T>();

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`1" /> class. When lazy initialization occurs, the parameterless constructor of the target type is used.</summary>
    public Lazy()
      : this((Func<T>) null, LazyThreadSafetyMode.ExecutionAndPublication, true)
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`1" /> class that uses a preinitialized specified value.</summary>
    /// <param name="value">The preinitialized value to be used.</param>
    public Lazy(T value) => this._value = value;

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`1" /> class. When lazy initialization occurs, the specified initialization function is used.</summary>
    /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="valueFactory" /> is <see langword="null" />.</exception>
    public Lazy(Func<T> valueFactory)
      : this(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`1" /> class. When lazy initialization occurs, the parameterless constructor of the target type and the specified initialization mode are used.</summary>
    /// <param name="isThreadSafe">
    /// <see langword="true" /> to make this instance usable concurrently by multiple threads; <see langword="false" /> to make the instance usable by only one thread at a time.</param>
    public Lazy(bool isThreadSafe)
      : this((Func<T>) null, LazyHelper.GetModeFromIsThreadSafe(isThreadSafe), true)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`1" /> class that uses the parameterless constructor of <paramref name="T" /> and the specified thread-safety mode.</summary>
    /// <param name="mode">One of the enumeration values that specifies the thread safety mode.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> contains an invalid value.</exception>
    public Lazy(LazyThreadSafetyMode mode)
      : this((Func<T>) null, mode, true)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`1" /> class. When lazy initialization occurs, the specified initialization function and initialization mode are used.</summary>
    /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
    /// <param name="isThreadSafe">
    /// <see langword="true" /> to make this instance usable concurrently by multiple threads; <see langword="false" /> to make this instance usable by only one thread at a time.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="valueFactory" /> is <see langword="null" />.</exception>
    public Lazy(Func<T> valueFactory, bool isThreadSafe)
      : this(valueFactory, LazyHelper.GetModeFromIsThreadSafe(isThreadSafe), false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`1" /> class that uses the specified initialization function and thread-safety mode.</summary>
    /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
    /// <param name="mode">One of the enumeration values that specifies the thread safety mode.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> contains an invalid value.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="valueFactory" /> is <see langword="null" />.</exception>
    public Lazy(Func<T> valueFactory, LazyThreadSafetyMode mode)
      : this(valueFactory, mode, false)
    {
    }


    #nullable disable
    private Lazy(Func<T> valueFactory, LazyThreadSafetyMode mode, bool useDefaultConstructor)
    {
      this._factory = valueFactory != null || useDefaultConstructor ? valueFactory : throw new ArgumentNullException(nameof (valueFactory));
      this._state = LazyHelper.Create(mode, useDefaultConstructor);
    }

    private void ViaConstructor()
    {
      this._value = Lazy<T>.CreateViaDefaultConstructor();
      this._state = (LazyHelper) null;
    }

    private void ViaFactory(LazyThreadSafetyMode mode)
    {
      try
      {
        Func<T> factory = this._factory;
        if (factory == null)
          throw new InvalidOperationException(SR.Lazy_Value_RecursiveCallsToValue);
        this._factory = (Func<T>) null;
        this._value = factory();
        this._state = (LazyHelper) null;
      }
      catch (Exception ex)
      {
        this._state = new LazyHelper(mode, ex);
        throw;
      }
    }

    private void ExecutionAndPublication(
      LazyHelper executionAndPublication,
      bool useDefaultConstructor)
    {
      lock (executionAndPublication)
      {
        if (this._state != executionAndPublication)
          return;
        if (useDefaultConstructor)
          this.ViaConstructor();
        else
          this.ViaFactory(LazyThreadSafetyMode.ExecutionAndPublication);
      }
    }

    private void PublicationOnly(LazyHelper publicationOnly, T possibleValue)
    {
      if (Interlocked.CompareExchange<LazyHelper>(ref this._state, LazyHelper.PublicationOnlyWaitForOtherThreadToPublish, publicationOnly) != publicationOnly)
        return;
      this._factory = (Func<T>) null;
      this._value = possibleValue;
      this._state = (LazyHelper) null;
    }

    private void PublicationOnlyViaConstructor(LazyHelper initializer) => this.PublicationOnly(initializer, Lazy<T>.CreateViaDefaultConstructor());

    private void PublicationOnlyViaFactory(LazyHelper initializer)
    {
      Func<T> factory = this._factory;
      if (factory == null)
        this.PublicationOnlyWaitForOtherThreadToPublish();
      else
        this.PublicationOnly(initializer, factory());
    }

    private void PublicationOnlyWaitForOtherThreadToPublish()
    {
      SpinWait spinWait = new SpinWait();
      while (this._state != null)
        spinWait.SpinOnce();
    }

    private T CreateValue()
    {
      LazyHelper state = this._state;
      if (state != null)
      {
        switch (state.State)
        {
          case LazyState.NoneViaConstructor:
            this.ViaConstructor();
            break;
          case LazyState.NoneViaFactory:
            this.ViaFactory(LazyThreadSafetyMode.None);
            break;
          case LazyState.PublicationOnlyViaConstructor:
            this.PublicationOnlyViaConstructor(state);
            break;
          case LazyState.PublicationOnlyViaFactory:
            this.PublicationOnlyViaFactory(state);
            break;
          case LazyState.PublicationOnlyWait:
            this.PublicationOnlyWaitForOtherThreadToPublish();
            break;
          case LazyState.ExecutionAndPublicationViaConstructor:
            this.ExecutionAndPublication(state, true);
            break;
          case LazyState.ExecutionAndPublicationViaFactory:
            this.ExecutionAndPublication(state, false);
            break;
          default:
            state.ThrowException();
            break;
        }
      }
      return this.Value;
    }


    #nullable enable
    /// <summary>Creates and returns a string representation of the <see cref="P:System.Lazy`1.Value" /> property for this instance.</summary>
    /// <exception cref="T:System.NullReferenceException">The <see cref="P:System.Lazy`1.Value" /> property is <see langword="null" />.</exception>
    /// <returns>The result of calling the <see cref="M:System.Object.ToString" /> method on the <see cref="P:System.Lazy`1.Value" /> property for this instance, if the value has been created (that is, if the <see cref="P:System.Lazy`1.IsValueCreated" /> property returns <see langword="true" />). Otherwise, a string indicating that the value has not been created.</returns>
    public override string? ToString() => !this.IsValueCreated ? SR.Lazy_ToString_ValueNotCreated : this.Value.ToString();

    internal T? ValueForDebugDisplay => !this.IsValueCreated ? default (T) : this._value;

    internal LazyThreadSafetyMode? Mode => LazyHelper.GetMode(this._state);

    internal bool IsValueFaulted => LazyHelper.GetIsValueFaulted(this._state);

    /// <summary>Gets a value that indicates whether a value has been created for this <see cref="T:System.Lazy`1" /> instance.</summary>
    /// <returns>
    /// <see langword="true" /> if a value has been created for this <see cref="T:System.Lazy`1" /> instance; otherwise, <see langword="false" />.</returns>
    public bool IsValueCreated => this._state == null;

    /// <summary>Gets the lazily initialized value of the current <see cref="T:System.Lazy`1" /> instance.</summary>
    /// <exception cref="T:System.MemberAccessException">The <see cref="T:System.Lazy`1" /> instance is initialized to use the parameterless constructor of the type that is being lazily initialized, and permissions to access the constructor are missing.</exception>
    /// <exception cref="T:System.MissingMemberException">The <see cref="T:System.Lazy`1" /> instance is initialized to use the parameterless constructor of the type that is being lazily initialized, and that type does not have a public, parameterless constructor.</exception>
    /// <exception cref="T:System.InvalidOperationException">The initialization function tries to access <see cref="P:System.Lazy`1.Value" /> on this instance.</exception>
    /// <returns>The lazily initialized value of the current <see cref="T:System.Lazy`1" /> instance.</returns>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Value => this._state != null ? this.CreateValue() : this._value;
  }
}
