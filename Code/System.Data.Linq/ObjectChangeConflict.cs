// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.ObjectChangeConflict
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;

namespace System.Data.Linq
{
  public sealed class ObjectChangeConflict
  {
    private ChangeConflictSession session;
    private TrackedObject trackedObject;
    private bool isResolved;
    private ReadOnlyCollection<MemberChangeConflict> memberConflicts;
    private object database;
    private object original;
    private bool? isDeleted;

    internal ObjectChangeConflict(ChangeConflictSession session, TrackedObject trackedObject)
    {
      this.session = session;
      this.trackedObject = trackedObject;
      this.original = trackedObject.CreateDataCopy(trackedObject.Original);
    }

    internal ObjectChangeConflict(
      ChangeConflictSession session,
      TrackedObject trackedObject,
      bool isDeleted)
      : this(session, trackedObject)
    {
      this.isDeleted = new bool?(isDeleted);
    }

    internal ChangeConflictSession Session => this.session;

    internal TrackedObject TrackedObject => this.trackedObject;

    public object Object => this.trackedObject.Current;

    internal object Original => this.original;

    public bool IsResolved => this.isResolved;

    public bool IsDeleted => this.isDeleted.HasValue ? this.isDeleted.Value : this.Database == null;

    internal object Database
    {
      get
      {
        if (this.database == null)
          this.database = this.session.RefreshContext.Services.GetObjectByKey(this.trackedObject.Type, CommonDataServices.GetKeyValues(this.trackedObject.Type, this.original));
        return this.database;
      }
    }

    public void Resolve() => this.Resolve(RefreshMode.KeepCurrentValues, true);

    public void Resolve(RefreshMode refreshMode) => this.Resolve(refreshMode, false);

    public void Resolve(RefreshMode refreshMode, bool autoResolveDeletes)
    {
      if (autoResolveDeletes && this.IsDeleted)
      {
        this.ResolveDelete();
      }
      else
      {
        if (this.Database == null)
          throw Error.RefreshOfDeletedObject();
        this.trackedObject.Refresh(refreshMode, this.Database);
        this.isResolved = true;
      }
    }

    private void ResolveDelete()
    {
      if (!this.trackedObject.IsDeleted)
        this.trackedObject.ConvertToDeleted();
      this.Session.Context.Services.RemoveCachedObjectLike(this.trackedObject.Type, this.trackedObject.Original);
      this.trackedObject.AcceptChanges();
      this.isResolved = true;
    }

    public ReadOnlyCollection<MemberChangeConflict> MemberConflicts
    {
      get
      {
        if (this.memberConflicts == null)
        {
          List<MemberChangeConflict> memberChangeConflictList = new List<MemberChangeConflict>();
          if (this.Database != null)
          {
            foreach (MetaDataMember persistentDataMember in this.trackedObject.Type.PersistentDataMembers)
            {
              if (!persistentDataMember.IsAssociation && this.HasMemberConflict(persistentDataMember))
                memberChangeConflictList.Add(new MemberChangeConflict(this, persistentDataMember));
            }
          }
          this.memberConflicts = memberChangeConflictList.AsReadOnly();
        }
        return this.memberConflicts;
      }
    }

    private bool HasMemberConflict(MetaDataMember member)
    {
      object boxedValue1 = member.StorageAccessor.GetBoxedValue(this.original);
      if (!member.DeclaringType.Type.IsAssignableFrom(this.database.GetType()))
        return false;
      object boxedValue2 = member.StorageAccessor.GetBoxedValue(this.database);
      return !this.AreEqual(member, boxedValue1, boxedValue2);
    }

    private bool AreEqual(MetaDataMember member, object v1, object v2)
    {
      if (v1 == null && v2 == null)
        return true;
      if (v1 == null || v2 == null)
        return false;
      if (member.Type == typeof (char[]))
        return this.AreEqual((char[]) v1, (char[]) v2);
      return member.Type == typeof (byte[]) ? this.AreEqual((byte[]) v1, (byte[]) v2) : object.Equals(v1, v2);
    }

    private bool AreEqual(char[] a1, char[] a2)
    {
      if (a1.Length != a2.Length)
        return false;
      int index = 0;
      for (int length = a1.Length; index < length; ++index)
      {
        if ((int) a1[index] != (int) a2[index])
          return false;
      }
      return true;
    }

    private bool AreEqual(byte[] a1, byte[] a2)
    {
      if (a1.Length != a2.Length)
        return false;
      int index = 0;
      for (int length = a1.Length; index < length; ++index)
      {
        if ((int) a1[index] != (int) a2[index])
          return false;
      }
      return true;
    }

    internal void OnMemberResolved()
    {
      if (this.IsResolved || this.memberConflicts.AsEnumerable<MemberChangeConflict>().Count<MemberChangeConflict>((Func<MemberChangeConflict, bool>) (m => m.IsResolved)) != this.memberConflicts.Count)
        return;
      this.Resolve(RefreshMode.KeepCurrentValues, false);
    }
  }
}
