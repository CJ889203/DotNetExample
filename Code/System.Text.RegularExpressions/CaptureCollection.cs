// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.CaptureCollection
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
  /// <summary>Represents the set of captures made by a single capturing group. The collection is immutable (read-only) and has no public constructor.</summary>
  [DebuggerDisplay("Count = {Count}")]
  [DebuggerTypeProxy(typeof (CollectionDebuggerProxy<Capture>))]
  public class CaptureCollection : 
    IList<Capture>,
    ICollection<Capture>,
    IEnumerable<Capture>,
    IEnumerable,
    IReadOnlyList<Capture>,
    IReadOnlyCollection<Capture>,
    IList,
    ICollection
  {

    #nullable disable
    private readonly Group _group;
    private readonly int _capcount;
    private Capture[] _captures;

    internal CaptureCollection(Group group)
    {
      this._group = group;
      this._capcount = this._group._capcount;
    }

    /// <summary>Gets a value that indicates whether the collection is read only.</summary>
    /// <returns>
    /// <see langword="true" /> in all cases.</returns>
    public bool IsReadOnly => true;

    /// <summary>Gets the number of substrings captured by the group.</summary>
    /// <returns>The number of items in the <see cref="T:System.Text.RegularExpressions.CaptureCollection" />.</returns>
    public int Count => this._capcount;


    #nullable enable
    /// <summary>Gets an individual member of the collection.</summary>
    /// <param name="i">Index into the capture collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="i" /> is less than 0 or greater than <see cref="P:System.Text.RegularExpressions.CaptureCollection.Count" />.</exception>
    /// <returns>The captured substring at position <paramref name="i" /> in the collection.</returns>
    public Capture this[int i] => this.GetCapture(i);

    /// <summary>Provides an enumerator that iterates through the collection.</summary>
    /// <returns>An object that contains all <see cref="T:System.Text.RegularExpressions.Capture" /> objects within the <see cref="T:System.Text.RegularExpressions.CaptureCollection" />.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) new CaptureCollection.Enumerator(this);


    #nullable disable
    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator<Capture> IEnumerable<Capture>.GetEnumerator() => (IEnumerator<Capture>) new CaptureCollection.Enumerator(this);

    private Capture GetCapture(int i)
    {
      if ((long) (uint) i == (long) (this._capcount - 1))
        return (Capture) this._group;
      if (i >= this._capcount || i < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.i);
      if (this._captures == null)
        this.ForceInitialized();
      return this._captures[i];
    }

    internal void ForceInitialized()
    {
      this._captures = new Capture[this._capcount];
      for (int index = 0; index < this._capcount - 1; ++index)
        this._captures[index] = new Capture(this._group.Text, this._group._caps[index * 2], this._group._caps[index * 2 + 1]);
    }

    /// <summary>Gets a value that indicates whether access to the collection is synchronized (thread-safe).</summary>
    /// <returns>
    /// <see langword="false" /> in all cases.</returns>
    public bool IsSynchronized => false;


    #nullable enable
    /// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
    /// <returns>An object that can be used to synchronize access to the collection.</returns>
    public object SyncRoot => (object) this._group;

    /// <summary>Copies all the elements of the collection to the given array beginning at the given index.</summary>
    /// <param name="array">The array the collection is to be copied into.</param>
    /// <param name="arrayIndex">The position in the destination array where copying is to begin.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="arrayIndex" /> is outside the bounds of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="arrayIndex" /> plus <see cref="P:System.Text.RegularExpressions.CaptureCollection.Count" /> is outside the bounds of <paramref name="array" />.</exception>
    public void CopyTo(Array array, int arrayIndex)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      int index = arrayIndex;
      for (int i = 0; i < this.Count; ++i)
      {
        array.SetValue((object) this[i], index);
        ++index;
      }
    }

    /// <summary>Copies the elements of the collection to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the collection. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="arrayIndex" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in the source collection is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
    public void CopyTo(Capture[] array, int arrayIndex)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if ((uint) arrayIndex > (uint) array.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex);
      if (array.Length - arrayIndex < this.Count)
        throw new ArgumentException(SR.Arg_ArrayPlusOffTooSmall);
      int index = arrayIndex;
      for (int i = 0; i < this.Count; ++i)
      {
        array[index] = this[i];
        ++index;
      }
    }


    #nullable disable
    /// <summary>Determines the index of a specific item in the collection.</summary>
    /// <param name="item">The object to locate in the collection.</param>
    /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
    int IList<Capture>.IndexOf(Capture item)
    {
      for (int i = 0; i < this.Count; ++i)
      {
        if (EqualityComparer<Capture>.Default.Equals(this[i], item))
          return i;
      }
      return -1;
    }

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
    /// <param name="item">The object to insert into the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList<Capture>.Insert(int index, Capture item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList<Capture>.RemoveAt(int index) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);


    #nullable enable
    /// <summary>Gets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    Capture IList<
    #nullable disable
    Capture>.this[int index]
    {
      get => this[index];
      set => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);
    }

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="item">The object to add to the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void ICollection<Capture>.Add(Capture item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void ICollection<Capture>.Clear() => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Determines whether the collection contains a specific value.</summary>
    /// <param name="item">The object to locate in the collection.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is found in the collection; otherwise, <see langword="false" />.</returns>
    bool ICollection<Capture>.Contains(Capture item) => ((IList<Capture>) this).IndexOf(item) >= 0;

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="item">The object to remove from the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    /// <returns>Calling this method always throws <see cref="T:System.NotSupportedException" />.</returns>
    bool ICollection<Capture>.Remove(Capture item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">The object to add to the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    /// <returns>Calling this method always throws <see cref="T:System.NotSupportedException" />.</returns>
    int IList.Add(object value) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList.Clear() => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Determines whether the collection contains a specific value.</summary>
    /// <param name="value">The object to locate in the collection.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Object" /> is found in the collection; otherwise, <see langword="false" />.</returns>
    bool IList.Contains(object value) => value is Capture capture && ((ICollection<Capture>) this).Contains(capture);

    /// <summary>Determines the index of a specific item in the collection.</summary>
    /// <param name="value">The object to locate in the collection.</param>
    /// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
    int IList.IndexOf(object value) => !(value is Capture capture) ? -1 : ((IList<Capture>) this).IndexOf(capture);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
    /// <param name="value">The object to insert into the collection.</param>
    /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
    void IList.Insert(int index, object value) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
    /// <returns>
    /// <see langword="true" />, since the collection has a fixed size.</returns>
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
    private sealed class Enumerator : IEnumerator<Capture>, IEnumerator, IDisposable
    {
      private readonly CaptureCollection _collection;
      private int _index;

      internal Enumerator(CaptureCollection collection)
      {
        this._collection = collection;
        this._index = -1;
      }

      public bool MoveNext()
      {
        int count = this._collection.Count;
        if (this._index >= count)
          return false;
        ++this._index;
        return this._index < count;
      }

      public Capture Current
      {
        get
        {
          if (this._index < 0 || this._index >= this._collection.Count)
            throw new InvalidOperationException(SR.EnumNotStarted);
          return this._collection[this._index];
        }
      }

      object IEnumerator.Current => (object) this.Current;

      void IEnumerator.Reset() => this._index = -1;

      void IDisposable.Dispose()
      {
      }
    }
  }
}
