// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.ChangeSet
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace System.Data.Linq
{
  public sealed class ChangeSet
  {
    private ReadOnlyCollection<object> inserts;
    private ReadOnlyCollection<object> deletes;
    private ReadOnlyCollection<object> updates;

    internal ChangeSet(
      ReadOnlyCollection<object> inserts,
      ReadOnlyCollection<object> deletes,
      ReadOnlyCollection<object> updates)
    {
      this.inserts = inserts;
      this.deletes = deletes;
      this.updates = updates;
    }

    public IList<object> Inserts => (IList<object>) this.inserts;

    public IList<object> Deletes => (IList<object>) this.deletes;

    public IList<object> Updates => (IList<object>) this.updates;

    public override string ToString() => "{" + string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Inserts: {0}, Deletes: {1}, Updates: {2}", new object[3]
    {
      (object) this.Inserts.Count,
      (object) this.Deletes.Count,
      (object) this.Updates.Count
    }) + "}";
  }
}
