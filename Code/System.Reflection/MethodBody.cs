// Decompiled with JetBrains decompiler
// Type: System.Reflection.MethodBody
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides access to the metadata and MSIL for the body of a method.</summary>
  public class MethodBody
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.MethodBody" /> class.</summary>
    protected MethodBody()
    {
    }

    /// <summary>Gets a metadata token for the signature that describes the local variables for the method in metadata.</summary>
    /// <returns>An integer that represents the metadata token.</returns>
    public virtual int LocalSignatureMetadataToken => 0;

    /// <summary>Gets the list of local variables declared in the method body.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IList`1" /> of <see cref="T:System.Reflection.LocalVariableInfo" /> objects that describe the local variables declared in the method body.</returns>
    public virtual IList<LocalVariableInfo> LocalVariables => throw new ArgumentNullException("array");

    /// <summary>Gets the maximum number of items on the operand stack when the method is executing.</summary>
    /// <returns>The maximum number of items on the operand stack when the method is executing.</returns>
    public virtual int MaxStackSize => 0;

    /// <summary>Gets a value indicating whether local variables in the method body are initialized to the default values for their types.</summary>
    /// <returns>
    /// <see langword="true" /> if the method body contains code to initialize local variables to <see langword="null" /> for reference types, or to the zero-initialized value for value types; otherwise, <see langword="false" />.</returns>
    public virtual bool InitLocals => false;

    /// <summary>Returns the MSIL for the method body, as an array of bytes.</summary>
    /// <returns>An array of type <see cref="T:System.Byte" /> that contains the MSIL for the method body.</returns>
    public virtual byte[]? GetILAsByteArray() => (byte[]) null;

    /// <summary>Gets a list that includes all the exception-handling clauses in the method body.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IList`1" /> of <see cref="T:System.Reflection.ExceptionHandlingClause" /> objects representing the exception-handling clauses in the body of the method.</returns>
    public virtual IList<ExceptionHandlingClause> ExceptionHandlingClauses => throw new ArgumentNullException("array");
  }
}
