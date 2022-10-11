// Decompiled with JetBrains decompiler
// Type: System.Threading.AsyncLocal`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Threading
{
  /// <summary>Represents ambient data that is local to a given asynchronous control flow, such as an asynchronous method.</summary>
  /// <typeparam name="T">The type of the ambient data.</typeparam>
  public sealed class AsyncLocal<T> : IAsyncLocal
  {

    #nullable disable
    private readonly Action<AsyncLocalValueChangedArgs<T>> m_valueChangedHandler;

    /// <summary>Instantiates an <see cref="T:System.Threading.AsyncLocal`1" /> instance that does not receive change notifications.</summary>
    public AsyncLocal()
    {
    }


    #nullable enable
    /// <summary>Instantiates an <see cref="T:System.Threading.AsyncLocal`1" /> local instance that receives change notifications.</summary>
    /// <param name="valueChangedHandler">The delegate that is called whenever the current value changes on any thread.</param>
    public AsyncLocal(
      Action<AsyncLocalValueChangedArgs<T>>? valueChangedHandler)
    {
      this.m_valueChangedHandler = valueChangedHandler;
    }

    /// <summary>Gets or sets the value of the ambient data.</summary>
    /// <returns>The value of the ambient data. If no value has been set, the returned value is <c>default(T)</c>.</returns>
    public T Value
    {
      [return: MaybeNull] get
      {
        object localValue = ExecutionContext.GetLocalValue((IAsyncLocal) this);
        return localValue != null ? (T) localValue : default (T);
      }
      set => ExecutionContext.SetLocalValue((IAsyncLocal) this, (object) value, this.m_valueChangedHandler != null);
    }


    #nullable disable
    void IAsyncLocal.OnValueChanged(
      object previousValueObj,
      object currentValueObj,
      bool contextChanged)
    {
      this.m_valueChangedHandler(new AsyncLocalValueChangedArgs<T>(previousValueObj == null ? default (T) : (T) previousValueObj, currentValueObj == null ? default (T) : (T) currentValueObj, contextChanged));
    }
  }
}
