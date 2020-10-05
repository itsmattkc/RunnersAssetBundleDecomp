using System;
using System.Runtime.InteropServices;

public class CriAtomExAcb : IDisposable
{
	private IntPtr handle = IntPtr.Zero;

	public IntPtr nativeHandle
	{
		get
		{
			return this.handle;
		}
	}

	private CriAtomExAcb(IntPtr handle)
	{
		this.handle = handle;
	}

	public static CriAtomExAcb LoadAcbFile(CriFsBinder binder, string acbPath, string awbPath)
	{
		IntPtr intPtr = (binder == null) ? IntPtr.Zero : binder.nativeHandle;
		IntPtr value = CriAtomExAcb.criAtomExAcb_LoadAcbFile(intPtr, acbPath, intPtr, awbPath, IntPtr.Zero, 0);
		if (value == IntPtr.Zero)
		{
			return null;
		}
		return new CriAtomExAcb(value);
	}

	public void Dispose()
	{
		CriAtomExAcb.criAtomExAcb_Release(this.handle);
		GC.SuppressFinalize(this);
	}

	public bool Exists(string cueName)
	{
		return CriAtomExAcb.criAtomExAcb_ExistsName(this.handle, cueName);
	}

	public bool Exists(int cueId)
	{
		return CriAtomExAcb.criAtomExAcb_ExistsId(this.handle, cueId);
	}

	public bool GetCueInfo(string cueName, out CriAtomEx.CueInfo info)
	{
		bool result;
		using (CriStructMemory<CriAtomEx.CueInfo> criStructMemory = new CriStructMemory<CriAtomEx.CueInfo>())
		{
			bool flag = CriAtomExAcb.criAtomExAcb_GetCueInfoByName(this.handle, cueName, criStructMemory.ptr);
			info = new CriAtomEx.CueInfo(criStructMemory.bytes, 0);
			result = flag;
		}
		return result;
	}

	public bool GetCueInfo(int cueId, out CriAtomEx.CueInfo info)
	{
		bool result;
		using (CriStructMemory<CriAtomEx.CueInfo> criStructMemory = new CriStructMemory<CriAtomEx.CueInfo>())
		{
			bool flag = CriAtomExAcb.criAtomExAcb_GetCueInfoById(this.handle, cueId, criStructMemory.ptr);
			info = new CriAtomEx.CueInfo(criStructMemory.bytes, 0);
			result = flag;
		}
		return result;
	}

	public bool GetCueInfoByIndex(int index, out CriAtomEx.CueInfo info)
	{
		bool result;
		using (CriStructMemory<CriAtomEx.CueInfo> criStructMemory = new CriStructMemory<CriAtomEx.CueInfo>())
		{
			bool flag = CriAtomExAcb.criAtomExAcb_GetCueInfoByIndex(this.handle, index, criStructMemory.ptr);
			info = new CriAtomEx.CueInfo(criStructMemory.bytes, 0);
			result = flag;
		}
		return result;
	}

	public CriAtomEx.CueInfo[] GetCueInfoList()
	{
		int num = CriAtomExAcb.criAtomExAcb_GetNumCues(this.handle);
		CriAtomEx.CueInfo[] array = new CriAtomEx.CueInfo[num];
		for (int i = 0; i < num; i++)
		{
			this.GetCueInfoByIndex(i, out array[i]);
		}
		return array;
	}

	public bool GetWaveFormInfo(string cueName, out CriAtomEx.WaveformInfo info)
	{
		bool result;
		using (CriStructMemory<CriAtomEx.WaveformInfo> criStructMemory = new CriStructMemory<CriAtomEx.WaveformInfo>())
		{
			bool flag = CriAtomExAcb.criAtomExAcb_GetWaveformInfoByName(this.handle, cueName, criStructMemory.ptr);
			info = new CriAtomEx.WaveformInfo(criStructMemory.bytes, 0);
			result = flag;
		}
		return result;
	}

	public bool GetWaveFormInfo(int cueId, out CriAtomEx.WaveformInfo info)
	{
		bool result;
		using (CriStructMemory<CriAtomEx.WaveformInfo> criStructMemory = new CriStructMemory<CriAtomEx.WaveformInfo>())
		{
			bool flag = CriAtomExAcb.criAtomExAcb_GetWaveformInfoById(this.handle, cueId, criStructMemory.ptr);
			info = new CriAtomEx.WaveformInfo(criStructMemory.bytes, 0);
			result = flag;
		}
		return result;
	}

	public int GetNumCuePlaying(string name)
	{
		return CriAtomExAcb.criAtomExAcb_GetNumCuePlayingCountByName(this.handle, name);
	}

	public int GetNumCuePlaying(int id)
	{
		return CriAtomExAcb.criAtomExAcb_GetNumCuePlayingCountById(this.handle, id);
	}

	public int GetBlockIndex(string cueName, string blockName)
	{
		return CriAtomExAcb.criAtomExAcb_GetBlockIndexByName(this.handle, cueName, blockName);
	}

	public int GetBlockIndex(int cueId, string blockName)
	{
		return CriAtomExAcb.criAtomExAcb_GetBlockIndexById(this.handle, cueId, blockName);
	}

	public int GetNumUsableAisacControls(string cueName)
	{
		return CriAtomExAcb.criAtomExAcb_GetNumUsableAisacControlsByName(this.handle, cueName);
	}

	public int GetNumUsableAisacControls(int cueId)
	{
		return CriAtomExAcb.criAtomExAcb_GetNumUsableAisacControlsById(this.handle, cueId);
	}

	public bool GetUsableAisacControl(string cueName, int index, out CriAtomEx.AisacControlInfo info)
	{
		bool result;
		using (CriStructMemory<CriAtomEx.AisacControlInfo> criStructMemory = new CriStructMemory<CriAtomEx.AisacControlInfo>())
		{
			bool flag = CriAtomExAcb.criAtomExAcb_GetUsableAisacControlByName(this.handle, cueName, (ushort)index, criStructMemory.ptr);
			info = new CriAtomEx.AisacControlInfo(criStructMemory.bytes, 0);
			result = flag;
		}
		return result;
	}

	public bool GetUsableAisacControl(int cueId, int index, out CriAtomEx.AisacControlInfo info)
	{
		bool result;
		using (CriStructMemory<CriAtomEx.AisacControlInfo> criStructMemory = new CriStructMemory<CriAtomEx.AisacControlInfo>())
		{
			bool flag = CriAtomExAcb.criAtomExAcb_GetUsableAisacControlById(this.handle, cueId, (ushort)index, criStructMemory.ptr);
			info = new CriAtomEx.AisacControlInfo(criStructMemory.bytes, 0);
			result = flag;
		}
		return result;
	}

	public CriAtomEx.AisacControlInfo[] GetUsableAisacControlList(string cueName)
	{
		int numUsableAisacControls = this.GetNumUsableAisacControls(cueName);
		CriAtomEx.AisacControlInfo[] array = new CriAtomEx.AisacControlInfo[numUsableAisacControls];
		for (int i = 0; i < numUsableAisacControls; i++)
		{
			this.GetUsableAisacControl(cueName, i, out array[i]);
		}
		return array;
	}

	public CriAtomEx.AisacControlInfo[] GetUsableAisacControlList(int cueId)
	{
		int numUsableAisacControls = this.GetNumUsableAisacControls(cueId);
		CriAtomEx.AisacControlInfo[] array = new CriAtomEx.AisacControlInfo[numUsableAisacControls];
		for (int i = 0; i < numUsableAisacControls; i++)
		{
			this.GetUsableAisacControl(cueId, i, out array[i]);
		}
		return array;
	}

	public void ResetCueTypeState(string cueName)
	{
		CriAtomExAcb.criAtomExAcb_ResetCueTypeStateByName(this.handle, cueName);
	}

	public void ResetCueTypeState(int cueId)
	{
		CriAtomExAcb.criAtomExAcb_ResetCueTypeStateById(this.handle, cueId);
	}

	~CriAtomExAcb()
	{
		this.Dispose();
	}

	[DllImport("cri_ware_unity")]
	private static extern IntPtr criAtomExAcb_LoadAcbFile(IntPtr acb_binder, string acb_path, IntPtr awb_binder, string awb_path, IntPtr work, int work_size);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExAcb_Release(IntPtr acb_hn);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExAcb_GetNumCues(IntPtr acb_hn);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_ExistsId(IntPtr acb_hn, int id);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_ExistsName(IntPtr acb_hn, string name);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExAcb_GetNumUsableAisacControlsById(IntPtr acb_hn, int id);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExAcb_GetNumUsableAisacControlsByName(IntPtr acb_hn, string name);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_GetUsableAisacControlById(IntPtr acb_hn, int id, ushort index, IntPtr info);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_GetUsableAisacControlByName(IntPtr acb_hn, string name, ushort index, IntPtr info);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_GetWaveformInfoById(IntPtr acb_hn, int id, IntPtr waveform_info);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_GetWaveformInfoByName(IntPtr acb_hn, string name, IntPtr waveform_info);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_GetCueInfoByName(IntPtr acb_hn, string name, IntPtr info);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_GetCueInfoById(IntPtr acb_hn, int id, IntPtr info);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExAcb_GetCueInfoByIndex(IntPtr acb_hn, int index, IntPtr info);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExAcb_GetNumCuePlayingCountByName(IntPtr acb_hn, string name);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExAcb_GetNumCuePlayingCountById(IntPtr acb_hn, int id);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExAcb_GetBlockIndexById(IntPtr acb_hn, int id, string block_name);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExAcb_GetBlockIndexByName(IntPtr acb_hn, string name, string block_name);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExAcb_ResetCueTypeStateByName(IntPtr acb_hn, string name);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExAcb_ResetCueTypeStateById(IntPtr acb_hn, int id);
}
