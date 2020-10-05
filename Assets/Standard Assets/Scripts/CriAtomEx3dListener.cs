using System;
using System.Runtime.InteropServices;

public class CriAtomEx3dListener : IDisposable
{
	public struct Config
	{
		public int reserved;
	}

	private struct CriAtomExVector
	{
		public float x;

		public float y;

		public float z;
	}

	private IntPtr handle = IntPtr.Zero;

	public IntPtr nativeHandle
	{
		get
		{
			return this.handle;
		}
	}

	public CriAtomEx3dListener()
	{
		CriAtomEx3dListener.Config config = default(CriAtomEx3dListener.Config);
		this.handle = CriAtomEx3dListener.criAtomEx3dListener_Create(ref config, IntPtr.Zero, 0);
	}

	public void Dispose()
	{
		CriAtomEx3dListener.criAtomEx3dListener_Destroy(this.handle);
		GC.SuppressFinalize(this);
	}

	public void Update()
	{
		CriAtomEx3dListener.criAtomEx3dListener_Update(this.handle);
	}

	public void ResetParameters()
	{
		CriAtomEx3dListener.criAtomEx3dListener_ResetParameters(this.handle);
	}

	public void SetPosition(float x, float y, float z)
	{
		CriAtomEx3dListener.CriAtomExVector criAtomExVector;
		criAtomExVector.x = x;
		criAtomExVector.y = y;
		criAtomExVector.z = z;
		CriAtomEx3dListener.criAtomEx3dListener_SetPosition(this.handle, ref criAtomExVector);
	}

	public void SetVelocity(float x, float y, float z)
	{
		CriAtomEx3dListener.CriAtomExVector criAtomExVector;
		criAtomExVector.x = x;
		criAtomExVector.y = y;
		criAtomExVector.z = z;
		CriAtomEx3dListener.criAtomEx3dListener_SetVelocity(this.handle, ref criAtomExVector);
	}

	public void SetOrientation(float fx, float fy, float fz, float ux, float uy, float uz)
	{
		CriAtomEx3dListener.CriAtomExVector criAtomExVector;
		criAtomExVector.x = fx;
		criAtomExVector.y = fy;
		criAtomExVector.z = fz;
		CriAtomEx3dListener.CriAtomExVector criAtomExVector2;
		criAtomExVector2.x = ux;
		criAtomExVector2.y = uy;
		criAtomExVector2.z = uz;
		CriAtomEx3dListener.criAtomEx3dListener_SetOrientation(this.handle, ref criAtomExVector, ref criAtomExVector2);
	}

	public void SetDistanceFactor(float distanceFactor)
	{
		CriAtomEx3dListener.criAtomEx3dListener_SetDistanceFactor(this.handle, distanceFactor);
	}

	public void SetFocusPoint(float x, float y, float z)
	{
		CriAtomEx3dListener.CriAtomExVector criAtomExVector;
		criAtomExVector.x = x;
		criAtomExVector.y = y;
		criAtomExVector.z = z;
		CriAtomEx3dListener.criAtomEx3dListener_SetFocusPoint(this.handle, ref criAtomExVector);
	}

	public void SetDistanceFocusLevel(float distanceFocusLevel)
	{
		CriAtomEx3dListener.criAtomEx3dListener_SetDistanceFocusLevel(this.handle, distanceFocusLevel);
	}

	public void SetDirectionFocusLevel(float directionFocusLevel)
	{
		CriAtomEx3dListener.criAtomEx3dListener_SetDirectionFocusLevel(this.handle, directionFocusLevel);
	}

	~CriAtomEx3dListener()
	{
		this.Dispose();
	}

	[DllImport("cri_ware_unity")]
	private static extern IntPtr criAtomEx3dListener_Create(ref CriAtomEx3dListener.Config config, IntPtr work, int work_size);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_Destroy(IntPtr ex_3d_listener);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_Update(IntPtr ex_3d_listener);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_ResetParameters(IntPtr ex_3d_listener);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_SetPosition(IntPtr ex_3d_listener, ref CriAtomEx3dListener.CriAtomExVector position);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_SetVelocity(IntPtr ex_3d_listener, ref CriAtomEx3dListener.CriAtomExVector velocity);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_SetOrientation(IntPtr ex_3d_listener, ref CriAtomEx3dListener.CriAtomExVector front, ref CriAtomEx3dListener.CriAtomExVector top);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_SetDistanceFactor(IntPtr ex_3d_listener, float distance_factor);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_SetFocusPoint(IntPtr ex_3d_listener, ref CriAtomEx3dListener.CriAtomExVector focus_point);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_SetDistanceFocusLevel(IntPtr ex_3d_listener, float distance_focus_level);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dListener_SetDirectionFocusLevel(IntPtr ex_3d_listener, float direction_focus_level);
}
