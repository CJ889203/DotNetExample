// Decompiled with JetBrains decompiler
// Type: System.Memory`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Represents a contiguous region of memory.</summary>
  /// <typeparam name="T">The type of items in the <see cref="T:System.Memory`1" />.</typeparam>
  [DebuggerTypeProxy(typeof (MemoryDebugView<>))]
  [DebuggerDisplay("{ToString(),raw}")]
  public readonly struct Memory<T> : IEquatable<Memory<T>>
  {

    #nullable disable
    private readonly object _object;
    private readonly int _index;
    private readonly int _length;


    #nullable enable
    /// <summary>Creates a new <see cref="T:System.Memory`1" /> object over the entirety of a specified array.</summary>
    /// <param name="array">The array from which to create the <see cref="T:System.Memory`1" /> object.</param>
    /// <exception cref="T:System.ArrayTypeMismatchException">
    ///         <paramref name="T" /> is a reference type, and <paramref name="array" /> is not an array of type <paramref name="T" />.
    /// 
    /// -or-
    /// 
    /// The array is covariant.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Memory(T[]? array)
    {
      if (array == null)
      {
        *(Memory<T>*) ref this = new Memory<T>();
      }
      else
      {
        if (!typeof (T).IsValueType && array.GetType() != typeof (T[]))
          ThrowHelper.ThrowArrayTypeMismatchException();
        this._object = (object) array;
        this._index = 0;
        this._length = array.Length;
      }
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal unsafe Memory(T[] array, int start)
    {
      if (array == null)
      {
        if (start != 0)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        *(Memory<T>*) ref this = new Memory<T>();
      }
      else
      {
        if (!typeof (T).IsValueType && array.GetType() != typeof (T[]))
          ThrowHelper.ThrowArrayTypeMismatchException();
        if ((uint) start > (uint) array.Length)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        this._object = (object) array;
        this._index = start;
        this._length = array.Length - start;
      }
    }


    #nullable enable
    /// <summary>Creates a new <see cref="T:System.Memory`1" /> object that includes a specified number of elements of an array beginning at a specified index.</summary>
    /// <param name="array">The source array.</param>
    /// <param name="start">The index of the first element to include in the new <see cref="T:System.Memory`1" />.</param>
    /// <param name="length">The number of elements to include in the new <see cref="T:System.Memory`1" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="array" /> is <see langword="null" />, but <paramref name="start" /> or <paramref name="length" /> is non-zero.
    /// 
    /// 
    /// -or-
    /// 
    /// <paramref name="start" /> is outside the bounds of the array.
    /// 
    /// -or-
    /// 
    /// <paramref name="start" /> and <paramref name="length" /> exceeds the number of elements in the array.</exception>
    /// <exception cref="T:System.ArrayTypeMismatchException">
    /// <paramref name="T" /> is a reference type, and <paramref name="array" /> is not an array of type <paramref name="T" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Memory(T[]? array, int start, int length)
    {
      if (array == null)
      {
        if (start != 0 || length != 0)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        *(Memory<T>*) ref this = new Memory<T>();
      }
      else
      {
        if (!typeof (T).IsValueType && array.GetType() != typeof (T[]))
          ThrowHelper.ThrowArrayTypeMismatchException();
        if ((ulong) (uint) start + (ulong) (uint) length > (ulong) (uint) array.Length)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        this._object = (object) array;
        this._index = start;
        this._length = length;
      }
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Memory(MemoryManager<T> manager, int length)
    {
      if (length < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException();
      this._object = (object) manager;
      this._index = 0;
      this._length = length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Memory(MemoryManager<T> manager, int start, int length)
    {
      if (length < 0 || start < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException();
      this._object = (object) manager;
      this._index = start;
      this._length = length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Memory(object obj, int start, int length)
    {
      this._object = obj;
      this._index = start;
      this._length = length;
    }


    #nullable enable
    public static implicit operator Memory<T>(T[]? array) => new Memory<T>(array);

    /// <summary>Defines an implicit conversion of an <see cref="T:System.ArraySegment`1" /> object to a <see cref="T:System.Memory`1" /> object.</summary>
    /// <param name="segment">The object to convert.</param>
    /// <returns>The converted <see cref="T:System.ArraySegment`1" /> object.</returns>
    public static implicit operator Memory<T>(ArraySegment<T> segment) => new Memory<T>(segment.Array, segment.Offset, segment.Count);

    /// <summary>Defines an implicit conversion of a <see cref="T:System.Memory`1" /> object to a <see cref="T:System.ReadOnlyMemory`1" /> object.</summary>
    /// <param name="memory">The object to convert.</param>
    /// <returns>The converted object.</returns>
    public static implicit operator ReadOnlyMemory<T>(Memory<T> memory) => Unsafe.As<Memory<T>, ReadOnlyMemory<T>>(ref memory);

    /// <summary>Returns an empty <see cref="T:System.Memory`1" /> object.</summary>
    /// <returns>An empty object.</returns>
    public static Memory<T> Empty => new Memory<T>();

    /// <summary>Gets the number of items in the current instance.</summary>
    /// <returns>The number of items in the current instance.</returns>
    public int Length => this._length;

    /// <summary>Indicates whether the current instance is empty.</summary>
    /// <returns>
    /// <see langword="true" /> if the current instance is empty; otherwise, <see langword="false" />.</returns>
    public bool IsEmpty => this._length == 0;

    /// <summary>Returns the string representation of this <see cref="T:System.Memory`1" /> object.</summary>
    /// <returns>the string representation of this <see cref="T:System.Memory`1" /> object.</returns>
    public override string ToString()
    {
      if (typeof (T) == typeof (char))
        return !(this._object is string str) ? this.Span.ToString() : str.Substring(this._index, this._length);
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
      interpolatedStringHandler.AppendLiteral("System.Memory<");
      interpolatedStringHandler.AppendFormatted(typeof (T).Name);
      interpolatedStringHandler.AppendLiteral(">[");
      interpolatedStringHandler.AppendFormatted<int>(this._length);
      interpolatedStringHandler.AppendLiteral("]");
      return interpolatedStringHandler.ToStringAndClear();
    }

    /// <summary>Forms a slice out of the current memory that begins at a specified index.</summary>
    /// <param name="start">The index at which to begin the slice.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="start" /> is less than zero or greater than <see cref="P:System.Memory`1.Length" />.</exception>
    /// <returns>An object that contains all elements of the current instance from <paramref name="start" /> to the end of the instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<T> Slice(int start)
    {
      if ((uint) start > (uint) this._length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
      return new Memory<T>(this._object, this._index + start, this._length - start);
    }

    /// <summary>Forms a slice out of the current memory starting at a specified index for a specified length.</summary>
    /// <param name="start">The index at which to begin the slice.</param>
    /// <param name="length">The number of elements to include in the slice.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="start" /> is less than zero or greater than <see cref="P:System.Memory`1.Length" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is greater than <see cref="P:System.Memory`1.Length" /> - <paramref name="start" /></exception>
    /// <returns>An object that contains <paramref name="length" /> elements from the current instance starting at <paramref name="start" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<T> Slice(int start, int length)
    {
      if ((ulong) (uint) start + (ulong) (uint) length > (ulong) (uint) this._length)
        ThrowHelper.ThrowArgumentOutOfRangeException();
      return new Memory<T>(this._object, this._index + start, length);
    }

    /// <summary>Returns a span from the current instance.</summary>
    /// <returns>A span created from the current <see cref="T:System.Memory`1" /> object.</returns>
    public unsafe System.Span<T> Span
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get
      {
        ref T local1 = ref Unsafe.NullRef<T>();
        int length1 = 0;
        object obj = this._object;
        if (obj != null)
        {
          T& local2;
          int length2;
          if (typeof (T) == typeof (char) && obj.GetType() == typeof (string))
          {
            local2 = ref Unsafe.As<char, T>(ref Unsafe.As<string>(obj).GetRawStringData());
            length2 = Unsafe.As<string>(obj).Length;
          }
          else if (RuntimeHelpers.ObjectHasComponentSize(obj))
          {
            local2 = ref MemoryMarshal.GetArrayDataReference<T>(Unsafe.As<T[]>(obj));
            length2 = Unsafe.As<T[]>(obj).Length;
          }
          else
          {
            System.Span<T> span = Unsafe.As<MemoryManager<T>>(obj).GetSpan();
            local2 = ref MemoryMarshal.GetReference<T>(span);
            length2 = span.Length;
          }
          UIntPtr elementOffset = (UIntPtr) (uint) (this._index & int.MaxValue);
          int length3 = this._length;
          if ((ulong) elementOffset + (ulong) (uint) length3 > (ulong) (uint) length2)
            ThrowHelper.ThrowArgumentOutOfRangeException();
          local1 = ref Unsafe.Add<T>(ref local2, (IntPtr) (void*) elementOffset);
          length1 = length3;
        }
        return new System.Span<T>(ref local1, length1);
      }
    }

    /// <summary>Copies the contents of a <see cref="T:System.Memory`1" /> object into a destination <see cref="T:System.Memory`1" /> object.</summary>
    /// <param name="destination">The destination <see cref="T:System.Memory`1" /> object.</param>
    /// <exception cref="T:System.ArgumentException">The length of <paramref name="destination" /> is less than the length of the current instance.</exception>
    public void CopyTo(Memory<T> destination) => this.Span.CopyTo(destination.Span);

    /// <summary>Copies the contents of the memory into a destination <see cref="T:System.Memory`1" /> instance.</summary>
    /// <param name="destination">The destination <see cref="T:System.Memory`1" /> object.</param>
    /// <returns>
    /// <see langword="true" /> if the copy operation succeeds; otherwise, <see langword="false" />.</returns>
    public bool TryCopyTo(Memory<T> destination) => this.Span.TryCopyTo(destination.Span);

    /// <summary>Creates a handle for the <see cref="T:System.Memory`1" /> object.</summary>
    /// <exception cref="T:System.ArgumentException">An instance with non-primitive (non-blittable) members cannot be pinned.</exception>
    /// <returns>A handle for the <see cref="T:System.Memory`1" /> object.</returns>
    public unsafe MemoryHandle Pin()
    {
      object obj = this._object;
      if (obj == null)
        return new MemoryHandle();
      if (typeof (T) == typeof (char) && obj is string str)
      {
        GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
        return new MemoryHandle(Unsafe.AsPointer<char>(ref Unsafe.Add<char>(ref str.GetRawStringData(), this._index)), handle);
      }
      if (!RuntimeHelpers.ObjectHasComponentSize(obj))
        return Unsafe.As<MemoryManager<T>>(obj).Pin(this._index);
      if (this._index < 0)
        return new MemoryHandle(Unsafe.Add<T>(Unsafe.AsPointer<T>(ref MemoryMarshal.GetArrayDataReference<T>(Unsafe.As<T[]>(obj))), this._index & int.MaxValue));
      GCHandle handle1 = GCHandle.Alloc(obj, GCHandleType.Pinned);
      return new MemoryHandle(Unsafe.Add<T>(Unsafe.AsPointer<T>(ref MemoryMarshal.GetArrayDataReference<T>(Unsafe.As<T[]>(obj))), this._index), handle1);
    }

    /// <summary>Copies the contents from the memory into a new array.</summary>
    /// <returns>An array containing the elements in the current memory.</returns>
    public T[] ToArray() => this.Span.ToArray();

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
      switch (obj)
      {
        case ReadOnlyMemory<T> readOnlyMemory:
          return readOnlyMemory.Equals((ReadOnlyMemory<T>) this);
        case Memory<T> other:
          return this.Equals(other);
        default:
          return false;
      }
    }

    /// <summary>Determines whether the specified <see cref="T:System.Memory`1" /> object is equal to the current object.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance and <paramref name="other" /> are equal; otherwise, <see langword="false" />.</returns>
    public bool Equals(Memory<T> other) => this._object == other._object && this._index == other._index && this._length == other._length;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => this._object == null ? 0 : HashCode.Combine<int, int, int>(RuntimeHelpers.GetHashCode(this._object), this._index, this._length);
  }
}
