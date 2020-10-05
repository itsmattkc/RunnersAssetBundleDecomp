using System;
using System.Runtime.InteropServices;

public static class CriAtomExCategory
{
	public static void SetVolume(string name, float volume)
	{
		CriAtomExCategory.criAtomExCategory_SetVolumeByName(name, volume);
	}

	public static void SetVolume(int id, float volume)
	{
		CriAtomExCategory.criAtomExCategory_SetVolumeById(id, volume);
	}

	public static float GetVolume(string name)
	{
		return CriAtomExCategory.criAtomExCategory_GetVolumeByName(name);
	}

	public static float GetVolume(int id)
	{
		return CriAtomExCategory.criAtomExCategory_GetVolumeById(id);
	}

	public static void Mute(string name, bool mute)
	{
		CriAtomExCategory.criAtomExCategory_MuteByName(name, mute);
	}

	public static void Mute(int id, bool mute)
	{
		CriAtomExCategory.criAtomExCategory_MuteById(id, mute);
	}

	public static bool IsMuted(string name)
	{
		return CriAtomExCategory.criAtomExCategory_IsMutedByName(name);
	}

	public static bool IsMuted(int id)
	{
		return CriAtomExCategory.criAtomExCategory_IsMutedById(id);
	}

	public static void Solo(string name, bool solo, float muteVolume)
	{
		CriAtomExCategory.criAtomExCategory_SoloByName(name, solo, muteVolume);
	}

	public static void Solo(int id, bool solo, float muteVolume)
	{
		CriAtomExCategory.criAtomExCategory_SoloById(id, solo, muteVolume);
	}

	public static bool IsSoloed(string name)
	{
		return CriAtomExCategory.criAtomExCategory_IsSoloedByName(name);
	}

	public static bool IsSoloed(int id)
	{
		return CriAtomExCategory.criAtomExCategory_IsSoloedById(id);
	}

	public static void Pause(string name, bool pause)
	{
		CriAtomExCategory.criAtomExCategory_PauseByName(name, pause);
	}

	public static void Pause(int id, bool pause)
	{
		CriAtomExCategory.criAtomExCategory_PauseById(id, pause);
	}

	public static bool IsPaused(string name)
	{
		return CriAtomExCategory.criAtomExCategory_IsPausedByName(name);
	}

	public static bool IsPaused(int id)
	{
		return CriAtomExCategory.criAtomExCategory_IsPausedById(id);
	}

	public static void SetAisac(string name, string controlName, float value)
	{
		CriAtomExCategory.criAtomExCategory_SetAisacControlByName(name, controlName, value);
	}

	public static void SetAisac(int id, int controlId, float value)
	{
		CriAtomExCategory.criAtomExCategory_SetAisacControlById(id, (ushort)controlId, value);
	}

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_SetVolumeByName(string name, float volume);

	[DllImport("cri_ware_unity")]
	private static extern float criAtomExCategory_GetVolumeByName(string name);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_SetVolumeById(int id, float volume);

	[DllImport("cri_ware_unity")]
	private static extern float criAtomExCategory_GetVolumeById(int id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_MuteById(int id, bool mute);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExCategory_IsMutedById(int id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_MuteByName(string name, bool mute);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExCategory_IsMutedByName(string name);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_SoloById(int id, bool solo, float volume);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExCategory_IsSoloedById(int id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_SoloByName(string name, bool solo, float volume);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExCategory_IsSoloedByName(string name);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_PauseById(int id, bool pause);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExCategory_IsPausedById(int id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_PauseByName(string name, bool pause);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExCategory_IsPausedByName(string name);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_SetAisacControlById(int id, ushort controlId, float value);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExCategory_SetAisacControlByName(string name, string controlName, float value);
}
