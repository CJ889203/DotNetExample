// Decompiled with JetBrains decompiler
// Type: System.Reflection.AmbiguousMatchException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>The exception that is thrown when binding to a member results in more than one member matching the binding criteria. This class cannot be inherited.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public sealed class AmbiguousMatchException : SystemException
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AmbiguousMatchException" /> class with an empty message string and the root cause exception set to <see langword="null" />.</summary>
    public AmbiguousMatchException()
      : base(SR.RFLCT_Ambiguous)
    {
      this.HResult = -2147475171;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AmbiguousMatchException" /> class with its message string set to the given message and the root cause exception set to <see langword="null" />.</summary>
    /// <param name="message">A string indicating the reason this exception was thrown.</param>
    public AmbiguousMatchException(string? message)
      : base(message)
    {
      this.HResult = -2147475171;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AmbiguousMatchException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public AmbiguousMatchException(string? message, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2147475171;
    }


    #nullable disable
    private AmbiguousMatchException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
