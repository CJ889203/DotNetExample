// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipeDirection
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.xml

namespace System.IO.Pipes
{
  /// <summary>Specifies the direction of the pipe.</summary>
  public enum PipeDirection
  {
    /// <summary>Specifies that the pipe direction is in.</summary>
    In = 1,
    /// <summary>Specifies that the pipe direction is out.</summary>
    Out = 2,
    /// <summary>Specifies that the pipe direction is two-way.</summary>
    InOut = 3,
  }
}
