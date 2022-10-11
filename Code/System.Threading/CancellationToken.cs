// Decompiled with JetBrains decompiler
// Type: System.Threading.CancellationToken
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Threading
{
  /// <summary>Propagates notification that operations should be canceled.</summary>
  [DebuggerDisplay("IsCancellationRequested = {IsCancellationRequested}")]
  public readonly struct CancellationToken
  {

    #nullable disable
    private readonly CancellationTokenSource _source;

    /// <summary>Returns an empty <see cref="T:System.Threading.CancellationToken" /> value.</summary>
    /// <returns>An empty cancellation token.</returns>
    public static CancellationToken None => new CancellationToken();

    /// <summary>Gets whether cancellation has been requested for this token.</summary>
    /// <returns>
    /// <see langword="true" /> if cancellation has been requested for this token; otherwise, <see langword="false" />.</returns>
    public bool IsCancellationRequested => this._source != null && this._source.IsCancellationRequested;

    /// <summary>Gets whether this token is capable of being in the canceled state.</summary>
    /// <returns>
    /// <see langword="true" /> if this token is capable of being in the canceled state; otherwise, <see langword="false" />.</returns>
    public bool CanBeCanceled => this._source != null;


    #nullable enable
    /// <summary>Gets a <see cref="T:System.Threading.WaitHandle" /> that is signaled when the token is canceled.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <returns>A <see cref="T:System.Threading.WaitHandle" /> that is signaled when the token is canceled.</returns>
    public WaitHandle WaitHandle => (this._source ?? CancellationTokenSource.s_neverCanceledSource).WaitHandle;


    #nullable disable
    internal CancellationToken(CancellationTokenSource source) => this._source = source;

    /// <summary>Initializes the <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="canceled">The canceled state for the token.</param>
    public CancellationToken(bool canceled)
      : this(canceled ? CancellationTokenSource.s_canceledSource : (CancellationTokenSource) null)
    {
    }


    #nullable enable
    /// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
    /// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
    /// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is null.</exception>
    /// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
    public CancellationTokenRegistration Register(Action callback) => this.Register(callback, false);

    /// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
    /// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
    /// <param name="useSynchronizationContext">A value that indicates whether to capture the current <see cref="T:System.Threading.SynchronizationContext" /> and use it when invoking the <paramref name="callback" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is null.</exception>
    /// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
    public CancellationTokenRegistration Register(
      Action callback,
      bool useSynchronizationContext)
    {
      Action state = callback;
      if (state == null)
        throw new ArgumentNullException(nameof (callback));
      return this.Register((Delegate) (obj => ((Action) obj)()), (object) state, useSynchronizationContext, true);
    }

    /// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
    /// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
    /// <param name="state">The state to pass to the <paramref name="callback" /> when the delegate is invoked. This may be null.</param>
    /// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is null.</exception>
    /// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
    public CancellationTokenRegistration Register(
      Action<object?> callback,
      object? state)
    {
      return this.Register((Delegate) callback, state, false, true);
    }

    /// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken">CancellationToken</see> is canceled.</summary>
    /// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> is canceled.</param>
    /// <param name="state">The state to pass to the <paramref name="callback" /> when the delegate is invoked.  This may be <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is <see langword="null" />.</exception>
    /// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
    public CancellationTokenRegistration Register(
      Action<object?, CancellationToken> callback,
      object? state)
    {
      return this.Register((Delegate) callback, state, false, true);
    }

    /// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
    /// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
    /// <param name="state">The state to pass to the <paramref name="callback" /> when the delegate is invoked. This may be null.</param>
    /// <param name="useSynchronizationContext">A Boolean value that indicates whether to capture the current <see cref="T:System.Threading.SynchronizationContext" /> and use it when invoking the <paramref name="callback" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is null.</exception>
    /// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
    public CancellationTokenRegistration Register(
      Action<object?> callback,
      object? state,
      bool useSynchronizationContext)
    {
      return this.Register((Delegate) callback, state, useSynchronizationContext, true);
    }

    /// <summary>Registers a delegate that is called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
    /// <param name="callback">The delegate to execute when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
    /// <param name="state">The state to pass to the <paramref name="callback" /> when the delegate is invoked.  This may be <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is null.</exception>
    /// <returns>An object that can
    /// be used to unregister the callback.</returns>
    public CancellationTokenRegistration UnsafeRegister(
      Action<object?> callback,
      object? state)
    {
      return this.Register((Delegate) callback, state, false, false);
    }

    /// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken">CancellationToken</see> is canceled.</summary>
    /// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> is canceled.</param>
    /// <param name="state">The state to pass to the <paramref name="callback" /> when the delegate is invoked.  This may be <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is <see langword="null" />.</exception>
    /// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
    public CancellationTokenRegistration UnsafeRegister(
      Action<object?, CancellationToken> callback,
      object? state)
    {
      return this.Register((Delegate) callback, state, false, false);
    }


    #nullable disable
    private CancellationTokenRegistration Register(
      Delegate callback,
      object state,
      bool useSynchronizationContext,
      bool useExecutionContext)
    {
      if ((object) callback == null)
        throw new ArgumentNullException(nameof (callback));
      CancellationTokenSource source = this._source;
      return source == null ? new CancellationTokenRegistration() : source.Register(callback, state, useSynchronizationContext ? SynchronizationContext.Current : (SynchronizationContext) null, useExecutionContext ? ExecutionContext.Capture() : (ExecutionContext) null);
    }

    /// <summary>Determines whether the current <see cref="T:System.Threading.CancellationToken" /> instance is equal to the specified token.</summary>
    /// <param name="other">The other <see cref="T:System.Threading.CancellationToken" /> to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the instances are equal; otherwise, <see langword="false" />. See the Remarks section for more information.</returns>
    public bool Equals(CancellationToken other) => this._source == other._source;


    #nullable enable
    /// <summary>Determines whether the current <see cref="T:System.Threading.CancellationToken" /> instance is equal to the specified <see cref="T:System.Object" />.</summary>
    /// <param name="other">The other object to compare with this instance.</param>
    /// <exception cref="T:System.ObjectDisposedException">An associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="other" /> is a <see cref="T:System.Threading.CancellationToken" /> and if the two instances are equal; otherwise, <see langword="false" />. See the Remarks section for more information.</returns>
    public override bool Equals([NotNullWhen(true)] object? other) => other is CancellationToken other1 && this.Equals(other1);

    /// <summary>Serves as a hash function for a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <returns>A hash code for the current <see cref="T:System.Threading.CancellationToken" /> instance.</returns>
    public override int GetHashCode() => (this._source ?? CancellationTokenSource.s_neverCanceledSource).GetHashCode();

    /// <summary>Determines whether two <see cref="T:System.Threading.CancellationToken" /> instances are equal.</summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <exception cref="T:System.ObjectDisposedException">An associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the instances are equal; otherwise, <see langword="false" /> See the Remarks section for more information.</returns>
    public static bool operator ==(CancellationToken left, CancellationToken right) => left.Equals(right);

    /// <summary>Determines whether two <see cref="T:System.Threading.CancellationToken" /> instances are not equal.</summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <exception cref="T:System.ObjectDisposedException">An associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the instances are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(CancellationToken left, CancellationToken right) => !left.Equals(right);

    /// <summary>Throws a <see cref="T:System.OperationCanceledException" /> if this token has had cancellation requested.</summary>
    /// <exception cref="T:System.OperationCanceledException">The token has had cancellation requested.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    public void ThrowIfCancellationRequested()
    {
      if (!this.IsCancellationRequested)
        return;
      this.ThrowOperationCanceledException();
    }

    [DoesNotReturn]
    private void ThrowOperationCanceledException() => throw new OperationCanceledException(SR.OperationCanceled, this);
  }
}
