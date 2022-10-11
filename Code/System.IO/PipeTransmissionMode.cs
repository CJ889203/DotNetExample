// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipeTransmissionMode
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.xml

using System.Runtime.Versioning;

namespace System.IO.Pipes
{
  /// <summary>Specifies the transmission mode of the pipe.</summary>
  public enum PipeTransmissionMode
  {
    /// <summary>Indicates that data in the pipe is transmitted and read as a stream of bytes.</summary>
    Byte,
    /// <summary>Indicates that data in the pipe is transmitted and read as a stream of messages.</summary>
    [SupportedOSPlatform("windows")] Message,
  }
}
