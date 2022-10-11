// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.DataContext
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq
{
  public class DataContext : IDisposable
  {
    private CommonDataServices services;
    private IProvider provider;
    private Dictionary<MetaTable, ITable> tables;
    private bool objectTrackingEnabled = true;
    private bool deferredLoadingEnabled = true;
    private bool disposed;
    private bool isInSubmitChanges;
    private DataLoadOptions loadOptions;
    private ChangeConflictCollection conflicts;
    private static MethodInfo _miExecuteQuery;

    private DataContext()
    {
    }

    public DataContext(string fileOrServerOrConnection)
    {
      if (fileOrServerOrConnection == null)
        throw Error.ArgumentNull(nameof (fileOrServerOrConnection));
      this.InitWithDefaultMapping((object) fileOrServerOrConnection);
    }

    public DataContext(string fileOrServerOrConnection, MappingSource mapping)
    {
      if (fileOrServerOrConnection == null)
        throw Error.ArgumentNull(nameof (fileOrServerOrConnection));
      if (mapping == null)
        throw Error.ArgumentNull(nameof (mapping));
      this.Init((object) fileOrServerOrConnection, mapping);
    }

    public DataContext(IDbConnection connection)
    {
      if (connection == null)
        throw Error.ArgumentNull(nameof (connection));
      this.InitWithDefaultMapping((object) connection);
    }

    public DataContext(IDbConnection connection, MappingSource mapping)
    {
      if (connection == null)
        throw Error.ArgumentNull(nameof (connection));
      if (mapping == null)
        throw Error.ArgumentNull(nameof (mapping));
      this.Init((object) connection, mapping);
    }

    internal DataContext(DataContext context)
    {
      if (context == null)
        throw Error.ArgumentNull(nameof (context));
      this.Init((object) context.Connection, context.Mapping.MappingSource);
      this.LoadOptions = context.LoadOptions;
      this.Transaction = context.Transaction;
      this.Log = context.Log;
      this.CommandTimeout = context.CommandTimeout;
    }

    public void Dispose()
    {
      this.disposed = true;
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      if (this.provider != null)
      {
        this.provider.Dispose();
        this.provider = (IProvider) null;
      }
      this.services = (CommonDataServices) null;
      this.tables = (Dictionary<MetaTable, ITable>) null;
      this.loadOptions = (DataLoadOptions) null;
    }

    internal void CheckDispose()
    {
      if (this.disposed)
        throw Error.DataContextCannotBeUsedAfterDispose();
    }

    private void InitWithDefaultMapping(object connection) => this.Init(connection, (MappingSource) new AttributeMappingSource());

    internal object Clone()
    {
      this.CheckDispose();
      return Activator.CreateInstance(this.GetType(), (object) this.Connection, (object) this.Mapping.MappingSource);
    }

    private void Init(object connection, MappingSource mapping)
    {
      MetaModel model = mapping.GetModel(this.GetType());
      this.services = new CommonDataServices(this, model);
      this.conflicts = new ChangeConflictCollection();
      Type type = model.ProviderType != (Type) null ? model.ProviderType : throw Error.ProviderTypeNull();
      this.provider = typeof (IProvider).IsAssignableFrom(type) ? (IProvider) Activator.CreateInstance(type) : throw Error.ProviderDoesNotImplementRequiredInterface((object) type, (object) typeof (IProvider));
      this.provider.Initialize((IDataServices) this.services, connection);
      this.tables = new Dictionary<MetaTable, ITable>();
      this.InitTables((object) this);
    }

    internal void ClearCache()
    {
      this.CheckDispose();
      this.services.ResetServices();
    }

    internal CommonDataServices Services
    {
      get
      {
        this.CheckDispose();
        return this.services;
      }
    }

    public DbConnection Connection
    {
      get
      {
        this.CheckDispose();
        return this.provider.Connection;
      }
    }

    public DbTransaction Transaction
    {
      get
      {
        this.CheckDispose();
        return this.provider.Transaction;
      }
      set
      {
        this.CheckDispose();
        this.provider.Transaction = value;
      }
    }

    public int CommandTimeout
    {
      get
      {
        this.CheckDispose();
        return this.provider.CommandTimeout;
      }
      set
      {
        this.CheckDispose();
        this.provider.CommandTimeout = value;
      }
    }

    public TextWriter Log
    {
      get
      {
        this.CheckDispose();
        return this.provider.Log;
      }
      set
      {
        this.CheckDispose();
        this.provider.Log = value;
      }
    }

    public bool ObjectTrackingEnabled
    {
      get
      {
        this.CheckDispose();
        return this.objectTrackingEnabled;
      }
      set
      {
        this.CheckDispose();
        if (this.Services.HasCachedObjects)
          throw Error.OptionsCannotBeModifiedAfterQuery();
        this.objectTrackingEnabled = value;
        if (!this.objectTrackingEnabled)
          this.deferredLoadingEnabled = false;
        this.services.ResetServices();
      }
    }

    public bool DeferredLoadingEnabled
    {
      get
      {
        this.CheckDispose();
        return this.deferredLoadingEnabled;
      }
      set
      {
        this.CheckDispose();
        if (this.Services.HasCachedObjects)
          throw Error.OptionsCannotBeModifiedAfterQuery();
        this.deferredLoadingEnabled = !(!this.ObjectTrackingEnabled & value) ? value : throw Error.DeferredLoadingRequiresObjectTracking();
      }
    }

    public MetaModel Mapping
    {
      get
      {
        this.CheckDispose();
        return this.services.Model;
      }
    }

    internal void VerifyTrackingEnabled()
    {
      this.CheckDispose();
      if (!this.ObjectTrackingEnabled)
        throw Error.ObjectTrackingRequired();
    }

    internal void CheckNotInSubmitChanges()
    {
      this.CheckDispose();
      if (this.isInSubmitChanges)
        throw Error.CannotPerformOperationDuringSubmitChanges();
    }

    internal void CheckInSubmitChanges()
    {
      this.CheckDispose();
      if (!this.isInSubmitChanges)
        throw Error.CannotPerformOperationOutsideSubmitChanges();
    }

    public Table<TEntity> GetTable<TEntity>() where TEntity : class
    {
      this.CheckDispose();
      MetaTable table1 = this.services.Model.GetTable(typeof (TEntity));
      ITable table2 = table1 != null ? this.GetTable(table1) : throw Error.TypeIsNotMarkedAsTable((object) typeof (TEntity));
      return !(table2.ElementType != typeof (TEntity)) ? (Table<TEntity>) table2 : throw Error.CouldNotGetTableForSubtype((object) typeof (TEntity), (object) table1.RowType.Type);
    }

    public ITable GetTable(Type type)
    {
      this.CheckDispose();
      if (type == (Type) null)
        throw Error.ArgumentNull(nameof (type));
      MetaTable table = this.services.Model.GetTable(type);
      if (table == null)
        throw Error.TypeIsNotMarkedAsTable((object) type);
      if (table.RowType.Type != type)
        throw Error.CouldNotGetTableForSubtype((object) type, (object) table.RowType.Type);
      return this.GetTable(table);
    }

    private ITable GetTable(MetaTable metaTable)
    {
      ITable instance;
      if (!this.tables.TryGetValue(metaTable, out instance))
      {
        DataContext.ValidateTable(metaTable);
        instance = (ITable) Activator.CreateInstance(typeof (Table<>).MakeGenericType(metaTable.RowType.Type), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new object[2]
        {
          (object) this,
          (object) metaTable
        }, (CultureInfo) null);
        this.tables.Add(metaTable, instance);
      }
      return instance;
    }

    private static void ValidateTable(MetaTable metaTable)
    {
      foreach (MetaAssociation association in metaTable.RowType.Associations)
      {
        if (!association.ThisMember.DeclaringType.IsEntity)
          throw Error.NonEntityAssociationMapping((object) association.ThisMember.DeclaringType.Type, (object) association.ThisMember.Name, (object) association.ThisMember.DeclaringType.Type);
        if (!association.OtherType.IsEntity)
          throw Error.NonEntityAssociationMapping((object) association.ThisMember.DeclaringType.Type, (object) association.ThisMember.Name, (object) association.OtherType.Type);
      }
    }

    private void InitTables(object schema)
    {
      foreach (FieldInfo field in schema.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
      {
        Type fieldType = field.FieldType;
        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof (Table<>) && (ITable) field.GetValue(schema) == null)
        {
          ITable table = this.GetTable(fieldType.GetGenericArguments()[0]);
          field.SetValue(schema, (object) table);
        }
      }
    }

    internal IProvider Provider
    {
      get
      {
        this.CheckDispose();
        return this.provider;
      }
    }

    public bool DatabaseExists()
    {
      this.CheckDispose();
      return this.provider.DatabaseExists();
    }

    public void CreateDatabase()
    {
      this.CheckDispose();
      this.provider.CreateDatabase();
    }

    public void DeleteDatabase()
    {
      this.CheckDispose();
      this.provider.DeleteDatabase();
    }

    public void SubmitChanges()
    {
      this.CheckDispose();
      this.SubmitChanges(ConflictMode.FailOnFirstConflict);
    }

    public virtual void SubmitChanges(ConflictMode failureMode)
    {
      this.CheckDispose();
      this.CheckNotInSubmitChanges();
      this.VerifyTrackingEnabled();
      this.conflicts.Clear();
      try
      {
        this.isInSubmitChanges = true;
        if (System.Transactions.Transaction.Current == (System.Transactions.Transaction) null && this.provider.Transaction == null)
        {
          bool flag = false;
          DbTransaction dbTransaction = (DbTransaction) null;
          try
          {
            if (this.provider.Connection.State == ConnectionState.Open)
              this.provider.ClearConnection();
            if (this.provider.Connection.State == ConnectionState.Closed)
            {
              this.provider.Connection.Open();
              flag = true;
            }
            dbTransaction = this.provider.Connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            this.provider.Transaction = dbTransaction;
            new ChangeProcessor(this.services, this).SubmitChanges(failureMode);
            this.AcceptChanges();
            this.provider.ClearConnection();
            dbTransaction.Commit();
          }
          catch
          {
            dbTransaction?.Rollback();
            throw;
          }
          finally
          {
            this.provider.Transaction = (DbTransaction) null;
            if (flag)
              this.provider.Connection.Close();
          }
        }
        else
        {
          new ChangeProcessor(this.services, this).SubmitChanges(failureMode);
          this.AcceptChanges();
        }
      }
      finally
      {
        this.isInSubmitChanges = false;
      }
    }

    public void Refresh(RefreshMode mode, object entity)
    {
      this.CheckDispose();
      this.CheckNotInSubmitChanges();
      this.VerifyTrackingEnabled();
      Array entities = entity != null ? Array.CreateInstance(entity.GetType(), 1) : throw Error.ArgumentNull(nameof (entity));
      entities.SetValue(entity, 0);
      this.Refresh(mode, (IEnumerable) entities);
    }

    public void Refresh(RefreshMode mode, params object[] entities)
    {
      this.CheckDispose();
      if (entities == null)
        throw Error.ArgumentNull(nameof (entities));
      this.Refresh(mode, (IEnumerable) entities);
    }

    public void Refresh(RefreshMode mode, IEnumerable entities)
    {
      this.CheckDispose();
      this.CheckNotInSubmitChanges();
      this.VerifyTrackingEnabled();
      List<object> objectList = entities != null ? entities.Cast<object>().ToList<object>() : throw Error.ArgumentNull(nameof (entities));
      DataContext refreshContext = this.CreateRefreshContext();
      foreach (object obj in objectList)
      {
        this.GetTable(this.services.Model.GetMetaType(obj.GetType()).InheritanceRoot.Type);
        TrackedObject trackedObject = this.services.ChangeTracker.GetTrackedObject(obj);
        if (trackedObject == null)
          throw Error.UnrecognizedRefreshObject();
        if (trackedObject.IsNew)
          throw Error.RefreshOfNewObject();
        object[] keyValues = CommonDataServices.GetKeyValues(trackedObject.Type, trackedObject.Original);
        trackedObject.Refresh(mode, refreshContext.Services.GetObjectByKey(trackedObject.Type, keyValues) ?? throw Error.RefreshOfDeletedObject());
      }
    }

    internal DataContext CreateRefreshContext()
    {
      this.CheckDispose();
      return new DataContext(this);
    }

    private void AcceptChanges()
    {
      this.CheckDispose();
      this.VerifyTrackingEnabled();
      this.services.ChangeTracker.AcceptChanges();
    }

    internal string GetQueryText(IQueryable query)
    {
      this.CheckDispose();
      if (query == null)
        throw Error.ArgumentNull(nameof (query));
      return this.provider.GetQueryText(query.Expression);
    }

    public DbCommand GetCommand(IQueryable query)
    {
      this.CheckDispose();
      if (query == null)
        throw Error.ArgumentNull(nameof (query));
      return this.provider.GetCommand(query.Expression);
    }

    internal string GetChangeText()
    {
      this.CheckDispose();
      this.VerifyTrackingEnabled();
      return new ChangeProcessor(this.services, this).GetChangeText();
    }

    public ChangeSet GetChangeSet()
    {
      this.CheckDispose();
      return new ChangeProcessor(this.services, this).GetChangeSet();
    }

    public int ExecuteCommand(string command, params object[] parameters)
    {
      this.CheckDispose();
      if (command == null)
        throw Error.ArgumentNull(nameof (command));
      if (parameters == null)
        throw Error.ArgumentNull(nameof (parameters));
      return (int) this.ExecuteMethodCall((object) this, (MethodInfo) MethodBase.GetCurrentMethod(), (object) command, (object) parameters).ReturnValue;
    }

    public IEnumerable<TResult> ExecuteQuery<TResult>(
      string query,
      params object[] parameters)
    {
      this.CheckDispose();
      if (query == null)
        throw Error.ArgumentNull(nameof (query));
      if (parameters == null)
        throw Error.ArgumentNull(nameof (parameters));
      return (IEnumerable<TResult>) this.ExecuteMethodCall((object) this, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TResult)), (object) query, (object) parameters).ReturnValue;
    }

    public IEnumerable ExecuteQuery(
      Type elementType,
      string query,
      params object[] parameters)
    {
      this.CheckDispose();
      if (elementType == (Type) null)
        throw Error.ArgumentNull(nameof (elementType));
      if (query == null)
        throw Error.ArgumentNull(nameof (query));
      if (parameters == null)
        throw Error.ArgumentNull(nameof (parameters));
      if (DataContext._miExecuteQuery == (MethodInfo) null)
        DataContext._miExecuteQuery = ((IEnumerable<MethodInfo>) typeof (DataContext).GetMethods()).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == nameof (ExecuteQuery) && m.GetParameters().Length == 2));
      return (IEnumerable) this.ExecuteMethodCall((object) this, DataContext._miExecuteQuery.MakeGenericMethod(elementType), (object) query, (object) parameters).ReturnValue;
    }

    protected internal IExecuteResult ExecuteMethodCall(
      object instance,
      MethodInfo methodInfo,
      params object[] parameters)
    {
      this.CheckDispose();
      if (instance == null)
        throw Error.ArgumentNull(nameof (instance));
      if (methodInfo == (MethodInfo) null)
        throw Error.ArgumentNull(nameof (methodInfo));
      if (parameters == null)
        throw Error.ArgumentNull(nameof (parameters));
      return this.provider.Execute(this.GetMethodCall(instance, methodInfo, parameters));
    }

    protected internal IQueryable<TResult> CreateMethodCallQuery<TResult>(
      object instance,
      MethodInfo methodInfo,
      params object[] parameters)
    {
      this.CheckDispose();
      if (instance == null)
        throw Error.ArgumentNull(nameof (instance));
      if (methodInfo == (MethodInfo) null)
        throw Error.ArgumentNull(nameof (methodInfo));
      if (parameters == null)
        throw Error.ArgumentNull(nameof (parameters));
      if (!typeof (IQueryable<TResult>).IsAssignableFrom(methodInfo.ReturnType))
        throw Error.ExpectedQueryableArgument((object) nameof (methodInfo), (object) typeof (IQueryable<TResult>));
      return (IQueryable<TResult>) new DataQuery<TResult>(this, this.GetMethodCall(instance, methodInfo, parameters));
    }

    private Expression GetMethodCall(
      object instance,
      MethodInfo methodInfo,
      params object[] parameters)
    {
      this.CheckDispose();
      if (parameters.Length == 0)
        return (Expression) Expression.Call((Expression) Expression.Constant(instance), methodInfo);
      ParameterInfo[] parameters1 = methodInfo.GetParameters();
      List<Expression> arguments = new List<Expression>(parameters.Length);
      int index = 0;
      for (int length = parameters.Length; index < length; ++index)
      {
        Type type = parameters1[index].ParameterType;
        if (type.IsByRef)
          type = type.GetElementType();
        arguments.Add((Expression) Expression.Constant(parameters[index], type));
      }
      return (Expression) Expression.Call((Expression) Expression.Constant(instance), methodInfo, (IEnumerable<Expression>) arguments);
    }

    protected internal void ExecuteDynamicInsert(object entity)
    {
      this.CheckDispose();
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      this.CheckInSubmitChanges();
      this.services.ChangeDirector.DynamicInsert(this.services.ChangeTracker.GetTrackedObject(entity) ?? throw Error.CannotPerformOperationForUntrackedObject());
    }

    protected internal void ExecuteDynamicUpdate(object entity)
    {
      this.CheckDispose();
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      this.CheckInSubmitChanges();
      TrackedObject trackedObject = this.services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null)
        throw Error.CannotPerformOperationForUntrackedObject();
      if (this.services.ChangeDirector.DynamicUpdate(trackedObject) == 0)
        throw new ChangeConflictException();
    }

    protected internal void ExecuteDynamicDelete(object entity)
    {
      this.CheckDispose();
      if (entity == null)
        throw Error.ArgumentNull(nameof (entity));
      this.CheckInSubmitChanges();
      TrackedObject trackedObject = this.services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null)
        throw Error.CannotPerformOperationForUntrackedObject();
      if (this.services.ChangeDirector.DynamicDelete(trackedObject) == 0)
        throw new ChangeConflictException();
    }

    public IEnumerable<TResult> Translate<TResult>(DbDataReader reader)
    {
      this.CheckDispose();
      return (IEnumerable<TResult>) this.Translate(typeof (TResult), reader);
    }

    public IEnumerable Translate(Type elementType, DbDataReader reader)
    {
      this.CheckDispose();
      if (elementType == (Type) null)
        throw Error.ArgumentNull(nameof (elementType));
      if (reader == null)
        throw Error.ArgumentNull(nameof (reader));
      return this.provider.Translate(elementType, reader);
    }

    public IMultipleResults Translate(DbDataReader reader)
    {
      this.CheckDispose();
      return reader != null ? this.provider.Translate(reader) : throw Error.ArgumentNull(nameof (reader));
    }

    internal void ResetLoadOptions()
    {
      this.CheckDispose();
      this.loadOptions = (DataLoadOptions) null;
    }

    public DataLoadOptions LoadOptions
    {
      get
      {
        this.CheckDispose();
        return this.loadOptions;
      }
      set
      {
        this.CheckDispose();
        if (this.services.HasCachedObjects && value != this.loadOptions)
          throw Error.LoadOptionsChangeNotAllowedAfterQuery();
        value?.Freeze();
        this.loadOptions = value;
      }
    }

    public ChangeConflictCollection ChangeConflicts
    {
      get
      {
        this.CheckDispose();
        return this.conflicts;
      }
    }
  }
}
