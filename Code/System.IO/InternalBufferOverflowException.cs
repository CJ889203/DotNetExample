// Decompiled with JetBrains decompiler
// Type: System.IO.InternalBufferOverflowException
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.IO
{
  /// <summary>The exception thrown when the internal buffer overflows.</summary>
  [TypeForwardedFrom("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class InternalBufferOverflowException : SystemException
  {
    /// <summary>Initializes a new default instance of the <see cref="T:System.IO.InternalBufferOverflowException" /> class.</summary>
    public InternalBufferOverflowException() => this.HResult = -2146232059;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.InternalBufferOverflowException" /> class with the error message to be displayed specified.</summary>
    /// <param name="message">The message to be given for the exception.</param>
    public InternalBufferOverflowException(string? message)
      : base(message)
    {
      this.HResult = -2146232059;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.InternalBufferOverflowException" /> class with the message to be displayed and the generated inner exception specified.</summary>
    /// <param name="message">The message to be given for the exception.</param>
    /// <param name="inner">The inner exception.</param>
    public InternalBufferOverflowException(string? message, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2146232059;
    }

    /// <summary>Initializes a new, empty instance of the <see cref="T:System.IO.InternalBufferOverflowException" /> class that is serializable using the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> objects.</summary>
    /// <param name="info">The information required to serialize the <see cref="T:System.IO.InternalBufferOverflowException" /> object.</param>
    /// <param name="context">The source and destination of the serialized stream associated with the <see cref="T:System.IO.InternalBufferOverflowException" /> object.</param>
    protected InternalBufferOverflowException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
