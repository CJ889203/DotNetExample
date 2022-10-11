// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.Link`1
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections.Generic;

namespace System.Data.Linq
{
  public struct Link<T>
  {
    private T underlyingValue;
    private IEnumerable<T> source;

    public Link(T value)
    {
      this.underlyingValue = value;
      this.source = (IEnumerable<T>) null;
    }

    public Link(IEnumerable<T> source)
    {
      this.source = source;
      this.underlyingValue = default (T);
    }

    public Link(Link<T> link)
    {
      this.underlyingValue = link.underlyingValue;
      this.source = link.source;
    }

    public bool HasValue => this.source == null || this.HasLoadedValue || this.HasAssignedValue;

    public bool HasLoadedOrAssignedValue => this.HasLoadedValue || this.HasAssignedValue;

    internal bool HasLoadedValue => this.source == SourceState<T>.Loaded;

    internal bool HasAssignedValue => this.source == SourceState<T>.Assigned;

    internal T UnderlyingValue => this.underlyingValue;

    internal IEnumerable<T> Source => this.source;

    internal bool HasSource => this.source != null && !this.HasAssignedValue && !this.HasLoadedValue;

    public T Value
    {
      get
      {
        if (this.HasSource)
        {
          this.underlyingValue = this.source.SingleOrDefault<T>();
          this.source = SourceState<T>.Loaded;
        }
        return this.underlyingValue;
      }
      set
      {
        this.underlyingValue = value;
        this.source = SourceState<T>.Assigned;
      }
    }
  }
}
