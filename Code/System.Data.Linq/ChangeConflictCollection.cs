// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.ChangeConflictCollection
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.Linq
{
  public sealed class ChangeConflictCollection : 
    ICollection<ObjectChangeConflict>,
    IEnumerable<ObjectChangeConflict>,
    IEnumerable,
    ICollection
  {
    private List<ObjectChangeConflict> conflicts;

    internal ChangeConflictCollection() => this.conflicts = new List<ObjectChangeConflict>();

    public int Count => this.conflicts.Count;

    public ObjectChangeConflict this[int index] => this.conflicts[index];

    bool ICollection<ObjectChangeConflict>.IsReadOnly => true;

    void ICollection<ObjectChangeConflict>.Add(
      ObjectChangeConflict item)
    {
      throw Error.CannotAddChangeConflicts();
    }

    public bool Remove(ObjectChangeConflict item) => this.conflicts.Remove(item);

    public void Clear() => this.conflicts.Clear();

    public bool Contains(ObjectChangeConflict item) => this.conflicts.Contains(item);

    public void CopyTo(ObjectChangeConflict[] array, int arrayIndex) => this.conflicts.CopyTo(array, arrayIndex);

    public IEnumerator<ObjectChangeConflict> GetEnumerator() => (IEnumerator<ObjectChangeConflict>) this.conflicts.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.conflicts.GetEnumerator();

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => (object) null;

    void ICollection.CopyTo(Array array, int index) => ((ICollection) this.conflicts).CopyTo(array, index);

    public void ResolveAll(RefreshMode mode) => this.ResolveAll(mode, true);

    public void ResolveAll(RefreshMode mode, bool autoResolveDeletes)
    {
      foreach (ObjectChangeConflict conflict in this.conflicts)
      {
        if (!conflict.IsResolved)
          conflict.Resolve(mode, autoResolveDeletes);
      }
    }

    internal void Fill(List<ObjectChangeConflict> conflictList) => this.conflicts = conflictList;
  }
}
