// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipeAuditRule
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.AccessControl.xml

using System.Security.AccessControl;
using System.Security.Principal;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Represents an abstraction of an access control entry (ACE) that defines an audit rule for a pipe.</summary>
  public sealed class PipeAuditRule : AuditRule
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.PipeAuditRule" /> class for a user account specified in a <see cref="T:System.Security.Principal.IdentityReference" /> object.</summary>
    /// <param name="identity">An <see cref="T:System.Security.Principal.IdentityReference" /> object that encapsulates a reference to a user account.</param>
    /// <param name="rights">One of the <see cref="T:System.IO.Pipes.PipeAccessRights" /> values that specifies the type of operation associated with the access rule.</param>
    /// <param name="flags">One of the <see cref="T:System.Security.AccessControl.AuditFlags" /> values that specifies when to perform auditing.</param>
    public PipeAuditRule(IdentityReference identity, PipeAccessRights rights, AuditFlags flags)
      : this(identity, PipeAuditRule.AccessMaskFromRights(rights), false, flags)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.PipeAuditRule" /> class for a named user account.</summary>
    /// <param name="identity">The name of the user account.</param>
    /// <param name="rights">One of the <see cref="T:System.IO.Pipes.PipeAccessRights" /> values that specifies the type of operation associated with the access rule.</param>
    /// <param name="flags">One of the <see cref="T:System.Security.AccessControl.AuditFlags" /> values that specifies when to perform auditing.</param>
    public PipeAuditRule(string identity, PipeAccessRights rights, AuditFlags flags)
      : this((IdentityReference) new NTAccount(identity), PipeAuditRule.AccessMaskFromRights(rights), false, flags)
    {
    }


    #nullable disable
    internal PipeAuditRule(
      IdentityReference identity,
      int accessMask,
      bool isInherited,
      AuditFlags flags)
      : base(identity, accessMask, isInherited, InheritanceFlags.None, PropagationFlags.None, flags)
    {
    }

    private static int AccessMaskFromRights(PipeAccessRights rights) => rights >= (PipeAccessRights) 0 && rights <= (PipeAccessRights.FullControl | PipeAccessRights.AccessSystemSecurity) ? (int) rights : throw new ArgumentOutOfRangeException(nameof (rights), SR.ArgumentOutOfRange_NeedValidPipeAccessRights);

    /// <summary>Gets the <see cref="T:System.IO.Pipes.PipeAccessRights" /> flags that are associated with the current <see cref="T:System.IO.Pipes.PipeAuditRule" /> object.</summary>
    /// <returns>A bitwise combination of the <see cref="T:System.IO.Pipes.PipeAccessRights" /> values.</returns>
    public PipeAccessRights PipeAccessRights => PipeAccessRule.RightsFromAccessMask(this.AccessMask);
  }
}
