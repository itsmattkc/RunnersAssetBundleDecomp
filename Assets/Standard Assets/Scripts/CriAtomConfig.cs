using System;

[Serializable]
public class CriAtomConfig
{
	[Serializable]
	public class StandardVoicePoolConfig
	{
		public int memoryVoices = 16;

		public int streamingVoices = 8;
	}

	[Serializable]
	public class HcaMxVoicePoolConfig
	{
		public int memoryVoices;

		public int streamingVoices;
	}

	[Serializable]
	public class AndroidLowLatencyStandardVoicePoolConfig
	{
		public int memoryVoices;

		public int streamingVoices;
	}

	[Serializable]
	public class VitaAtrac9VoicePoolConfig
	{
		public int memoryVoices;

		public int streamingVoices;
	}

	[Serializable]
	public class Ps4Atrac9VoicePoolConfig
	{
		public int memoryVoices;

		public int streamingVoices;
	}

	public string acfFileName = string.Empty;

	public int maxVirtualVoices = 32;

	public CriAtomConfig.StandardVoicePoolConfig standardVoicePoolConfig;

	public CriAtomConfig.HcaMxVoicePoolConfig hcaMxVoicePoolConfig;

	public int outputSamplingRate;

	public bool usesInGamePreview;

	public float serverFrequency = 60f;

	public bool useRandomSeedWithTime;

	public int iosBufferingTime = 100;

	public bool iosOverrideIPodMusic;

	public int androidBufferingTime = 133;

	public int androidStartBufferingTime = 100;

	public bool androidEnableAudioLatencyCheck;

	public CriAtomConfig.AndroidLowLatencyStandardVoicePoolConfig androidLowLatencyStandardVoicePoolConfig;

	public CriAtomConfig.VitaAtrac9VoicePoolConfig vitaAtrac9VoicePoolConfig;

	public CriAtomConfig.Ps4Atrac9VoicePoolConfig ps4Atrac9VoicePoolConfig;
}
