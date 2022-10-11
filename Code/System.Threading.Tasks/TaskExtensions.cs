// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskExtensions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides a set of static (Shared in Visual Basic) methods for working with specific kinds of <see cref="T:System.Threading.Tasks.Task" /> instances.</summary>
  public static class TaskExtensions
  {
    /// <summary>Creates a proxy <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation of a <see cref="M:System.Threading.Tasks.TaskScheduler.TryExecuteTaskInline(System.Threading.Tasks.Task,System.Boolean)" />.</summary>
    /// <param name="task">The <see langword="Task&lt;Task&gt;" /> (C#) or <see langword="Task (Of Task)" /> (Visual Basic) to unwrap.</param>
    /// <exception cref="T:System.ArgumentNullException">The exception that is thrown if the <paramref name="task" /> argument is null.</exception>
    /// <returns>A Task that represents the asynchronous operation of the provided <see langword="System.Threading.Tasks.Task(Of Task)" />.</returns>
    public static Task Unwrap(this Task<Task> task)
    {
      if (task == null)
        throw new ArgumentNullException(nameof (task));
      return task.IsCompletedSuccessfully ? task.Result ?? Task.FromCanceled(new CancellationToken(true)) : (Task) Task.CreateUnwrapPromise<VoidTaskResult>((Task) task, false);
    }

    /// <summary>Creates a proxy <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation of a <see langword="Task&lt;Task&lt;T&gt;&gt;" /> (C#) or <see langword="Task (Of Task(Of T))" /> (Visual Basic).</summary>
    /// <param name="task">The <see langword="Task&lt;Task&lt;T&gt;&gt;" /> (C#) or <see langword="Task (Of Task(Of T))" /> (Visual Basic) to unwrap.</param>
    /// <typeparam name="TResult">The type of the task's result.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The exception that is thrown if the <paramref name="task" /> argument is null.</exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation of the provided <see langword="Task&lt;Task&lt;T&gt;&gt;" /> (C#) or <see langword="Task (Of Task(Of T))" /> (Visual Basic).</returns>
    public static Task<TResult> Unwrap<TResult>(this Task<Task<TResult>> task)
    {
      if (task == null)
        throw new ArgumentNullException(nameof (task));
      return task.IsCompletedSuccessfully ? task.Result ?? Task.FromCanceled<TResult>(new CancellationToken(true)) : Task.CreateUnwrapPromise<TResult>((Task) task, false);
    }
  }
}
