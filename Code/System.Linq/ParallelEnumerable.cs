// Decompiled with JetBrains decompiler
// Type: System.Linq.ParallelEnumerable
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Parallel;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public static class ParallelEnumerable
  {
    private const string RIGHT_SOURCE_NOT_PARALLEL_STR = "The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.";

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> AsParallel<TSource>(
      this IEnumerable<TSource> source)
    {
      return source != null ? (ParallelQuery<TSource>) new ParallelEnumerableWrapper<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> AsParallel<TSource>(
      this Partitioner<TSource> source)
    {
      return source != null ? (ParallelQuery<TSource>) new PartitionerQueryOperator<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> AsOrdered<TSource>(
      this ParallelQuery<TSource> source)
    {
      switch (source)
      {
        case null:
          throw new ArgumentNullException(nameof (source));
        case ParallelEnumerableWrapper<TSource> _:
        case IParallelPartitionable<TSource> _:
label_5:
          return (ParallelQuery<TSource>) new OrderingQueryOperator<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), true);
        case PartitionerQueryOperator<TSource> partitionerQueryOperator:
          if (!partitionerQueryOperator.Orderable)
            throw new InvalidOperationException(SR.GetString("ParallelQuery_PartitionerNotOrderable"));
          goto label_5;
        default:
          throw new InvalidOperationException(SR.GetString("ParallelQuery_InvalidAsOrderedCall"));
      }
    }

    [__DynamicallyInvokable]
    public static ParallelQuery AsOrdered(this ParallelQuery source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source is ParallelEnumerableWrapper source1 ? (ParallelQuery) new OrderingQueryOperator<object>(QueryOperator<object>.AsQueryOperator((IEnumerable<object>) source1), true) : throw new InvalidOperationException(SR.GetString("ParallelQuery_InvalidNonGenericAsOrderedCall"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> AsUnordered<TSource>(
      this ParallelQuery<TSource> source)
    {
      return source != null ? (ParallelQuery<TSource>) new OrderingQueryOperator<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), false) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery AsParallel(this IEnumerable source) => source != null ? (ParallelQuery) new ParallelEnumerableWrapper(source) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static IEnumerable<TSource> AsSequential<TSource>(
      this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source is ParallelEnumerableWrapper<TSource> enumerableWrapper ? enumerableWrapper.WrappedEnumerable : (IEnumerable<TSource>) source;
    }

    internal static ParallelQuery<TSource> WithTaskScheduler<TSource>(
      this ParallelQuery<TSource> source,
      TaskScheduler taskScheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      QuerySettings settings = taskScheduler != null ? QuerySettings.Empty with
      {
        TaskScheduler = taskScheduler
      } : throw new ArgumentNullException(nameof (taskScheduler));
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), settings);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> WithDegreeOfParallelism<TSource>(
      this ParallelQuery<TSource> source,
      int degreeOfParallelism)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      QuerySettings settings = degreeOfParallelism >= 1 && degreeOfParallelism <= 512 ? QuerySettings.Empty with
      {
        DegreeOfParallelism = new int?(degreeOfParallelism)
      } : throw new ArgumentOutOfRangeException(nameof (degreeOfParallelism));
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), settings);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> WithCancellation<TSource>(
      this ParallelQuery<TSource> source,
      CancellationToken cancellationToken)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      CancellationTokenRegistration tokenRegistration = new CancellationTokenRegistration();
      try
      {
        tokenRegistration = cancellationToken.Register((Action) (() => { }));
      }
      catch (ObjectDisposedException ex)
      {
        throw new ArgumentException(SR.GetString("ParallelEnumerable_WithCancellation_TokenSourceDisposed"), nameof (cancellationToken));
      }
      finally
      {
        tokenRegistration.Dispose();
      }
      QuerySettings empty = QuerySettings.Empty with
      {
        CancellationState = new CancellationState(cancellationToken)
      };
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), empty);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> WithExecutionMode<TSource>(
      this ParallelQuery<TSource> source,
      ParallelExecutionMode executionMode)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      QuerySettings settings = executionMode == ParallelExecutionMode.Default || executionMode == ParallelExecutionMode.ForceParallelism ? QuerySettings.Empty with
      {
        ExecutionMode = new ParallelExecutionMode?(executionMode)
      } : throw new ArgumentException(SR.GetString("ParallelEnumerable_WithQueryExecutionMode_InvalidMode"));
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), settings);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> WithMergeOptions<TSource>(
      this ParallelQuery<TSource> source,
      ParallelMergeOptions mergeOptions)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      QuerySettings settings = mergeOptions == ParallelMergeOptions.Default || mergeOptions == ParallelMergeOptions.AutoBuffered || mergeOptions == ParallelMergeOptions.NotBuffered || mergeOptions == ParallelMergeOptions.FullyBuffered ? QuerySettings.Empty with
      {
        MergeOptions = new ParallelMergeOptions?(mergeOptions)
      } : throw new ArgumentException(SR.GetString("ParallelEnumerable_WithMergeOptions_InvalidOptions"));
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), settings);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<int> Range(int start, int count) => count >= 0 && (count <= 0 || int.MaxValue - (count - 1) >= start) ? (ParallelQuery<int>) new RangeEnumerable(start, count) : throw new ArgumentOutOfRangeException(nameof (count));

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Repeat<TResult>(TResult element, int count) => count >= 0 ? (ParallelQuery<TResult>) new RepeatEnumerable<TResult>(element, count) : throw new ArgumentOutOfRangeException(nameof (count));

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Empty<TResult>() => (ParallelQuery<TResult>) EmptyEnumerable<TResult>.Instance;

    [__DynamicallyInvokable]
    public static void ForAll<TSource>(this ParallelQuery<TSource> source, Action<TSource> action)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      new ForAllOperator<TSource>((IEnumerable<TSource>) source, action).RunSynchronously();
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Where<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? (ParallelQuery<TSource>) new WhereQueryOperator<TSource>((IEnumerable<TSource>) source, predicate) : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Where<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? (ParallelQuery<TSource>) new IndexedWhereQueryOperator<TSource>((IEnumerable<TSource>) source, predicate) : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Select<TSource, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return selector != null ? (ParallelQuery<TResult>) new SelectQueryOperator<TSource, TResult>((IEnumerable<TSource>) source, selector) : throw new ArgumentNullException(nameof (selector));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Select<TSource, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, int, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return selector != null ? (ParallelQuery<TResult>) new IndexedSelectQueryOperator<TSource, TResult>((IEnumerable<TSource>) source, selector) : throw new ArgumentNullException(nameof (selector));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Zip<TFirst, TSecond, TResult>(
      this ParallelQuery<TFirst> first,
      ParallelQuery<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return (ParallelQuery<TResult>) new ZipQueryOperator<TFirst, TSecond, TResult>(first, (IEnumerable<TSecond>) second, resultSelector);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Zip<TFirst, TSecond, TResult>(
      this ParallelQuery<TFirst> first,
      IEnumerable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(
      this ParallelQuery<TOuter> outer,
      ParallelQuery<TInner> inner,
      Func<TOuter, TKey> outerKeySelector,
      Func<TInner, TKey> innerKeySelector,
      Func<TOuter, TInner, TResult> resultSelector)
    {
      return ParallelEnumerable.Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, (IEqualityComparer<TKey>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(
      this ParallelQuery<TOuter> outer,
      IEnumerable<TInner> inner,
      Func<TOuter, TKey> outerKeySelector,
      Func<TInner, TKey> innerKeySelector,
      Func<TOuter, TInner, TResult> resultSelector)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(
      this ParallelQuery<TOuter> outer,
      ParallelQuery<TInner> inner,
      Func<TOuter, TKey> outerKeySelector,
      Func<TInner, TKey> innerKeySelector,
      Func<TOuter, TInner, TResult> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (outer == null)
        throw new ArgumentNullException(nameof (outer));
      if (inner == null)
        throw new ArgumentNullException(nameof (inner));
      if (outerKeySelector == null)
        throw new ArgumentNullException(nameof (outerKeySelector));
      if (innerKeySelector == null)
        throw new ArgumentNullException(nameof (innerKeySelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return (ParallelQuery<TResult>) new JoinQueryOperator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(
      this ParallelQuery<TOuter> outer,
      IEnumerable<TInner> inner,
      Func<TOuter, TKey> outerKeySelector,
      Func<TInner, TKey> innerKeySelector,
      Func<TOuter, TInner, TResult> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
      this ParallelQuery<TOuter> outer,
      ParallelQuery<TInner> inner,
      Func<TOuter, TKey> outerKeySelector,
      Func<TInner, TKey> innerKeySelector,
      Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
    {
      return ParallelEnumerable.GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, (IEqualityComparer<TKey>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
      this ParallelQuery<TOuter> outer,
      IEnumerable<TInner> inner,
      Func<TOuter, TKey> outerKeySelector,
      Func<TInner, TKey> innerKeySelector,
      Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
      this ParallelQuery<TOuter> outer,
      ParallelQuery<TInner> inner,
      Func<TOuter, TKey> outerKeySelector,
      Func<TInner, TKey> innerKeySelector,
      Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (outer == null)
        throw new ArgumentNullException(nameof (outer));
      if (inner == null)
        throw new ArgumentNullException(nameof (inner));
      if (outerKeySelector == null)
        throw new ArgumentNullException(nameof (outerKeySelector));
      if (innerKeySelector == null)
        throw new ArgumentNullException(nameof (innerKeySelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return (ParallelQuery<TResult>) new GroupJoinQueryOperator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
      this ParallelQuery<TOuter> outer,
      IEnumerable<TInner> inner,
      Func<TOuter, TKey> outerKeySelector,
      Func<TInner, TKey> innerKeySelector,
      Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> SelectMany<TSource, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, IEnumerable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return selector != null ? (ParallelQuery<TResult>) new SelectManyQueryOperator<TSource, TResult, TResult>((IEnumerable<TSource>) source, selector, (Func<TSource, int, IEnumerable<TResult>>) null, (Func<TSource, TResult, TResult>) null) : throw new ArgumentNullException(nameof (selector));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> SelectMany<TSource, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, int, IEnumerable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return selector != null ? (ParallelQuery<TResult>) new SelectManyQueryOperator<TSource, TResult, TResult>((IEnumerable<TSource>) source, (Func<TSource, IEnumerable<TResult>>) null, selector, (Func<TSource, TResult, TResult>) null) : throw new ArgumentNullException(nameof (selector));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> SelectMany<TSource, TCollection, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (collectionSelector == null)
        throw new ArgumentNullException(nameof (collectionSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return (ParallelQuery<TResult>) new SelectManyQueryOperator<TSource, TCollection, TResult>((IEnumerable<TSource>) source, collectionSelector, (Func<TSource, int, IEnumerable<TCollection>>) null, resultSelector);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> SelectMany<TSource, TCollection, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (collectionSelector == null)
        throw new ArgumentNullException(nameof (collectionSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return (ParallelQuery<TResult>) new SelectManyQueryOperator<TSource, TCollection, TResult>((IEnumerable<TSource>) source, (Func<TSource, IEnumerable<TCollection>>) null, collectionSelector, resultSelector);
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> OrderBy<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return keySelector != null ? new OrderedParallelQuery<TSource>((QueryOperator<TSource>) new SortQueryOperator<TSource, TKey>((IEnumerable<TSource>) source, keySelector, (IComparer<TKey>) null, false)) : throw new ArgumentNullException(nameof (keySelector));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> OrderBy<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) new SortQueryOperator<TSource, TKey>((IEnumerable<TSource>) source, keySelector, comparer, false));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> OrderByDescending<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return keySelector != null ? new OrderedParallelQuery<TSource>((QueryOperator<TSource>) new SortQueryOperator<TSource, TKey>((IEnumerable<TSource>) source, keySelector, (IComparer<TKey>) null, true)) : throw new ArgumentNullException(nameof (keySelector));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> OrderByDescending<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) new SortQueryOperator<TSource, TKey>((IEnumerable<TSource>) source, keySelector, comparer, true));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> ThenBy<TSource, TKey>(
      this OrderedParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) source.OrderedEnumerable.CreateOrderedEnumerable<TKey>(keySelector, (IComparer<TKey>) null, false));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> ThenBy<TSource, TKey>(
      this OrderedParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) source.OrderedEnumerable.CreateOrderedEnumerable<TKey>(keySelector, comparer, false));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> ThenByDescending<TSource, TKey>(
      this OrderedParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) source.OrderedEnumerable.CreateOrderedEnumerable<TKey>(keySelector, (IComparer<TKey>) null, true));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> ThenByDescending<TSource, TKey>(
      this OrderedParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) source.OrderedEnumerable.CreateOrderedEnumerable<TKey>(keySelector, comparer, true));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return ParallelEnumerable.GroupBy<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) null);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      return (ParallelQuery<IGrouping<TKey, TSource>>) new GroupByQueryOperator<TSource, TKey, TSource>((IEnumerable<TSource>) source, keySelector, (Func<TSource, TSource>) null, comparer);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return ParallelEnumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) null);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      return (ParallelQuery<IGrouping<TKey, TElement>>) new GroupByQueryOperator<TSource, TKey, TElement>((IEnumerable<TSource>) source, keySelector, elementSelector, comparer);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupBy<TSource, TKey, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
    {
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return ParallelEnumerable.GroupBy<TSource, TKey>(source, keySelector).Select<IGrouping<TKey, TSource>, TResult>((Func<IGrouping<TKey, TSource>, TResult>) (grouping => resultSelector(grouping.Key, (IEnumerable<TSource>) grouping)));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupBy<TSource, TKey, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return ParallelEnumerable.GroupBy<TSource, TKey>(source, keySelector, comparer).Select<IGrouping<TKey, TSource>, TResult>((Func<IGrouping<TKey, TSource>, TResult>) (grouping => resultSelector(grouping.Key, (IEnumerable<TSource>) grouping)));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupBy<TSource, TKey, TElement, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return ParallelEnumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector).Select<IGrouping<TKey, TElement>, TResult>((Func<IGrouping<TKey, TElement>, TResult>) (grouping => resultSelector(grouping.Key, (IEnumerable<TElement>) grouping)));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupBy<TSource, TKey, TElement, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return ParallelEnumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer).Select<IGrouping<TKey, TElement>, TResult>((Func<IGrouping<TKey, TElement>, TResult>) (grouping => resultSelector(grouping.Key, (IEnumerable<TElement>) grouping)));
    }

    private static T PerformAggregation<T>(
      this ParallelQuery<T> source,
      Func<T, T, T> reduce,
      T seed,
      bool seedIsSpecified,
      bool throwIfEmpty,
      QueryAggregationOptions options)
    {
      return new AssociativeAggregationOperator<T, T, T>((IEnumerable<T>) source, seed, (Func<T>) null, seedIsSpecified, reduce, reduce, (Func<T, T>) (obj => obj), throwIfEmpty, options).Aggregate();
    }

    private static TAccumulate PerformSequentialAggregation<TSource, TAccumulate>(
      this ParallelQuery<TSource> source,
      TAccumulate seed,
      bool seedIsSpecified,
      Func<TAccumulate, TSource, TAccumulate> func)
    {
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
      {
        TAccumulate accumulate;
        if (seedIsSpecified)
          accumulate = seed;
        else
          accumulate = enumerator.MoveNext() ? (TAccumulate) (object) enumerator.Current : throw new InvalidOperationException(SR.GetString("NoElements"));
        while (enumerator.MoveNext())
        {
          TSource current = enumerator.Current;
          try
          {
            accumulate = func(accumulate, current);
          }
          catch (ThreadAbortException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            throw new AggregateException(new Exception[1]
            {
              ex
            });
          }
        }
        return accumulate;
      }
    }

    [__DynamicallyInvokable]
    public static TSource Aggregate<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, TSource, TSource> func)
    {
      return source.Aggregate<TSource>(func, QueryAggregationOptions.AssociativeCommutative);
    }

    internal static TSource Aggregate<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, TSource, TSource> func,
      QueryAggregationOptions options)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (func == null)
        throw new ArgumentNullException(nameof (func));
      if ((~QueryAggregationOptions.AssociativeCommutative & options) != QueryAggregationOptions.None)
        throw new ArgumentOutOfRangeException(nameof (options));
      return (options & QueryAggregationOptions.Associative) != QueryAggregationOptions.Associative ? source.PerformSequentialAggregation<TSource, TSource>(default (TSource), false, func) : source.PerformAggregation<TSource>(func, default (TSource), false, true, options);
    }

    [__DynamicallyInvokable]
    public static TAccumulate Aggregate<TSource, TAccumulate>(
      this ParallelQuery<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> func)
    {
      return source.Aggregate<TSource, TAccumulate>(seed, func, QueryAggregationOptions.AssociativeCommutative);
    }

    internal static TAccumulate Aggregate<TSource, TAccumulate>(
      this ParallelQuery<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> func,
      QueryAggregationOptions options)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (func == null)
        throw new ArgumentNullException(nameof (func));
      if ((~QueryAggregationOptions.AssociativeCommutative & options) != QueryAggregationOptions.None)
        throw new ArgumentOutOfRangeException(nameof (options));
      return source.PerformSequentialAggregation<TSource, TAccumulate>(seed, true, func);
    }

    [__DynamicallyInvokable]
    public static TResult Aggregate<TSource, TAccumulate, TResult>(
      this ParallelQuery<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> func,
      Func<TAccumulate, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (func == null)
        throw new ArgumentNullException(nameof (func));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      TAccumulate accumulate = source.PerformSequentialAggregation<TSource, TAccumulate>(seed, true, func);
      try
      {
        return resultSelector(accumulate);
      }
      catch (ThreadAbortException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new AggregateException(new Exception[1]{ ex });
      }
    }

    [__DynamicallyInvokable]
    public static TResult Aggregate<TSource, TAccumulate, TResult>(
      this ParallelQuery<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> updateAccumulatorFunc,
      Func<TAccumulate, TAccumulate, TAccumulate> combineAccumulatorsFunc,
      Func<TAccumulate, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (updateAccumulatorFunc == null)
        throw new ArgumentNullException(nameof (updateAccumulatorFunc));
      if (combineAccumulatorsFunc == null)
        throw new ArgumentNullException(nameof (combineAccumulatorsFunc));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return new AssociativeAggregationOperator<TSource, TAccumulate, TResult>((IEnumerable<TSource>) source, seed, (Func<TAccumulate>) null, true, updateAccumulatorFunc, combineAccumulatorsFunc, resultSelector, false, QueryAggregationOptions.AssociativeCommutative).Aggregate();
    }

    [__DynamicallyInvokable]
    public static TResult Aggregate<TSource, TAccumulate, TResult>(
      this ParallelQuery<TSource> source,
      Func<TAccumulate> seedFactory,
      Func<TAccumulate, TSource, TAccumulate> updateAccumulatorFunc,
      Func<TAccumulate, TAccumulate, TAccumulate> combineAccumulatorsFunc,
      Func<TAccumulate, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (seedFactory == null)
        throw new ArgumentNullException(nameof (seedFactory));
      if (updateAccumulatorFunc == null)
        throw new ArgumentNullException(nameof (updateAccumulatorFunc));
      if (combineAccumulatorsFunc == null)
        throw new ArgumentNullException(nameof (combineAccumulatorsFunc));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
      return new AssociativeAggregationOperator<TSource, TAccumulate, TResult>((IEnumerable<TSource>) source, default (TAccumulate), seedFactory, true, updateAccumulatorFunc, combineAccumulatorsFunc, resultSelector, false, QueryAggregationOptions.AssociativeCommutative).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int Count<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source is ParallelEnumerableWrapper<TSource> enumerableWrapper && enumerableWrapper.WrappedEnumerable is ICollection<TSource> wrappedEnumerable ? wrappedEnumerable.Count : new CountAggregationOperator<TSource>((IEnumerable<TSource>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int Count<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? new CountAggregationOperator<TSource>((IEnumerable<TSource>) ParallelEnumerable.Where<TSource>(source, predicate)).Aggregate() : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static long LongCount<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source is ParallelEnumerableWrapper<TSource> enumerableWrapper && enumerableWrapper.WrappedEnumerable is ICollection<TSource> wrappedEnumerable ? (long) wrappedEnumerable.Count : new LongCountAggregationOperator<TSource>((IEnumerable<TSource>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long LongCount<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? new LongCountAggregationOperator<TSource>((IEnumerable<TSource>) ParallelEnumerable.Where<TSource>(source, predicate)).Aggregate() : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static int Sum(this ParallelQuery<int> source) => source != null ? new IntSumAggregationOperator((IEnumerable<int>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static int? Sum(this ParallelQuery<int?> source) => source != null ? new NullableIntSumAggregationOperator((IEnumerable<int?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static long Sum(this ParallelQuery<long> source) => source != null ? new LongSumAggregationOperator((IEnumerable<long>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static long? Sum(this ParallelQuery<long?> source) => source != null ? new NullableLongSumAggregationOperator((IEnumerable<long?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static float Sum(this ParallelQuery<float> source) => source != null ? new FloatSumAggregationOperator((IEnumerable<float>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static float? Sum(this ParallelQuery<float?> source) => source != null ? new NullableFloatSumAggregationOperator((IEnumerable<float?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double Sum(this ParallelQuery<double> source) => source != null ? new DoubleSumAggregationOperator((IEnumerable<double>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double? Sum(this ParallelQuery<double?> source) => source != null ? new NullableDoubleSumAggregationOperator((IEnumerable<double?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static Decimal Sum(this ParallelQuery<Decimal> source) => source != null ? new DecimalSumAggregationOperator((IEnumerable<Decimal>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static Decimal? Sum(this ParallelQuery<Decimal?> source) => source != null ? new NullableDecimalSumAggregationOperator((IEnumerable<Decimal?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static int Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector) => ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, int>(source, selector));

    [__DynamicallyInvokable]
    public static int? Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, int?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, int?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static long Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, long> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, long>(source, selector));
    }

    [__DynamicallyInvokable]
    public static long? Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, long?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, long?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, float> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, float>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float? Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, float?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, float?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, double> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, double>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, double?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, double?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, Decimal>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal? Sum<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, Decimal?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static int Min(this ParallelQuery<int> source) => source != null ? new IntMinMaxAggregationOperator((IEnumerable<int>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static int? Min(this ParallelQuery<int?> source) => source != null ? new NullableIntMinMaxAggregationOperator((IEnumerable<int?>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static long Min(this ParallelQuery<long> source) => source != null ? new LongMinMaxAggregationOperator((IEnumerable<long>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static long? Min(this ParallelQuery<long?> source) => source != null ? new NullableLongMinMaxAggregationOperator((IEnumerable<long?>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static float Min(this ParallelQuery<float> source) => source != null ? new FloatMinMaxAggregationOperator((IEnumerable<float>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static float? Min(this ParallelQuery<float?> source) => source != null ? new NullableFloatMinMaxAggregationOperator((IEnumerable<float?>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double Min(this ParallelQuery<double> source) => source != null ? new DoubleMinMaxAggregationOperator((IEnumerable<double>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double? Min(this ParallelQuery<double?> source) => source != null ? new NullableDoubleMinMaxAggregationOperator((IEnumerable<double?>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static Decimal Min(this ParallelQuery<Decimal> source) => source != null ? new DecimalMinMaxAggregationOperator((IEnumerable<Decimal>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static Decimal? Min(this ParallelQuery<Decimal?> source) => source != null ? new NullableDecimalMinMaxAggregationOperator((IEnumerable<Decimal?>) source, -1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static TSource Min<TSource>(this ParallelQuery<TSource> source) => source != null ? AggregationMinMaxHelpers<TSource>.ReduceMin((IEnumerable<TSource>) source) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static int Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector) => ParallelEnumerable.Select<TSource, int>(source, selector).Min<int>();

    [__DynamicallyInvokable]
    public static int? Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, int?> selector)
    {
      return ParallelEnumerable.Select<TSource, int?>(source, selector).Min<int?>();
    }

    [__DynamicallyInvokable]
    public static long Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, long> selector)
    {
      return ParallelEnumerable.Select<TSource, long>(source, selector).Min<long>();
    }

    [__DynamicallyInvokable]
    public static long? Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, long?> selector)
    {
      return ParallelEnumerable.Select<TSource, long?>(source, selector).Min<long?>();
    }

    [__DynamicallyInvokable]
    public static float Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, float> selector)
    {
      return ParallelEnumerable.Select<TSource, float>(source, selector).Min<float>();
    }

    [__DynamicallyInvokable]
    public static float? Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, float?> selector)
    {
      return ParallelEnumerable.Select<TSource, float?>(source, selector).Min<float?>();
    }

    [__DynamicallyInvokable]
    public static double Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, double> selector)
    {
      return ParallelEnumerable.Select<TSource, double>(source, selector).Min<double>();
    }

    [__DynamicallyInvokable]
    public static double? Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, double?> selector)
    {
      return ParallelEnumerable.Select<TSource, double?>(source, selector).Min<double?>();
    }

    [__DynamicallyInvokable]
    public static Decimal Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return ParallelEnumerable.Select<TSource, Decimal>(source, selector).Min<Decimal>();
    }

    [__DynamicallyInvokable]
    public static Decimal? Min<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return ParallelEnumerable.Select<TSource, Decimal?>(source, selector).Min<Decimal?>();
    }

    [__DynamicallyInvokable]
    public static TResult Min<TSource, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, TResult> selector)
    {
      return ParallelEnumerable.Select<TSource, TResult>(source, selector).Min<TResult>();
    }

    [__DynamicallyInvokable]
    public static int Max(this ParallelQuery<int> source) => source != null ? new IntMinMaxAggregationOperator((IEnumerable<int>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static int? Max(this ParallelQuery<int?> source) => source != null ? new NullableIntMinMaxAggregationOperator((IEnumerable<int?>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static long Max(this ParallelQuery<long> source) => source != null ? new LongMinMaxAggregationOperator((IEnumerable<long>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static long? Max(this ParallelQuery<long?> source) => source != null ? new NullableLongMinMaxAggregationOperator((IEnumerable<long?>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static float Max(this ParallelQuery<float> source) => source != null ? new FloatMinMaxAggregationOperator((IEnumerable<float>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static float? Max(this ParallelQuery<float?> source) => source != null ? new NullableFloatMinMaxAggregationOperator((IEnumerable<float?>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double Max(this ParallelQuery<double> source) => source != null ? new DoubleMinMaxAggregationOperator((IEnumerable<double>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double? Max(this ParallelQuery<double?> source) => source != null ? new NullableDoubleMinMaxAggregationOperator((IEnumerable<double?>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static Decimal Max(this ParallelQuery<Decimal> source) => source != null ? new DecimalMinMaxAggregationOperator((IEnumerable<Decimal>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static Decimal? Max(this ParallelQuery<Decimal?> source) => source != null ? new NullableDecimalMinMaxAggregationOperator((IEnumerable<Decimal?>) source, 1).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static TSource Max<TSource>(this ParallelQuery<TSource> source) => source != null ? AggregationMinMaxHelpers<TSource>.ReduceMax((IEnumerable<TSource>) source) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static int Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector) => ParallelEnumerable.Select<TSource, int>(source, selector).Max<int>();

    [__DynamicallyInvokable]
    public static int? Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, int?> selector)
    {
      return ParallelEnumerable.Select<TSource, int?>(source, selector).Max<int?>();
    }

    [__DynamicallyInvokable]
    public static long Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, long> selector)
    {
      return ParallelEnumerable.Select<TSource, long>(source, selector).Max<long>();
    }

    [__DynamicallyInvokable]
    public static long? Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, long?> selector)
    {
      return ParallelEnumerable.Select<TSource, long?>(source, selector).Max<long?>();
    }

    [__DynamicallyInvokable]
    public static float Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, float> selector)
    {
      return ParallelEnumerable.Select<TSource, float>(source, selector).Max<float>();
    }

    [__DynamicallyInvokable]
    public static float? Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, float?> selector)
    {
      return ParallelEnumerable.Select<TSource, float?>(source, selector).Max<float?>();
    }

    [__DynamicallyInvokable]
    public static double Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, double> selector)
    {
      return ParallelEnumerable.Select<TSource, double>(source, selector).Max<double>();
    }

    [__DynamicallyInvokable]
    public static double? Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, double?> selector)
    {
      return ParallelEnumerable.Select<TSource, double?>(source, selector).Max<double?>();
    }

    [__DynamicallyInvokable]
    public static Decimal Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return ParallelEnumerable.Select<TSource, Decimal>(source, selector).Max<Decimal>();
    }

    [__DynamicallyInvokable]
    public static Decimal? Max<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return ParallelEnumerable.Select<TSource, Decimal?>(source, selector).Max<Decimal?>();
    }

    [__DynamicallyInvokable]
    public static TResult Max<TSource, TResult>(
      this ParallelQuery<TSource> source,
      Func<TSource, TResult> selector)
    {
      return ParallelEnumerable.Select<TSource, TResult>(source, selector).Max<TResult>();
    }

    [__DynamicallyInvokable]
    public static double Average(this ParallelQuery<int> source) => source != null ? new IntAverageAggregationOperator((IEnumerable<int>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double? Average(this ParallelQuery<int?> source) => source != null ? new NullableIntAverageAggregationOperator((IEnumerable<int?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double Average(this ParallelQuery<long> source) => source != null ? new LongAverageAggregationOperator((IEnumerable<long>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double? Average(this ParallelQuery<long?> source) => source != null ? new NullableLongAverageAggregationOperator((IEnumerable<long?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static float Average(this ParallelQuery<float> source) => source != null ? new FloatAverageAggregationOperator((IEnumerable<float>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static float? Average(this ParallelQuery<float?> source) => source != null ? new NullableFloatAverageAggregationOperator((IEnumerable<float?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double Average(this ParallelQuery<double> source) => source != null ? new DoubleAverageAggregationOperator((IEnumerable<double>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double? Average(this ParallelQuery<double?> source) => source != null ? new NullableDoubleAverageAggregationOperator((IEnumerable<double?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static Decimal Average(this ParallelQuery<Decimal> source) => source != null ? new DecimalAverageAggregationOperator((IEnumerable<Decimal>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static Decimal? Average(this ParallelQuery<Decimal?> source) => source != null ? new NullableDecimalAverageAggregationOperator((IEnumerable<Decimal?>) source).Aggregate() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static double Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, int> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, int>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, int?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, int?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, long> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, long>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, long?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, long?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, float> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, float>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float? Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, float?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, float?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, double> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, double>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, double?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, double?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, Decimal>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal? Average<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, Decimal?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static bool Any<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? new AnyAllSearchOperator<TSource>((IEnumerable<TSource>) source, true, predicate).Aggregate() : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static bool Any<TSource>(this ParallelQuery<TSource> source) => source != null ? ParallelEnumerable.Any<TSource>(source, (Func<TSource, bool>) (x => true)) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static bool All<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? new AnyAllSearchOperator<TSource>((IEnumerable<TSource>) source, false, predicate).Aggregate() : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static bool Contains<TSource>(this ParallelQuery<TSource> source, TSource value) => ParallelEnumerable.Contains<TSource>(source, value, (IEqualityComparer<TSource>) null);

    [__DynamicallyInvokable]
    public static bool Contains<TSource>(
      this ParallelQuery<TSource> source,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return new ContainsSearchOperator<TSource>((IEnumerable<TSource>) source, value, comparer).Aggregate();
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Take<TSource>(
      this ParallelQuery<TSource> source,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return count > 0 ? (ParallelQuery<TSource>) new TakeOrSkipQueryOperator<TSource>((IEnumerable<TSource>) source, count, true) : ParallelEnumerable.Empty<TSource>();
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> TakeWhile<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? (ParallelQuery<TSource>) new TakeOrSkipWhileQueryOperator<TSource>((IEnumerable<TSource>) source, predicate, (Func<TSource, int, bool>) null, true) : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> TakeWhile<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? (ParallelQuery<TSource>) new TakeOrSkipWhileQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null, predicate, true) : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Skip<TSource>(
      this ParallelQuery<TSource> source,
      int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return count <= 0 ? source : (ParallelQuery<TSource>) new TakeOrSkipQueryOperator<TSource>((IEnumerable<TSource>) source, count, false);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> SkipWhile<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? (ParallelQuery<TSource>) new TakeOrSkipWhileQueryOperator<TSource>((IEnumerable<TSource>) source, predicate, (Func<TSource, int, bool>) null, false) : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> SkipWhile<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? (ParallelQuery<TSource>) new TakeOrSkipWhileQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null, predicate, false) : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Concat<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      return second != null ? (ParallelQuery<TSource>) new ConcatQueryOperator<TSource>(first, second) : throw new ArgumentNullException(nameof (second));
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Concat<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      return second != null ? ParallelEnumerable.SequenceEqual<TSource>(first, second, (IEqualityComparer<TSource>) null) : throw new ArgumentNullException(nameof (second));
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      comparer = comparer ?? (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default;
      QuerySettings querySettings1 = QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) first).SpecifiedQuerySettings.Merge(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) second).SpecifiedQuerySettings);
      querySettings1 = querySettings1.WithDefaults();
      QuerySettings querySettings2 = querySettings1.WithPerExecutionSettings(new CancellationTokenSource(), new Shared<bool>(false));
      IEnumerator<TSource> enumerator1 = first.GetEnumerator();
      try
      {
        IEnumerator<TSource> enumerator2 = second.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
          {
            if (!enumerator2.MoveNext() || !comparer.Equals(enumerator1.Current, enumerator2.Current))
              return false;
          }
          if (enumerator2.MoveNext())
            return false;
        }
        catch (ThreadAbortException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          ExceptionAggregator.ThrowOCEorAggregateException(ex, querySettings2.CancellationState);
        }
        finally
        {
          ParallelEnumerable.DisposeEnumerator<TSource>(enumerator2, querySettings2.CancellationState);
        }
      }
      finally
      {
        ParallelEnumerable.DisposeEnumerator<TSource>(enumerator1, querySettings2.CancellationState);
      }
      return true;
    }

    private static void DisposeEnumerator<TSource>(
      IEnumerator<TSource> e,
      CancellationState cancelState)
    {
      try
      {
        e.Dispose();
      }
      catch (ThreadAbortException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        ExceptionAggregator.ThrowOCEorAggregateException(ex, cancelState);
      }
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Distinct<TSource>(
      this ParallelQuery<TSource> source)
    {
      return ParallelEnumerable.Distinct<TSource>(source, (IEqualityComparer<TSource>) null);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Distinct<TSource>(
      this ParallelQuery<TSource> source,
      IEqualityComparer<TSource> comparer)
    {
      return source != null ? (ParallelQuery<TSource>) new DistinctQueryOperator<TSource>((IEnumerable<TSource>) source, comparer) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Union<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second)
    {
      return ParallelEnumerable.Union<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Union<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Union<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return (ParallelQuery<TSource>) new UnionQueryOperator<TSource>(first, second, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Union<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Intersect<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second)
    {
      return ParallelEnumerable.Intersect<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Intersect<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Intersect<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return (ParallelQuery<TSource>) new IntersectQueryOperator<TSource>(first, second, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Intersect<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Except<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second)
    {
      return ParallelEnumerable.Except<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Except<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Except<TSource>(
      this ParallelQuery<TSource> first,
      ParallelQuery<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
      return (ParallelQuery<TSource>) new ExceptQueryOperator<TSource>(first, second, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Except<TSource>(
      this ParallelQuery<TSource> first,
      IEnumerable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static IEnumerable<TSource> AsEnumerable<TSource>(
      this ParallelQuery<TSource> source)
    {
      return source.AsSequential<TSource>();
    }

    [__DynamicallyInvokable]
    public static TSource[] ToArray<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source is QueryOperator<TSource> queryOperator ? queryOperator.ExecuteAndGetResultsAsArray() : ParallelEnumerable.ToList<TSource>(source).ToArray<TSource>();
    }

    [__DynamicallyInvokable]
    public static List<TSource> ToList<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      List<TSource> list = new List<TSource>();
      IEnumerator<TSource> enumerator;
      if (source is QueryOperator<TSource> queryOperator)
      {
        if (queryOperator.OrdinalIndexState == OrdinalIndexState.Indexible && queryOperator.OutputOrdered)
          return new List<TSource>((IEnumerable<TSource>) ParallelEnumerable.ToArray<TSource>(source));
        enumerator = queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered));
      }
      else
        enumerator = source.GetEnumerator();
      using (enumerator)
      {
        while (enumerator.MoveNext())
          list.Add(enumerator.Current);
      }
      return list;
    }

    [__DynamicallyInvokable]
    public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return ParallelEnumerable.ToDictionary<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    [__DynamicallyInvokable]
    public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>(comparer);
      IEnumerator<TSource> enumerator = !(source is QueryOperator<TSource> queryOperator) ? source.GetEnumerator() : queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true);
      using (enumerator)
      {
        while (enumerator.MoveNext())
        {
          TSource current = enumerator.Current;
          try
          {
            TKey key = keySelector(current);
            dictionary.Add(key, current);
          }
          catch (ThreadAbortException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            throw new AggregateException(new Exception[1]
            {
              ex
            });
          }
        }
      }
      return dictionary;
    }

    [__DynamicallyInvokable]
    public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return ParallelEnumerable.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    [__DynamicallyInvokable]
    public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);
      IEnumerator<TSource> enumerator = !(source is QueryOperator<TSource> queryOperator) ? source.GetEnumerator() : queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true);
      using (enumerator)
      {
        while (enumerator.MoveNext())
        {
          TSource current = enumerator.Current;
          try
          {
            dictionary.Add(keySelector(current), elementSelector(current));
          }
          catch (ThreadAbortException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            throw new AggregateException(new Exception[1]
            {
              ex
            });
          }
        }
      }
      return dictionary;
    }

    [__DynamicallyInvokable]
    public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return ParallelEnumerable.ToLookup<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    [__DynamicallyInvokable]
    public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      comparer = comparer ?? (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
      ParallelQuery<IGrouping<TKey, TSource>> parallelQuery = ParallelEnumerable.GroupBy<TSource, TKey>(source, keySelector, comparer);
      Lookup<TKey, TSource> lookup = new Lookup<TKey, TSource>(comparer);
      IEnumerator<IGrouping<TKey, TSource>> enumerator = !(parallelQuery is QueryOperator<IGrouping<TKey, TSource>> queryOperator) ? parallelQuery.GetEnumerator() : queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered));
      using (enumerator)
      {
        while (enumerator.MoveNext())
          lookup.Add(enumerator.Current);
      }
      return (ILookup<TKey, TSource>) lookup;
    }

    [__DynamicallyInvokable]
    public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return ParallelEnumerable.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    [__DynamicallyInvokable]
    public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
      this ParallelQuery<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      comparer = comparer ?? (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
      ParallelQuery<IGrouping<TKey, TElement>> parallelQuery = ParallelEnumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
      Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
      IEnumerator<IGrouping<TKey, TElement>> enumerator = !(parallelQuery is QueryOperator<IGrouping<TKey, TElement>> queryOperator) ? parallelQuery.GetEnumerator() : queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered));
      using (enumerator)
      {
        while (enumerator.MoveNext())
          lookup.Add(enumerator.Current);
      }
      return (ILookup<TKey, TElement>) lookup;
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Reverse<TSource>(
      this ParallelQuery<TSource> source)
    {
      return source != null ? (ParallelQuery<TSource>) new ReverseQueryOperator<TSource>((IEnumerable<TSource>) source) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> OfType<TResult>(this ParallelQuery source) => source != null ? source.OfType<TResult>() : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Cast<TResult>(this ParallelQuery source) => source.Cast<TResult>();

    private static TSource GetOneWithPossibleDefault<TSource>(
      QueryOperator<TSource> queryOp,
      bool throwIfTwo,
      bool defaultIfEmpty)
    {
      using (IEnumerator<TSource> enumerator = queryOp.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered)))
      {
        if (enumerator.MoveNext())
        {
          TSource current = enumerator.Current;
          if (throwIfTwo && enumerator.MoveNext())
            throw new InvalidOperationException(SR.GetString("MoreThanOneMatch"));
          return current;
        }
      }
      if (defaultIfEmpty)
        return default (TSource);
      throw new InvalidOperationException(SR.GetString("NoElements"));
    }

    [__DynamicallyInvokable]
    public static TSource First<TSource>(this ParallelQuery<TSource> source)
    {
      FirstQueryOperator<TSource> queryOp = source != null ? new FirstQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null) : throw new ArgumentNullException(nameof (source));
      QuerySettings querySettings = queryOp.SpecifiedQuerySettings.WithDefaults();
      if (queryOp.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        ParallelExecutionMode parallelExecutionMode = ParallelExecutionMode.ForceParallelism;
        if ((executionMode.GetValueOrDefault() == parallelExecutionMode ? (!executionMode.HasValue ? 1 : 0) : 1) != 0)
          return ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(queryOp.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).First<TSource>();
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) queryOp, false, false);
    }

    [__DynamicallyInvokable]
    public static TSource First<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      FirstQueryOperator<TSource> queryOp = predicate != null ? new FirstQueryOperator<TSource>((IEnumerable<TSource>) source, predicate) : throw new ArgumentNullException(nameof (predicate));
      QuerySettings querySettings = queryOp.SpecifiedQuerySettings.WithDefaults();
      if (queryOp.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        ParallelExecutionMode parallelExecutionMode = ParallelExecutionMode.ForceParallelism;
        if ((executionMode.GetValueOrDefault() == parallelExecutionMode ? (!executionMode.HasValue ? 1 : 0) : 1) != 0)
          return ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(queryOp.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).First<TSource>(ExceptionAggregator.WrapFunc<TSource, bool>(predicate, querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) queryOp, false, false);
    }

    [__DynamicallyInvokable]
    public static TSource FirstOrDefault<TSource>(this ParallelQuery<TSource> source)
    {
      FirstQueryOperator<TSource> queryOp = source != null ? new FirstQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null) : throw new ArgumentNullException(nameof (source));
      QuerySettings querySettings = queryOp.SpecifiedQuerySettings.WithDefaults();
      if (queryOp.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        ParallelExecutionMode parallelExecutionMode = ParallelExecutionMode.ForceParallelism;
        if ((executionMode.GetValueOrDefault() == parallelExecutionMode ? (!executionMode.HasValue ? 1 : 0) : 1) != 0)
          return ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(queryOp.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).FirstOrDefault<TSource>();
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) queryOp, false, true);
    }

    [__DynamicallyInvokable]
    public static TSource FirstOrDefault<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      FirstQueryOperator<TSource> queryOp = predicate != null ? new FirstQueryOperator<TSource>((IEnumerable<TSource>) source, predicate) : throw new ArgumentNullException(nameof (predicate));
      QuerySettings querySettings = queryOp.SpecifiedQuerySettings.WithDefaults();
      if (queryOp.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        ParallelExecutionMode parallelExecutionMode = ParallelExecutionMode.ForceParallelism;
        if ((executionMode.GetValueOrDefault() == parallelExecutionMode ? (!executionMode.HasValue ? 1 : 0) : 1) != 0)
          return ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(queryOp.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).FirstOrDefault<TSource>(ExceptionAggregator.WrapFunc<TSource, bool>(predicate, querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) queryOp, false, true);
    }

    [__DynamicallyInvokable]
    public static TSource Last<TSource>(this ParallelQuery<TSource> source)
    {
      LastQueryOperator<TSource> queryOp = source != null ? new LastQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null) : throw new ArgumentNullException(nameof (source));
      QuerySettings querySettings = queryOp.SpecifiedQuerySettings.WithDefaults();
      if (queryOp.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        ParallelExecutionMode parallelExecutionMode = ParallelExecutionMode.ForceParallelism;
        if ((executionMode.GetValueOrDefault() == parallelExecutionMode ? (!executionMode.HasValue ? 1 : 0) : 1) != 0)
          return ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(queryOp.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).Last<TSource>();
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) queryOp, false, false);
    }

    [__DynamicallyInvokable]
    public static TSource Last<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      LastQueryOperator<TSource> queryOp = predicate != null ? new LastQueryOperator<TSource>((IEnumerable<TSource>) source, predicate) : throw new ArgumentNullException(nameof (predicate));
      QuerySettings querySettings = queryOp.SpecifiedQuerySettings.WithDefaults();
      if (queryOp.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        ParallelExecutionMode parallelExecutionMode = ParallelExecutionMode.ForceParallelism;
        if ((executionMode.GetValueOrDefault() == parallelExecutionMode ? (!executionMode.HasValue ? 1 : 0) : 1) != 0)
          return ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(queryOp.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).Last<TSource>(ExceptionAggregator.WrapFunc<TSource, bool>(predicate, querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) queryOp, false, false);
    }

    [__DynamicallyInvokable]
    public static TSource LastOrDefault<TSource>(this ParallelQuery<TSource> source)
    {
      LastQueryOperator<TSource> queryOp = source != null ? new LastQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null) : throw new ArgumentNullException(nameof (source));
      QuerySettings querySettings = queryOp.SpecifiedQuerySettings.WithDefaults();
      if (queryOp.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        ParallelExecutionMode parallelExecutionMode = ParallelExecutionMode.ForceParallelism;
        if ((executionMode.GetValueOrDefault() == parallelExecutionMode ? (!executionMode.HasValue ? 1 : 0) : 1) != 0)
          return ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(queryOp.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).LastOrDefault<TSource>();
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) queryOp, false, true);
    }

    [__DynamicallyInvokable]
    public static TSource LastOrDefault<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      LastQueryOperator<TSource> queryOp = predicate != null ? new LastQueryOperator<TSource>((IEnumerable<TSource>) source, predicate) : throw new ArgumentNullException(nameof (predicate));
      QuerySettings querySettings = queryOp.SpecifiedQuerySettings.WithDefaults();
      if (queryOp.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        ParallelExecutionMode parallelExecutionMode = ParallelExecutionMode.ForceParallelism;
        if ((executionMode.GetValueOrDefault() == parallelExecutionMode ? (!executionMode.HasValue ? 1 : 0) : 1) != 0)
          return ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(queryOp.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).LastOrDefault<TSource>(ExceptionAggregator.WrapFunc<TSource, bool>(predicate, querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) queryOp, false, true);
    }

    [__DynamicallyInvokable]
    public static TSource Single<TSource>(this ParallelQuery<TSource> source) => source != null ? ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) new SingleQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null), true, false) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static TSource Single<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) new SingleQueryOperator<TSource>((IEnumerable<TSource>) source, predicate), true, false) : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static TSource SingleOrDefault<TSource>(this ParallelQuery<TSource> source) => source != null ? ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) new SingleQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null), true, true) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static TSource SingleOrDefault<TSource>(
      this ParallelQuery<TSource> source,
      Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return predicate != null ? ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) new SingleQueryOperator<TSource>((IEnumerable<TSource>) source, predicate), true, true) : throw new ArgumentNullException(nameof (predicate));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> DefaultIfEmpty<TSource>(
      this ParallelQuery<TSource> source)
    {
      return ParallelEnumerable.DefaultIfEmpty<TSource>(source, default (TSource));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> DefaultIfEmpty<TSource>(
      this ParallelQuery<TSource> source,
      TSource defaultValue)
    {
      return source != null ? (ParallelQuery<TSource>) new DefaultIfEmptyQueryOperator<TSource>((IEnumerable<TSource>) source, defaultValue) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static TSource ElementAt<TSource>(this ParallelQuery<TSource> source, int index)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      TSource result;
      if (new ElementAtQueryOperator<TSource>((IEnumerable<TSource>) source, index).Aggregate(out result, false))
        return result;
      throw new ArgumentOutOfRangeException(nameof (index));
    }

    [__DynamicallyInvokable]
    public static TSource ElementAtOrDefault<TSource>(this ParallelQuery<TSource> source, int index)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      TSource result;
      return index >= 0 && new ElementAtQueryOperator<TSource>((IEnumerable<TSource>) source, index).Aggregate(out result, true) ? result : default (TSource);
    }
  }
}
