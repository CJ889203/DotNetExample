// Decompiled with JetBrains decompiler
// Type: System.Linq.EnumerableQuery`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class EnumerableQuery<T> : 
    EnumerableQuery,
    IOrderedQueryable<T>,
    IQueryable<T>,
    IEnumerable<T>,
    IEnumerable,
    IQueryable,
    IOrderedQueryable,
    IQueryProvider
  {
    private Expression expression;
    private IEnumerable<T> enumerable;

    [__DynamicallyInvokable]
    IQueryProvider IQueryable.Provider
    {
      [__DynamicallyInvokable] get => (IQueryProvider) this;
    }

    [__DynamicallyInvokable]
    public EnumerableQuery(IEnumerable<T> enumerable)
    {
      this.enumerable = enumerable;
      this.expression = (Expression) Expression.Constant((object) this);
    }

    [__DynamicallyInvokable]
    public EnumerableQuery(Expression expression) => this.expression = expression;

    internal override Expression Expression => this.expression;

    internal override IEnumerable Enumerable => (IEnumerable) this.enumerable;

    [__DynamicallyInvokable]
    Expression IQueryable.Expression
    {
      [__DynamicallyInvokable] get => this.expression;
    }

    [__DynamicallyInvokable]
    Type IQueryable.ElementType
    {
      [__DynamicallyInvokable] get => typeof (T);
    }

    [__DynamicallyInvokable]
    IQueryable IQueryProvider.CreateQuery(Expression expression)
    {
      Type type = expression != null ? TypeHelper.FindGenericType(typeof (IQueryable<>), expression.Type) : throw Error.ArgumentNull(nameof (expression));
      return !(type == (Type) null) ? EnumerableQuery.Create(type.GetGenericArguments()[0], expression) : throw Error.ArgumentNotValid((object) nameof (expression));
    }

    [__DynamicallyInvokable]
    IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull(nameof (expression));
      return typeof (IQueryable<S>).IsAssignableFrom(expression.Type) ? (IQueryable<S>) new EnumerableQuery<S>(expression) : throw Error.ArgumentNotValid((object) nameof (expression));
    }

    [__DynamicallyInvokable]
    object IQueryProvider.Execute(Expression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull(nameof (expression));
      typeof (EnumerableExecutor<>).MakeGenericType(expression.Type);
      return EnumerableExecutor.Create(expression).ExecuteBoxed();
    }

    [__DynamicallyInvokable]
    S IQueryProvider.Execute<S>(Expression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull(nameof (expression));
      return typeof (S).IsAssignableFrom(expression.Type) ? new EnumerableExecutor<S>(expression).Execute() : throw Error.ArgumentNotValid((object) nameof (expression));
    }

    [__DynamicallyInvokable]
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    [__DynamicallyInvokable]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

    private IEnumerator<T> GetEnumerator()
    {
      if (this.enumerable == null)
        this.enumerable = Expression.Lambda<Func<IEnumerable<T>>>(new EnumerableRewriter().Visit(this.expression), (IEnumerable<ParameterExpression>) null).Compile()();
      return this.enumerable.GetEnumerator();
    }

    [__DynamicallyInvokable]
    public override string ToString()
    {
      if (!(this.expression is ConstantExpression expression) || expression.Value != this)
        return this.expression.ToString();
      return this.enumerable != null ? this.enumerable.ToString() : "null";
    }
  }
}
