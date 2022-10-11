// Decompiled with JetBrains decompiler
// Type: System.Threading.IThreadPoolWorkItem
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.ThreadPool.xml

namespace System.Threading
{
  /// <summary>Represents a work item that can be executed by the <see cref="T:System.Threading.ThreadPool" />.</summary>
  public interface IThreadPoolWorkItem
  {
    /// <summary>Executes the work item on the thread pool.</summary>
    void Execute();
  }
}
