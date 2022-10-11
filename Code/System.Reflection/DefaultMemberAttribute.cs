// Decompiled with JetBrains decompiler
// Type: System.Reflection.DefaultMemberAttribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Defines the member of a type that is the default member used by <see cref="M:System.Type.InvokeMember(System.String,System.Reflection.BindingFlags,System.Reflection.Binder,System.Object,System.Object[],System.Reflection.ParameterModifier[],System.Globalization.CultureInfo,System.String[])" />.</summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
  public sealed class DefaultMemberAttribute : Attribute
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.DefaultMemberAttribute" /> class.</summary>
    /// <param name="memberName">A <see langword="String" /> containing the name of the member to invoke. This may be a constructor, method, property, or field. A suitable invocation attribute must be specified when the member is invoked. The default member of a class can be specified by passing an empty <see langword="String" /> as the name of the member.
    /// 
    /// The default member of a type is marked with the <see langword="DefaultMemberAttribute" /> custom attribute or marked in COM in the usual way.</param>
    public DefaultMemberAttribute(string memberName) => this.MemberName = memberName;

    /// <summary>Gets the name from the attribute.</summary>
    /// <returns>A string representing the member name.</returns>
    public string MemberName { get; }
  }
}
