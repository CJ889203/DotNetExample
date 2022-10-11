// Decompiled with JetBrains decompiler
// Type: System.Threading.CancellationTokenRegistration
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;


#nullable enable
namespace System.Threading
{
  /// <summary>Represents a callback delegate that has been registered with a <see cref="T:System.Threading.CancellationToken" />.</summary>
  public readonly struct CancellationTokenRegistration : 
    IEquatable<CancellationTokenRegistration>,
    IDisposable,
    IAsyncDisposable
  {
    private readonly long _id;

    #nullable disable
    private readonly CancellationTokenSource.CallbackNode _node;

    internal CancellationTokenRegistration(long id, CancellationTokenSource.CallbackNode node)
    {
      this._id = id;
      this._node = node;
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.CancellationTokenRegistration" /> class.</summary>
    public void Dispose()
    {
      CancellationTokenSource.CallbackNode node = this._node;
      if (node == null || node.Registrations.Unregister(this._id, node))
        return;
      WaitForCallbackIfNecessary(this._id, node);

      static void WaitForCallbackIfNecessary(long id, CancellationTokenSource.CallbackNode node)
      {
        CancellationTokenSource source = node.Registrations.Source;
        if (!source.IsCancellationRequested || source.IsCancellationCompleted || node.Registrations.ThreadIDExecutingCallbacks == Environment.CurrentManagedThreadId)
          return;
        node.Registrations.WaitForCallbackToComplete(id);
      }
    }

    /// <summary>Disposes of the registration and unregisters the target callback from the associated
    /// <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public ValueTask DisposeAsync()
    {
      CancellationTokenSource.CallbackNode node = this._node;
      return node == null || node.Registrations.Unregister(this._id, node) ? new ValueTask() : WaitForCallbackIfNecessaryAsync(this._id, node);

      static ValueTask WaitForCallbackIfNecessaryAsync(
        long id,
        CancellationTokenSource.CallbackNode node)
      {
        CancellationTokenSource source = node.Registrations.Source;
        return source.IsCancellationRequested && !source.IsCancellationCompleted && node.Registrations.ThreadIDExecutingCallbacks != Environment.CurrentManagedThreadId ? node.Registrations.WaitForCallbackToCompleteAsync(id) : new ValueTask();
      }
    }

    /// <summary>Gets the <see cref="T:System.Threading.CancellationToken" /> with which this registration is associated.</summary>
    /// <returns>The cancellation token with which this registration is associated, or a default token if the
    /// registration isn't associated with a token.</returns>
    public CancellationToken Token
    {
      get
      {
        CancellationTokenSource.CallbackNode node = this._node;
        return node == null ? new CancellationToken() : new CancellationToken(node.Registrations.Source);
      }
    }

    /// <summary>Disposes of the registration and unregisters the target callback from the associated
    /// <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <returns>
    /// <see langword="true" /> if the method succeeds; otherwise, <see langword="false" />.</returns>
    public bool Unregister()
    {
      CancellationTokenSource.CallbackNode node = this._node;
      return node != null && node.Registrations.Unregister(this._id, node);
    }

    /// <summary>Determines whether two <see cref="T:System.Threading.CancellationTokenRegistration" /> instances are equal.</summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    /// <see langword="true" /> if the instances are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(
      CancellationTokenRegistration left,
      CancellationTokenRegistration right)
    {
      return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="T:System.Threading.CancellationTokenRegistration" /> instances are not equal.</summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>
    /// <see langword="true" /> if the instances are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(
      CancellationTokenRegistration left,
      CancellationTokenRegistration right)
    {
      return !left.Equals(right);
    }


    #nullable enable
    /// <summary>Determines whether the current <see cref="T:System.Threading.CancellationTokenRegistration" /> instance is equal to the specified <see cref="T:System.Threading.CancellationTokenRegistration" />.</summary>
    /// <param name="obj">The other object to which to compare this instance.</param>
    /// <returns>
    ///        <see langword="true" /> if both this and <paramref name="obj" /> are equal. False, otherwise.
    /// 
    /// Two <see cref="T:System.Threading.CancellationTokenRegistration" /> instances are equal if they both refer to the output of a single call to the same Register method of a <see cref="T:System.Threading.CancellationToken" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is CancellationTokenRegistration other && this.Equals(other);

    /// <summary>Determines whether the current <see cref="T:System.Threading.CancellationTokenRegistration" /> instance is equal to the specified <see cref="T:System.Threading.CancellationTokenRegistration" />.</summary>
    /// <param name="other">The other <see cref="T:System.Threading.CancellationTokenRegistration" /> to which to compare this instance.</param>
    /// <returns>
    ///        <see langword="true" /> if both this and <paramref name="other" /> are equal. False, otherwise.
    /// 
    /// Two <see cref="T:System.Threading.CancellationTokenRegistration" /> instances are equal if they both refer to the output of a single call to the same Register method of a <see cref="T:System.Threading.CancellationToken" />.</returns>
    public bool Equals(CancellationTokenRegistration other) => this._node == other._node && this._id == other._id;

    /// <summary>Serves as a hash function for a <see cref="T:System.Threading.CancellationTokenRegistration" />.</summary>
    /// <returns>A hash code for the current <see cref="T:System.Threading.CancellationTokenRegistration" /> instance.</returns>
    public override int GetHashCode() => this._node == null ? this._id.GetHashCode() : this._node.GetHashCode() ^ this._id.GetHashCode();
  }
}
