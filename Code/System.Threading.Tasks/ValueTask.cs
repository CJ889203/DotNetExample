// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.ValueTask
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Sources;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides an awaitable result of an asynchronous operation.</summary>
  [AsyncMethodBuilder(typeof (AsyncValueTaskMethodBuilder))]
  [StructLayout(LayoutKind.Auto)]
  public readonly struct ValueTask : IEquatable<ValueTask>
  {

    #nullable disable
    private static volatile Task s_canceledTask;
    internal readonly object _obj;
    internal readonly short _token;
    internal readonly bool _continueOnCapturedContext;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ValueTask" /> class using the supplied task that represents the operation.</summary>
    /// <param name="task">The task that represents the operation.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask(Task task)
    {
      if (task == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.task);
      this._obj = (object) task;
      this._continueOnCapturedContext = true;
      this._token = (short) 0;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ValueTask" /> class using the supplied <see cref="T:System.Threading.Tasks.Sources.IValueTaskSource" /> object that represents the operation.</summary>
    /// <param name="source">An object that represents the operation.</param>
    /// <param name="token">An opaque value that is passed through to the <see cref="T:System.Threading.Tasks.Sources.IValueTaskSource" />.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask(IValueTaskSource source, short token)
    {
      if (source == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
      this._obj = (object) source;
      this._token = token;
      this._continueOnCapturedContext = true;
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ValueTask(object obj, short token, bool continueOnCapturedContext)
    {
      this._obj = obj;
      this._token = token;
      this._continueOnCapturedContext = continueOnCapturedContext;
    }

    /// <summary>Gets a task that has already completed successfully.</summary>
    public static ValueTask CompletedTask => new ValueTask();


    #nullable enable
    /// <summary>Creates a <see cref="T:System.Threading.Tasks.ValueTask`1" /> that's completed successfully with the specified result.</summary>
    /// <param name="result">The result to store into the completed task.</param>
    /// <typeparam name="TResult">The type of the result returned by the task.</typeparam>
    /// <returns>The successfully completed task.</returns>
    public static ValueTask<TResult> FromResult<TResult>(TResult result) => new ValueTask<TResult>(result);

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.ValueTask" /> that has completed due to cancellation with the specified cancellation token.</summary>
    /// <param name="cancellationToken">The cancellation token with which to complete the task.</param>
    /// <returns>The canceled task.</returns>
    public static ValueTask FromCanceled(CancellationToken cancellationToken) => new ValueTask(Task.FromCanceled(cancellationToken));

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.ValueTask`1" /> that has completed due to cancellation with the specified cancellation token.</summary>
    /// <param name="cancellationToken">The cancellation token with which to complete the task.</param>
    /// <typeparam name="TResult">The type of the result of the returned task.</typeparam>
    /// <returns>The canceled task.</returns>
    public static ValueTask<TResult> FromCanceled<TResult>(
      CancellationToken cancellationToken)
    {
      return new ValueTask<TResult>(Task.FromCanceled<TResult>(cancellationToken));
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.ValueTask" /> that has completed with the specified exception.</summary>
    /// <param name="exception">The exception with which to complete the task.</param>
    /// <returns>The faulted task.</returns>
    public static ValueTask FromException(Exception exception) => new ValueTask(Task.FromException(exception));

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.ValueTask`1" /> that has completed with the specified exception.</summary>
    /// <param name="exception">The exception with which to complete the task.</param>
    /// <typeparam name="TResult">The type of the result of the returned task.</typeparam>
    /// <returns>The faulted task.</returns>
    public static ValueTask<TResult> FromException<TResult>(Exception exception) => new ValueTask<TResult>(Task.FromException<TResult>(exception));

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hash code for the current object.</returns>
    public override int GetHashCode()
    {
      object obj = this._obj;
      return obj == null ? 0 : obj.GetHashCode();
    }

    /// <summary>Determines whether the specified object is equal to the current <see cref="T:System.Threading.Tasks.ValueTask" /> instance.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ValueTask other && this.Equals(other);

    /// <summary>Determines whether the specified <see cref="T:System.Threading.Tasks.ValueTask" /> object is equal to the current <see cref="T:System.Threading.Tasks.ValueTask" /> object.</summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public bool Equals(ValueTask other) => this._obj == other._obj && (int) this._token == (int) other._token;

    /// <summary>Compares two <see cref="T:System.Threading.Tasks.ValueTask" /> values for equality.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Threading.Tasks.ValueTask" /> values are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(ValueTask left, ValueTask right) => left.Equals(right);

    /// <summary>Determines whether two <see cref="T:System.Threading.Tasks.ValueTask" /> values are unequal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Threading.Tasks.ValueTask" /> values are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(ValueTask left, ValueTask right) => !left.Equals(right);

    /// <summary>Retrieves a <see cref="T:System.Threading.Tasks.Task" /> object that represents this <see cref="T:System.Threading.Tasks.ValueTask" />.</summary>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> object that is wrapped in this <see cref="T:System.Threading.Tasks.ValueTask" /> if one exists, or a new <see cref="T:System.Threading.Tasks.Task" /> object that represents the result.</returns>
    public Task AsTask()
    {
      object obj = this._obj;
      if (obj == null)
        return Task.CompletedTask;
      return obj is Task task ? task : this.GetTaskForValueTaskSource(Unsafe.As<IValueTaskSource>(obj));
    }

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.ValueTask" /> that may be used at any point in the future.</summary>
    /// <returns>The preserved <see cref="T:System.Threading.Tasks.ValueTask" />.</returns>
    public ValueTask Preserve() => this._obj != null ? new ValueTask(this.AsTask()) : this;


    #nullable disable
    private Task GetTaskForValueTaskSource(IValueTaskSource t)
    {
      ValueTaskSourceStatus status = t.GetStatus(this._token);
      if (status == ValueTaskSourceStatus.Pending)
        return (Task) new ValueTask.ValueTaskSourceAsTask(t, this._token);
      try
      {
        t.GetResult(this._token);
        return Task.CompletedTask;
      }
      catch (Exception ex)
      {
        if (status != ValueTaskSourceStatus.Canceled)
          return Task.FromException(ex);
        if (!(ex is OperationCanceledException cancellationException))
          return ValueTask.s_canceledTask ?? (ValueTask.s_canceledTask = Task.FromCanceled(new CancellationToken(true)));
        Task forValueTaskSource = new Task();
        forValueTaskSource.TrySetCanceled(cancellationException.CancellationToken, (object) cancellationException);
        return forValueTaskSource;
      }
    }

    /// <summary>Gets a value that indicates whether this object represents a completed operation.</summary>
    /// <returns>
    /// <see langword="true" /> if this object represents a completed operation; otherwise, <see langword="false" />.</returns>
    public bool IsCompleted
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get
      {
        object obj = this._obj;
        if (obj == null)
          return true;
        return obj is Task task ? task.IsCompleted : Unsafe.As<IValueTaskSource>(obj).GetStatus(this._token) != 0;
      }
    }

    /// <summary>Gets a value that indicates whether this object represents a successfully completed operation.</summary>
    /// <returns>
    /// <see langword="true" /> if this object represents a successfully completed operation; otherwise, <see langword="false" />.</returns>
    public bool IsCompletedSuccessfully
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get
      {
        object obj = this._obj;
        if (obj == null)
          return true;
        return obj is Task task ? task.IsCompletedSuccessfully : Unsafe.As<IValueTaskSource>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Succeeded;
      }
    }

    /// <summary>Gets a value that indicates whether this object represents a failed operation.</summary>
    /// <returns>
    /// <see langword="true" /> if this object represents a failed operation; otherwise, <see langword="false" />.</returns>
    public bool IsFaulted
    {
      get
      {
        object obj = this._obj;
        if (obj == null)
          return false;
        return obj is Task task ? task.IsFaulted : Unsafe.As<IValueTaskSource>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Faulted;
      }
    }

    /// <summary>Gets a value that indicates whether this object represents a canceled operation.</summary>
    /// <returns>
    /// <see langword="true" /> if this object represents a canceled operation; otherwise, <see langword="false" />.</returns>
    public bool IsCanceled
    {
      get
      {
        object obj = this._obj;
        if (obj == null)
          return false;
        return obj is Task task ? task.IsCanceled : Unsafe.As<IValueTaskSource>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Canceled;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void ThrowIfCompletedUnsuccessfully()
    {
      object obj = this._obj;
      if (obj == null)
        return;
      if (obj is Task task)
        TaskAwaiter.ValidateEnd(task);
      else
        Unsafe.As<IValueTaskSource>(obj).GetResult(this._token);
    }

    /// <summary>Creates an awaiter for this value.</summary>
    /// <returns>The awaiter.</returns>
    public ValueTaskAwaiter GetAwaiter() => new ValueTaskAwaiter(in this);

    /// <summary>Configures an awaiter for this value.</summary>
    /// <param name="continueOnCapturedContext">
    /// <see langword="true" /> to attempt to marshal the continuation back to the captured context; otherwise, <see langword="false" />.</param>
    /// <returns>The configured awaiter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConfiguredValueTaskAwaitable ConfigureAwait(
      bool continueOnCapturedContext)
    {
      ValueTask valueTask = new ValueTask(this._obj, this._token, continueOnCapturedContext);
      return new ConfiguredValueTaskAwaitable(in valueTask);
    }

    private sealed class ValueTaskSourceAsTask : Task
    {
      private static readonly Action<object> s_completionAction = (Action<object>) (state =>
      {
        if (state is ValueTask.ValueTaskSourceAsTask taskSourceAsTask2)
        {
          IValueTaskSource source = taskSourceAsTask2._source;
          if (source != null)
          {
            taskSourceAsTask2._source = (IValueTaskSource) null;
            ValueTaskSourceStatus status = source.GetStatus(taskSourceAsTask2._token);
            try
            {
              source.GetResult(taskSourceAsTask2._token);
              taskSourceAsTask2.TrySetResult();
              return;
            }
            catch (Exception ex)
            {
              if (status == ValueTaskSourceStatus.Canceled)
              {
                if (ex is OperationCanceledException cancellationException2)
                {
                  taskSourceAsTask2.TrySetCanceled(cancellationException2.CancellationToken, (object) cancellationException2);
                  return;
                }
                taskSourceAsTask2.TrySetCanceled(new CancellationToken(true));
                return;
              }
              taskSourceAsTask2.TrySetException((object) ex);
              return;
            }
          }
        }
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.state);
      });
      private IValueTaskSource _source;
      private readonly short _token;

      internal ValueTaskSourceAsTask(IValueTaskSource source, short token)
      {
        this._token = token;
        this._source = source;
        source.OnCompleted(ValueTask.ValueTaskSourceAsTask.s_completionAction, (object) this, token, ValueTaskSourceOnCompletedFlags.None);
      }
    }
  }
}
