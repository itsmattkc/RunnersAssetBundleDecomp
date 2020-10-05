using System;
using System.Runtime.InteropServices;

public class CriAtomExPlayer : IDisposable
{
	public enum Status
	{
		Stop,
		Prep,
		Playing,
		PlayEnd,
		Error
	}

	private struct Config
	{
		public CriAtomEx.VoiceAllocationMethod voiceAllocationMethod;

		public int maxPathStrings;

		public int maxPath;

		public bool updatesTime;
	}

	private IntPtr handle = IntPtr.Zero;

	public IntPtr nativeHandle
	{
		get
		{
			return this.handle;
		}
	}

	public CriAtomExPlayer() : this(0, 0)
	{
	}

	public CriAtomExPlayer(int maxPath, int maxPathStrings)
	{
		CriAtomPlugin.InitializeLibrary();
		CriAtomExPlayer.Config config;
		config.voiceAllocationMethod = CriAtomEx.VoiceAllocationMethod.Once;
		config.maxPath = maxPath;
		config.maxPathStrings = maxPathStrings;
		config.updatesTime = true;
		this.handle = CriAtomExPlayer.criAtomExPlayer_Create(ref config, IntPtr.Zero, 0);
	}

	public void Dispose()
	{
		CriAtomExPlayer.criAtomExPlayer_Destroy(this.handle);
		CriAtomPlugin.FinalizeLibrary();
		GC.SuppressFinalize(this);
	}

	public void SetCue(CriAtomExAcb acb, string name)
	{
		CriAtomExPlayer.criAtomExPlayer_SetCueName(this.handle, (acb == null) ? IntPtr.Zero : acb.nativeHandle, name);
	}

	public void SetCue(CriAtomExAcb acb, int id)
	{
		CriAtomExPlayer.criAtomExPlayer_SetCueId(this.handle, (acb == null) ? IntPtr.Zero : acb.nativeHandle, id);
	}

	public void SetCueIndex(CriAtomExAcb acb, int index)
	{
		CriAtomExPlayer.criAtomExPlayer_SetCueIndex(this.handle, (acb == null) ? IntPtr.Zero : acb.nativeHandle, index);
	}

	public void SetContentId(CriFsBinder binder, int contentId)
	{
		CriAtomExPlayer.criAtomExPlayer_SetContentId(this.handle, (binder == null) ? IntPtr.Zero : binder.nativeHandle, contentId);
	}

	public void SetFile(CriFsBinder binder, string path)
	{
		CriAtomExPlayer.criAtomExPlayer_SetFile(this.handle, (binder == null) ? IntPtr.Zero : binder.nativeHandle, path);
	}

	public void SetFormat(CriAtomEx.Format format)
	{
		CriAtomExPlayer.criAtomExPlayer_SetFormat(this.handle, format);
	}

	public void SetNumChannels(int numChannels)
	{
		CriAtomExPlayer.criAtomExPlayer_SetNumChannels(this.handle, numChannels);
	}

	public void SetSamplingRate(int samplingRate)
	{
		CriAtomExPlayer.criAtomExPlayer_SetSamplingRate(this.handle, samplingRate);
	}

	public CriAtomExPlayback Start()
	{
		return new CriAtomExPlayback(CriAtomExPlayer.criAtomExPlayer_Start(this.handle));
	}

	public CriAtomExPlayback Prepare()
	{
		return new CriAtomExPlayback(CriAtomExPlayer.criAtomExPlayer_Prepare(this.handle));
	}

	public void Stop(bool ignoresReleaseTime)
	{
		if (!ignoresReleaseTime)
		{
			CriAtomExPlayer.criAtomExPlayer_Stop(this.handle);
		}
		else
		{
			CriAtomExPlayer.criAtomExPlayer_StopWithoutReleaseTime(this.handle);
		}
	}

	public void Pause()
	{
		CriAtomExPlayer.criAtomExPlayer_Pause(this.handle, true);
	}

	public void Resume(CriAtomEx.ResumeMode mode)
	{
		CriAtomExPlayer.criAtomExPlayer_Resume(this.handle, mode);
	}

	public bool IsPaused()
	{
		return CriAtomExPlayer.criAtomExPlayer_IsPaused(this.handle);
	}

	public void SetVolume(float volume)
	{
		CriAtomExPlayer.criAtomExPlayer_SetVolume(this.handle, volume);
	}

	public void SetPitch(float pitch)
	{
		CriAtomExPlayer.criAtomExPlayer_SetPitch(this.handle, pitch);
	}

	public void SetPan3dAngle(float angle)
	{
		CriAtomExPlayer.criAtomExPlayer_SetPan3dAngle(this.handle, angle);
	}

	public void SetPan3dInteriorDistance(float distance)
	{
		CriAtomExPlayer.criAtomExPlayer_SetPan3dInteriorDistance(this.handle, distance);
	}

	public void SetPan3dVolume(float volume)
	{
		CriAtomExPlayer.criAtomExPlayer_SetPan3dVolume(this.handle, volume);
	}

	public void SetPanType(CriAtomEx.PanType panType)
	{
		CriAtomExPlayer.criAtomExPlayer_SetPanType(this.handle, panType);
	}

	public void SetSendLevel(int channel, CriAtomEx.Speaker id, float level)
	{
		CriAtomExPlayer.criAtomExPlayer_SetSendLevel(this.handle, channel, id, level);
	}

	public void SetBiquadFilterParameters(CriAtomEx.BiquadFilterType type, float frequency, float gain, float q)
	{
		CriAtomExPlayer.criAtomExPlayer_SetBiquadFilterParameters(this.handle, type, frequency, gain, q);
	}

	public void SetBandpassFilterParameters(float cofLow, float cofHigh)
	{
		CriAtomExPlayer.criAtomExPlayer_SetBandpassFilterParameters(this.handle, cofLow, cofHigh);
	}

	public void SetBusSendLevel(int busId, float level)
	{
		CriAtomExPlayer.criAtomExPlayer_SetBusSendLevel(this.handle, busId, level);
	}

	public void SetBusSendLevelOffset(int busId, float levelOffset)
	{
		CriAtomExPlayer.criAtomExPlayer_SetBusSendLevelOffset(this.handle, busId, levelOffset);
	}

	public void SetDeviceSendLevel(int deviceId, float level)
	{
		CriAtomExPlayer.criAtomExPlayer_SetDeviceSendLevel(this.handle, deviceId, level);
	}

	public void SetAisac(string controlName, float value)
	{
		CriAtomExPlayer.criAtomExPlayer_SetAisacControlByName(this.handle, controlName, value);
	}

	public void SetAisac(uint controlId, float value)
	{
		CriAtomExPlayer.criAtomExPlayer_SetAisacControlById(this.handle, (ushort)controlId, value);
	}

	public void Set3dSource(CriAtomEx3dSource source)
	{
		CriAtomExPlayer.criAtomExPlayer_Set3dSourceHn(this.handle, (source != null) ? source.nativeHandle : IntPtr.Zero);
	}

	public void Set3dListener(CriAtomEx3dListener listener)
	{
		CriAtomExPlayer.criAtomExPlayer_Set3dListenerHn(this.handle, (listener != null) ? listener.nativeHandle : IntPtr.Zero);
	}

	public void SetStartTime(long startTimeMs)
	{
		CriAtomExPlayer.criAtomExPlayer_SetStartTime(this.handle, startTimeMs);
	}

	public void SetFirstBlockIndex(int index)
	{
		CriAtomExPlayer.criAtomExPlayer_SetFirstBlockIndex(this.handle, index);
	}

	public void SetSelectorLabel(string selector, string label)
	{
		CriAtomExPlayer.criAtomExPlayer_SetSelectorLabel(this.handle, selector, label);
	}

	public void SetCategory(int categoryId)
	{
		CriAtomExPlayer.criAtomExPlayer_SetCategoryById(this.handle, (uint)categoryId);
	}

	public void SetCategory(string categoryName)
	{
		CriAtomExPlayer.criAtomExPlayer_SetCategoryByName(this.handle, categoryName);
	}

	public void UnsetCategory()
	{
		CriAtomExPlayer.criAtomExPlayer_UnsetCategory(this.handle);
	}

	public void SetCuePriority(int priority)
	{
		CriAtomExPlayer.criAtomExPlayer_SetCuePriority(this.handle, priority);
	}

	public void SetVoicePriority(int priority)
	{
		CriAtomExPlayer.criAtomExPlayer_SetVoicePriority(this.handle, priority);
	}

	public void SetVoiceControlMethod(CriAtomEx.VoiceControlMethod method)
	{
		CriAtomExPlayer.criAtomExPlayer_SetVoiceControlMethod(this.handle, method);
	}

	public void SetEnvelopeAttackTime(float time)
	{
		CriAtomExPlayer.criAtomExPlayer_SetEnvelopeAttackTime(this.handle, time);
	}

	public void SetEnvelopeHoldTime(float time)
	{
		CriAtomExPlayer.criAtomExPlayer_SetEnvelopeHoldTime(this.handle, time);
	}

	public void SetEnvelopeDecayTime(float time)
	{
		CriAtomExPlayer.criAtomExPlayer_SetEnvelopeDecayTime(this.handle, time);
	}

	public void SetEnvelopeReleaseTime(float time)
	{
		CriAtomExPlayer.criAtomExPlayer_SetEnvelopeReleaseTime(this.handle, time);
	}

	public void SetEnvelopeSustainLevel(float level)
	{
		CriAtomExPlayer.criAtomExPlayer_SetEnvelopeSustainLevel(this.handle, level);
	}

	public void AttachFader()
	{
		CriAtomExPlayer.criAtomExPlayer_AttachFader(this.handle, IntPtr.Zero, IntPtr.Zero, 0);
	}

	public void DetachFader()
	{
		CriAtomExPlayer.criAtomExPlayer_DetachFader(this.handle);
	}

	public void SetFadeOutTime(int ms)
	{
		CriAtomExPlayer.criAtomExPlayer_SetFadeOutTime(this.handle, ms);
	}

	public void SetFadeInTime(int ms)
	{
		CriAtomExPlayer.criAtomExPlayer_SetFadeInTime(this.handle, ms);
	}

	public void SetFadeInStartOffset(int ms)
	{
		CriAtomExPlayer.criAtomExPlayer_SetFadeInStartOffset(this.handle, ms);
	}

	public void SetFadeOutEndDelay(int ms)
	{
		CriAtomExPlayer.criAtomExPlayer_SetFadeOutEndDelay(this.handle, ms);
	}

	public bool IsFading()
	{
		return CriAtomExPlayer.criAtomExPlayer_IsFading(this.handle);
	}

	public void ResetFaderParameters()
	{
		CriAtomExPlayer.criAtomExPlayer_ResetFaderParameters(this.handle);
	}

	public void Update(CriAtomExPlayback playback)
	{
		CriAtomExPlayer.criAtomExPlayer_Update(this.handle, playback.id);
	}

	public void UpdateAll()
	{
		CriAtomExPlayer.criAtomExPlayer_UpdateAll(this.handle);
	}

	public void ResetParameters()
	{
		CriAtomExPlayer.criAtomExPlayer_ResetParameters(this.handle);
	}

	public long GetTime()
	{
		return CriAtomExPlayer.criAtomExPlayer_GetTime(this.handle);
	}

	public CriAtomExPlayer.Status GetStatus()
	{
		return CriAtomExPlayer.criAtomExPlayer_GetStatus(this.handle);
	}

	public float GetParameterFloat32(CriAtomEx.Parameter id)
	{
		return CriAtomExPlayer.criAtomExPlayer_GetParameterFloat32(this.handle, id);
	}

	public uint GetParameterUint32(CriAtomEx.Parameter id)
	{
		return CriAtomExPlayer.criAtomExPlayer_GetParameterUint32(this.handle, id);
	}

	public int GetParameterSint32(CriAtomEx.Parameter id)
	{
		return CriAtomExPlayer.criAtomExPlayer_GetParameterSint32(this.handle, id);
	}

	public void SetSoundRendererType(CriAtomEx.SoundRendererType type)
	{
		CriAtomExPlayer.criAtomExPlayer_SetSoundRendererType(this.handle, type);
	}

	public void SetRandomSeed(uint seed)
	{
		CriAtomExPlayer.criAtomExPlayer_SetRandomSeed(this.handle, seed);
	}

	public void Loop(bool sw)
	{
		int count = (!sw) ? (-1) : (-3);
		CriAtomExPlayer.criAtomExPlayer_LimitLoopCount(this.handle, count);
	}

	public void SetAsrRackId(int asr_rack_id)
	{
		CriAtomExPlayer.criAtomExPlayer_SetAsrRackId(this.handle, asr_rack_id);
	}

	public void SetSequencePrepareTime(uint ms)
	{
		CriAtomExPlayer.criAtomExPlayer_SetSequencePrepareTime(this.handle, ms);
	}

	public void Stop()
	{
		CriAtomExPlayer.criAtomExPlayer_Stop(this.handle);
	}

	public void StopWithoutReleaseTime()
	{
		CriAtomExPlayer.criAtomExPlayer_StopWithoutReleaseTime(this.handle);
	}

	public void Pause(bool sw)
	{
		CriAtomExPlayer.criAtomExPlayer_Pause(this.handle, sw);
	}

	~CriAtomExPlayer()
	{
		this.Dispose();
	}

	[DllImport("cri_ware_unity")]
	private static extern IntPtr criAtomExPlayer_Create(ref CriAtomExPlayer.Config config, IntPtr work, int work_size);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_Destroy(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetCueId(IntPtr player, IntPtr acb_hn, int id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetCueName(IntPtr player, IntPtr acb_hn, string cue_name);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetCueIndex(IntPtr player, IntPtr acb_hn, int index);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetFile(IntPtr player, IntPtr binder, string path);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetContentId(IntPtr player, IntPtr binder, int id);

	[DllImport("cri_ware_unity")]
	private static extern uint criAtomExPlayer_Start(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern uint criAtomExPlayer_Prepare(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_Stop(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_StopWithoutReleaseTime(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_Pause(IntPtr player, bool sw);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_Resume(IntPtr player, CriAtomEx.ResumeMode mode);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExPlayer_IsPaused(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern CriAtomExPlayer.Status criAtomExPlayer_GetStatus(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern long criAtomExPlayer_GetTime(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetFormat(IntPtr player, CriAtomEx.Format format);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetNumChannels(IntPtr player, int num_channels);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetSamplingRate(IntPtr player, int sampling_rate);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetStartTime(IntPtr player, long start_time_ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetSequencePrepareTime(IntPtr player, uint seq_prep_time_ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_LimitLoopCount(IntPtr player, int count);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_Update(IntPtr player, uint id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_UpdateAll(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_ResetParameters(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern float criAtomExPlayer_GetParameterFloat32(IntPtr player, CriAtomEx.Parameter id);

	[DllImport("cri_ware_unity")]
	private static extern uint criAtomExPlayer_GetParameterUint32(IntPtr player, CriAtomEx.Parameter id);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExPlayer_GetParameterSint32(IntPtr player, CriAtomEx.Parameter id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetVolume(IntPtr player, float volume);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetPitch(IntPtr player, float pitch);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetPan3dAngle(IntPtr player, float pan3d_angle);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetPan3dInteriorDistance(IntPtr player, float pan3d_interior_distance);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetPan3dVolume(IntPtr player, float pan3d_volume);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetPanType(IntPtr player, CriAtomEx.PanType panType);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetSendLevel(IntPtr player, int channel, CriAtomEx.Speaker id, float level);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetBusSendLevel(IntPtr player, int bus_id, float level);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetBusSendLevelOffset(IntPtr player, int bus_id, float level_offset);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetDeviceSendLevel(IntPtr player, int device_id, float level);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetBandpassFilterParameters(IntPtr player, float cof_low, float cof_high);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetBiquadFilterParameters(IntPtr player, CriAtomEx.BiquadFilterType type, float frequency, float gain, float q);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetVoicePriority(IntPtr player, int priority);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetVoiceControlMethod(IntPtr player, CriAtomEx.VoiceControlMethod method);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetAisacControlById(IntPtr player, ushort control_id, float control_value);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetAisacControlByName(IntPtr player, string control_name, float control_value);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_Set3dSourceHn(IntPtr player, IntPtr source);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_Set3dListenerHn(IntPtr player, IntPtr listener);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetCategoryById(IntPtr player, uint category_id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetCategoryByName(IntPtr player, string category_name);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_UnsetCategory(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetCuePriority(IntPtr player, int cue_priority);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetEnvelopeAttackTime(IntPtr player, float attack_time_ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetEnvelopeHoldTime(IntPtr player, float hold_time_ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetEnvelopeDecayTime(IntPtr player, float decay_time_ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetEnvelopeReleaseTime(IntPtr player, float release_time_ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetEnvelopeSustainLevel(IntPtr player, float susutain_level);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_AttachFader(IntPtr player, IntPtr config, IntPtr work, int work_size);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_DetachFader(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetFadeOutTime(IntPtr player, int ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetFadeInTime(IntPtr player, int ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetFadeInStartOffset(IntPtr player, int ms);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetFadeOutEndDelay(IntPtr player, int ms);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExPlayer_IsFading(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_ResetFaderParameters(IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExPlayer_GetAttachedAisacInfo(IntPtr player, int aisac_attached_index, IntPtr aisac_info);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetFirstBlockIndex(IntPtr player, int index);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetSelectorLabel(IntPtr player, string selector, string label);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetSoundRendererType(IntPtr player, CriAtomEx.SoundRendererType type);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetRandomSeed(IntPtr player, uint seed);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_Loop(IntPtr player, bool sw);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayer_SetAsrRackId(IntPtr player, int asr_rack_id);
}
