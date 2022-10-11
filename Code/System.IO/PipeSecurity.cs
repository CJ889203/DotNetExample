// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipeSecurity
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.AccessControl.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Represents the access control and audit security for a pipe.</summary>
  public class PipeSecurity : NativeObjectSecurity
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.PipeSecurity" /> class.</summary>
    public PipeSecurity()
      : base(false, ResourceType.KernelObject)
    {
    }


    #nullable disable
    internal PipeSecurity(SafePipeHandle safeHandle, AccessControlSections includeSections)
      : base(false, ResourceType.KernelObject, (SafeHandle) safeHandle, includeSections)
    {
    }


    #nullable enable
    /// <summary>Adds an access rule to the Discretionary Access Control List (DACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The access rule to add.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    public void AddAccessRule(PipeAccessRule rule)
    {
      if (rule == null)
        throw new ArgumentNullException(nameof (rule));
      this.AddAccessRule((AccessRule) rule);
    }

    /// <summary>Sets an access rule in the Discretionary Access Control List (DACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The rule to set.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    public void SetAccessRule(PipeAccessRule rule)
    {
      if (rule == null)
        throw new ArgumentNullException(nameof (rule));
      this.SetAccessRule((AccessRule) rule);
    }

    /// <summary>Removes all access rules in the Discretionary Access Control List (DACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object and then adds the specified access rule.</summary>
    /// <param name="rule">The access rule to add.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    public void ResetAccessRule(PipeAccessRule rule)
    {
      if (rule == null)
        throw new ArgumentNullException(nameof (rule));
      this.ResetAccessRule((AccessRule) rule);
    }

    /// <summary>Removes an access rule from the Discretionary Access Control List (DACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The access rule to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the operation is successful; otherwise, <see langword="false" />.</returns>
    public bool RemoveAccessRule(PipeAccessRule rule)
    {
      AuthorizationRuleCollection authorizationRuleCollection = rule != null ? this.GetAccessRules(true, true, rule.IdentityReference.GetType()) : throw new ArgumentNullException(nameof (rule));
      for (int index = 0; index < authorizationRuleCollection.Count; ++index)
      {
        if (authorizationRuleCollection[index] is PipeAccessRule pipeAccessRule && pipeAccessRule.PipeAccessRights == rule.PipeAccessRights && pipeAccessRule.IdentityReference == rule.IdentityReference && pipeAccessRule.AccessControlType == rule.AccessControlType)
          return this.RemoveAccessRule((AccessRule) rule);
      }
      return rule.PipeAccessRights != PipeAccessRights.FullControl ? this.RemoveAccessRule((AccessRule) new PipeAccessRule(rule.IdentityReference, PipeAccessRule.AccessMaskFromRights(rule.PipeAccessRights, AccessControlType.Deny), false, rule.AccessControlType)) : this.RemoveAccessRule((AccessRule) rule);
    }

    /// <summary>Removes the specified access rule from the Discretionary Access Control List (DACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The access rule to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    public void RemoveAccessRuleSpecific(PipeAccessRule rule)
    {
      AuthorizationRuleCollection authorizationRuleCollection = rule != null ? this.GetAccessRules(true, true, rule.IdentityReference.GetType()) : throw new ArgumentNullException(nameof (rule));
      for (int index = 0; index < authorizationRuleCollection.Count; ++index)
      {
        if (authorizationRuleCollection[index] is PipeAccessRule pipeAccessRule && pipeAccessRule.PipeAccessRights == rule.PipeAccessRights && pipeAccessRule.IdentityReference == rule.IdentityReference && pipeAccessRule.AccessControlType == rule.AccessControlType)
        {
          this.RemoveAccessRuleSpecific((AccessRule) rule);
          return;
        }
      }
      if (rule.PipeAccessRights != PipeAccessRights.FullControl)
        this.RemoveAccessRuleSpecific((AccessRule) new PipeAccessRule(rule.IdentityReference, PipeAccessRule.AccessMaskFromRights(rule.PipeAccessRights, AccessControlType.Deny), false, rule.AccessControlType));
      else
        this.RemoveAccessRuleSpecific((AccessRule) rule);
    }

    /// <summary>Adds an audit rule to the System Access Control List (SACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The audit rule to add.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    public void AddAuditRule(PipeAuditRule rule) => this.AddAuditRule((AuditRule) rule);

    /// <summary>Sets an audit rule in the System Access Control List (SACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The rule to set.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    public void SetAuditRule(PipeAuditRule rule) => this.SetAuditRule((AuditRule) rule);

    /// <summary>Removes an audit rule from the System Access Control List (SACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The audit rule to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the audit rule was removed; otherwise, <see langword="false" />.</returns>
    public bool RemoveAuditRule(PipeAuditRule rule) => this.RemoveAuditRule((AuditRule) rule);

    /// <summary>Removes all audit rules that have the same security identifier as the specified audit rule from the System Access Control List (SACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The audit rule to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    public void RemoveAuditRuleAll(PipeAuditRule rule) => this.RemoveAuditRuleAll((AuditRule) rule);

    /// <summary>Removes the specified audit rule from the System Access Control List (SACL) that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <param name="rule">The audit rule to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="rule" /> parameter is <see langword="null" />.</exception>
    public void RemoveAuditRuleSpecific(PipeAuditRule rule) => this.RemoveAuditRuleSpecific((AuditRule) rule);

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.AccessRule" /> class with the specified values.</summary>
    /// <param name="identityReference">The identity that the access rule applies to. It must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</param>
    /// <param name="accessMask">The access mask of this rule. The access mask is a 32-bit collection of anonymous bits, the meaning of which is defined by the individual integrators.</param>
    /// <param name="isInherited">
    /// <see langword="true" /> if this rule is inherited from a parent container; otherwise, <see langword="false" />.</param>
    /// <param name="inheritanceFlags">One of the <see cref="T:System.Security.AccessControl.InheritanceFlags" /> values that specifies the inheritance properties of the access rule.</param>
    /// <param name="propagationFlags">One of the <see cref="T:System.Security.AccessControl.PropagationFlags" /> values that specifies whether inherited access rules are automatically propagated. The propagation flags are ignored if <paramref name="inheritanceFlags" /> is set to <see cref="F:System.Security.AccessControl.InheritanceFlags.None" />.</param>
    /// <param name="type">Specifies the valid access control type.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="accessMask" />, <paramref name="inheritanceFlags" />, <paramref name="propagationFlags" />, or <paramref name="type" /> specifies an invalid value.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="identityReference" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="accessMask" /> is zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="identityReference" /> is neither of type <see cref="T:System.Security.Principal.SecurityIdentifier" /> nor of a type, such as <see cref="T:System.Security.Principal.NTAccount" />, that can be converted to type <see cref="T:System.Security.Principal.SecurityIdentifier" />.</exception>
    /// <returns>The <see cref="T:System.Security.AccessControl.AccessRule" /> object that this method creates.</returns>
    public override AccessRule AccessRuleFactory(
      IdentityReference identityReference,
      int accessMask,
      bool isInherited,
      InheritanceFlags inheritanceFlags,
      PropagationFlags propagationFlags,
      AccessControlType type)
    {
      if (inheritanceFlags != InheritanceFlags.None)
        throw new ArgumentException(SR.Argument_NonContainerInvalidAnyFlag, nameof (inheritanceFlags));
      if (propagationFlags != PropagationFlags.None)
        throw new ArgumentException(SR.Argument_NonContainerInvalidAnyFlag, nameof (propagationFlags));
      return (AccessRule) new PipeAccessRule(identityReference, accessMask, isInherited, type);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.AuditRule" /> class with the specified values.</summary>
    /// <param name="identityReference">The identity that the access rule applies to. It must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</param>
    /// <param name="accessMask">The access mask of this rule. The access mask is a 32-bit collection of anonymous bits, the meaning of which is defined by the individual integrators.</param>
    /// <param name="isInherited">
    /// <see langword="true" /> if this rule is inherited from a parent container; otherwise, false.</param>
    /// <param name="inheritanceFlags">One of the <see cref="T:System.Security.AccessControl.InheritanceFlags" /> values that specifies the inheritance properties of the access rule.</param>
    /// <param name="propagationFlags">One of the <see cref="T:System.Security.AccessControl.PropagationFlags" /> values that specifies whether inherited access rules are automatically propagated. The propagation flags are ignored if <paramref name="inheritanceFlags" /> is set to <see cref="F:System.Security.AccessControl.InheritanceFlags.None" />.</param>
    /// <param name="flags">One of the <see cref="T:System.Security.AccessControl.AuditFlags" /> values that specifies the valid access control type.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="accessMask" />, <paramref name="inheritanceFlags" />, <paramref name="propagationFlags" />, or <paramref name="flags" /> properties specify an invalid value.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="identityReference" /> property is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="accessMask" /> property is zero.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="identityReference" /> property is neither of type <see cref="T:System.Security.Principal.SecurityIdentifier" /> nor of a type, such as <see cref="T:System.Security.Principal.NTAccount" />, that can be converted to type <see cref="T:System.Security.Principal.SecurityIdentifier" />.</exception>
    /// <returns>The <see cref="T:System.Security.AccessControl.AuditRule" /> object that this method creates.</returns>
    public override sealed AuditRule AuditRuleFactory(
      IdentityReference identityReference,
      int accessMask,
      bool isInherited,
      InheritanceFlags inheritanceFlags,
      PropagationFlags propagationFlags,
      AuditFlags flags)
    {
      if (inheritanceFlags != InheritanceFlags.None)
        throw new ArgumentException(SR.Argument_NonContainerInvalidAnyFlag, nameof (inheritanceFlags));
      if (propagationFlags != PropagationFlags.None)
        throw new ArgumentException(SR.Argument_NonContainerInvalidAnyFlag, nameof (propagationFlags));
      return (AuditRule) new PipeAuditRule(identityReference, accessMask, isInherited, flags);
    }

    private AccessControlSections GetAccessControlSectionsFromChanges()
    {
      AccessControlSections sectionsFromChanges = AccessControlSections.None;
      if (this.AccessRulesModified)
        sectionsFromChanges = AccessControlSections.Access;
      if (this.AuditRulesModified)
        sectionsFromChanges |= AccessControlSections.Audit;
      if (this.OwnerModified)
        sectionsFromChanges |= AccessControlSections.Owner;
      if (this.GroupModified)
        sectionsFromChanges |= AccessControlSections.Group;
      return sectionsFromChanges;
    }

    /// <summary>Saves the specified sections of the security descriptor that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object to permanent storage.</summary>
    /// <param name="handle">The handle of the securable object that the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object is associated with.</param>
    protected internal void Persist(SafeHandle handle)
    {
      this.WriteLock();
      try
      {
        AccessControlSections sectionsFromChanges = this.GetAccessControlSectionsFromChanges();
        this.Persist(handle, sectionsFromChanges);
        this.OwnerModified = this.GroupModified = this.AuditRulesModified = this.AccessRulesModified = false;
      }
      finally
      {
        this.WriteUnlock();
      }
    }

    /// <summary>Saves the specified sections of the security descriptor that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object to permanent storage.</summary>
    /// <param name="name">The name of the securable object that the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object is associated with.</param>
    protected internal void Persist(string name)
    {
      this.WriteLock();
      try
      {
        AccessControlSections sectionsFromChanges = this.GetAccessControlSectionsFromChanges();
        this.Persist(name, sectionsFromChanges);
        this.OwnerModified = this.GroupModified = this.AuditRulesModified = this.AccessRulesModified = false;
      }
      finally
      {
        this.WriteUnlock();
      }
    }

    /// <summary>Gets the <see cref="T:System.Type" /> of the securable object that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <returns>The type of the securable object that is associated with the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</returns>
    public override Type AccessRightType => typeof (PipeAccessRights);

    /// <summary>Gets the <see cref="T:System.Type" /> of the object that is associated with the access rules of the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <returns>The type of the object that is associated with the access rules of the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</returns>
    public override Type AccessRuleType => typeof (PipeAccessRule);

    /// <summary>Gets the <see cref="T:System.Type" /> object associated with the audit rules of the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</summary>
    /// <returns>The type of the object that is associated with the audit rules of the current <see cref="T:System.IO.Pipes.PipeSecurity" /> object.</returns>
    public override Type AuditRuleType => typeof (PipeAuditRule);
  }
}
