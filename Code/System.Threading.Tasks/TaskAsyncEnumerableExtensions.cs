// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskAsyncEnumerableExtensions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides a set of static methods for configuring task-related behaviors on asynchronous enumerables and disposables.</summary>
  public static class TaskAsyncEnumerableExtensions
  {
    /// <summary>Configures how awaits on the tasks returned from an async disposable are performed.</summary>
    /// <param name="source">The source async disposable.</param>
    /// <param name="continueOnCapturedContext">
    /// <see langword="true" /> to capture and marshal back to the current context; otherwise, <see langword="false" />.</param>
    /// <returns>The configured async disposable.</returns>
    public static ConfiguredAsyncDisposable ConfigureAwait(
      this IAsyncDisposable source,
      bool continueOnCapturedContext)
    {
      return new ConfiguredAsyncDisposable(source, continueOnCapturedContext);
    }

    /// <summary>Configures how awaits on the tasks returned from an async iteration are performed.</summary>
    /// <param name="source">The source enumerable to iterate.</param>
    /// <param name="continueOnCapturedContext">
    /// <see langword="true" /> to capture and marshal back to the current context; otherwise, <see langword="false" />.</param>
    /// <typeparam name="T">The type of the objects to iterate.</typeparam>
    /// <returns>The configured enumerable.</returns>
    public static ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait<T>(
      this IAsyncEnumerable<T> source,
      bool continueOnCapturedContext)
    {
      return new ConfiguredCancelableAsyncEnumerable<T>(source, continueOnCapturedContext, new CancellationToken());
    }

    /// <summary>Sets the <see cref="T:System.Threading.CancellationToken" /> to be passed to <see cref="M:System.Collections.Generic.IAsyncEnumerable`1.GetAsyncEnumerator(System.Threading.CancellationToken)" /> when iterating.</summary>
    /// <param name="source">The source enumerable to iterate.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <typeparam name="T">The type of the objects to iterate.</typeparam>
    /// <returns>The configured enumerable.</returns>
    public static ConfiguredCancelableAsyncEnumerable<T> WithCancellation<T>(
      this IAsyncEnumerable<T> source,
      CancellationToken cancellationToken)
    {
      return new ConfiguredCancelableAsyncEnumerable<T>(source, true, cancellationToken);
    }
  }
}
