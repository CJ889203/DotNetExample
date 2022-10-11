// Decompiled with JetBrains decompiler
// Type: System.Reflection.LocalVariableInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;


#nullable enable
namespace System.Reflection
{
  /// <summary>Discovers the attributes of a local variable and provides access to local variable metadata.</summary>
  public class LocalVariableInfo
  {
    /// <summary>Gets the type of the local variable.</summary>
    /// <returns>The type of the local variable.</returns>
    public virtual Type LocalType => (Type) null;

    /// <summary>Gets the index of the local variable within the method body.</summary>
    /// <returns>An integer value that represents the order of declaration of the local variable within the method body.</returns>
    public virtual int LocalIndex => 0;

    /// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the object referred to by the local variable is pinned in memory.</summary>
    /// <returns>
    /// <see langword="true" /> if the object referred to by the variable is pinned in memory; otherwise, <see langword="false" />.</returns>
    public virtual bool IsPinned => false;

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.LocalVariableInfo" /> class.</summary>
    protected LocalVariableInfo()
    {
    }

    /// <summary>Returns a user-readable string that describes the local variable.</summary>
    /// <returns>A string that displays information about the local variable, including the type name, index, and pinned status.</returns>
    public override string ToString()
    {
      if (!this.IsPinned)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
        interpolatedStringHandler.AppendFormatted<Type>(this.LocalType);
        interpolatedStringHandler.AppendLiteral(" (");
        interpolatedStringHandler.AppendFormatted<int>(this.LocalIndex);
        interpolatedStringHandler.AppendLiteral(")");
        return interpolatedStringHandler.ToStringAndClear();
      }
      DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(12, 2);
      interpolatedStringHandler1.AppendFormatted<Type>(this.LocalType);
      interpolatedStringHandler1.AppendLiteral(" (");
      interpolatedStringHandler1.AppendFormatted<int>(this.LocalIndex);
      interpolatedStringHandler1.AppendLiteral(") (pinned)");
      return interpolatedStringHandler1.ToStringAndClear();
    }
  }
}
