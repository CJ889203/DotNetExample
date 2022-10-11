// Decompiled with JetBrains decompiler
// Type: System.Threading.HostExecutionContextManager
// Assembly: System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3EED92AD-A1EE-4F59-AFCF-58DB2345788A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Provides the functionality that allows a common language runtime host to participate in the flow, or migration, of the execution context.</summary>
  public class HostExecutionContextManager
  {

    #nullable disable
    [ThreadStatic]
    private static HostExecutionContext t_currentContext;


    #nullable enable
    /// <summary>Captures the host execution context from the current thread.</summary>
    /// <returns>A <see cref="T:System.Threading.HostExecutionContext" /> object representing the host execution context of the current thread.</returns>
    public virtual HostExecutionContext? Capture() => (HostExecutionContext) null;

    /// <summary>Sets the current host execution context to the specified host execution context.</summary>
    /// <param name="hostExecutionContext">The <see cref="T:System.Threading.HostExecutionContext" /> to be set.</param>
    /// <exception cref="T:System.InvalidOperationException">
    ///        <paramref name="hostExecutionContext" /> was not acquired through a capture operation.
    /// 
    /// -or-
    /// 
    /// <paramref name="hostExecutionContext" /> has been the argument to a previous <see cref="M:System.Threading.HostExecutionContextManager.SetHostExecutionContext(System.Threading.HostExecutionContext)" /> method call.</exception>
    /// <returns>An object for restoring the <see cref="T:System.Threading.HostExecutionContext" /> to its previous state.</returns>
    public virtual object SetHostExecutionContext(HostExecutionContext hostExecutionContext)
    {
      HostExecutionContextManager.HostExecutionContextSwitcher executionContextSwitcher = hostExecutionContext != null ? new HostExecutionContextManager.HostExecutionContextSwitcher(hostExecutionContext) : throw new InvalidOperationException(SR.HostExecutionContextManager_InvalidOperation_NotNewCaptureContext);
      HostExecutionContextManager.t_currentContext = hostExecutionContext;
      return (object) executionContextSwitcher;
    }

    /// <summary>Restores the host execution context to its prior state.</summary>
    /// <param name="previousState">The previous context state to revert to.</param>
    /// <exception cref="T:System.InvalidOperationException">
    ///        <paramref name="previousState" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="previousState" /> was not created on the current thread.
    /// 
    /// -or-
    /// 
    /// <paramref name="previousState" /> is not the last state for the <see cref="T:System.Threading.HostExecutionContext" />.</exception>
    public virtual void Revert(object previousState)
    {
      if (!(previousState is HostExecutionContextManager.HostExecutionContextSwitcher executionContextSwitcher))
        throw new InvalidOperationException(SR.HostExecutionContextManager_InvalidOperation_CannotOverrideSetWithoutRevert);
      if (HostExecutionContextManager.t_currentContext != executionContextSwitcher._currentContext || executionContextSwitcher._asyncLocal == null || !executionContextSwitcher._asyncLocal.Value)
        throw new InvalidOperationException(SR.HostExecutionContextManager_InvalidOperation_CannotUseSwitcherOtherThread);
      executionContextSwitcher._asyncLocal = (AsyncLocal<bool>) null;
      HostExecutionContextManager.t_currentContext = (HostExecutionContext) null;
    }


    #nullable disable
    private sealed class HostExecutionContextSwitcher
    {
      public readonly HostExecutionContext _currentContext;
      public AsyncLocal<bool> _asyncLocal;

      public HostExecutionContextSwitcher(HostExecutionContext currentContext)
      {
        this._currentContext = currentContext;
        this._asyncLocal = new AsyncLocal<bool>();
        this._asyncLocal.Value = true;
      }
    }
  }
}
