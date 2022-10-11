// Decompiled with JetBrains decompiler
// Type: System.Reflection.ExceptionHandlingClauseOptions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Identifies kinds of exception-handling clauses.</summary>
  [Flags]
  public enum ExceptionHandlingClauseOptions
  {
    /// <summary>The clause accepts all exceptions that derive from a specified type.</summary>
    Clause = 0,
    /// <summary>The clause contains user-specified instructions that determine whether the exception should be ignored (that is, whether normal execution should resume), be handled by the associated handler, or be passed on to the next clause.</summary>
    Filter = 1,
    /// <summary>The clause is executed whenever the try block exits, whether through normal control flow or because of an unhandled exception.</summary>
    Finally = 2,
    /// <summary>The clause is executed if an exception occurs, but not on completion of normal control flow.</summary>
    Fault = 4,
  }
}
