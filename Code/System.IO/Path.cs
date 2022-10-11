// Decompiled with JetBrains decompiler
// Type: System.IO.Path
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;


#nullable enable
namespace System.IO
{
  /// <summary>Performs operations on <see cref="T:System.String" /> instances that contain file or directory path information. These operations are performed in a cross-platform manner.</summary>
  public static class Path
  {
    /// <summary>Provides a platform-specific character used to separate directory levels in a path string that reflects a hierarchical file system organization.</summary>
    public static readonly char DirectorySeparatorChar = '\\';
    /// <summary>Provides a platform-specific alternate character used to separate directory levels in a path string that reflects a hierarchical file system organization.</summary>
    public static readonly char AltDirectorySeparatorChar = '/';
    /// <summary>Provides a platform-specific volume separator character.</summary>
    public static readonly char VolumeSeparatorChar = ':';
    /// <summary>A platform-specific separator character used to separate path strings in environment variables.</summary>
    public static readonly char PathSeparator = ';';
    /// <summary>Provides a platform-specific array of characters that cannot be specified in path string arguments passed to members of the <see cref="T:System.IO.Path" /> class.</summary>
    [Obsolete("Path.InvalidPathChars has been deprecated. Use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
    public static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();

    /// <summary>Changes the extension of a path string.</summary>
    /// <param name="path">The path information to modify.</param>
    /// <param name="extension">The new extension (with or without a leading period). Specify <see langword="null" /> to remove an existing extension from <paramref name="path" />.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <returns>The modified path information.
    /// 
    /// On Windows-based desktop platforms, if <paramref name="path" /> is <see langword="null" /> or an empty string (""), the path information is returned unmodified. If <paramref name="extension" /> is <see langword="null" />, the returned string contains the specified path with its extension removed. If <paramref name="path" /> has no extension, and <paramref name="extension" /> is not <see langword="null" />, the returned path string contains <paramref name="extension" /> appended to the end of <paramref name="path" />.</returns>
    [return: NotNullIfNotNull("path")]
    public static string? ChangeExtension(string? path, string? extension)
    {
      if (path == null)
        return (string) null;
      int length = path.Length;
      if (length == 0)
        return string.Empty;
      for (int index = path.Length - 1; index >= 0; --index)
      {
        char c = path[index];
        if (c == '.')
        {
          length = index;
          break;
        }
        if (PathInternal.IsDirectorySeparator(c))
          break;
      }
      if (extension == null)
        return path.Substring(0, length);
      ReadOnlySpan<char> readOnlySpan = path.AsSpan(0, length);
      return !extension.StartsWith('.') ? readOnlySpan.ToString() + (ReadOnlySpan<char>) "." + (ReadOnlySpan<char>) extension : readOnlySpan.ToString() + (ReadOnlySpan<char>) extension;
    }

    /// <summary>Returns the directory information for the specified path.</summary>
    /// <param name="path">The path of a file or directory.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: The <paramref name="path" /> parameter contains invalid characters, is empty, or contains only white spaces.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="path" /> parameter is longer than the system-defined maximum length.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.IO.IOException" />, instead.</exception>
    /// <returns>Directory information for <paramref name="path" />, or <see langword="null" /> if <paramref name="path" /> denotes a root directory or is null. Returns <see cref="F:System.String.Empty" /> if <paramref name="path" /> does not contain directory information.</returns>
    public static string? GetDirectoryName(string? path)
    {
      if (path == null || PathInternal.IsEffectivelyEmpty(path.AsSpan()))
        return (string) null;
      int directoryNameOffset = Path.GetDirectoryNameOffset(path.AsSpan());
      return directoryNameOffset < 0 ? (string) null : PathInternal.NormalizeDirectorySeparators(path.Substring(0, directoryNameOffset));
    }

    /// <summary>Returns the directory information for the specified path represented by a character span.</summary>
    /// <param name="path">The path to retrieve the directory information from.</param>
    /// <returns>Directory information for <paramref name="path" />, or an empty span if <paramref name="path" /> is <see langword="null" />, an empty span, or a root (such as \, C:, or \\server\share).</returns>
    public static ReadOnlySpan<char> GetDirectoryName(ReadOnlySpan<char> path)
    {
      if (PathInternal.IsEffectivelyEmpty(path))
        return ReadOnlySpan<char>.Empty;
      int directoryNameOffset = Path.GetDirectoryNameOffset(path);
      return directoryNameOffset < 0 ? ReadOnlySpan<char>.Empty : path.Slice(0, directoryNameOffset);
    }


    #nullable disable
    internal static int GetDirectoryNameOffset(ReadOnlySpan<char> path)
    {
      int rootLength = PathInternal.GetRootLength(path);
      int length = path.Length;
      if (length <= rootLength)
        return -1;
      do
        ;
      while (length > rootLength && !PathInternal.IsDirectorySeparator(path[--length]));
      while (length > rootLength && PathInternal.IsDirectorySeparator(path[length - 1]))
        --length;
      return length;
    }


    #nullable enable
    /// <summary>Returns the extension (including the period ".") of the specified path string.</summary>
    /// <param name="path">The path string from which to get the extension.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <returns>The extension of the specified path (including the period "."), or <see langword="null" />, or <see cref="F:System.String.Empty" />. If <paramref name="path" /> is <see langword="null" />, <see cref="M:System.IO.Path.GetExtension(System.String)" /> returns <see langword="null" />. If <paramref name="path" /> does not have extension information, <see cref="M:System.IO.Path.GetExtension(System.String)" /> returns <see cref="F:System.String.Empty" />.</returns>
    [return: NotNullIfNotNull("path")]
    public static string? GetExtension(string? path) => path == null ? (string) null : Path.GetExtension(path.AsSpan()).ToString();

    /// <summary>Returns the extension of a file path that is represented by a read-only character span.</summary>
    /// <param name="path">The file path from which to get the extension.</param>
    /// <returns>The extension of the specified path (including the period, "."), or <see cref="P:System.ReadOnlySpan`1.Empty" /> if <paramref name="path" /> does not have extension information.</returns>
    public static ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> path)
    {
      int length = path.Length;
      for (int index = length - 1; index >= 0; --index)
      {
        char c = path[index];
        if (c == '.')
          return index != length - 1 ? path.Slice(index, length - index) : ReadOnlySpan<char>.Empty;
        if (PathInternal.IsDirectorySeparator(c))
          break;
      }
      return ReadOnlySpan<char>.Empty;
    }

    /// <summary>Returns the file name and extension of the specified path string.</summary>
    /// <param name="path">The path string from which to obtain the file name and extension.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <returns>The characters after the last directory separator character in <paramref name="path" />. If the last character of <paramref name="path" /> is a directory or volume separator character, this method returns <see cref="F:System.String.Empty" />. If <paramref name="path" /> is <see langword="null" />, this method returns <see langword="null" />.</returns>
    [return: NotNullIfNotNull("path")]
    public static string? GetFileName(string? path)
    {
      if (path == null)
        return (string) null;
      ReadOnlySpan<char> fileName = Path.GetFileName(path.AsSpan());
      return path.Length == fileName.Length ? path : fileName.ToString();
    }

    /// <summary>Returns the file name and extension of a file path that is represented by a read-only character span.</summary>
    /// <param name="path">A read-only span that contains the path from which to obtain the file name and extension.</param>
    /// <returns>The characters after the last directory separator character in <paramref name="path" />.</returns>
    public static ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path)
    {
      int length1 = Path.GetPathRoot(path).Length;
      int length2 = path.Length;
      while (--length2 >= 0)
      {
        if (length2 < length1 || PathInternal.IsDirectorySeparator(path[length2]))
          return path.Slice(length2 + 1, path.Length - length2 - 1);
      }
      return path;
    }

    /// <summary>Returns the file name of the specified path string without the extension.</summary>
    /// <param name="path">The path of the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <returns>The string returned by <see cref="M:System.IO.Path.GetFileName(System.ReadOnlySpan{System.Char})" />, minus the last period (.) and all characters following it.</returns>
    [return: NotNullIfNotNull("path")]
    public static string? GetFileNameWithoutExtension(string? path)
    {
      if (path == null)
        return (string) null;
      ReadOnlySpan<char> withoutExtension = Path.GetFileNameWithoutExtension(path.AsSpan());
      return path.Length == withoutExtension.Length ? path : withoutExtension.ToString();
    }

    /// <summary>Returns the file name without the extension of a file path that is represented by a read-only character span.</summary>
    /// <param name="path">A read-only span that contains the path from which to obtain the file name without the extension.</param>
    /// <returns>The characters in the read-only span returned by <see cref="M:System.IO.Path.GetFileName(System.ReadOnlySpan{System.Char})" />, minus the last period (.) and all characters following it.</returns>
    public static ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path)
    {
      ReadOnlySpan<char> fileName = Path.GetFileName(path);
      int length = fileName.LastIndexOf<char>('.');
      return length != -1 ? fileName.Slice(0, length) : fileName;
    }

    /// <summary>Returns a random folder name or file name.</summary>
    /// <returns>A random folder name or file name.</returns>
    public static unsafe string GetRandomFileName()
    {
      byte* numPtr = stackalloc byte[8];
      Interop.GetRandomBytes(numPtr, 8);
      return string.Create<IntPtr>(12, (IntPtr) (void*) numPtr, (SpanAction<char, IntPtr>) ((span, key) => Path.Populate83FileNameFromRandomBytes((byte*) (void*) key, 8, span)));
    }

    /// <summary>Returns a value that indicates whether the specified file path is fixed to a specific drive or UNC path.</summary>
    /// <param name="path">A file path.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the path is fixed to a specific drive or UNC path; <see langword="false" /> if the path is relative to the current drive or working directory.</returns>
    public static bool IsPathFullyQualified(string path) => path != null ? Path.IsPathFullyQualified(path.AsSpan()) : throw new ArgumentNullException(nameof (path));

    /// <summary>Returns a value that indicates whether the file path represented by the specified character span is fixed to a specific drive or UNC path.</summary>
    /// <param name="path">A file path.</param>
    /// <returns>
    /// <see langword="true" /> if the path is fixed to a specific drive or UNC path; <see langword="false" /> if the path is relative to the current drive or working directory.</returns>
    public static bool IsPathFullyQualified(ReadOnlySpan<char> path) => !PathInternal.IsPartiallyQualified(path);

    /// <summary>Determines whether a path includes a file name extension.</summary>
    /// <param name="path">The path to search for an extension.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the characters that follow the last directory separator (\ or /) or volume separator (:) in the path include a period (.) followed by one or more characters; otherwise, <see langword="false" />.</returns>
    public static bool HasExtension([NotNullWhen(true)] string? path) => path != null && Path.HasExtension(path.AsSpan());

    /// <summary>Determines whether the path represented by the specified character span includes a file name extension.</summary>
    /// <param name="path">The path to search for an extension.</param>
    /// <returns>
    /// <see langword="true" /> if the characters that follow the last directory separator character or volume separator in the path include a period (".") followed by one or more characters; otherwise, <see langword="false" />.</returns>
    public static bool HasExtension(ReadOnlySpan<char> path)
    {
      for (int index = path.Length - 1; index >= 0; --index)
      {
        char c = path[index];
        if (c == '.')
          return index != path.Length - 1;
        if (PathInternal.IsDirectorySeparator(c))
          break;
      }
      return false;
    }

    /// <summary>Combines two strings into a path.</summary>
    /// <param name="path1">The first path to combine.</param>
    /// <param name="path2">The second path to combine.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path1" /> or <paramref name="path2" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path1" /> or <paramref name="path2" /> is <see langword="null" />.</exception>
    /// <returns>The combined paths. If one of the specified paths is a zero-length string, this method returns the other path. If <paramref name="path2" /> contains an absolute path, this method returns <paramref name="path2" />.</returns>
    public static string Combine(string path1, string path2) => path1 != null && path2 != null ? Path.CombineInternal(path1, path2) : throw new ArgumentNullException(path1 == null ? nameof (path1) : nameof (path2));

    /// <summary>Combines three strings into a path.</summary>
    /// <param name="path1">The first path to combine.</param>
    /// <param name="path2">The second path to combine.</param>
    /// <param name="path3">The third path to combine.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path1" />, <paramref name="path2" />, or <paramref name="path3" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path1" />, <paramref name="path2" />, or <paramref name="path3" /> is <see langword="null" />.</exception>
    /// <returns>The combined paths.</returns>
    public static string Combine(string path1, string path2, string path3)
    {
      if (path1 == null || path2 == null || path3 == null)
        throw new ArgumentNullException(path1 == null ? nameof (path1) : (path2 == null ? nameof (path2) : nameof (path3)));
      return Path.CombineInternal(path1, path2, path3);
    }

    /// <summary>Combines four strings into a path.</summary>
    /// <param name="path1">The first path to combine.</param>
    /// <param name="path2">The second path to combine.</param>
    /// <param name="path3">The third path to combine.</param>
    /// <param name="path4">The fourth path to combine.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path1" />, <paramref name="path2" />, <paramref name="path3" />, or <paramref name="path4" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path1" />, <paramref name="path2" />, <paramref name="path3" />, or <paramref name="path4" /> is <see langword="null" />.</exception>
    /// <returns>The combined paths.</returns>
    public static string Combine(string path1, string path2, string path3, string path4)
    {
      if (path1 == null || path2 == null || path3 == null || path4 == null)
        throw new ArgumentNullException(path1 == null ? nameof (path1) : (path2 == null ? nameof (path2) : (path3 == null ? nameof (path3) : nameof (path4))));
      return Path.CombineInternal(path1, path2, path3, path4);
    }

    /// <summary>Combines an array of strings into a path.</summary>
    /// <param name="paths">An array of parts of the path.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: One of the strings in the array contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">One of the strings in the array is <see langword="null" />.</exception>
    /// <returns>The combined paths.</returns>
    public static unsafe string Combine(params string[] paths)
    {
      if (paths == null)
        throw new ArgumentNullException(nameof (paths));
      int capacity = 0;
      int num = 0;
      for (int index = 0; index < paths.Length; ++index)
      {
        if (paths[index] == null)
          throw new ArgumentNullException(nameof (paths));
        if (paths[index].Length != 0)
        {
          if (Path.IsPathRooted(paths[index]))
          {
            num = index;
            capacity = paths[index].Length;
          }
          else
            capacity += paths[index].Length;
          if (!PathInternal.IsDirectorySeparator(paths[index][paths[index].Length - 1]))
            ++capacity;
        }
      }
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(520)), 260));
      valueStringBuilder.EnsureCapacity(capacity);
      for (int index = num; index < paths.Length; ++index)
      {
        if (paths[index].Length != 0)
        {
          if (valueStringBuilder.Length == 0)
          {
            valueStringBuilder.Append(paths[index]);
          }
          else
          {
            if (!PathInternal.IsDirectorySeparator(valueStringBuilder[valueStringBuilder.Length - 1]))
              valueStringBuilder.Append('\\');
            valueStringBuilder.Append(paths[index]);
          }
        }
      }
      return valueStringBuilder.ToString();
    }

    /// <summary>Concatenates two path components into a single path.</summary>
    /// <param name="path1">A character span that contains the first path to join.</param>
    /// <param name="path2">A character span that contains the second path to join.</param>
    /// <returns>The combined paths.</returns>
    public static string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2)
    {
      if (path1.Length == 0)
        return path2.ToString();
      return path2.Length == 0 ? path1.ToString() : Path.JoinInternal(path1, path2);
    }

    /// <summary>Concatenates three path components into a single path.</summary>
    /// <param name="path1">A character span that contains the first path to join.</param>
    /// <param name="path2">A character span that contains the second path to join.</param>
    /// <param name="path3">A character span that contains the third path to join.</param>
    /// <returns>The concatenated path.</returns>
    public static string Join(
      ReadOnlySpan<char> path1,
      ReadOnlySpan<char> path2,
      ReadOnlySpan<char> path3)
    {
      if (path1.Length == 0)
        return Path.Join(path2, path3);
      if (path2.Length == 0)
        return Path.Join(path1, path3);
      return path3.Length == 0 ? Path.Join(path1, path2) : Path.JoinInternal(path1, path2, path3);
    }

    /// <summary>Concatenates four path components into a single path.</summary>
    /// <param name="path1">A character span that contains the first path to join.</param>
    /// <param name="path2">A character span that contains the second path to join.</param>
    /// <param name="path3">A character span that contains the third path to join.</param>
    /// <param name="path4">A character span that contains the fourth path to join.</param>
    /// <returns>The concatenated path.</returns>
    public static string Join(
      ReadOnlySpan<char> path1,
      ReadOnlySpan<char> path2,
      ReadOnlySpan<char> path3,
      ReadOnlySpan<char> path4)
    {
      if (path1.Length == 0)
        return Path.Join(path2, path3, path4);
      if (path2.Length == 0)
        return Path.Join(path1, path3, path4);
      if (path3.Length == 0)
        return Path.Join(path1, path2, path4);
      return path4.Length == 0 ? Path.Join(path1, path2, path3) : Path.JoinInternal(path1, path2, path3, path4);
    }

    /// <summary>Concatenates two paths into a single path.</summary>
    /// <param name="path1">The first path to join.</param>
    /// <param name="path2">The second path to join.</param>
    /// <returns>The concatenated path.</returns>
    public static string Join(string? path1, string? path2) => Path.Join(path1.AsSpan(), path2.AsSpan());

    /// <summary>Concatenates three paths into a single path.</summary>
    /// <param name="path1">The first path to join.</param>
    /// <param name="path2">The second path to join.</param>
    /// <param name="path3">The third path to join.</param>
    /// <returns>The concatenated path.</returns>
    public static string Join(string? path1, string? path2, string? path3) => Path.Join(path1.AsSpan(), path2.AsSpan(), path3.AsSpan());

    /// <summary>Concatenates four paths into a single path.</summary>
    /// <param name="path1">The first path to join.</param>
    /// <param name="path2">The second path to join.</param>
    /// <param name="path3">The third path to join.</param>
    /// <param name="path4">The fourth path to join.</param>
    /// <returns>The concatenated path.</returns>
    public static string Join(string? path1, string? path2, string? path3, string? path4) => Path.Join(path1.AsSpan(), path2.AsSpan(), path3.AsSpan(), path4.AsSpan());

    /// <summary>Concatenates an array of paths into a single path.</summary>
    /// <param name="paths">An array of paths.</param>
    /// <returns>The concatenated path.</returns>
    public static unsafe string Join(params string?[] paths)
    {
      if (paths == null)
        throw new ArgumentNullException(nameof (paths));
      if (paths.Length == 0)
        return string.Empty;
      int num = 0;
      foreach (string path in paths)
        num += path != null ? path.Length : 0;
      int capacity = num + (paths.Length - 1);
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(520)), 260));
      valueStringBuilder.EnsureCapacity(capacity);
      for (int index = 0; index < paths.Length; ++index)
      {
        string path = paths[index];
        if (!string.IsNullOrEmpty(path))
        {
          if (valueStringBuilder.Length == 0)
          {
            valueStringBuilder.Append(path);
          }
          else
          {
            if (!PathInternal.IsDirectorySeparator(valueStringBuilder[valueStringBuilder.Length - 1]) && !PathInternal.IsDirectorySeparator(path[0]))
              valueStringBuilder.Append('\\');
            valueStringBuilder.Append(path);
          }
        }
      }
      return valueStringBuilder.ToString();
    }

    /// <summary>Attempts to concatenate two path components to a single preallocated character span, and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="path1">A character span that contains the first path to join.</param>
    /// <param name="path2">A character span that contains the second path to join.</param>
    /// <param name="destination">A character span to hold the concatenated path.</param>
    /// <param name="charsWritten">When the method returns, a value that indicates the number of characters written to the <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the concatenation operation is successful; otherwise, <see langword="false" />.</returns>
    public static bool TryJoin(
      ReadOnlySpan<char> path1,
      ReadOnlySpan<char> path2,
      Span<char> destination,
      out int charsWritten)
    {
      charsWritten = 0;
      if (path1.Length == 0 && path2.Length == 0)
        return true;
      if (path1.Length == 0 || path2.Length == 0)
      {
        ref ReadOnlySpan<char> local = ref (path1.Length == 0 ? ref path2 : ref path1);
        if (destination.Length < local.Length)
          return false;
        local.CopyTo(destination);
        charsWritten = local.Length;
        return true;
      }
      bool flag = !Path.EndsInDirectorySeparator(path1) && !PathInternal.StartsWithDirectorySeparator(path2);
      int num = path1.Length + path2.Length + (flag ? 1 : 0);
      if (destination.Length < num)
        return false;
      path1.CopyTo(destination);
      if (flag)
        destination[path1.Length] = Path.DirectorySeparatorChar;
      path2.CopyTo(destination.Slice(path1.Length + (flag ? 1 : 0)));
      charsWritten = num;
      return true;
    }

    /// <summary>Attempts to concatenate two path components to a single preallocated character span, and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="path1">A character span that contains the first path to join.</param>
    /// <param name="path2">A character span that contains the second path to join.</param>
    /// <param name="path3">A character span that contains the third path to join.</param>
    /// <param name="destination">A character span to hold the concatenated path.</param>
    /// <param name="charsWritten">When the method returns, a value that indicates the number of characters written to the <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the concatenation operation is successful; otherwise, <see langword="false" />.</returns>
    public static bool TryJoin(
      ReadOnlySpan<char> path1,
      ReadOnlySpan<char> path2,
      ReadOnlySpan<char> path3,
      Span<char> destination,
      out int charsWritten)
    {
      charsWritten = 0;
      if (path1.Length == 0 && path2.Length == 0 && path3.Length == 0)
        return true;
      if (path1.Length == 0)
        return Path.TryJoin(path2, path3, destination, out charsWritten);
      if (path2.Length == 0)
        return Path.TryJoin(path1, path3, destination, out charsWritten);
      if (path3.Length == 0)
        return Path.TryJoin(path1, path2, destination, out charsWritten);
      int num1 = Path.EndsInDirectorySeparator(path1) || PathInternal.StartsWithDirectorySeparator(path2) ? 0 : 1;
      bool flag = !Path.EndsInDirectorySeparator(path2) && !PathInternal.StartsWithDirectorySeparator(path3);
      if (flag)
        ++num1;
      int num2 = path1.Length + path2.Length + path3.Length + num1;
      if (destination.Length < num2)
        return false;
      Path.TryJoin(path1, path2, destination, out charsWritten);
      if (flag)
        destination[charsWritten++] = Path.DirectorySeparatorChar;
      path3.CopyTo(destination.Slice(charsWritten));
      charsWritten += path3.Length;
      return true;
    }


    #nullable disable
    private static string CombineInternal(string first, string second)
    {
      if (string.IsNullOrEmpty(first))
        return second;
      if (string.IsNullOrEmpty(second))
        return first;
      return Path.IsPathRooted(second.AsSpan()) ? second : Path.JoinInternal(first.AsSpan(), second.AsSpan());
    }

    private static string CombineInternal(string first, string second, string third)
    {
      if (string.IsNullOrEmpty(first))
        return Path.CombineInternal(second, third);
      if (string.IsNullOrEmpty(second))
        return Path.CombineInternal(first, third);
      if (string.IsNullOrEmpty(third))
        return Path.CombineInternal(first, second);
      if (Path.IsPathRooted(third.AsSpan()))
        return third;
      return Path.IsPathRooted(second.AsSpan()) ? Path.CombineInternal(second, third) : Path.JoinInternal(first.AsSpan(), second.AsSpan(), third.AsSpan());
    }

    private static string CombineInternal(
      string first,
      string second,
      string third,
      string fourth)
    {
      if (string.IsNullOrEmpty(first))
        return Path.CombineInternal(second, third, fourth);
      if (string.IsNullOrEmpty(second))
        return Path.CombineInternal(first, third, fourth);
      if (string.IsNullOrEmpty(third))
        return Path.CombineInternal(first, second, fourth);
      if (string.IsNullOrEmpty(fourth))
        return Path.CombineInternal(first, second, third);
      if (Path.IsPathRooted(fourth.AsSpan()))
        return fourth;
      if (Path.IsPathRooted(third.AsSpan()))
        return Path.CombineInternal(third, fourth);
      return Path.IsPathRooted(second.AsSpan()) ? Path.CombineInternal(second, third, fourth) : Path.JoinInternal(first.AsSpan(), second.AsSpan(), third.AsSpan(), fourth.AsSpan());
    }

    private static string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second) => !PathInternal.IsDirectorySeparator(first[first.Length - 1]) && !PathInternal.IsDirectorySeparator(second[0]) ? first.ToString() + (ReadOnlySpan<char>) "\\" + second : first.ToString() + second;

    private static unsafe string JoinInternal(
      ReadOnlySpan<char> first,
      ReadOnlySpan<char> second,
      ReadOnlySpan<char> third)
    {
      byte num1 = PathInternal.IsDirectorySeparator(first[first.Length - 1]) || PathInternal.IsDirectorySeparator(second[0]) ? (byte) 0 : (byte) 1;
      byte num2 = PathInternal.IsDirectorySeparator(second[second.Length - 1]) || PathInternal.IsDirectorySeparator(third[0]) ? (byte) 0 : (byte) 1;
      fixed (char* first1 = &MemoryMarshal.GetReference<char>(first))
        fixed (char* second1 = &MemoryMarshal.GetReference<char>(second))
          fixed (char* third1 = &MemoryMarshal.GetReference<char>(third))
          {
            Path.Join3Payload join3Payload = new Path.Join3Payload(first1, first.Length, second1, second.Length, third1, third.Length, (byte) ((uint) num1 | (uint) num2 << 1));
            return string.Create<IntPtr>(first.Length + second.Length + third.Length + (int) num1 + (int) num2, (IntPtr) (void*) &join3Payload, (SpanAction<char, IntPtr>) ((destination, statePtr) =>
            {
              ref Path.Join3Payload local = ref (*(Path.Join3Payload*) (void*) statePtr);
              new Span<char>((void*) local.First, local.FirstLength).CopyTo(destination);
              if (((int) local.Separators & 1) != 0)
                destination[local.FirstLength] = '\\';
              new Span<char>((void*) local.Second, local.SecondLength).CopyTo(destination.Slice(local.FirstLength + ((int) local.Separators & 1)));
              if (((int) local.Separators & 2) != 0)
                destination[destination.Length - local.ThirdLength - 1] = '\\';
              new Span<char>((void*) local.Third, local.ThirdLength).CopyTo(destination.Slice(destination.Length - local.ThirdLength));
            }));
          }
    }

    private static unsafe string JoinInternal(
      ReadOnlySpan<char> first,
      ReadOnlySpan<char> second,
      ReadOnlySpan<char> third,
      ReadOnlySpan<char> fourth)
    {
      byte num1 = PathInternal.IsDirectorySeparator(first[first.Length - 1]) || PathInternal.IsDirectorySeparator(second[0]) ? (byte) 0 : (byte) 1;
      byte num2 = PathInternal.IsDirectorySeparator(second[second.Length - 1]) || PathInternal.IsDirectorySeparator(third[0]) ? (byte) 0 : (byte) 1;
      byte num3 = PathInternal.IsDirectorySeparator(third[third.Length - 1]) || PathInternal.IsDirectorySeparator(fourth[0]) ? (byte) 0 : (byte) 1;
      fixed (char* first1 = &MemoryMarshal.GetReference<char>(first))
        fixed (char* second1 = &MemoryMarshal.GetReference<char>(second))
          fixed (char* third1 = &MemoryMarshal.GetReference<char>(third))
            fixed (char* fourth1 = &MemoryMarshal.GetReference<char>(fourth))
            {
              Path.Join4Payload join4Payload = new Path.Join4Payload(first1, first.Length, second1, second.Length, third1, third.Length, fourth1, fourth.Length, (byte) ((int) num1 | (int) num2 << 1 | (int) num3 << 2));
              return string.Create<IntPtr>(first.Length + second.Length + third.Length + fourth.Length + (int) num1 + (int) num2 + (int) num3, (IntPtr) (void*) &join4Payload, (SpanAction<char, IntPtr>) ((destination, statePtr) =>
              {
                ref Path.Join4Payload local = ref (*(Path.Join4Payload*) (void*) statePtr);
                new Span<char>((void*) local.First, local.FirstLength).CopyTo(destination);
                int firstLength = local.FirstLength;
                if (((int) local.Separators & 1) != 0)
                  destination[firstLength++] = '\\';
                new Span<char>((void*) local.Second, local.SecondLength).CopyTo(destination.Slice(firstLength));
                int start1 = firstLength + local.SecondLength;
                if (((int) local.Separators & 2) != 0)
                  destination[start1++] = '\\';
                new Span<char>((void*) local.Third, local.ThirdLength).CopyTo(destination.Slice(start1));
                int start2 = start1 + local.ThirdLength;
                if (((int) local.Separators & 4) != 0)
                  destination[start2++] = '\\';
                new Span<char>((void*) local.Fourth, local.FourthLength).CopyTo(destination.Slice(start2));
              }));
            }
    }


    #nullable enable
    private static unsafe ReadOnlySpan<byte> Base32Char => new ReadOnlySpan<byte>((void*) &\u003CPrivateImplementationDetails\u003E.\u003653BB1245E828FCDA4FA53FCD5A3DEF5BD7654E651F54B4132B73D74E64435C4, 32);


    #nullable disable
    private static unsafe void Populate83FileNameFromRandomBytes(
      byte* bytes,
      int byteCount,
      Span<char> chars)
    {
      byte num1 = *bytes;
      byte num2 = bytes[1];
      byte num3 = bytes[2];
      byte num4 = bytes[3];
      byte num5 = bytes[4];
      chars[11] = (char) Path.Base32Char[(int) bytes[7] & 31];
      chars[0] = (char) Path.Base32Char[(int) num1 & 31];
      chars[1] = (char) Path.Base32Char[(int) num2 & 31];
      chars[2] = (char) Path.Base32Char[(int) num3 & 31];
      chars[3] = (char) Path.Base32Char[(int) num4 & 31];
      chars[4] = (char) Path.Base32Char[(int) num5 & 31];
      chars[5] = (char) Path.Base32Char[((int) num1 & 224) >> 5 | ((int) num4 & 96) >> 2];
      chars[6] = (char) Path.Base32Char[((int) num2 & 224) >> 5 | ((int) num5 & 96) >> 2];
      byte index = (byte) ((uint) num3 >> 5);
      if (((int) num4 & 128) != 0)
        index |= (byte) 8;
      if (((int) num5 & 128) != 0)
        index |= (byte) 16;
      chars[7] = (char) Path.Base32Char[(int) index];
      chars[8] = '.';
      chars[9] = (char) Path.Base32Char[(int) bytes[5] & 31];
      chars[10] = (char) Path.Base32Char[(int) bytes[6] & 31];
    }


    #nullable enable
    /// <summary>Returns a relative path from one path to another.</summary>
    /// <param name="relativeTo">The source path the result should be relative to. This path is always considered to be a directory.</param>
    /// <param name="path">The destination path.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="relativeTo" /> or <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="relativeTo" /> or <paramref name="path" /> is effectively empty.</exception>
    /// <returns>The relative path, or <paramref name="path" /> if the paths don't share the same root.</returns>
    public static string GetRelativePath(string relativeTo, string path) => Path.GetRelativePath(relativeTo, path, PathInternal.StringComparison);


    #nullable disable
    private static unsafe string GetRelativePath(
      string relativeTo,
      string path,
      StringComparison comparisonType)
    {
      if (relativeTo == null)
        throw new ArgumentNullException(nameof (relativeTo));
      if (PathInternal.IsEffectivelyEmpty(relativeTo.AsSpan()))
        throw new ArgumentException(SR.Arg_PathEmpty, nameof (relativeTo));
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (PathInternal.IsEffectivelyEmpty(path.AsSpan()))
        throw new ArgumentException(SR.Arg_PathEmpty, nameof (path));
      relativeTo = Path.GetFullPath(relativeTo);
      path = Path.GetFullPath(path);
      if (!PathInternal.AreRootsEqual(relativeTo, path, comparisonType))
        return path;
      int commonPathLength = PathInternal.GetCommonPathLength(relativeTo, path, comparisonType == StringComparison.OrdinalIgnoreCase);
      if (commonPathLength == 0)
        return path;
      int length1 = relativeTo.Length;
      if (Path.EndsInDirectorySeparator(relativeTo.AsSpan()))
        --length1;
      bool flag = Path.EndsInDirectorySeparator(path.AsSpan());
      int length2 = path.Length;
      if (flag)
        --length2;
      if (length1 == length2 && commonPathLength >= length1)
        return ".";
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(520)), 260));
      valueStringBuilder.EnsureCapacity(Math.Max(relativeTo.Length, path.Length));
      if (commonPathLength < length1)
      {
        valueStringBuilder.Append("..");
        for (int index = commonPathLength + 1; index < length1; ++index)
        {
          if (PathInternal.IsDirectorySeparator(relativeTo[index]))
          {
            valueStringBuilder.Append(Path.DirectorySeparatorChar);
            valueStringBuilder.Append("..");
          }
        }
      }
      else if (PathInternal.IsDirectorySeparator(path[commonPathLength]))
        ++commonPathLength;
      int length3 = length2 - commonPathLength;
      if (flag)
        ++length3;
      if (length3 > 0)
      {
        if (valueStringBuilder.Length > 0)
          valueStringBuilder.Append(Path.DirectorySeparatorChar);
        valueStringBuilder.Append(path.AsSpan(commonPathLength, length3));
      }
      return valueStringBuilder.ToString();
    }


    #nullable enable
    /// <summary>Trims one trailing directory separator beyond the root of the specified path.</summary>
    /// <param name="path">The path to trim.</param>
    /// <returns>The <paramref name="path" /> without any trailing directory separators.</returns>
    public static string TrimEndingDirectorySeparator(string path) => PathInternal.TrimEndingDirectorySeparator(path);

    /// <summary>Trims one trailing directory separator beyond the root of the specified path.</summary>
    /// <param name="path">The path to trim.</param>
    /// <returns>The <paramref name="path" /> without any trailing directory separators.</returns>
    public static ReadOnlySpan<char> TrimEndingDirectorySeparator(
      ReadOnlySpan<char> path)
    {
      return PathInternal.TrimEndingDirectorySeparator(path);
    }

    /// <summary>Returns a value that indicates whether the path, specified as a read-only span, ends in a directory separator.</summary>
    /// <param name="path">The path to analyze.</param>
    /// <returns>
    /// <see langword="true" /> if the path ends in a directory separator; otherwise, <see langword="false" />.</returns>
    public static bool EndsInDirectorySeparator(ReadOnlySpan<char> path) => PathInternal.EndsInDirectorySeparator(path);

    /// <summary>Returns a value that indicates whether the specified path ends in a directory separator.</summary>
    /// <param name="path">The path to analyze.</param>
    /// <returns>
    /// <see langword="true" /> if the path ends in a directory separator; otherwise, <see langword="false" />.</returns>
    public static bool EndsInDirectorySeparator(string path) => PathInternal.EndsInDirectorySeparator(path);

    /// <summary>Gets an array containing the characters that are not allowed in file names.</summary>
    /// <returns>An array containing the characters that are not allowed in file names.</returns>
    public static char[] GetInvalidFileNameChars() => new char[41]
    {
      '"',
      '<',
      '>',
      '|',
      char.MinValue,
      '\u0001',
      '\u0002',
      '\u0003',
      '\u0004',
      '\u0005',
      '\u0006',
      '\a',
      '\b',
      '\t',
      '\n',
      '\v',
      '\f',
      '\r',
      '\u000E',
      '\u000F',
      '\u0010',
      '\u0011',
      '\u0012',
      '\u0013',
      '\u0014',
      '\u0015',
      '\u0016',
      '\u0017',
      '\u0018',
      '\u0019',
      '\u001A',
      '\u001B',
      '\u001C',
      '\u001D',
      '\u001E',
      '\u001F',
      ':',
      '*',
      '?',
      '\\',
      '/'
    };

    /// <summary>Gets an array containing the characters that are not allowed in path names.</summary>
    /// <returns>An array containing the characters that are not allowed in path names.</returns>
    public static char[] GetInvalidPathChars() => new char[33]
    {
      '|',
      char.MinValue,
      '\u0001',
      '\u0002',
      '\u0003',
      '\u0004',
      '\u0005',
      '\u0006',
      '\a',
      '\b',
      '\t',
      '\n',
      '\v',
      '\f',
      '\r',
      '\u000E',
      '\u000F',
      '\u0010',
      '\u0011',
      '\u0012',
      '\u0013',
      '\u0014',
      '\u0015',
      '\u0016',
      '\u0017',
      '\u0018',
      '\u0019',
      '\u001A',
      '\u001B',
      '\u001C',
      '\u001D',
      '\u001E',
      '\u001F'
    };

    /// <summary>Returns the absolute path for the specified path string.</summary>
    /// <param name="path">The file or directory for which to obtain absolute path information.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="path" /> is a zero-length string, contains only white space, or contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.
    /// 
    /// -or-
    /// 
    ///  The system could not retrieve the absolute path.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> contains a colon (":") that is not part of a volume identifier (for example, "c:\").</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <returns>The fully qualified location of <paramref name="path" />, such as "C:\MyFile.txt".</returns>
    public static string GetFullPath(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (PathInternal.IsEffectivelyEmpty(path.AsSpan()))
        throw new ArgumentException(SR.Arg_PathEmpty, nameof (path));
      return !path.Contains(char.MinValue) ? Path.GetFullPathInternal(path) : throw new ArgumentException(SR.Argument_InvalidPathChars, nameof (path));
    }

    /// <summary>Returns an absolute path from a relative path and a fully qualified base path.</summary>
    /// <param name="path">A relative path to concatenate to <paramref name="basePath" />.</param>
    /// <param name="basePath">The beginning of a fully qualified path.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="basePath" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="basePath" /> is not a fully qualified path.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> or <paramref name="basePath" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <returns>The absolute path.</returns>
    public static string GetFullPath(string path, string basePath)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (basePath == null)
        throw new ArgumentNullException(nameof (basePath));
      if (!Path.IsPathFullyQualified(basePath))
        throw new ArgumentException(SR.Arg_BasePathNotFullyQualified, nameof (basePath));
      if (basePath.Contains(char.MinValue) || path.Contains(char.MinValue))
        throw new ArgumentException(SR.Argument_InvalidPathChars);
      if (Path.IsPathFullyQualified(path))
        return Path.GetFullPathInternal(path);
      if (PathInternal.IsEffectivelyEmpty(path.AsSpan()))
        return basePath;
      int length = path.Length;
      string str = length < 1 || !PathInternal.IsDirectorySeparator(path[0]) ? (length < 2 || !PathInternal.IsValidDriveChar(path[0]) || path[1] != ':' ? Path.JoinInternal(basePath.AsSpan(), path.AsSpan()) : (!Path.GetVolumeName(path.AsSpan()).EqualsOrdinal(Path.GetVolumeName(basePath.AsSpan())) ? (!PathInternal.IsDevice(basePath.AsSpan()) ? path.Insert(2, "\\") : (length == 2 ? Path.JoinInternal(basePath.AsSpan(0, 4), path.AsSpan(), "\\".AsSpan()) : Path.JoinInternal(basePath.AsSpan(0, 4), path.AsSpan(0, 2), "\\".AsSpan(), path.AsSpan(2)))) : Path.Join(basePath.AsSpan(), path.AsSpan(2)))) : Path.Join(Path.GetPathRoot(basePath.AsSpan()), path.AsSpan(1));
      return !PathInternal.IsDevice(str.AsSpan()) ? Path.GetFullPathInternal(str) : PathInternal.RemoveRelativeSegments(str, PathInternal.GetRootLength(str.AsSpan()));
    }


    #nullable disable
    private static string GetFullPathInternal(string path) => PathInternal.IsExtended(path.AsSpan()) ? path : PathHelper.Normalize(path);


    #nullable enable
    /// <summary>Returns the path of the current user's temporary folder.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions.</exception>
    /// <returns>The path to the temporary folder, ending with a  <see cref="F:System.IO.Path.DirectorySeparatorChar" />.</returns>
    public static unsafe string GetTempPath()
    {
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(520)), 260));
      Path.GetTempPath(ref valueStringBuilder);
      string tempPath = PathHelper.Normalize(ref valueStringBuilder);
      valueStringBuilder.Dispose();
      return tempPath;
    }


    #nullable disable
    private static void GetTempPath(ref ValueStringBuilder builder)
    {
      uint tempPathW;
      while ((long) (tempPathW = Interop.Kernel32.GetTempPathW(builder.Capacity, ref builder.GetPinnableReference())) > (long) builder.Capacity)
        builder.EnsureCapacity(checked ((int) tempPathW));
      builder.Length = tempPathW != 0U ? (int) tempPathW : throw Win32Marshal.GetExceptionForLastWin32Error();
    }


    #nullable enable
    /// <summary>Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs, such as no unique temporary file name is available.
    /// 
    /// -or-
    /// 
    ///  This method was unable to create a temporary file.</exception>
    /// <returns>The full path of the temporary file.</returns>
    public static unsafe string GetTempFileName()
    {
      // ISSUE: untyped stack allocation
      ValueStringBuilder builder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(520)), 260));
      Path.GetTempPath(ref builder);
      // ISSUE: untyped stack allocation
      ValueStringBuilder path = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(520)), 260));
      uint tempFileNameW = Interop.Kernel32.GetTempFileNameW(ref builder.GetPinnableReference(), "tmp", 0U, ref path.GetPinnableReference());
      builder.Dispose();
      if (tempFileNameW == 0U)
        throw Win32Marshal.GetExceptionForLastWin32Error();
      path.Length = path.RawChars.IndexOf<char>(char.MinValue);
      string tempFileName = PathHelper.Normalize(ref path);
      path.Dispose();
      return tempFileName;
    }

    /// <summary>Returns a value indicating whether the specified path string contains a root.</summary>
    /// <param name="path">The path to test.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="path" /> contains a root; otherwise, <see langword="false" />.</returns>
    public static bool IsPathRooted([NotNullWhen(true)] string? path) => path != null && Path.IsPathRooted(path.AsSpan());

    /// <summary>Returns a value that indicates whether the specified character span that represents a file path contains a root.</summary>
    /// <param name="path">The path to test.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="path" /> contains a root; otherwise, <see langword="false" />.</returns>
    public static bool IsPathRooted(ReadOnlySpan<char> path)
    {
      int length = path.Length;
      if (length >= 1 && PathInternal.IsDirectorySeparator(path[0]))
        return true;
      return length >= 2 && PathInternal.IsValidDriveChar(path[0]) && path[1] == ':';
    }

    /// <summary>Gets the root directory information from the path contained in the specified string.</summary>
    /// <param name="path">A string containing the path from which to obtain root directory information.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> contains one or more of the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars" />.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: <see cref="F:System.String.Empty" /> was passed to <paramref name="path" />.</exception>
    /// <returns>The root directory of <paramref name="path" /> if it is rooted.
    /// 
    ///  -or-
    /// 
    /// <see cref="F:System.String.Empty" /> if <paramref name="path" /> does not contain root directory information.
    /// 
    ///  -or-
    /// 
    /// <see langword="null" /> if <paramref name="path" /> is <see langword="null" /> or is effectively empty.</returns>
    public static string? GetPathRoot(string? path)
    {
      if (PathInternal.IsEffectivelyEmpty(path.AsSpan()))
        return (string) null;
      ReadOnlySpan<char> pathRoot = Path.GetPathRoot(path.AsSpan());
      return path.Length == pathRoot.Length ? PathInternal.NormalizeDirectorySeparators(path) : PathInternal.NormalizeDirectorySeparators(pathRoot.ToString());
    }

    /// <summary>Gets the root directory information from the path contained in the specified character span.</summary>
    /// <param name="path">A read-only span of characters containing the path from which to obtain root directory information.</param>
    /// <returns>A read-only span of characters containing the root directory of <paramref name="path" />.</returns>
    public static ReadOnlySpan<char> GetPathRoot(ReadOnlySpan<char> path)
    {
      if (PathInternal.IsEffectivelyEmpty(path))
        return ReadOnlySpan<char>.Empty;
      int rootLength = PathInternal.GetRootLength(path);
      return rootLength > 0 ? path.Slice(0, rootLength) : ReadOnlySpan<char>.Empty;
    }


    #nullable disable
    internal static ReadOnlySpan<char> GetVolumeName(ReadOnlySpan<char> path)
    {
      ReadOnlySpan<char> pathRoot = Path.GetPathRoot(path);
      if (pathRoot.Length == 0)
        return pathRoot;
      int start = Path.GetUncRootLength(path);
      if (start == -1)
        start = !PathInternal.IsDevice(path) ? 0 : 4;
      ReadOnlySpan<char> path1 = pathRoot.Slice(start);
      return !Path.EndsInDirectorySeparator(path1) ? path1 : path1.Slice(0, path1.Length - 1);
    }

    internal static int GetUncRootLength(ReadOnlySpan<char> path)
    {
      bool flag = PathInternal.IsDevice(path);
      if (!flag && path.Slice(0, 2).EqualsOrdinal("\\\\".AsSpan()))
        return 2;
      return flag && path.Length >= 8 && (path.Slice(0, 8).EqualsOrdinal("\\\\?\\UNC\\".AsSpan()) || path.Slice(5, 4).EqualsOrdinal("UNC\\".AsSpan())) ? 8 : -1;
    }

    private readonly struct Join3Payload
    {
      public readonly unsafe char* First;
      public readonly int FirstLength;
      public readonly unsafe char* Second;
      public readonly int SecondLength;
      public readonly unsafe char* Third;
      public readonly int ThirdLength;
      public readonly byte Separators;

      public unsafe Join3Payload(
        char* first,
        int firstLength,
        char* second,
        int secondLength,
        char* third,
        int thirdLength,
        byte separators)
      {
        this.First = first;
        this.FirstLength = firstLength;
        this.Second = second;
        this.SecondLength = secondLength;
        this.Third = third;
        this.ThirdLength = thirdLength;
        this.Separators = separators;
      }
    }

    private readonly struct Join4Payload
    {
      public readonly unsafe char* First;
      public readonly int FirstLength;
      public readonly unsafe char* Second;
      public readonly int SecondLength;
      public readonly unsafe char* Third;
      public readonly int ThirdLength;
      public readonly unsafe char* Fourth;
      public readonly int FourthLength;
      public readonly byte Separators;

      public unsafe Join4Payload(
        char* first,
        int firstLength,
        char* second,
        int secondLength,
        char* third,
        int thirdLength,
        char* fourth,
        int fourthLength,
        byte separators)
      {
        this.First = first;
        this.FirstLength = firstLength;
        this.Second = second;
        this.SecondLength = secondLength;
        this.Third = third;
        this.ThirdLength = thirdLength;
        this.Fourth = fourth;
        this.FourthLength = fourthLength;
        this.Separators = separators;
      }
    }
  }
}
