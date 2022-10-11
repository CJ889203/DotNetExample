// Decompiled with JetBrains decompiler
// Type: System.Progress`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Provides an <see cref="T:System.IProgress`1" /> that invokes callbacks for each reported progress value.</summary>
  /// <typeparam name="T">Specifies the type of the progress report value.</typeparam>
  public class Progress<T> : IProgress<T>
  {

    #nullable disable
    private readonly SynchronizationContext _synchronizationContext;
    private readonly Action<T> _handler;
    private readonly SendOrPostCallback _invokeHandlers;

    /// <summary>Initializes the <see cref="T:System.Progress`1" /> object.</summary>
    public Progress()
    {
      this._synchronizationContext = SynchronizationContext.Current ?? ProgressStatics.DefaultContext;
      this._invokeHandlers = new SendOrPostCallback(this.InvokeHandlers);
    }


    #nullable enable
    /// <summary>Initializes the <see cref="T:System.Progress`1" /> object with the specified callback.</summary>
    /// <param name="handler">A handler to invoke for each reported progress value. This handler will be invoked in addition to any delegates registered with the <see cref="E:System.Progress`1.ProgressChanged" /> event. Depending on the <see cref="T:System.Threading.SynchronizationContext" /> instance captured by the <see cref="T:System.Progress`1" /> at construction, it is possible that this handler instance could be invoked concurrently with itself.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handler" /> is <see langword="null" /> (<see langword="Nothing" /> in Visual Basic).</exception>
    public Progress(Action<T> handler)
      : this()
    {
      this._handler = handler ?? throw new ArgumentNullException(nameof (handler));
    }

    /// <summary>Raised for each reported progress value.</summary>
    public event EventHandler<T>? ProgressChanged;

    /// <summary>Reports a progress change.</summary>
    /// <param name="value">The value of the updated progress.</param>
    protected virtual void OnReport(T value)
    {
      Action<T> handler = this._handler;
      EventHandler<T> progressChanged = this.ProgressChanged;
      if (handler == null && progressChanged == null)
        return;
      this._synchronizationContext.Post(this._invokeHandlers, (object) value);
    }


    #nullable disable
    /// <summary>Reports a progress change.</summary>
    /// <param name="value">The value of the updated progress.</param>
    void IProgress<T>.Report(T value) => this.OnReport(value);

    private void InvokeHandlers(object state)
    {
      T e = (T) state;
      Action<T> handler = this._handler;
      EventHandler<T> progressChanged = this.ProgressChanged;
      if (handler != null)
        handler(e);
      if (progressChanged == null)
        return;
      progressChanged((object) this, e);
    }
  }
}
