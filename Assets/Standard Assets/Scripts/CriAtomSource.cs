using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("CRIWARE/CRI Atom Source")]
public class CriAtomSource : MonoBehaviour
{
	public enum Status
	{
		Stop,
		Prep,
		Playing,
		PlayEnd,
		Error
	}

	private CriAtomEx3dSource source;

	private Vector3 lastPosition;

	[SerializeField]
	private bool _playOnStart;

	[SerializeField]
	private string _cueName = string.Empty;

	[SerializeField]
	private string _cueSheet = string.Empty;

	[SerializeField]
	private bool _use3dPositioning = true;

	[SerializeField]
	private bool _loop;

	[SerializeField]
	private float _volume = 1f;

	[SerializeField]
	private float _pitch;

	[SerializeField]
	private bool _androidUseLowLatencyVoicePool;

	[SerializeField]
	private bool need_to_player_update_all = true;

	private CriAtomExPlayer _player_k__BackingField;

	public CriAtomExPlayer player
	{
		get;
		private set;
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

	public string cueName
	{
		get
		{
			return this._cueName;
		}
		set
		{
			this._cueName = value;
		}
	}

	public string cueSheet
	{
		get
		{
			return this._cueSheet;
		}
		set
		{
			this._cueSheet = value;
		}
	}

	public bool use3dPositioning
	{
		get
		{
			return this._use3dPositioning;
		}
		set
		{
			this._use3dPositioning = value;
			if (this.player != null)
			{
				this.player.Set3dSource((!this.use3dPositioning) ? null : this.source);
				this.SetNeedToPlayerUpdateAll();
			}
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
			if (this.player != null)
			{
				this.player.SetVolume(this._volume);
				this.SetNeedToPlayerUpdateAll();
			}
		}
	}

	public float pitch
	{
		get
		{
			return this._pitch;
		}
		set
		{
			this._pitch = value;
			if (this.player != null)
			{
				this.player.SetPitch(this._pitch);
				this.SetNeedToPlayerUpdateAll();
			}
		}
	}

	public float pan3dAngle
	{
		get
		{
			return this.player.GetParameterFloat32(CriAtomEx.Parameter.Pan3dAngle);
		}
		set
		{
			this.player.SetPan3dAngle(value);
			this.SetNeedToPlayerUpdateAll();
		}
	}

	public float pan3dDistance
	{
		get
		{
			return this.player.GetParameterFloat32(CriAtomEx.Parameter.Pan3dDistance);
		}
		set
		{
			this.player.SetPan3dInteriorDistance(value);
			this.SetNeedToPlayerUpdateAll();
		}
	}

	public int startTime
	{
		get
		{
			return this.player.GetParameterSint32(CriAtomEx.Parameter.StartTime);
		}
		set
		{
			this.player.SetStartTime((long)value);
			this.SetNeedToPlayerUpdateAll();
		}
	}

	public long time
	{
		get
		{
			return (this.player == null) ? 0L : this.player.GetTime();
		}
	}

	public CriAtomSource.Status status
	{
		get
		{
			return (CriAtomSource.Status)((this.player == null) ? CriAtomExPlayer.Status.Error : this.player.GetStatus());
		}
	}

	public bool androidUseLowLatencyVoicePool
	{
		get
		{
			return this._androidUseLowLatencyVoicePool;
		}
		set
		{
			this._androidUseLowLatencyVoicePool = value;
		}
	}

	protected void SetNeedToPlayerUpdateAll()
	{
		this.need_to_player_update_all = true;
	}

	protected virtual void InternalInitialize()
	{
		CriAtomPlugin.InitializeLibrary();
		this.player = new CriAtomExPlayer();
		this.source = new CriAtomEx3dSource();
	}

	protected virtual void InternalFinalize()
	{
		this.player.Dispose();
		this.player = null;
		this.source.Dispose();
		this.source = null;
		CriAtomPlugin.FinalizeLibrary();
	}

	private void Awake()
	{
		this.InternalInitialize();
	}

	private void OnEnable()
	{
		this.SetInitialParameters();
		this.SetNeedToPlayerUpdateAll();
	}

	private void OnDestroy()
	{
		this.InternalFinalize();
	}

	protected virtual void SetInitialParameters()
	{
		this.use3dPositioning = this.use3dPositioning;
		CriAtomListener instance = CriAtomListener.instance;
		if (instance != null)
		{
			this.player.Set3dListener(instance.internalListener);
		}
		this.lastPosition = base.transform.position;
		this.source.SetPosition(this.lastPosition.x, this.lastPosition.y, this.lastPosition.z);
		this.source.Update();
		this.player.SetVolume(this._volume);
		this.player.SetPitch(this._pitch);
	}

	private void Start()
	{
		if (this.playOnStart)
		{
			this.Play();
		}
	}

	private void LateUpdate()
	{
		Vector3 position = base.transform.position;
		Vector3 vector = (position - this.lastPosition) / Time.deltaTime;
		this.lastPosition = position;
		this.source.SetPosition(position.x, position.y, position.z);
		this.source.SetVelocity(vector.x, vector.y, vector.z);
		this.source.Update();
		if (this.need_to_player_update_all)
		{
			this.player.UpdateAll();
			this.need_to_player_update_all = false;
		}
	}

	public void OnDrawGizmos()
	{
		if (Application.isPlaying && this.status == CriAtomSource.Status.Playing)
		{
			Gizmos.DrawIcon(base.transform.position, "Criware/VoiceOn.png");
		}
		else
		{
			Gizmos.DrawIcon(base.transform.position, "Criware/VoiceOff.png");
		}
	}

	public CriAtomExPlayback Play()
	{
		return this.Play(this.cueName);
	}

	public CriAtomExPlayback Play(string cueName)
	{
		CriAtomExAcb acb = null;
		if (!string.IsNullOrEmpty(this.cueSheet))
		{
			acb = CriAtom.GetAcb(this.cueSheet);
		}
		this.player.SetCue(acb, cueName);
		if (this.androidUseLowLatencyVoicePool)
		{
			this.player.SetSoundRendererType(CriAtomEx.SoundRendererType.Native);
		}
		else
		{
			this.player.SetSoundRendererType(CriAtomEx.SoundRendererType.Asr);
		}
		if (this.status == CriAtomSource.Status.Stop)
		{
			this.player.Loop(this._loop);
		}
		return this.player.Start();
	}

	public CriAtomExPlayback Play(int cueId)
	{
		CriAtomExAcb acb = null;
		if (!string.IsNullOrEmpty(this.cueSheet))
		{
			acb = CriAtom.GetAcb(this.cueSheet);
		}
		this.player.SetCue(acb, cueId);
		if (this.androidUseLowLatencyVoicePool)
		{
			this.player.SetSoundRendererType(CriAtomEx.SoundRendererType.Native);
		}
		else
		{
			this.player.SetSoundRendererType(CriAtomEx.SoundRendererType.Asr);
		}
		if (this.status == CriAtomSource.Status.Stop)
		{
			this.player.Loop(this._loop);
		}
		return this.player.Start();
	}

	public void Stop()
	{
		this.player.Stop();
	}

	public void Pause(bool sw)
	{
		if (!sw)
		{
			this.player.Resume(CriAtomEx.ResumeMode.PausedPlayback);
		}
		else
		{
			this.player.Pause();
		}
	}

	public bool IsPaused()
	{
		return this.player.IsPaused();
	}

	public void SetBusSendLevel(int busId, float level)
	{
		if (this.player != null)
		{
			this.player.SetBusSendLevel(busId, level);
			this.SetNeedToPlayerUpdateAll();
		}
	}

	public void SetBusSendLevelOffset(int busId, float levelOffset)
	{
		if (this.player != null)
		{
			this.player.SetBusSendLevelOffset(busId, levelOffset);
			this.SetNeedToPlayerUpdateAll();
		}
	}

	public void SetDeviceSendLevel(int deviceId, float level)
	{
	}

	public void SetAisac(string controlName, float value)
	{
		if (this.player != null)
		{
			this.player.SetAisac(controlName, value);
			this.SetNeedToPlayerUpdateAll();
		}
	}

	public void SetAisac(uint controlId, float value)
	{
		if (this.player != null)
		{
			this.player.SetAisac(controlId, value);
			this.SetNeedToPlayerUpdateAll();
		}
	}

	public void AttachToAnalyzer(CriAtomExPlayerOutputAnalyzer analyzer)
	{
		if (this.player != null)
		{
			analyzer.AttachExPlayer(this.player);
		}
	}

	public void DetachFromAnalyzer(CriAtomExPlayerOutputAnalyzer analyzer)
	{
		analyzer.DetachExPlayer();
	}
}
