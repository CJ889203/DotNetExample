// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.ParallelOptions
// Assembly: System.Threading.Tasks.Parallel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: CD664842-108A-425B-971D-793D618C3E3A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.Tasks.Parallel.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Tasks.Parallel.xml


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Stores options that configure the operation of methods on the <see cref="T:System.Threading.Tasks.Parallel" /> class.</summary>
  public class ParallelOptions
  {

    #nullable disable
    private TaskScheduler _scheduler;
    private int _maxDegreeOfParallelism;
    private CancellationToken _cancellationToken;

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ParallelOptions" /> class.</summary>
    public ParallelOptions()
    {
      this._scheduler = TaskScheduler.Default;
      this._maxDegreeOfParallelism = -1;
      this._cancellationToken = CancellationToken.None;
    }


    #nullable enable
    /// <summary>Gets or sets the <see cref="T:System.Threading.Tasks.TaskScheduler" /> associated with this <see cref="T:System.Threading.Tasks.ParallelOptions" /> instance. Setting this property to null indicates that the current scheduler should be used.</summary>
    /// <returns>The task scheduler that is associated with this instance.</returns>
    public TaskScheduler? TaskScheduler
    {
      get => this._scheduler;
      set => this._scheduler = value;
    }

    internal TaskScheduler EffectiveTaskScheduler => this._scheduler ?? TaskScheduler.Current;

    /// <summary>Gets or sets the maximum number of concurrent tasks enabled by this <see cref="T:System.Threading.Tasks.ParallelOptions" /> instance.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to zero or to a value that is less than -1.</exception>
    /// <returns>An integer that represents the maximum degree of parallelism.</returns>
    public int MaxDegreeOfParallelism
    {
      get => this._maxDegreeOfParallelism;
      set => this._maxDegreeOfParallelism = value != 0 && value >= -1 ? value : throw new ArgumentOutOfRangeException(nameof (MaxDegreeOfParallelism));
    }

    /// <summary>Gets or sets the <see cref="T:System.Threading.CancellationToken" /> associated with this <see cref="T:System.Threading.Tasks.ParallelOptions" /> instance.</summary>
    /// <returns>The token that is associated with this instance.</returns>
    public CancellationToken CancellationToken
    {
      get => this._cancellationToken;
      set => this._cancellationToken = value;
    }

    internal int EffectiveMaxConcurrencyLevel
    {
      get
      {
        int val2 = this.MaxDegreeOfParallelism;
        int concurrencyLevel = this.EffectiveTaskScheduler.MaximumConcurrencyLevel;
        if (concurrencyLevel > 0 && concurrencyLevel != int.MaxValue)
          val2 = val2 == -1 ? concurrencyLevel : Math.Min(concurrencyLevel, val2);
        return val2;
      }
    }
  }
}
