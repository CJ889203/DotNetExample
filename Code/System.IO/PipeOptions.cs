// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipeOptions
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.xml

namespace System.IO.Pipes
{
  /// <summary>Provides options for creating a <see cref="T:System.IO.Pipes.PipeStream" /> object. This enumeration has a <see cref="T:System.FlagsAttribute" /> attribute that allows a bitwise combination of its member values.</summary>
  [Flags]
  public enum PipeOptions
  {
    /// <summary>Indicates that there are no additional parameters.</summary>
    None = 0,
    /// <summary>Indicates that the system should write through any intermediate cache and go directly to the pipe.</summary>
    WriteThrough = -2147483648, // 0x80000000
    /// <summary>Indicates that the pipe can be used for asynchronous reading and writing.</summary>
    Asynchronous = 1073741824, // 0x40000000
    /// <summary>When used to create a <see cref="T:System.IO.Pipes.NamedPipeServerStream" /> instance, indicates that the pipe can only be connected to a client created by the same user. When used to create a <see cref="T:System.IO.Pipes.NamedPipeClientStream" /> instance, indicates that the pipe can only connect to a server created by the same user. On Windows, it verifies both the user account and elevation level.</summary>
    CurrentUserOnly = 536870912, // 0x20000000
  }
}
