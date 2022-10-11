// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.EntityRef`1
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections.Generic;

namespace System.Data.Linq
{
  public struct EntityRef<TEntity> where TEntity : class
  {
    private IEnumerable<TEntity> source;
    private TEntity entity;

    public EntityRef(TEntity entity)
    {
      this.entity = entity;
      this.source = SourceState<TEntity>.Assigned;
    }

    public EntityRef(IEnumerable<TEntity> source)
    {
      this.source = source;
      this.entity = default (TEntity);
    }

    public EntityRef(EntityRef<TEntity> entityRef)
    {
      this.source = entityRef.source;
      this.entity = entityRef.entity;
    }

    public TEntity Entity
    {
      get
      {
        if (this.HasSource)
        {
          this.entity = this.source.SingleOrDefault<TEntity>();
          this.source = SourceState<TEntity>.Loaded;
        }
        return this.entity;
      }
      set
      {
        this.entity = value;
        this.source = SourceState<TEntity>.Assigned;
      }
    }

    public bool HasLoadedOrAssignedValue => this.HasLoadedValue || this.HasAssignedValue;

    internal bool HasValue => this.source == null || this.HasLoadedValue || this.HasAssignedValue;

    internal bool HasLoadedValue => this.source == SourceState<TEntity>.Loaded;

    internal bool HasAssignedValue => this.source == SourceState<TEntity>.Assigned;

    internal bool HasSource => this.source != null && !this.HasLoadedValue && !this.HasAssignedValue;

    internal IEnumerable<TEntity> Source => this.source;

    internal TEntity UnderlyingValue => this.entity;
  }
}
