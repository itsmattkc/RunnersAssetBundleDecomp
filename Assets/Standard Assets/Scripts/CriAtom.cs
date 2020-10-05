using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("CRIWARE/CRI Atom")]
public class CriAtom : MonoBehaviour
{
	private sealed class _MargeCueSheet_c__AnonStorey4
	{
		internal int i;

		internal CriAtom __f__this;

		internal bool __m__0(CriAtomCueSheet sheet)
		{
			return sheet.name == this.__f__this.cueSheets[this.i].name;
		}
	}

	public string acfFile = string.Empty;

	public CriAtomCueSheet[] cueSheets = new CriAtomCueSheet[0];

	public string dspBusSetting = string.Empty;

	public bool dontDestroyOnLoad;

	private static CriAtomExSequencer.EventCbFunc eventUserCbFunc = null;

	private static Queue<string> eventQueue = new Queue<string>();

	public bool dontRemoveExistsCueSheet;

	private static CriAtom _instance_k__BackingField;

	private static CriAtom instance
	{
		get;
		set;
	}

	public static void AttachDspBusSetting(string settingName)
	{
		CriAtom.instance.dspBusSetting = settingName;
		if (!string.IsNullOrEmpty(settingName))
		{
			CriAtomEx.AttachDspBusSetting(settingName);
		}
		else
		{
			CriAtomEx.DetachDspBusSetting();
		}
	}

	public static void DetachDspBusSetting()
	{
		CriAtom.instance.dspBusSetting = string.Empty;
		CriAtomEx.DetachDspBusSetting();
	}

	public static CriAtomCueSheet GetCueSheet(string name)
	{
		return CriAtom.instance.GetCueSheetInternal(name);
	}

	public static CriAtomCueSheet AddCueSheet(string name, string acbFile, string awbFile, CriFsBinder binder = null)
	{
		return CriAtom.instance.AddCueSheetInternal(name, acbFile, awbFile, binder);
	}

	public static void RemoveCueSheet(string name)
	{
		CriAtom.instance.RemoveCueSheetInternal(name);
	}

	public static CriAtomExAcb GetAcb(string cueSheetName)
	{
		CriAtomCueSheet[] array = CriAtom.instance.cueSheets;
		for (int i = 0; i < array.Length; i++)
		{
			CriAtomCueSheet criAtomCueSheet = array[i];
			if (cueSheetName == criAtomCueSheet.name)
			{
				return criAtomCueSheet.acb;
			}
		}
		UnityEngine.Debug.LogWarning(cueSheetName + " is not loaded.");
		return null;
	}

	public static void SetCategoryVolume(string name, float volume)
	{
		CriAtomExCategory.SetVolume(name, volume);
	}

	public static void SetCategoryVolume(int id, float volume)
	{
		CriAtomExCategory.SetVolume(id, volume);
	}

	public static float GetCategoryVolume(string name)
	{
		return CriAtomExCategory.GetVolume(name);
	}

	public static float GetCategoryVolume(int id)
	{
		return CriAtomExCategory.GetVolume(id);
	}

	public static void SetBusAnalyzer(bool sw)
	{
		if (sw)
		{
			CriAtomExAsr.AttachBusAnalyzer(50, 1000);
		}
		else
		{
			CriAtomExAsr.DetachBusAnalyzer();
		}
	}

	public static CriAtomExAsr.BusAnalyzerInfo GetBusAnalyzerInfo(int bus)
	{
		CriAtomExAsr.BusAnalyzerInfo result;
		CriAtomExAsr.GetBusAnalyzerInfo(bus, out result);
		return result;
	}

	private void Setup()
	{
		CriAtom.instance = this;
		CriAtomPlugin.InitializeLibrary();
		if (!string.IsNullOrEmpty(this.acfFile))
		{
			string acfPath = Path.Combine(CriWare.streamingAssetsPath, this.acfFile);
			CriAtomEx.RegisterAcf(null, acfPath);
		}
		if (!string.IsNullOrEmpty(this.dspBusSetting))
		{
			CriAtom.AttachDspBusSetting(this.dspBusSetting);
		}
		CriAtomCueSheet[] array = this.cueSheets;
		for (int i = 0; i < array.Length; i++)
		{
			CriAtomCueSheet criAtomCueSheet = array[i];
			criAtomCueSheet.acb = this.LoadAcbFile(null, criAtomCueSheet.acbFile, criAtomCueSheet.awbFile);
		}
		if (this.dontDestroyOnLoad)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void Shutdown()
	{
		CriAtomCueSheet[] array = this.cueSheets;
		for (int i = 0; i < array.Length; i++)
		{
			CriAtomCueSheet criAtomCueSheet = array[i];
			if (criAtomCueSheet.acb != null)
			{
				criAtomCueSheet.acb.Dispose();
				criAtomCueSheet.acb = null;
			}
		}
		CriAtomPlugin.FinalizeLibrary();
		CriAtom.instance = null;
	}

	private void Awake()
	{
		if (CriAtom.instance != null)
		{
			if (CriAtom.instance.acfFile != this.acfFile)
			{
				GameObject gameObject = CriAtom.instance.gameObject;
				CriAtom.instance.Shutdown();
				CriAtomEx.UnregisterAcf();
				UnityEngine.Object.Destroy(gameObject);
				return;
			}
			if (CriAtom.instance.dspBusSetting != this.dspBusSetting)
			{
				CriAtom.AttachDspBusSetting(this.dspBusSetting);
			}
			CriAtom.instance.MargeCueSheet(this.cueSheets, this.dontRemoveExistsCueSheet);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnEnable()
	{
		if (CriAtom.instance != null)
		{
			return;
		}
		this.Setup();
	}

	private void OnDestroy()
	{
		if (this != CriAtom.instance)
		{
			return;
		}
		this.Shutdown();
	}

	private void Update()
	{
		if (CriAtom.eventUserCbFunc != null)
		{
			int count = CriAtom.eventQueue.Count;
			for (int i = 0; i < count; i++)
			{
				object syncRoot = ((ICollection)CriAtom.eventQueue).SyncRoot;
				string eventParamsString;
				lock (syncRoot)
				{
					eventParamsString = CriAtom.eventQueue.Dequeue();
				}
				CriAtom.eventUserCbFunc(eventParamsString);
			}
		}
	}

	public CriAtomCueSheet GetCueSheetInternal(string name)
	{
		for (int i = 0; i < this.cueSheets.Length; i++)
		{
			CriAtomCueSheet criAtomCueSheet = this.cueSheets[i];
			if (criAtomCueSheet.name == name)
			{
				return criAtomCueSheet;
			}
		}
		return null;
	}

	public CriAtomCueSheet AddCueSheetInternal(string name, string acbFile, string awbFile, CriFsBinder binder)
	{
		CriAtomCueSheet[] array = new CriAtomCueSheet[this.cueSheets.Length + 1];
		this.cueSheets.CopyTo(array, 0);
		this.cueSheets = array;
		CriAtomCueSheet criAtomCueSheet = new CriAtomCueSheet();
		this.cueSheets[this.cueSheets.Length - 1] = criAtomCueSheet;
		if (string.IsNullOrEmpty(name))
		{
			criAtomCueSheet.name = Path.GetFileNameWithoutExtension(acbFile);
		}
		else
		{
			criAtomCueSheet.name = name;
		}
		criAtomCueSheet.acbFile = acbFile;
		criAtomCueSheet.awbFile = awbFile;
		if (Application.isPlaying)
		{
			criAtomCueSheet.acb = this.LoadAcbFile(binder, acbFile, awbFile);
		}
		return criAtomCueSheet;
	}

	public void RemoveCueSheetInternal(string name)
	{
		int num = -1;
		for (int i = 0; i < this.cueSheets.Length; i++)
		{
			if (name == this.cueSheets[i].name)
			{
				num = i;
			}
		}
		if (num < 0)
		{
			return;
		}
		CriAtomCueSheet criAtomCueSheet = this.cueSheets[num];
		if (criAtomCueSheet.acb != null)
		{
			criAtomCueSheet.acb.Dispose();
		}
		CriAtomCueSheet[] destinationArray = new CriAtomCueSheet[this.cueSheets.Length - 1];
		Array.Copy(this.cueSheets, 0, destinationArray, 0, num);
		Array.Copy(this.cueSheets, num + 1, destinationArray, num, this.cueSheets.Length - num - 1);
		this.cueSheets = destinationArray;
	}

	private void MargeCueSheet(CriAtomCueSheet[] newCueSheets, bool newDontRemoveExistsCueSheet)
	{
		if (!newDontRemoveExistsCueSheet)
		{
			int i = 0;
			while (i < this.cueSheets.Length)
			{
				int num = Array.FindIndex<CriAtomCueSheet>(newCueSheets, (CriAtomCueSheet sheet) => sheet.name == this.cueSheets[i].name);
				if (num < 0)
				{
					CriAtom.RemoveCueSheet(this.cueSheets[i].name);
				}
				else
				{
					i++;
				}
			}
		}
		for (int j = 0; j < newCueSheets.Length; j++)
		{
			CriAtomCueSheet criAtomCueSheet = newCueSheets[j];
			if (this.GetCueSheetInternal(criAtomCueSheet.name) == null)
			{
				this.AddCueSheetInternal(null, criAtomCueSheet.acbFile, criAtomCueSheet.awbFile, null);
			}
		}
	}

	private CriAtomExAcb LoadAcbFile(CriFsBinder binder, string acbFile, string awbFile)
	{
		if (string.IsNullOrEmpty(acbFile))
		{
			return null;
		}
		string text = acbFile;
		if (binder == null && CriWare.IsStreamingAssetsPath(text))
		{
			text = Path.Combine(CriWare.streamingAssetsPath, text);
		}
		string text2 = awbFile;
		if (!string.IsNullOrEmpty(text2) && binder == null && CriWare.IsStreamingAssetsPath(text2))
		{
			text2 = Path.Combine(CriWare.streamingAssetsPath, text2);
		}
		return CriAtomExAcb.LoadAcbFile(binder, text, text2);
	}

	public void EventCallbackFromNative(string eventString)
	{
		if (CriAtom.eventUserCbFunc != null)
		{
			object syncRoot = ((ICollection)CriAtom.eventQueue).SyncRoot;
			lock (syncRoot)
			{
				CriAtom.eventQueue.Enqueue(eventString);
			}
		}
	}

	public static void SetEventCallback(CriAtomExSequencer.EventCbFunc func, string separator)
	{
		IntPtr cbfunc = IntPtr.Zero;
		CriAtom.eventUserCbFunc = func;
		if (func != null)
		{
			CriAtomExSequencer.EventCbFunc d = new CriAtomExSequencer.EventCbFunc(CriAtom.instance.EventCallbackFromNative);
			cbfunc = Marshal.GetFunctionPointerForDelegate(d);
			CriAtomPlugin.criAtomUnitySeqencer_SetEventCallback(cbfunc, CriAtom.instance.gameObject.name, "EventCallbackFromNative", separator);
		}
	}
}
