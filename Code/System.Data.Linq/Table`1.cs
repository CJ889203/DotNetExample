// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.Table`1
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Data.Linq
{
  public sealed class Table<TEntity> : 
    IQueryable<TEntity>,
    IEnumerable<TEntity>,
    IEnumerable,
    IQueryable,
    IQueryProvider,
    ITable,
    IListSource,
    ITable<TEntity>
    where TEntity : class
  {
    private DataContext context;
    private MetaTable metaTable;
    private IBindingList cachedList;

    internal Table(DataContext context, MetaTable metaTable)
    {
      this.context = context;
      this.metaTable = metaTable;
    }

    public DataContext Context => this.context;

    public bool IsReadOnly => !this.metaTable.RowType.IsEntity;

    Expression IQueryable.Expression => (Expression) Expression.Constant((object) this);

    Type IQueryable.ElementType => typeof (TEntity);

    IQueryProvider IQueryable.Provider => (IQueryProvider) this;

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    IQueryable IQueryProvider.CreateQuery(Expression expression)
    {
      Type type = expression != null ? TypeSystem.GetElementType(expression.Type) : throw Error.ArgumentNull(nameof (expression));
      Type p1 = typeof (IQueryable<>).MakeGenericType(type);
      if (!p1.IsAssignableFrom(expression.Type))
        throw Error.ExpectedQueryableArgument((object) nameof (expression), (object) p1);
      return (IQueryable) Activator.CreateInstance(typeof (DataQuery<>).MakeGenericType(type), (object) this.context, (object) expression);
    }

    IQueryable<TResult> IQueryProvider.CreateQuery<TResult>(
      Expression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull(nameof (expression));
      return typeof (IQueryable<TResult>).IsAssignableFrom(expression.Type) ? (IQueryable<TResult>) new DataQuery<TResult>(this.context, expression) : throw Error.ExpectedQueryableArgument((object) nameof (expression), (object) typeof (IEnumerable<TResult>));
    }

    object IQueryProvider.Execute(Expression expression) => this.context.Provider.Execute(expression).ReturnValue;

    TResult IQueryProvider.Execute<TResult>(Expression expression) => (TResult) this.context.Provider.Execute(expression).ReturnValue;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => this.GetEnumerator();

    public IEnumerator<TEntity> GetEnumerator() => ((IEnumerable<TEntity>) this.context.Provider.Execute((Expression) Expression.Constant((object) this)).ReturnValue).GetEnumerator();

    bool IListSource.ContainsListCollection => false;

    IList IListSource.GetList()
    {
      if (this.cachedList == null)
        this.cachedList = this.GetNewBindingList();
      return (IList) this.cachedList;
    }

    public IBindingList GetNewBindingList() => BindingList.Create<TEntity>(this.context, (IEnumerable<TEntity>) this);

    public void InsertOnSubmit(TEntity entity)
    {
      if ((object) entity == null)
        throw Error.ArgumentNull(nameof (entity));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      MetaType inheritanceType = this.metaTable.RowType.GetInheritanceType(entity.GetType());
      if (!Table<TEntity>.IsTrackableType(inheritanceType))
        throw Error.TypeCouldNotBeAdded((object) inheritanceType.Type);
      TrackedObject trackedObject = this.context.Services.ChangeTracker.GetTrackedObject((object) entity);
      if (trackedObject == null)
        this.context.Services.ChangeTracker.Track((object) entity).ConvertToNew();
      else if (trackedObject.IsWeaklyTracked)
        trackedObject.ConvertToNew();
      else if (trackedObject.IsDeleted)
        trackedObject.ConvertToPossiblyModified();
      else if (trackedObject.IsRemoved)
        trackedObject.ConvertToNew();
      else if (!trackedObject.IsNew)
        throw Error.CantAddAlreadyExistingItem();
    }

    void ITable.InsertOnSubmit(object entity)
    {
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      if (!(entity is TEntity entity1))
        throw Error.EntityIsTheWrongType();
      this.InsertOnSubmit(entity1);
    }

    public void InsertAllOnSubmit<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
    {
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      foreach (TSubEntity subEntity in entities.ToList<TSubEntity>())
        this.InsertOnSubmit((TEntity) subEntity);
    }

    void ITable.InsertAllOnSubmit(IEnumerable entities)
    {
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      List<object> list = entities.Cast<object>().ToList<object>();
      ITable table = (ITable) this;
      foreach (object entity in list)
        table.InsertOnSubmit(entity);
    }

    private static bool IsTrackableType(MetaType type) => type != null && type.CanInstantiate && (!type.HasInheritance || type.HasInheritanceCode);

    public void DeleteOnSubmit(TEntity entity)
    {
      if ((object) entity == null)
        throw Error.ArgumentNull(nameof (entity));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      TrackedObject trackedObject = this.context.Services.ChangeTracker.GetTrackedObject((object) entity);
      if (trackedObject == null)
        throw Error.CannotRemoveUnattachedEntity();
      if (trackedObject.IsNew)
      {
        trackedObject.ConvertToRemoved();
      }
      else
      {
        if (!trackedObject.IsPossiblyModified && !trackedObject.IsModified)
          return;
        trackedObject.ConvertToDeleted();
      }
    }

    void ITable.DeleteOnSubmit(object entity)
    {
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      if (!(entity is TEntity entity1))
        throw Error.EntityIsTheWrongType();
      this.DeleteOnSubmit(entity1);
    }

    public void DeleteAllOnSubmit<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
    {
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      foreach (TSubEntity subEntity in entities.ToList<TSubEntity>())
        this.DeleteOnSubmit((TEntity) subEntity);
    }

    void ITable.DeleteAllOnSubmit(IEnumerable entities)
    {
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      List<object> list = entities.Cast<object>().ToList<object>();
      ITable table = (ITable) this;
      foreach (object entity in list)
        table.DeleteOnSubmit(entity);
    }

    public void Attach(TEntity entity)
    {
      if ((object) entity == null)
        throw Error.ArgumentNull(nameof (entity));
      this.Attach(entity, false);
    }

    void ITable.Attach(object entity)
    {
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      if (!(entity is TEntity entity1))
        throw Error.EntityIsTheWrongType();
      this.Attach(entity1, false);
    }

    public void Attach(TEntity entity, bool asModified)
    {
      if ((object) entity == null)
        throw Error.ArgumentNull(nameof (entity));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      MetaType inheritanceType = this.metaTable.RowType.GetInheritanceType(entity.GetType());
      if (!Table<TEntity>.IsTrackableType(inheritanceType))
        throw Error.TypeCouldNotBeTracked((object) inheritanceType.Type);
      if (asModified && inheritanceType.VersionMember == null && inheritanceType.HasUpdateCheck)
        throw Error.CannotAttachAsModifiedWithoutOriginalState();
      TrackedObject trackedObject = this.Context.Services.ChangeTracker.GetTrackedObject((object) entity);
      if (trackedObject != null && !trackedObject.IsWeaklyTracked)
        throw Error.CannotAttachAlreadyExistingEntity();
      if (trackedObject == null)
        trackedObject = this.context.Services.ChangeTracker.Track((object) entity, true);
      if (asModified)
        trackedObject.ConvertToModified();
      else
        trackedObject.ConvertToUnmodified();
      if (this.Context.Services.InsertLookupCachedObject(inheritanceType, (object) entity) != (object) entity)
        throw new DuplicateKeyException((object) entity, Strings.CantAddAlreadyExistingKey);
      trackedObject.InitializeDeferredLoaders();
    }

    void ITable.Attach(object entity, bool asModified)
    {
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      if (!(entity is TEntity entity1))
        throw Error.EntityIsTheWrongType();
      this.Attach(entity1, asModified);
    }

    public void Attach(TEntity entity, TEntity original)
    {
      if ((object) entity == null)
        throw Error.ArgumentNull(nameof (entity));
      if ((object) original == null)
        throw Error.ArgumentNull(nameof (original));
      if (entity.GetType() != original.GetType())
        throw Error.OriginalEntityIsWrongType();
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      MetaType inheritanceType = this.metaTable.RowType.GetInheritanceType(entity.GetType());
      if (!Table<TEntity>.IsTrackableType(inheritanceType))
        throw Error.TypeCouldNotBeTracked((object) inheritanceType.Type);
      TrackedObject trackedObject = this.context.Services.ChangeTracker.GetTrackedObject((object) entity);
      if (trackedObject != null && !trackedObject.IsWeaklyTracked)
        throw Error.CannotAttachAlreadyExistingEntity();
      if (trackedObject == null)
        trackedObject = this.context.Services.ChangeTracker.Track((object) entity, true);
      trackedObject.ConvertToPossiblyModified((object) original);
      if (this.Context.Services.InsertLookupCachedObject(inheritanceType, (object) entity) != (object) entity)
        throw new DuplicateKeyException((object) entity, Strings.CantAddAlreadyExistingKey);
      trackedObject.InitializeDeferredLoaders();
    }

    void ITable.Attach(object entity, object original)
    {
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      if (original == null)
        throw Error.ArgumentNull(nameof (original));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      if (!(entity is TEntity entity1))
        throw Error.EntityIsTheWrongType();
      if (entity.GetType() != original.GetType())
        throw Error.OriginalEntityIsWrongType();
      this.Attach(entity1, (TEntity) original);
    }

    public void AttachAll<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity
    {
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      this.AttachAll<TSubEntity>(entities, false);
    }

    void ITable.AttachAll(IEnumerable entities)
    {
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      ((ITable) this).AttachAll(entities, false);
    }

    public void AttachAll<TSubEntity>(IEnumerable<TSubEntity> entities, bool asModified) where TSubEntity : TEntity
    {
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      foreach (TSubEntity subEntity in entities.ToList<TSubEntity>())
        this.Attach((TEntity) subEntity, asModified);
    }

    void ITable.AttachAll(IEnumerable entities, bool asModified)
    {
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      this.CheckReadOnly();
      this.context.CheckNotInSubmitChanges();
      this.context.VerifyTrackingEnabled();
      List<object> list = entities.Cast<object>().ToList<object>();
      ITable table = (ITable) this;
      foreach (object entity in list)
        table.Attach(entity, asModified);
    }

    public TEntity GetOriginalEntityState(TEntity entity)
    {
      if ((object) entity == null)
        throw Error.ArgumentNull(nameof (entity));
      MetaType metaType = this.Context.Mapping.GetMetaType(entity.GetType());
      if (metaType == null || !metaType.IsEntity)
        throw Error.EntityIsTheWrongType();
      TrackedObject trackedObject = this.Context.Services.ChangeTracker.GetTrackedObject((object) entity);
      if (trackedObject == null)
        return default (TEntity);
      return trackedObject.Original != null ? (TEntity) trackedObject.CreateDataCopy(trackedObject.Original) : (TEntity) trackedObject.CreateDataCopy(trackedObject.Current);
    }

    object ITable.GetOriginalEntityState(object entity)
    {
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      return entity is TEntity entity1 ? (object) this.GetOriginalEntityState(entity1) : throw Error.EntityIsTheWrongType();
    }

    public ModifiedMemberInfo[] GetModifiedMembers(TEntity entity)
    {
      if ((object) entity == null)
        throw Error.ArgumentNull(nameof (entity));
      MetaType metaType = this.Context.Mapping.GetMetaType(entity.GetType());
      if (metaType == null || !metaType.IsEntity)
        throw Error.EntityIsTheWrongType();
      TrackedObject trackedObject = this.Context.Services.ChangeTracker.GetTrackedObject((object) entity);
      return trackedObject != null ? trackedObject.GetModifiedMembers().ToArray<ModifiedMemberInfo>() : new ModifiedMemberInfo[0];
    }

    ModifiedMemberInfo[] ITable.GetModifiedMembers(object entity)
    {
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      return entity is TEntity entity1 ? this.GetModifiedMembers(entity1) : throw Error.EntityIsTheWrongType();
    }

    private void CheckReadOnly()
    {
      if (this.IsReadOnly)
        throw Error.CannotPerformCUDOnReadOnlyTable((object) this.ToString());
    }

    public override string ToString() => "Table(" + typeof (TEntity).Name + ")";
  }
}
