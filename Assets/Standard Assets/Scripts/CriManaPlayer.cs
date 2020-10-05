using CriManaPlayerDetail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("CRIWARE/CRI Mana Player")]
public class CriManaPlayer : MonoBehaviour
{
	public enum ShaderType
	{
		Yuv2Rgb,
		ForwardRgb
	}

	public enum AlphaType
	{
		CompoOpaq,
		CompoAlphaFull,
		CompoAlpha3Step,
		CompoAlpha32Bit
	}

	public enum Status
	{
		Stop,
		Dechead,
		WaitPrep,
		Prep,
		Ready,
		Playing,
		PlayEnd,
		Error
	}

	public enum SetMode
	{
		New,
		Append,
		AppendRepeatedly
	}

	public enum AudioTrack
	{
		Off,
		Auto
	}

	public struct AudioInfo
	{
		public uint sampling_rate;

		public uint num_channels;

		public uint total_samples;
	}

	public struct MovieInfo
	{
		public uint is_playable;

		public uint has_alpha;

		public uint width;

		public uint height;

		public uint disp_width;

		public uint disp_height;

		public uint framerate_n;

		public uint framerate_d;

		public uint total_frames;

		public uint num_audio_streams;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public CriManaPlayer.AudioInfo[] audio_prm;

		public uint num_subtitle_channels;
	}

	public struct FrameInfo
	{
		public int frame_no;

		public int frame_no_per_file;

		public uint width;

		public uint height;

		public uint disp_width;

		public uint disp_height;

		public uint framerate_n;

		public uint framerate_d;

		public ulong time;

		public ulong tunit;

		public uint cnt_concatenated_movie;

		private CriManaPlayer.AlphaType alpha_type;

		public uint cnt_skipped_frames;
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void CuePointCbFunc(string eventParamsString);

	private sealed class _CreateTextureByCoroutine_c__Iterator1 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal TextureHolder texture_holder;

		internal int _PC;

		internal object _current;

		internal TextureHolder ___texture_holder;

		internal CriManaPlayer __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = this.__f__this.StartCoroutine(this.texture_holder.CreateTexture());
				this._PC = 1;
				return true;
			case 1u:
				this.__f__this.texture_holder = this.texture_holder;
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _SetFile_c__AnonStorey5
	{
		internal IntPtr binderPtr;

		internal string trueMoviePath;

		internal CriManaPlayer __f__this;

		internal void __m__2()
		{
			CriManaPlugin.criManaUnityPlayer_SetFile(this.__f__this.playerId, this.binderPtr, this.trueMoviePath);
		}
	}

	private sealed class _SetContentId_c__AnonStorey6
	{
		internal int contentId;

		internal CriManaPlayer __f__this;

		internal void __m__3()
		{
			CriManaPlugin.criManaUnityPlayer_SetContentId(this.__f__this.playerId, this.__f__this.binder.nativeHandle, this.contentId);
		}
	}

	private sealed class _SetFileRange_c__AnonStorey7
	{
		internal string filePath;

		internal ulong offset;

		internal long range;

		internal CriManaPlayer __f__this;

		internal void __m__4()
		{
			CriManaPlugin.criManaUnityPlayer_SetFileRange(this.__f__this.playerId, this.filePath, this.offset, this.range);
		}
	}

	public const CriManaPlayer.ShaderType RequiredShaderType = CriManaPlayer.ShaderType.Yuv2Rgb;

	private const int InvalidPlayerId = -1;

	private Action setFileDelegate;

	private int playerId = -1;

	private TextureHolder texture_holder;

	private bool setupOnceFlag;

	private bool prepareRequire;

	private bool playRequire;

	private bool stopRequire;

	private bool gotFirstFrame;

	private bool gotMovieInfo;

	private bool seekRequired;

	private int seekFrameNumber;

	private bool unpauseOnApplicationUnpause;

	private Material originalMaterial;

	private Material createdMaterial;

	[SerializeField]
	private Material _movieMaterial;

	private Shader movieShader;

	private CriFsBinder _binder;

	private Func<bool, bool, Shader> _shaderDispatchFunction = new Func<bool, bool, Shader>(CriManaPlayer.DefaultShaderDispatchFunction);

	private CriManaPlayer.CuePointCbFunc cuePointUserCbFunc;

	private GCHandle cuePointUserCbFuncPinHandle;

	[SerializeField]
	private uint _texNumber = 2u;

	[SerializeField]
	private string _moviePath = string.Empty;

	[SerializeField]
	private bool _playOnStart;

	[SerializeField]
	private bool _loop;

	[SerializeField]
	private float _volume = 1f;

	[SerializeField]
	private float _subAudioVolume = 1f;

	[SerializeField]
	private float _speed = 1f;

	[SerializeField]
	private bool _additiveMode;

	[SerializeField]
	private bool _flipTopBottom = true;

	[SerializeField]
	private bool _flipLeftRight = true;

	private CriManaPlayer.MovieInfo _movieInfo;

	private CriManaPlayer.FrameInfo _frameInfo;

	private bool _frameUpdated_k__BackingField;

	public Func<bool, bool, Shader> shaderDispatchFunction
	{
		get
		{
			return this._shaderDispatchFunction;
		}
		set
		{
			this._shaderDispatchFunction = value;
		}
	}

	[Obsolete("This property is obsolete; use shaderDispatchFunction instead", false)]
	public Func<bool, bool, Shader> shaderDispachFunction
	{
		get
		{
			return this.shaderDispatchFunction;
		}
		set
		{
			this.shaderDispatchFunction = value;
		}
	}

	public uint texNumber
	{
		get
		{
			return this._texNumber;
		}
		set
		{
			this._texNumber = value;
		}
	}

	public CriFsBinder binder
	{
		get
		{
			return this._binder;
		}
		private set
		{
			this._binder = value;
		}
	}

	public string moviePath
	{
		get
		{
			return this._moviePath;
		}
		set
		{
			this._moviePath = value;
		}
	}

	public bool playOnStart
	{
		get
		{
			return this._playOnStart;
		}
		set
		{
			this._playOnStart = value;
		}
	}

	public bool loop
	{
		get
		{
			return this._loop;
		}
		set
		{
			this._loop = value;
			if (this.playerId != -1)
			{
				CriManaPlugin.criManaUnityPlayer_Loop(this.playerId, (!this._loop) ? 0 : 1);
			}
		}
	}

	public float volume
	{
		get
		{
			return this._volume;
		}
		set
		{
			this._volume = value;
			if (this.playerId != -1)
			{
				CriManaPlugin.criManaUnityPlayer_SetVolume(this.playerId, this._volume);
			}
		}
	}

	public float subAudioVolume
	{
		get
		{
			return this._subAudioVolume;
		}
		set
		{
			this._subAudioVolume = value;
			if (this.playerId != -1)
			{
				CriManaPlugin.criManaUnityPlayer_SetSubAudioVolume(this.playerId, this._subAudioVolume);
			}
		}
	}

	public float speed
	{
		get
		{
			return this._speed;
		}
		set
		{
			if (value < 0f)
			{
				return;
			}
			this._speed = value;
			if (this.playerId != -1)
			{
				CriManaPlugin.criManaUnityPlayer_SetSpeed(this.playerId, this._speed);
			}
		}
	}

	public Material movieMaterial
	{
		get
		{
			return this._movieMaterial;
		}
		set
		{
			this._movieMaterial = value;
		}
	}

	public bool flipTopBottom
	{
		get
		{
			return this._flipTopBottom;
		}
		set
		{
			this._flipTopBottom = value;
			this.SetTextureConfigIfTextureHolderIsNotNull();
		}
	}

	public bool flipLeftRight
	{
		get
		{
			return this._flipLeftRight;
		}
		set
		{
			this._flipLeftRight = value;
			this.SetTextureConfigIfTextureHolderIsNotNull();
		}
	}

	public bool additiveMode
	{
		get
		{
			return this._additiveMode;
		}
		set
		{
			this._additiveMode = value;
		}
	}

	public CriManaPlayer.MovieInfo movieInfo
	{
		get
		{
			return this._movieInfo;
		}
	}

	public CriManaPlayer.FrameInfo frameInfo
	{
		get
		{
			return this._frameInfo;
		}
		private set
		{
			this._frameInfo = value;
		}
	}

	public uint width
	{
		get
		{
			return this.movieInfo.disp_width;
		}
	}

	public uint height
	{
		get
		{
			return this.movieInfo.disp_height;
		}
	}

	public CriManaPlayer.Status status
	{
		get
		{
			if (this.playerId == -1)
			{
				return CriManaPlayer.Status.Stop;
			}
			CriManaPlayer.Status status = (CriManaPlayer.Status)CriManaPlugin.criManaUnityPlayer_GetStatus(this.playerId);
			if (status == CriManaPlayer.Status.Ready && this.texture_holder == null)
			{
				return CriManaPlayer.Status.Prep;
			}
			return status;
		}
	}

	public int numberOfEntries
	{
		get
		{
			return (this.playerId == -1) ? (-1) : CriManaPlugin.criManaUnityPlayer_GetNumberOfEntry(this.playerId);
		}
	}

	public bool frameUpdated
	{
		get;
		private set;
	}

	public static Shader DefaultShaderDispatchFunction(bool alpha, bool additive)
	{
		Shader shader;
		if (alpha)
		{
			shader = Shader.Find((!additive) ? "CriMana/Yuva2Rgba" : "CriMana/Yuva2RgbaAdditive");
		}
		else
		{
			shader = Shader.Find((!additive) ? "CriMana/Yuv2Rgb" : "CriMana/Yuv2RgbAdditive");
		}
		if (shader == null)
		{
			UnityEngine.Debug.LogError("Can't find CriMana Sharder. Probably, the link from a material to shader has broken. Please reimport 'CRIWARE SDK for Unity'.");
			shader = Shader.Find("Diffuse");
		}
		return shader;
	}

	public bool SetFile(CriFsBinder argBinder, string argMoviePath, CriManaPlayer.SetMode setMode = CriManaPlayer.SetMode.New)
	{
		IntPtr binderPtr = (argBinder != null) ? argBinder.nativeHandle : IntPtr.Zero;
		string trueMoviePath;
		if (argBinder != null)
		{
			trueMoviePath = argMoviePath;
		}
		else
		{
			trueMoviePath = ((!CriWare.IsStreamingAssetsPath(argMoviePath)) ? argMoviePath : Path.Combine(CriWare.streamingAssetsPath, argMoviePath));
		}
		if (setMode == CriManaPlayer.SetMode.New)
		{
			CriManaPlugin.criManaUnityPlayer_ClearEntry(this.playerId);
			this.binder = argBinder;
			this.moviePath = argMoviePath;
			this.setFileDelegate = delegate
			{
				CriManaPlugin.criManaUnityPlayer_SetFile(this.playerId, binderPtr, trueMoviePath);
			};
			return true;
		}
		bool repeat = setMode == CriManaPlayer.SetMode.AppendRepeatedly;
		return CriManaPlugin.criManaUnityPlayer_EntryFile(this.playerId, binderPtr, trueMoviePath, repeat);
	}

	public bool SetContentId(CriFsBinder argBinder, int contentId, CriManaPlayer.SetMode setMode = CriManaPlayer.SetMode.New)
	{
		if (argBinder == null)
		{
			UnityEngine.Debug.LogError("[CRIWARE] CriFsBinder is null");
		}
		if (setMode == CriManaPlayer.SetMode.New)
		{
			CriManaPlugin.criManaUnityPlayer_ClearEntry(this.playerId);
			this.binder = argBinder;
			this.moviePath = string.Empty;
			this.setFileDelegate = delegate
			{
				CriManaPlugin.criManaUnityPlayer_SetContentId(this.playerId, this.binder.nativeHandle, contentId);
			};
			return true;
		}
		bool repeat = setMode == CriManaPlayer.SetMode.AppendRepeatedly;
		return CriManaPlugin.criManaUnityPlayer_EntryContentId(this.playerId, argBinder.nativeHandle, contentId, repeat);
	}

	public bool SetFileRange(string filePath, ulong offset, long range, CriManaPlayer.SetMode setMode = CriManaPlayer.SetMode.New)
	{
		if (setMode == CriManaPlayer.SetMode.New)
		{
			CriManaPlugin.criManaUnityPlayer_ClearEntry(this.playerId);
			this.binder = null;
			this.moviePath = filePath;
			this.setFileDelegate = delegate
			{
				CriManaPlugin.criManaUnityPlayer_SetFileRange(this.playerId, filePath, offset, range);
			};
			return true;
		}
		bool repeat = setMode == CriManaPlayer.SetMode.AppendRepeatedly;
		return CriManaPlugin.criManaUnityPlayer_EntryFileRange(this.playerId, filePath, offset, range, repeat);
	}

	public void Prepare()
	{
		this.SetupCallOnce();
		this.prepareRequire = true;
		this.UpdatePlayer();
	}

	public void Play()
	{
		this.SetupCallOnce();
		this.gotFirstFrame = false;
		this.playRequire = true;
		this.UpdatePlayer();
	}

	public void Stop()
	{
		if (this.originalMaterial != null && base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().material = this.originalMaterial;
		}
		CriManaPlugin.criManaUnityPlayer_Stop(this.playerId);
		this.prepareRequire = false;
		this.playRequire = false;
		this.gotFirstFrame = false;
		this.stopRequire = true;
	}

	public void Pause(bool sw)
	{
		if (this.playerId != -1)
		{
			CriManaPlugin.criManaUnityPlayer_Pause(this.playerId, (!sw) ? 0 : 1);
		}
	}

	public bool IsPaused()
	{
		return CriManaPlugin.criManaUnityPlayer_IsPaused(this.playerId);
	}

	public long GetTime()
	{
		if (this.playerId != -1)
		{
			return CriManaPlugin.criManaUnityPlayer_GetTime(this.playerId);
		}
		return 0L;
	}

	public uint GetTotalFrames()
	{
		if (this.playerId != -1)
		{
			return this.movieInfo.total_frames;
		}
		return 0u;
	}

	public uint GetFramerate()
	{
		if (this.playerId != -1)
		{
			return this.movieInfo.framerate_n;
		}
		return 0u;
	}

	public CriManaPlayer.Status GetStatus()
	{
		return this.status;
	}

	public void SetCuePointCallback(CriManaPlayer.CuePointCbFunc func)
	{
		if (func != null)
		{
			this.cuePointUserCbFunc = func;
			IntPtr cbfunc = IntPtr.Zero;
			if (this.cuePointUserCbFuncPinHandle.IsAllocated)
			{
				this.cuePointUserCbFuncPinHandle.Free();
			}
			CriManaPlayer.CuePointCbFunc cuePointCbFunc = new CriManaPlayer.CuePointCbFunc(this.CuePointCallbackFromNative);
			this.cuePointUserCbFuncPinHandle = GCHandle.Alloc(cuePointCbFunc, GCHandleType.Pinned);
			cbfunc = Marshal.GetFunctionPointerForDelegate(cuePointCbFunc);
			CriManaPlugin.criManaUnityPlayer_SetCuePointCallback(this.playerId, cbfunc, base.gameObject.name, "CuePointCallbackFromNative");
		}
	}

	public void SetSeekPosition(int argSeekFrameNumber)
	{
		this.seekFrameNumber = argSeekFrameNumber;
		this.seekRequired = true;
	}

	public void SetDeviceSendLevel(int deviceId, float level)
	{
	}

	public void SetAudioTrack(int track)
	{
		if (this.playerId != -1)
		{
			CriManaPlugin.criManaUnityPlayer_SetAudioTrack(this.playerId, track);
		}
	}

	public void SetAudioTrack(CriManaPlayer.AudioTrack track)
	{
		if (this.playerId != -1)
		{
			if (track == CriManaPlayer.AudioTrack.Off)
			{
				CriManaPlugin.criManaUnityPlayer_SetAudioTrack(this.playerId, -1);
			}
			else if (track == CriManaPlayer.AudioTrack.Auto)
			{
				CriManaPlugin.criManaUnityPlayer_SetAudioTrack(this.playerId, 100);
			}
		}
	}

	public void SetSubAudioTrack(int track)
	{
		if (this.playerId != -1)
		{
			CriManaPlugin.criManaUnityPlayer_SetSubAudioTrack(this.playerId, track);
		}
	}

	public void SetSubAudioTrack(CriManaPlayer.AudioTrack track)
	{
		if (this.playerId != -1)
		{
			if (track == CriManaPlayer.AudioTrack.Off)
			{
				CriManaPlugin.criManaUnityPlayer_SetSubAudioTrack(this.playerId, -1);
			}
			else if (track == CriManaPlayer.AudioTrack.Auto)
			{
				CriManaPlugin.criManaUnityPlayer_SetSubAudioTrack(this.playerId, 100);
			}
		}
	}

	public void CreateTexture(int argWidth, int argHeight, bool alphaMode, bool coroutineMode)
	{
		if (this.texture_holder != null && !this.texture_holder.IsAvailable(argWidth, argHeight, this.texNumber, alphaMode))
		{
			this.DestroyTexture();
		}
		if (this.texture_holder == null)
		{
			IEnumerator enumerator = this.CreateTextureByCoroutine(TextureHolder.Create(argWidth, argHeight, this.texNumber, alphaMode));
			if (coroutineMode)
			{
				base.StartCoroutine(enumerator);
			}
			else
			{
				while (enumerator.MoveNext())
				{
				}
			}
		}
	}

	public void DestroyTexture()
	{
		if (this.texture_holder != null)
		{
			this.texture_holder.DestroyTexture();
			this.texture_holder = null;
		}
	}

	private void Awake()
	{
		CriManaPlugin.InitializeLibrary();
		this.playerId = CriManaPlugin.criManaUnityPlayer_Create();
		this.frameUpdated = false;
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
		this.SetupCallOnce();
		if (this.playOnStart)
		{
			this.Play();
		}
	}

	private void OnDisable()
	{
		this.Stop();
	}

	private void OnDestroy()
	{
		this.movieMaterial = null;
		this.originalMaterial = null;
		if (this.createdMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(this.createdMaterial);
		}
		this.createdMaterial = null;
		this.DestroyTexture();
		if (this.playerId != -1)
		{
			CriManaPlugin.criManaUnityPlayer_Destroy(this.playerId);
			this.playerId = -1;
		}
		if (this.cuePointUserCbFuncPinHandle.IsAllocated)
		{
			this.cuePointUserCbFuncPinHandle.Free();
		}
		CriManaPlugin.FinalizeLibrary();
	}

	private void OnApplicationPause(bool appPause)
	{
		if (appPause)
		{
			this.unpauseOnApplicationUnpause = !this.IsPaused();
			if (this.unpauseOnApplicationUnpause)
			{
				this.Pause(true);
			}
		}
		else
		{
			if (this.unpauseOnApplicationUnpause)
			{
				this.Pause(false);
			}
			this.unpauseOnApplicationUnpause = false;
		}
	}

	public void Update()
	{
		this.UpdatePlayer();
	}

	public void OnDrawGizmos()
	{
		if (this.status == CriManaPlayer.Status.Playing)
		{
			Gizmos.color = new Color(1f, 1f, 1f, 0.8f);
		}
		else
		{
			Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
		}
		Gizmos.DrawIcon(base.transform.position, "CriWare/film.png");
		Gizmos.DrawLine(base.transform.position, new Vector3(0f, 0f, 0f));
	}

	private void SetupCallOnce()
	{
		if (this.setupOnceFlag)
		{
			return;
		}
		if (this.movieMaterial == null)
		{
			if (base.GetComponent<Renderer>() != null)
			{
				this.originalMaterial = base.GetComponent<Renderer>().sharedMaterial;
			}
			Shader shader = Shader.Find("CriMana/ForwardRgb");
			if (shader == null)
			{
				UnityEngine.Debug.LogError("Can't find CriMana Sharder. Probably, the link from a material to shader has broken. Please reimport 'CRIWARE SDK for Unity'.");
				shader = Shader.Find("Diffuse");
			}
			this.createdMaterial = new Material(shader);
			this.movieMaterial = this.createdMaterial;
		}
		if (this.setFileDelegate == null)
		{
			this.setFileDelegate = delegate
			{
				string path = (!CriWare.IsStreamingAssetsPath(this.moviePath)) ? this.moviePath : Path.Combine(CriWare.streamingAssetsPath, this.moviePath);
				CriManaPlugin.criManaUnityPlayer_SetFile(this.playerId, IntPtr.Zero, path);
			};
		}
		this.frameUpdated = false;
		this.loop = this._loop;
		this.volume = this._volume;
		this.subAudioVolume = this._subAudioVolume;
		this.playRequire = this._playOnStart;
		this.setupOnceFlag = true;
	}

	private IEnumerator CreateTextureByCoroutine(TextureHolder texture_holder)
	{
		CriManaPlayer._CreateTextureByCoroutine_c__Iterator1 _CreateTextureByCoroutine_c__Iterator = new CriManaPlayer._CreateTextureByCoroutine_c__Iterator1();
		_CreateTextureByCoroutine_c__Iterator.texture_holder = texture_holder;
		_CreateTextureByCoroutine_c__Iterator.___texture_holder = texture_holder;
		_CreateTextureByCoroutine_c__Iterator.__f__this = this;
		return _CreateTextureByCoroutine_c__Iterator;
	}

	private void GetMovieInfoAndCreateTextureIfNotYet()
	{
		if (!this.gotMovieInfo)
		{
			CriManaPlugin.criManaUnityPlayer_GetMovieInfo(this.playerId, out this._movieInfo);
			this.movieShader = this.shaderDispatchFunction(this.movieInfo.has_alpha == 1u, this.additiveMode);
			this.CreateTexture((int)this.movieInfo.disp_width, (int)this.movieInfo.disp_height, this.movieInfo.has_alpha == 1u, true);
			this.gotMovieInfo = true;
		}
	}

	private void UpdatePlayer()
	{
		this.frameUpdated = false;
		if (this.playerId == -1)
		{
			return;
		}
		CriManaPlugin.criManaUnityPlayer_Update(this.playerId);
		switch (CriManaPlugin.criManaUnityPlayer_GetStatus(this.playerId))
		{
		case 0:
			break;
		case 1:
		case 2:
			return;
		case 3:
			if (!this.stopRequire)
			{
				this.GetMovieInfoAndCreateTextureIfNotYet();
			}
			return;
		case 4:
			if (!this.stopRequire)
			{
				this.GetMovieInfoAndCreateTextureIfNotYet();
				if (this.texture_holder != null && this.playRequire)
				{
					CriManaPlugin.criManaUnityPlayer_Start(this.playerId);
					CriManaPlugin.criManaUnityPlayer_Update(this.playerId);
					this.playRequire = false;
					this.UpdateFrame();
				}
			}
			return;
		case 5:
			if (!this.stopRequire)
			{
				this.UpdateFrame();
			}
			return;
		case 6:
			if (this.numberOfEntries > 0)
			{
				CriManaPlugin.criManaUnityPlayer_ClearEntry(this.playerId);
			}
			break;
		default:
			return;
		}
		if (this.playRequire || this.prepareRequire)
		{
			this.setFileDelegate();
			if (this.seekRequired)
			{
				CriManaPlugin.criManaUnityPlayer_SetSeekPosition(this.playerId, this.seekFrameNumber);
			}
			CriManaPlugin.criManaUnityPlayer_Prepare(this.playerId);
			CriManaPlugin.criManaUnityPlayer_Update(this.playerId);
			this.seekRequired = false;
			this.stopRequire = false;
			this.gotMovieInfo = false;
			this.frameUpdated = false;
			this.gotFirstFrame = false;
			this.prepareRequire = false;
		}
	}

	private void UpdateFrame()
	{
		if (this.gotFirstFrame)
		{
			this.frameUpdated = this.texture_holder.UpdateTexture(this.movieMaterial, this.playerId, out this._frameInfo);
		}
		else
		{
			Shader shader = this.movieMaterial.shader;
			this.movieMaterial.shader = this.movieShader;
			this.frameUpdated = this.texture_holder.UpdateTexture(this.movieMaterial, this.playerId, out this._frameInfo);
			if (this.frameUpdated)
			{
				if (this.originalMaterial != null && base.GetComponent<Renderer>() != null)
				{
					base.GetComponent<Renderer>().material = this.movieMaterial;
				}
				this.SetTextureConfigIfTextureHolderIsNotNull();
				this.movieShader = null;
				this.gotFirstFrame = true;
			}
			else
			{
				this.movieMaterial.shader = shader;
			}
		}
	}

	private void SetTextureConfigIfTextureHolderIsNotNull()
	{
		if (this.texture_holder != null)
		{
			this.texture_holder.SetTextureConfig(this.movieMaterial, (int)this.movieInfo.disp_width, (int)this.movieInfo.disp_height, this.flipTopBottom, this.flipLeftRight);
		}
	}

	private void CuePointCallbackFromNative(string eventString)
	{
		if (this.cuePointUserCbFunc != null)
		{
			this.cuePointUserCbFunc(eventString);
		}
	}
}
