// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.MemberChangeConflict
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Data.Linq.Mapping;
using System.Reflection;

namespace System.Data.Linq
{
  public sealed class MemberChangeConflict
  {
    private ObjectChangeConflict conflict;
    private MetaDataMember metaMember;
    private object originalValue;
    private object databaseValue;
    private object currentValue;
    private bool isResolved;

    internal MemberChangeConflict(ObjectChangeConflict conflict, MetaDataMember metaMember)
    {
      this.conflict = conflict;
      this.metaMember = metaMember;
      this.originalValue = metaMember.StorageAccessor.GetBoxedValue(conflict.Original);
      this.databaseValue = metaMember.StorageAccessor.GetBoxedValue(conflict.Database);
      this.currentValue = metaMember.StorageAccessor.GetBoxedValue(conflict.TrackedObject.Current);
    }

    public object OriginalValue => this.originalValue;

    public object DatabaseValue => this.databaseValue;

    public object CurrentValue => this.currentValue;

    public MemberInfo Member => this.metaMember.Member;

    public void Resolve(object value)
    {
      this.conflict.TrackedObject.RefreshMember(this.metaMember, RefreshMode.OverwriteCurrentValues, value);
      this.isResolved = true;
      this.conflict.OnMemberResolved();
    }

    public void Resolve(RefreshMode refreshMode)
    {
      this.conflict.TrackedObject.RefreshMember(this.metaMember, refreshMode, this.databaseValue);
      this.isResolved = true;
      this.conflict.OnMemberResolved();
    }

    public bool IsModified => this.conflict.TrackedObject.HasChangedValue(this.metaMember);

    public bool IsResolved => this.isResolved;
  }
}
