// Decompiled with JetBrains decompiler
// Type: System.Range
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


#nullable enable
namespace System
{
  /// <summary>Represents a range that has start and end indexes.</summary>
  public readonly struct Range : IEquatable<Range>
  {
    /// <summary>Gets the inclusive start index of the <see cref="T:System.Range" />.</summary>
    /// <returns>The inclusive start index of the range.</returns>
    public Index Start { get; }

    /// <summary>Gets an <see cref="T:System.Index" /> that represents the exclusive end index of the range.</summary>
    /// <returns>The end index of the range.</returns>
    public Index End { get; }

    /// <summary>Instantiates a new <see cref="T:System.Range" /> instance with the specified starting and ending indexes.</summary>
    /// <param name="start">The inclusive start index of the range.</param>
    /// <param name="end">The exclusive end index of the range.</param>
    public Range(Index start, Index end)
    {
      this.Start = start;
      this.End = end;
    }

    /// <summary>Returns a value that indicates whether the current instance is equal to a specified object.</summary>
    /// <param name="value">An object to compare with this Range object.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is of type <see cref="T:System.Range" /> and is equal to the current instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? value) => value is Range range && range.Start.Equals(this.Start) && range.End.Equals(this.End);

    /// <summary>Returns a value that indicates whether the current instance is equal to another <see cref="T:System.Range" /> object.</summary>
    /// <param name="other">A Range object to compare with this Range object.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to <paramref name="other" />; otherwise, <see langword="false" />.</returns>
    public bool Equals(Range other) => other.Start.Equals(this.Start) && other.End.Equals(this.End);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
      Index index = this.Start;
      int hashCode1 = index.GetHashCode();
      index = this.End;
      int hashCode2 = index.GetHashCode();
      return HashCode.Combine<int, int>(hashCode1, hashCode2);
    }

    /// <summary>Returns the string representation of the current <see cref="T:System.Range" /> object.</summary>
    /// <returns>The string representation of the range.</returns>
    public override unsafe string ToString()
    {
      // ISSUE: untyped stack allocation
      Span<char> span = new Span<char>((void*) __untypedstackalloc(new IntPtr(48)), 24);
      int start1 = 0;
      if (this.Start.IsFromEnd)
      {
        span[0] = '^';
        start1 = 1;
      }
      int charsWritten;
      bool flag = ((uint) this.Start.Value).TryFormat(span.Slice(start1), out charsWritten);
      int num1 = start1 + charsWritten;
      ref Span<char> local1 = ref span;
      int index1 = num1;
      int num2 = index1 + 1;
      local1[index1] = '.';
      ref Span<char> local2 = ref span;
      int index2 = num2;
      int start2 = index2 + 1;
      local2[index2] = '.';
      if (this.End.IsFromEnd)
        span[start2++] = '^';
      flag = ((uint) this.End.Value).TryFormat(span.Slice(start2), out charsWritten);
      int length = start2 + charsWritten;
      return new string((ReadOnlySpan<char>) span.Slice(0, length));
    }

    /// <summary>Returns a new <see cref="T:System.Range" /> instance starting from a specified start index to the end of the collection.</summary>
    /// <param name="start">The position of the first element from which the Range will be created.</param>
    /// <returns>A range from <paramref name="start" /> to the end of the collection.</returns>
    public static Range StartAt(Index start) => new Range(start, Index.End);

    /// <summary>Creates a <see cref="T:System.Range" /> object starting from the first element in the collection to a specified end index.</summary>
    /// <param name="end">The position of the last element up to which the <see cref="T:System.Range" /> object will be created.</param>
    /// <returns>A range that starts from the first element to <paramref name="end" />.</returns>
    public static Range EndAt(Index end) => new Range(Index.Start, end);

    /// <summary>Gets a <see cref="T:System.Range" /> object that starts from the first element to the end.</summary>
    /// <returns>A range from the start to the end.</returns>
    public static Range All => new Range(Index.Start, Index.End);

    /// <summary>Calculates the start offset and length of the range object using a collection length.</summary>
    /// <param name="length">A positive integer that represents the length of the collection that the range will be used with.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="length" /> is outside the bounds of the current range.</exception>
    /// <returns>The start offset and length of the range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int Offset, int Length) GetOffsetAndLength(int length)
    {
      Index start = this.Start;
      int num1 = !start.IsFromEnd ? start.Value : length - start.Value;
      Index end = this.End;
      int num2 = !end.IsFromEnd ? end.Value : length - end.Value;
      if ((uint) num2 > (uint) length || (uint) num1 > (uint) num2)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
      return (num1, num2 - num1);
    }
  }
}
