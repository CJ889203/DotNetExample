// Decompiled with JetBrains decompiler
// Type: System.Reflection.ParameterModifier
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Attaches a modifier to parameters so that binding can work with parameter signatures in which the types have been modified.</summary>
  public readonly struct ParameterModifier
  {

    #nullable disable
    private readonly bool[] _byRef;

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ParameterModifier" /> structure representing the specified number of parameters.</summary>
    /// <param name="parameterCount">The number of parameters.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="parameterCount" /> is negative.</exception>
    public ParameterModifier(int parameterCount) => this._byRef = parameterCount > 0 ? new bool[parameterCount] : throw new ArgumentException(SR.Arg_ParmArraySize);

    /// <summary>Gets or sets a value that specifies whether the parameter at the specified index position is to be modified by the current <see cref="T:System.Reflection.ParameterModifier" />.</summary>
    /// <param name="index">The index position of the parameter whose modification status is being examined or set.</param>
    /// <returns>
    /// <see langword="true" /> if the parameter at this index position is to be modified by this <see cref="T:System.Reflection.ParameterModifier" />; otherwise, <see langword="false" />.</returns>
    public bool this[int index]
    {
      get => this._byRef[index];
      set => this._byRef[index] = value;
    }


    #nullable enable
    internal bool[] IsByRefArray => this._byRef;
  }
}
