// Decompiled with JetBrains decompiler
// Type: System.Threading.ThreadExceptionEventHandler
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Represents the method that will handle the <see cref="E:System.Windows.Forms.Application.ThreadException" /> event of an <see cref="T:System.Windows.Forms.Application" />.</summary>
  /// <param name="sender">The source of the event.</param>
  /// <param name="e">A <see cref="T:System.Threading.ThreadExceptionEventArgs" /> that contains the event data.</param>
  public delegate void ThreadExceptionEventHandler(object sender, ThreadExceptionEventArgs e);
}
