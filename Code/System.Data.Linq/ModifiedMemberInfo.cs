// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.ModifiedMemberInfo
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Reflection;

namespace System.Data.Linq
{
  public struct ModifiedMemberInfo
  {
    private MemberInfo member;
    private object current;
    private object original;

    internal ModifiedMemberInfo(MemberInfo member, object current, object original)
    {
      this.member = member;
      this.current = current;
      this.original = original;
    }

    public MemberInfo Member => this.member;

    public object CurrentValue => this.current;

    public object OriginalValue => this.original;
  }
}
