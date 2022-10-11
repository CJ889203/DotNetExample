// Decompiled with JetBrains decompiler
// Type: System.Threading.CompressedStack
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.xml

using System.Runtime.Serialization;


#nullable enable
namespace System.Threading
{
  /// <summary>Provides methods for setting and capturing the compressed stack on the current thread. This class cannot be inherited.</summary>
  public sealed class CompressedStack : ISerializable
  {
    private CompressedStack()
    {
    }

    /// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the logical context information needed to recreate an instance of this execution context.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to be populated with serialization information.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure representing the destination context of the serialization.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    /// <summary>Captures the compressed stack from the current thread.</summary>
    /// <returns>A <see cref="T:System.Threading.CompressedStack" /> object.</returns>
    public static CompressedStack Capture() => CompressedStack.GetCompressedStack();

    /// <summary>Creates a copy of the current compressed stack.</summary>
    /// <returns>A <see cref="T:System.Threading.CompressedStack" /> object representing the current compressed stack.</returns>
    public CompressedStack CreateCopy() => this;

    /// <summary>Gets the compressed stack for the current thread.</summary>
    /// <exception cref="T:System.Security.SecurityException">A caller in the call chain does not have permission to access unmanaged code.
    /// 
    /// -or-
    /// 
    /// The request for <see cref="T:System.Security.Permissions.StrongNameIdentityPermission" /> failed.</exception>
    /// <returns>A <see cref="T:System.Threading.CompressedStack" /> for the current thread.</returns>
    public static CompressedStack GetCompressedStack() => new CompressedStack();

    /// <summary>Runs a method in the specified compressed stack on the current thread.</summary>
    /// <param name="compressedStack">The <see cref="T:System.Threading.CompressedStack" /> to set.</param>
    /// <param name="callback">A <see cref="T:System.Threading.ContextCallback" /> that represents the method to be run in the specified security context.</param>
    /// <param name="state">The object to be passed to the callback method.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="compressedStack" /> is <see langword="null" />.</exception>
    public static void Run(CompressedStack compressedStack, ContextCallback callback, object? state)
    {
      if (compressedStack == null)
        throw new ArgumentNullException(nameof (compressedStack));
      callback(state);
    }
  }
}
