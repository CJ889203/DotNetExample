// Decompiled with JetBrains decompiler
// Type: System.Threading.ThreadExceptionEventArgs
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Provides data for the <see cref="E:System.Windows.Forms.Application.ThreadException" /> event.</summary>
  public class ThreadExceptionEventArgs : EventArgs
  {

    #nullable disable
    private readonly Exception m_exception;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.ThreadExceptionEventArgs" /> class.</summary>
    /// <param name="t">The <see cref="T:System.Exception" /> that occurred.</param>
    public ThreadExceptionEventArgs(Exception t) => this.m_exception = t;

    /// <summary>Gets the <see cref="T:System.Exception" /> that occurred.</summary>
    /// <returns>The <see cref="T:System.Exception" /> that occurred.</returns>
    public Exception Exception => this.m_exception;
  }
}
