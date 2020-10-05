using System;
using System.Runtime.InteropServices;

public class CriAtomEx3dSource : IDisposable
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

	public CriAtomEx3dSource()
	{
		CriAtomEx3dSource.Config config = default(CriAtomEx3dSource.Config);
		this.handle = CriAtomEx3dSource.criAtomEx3dSource_Create(ref config, IntPtr.Zero, 0);
	}

	public void Dispose()
	{
		CriAtomEx3dSource.criAtomEx3dSource_Destroy(this.handle);
		GC.SuppressFinalize(this);
	}

	public void Update()
	{
		CriAtomEx3dSource.criAtomEx3dSource_Update(this.handle);
	}

	public void ResetParameters()
	{
		CriAtomEx3dSource.criAtomEx3dSource_ResetParameters(this.handle);
	}

	public void SetPosition(float x, float y, float z)
	{
		CriAtomEx3dSource.CriAtomExVector criAtomExVector;
		criAtomExVector.x = x;
		criAtomExVector.y = y;
		criAtomExVector.z = z;
		CriAtomEx3dSource.criAtomEx3dSource_SetPosition(this.handle, ref criAtomExVector);
	}

	public void SetVelocity(float x, float y, float z)
	{
		CriAtomEx3dSource.CriAtomExVector criAtomExVector;
		criAtomExVector.x = x;
		criAtomExVector.y = y;
		criAtomExVector.z = z;
		CriAtomEx3dSource.criAtomEx3dSource_SetVelocity(this.handle, ref criAtomExVector);
	}

	public void SetConeOrientation(float x, float y, float z)
	{
		CriAtomEx3dSource.CriAtomExVector criAtomExVector;
		criAtomExVector.x = x;
		criAtomExVector.y = y;
		criAtomExVector.z = z;
		CriAtomEx3dSource.criAtomEx3dSource_SetConeOrientation(this.handle, ref criAtomExVector);
	}

	public void SetConeParameter(float insideAngle, float outsideAngle, float outsideVolume)
	{
		CriAtomEx3dSource.criAtomEx3dSource_SetConeParameter(this.handle, insideAngle, outsideAngle, outsideVolume);
	}

	public void SetMinMaxDistance(float minDistance, float maxDistance)
	{
		CriAtomEx3dSource.criAtomEx3dSource_SetMinMaxDistance(this.handle, minDistance, maxDistance);
	}

	public void SetDopplerFactor(float dopplerFactor)
	{
		CriAtomEx3dSource.criAtomEx3dSource_SetDopplerFactor(this.handle, dopplerFactor);
	}

	public void SetVolume(float volume)
	{
		CriAtomEx3dSource.criAtomEx3dSource_SetVolume(this.handle, volume);
	}

	public void SetMaxAngleAisacDelta(float maxDelta)
	{
		CriAtomEx3dSource.criAtomEx3dSource_SetMaxAngleAisacDelta(this.handle, maxDelta);
	}

	~CriAtomEx3dSource()
	{
		this.Dispose();
	}

	[DllImport("cri_ware_unity")]
	private static extern IntPtr criAtomEx3dSource_Create(ref CriAtomEx3dSource.Config config, IntPtr work, int work_size);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_Destroy(IntPtr ex_3d_source);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_Update(IntPtr ex_3d_source);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_ResetParameters(IntPtr ex_3d_source);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_SetPosition(IntPtr ex_3d_source, ref CriAtomEx3dSource.CriAtomExVector position);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_SetVelocity(IntPtr ex_3d_source, ref CriAtomEx3dSource.CriAtomExVector velocity);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_SetConeOrientation(IntPtr ex_3d_source, ref CriAtomEx3dSource.CriAtomExVector cone_orient);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_SetConeParameter(IntPtr ex_3d_source, float inside_angle, float outside_angle, float outside_volume);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_SetMinMaxDistance(IntPtr ex_3d_source, float min_distance, float max_distance);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_SetDopplerFactor(IntPtr ex_3d_source, float doppler_factor);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_SetVolume(IntPtr ex_3d_source, float volume);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomEx3dSource_SetMaxAngleAisacDelta(IntPtr ex_3d_source, float max_delta);
}
