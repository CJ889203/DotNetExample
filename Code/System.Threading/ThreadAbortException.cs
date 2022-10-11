// Decompiled with JetBrains decompiler
// Type: System.Threading.ThreadAbortException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Threading
{
  /// <summary>The exception that is thrown when a call is made to the <see cref="M:System.Threading.Thread.Abort(System.Object)" /> method. This class cannot be inherited.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public sealed class ThreadAbortException : SystemException
  {
    internal ThreadAbortException() => this.HResult = -2146233040;

    /// <summary>Gets an object that contains application-specific information related to the thread abort.</summary>
    /// <returns>An object containing application-specific information.</returns>
    public object? ExceptionState => (object) null;


    #nullable disable
    private ThreadAbortException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
