// Decompiled with JetBrains decompiler
// Type: System.IO.UnmanagedMemoryAccessor
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.InteropServices.xml

using Internal.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System.IO
{
  /// <summary>Provides random access to unmanaged blocks of memory from managed code.</summary>
  public class UnmanagedMemoryAccessor : IDisposable
  {

    #nullable disable
    private SafeBuffer _buffer;
    private long _offset;
    private long _capacity;
    private FileAccess _access;
    private bool _isOpen;
    private bool _canRead;
    private bool _canWrite;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryAccessor" /> class.</summary>
    protected UnmanagedMemoryAccessor()
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryAccessor" /> class with a specified buffer, offset, and capacity.</summary>
    /// <param name="buffer">The buffer to contain the accessor.</param>
    /// <param name="offset">The byte at which to start the accessor.</param>
    /// <param name="capacity">The size, in bytes, of memory to allocate.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="offset" /> plus <paramref name="capacity" /> is greater than <paramref name="buffer" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="capacity" /> is less than zero.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="offset" /> plus <paramref name="capacity" /> would wrap around the high end of the address space.</exception>
    public UnmanagedMemoryAccessor(SafeBuffer buffer, long offset, long capacity) => this.Initialize(buffer, offset, capacity, FileAccess.Read);

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryAccessor" /> class with a specified buffer, offset, capacity, and access right.</summary>
    /// <param name="buffer">The buffer to contain the accessor.</param>
    /// <param name="offset">The byte at which to start the accessor.</param>
    /// <param name="capacity">The size, in bytes, of memory to allocate.</param>
    /// <param name="access">The type of access allowed to the memory. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="offset" /> plus <paramref name="capacity" /> is greater than <paramref name="buffer" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="capacity" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="offset" /> plus <paramref name="capacity" /> would wrap around the high end of the address space.</exception>
    public UnmanagedMemoryAccessor(
      SafeBuffer buffer,
      long offset,
      long capacity,
      FileAccess access)
    {
      this.Initialize(buffer, offset, capacity, access);
    }

    /// <summary>Sets the initial values for the accessor.</summary>
    /// <param name="buffer">The buffer to contain the accessor.</param>
    /// <param name="offset">The byte at which to start the accessor.</param>
    /// <param name="capacity">The size, in bytes, of memory to allocate.</param>
    /// <param name="access">The type of access allowed to the memory. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="offset" /> plus <paramref name="capacity" /> is greater than <paramref name="buffer" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="capacity" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="offset" /> plus <paramref name="capacity" /> would wrap around the high end of the address space.</exception>
    protected unsafe void Initialize(
      SafeBuffer buffer,
      long offset,
      long capacity,
      FileAccess access)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0L)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (capacity < 0L)
        throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.ByteLength < (ulong) (offset + capacity))
        throw new ArgumentException(SR.Argument_OffsetAndCapacityOutOfBounds);
      if (access < FileAccess.Read || access > FileAccess.ReadWrite)
        throw new ArgumentOutOfRangeException(nameof (access));
      if (this._isOpen)
        throw new InvalidOperationException(SR.InvalidOperation_CalledTwice);
      byte* pointer = (byte*) null;
      try
      {
        buffer.AcquirePointer(ref pointer);
        if ((UIntPtr) (ulong) ((long) pointer + offset + capacity) < (UIntPtr) pointer)
          throw new ArgumentException(SR.Argument_UnmanagedMemAccessorWrapAround);
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          buffer.ReleasePointer();
      }
      this._offset = offset;
      this._buffer = buffer;
      this._capacity = capacity;
      this._access = access;
      this._isOpen = true;
      this._canRead = (this._access & FileAccess.Read) != 0;
      this._canWrite = (this._access & FileAccess.Write) != 0;
    }

    /// <summary>Gets the capacity of the accessor.</summary>
    /// <returns>The capacity of the accessor.</returns>
    public long Capacity => this._capacity;

    /// <summary>Determines whether the accessor is readable.</summary>
    /// <returns>
    /// <see langword="true" /> if the accessor is readable; otherwise, <see langword="false" />.</returns>
    public bool CanRead => this._isOpen && this._canRead;

    /// <summary>Determines whether the accessory is writable.</summary>
    /// <returns>
    /// <see langword="true" /> if the accessor is writable; otherwise, <see langword="false" />.</returns>
    public bool CanWrite => this._isOpen && this._canWrite;

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.UnmanagedMemoryAccessor" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing) => this._isOpen = false;

    /// <summary>Releases all resources used by the <see cref="T:System.IO.UnmanagedMemoryAccessor" />.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Determines whether the accessor is currently open by a process.</summary>
    /// <returns>
    /// <see langword="true" /> if the accessor is open; otherwise, <see langword="false" />.</returns>
    protected bool IsOpen => this._isOpen;

    /// <summary>Reads a Boolean value from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> or <see langword="false" />.</returns>
    public bool ReadBoolean(long position) => this.ReadByte(position) > (byte) 0;

    /// <summary>Reads a byte value from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    public unsafe byte ReadByte(long position)
    {
      this.EnsureSafeToRead(position, 1);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        return (pointer + this._offset)[position];
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Reads a character from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    public char ReadChar(long position) => (char) this.ReadInt16(position);

    /// <summary>Reads a 16-bit integer from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    public unsafe short ReadInt16(long position)
    {
      this.EnsureSafeToRead(position, 2);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        return Unsafe.ReadUnaligned<short>((void*) (pointer + this._offset + position));
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Reads a 32-bit integer from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    public unsafe int ReadInt32(long position)
    {
      this.EnsureSafeToRead(position, 4);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        return Unsafe.ReadUnaligned<int>((void*) (pointer + this._offset + position));
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Reads a 64-bit integer from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    public unsafe long ReadInt64(long position)
    {
      this.EnsureSafeToRead(position, 8);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        return Unsafe.ReadUnaligned<long>((void*) (pointer + this._offset + position));
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Reads a decimal value from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.
    /// 
    /// -or-
    /// 
    /// The decimal to read is invalid.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    public unsafe Decimal ReadDecimal(long position)
    {
      this.EnsureSafeToRead(position, 16);
      byte* pointer = (byte*) null;
      int lo;
      int mid;
      int hi;
      int num;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        pointer += this._offset + position;
        lo = Unsafe.ReadUnaligned<int>((void*) pointer);
        mid = Unsafe.ReadUnaligned<int>((void*) (pointer + 4));
        hi = Unsafe.ReadUnaligned<int>((void*) (pointer + 8));
        num = Unsafe.ReadUnaligned<int>((void*) (pointer + 12));
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
      if ((num & 2130771967) != 0 || (num & 16711680) > 1835008)
        throw new ArgumentException(SR.Arg_BadDecimal);
      bool isNegative = (num & int.MinValue) != 0;
      byte scale = (byte) (num >> 16);
      return new Decimal(lo, mid, hi, isNegative, scale);
    }

    /// <summary>Reads a single-precision floating-point value from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    public float ReadSingle(long position) => BitConverter.Int32BitsToSingle(this.ReadInt32(position));

    /// <summary>Reads a double-precision floating-point value from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    public double ReadDouble(long position) => BitConverter.Int64BitsToDouble(this.ReadInt64(position));

    /// <summary>Reads an 8-bit signed integer from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public sbyte ReadSByte(long position) => (sbyte) this.ReadByte(position);

    /// <summary>Reads an unsigned 16-bit integer from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public ushort ReadUInt16(long position) => (ushort) this.ReadInt16(position);

    /// <summary>Reads an unsigned 32-bit integer from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public uint ReadUInt32(long position) => (uint) this.ReadInt32(position);

    /// <summary>Reads an unsigned 64-bit integer from the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin reading.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public ulong ReadUInt64(long position) => (ulong) this.ReadInt64(position);

    /// <summary>Reads a structure of type <paramref name="T" /> from the accessor into a provided reference.</summary>
    /// <param name="position">The position in the accessor at which to begin reading.</param>
    /// <param name="structure">The structure to contain the read data.</param>
    /// <typeparam name="T">The type of structure.</typeparam>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to read in a structure of type <paramref name="T" />.
    /// 
    /// -or-
    /// 
    /// <see langword="T" /> is a value type that contains one or more reference types.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public void Read<T>(long position, out T structure) where T : struct
    {
      if (position < 0L)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (!this._isOpen)
        throw new ObjectDisposedException(nameof (UnmanagedMemoryAccessor), SR.ObjectDisposed_ViewAccessorClosed);
      if (!this._canRead)
        throw new NotSupportedException(SR.NotSupported_Reading);
      uint num = SafeBuffer.SizeOf<T>();
      if (position > this._capacity - (long) num)
      {
        if (position >= this._capacity)
          throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_PositionLessThanCapacityRequired);
        throw new ArgumentException(SR.Format(SR.Argument_NotEnoughBytesToRead, (object) typeof (T)), nameof (position));
      }
      structure = this._buffer.Read<T>((ulong) (this._offset + position));
    }

    /// <summary>Reads structures of type <paramref name="T" /> from the accessor into an array of type <paramref name="T" />.</summary>
    /// <param name="position">The number of bytes in the accessor at which to begin reading.</param>
    /// <param name="array">The array to contain the structures read from the accessor.</param>
    /// <param name="offset">The index in <paramref name="array" /> in which to place the first copied structure.</param>
    /// <param name="count">The number of structures of type <c>T</c> to read from the accessor.</param>
    /// <typeparam name="T">The type of structure.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="array" /> is not large enough to contain <paramref name="count" /> of structures (starting from <paramref name="position" />).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    /// <returns>The number of structures read into <paramref name="array" />. This value can be less than <paramref name="count" /> if there are fewer structures available, or zero if the end of the accessor is reached.</returns>
    public int ReadArray<T>(long position, T[] array, int offset, int count) where T : struct
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array), SR.ArgumentNull_Buffer);
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (array.Length - offset < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      if (!this._isOpen)
        throw new ObjectDisposedException(nameof (UnmanagedMemoryAccessor), SR.ObjectDisposed_ViewAccessorClosed);
      if (!this._canRead)
        throw new NotSupportedException(SR.NotSupported_Reading);
      if (position < 0L)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_NeedNonNegNum);
      uint num1 = SafeBuffer.AlignedSizeOf<T>();
      if (position >= this._capacity)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_PositionLessThanCapacityRequired);
      int count1 = count;
      long num2 = this._capacity - position;
      if (num2 < 0L)
      {
        count1 = 0;
      }
      else
      {
        ulong num3 = (ulong) num1 * (ulong) count;
        if ((ulong) num2 < num3)
          count1 = (int) (num2 / (long) num1);
      }
      this._buffer.ReadArray<T>((ulong) (this._offset + position), array, offset, count1);
      return count1;
    }

    /// <summary>Writes a Boolean value into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public void Write(long position, bool value) => this.Write(position, value ? (byte) 1 : (byte) 0);

    /// <summary>Writes a byte value into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public unsafe void Write(long position, byte value)
    {
      this.EnsureSafeToWrite(position, 1);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        (pointer + this._offset)[position] = value;
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Writes a character into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public void Write(long position, char value) => this.Write(position, (short) value);

    /// <summary>Writes a 16-bit integer into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public unsafe void Write(long position, short value)
    {
      this.EnsureSafeToWrite(position, 2);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        Unsafe.WriteUnaligned<short>((void*) (pointer + this._offset + position), value);
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Writes a 32-bit integer into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public unsafe void Write(long position, int value)
    {
      this.EnsureSafeToWrite(position, 4);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        Unsafe.WriteUnaligned<int>((void*) (pointer + this._offset + position), value);
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Writes a 64-bit integer into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after position to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public unsafe void Write(long position, long value)
    {
      this.EnsureSafeToWrite(position, 8);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        Unsafe.WriteUnaligned<long>((void*) (pointer + this._offset + position), value);
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Writes a decimal value into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.
    /// 
    /// -or-
    /// 
    /// The decimal is invalid.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public unsafe void Write(long position, Decimal value)
    {
      this.EnsureSafeToWrite(position, 16);
      // ISSUE: untyped stack allocation
      Span<int> destination = new Span<int>((void*) __untypedstackalloc(new IntPtr(16)), 4);
      Decimal.TryGetBits(value, destination, out int _);
      byte* pointer = (byte*) null;
      try
      {
        this._buffer.AcquirePointer(ref pointer);
        pointer += this._offset + position;
        Unsafe.WriteUnaligned<int>((void*) pointer, destination[0]);
        Unsafe.WriteUnaligned<int>((void*) (pointer + 4), destination[1]);
        Unsafe.WriteUnaligned<int>((void*) (pointer + 8), destination[2]);
        Unsafe.WriteUnaligned<int>((void*) (pointer + 12), destination[3]);
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this._buffer.ReleasePointer();
      }
    }

    /// <summary>Writes a <see cref="T:System.Single" /> into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public void Write(long position, float value) => this.Write(position, BitConverter.SingleToInt32Bits(value));

    /// <summary>Writes a <see cref="T:System.Double" /> value into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public void Write(long position, double value) => this.Write(position, BitConverter.DoubleToInt64Bits(value));

    /// <summary>Writes an 8-bit integer into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    [CLSCompliant(false)]
    public void Write(long position, sbyte value) => this.Write(position, (byte) value);

    /// <summary>Writes an unsigned 16-bit integer into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    [CLSCompliant(false)]
    public void Write(long position, ushort value) => this.Write(position, (short) value);

    /// <summary>Writes an unsigned 32-bit integer into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    [CLSCompliant(false)]
    public void Write(long position, uint value) => this.Write(position, (int) value);

    /// <summary>Writes an unsigned 64-bit integer into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes after <paramref name="position" /> to write a value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    [CLSCompliant(false)]
    public void Write(long position, ulong value) => this.Write(position, (long) value);

    /// <summary>Writes a structure into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="structure">The structure to write.</param>
    /// <typeparam name="T">The type of structure.</typeparam>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes in the accessor after <paramref name="position" /> to write a structure of type <paramref name="T" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> is less than zero or greater than the capacity of the accessor.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public void Write<T>(long position, ref T structure) where T : struct
    {
      if (position < 0L)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (!this._isOpen)
        throw new ObjectDisposedException(nameof (UnmanagedMemoryAccessor), SR.ObjectDisposed_ViewAccessorClosed);
      if (!this._canWrite)
        throw new NotSupportedException(SR.NotSupported_Writing);
      uint num = SafeBuffer.SizeOf<T>();
      if (position > this._capacity - (long) num)
      {
        if (position >= this._capacity)
          throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_PositionLessThanCapacityRequired);
        throw new ArgumentException(SR.Format(SR.Argument_NotEnoughBytesToWrite, (object) typeof (T)), nameof (position));
      }
      this._buffer.Write<T>((ulong) (this._offset + position), structure);
    }

    /// <summary>Writes structures from an array of type <paramref name="T" /> into the accessor.</summary>
    /// <param name="position">The number of bytes into the accessor at which to begin writing.</param>
    /// <param name="array">The array to write into the accessor.</param>
    /// <param name="offset">The index in <paramref name="array" /> to start writing from.</param>
    /// <param name="count">The number of structures in <paramref name="array" /> to write.</param>
    /// <typeparam name="T">The type of structure.</typeparam>
    /// <exception cref="T:System.ArgumentException">There are not enough bytes in the accessor after <paramref name="position" /> to write the number of structures specified by <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="position" /> is less than zero or greater than the capacity of the accessor.
    /// 
    /// -or-
    /// 
    /// <paramref name="offset" /> or <paramref name="count" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The accessor does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The accessor has been disposed.</exception>
    public void WriteArray<T>(long position, T[] array, int offset, int count) where T : struct
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array), SR.ArgumentNull_Buffer);
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (array.Length - offset < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      if (position < 0L)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (position >= this.Capacity)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_PositionLessThanCapacityRequired);
      if (!this._isOpen)
        throw new ObjectDisposedException(nameof (UnmanagedMemoryAccessor), SR.ObjectDisposed_ViewAccessorClosed);
      if (!this._canWrite)
        throw new NotSupportedException(SR.NotSupported_Writing);
      this._buffer.WriteArray<T>((ulong) (this._offset + position), array, offset, count);
    }

    private void EnsureSafeToRead(long position, int sizeOfType)
    {
      if (!this._isOpen)
        throw new ObjectDisposedException(nameof (UnmanagedMemoryAccessor), SR.ObjectDisposed_ViewAccessorClosed);
      if (!this._canRead)
        throw new NotSupportedException(SR.NotSupported_Reading);
      if (position < 0L)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (position <= this._capacity - (long) sizeOfType)
        return;
      if (position >= this._capacity)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_PositionLessThanCapacityRequired);
      throw new ArgumentException(SR.Argument_NotEnoughBytesToRead, nameof (position));
    }

    private void EnsureSafeToWrite(long position, int sizeOfType)
    {
      if (!this._isOpen)
        throw new ObjectDisposedException(nameof (UnmanagedMemoryAccessor), SR.ObjectDisposed_ViewAccessorClosed);
      if (!this._canWrite)
        throw new NotSupportedException(SR.NotSupported_Writing);
      if (position < 0L)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (position <= this._capacity - (long) sizeOfType)
        return;
      if (position >= this._capacity)
        throw new ArgumentOutOfRangeException(nameof (position), SR.ArgumentOutOfRange_PositionLessThanCapacityRequired);
      throw new ArgumentException(SR.Argument_NotEnoughBytesToWrite, nameof (position));
    }
  }
}
