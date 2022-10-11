// Decompiled with JetBrains decompiler
// Type: System.IO.InvalidDataException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.IO
{
  /// <summary>The exception that is thrown when a data stream is in an invalid format.</summary>
  [TypeForwardedFrom("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public sealed class InvalidDataException : SystemException
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.InvalidDataException" /> class.</summary>
    public InvalidDataException()
      : base(SR.GenericInvalidData)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.InvalidDataException" /> class with a specified error message.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public InvalidDataException(string? message)
      : base(message)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.InvalidDataException" /> class with a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public InvalidDataException(string? message, Exception? innerException)
      : base(message, innerException)
    {
    }


    #nullable disable
    private InvalidDataException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
