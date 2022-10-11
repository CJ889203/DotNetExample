// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskCanceledException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Represents an exception used to communicate task cancellation.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class TaskCanceledException : OperationCanceledException
  {

    #nullable disable
    [NonSerialized]
    private readonly Task _canceledTask;

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> class with a system-supplied message that describes the error.</summary>
    public TaskCanceledException()
      : base(SR.TaskCanceledException_ctor_DefaultMessage)
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> class with a specified message that describes the error.</summary>
    /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    public TaskCanceledException(string? message)
      : base(message)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public TaskCanceledException(string? message, Exception? innerException)
      : base(message, innerException)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> class with a specified error message, a reference to the inner exception that is the cause of this exception, and the <see cref="T:System.Threading.CancellationToken" /> that triggered the cancellation.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <param name="token">The cancellation token that triggered the cancellation.</param>
    public TaskCanceledException(string? message, Exception? innerException, CancellationToken token)
      : base(message, innerException, token)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> class with a reference to the <see cref="T:System.Threading.Tasks.Task" /> that has been canceled.</summary>
    /// <param name="task">A task that has been canceled.</param>
    public TaskCanceledException(Task? task)
      : base(SR.TaskCanceledException_ctor_DefaultMessage, task != null ? task.CancellationToken : CancellationToken.None)
    {
      this._canceledTask = task;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> class with serialized data.</summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected TaskCanceledException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    /// <summary>Gets the task associated with this exception.</summary>
    /// <returns>A reference to the <see cref="T:System.Threading.Tasks.Task" /> that is associated with this exception.</returns>
    public Task? Task => this._canceledTask;
  }
}
