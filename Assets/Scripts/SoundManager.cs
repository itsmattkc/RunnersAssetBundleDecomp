using SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public enum SourceId
	{
		NONE = -1,
		BGM_NORMAL,
		BGM_BEGIN = 0,
		BGM_CROSSFADE,
		SE,
		BGM_END = 2,
		COUNT
	}

	public enum PlayId
	{
		NONE
	}

	public class Playback
	{
		private CriAtomExPlayback m_atomExPlayback;

		private string _cueName_k__BackingField;

		private string _cueSheet_k__BackingField;

		public string cueName
		{
			get;
			private set;
		}

		public string cueSheet
		{
			get;
			private set;
		}

		public CriAtomExPlayback.Status status
		{
			get
			{
				return this.m_atomExPlayback.status;
			}
		}

		public Playback(CriAtomExPlayback playback, string cueName, string cueSheet)
		{
			this.m_atomExPlayback = playback;
			this.cueName = cueName;
			this.cueSheet = cueSheet;
		}

		public void Stop()
		{
			this.m_atomExPlayback.Stop();
		}

		public void Pause(bool sw)
		{
			this.m_atomExPlayback.Pause(sw);
		}
	}

	public class Source
	{
		private enum FadeType
		{
			NONE,
			OUT,
			OFF,
			IN
		}

		private static SoundManager.PlayId s_playIdBase;

		private float m_masterVolume = 1f;

		private bool m_isPaused;

		private SoundManager.Source.FadeType m_fadeType;

		private float m_fadeTime;

		private Dictionary<SoundManager.PlayId, SoundManager.Playback> m_playbacks = new Dictionary<SoundManager.PlayId, SoundManager.Playback>();

		private CriAtomSource m_atomSource;

		private SoundManager.PlayId m_playId;

		public Dictionary<SoundManager.PlayId, SoundManager.Playback> playbacks
		{
			get
			{
				return this.m_playbacks;
			}
		}

		public SoundManager.PlayId playId
		{
			get
			{
				return this.m_playId;
			}
		}

		public CriAtomSource.Status status
		{
			get
			{
				return this.m_atomSource.status;
			}
		}

		public string cueName
		{
			get
			{
				return this.m_atomSource.cueName;
			}
			set
			{
				this.m_atomSource.cueName = value;
			}
		}

		public string cueSheet
		{
			get
			{
				return this.m_atomSource.cueSheet;
			}
			set
			{
				this.m_atomSource.cueSheet = value;
			}
		}

		public float volume
		{
			get
			{
				return this.m_atomSource.volume;
			}
			set
			{
				this.m_atomSource.volume = value;
			}
		}

		public bool loop
		{
			get
			{
				return this.m_atomSource.loop;
			}
			set
			{
				this.m_atomSource.loop = value;
			}
		}

		public float masterVolume
		{
			get
			{
				return this.m_masterVolume;
			}
			set
			{
				this.m_masterVolume = value;
				if (this.m_fadeType == SoundManager.Source.FadeType.NONE)
				{
					this.volume = value;
				}
			}
		}

		public Source(CriAtomSource atomSource)
		{
			this.m_atomSource = atomSource;
		}

		private SoundManager.PlayId GeneratePlayId()
		{
			do
			{
				SoundManager.Source.s_playIdBase = (SoundManager.Source.s_playIdBase + 1) % (SoundManager.PlayId)2147483647;
			}
			while (SoundManager.Source.s_playIdBase == SoundManager.PlayId.NONE || this.m_playbacks.ContainsKey(SoundManager.Source.s_playIdBase));
			return SoundManager.Source.s_playIdBase;
		}

		public SoundManager.PlayId Play(string cueName)
		{
			this.m_atomSource.volume = this.m_masterVolume;
			CriAtomExPlayback playback = this.m_atomSource.Play(cueName);
			this.m_playId = this.GeneratePlayId();
			this.m_playbacks.Add(this.m_playId, new SoundManager.Playback(playback, cueName, this.cueSheet));
			this.Removes();
			return this.m_playId;
		}

		public void Stop()
		{
			this.m_atomSource.Stop();
			this.FadeClear();
		}

		public void Pause(bool sw)
		{
			this.m_atomSource.Pause(sw);
			this.m_isPaused = sw;
		}

		public void Stop(SoundManager.PlayId playId)
		{
			if (this.m_playbacks.ContainsKey(playId))
			{
				this.m_playbacks[playId].Stop();
			}
		}

		public void Pause(SoundManager.PlayId playId, bool sw)
		{
			if (this.m_playbacks.ContainsKey(playId))
			{
				this.m_playbacks[playId].Pause(sw);
			}
		}

		private void Removes()
		{
			List<SoundManager.PlayId> list = new List<SoundManager.PlayId>();
			foreach (SoundManager.PlayId current in this.m_playbacks.Keys)
			{
				if (this.m_playbacks[current].status == CriAtomExPlayback.Status.Removed)
				{
					list.Add(current);
				}
			}
			foreach (SoundManager.PlayId current2 in list)
			{
				this.m_playbacks.Remove(current2);
			}
		}

		public void FadeOutStart(float fadeTime)
		{
			this.FadeStart(SoundManager.Source.FadeType.OUT, fadeTime, -1f);
		}

		public void FadeOffStart(float fadeTime)
		{
			this.FadeStart(SoundManager.Source.FadeType.OFF, fadeTime, -1f);
		}

		public void FadeInStart(float fadeTime, float startVolume = -1f)
		{
			this.FadeStart(SoundManager.Source.FadeType.IN, fadeTime, startVolume);
		}

		private void FadeStart(SoundManager.Source.FadeType fadeType, float fadeTime, float startVolume)
		{
			if (SoundManager.IsPlayingStatus(this.status))
			{
				if (fadeTime == 0f)
				{
					this.FadeEnd(fadeType);
				}
				else
				{
					this.m_fadeType = fadeType;
					this.m_fadeTime = fadeTime;
					if (startVolume != -1f)
					{
						this.volume = startVolume;
					}
				}
			}
		}

		private void FadeEnd(SoundManager.Source.FadeType fadeType)
		{
			if (fadeType == SoundManager.Source.FadeType.OUT)
			{
				this.Stop();
				this.volume = this.m_masterVolume;
			}
			else
			{
				this.volume = this.GetTargetVolume(fadeType);
			}
			this.FadeClear();
		}

		private void FadeClear()
		{
			this.m_fadeType = SoundManager.Source.FadeType.NONE;
			this.m_fadeTime = 0f;
		}

		private float GetTargetVolume(SoundManager.Source.FadeType fadeType)
		{
			return (fadeType != SoundManager.Source.FadeType.IN) ? 0f : this.m_masterVolume;
		}

		public void FixedUpdate()
		{
			if (this.m_fadeType != SoundManager.Source.FadeType.NONE && !this.m_isPaused)
			{
				float targetVolume = this.GetTargetVolume(this.m_fadeType);
				if (this.m_fadeTime > Time.fixedDeltaTime)
				{
					this.volume += (targetVolume - this.volume) * Time.fixedDeltaTime / this.m_fadeTime;
					this.m_fadeTime -= Time.fixedDeltaTime;
				}
				else
				{
					this.FadeEnd(this.m_fadeType);
				}
			}
		}
	}

	private const string PLATFORM_DATA_PATH = "Android/";

	private const string ACF_FILE_PATH = "Android/Sonic_Runners_Sound.acf";

	[SerializeField]
	private bool m_isOutputPlayLog;

	private static SoundManager s_instance;

	private List<string> m_cueSheetNameList = new List<string>();

	private SoundManager.Source[] m_sources = new SoundManager.Source[3];

	private float m_bgmVolume = 1f;

	private float m_seVolume = 1f;

	private string proxyHost;

	private ushort proxyPort;

	public static float BgmVolume
	{
		get
		{
			return (!(SoundManager.s_instance != null)) ? 0f : SoundManager.s_instance.m_bgmVolume;
		}
		set
		{
			if (SoundManager.s_instance != null)
			{
				SoundManager.s_instance.m_bgmVolume = value;
				for (SoundManager.SourceId sourceId = SoundManager.SourceId.BGM_NORMAL; sourceId < SoundManager.SourceId.SE; sourceId++)
				{
					SoundManager.GetSource(sourceId).masterVolume = value;
				}
			}
		}
	}

	public static float SeVolume
	{
		get
		{
			return (!(SoundManager.s_instance != null)) ? 0f : SoundManager.s_instance.m_seVolume;
		}
		set
		{
			if (SoundManager.s_instance != null)
			{
				SoundManager.s_instance.m_seVolume = value;
				SoundManager.GetSource(SoundManager.SourceId.SE).masterVolume = value;
			}
		}
	}

	private void Start()
	{
		this.Initialize();
		SoundManager.s_instance = this;
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				SoundManager.BgmVolume = (float)systemdata.bgmVolume / 100f;
				SoundManager.SeVolume = (float)systemdata.seVolume / 100f;
			}
		}
	}

	private void Update()
	{
		base.gameObject.transform.position = new Vector3(-1000f, -1000f, 0f);
	}

	private void FixedUpdate()
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		for (SoundManager.SourceId sourceId = SoundManager.SourceId.BGM_NORMAL; sourceId < SoundManager.SourceId.SE; sourceId++)
		{
			SoundManager.GetSource(sourceId).FixedUpdate();
		}
	}

	private void OnDestroy()
	{
		this.m_cueSheetNameList = new List<string>();
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.SetProxyToCriFs();
		}
	}

	public static void AddTitleCueSheet()
	{
		SoundManager.AddCueSheet("BGM", "BGM_title.acb", "BGM_title_streamfiles.awb", false);
		SoundManager.AddCueSheet("SE", "se_runners_title.acb", null, false);
	}

	public static void AddMainMenuCommonCueSheet()
	{
		SoundManager.AddCueSheet("BGM", "BGM_menu.acb", "BGM_menu_streamfiles.awb", true);
		SoundManager.AddCueSheet("BGM_menu_v2", "BGM_menu_v2.acb", "BGM_menu_v2_streamfiles.awb", true);
		SoundManager.AddCueSheet("SE", "se_runners.acb", null, true);
	}

	public static void AddMainMenuEventCueSheet()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsInEvent())
		{
			SoundManager.AddEventBgmCueSheet(EventManager.Instance.Type);
			SoundManager.AddEventSeCueSheet(EventManager.Instance.Type);
		}
	}

	public static void AddStageCommonCueSheet()
	{
		SoundManager.AddCueSheet("SE", "se_runners.acb", null, true);
	}

	public static void AddStageCueSheet(string stageCueSheetName)
	{
		if (stageCueSheetName != null && stageCueSheetName != string.Empty)
		{
			SoundManager.AddCueSheet("BGM", stageCueSheetName + ".acb", stageCueSheetName + "_streamfiles.awb", true);
		}
		SoundManager.AddCueSheet("BGM_jingle", "BGM_jingle.acb", "BGM_jingle_streamfiles.awb", true);
		if (EventManager.Instance != null)
		{
			if (EventManager.Instance.IsSpecialStage())
			{
				SoundManager.AddEventSeCueSheet(EventManager.EventType.SPECIAL_STAGE);
			}
			if (EventManager.Instance.IsRaidBossStage())
			{
				SoundManager.AddEventSeCueSheet(EventManager.EventType.RAID_BOSS);
			}
		}
	}

	private static void AddEventBgmCueSheet(EventManager.EventType type)
	{
		if (EventCommonDataTable.Instance != null)
		{
			string data = EventCommonDataTable.Instance.GetData(EventCommonDataItem.MenuBgmFileName);
			if (data != null && data != string.Empty)
			{
				SoundManager.AddCueSheet("BGM_" + EventManager.GetEventTypeName(type), data + ".acb", data + "_streamfiles.awb", true);
			}
		}
	}

	private static void AddEventSeCueSheet(EventManager.EventType type)
	{
		if (EventCommonDataTable.Instance != null)
		{
			string data = EventCommonDataTable.Instance.GetData(EventCommonDataItem.SeFileName);
			if (data != null && data != string.Empty)
			{
				SoundManager.AddCueSheet("SE_" + EventManager.GetEventTypeName(type), data + ".acb", null, true);
			}
		}
	}

	private void Initialize()
	{
		CriWareErrorHandler criWareErrorHandler = UnityEngine.Object.FindObjectOfType<CriWareErrorHandler>();
		if (criWareErrorHandler != null)
		{
			criWareErrorHandler.enabled = false;
		}
		CriAtomEx.RegisterAcf(null, Path.Combine(CriWare.streamingAssetsPath, "Android/Sonic_Runners_Sound.acf"));
		for (int i = 0; i < this.m_sources.Length; i++)
		{
			this.m_sources[i] = new SoundManager.Source(base.gameObject.AddComponent<CriAtomSource>());
		}
		this.SetProxyToCriFs();
	}

	private static bool IsPlayingStatus(CriAtomSource.Status status)
	{
		return status == CriAtomSource.Status.Playing || status == CriAtomSource.Status.Prep;
	}

	private static bool IsPlayingStatus(CriAtomExPlayback.Status status)
	{
		return status == CriAtomExPlayback.Status.Playing || status == CriAtomExPlayback.Status.Prep;
	}

	private static List<SoundManager.Playback> FindPlayingPlayback(string cueName, string cueSheetName)
	{
		List<SoundManager.Playback> list = new List<SoundManager.Playback>();
		SoundManager.Source[] sources = SoundManager.s_instance.m_sources;
		for (int i = 0; i < sources.Length; i++)
		{
			SoundManager.Source source = sources[i];
			foreach (SoundManager.PlayId current in source.playbacks.Keys)
			{
				SoundManager.Playback playback = source.playbacks[current];
				if (SoundManager.IsPlayingStatus(playback.status) && playback.cueName == cueName && playback.cueSheet == cueSheetName)
				{
					list.Add(playback);
				}
			}
		}
		return list;
	}

	public static string[] GetCueSheetNameList()
	{
		if (SoundManager.s_instance == null)
		{
			return new string[0];
		}
		return SoundManager.s_instance.m_cueSheetNameList.ToArray();
	}

	public static bool ExistsCueSheet(string cueSheetName)
	{
		return !(SoundManager.s_instance == null) && SoundManager.s_instance.m_cueSheetNameList.Contains(cueSheetName);
	}

	public static CriAtomEx.CueInfo[] GetCueInfoList(string cueSheetName)
	{
		if (!SoundManager.ExistsCueSheet(cueSheetName))
		{
			return null;
		}
		CriAtomExAcb acb = CriAtom.GetAcb(cueSheetName);
		return (acb == null) ? null : acb.GetCueInfoList();
	}

	public static SoundManager.Source[] GetSourseList()
	{
		if (SoundManager.s_instance == null)
		{
			return new SoundManager.Source[0];
		}
		return SoundManager.s_instance.m_sources;
	}

	private static SoundManager.Source GetSource(SoundManager.SourceId sourceId)
	{
		return SoundManager.s_instance.m_sources[(int)sourceId];
	}

	private static SoundManager.PlayId Play(SoundManager.SourceId sourceId, string cueName, string cueSheetName, bool loopFlag = false)
	{
		SoundManager.Source source = SoundManager.GetSource(sourceId);
		if (sourceId >= SoundManager.SourceId.BGM_NORMAL && sourceId < SoundManager.SourceId.SE)
		{
			source.Stop();
		}
		if (!SoundManager.ExistsCueSheet(cueSheetName))
		{
			global::Debug.LogWarning("CueSheet " + cueSheetName + " not loaded.");
			return SoundManager.PlayId.NONE;
		}
		source.cueSheet = cueSheetName;
		source.cueName = cueName;
		source.loop = loopFlag;
		return source.Play(cueName);
	}

	private static void Change(SoundManager.SourceId sourceId, string cueName, string cueSheetName, bool loopFlag = false)
	{
		SoundManager.Source source = SoundManager.GetSource(sourceId);
		if (SoundManager.IsPlayingStatus(source.status) && source.cueName == cueName)
		{
			return;
		}
		SoundManager.Play(sourceId, cueName, cueSheetName, loopFlag);
	}

	private static void Stop(SoundManager.SourceId sourceId)
	{
		SoundManager.GetSource(sourceId).Stop();
	}

	private static void Stop(SoundManager.SourceId sourceId, SoundManager.PlayId playId)
	{
		SoundManager.GetSource(sourceId).Stop(playId);
	}

	private static void Stop(SoundManager.SourceId sourceId, string cueName, string cueSheetName)
	{
		foreach (SoundManager.Playback current in SoundManager.FindPlayingPlayback(cueName, cueSheetName))
		{
			current.Stop();
		}
	}

	private static void Pause(SoundManager.SourceId sourceId, bool sw)
	{
		SoundManager.GetSource(sourceId).Pause(sw);
	}

	private static void PausePlaying(SoundManager.SourceId sourceId, bool sw)
	{
		SoundManager.Source source = SoundManager.GetSource(sourceId);
		foreach (SoundManager.PlayId current in source.playbacks.Keys)
		{
			SoundManager.Playback playback = source.playbacks[current];
			playback.Pause(sw);
		}
	}

	public static string GetDownloadURL()
	{
		return NetBaseUtil.AssetServerURL + "sound/Android/";
	}

	public static string GetDownloadedDataPath()
	{
		return CriWare.installTargetPath + "/";
	}

	public static void AddCueSheet(string cueSheetName, string acbFile, string awbFile, bool isUrlLoad = false)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		if (SoundManager.ExistsCueSheet(cueSheetName))
		{
			return;
		}
		SoundManager.s_instance.m_cueSheetNameList.Add(cueSheetName);
		string str = (!isUrlLoad) ? "Android/" : SoundManager.GetDownloadedDataPath();
		string acbFile2 = (acbFile == null) ? null : (str + acbFile);
		string awbFile2 = (awbFile == null) ? null : (str + awbFile);
		CriAtomCueSheet criAtomCueSheet = CriAtom.AddCueSheet(cueSheetName, acbFile2, awbFile2, null);
	}

	public static void RemoveCueSheet(string cueSheetName)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.s_instance.m_cueSheetNameList.Remove(cueSheetName);
		CriAtom.RemoveCueSheet(cueSheetName);
	}

	public static void BgmPlay(string cueName, string cueSheetName = "BGM", bool loop = false)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog(string.Concat(new string[]
		{
			"BgmPlay(",
			cueName,
			", ",
			cueSheetName,
			")"
		}));
		SoundManager.Play(SoundManager.SourceId.BGM_NORMAL, cueName, cueSheetName, loop);
	}

	public static void BgmChange(string cueName, string cueSheetName = "BGM")
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog(string.Concat(new string[]
		{
			"BgmChange(",
			cueName,
			", ",
			cueSheetName,
			")"
		}));
		SoundManager.Change(SoundManager.SourceId.BGM_NORMAL, cueName, cueSheetName, true);
	}

	public static void BgmStop()
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog("BgmStop()");
		SoundManager.Stop(SoundManager.SourceId.BGM_NORMAL);
		SoundManager.Stop(SoundManager.SourceId.BGM_CROSSFADE);
	}

	public static void BgmPause(bool sw)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog("BgmPause(" + sw + ")");
		SoundManager.Pause(SoundManager.SourceId.BGM_NORMAL, sw);
		SoundManager.Pause(SoundManager.SourceId.BGM_CROSSFADE, sw);
	}

	public static void BgmFadeOut(float fadeOutTime)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog("BgmFadeOut(" + fadeOutTime + ")");
		SoundManager.GetSource(SoundManager.SourceId.BGM_NORMAL).FadeOutStart(fadeOutTime);
		SoundManager.GetSource(SoundManager.SourceId.BGM_CROSSFADE).FadeOutStart(fadeOutTime);
	}

	public static void BgmCrossFadePlay(string cueName, string cueSheetName = "BGM", float fadeOutTime = 0f)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog(string.Concat(new object[]
		{
			"BgmCrossFadePlay(",
			cueName,
			", ",
			cueSheetName,
			", ",
			fadeOutTime,
			")"
		}));
		SoundManager.GetSource(SoundManager.SourceId.BGM_NORMAL).FadeOffStart(fadeOutTime);
		SoundManager.Play(SoundManager.SourceId.BGM_CROSSFADE, cueName, cueSheetName, true);
	}

	public static void ItemBgmCrossFadePlay(string cueName, string cueSheetName = "BGM", float fadeOutTime = 0f)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog(string.Concat(new object[]
		{
			"BgmCrossFadePlay(",
			cueName,
			", ",
			cueSheetName,
			", ",
			fadeOutTime,
			")"
		}));
		SoundManager.Source source = SoundManager.GetSource(SoundManager.SourceId.BGM_CROSSFADE);
		if (SoundManager.IsPlayingStatus(source.status) && source.cueName == cueName)
		{
			return;
		}
		SoundManager.GetSource(SoundManager.SourceId.BGM_NORMAL).FadeOffStart(fadeOutTime);
		SoundManager.Play(SoundManager.SourceId.BGM_CROSSFADE, cueName, cueSheetName, true);
	}

	public static void BgmCrossFadeStop(float fadeOutTime = 0f, float fadeInTime = 0f)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog(string.Concat(new object[]
		{
			"BgmCrossFadeStop(",
			fadeOutTime,
			", ",
			fadeInTime,
			")"
		}));
		SoundManager.GetSource(SoundManager.SourceId.BGM_CROSSFADE).FadeOutStart(fadeOutTime);
		SoundManager.GetSource(SoundManager.SourceId.BGM_NORMAL).FadeInStart(fadeInTime, -1f);
	}

	public static SoundManager.PlayId SePlay(string cueName, string cueSheetName = "SE")
	{
		if (SoundManager.s_instance == null)
		{
			return SoundManager.PlayId.NONE;
		}
		SoundManager.OutputPlayLog(string.Concat(new string[]
		{
			"SePlay(",
			cueName,
			", ",
			cueSheetName,
			")"
		}));
		return SoundManager.Play(SoundManager.SourceId.SE, cueName, cueSheetName, false);
	}

	public static void SeStop(string cueName, string cueSheetName = "SE")
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog(string.Concat(new string[]
		{
			"SeStop(",
			cueName,
			", ",
			cueSheetName,
			")"
		}));
		SoundManager.Stop(SoundManager.SourceId.SE, cueName, cueSheetName);
	}

	public static void SeStop(SoundManager.PlayId playId)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog("SeStop(" + playId + ")");
		SoundManager.Stop(SoundManager.SourceId.SE, playId);
	}

	public static void SePause(bool sw)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog("SePause(" + sw + ")");
		SoundManager.Pause(SoundManager.SourceId.SE, sw);
	}

	public static void SePausePlaying(bool sw)
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.OutputPlayLog("SePausePlaying(" + sw + ")");
		SoundManager.PausePlaying(SoundManager.SourceId.SE, sw);
	}

	private void SetProxyToCriFs()
	{
		string text;
		ushort num;
		SoundManager.GetSystemProxy(out text, out num);
		if (text != this.proxyHost || num != this.proxyPort)
		{
			CriFsUtility.SetProxyServer(text, num);
			global::Debug.Log(string.Concat(new object[]
			{
				"SetProxyToCriFs: ",
				text,
				":",
				num
			}));
			this.proxyHost = text;
			this.proxyPort = num;
		}
	}

	public static void SetProxyForDownloadData()
	{
		if (SoundManager.s_instance == null)
		{
			return;
		}
		SoundManager.s_instance.SetProxyToCriFs();
	}

	private static void GetSystemProxy(out string host, out ushort port)
	{
		Binding.Instance.GetSystemProxy(out host, out port);
	}

	private static void OutputPlayLog(string s)
	{
		if (SoundManager.s_instance != null && SoundManager.s_instance.m_isOutputPlayLog)
		{
			global::Debug.Log("SoundManager PlayLog: " + s);
		}
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}
}
