// Decompiled with JetBrains decompiler
// Type: System.IO.IsolatedStorage.IsolatedStorage
// Assembly: System.IO.IsolatedStorage, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 87FE0B2F-0A44-4572-BEFC-C86F7165516A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.IsolatedStorage.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.IsolatedStorage.xml

using System.Runtime.CompilerServices;


#nullable enable
namespace System.IO.IsolatedStorage
{
  /// <summary>Represents the abstract base class from which all isolated storage implementations must derive.</summary>
  public abstract class IsolatedStorage : MarshalByRefObject
  {
    private ulong _quota;
    private bool _validQuota;

    #nullable disable
    private object _applicationIdentity;
    private object _assemblyIdentity;
    private object _domainIdentity;


    #nullable enable
    /// <summary>Gets an application identity that scopes isolated storage.</summary>
    /// <exception cref="T:System.Security.SecurityException">The code lacks the required <see cref="T:System.Security.Permissions.SecurityPermission" /> to access this object. These permissions are granted by the runtime based on security policy.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object is not isolated by the application <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />.</exception>
    /// <returns>An <see cref="T:System.Object" /> that represents the <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Application" /> identity.</returns>
    public object ApplicationIdentity
    {
      get
      {
        if (Helper.IsApplication(this.Scope))
          return this._applicationIdentity;
        throw new InvalidOperationException(SR.IsolatedStorage_ApplicationUndefined);
      }
    }

    /// <summary>Gets an assembly identity used to scope isolated storage.</summary>
    /// <exception cref="T:System.Security.SecurityException">The code lacks the required <see cref="T:System.Security.Permissions.SecurityPermission" /> to access this object.</exception>
    /// <exception cref="T:System.InvalidOperationException">The assembly is not defined.</exception>
    /// <returns>An <see cref="T:System.Object" /> that represents the <see cref="T:System.Reflection.Assembly" /> identity.</returns>
    public object AssemblyIdentity
    {
      get
      {
        if (Helper.IsAssembly(this.Scope))
          return this._assemblyIdentity;
        throw new InvalidOperationException(SR.IsolatedStorage_AssemblyUndefined);
      }
    }

    /// <summary>Gets a domain identity that scopes isolated storage.</summary>
    /// <exception cref="T:System.Security.SecurityException">The code lacks the required <see cref="T:System.Security.Permissions.SecurityPermission" /> to access this object. These permissions are granted by the runtime based on security policy.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object is not isolated by the domain <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />.</exception>
    /// <returns>An <see cref="T:System.Object" /> that represents the <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Domain" /> identity.</returns>
    public object DomainIdentity
    {
      get
      {
        if (Helper.IsDomain(this.Scope))
          return this._domainIdentity;
        throw new InvalidOperationException(SR.IsolatedStorage_AssemblyUndefined);
      }
    }

    /// <summary>Gets a value representing the current size of isolated storage.</summary>
    /// <exception cref="T:System.InvalidOperationException">The current size of the isolated store is undefined.</exception>
    /// <returns>The number of storage units currently used within the isolated storage scope.</returns>
    [CLSCompliant(false)]
    [Obsolete("IsolatedStorage.CurrentSize has been deprecated because it is not CLS Compliant. To get the current size use IsolatedStorage.UsedSize instead.")]
    public virtual ulong CurrentSize => throw new InvalidOperationException(SR.Format(SR.IsolatedStorage_CurrentSizeUndefined, (object) nameof (CurrentSize)));

    /// <summary>When overridden in a derived class, gets a value that represents the amount of the space used for isolated storage.</summary>
    /// <exception cref="T:System.InvalidOperationException">An operation was performed that requires access to <see cref="P:System.IO.IsolatedStorage.IsolatedStorage.UsedSize" />, but that property is not defined for this store. Stores that are obtained by using enumerations do not have a well-defined <see cref="P:System.IO.IsolatedStorage.IsolatedStorage.UsedSize" /> property, because partial evidence is used to open the store.</exception>
    /// <returns>The used amount of isolated storage space, in bytes.</returns>
    public virtual long UsedSize => throw new InvalidOperationException(SR.Format(SR.IsolatedStorage_QuotaIsUndefined, (object) nameof (UsedSize)));

    /// <summary>When overridden in a derived class, gets the available free space for isolated storage, in bytes.</summary>
    /// <exception cref="T:System.InvalidOperationException">An operation was performed that requires access to <see cref="P:System.IO.IsolatedStorage.IsolatedStorage.AvailableFreeSpace" />, but that property is not defined for this store. Stores that are obtained by using enumerations do not have a well-defined <see cref="P:System.IO.IsolatedStorage.IsolatedStorage.AvailableFreeSpace" /> property, because partial evidence is used to open the store.</exception>
    /// <returns>The available free space for isolated storage, in bytes.</returns>
    public virtual long AvailableFreeSpace => throw new InvalidOperationException(SR.Format(SR.IsolatedStorage_QuotaIsUndefined, (object) nameof (AvailableFreeSpace)));

    /// <summary>Gets a value representing the maximum amount of space available for isolated storage. When overridden in a derived class, this value can take different units of measure.</summary>
    /// <exception cref="T:System.InvalidOperationException">The quota has not been defined.</exception>
    /// <returns>The maximum amount of isolated storage space in bytes. Derived classes can return different units of value.</returns>
    [CLSCompliant(false)]
    [Obsolete("IsolatedStorage.MaximumSize has been deprecated because it is not CLS Compliant. To get the maximum size use IsolatedStorage.Quota instead.")]
    public virtual ulong MaximumSize
    {
      get
      {
        if (this._validQuota)
          return this._quota;
        throw new InvalidOperationException(SR.Format(SR.IsolatedStorage_QuotaIsUndefined, (object) nameof (MaximumSize)));
      }
    }

    /// <summary>When overridden in a derived class, gets a value that represents the maximum amount of space available for isolated storage.</summary>
    /// <exception cref="T:System.InvalidOperationException">An operation was performed that requires access to <see cref="P:System.IO.IsolatedStorage.IsolatedStorage.Quota" />, but that property is not defined for this store. Stores that are obtained by using enumerations do not have a well-defined <see cref="P:System.IO.IsolatedStorage.IsolatedStorage.Quota" /> property, because partial evidence is used to open the store.</exception>
    /// <returns>The limit of isolated storage space, in bytes.</returns>
    public virtual long Quota
    {
      get
      {
        if (this._validQuota)
          return (long) this._quota;
        throw new InvalidOperationException(SR.Format(SR.IsolatedStorage_QuotaIsUndefined, (object) nameof (Quota)));
      }
    }

    /// <summary>Gets an <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> enumeration value specifying the scope used to isolate the store.</summary>
    /// <returns>A bitwise combination of <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values specifying the scope used to isolate the store.</returns>
    public IsolatedStorageScope Scope { get; private set; }

    /// <summary>Gets a backslash character that can be used in a directory string. When overridden in a derived class, another character might be returned.</summary>
    /// <returns>The default implementation returns the '\' (backslash) character.</returns>
    protected virtual char SeparatorExternal => Path.DirectorySeparatorChar;

    /// <summary>Gets a period character that can be used in a directory string. When overridden in a derived class, another character might be returned.</summary>
    /// <returns>The default implementation returns the '.' (period) character.</returns>
    protected virtual char SeparatorInternal => '.';

    /// <summary>When overridden in a derived class, prompts a user to approve a larger quota size, in bytes, for isolated storage.</summary>
    /// <param name="newQuotaSize">The requested new quota size, in bytes, for the user to approve.</param>
    /// <returns>
    /// <see langword="false" /> in all cases.</returns>
    public virtual bool IncreaseQuotaTo(long newQuotaSize) => false;

    /// <summary>When overridden in a derived class, removes the individual isolated store and all contained data.</summary>
    public abstract void Remove();

    internal string? IdentityHash { get; private set; }

    /// <summary>Initializes a new <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object.</summary>
    /// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values.</param>
    /// <param name="appEvidenceType">The type of <see cref="T:System.Security.Policy.Evidence" /> that you can choose from the list of <see cref="T:System.Security.Policy.Evidence" /> for the calling application. <see langword="null" /> lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The assembly specified has insufficient permissions to create isolated stores.</exception>
    protected void InitStore(IsolatedStorageScope scope, Type appEvidenceType) => this.InitStore(scope, (Type) null, appEvidenceType);

    /// <summary>Initializes a new <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object.</summary>
    /// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values.</param>
    /// <param name="domainEvidenceType">The type of <see cref="T:System.Security.Policy.Evidence" /> that you can choose from the list of <see cref="T:System.Security.Policy.Evidence" /> present in the domain of the calling application. <see langword="null" /> lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence.</param>
    /// <param name="assemblyEvidenceType">The type of <see cref="T:System.Security.Policy.Evidence" /> that you can choose from the list of <see cref="T:System.Security.Policy.Evidence" /> present in the assembly of the calling application. <see langword="null" /> lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The assembly specified has insufficient permissions to create isolated stores.</exception>
    protected unsafe void InitStore(
      IsolatedStorageScope scope,
      Type? domainEvidenceType,
      Type? assemblyEvidenceType)
    {
      System.IO.IsolatedStorage.IsolatedStorage.VerifyScope(scope);
      this.Scope = scope;
      object identity;
      string hash;
      Helper.GetDefaultIdentityAndHash(out identity, out hash, this.SeparatorInternal);
      if (Helper.IsApplication(scope))
      {
        this._applicationIdentity = identity;
      }
      else
      {
        if (Helper.IsDomain(scope))
        {
          this._domainIdentity = identity;
          IFormatProvider provider1 = (IFormatProvider) null;
          IFormatProvider formatProvider = provider1;
          // ISSUE: untyped stack allocation
          Span<char> initialBuffer1 = new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
          IFormatProvider provider2 = formatProvider;
          Span<char> initialBuffer2 = initialBuffer1;
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 3, provider1, initialBuffer1);
          interpolatedStringHandler.AppendFormatted(hash);
          interpolatedStringHandler.AppendFormatted<char>(this.SeparatorExternal);
          interpolatedStringHandler.AppendFormatted(hash);
          ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
          hash = string.Create(provider2, initialBuffer2, ref local);
        }
        this._assemblyIdentity = identity;
      }
      this.IdentityHash = hash;
    }

    private static void VerifyScope(IsolatedStorageScope scope)
    {
      if (scope <= (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming))
      {
        if (scope <= (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly))
        {
          if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly))
            return;
        }
        else if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming))
          return;
      }
      else if (scope <= (IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine))
      {
        if (scope == (IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine) || scope == (IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine))
          return;
      }
      else if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Application) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application) || scope == (IsolatedStorageScope.Machine | IsolatedStorageScope.Application))
        return;
      throw new ArgumentException(SR.IsolatedStorage_Scope_Invalid);
    }
  }
}
