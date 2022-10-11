// Decompiled with JetBrains decompiler
// Type: System.Threading.ThreadLocal`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Threading
{
  /// <summary>Provides thread-local storage of data.</summary>
  /// <typeparam name="T">Specifies the type of data stored per-thread.</typeparam>
  [DebuggerTypeProxy(typeof (SystemThreading_ThreadLocalDebugView<>))]
  [DebuggerDisplay("IsValueCreated={IsValueCreated}, Value={ValueForDebugDisplay}, Count={ValuesCountForDebugDisplay}")]
  public class ThreadLocal<T> : IDisposable
  {

    #nullable disable
    private Func<T> _valueFactory;
    [ThreadStatic]
    private static ThreadLocal<T>.LinkedSlotVolatile[] ts_slotArray;
    [ThreadStatic]
    private static ThreadLocal<T>.FinalizationHelper ts_finalizationHelper;
    private int _idComplement;
    private volatile bool _initialized;
    private static readonly ThreadLocal<T>.IdManager s_idManager = new ThreadLocal<T>.IdManager();
    private ThreadLocal<T>.LinkedSlot _linkedSlot = new ThreadLocal<T>.LinkedSlot((ThreadLocal<T>.LinkedSlotVolatile[]) null);
    private bool _trackAllValues;

    /// <summary>Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance.</summary>
    public ThreadLocal() => this.Initialize((Func<T>) null, false);

    /// <summary>Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance and specifies whether all values are accessible from any thread.</summary>
    /// <param name="trackAllValues">
    /// <see langword="true" /> to track all values set on the instance and expose them through the <see cref="P:System.Threading.ThreadLocal`1.Values" /> property; <see langword="false" /> otherwise. When set to <see langword="true" />, a value stored from a given thread will be available through <see cref="P:System.Threading.ThreadLocal`1.Values" /> even after that thread has exited.</param>
    public ThreadLocal(bool trackAllValues) => this.Initialize((Func<T>) null, trackAllValues);


    #nullable enable
    /// <summary>Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance with the specified <paramref name="valueFactory" /> function.</summary>
    /// <param name="valueFactory">The  <see cref="T:System.Func`1" /> invoked to produce a lazily-initialized value when an attempt is made to retrieve <see cref="P:System.Threading.ThreadLocal`1.Value" /> without it having been previously initialized.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="valueFactory" /> is a null reference (Nothing in Visual Basic).</exception>
    public ThreadLocal(Func<T> valueFactory)
    {
      if (valueFactory == null)
        throw new ArgumentNullException(nameof (valueFactory));
      this.Initialize(valueFactory, false);
    }

    /// <summary>Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance with the specified <paramref name="valueFactory" /> function and a flag that indicates whether all values are accessible from any thread.</summary>
    /// <param name="valueFactory">The <see cref="T:System.Func`1" /> invoked to produce a lazily-initialized value when an attempt is made to retrieve <see cref="P:System.Threading.ThreadLocal`1.Value" /> without it having been previously initialized.</param>
    /// <param name="trackAllValues">
    /// <see langword="true" /> to track all values set on the instance and expose them through the <see cref="P:System.Threading.ThreadLocal`1.Values" /> property; <see langword="false" /> otherwise. When set to <see langword="true" />, a value stored from a given thread will be available through <see cref="P:System.Threading.ThreadLocal`1.Values" /> even after that thread has exited.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="valueFactory" /> is a <see langword="null" /> reference (<see langword="Nothing" /> in Visual Basic).</exception>
    public ThreadLocal(Func<T> valueFactory, bool trackAllValues)
    {
      if (valueFactory == null)
        throw new ArgumentNullException(nameof (valueFactory));
      this.Initialize(valueFactory, trackAllValues);
    }


    #nullable disable
    private void Initialize(Func<T> valueFactory, bool trackAllValues)
    {
      this._valueFactory = valueFactory;
      this._trackAllValues = trackAllValues;
      this._idComplement = ~ThreadLocal<T>.s_idManager.GetId(trackAllValues);
      this._initialized = true;
    }

    /// <summary>Releases the resources used by this <see cref="T:System.Threading.ThreadLocal`1" /> instance.</summary>
    ~ThreadLocal() => this.Dispose(false);

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.ThreadLocal`1" /> class.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the resources used by this <see cref="T:System.Threading.ThreadLocal`1" /> instance.</summary>
    /// <param name="disposing">A Boolean value that indicates whether this method is being called due to a call to <see cref="M:System.Threading.ThreadLocal`1.Dispose" />.</param>
    protected virtual void Dispose(bool disposing)
    {
      int id;
      lock (ThreadLocal<T>.s_idManager)
      {
        id = ~this._idComplement;
        this._idComplement = 0;
        if (id < 0 || !this._initialized)
          return;
        this._initialized = false;
        for (ThreadLocal<T>.LinkedSlot next = this._linkedSlot._next; next != null; next = next._next)
        {
          ThreadLocal<T>.LinkedSlotVolatile[] slotArray = next._slotArray;
          if (slotArray != null)
          {
            next._slotArray = (ThreadLocal<T>.LinkedSlotVolatile[]) null;
            slotArray[id].Value._value = default (T);
            slotArray[id].Value = (ThreadLocal<T>.LinkedSlot) null;
          }
        }
      }
      this._linkedSlot = (ThreadLocal<T>.LinkedSlot) null;
      ThreadLocal<T>.s_idManager.ReturnId(id, this._trackAllValues);
    }


    #nullable enable
    /// <summary>Creates and returns a string representation of this instance for the current thread.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.</exception>
    /// <exception cref="T:System.NullReferenceException">The <see cref="P:System.Threading.ThreadLocal`1.Value" /> for the current thread is a null reference (Nothing in Visual Basic).</exception>
    /// <exception cref="T:System.InvalidOperationException">The initialization function attempted to reference <see cref="P:System.Threading.ThreadLocal`1.Value" /> recursively.</exception>
    /// <exception cref="T:System.MissingMemberException">No parameterless constructor is provided and no value factory is supplied.</exception>
    /// <returns>The result of calling <see cref="M:System.Object.ToString" /> on the <see cref="P:System.Threading.ThreadLocal`1.Value" />.</returns>
    public override string? ToString() => this.Value.ToString();

    /// <summary>Gets or sets the value of this instance for the current thread.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The initialization function attempted to reference <see cref="P:System.Threading.ThreadLocal`1.Value" /> recursively.</exception>
    /// <exception cref="T:System.MissingMemberException">No parameterless constructor is provided and no value factory is supplied.</exception>
    /// <returns>Returns an instance of the object that this ThreadLocal is responsible for initializing.</returns>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Value
    {
      [return: MaybeNull] get
      {
        ThreadLocal<T>.LinkedSlotVolatile[] tsSlotArray = ThreadLocal<T>.ts_slotArray;
        int index = ~this._idComplement;
        ThreadLocal<T>.LinkedSlot linkedSlot;
        return tsSlotArray != null && index >= 0 && index < tsSlotArray.Length && (linkedSlot = tsSlotArray[index].Value) != null && this._initialized ? linkedSlot._value : this.GetValueSlow();
      }
      set
      {
        ThreadLocal<T>.LinkedSlotVolatile[] tsSlotArray = ThreadLocal<T>.ts_slotArray;
        int index = ~this._idComplement;
        ThreadLocal<T>.LinkedSlot linkedSlot;
        if (tsSlotArray != null && index >= 0 && index < tsSlotArray.Length && (linkedSlot = tsSlotArray[index].Value) != null && this._initialized)
          linkedSlot._value = value;
        else
          this.SetValueSlow(value, tsSlotArray);
      }
    }


    #nullable disable
    private T GetValueSlow()
    {
      if (~this._idComplement < 0)
        throw new ObjectDisposedException(SR.ThreadLocal_Disposed);
      Debugger.NotifyOfCrossThreadDependency();
      T valueSlow;
      if (this._valueFactory == null)
      {
        valueSlow = default (T);
      }
      else
      {
        valueSlow = this._valueFactory();
        if (this.IsValueCreated)
          throw new InvalidOperationException(SR.ThreadLocal_Value_RecursiveCallsToValue);
      }
      this.Value = valueSlow;
      return valueSlow;
    }

    private void SetValueSlow(T value, ThreadLocal<T>.LinkedSlotVolatile[] slotArray)
    {
      int id = ~this._idComplement;
      if (id < 0)
        throw new ObjectDisposedException(SR.ThreadLocal_Disposed);
      if (slotArray == null)
      {
        slotArray = new ThreadLocal<T>.LinkedSlotVolatile[ThreadLocal<T>.GetNewTableSize(id + 1)];
        ThreadLocal<T>.ts_finalizationHelper = new ThreadLocal<T>.FinalizationHelper(slotArray);
        ThreadLocal<T>.ts_slotArray = slotArray;
      }
      if (id >= slotArray.Length)
      {
        ThreadLocal<T>.GrowTable(ref slotArray, id + 1);
        ThreadLocal<T>.ts_finalizationHelper.SlotArray = slotArray;
        ThreadLocal<T>.ts_slotArray = slotArray;
      }
      if (slotArray[id].Value == null)
      {
        this.CreateLinkedSlot(slotArray, id, value);
      }
      else
      {
        ThreadLocal<T>.LinkedSlot linkedSlot = slotArray[id].Value;
        if (!this._initialized)
          throw new ObjectDisposedException(SR.ThreadLocal_Disposed);
        linkedSlot._value = value;
      }
    }

    private void CreateLinkedSlot(ThreadLocal<T>.LinkedSlotVolatile[] slotArray, int id, T value)
    {
      ThreadLocal<T>.LinkedSlot linkedSlot = new ThreadLocal<T>.LinkedSlot(slotArray);
      lock (ThreadLocal<T>.s_idManager)
      {
        if (!this._initialized)
          throw new ObjectDisposedException(SR.ThreadLocal_Disposed);
        ThreadLocal<T>.LinkedSlot next = this._linkedSlot._next;
        linkedSlot._next = next;
        linkedSlot._previous = this._linkedSlot;
        linkedSlot._value = value;
        if (next != null)
          next._previous = linkedSlot;
        this._linkedSlot._next = linkedSlot;
        slotArray[id].Value = linkedSlot;
      }
    }


    #nullable enable
    /// <summary>Gets a list containing the values stored by all threads that have accessed this instance.</summary>
    /// <exception cref="T:System.InvalidOperationException">Values stored by all threads are not available because this instance was initialized with the <paramref name="trackAllValues" /> argument set to <see langword="false" /> in the call to a class constructor.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.</exception>
    /// <returns>A list for all of the values stored by all of the threads that have accessed this instance.</returns>
    public IList<T> Values
    {
      get
      {
        if (!this._trackAllValues)
          throw new InvalidOperationException(SR.ThreadLocal_ValuesNotAvailable);
        return (IList<T>) (this.GetValuesAsList() ?? throw new ObjectDisposedException(SR.ThreadLocal_Disposed));
      }
    }


    #nullable disable
    private List<T> GetValuesAsList()
    {
      ThreadLocal<T>.LinkedSlot linkedSlot = this._linkedSlot;
      if (~this._idComplement == -1 || linkedSlot == null)
        return (List<T>) null;
      List<T> valuesAsList = new List<T>();
      for (ThreadLocal<T>.LinkedSlot next = linkedSlot._next; next != null; next = next._next)
        valuesAsList.Add(next._value);
      return valuesAsList;
    }

    private int ValuesCountForDebugDisplay
    {
      get
      {
        int countForDebugDisplay = 0;
        for (ThreadLocal<T>.LinkedSlot next = this._linkedSlot?._next; next != null; next = next._next)
          ++countForDebugDisplay;
        return countForDebugDisplay;
      }
    }

    /// <summary>Gets whether <see cref="P:System.Threading.ThreadLocal`1.Value" /> is initialized on the current thread.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.</exception>
    /// <returns>true if <see cref="P:System.Threading.ThreadLocal`1.Value" /> is initialized on the current thread; otherwise false.</returns>
    public bool IsValueCreated
    {
      get
      {
        int index = ~this._idComplement;
        if (index < 0)
          throw new ObjectDisposedException(SR.ThreadLocal_Disposed);
        ThreadLocal<T>.LinkedSlotVolatile[] tsSlotArray = ThreadLocal<T>.ts_slotArray;
        return tsSlotArray != null && index < tsSlotArray.Length && tsSlotArray[index].Value != null;
      }
    }


    #nullable enable
    internal T? ValueForDebugDisplay
    {
      get
      {
        ThreadLocal<T>.LinkedSlotVolatile[] tsSlotArray = ThreadLocal<T>.ts_slotArray;
        int index = ~this._idComplement;
        ThreadLocal<T>.LinkedSlot linkedSlot;
        return tsSlotArray == null || index >= tsSlotArray.Length || (linkedSlot = tsSlotArray[index].Value) == null || !this._initialized ? default (T) : linkedSlot._value;
      }
    }

    internal List<T>? ValuesForDebugDisplay => this.GetValuesAsList();


    #nullable disable
    private static void GrowTable(ref ThreadLocal<T>.LinkedSlotVolatile[] table, int minLength)
    {
      ThreadLocal<T>.LinkedSlotVolatile[] linkedSlotVolatileArray = new ThreadLocal<T>.LinkedSlotVolatile[ThreadLocal<T>.GetNewTableSize(minLength)];
      lock (ThreadLocal<T>.s_idManager)
      {
        for (int index = 0; index < table.Length; ++index)
        {
          ThreadLocal<T>.LinkedSlot linkedSlot = table[index].Value;
          if (linkedSlot != null && linkedSlot._slotArray != null)
          {
            linkedSlot._slotArray = linkedSlotVolatileArray;
            linkedSlotVolatileArray[index] = table[index];
          }
        }
      }
      table = linkedSlotVolatileArray;
    }

    private static int GetNewTableSize(int minSize)
    {
      if ((long) (uint) minSize > (long) Array.MaxLength)
        return int.MaxValue;
      int num1 = minSize - 1;
      int num2 = num1 | num1 >> 1;
      int num3 = num2 | num2 >> 2;
      int num4 = num3 | num3 >> 4;
      int num5 = num4 | num4 >> 8;
      int newTableSize = (num5 | num5 >> 16) + 1;
      if ((long) (uint) newTableSize > (long) Array.MaxLength)
        newTableSize = Array.MaxLength;
      return newTableSize;
    }

    private struct LinkedSlotVolatile
    {
      internal volatile ThreadLocal<T>.LinkedSlot Value;
    }

    private sealed class LinkedSlot
    {
      internal volatile ThreadLocal<T>.LinkedSlot _next;
      internal volatile ThreadLocal<T>.LinkedSlot _previous;
      internal volatile ThreadLocal<T>.LinkedSlotVolatile[] _slotArray;
      internal T _value;

      internal LinkedSlot(ThreadLocal<T>.LinkedSlotVolatile[] slotArray) => this._slotArray = slotArray;
    }

    private sealed class IdManager
    {
      private int _nextIdToTry;
      private volatile int _idsThatDoNotTrackAllValues;
      private readonly List<byte> _ids = new List<byte>();

      internal int GetId(bool trackAllValues)
      {
        lock (this._ids)
        {
          int nextIdToTry = this._nextIdToTry;
          while (nextIdToTry < this._ids.Count && this._ids[nextIdToTry] != (byte) 0)
            ++nextIdToTry;
          byte num = trackAllValues ? (byte) 1 : (byte) 2;
          if (nextIdToTry == this._ids.Count)
            this._ids.Add(num);
          else
            this._ids[nextIdToTry] = num;
          if (!trackAllValues)
            ++this._idsThatDoNotTrackAllValues;
          this._nextIdToTry = nextIdToTry + 1;
          return nextIdToTry;
        }
      }

      internal bool IdTracksAllValues(int id)
      {
        lock (this._ids)
          return this._ids[id] == (byte) 1;
      }

      internal int IdsThatDoNotTrackValuesCount => this._idsThatDoNotTrackAllValues;

      internal void ReturnId(int id, bool idTracksAllValues)
      {
        lock (this._ids)
        {
          if (!idTracksAllValues)
            --this._idsThatDoNotTrackAllValues;
          this._ids[id] = (byte) 0;
          if (id >= this._nextIdToTry)
            return;
          this._nextIdToTry = id;
        }
      }
    }

    private sealed class FinalizationHelper
    {
      internal ThreadLocal<T>.LinkedSlotVolatile[] SlotArray;

      internal FinalizationHelper(ThreadLocal<T>.LinkedSlotVolatile[] slotArray) => this.SlotArray = slotArray;

      ~FinalizationHelper()
      {
        ThreadLocal<T>.LinkedSlotVolatile[] slotArray = this.SlotArray;
        int trackValuesCount = ThreadLocal<T>.s_idManager.IdsThatDoNotTrackValuesCount;
        for (int id = 0; id < slotArray.Length; ++id)
        {
          ThreadLocal<T>.LinkedSlot linkedSlot = slotArray[id].Value;
          if (linkedSlot != null)
          {
            if (trackValuesCount == 0 || ThreadLocal<T>.s_idManager.IdTracksAllValues(id))
            {
              linkedSlot._slotArray = (ThreadLocal<T>.LinkedSlotVolatile[]) null;
            }
            else
            {
              lock (ThreadLocal<T>.s_idManager)
              {
                if (slotArray[id].Value != null)
                  --trackValuesCount;
                if (linkedSlot._next != null)
                  linkedSlot._next._previous = linkedSlot._previous;
                linkedSlot._previous._next = linkedSlot._next;
              }
            }
          }
        }
      }
    }
  }
}
