// Decompiled with JetBrains decompiler
// Type: System.Span`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>Provides a type- and memory-safe representation of a contiguous region of arbitrary memory.</summary>
  /// <typeparam name="T">The type of items in the <see cref="T:System.Span`1" />.</typeparam>
  [DebuggerTypeProxy(typeof (SpanDebugView<>))]
  [DebuggerDisplay("{ToString(),raw}")]
  [NonVersionable]
  public readonly ref struct Span<T>
  {

    #nullable disable
    internal readonly ByReference<T> _pointer;
    private readonly int _length;


    #nullable enable
    /// <summary>Creates a new <see cref="T:System.Span`1" /> object over the entirety of a specified array.</summary>
    /// <param name="array">The array from which to create the <see cref="T:System.Span`1" /> object.</param>
    /// <exception cref="T:System.ArrayTypeMismatchException">
    /// <paramref name="T" /> is a reference type, and <paramref name="array" /> is not an array of type <paramref name="T" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Span(T[]? array)
    {
      if (array == null)
      {
        *(Span<T>*) ref this = new Span<T>();
      }
      else
      {
        if (!typeof (T).IsValueType && array.GetType() != typeof (T[]))
          ThrowHelper.ThrowArrayTypeMismatchException();
        this._pointer = new ByReference<T>(ref MemoryMarshal.GetArrayDataReference<T>(array));
        this._length = array.Length;
      }
    }

    /// <summary>Creates a new <see cref="T:System.Span`1" /> object that includes a specified number of elements of an array starting at a specified index.</summary>
    /// <param name="array">The source array.</param>
    /// <param name="start">The index of the first element to include in the new <see cref="T:System.Span`1" />.</param>
    /// <param name="length">The number of elements to include in the new <see cref="T:System.Span`1" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="array" /> is <see langword="null" />, but <paramref name="start" /> or <paramref name="length" /> is non-zero.
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
    public unsafe Span(T[]? array, int start, int length)
    {
      if (array == null)
      {
        if (start != 0 || length != 0)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        *(Span<T>*) ref this = new Span<T>();
      }
      else
      {
        if (!typeof (T).IsValueType && array.GetType() != typeof (T[]))
          ThrowHelper.ThrowArrayTypeMismatchException();
        if ((ulong) (uint) start + (ulong) (uint) length > (ulong) (uint) array.Length)
          ThrowHelper.ThrowArgumentOutOfRangeException();
        this._pointer = new ByReference<T>(ref Unsafe.Add<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), (IntPtr) (uint) start));
        this._length = length;
      }
    }

    /// <summary>Creates a new <see cref="T:System.Span`1" /> object  from a specified number of <typeparamref name="T" /> elements starting at a specified memory address.</summary>
    /// <param name="pointer">A pointer to the starting address of a specified number of <typeparamref name="T" /> elements in memory.</param>
    /// <param name="length">The number of <typeparamref name="T" /> elements to be included in the <see cref="T:System.Span`1" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="T" /> is a reference type or contains pointers and therefore cannot be stored in unmanaged memory.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="length" /> is negative.</exception>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Span(void* pointer, int length)
    {
      if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        ThrowHelper.ThrowInvalidTypeWithPointersNotSupported(typeof (T));
      if (length < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException();
      this._pointer = new ByReference<T>(ref Unsafe.As<byte, T>(ref *(byte*) pointer));
      this._length = length;
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Span(ref T ptr, int length)
    {
      this._pointer = new ByReference<T>(ref ptr);
      this._length = length;
    }


    #nullable enable
    /// <summary>Gets the element at the specified zero-based index.</summary>
    /// <param name="index">The zero-based index of the element.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than or equal to <see cref="P:System.Span`1.Length" />.</exception>
    /// <returns>The element at the specified index.</returns>
    public ref T this[int index]
    {
      [Intrinsic, NonVersionable, MethodImpl(MethodImplOptions.AggressiveInlining)] get
      {
        if ((uint) index >= (uint) this._length)
          ThrowHelper.ThrowIndexOutOfRangeException();
        return ref Unsafe.Add<T>(ref this._pointer.Value, (IntPtr) (uint) index);
      }
    }

    /// <summary>Returns the length of the current span.</summary>
    /// <returns>The length of the current span.</returns>
    public int Length
    {
      [NonVersionable] get => this._length;
    }

    /// <summary>Returns a value that indicates whether the current <see cref="T:System.Span`1" /> is empty.</summary>
    /// <returns>
    /// <see langword="true" /> if the current span is empty; otherwise, <see langword="false" />.</returns>
    public bool IsEmpty
    {
      [NonVersionable] get => 0U >= (uint) this._length;
    }

    /// <summary>Returns a value that indicates whether two <see cref="T:System.Span`1" /> objects are not equal.</summary>
    /// <param name="left">The first span to compare.</param>
    /// <param name="right">The second span to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Span`1" /> objects are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Span<T> left, Span<T> right) => !(left == right);

    /// <summary>Calls to this method are not supported.</summary>
    /// <param name="obj">Not supported.</param>
    /// <exception cref="T:System.NotSupportedException">Calls to this method are not supported.</exception>
    /// <returns>Calls to this method are not supported.</returns>
    [Obsolete("Equals() on Span will always throw an exception. Use the equality operator instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj) => throw new NotSupportedException(SR.NotSupported_CannotCallEqualsOnSpan);

    /// <summary>Throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">Calls to this method are not supported.</exception>
    /// <returns>Calls to this method always throw a <see cref="T:System.NotSupportedException" />.</returns>
    [Obsolete("GetHashCode() on Span will always throw an exception.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnSpan);

    public static implicit operator Span<T>(T[]? array) => new Span<T>(array);

    /// <summary>Defines an implicit conversion of an <see cref="T:System.ArraySegment`1" /> to a <see cref="T:System.Span`1" />.</summary>
    /// <param name="segment">The array segment to be converted to a <see cref="T:System.Span`1" />.</param>
    /// <returns>A span that corresponds to the array segment.</returns>
    public static implicit operator Span<T>(ArraySegment<T> segment) => new Span<T>(segment.Array, segment.Offset, segment.Count);

    /// <summary>Returns an empty <see cref="T:System.Span`1" /> object.</summary>
    /// <returns>An empty <see cref="T:System.Span`1" /> object.</returns>
    public static Span<T> Empty => new Span<T>();

    /// <summary>Returns an enumerator for this <see cref="T:System.Span`1" />.</summary>
    /// <returns>An enumerator for this span.</returns>
    public Span<
    #nullable disable
    T>.Enumerator GetEnumerator() => new Span<T>.Enumerator(this);


    #nullable enable
    /// <summary>Returns a reference to an object of type T that can be used for pinning.
    /// 
    /// This method is intended to support .NET compilers and is not intended to be called by user code.</summary>
    /// <returns>A reference to the element of the span at index 0, or <see langword="null" /> if the span is empty.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref T GetPinnableReference()
    {
      ref T local = ref Unsafe.NullRef<T>();
      if (this._length != 0)
        local = ref this._pointer.Value;
      return ref local;
    }

    /// <summary>Clears the contents of this <see cref="T:System.Span`1" /> object.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
      if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        SpanHelpers.ClearWithReferences(ref Unsafe.As<T, IntPtr>(ref this._pointer.Value), (UIntPtr) (uint) this._length * (UIntPtr) (Unsafe.SizeOf<T>() / sizeof (UIntPtr)));
      else
        SpanHelpers.ClearWithoutReferences(ref Unsafe.As<T, byte>(ref this._pointer.Value), (UIntPtr) (uint) this._length * (UIntPtr) Unsafe.SizeOf<T>());
    }

    /// <summary>Fills the elements of this span with a specified value.</summary>
    /// <param name="value">The value to assign to each element of the span.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Fill(T value)
    {
      if (Unsafe.SizeOf<T>() == 1)
        Unsafe.InitBlockUnaligned(ref Unsafe.As<T, byte>(ref this._pointer.Value), Unsafe.As<T, byte>(ref value), (uint) this._length);
      else
        SpanHelpers.Fill<T>(ref this._pointer.Value, (UIntPtr) (uint) this._length, value);
    }

    /// <summary>Copies the contents of this <see cref="T:System.Span`1" /> into a destination <see cref="T:System.Span`1" />.</summary>
    /// <param name="destination">The destination <see cref="T:System.Span`1" /> object.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="destination" /> is shorter than the source <see cref="T:System.Span`1" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(Span<T> destination)
    {
      if ((uint) this._length <= (uint) destination.Length)
        Buffer.Memmove<T>(ref destination._pointer.Value, ref this._pointer.Value, (UIntPtr) (uint) this._length);
      else
        ThrowHelper.ThrowArgumentException_DestinationTooShort();
    }

    /// <summary>Attempts to copy the current <see cref="T:System.Span`1" /> to a destination <see cref="T:System.Span`1" /> and returns a value that indicates whether the copy operation succeeded.</summary>
    /// <param name="destination">The target of the copy operation.</param>
    /// <returns>
    /// <see langword="true" /> if the copy operation succeeded; otherwise, <see langword="false" />.</returns>
    public bool TryCopyTo(Span<T> destination)
    {
      bool flag = false;
      if ((uint) this._length <= (uint) destination.Length)
      {
        Buffer.Memmove<T>(ref destination._pointer.Value, ref this._pointer.Value, (UIntPtr) (uint) this._length);
        flag = true;
      }
      return flag;
    }

    /// <summary>Returns a value that indicates whether two <see cref="T:System.Span`1" /> objects are equal.</summary>
    /// <param name="left">The first span to compare.</param>
    /// <param name="right">The second span to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Span`1" /> objects are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Span<T> left, Span<T> right) => left._length == right._length && Unsafe.AreSame<T>(ref left._pointer.Value, ref right._pointer.Value);

    /// <summary>Defines an implicit conversion of a <see cref="T:System.Span`1" /> to a <see cref="T:System.ReadOnlySpan`1" />.</summary>
    /// <param name="span">The object to convert to a <see cref="T:System.ReadOnlySpan`1" />.</param>
    /// <returns>A read-only span that corresponds to the current instance.</returns>
    public static implicit operator ReadOnlySpan<T>(Span<T> span) => new ReadOnlySpan<T>(ref span._pointer.Value, span._length);

    /// <summary>Returns the string representation of this <see cref="T:System.Span`1" /> object.</summary>
    /// <returns>The string representation of this <see cref="T:System.Span`1" /> object.</returns>
    public override string ToString()
    {
      if (typeof (T) == typeof (char))
        return new string(new ReadOnlySpan<char>(ref Unsafe.As<T, char>(ref this._pointer.Value), this._length));
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 2);
      interpolatedStringHandler.AppendLiteral("System.Span<");
      interpolatedStringHandler.AppendFormatted(typeof (T).Name);
      interpolatedStringHandler.AppendLiteral(">[");
      interpolatedStringHandler.AppendFormatted<int>(this._length);
      interpolatedStringHandler.AppendLiteral("]");
      return interpolatedStringHandler.ToStringAndClear();
    }

    /// <summary>Forms a slice out of the current span that begins at a specified index.</summary>
    /// <param name="start">The index at which to begin the slice.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="start" /> is less than zero or greater than <see cref="P:System.Span`1.Length" />.</exception>
    /// <returns>A span that consists of all elements of the current span from <paramref name="start" /> to the end of the span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> Slice(int start)
    {
      if ((uint) start > (uint) this._length)
        ThrowHelper.ThrowArgumentOutOfRangeException();
      return new Span<T>(ref Unsafe.Add<T>(ref this._pointer.Value, (IntPtr) (uint) start), this._length - start);
    }

    /// <summary>Forms a slice out of the current span starting at a specified index for a specified length.</summary>
    /// <param name="start">The index at which to begin this slice.</param>
    /// <param name="length">The desired length for the slice.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="start" /> or <paramref name="start" /> + <paramref name="length" /> is less than zero or greater than <see cref="P:System.Span`1.Length" />.</exception>
    /// <returns>A span that consists of <paramref name="length" /> elements from the current span starting at <paramref name="start" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> Slice(int start, int length)
    {
      if ((ulong) (uint) start + (ulong) (uint) length > (ulong) (uint) this._length)
        ThrowHelper.ThrowArgumentOutOfRangeException();
      return new Span<T>(ref Unsafe.Add<T>(ref this._pointer.Value, (IntPtr) (uint) start), length);
    }

    /// <summary>Copies the contents of this span into a new array.</summary>
    /// <returns>An array containing the data in the current span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] ToArray()
    {
      if (this._length == 0)
        return Array.Empty<T>();
      T[] array = new T[this._length];
      Buffer.Memmove<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), ref this._pointer.Value, (UIntPtr) (uint) this._length);
      return array;
    }

    /// <summary>Provides an enumerator for the elements of a <see cref="T:System.Span`1" />.</summary>
    /// <typeparam name="T" />
    public ref struct Enumerator
    {

      #nullable disable
      private readonly Span<T> _span;
      private int _index;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      internal Enumerator(Span<T> span)
      {
        this._span = span;
        this._index = -1;
      }

      /// <summary>Advances the enumerator to the next item of the <see cref="T:System.Span`1" />.</summary>
      /// <returns>
      /// <see langword="true" /> if the enumerator successfully advanced to the next item; <see langword="false" /> if the end of the span has been passed.</returns>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
        int num = this._index + 1;
        if (num >= this._span.Length)
          return false;
        this._index = num;
        return true;
      }


      #nullable enable
      /// <summary>Gets a reference to the item at the current position of the enumerator.</summary>
      /// <returns>The element in the <see cref="T:System.Span`1" /> at the current position of the enumerator.</returns>
      public ref T Current
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] get => ref this._span[this._index];
      }
    }
  }
}
