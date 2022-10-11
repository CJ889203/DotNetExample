// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.ITable
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections;
using System.Linq;

namespace System.Data.Linq
{
  public interface ITable : IQueryable, IEnumerable
  {
    DataContext Context { get; }

    void InsertOnSubmit(object entity);

    void InsertAllOnSubmit(IEnumerable entities);

    void Attach(object entity);

    void Attach(object entity, bool asModified);

    void Attach(object entity, object original);

    void AttachAll(IEnumerable entities);

    void AttachAll(IEnumerable entities, bool asModified);

    void DeleteOnSubmit(object entity);

    void DeleteAllOnSubmit(IEnumerable entities);

    object GetOriginalEntityState(object entity);

    ModifiedMemberInfo[] GetModifiedMembers(object entity);

    bool IsReadOnly { get; }
  }
}
