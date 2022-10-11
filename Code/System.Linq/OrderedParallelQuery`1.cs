// Decompiled with JetBrains decompiler
// Type: System.Linq.OrderedParallelQuery`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections.Generic;
using System.Linq.Parallel;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class OrderedParallelQuery<TSource> : ParallelQuery<TSource>
  {
    private QueryOperator<TSource> m_sortOp;

    internal OrderedParallelQuery(QueryOperator<TSource> sortOp)
      : base(sortOp.SpecifiedQuerySettings)
    {
      this.m_sortOp = sortOp;
    }

    internal QueryOperator<TSource> SortOperator => this.m_sortOp;

    internal IOrderedEnumerable<TSource> OrderedEnumerable => (IOrderedEnumerable<TSource>) this.m_sortOp;

    [__DynamicallyInvokable]
    public override IEnumerator<TSource> GetEnumerator() => this.m_sortOp.GetEnumerator();
  }
}
