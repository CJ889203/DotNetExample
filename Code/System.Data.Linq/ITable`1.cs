// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.ITable`1
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Data.Linq
{
  public interface ITable<TEntity> : 
    IQueryable<TEntity>,
    IEnumerable<TEntity>,
    IEnumerable,
    IQueryable
    where TEntity : class
  {
    void InsertOnSubmit(TEntity entity);

    void Attach(TEntity entity);

    void DeleteOnSubmit(TEntity entity);
  }
}
