// Decompiled with JetBrains decompiler
// Type: System.IO.Enumeration.FileSystemEnumerable`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections;
using System.Collections.Generic;
using System.Threading;


#nullable enable
namespace System.IO.Enumeration
{
  /// <summary>Allows utilizing custom filter predicates and transform delegates for enumeration purposes.</summary>
  /// <typeparam name="TResult">The type that this enumerable encapsulates.</typeparam>
  public class FileSystemEnumerable<TResult> : IEnumerable<TResult>, IEnumerable
  {

    #nullable disable
    private FileSystemEnumerable<TResult>.DelegateEnumerator _enumerator;
    private readonly FileSystemEnumerable<TResult>.FindTransform _transform;
    private readonly EnumerationOptions _options;
    private readonly string _directory;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Enumeration.FileSystemEnumerable`1" /> class with specific search and filtering options.</summary>
    /// <param name="directory">The path of the directory where the enumeration will be performed.</param>
    /// <param name="transform">A delegate method for transforming raw find data into a result.</param>
    /// <param name="options">An object describing the enumeration options.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="directory" /> or <paramref name="transform" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="directory" /> path is empty.
    /// 
    /// -or-
    /// 
    /// <paramref name="directory" /> contains a null character "\0".</exception>
    public FileSystemEnumerable(
      string directory,
      FileSystemEnumerable<
      #nullable disable
      TResult>.FindTransform transform,

      #nullable enable
      EnumerationOptions? options = null)
      : this(directory, transform, options, false)
    {
    }


    #nullable disable
    internal FileSystemEnumerable(
      string directory,
      FileSystemEnumerable<TResult>.FindTransform transform,
      EnumerationOptions options,
      bool isNormalized)
    {
      this._directory = directory ?? throw new ArgumentNullException(nameof (directory));
      this._transform = transform ?? throw new ArgumentNullException(nameof (transform));
      this._options = options ?? EnumerationOptions.Default;
      this._enumerator = new FileSystemEnumerable<TResult>.DelegateEnumerator(this, isNormalized);
    }


    #nullable enable
    /// <summary>Gets or sets the predicate that can be used to verify if the TResults should be included.</summary>
    /// <returns>The include predicate.</returns>
    public FileSystemEnumerable<
    #nullable disable
    TResult>.FindPredicate
    #nullable enable
    ? ShouldIncludePredicate { get; set; }

    /// <summary>Gets or sets the predicate that can be used to verify if the TResults should be recursed.</summary>
    /// <returns>The recurse predicate.</returns>
    public FileSystemEnumerable<
    #nullable disable
    TResult>.FindPredicate
    #nullable enable
    ? ShouldRecursePredicate { get; set; }

    /// <summary>Retrieves the enumerator for this type of result.</summary>
    /// <returns>An enumerator.</returns>
    public IEnumerator<TResult> GetEnumerator() => (IEnumerator<TResult>) Interlocked.Exchange<FileSystemEnumerable<TResult>.DelegateEnumerator>(ref this._enumerator, (FileSystemEnumerable<TResult>.DelegateEnumerator) null) ?? (IEnumerator<TResult>) new FileSystemEnumerable<TResult>.DelegateEnumerator(this, false);


    #nullable disable
    /// <summary>Gets an enumerator that can be used to iterate.</summary>
    /// <returns>An enumerator instance.</returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();


    #nullable enable
    /// <summary>Encapsulates a method for filtering out find results.</summary>
    /// <param name="entry">A reference to the file system entry that will be evaluated with the predicate condition.</param>
    /// <returns>
    /// <see langword="true" /> if the predicate condition is met; otherwise, <see langword="false" />.</returns>
    public delegate bool FindPredicate(ref FileSystemEntry entry);

    /// <summary>Encapsulates a method for transforming raw find data into a result.</summary>
    /// <param name="entry">A reference to the file system entry that will be evaluated with the predicate condition.</param>
    /// <returns>An instance of the type that this delegate encapsulates.</returns>
    public delegate TResult FindTransform(ref FileSystemEntry entry);


    #nullable disable
    private sealed class DelegateEnumerator : FileSystemEnumerator<TResult>
    {
      private readonly FileSystemEnumerable<TResult> _enumerable;

      public DelegateEnumerator(FileSystemEnumerable<TResult> enumerable, bool isNormalized)
        : base(enumerable._directory, isNormalized, enumerable._options)
      {
        this._enumerable = enumerable;
      }

      protected override TResult TransformEntry(ref FileSystemEntry entry) => this._enumerable._transform(ref entry);

      protected override bool ShouldRecurseIntoEntry(ref FileSystemEntry entry)
      {
        FileSystemEnumerable<TResult>.FindPredicate recursePredicate = this._enumerable.ShouldRecursePredicate;
        return recursePredicate == null || recursePredicate(ref entry);
      }

      protected override bool ShouldIncludeEntry(ref FileSystemEntry entry)
      {
        FileSystemEnumerable<TResult>.FindPredicate includePredicate = this._enumerable.ShouldIncludePredicate;
        return includePredicate == null || includePredicate(ref entry);
      }
    }
  }
}
