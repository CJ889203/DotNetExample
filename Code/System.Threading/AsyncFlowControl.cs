// Decompiled with JetBrains decompiler
// Type: System.Threading.AsyncFlowControl
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Threading
{
  /// <summary>Provides the functionality to restore the migration, or flow, of the execution context between threads.</summary>
  public struct AsyncFlowControl : IEquatable<AsyncFlowControl>, IDisposable
  {

    #nullable disable
    private Thread _thread;

    internal void Initialize(Thread currentThread) => this._thread = currentThread;

    /// <summary>Restores the flow of the execution context between threads.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Threading.AsyncFlowControl" /> structure is not used on the thread where it was created.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.AsyncFlowControl" /> structure has already been used to call <see cref="M:System.Threading.AsyncFlowControl.Dispose" /> or <see cref="M:System.Threading.AsyncFlowControl.Undo" />.</exception>
    public void Undo()
    {
      if (this._thread == null)
        throw new InvalidOperationException(SR.InvalidOperation_CannotUseAFCMultiple);
      if (Thread.CurrentThread != this._thread)
        throw new InvalidOperationException(SR.InvalidOperation_CannotUseAFCOtherThread);
      if (!ExecutionContext.IsFlowSuppressed())
        throw new InvalidOperationException(SR.InvalidOperation_AsyncFlowCtrlCtxMismatch);
      this._thread = (Thread) null;
      ExecutionContext.RestoreFlow();
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.AsyncFlowControl" /> class.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Threading.AsyncFlowControl" /> structure is not used on the thread where it was created.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.AsyncFlowControl" /> structure has already been used to call <see cref="M:System.Threading.AsyncFlowControl.Dispose" /> or <see cref="M:System.Threading.AsyncFlowControl.Undo" />.</exception>
    public void Dispose() => this.Undo();


    #nullable enable
    /// <summary>Determines whether the specified object is equal to the current <see cref="T:System.Threading.AsyncFlowControl" /> structure.</summary>
    /// <param name="obj">An object to compare with the current structure.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an <see cref="T:System.Threading.AsyncFlowControl" /> structure and is equal to the current <see cref="T:System.Threading.AsyncFlowControl" /> structure; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AsyncFlowControl asyncFlowControl && this.Equals(asyncFlowControl);

    /// <summary>Determines whether the specified <see cref="T:System.Threading.AsyncFlowControl" /> structure is equal to the current <see cref="T:System.Threading.AsyncFlowControl" /> structure.</summary>
    /// <param name="obj">An <see cref="T:System.Threading.AsyncFlowControl" /> structure to compare with the current structure.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is equal to the current <see cref="T:System.Threading.AsyncFlowControl" /> structure; otherwise, <see langword="false" />.</returns>
    public bool Equals(AsyncFlowControl obj) => this._thread == obj._thread;

    /// <summary>Gets a hash code for the current <see cref="T:System.Threading.AsyncFlowControl" /> structure.</summary>
    /// <returns>A hash code for the current <see cref="T:System.Threading.AsyncFlowControl" /> structure.</returns>
    public override int GetHashCode()
    {
      Thread thread = this._thread;
      return thread == null ? 0 : thread.GetHashCode();
    }

    /// <summary>Compares two <see cref="T:System.Threading.AsyncFlowControl" /> structures to determine whether they are equal.</summary>
    /// <param name="a">An <see cref="T:System.Threading.AsyncFlowControl" /> structure.</param>
    /// <param name="b">An <see cref="T:System.Threading.AsyncFlowControl" /> structure.</param>
    /// <returns>
    /// <see langword="true" /> if the two structures are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(AsyncFlowControl a, AsyncFlowControl b) => a.Equals(b);

    /// <summary>Compares two <see cref="T:System.Threading.AsyncFlowControl" /> structures to determine whether they are not equal.</summary>
    /// <param name="a">An <see cref="T:System.Threading.AsyncFlowControl" /> structure.</param>
    /// <param name="b">An <see cref="T:System.Threading.AsyncFlowControl" /> structure.</param>
    /// <returns>
    /// <see langword="true" /> if the structures are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(AsyncFlowControl a, AsyncFlowControl b) => !(a == b);
  }
}
