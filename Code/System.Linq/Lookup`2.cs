// Decompiled with JetBrains decompiler
// Type: System.Linq.Lookup`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class Lookup<TKey, TElement> : 
    IEnumerable<IGrouping<TKey, TElement>>,
    IEnumerable,
    ILookup<TKey, TElement>
  {
    private IEqualityComparer<TKey> comparer;
    private Lookup<TKey, TElement>.Grouping[] groupings;
    private Lookup<TKey, TElement>.Grouping lastGrouping;
    private int count;

    internal static Lookup<TKey, TElement> Create<TSource>(
      IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      if (elementSelector == null)
        throw Error.ArgumentNull(nameof (elementSelector));
      Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
      foreach (TSource source1 in source)
        lookup.GetGrouping(keySelector(source1), true).Add(elementSelector(source1));
      return lookup;
    }

    internal static Lookup<TKey, TElement> CreateForJoin(
      IEnumerable<TElement> source,
      Func<TElement, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      Lookup<TKey, TElement> forJoin = new Lookup<TKey, TElement>(comparer);
      foreach (TElement element in source)
      {
        TKey key = keySelector(element);
        if ((object) key != null)
          forJoin.GetGrouping(key, true).Add(element);
      }
      return forJoin;
    }

    private Lookup(IEqualityComparer<TKey> comparer)
    {
      if (comparer == null)
        comparer = (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
      this.comparer = comparer;
      this.groupings = new Lookup<TKey, TElement>.Grouping[7];
    }

    [__DynamicallyInvokable]
    public int Count
    {
      [__DynamicallyInvokable] get => this.count;
    }

    [__DynamicallyInvokable]
    public IEnumerable<TElement> this[TKey key]
    {
      [__DynamicallyInvokable] get => (IEnumerable<TElement>) this.GetGrouping(key, false) ?? (IEnumerable<TElement>) EmptyEnumerable<TElement>.Instance;
    }

    [__DynamicallyInvokable]
    public bool Contains(TKey key) => this.GetGrouping(key, false) != null;

    [__DynamicallyInvokable]
    public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
    {
      Lookup<TKey, TElement>.Grouping g = this.lastGrouping;
      if (g != null)
      {
        do
        {
          g = g.next;
          yield return (IGrouping<TKey, TElement>) g;
        }
        while (g != this.lastGrouping);
      }
    }

    [__DynamicallyInvokable]
    public IEnumerable<TResult> ApplyResultSelector<TResult>(
      Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
      Lookup<TKey, TElement>.Grouping g = this.lastGrouping;
      if (g != null)
      {
        do
        {
          g = g.next;
          if (g.count != g.elements.Length)
            Array.Resize<TElement>(ref g.elements, g.count);
          yield return resultSelector(g.key, (IEnumerable<TElement>) g.elements);
        }
        while (g != this.lastGrouping);
      }
    }

    [__DynamicallyInvokable]
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    internal int InternalGetHashCode(TKey key) => (object) key != null ? this.comparer.GetHashCode(key) & int.MaxValue : 0;

    internal Lookup<TKey, TElement>.Grouping GetGrouping(TKey key, bool create)
    {
      int hashCode = this.InternalGetHashCode(key);
      for (Lookup<TKey, TElement>.Grouping grouping = this.groupings[hashCode % this.groupings.Length]; grouping != null; grouping = grouping.hashNext)
      {
        if (grouping.hashCode == hashCode && this.comparer.Equals(grouping.key, key))
          return grouping;
      }
      if (!create)
        return (Lookup<TKey, TElement>.Grouping) null;
      if (this.count == this.groupings.Length)
        this.Resize();
      int index = hashCode % this.groupings.Length;
      Lookup<TKey, TElement>.Grouping grouping1 = new Lookup<TKey, TElement>.Grouping();
      grouping1.key = key;
      grouping1.hashCode = hashCode;
      grouping1.elements = new TElement[1];
      grouping1.hashNext = this.groupings[index];
      this.groupings[index] = grouping1;
      if (this.lastGrouping == null)
      {
        grouping1.next = grouping1;
      }
      else
      {
        grouping1.next = this.lastGrouping.next;
        this.lastGrouping.next = grouping1;
      }
      this.lastGrouping = grouping1;
      ++this.count;
      return grouping1;
    }

    private void Resize()
    {
      int length = checked (this.count * 2 + 1);
      Lookup<TKey, TElement>.Grouping[] groupingArray = new Lookup<TKey, TElement>.Grouping[length];
      Lookup<TKey, TElement>.Grouping grouping = this.lastGrouping;
      do
      {
        grouping = grouping.next;
        int index = grouping.hashCode % length;
        grouping.hashNext = groupingArray[index];
        groupingArray[index] = grouping;
      }
      while (grouping != this.lastGrouping);
      this.groupings = groupingArray;
    }

    internal class Grouping : 
      IGrouping<TKey, TElement>,
      IEnumerable<TElement>,
      IEnumerable,
      IList<TElement>,
      ICollection<TElement>
    {
      internal TKey key;
      internal int hashCode;
      internal TElement[] elements;
      internal int count;
      internal Lookup<TKey, TElement>.Grouping hashNext;
      internal Lookup<TKey, TElement>.Grouping next;

      internal void Add(TElement element)
      {
        if (this.elements.Length == this.count)
          Array.Resize<TElement>(ref this.elements, checked (this.count * 2));
        this.elements[this.count] = element;
        ++this.count;
      }

      public IEnumerator<TElement> GetEnumerator()
      {
        for (int i = 0; i < this.count; ++i)
          yield return this.elements[i];
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

      public TKey Key => this.key;

      int ICollection<TElement>.Count => this.count;

      bool ICollection<TElement>.IsReadOnly => true;

      void ICollection<TElement>.Add(TElement item) => throw Error.NotSupported();

      void ICollection<TElement>.Clear() => throw Error.NotSupported();

      bool ICollection<TElement>.Contains(TElement item) => Array.IndexOf<TElement>(this.elements, item, 0, this.count) >= 0;

      void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) => Array.Copy((Array) this.elements, 0, (Array) array, arrayIndex, this.count);

      bool ICollection<TElement>.Remove(TElement item) => throw Error.NotSupported();

      int IList<TElement>.IndexOf(TElement item) => Array.IndexOf<TElement>(this.elements, item, 0, this.count);

      void IList<TElement>.Insert(int index, TElement item) => throw Error.NotSupported();

      void IList<TElement>.RemoveAt(int index) => throw Error.NotSupported();

      TElement IList<TElement>.this[int index]
      {
        get => index >= 0 && index < this.count ? this.elements[index] : throw Error.ArgumentOutOfRange(nameof (index));
        set => throw Error.NotSupported();
      }
    }
  }
}
