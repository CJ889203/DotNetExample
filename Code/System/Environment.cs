// Decompiled with JetBrains decompiler
// Type: System.Environment
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Provides information about, and means to manipulate, the current environment and platform. This class cannot be inherited.</summary>
  public static class Environment
  {

    #nullable disable
    private static string[] s_commandLineArgs;
    private static volatile int s_processId;
    private static volatile string s_processPath;
    private static volatile OperatingSystem s_osVersion;

    /// <summary>Gets a unique identifier for the current managed thread.</summary>
    /// <returns>A unique identifier for this managed thread.</returns>
    public static extern int CurrentManagedThreadId { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [DoesNotReturn]
    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void _Exit(int exitCode);

    /// <summary>Terminates this process and returns an exit code to the operating system.</summary>
    /// <param name="exitCode">The exit code to return to the operating system. Use 0 (zero) to indicate that the process completed successfully.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have sufficient security permission to perform this function.</exception>
    [DoesNotReturn]
    public static void Exit(int exitCode) => Environment._Exit(exitCode);

    /// <summary>Gets or sets the exit code of the process.</summary>
    /// <returns>A 32-bit signed integer containing the exit code. The default value is 0 (zero), which indicates that the process completed successfully.</returns>
    public static extern int ExitCode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }


    #nullable enable
    /// <summary>Immediately terminates a process after writing a message to the Windows Application event log, and then includes the message in error reporting to Microsoft.</summary>
    /// <param name="message">A message that explains why the process was terminated, or <see langword="null" /> if no explanation is provided.</param>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FailFast(string? message);

    /// <summary>Immediately terminates a process after writing a message to the Windows Application event log, and then includes the message and exception information in error reporting to Microsoft.</summary>
    /// <param name="message">A message that explains why the process was terminated, or <see langword="null" /> if no explanation is provided.</param>
    /// <param name="exception">An exception that represents the error that caused the termination. This is typically the exception in a <see langword="catch" /> block.</param>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FailFast(string? message, Exception? exception);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FailFast(string? message, Exception? exception, string? errorMessage);


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string[] GetCommandLineArgsNative();


    #nullable enable
    /// <summary>Returns a string array containing the command-line arguments for the current process.</summary>
    /// <exception cref="T:System.NotSupportedException">The system does not support command-line arguments.</exception>
    /// <returns>An array of strings where each element contains a command-line argument. The first element is the executable file name, and the following zero or more elements contain the remaining command-line arguments.</returns>
    public static string[] GetCommandLineArgs() => Environment.s_commandLineArgs == null ? Environment.GetCommandLineArgsNative() : (string[]) Environment.s_commandLineArgs.Clone();

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern int GetProcessorCount();


    #nullable disable
    internal static string GetResourceStringLocal(string key) => SR.GetResourceString(key);

    /// <summary>Gets the number of milliseconds elapsed since the system started.</summary>
    /// <returns>A 32-bit signed integer containing the amount of time in milliseconds that has passed since the last time the computer was started.</returns>
    public static extern int TickCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>Gets the number of milliseconds elapsed since the system started.</summary>
    /// <returns>The elapsed milliseconds since the system started.</returns>
    public static extern long TickCount64 { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>Gets the number of processors available to the current process.</summary>
    /// <returns>The 32-bit signed integer that specifies the number of processors that are available.</returns>
    public static int ProcessorCount { get; } = Environment.GetProcessorCount();

    internal static bool IsSingleProcessor => Environment.ProcessorCount == 1;

    /// <summary>Gets a value that indicates whether the current application domain is being unloaded or the common language runtime (CLR) is shutting down.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application domain is being unloaded or the CLR is shutting down; otherwise, <see langword="false" />.</returns>
    public static bool HasShutdownStarted => false;


    #nullable enable
    /// <summary>Retrieves the value of an environment variable from the current process.</summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    /// <returns>The value of the environment variable specified by <paramref name="variable" />, or <see langword="null" /> if the environment variable is not found.</returns>
    public static string? GetEnvironmentVariable(string variable) => variable != null ? Environment.GetEnvironmentVariableCore(variable) : throw new ArgumentNullException(nameof (variable));

    /// <summary>Retrieves the value of an environment variable from the current process or from the Windows operating system registry key for the current user or local machine.</summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="target">One of the <see cref="T:System.EnvironmentVariableTarget" /> values. Only <see cref="F:System.EnvironmentVariableTarget.Process" /> is supported on .NET Core running on Unix-bases systems.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="target" /> is not a valid <see cref="T:System.EnvironmentVariableTarget" /> value.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    /// <returns>The value of the environment variable specified by the <paramref name="variable" /> and <paramref name="target" /> parameters, or <see langword="null" /> if the environment variable is not found.</returns>
    public static string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
    {
      if (target == EnvironmentVariableTarget.Process)
        return Environment.GetEnvironmentVariable(variable);
      if (variable == null)
        throw new ArgumentNullException(nameof (variable));
      bool fromMachine = Environment.ValidateAndConvertRegistryTarget(target);
      return Environment.GetEnvironmentVariableFromRegistry(variable, fromMachine);
    }

    /// <summary>Retrieves all environment variable names and their values from the current process, or from the Windows operating system registry key for the current user or local machine.</summary>
    /// <param name="target">One of the <see cref="T:System.EnvironmentVariableTarget" /> values. Only <see cref="F:System.EnvironmentVariableTarget.Process" /> is supported on .NET Core running on Unix-based systems.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation for the specified value of <paramref name="target" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="target" /> contains an illegal value.</exception>
    /// <returns>A dictionary that contains all environment variable names and their values from the source specified by the <paramref name="target" /> parameter; otherwise, an empty dictionary if no environment variables are found.</returns>
    public static IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target) => target == EnvironmentVariableTarget.Process ? Environment.GetEnvironmentVariables() : Environment.GetEnvironmentVariablesFromRegistry(Environment.ValidateAndConvertRegistryTarget(target));

    /// <summary>Creates, modifies, or deletes an environment variable stored in the current process.</summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="value">A value to assign to <paramref name="variable" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="variable" /> contains a zero-length string, an initial hexadecimal zero character (0x00), or an equal sign ("=").
    /// 
    /// -or-
    /// 
    /// The length of <paramref name="variable" /> or <paramref name="value" /> is greater than or equal to 32,767 characters.
    /// 
    /// -or-
    /// 
    /// An error occurred during the execution of this operation.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    public static void SetEnvironmentVariable(string variable, string? value)
    {
      Environment.ValidateVariableAndValue(variable, ref value);
      Environment.SetEnvironmentVariableCore(variable, value);
    }

    /// <summary>Creates, modifies, or deletes an environment variable stored in the current process or in the Windows operating system registry key reserved for the current user or local machine.</summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="value">A value to assign to <paramref name="variable" />.</param>
    /// <param name="target">One of the enumeration values that specifies the location of the environment variable.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="variable" /> contains a zero-length string, an initial hexadecimal zero character (0x00), or an equal sign ("=").
    /// 
    /// -or-
    /// 
    /// The length of <paramref name="variable" /> is greater than or equal to 32,767 characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is not a member of the <see cref="T:System.EnvironmentVariableTarget" /> enumeration.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see cref="F:System.EnvironmentVariableTarget.Machine" /> or <see cref="F:System.EnvironmentVariableTarget.User" />, and the length of <paramref name="variable" /> is greater than or equal to 255.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see cref="F:System.EnvironmentVariableTarget.Process" /> and the length of <paramref name="value" /> is greater than or equal to 32,767 characters.
    /// 
    /// -or-
    /// 
    /// An error occurred during the execution of this operation.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    public static void SetEnvironmentVariable(
      string variable,
      string? value,
      EnvironmentVariableTarget target)
    {
      if (target == EnvironmentVariableTarget.Process)
      {
        Environment.SetEnvironmentVariable(variable, value);
      }
      else
      {
        Environment.ValidateVariableAndValue(variable, ref value);
        bool fromMachine = Environment.ValidateAndConvertRegistryTarget(target);
        Environment.SetEnvironmentVariableFromRegistry(variable, value, fromMachine);
      }
    }

    /// <summary>Gets the command line for this process.</summary>
    /// <returns>A string containing command-line arguments.</returns>
    public static string CommandLine => PasteArguments.Paste((IEnumerable<string>) Environment.GetCommandLineArgs(), true);

    /// <summary>Gets or sets the fully qualified path of the current working directory.</summary>
    /// <exception cref="T:System.ArgumentException">Attempted to set to an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">Attempted to set to <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the appropriate permission.</exception>
    /// <returns>The directory path.</returns>
    public static string CurrentDirectory
    {
      get => Environment.CurrentDirectoryCore;
      set
      {
        switch (value)
        {
          case "":
            throw new ArgumentException(SR.Argument_PathEmpty, nameof (value));
          case null:
            throw new ArgumentNullException(nameof (value));
          default:
            Environment.CurrentDirectoryCore = value;
            break;
        }
      }
    }

    /// <summary>Replaces the name of each environment variable embedded in the specified string with the string equivalent of the value of the variable, then returns the resulting string.</summary>
    /// <param name="name">A string containing the names of zero or more environment variables. Each environment variable is quoted with the percent sign character (%).</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>A string with each environment variable replaced by its value.</returns>
    public static string ExpandEnvironmentVariables(string name)
    {
      switch (name)
      {
        case "":
          return name;
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          return Environment.ExpandEnvironmentVariablesCore(name);
      }
    }


    #nullable disable
    internal static void SetCommandLineArgs(string[] cmdLineArgs) => Environment.s_commandLineArgs = cmdLineArgs;


    #nullable enable
    /// <summary>Gets the path to the system special folder that is identified by the specified enumeration.</summary>
    /// <param name="folder">One of enumeration values that identifies a system special folder.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="folder" /> is not a member of <see cref="T:System.Environment.SpecialFolder" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current platform is not supported.</exception>
    /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string ("").
    /// 
    /// A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
    public static string GetFolderPath(Environment.SpecialFolder folder) => Environment.GetFolderPath(folder, Environment.SpecialFolderOption.None);

    /// <summary>Gets the path to the system special folder that is identified by the specified enumeration, and uses a specified option for accessing special folders.</summary>
    /// <param name="folder">One of the enumeration values that identifies a system special folder.</param>
    /// <param name="option">One of the enumeration values that specifies options to use for accessing a special folder.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="folder" /> is not a member of <see cref="T:System.Environment.SpecialFolder" />.
    /// -or-
    /// 
    /// <paramref name="options" /> is not a member of <see cref="T:System.Environment.SpecialFolderOption" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current platform is not supported.</exception>
    /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string ("").
    /// 
    /// A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
    public static string GetFolderPath(
      Environment.SpecialFolder folder,
      Environment.SpecialFolderOption option)
    {
      if (!Enum.IsDefined(typeof (Environment.SpecialFolder), (object) folder))
        throw new ArgumentOutOfRangeException(nameof (folder), (object) folder, SR.Format(SR.Arg_EnumIllegalVal, (object) folder));
      return option == Environment.SpecialFolderOption.None || Enum.IsDefined(typeof (Environment.SpecialFolderOption), (object) option) ? Environment.GetFolderPathCore(folder, option) : throw new ArgumentOutOfRangeException(nameof (option), (object) option, SR.Format(SR.Arg_EnumIllegalVal, (object) option));
    }

    /// <summary>Gets the unique identifier for the current process.</summary>
    /// <returns>A number that represents the unique identifier for the current process.</returns>
    public static int ProcessId
    {
      get
      {
        int processId = Environment.s_processId;
        if (processId == 0)
        {
          Interlocked.CompareExchange(ref Environment.s_processId, Environment.GetProcessId(), 0);
          processId = Environment.s_processId;
        }
        return processId;
      }
    }

    /// <summary>Returns the path of the executable that started the currently executing process. Returns <see langword="null" /> when the path is not available.</summary>
    /// <returns>The path of the executable that started the currently executing process.</returns>
    public static string? ProcessPath
    {
      get
      {
        string processPath = Environment.s_processPath;
        if (processPath == null)
        {
          Interlocked.CompareExchange<string>(ref Environment.s_processPath, Environment.GetProcessPath() ?? "", (string) null);
          processPath = Environment.s_processPath;
        }
        return processPath.Length == 0 ? (string) null : processPath;
      }
    }

    /// <summary>Gets a value that indicates whether the current process is a 64-bit process.</summary>
    /// <returns>
    /// <see langword="true" /> if the process is 64-bit; otherwise, <see langword="false" />.</returns>
    public static bool Is64BitProcess => IntPtr.Size == 8;

    /// <summary>Gets a value that indicates whether the current operating system is a 64-bit operating system.</summary>
    /// <returns>
    /// <see langword="true" /> if the operating system is 64-bit; otherwise, <see langword="false" />.</returns>
    public static bool Is64BitOperatingSystem
    {
      get
      {
        if (Environment.Is64BitProcess)
          ;
        return true;
      }
    }

    /// <summary>Gets the newline string defined for this environment.</summary>
    /// <returns>
    /// <see langword="\r\n" /> for non-Unix platforms, or <see langword="\n" /> for Unix platforms.</returns>
    public static string NewLine => "\r\n";

    /// <summary>Gets the current platform identifier and version number.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property was unable to obtain the system version.
    /// 
    /// -or-
    /// 
    /// The obtained platform identifier is not a member of <see cref="T:System.PlatformID" /></exception>
    /// <returns>The platform identifier and version number.</returns>
    public static OperatingSystem OSVersion
    {
      get
      {
        OperatingSystem osVersion = Environment.s_osVersion;
        if (osVersion == null)
        {
          Interlocked.CompareExchange<OperatingSystem>(ref Environment.s_osVersion, Environment.GetOSVersion(), (OperatingSystem) null);
          osVersion = Environment.s_osVersion;
        }
        return osVersion;
      }
    }

    /// <summary>Gets a version consisting of the major, minor, build, and revision numbers of the common language runtime.</summary>
    /// <returns>The version of the common language runtime.</returns>
    public static Version Version
    {
      get
      {
        ReadOnlySpan<char> readOnlySpan = typeof (object).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.AsSpan();
        int length = readOnlySpan.IndexOfAny<char>('-', '+', ' ');
        if (length != -1)
          readOnlySpan = readOnlySpan.Slice(0, length);
        Version result;
        return !Version.TryParse(readOnlySpan, out result) ? new Version() : result;
      }
    }

    /// <summary>Gets current stack trace information.</summary>
    /// <returns>A string containing stack trace information. This value can be <see cref="F:System.String.Empty" />.</returns>
    public static string StackTrace
    {
      [MethodImpl(MethodImplOptions.NoInlining)] get => new System.Diagnostics.StackTrace(true).ToString(System.Diagnostics.StackTrace.TraceFormat.Normal);
    }

    private static bool ValidateAndConvertRegistryTarget(EnvironmentVariableTarget target)
    {
      if (target == EnvironmentVariableTarget.Machine)
        return true;
      if (target == EnvironmentVariableTarget.User)
        return false;
      throw new ArgumentOutOfRangeException(nameof (target), (object) target, SR.Format(SR.Arg_EnumIllegalVal, (object) target));
    }


    #nullable disable
    private static void ValidateVariableAndValue(string variable, ref string value)
    {
      switch (variable)
      {
        case "":
          throw new ArgumentException(SR.Argument_StringZeroLength, nameof (variable));
        case null:
          throw new ArgumentNullException(nameof (variable));
        default:
          if (variable[0] == char.MinValue)
            throw new ArgumentException(SR.Argument_StringFirstCharIsZero, nameof (variable));
          if (variable.Contains('='))
            throw new ArgumentException(SR.Argument_IllegalEnvVarName, nameof (variable));
          if (!string.IsNullOrEmpty(value) && value[0] != char.MinValue)
            break;
          value = (string) null;
          break;
      }
    }

    internal static bool IsWindows8OrAbove => Environment.WindowsVersion.IsWindows8OrAbove;

    private static string GetEnvironmentVariableFromRegistry(string variable, bool fromMachine)
    {
      using (RegistryKey registryKey = Environment.OpenEnvironmentKeyIfExists(fromMachine, false))
        return registryKey?.GetValue(variable) as string;
    }

    private static unsafe void SetEnvironmentVariableFromRegistry(
      string variable,
      string value,
      bool fromMachine)
    {
      if (!fromMachine && variable.Length >= (int) byte.MaxValue)
        throw new ArgumentException(SR.Argument_LongEnvVarValue, nameof (variable));
      using (RegistryKey registryKey = Environment.OpenEnvironmentKeyIfExists(fromMachine, true))
      {
        if (registryKey != null)
        {
          if (value == null)
            registryKey.DeleteValue(variable, false);
          else
            registryKey.SetValue(variable, value);
        }
      }
      fixed (char* lParam = &nameof (Environment).GetPinnableReference())
        Interop.User32.SendMessageTimeout(new IntPtr((int) ushort.MaxValue), 26, IntPtr.Zero, (IntPtr) (void*) lParam, 0, 1000, out IntPtr _);
    }

    private static IDictionary GetEnvironmentVariablesFromRegistry(bool fromMachine)
    {
      Hashtable variablesFromRegistry = new Hashtable();
      using (RegistryKey registryKey = Environment.OpenEnvironmentKeyIfExists(fromMachine, false))
      {
        if (registryKey != null)
        {
          foreach (string valueName in registryKey.GetValueNames())
          {
            string str = registryKey.GetValue(valueName, (object) "").ToString();
            try
            {
              variablesFromRegistry.Add((object) valueName, (object) str);
            }
            catch (ArgumentException ex)
            {
            }
          }
        }
      }
      return (IDictionary) variablesFromRegistry;
    }

    private static RegistryKey OpenEnvironmentKeyIfExists(
      bool fromMachine,
      bool writable)
    {
      RegistryKey registryKey;
      string name;
      if (fromMachine)
      {
        registryKey = Registry.LocalMachine;
        name = "System\\CurrentControlSet\\Control\\Session Manager\\Environment";
      }
      else
      {
        registryKey = Registry.CurrentUser;
        name = nameof (Environment);
      }
      return registryKey.OpenSubKey(name, writable);
    }


    #nullable enable
    /// <summary>Gets the user name of the person who is associated with the current thread.</summary>
    /// <returns>The user name of the person who is associated with the current thread.</returns>
    public static unsafe string UserName
    {
      get
      {
        ValueStringBuilder builder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(80)), 40));
        Environment.GetUserName(ref builder);
        ReadOnlySpan<char> span = builder.AsSpan();
        int num = span.IndexOf<char>('\\');
        if (num != -1)
          span = span.Slice(num + 1);
        string userName = span.ToString();
        builder.Dispose();
        return userName;
      }
    }


    #nullable disable
    private static void GetUserName(ref ValueStringBuilder builder)
    {
      uint lpnSize = 0;
      while (Interop.Secur32.GetUserNameExW(2, ref builder.GetPinnableReference(), ref lpnSize) == Interop.BOOLEAN.FALSE)
      {
        if (Marshal.GetLastPInvokeError() == 234)
        {
          builder.EnsureCapacity(checked ((int) lpnSize));
        }
        else
        {
          builder.Length = 0;
          return;
        }
      }
      builder.Length = (int) lpnSize;
    }


    #nullable enable
    /// <summary>Gets the network domain name associated with the current user.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">The operating system does not support retrieving the network domain name.</exception>
    /// <exception cref="T:System.InvalidOperationException">The network domain name cannot be retrieved.</exception>
    /// <returns>The network domain name associated with the current user.</returns>
    public static unsafe string UserDomainName
    {
      get
      {
        ValueStringBuilder builder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(80)), 40));
        Environment.GetUserName(ref builder);
        int num = builder.AsSpan().IndexOf<char>('\\');
        if (num != -1)
        {
          builder.Length = num;
          return builder.ToString();
        }
        ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(128)), 64));
        uint capacity = (uint) valueStringBuilder.Capacity;
        Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(68)), 68);
        uint cbSid = 68;
        while (!Interop.Advapi32.LookupAccountNameW((string) null, ref builder.GetPinnableReference(), ref MemoryMarshal.GetReference<byte>(span), ref cbSid, ref valueStringBuilder.GetPinnableReference(), ref capacity, out uint _))
        {
          int lastPinvokeError = Marshal.GetLastPInvokeError();
          if (lastPinvokeError != 122)
            throw new InvalidOperationException(Win32Marshal.GetMessage(lastPinvokeError));
          valueStringBuilder.EnsureCapacity((int) capacity);
        }
        builder.Dispose();
        valueStringBuilder.Length = (int) capacity;
        return valueStringBuilder.ToString();
      }
    }


    #nullable disable
    private static string GetFolderPathCore(
      Environment.SpecialFolder folder,
      Environment.SpecialFolderOption option)
    {
      string folderGuid;
      switch (folder)
      {
        case Environment.SpecialFolder.Desktop:
          folderGuid = "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}";
          break;
        case Environment.SpecialFolder.Programs:
          folderGuid = "{A77F5D77-2E2B-44C3-A6A2-ABA601054A51}";
          break;
        case Environment.SpecialFolder.Personal:
          folderGuid = "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}";
          break;
        case Environment.SpecialFolder.Favorites:
          folderGuid = "{1777F761-68AD-4D8A-87BD-30B759FA33DD}";
          break;
        case Environment.SpecialFolder.Startup:
          folderGuid = "{B97D20BB-F46A-4C97-BA10-5E3608430854}";
          break;
        case Environment.SpecialFolder.Recent:
          folderGuid = "{AE50C081-EBD2-438A-8655-8A092E34987A}";
          break;
        case Environment.SpecialFolder.SendTo:
          folderGuid = "{8983036C-27C0-404B-8F08-102D10DCFD74}";
          break;
        case Environment.SpecialFolder.StartMenu:
          folderGuid = "{625B53C3-AB48-4EC1-BA1F-A1EF4146FC19}";
          break;
        case Environment.SpecialFolder.MyMusic:
          folderGuid = "{4BD8D571-6D19-48D3-BE97-422220080E43}";
          break;
        case Environment.SpecialFolder.MyVideos:
          folderGuid = "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}";
          break;
        case Environment.SpecialFolder.DesktopDirectory:
          folderGuid = "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}";
          break;
        case Environment.SpecialFolder.MyComputer:
          folderGuid = "{0AC0837C-BBF8-452A-850D-79D08E667CA7}";
          break;
        case Environment.SpecialFolder.NetworkShortcuts:
          folderGuid = "{C5ABBF53-E17F-4121-8900-86626FC2C973}";
          break;
        case Environment.SpecialFolder.Fonts:
          folderGuid = "{FD228CB7-AE11-4AE3-864C-16F3910AB8FE}";
          break;
        case Environment.SpecialFolder.Templates:
          folderGuid = "{A63293E8-664E-48DB-A079-DF759E0509F7}";
          break;
        case Environment.SpecialFolder.CommonStartMenu:
          folderGuid = "{A4115719-D62E-491D-AA7C-E74B8BE3B067}";
          break;
        case Environment.SpecialFolder.CommonPrograms:
          folderGuid = "{0139D44E-6AFE-49F2-8690-3DAFCAE6FFB8}";
          break;
        case Environment.SpecialFolder.CommonStartup:
          folderGuid = "{82A5EA35-D9CD-47C5-9629-E15D2F714E6E}";
          break;
        case Environment.SpecialFolder.CommonDesktopDirectory:
          folderGuid = "{C4AA340D-F20F-4863-AFEF-F87EF2E6BA25}";
          break;
        case Environment.SpecialFolder.ApplicationData:
          folderGuid = "{3EB685DB-65F9-4CF6-A03A-E3EF65729F3D}";
          break;
        case Environment.SpecialFolder.PrinterShortcuts:
          folderGuid = "{76FC4E2D-D6AD-4519-A663-37BD56068185}";
          break;
        case Environment.SpecialFolder.LocalApplicationData:
          folderGuid = "{F1B32785-6FBA-4FCF-9D55-7B8E7F157091}";
          break;
        case Environment.SpecialFolder.InternetCache:
          folderGuid = "{352481E8-33BE-4251-BA85-6007CAEDCF9D}";
          break;
        case Environment.SpecialFolder.Cookies:
          folderGuid = "{2B0F765D-C0E9-4171-908E-08A611B84FF6}";
          break;
        case Environment.SpecialFolder.History:
          folderGuid = "{D9DC8A3B-B784-432E-A781-5A1130A75963}";
          break;
        case Environment.SpecialFolder.CommonApplicationData:
          folderGuid = "{62AB5D82-FDC1-4DC3-A9DD-070D1D495D97}";
          break;
        case Environment.SpecialFolder.Windows:
          folderGuid = "{F38BF404-1D43-42F2-9305-67DE0B28FC23}";
          break;
        case Environment.SpecialFolder.System:
          folderGuid = "{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}";
          break;
        case Environment.SpecialFolder.ProgramFiles:
          folderGuid = "{905e63b6-c1bf-494e-b29c-65b732d3d21a}";
          break;
        case Environment.SpecialFolder.MyPictures:
          folderGuid = "{33E28130-4E1E-4676-835A-98395C3BC3BB}";
          break;
        case Environment.SpecialFolder.UserProfile:
          folderGuid = "{5E6C858F-0E22-4760-9AFE-EA3317B67173}";
          break;
        case Environment.SpecialFolder.SystemX86:
          folderGuid = "{D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27}";
          break;
        case Environment.SpecialFolder.ProgramFilesX86:
          folderGuid = "{7C5A40EF-A0FB-4BFC-874A-C0F2E0B9FA8E}";
          break;
        case Environment.SpecialFolder.CommonProgramFiles:
          folderGuid = "{F7F1ED05-9F6D-47A2-AAAE-29D317C6F066}";
          break;
        case Environment.SpecialFolder.CommonProgramFilesX86:
          folderGuid = "{DE974D24-D9C6-4D3E-BF91-F4455120B917}";
          break;
        case Environment.SpecialFolder.CommonTemplates:
          folderGuid = "{B94237E7-57AC-4347-9151-B08C6C32D1F7}";
          break;
        case Environment.SpecialFolder.CommonDocuments:
          folderGuid = "{ED4824AF-DCE4-45A8-81E2-FC7965083634}";
          break;
        case Environment.SpecialFolder.CommonAdminTools:
          folderGuid = "{D0384E7D-BAC3-4797-8F14-CBA229B392B5}";
          break;
        case Environment.SpecialFolder.AdminTools:
          folderGuid = "{724EF170-A42D-4FEF-9F26-B60E846FBA4F}";
          break;
        case Environment.SpecialFolder.CommonMusic:
          folderGuid = "{3214FAB5-9757-4298-BB61-92A9DEAA44FF}";
          break;
        case Environment.SpecialFolder.CommonPictures:
          folderGuid = "{B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5}";
          break;
        case Environment.SpecialFolder.CommonVideos:
          folderGuid = "{2400183A-6185-49FB-A2D8-4A392A602BA3}";
          break;
        case Environment.SpecialFolder.Resources:
          folderGuid = "{8AD10C31-2ADB-4296-A8F7-E4701232C972}";
          break;
        case Environment.SpecialFolder.LocalizedResources:
          folderGuid = "{2A00375E-224C-49DE-B8D1-440DF7EF3DDC}";
          break;
        case Environment.SpecialFolder.CommonOemLinks:
          folderGuid = "{C1BAE2D0-10DF-4334-BEDD-7AA20B227A9D}";
          break;
        case Environment.SpecialFolder.CDBurning:
          folderGuid = "{9E52AB10-F80D-49DF-ACB8-4330F5687855}";
          break;
        default:
          return string.Empty;
      }
      return Environment.GetKnownFolderPath(folderGuid, option);
    }

    private static string GetKnownFolderPath(
      string folderGuid,
      Environment.SpecialFolderOption option)
    {
      string ppszPath;
      return Interop.Shell32.SHGetKnownFolderPath(new Guid(folderGuid), (uint) option, IntPtr.Zero, out ppszPath) != 0 ? string.Empty : ppszPath;
    }


    #nullable enable
    private static unsafe string CurrentDirectoryCore
    {
      get
      {
        ValueStringBuilder outputBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(520)), 260));
        uint currentDirectory;
        while ((long) (currentDirectory = Interop.Kernel32.GetCurrentDirectory((uint) outputBuilder.Capacity, ref outputBuilder.GetPinnableReference())) > (long) outputBuilder.Capacity)
          outputBuilder.EnsureCapacity((int) currentDirectory);
        outputBuilder.Length = currentDirectory != 0U ? (int) currentDirectory : throw Win32Marshal.GetExceptionForLastWin32Error();
        if (!outputBuilder.AsSpan().Contains<char>('~'))
          return outputBuilder.ToString();
        string currentDirectoryCore = PathHelper.TryExpandShortFileName(ref outputBuilder, (string) null);
        outputBuilder.Dispose();
        return currentDirectoryCore;
      }
      set
      {
        if (!Interop.Kernel32.SetCurrentDirectory(value))
        {
          int lastPinvokeError = Marshal.GetLastPInvokeError();
          throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError == 2 ? 3 : lastPinvokeError, value);
        }
      }
    }

    /// <summary>Returns an array of string containing the names of the logical drives on the current computer.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions.</exception>
    /// <returns>An array of strings where each element contains the name of a logical drive. For example, if the computer's hard drive is the first logical drive, the first element returned is "C:\".</returns>
    public static string[] GetLogicalDrives() => DriveInfoInternal.GetLogicalDrives();

    /// <summary>Gets the number of bytes in the operating system's memory page.</summary>
    /// <returns>The number of bytes in the system memory page.</returns>
    public static int SystemPageSize
    {
      get
      {
        Interop.Kernel32.SYSTEM_INFO lpSystemInfo;
        Interop.Kernel32.GetSystemInfo(out lpSystemInfo);
        return lpSystemInfo.dwPageSize;
      }
    }


    #nullable disable
    private static unsafe string ExpandEnvironmentVariablesCore(string name)
    {
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128));
      uint capacity;
      while ((long) (capacity = Interop.Kernel32.ExpandEnvironmentStrings(name, ref valueStringBuilder.GetPinnableReference(), (uint) valueStringBuilder.Capacity)) > (long) valueStringBuilder.Capacity)
        valueStringBuilder.EnsureCapacity((int) capacity);
      if (capacity == 0U)
        throw Win32Marshal.GetExceptionForLastWin32Error();
      valueStringBuilder.Length = (int) capacity - 1;
      return valueStringBuilder.ToString();
    }


    #nullable enable
    /// <summary>Gets the NetBIOS name of this local computer.</summary>
    /// <exception cref="T:System.InvalidOperationException">The name of this computer cannot be obtained.</exception>
    /// <returns>The name of this computer.</returns>
    public static string MachineName => Interop.Kernel32.GetComputerName() ?? throw new InvalidOperationException(SR.InvalidOperation_ComputerName);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int GetProcessId() => (int) Interop.Kernel32.GetCurrentProcessId();


    #nullable disable
    private static unsafe string GetProcessPath()
    {
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(520)), 260));
      uint moduleFileName;
      while ((long) (moduleFileName = Interop.Kernel32.GetModuleFileName(IntPtr.Zero, ref valueStringBuilder.GetPinnableReference(), (uint) valueStringBuilder.Capacity)) >= (long) valueStringBuilder.Capacity)
        valueStringBuilder.EnsureCapacity((int) moduleFileName);
      valueStringBuilder.Length = moduleFileName != 0U ? (int) moduleFileName : throw Win32Marshal.GetExceptionForLastWin32Error();
      return valueStringBuilder.ToString();
    }

    private static unsafe OperatingSystem GetOSVersion()
    {
      Interop.NtDll.RTL_OSVERSIONINFOEX osvi;
      if (Interop.NtDll.RtlGetVersionEx(out osvi) != 0)
        throw new InvalidOperationException(SR.InvalidOperation_GetVersion);
      Version version = new Version((int) osvi.dwMajorVersion, (int) osvi.dwMinorVersion, (int) osvi.dwBuildNumber, 0);
      // ISSUE: reference to a compiler-generated field
      return osvi.szCSDVersion.FixedElementField == char.MinValue ? new OperatingSystem(PlatformID.Win32NT, version) : new OperatingSystem(PlatformID.Win32NT, version, new string(osvi.szCSDVersion));
    }


    #nullable enable
    /// <summary>Gets the fully qualified path of the system directory.</summary>
    /// <returns>A string containing a directory path.</returns>
    public static unsafe string SystemDirectory
    {
      get
      {
        ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(64)), 32));
        uint systemDirectoryW;
        while ((long) (systemDirectoryW = Interop.Kernel32.GetSystemDirectoryW(ref valueStringBuilder.GetPinnableReference(), (uint) valueStringBuilder.Capacity)) > (long) valueStringBuilder.Capacity)
          valueStringBuilder.EnsureCapacity((int) systemDirectoryW);
        valueStringBuilder.Length = systemDirectoryW != 0U ? (int) systemDirectoryW : throw Win32Marshal.GetExceptionForLastWin32Error();
        return valueStringBuilder.ToString();
      }
    }

    /// <summary>Gets a value indicating whether the current process is running in user interactive mode.</summary>
    /// <returns>
    /// <see langword="true" /> if the current process is running in user interactive mode; otherwise, <see langword="false" />.</returns>
    public static unsafe bool UserInteractive
    {
      get
      {
        IntPtr processWindowStation = Interop.User32.GetProcessWindowStation();
        if (processWindowStation != IntPtr.Zero)
        {
          Interop.User32.USEROBJECTFLAGS userobjectflags = new Interop.User32.USEROBJECTFLAGS();
          uint lpnLengthNeeded = 0;
          if (Interop.User32.GetUserObjectInformationW(processWindowStation, 1, (void*) &userobjectflags, (uint) sizeof (Interop.User32.USEROBJECTFLAGS), ref lpnLengthNeeded))
            return (userobjectflags.dwFlags & 1) != 0;
        }
        return true;
      }
    }

    /// <summary>Gets the amount of physical memory mapped to the process context.</summary>
    /// <returns>A 64-bit signed integer containing the number of bytes of physical memory mapped to the process context.</returns>
    public static long WorkingSet
    {
      get
      {
        Interop.Kernel32.PROCESS_MEMORY_COUNTERS ppsmemCounters = new Interop.Kernel32.PROCESS_MEMORY_COUNTERS();
        ppsmemCounters.cb = (uint) sizeof (Interop.Kernel32.PROCESS_MEMORY_COUNTERS);
        return !Interop.Kernel32.GetProcessMemoryInfo(Interop.Kernel32.GetCurrentProcess(), ref ppsmemCounters, ppsmemCounters.cb) ? 0L : (long) (ulong) ppsmemCounters.WorkingSetSize;
      }
    }


    #nullable disable
    private static unsafe string GetEnvironmentVariableCore(string variable)
    {
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128));
      uint environmentVariable;
      while ((long) (environmentVariable = Interop.Kernel32.GetEnvironmentVariable(variable, ref valueStringBuilder.GetPinnableReference(), (uint) valueStringBuilder.Capacity)) > (long) valueStringBuilder.Capacity)
        valueStringBuilder.EnsureCapacity((int) environmentVariable);
      if (environmentVariable == 0U && Marshal.GetLastPInvokeError() == 203)
      {
        valueStringBuilder.Dispose();
        return (string) null;
      }
      valueStringBuilder.Length = (int) environmentVariable;
      return valueStringBuilder.ToString();
    }

    private static void SetEnvironmentVariableCore(string variable, string value)
    {
      if (Interop.Kernel32.SetEnvironmentVariable(variable, value))
        return;
      int lastPinvokeError = Marshal.GetLastPInvokeError();
      switch (lastPinvokeError)
      {
        case 8:
        case 1450:
          throw new OutOfMemoryException(Interop.Kernel32.GetMessage(lastPinvokeError));
        case 203:
          break;
        case 206:
          throw new ArgumentException(SR.Argument_LongEnvVarValue);
        default:
          throw new ArgumentException(Interop.Kernel32.GetMessage(lastPinvokeError));
      }
    }


    #nullable enable
    /// <summary>Retrieves all environment variable names and their values from the current process.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    /// <exception cref="T:System.OutOfMemoryException">The buffer is out of memory.</exception>
    /// <returns>A dictionary that contains all environment variable names and their values; otherwise, an empty dictionary if no environment variables are found.</returns>
    public static unsafe IDictionary GetEnvironmentVariables()
    {
      char* environmentStringsW = Interop.Kernel32.GetEnvironmentStringsW();
      if ((IntPtr) environmentStringsW == IntPtr.Zero)
        throw new OutOfMemoryException();
      try
      {
        Hashtable environmentVariables = new Hashtable();
        char* chPtr = environmentStringsW;
        while (true)
        {
          ReadOnlySpan<char> fromNullTerminated = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(chPtr);
          if (!fromNullTerminated.IsEmpty)
          {
            int length = fromNullTerminated.IndexOf<char>('=');
            if (length > 0)
            {
              string key = new string(fromNullTerminated.Slice(0, length));
              string str = new string(fromNullTerminated.Slice(length + 1));
              try
              {
                environmentVariables.Add((object) key, (object) str);
              }
              catch (ArgumentException ex)
              {
              }
            }
            chPtr += fromNullTerminated.Length + 1;
          }
          else
            break;
        }
        return (IDictionary) environmentVariables;
      }
      finally
      {
        Interop.Kernel32.FreeEnvironmentStringsW(environmentStringsW);
      }
    }

    /// <summary>Specifies enumerated constants used to retrieve directory paths to system special folders.</summary>
    public enum SpecialFolder
    {
      /// <summary>The logical Desktop rather than the physical file system location.</summary>
      Desktop = 0,
      /// <summary>The directory that contains the user's program groups.</summary>
      Programs = 2,
      /// <summary>The My Documents folder. This member is equivalent to  <see cref="F:System.Environment.SpecialFolder.Personal" />.</summary>
      MyDocuments = 5,
      /// <summary>The directory that serves as a common repository for documents.  This member is equivalent to  <see cref="F:System.Environment.SpecialFolder.MyDocuments" />.</summary>
      Personal = 5,
      /// <summary>The directory that serves as a common repository for the user's favorite items.</summary>
      Favorites = 6,
      /// <summary>The directory that corresponds to the user's Startup program group. The system starts these programs whenever a user logs on or starts Windows.</summary>
      Startup = 7,
      /// <summary>The directory that contains the user's most recently used documents.</summary>
      Recent = 8,
      /// <summary>The directory that contains the Send To menu items.</summary>
      SendTo = 9,
      /// <summary>The directory that contains the Start menu items.</summary>
      StartMenu = 11, // 0x0000000B
      /// <summary>The My Music folder.</summary>
      MyMusic = 13, // 0x0000000D
      /// <summary>The file system directory that serves as a repository for videos that belong to a user.</summary>
      MyVideos = 14, // 0x0000000E
      /// <summary>The directory used to physically store file objects on the desktop. Do not confuse this directory with the desktop folder itself, which is a virtual folder.</summary>
      DesktopDirectory = 16, // 0x00000010
      /// <summary>The My Computer folder. When passed to the <see langword="Environment.GetFolderPath" /> method, the <see langword="MyComputer" /> enumeration member always yields the empty string ("") because no path is defined for the My Computer folder.</summary>
      MyComputer = 17, // 0x00000011
      /// <summary>A file system directory that contains the link objects that may exist in the My Network Places virtual folder.</summary>
      NetworkShortcuts = 19, // 0x00000013
      /// <summary>A virtual folder that contains fonts.</summary>
      Fonts = 20, // 0x00000014
      /// <summary>The directory that serves as a common repository for document templates.</summary>
      Templates = 21, // 0x00000015
      /// <summary>The file system directory that contains the programs and folders that appear on the Start menu for all users.</summary>
      CommonStartMenu = 22, // 0x00000016
      /// <summary>A folder for components that are shared across applications.</summary>
      CommonPrograms = 23, // 0x00000017
      /// <summary>The file system directory that contains the programs that appear in the Startup folder for all users.</summary>
      CommonStartup = 24, // 0x00000018
      /// <summary>The file system directory that contains files and folders that appear on the desktop for all users.</summary>
      CommonDesktopDirectory = 25, // 0x00000019
      /// <summary>The directory that serves as a common repository for application-specific data for the current roaming user. A roaming user works on more than one computer on a network. A roaming user's profile is kept on a server on the network and is loaded onto a system when the user logs on.</summary>
      ApplicationData = 26, // 0x0000001A
      /// <summary>The file system directory that contains the link objects that can exist in the Printers virtual folder.</summary>
      PrinterShortcuts = 27, // 0x0000001B
      /// <summary>The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user.</summary>
      LocalApplicationData = 28, // 0x0000001C
      /// <summary>The directory that serves as a common repository for temporary Internet files.</summary>
      InternetCache = 32, // 0x00000020
      /// <summary>The directory that serves as a common repository for Internet cookies.</summary>
      Cookies = 33, // 0x00000021
      /// <summary>The directory that serves as a common repository for Internet history items.</summary>
      History = 34, // 0x00000022
      /// <summary>The directory that serves as a common repository for application-specific data that is used by all users.</summary>
      CommonApplicationData = 35, // 0x00000023
      /// <summary>The Windows directory or SYSROOT. This corresponds to the %windir% or %SYSTEMROOT% environment variables.</summary>
      Windows = 36, // 0x00000024
      /// <summary>The System directory.</summary>
      System = 37, // 0x00000025
      /// <summary>The program files directory.
      /// 
      /// In a non-x86 process, passing <see cref="F:System.Environment.SpecialFolder.ProgramFiles" /> to the <see cref="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)" /> method returns the path for non-x86 programs. To get the x86 program files directory in a non-x86 process, use the <see cref="F:System.Environment.SpecialFolder.ProgramFilesX86" /> member.</summary>
      ProgramFiles = 38, // 0x00000026
      /// <summary>The My Pictures folder.</summary>
      MyPictures = 39, // 0x00000027
      /// <summary>The user's profile folder. Applications should not create files or folders at this level; they should put their data under the locations referred to by <see cref="F:System.Environment.SpecialFolder.ApplicationData" />.</summary>
      UserProfile = 40, // 0x00000028
      /// <summary>The Windows System folder.</summary>
      SystemX86 = 41, // 0x00000029
      /// <summary>The x86 Program Files folder.</summary>
      ProgramFilesX86 = 42, // 0x0000002A
      /// <summary>The directory for components that are shared across applications.
      /// 
      /// To get the x86 common program files directory in a non-x86 process, use the <see cref="F:System.Environment.SpecialFolder.ProgramFilesX86" /> member.</summary>
      CommonProgramFiles = 43, // 0x0000002B
      /// <summary>The Program Files folder.</summary>
      CommonProgramFilesX86 = 44, // 0x0000002C
      /// <summary>The file system directory that contains the templates that are available to all users.</summary>
      CommonTemplates = 45, // 0x0000002D
      /// <summary>The file system directory that contains documents that are common to all users.</summary>
      CommonDocuments = 46, // 0x0000002E
      /// <summary>The file system directory that contains administrative tools for all users of the computer.</summary>
      CommonAdminTools = 47, // 0x0000002F
      /// <summary>The file system directory that is used to store administrative tools for an individual user. The Microsoft Management Console (MMC) will save customized consoles to this directory, and it will roam with the user.</summary>
      AdminTools = 48, // 0x00000030
      /// <summary>The file system directory that serves as a repository for music files common to all users.</summary>
      CommonMusic = 53, // 0x00000035
      /// <summary>The file system directory that serves as a repository for image files common to all users.</summary>
      CommonPictures = 54, // 0x00000036
      /// <summary>The file system directory that serves as a repository for video files common to all users.</summary>
      CommonVideos = 55, // 0x00000037
      /// <summary>The file system directory that contains resource data.</summary>
      Resources = 56, // 0x00000038
      /// <summary>The file system directory that contains localized resource data.</summary>
      LocalizedResources = 57, // 0x00000039
      /// <summary>This value is recognized in Windows Vista for backward compatibility, but the special folder itself is no longer used.</summary>
      CommonOemLinks = 58, // 0x0000003A
      /// <summary>The file system directory that acts as a staging area for files waiting to be written to a CD.</summary>
      CDBurning = 59, // 0x0000003B
    }

    /// <summary>Specifies options to use for getting the path to a special folder.</summary>
    public enum SpecialFolderOption
    {
      /// <summary>The path to the folder is verified. If the folder exists, the path is returned. If the folder does not exist, an empty string is returned. This is the default behavior.</summary>
      None = 0,
      /// <summary>The path to the folder is returned without verifying whether the path exists. If the folder is located on a network, specifying this option can reduce lag time.</summary>
      DoNotVerify = 16384, // 0x00004000
      /// <summary>The path to the folder is created if it does not already exist.</summary>
      Create = 32768, // 0x00008000
    }


    #nullable disable
    private static class WindowsVersion
    {
      internal static readonly bool IsWindows8OrAbove = Environment.WindowsVersion.GetIsWindows8OrAbove();

      private static bool GetIsWindows8OrAbove() => Interop.Kernel32.VerifyVersionInfoW(ref new Interop.Kernel32.OSVERSIONINFOEX()
      {
        dwOSVersionInfoSize = sizeof (Interop.Kernel32.OSVERSIONINFOEX),
        dwMajorVersion = 6,
        dwMinorVersion = 2,
        wServicePackMajor = (ushort) 0,
        wServicePackMinor = (ushort) 0
      }, 51U, Interop.Kernel32.VerSetConditionMask(Interop.Kernel32.VerSetConditionMask(Interop.Kernel32.VerSetConditionMask(Interop.Kernel32.VerSetConditionMask(0UL, 2U, (byte) 3), 1U, (byte) 3), 32U, (byte) 3), 16U, (byte) 3));
    }
  }
}
