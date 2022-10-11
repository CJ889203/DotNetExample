// Decompiled with JetBrains decompiler
// Type: System.Index
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


#nullable enable
namespace System
{
  /// <summary>Represents a type that can be used to index a collection either from the start or the end.</summary>
  public readonly struct Index : IEquatable<Index>
  {
    private readonly int _value;

    /// <summary>Initializes a new <see cref="T:System.Index" /> with a specified index position and a value that indicates if the index is from the start or the end of a collection.</summary>
    /// <param name="value">The index value. It has to be greater or equal than zero.</param>
    /// <param name="fromEnd">A boolean indicating if the index is from the start (<see langword="false" />) or from the end (<see langword="true" />) of a collection.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Index(int value, bool fromEnd = false)
    {
      if (value < 0)
        ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
      if (fromEnd)
        this._value = ~value;
      else
        this._value = value;
    }

    private Index(int value) => this._value = value;

    /// <summary>Gets an <see cref="T:System.Index" /> that points to the first element of a collection.</summary>
    /// <returns>An instance that points to the first element of a collection.</returns>
    public static Index Start => new Index(0);

    /// <summary>Gets an <see cref="T:System.Index" /> that points beyond the last element.</summary>
    /// <returns>an <see cref="T:System.Index" /> that points beyond the last element.</returns>
    public static Index End => new Index(-1);

    /// <summary>Create an <see cref="T:System.Index" /> from the specified index at the start of a collection.</summary>
    /// <param name="value">The index position from the start of a collection.</param>
    /// <returns>The Index value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index FromStart(int value)
    {
      if (value < 0)
        ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
      return new Index(value);
    }

    /// <summary>Creates an <see cref="T:System.Index" /> from the end of a collection at a specified index position.</summary>
    /// <param name="value">The index value from the end of a collection.</param>
    /// <returns>The Index value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index FromEnd(int value)
    {
      if (value < 0)
        ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
      return new Index(~value);
    }

    /// <summary>Gets the index value.</summary>
    /// <returns>The index value.</returns>
    public int Value => this._value < 0 ? ~this._value : this._value;

    /// <summary>Gets a value that indicates whether the index is from the start or the end.</summary>
    /// <returns>
    /// <see langword="true" /> if the Index is from the end; otherwise, <see langword="false" />.</returns>
    public bool IsFromEnd => this._value < 0;

    /// <summary>Calculates the offset from the start of the collection using the given collection length.</summary>
    /// <param name="length">The length of the collection that the Index will be used with. Must be a positive value.</param>
    /// <returns>The offset.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetOffset(int length)
    {
      int offset = this._value;
      if (this.IsFromEnd)
        offset += length + 1;
      return offset;
    }

    /// <summary>Indicates whether the current Index object is equal to a specified object.</summary>
    /// <param name="value">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is of type <see cref="T:System.Index" /> and is equal to the current instance; <see langword="false" /> otherwise.</returns>
    public override bool Equals([NotNullWhen(true)] object? value) => value is Index index && this._value == index._value;

    /// <summary>Returns a value that indicates whether the current object is equal to another <see cref="T:System.Index" /> object.</summary>
    /// <param name="other">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current Index object is equal to <paramref name="other" />; <see langword="false" /> otherwise.</returns>
    public bool Equals(Index other) => this._value == other._value;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() => this._value;

    /// <summary>Converts integer number to an Index.</summary>
    /// <param name="value">The integer to convert.</param>
    /// <returns>An Index representing the integer.</returns>
    public static implicit operator Index(int value) => Index.FromStart(value);

    /// <summary>Returns the string representation of the current <see cref="T:System.Index" /> instance.</summary>
    /// <returns>The string representation of the <see cref="T:System.Index" />.</returns>
    public override string ToString() => this.IsFromEnd ? this.ToStringFromEnd() : ((uint) this.Value).ToString();


    #nullable disable
    private unsafe string ToStringFromEnd()
    {
      // ISSUE: untyped stack allocation
      Span<char> span = new Span<char>((void*) __untypedstackalloc(new IntPtr(22)), 11);
      int charsWritten;
      ((uint) this.Value).TryFormat(span.Slice(1), out charsWritten);
      span[0] = '^';
      return new string((ReadOnlySpan<char>) span.Slice(0, charsWritten + 1));
    }
  }
}
