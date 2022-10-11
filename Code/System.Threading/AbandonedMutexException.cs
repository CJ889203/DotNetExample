// Decompiled with JetBrains decompiler
// Type: System.Threading.AbandonedMutexException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Threading
{
  /// <summary>The exception that is thrown when one thread acquires a <see cref="T:System.Threading.Mutex" /> object that another thread has abandoned by exiting without releasing it.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class AbandonedMutexException : SystemException
  {
    private int _mutexIndex = -1;

    #nullable disable
    private Mutex _mutex;

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with default values.</summary>
    public AbandonedMutexException()
      : base(SR.Threading_AbandonedMutexException)
    {
      this.HResult = -2146233043;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified error message.</summary>
    /// <param name="message">An error message that explains the reason for the exception.</param>
    public AbandonedMutexException(string? message)
      : base(message)
    {
      this.HResult = -2146233043;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified error message and inner exception.</summary>
    /// <param name="message">An error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public AbandonedMutexException(string? message, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2146233043;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified index for the abandoned mutex, if applicable, and a <see cref="T:System.Threading.Mutex" /> object that represents the mutex.</summary>
    /// <param name="location">The index of the abandoned mutex in the array of wait handles if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitAny" /> method, or -1 if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitOne" /> or <see cref="Overload:System.Threading.WaitHandle.WaitAll" /> methods.</param>
    /// <param name="handle">A <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex.</param>
    public AbandonedMutexException(int location, WaitHandle? handle)
      : base(SR.Threading_AbandonedMutexException)
    {
      this.HResult = -2146233043;
      this.SetupException(location, handle);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified error message, the index of the abandoned mutex, if applicable, and the abandoned mutex.</summary>
    /// <param name="message">An error message that explains the reason for the exception.</param>
    /// <param name="location">The index of the abandoned mutex in the array of wait handles if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitAny" /> method, or -1 if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitOne" /> or <see cref="Overload:System.Threading.WaitHandle.WaitAll" /> methods.</param>
    /// <param name="handle">A <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex.</param>
    public AbandonedMutexException(string? message, int location, WaitHandle? handle)
      : base(message)
    {
      this.HResult = -2146233043;
      this.SetupException(location, handle);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified error message, the inner exception, the index for the abandoned mutex, if applicable, and a <see cref="T:System.Threading.Mutex" /> object that represents the mutex.</summary>
    /// <param name="message">An error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    /// <param name="location">The index of the abandoned mutex in the array of wait handles if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitAny" /> method, or -1 if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitOne" /> or <see cref="Overload:System.Threading.WaitHandle.WaitAll" /> methods.</param>
    /// <param name="handle">A <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex.</param>
    public AbandonedMutexException(
      string? message,
      Exception? inner,
      int location,
      WaitHandle? handle)
      : base(message, inner)
    {
      this.HResult = -2146233043;
      this.SetupException(location, handle);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with serialized data.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains contextual information about the source or destination.</param>
    protected AbandonedMutexException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }


    #nullable disable
    private void SetupException(int location, WaitHandle handle)
    {
      this._mutexIndex = location;
      this._mutex = handle as Mutex;
    }


    #nullable enable
    /// <summary>Gets the abandoned mutex that caused the exception, if known.</summary>
    /// <returns>A <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex, or <see langword="null" /> if the abandoned mutex could not be identified.</returns>
    public Mutex? Mutex => this._mutex;

    /// <summary>Gets the index of the abandoned mutex that caused the exception, if known.</summary>
    /// <returns>The index, in the array of wait handles passed to the <see cref="Overload:System.Threading.WaitHandle.WaitAny" /> method, of the <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex, or -1 if the index of the abandoned mutex could not be determined.</returns>
    public int MutexIndex => this._mutexIndex;
  }
}
