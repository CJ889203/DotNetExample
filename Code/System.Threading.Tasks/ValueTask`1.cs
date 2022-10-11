// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.ValueTask`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Sources;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides a value type that wraps a <see cref="T:System.Threading.Tasks.Task`1" /> and a <typeparamref name="TResult" />, only one of which is used.</summary>
  /// <typeparam name="TResult">The result.</typeparam>
  [AsyncMethodBuilder(typeof (AsyncValueTaskMethodBuilder<>))]
  [StructLayout(LayoutKind.Auto)]
  public readonly struct ValueTask<TResult> : IEquatable<ValueTask<TResult>>
  {

    #nullable disable
    private static volatile Task<TResult> s_canceledTask;
    internal readonly object _obj;
    internal readonly TResult _result;
    internal readonly short _token;
    internal readonly bool _continueOnCapturedContext;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ValueTask`1" /> class using the supplied result of a successful operation.</summary>
    /// <param name="result">The result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask(TResult result)
    {
      this._result = result;
      this._obj = (object) null;
      this._continueOnCapturedContext = true;
      this._token = (short) 0;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ValueTask`1" /> class using the supplied task that represents the operation.</summary>
    /// <param name="task">The task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is <see langword="null" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask(Task<TResult> task)
    {
      if (task == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.task);
      this._obj = (object) task;
      this._result = default (TResult);
      this._continueOnCapturedContext = true;
      this._token = (short) 0;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ValueTask`1" /> class with a <see cref="T:System.Threading.Tasks.Sources.IValueTaskSource`1" /> object that represents the operation.</summary>
    /// <param name="source">The source.</param>
    /// <param name="token">An opaque value that is passed to the <see cref="T:System.Threading.Tasks.Sources.IValueTaskSource" />.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask(IValueTaskSource<TResult> source, short token)
    {
      if (source == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
      this._obj = (object) source;
      this._token = token;
      this._result = default (TResult);
      this._continueOnCapturedContext = true;
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ValueTask(object obj, TResult result, short token, bool continueOnCapturedContext)
    {
      this._obj = obj;
      this._result = result;
      this._token = token;
      this._continueOnCapturedContext = continueOnCapturedContext;
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hash code for the current object.</returns>
    public override int GetHashCode()
    {
      if (this._obj != null)
        return this._obj.GetHashCode();
      return (object) this._result == null ? 0 : this._result.GetHashCode();
    }


    #nullable enable
    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ValueTask<TResult> other && this.Equals(other);

    /// <summary>Determines whether the specified <see cref="T:System.Threading.Tasks.ValueTask`1" /> object is equal to the current <see cref="T:System.Threading.Tasks.ValueTask`1" /> object.</summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public bool Equals(ValueTask<TResult> other)
    {
      if (this._obj == null && other._obj == null)
        return EqualityComparer<TResult>.Default.Equals(this._result, other._result);
      return this._obj == other._obj && (int) this._token == (int) other._token;
    }

    /// <summary>Compares two values for equality.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Threading.Tasks.ValueTask`1" /> values are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right) => left.Equals(right);

    /// <summary>Determines whether two <see cref="T:System.Threading.Tasks.ValueTask`1" /> values are unequal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Threading.Tasks.ValueTask`1" /> values are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right) => !left.Equals(right);

    /// <summary>Retrieves a <see cref="T:System.Threading.Tasks.Task`1" /> object that represents this <see cref="T:System.Threading.Tasks.ValueTask`1" />.</summary>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task`1" /> object that is wrapped in this <see cref="T:System.Threading.Tasks.ValueTask`1" /> if one exists, or a new <see cref="T:System.Threading.Tasks.Task`1" /> object that represents the result.</returns>
    public Task<TResult> AsTask()
    {
      object obj = this._obj;
      if (obj == null)
        return Task.FromResult<TResult>(this._result);
      return obj is Task<TResult> task ? task : this.GetTaskForValueTaskSource(Unsafe.As<IValueTaskSource<TResult>>(obj));
    }

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.ValueTask`1" /> that may be used at any point in the future.</summary>
    /// <returns>A task object for future use.</returns>
    public ValueTask<TResult> Preserve() => this._obj != null ? new ValueTask<TResult>(this.AsTask()) : this;


    #nullable disable
    private Task<TResult> GetTaskForValueTaskSource(IValueTaskSource<TResult> t)
    {
      ValueTaskSourceStatus status = t.GetStatus(this._token);
      if (status == ValueTaskSourceStatus.Pending)
        return (Task<TResult>) new ValueTask<TResult>.ValueTaskSourceAsTask(t, this._token);
      try
      {
        return Task.FromResult<TResult>(t.GetResult(this._token));
      }
      catch (Exception ex)
      {
        if (status != ValueTaskSourceStatus.Canceled)
          return Task.FromException<TResult>(ex);
        if (!(ex is OperationCanceledException cancellationException))
          return ValueTask<TResult>.s_canceledTask ?? (ValueTask<TResult>.s_canceledTask = Task.FromCanceled<TResult>(new CancellationToken(true)));
        Task<TResult> forValueTaskSource = new Task<TResult>();
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
        return obj is Task<TResult> task ? task.IsCompleted : Unsafe.As<IValueTaskSource<TResult>>(obj).GetStatus(this._token) != 0;
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
        return obj is Task<TResult> task ? task.IsCompletedSuccessfully : Unsafe.As<IValueTaskSource<TResult>>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Succeeded;
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
        return obj is Task<TResult> task ? task.IsFaulted : Unsafe.As<IValueTaskSource<TResult>>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Faulted;
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
        return obj is Task<TResult> task ? task.IsCanceled : Unsafe.As<IValueTaskSource<TResult>>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Canceled;
      }
    }


    #nullable enable
    /// <summary>Gets the result.</summary>
    /// <returns>The result.</returns>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TResult Result
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get
      {
        object obj = this._obj;
        if (obj == null)
          return this._result;
        if (!(obj is Task<TResult> task))
          return Unsafe.As<IValueTaskSource<TResult>>(obj).GetResult(this._token);
        TaskAwaiter.ValidateEnd((Task) task);
        return task.ResultOnSuccess;
      }
    }

    /// <summary>Creates an awaiter for this value.</summary>
    /// <returns>The awaiter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTaskAwaiter<TResult> GetAwaiter() => new ValueTaskAwaiter<TResult>(in this);

    /// <summary>Configures an awaiter for this value.</summary>
    /// <param name="continueOnCapturedContext">
    /// <see langword="true" /> to attempt to marshal the continuation back to the captured context; otherwise, <see langword="false" />.</param>
    /// <returns>The configured awaiter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(
      bool continueOnCapturedContext)
    {
      ValueTask<TResult> valueTask = new ValueTask<TResult>(this._obj, this._result, this._token, continueOnCapturedContext);
      return new ConfiguredValueTaskAwaitable<TResult>(in valueTask);
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string? ToString()
    {
      if (this.IsCompletedSuccessfully)
      {
        Debugger.NotifyOfCrossThreadDependency();
        TResult result = this.Result;
        if ((object) result != null)
          return result.ToString();
      }
      return string.Empty;
    }


    #nullable disable
    private sealed class ValueTaskSourceAsTask : Task<TResult>
    {
      private static readonly Action<object> s_completionAction = (Action<object>) (state =>
      {
        if (state is ValueTask<TResult>.ValueTaskSourceAsTask taskSourceAsTask2)
        {
          IValueTaskSource<TResult> source = taskSourceAsTask2._source;
          if (source != null)
          {
            taskSourceAsTask2._source = (IValueTaskSource<TResult>) null;
            ValueTaskSourceStatus status = source.GetStatus(taskSourceAsTask2._token);
            try
            {
              taskSourceAsTask2.TrySetResult(source.GetResult(taskSourceAsTask2._token));
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
      private IValueTaskSource<TResult> _source;
      private readonly short _token;

      public ValueTaskSourceAsTask(IValueTaskSource<TResult> source, short token)
      {
        this._source = source;
        this._token = token;
        source.OnCompleted(ValueTask<TResult>.ValueTaskSourceAsTask.s_completionAction, (object) this, token, ValueTaskSourceOnCompletedFlags.None);
      }
    }
  }
}
