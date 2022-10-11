// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.UnobservedTaskExceptionEventArgs
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides data for the event that is raised when a faulted <see cref="T:System.Threading.Tasks.Task" />'s exception goes unobserved.</summary>
  public class UnobservedTaskExceptionEventArgs : EventArgs
  {

    #nullable disable
    private readonly AggregateException m_exception;
    internal bool m_observed;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.UnobservedTaskExceptionEventArgs" /> class with the unobserved exception.</summary>
    /// <param name="exception">The Exception that has gone unobserved.</param>
    public UnobservedTaskExceptionEventArgs(AggregateException exception) => this.m_exception = exception;

    /// <summary>Marks the <see cref="P:System.Threading.Tasks.UnobservedTaskExceptionEventArgs.Exception" /> as "observed," thus preventing it from triggering exception escalation policy which, by default, terminates the process.</summary>
    public void SetObserved() => this.m_observed = true;

    /// <summary>Gets whether this exception has been marked as "observed."</summary>
    /// <returns>true if this exception has been marked as "observed"; otherwise false.</returns>
    public bool Observed => this.m_observed;

    /// <summary>The Exception that went unobserved.</summary>
    /// <returns>The Exception that went unobserved.</returns>
    public AggregateException Exception => this.m_exception;
  }
}
