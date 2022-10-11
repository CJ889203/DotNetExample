// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.EntitySet`1
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Data.Linq
{
  public sealed class EntitySet<TEntity> : 
    IList,
    ICollection,
    IEnumerable,
    IList<TEntity>,
    ICollection<TEntity>,
    IEnumerable<TEntity>,
    IListSource
    where TEntity : class
  {
    private IEnumerable<TEntity> source;
    private ItemList<TEntity> entities;
    private ItemList<TEntity> removedEntities;
    private Action<TEntity> onAdd;
    private Action<TEntity> onRemove;
    private TEntity onAddEntity;
    private TEntity onRemoveEntity;
    private int version;
    private ListChangedEventHandler onListChanged;
    private bool isModified;
    private bool isLoaded;
    private bool listChanged;
    private IBindingList cachedList;

    public EntitySet()
    {
    }

    public EntitySet(Action<TEntity> onAdd, Action<TEntity> onRemove)
    {
      this.onAdd = onAdd;
      this.onRemove = onRemove;
    }

    internal EntitySet(EntitySet<TEntity> es, bool copyNotifications)
    {
      this.source = es.source;
      foreach (TEntity entity in es.entities)
        this.entities.Add(entity);
      foreach (TEntity removedEntity in es.removedEntities)
        this.removedEntities.Add(removedEntity);
      this.version = es.version;
      if (!copyNotifications)
        return;
      this.onAdd = es.onAdd;
      this.onRemove = es.onRemove;
    }

    public int Count
    {
      get
      {
        this.Load();
        return this.entities.Count;
      }
    }

    public TEntity this[int index]
    {
      get
      {
        this.Load();
        if (index < 0 || index >= this.entities.Count)
          throw Error.ArgumentOutOfRange(nameof (index));
        return this.entities[index];
      }
      set
      {
        this.Load();
        if (index < 0 || index >= this.entities.Count)
          throw Error.ArgumentOutOfRange(nameof (index));
        if ((object) value == null || this.IndexOf(value) >= 0)
          throw Error.ArgumentOutOfRange(nameof (value));
        this.CheckModify();
        this.OnRemove(this.entities[index]);
        this.OnListChanged(ListChangedType.ItemDeleted, index);
        this.OnAdd(value);
        this.entities[index] = value;
        this.OnModified();
        this.OnListChanged(ListChangedType.ItemAdded, index);
      }
    }

    public void Add(TEntity entity)
    {
      if ((object) entity == null)
        throw Error.ArgumentNull(nameof (entity));
      if ((object) entity == (object) this.onAddEntity)
        return;
      this.CheckModify();
      if (!this.entities.Contains(entity))
      {
        this.OnAdd(entity);
        if (this.HasSource)
          this.removedEntities.Remove(entity);
        this.entities.Add(entity);
        this.OnListChanged(ListChangedType.ItemAdded, this.entities.IndexOf(entity));
      }
      this.OnModified();
    }

    public void AddRange(IEnumerable<TEntity> collection)
    {
      if (collection == null)
        throw Error.ArgumentNull(nameof (collection));
      this.CheckModify();
      collection = (IEnumerable<TEntity>) collection.ToList<TEntity>();
      foreach (TEntity entity in collection)
      {
        if (!this.entities.Contains(entity))
        {
          this.OnAdd(entity);
          if (this.HasSource)
            this.removedEntities.Remove(entity);
          this.entities.Add(entity);
          this.OnListChanged(ListChangedType.ItemAdded, this.entities.IndexOf(entity));
        }
      }
      this.OnModified();
    }

    public void Assign(IEnumerable<TEntity> entitySource)
    {
      if (this == entitySource)
        return;
      this.Clear();
      if (entitySource != null)
        this.AddRange(entitySource);
      this.isLoaded = true;
    }

    public void Clear()
    {
      this.Load();
      this.CheckModify();
      if (this.entities.Items != null)
      {
        foreach (TEntity entity in new List<TEntity>((IEnumerable<TEntity>) this.entities.Items))
          this.Remove(entity);
      }
      this.entities = new ItemList<TEntity>();
      this.OnModified();
      this.OnListChanged(ListChangedType.Reset, 0);
    }

    public bool Contains(TEntity entity) => this.IndexOf(entity) >= 0;

    public void CopyTo(TEntity[] array, int arrayIndex)
    {
      this.Load();
      if (this.entities.Count <= 0)
        return;
      Array.Copy((Array) this.entities.Items, 0, (Array) array, arrayIndex, this.entities.Count);
    }

    public IEnumerator<TEntity> GetEnumerator()
    {
      this.Load();
      return (IEnumerator<TEntity>) new EntitySet<TEntity>.Enumerator(this);
    }

    internal IEnumerable<TEntity> GetUnderlyingValues() => (IEnumerable<TEntity>) new EntitySet<TEntity>.UnderlyingValues(this);

    public int IndexOf(TEntity entity)
    {
      this.Load();
      return this.entities.IndexOf(entity);
    }

    public void Insert(int index, TEntity entity)
    {
      this.Load();
      if (index < 0 || index > this.Count)
        throw Error.ArgumentOutOfRange(nameof (index));
      if ((object) entity == null || this.IndexOf(entity) >= 0)
        throw Error.ArgumentOutOfRange(nameof (entity));
      this.CheckModify();
      this.entities.Insert(index, entity);
      this.OnListChanged(ListChangedType.ItemAdded, index);
      this.OnAdd(entity);
    }

    public bool IsDeferred => this.HasSource;

    internal bool HasValues => this.source == null || this.HasAssignedValues || this.HasLoadedValues;

    public bool HasLoadedOrAssignedValues => this.HasAssignedValues || this.HasLoadedValues;

    internal bool HasAssignedValues => this.isModified;

    internal bool HasLoadedValues => this.isLoaded;

    internal bool HasSource => this.source != null && !this.HasLoadedValues;

    internal bool IsLoaded => this.isLoaded;

    internal IEnumerable<TEntity> Source => this.source;

    public void Load()
    {
      if (!this.HasSource)
        return;
      ItemList<TEntity> entities = this.entities;
      this.entities = new ItemList<TEntity>();
      foreach (TEntity entity in this.source)
        this.entities.Add(entity);
      foreach (TEntity entity in entities)
        this.entities.Include(entity);
      foreach (TEntity removedEntity in this.removedEntities)
        this.entities.Remove(removedEntity);
      this.source = SourceState<TEntity>.Loaded;
      this.isLoaded = true;
      this.removedEntities = new ItemList<TEntity>();
    }

    private void OnModified() => this.isModified = true;

    public bool Remove(TEntity entity)
    {
      if ((object) entity == null || (object) entity == (object) this.onRemoveEntity)
        return false;
      this.CheckModify();
      int index = -1;
      bool flag = false;
      if (this.HasSource)
      {
        if (!this.removedEntities.Contains(entity))
        {
          this.OnRemove(entity);
          index = this.entities.IndexOf(entity);
          if (index != -1)
            this.entities.RemoveAt(index);
          else
            this.removedEntities.Add(entity);
          flag = true;
        }
      }
      else
      {
        index = this.entities.IndexOf(entity);
        if (index != -1)
        {
          this.OnRemove(entity);
          this.entities.RemoveAt(index);
          flag = true;
        }
      }
      if (flag)
      {
        this.OnModified();
        if (index != -1)
          this.OnListChanged(ListChangedType.ItemDeleted, index);
      }
      return flag;
    }

    public void RemoveAt(int index)
    {
      this.Load();
      if (index < 0 || index >= this.Count)
        throw Error.ArgumentOutOfRange(nameof (index));
      this.CheckModify();
      this.OnRemove(this.entities[index]);
      this.entities.RemoveAt(index);
      this.OnModified();
      this.OnListChanged(ListChangedType.ItemDeleted, index);
    }

    public void SetSource(IEnumerable<TEntity> entitySource)
    {
      if (this.HasAssignedValues || this.HasLoadedValues)
        throw Error.EntitySetAlreadyLoaded();
      this.source = entitySource;
    }

    private void CheckModify()
    {
      if ((object) this.onAddEntity != null || (object) this.onRemoveEntity != null)
        throw Error.ModifyDuringAddOrRemove();
      ++this.version;
    }

    private void OnAdd(TEntity entity)
    {
      if (this.onAdd == null)
        return;
      TEntity onAddEntity = this.onAddEntity;
      this.onAddEntity = entity;
      try
      {
        this.onAdd(entity);
      }
      finally
      {
        this.onAddEntity = onAddEntity;
      }
    }

    private void OnRemove(TEntity entity)
    {
      if (this.onRemove == null)
        return;
      TEntity onRemoveEntity = this.onRemoveEntity;
      this.onRemoveEntity = entity;
      try
      {
        this.onRemove(entity);
      }
      finally
      {
        this.onRemoveEntity = onRemoveEntity;
      }
    }

    int IList.Add(object value)
    {
      if (!(value is TEntity entity) || this.IndexOf(entity) >= 0)
        throw Error.ArgumentOutOfRange(nameof (value));
      this.CheckModify();
      int count = this.entities.Count;
      this.entities.Add(entity);
      this.OnAdd(entity);
      return count;
    }

    bool IList.Contains(object value) => this.Contains(value as TEntity);

    int IList.IndexOf(object value) => this.IndexOf(value as TEntity);

    void IList.Insert(int index, object value)
    {
      TEntity entity = value as TEntity;
      if (value == null)
        throw Error.ArgumentOutOfRange(nameof (value));
      this.Insert(index, entity);
    }

    bool IList.IsFixedSize => false;

    bool IList.IsReadOnly => false;

    void IList.Remove(object value) => this.Remove(value as TEntity);

    object IList.this[int index]
    {
      get => (object) this[index];
      set
      {
        TEntity entity = value as TEntity;
        if (value == null)
          throw Error.ArgumentOutOfRange(nameof (value));
        this[index] = entity;
      }
    }

    void ICollection.CopyTo(Array array, int index)
    {
      this.Load();
      if (this.entities.Count <= 0)
        return;
      Array.Copy((Array) this.entities.Items, 0, array, index, this.entities.Count);
    }

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => (object) this;

    bool ICollection<TEntity>.IsReadOnly => false;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private void OnListChanged(ListChangedType type, int index)
    {
      this.listChanged = true;
      if (this.onListChanged == null)
        return;
      this.onListChanged((object) this, new ListChangedEventArgs(type, index));
    }

    public event ListChangedEventHandler ListChanged
    {
      add => this.onListChanged += value;
      remove => this.onListChanged -= value;
    }

    bool IListSource.ContainsListCollection => true;

    IList IListSource.GetList()
    {
      if (this.cachedList == null || this.listChanged)
      {
        this.cachedList = this.GetNewBindingList();
        this.listChanged = false;
      }
      return (IList) this.cachedList;
    }

    public IBindingList GetNewBindingList() => (IBindingList) new EntitySetBindingList<TEntity>((IList<TEntity>) this.ToList<TEntity>(), this);

    private class UnderlyingValues : IEnumerable<TEntity>, IEnumerable
    {
      private EntitySet<TEntity> entitySet;

      internal UnderlyingValues(EntitySet<TEntity> entitySet) => this.entitySet = entitySet;

      public IEnumerator<TEntity> GetEnumerator() => (IEnumerator<TEntity>) new EntitySet<TEntity>.Enumerator(this.entitySet);

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }

    private class Enumerable : IEnumerable<TEntity>, IEnumerable
    {
      private EntitySet<TEntity> entitySet;

      public Enumerable(EntitySet<TEntity> entitySet) => this.entitySet = entitySet;

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

      public IEnumerator<TEntity> GetEnumerator() => (IEnumerator<TEntity>) new EntitySet<TEntity>.Enumerator(this.entitySet);
    }

    private class Enumerator : IEnumerator<TEntity>, IDisposable, IEnumerator
    {
      private EntitySet<TEntity> entitySet;
      private TEntity[] items;
      private int index;
      private int endIndex;
      private int version;

      public Enumerator(EntitySet<TEntity> entitySet)
      {
        this.entitySet = entitySet;
        this.items = entitySet.entities.Items;
        this.index = -1;
        this.endIndex = entitySet.entities.Count - 1;
        this.version = entitySet.version;
      }

      public void Dispose() => GC.SuppressFinalize((object) this);

      public bool MoveNext()
      {
        if (this.version != this.entitySet.version)
          throw Error.EntitySetModifiedDuringEnumeration();
        if (this.index == this.endIndex)
          return false;
        ++this.index;
        return true;
      }

      public TEntity Current => this.items[this.index];

      object IEnumerator.Current => (object) this.items[this.index];

      void IEnumerator.Reset()
      {
        if (this.version != this.entitySet.version)
          throw Error.EntitySetModifiedDuringEnumeration();
        this.index = -1;
      }
    }
  }
}
