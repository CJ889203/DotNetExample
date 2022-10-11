// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipeAccessRule
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.AccessControl.xml

using System.Security.AccessControl;
using System.Security.Principal;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Represents an abstraction of an access control entry (ACE) that defines an access rule for a pipe.</summary>
  public sealed class PipeAccessRule : AccessRule
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.PipeAccessRule" /> class with the specified identity, pipe access rights, and access control type.</summary>
    /// <param name="identity">The name of the user account.</param>
    /// <param name="rights">One of the <see cref="T:System.IO.Pipes.PipeAccessRights" /> values that specifies the type of operation associated with the access rule.</param>
    /// <param name="type">One of the <see cref="T:System.Security.AccessControl.AccessControlType" /> values that specifies whether to allow or deny the operation.</param>
    public PipeAccessRule(string identity, PipeAccessRights rights, AccessControlType type)
      : this((IdentityReference) new NTAccount(identity), PipeAccessRule.AccessMaskFromRights(rights, type), false, type)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.PipeAccessRule" /> class with the specified identity, pipe access rights, and access control type.</summary>
    /// <param name="identity">An <see cref="T:System.Security.Principal.IdentityReference" /> object that encapsulates a reference to a user account.</param>
    /// <param name="rights">One of the <see cref="T:System.IO.Pipes.PipeAccessRights" /> values that specifies the type of operation associated with the access rule.</param>
    /// <param name="type">One of the <see cref="T:System.Security.AccessControl.AccessControlType" /> values that specifies whether to allow or deny the operation.</param>
    public PipeAccessRule(
      IdentityReference identity,
      PipeAccessRights rights,
      AccessControlType type)
      : this(identity, PipeAccessRule.AccessMaskFromRights(rights, type), false, type)
    {
    }


    #nullable disable
    internal PipeAccessRule(
      IdentityReference identity,
      int accessMask,
      bool isInherited,
      AccessControlType type)
      : base(identity, accessMask, isInherited, InheritanceFlags.None, PropagationFlags.None, type)
    {
    }

    /// <summary>Gets the <see cref="T:System.IO.Pipes.PipeAccessRights" /> flags that are associated with the current <see cref="T:System.IO.Pipes.PipeAccessRule" /> object.</summary>
    /// <returns>A bitwise combination of the <see cref="T:System.IO.Pipes.PipeAccessRights" /> values.</returns>
    public PipeAccessRights PipeAccessRights => PipeAccessRule.RightsFromAccessMask(this.AccessMask);

    internal static int AccessMaskFromRights(PipeAccessRights rights, AccessControlType controlType)
    {
      if (rights < (PipeAccessRights) 0 || rights > (PipeAccessRights.FullControl | PipeAccessRights.AccessSystemSecurity))
        throw new ArgumentOutOfRangeException(nameof (rights), SR.ArgumentOutOfRange_NeedValidPipeAccessRights);
      switch (controlType)
      {
        case AccessControlType.Allow:
          rights |= PipeAccessRights.Synchronize;
          break;
        case AccessControlType.Deny:
          if (rights != PipeAccessRights.FullControl)
          {
            rights &= ~PipeAccessRights.Synchronize;
            break;
          }
          break;
      }
      return (int) rights;
    }

    internal static PipeAccessRights RightsFromAccessMask(int accessMask) => (PipeAccessRights) accessMask;
  }
}
