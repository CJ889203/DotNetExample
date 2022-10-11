// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.AnonymousPipeServerStreamAcl
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.AccessControl.xml


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Provides security related APIs for the <see. cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> class.</summary>
  public static class AnonymousPipeServerStreamAcl
  {
    /// <summary>Creates a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> class with the specified pipe direction, inheritability mode, buffer size, and pipe security.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe. Anonymous pipes are unidirectional, so direction cannot be set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</param>
    /// <param name="inheritability">One of the enumeration values that determines whether the underlying handle can be inherited by child processes.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than or equal to 0.</param>
    /// <param name="pipeSecurity">An object that determines the access control and audit security for the pipe.</param>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="direction" /> is <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="inheritability" /> is not set to a valid <see cref="T:System.IO.HandleInheritability" /> enum value.
    /// 
    ///  -or-
    /// 
    /// <paramref name="bufferSize" /> is less than 0.</exception>
    /// <returns>A new anonymous pipe server stream instance.</returns>
    public static AnonymousPipeServerStream Create(
      PipeDirection direction,
      HandleInheritability inheritability,
      int bufferSize,
      PipeSecurity? pipeSecurity)
    {
      return new AnonymousPipeServerStream(direction, inheritability, bufferSize, pipeSecurity);
    }
  }
}
