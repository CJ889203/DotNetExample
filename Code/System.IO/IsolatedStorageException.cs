// Decompiled with JetBrains decompiler
// Type: System.IO.IsolatedStorage.IsolatedStorageException
// Assembly: System.IO.IsolatedStorage, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 87FE0B2F-0A44-4572-BEFC-C86F7165516A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.IsolatedStorage.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.IsolatedStorage.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.IO.IsolatedStorage
{
  /// <summary>The exception that is thrown when an operation in isolated storage fails.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class IsolatedStorageException : Exception, ISerializable
  {

    #nullable disable
    internal Exception _underlyingException;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageException" /> class with default properties.</summary>
    public IsolatedStorageException()
      : base(SR.IsolatedStorage_Exception)
    {
      this.HResult = -2146233264;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageException" /> class with a specified error message.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public IsolatedStorageException(string? message)
      : base(message)
    {
      this.HResult = -2146233264;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public IsolatedStorageException(string? message, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2146233264;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageException" /> class with serialized data.</summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected IsolatedStorageException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
