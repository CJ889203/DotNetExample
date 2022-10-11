// Decompiled with JetBrains decompiler
// Type: System.Linq.ParallelQuery
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections;
using System.Linq.Parallel;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class ParallelQuery : IEnumerable
  {
    private QuerySettings m_specifiedSettings;

    internal ParallelQuery(QuerySettings specifiedSettings) => this.m_specifiedSettings = specifiedSettings;

    internal QuerySettings SpecifiedQuerySettings => this.m_specifiedSettings;

    internal virtual ParallelQuery<TCastTo> Cast<TCastTo>() => throw new NotSupportedException();

    internal virtual ParallelQuery<TCastTo> OfType<TCastTo>() => throw new NotSupportedException();

    internal virtual IEnumerator GetEnumeratorUntyped() => throw new NotSupportedException();

    [__DynamicallyInvokable]
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumeratorUntyped();
  }
}
