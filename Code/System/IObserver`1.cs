// Decompiled with JetBrains decompiler
// Type: System.IObserver`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System
{
  /// <summary>Provides a mechanism for receiving push-based notifications.</summary>
  /// <typeparam name="T">The object that provides notification information.</typeparam>
  public interface IObserver<in T>
  {
    /// <summary>Provides the observer with new data.</summary>
    /// <param name="value">The current notification information.</param>
    void OnNext(T value);

    /// <summary>Notifies the observer that the provider has experienced an error condition.</summary>
    /// <param name="error">An object that provides additional information about the error.</param>
    void OnError(Exception error);

    /// <summary>Notifies the observer that the provider has finished sending push-based notifications.</summary>
    void OnCompleted();
  }
}
