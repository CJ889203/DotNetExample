// Decompiled with JetBrains decompiler
// Type: System.AppDomain
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Configuration.Assemblies;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Loader;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Represents an application domain, which is an isolated environment where applications execute. This class cannot be inherited.</summary>
  public sealed class AppDomain : MarshalByRefObject
  {

    #nullable disable
    private static readonly AppDomain s_domain = new AppDomain();
    private IPrincipal _defaultPrincipal;
    private PrincipalPolicy _principalPolicy = PrincipalPolicy.NoPrincipal;
    private Func<IPrincipal> s_getWindowsPrincipal;
    private Func<IPrincipal> s_getUnauthenticatedPrincipal;

    private AppDomain()
    {
    }


    #nullable enable
    /// <summary>Gets the current application domain for the current <see cref="T:System.Threading.Thread" />.</summary>
    /// <returns>The current application domain.</returns>
    public static AppDomain CurrentDomain => AppDomain.s_domain;

    /// <summary>Gets the base directory that the assembly resolver uses to probe for assemblies.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <returns>The base directory that the assembly resolver uses to probe for assemblies.</returns>
    public string BaseDirectory => AppContext.BaseDirectory;

    /// <summary>Gets the path under the base directory where the assembly resolver should probe for private assemblies.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <returns>The path under the base directory where the assembly resolver should probe for private assemblies.</returns>
    public string? RelativeSearchPath => (string) null;

    /// <summary>Gets the application domain configuration information for this instance.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <returns>The application domain initialization information.</returns>
    public AppDomainSetup SetupInformation => new AppDomainSetup();

    /// <summary>Gets the permission set of a sandboxed application domain.</summary>
    /// <returns>The permission set of the sandboxed application domain.</returns>
    [Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public PermissionSet PermissionSet => new PermissionSet(PermissionState.Unrestricted);

    /// <summary>Occurs when an exception is not caught.</summary>
    public event UnhandledExceptionEventHandler? UnhandledException
    {
      add => AppContext.UnhandledException += value;
      remove => AppContext.UnhandledException -= value;
    }

    /// <summary>Gets the directory that the assembly resolver uses to probe for dynamically created assemblies.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <returns>The directory that the assembly resolver uses to probe for dynamically created assemblies.</returns>
    public string? DynamicDirectory => (string) null;

    /// <summary>Establishes the specified directory path as the base directory for subdirectories where dynamically generated files are stored and accessed.</summary>
    /// <param name="path">The fully qualified path that is the base directory for subdirectories where dynamic assemblies are stored.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    [Obsolete("AppDomain.SetDynamicBase has been deprecated and is not supported.")]
    public void SetDynamicBase(string? path)
    {
    }

    /// <summary>Gets the friendly name of this application domain.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <returns>The friendly name of this application domain.</returns>
    public string FriendlyName
    {
      get
      {
        Assembly entryAssembly = Assembly.GetEntryAssembly();
        return !(entryAssembly != (Assembly) null) ? "DefaultDomain" : entryAssembly.GetName().Name;
      }
    }

    /// <summary>Gets an integer that uniquely identifies the application domain within the process.</summary>
    /// <returns>An integer that identifies the application domain.</returns>
    public int Id => 1;

    /// <summary>Gets a value that indicates whether assemblies that are loaded into the current application domain execute with full trust.</summary>
    /// <returns>
    /// <see langword="true" /> if assemblies that are loaded into the current application domain execute with full trust; otherwise, <see langword="false" />.</returns>
    public bool IsFullyTrusted => true;

    /// <summary>Gets a value that indicates whether the current application domain has a set of permissions that is granted to all assemblies that are loaded into the application domain.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application domain has a homogenous set of permissions; otherwise, <see langword="false" />.</returns>
    public bool IsHomogenous => true;

    /// <summary>Occurs when an <see cref="T:System.AppDomain" /> is about to be unloaded.</summary>
    public event EventHandler? DomainUnload;

    /// <summary>Occurs when an exception is thrown in managed code, before the runtime searches the call stack for an exception handler in the application domain.</summary>
    public event EventHandler<FirstChanceExceptionEventArgs>? FirstChanceException
    {
      add => AppContext.FirstChanceException += value;
      remove => AppContext.FirstChanceException -= value;
    }

    /// <summary>Occurs when the default application domain's parent process exits.</summary>
    public event EventHandler? ProcessExit
    {
      add => AppContext.ProcessExit += value;
      remove => AppContext.ProcessExit -= value;
    }

    /// <summary>Returns the assembly display name after policy has been applied.</summary>
    /// <param name="assemblyName">The assembly display name, in the form provided by the <see cref="P:System.Reflection.Assembly.FullName" /> property.</param>
    /// <returns>A string containing the assembly display name after policy has been applied.</returns>
    public string ApplyPolicy(string assemblyName)
    {
      switch (assemblyName)
      {
        case "":
          throw new ArgumentException(SR.Argument_StringZeroLength, nameof (assemblyName));
        case null:
          throw new ArgumentNullException(nameof (assemblyName));
        default:
          if (assemblyName[0] != char.MinValue)
            return assemblyName;
          goto case "";
      }
    }

    /// <summary>Creates a new application domain with the specified name.</summary>
    /// <param name="friendlyName">The friendly name of the domain.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="friendlyName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <returns>The newly created application domain.</returns>
    [Obsolete("Creating and unloading AppDomains is not supported and throws an exception.", DiagnosticId = "SYSLIB0024", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public static AppDomain CreateDomain(string friendlyName)
    {
      if (friendlyName == null)
        throw new ArgumentNullException(nameof (friendlyName));
      throw new PlatformNotSupportedException(SR.PlatformNotSupported_AppDomains);
    }

    /// <summary>Executes the assembly contained in the specified file.</summary>
    /// <param name="assemblyFile">The name of the file that contains the assembly to execute.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyFile" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.MissingMethodException">The specified assembly has no entry point.</exception>
    /// <returns>The value returned by the entry point of the assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public int ExecuteAssembly(string assemblyFile) => this.ExecuteAssembly(assemblyFile, (string[]) null);

    /// <summary>Executes the assembly contained in the specified file, using the specified arguments.</summary>
    /// <param name="assemblyFile">The name of the file that contains the assembly to execute.</param>
    /// <param name="args">The arguments to the entry point of the assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyFile" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.MissingMethodException">The specified assembly has no entry point.</exception>
    /// <returns>The value that is returned by the entry point of the assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public int ExecuteAssembly(string assemblyFile, string?[]? args) => assemblyFile != null ? AppDomain.ExecuteAssembly(Assembly.LoadFile(Path.GetFullPath(assemblyFile)), args) : throw new ArgumentNullException(nameof (assemblyFile));

    /// <summary>Executes the assembly contained in the specified file, using the specified arguments, hash value, and hash algorithm.</summary>
    /// <param name="assemblyFile">The name of the file that contains the assembly to execute.</param>
    /// <param name="args">The arguments to the entry point of the assembly.</param>
    /// <param name="hashValue">Represents the value of the computed hash code.</param>
    /// <param name="hashAlgorithm">Represents the hash algorithm used by the assembly manifest.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> is not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyFile" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.MissingMethodException">The specified assembly has no entry point.</exception>
    /// <returns>The value that is returned by the entry point of the assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    [Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public int ExecuteAssembly(
      string assemblyFile,
      string?[]? args,
      byte[]? hashValue,
      AssemblyHashAlgorithm hashAlgorithm)
    {
      throw new PlatformNotSupportedException(SR.PlatformNotSupported_CAS);
    }


    #nullable disable
    private static int ExecuteAssembly(Assembly assembly, string[] args)
    {
      MethodInfo entryPoint = assembly.EntryPoint;
      MethodInfo methodInfo = !(entryPoint == (MethodInfo) null) ? entryPoint : throw new MissingMethodException(SR.Arg_EntryPointNotFoundException);
      object[] parameters;
      if (entryPoint.GetParameters().Length == 0)
        parameters = (object[]) null;
      else
        parameters = new object[1]{ (object) args };
      object obj = methodInfo.Invoke((object) null, BindingFlags.DoNotWrapExceptions, (Binder) null, parameters, (CultureInfo) null);
      return obj == null ? 0 : (int) obj;
    }


    #nullable enable
    /// <summary>Executes the assembly given an <see cref="T:System.Reflection.AssemblyName" />, using the specified arguments.</summary>
    /// <param name="assemblyName">An <see cref="T:System.Reflection.AssemblyName" /> object representing the name of the assembly.</param>
    /// <param name="args">Command-line arguments to pass when starting the process.</param>
    /// <exception cref="T:System.IO.FileNotFoundException">The assembly specified by <paramref name="assemblyName" /> is not found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">The assembly specified by <paramref name="assemblyName" /> was found, but could not be loaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">The assembly specified by <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyName" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.MissingMethodException">The specified assembly has no entry point.</exception>
    /// <returns>The value that is returned by the entry point of the assembly.</returns>
    public int ExecuteAssemblyByName(AssemblyName assemblyName, params string?[]? args) => AppDomain.ExecuteAssembly(Assembly.Load(assemblyName), args);

    /// <summary>Executes an assembly given its display name.</summary>
    /// <param name="assemblyName">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The assembly specified by <paramref name="assemblyName" /> is not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">The assembly specified by <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyName" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileLoadException">The assembly specified by <paramref name="assemblyName" /> was found, but could not be loaded.</exception>
    /// <exception cref="T:System.MissingMethodException">The specified assembly has no entry point.</exception>
    /// <returns>The value returned by the entry point of the assembly.</returns>
    public int ExecuteAssemblyByName(string assemblyName) => this.ExecuteAssemblyByName(assemblyName, (string[]) null);

    /// <summary>Executes the assembly given its display name, using the specified arguments.</summary>
    /// <param name="assemblyName">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <param name="args">Command-line arguments to pass when starting the process.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The assembly specified by <paramref name="assemblyName" /> is not found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">The assembly specified by <paramref name="assemblyName" /> was found, but could not be loaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">The assembly specified by <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyName" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.MissingMethodException">The specified assembly has no entry point.</exception>
    /// <returns>The value that is returned by the entry point of the assembly.</returns>
    public int ExecuteAssemblyByName(string assemblyName, params string?[]? args) => AppDomain.ExecuteAssembly(Assembly.Load(assemblyName), args);

    /// <summary>Gets the value stored in the current application domain for the specified name.</summary>
    /// <param name="name">The name of a predefined application domain property, or the name of an application domain property you have defined.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <returns>The value of the <paramref name="name" /> property, or <see langword="null" /> if the property does not exist.</returns>
    public object? GetData(string name) => AppContext.GetData(name);

    /// <summary>Assigns the specified value to the specified application domain property.</summary>
    /// <param name="name">The name of a user-defined application domain property to create or change.</param>
    /// <param name="data">The value of the property.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    public void SetData(string name, object? data) => AppContext.SetData(name, data);

    /// <summary>Gets a nullable Boolean value that indicates whether any compatibility switches are set, and if so, whether the specified compatibility switch is set.</summary>
    /// <param name="value">The compatibility switch to test.</param>
    /// <returns>A null reference (<see langword="Nothing" /> in Visual Basic) if no compatibility switches are set; otherwise, a Boolean value that indicates whether the compatibility switch that is specified by <paramref name="value" /> is set.</returns>
    public bool? IsCompatibilitySwitchSet(string value)
    {
      bool isEnabled;
      return !AppContext.TryGetSwitch(value, out isEnabled) ? new bool?() : new bool?(isEnabled);
    }

    /// <summary>Returns a value that indicates whether the application domain is the default application domain for the process.</summary>
    /// <returns>
    /// <see langword="true" /> if the current <see cref="T:System.AppDomain" /> object represents the default application domain for the process; otherwise, <see langword="false" />.</returns>
    public bool IsDefaultAppDomain() => true;

    /// <summary>Indicates whether this application domain is unloading, and the objects it contains are being finalized by the common language runtime.</summary>
    /// <returns>
    /// <see langword="true" /> if this application domain is unloading and the common language runtime has started invoking finalizers; otherwise, <see langword="false" />.</returns>
    public bool IsFinalizingForUnload() => false;

    /// <summary>Obtains a string representation that includes the friendly name of the application domain and any context policies.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The application domain represented by the current <see cref="T:System.AppDomain" /> has been unloaded.</exception>
    /// <returns>A string formed by concatenating the literal string "Name:", the friendly name of the application domain, and either string representations of the context policies or the string "There are no context policies."</returns>
    public override string ToString() => SR.AppDomain_Name + this.FriendlyName + "\r\n" + SR.AppDomain_NoContextPolicies;

    /// <summary>Unloads the specified application domain.</summary>
    /// <param name="domain">An application domain to unload.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="domain" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.CannotUnloadAppDomainException">
    /// .NET Core and .NET 5+ only: In all cases.
    /// 
    /// -or-
    /// 
    /// <paramref name="domain" /> could not be unloaded.</exception>
    /// <exception cref="T:System.Exception">An error occurred during the unload process.</exception>
    [Obsolete("Creating and unloading AppDomains is not supported and throws an exception.", DiagnosticId = "SYSLIB0024", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public static void Unload(AppDomain domain)
    {
      if (domain == null)
        throw new ArgumentNullException(nameof (domain));
      throw new CannotUnloadAppDomainException(SR.Arg_PlatformNotSupported);
    }

    /// <summary>Loads the <see cref="T:System.Reflection.Assembly" /> with a common object file format (COFF) based image containing an emitted <see cref="T:System.Reflection.Assembly" />.</summary>
    /// <param name="rawAssembly">An array of type <see langword="byte" /> that is a COFF-based image containing an emitted assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="rawAssembly" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="rawAssembly" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="rawAssembly" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>The loaded assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public Assembly Load(byte[] rawAssembly) => Assembly.Load(rawAssembly);

    /// <summary>Loads the <see cref="T:System.Reflection.Assembly" /> with a common object file format (COFF) based image containing an emitted <see cref="T:System.Reflection.Assembly" />. The raw bytes representing the symbols for the <see cref="T:System.Reflection.Assembly" /> are also loaded.</summary>
    /// <param name="rawAssembly">An array of type <see langword="byte" /> that is a COFF-based image containing an emitted assembly.</param>
    /// <param name="rawSymbolStore">An array of type <see langword="byte" /> containing the raw bytes representing the symbols for the assembly.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="rawAssembly" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="rawAssembly" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="rawAssembly" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>The loaded assembly.</returns>
    [RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public Assembly Load(byte[] rawAssembly, byte[]? rawSymbolStore) => Assembly.Load(rawAssembly, rawSymbolStore);

    /// <summary>Loads an <see cref="T:System.Reflection.Assembly" /> given its <see cref="T:System.Reflection.AssemblyName" />.</summary>
    /// <param name="assemblyRef">An object that describes the assembly to load.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyRef" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyRef" /> is not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyRef" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyRef" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>The loaded assembly.</returns>
    public Assembly Load(AssemblyName assemblyRef) => Assembly.Load(assemblyRef);

    /// <summary>Loads an <see cref="T:System.Reflection.Assembly" /> given its display name.</summary>
    /// <param name="assemblyString">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyString" /> is <see langword="null" /></exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyString" /> is not found.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyString" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyString" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>The loaded assembly.</returns>
    public Assembly Load(string assemblyString) => Assembly.Load(assemblyString);

    /// <summary>Returns the assemblies that have been loaded into the reflection-only context of the application domain.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">An operation is attempted on an unloaded application domain.</exception>
    /// <returns>An array of <see cref="T:System.Reflection.Assembly" /> objects that represent the assemblies loaded into the reflection-only context of the application domain.</returns>
    public Assembly[] ReflectionOnlyGetAssemblies() => Array.Empty<Assembly>();

    /// <summary>Gets or sets a value that indicates whether CPU and memory monitoring of application domains is enabled for the current process. Once monitoring is enabled for a process, it cannot be disabled.</summary>
    /// <exception cref="T:System.ArgumentException">The current process attempted to assign the value <see langword="false" /> to this property.</exception>
    /// <returns>
    /// <see langword="true" /> if monitoring is enabled; otherwise <see langword="false" />.</returns>
    public static bool MonitoringIsEnabled
    {
      get => true;
      set
      {
        if (!value)
          throw new ArgumentException(SR.Arg_MustBeTrue);
      }
    }

    /// <summary>Gets the number of bytes that survived the last collection and that are known to be referenced by the current application domain.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see langword="static" /> (<see langword="Shared" /> in Visual Basic) <see cref="P:System.AppDomain.MonitoringIsEnabled" /> property is set to <see langword="false" />.</exception>
    /// <returns>The number of surviving bytes.</returns>
    public long MonitoringSurvivedMemorySize => AppDomain.MonitoringSurvivedProcessMemorySize;

    /// <summary>Gets the total bytes that survived from the last collection for all application domains in the process.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see langword="static" /> (<see langword="Shared" /> in Visual Basic) <see cref="P:System.AppDomain.MonitoringIsEnabled" /> property is set to <see langword="false" />.</exception>
    /// <returns>The total number of surviving bytes for the process.</returns>
    public static long MonitoringSurvivedProcessMemorySize
    {
      get
      {
        GCMemoryInfo gcMemoryInfo = GC.GetGCMemoryInfo();
        return gcMemoryInfo.HeapSizeBytes - gcMemoryInfo.FragmentedBytes;
      }
    }

    /// <summary>Gets the total size, in bytes, of all memory allocations that have been made by the application domain since it was created, without subtracting memory that has been collected.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see langword="static" /> (<see langword="Shared" /> in Visual Basic) <see cref="P:System.AppDomain.MonitoringIsEnabled" /> property is set to <see langword="false" />.</exception>
    /// <returns>The total size of all memory allocations.</returns>
    public long MonitoringTotalAllocatedMemorySize => GC.GetTotalAllocatedBytes();

    /// <summary>Gets the current thread identifier.</summary>
    /// <returns>A 32-bit signed integer that is the identifier of the current thread.</returns>
    [Obsolete("AppDomain.GetCurrentThreadId has been deprecated because it does not provide a stable Id when managed threads are running on fibers (aka lightweight threads). To get a stable identifier for a managed thread, use the ManagedThreadId property on Thread instead.")]
    public static int GetCurrentThreadId() => Environment.CurrentManagedThreadId;

    /// <summary>Gets an indication whether the application domain is configured to shadow copy files.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <returns>
    /// <see langword="true" /> if the application domain is configured to shadow copy files; otherwise, <see langword="false" />.</returns>
    public bool ShadowCopyFiles => false;

    /// <summary>Appends the specified directory name to the private path list.</summary>
    /// <param name="path">The name of the directory to be appended to the private path.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    [Obsolete("AppDomain.AppendPrivatePath has been deprecated and is not supported.")]
    public void AppendPrivatePath(string? path)
    {
    }

    /// <summary>Resets the path that specifies the location of private assemblies to the empty string ("").</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    [Obsolete("AppDomain.ClearPrivatePath has been deprecated and is not supported.")]
    public void ClearPrivatePath()
    {
    }

    /// <summary>Resets the list of directories containing shadow copied assemblies to the empty string ("").</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    [Obsolete("AppDomain.ClearShadowCopyPath has been deprecated and is not supported.")]
    public void ClearShadowCopyPath()
    {
    }

    /// <summary>Establishes the specified directory path as the location where assemblies are shadow copied.</summary>
    /// <param name="path">The fully qualified path to the shadow copy location.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    [Obsolete("AppDomain.SetCachePath has been deprecated and is not supported.")]
    public void SetCachePath(string? path)
    {
    }

    /// <summary>Turns on shadow copying.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    [Obsolete("AppDomain.SetShadowCopyFiles has been deprecated and is not supported.")]
    public void SetShadowCopyFiles()
    {
    }

    /// <summary>Establishes the specified directory path as the location of assemblies to be shadow copied.</summary>
    /// <param name="path">A list of directory names, where each name is separated by a semicolon.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    [Obsolete("AppDomain.SetShadowCopyPath has been deprecated and is not supported.")]
    public void SetShadowCopyPath(string? path)
    {
    }

    /// <summary>Gets the assemblies that have been loaded into the execution context of this application domain.</summary>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <returns>An array of assemblies in this application domain.</returns>
    public Assembly[] GetAssemblies() => AssemblyLoadContext.GetLoadedAssemblies();

    /// <summary>Occurs when an assembly is loaded.</summary>
    public event AssemblyLoadEventHandler? AssemblyLoad
    {
      add => AssemblyLoadContext.AssemblyLoad += value;
      remove => AssemblyLoadContext.AssemblyLoad -= value;
    }

    /// <summary>Occurs when the resolution of an assembly fails.</summary>
    public event ResolveEventHandler? AssemblyResolve
    {
      add => AssemblyLoadContext.AssemblyResolve += value;
      remove => AssemblyLoadContext.AssemblyResolve -= value;
    }

    /// <summary>Occurs when the resolution of an assembly fails in the reflection-only context.</summary>
    public event ResolveEventHandler? ReflectionOnlyAssemblyResolve;

    /// <summary>Occurs when the resolution of a type fails.</summary>
    public event ResolveEventHandler? TypeResolve
    {
      add => AssemblyLoadContext.TypeResolve += value;
      remove => AssemblyLoadContext.TypeResolve -= value;
    }

    /// <summary>Occurs when the resolution of a resource fails because the resource is not a valid linked or embedded resource in the assembly.</summary>
    public event ResolveEventHandler? ResourceResolve
    {
      add => AssemblyLoadContext.ResourceResolve += value;
      remove => AssemblyLoadContext.ResourceResolve -= value;
    }

    /// <summary>Specifies how principal and identity objects should be attached to a thread if the thread attempts to bind to a principal while executing in this application domain.</summary>
    /// <param name="policy">One of the <see cref="T:System.Security.Principal.PrincipalPolicy" /> values that specifies the type of the principal object to attach to threads.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    public void SetPrincipalPolicy(PrincipalPolicy policy) => this._principalPolicy = policy;

    /// <summary>Sets the default principal object to be attached to threads if they attempt to bind to a principal while executing in this application domain.</summary>
    /// <param name="principal">The principal object to attach to threads.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="principal" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.Policy.PolicyException">The thread principal has already been set.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    public void SetThreadPrincipal(IPrincipal principal)
    {
      if (principal == null)
        throw new ArgumentNullException(nameof (principal));
      if (Interlocked.CompareExchange<IPrincipal>(ref this._defaultPrincipal, principal, (IPrincipal) null) != null)
        throw new SystemException(SR.AppDomain_Policy_PrincipalTwice);
    }

    /// <summary>Creates a new instance of the specified type defined in the specified assembly.</summary>
    /// <param name="assemblyName">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyName" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.NullReferenceException">This instance is <see langword="null" />.</exception>
    /// <returns>An object that is a wrapper for the new instance specified by <paramref name="typeName" />. The return value needs to be unwrapped to access the real object.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public ObjectHandle? CreateInstance(string assemblyName, string typeName) => assemblyName != null ? Activator.CreateInstance(assemblyName, typeName) : throw new ArgumentNullException(nameof (assemblyName));

    /// <summary>Creates a new instance of the specified type defined in the specified assembly. Parameters specify a binder, binding flags, constructor arguments, culture-specific information used to interpret arguments, and optional activation attributes.</summary>
    /// <param name="assemblyName">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <param name="ignoreCase">A Boolean value specifying whether to perform a case-sensitive search or not.</param>
    /// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects using reflection. If <paramref name="binder" /> is null, the default binder is used.</param>
    /// <param name="args">The arguments to pass to the constructor. This array of arguments must match in number, order, and type the parameters of the constructor to invoke. If the parameterless constructor is preferred, <paramref name="args" /> must be an empty array or null.</param>
    /// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyName" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.NullReferenceException">This instance is <see langword="null" />.</exception>
    /// <returns>An object that is a wrapper for the new instance specified by <paramref name="typeName" />. The return value needs to be unwrapped to access the real object.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public ObjectHandle? CreateInstance(
      string assemblyName,
      string typeName,
      bool ignoreCase,
      BindingFlags bindingAttr,
      Binder? binder,
      object?[]? args,
      CultureInfo? culture,
      object?[]? activationAttributes)
    {
      if (assemblyName == null)
        throw new ArgumentNullException(nameof (assemblyName));
      return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes);
    }

    /// <summary>Creates a new instance of the specified type defined in the specified assembly. A parameter specifies an array of activation attributes.</summary>
    /// <param name="assemblyName">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects.Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyName" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.NullReferenceException">This instance is <see langword="null" />.</exception>
    /// <returns>An object that is a wrapper for the new instance specified by <paramref name="typeName" />. The return value needs to be unwrapped to access the real object.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public ObjectHandle? CreateInstance(
      string assemblyName,
      string typeName,
      object?[]? activationAttributes)
    {
      if (assemblyName == null)
        throw new ArgumentNullException(nameof (assemblyName));
      return Activator.CreateInstance(assemblyName, typeName, activationAttributes);
    }

    /// <summary>Creates a new instance of the specified type. Parameters specify the assembly where the type is defined, and the name of the type.</summary>
    /// <param name="assemblyName">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyName" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>An instance of the object specified by <paramref name="typeName" />.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public object? CreateInstanceAndUnwrap(string assemblyName, string typeName) => this.CreateInstance(assemblyName, typeName)?.Unwrap();

    /// <summary>Creates a new instance of the specified type defined in the specified assembly, specifying whether the case of the type name is ignored; the binding attributes and the binder that are used to select the type to be created; the arguments of the constructor; the culture; and the activation attributes.</summary>
    /// <param name="assemblyName">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <param name="ignoreCase">A Boolean value specifying whether to perform a case-sensitive search or not.</param>
    /// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects using reflection. If <paramref name="binder" /> is null, the default binder is used.</param>
    /// <param name="args">The arguments to pass to the constructor. This array of arguments must match in number, order, and type the parameters of the constructor to invoke. If the parameterless constructor is preferred, <paramref name="args" /> must be an empty array or null.</param>
    /// <param name="culture">A culture-specific object used to govern the coercion of types. If <paramref name="culture" /> is <see langword="null" />, the <see langword="CultureInfo" /> for the current thread is used.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyName" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>An instance of the object specified by <paramref name="typeName" />.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public object? CreateInstanceAndUnwrap(
      string assemblyName,
      string typeName,
      bool ignoreCase,
      BindingFlags bindingAttr,
      Binder? binder,
      object?[]? args,
      CultureInfo? culture,
      object?[]? activationAttributes)
    {
      return this.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes)?.Unwrap();
    }

    /// <summary>Creates a new instance of the specified type. Parameters specify the assembly where the type is defined, the name of the type, and an array of activation attributes.</summary>
    /// <param name="assemblyName">The display name of the assembly. See <see cref="P:System.Reflection.Assembly.FullName" />.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects.Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyName" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyName" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>An instance of the object specified by <paramref name="typeName" />.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public object? CreateInstanceAndUnwrap(
      string assemblyName,
      string typeName,
      object?[]? activationAttributes)
    {
      return this.CreateInstance(assemblyName, typeName, activationAttributes)?.Unwrap();
    }

    /// <summary>Creates a new instance of the specified type defined in the specified assembly file.</summary>
    /// <param name="assemblyFile">The name, including the path, of a file that contains an assembly that defines the requested type. The assembly is loaded using the <see cref="M:System.Reflection.Assembly.LoadFrom(System.String)" /> method.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="assemblyFile" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> was not found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typeName" /> was not found in <paramref name="assemblyFile" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.MissingMethodException">No parameterless public constructor was found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have sufficient permission to call this constructor.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyFile" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.NullReferenceException">This instance is <see langword="null" />.</exception>
    /// <returns>An object that is a wrapper for the new instance, or <see langword="null" /> if <paramref name="typeName" /> is not found. The return value needs to be unwrapped to access the real object.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public ObjectHandle? CreateInstanceFrom(string assemblyFile, string typeName) => Activator.CreateInstanceFrom(assemblyFile, typeName);

    /// <summary>Creates a new instance of the specified type defined in the specified assembly file.</summary>
    /// <param name="assemblyFile">The name, including the path, of a file that contains an assembly that defines the requested type. The assembly is loaded using the <see cref="M:System.Reflection.Assembly.LoadFrom(System.String)" /> method.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <param name="ignoreCase">A Boolean value specifying whether to perform a case-sensitive search or not.</param>
    /// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects through reflection. If <paramref name="binder" /> is null, the default binder is used.</param>
    /// <param name="args">The arguments to pass to the constructor. This array of arguments must match in number, order, and type the parameters of the constructor to invoke. If the parameterless constructor is preferred, <paramref name="args" /> must be an empty array or null.</param>
    /// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="assemblyFile" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> was not found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typeName" /> was not found in <paramref name="assemblyFile" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have sufficient permission to call this constructor.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyFile" /> was compiled with a later version of the common language runtime than the version that is currently loaded.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.NullReferenceException">This instance is <see langword="null" />.</exception>
    /// <returns>An object that is a wrapper for the new instance, or <see langword="null" /> if <paramref name="typeName" /> is not found. The return value needs to be unwrapped to access the real object.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public ObjectHandle? CreateInstanceFrom(
      string assemblyFile,
      string typeName,
      bool ignoreCase,
      BindingFlags bindingAttr,
      Binder? binder,
      object?[]? args,
      CultureInfo? culture,
      object?[]? activationAttributes)
    {
      return Activator.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes);
    }

    /// <summary>Creates a new instance of the specified type defined in the specified assembly file.</summary>
    /// <param name="assemblyFile">The name, including the path, of a file that contains an assembly that defines the requested type. The assembly is loaded using the <see cref="M:System.Reflection.Assembly.LoadFrom(System.String)" /> method.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects.Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="assemblyFile" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> was not found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typeName" /> was not found in <paramref name="assemblyFile" />.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have sufficient permission to call this constructor.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyFile" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <exception cref="T:System.NullReferenceException">This instance is <see langword="null" />.</exception>
    /// <returns>An object that is a wrapper for the new instance, or <see langword="null" /> if <paramref name="typeName" /> is not found. The return value needs to be unwrapped to access the real object.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public ObjectHandle? CreateInstanceFrom(
      string assemblyFile,
      string typeName,
      object?[]? activationAttributes)
    {
      return Activator.CreateInstanceFrom(assemblyFile, typeName, activationAttributes);
    }

    /// <summary>Creates a new instance of the specified type defined in the specified assembly file.</summary>
    /// <param name="assemblyFile" />
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="assemblyName" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typeName" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No parameterless public constructor was found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have sufficient permission to call this constructor.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyName" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>The requested object, or <see langword="null" /> if <paramref name="typeName" /> is not found.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public object? CreateInstanceFromAndUnwrap(string assemblyFile, string typeName) => this.CreateInstanceFrom(assemblyFile, typeName)?.Unwrap();

    /// <summary>Creates a new instance of the specified type defined in the specified assembly file, specifying whether the case of the type name is ignored; the binding attributes and the binder that are used to select the type to be created; the arguments of the constructor; the culture; and the activation attributes.</summary>
    /// <param name="assemblyFile">The file name and path of the assembly that defines the requested type.</param>
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly, as returned by the <see cref="P:System.Type.FullName" /> property.</param>
    /// <param name="ignoreCase">A Boolean value specifying whether to perform a case-sensitive search or not.</param>
    /// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects through reflection. If <paramref name="binder" /> is null, the default binder is used.</param>
    /// <param name="args">The arguments to pass to the constructor. This array of arguments must match in number, order, and type the parameters of the constructor to invoke. If the parameterless constructor is preferred, <paramref name="args" /> must be an empty array or null.</param>
    /// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="assemblyName" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typeName" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have sufficient permission to call this constructor.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="assemblyName" /> was compiled with a later version of the common language runtime that the version that is currently loaded.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>The requested object, or <see langword="null" /> if <paramref name="typeName" /> is not found.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public object? CreateInstanceFromAndUnwrap(
      string assemblyFile,
      string typeName,
      bool ignoreCase,
      BindingFlags bindingAttr,
      Binder? binder,
      object?[]? args,
      CultureInfo? culture,
      object?[]? activationAttributes)
    {
      return this.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes)?.Unwrap();
    }

    /// <summary>Creates a new instance of the specified type defined in the specified assembly file.</summary>
    /// <param name="assemblyFile" />
    /// <param name="typeName">The fully qualified name of the requested type, including the namespace but not the assembly (see the <see cref="P:System.Type.FullName" /> property).</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects.Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="assemblyName" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.AppDomainUnloadedException">The operation is attempted on an unloaded application domain.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typeName" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No parameterless public constructor was found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have sufficient permission to call this constructor.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// Version 2.0 or later of the common language runtime is currently loaded and <paramref name="assemblyName" /> was compiled with a later version.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.</exception>
    /// <returns>The requested object, or <see langword="null" /> if <paramref name="typeName" /> is not found.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public object? CreateInstanceFromAndUnwrap(
      string assemblyFile,
      string typeName,
      object?[]? activationAttributes)
    {
      return this.CreateInstanceFrom(assemblyFile, typeName, activationAttributes)?.Unwrap();
    }


    #nullable disable
    internal IPrincipal GetThreadPrincipal()
    {
      IPrincipal threadPrincipal = this._defaultPrincipal;
      if (threadPrincipal == null)
      {
        switch (this._principalPolicy)
        {
          case PrincipalPolicy.UnauthenticatedPrincipal:
            if (this.s_getUnauthenticatedPrincipal == null)
              Volatile.Write<Func<IPrincipal>>(ref this.s_getUnauthenticatedPrincipal, Type.GetType("System.Security.Principal.GenericPrincipal, System.Security.Claims", true).GetMethod("GetDefaultInstance", BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate<Func<IPrincipal>>());
            threadPrincipal = this.s_getUnauthenticatedPrincipal();
            break;
          case PrincipalPolicy.WindowsPrincipal:
            if (this.s_getWindowsPrincipal == null)
            {
              MethodInfo method = Type.GetType("System.Security.Principal.WindowsPrincipal, System.Security.Principal.Windows", true).GetMethod("GetDefaultInstance", BindingFlags.Static | BindingFlags.NonPublic);
              if (method == (MethodInfo) null)
                throw new PlatformNotSupportedException(SR.PlatformNotSupported_Principal);
              Volatile.Write<Func<IPrincipal>>(ref this.s_getWindowsPrincipal, method.CreateDelegate<Func<IPrincipal>>());
            }
            threadPrincipal = this.s_getWindowsPrincipal();
            break;
        }
      }
      return threadPrincipal;
    }

    /// <summary>Gets the total processor time that has been used by all threads while executing in the current application domain, since the process started.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see langword="static" /> (<see langword="Shared" /> in Visual Basic) <see cref="P:System.AppDomain.MonitoringIsEnabled" /> property is set to <see langword="false" />.</exception>
    /// <returns>Total processor time for the current application domain.</returns>
    public TimeSpan MonitoringTotalProcessorTime
    {
      get
      {
        long user;
        return !Interop.Kernel32.GetProcessTimes(Interop.Kernel32.GetCurrentProcess(), out long _, out long _, out long _, out user) ? TimeSpan.Zero : new TimeSpan(user);
      }
    }
  }
}
