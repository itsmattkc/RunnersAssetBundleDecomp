using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class CriStructMemory<Type> : IDisposable
{
	private GCHandle gch;

	private byte[] _bytes_k__BackingField;

	public byte[] bytes
	{
		get;
		private set;
	}

	public IntPtr ptr
	{
		get
		{
			return this.gch.AddrOfPinnedObject();
		}
	}

	public CriStructMemory()
	{
		this.bytes = new byte[Marshal.SizeOf(typeof(Type))];
		this.gch = GCHandle.Alloc(this.bytes, GCHandleType.Pinned);
	}

	public void Dispose()
	{
		this.gch.Free();
	}
}
