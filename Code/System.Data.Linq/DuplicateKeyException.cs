// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.DuplicateKeyException
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

namespace System.Data.Linq
{
  public class DuplicateKeyException : InvalidOperationException
  {
    private object duplicate;

    public DuplicateKeyException(object duplicate) => this.duplicate = duplicate;

    public DuplicateKeyException(object duplicate, string message)
      : base(message)
    {
      this.duplicate = duplicate;
    }

    public DuplicateKeyException(object duplicate, string message, Exception innerException)
      : base(message, innerException)
    {
      this.duplicate = duplicate;
    }

    public object Object => this.duplicate;
  }
}
