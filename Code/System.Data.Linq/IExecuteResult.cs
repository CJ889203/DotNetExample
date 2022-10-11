// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.IExecuteResult
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

namespace System.Data.Linq
{
  public interface IExecuteResult : IDisposable
  {
    object ReturnValue { get; }

    object GetParameterValue(int parameterIndex);
  }
}
