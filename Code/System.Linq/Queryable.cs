// Decompiled with JetBrains decompiler
// Type: System.Linq.Queryable
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public static class Queryable
  {
    private static MethodInfo GetMethodInfo<T1, T2>(Func<T1, T2> f, T1 unused1) => f.Method;

    private static MethodInfo GetMethodInfo<T1, T2, T3>(
      Func<T1, T2, T3> f,
      T1 unused1,
      T2 unused2)
    {
      return f.Method;
    }

    private static MethodInfo GetMethodInfo<T1, T2, T3, T4>(
      Func<T1, T2, T3, T4> f,
      T1 unused1,
      T2 unused2,
      T3 unused3)
    {
      return f.Method;
    }

    private static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5>(
      Func<T1, T2, T3, T4, T5> f,
      T1 unused1,
      T2 unused2,
      T3 unused3,
      T4 unused4)
    {
      return f.Method;
    }

    private static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6>(
      Func<T1, T2, T3, T4, T5, T6> f,
      T1 unused1,
      T2 unused2,
      T3 unused3,
      T4 unused4,
      T5 unused5)
    {
      return f.Method;
    }

    private static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(
      Func<T1, T2, T3, T4, T5, T6, T7> f,
      T1 unused1,
      T2 unused2,
      T3 unused3,
      T4 unused4,
      T5 unused5,
      T6 unused6)
    {
      return f.Method;
    }

    [__DynamicallyInvokable]
    public static IQueryable<TElement> AsQueryable<TElement>(
      this IEnumerable<TElement> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source is IQueryable<TElement> ? (IQueryable<TElement>) source : (IQueryable<TElement>) new EnumerableQuery<TElement>(source);
    }

    [__DynamicallyInvokable]
    public static IQueryable AsQueryable(this IEnumerable source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (source is IQueryable)
        return (IQueryable) source;
      Type genericType = TypeHelper.FindGenericType(typeof (IEnumerable<>), source.GetType());
      return !(genericType == (Type) null) ? EnumerableQuery.Create(genericType.GetGenericArguments()[0], source) : throw Error.ArgumentNotIEnumerableGeneric((object) nameof (source));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Where<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(Queryable.Where<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Where<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(Queryable.Where<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> OfType<TResult>(this IQueryable source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable, IQueryable<TResult>>(new Func<IQueryable, IQueryable<TResult>>(Queryable.OfType<TResult>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Cast<TResult>(this IQueryable source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable, IQueryable<TResult>>(new Func<IQueryable, IQueryable<TResult>>(Queryable.Cast<TResult>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Select<TSource, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TResult>>, IQueryable<TResult>>(Queryable.Select<TSource, TResult>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Select<TSource, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, int, TResult>>, IQueryable<TResult>>(Queryable.Select<TSource, TResult>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> SelectMany<TSource, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, IEnumerable<TResult>>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, IEnumerable<TResult>>>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, IEnumerable<TResult>>>, IQueryable<TResult>>(Queryable.SelectMany<TSource, TResult>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> SelectMany<TSource, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int, IEnumerable<TResult>>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int, IEnumerable<TResult>>>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, int, IEnumerable<TResult>>>, IQueryable<TResult>>(Queryable.SelectMany<TSource, TResult>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> SelectMany<TSource, TCollection, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector,
      Expression<Func<TSource, TCollection, TResult>> resultSelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (collectionSelector == null)
        throw Error.ArgumentNull(nameof (collectionSelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int, IEnumerable<TCollection>>>, Expression<Func<TSource, TCollection, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, int, IEnumerable<TCollection>>>, Expression<Func<TSource, TCollection, TResult>>, IQueryable<TResult>>(Queryable.SelectMany<TSource, TCollection, TResult>), source, collectionSelector, resultSelector), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) collectionSelector),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> SelectMany<TSource, TCollection, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector,
      Expression<Func<TSource, TCollection, TResult>> resultSelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (collectionSelector == null)
        throw Error.ArgumentNull(nameof (collectionSelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, IEnumerable<TCollection>>>, Expression<Func<TSource, TCollection, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, IEnumerable<TCollection>>>, Expression<Func<TSource, TCollection, TResult>>, IQueryable<TResult>>(Queryable.SelectMany<TSource, TCollection, TResult>), source, collectionSelector, resultSelector), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) collectionSelector),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    private static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source) => source is IQueryable<TSource> queryable ? queryable.Expression : (Expression) Expression.Constant((object) source, typeof (IEnumerable<TSource>));

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(
      this IQueryable<TOuter> outer,
      IEnumerable<TInner> inner,
      Expression<Func<TOuter, TKey>> outerKeySelector,
      Expression<Func<TInner, TKey>> innerKeySelector,
      Expression<Func<TOuter, TInner, TResult>> resultSelector)
    {
      if (outer == null)
        throw Error.ArgumentNull(nameof (outer));
      if (inner == null)
        throw Error.ArgumentNull(nameof (inner));
      if (outerKeySelector == null)
        throw Error.ArgumentNull(nameof (outerKeySelector));
      if (innerKeySelector == null)
        throw Error.ArgumentNull(nameof (innerKeySelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return outer.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, TInner, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, TInner, TResult>>, IQueryable<TResult>>(Queryable.Join<TOuter, TInner, TKey, TResult>), outer, inner, outerKeySelector, innerKeySelector, resultSelector), outer.Expression, Queryable.GetSourceExpression<TInner>(inner), (Expression) Expression.Quote((Expression) outerKeySelector), (Expression) Expression.Quote((Expression) innerKeySelector), (Expression) Expression.Quote((Expression) resultSelector)));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(
      this IQueryable<TOuter> outer,
      IEnumerable<TInner> inner,
      Expression<Func<TOuter, TKey>> outerKeySelector,
      Expression<Func<TInner, TKey>> innerKeySelector,
      Expression<Func<TOuter, TInner, TResult>> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (outer == null)
        throw Error.ArgumentNull(nameof (outer));
      if (inner == null)
        throw Error.ArgumentNull(nameof (inner));
      if (outerKeySelector == null)
        throw Error.ArgumentNull(nameof (outerKeySelector));
      if (innerKeySelector == null)
        throw Error.ArgumentNull(nameof (innerKeySelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return outer.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, TInner, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(new Func<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, TInner, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(Queryable.Join<TOuter, TInner, TKey, TResult>), outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer), outer.Expression, Queryable.GetSourceExpression<TInner>(inner), (Expression) Expression.Quote((Expression) outerKeySelector), (Expression) Expression.Quote((Expression) innerKeySelector), (Expression) Expression.Quote((Expression) resultSelector), (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
      this IQueryable<TOuter> outer,
      IEnumerable<TInner> inner,
      Expression<Func<TOuter, TKey>> outerKeySelector,
      Expression<Func<TInner, TKey>> innerKeySelector,
      Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
    {
      if (outer == null)
        throw Error.ArgumentNull(nameof (outer));
      if (inner == null)
        throw Error.ArgumentNull(nameof (inner));
      if (outerKeySelector == null)
        throw Error.ArgumentNull(nameof (outerKeySelector));
      if (innerKeySelector == null)
        throw Error.ArgumentNull(nameof (innerKeySelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return outer.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, IEnumerable<TInner>, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, IEnumerable<TInner>, TResult>>, IQueryable<TResult>>(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>), outer, inner, outerKeySelector, innerKeySelector, resultSelector), outer.Expression, Queryable.GetSourceExpression<TInner>(inner), (Expression) Expression.Quote((Expression) outerKeySelector), (Expression) Expression.Quote((Expression) innerKeySelector), (Expression) Expression.Quote((Expression) resultSelector)));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
      this IQueryable<TOuter> outer,
      IEnumerable<TInner> inner,
      Expression<Func<TOuter, TKey>> outerKeySelector,
      Expression<Func<TInner, TKey>> innerKeySelector,
      Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (outer == null)
        throw Error.ArgumentNull(nameof (outer));
      if (inner == null)
        throw Error.ArgumentNull(nameof (inner));
      if (outerKeySelector == null)
        throw Error.ArgumentNull(nameof (outerKeySelector));
      if (innerKeySelector == null)
        throw Error.ArgumentNull(nameof (innerKeySelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return outer.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, IEnumerable<TInner>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(new Func<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, IEnumerable<TInner>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>), outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer), outer.Expression, Queryable.GetSourceExpression<TInner>(inner), (Expression) Expression.Quote((Expression) outerKeySelector), (Expression) Expression.Quote((Expression) innerKeySelector), (Expression) Expression.Quote((Expression) resultSelector), (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(Queryable.OrderBy<TSource, TKey>), source, keySelector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(Queryable.OrderBy<TSource, TKey>), source, keySelector, comparer), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(Queryable.OrderByDescending<TSource, TKey>), source, keySelector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(Queryable.OrderByDescending<TSource, TKey>), source, keySelector, comparer), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(
      this IOrderedQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(new Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(Queryable.ThenBy<TSource, TKey>), source, keySelector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(
      this IOrderedQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(new Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(Queryable.ThenBy<TSource, TKey>), source, keySelector, comparer), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(
      this IOrderedQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(new Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(Queryable.ThenByDescending<TSource, TKey>), source, keySelector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(
      this IOrderedQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(new Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(Queryable.ThenByDescending<TSource, TKey>), source, keySelector, comparer), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Take<TSource>(
      this IQueryable<TSource> source,
      int count)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, int, IQueryable<TSource>>(new Func<IQueryable<TSource>, int, IQueryable<TSource>>(Queryable.Take<TSource>), source, count), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) count)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> TakeWhile<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(Queryable.TakeWhile<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> TakeWhile<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(Queryable.TakeWhile<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Skip<TSource>(
      this IQueryable<TSource> source,
      int count)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, int, IQueryable<TSource>>(new Func<IQueryable<TSource>, int, IQueryable<TSource>>(Queryable.Skip<TSource>), source, count), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) count)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> SkipWhile<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(Queryable.SkipWhile<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> SkipWhile<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(new Func<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(Queryable.SkipWhile<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return source.Provider.CreateQuery<IGrouping<TKey, TSource>>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IQueryable<IGrouping<TKey, TSource>>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IQueryable<IGrouping<TKey, TSource>>>(Queryable.GroupBy<TSource, TKey>), source, keySelector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      Expression<Func<TSource, TElement>> elementSelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      if (elementSelector == null)
        throw Error.ArgumentNull(nameof (elementSelector));
      return source.Provider.CreateQuery<IGrouping<TKey, TElement>>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, IQueryable<IGrouping<TKey, TElement>>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, IQueryable<IGrouping<TKey, TElement>>>(Queryable.GroupBy<TSource, TKey, TElement>), source, keySelector, elementSelector), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Quote((Expression) elementSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      return source.Provider.CreateQuery<IGrouping<TKey, TSource>>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IQueryable<IGrouping<TKey, TSource>>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IQueryable<IGrouping<TKey, TSource>>>(Queryable.GroupBy<TSource, TKey>), source, keySelector, comparer), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      Expression<Func<TSource, TElement>> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      if (elementSelector == null)
        throw Error.ArgumentNull(nameof (elementSelector));
      return source.Provider.CreateQuery<IGrouping<TKey, TElement>>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, IEqualityComparer<TKey>, IQueryable<IGrouping<TKey, TElement>>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, IEqualityComparer<TKey>, IQueryable<IGrouping<TKey, TElement>>>(Queryable.GroupBy<TSource, TKey, TElement>), source, keySelector, elementSelector, comparer), source.Expression, (Expression) Expression.Quote((Expression) keySelector), (Expression) Expression.Quote((Expression) elementSelector), (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      Expression<Func<TSource, TElement>> elementSelector,
      Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      if (elementSelector == null)
        throw Error.ArgumentNull(nameof (elementSelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, Expression<Func<TKey, IEnumerable<TElement>, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, Expression<Func<TKey, IEnumerable<TElement>, TResult>>, IQueryable<TResult>>(Queryable.GroupBy<TSource, TKey, TElement, TResult>), source, keySelector, elementSelector, resultSelector), source.Expression, (Expression) Expression.Quote((Expression) keySelector), (Expression) Expression.Quote((Expression) elementSelector), (Expression) Expression.Quote((Expression) resultSelector)));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupBy<TSource, TKey, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TKey, IEnumerable<TSource>, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TKey, IEnumerable<TSource>, TResult>>, IQueryable<TResult>>(Queryable.GroupBy<TSource, TKey, TResult>), source, keySelector, resultSelector), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupBy<TSource, TKey, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TKey, IEnumerable<TSource>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TKey, IEnumerable<TSource>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(Queryable.GroupBy<TSource, TKey, TResult>), source, keySelector, resultSelector, comparer), source.Expression, (Expression) Expression.Quote((Expression) keySelector), (Expression) Expression.Quote((Expression) resultSelector), (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TKey>> keySelector,
      Expression<Func<TSource, TElement>> elementSelector,
      Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (keySelector == null)
        throw Error.ArgumentNull(nameof (keySelector));
      if (elementSelector == null)
        throw Error.ArgumentNull(nameof (elementSelector));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, Expression<Func<TKey, IEnumerable<TElement>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, Expression<Func<TKey, IEnumerable<TElement>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(Queryable.GroupBy<TSource, TKey, TElement, TResult>), source, keySelector, elementSelector, resultSelector, comparer), source.Expression, (Expression) Expression.Quote((Expression) keySelector), (Expression) Expression.Quote((Expression) elementSelector), (Expression) Expression.Quote((Expression) resultSelector), (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IQueryable<TSource>>(Queryable.Distinct<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Distinct<TSource>(
      this IQueryable<TSource> source,
      IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(Queryable.Distinct<TSource>), source, comparer), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Concat<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(Queryable.Concat<TSource>), source1, source2), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Zip<TFirst, TSecond, TResult>(
      this IQueryable<TFirst> source1,
      IEnumerable<TSecond> source2,
      Expression<Func<TFirst, TSecond, TResult>> resultSelector)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      if (resultSelector == null)
        throw Error.ArgumentNull(nameof (resultSelector));
      return source1.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TFirst>, IEnumerable<TSecond>, Expression<Func<TFirst, TSecond, TResult>>, IQueryable<TResult>>(new Func<IQueryable<TFirst>, IEnumerable<TSecond>, Expression<Func<TFirst, TSecond, TResult>>, IQueryable<TResult>>(Queryable.Zip<TFirst, TSecond, TResult>), source1, source2, resultSelector), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSecond>(source2),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Union<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(Queryable.Union<TSource>), source1, source2), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Union<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2,
      IEqualityComparer<TSource> comparer)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(Queryable.Union<TSource>), source1, source2, comparer), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Intersect<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(Queryable.Intersect<TSource>), source1, source2), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Intersect<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2,
      IEqualityComparer<TSource> comparer)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(Queryable.Intersect<TSource>), source1, source2, comparer), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Except<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(Queryable.Except<TSource>), source1, source2), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Except<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2,
      IEqualityComparer<TSource> comparer)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(Queryable.Except<TSource>), source1, source2, comparer), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static TSource First<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource>(new Func<IQueryable<TSource>, TSource>(Queryable.First<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static TSource First<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.First<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource FirstOrDefault<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource>(new Func<IQueryable<TSource>, TSource>(Queryable.FirstOrDefault<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static TSource FirstOrDefault<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.FirstOrDefault<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Last<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource>(new Func<IQueryable<TSource>, TSource>(Queryable.Last<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static TSource Last<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.Last<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource LastOrDefault<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource>(new Func<IQueryable<TSource>, TSource>(Queryable.LastOrDefault<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static TSource LastOrDefault<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.LastOrDefault<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Single<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource>(new Func<IQueryable<TSource>, TSource>(Queryable.Single<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static TSource Single<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.Single<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource>(new Func<IQueryable<TSource>, TSource>(Queryable.SingleOrDefault<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static TSource SingleOrDefault<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.SingleOrDefault<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource ElementAt<TSource>(this IQueryable<TSource> source, int index)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (index < 0)
        throw Error.ArgumentOutOfRange(nameof (index));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, int, TSource>(new Func<IQueryable<TSource>, int, TSource>(Queryable.ElementAt<TSource>), source, index), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) index)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource ElementAtOrDefault<TSource>(this IQueryable<TSource> source, int index)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, int, TSource>(new Func<IQueryable<TSource>, int, TSource>(Queryable.ElementAtOrDefault<TSource>), source, index), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) index)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> DefaultIfEmpty<TSource>(
      this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IQueryable<TSource>>(Queryable.DefaultIfEmpty<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> DefaultIfEmpty<TSource>(
      this IQueryable<TSource> source,
      TSource defaultValue)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource, IQueryable<TSource>>(new Func<IQueryable<TSource>, TSource, IQueryable<TSource>>(Queryable.DefaultIfEmpty<TSource>), source, defaultValue), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) defaultValue, typeof (TSource))
      }));
    }

    [__DynamicallyInvokable]
    public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource, bool>(new Func<IQueryable<TSource>, TSource, bool>(Queryable.Contains<TSource>), source, item), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) item, typeof (TSource))
      }));
    }

    [__DynamicallyInvokable]
    public static bool Contains<TSource>(
      this IQueryable<TSource> source,
      TSource item,
      IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource, IEqualityComparer<TSource>, bool>(new Func<IQueryable<TSource>, TSource, IEqualityComparer<TSource>, bool>(Queryable.Contains<TSource>), source, item, comparer), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Constant((object) item, typeof (TSource)),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Reverse<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IQueryable<TSource>>(new Func<IQueryable<TSource>, IQueryable<TSource>>(Queryable.Reverse<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, bool>(new Func<IQueryable<TSource>, IEnumerable<TSource>, bool>(Queryable.SequenceEqual<TSource>), source1, source2), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(
      this IQueryable<TSource> source1,
      IEnumerable<TSource> source2,
      IEqualityComparer<TSource> comparer)
    {
      if (source1 == null)
        throw Error.ArgumentNull(nameof (source1));
      if (source2 == null)
        throw Error.ArgumentNull(nameof (source2));
      return source1.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, bool>(new Func<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, bool>(Queryable.SequenceEqual<TSource>), source1, source2, comparer), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static bool Any<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, bool>(new Func<IQueryable<TSource>, bool>(Queryable.Any<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static bool Any<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, bool>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, bool>(Queryable.Any<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static bool All<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, bool>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, bool>(Queryable.All<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static int Count<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<int>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, int>(new Func<IQueryable<TSource>, int>(Queryable.Count<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static int Count<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<int>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, int>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, int>(Queryable.Count<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static long LongCount<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<long>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, long>(new Func<IQueryable<TSource>, long>(Queryable.LongCount<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static long LongCount<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (predicate == null)
        throw Error.ArgumentNull(nameof (predicate));
      return source.Provider.Execute<long>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, bool>>, long>(new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, long>(Queryable.LongCount<TSource>), source, predicate), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Min<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource>(new Func<IQueryable<TSource>, TSource>(Queryable.Min<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static TResult Min<TSource, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TResult>>, TResult>(new Func<IQueryable<TSource>, Expression<Func<TSource, TResult>>, TResult>(Queryable.Min<TSource, TResult>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Max<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TSource>(new Func<IQueryable<TSource>, TSource>(Queryable.Max<TSource>), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static TResult Max<TSource, TResult>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TResult>>, TResult>(new Func<IQueryable<TSource>, Expression<Func<TSource, TResult>>, TResult>(Queryable.Max<TSource, TResult>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static int Sum(this IQueryable<int> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<int>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<int>, int>(new Func<IQueryable<int>, int>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static int? Sum(this IQueryable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<int?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<int?>, int?>(new Func<IQueryable<int?>, int?>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static long Sum(this IQueryable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<long>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<long>, long>(new Func<IQueryable<long>, long>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static long? Sum(this IQueryable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<long?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<long?>, long?>(new Func<IQueryable<long?>, long?>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static float Sum(this IQueryable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<float>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<float>, float>(new Func<IQueryable<float>, float>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static float? Sum(this IQueryable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<float?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<float?>, float?>(new Func<IQueryable<float?>, float?>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static double Sum(this IQueryable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<double>, double>(new Func<IQueryable<double>, double>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static double? Sum(this IQueryable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<double?>, double?>(new Func<IQueryable<double?>, double?>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static Decimal Sum(this IQueryable<Decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<Decimal>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<Decimal>, Decimal>(new Func<IQueryable<Decimal>, Decimal>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static Decimal? Sum(this IQueryable<Decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<Decimal?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<Decimal?>, Decimal?>(new Func<IQueryable<Decimal?>, Decimal?>(Queryable.Sum), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static int Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<int>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int>>, int>(new Func<IQueryable<TSource>, Expression<Func<TSource, int>>, int>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static int? Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<int?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int?>>, int?>(new Func<IQueryable<TSource>, Expression<Func<TSource, int?>>, int?>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static long Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, long>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<long>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, long>>, long>(new Func<IQueryable<TSource>, Expression<Func<TSource, long>>, long>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static long? Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, long?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<long?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, long?>>, long?>(new Func<IQueryable<TSource>, Expression<Func<TSource, long?>>, long?>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static float Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, float>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<float>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, float>>, float>(new Func<IQueryable<TSource>, Expression<Func<TSource, float>>, float>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static float? Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, float?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<float?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, float?>>, float?>(new Func<IQueryable<TSource>, Expression<Func<TSource, float?>>, float?>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, double>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, double>>, double>(new Func<IQueryable<TSource>, Expression<Func<TSource, double>>, double>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double? Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, double?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, double?>>, double?>(new Func<IQueryable<TSource>, Expression<Func<TSource, double?>>, double?>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, Decimal>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<Decimal>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, Decimal>>, Decimal>(new Func<IQueryable<TSource>, Expression<Func<TSource, Decimal>>, Decimal>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal? Sum<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, Decimal?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<Decimal?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, Decimal?>>, Decimal?>(new Func<IQueryable<TSource>, Expression<Func<TSource, Decimal?>>, Decimal?>(Queryable.Sum<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double Average(this IQueryable<int> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<int>, double>(new Func<IQueryable<int>, double>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static double? Average(this IQueryable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<int?>, double?>(new Func<IQueryable<int?>, double?>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static double Average(this IQueryable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<long>, double>(new Func<IQueryable<long>, double>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static double? Average(this IQueryable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<long?>, double?>(new Func<IQueryable<long?>, double?>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static float Average(this IQueryable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<float>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<float>, float>(new Func<IQueryable<float>, float>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static float? Average(this IQueryable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<float?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<float?>, float?>(new Func<IQueryable<float?>, float?>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static double Average(this IQueryable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<double>, double>(new Func<IQueryable<double>, double>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static double? Average(this IQueryable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<double?>, double?>(new Func<IQueryable<double?>, double?>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static Decimal Average(this IQueryable<Decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<Decimal>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<Decimal>, Decimal>(new Func<IQueryable<Decimal>, Decimal>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static Decimal? Average(this IQueryable<Decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      return source.Provider.Execute<Decimal?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<Decimal?>, Decimal?>(new Func<IQueryable<Decimal?>, Decimal?>(Queryable.Average), source), source.Expression));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int>>, double>(new Func<IQueryable<TSource>, Expression<Func<TSource, int>>, double>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, int?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, int?>>, double?>(new Func<IQueryable<TSource>, Expression<Func<TSource, int?>>, double?>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static float Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, float>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<float>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, float>>, float>(new Func<IQueryable<TSource>, Expression<Func<TSource, float>>, float>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static float? Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, float?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<float?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, float?>>, float?>(new Func<IQueryable<TSource>, Expression<Func<TSource, float?>>, float?>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, long>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, long>>, double>(new Func<IQueryable<TSource>, Expression<Func<TSource, long>>, double>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, long?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, long?>>, double?>(new Func<IQueryable<TSource>, Expression<Func<TSource, long?>>, double?>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, double>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, double>>, double>(new Func<IQueryable<TSource>, Expression<Func<TSource, double>>, double>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, double?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, double?>>, double?>(new Func<IQueryable<TSource>, Expression<Func<TSource, double?>>, double?>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, Decimal>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<Decimal>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, Decimal>>, Decimal>(new Func<IQueryable<TSource>, Expression<Func<TSource, Decimal>>, Decimal>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal? Average<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, Decimal?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<Decimal?>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, Decimal?>>, Decimal?>(new Func<IQueryable<TSource>, Expression<Func<TSource, Decimal?>>, Decimal?>(Queryable.Average<TSource>), source, selector), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Aggregate<TSource>(
      this IQueryable<TSource> source,
      Expression<Func<TSource, TSource, TSource>> func)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (func == null)
        throw Error.ArgumentNull(nameof (func));
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, Expression<Func<TSource, TSource, TSource>>, TSource>(new Func<IQueryable<TSource>, Expression<Func<TSource, TSource, TSource>>, TSource>(Queryable.Aggregate<TSource>), source, func), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) func)
      }));
    }

    [__DynamicallyInvokable]
    public static TAccumulate Aggregate<TSource, TAccumulate>(
      this IQueryable<TSource> source,
      TAccumulate seed,
      Expression<Func<TAccumulate, TSource, TAccumulate>> func)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (func == null)
        throw Error.ArgumentNull(nameof (func));
      return source.Provider.Execute<TAccumulate>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TAccumulate, Expression<Func<TAccumulate, TSource, TAccumulate>>, TAccumulate>(new Func<IQueryable<TSource>, TAccumulate, Expression<Func<TAccumulate, TSource, TAccumulate>>, TAccumulate>(Queryable.Aggregate<TSource, TAccumulate>), source, seed, func), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Constant((object) seed),
        (Expression) Expression.Quote((Expression) func)
      }));
    }

    [__DynamicallyInvokable]
    public static TResult Aggregate<TSource, TAccumulate, TResult>(
      this IQueryable<TSource> source,
      TAccumulate seed,
      Expression<Func<TAccumulate, TSource, TAccumulate>> func,
      Expression<Func<TAccumulate, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull(nameof (source));
      if (func == null)
        throw Error.ArgumentNull(nameof (func));
      if (selector == null)
        throw Error.ArgumentNull(nameof (selector));
      return source.Provider.Execute<TResult>((Expression) Expression.Call((Expression) null, Queryable.GetMethodInfo<IQueryable<TSource>, TAccumulate, Expression<Func<TAccumulate, TSource, TAccumulate>>, Expression<Func<TAccumulate, TResult>>, TResult>(new Func<IQueryable<TSource>, TAccumulate, Expression<Func<TAccumulate, TSource, TAccumulate>>, Expression<Func<TAccumulate, TResult>>, TResult>(Queryable.Aggregate<TSource, TAccumulate, TResult>), source, seed, func, selector), source.Expression, (Expression) Expression.Constant((object) seed), (Expression) Expression.Quote((Expression) func), (Expression) Expression.Quote((Expression) selector)));
    }
  }
}
