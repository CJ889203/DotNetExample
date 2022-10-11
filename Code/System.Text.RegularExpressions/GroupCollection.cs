// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.GroupCollection
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>Returns the set of captured groups in a single match. The collection is immutable (read-only) and has no public constructor.</summary>
  [DebuggerDisplay("Count = {Count}")]
  [DebuggerTypeProxy(typeof (CollectionDebuggerProxy<Group>))]
  public class GroupCollection : 
    IList<Group>,
    ICollection<Group>,
    IEnumerable<Group>,
    IEnumerable,
    IReadOnlyList<Group>,
    IReadOnlyCollection<Group>,
    IList,
    ICollection,
    IReadOnlyDictionary<string, Group>,
    IEnumerable<KeyValuePair<string, Group>>,
    IReadOnlyCollection<KeyValuePair<string, Group>>
  {

    #nullable disable
    private readonly Match _match;
    private readonly Hashtable _captureMap;
    private Group[] _groups;

    internal GroupCollection(Match match, Hashtable caps)
    {
      this._match = match;
      this._captureMap = caps;
    }

    internal void Reset() => this._groups = (Group[]) null;

    /// <summary>Gets a value that indicates whether the collection is read-only.</summary>
    /// <returns>
    /// <see langword="true" /> in all cases.</returns>
    public bool IsReadOnly => true;

    /// <summary>Returns the number of groups in the collection.</summary>
    /// <returns>The number of groups in the collection.</returns>
    public int Count => this._match._matchcount.Length;


    #nullable enable
    /// <summary>Enables access to a member of the collection by integer index.</summary>
    /// <param name="groupnum">The zero-based index of the collection member to be retrieved.</param>
    /// <returns>The member of the collection specified by <paramref name="groupnum" />.</returns>
    public Group this[int groupnum] => this.GetGroup(groupnum);

    /// <summary>Enables access to a member of the collection by string index.</summary>
    /// <param name="groupname">The name of a capturing group.</param>
    /// <returns>The member of the collection specified by <paramref name="groupname" />.</returns>
    public Group this[string groupname] => this._match._regex != null ? this.GetGroup(this._match._regex.GroupNumberFromName(groupname)) : Group.s_emptyGroup;

    /// <summary>Provides an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that contains all <see cref="T:System.Text.RegularExpressions.Group" /> objects in the <see cref="T:System.Text.RegularExpressions.GroupCollection" />.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) new GroupCollection.Enumerator(this);


    #nullable disable
    /// <summary>Provides an enumerator that iterates through the group collection.</summary>
    /// <returns>An enumerator that contains all <see cref="T:System.Text.RegularExpressions.Group" /> objects in the group collection.</returns>
    IEnumerator<Group> IEnumerable<Group>.GetEnumerator() => (IEnumerator<Group>) new GroupCollection.Enumerator(this);

    private Group GetGroup(int groupnum)
    {
      if (this._captureMap != null)
      {
        int groupnum1;
        if (this._captureMap.TryGetValue<int>((object) groupnum, out groupnum1))
          return this.GetGroupImpl(groupnum1);
      }
      else if ((long) (uint) groupnum < (long) this._match._matchcount.Length)
        return this.GetGroupImpl(groupnum);
      return Group.s_emptyGroup;
    }

    private Group GetGroupImpl(int groupnum)
    {
      if (groupnum == 0)
        return (Group) this._match;
      if (this._groups == null)
      {
        this._groups = new Group[this._match._matchcount.Length - 1];
        for (int index = 0; index < this._groups.Length; ++index)
        {
          string name = this._match._regex.GroupNameFromNumber(index + 1);
          this._groups[index] = new Group(this._match.Text, this._match._matches[index + 1], this._match._matchcount[index + 1], name);
        }
      }
      return this._groups[groupnum - 1];
    }

    /// <summary>Gets a value that indicates whether access to the <see cref="T:System.Text.RegularExpressions.GroupCollection" /> is synchronized (thread-safe).</summary>
    /// <returns>
    /// <see langword="false" /> in all cases.</returns>
    public bool IsSynchronized => false;


    #nullable enable
    /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Text.RegularExpressions.GroupCollection" />.</summary>
    /// <returns>A copy of the <see cref="T:System.Text.RegularExpressions.Match" /> object to synchronize.</returns>
    public object SyncRoot => (object) this._match;

    /// <summary>Copies all the elements of the collection to the given array beginning at the given index.</summary>
    /// <param name="array">The array the collection is to be copied into.</param>
    /// <param name="arrayIndex">The position in the destination array where the copying is to begin.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">
    ///         <paramref name="arrayIndex" /> is outside the bounds of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="arrayIndex" /> plus <see cref="P:System.Text.RegularExpressions.GroupCollection.Count" /> is outside the bounds of <paramref name="array" />.</exception>
    public void CopyTo(Array array, int arrayIndex)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      int index = arrayIndex;
      for (int groupnum = 0; groupnum < this.Count; ++groupnum)
      {
        array.SetValue((object) this[groupnum], index);
        ++index;
      }
    }

    /// <summary>Copies the elements of the group collection to a <see cref="T:System.Text.RegularExpressions.Group" /> array, starting at a particular array index.</summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from the group collection. The array must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="arrayIndex" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="arrayIndex" /> is greater than the length of <paramref name="array" />.</exception>
    /// <exception cref="T:System.ArgumentException">The length of <paramref name="array" /> - <paramref name="arrayIndex" /> is less than the group collection count.</exception>
    public void CopyTo(Group[] array, int arrayIndex)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (arrayIndex < 0 || arrayIndex > array.Length)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex));
      if (array.Length - arrayIndex < this.Count)
        throw new ArgumentException(SR.Arg_ArrayPlusOffTooSmall);
      int index = arrayIndex;
      for (int groupnum = 0; groupnum < this.Count; ++groupnum)
      {
        array[index] = this[groupnum];
        ++index;
      }
    }


    #nullable disable
    /// <summary>Determines the index of a specific group in the group collection.</summary>
    /// <param name="item">The group to locate in the group collection.</param>
    /// <returns>The index of the <paramref name="item" /> if found; otherwise, -1.</returns>
    int IList<Group>.IndexOf(Group item)
    {
      for (int groupnum = 0; groupnum < this.Count; ++groupnum)
      {
        if (EqualityComparer<Group>.Default.Equals(this[groupnum], item))
          return groupnum;
      }
      return -1;
    }

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The position at which to insert the group.</param>
    /// <param name="item">The group to insert.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    void IList<Group>.Insert(int index, Group item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index of the group to remove.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    void IList<Group>.RemoveAt(int index) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);


    #nullable enable
    /// <summary>Gets the group at the specified position in the collection.</summary>
    /// <param name="index">The zero-based index of the group in the group collection.</param>
    /// <exception cref="T:System.NotSupportedException">Cannot set an item. This is a read-only collection.</exception>
    /// <returns>The group in the desired position.</returns>
    Group IList<
    #nullable disable
    Group>.this[int index]
    {
      get => this[index];
      set => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);
    }

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="item">The group to add to the collection.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    void ICollection<Group>.Add(Group item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    void ICollection<Group>.Clear() => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Determines whether the group collection contains a specific group item.</summary>
    /// <param name="item">The group to locate in the group collection.</param>
    /// <returns>
    /// <see langword="true" /> if the group item is found in the group collection; <see langword="false" /> otherwise.</returns>
    bool ICollection<Group>.Contains(Group item) => ((IList<Group>) this).IndexOf(item) >= 0;

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="item">The group to remove.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    /// <returns>Calling this method always throws <see cref="T:System.NotSupportedException" />.</returns>
    bool ICollection<Group>.Remove(Group item) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">The object to add to the group collection.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    /// <returns>Calling this method always throws <see cref="T:System.NotSupportedException" />.</returns>
    int IList.Add(object value) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    void IList.Clear() => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Determines whether the group collection contains a specific group item.</summary>
    /// <param name="value">The group to locate in the group collection.</param>
    /// <returns>
    /// <see langword="true" /> if the group item is found in the group collection; <see langword="false" /> otherwise.</returns>
    bool IList.Contains(object value) => value is Group group && ((ICollection<Group>) this).Contains(group);

    /// <summary>Determines the index of a specific group in the group collection.</summary>
    /// <param name="value">The group to locate in the group collection.</param>
    /// <returns>The index of the <paramref name="item" /> if found; otherwise, -1.</returns>
    int IList.IndexOf(object value) => !(value is Group group) ? -1 : ((IList<Group>) this).IndexOf(group);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The position at which to insert the group.</param>
    /// <param name="value">The group to insert.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    void IList.Insert(int index, object value) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Gets a value indicating whether the group collection has a fixed size.</summary>
    /// <returns>
    /// <see langword="true" /> always.</returns>
    bool IList.IsFixedSize => true;

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">The group to remove.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    void IList.Remove(object value) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);

    /// <summary>Calling this method always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="index">The zero-based index of the group to remove.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported. This is a read-only collection.</exception>
    void IList.RemoveAt(int index) => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);


    #nullable enable
    /// <summary>Gets the group in the desired position.</summary>
    /// <param name="index">The zero-index position of the group in the group collection.</param>
    /// <exception cref="T:System.NotSupportedException">Cannot set an item. This is a read-only collection.</exception>
    /// <returns>The group in the desired position.</returns>
    object? IList.this[int index]
    {
      get => (object) this[index];
      set => throw new NotSupportedException(SR.NotSupported_ReadOnlyCollection);
    }


    #nullable disable
    /// <summary>Provides an enumerator that iterates through the group collection.</summary>
    /// <returns>An enumerator that contains all names and objects in the <see cref="T:System.Text.RegularExpressions.Group" /> collection.</returns>
    IEnumerator<KeyValuePair<string, Group>> IEnumerable<KeyValuePair<string, Group>>.GetEnumerator() => (IEnumerator<KeyValuePair<string, Group>>) new GroupCollection.Enumerator(this);


    #nullable enable
    /// <summary>Attempts to retrieve a group identified by the provided name key, if it exists in the group collection.</summary>
    /// <param name="key">A string with the group name key to look for.</param>
    /// <param name="value">When the method returns, the group whose name is <paramref name="key" />, if it is found; otherwise, <see langword="null" /> if not found.</param>
    /// <returns>
    /// <see langword="true" /> if a group identified by the provided name key exists; <see langword="false" /> otherwise.</returns>
    public bool TryGetValue(string key, [NotNullWhen(true)] out Group? value)
    {
      Group group = this[key];
      if (group == Group.s_emptyGroup)
      {
        value = (Group) null;
        return false;
      }
      value = group;
      return true;
    }

    /// <summary>Determines whether the group collection contains a captured group identified by the specified name.</summary>
    /// <param name="key">A string with the name of the captured group to locate.</param>
    /// <returns>
    /// <see langword="true" /> if the group collection contains a captured group identified by <paramref name="key" />; <see langword="false" /> otherwise.</returns>
    public bool ContainsKey(string key) => this._match._regex.GroupNumberFromName(key) >= 0;

    /// <summary>Gets a string enumeration that contains the name keys of the group collection.</summary>
    /// <returns>The name keys of the group collection.</returns>
    public IEnumerable<string> Keys
    {
      get
      {
        for (int i = 0; i < this.Count; ++i)
          yield return this.GetGroup(i).Name;
      }
    }

    /// <summary>Gets a group enumeration with all the groups in the group collection.</summary>
    /// <returns>A group enumeration.</returns>
    public IEnumerable<Group> Values
    {
      get
      {
        for (int i = 0; i < this.Count; ++i)
          yield return this.GetGroup(i);
      }
    }


    #nullable disable
    private sealed class Enumerator : 
      IEnumerator<Group>,
      IEnumerator,
      IDisposable,
      IEnumerator<KeyValuePair<string, Group>>
    {
      private readonly GroupCollection _collection;
      private int _index;

      internal Enumerator(GroupCollection collection)
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

      public Group Current
      {
        get
        {
          if (this._index < 0 || this._index >= this._collection.Count)
            throw new InvalidOperationException(SR.EnumNotStarted);
          return this._collection[this._index];
        }
      }

      KeyValuePair<string, Group> IEnumerator<KeyValuePair<string, Group>>.Current
      {
        get
        {
          if ((long) (uint) this._index >= (long) this._collection.Count)
            throw new InvalidOperationException(SR.EnumNotStarted);
          Group group = this._collection[this._index];
          return new KeyValuePair<string, Group>(group.Name, group);
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
