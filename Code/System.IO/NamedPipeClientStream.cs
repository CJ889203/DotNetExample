// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.NamedPipeClientStream
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Exposes a <see cref="T:System.IO.Stream" /> around a named pipe, which supports both synchronous and asynchronous read and write operations.</summary>
  public sealed class NamedPipeClientStream : PipeStream
  {

    #nullable disable
    private readonly string _normalizedPipePath;
    private readonly TokenImpersonationLevel _impersonationLevel;
    private readonly PipeOptions _pipeOptions;
    private readonly HandleInheritability _inheritability;
    private readonly PipeDirection _direction;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> class with the specified pipe name.</summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="pipeName" /> is set to "anonymous".</exception>
    public NamedPipeClientStream(string pipeName)
      : this(".", pipeName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.None, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> class with the specified pipe and server names.</summary>
    /// <param name="serverName">The name of the remote computer to connect to, or "." to specify the local computer.</param>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="pipeName" /> is set to "anonymous".</exception>
    public NamedPipeClientStream(string serverName, string pipeName)
      : this(serverName, pipeName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.None, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> class with the specified pipe and server names, and the specified pipe direction.</summary>
    /// <param name="serverName">The name of the remote computer to connect to, or "." to specify the local computer.</param>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.</exception>
    public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction)
      : this(serverName, pipeName, direction, PipeOptions.None, TokenImpersonationLevel.None, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> class with the specified pipe and server names, and the specified pipe direction and pipe options.</summary>
    /// <param name="serverName">The name of the remote computer to connect to, or "." to specify the local computer.</param>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="options">One of the enumeration values that determines how to open or create the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="options" /> is not a valid <see cref="T:System.IO.Pipes.PipeOptions" /> value.</exception>
    public NamedPipeClientStream(
      string serverName,
      string pipeName,
      PipeDirection direction,
      PipeOptions options)
      : this(serverName, pipeName, direction, options, TokenImpersonationLevel.None, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> class with the specified pipe and server names, and the specified pipe direction, pipe options, and security impersonation level.</summary>
    /// <param name="serverName">The name of the remote computer to connect to, or "." to specify the local computer.</param>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="options">One of the enumeration values that determines how to open or create the pipe.</param>
    /// <param name="impersonationLevel">One of the enumeration values that determines the security impersonation level.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="options" /> is not a valid <see cref="T:System.IO.Pipes.PipeOptions" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="impersonationLevel" /> is not a valid <see cref="T:System.Security.Principal.TokenImpersonationLevel" /> value.</exception>
    public NamedPipeClientStream(
      string serverName,
      string pipeName,
      PipeDirection direction,
      PipeOptions options,
      TokenImpersonationLevel impersonationLevel)
      : this(serverName, pipeName, direction, options, impersonationLevel, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> class with the specified pipe and server names, and the specified pipe direction, pipe options, security impersonation level, and inheritability mode.</summary>
    /// <param name="serverName">The name of the remote computer to connect to, or "." to specify the local computer.</param>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="options">One of the enumeration values that determines how to open or create the pipe.</param>
    /// <param name="impersonationLevel">One of the enumeration values that determines the security impersonation level.</param>
    /// <param name="inheritability">One of the enumeration values that determines whether the underlying handle will be inheritable by child processes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> or <paramref name="serverName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="options" /> is not a valid <see cref="T:System.IO.Pipes.PipeOptions" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="impersonationLevel" /> is not a valid <see cref="T:System.Security.Principal.TokenImpersonationLevel" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="inheritability" /> is not a valid <see cref="T:System.IO.HandleInheritability" /> value.</exception>
    public NamedPipeClientStream(
      string serverName,
      string pipeName,
      PipeDirection direction,
      PipeOptions options,
      TokenImpersonationLevel impersonationLevel,
      HandleInheritability inheritability)
      : base(direction, 0)
    {
      if (pipeName == null)
        throw new ArgumentNullException(nameof (pipeName));
      if (serverName == null)
        throw new ArgumentNullException(nameof (serverName), SR.ArgumentNull_ServerName);
      if (pipeName.Length == 0)
        throw new ArgumentException(SR.Argument_NeedNonemptyPipeName);
      if (serverName.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyServerName);
      if ((options & ~(PipeOptions.WriteThrough | PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly)) != PipeOptions.None)
        throw new ArgumentOutOfRangeException(nameof (options), SR.ArgumentOutOfRange_OptionsInvalid);
      if (impersonationLevel < TokenImpersonationLevel.None || impersonationLevel > TokenImpersonationLevel.Delegation)
        throw new ArgumentOutOfRangeException(nameof (impersonationLevel), SR.ArgumentOutOfRange_ImpersonationInvalid);
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException(nameof (inheritability), SR.ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable);
      if ((options & PipeOptions.CurrentUserOnly) != PipeOptions.None)
        this.IsCurrentUserOnly = true;
      this._normalizedPipePath = PipeStream.GetPipePath(serverName, pipeName);
      this._direction = direction;
      this._inheritability = inheritability;
      this._impersonationLevel = impersonationLevel;
      this._pipeOptions = options;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> class for the specified pipe handle with the specified pipe direction.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="isAsync">
    /// <see langword="true" /> to indicate that the handle was opened asynchronously; otherwise, <see langword="false" />.</param>
    /// <param name="isConnected">
    /// <see langword="true" /> to indicate that the pipe is connected; otherwise, <see langword="false" />.</param>
    /// <param name="safePipeHandle">A safe handle for the pipe that this <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> object will encapsulate.</param>
    /// <exception cref="T:System.IO.IOException">The stream has been closed.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="safePipeHandle" /> is not a valid handle.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="safePipeHandle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.</exception>
    public NamedPipeClientStream(
      PipeDirection direction,
      bool isAsync,
      bool isConnected,
      SafePipeHandle safePipeHandle)
      : base(direction, 0)
    {
      if (safePipeHandle == null)
        throw new ArgumentNullException(nameof (safePipeHandle));
      if (safePipeHandle.IsInvalid)
        throw new ArgumentException(SR.Argument_InvalidHandle, nameof (safePipeHandle));
      this.ValidateHandleIsPipe(safePipeHandle);
      this.InitializeHandle(safePipeHandle, true, isAsync);
      if (!isConnected)
        return;
      this.State = PipeState.Connected;
    }

    /// <summary>Releases unmanaged resources and performs other cleanup operations before the <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> instance is reclaimed by garbage collection.</summary>
    ~NamedPipeClientStream() => this.Dispose(false);

    /// <summary>Connects to a waiting server with an infinite time-out value.</summary>
    /// <exception cref="T:System.InvalidOperationException">The client is already connected.</exception>
    public void Connect() => this.Connect(-1);

    /// <summary>Connects to a waiting server within the specified time-out period.</summary>
    /// <param name="timeout">The number of milliseconds to wait for the server to respond before the connection times out.</param>
    /// <exception cref="T:System.TimeoutException">Could not connect to the server within the specified <paramref name="timeout" /> period.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> is less than 0 and not set to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The client is already connected.</exception>
    /// <exception cref="T:System.IO.IOException">The server is connected to another client and the time-out period has expired.</exception>
    public void Connect(int timeout)
    {
      this.CheckConnectOperationsClient();
      if (timeout < 0 && timeout != -1)
        throw new ArgumentOutOfRangeException(nameof (timeout), SR.ArgumentOutOfRange_InvalidTimeout);
      this.ConnectInternal(timeout, CancellationToken.None, Environment.TickCount);
    }

    private void ConnectInternal(int timeout, CancellationToken cancellationToken, int startTime)
    {
      int num = 0;
      SpinWait spinWait = new SpinWait();
      do
      {
        cancellationToken.ThrowIfCancellationRequested();
        int timeout1 = timeout - num;
        if (cancellationToken.CanBeCanceled && timeout1 > 50)
          timeout1 = 50;
        if (this.TryConnect(timeout1, cancellationToken))
          return;
        spinWait.SpinOnce();
      }
      while (timeout == -1 || (num = Environment.TickCount - startTime) < timeout);
      throw new TimeoutException();
    }

    /// <summary>Asynchronously connects to a waiting server with an infinite timeout period.</summary>
    /// <returns>A task that represents the asynchronous connect operation.</returns>
    public Task ConnectAsync() => this.ConnectAsync(-1, CancellationToken.None);

    /// <summary>Asynchronously connects to a waiting server within the specified timeout period.</summary>
    /// <param name="timeout">The number of milliseconds to wait for the server to respond before the connection times out.</param>
    /// <returns>A task that represents the asynchronous connect operation.</returns>
    public Task ConnectAsync(int timeout) => this.ConnectAsync(timeout, CancellationToken.None);

    /// <summary>Asynchronously connects to a waiting server and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous connect operation.</returns>
    public Task ConnectAsync(CancellationToken cancellationToken) => this.ConnectAsync(-1, cancellationToken);

    /// <summary>Asynchronously connects to a waiting server within the specified timeout period and monitors cancellation requests.</summary>
    /// <param name="timeout">The number of milliseconds to wait for the server to respond before the connection times out.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous connect operation.</returns>
    public Task ConnectAsync(int timeout, CancellationToken cancellationToken)
    {
      this.CheckConnectOperationsClient();
      if (timeout < 0 && timeout != -1)
        throw new ArgumentOutOfRangeException(nameof (timeout), SR.ArgumentOutOfRange_InvalidTimeout);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      int startTime = Environment.TickCount;
      return Task.Run((Action) (() => this.ConnectInternal(timeout, cancellationToken, startTime)), cancellationToken);
    }

    protected internal override void CheckPipePropertyOperations()
    {
      base.CheckPipePropertyOperations();
      if (this.State == PipeState.WaitingToConnect)
        throw new InvalidOperationException(SR.InvalidOperation_PipeNotYetConnected);
      if (this.State == PipeState.Broken)
        throw new IOException(SR.IO_PipeBroken);
    }

    private void CheckConnectOperationsClient()
    {
      if (this.State == PipeState.Connected)
        throw new InvalidOperationException(SR.InvalidOperation_PipeAlreadyConnected);
      if (this.State == PipeState.Closed)
        throw Error.GetPipeNotOpen();
    }

    private bool TryConnect(int timeout, CancellationToken cancellationToken)
    {
      Interop.Kernel32.SECURITY_ATTRIBUTES secAttrs = PipeStream.GetSecAttrs(this._inheritability);
      int dwFlagsAndAttributes = (int) (this._pipeOptions & ~PipeOptions.CurrentUserOnly);
      if (this._impersonationLevel != TokenImpersonationLevel.None)
        dwFlagsAndAttributes = dwFlagsAndAttributes | 1048576 | (int) (this._impersonationLevel - 1) << 16;
      int dwDesiredAccess = 0;
      if ((PipeDirection.In & this._direction) != (PipeDirection) 0)
        dwDesiredAccess |= int.MinValue;
      if ((PipeDirection.Out & this._direction) != (PipeDirection) 0)
        dwDesiredAccess |= 1073741824;
      SafePipeHandle namedPipeClient = Interop.Kernel32.CreateNamedPipeClient(this._normalizedPipePath, dwDesiredAccess, FileShare.None, ref secAttrs, FileMode.Open, dwFlagsAndAttributes, IntPtr.Zero);
      if (namedPipeClient.IsInvalid)
      {
        int lastPinvokeError1 = Marshal.GetLastPInvokeError();
        switch (lastPinvokeError1)
        {
          case 2:
          case 231:
            if (!Interop.Kernel32.WaitNamedPipe(this._normalizedPipePath, timeout))
            {
              int lastPinvokeError2 = Marshal.GetLastPInvokeError();
              switch (lastPinvokeError2)
              {
                case 2:
                case 121:
                  return false;
                default:
                  throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError2);
              }
            }
            else
            {
              namedPipeClient = Interop.Kernel32.CreateNamedPipeClient(this._normalizedPipePath, dwDesiredAccess, FileShare.None, ref secAttrs, FileMode.Open, dwFlagsAndAttributes, IntPtr.Zero);
              if (namedPipeClient.IsInvalid)
              {
                int lastPinvokeError3 = Marshal.GetLastPInvokeError();
                if (lastPinvokeError3 == 231)
                  return false;
                throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError3);
              }
              break;
            }
          default:
            throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError1);
        }
      }
      this.InitializeHandle(namedPipeClient, false, (this._pipeOptions & PipeOptions.Asynchronous) != 0);
      this.State = PipeState.Connected;
      this.ValidateRemotePipeUser();
      return true;
    }

    /// <summary>Gets the number of server instances that share the same pipe name.</summary>
    /// <exception cref="T:System.InvalidOperationException">The pipe handle has not been set.
    /// 
    /// -or-
    /// 
    /// The current <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> object has not yet connected to a <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> object.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or an I/O error occurred.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The underlying pipe handle is closed.</exception>
    /// <returns>The number of server instances that share the same pipe name.</returns>
    [SupportedOSPlatform("windows")]
    public unsafe int NumberOfServerInstances
    {
      get
      {
        this.CheckPipePropertyOperations();
        uint num;
        return Interop.Kernel32.GetNamedPipeHandleStateW(this.InternalHandle, (uint*) null, &num, (uint*) null, (uint*) null, (char*) null, 0U) ? (int) num : throw this.WinIOError(Marshal.GetLastPInvokeError());
      }
    }

    private void ValidateRemotePipeUser()
    {
      if (!this.IsCurrentUserOnly)
        return;
      using (WindowsIdentity current = WindowsIdentity.GetCurrent())
      {
        if (this.GetAccessControl().GetOwner(typeof (SecurityIdentifier)) != (IdentityReference) current.Owner)
        {
          this.State = PipeState.Closed;
          throw new UnauthorizedAccessException(SR.UnauthorizedAccess_NotOwnedByCurrentUser);
        }
      }
    }
  }
}
