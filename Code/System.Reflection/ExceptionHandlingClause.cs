// Decompiled with JetBrains decompiler
// Type: System.Reflection.ExceptionHandlingClause
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents a clause in a structured exception-handling block.</summary>
  public class ExceptionHandlingClause
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ExceptionHandlingClause" /> class.</summary>
    protected ExceptionHandlingClause()
    {
    }

    /// <summary>Gets a value indicating whether this exception-handling clause is a finally clause, a type-filtered clause, or a user-filtered clause.</summary>
    /// <returns>An <see cref="T:System.Reflection.ExceptionHandlingClauseOptions" /> value that indicates what kind of action this clause performs.</returns>
    public virtual ExceptionHandlingClauseOptions Flags => ExceptionHandlingClauseOptions.Clause;

    /// <summary>The offset within the method, in bytes, of the try block that includes this exception-handling clause.</summary>
    /// <returns>An integer that represents the offset within the method, in bytes, of the try block that includes this exception-handling clause.</returns>
    public virtual int TryOffset => 0;

    /// <summary>The total length, in bytes, of the try block that includes this exception-handling clause.</summary>
    /// <returns>The total length, in bytes, of the try block that includes this exception-handling clause.</returns>
    public virtual int TryLength => 0;

    /// <summary>Gets the offset within the method body, in bytes, of this exception-handling clause.</summary>
    /// <returns>An integer that represents the offset within the method body, in bytes, of this exception-handling clause.</returns>
    public virtual int HandlerOffset => 0;

    /// <summary>Gets the length, in bytes, of the body of this exception-handling clause.</summary>
    /// <returns>An integer that represents the length, in bytes, of the MSIL that forms the body of this exception-handling clause.</returns>
    public virtual int HandlerLength => 0;

    /// <summary>Gets the offset within the method body, in bytes, of the user-supplied filter code.</summary>
    /// <exception cref="T:System.InvalidOperationException">Cannot get the offset because the exception handling clause is not a filter.</exception>
    /// <returns>The offset within the method body, in bytes, of the user-supplied filter code. The value of this property has no meaning if the <see cref="P:System.Reflection.ExceptionHandlingClause.Flags" /> property has any value other than <see cref="F:System.Reflection.ExceptionHandlingClauseOptions.Filter" />.</returns>
    public virtual int FilterOffset => throw new InvalidOperationException(SR.Arg_EHClauseNotFilter);

    /// <summary>Gets the type of exception handled by this clause.</summary>
    /// <exception cref="T:System.InvalidOperationException">Invalid use of property for the object's current state.</exception>
    /// <returns>A <see cref="T:System.Type" /> object that represents that type of exception handled by this clause, or <see langword="null" /> if the <see cref="P:System.Reflection.ExceptionHandlingClause.Flags" /> property is <see cref="F:System.Reflection.ExceptionHandlingClauseOptions.Filter" /> or <see cref="F:System.Reflection.ExceptionHandlingClauseOptions.Finally" />.</returns>
    public virtual Type? CatchType => (Type) null;

    /// <summary>A string representation of the exception-handling clause.</summary>
    /// <returns>A string that lists appropriate property values for the filter clause type.</returns>
    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 6);
      interpolatedStringHandler.AppendLiteral("Flags=");
      interpolatedStringHandler.AppendFormatted<ExceptionHandlingClauseOptions>(this.Flags);
      interpolatedStringHandler.AppendLiteral(", TryOffset=");
      interpolatedStringHandler.AppendFormatted<int>(this.TryOffset);
      interpolatedStringHandler.AppendLiteral(", TryLength=");
      interpolatedStringHandler.AppendFormatted<int>(this.TryLength);
      interpolatedStringHandler.AppendLiteral(", HandlerOffset=");
      interpolatedStringHandler.AppendFormatted<int>(this.HandlerOffset);
      interpolatedStringHandler.AppendLiteral(", HandlerLength=");
      interpolatedStringHandler.AppendFormatted<int>(this.HandlerLength);
      interpolatedStringHandler.AppendLiteral(", CatchType=");
      interpolatedStringHandler.AppendFormatted<Type>(this.CatchType);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
