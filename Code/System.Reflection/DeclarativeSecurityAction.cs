// Decompiled with JetBrains decompiler
// Type: System.Reflection.DeclarativeSecurityAction
// Assembly: System.Reflection.Metadata, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: FDD13CB9-4DB5-4759-8B88-2D188C369E68
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.Metadata.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.Metadata.xml

namespace System.Reflection
{
  /// <summary>Specifies the security actions that can be performed using declarative security.</summary>
  public enum DeclarativeSecurityAction : short
  {
    /// <summary>No declarative security action.</summary>
    None = 0,
    /// <summary>Check that all callers in the call chain have been granted the specified permission.</summary>
    Demand = 2,
    /// <summary>The calling code can access the resource identified by the current permission object, even if callers higher in the stack have not been granted permission to access the resource.</summary>
    Assert = 3,
    /// <summary>Without further checks refuse Demand for the specified permission.</summary>
    Deny = 4,
    /// <summary>Without further checks, refuse the demand for all permissions other than those specified.</summary>
    PermitOnly = 5,
    /// <summary>Check that the immediate caller has been granted the specified permission.</summary>
    LinkDemand = 6,
    /// <summary>The derived class inheriting the class or overriding a method is required to have the specified permission.</summary>
    InheritanceDemand = 7,
    /// <summary>Request the minimum permissions required for code to run. This action can only be used within the scope of the assembly.</summary>
    RequestMinimum = 8,
    /// <summary>Request additional permissions that are optional (not required to run). This request implicitly refuses all other permissions not specifically requested. This action can only be used within the scope of the assembly.</summary>
    RequestOptional = 9,
    /// <summary>Request that permissions that might be misused not be granted to the calling code. This action can only be used within the scope of the assembly.</summary>
    RequestRefuse = 10, // 0x000A
  }
}
