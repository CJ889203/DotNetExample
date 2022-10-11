// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.MatchCollection
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>Represents the set of successful matches found by iteratively applying a regular expression pattern to the input string.  The collection is immutable (read-only) and has no public constructor. The <see cref="T:System.Text.RegularExpressions.Regex.Matches(System.String)" /> method returns a <see cref="T:System.Text.RegularExpressions.MatchCollection" /> object.</summary>
  [DebuggerDisplay("Count = {Count}")]
  [DebuggerTypeProxy(typeof (CollectionDebuggerProxy<Match>))]
  public class MatchCollection : 
    IList<Match>,
    ICollection<Match>,
    IEnumerable<Match>,
    IEnumerable,
    IReadOnlyList<Match>,
    IReadOnlyCollection<Match>,
    IList,
    ICollection
  {

    #nullable disable
    private readonly Regex _regex;
    private readonly List<Match> _matches;
    private readonly string _input;
    private int _startat;
    private int _prevlen;
    private bool _done;

    internal MatchCollection(Regex regex, string input, int startat)
    {
      if ((uint) startat > (uint) input.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startat, ExceptionResource.BeginIndexNotNegative);
      this._regex = regex;
      this._input = input;
      this._startat = startat;
      this._prevlen = -1;
      this._matches = new List<Match>();
      this._done = false;
    }

    /// <summary>Gets a value that indicates whether the collection is read only.</summary>
    /// <returns>
    /// <see langword="true" /> in all cases.</returns>
    public bool IsReadOnly => true;

    /// <summary>Gets the number of matches.</summary>
    /// <exception cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException">A time-out occurred.</exception>
    /// <returns>The number of matches.</returns>
    public int Count
    {
      get
      {
        this.EnsureInitialized();
        return this._matches.Count;
      }
    }


    #nullable enable
    /// <summary>Gets an individual member of the collection.</summary>
    /// <param name="i">Index into the <see cref="T:System.Text.RegularExpressions.Match" /> collection.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="i" /> is less than 0 or greater than or equal to <see cref="P:System.Text.RegularExpressions.MatchCollection.Count" />.</exception>
    /// <exception cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException">A time-out occurred.</exception>
    /// <returns>The captured substring at position <paramref name="i" /> in the collection.</returns>
    public virtual Match this[int i]
    {
      get
      {
        Match match = (Match) null;
        if (i < 0 || (match = this.GetMatch(i)) == null)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.i);
        return match;
      }
    }

    /// <summary>Provides an enumerator that iterates through the collection.</summary>
    /// <exception cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException">A time-out occurred.</exception>
    /// <returns>An object that contains all <see cref="T:System.Text.RegularExpressions.Match" /> objects within the <see cref="T:System.Text.RegularExpressions.MatchCollection" />.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) new MatchCollection.Enumerator(this);


    #nullable disable
    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator<Match> IEnumerable<Match>.GetEnumerator() => (IEnumerator<Match>) new MatchCollection.Enumerator(this);

    private Match GetMatch(int i)
    {
      if (this._matches.Count > i)
        return this._matches[i];
      if (this._done)
        return (Match) null;
      Match match;
      do
      {
        match = this._regex.Run(false, this._prevlen, this._input, 0, this._input.Length, this._startat);
        if (!match.Success)
        {
          this._done = true;
          return (Match) null;
        }
        this._matches.Add(match);
        this._prevlen = match.Length;
        this._startat = match._textpos;
      }
      while (this._matches.Count <= i);
      return match;
    }

    private void EnsureInitialized()
    {
      if (this._done)
        return;
      this.GetMatch(int.MaxValue);
    }

    /// <summary>Gets a value indicating whether access to the collection is synchronized (thread-safe).</summary>
    /// <returns>
    /// <see langword="false" /> in all cases.</returns>
    public bool IsSynchronized => false;


    #nullable enable
    /// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
    /// <returns>An object that can be used to synchronize access to the collection. This property always returns the object itself.</returns>
    public object SyncRoot => (object) this;

    /// <summary>Copies all the elements of the collection to the given array starting at the given index.</summary>
    /// <param name="array">The array the collection is to be copied into.</param>
    /// <param name="arrayIndex">The position in the array where copying is to begin.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="array" /> is a multi-dimensional array.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">
    ///        <paramref name="arrayIndex" /> is outside the bounds of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="arrayIndex" /> plus <see cref="P:System.Text.RegularExpressions.MatchCollection.Count" /> is outside the bounds of <paramref name="array" />.</exception>
    /// <exception cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException">A time-out occurred.</exception>
    public void CopyTo(Array array, int arrayIndex)
    {
      this.EnsureInitialized();
      ((ICollection) this._matches).CopyTo(array, arrayIndex);
    }

    /// <summary>Copies the elements of the collection to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the collection. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    public void CopyTo(Match[] array, int arrayIndex)
    {
      this.EnsureInitialized();
      this._matches.CopyTo(array, arrayIndex);
    }


    #nullable disable
    /// <summary>Determines the index of a specific item in the collection.</summary>
    /// <param name="item">The object to locate in the collection.</param>
    /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
    int IList<Match>.IndexOf(Match item)
    {
      this.EnsureInitialized();
      return this._matches.IndexOf(item);
    }

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
    /// <param name="item">The object to insert into the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList<Match>.Insert(int index, Match item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList<Match>.RemoveAt(int index) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);


    #nullable enable
    /// <summary>Gets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    Match IList<
    #nullable disable
    Match>.this[int index]
    {
      get => this[index];
      set => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);
    }

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="item">The object to add to the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void ICollection<Match>.Add(Match item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void ICollection<Match>.Clear() => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Determines whether the collection contains a specific value.</summary>
    /// <param name="item">The object to locate in the collection.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is found in the collection; otherwise, <see langword="false" />.</returns>
    bool ICollection<Match>.Contains(Match item)
    {
      this.EnsureInitialized();
      return this._matches.Contains(item);
    }

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="item">The object to remove from the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> was successfully removed from the collection; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original collection.</returns>
    bool ICollection<Match>.Remove(Match item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">The object to add to the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    /// <returns>Calling this method always throws <see cref="T:System.NotSupportedException" />.</returns>
    int IList.Add(object value) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Removes all items from the collection.</summary>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList.Clear() => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Determines whether the collection contains a specific value.</summary>
    /// <param name="value">The object to locate in the collection.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Object" /> is found in the collection; otherwise, <see langword="false" />.</returns>
    bool IList.Contains(object value) => value is Match && ((ICollection<Match>) this).Contains((Match) value);

    /// <summary>Determines the index of a specific item in the collection.</summary>
    /// <param name="value">The object to locate in the collection.</param>
    /// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
    int IList.IndexOf(object value) => !(value is Match match) ? -1 : ((IList<Match>) this).IndexOf(match);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
    /// <param name="value">The object to insert into the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList.Insert(int index, object value) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
    /// <returns>
    /// <see langword="true" />.</returns>
    bool IList.IsFixedSize => true;

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">The object to remove from the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList.Remove(object value) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList.RemoveAt(int index) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);


    #nullable enable
    /// <summary>Gets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    object? IList.this[int index]
    {
      get => (object) this[index];
      set => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);
    }


    #nullable disable
    private sealed class Enumerator : IEnumerator<Match>, IEnumerator, IDisposable
    {
      private readonly MatchCollection _collection;
      private int _index;

      internal Enumerator(MatchCollection collection)
      {
        this._collection = collection;
        this._index = -1;
      }

      public bool MoveNext()
      {
        if (this._index == -2)
          return false;
        ++this._index;
        if (this._collection.GetMatch(this._index) != null)
          return true;
        this._index = -2;
        return false;
      }

      public Match Current => this._index >= 0 ? this._collection.GetMatch(this._index) : throw new InvalidOperationException(SR.EnumNotStarted);

      object IEnumerator.Current => (object) this.Current;

      void IEnumerator.Reset() => this._index = -1;

      void IDisposable.Dispose()
      {
      }
    }
  }
}
