// Decompiled with JetBrains decompiler
// Type: System.Threading.BarrierPostPhaseException
// Assembly: System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3EED92AD-A1EE-4F59-AFCF-58DB2345788A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Threading
{
  /// <summary>The exception that is thrown when the post-phase action of a <see cref="T:System.Threading.Barrier" /> fails.</summary>
  [TypeForwardedFrom("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class BarrierPostPhaseException : Exception
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with a system-supplied message that describes the error.</summary>
    public BarrierPostPhaseException()
      : this((string) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with the specified inner exception.</summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BarrierPostPhaseException(Exception? innerException)
      : this((string) null, innerException)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with a specified message that describes the error.</summary>
    /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    public BarrierPostPhaseException(string? message)
      : this(message, (Exception) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public BarrierPostPhaseException(string? message, Exception? innerException)
      : base(message == null ? SR.BarrierPostPhaseException : message, innerException)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.BarrierPostPhaseException" /> class with serialized data.</summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected BarrierPostPhaseException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
