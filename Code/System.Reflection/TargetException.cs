// Decompiled with JetBrains decompiler
// Type: System.Reflection.TargetException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents the exception that is thrown when an attempt is made to invoke an invalid target.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class TargetException : ApplicationException
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.TargetException" /> class with an empty message and the root cause of the exception.</summary>
    public TargetException()
      : this((string) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.TargetException" /> class with the given message and the root cause exception.</summary>
    /// <param name="message">A <see langword="String" /> describing the reason why the exception occurred.</param>
    public TargetException(string? message)
      : this(message, (Exception) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.TargetException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public TargetException(string? message, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2146232829;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.TargetException" /> class with the specified serialization and context information.</summary>
    /// <param name="info">The data for serializing or deserializing the object.</param>
    /// <param name="context">The source of and destination for the object.</param>
    protected TargetException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
