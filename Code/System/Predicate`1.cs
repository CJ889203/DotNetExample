// Decompiled with JetBrains decompiler
// Type: System.Predicate`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System
{
  /// <summary>Represents the method that defines a set of criteria and determines whether the specified object meets those criteria.</summary>
  /// <param name="obj">The object to compare against the criteria defined within the method represented by this delegate.</param>
  /// <typeparam name="T">The type of the object to compare.</typeparam>
  /// <returns>
  /// <see langword="true" /> if <paramref name="obj" /> meets the criteria defined within the method represented by this delegate; otherwise, <see langword="false" />.</returns>
  public delegate bool Predicate<in T>(T obj);
}
