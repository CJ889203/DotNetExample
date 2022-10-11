// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskStatus
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Threading.Tasks
{
  /// <summary>Represents the current stage in the lifecycle of a <see cref="T:System.Threading.Tasks.Task" />.</summary>
  public enum TaskStatus
  {
    /// <summary>The task has been initialized but has not yet been scheduled.</summary>
    Created,
    /// <summary>The task is waiting to be activated and scheduled internally by the .NET infrastructure.</summary>
    WaitingForActivation,
    /// <summary>The task has been scheduled for execution but has not yet begun executing.</summary>
    WaitingToRun,
    /// <summary>The task is running but has not yet completed.</summary>
    Running,
    /// <summary>The task has finished executing and is implicitly waiting for attached child tasks to complete.</summary>
    WaitingForChildrenToComplete,
    /// <summary>The task completed execution successfully.</summary>
    RanToCompletion,
    /// <summary>The task acknowledged cancellation by throwing an OperationCanceledException with its own CancellationToken while the token was in signaled state, or the task's CancellationToken was already signaled before the task started executing. For more information, see Task Cancellation.</summary>
    Canceled,
    /// <summary>The task completed due to an unhandled exception.</summary>
    Faulted,
  }
}
