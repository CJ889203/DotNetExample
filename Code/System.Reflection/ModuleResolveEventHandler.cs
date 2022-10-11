// Decompiled with JetBrains decompiler
// Type: System.Reflection.ModuleResolveEventHandler
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents the method that will handle the <see cref="E:System.Reflection.Assembly.ModuleResolve" /> event of an <see cref="T:System.Reflection.Assembly" />.</summary>
  /// <param name="sender">The assembly that was the source of the event.</param>
  /// <param name="e">The arguments supplied by the object describing the event.</param>
  /// <returns>The module that satisfies the request.</returns>
  public delegate Module ModuleResolveEventHandler(object sender, ResolveEventArgs e);
}
