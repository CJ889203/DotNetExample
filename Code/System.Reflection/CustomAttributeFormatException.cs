// Decompiled with JetBrains decompiler
// Type: System.Reflection.CustomAttributeFormatException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>The exception that is thrown when the binary format of a custom attribute is invalid.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class CustomAttributeFormatException : FormatException
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeFormatException" /> class with the default properties.</summary>
    public CustomAttributeFormatException()
      : this(SR.Arg_CustomAttributeFormatException)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeFormatException" /> class with the specified message.</summary>
    /// <param name="message">The message that indicates the reason this exception was thrown.</param>
    public CustomAttributeFormatException(string? message)
      : this(message, (Exception) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeFormatException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public CustomAttributeFormatException(string? message, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2146232827;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeFormatException" /> class with the specified serialization and context information.</summary>
    /// <param name="info">The data for serializing or deserializing the custom attribute.</param>
    /// <param name="context">The source and destination for the custom attribute.</param>
    protected CustomAttributeFormatException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
