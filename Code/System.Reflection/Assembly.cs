// Decompiled with JetBrains decompiler
// Type: System.Reflection.Assembly
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents an assembly, which is a reusable, versionable, and self-describing building block of a common language runtime application.</summary>
  public abstract class Assembly : ICustomAttributeProvider, ISerializable
  {

    #nullable disable
    private static readonly Dictionary<string, Assembly> s_loadfile = new Dictionary<string, Assembly>();
    private static readonly List<string> s_loadFromAssemblyList = new List<string>();
    private static bool s_loadFromHandlerSet;
    private static int s_cachedSerializationSwitch;
    private static bool s_forceNullEntryPoint;


    #nullable enable
    /// <summary>Loads an assembly with the specified name.</summary>
    /// <param name="assemblyString">The long or short form of the assembly name.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyString" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="assemblyString" /> is a zero-length string.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyString" /> is not found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyString" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyString" /> was compiled with a later version.</exception>
    /// <returns>The loaded assembly.</returns>
    public static Assembly Load(string assemblyString)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return (Assembly) RuntimeAssembly.InternalLoad(assemblyString, ref stackMark, AssemblyLoadContext.CurrentContextualReflectionContext);
    }

    /// <summary>Loads an assembly from the application directory or from the global assembly cache using a partial name.</summary>
    /// <param name="partialName">The display name of the assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="partialName" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="partialName" /> was compiled with a later version.</exception>
    /// <returns>The loaded assembly. If <paramref name="partialName" /> is not found, this method returns <see langword="null" />.</returns>
    [Obsolete("Assembly.LoadWithPartialName has been deprecated. Use Assembly.Load() instead.")]
    public static Assembly? LoadWithPartialName(string partialName)
    {
      switch (partialName)
      {
        case "":
          throw new ArgumentException(SR.Format_StringZeroLength, nameof (partialName));
        case null:
          throw new ArgumentNullException(nameof (partialName));
        default:
          if (partialName[0] != char.MinValue)
          {
            try
            {
              StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
              return (Assembly) RuntimeAssembly.InternalLoad(partialName, ref stackMark, AssemblyLoadContext.CurrentContextualReflectionContext);
            }
            catch (FileNotFoundException ex)
            {
              return (Assembly) null;
            }
          }
          else
            goto case "";
      }
    }

    /// <summary>Loads an assembly given its <see cref="T:System.Reflection.AssemblyName" />.</summary>
    /// <param name="assemblyRef">The object that describes the assembly to be loaded.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyRef" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyRef" /> is not found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyRef" /> specifies a remote assembly, but the ability to execute code in remote assemblies is disabled. See &lt;loadFromRemoteSources&gt;.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.IO.IOException" />, instead.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyRef" /> is not a valid assembly. -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyRef" /> was compiled with a later version.</exception>
    /// <returns>The loaded assembly.</returns>
    public static Assembly Load(AssemblyName assemblyRef)
    {
      if (assemblyRef == null)
        throw new ArgumentNullException(nameof (assemblyRef));
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return (Assembly) RuntimeAssembly.InternalLoad(assemblyRef, ref stackMark, AssemblyLoadContext.CurrentContextualReflectionContext);
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void GetExecutingAssemblyNative(
      StackCrawlMarkHandle stackMark,
      ObjectHandleOnStack retAssembly);


    #nullable disable
    internal static RuntimeAssembly GetExecutingAssembly(ref StackCrawlMark stackMark)
    {
      RuntimeAssembly o = (RuntimeAssembly) null;
      Assembly.GetExecutingAssemblyNative(new StackCrawlMarkHandle(ref stackMark), ObjectHandleOnStack.Create<RuntimeAssembly>(ref o));
      return o;
    }


    #nullable enable
    /// <summary>Gets the assembly that contains the code that is currently executing.</summary>
    /// <returns>The assembly that contains the code that is currently executing.</returns>
    public static Assembly GetExecutingAssembly()
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return (Assembly) Assembly.GetExecutingAssembly(ref stackMark);
    }

    /// <summary>Returns the <see cref="T:System.Reflection.Assembly" /> of the method that invoked the currently executing method.</summary>
    /// <returns>The <see langword="Assembly" /> object of the method that invoked the currently executing method.</returns>
    public static Assembly GetCallingAssembly()
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCallersCaller;
      return (Assembly) Assembly.GetExecutingAssembly(ref stackMark);
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void GetEntryAssemblyNative(ObjectHandleOnStack retAssembly);


    #nullable disable
    private static Assembly GetEntryAssemblyInternal()
    {
      RuntimeAssembly o = (RuntimeAssembly) null;
      Assembly.GetEntryAssemblyNative(ObjectHandleOnStack.Create<RuntimeAssembly>(ref o));
      return (Assembly) o;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool IsRuntimeImplemented() => this is RuntimeAssembly;

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern uint GetAssemblyCount();


    #nullable enable
    /// <summary>Gets a collection of the types defined in this assembly.</summary>
    /// <returns>A collection of the types defined in this assembly.</returns>
    public virtual IEnumerable<TypeInfo> DefinedTypes
    {
      [RequiresUnreferencedCode("Types might be removed")] get
      {
        Type[] types = this.GetTypes();
        TypeInfo[] definedTypes = new TypeInfo[types.Length];
        for (int index = 0; index < types.Length; ++index)
        {
          TypeInfo typeInfo = types[index].GetTypeInfo();
          definedTypes[index] = !((Type) typeInfo == (Type) null) ? typeInfo : throw new NotSupportedException(SR.Format(SR.NotSupported_NoTypeInfo, (object) types[index].FullName));
        }
        return (IEnumerable<TypeInfo>) definedTypes;
      }
    }

    /// <summary>Gets the types defined in this assembly.</summary>
    /// <exception cref="T:System.Reflection.ReflectionTypeLoadException">The assembly contains one or more types that cannot be loaded. The array returned by the <see cref="P:System.Reflection.ReflectionTypeLoadException.Types" /> property of this exception contains a <see cref="T:System.Type" /> object for each type that was loaded and <see langword="null" /> for each type that could not be loaded, while the <see cref="P:System.Reflection.ReflectionTypeLoadException.LoaderExceptions" /> property contains an exception for each type that could not be loaded.</exception>
    /// <returns>An array that contains all the types that are defined in this assembly.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type[] GetTypes()
    {
      Module[] modules = this.GetModules(false);
      if (modules.Length == 1)
        return modules[0].GetTypes();
      int length1 = 0;
      Type[][] typeArray = new Type[modules.Length][];
      for (int index = 0; index < typeArray.Length; ++index)
      {
        typeArray[index] = modules[index].GetTypes();
        length1 += typeArray[index].Length;
      }
      int destinationIndex = 0;
      Type[] destinationArray = new Type[length1];
      for (int index = 0; index < typeArray.Length; ++index)
      {
        int length2 = typeArray[index].Length;
        Array.Copy((Array) typeArray[index], 0, (Array) destinationArray, destinationIndex, length2);
        destinationIndex += length2;
      }
      return destinationArray;
    }

    /// <summary>Gets a collection of the public types defined in this assembly that are visible outside the assembly.</summary>
    /// <returns>A collection of the public types defined in this assembly that are visible outside the assembly.</returns>
    public virtual IEnumerable<Type> ExportedTypes
    {
      [RequiresUnreferencedCode("Types might be removed")] get => (IEnumerable<Type>) this.GetExportedTypes();
    }

    /// <summary>Gets the public types defined in this assembly that are visible outside the assembly.</summary>
    /// <exception cref="T:System.NotSupportedException">The assembly is a dynamic assembly.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">Unable to load a dependent assembly.</exception>
    /// <returns>An array that represents the types defined in this assembly that are visible outside the assembly.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type[] GetExportedTypes() => throw NotImplemented.ByDesign;

    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type[] GetForwardedTypes() => throw NotImplemented.ByDesign;

    /// <summary>Gets the location of the assembly as specified originally, for example, in an <see cref="T:System.Reflection.AssemblyName" /> object.</summary>
    /// <exception cref="T:System.NotImplementedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <returns>The location of the assembly as specified originally.</returns>
    [RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
    public virtual string? CodeBase => throw NotImplemented.ByDesign;

    /// <summary>Gets the entry point of this assembly.</summary>
    /// <returns>An object that represents the entry point of this assembly. If no entry point is found (for example, the assembly is a DLL), <see langword="null" /> is returned.</returns>
    public virtual MethodInfo? EntryPoint => throw NotImplemented.ByDesign;

    /// <summary>Gets the display name of the assembly.</summary>
    /// <returns>The display name of the assembly.</returns>
    public virtual string? FullName => throw NotImplemented.ByDesign;

    /// <summary>Gets a string representing the version of the common language runtime (CLR) saved in the file containing the manifest.</summary>
    /// <returns>The CLR version folder name. This is not a full path.</returns>
    public virtual string ImageRuntimeVersion => throw NotImplemented.ByDesign;

    /// <summary>Gets a value that indicates whether the current assembly was generated dynamically in the current process by using reflection emit.</summary>
    /// <returns>
    /// <see langword="true" /> if the current assembly was generated dynamically in the current process; otherwise, <see langword="false" />.</returns>
    public virtual bool IsDynamic => false;

    /// <summary>Gets the full path or UNC location of the loaded file that contains the manifest.</summary>
    /// <exception cref="T:System.NotSupportedException">The current assembly is a dynamic assembly, represented by an <see cref="T:System.Reflection.Emit.AssemblyBuilder" /> object.</exception>
    /// <returns>The location of the loaded file that contains the manifest. If the assembly is loaded from a byte array, such as when using <see cref="M:System.Reflection.Assembly.Load(System.Byte[])" />, the value returned is an empty string ("").</returns>
    public virtual string Location => throw NotImplemented.ByDesign;

    /// <summary>Gets a <see cref="T:System.Boolean" /> value indicating whether this assembly was loaded into the reflection-only context.</summary>
    /// <returns>
    /// <see langword="true" /> if the assembly was loaded into the reflection-only context, rather than the execution context; otherwise, <see langword="false" />.</returns>
    public virtual bool ReflectionOnly => throw NotImplemented.ByDesign;

    /// <summary>Gets a value that indicates whether this assembly is held in a collectible <see cref="T:System.Runtime.Loader.AssemblyLoadContext" />.</summary>
    /// <returns>
    /// <see langword="true" /> if this assembly is held in a collectible <see cref="T:System.Runtime.Loader.AssemblyLoadContext" />; otherwise, <see langword="false" />.</returns>
    public virtual bool IsCollectible => true;

    /// <summary>Returns information about how the given resource has been persisted.</summary>
    /// <param name="resourceName">The case-sensitive name of the resource.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="resourceName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="resourceName" /> parameter is an empty string ("").</exception>
    /// <returns>An object that is populated with information about the resource's topology, or <see langword="null" /> if the resource is not found.</returns>
    public virtual ManifestResourceInfo? GetManifestResourceInfo(
      string resourceName)
    {
      throw NotImplemented.ByDesign;
    }

    /// <summary>Returns the names of all the resources in this assembly.</summary>
    /// <returns>An array that contains the names of all the resources.</returns>
    public virtual string[] GetManifestResourceNames() => throw NotImplemented.ByDesign;

    /// <summary>Loads the specified manifest resource from this assembly.</summary>
    /// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is an empty string ("").</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.IO.IOException" />, instead.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="name" /> was not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    /// <paramref name="name" /> is not a valid assembly.</exception>
    /// <exception cref="T:System.NotImplementedException">Resource length is greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>The manifest resource; or <see langword="null" /> if no resources were specified during compilation or if the resource is not visible to the caller.</returns>
    public virtual Stream? GetManifestResourceStream(string name) => throw NotImplemented.ByDesign;

    /// <summary>Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly.</summary>
    /// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
    /// <param name="name">The case-sensitive name of the manifest resource being requested.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is an empty string ("").</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="name" /> was not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    /// <paramref name="name" /> is not a valid assembly.</exception>
    /// <exception cref="T:System.NotImplementedException">Resource length is greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>The manifest resource; or <see langword="null" /> if no resources were specified during compilation or if the resource is not visible to the caller.</returns>
    public virtual Stream? GetManifestResourceStream(Type type, string name) => throw NotImplemented.ByDesign;

    /// <summary>Gets a value that indicates whether the current assembly is loaded with full trust.</summary>
    /// <returns>
    /// <see langword="true" /> if the current assembly is loaded with full trust; otherwise, <see langword="false" />.</returns>
    public bool IsFullyTrusted => true;

    /// <summary>Gets an <see cref="T:System.Reflection.AssemblyName" /> for this assembly.</summary>
    /// <returns>An object that contains the fully parsed display name for this assembly.</returns>
    public virtual AssemblyName GetName() => this.GetName(false);

    /// <summary>Gets an <see cref="T:System.Reflection.AssemblyName" /> for this assembly, setting the codebase as specified by <paramref name="copiedName" />.</summary>
    /// <param name="copiedName">
    /// <see langword="true" /> to set the <see cref="P:System.Reflection.Assembly.CodeBase" /> to the location of the assembly after it was shadow copied; <see langword="false" /> to set <see cref="P:System.Reflection.Assembly.CodeBase" /> to the original location.</param>
    /// <returns>An object that contains the fully parsed display name for this assembly.</returns>
    public virtual AssemblyName GetName(bool copiedName) => throw NotImplemented.ByDesign;

    /// <summary>Gets the <see cref="T:System.Type" /> object with the specified name in the assembly instance.</summary>
    /// <param name="name">The full name of the type.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="name" /> is invalid.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="name" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///         <paramref name="name" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    ///  -or-
    /// 
    ///  The current assembly was loaded into the reflection-only context, and <paramref name="name" /> requires a dependent assembly that was not preloaded.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.IO.IOException" />, instead.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="name" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> requires a dependent assembly which was compiled for a version of the runtime later than the currently loaded version.</exception>
    /// <returns>An object that represents the specified class, or <see langword="null" /> if the class is not found.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type? GetType(string name) => this.GetType(name, false, false);

    /// <summary>Gets the <see cref="T:System.Type" /> object with the specified name in the assembly instance and optionally throws an exception if the type is not found.</summary>
    /// <param name="name">The full name of the type.</param>
    /// <param name="throwOnError">
    /// <see langword="true" /> to throw an exception if the type is not found; <see langword="false" /> to return <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="name" /> is invalid.
    /// 
    /// -or-
    /// 
    /// The length of <paramref name="name" /> exceeds 1024 characters.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="throwOnError" /> is <see langword="true" />, and the type cannot be found.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="name" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///        <paramref name="name" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context, and <paramref name="name" /> requires a dependent assembly that was not preloaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="name" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> requires a dependent assembly which was compiled for a version of the runtime later than the currently loaded version.</exception>
    /// <returns>An object that represents the specified class.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type? GetType(string name, bool throwOnError) => this.GetType(name, throwOnError, false);

    /// <summary>Gets the <see cref="T:System.Type" /> object with the specified name in the assembly instance, with the options of ignoring the case, and of throwing an exception if the type is not found.</summary>
    /// <param name="name">The full name of the type.</param>
    /// <param name="throwOnError">
    /// <see langword="true" /> to throw an exception if the type is not found; <see langword="false" /> to return <see langword="null" />.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore the case of the type name; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="name" /> is invalid.
    /// 
    /// -or-
    /// 
    /// The length of <paramref name="name" /> exceeds 1024 characters.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="throwOnError" /> is <see langword="true" />, and the type cannot be found.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="name" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///        <paramref name="name" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context, and <paramref name="name" /> requires a dependent assembly that was not preloaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="name" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> requires a dependent assembly which was compiled for a version of the runtime later than the currently loaded version.</exception>
    /// <returns>An object that represents the specified class.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type? GetType(string name, bool throwOnError, bool ignoreCase) => throw NotImplemented.ByDesign;

    /// <summary>Indicates whether or not a specified attribute has been applied to the assembly.</summary>
    /// <param name="attributeType">The type of the attribute to be checked for this assembly.</param>
    /// <param name="inherit">This argument is ignored for objects of this type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> uses an invalid type.</exception>
    /// <returns>
    /// <see langword="true" /> if the attribute has been applied to the assembly; otherwise, <see langword="false" />.</returns>
    public virtual bool IsDefined(Type attributeType, bool inherit) => throw NotImplemented.ByDesign;

    /// <summary>Gets a collection that contains this assembly's custom attributes.</summary>
    /// <returns>A collection that contains this assembly's custom attributes.</returns>
    public virtual IEnumerable<CustomAttributeData> CustomAttributes => (IEnumerable<CustomAttributeData>) this.GetCustomAttributesData();

    /// <summary>Returns information about the attributes that have been applied to the current <see cref="T:System.Reflection.Assembly" />, expressed as <see cref="T:System.Reflection.CustomAttributeData" /> objects.</summary>
    /// <returns>A generic list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the current assembly.</returns>
    public virtual IList<CustomAttributeData> GetCustomAttributesData() => throw NotImplemented.ByDesign;

    /// <summary>Gets all the custom attributes for this assembly.</summary>
    /// <param name="inherit">This argument is ignored for objects of type <see cref="T:System.Reflection.Assembly" />.</param>
    /// <returns>An array that contains the custom attributes for this assembly.</returns>
    public virtual object[] GetCustomAttributes(bool inherit) => throw NotImplemented.ByDesign;

    /// <summary>Gets the custom attributes for this assembly as specified by type.</summary>
    /// <param name="attributeType">The type for which the custom attributes are to be returned.</param>
    /// <param name="inherit">This argument is ignored for objects of type <see cref="T:System.Reflection.Assembly" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not a runtime type.</exception>
    /// <returns>An array that contains the custom attributes for this assembly as specified by <paramref name="attributeType" />.</returns>
    public virtual object[] GetCustomAttributes(Type attributeType, bool inherit) => throw NotImplemented.ByDesign;

    /// <summary>Gets the URI, including escape characters, that represents the codebase.</summary>
    /// <returns>A URI with escape characters.</returns>
    [RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
    public virtual string EscapedCodeBase => AssemblyName.EscapeCodeBase(this.CodeBase);

    /// <summary>Locates the specified type from this assembly and creates an instance of it using the system activator, using case-sensitive search.</summary>
    /// <param name="typeName">The <see cref="P:System.Type.FullName" /> of the type to locate.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="typeName" /> is an empty string ("") or a string beginning with a null character.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="typeName" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///        <paramref name="typeName" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context, and <paramref name="typeName" /> requires a dependent assembly that was not preloaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="typeName" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeName" /> requires a dependent assembly that was compiled for a version of the runtime that is later than the currently loaded version.</exception>
    /// <returns>An instance of the specified type created with the parameterless constructor; or <see langword="null" /> if <paramref name="typeName" /> is not found. The type is resolved using the default binder, without specifying culture or activation attributes, and with <see cref="T:System.Reflection.BindingFlags" /> set to <see langword="Public" /> or <see langword="Instance" />.</returns>
    [RequiresUnreferencedCode("Assembly.CreateInstance is not supported with trimming. Use Type.GetType instead.")]
    public object? CreateInstance(string typeName) => this.CreateInstance(typeName, false, BindingFlags.Instance | BindingFlags.Public, (Binder) null, (object[]) null, (CultureInfo) null, (object[]) null);

    /// <summary>Locates the specified type from this assembly and creates an instance of it using the system activator, with optional case-sensitive search.</summary>
    /// <param name="typeName">The <see cref="P:System.Type.FullName" /> of the type to locate.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore the case of the type name; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="typeName" /> is an empty string ("") or a string beginning with a null character.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="typeName" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///        <paramref name="typeName" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context, and <paramref name="typeName" /> requires a dependent assembly that was not preloaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="typeName" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeName" /> requires a dependent assembly that was compiled for a version of the runtime that is later than the currently loaded version.</exception>
    /// <returns>An instance of the specified type created with the parameterless constructor; or <see langword="null" /> if <paramref name="typeName" /> is not found. The type is resolved using the default binder, without specifying culture or activation attributes, and with <see cref="T:System.Reflection.BindingFlags" /> set to <see langword="Public" /> or <see langword="Instance" />.</returns>
    [RequiresUnreferencedCode("Assembly.CreateInstance is not supported with trimming. Use Type.GetType instead.")]
    public object? CreateInstance(string typeName, bool ignoreCase) => this.CreateInstance(typeName, ignoreCase, BindingFlags.Instance | BindingFlags.Public, (Binder) null, (object[]) null, (CultureInfo) null, (object[]) null);

    /// <summary>Locates the specified type from this assembly and creates an instance of it using the system activator, with optional case-sensitive search and having the specified culture, arguments, and binding and activation attributes.</summary>
    /// <param name="typeName">The <see cref="P:System.Type.FullName" /> of the type to locate.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore the case of the type name; otherwise, <see langword="false" />.</param>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see langword="MemberInfo" /> objects via reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="args">An array that contains the arguments to be passed to the constructor. This array of arguments must match in number, order, and type the parameters of the constructor to be invoked. If the parameterless constructor is desired, <paramref name="args" /> must be an empty array or <see langword="null" />.</param>
    /// <param name="culture">An instance of <see langword="CultureInfo" /> used to govern the coercion of types. If this is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. (This is necessary to convert a string that represents 1000 to a <see cref="T:System.Double" /> value, for example, since 1000 is represented differently by different cultures.)</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="typeName" /> is an empty string ("") or a string beginning with a null character.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.NotSupportedException">A non-empty activation attributes array is passed to a type that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="typeName" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///        <paramref name="typeName" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context, and <paramref name="typeName" /> requires a dependent assembly that was not preloaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="typeName" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeName" /> requires a dependent assembly which that was compiled for a version of the runtime that is later than the currently loaded version.</exception>
    /// <returns>An instance of the specified type, or <see langword="null" /> if <paramref name="typeName" /> is not found. The supplied arguments are used to resolve the type, and to bind the constructor that is used to create the instance.</returns>
    [RequiresUnreferencedCode("Assembly.CreateInstance is not supported with trimming. Use Type.GetType instead.")]
    public virtual object? CreateInstance(
      string typeName,
      bool ignoreCase,
      BindingFlags bindingAttr,
      Binder? binder,
      object[]? args,
      CultureInfo? culture,
      object[]? activationAttributes)
    {
      Type type = this.GetType(typeName, false, ignoreCase);
      return type == (Type) null ? (object) null : Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
    }

    /// <summary>Occurs when the common language runtime class loader cannot resolve a reference to an internal module of an assembly through normal means.</summary>
    public virtual event ModuleResolveEventHandler? ModuleResolve
    {
      add => throw NotImplemented.ByDesign;
      remove => throw NotImplemented.ByDesign;
    }

    /// <summary>Gets the module that contains the manifest for the current assembly.</summary>
    /// <returns>The module that contains the manifest for the assembly.</returns>
    public virtual Module ManifestModule => throw NotImplemented.ByDesign;

    /// <summary>Gets the specified module in this assembly.</summary>
    /// <param name="name">The name of the module being requested.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is an empty string ("").</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="name" /> was not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    /// <paramref name="name" /> is not a valid assembly.</exception>
    /// <returns>The module being requested, or <see langword="null" /> if the module is not found.</returns>
    public virtual Module? GetModule(string name) => throw NotImplemented.ByDesign;

    /// <summary>Gets all the modules that are part of this assembly.</summary>
    /// <exception cref="T:System.IO.FileNotFoundException">The module to be loaded does not specify a file name extension.</exception>
    /// <returns>An array of modules.</returns>
    public Module[] GetModules() => this.GetModules(false);

    /// <summary>Gets all the modules that are part of this assembly, specifying whether to include resource modules.</summary>
    /// <param name="getResourceModules">
    /// <see langword="true" /> to include resource modules; otherwise, <see langword="false" />.</param>
    /// <returns>An array of modules.</returns>
    public virtual Module[] GetModules(bool getResourceModules) => throw NotImplemented.ByDesign;

    /// <summary>Gets a collection that contains the modules in this assembly.</summary>
    /// <returns>A collection that contains the modules in this assembly.</returns>
    public virtual IEnumerable<Module> Modules => (IEnumerable<Module>) this.GetLoadedModules(true);

    /// <summary>Gets all the loaded modules that are part of this assembly.</summary>
    /// <returns>An array of modules.</returns>
    public Module[] GetLoadedModules() => this.GetLoadedModules(false);

    /// <summary>Gets all the loaded modules that are part of this assembly, specifying whether to include resource modules.</summary>
    /// <param name="getResourceModules">
    /// <see langword="true" /> to include resource modules; otherwise, <see langword="false" />.</param>
    /// <returns>An array of modules.</returns>
    public virtual Module[] GetLoadedModules(bool getResourceModules) => throw NotImplemented.ByDesign;

    /// <summary>Gets the <see cref="T:System.Reflection.AssemblyName" /> objects for all the assemblies referenced by this assembly.</summary>
    /// <returns>An array that contains the fully parsed display names of all the assemblies referenced by this assembly.</returns>
    [RequiresUnreferencedCode("Assembly references might be removed")]
    public virtual AssemblyName[] GetReferencedAssemblies() => throw NotImplemented.ByDesign;

    /// <summary>Gets the satellite assembly for the specified culture.</summary>
    /// <param name="culture">The specified culture.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="culture" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The assembly cannot be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">The satellite assembly with a matching file name was found, but the <see langword="CultureInfo" /> did not match the one specified.</exception>
    /// <exception cref="T:System.BadImageFormatException">The satellite assembly is not a valid assembly.</exception>
    /// <returns>The specified satellite assembly.</returns>
    public virtual Assembly GetSatelliteAssembly(CultureInfo culture) => throw NotImplemented.ByDesign;

    /// <summary>Gets the specified version of the satellite assembly for the specified culture.</summary>
    /// <param name="culture">The specified culture.</param>
    /// <param name="version">The version of the satellite assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="culture" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileLoadException">The satellite assembly with a matching file name was found, but the <see langword="CultureInfo" /> or the version did not match the one specified.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The assembly cannot be found.</exception>
    /// <exception cref="T:System.BadImageFormatException">The satellite assembly is not a valid assembly.</exception>
    /// <returns>The specified satellite assembly.</returns>
    public virtual Assembly GetSatelliteAssembly(CultureInfo culture, Version? version) => throw NotImplemented.ByDesign;

    /// <summary>Gets a <see cref="T:System.IO.FileStream" /> for the specified file in the file table of the manifest of this assembly.</summary>
    /// <param name="name">The name of the specified file. Do not include the path to the file.</param>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is an empty string ("").</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="name" /> was not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    /// <paramref name="name" /> is not a valid assembly.</exception>
    /// <returns>A stream that contains the specified file, or <see langword="null" /> if the file is not found.</returns>
    [RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
    public virtual FileStream? GetFile(string name) => throw NotImplemented.ByDesign;

    /// <summary>Gets the files in the file table of an assembly manifest.</summary>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">A file was not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">A file was not a valid assembly.</exception>
    /// <returns>An array of streams that contain the files.</returns>
    [RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
    public virtual FileStream[] GetFiles() => this.GetFiles(false);

    /// <summary>Gets the files in the file table of an assembly manifest, specifying whether to include resource modules.</summary>
    /// <param name="getResourceModules">
    /// <see langword="true" /> to include resource modules; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">A file was not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">A file was not a valid assembly.</exception>
    /// <returns>An array of streams that contain the files.</returns>
    [RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
    public virtual FileStream[] GetFiles(bool getResourceModules) => throw NotImplemented.ByDesign;

    /// <summary>Gets serialization information with all of the data needed to reinstantiate this assembly.</summary>
    /// <param name="info">The object to be populated with serialization information.</param>
    /// <param name="context">The destination context of the serialization.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => throw NotImplemented.ByDesign;

    /// <summary>Returns the full name of the assembly, also known as the display name.</summary>
    /// <returns>The full name of the assembly, or the class name if the full name of the assembly cannot be determined.</returns>
    public override string ToString() => this.FullName ?? base.ToString();

    /// <summary>Gets a value indicating whether the assembly was loaded from the global assembly cache (.NET Framework only).</summary>
    /// <returns>
    /// .NET Framework only: <see langword="true" /> if the assembly was loaded from the global assembly cache; otherwise, <see langword="false" />.
    /// 
    /// .NET Core and .NET 5.0 and later: <see langword="false" /> in all cases.</returns>
    [Obsolete("The Global Assembly Cache is not supported.", DiagnosticId = "SYSLIB0005", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public virtual bool GlobalAssemblyCache => throw NotImplemented.ByDesign;

    /// <summary>Gets the host context with which the assembly was loaded.</summary>
    /// <returns>An <see cref="T:System.Int64" /> value that indicates the host context with which the assembly was loaded, if any.</returns>
    public virtual long HostContext => throw NotImplemented.ByDesign;

    /// <summary>Determines whether this assembly and the specified object are equal.</summary>
    /// <param name="o">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="o" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? o) => base.Equals(o);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Indicates whether two <see cref="T:System.Reflection.Assembly" /> objects are equal.</summary>
    /// <param name="left">The assembly to compare to <paramref name="right" />.</param>
    /// <param name="right">The assembly to compare to <paramref name="left" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Assembly? left, Assembly? right)
    {
      if ((object) right == null)
        return (object) left == null;
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>Indicates whether two <see cref="T:System.Reflection.Assembly" /> objects are not equal.</summary>
    /// <param name="left">The assembly to compare to <paramref name="right" />.</param>
    /// <param name="right">The assembly to compare to <paramref name="left" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Assembly? left, Assembly? right) => !(left == right);

    /// <summary>Creates the name of a type qualified by the display name of its assembly.</summary>
    /// <param name="assemblyName">The display name of an assembly.</param>
    /// <param name="typeName">The full name of a type.</param>
    /// <returns>The full name of the type qualified by the display name of the assembly.</returns>
    public static string CreateQualifiedName(string? assemblyName, string? typeName) => typeName + ", " + assemblyName;

    /// <summary>Gets the currently loaded assembly in which the specified type is defined.</summary>
    /// <param name="type">An object representing a type in the assembly that will be returned.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <returns>The assembly in which the specified type is defined.</returns>
    public static Assembly? GetAssembly(Type type)
    {
      Module module = !(type == (Type) null) ? type.Module : throw new ArgumentNullException(nameof (type));
      return module == (Module) null ? (Assembly) null : module.Assembly;
    }

    /// <summary>Gets the process executable in the default application domain. In other application domains, this is the first executable that was executed by <see cref="M:System.AppDomain.ExecuteAssembly(System.String)" />.</summary>
    /// <returns>The assembly that is the process executable in the default application domain, or the first executable that was executed by <see cref="M:System.AppDomain.ExecuteAssembly(System.String)" />. Can return <see langword="null" /> when called from unmanaged code.</returns>
    public static Assembly? GetEntryAssembly() => Assembly.s_forceNullEntryPoint ? (Assembly) null : Assembly.GetEntryAssemblyInternal();

    /// <summary>Loads the assembly with a common object file format (COFF)-based image containing an emitted assembly. The assembly is loaded into the application domain of the caller.</summary>
    /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="rawAssembly" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="rawAssembly" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="rawAssembly" /> was compiled with a later version.</exception>
    /// <returns>The loaded assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly Load(byte[] rawAssembly) => Assembly.Load(rawAssembly, (byte[]) null);

    /// <summary>Loads the assembly with a common object file format (COFF)-based image containing an emitted assembly, optionally including symbols for the assembly. The assembly is loaded into the application domain of the caller.</summary>
    /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly.</param>
    /// <param name="rawSymbolStore">A byte array that contains the raw bytes representing the symbols for the assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="rawAssembly" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="rawAssembly" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="rawAssembly" /> was compiled with a later version.</exception>
    /// <returns>The loaded assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly Load(byte[] rawAssembly, byte[]? rawSymbolStore)
    {
      if (rawAssembly == null)
        throw new ArgumentNullException(nameof (rawAssembly));
      if (rawAssembly.Length == 0)
        throw new BadImageFormatException(SR.BadImageFormat_BadILFormat);
      SerializationInfo.ThrowIfDeserializationInProgress("AllowAssembliesFromByteArrays", ref Assembly.s_cachedSerializationSwitch);
      return new IndividualAssemblyLoadContext("Assembly.Load(byte[], ...)").InternalLoad((ReadOnlySpan<byte>) rawAssembly, (ReadOnlySpan<byte>) rawSymbolStore);
    }

    /// <summary>Loads the contents of an assembly file on the specified path.</summary>
    /// <param name="path">The fully qualified path of the file to load.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> argument is not an absolute path.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.
    /// 
    /// -or-
    /// 
    /// The ability to execute code in remote assemblies is disabled. See &lt;loadFromRemoteSources&gt;.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The <paramref name="path" /> parameter is an empty string ("") or does not exist.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="path" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="path" /> was compiled with a later version.</exception>
    /// <returns>The loaded assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly LoadFile(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      string str = !PathInternal.IsPartiallyQualified((ReadOnlySpan<char>) path) ? Path.GetFullPath(path) : throw new ArgumentException(SR.Format(SR.Argument_AbsolutePathRequired, (object) path), nameof (path));
      Assembly assembly;
      lock (Assembly.s_loadfile)
      {
        if (Assembly.s_loadfile.TryGetValue(str, out assembly))
          return assembly;
        assembly = new IndividualAssemblyLoadContext("Assembly.LoadFile(" + str + ")").LoadFromAssemblyPath(str);
        Assembly.s_loadfile.Add(str, assembly);
      }
      return assembly;
    }


    #nullable disable
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    [UnconditionalSuppressMessage("SingleFile", "IL3000:Avoid accessing Assembly file path when publishing as a single file", Justification = "The assembly is loaded by specifying a path outside of the single-file bundle, the location of the path will not be empty if the path exist, otherwise it will be handled as null")]
    private static Assembly LoadFromResolveHandler(object sender, ResolveEventArgs args)
    {
      Assembly requestingAssembly = args.RequestingAssembly;
      if (requestingAssembly == (Assembly) null)
        return (Assembly) null;
      if (AssemblyLoadContext.Default != AssemblyLoadContext.GetLoadContext(requestingAssembly))
        return (Assembly) null;
      string location = requestingAssembly.Location;
      if (string.IsNullOrEmpty(location))
        return (Assembly) null;
      string fullPath = Path.GetFullPath(location);
      lock (Assembly.s_loadFromAssemblyList)
      {
        if (!Assembly.s_loadFromAssemblyList.Contains(fullPath))
        {
          if (AssemblyLoadContext.IsTracingEnabled())
            AssemblyLoadContext.TraceAssemblyLoadFromResolveHandlerInvoked(args.Name, false, fullPath, (string) null);
          return (Assembly) null;
        }
      }
      AssemblyName assemblyName = new AssemblyName(args.Name);
      string str = Path.Combine(Path.GetDirectoryName(fullPath), assemblyName.Name + ".dll");
      if (AssemblyLoadContext.IsTracingEnabled())
        AssemblyLoadContext.TraceAssemblyLoadFromResolveHandlerInvoked(args.Name, true, fullPath, str);
      try
      {
        return Assembly.LoadFrom(str);
      }
      catch (FileNotFoundException ex)
      {
        return (Assembly) null;
      }
    }


    #nullable enable
    /// <summary>Loads an assembly given its file name or path.</summary>
    /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found, or the module you are trying to load does not specify a filename extension.</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.
    /// 
    /// -or-
    /// 
    /// The ability to execute code in remote assemblies is disabled. See &lt;loadFromRemoteSources&gt;.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly; for example, a 32-bit assembly in a 64-bit process. See the exception topic for more information.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyFile" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.Security.SecurityException">A codebase that does not start with "file://" was specified without the required <see cref="T:System.Net.WebPermission" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="assemblyFile" /> parameter is an empty string ("").</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The assembly name exceeds the system-defined maximum length.</exception>
    /// <returns>The loaded assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly LoadFrom(string assemblyFile)
    {
      string assemblyPath = assemblyFile != null ? Path.GetFullPath(assemblyFile) : throw new ArgumentNullException(nameof (assemblyFile));
      if (!Assembly.s_loadFromHandlerSet)
      {
        lock (Assembly.s_loadFromAssemblyList)
        {
          if (!Assembly.s_loadFromHandlerSet)
          {
            AssemblyLoadContext.AssemblyResolve += new ResolveEventHandler(Assembly.LoadFromResolveHandler);
            Assembly.s_loadFromHandlerSet = true;
          }
        }
      }
      lock (Assembly.s_loadFromAssemblyList)
      {
        if (!Assembly.s_loadFromAssemblyList.Contains(assemblyPath))
          Assembly.s_loadFromAssemblyList.Add(assemblyPath);
      }
      return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
    }

    /// <summary>Loads an assembly given its file name or path, hash value, and hash algorithm.</summary>
    /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly.</param>
    /// <param name="hashValue">The value of the computed hash code.</param>
    /// <param name="hashAlgorithm">The hash algorithm used for hashing files and for generating the strong name.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found, or the module you are trying to load does not specify a file name extension.</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.
    /// 
    /// -or-
    /// 
    /// The ability to execute code in remote assemblies is disabled. See &lt;loadFromRemoteSources&gt;.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly; for example, a 32-bit assembly in a 64-bit process. See the exception topic for more information.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyFile" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.Security.SecurityException">A codebase that does not start with "file://" was specified without the required <see cref="T:System.Net.WebPermission" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="assemblyFile" /> parameter is an empty string ("").</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The assembly name exceeds the system-defined maximum length.</exception>
    /// <returns>The loaded assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly LoadFrom(
      string assemblyFile,
      byte[]? hashValue,
      AssemblyHashAlgorithm hashAlgorithm)
    {
      throw new NotSupportedException(SR.NotSupported_AssemblyLoadFromHash);
    }

    /// <summary>Loads an assembly into the load-from context, bypassing some security checks.</summary>
    /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found, or the module you are trying to load does not specify a filename extension.</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyFile" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.Security.SecurityException">A codebase that does not start with "file://" was specified without the required <see cref="T:System.Net.WebPermission" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="assemblyFile" /> parameter is an empty string ("").</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The assembly name exceeds the system-defined maximum length.</exception>
    /// <returns>The loaded assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly UnsafeLoadFrom(string assemblyFile) => Assembly.LoadFrom(assemblyFile);

    /// <summary>Loads the module, internal to this assembly, with a common object file format (COFF)-based image containing an emitted module, or a resource file.</summary>
    /// <param name="moduleName">The name of the module. This string must correspond to a file name in this assembly's manifest.</param>
    /// <param name="rawModule">A byte array that is a COFF-based image containing an emitted module, or a resource.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="moduleName" /> or <paramref name="rawModule" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="moduleName" /> does not match a file entry in this assembly's manifest.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    /// <paramref name="rawModule" /> is not a valid module.</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <returns>The loaded module.</returns>
    [RequiresUnreferencedCode("Types and members the loaded module depends on might be removed")]
    public Module LoadModule(string moduleName, byte[]? rawModule) => this.LoadModule(moduleName, rawModule, (byte[]) null);

    /// <summary>Loads the module, internal to this assembly, with a common object file format (COFF)-based image containing an emitted module, or a resource file. The raw bytes representing the symbols for the module are also loaded.</summary>
    /// <param name="moduleName">The name of the module. This string must correspond to a file name in this assembly's manifest.</param>
    /// <param name="rawModule">A byte array that is a COFF-based image containing an emitted module, or a resource.</param>
    /// <param name="rawSymbolStore">A byte array containing the raw bytes representing the symbols for the module. Must be <see langword="null" /> if this is a resource file.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="moduleName" /> or <paramref name="rawModule" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="moduleName" /> does not match a file entry in this assembly's manifest.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    /// <paramref name="rawModule" /> is not a valid module.</exception>
    /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
    /// <returns>The loaded module.</returns>
    [RequiresUnreferencedCode("Types and members the loaded module depends on might be removed")]
    public virtual Module LoadModule(
      string moduleName,
      byte[]? rawModule,
      byte[]? rawSymbolStore)
    {
      throw NotImplemented.ByDesign;
    }

    /// <summary>Loads the assembly from a common object file format (COFF)-based image containing an emitted assembly. The assembly is loaded into the reflection-only context of the caller's application domain.</summary>
    /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="rawAssembly" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="rawAssembly" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="rawAssembly" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    /// <paramref name="rawAssembly" /> cannot be loaded.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <returns>The loaded assembly.</returns>
    [Obsolete("ReflectionOnly loading is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0018", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly ReflectionOnlyLoad(byte[] rawAssembly) => throw new PlatformNotSupportedException(SR.PlatformNotSupported_ReflectionOnly);

    /// <summary>Loads an assembly into the reflection-only context, given its display name.</summary>
    /// <param name="assemblyString">The display name of the assembly, as returned by the <see cref="P:System.Reflection.AssemblyName.FullName" /> property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyString" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="assemblyString" /> is an empty string ("").</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyString" /> is not found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    /// <paramref name="assemblyString" /> is found, but cannot be loaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyString" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyString" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <returns>The loaded assembly.</returns>
    [Obsolete("ReflectionOnly loading is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0018", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly ReflectionOnlyLoad(string assemblyString) => throw new PlatformNotSupportedException(SR.PlatformNotSupported_ReflectionOnly);

    /// <summary>Loads an assembly into the reflection-only context, given its path.</summary>
    /// <param name="assemblyFile">The path of the file that contains the manifest of the assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found, or the module you are trying to load does not specify a file name extension.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    /// <paramref name="assemblyFile" /> is found, but could not be loaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyFile" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.Security.SecurityException">A codebase that does not start with "file://" was specified without the required <see cref="T:System.Net.WebPermission" />.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The assembly name exceeds the system-defined maximum length.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="assemblyFile" /> is an empty string ("").</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <returns>The loaded assembly.</returns>
    [Obsolete("ReflectionOnly loading is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0018", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static Assembly ReflectionOnlyLoadFrom(string assemblyFile) => throw new PlatformNotSupportedException(SR.PlatformNotSupported_ReflectionOnly);

    /// <summary>Gets a value that indicates which set of security rules the common language runtime (CLR) enforces for this assembly.</summary>
    /// <returns>The security rule set that the CLR enforces for this assembly.</returns>
    public virtual SecurityRuleSet SecurityRuleSet => SecurityRuleSet.None;
  }
}
