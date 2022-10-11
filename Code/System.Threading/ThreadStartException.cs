// Decompiled with JetBrains decompiler
// Type: System.Threading.ThreadStartException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System.Threading
{
  /// <summary>The exception that is thrown when a failure occurs in a managed thread after the underlying operating system thread has been started, but before the thread is ready to execute user code.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public sealed class ThreadStartException : SystemException
  {
    internal ThreadStartException()
      : base(SR.Arg_ThreadStartException)
    {
      this.HResult = -2146233051;
    }

    internal ThreadStartException(Exception reason)
      : base(SR.Arg_ThreadStartException, reason)
    {
      this.HResult = -2146233051;
    }

    private ThreadStartException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
