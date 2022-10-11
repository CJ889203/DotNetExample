// Decompiled with JetBrains decompiler
// Type: System.Reflection.ReflectionTypeLoadException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;


#nullable enable
namespace System.Reflection
{
  /// <summary>The exception that is thrown by the <see cref="M:System.Reflection.Module.GetTypes" /> method if any of the classes in a module cannot be loaded. This class cannot be inherited.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public sealed class ReflectionTypeLoadException : SystemException, ISerializable
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ReflectionTypeLoadException" /> class with the given classes and their associated exceptions.</summary>
    /// <param name="classes">An array of type <see langword="Type" /> containing the classes that were defined in the module and loaded. This array can contain null reference (<see langword="Nothing" /> in Visual Basic) values.</param>
    /// <param name="exceptions">An array of type <see langword="Exception" /> containing the exceptions that were thrown by the class loader. The null reference (<see langword="Nothing" /> in Visual Basic) values in the <paramref name="classes" /> array line up with the exceptions in this <paramref name="exceptions" /> array.</param>
    public ReflectionTypeLoadException(Type?[]? classes, Exception?[]? exceptions)
      : this(classes, exceptions, (string) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ReflectionTypeLoadException" /> class with the given classes, their associated exceptions, and exception descriptions.</summary>
    /// <param name="classes">An array of type <see langword="Type" /> containing the classes that were defined in the module and loaded. This array can contain null reference (<see langword="Nothing" /> in Visual Basic) values.</param>
    /// <param name="exceptions">An array of type <see langword="Exception" /> containing the exceptions that were thrown by the class loader. The null reference (<see langword="Nothing" /> in Visual Basic) values in the <paramref name="classes" /> array line up with the exceptions in this <paramref name="exceptions" /> array.</param>
    /// <param name="message">A <see langword="String" /> describing the reason the exception was thrown.</param>
    public ReflectionTypeLoadException(Type?[]? classes, Exception?[]? exceptions, string? message)
      : base(message)
    {
      this.Types = classes ?? Type.EmptyTypes;
      this.LoaderExceptions = exceptions ?? Array.Empty<Exception>();
      this.HResult = -2146232830;
    }


    #nullable disable
    private ReflectionTypeLoadException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.Types = Type.EmptyTypes;
      this.LoaderExceptions = (Exception[]) info.GetValue("Exceptions", typeof (Exception[])) ?? Array.Empty<Exception>();
    }


    #nullable enable
    /// <summary>Provides an <see cref="T:System.Runtime.Serialization.ISerializable" /> implementation for serialized objects.</summary>
    /// <param name="info">The information and data needed to serialize or deserialize an object.</param>
    /// <param name="context">The context for the serialization.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <see langword="info" /> is <see langword="null" />.</exception>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("Types", (object) null, typeof (Type[]));
      info.AddValue("Exceptions", (object) this.LoaderExceptions, typeof (Exception[]));
    }

    /// <summary>Gets the array of classes that were defined in the module and loaded.</summary>
    /// <returns>An array of type <see langword="Type" /> containing the classes that were defined in the module and loaded. This array can contain some <see langword="null" /> values.</returns>
    public Type?[] Types { get; }

    /// <summary>Gets the array of exceptions thrown by the class loader.</summary>
    /// <returns>An array of type <see langword="Exception" /> containing the exceptions thrown by the class loader. The null values in the <see cref="P:System.Reflection.ReflectionTypeLoadException.Types" /> array of this instance line up with the exceptions in this array.</returns>
    public Exception?[] LoaderExceptions { get; }

    /// <summary>Gets the error message for this exception.</summary>
    /// <returns>A string containing the error message for this exception.</returns>
    public override string Message => this.CreateString(true);

    /// <summary>Returns the fully qualified name of this exception and the messages for all the loader exceptions.</summary>
    /// <returns>A string containing the fully qualified name of this exception and the exception messages for its loader exceptions.</returns>
    public override string ToString() => this.CreateString(false);


    #nullable disable
    private string CreateString(bool isMessage)
    {
      string str = isMessage ? base.Message : base.ToString();
      Exception[] loaderExceptions = this.LoaderExceptions;
      if (loaderExceptions.Length == 0)
        return str;
      StringBuilder stringBuilder = new StringBuilder(str);
      foreach (Exception exception in loaderExceptions)
      {
        if (exception != null)
          stringBuilder.AppendLine().Append(isMessage ? exception.Message : exception.ToString());
      }
      return stringBuilder.ToString();
    }
  }
}
