﻿// Decompiled with JetBrains decompiler
// Type: System.IO.ErrorEventHandler
// Assembly: System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: E02CAEC2-7763-4746-A603-8425091F7D99
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.Watcher.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.Watcher.xml


#nullable enable
namespace System.IO
{
  /// <summary>Represents the method that will handle the <see cref="E:System.IO.FileSystemWatcher.Error" /> event of a <see cref="T:System.IO.FileSystemWatcher" /> object.</summary>
  /// <param name="sender">The source of the event.</param>
  /// <param name="e">An <see cref="T:System.IO.ErrorEventArgs" /> object that contains the event data.</param>
  public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
}