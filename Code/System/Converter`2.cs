// Decompiled with JetBrains decompiler
// Type: System.Converter`2
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System
{
  /// <summary>Represents a method that converts an object from one type to another type.</summary>
  /// <param name="input">The object to convert.</param>
  /// <typeparam name="TInput">The type of object that is to be converted.</typeparam>
  /// <typeparam name="TOutput">The type the input object is to be converted to.</typeparam>
  /// <returns>The <typeparamref name="TOutput" /> that represents the converted <typeparamref name="TInput" />.</returns>
  public delegate TOutput Converter<in TInput, out TOutput>(TInput input);
}
