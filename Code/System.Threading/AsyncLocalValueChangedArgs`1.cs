// Decompiled with JetBrains decompiler
// Type: System.Threading.AsyncLocalValueChangedArgs`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml


#nullable enable
namespace System.Threading
{
  /// <summary>The class that provides data change information to <see cref="T:System.Threading.AsyncLocal`1" /> instances that register for change notifications.</summary>
  /// <typeparam name="T">The type of the data.</typeparam>
  public readonly struct AsyncLocalValueChangedArgs<T>
  {
    /// <summary>Gets the data's previous value.</summary>
    /// <returns>The data's previous value.</returns>
    public T? PreviousValue { get; }

    /// <summary>Gets the data's current value.</summary>
    /// <returns>The data's current value.</returns>
    public T? CurrentValue { get; }

    /// <summary>Returns a value that indicates whether the value changes because of a change of execution context.</summary>
    /// <returns>
    /// <see langword="true" /> if the value changed because of a change of execution context; otherwise, <see langword="false" />.</returns>
    public bool ThreadContextChanged { get; }


    #nullable disable
    internal AsyncLocalValueChangedArgs(T previousValue, T currentValue, bool contextChanged)
    {
      this.PreviousValue = previousValue;
      this.CurrentValue = currentValue;
      this.ThreadContextChanged = contextChanged;
    }
  }
}
