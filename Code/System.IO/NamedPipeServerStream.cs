// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.NamedPipeServerStream
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Exposes a <see cref="T:System.IO.Stream" /> around a named pipe, supporting both synchronous and asynchronous read and write operations.</summary>
  public sealed class NamedPipeServerStream : PipeStream
  {
    /// <summary>Represents the maximum number of server instances that the system resources allow.</summary>
    public const int MaxAllowedServerInstances = -1;

    #nullable disable
    private PipeStream.ConnectionValueTaskSource _reusableConnectionValueTaskSource;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> class with the specified pipe name.</summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="pipeName" /> is set to "anonymous".</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="pipeName" /> contains a colon (":").</exception>
    /// <exception cref="T:System.IO.IOException">The maximum number of server instances has been exceeded.</exception>
    public NamedPipeServerStream(string pipeName)
      : this(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> class with the specified pipe name and pipe direction.</summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="pipeName" /> contains a colon (":").</exception>
    /// <exception cref="T:System.IO.IOException">The maximum number of server instances has been exceeded.</exception>
    public NamedPipeServerStream(string pipeName, PipeDirection direction)
      : this(pipeName, direction, 1, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> class with the specified pipe name, pipe direction, and maximum number of server instances.</summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="maxNumberOfServerInstances">The maximum number of server instances that share the same name. You can pass <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" /> for this value.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// A non-negative number is required.
    /// 
    /// -or-
    /// 
    /// <paramref name="maxNumberofServerInstances" /> is less than -1 or greater than 254 (-1 indicates <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" />)
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.IO.HandleInheritability.None" /> or <see cref="F:System.IO.HandleInheritability.Inheritable" /> is required.
    /// 
    /// -or-
    /// 
    /// Access rights is limited to the <see cref="F:System.IO.Pipes.PipeAccessRights.ChangePermissions" /> , <see cref="F:System.IO.Pipes.PipeAccessRights.TakeOwnership" /> , and <see cref="F:System.IO.Pipes.PipeAccessRights.AccessSystemSecurity" /> flags.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="pipeName" /> contains a colon (":").</exception>
    /// <exception cref="T:System.IO.IOException">The maximum number of server instances has been exceeded.</exception>
    public NamedPipeServerStream(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances)
      : this(pipeName, direction, maxNumberOfServerInstances, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> class with the specified pipe name, pipe direction, maximum number of server instances, and transmission mode.</summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="maxNumberOfServerInstances">The maximum number of server instances that share the same name. You can pass <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" /> for this value.</param>
    /// <param name="transmissionMode">One of the enumeration values that determines the transmission mode of the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="maxNumberofServerInstances" /> is less than -1 or greater than 254 (-1 indicates <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" />)</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="pipeName" /> contains a colon (":").</exception>
    /// <exception cref="T:System.IO.IOException">The maximum number of server instances has been exceeded.</exception>
    public NamedPipeServerStream(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode)
      : this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, PipeOptions.None, 0, 0, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> class with the specified pipe name, pipe direction, maximum number of server instances, transmission mode, and pipe options.</summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="maxNumberOfServerInstances">The maximum number of server instances that share the same name. You can pass <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" /> for this value.</param>
    /// <param name="transmissionMode">One of the enumeration values that determines the transmission mode of the pipe.</param>
    /// <param name="options">One of the enumeration values that determines how to open or create the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="maxNumberofServerInstances" /> is less than -1 or greater than 254 (-1 indicates <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" />)
    /// 
    /// -or-
    /// 
    /// <paramref name="options" /> is not a valid <see cref="T:System.IO.Pipes.PipeOptions" /> value.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="pipeName" /> contains a colon (":").</exception>
    /// <exception cref="T:System.IO.IOException">The maximum number of server instances has been exceeded.</exception>
    public NamedPipeServerStream(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode,
      PipeOptions options)
      : this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, 0, 0, HandleInheritability.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> class with the specified pipe name, pipe direction, maximum number of server instances, transmission mode, pipe options, and recommended in and out buffer sizes.</summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="maxNumberOfServerInstances">The maximum number of server instances that share the same name. You can pass <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" /> for this value.</param>
    /// <param name="transmissionMode">One of the enumeration values that determines the transmission mode of the pipe.</param>
    /// <param name="options">One of the enumeration values that determines how to open or create the pipe.</param>
    /// <param name="inBufferSize">A positive value greater than 0 that indicates the input buffer size.</param>
    /// <param name="outBufferSize">A positive value greater than 0 that indicates the output buffer size.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> is a zero-length string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="pipeName" /> is set to "anonymous".
    /// 
    /// -or-
    /// 
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="maxNumberofServerInstances" /> is less than -1 or greater than 254 (-1 indicates <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" />)
    /// 
    /// -or-
    /// 
    /// <paramref name="options" /> is not a valid <see cref="T:System.IO.Pipes.PipeOptions" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="inBufferSize" /> is negative.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="pipeName" /> contains a colon (":").</exception>
    /// <exception cref="T:System.IO.IOException">The maximum number of server instances has been exceeded.</exception>
    public NamedPipeServerStream(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode,
      PipeOptions options,
      int inBufferSize,
      int outBufferSize)
      : this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, HandleInheritability.None)
    {
    }


    #nullable disable
    private NamedPipeServerStream(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode,
      PipeOptions options,
      int inBufferSize,
      int outBufferSize,
      HandleInheritability inheritability)
      : base(direction, transmissionMode, outBufferSize)
    {
      this.ValidateParameters(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, inheritability);
      this.Create(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, inheritability);
    }

    private void ValidateParameters(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode,
      PipeOptions options,
      int inBufferSize,
      int outBufferSize,
      HandleInheritability inheritability)
    {
      switch (pipeName)
      {
        case "":
          throw new ArgumentException(SR.Argument_NeedNonemptyPipeName);
        case null:
          throw new ArgumentNullException(nameof (pipeName));
        default:
          if (direction < PipeDirection.In || direction > PipeDirection.InOut)
            throw new ArgumentOutOfRangeException(nameof (direction), SR.ArgumentOutOfRange_DirectionModeInOutOrInOut);
          if (transmissionMode < PipeTransmissionMode.Byte || transmissionMode > PipeTransmissionMode.Message)
            throw new ArgumentOutOfRangeException(nameof (transmissionMode), SR.ArgumentOutOfRange_TransmissionModeByteOrMsg);
          if ((options & ~(PipeOptions.WriteThrough | PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly)) != PipeOptions.None)
            throw new ArgumentOutOfRangeException(nameof (options), SR.ArgumentOutOfRange_OptionsInvalid);
          if (inBufferSize < 0)
            throw new ArgumentOutOfRangeException(nameof (inBufferSize), SR.ArgumentOutOfRange_NeedNonNegNum);
          if (outBufferSize < 0)
            throw new ArgumentOutOfRangeException(nameof (outBufferSize), SR.ArgumentOutOfRange_NeedNonNegNum);
          if ((maxNumberOfServerInstances < 1 || maxNumberOfServerInstances > 254) && maxNumberOfServerInstances != -1)
            throw new ArgumentOutOfRangeException(nameof (maxNumberOfServerInstances), SR.ArgumentOutOfRange_MaxNumServerInstances);
          if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
            throw new ArgumentOutOfRangeException(nameof (inheritability), SR.ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable);
          if ((options & PipeOptions.CurrentUserOnly) == PipeOptions.None)
            break;
          this.IsCurrentUserOnly = true;
          break;
      }
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> class from the specified pipe handle.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="isAsync">
    /// <see langword="true" /> to indicate that the handle was opened asynchronously; otherwise, <see langword="false" />.</param>
    /// <param name="isConnected">
    /// <see langword="true" /> to indicate that the pipe is connected; otherwise, <see langword="false" />.</param>
    /// <param name="safePipeHandle">A safe handle for the pipe that this <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> object will encapsulate.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="safePipeHandle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="safePipeHandle" /> is an invalid handle.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="safePipeHandle" /> is not a valid pipe handle.
    /// 
    /// -or-
    /// 
    /// The maximum number of server instances has been exceeded.</exception>
    public NamedPipeServerStream(
      PipeDirection direction,
      bool isAsync,
      bool isConnected,
      SafePipeHandle safePipeHandle)
      : base(direction, PipeTransmissionMode.Byte, 0)
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

    /// <summary>Releases unmanaged resources and performs other cleanup operations before the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> instance is reclaimed by garbage collection.</summary>
    ~NamedPipeServerStream() => this.Dispose(false);

    /// <summary>Asynchronously waits for a client to connect to this <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> object.</summary>
    /// <returns>A task that represents the asynchronous wait operation.</returns>
    public Task WaitForConnectionAsync() => this.WaitForConnectionAsync(CancellationToken.None);

    /// <summary>Begins an asynchronous operation to wait for a client to connect.</summary>
    /// <param name="callback">The method to call when a client connects to the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> object.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous request from other requests.</param>
    /// <exception cref="T:System.InvalidOperationException">The pipe was not opened asynchronously.
    /// 
    /// -or-
    /// 
    /// A pipe connection has already been established.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe connection has been broken.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <returns>An object that references the asynchronous request.</returns>
    public IAsyncResult BeginWaitForConnection(AsyncCallback? callback, object? state) => TaskToApm.Begin(this.WaitForConnectionAsync(), callback, state);

    /// <summary>Ends an asynchronous operation to wait for a client to connect.</summary>
    /// <param name="asyncResult">The pending asynchronous request.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe was not opened asynchronously.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe connection has been broken.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    public void EndWaitForConnection(IAsyncResult asyncResult) => TaskToApm.End(asyncResult);

    private void CheckConnectOperationsServer()
    {
      if (this.State == PipeState.Closed)
        throw Error.GetPipeNotOpen();
      if (this.InternalHandle != null && this.InternalHandle.IsClosed)
        throw Error.GetPipeNotOpen();
      if (this.State == PipeState.Broken)
        throw new IOException(SR.IO_PipeBroken);
    }

    private void CheckDisconnectOperations()
    {
      if (this.State == PipeState.WaitingToConnect)
        throw new InvalidOperationException(SR.InvalidOperation_PipeNotYetConnected);
      if (this.State == PipeState.Disconnected)
        throw new InvalidOperationException(SR.InvalidOperation_PipeAlreadyDisconnected);
      if (this.InternalHandle == null)
        throw new InvalidOperationException(SR.InvalidOperation_PipeHandleNotSet);
      if (this.State == PipeState.Closed || this.InternalHandle != null && this.InternalHandle.IsClosed)
        throw Error.GetPipeNotOpen();
    }


    #nullable disable
    internal NamedPipeServerStream(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode,
      PipeOptions options,
      int inBufferSize,
      int outBufferSize,
      PipeSecurity pipeSecurity,
      HandleInheritability inheritability = HandleInheritability.None,
      PipeAccessRights additionalAccessRights = (PipeAccessRights) 0)
      : base(direction, transmissionMode, outBufferSize)
    {
      this.ValidateParameters(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, inheritability);
      if (pipeSecurity != null && this.IsCurrentUserOnly)
        throw new ArgumentException(SR.NotSupported_PipeSecurityIsCurrentUserOnly, nameof (pipeSecurity));
      this.Create(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, pipeSecurity, inheritability, additionalAccessRights);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        Interlocked.Exchange<PipeStream.ConnectionValueTaskSource>(ref this._reusableConnectionValueTaskSource, (PipeStream.ConnectionValueTaskSource) null)?.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    internal override void TryToReuse(PipeStream.PipeValueTaskSource source)
    {
      base.TryToReuse(source);
      if (!(source is PipeStream.ConnectionValueTaskSource connectionValueTaskSource) || Interlocked.CompareExchange<PipeStream.ConnectionValueTaskSource>(ref this._reusableConnectionValueTaskSource, connectionValueTaskSource, (PipeStream.ConnectionValueTaskSource) null) == null)
        return;
      source._preallocatedOverlapped.Dispose();
    }

    private void Create(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode,
      PipeOptions options,
      int inBufferSize,
      int outBufferSize,
      HandleInheritability inheritability)
    {
      this.Create(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, (PipeSecurity) null, inheritability, (PipeAccessRights) 0);
    }

    private void Create(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode,
      PipeOptions options,
      int inBufferSize,
      int outBufferSize,
      PipeSecurity pipeSecurity,
      HandleInheritability inheritability,
      PipeAccessRights additionalAccessRights)
    {
      string fullPath = Path.GetFullPath("\\\\.\\pipe\\" + pipeName);
      if (string.Equals(fullPath, "\\\\.\\pipe\\anonymous", StringComparison.OrdinalIgnoreCase))
        throw new ArgumentOutOfRangeException(nameof (pipeName), SR.ArgumentOutOfRange_AnonymousReserved);
      if (this.IsCurrentUserOnly)
      {
        using (WindowsIdentity current = WindowsIdentity.GetCurrent())
        {
          SecurityIdentifier owner = current.Owner;
          PipeAccessRule rule = new PipeAccessRule((IdentityReference) owner, PipeAccessRights.FullControl, AccessControlType.Allow);
          pipeSecurity = new PipeSecurity();
          pipeSecurity.AddAccessRule(rule);
          pipeSecurity.SetOwner((IdentityReference) owner);
        }
        options &= ~PipeOptions.CurrentUserOnly;
      }
      int openMode = (int) ((PipeOptions) (direction | (maxNumberOfServerInstances == 1 ? (PipeDirection) 524288 : (PipeDirection) 0)) | options | (PipeOptions) additionalAccessRights);
      int pipeMode = (int) transmissionMode << 2 | (int) transmissionMode << 1;
      if (maxNumberOfServerInstances == -1)
        maxNumberOfServerInstances = (int) byte.MaxValue;
      GCHandle pinningHandle = new GCHandle();
      try
      {
        Interop.Kernel32.SECURITY_ATTRIBUTES secAttrs = PipeStream.GetSecAttrs(inheritability, pipeSecurity, ref pinningHandle);
        SafePipeHandle namedPipe = Interop.Kernel32.CreateNamedPipe(fullPath, openMode, pipeMode, maxNumberOfServerInstances, outBufferSize, inBufferSize, 0, ref secAttrs);
        if (namedPipe.IsInvalid)
          throw Win32Marshal.GetExceptionForLastWin32Error();
        this.InitializeHandle(namedPipe, false, (options & PipeOptions.Asynchronous) != 0);
      }
      finally
      {
        if (pinningHandle.IsAllocated)
          pinningHandle.Free();
      }
    }

    /// <summary>Waits for a client to connect to this <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> object.</summary>
    /// <exception cref="T:System.InvalidOperationException">A pipe connection has already been established.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe connection has been broken.</exception>
    public void WaitForConnection()
    {
      this.CheckConnectOperationsServerWithHandle();
      if (this.IsAsync)
      {
        this.WaitForConnectionCoreAsync(CancellationToken.None).AsTask().GetAwaiter().GetResult();
      }
      else
      {
        if (!Interop.Kernel32.ConnectNamedPipe(this.InternalHandle, IntPtr.Zero))
        {
          int lastPinvokeError = Marshal.GetLastPInvokeError();
          if (lastPinvokeError != 535)
            throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError);
          if (lastPinvokeError == 535 && this.State == PipeState.Connected)
            throw new InvalidOperationException(SR.InvalidOperation_PipeAlreadyConnected);
        }
        this.State = PipeState.Connected;
      }
    }


    #nullable enable
    /// <summary>Asynchronously waits for a client to connect to this <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> object and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous wait operation.</returns>
    public Task WaitForConnectionAsync(CancellationToken cancellationToken)
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      return !this.IsAsync ? Task.Factory.StartNew((Action<object>) (s => ((NamedPipeServerStream) s).WaitForConnection()), (object) this, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default) : this.WaitForConnectionCoreAsync(cancellationToken).AsTask();
    }

    /// <summary>Disconnects the current connection.</summary>
    /// <exception cref="T:System.InvalidOperationException">No pipe connections have been made yet.
    /// 
    /// -or-
    /// 
    /// The connected pipe has already disconnected.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    public void Disconnect()
    {
      this.CheckDisconnectOperations();
      if (!Interop.Kernel32.DisconnectNamedPipe(this.InternalHandle))
        throw Win32Marshal.GetExceptionForLastWin32Error();
      this.State = PipeState.Disconnected;
    }

    /// <summary>Gets the user name of the client on the other end of the pipe.</summary>
    /// <exception cref="T:System.InvalidOperationException">No pipe connections have been made yet.
    /// 
    /// -or-
    /// 
    /// The connected pipe has already disconnected.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe connection has been broken.
    /// 
    /// -or-
    /// 
    /// The user name of the client is longer than 19 characters.</exception>
    /// <returns>The user name of the client on the other end of the pipe.</returns>
    public unsafe string GetImpersonationUserName()
    {
      this.CheckWriteOperations();
      char* chPtr = stackalloc char[514];
      return Interop.Kernel32.GetNamedPipeHandleStateW(this.InternalHandle, (uint*) null, (uint*) null, (uint*) null, (uint*) null, chPtr, 514U) ? new string(chPtr) : this.HandleGetImpersonationUserNameError(Marshal.GetLastPInvokeError(), 514U, chPtr);
    }

    /// <summary>Calls a delegate while impersonating the client.</summary>
    /// <param name="impersonationWorker">The delegate that specifies a method to call.</param>
    /// <exception cref="T:System.InvalidOperationException">No pipe connections have been made yet.
    /// 
    /// -or-
    /// 
    /// The connected pipe has already disconnected.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe connection has been broken.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred.</exception>
    public void RunAsClient(PipeStreamImpersonationWorker impersonationWorker)
    {
      this.CheckWriteOperations();
      NamedPipeServerStream.ExecuteHelper helper = new NamedPipeServerStream.ExecuteHelper(impersonationWorker, this.InternalHandle);
      bool exceptionThrown = true;
      try
      {
        NamedPipeServerStream.ImpersonateAndTryCode((object) helper);
        exceptionThrown = false;
      }
      finally
      {
        NamedPipeServerStream.RevertImpersonationOnBackout((object) helper, exceptionThrown);
      }
      if (helper._impersonateErrorCode != 0)
        throw this.WinIOError(helper._impersonateErrorCode);
      if (helper._revertImpersonateErrorCode != 0)
        throw this.WinIOError(helper._revertImpersonateErrorCode);
    }


    #nullable disable
    private static void ImpersonateAndTryCode(object helper)
    {
      NamedPipeServerStream.ExecuteHelper executeHelper = (NamedPipeServerStream.ExecuteHelper) helper;
      if (Interop.Advapi32.ImpersonateNamedPipeClient(executeHelper._handle))
        executeHelper._mustRevert = true;
      else
        executeHelper._impersonateErrorCode = Marshal.GetLastPInvokeError();
      if (!executeHelper._mustRevert)
        return;
      executeHelper._userCode();
    }

    private static void RevertImpersonationOnBackout(object helper, bool exceptionThrown)
    {
      NamedPipeServerStream.ExecuteHelper executeHelper = (NamedPipeServerStream.ExecuteHelper) helper;
      if (!executeHelper._mustRevert || Interop.Advapi32.RevertToSelf())
        return;
      executeHelper._revertImpersonateErrorCode = Marshal.GetLastPInvokeError();
    }

    private unsafe ValueTask WaitForConnectionCoreAsync(CancellationToken cancellationToken)
    {
      this.CheckConnectOperationsServerWithHandle();
      PipeStream.ConnectionValueTaskSource source = Interlocked.Exchange<PipeStream.ConnectionValueTaskSource>(ref this._reusableConnectionValueTaskSource, (PipeStream.ConnectionValueTaskSource) null) ?? new PipeStream.ConnectionValueTaskSource(this);
      try
      {
        source.PrepareForOperation();
        if (!Interop.Kernel32.ConnectNamedPipe(this.InternalHandle, source._overlapped))
        {
          int lastPinvokeError = Marshal.GetLastPInvokeError();
          switch (lastPinvokeError)
          {
            case 535:
              source.Dispose();
              if (this.State == PipeState.Connected)
                return ValueTask.FromException(ExceptionDispatchInfo.SetCurrentStackTrace((Exception) new InvalidOperationException(SR.InvalidOperation_PipeAlreadyConnected)));
              this.State = PipeState.Connected;
              return ValueTask.CompletedTask;
            case 997:
              source.RegisterForCancellation(cancellationToken);
              break;
            default:
              source.Dispose();
              return ValueTask.FromException(ExceptionDispatchInfo.SetCurrentStackTrace(Win32Marshal.GetExceptionForWin32Error(lastPinvokeError)));
          }
        }
      }
      catch
      {
        source.Dispose();
        throw;
      }
      source.FinishedScheduling();
      return new ValueTask((IValueTaskSource) source, source.Version);
    }

    private void CheckConnectOperationsServerWithHandle()
    {
      if (this.InternalHandle == null)
        throw new InvalidOperationException(SR.InvalidOperation_PipeHandleNotSet);
      this.CheckConnectOperationsServer();
    }

    private unsafe string HandleGetImpersonationUserNameError(
      int error,
      uint userNameMaxLength,
      char* userName)
    {
      if ((error == 0 || error == 1368) && Environment.Is64BitProcess)
      {
        Interop.Kernel32.LoadLibraryEx("sspicli.dll", IntPtr.Zero, 2048);
        if (Interop.Kernel32.GetNamedPipeHandleStateW(this.InternalHandle, (uint*) null, (uint*) null, (uint*) null, (uint*) null, userName, userNameMaxLength))
          return new string(userName);
        error = Marshal.GetLastPInvokeError();
      }
      throw this.WinIOError(error);
    }

    internal sealed class ExecuteHelper
    {
      internal PipeStreamImpersonationWorker _userCode;
      internal SafePipeHandle _handle;
      internal bool _mustRevert;
      internal int _impersonateErrorCode;
      internal int _revertImpersonateErrorCode;

      internal ExecuteHelper(PipeStreamImpersonationWorker userCode, SafePipeHandle handle)
      {
        this._userCode = userCode;
        this._handle = handle;
      }
    }
  }
}
