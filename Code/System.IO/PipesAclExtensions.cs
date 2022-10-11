// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipesAclExtensions
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.AccessControl.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.AccessControl;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Provides Windows-specific static extension methods for manipulating Access Control List (ACL) security attributes for pipe streams.</summary>
  public static class PipesAclExtensions
  {
    /// <summary>Returns the security information of a pipe stream.</summary>
    /// <param name="stream">The existing pipe stream from which to obtain the security information.</param>
    /// <returns>The security descriptors of all the access control sections of the pipe stream.</returns>
    public static PipeSecurity GetAccessControl(this PipeStream stream) => new PipeSecurity(stream.SafePipeHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);

    /// <summary>Changes the security attributes of an existing pipe stream.</summary>
    /// <param name="stream">An existing pipe stream.</param>
    /// <param name="pipeSecurity">The security information to apply to the pipe stream.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeSecurity" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="stream" /> is a disconnected <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> instance.</exception>
    public static void SetAccessControl(this PipeStream stream, PipeSecurity pipeSecurity)
    {
      if (pipeSecurity == null)
        throw new ArgumentNullException(nameof (pipeSecurity));
      SafePipeHandle safePipeHandle = stream.SafePipeHandle;
      if (stream is NamedPipeClientStream && !stream.IsConnected)
        throw new IOException(SR.IO_IO_PipeBroken);
      pipeSecurity.Persist((SafeHandle) safePipeHandle);
    }
  }
}
