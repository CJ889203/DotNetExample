// Decompiled with JetBrains decompiler
// Type: System.Linq.ParallelQuery`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq.Parallel;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class ParallelQuery<TSource> : ParallelQuery, IEnumerable<TSource>, IEnumerable
  {
    internal ParallelQuery(QuerySettings settings)
      : base(settings)
    {
    }

    internal override sealed ParallelQuery<TCastTo> Cast<TCastTo>() => this.Select<TSource, TCastTo>((Func<TSource, TCastTo>) (elem => (TCastTo) (object) elem));

    internal override sealed ParallelQuery<TCastTo> OfType<TCastTo>() => this.Where<TSource>((Func<TSource, bool>) (elem => (object) elem is TCastTo)).Select<TSource, TCastTo>((Func<TSource, TCastTo>) (elem => (TCastTo) (object) elem));

    internal override IEnumerator GetEnumeratorUntyped() => (IEnumerator) this.GetEnumerator();

    [__DynamicallyInvokable]
    public virtual IEnumerator<TSource> GetEnumerator() => throw new NotSupportedException();
  }
}
