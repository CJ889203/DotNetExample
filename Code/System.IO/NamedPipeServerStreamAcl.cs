// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.NamedPipeServerStreamAcl
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.AccessControl.xml


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Provides security related APIs for the <see. cref="T:System.IO.Pipes.NamedPipeServerStream" /> class.</summary>
  public static class NamedPipeServerStreamAcl
  {
    /// <summary>Creates a new instance of the <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> class with the specified pipe name, pipe direction, maximum number of server instances, transmission mode, pipe options, recommended in and out buffer sizes, pipe security, inheritability mode, and pipe access rights.</summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.</param>
    /// <param name="maxNumberOfServerInstances">The maximum number of server instances that share the same name. You can pass <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" /> for this value.</param>
    /// <param name="transmissionMode">One of the enumeration values that determines the transmission mode of the pipe.</param>
    /// <param name="options">One of the enumeration values that determines how to open or create the pipe.</param>
    /// <param name="inBufferSize">The input buffer size.</param>
    /// <param name="outBufferSize">The output buffer size.</param>
    /// <param name="pipeSecurity">An object that determines the access control and audit security for the pipe.</param>
    /// <param name="inheritability">One of the enumeration values that determines whether the underlying handle can be inherited by child processes.</param>
    /// <param name="additionalAccessRights">One of the enumeration values that specifies the access rights of the pipe.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeName" /> is empty.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="options" /> is <see cref="F:System.IO.Pipes.PipeOptions.None" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="options" /> contains an invalid flag.
    /// 
    /// -or-
    /// 
    /// <paramref name="inBufferSize" /> or <paramref name="outBufferSize" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="maxNumberOfServerInstances" /> is not a valid number: it should be greater than or equal to 1 and less than or equal to 254, or should be set to the value of <see cref="F:System.IO.Pipes.NamedPipeServerStream.MaxAllowedServerInstances" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="inheritability" /> contains an invalid enum value.
    /// 
    /// -or-
    /// 
    /// <paramref name="pipeName" /> is 'anonymous', which is reserved.</exception>
    /// <returns>A new named pipe server stream instance.</returns>
    public static NamedPipeServerStream Create(
      string pipeName,
      PipeDirection direction,
      int maxNumberOfServerInstances,
      PipeTransmissionMode transmissionMode,
      PipeOptions options,
      int inBufferSize,
      int outBufferSize,
      PipeSecurity? pipeSecurity,
      HandleInheritability inheritability = HandleInheritability.None,
      PipeAccessRights additionalAccessRights = (PipeAccessRights) 0)
    {
      return new NamedPipeServerStream(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, pipeSecurity, inheritability, additionalAccessRights);
    }
  }
}
