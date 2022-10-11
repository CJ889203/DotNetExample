// Decompiled with JetBrains decompiler
// Type: System.Reflection.InvalidFilterCriteriaException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>The exception that is thrown in <see cref="M:System.Type.FindMembers(System.Reflection.MemberTypes,System.Reflection.BindingFlags,System.Reflection.MemberFilter,System.Object)" /> when the filter criteria is not valid for the type of filter you are using.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class InvalidFilterCriteriaException : ApplicationException
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.InvalidFilterCriteriaException" /> class with the default properties.</summary>
    public InvalidFilterCriteriaException()
      : this(SR.Arg_InvalidFilterCriteriaException)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.InvalidFilterCriteriaException" /> class with the given HRESULT and message string.</summary>
    /// <param name="message">The message text for the exception.</param>
    public InvalidFilterCriteriaException(string? message)
      : this(message, (Exception) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.InvalidFilterCriteriaException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public InvalidFilterCriteriaException(string? message, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2146232831;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.InvalidFilterCriteriaException" /> class with the specified serialization and context information.</summary>
    /// <param name="info">A <see langword="SerializationInfo" /> object that contains the information required to serialize this instance.</param>
    /// <param name="context">A <see langword="StreamingContext" /> object that contains the source and destination of the serialized stream associated with this instance.</param>
    protected InvalidFilterCriteriaException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
