// Decompiled with JetBrains decompiler
// Type: System.Func`3
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System
{
  /// <summary>Encapsulates a method that has two parameters and returns a value of the type specified by the <typeparamref name="TResult" /> parameter.</summary>
  /// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
  /// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
  /// <typeparam name="T1">The type of the first parameter of the method that this delegate encapsulates.</typeparam>
  /// <typeparam name="T2">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
  /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
  /// <returns>The return value of the method that this delegate encapsulates.</returns>
  public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
}
