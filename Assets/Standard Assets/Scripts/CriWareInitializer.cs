using System;
using System.IO;
using UnityEngine;

[AddComponentMenu("CRIWARE/Library Initializer")]
public class CriWareInitializer : MonoBehaviour
{
	public bool initializesFileSystem = true;

	public CriFsConfig fileSystemConfig = new CriFsConfig();

	public bool initializesAtom = true;

	public CriAtomConfig atomConfig = new CriAtomConfig();

	public bool initializesMana = true;

	public CriManaConfig manaConfig = new CriManaConfig();

	public bool useDecrypter;

	public CriWareDecrypterConfig decrypterConfig = new CriWareDecrypterConfig();

	public bool dontInitializeOnAwake;

	public bool dontDestroyOnLoad;

	private static int initializationCount;

	private void Awake()
	{
		CriWare.CheckBinaryVersionCompatibility();
		if (this.dontInitializeOnAwake)
		{
			return;
		}
		this.Initialize();
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Initialize()
	{
		CriWareInitializer.initializationCount++;
		if (CriWareInitializer.initializationCount != 1)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (this.initializesFileSystem)
		{
			CriFsPlugin.SetConfigParameters(this.fileSystemConfig.numberOfLoaders, this.fileSystemConfig.numberOfBinders, this.fileSystemConfig.numberOfInstallers, this.fileSystemConfig.installBufferSize * 1024, this.fileSystemConfig.maxPath, this.fileSystemConfig.minimizeFileDescriptorUsage);
			if (this.fileSystemConfig.androidDeviceReadBitrate == 0)
			{
				this.fileSystemConfig.androidDeviceReadBitrate = 50000000;
			}
			CriFsPlugin.SetConfigAdditionalParameters_ANDROID(this.fileSystemConfig.androidDeviceReadBitrate);
			CriFsPlugin.InitializeLibrary();
			if (this.fileSystemConfig.userAgentString.Length != 0)
			{
				CriFsUtility.SetUserAgentString(this.fileSystemConfig.userAgentString);
			}
		}
		if (this.initializesAtom)
		{
			int num = this.atomConfig.standardVoicePoolConfig.memoryVoices;
			int num2 = this.atomConfig.hcaMxVoicePoolConfig.memoryVoices;
			num += num2;
			num2 = 0;
			CriAtomPlugin.SetConfigParameters(Math.Max(this.atomConfig.maxVirtualVoices, CriAtomPlugin.GetRequiredMaxVirtualVoices(this.atomConfig)), num, this.atomConfig.standardVoicePoolConfig.streamingVoices, num2, this.atomConfig.hcaMxVoicePoolConfig.streamingVoices, this.atomConfig.outputSamplingRate, this.atomConfig.usesInGamePreview, this.atomConfig.serverFrequency);
			CriAtomPlugin.SetConfigAdditionalParameters_IOS((uint)Math.Max(this.atomConfig.iosBufferingTime, 16), this.atomConfig.iosOverrideIPodMusic);
			if (this.atomConfig.androidBufferingTime == 0)
			{
				this.atomConfig.androidBufferingTime = (int)(4000.0 / (double)this.atomConfig.serverFrequency);
			}
			if (this.atomConfig.androidStartBufferingTime == 0)
			{
				this.atomConfig.androidStartBufferingTime = (int)(3000.0 / (double)this.atomConfig.serverFrequency);
			}
			CriAtomPlugin.SetConfigAdditionalParameters_ANDROID(this.atomConfig.androidLowLatencyStandardVoicePoolConfig.memoryVoices, this.atomConfig.androidLowLatencyStandardVoicePoolConfig.streamingVoices, this.atomConfig.androidBufferingTime, this.atomConfig.androidStartBufferingTime);
			CriAtomPlugin.SetConfigAdditionalParameters_VITA(this.atomConfig.vitaAtrac9VoicePoolConfig.memoryVoices, this.atomConfig.vitaAtrac9VoicePoolConfig.streamingVoices, (!this.initializesMana) ? 0 : this.manaConfig.numberOfDecoders);
			CriAtomPlugin.SetConfigAdditionalParameters_PS4(this.atomConfig.ps4Atrac9VoicePoolConfig.memoryVoices, this.atomConfig.ps4Atrac9VoicePoolConfig.streamingVoices);
			CriAtomEx.SetAudioLatencyCheckEnabled_ANDROID(this.atomConfig.androidEnableAudioLatencyCheck);
			CriAtomPlugin.InitializeLibrary();
			if (this.atomConfig.useRandomSeedWithTime)
			{
				CriAtomEx.SetRandomSeed((uint)DateTime.Now.Ticks);
			}
			if (this.atomConfig.acfFileName.Length != 0)
			{
				string text = this.atomConfig.acfFileName;
				if (CriWare.IsStreamingAssetsPath(text))
				{
					text = Path.Combine(CriWare.streamingAssetsPath, text);
				}
				CriAtomEx.RegisterAcf(null, text);
			}
		}
		if (this.initializesMana)
		{
			CriManaPlugin.SetConfigParameters(this.manaConfig.numberOfDecoders, this.manaConfig.numberOfMaxEntries, this.manaConfig.enableCuePoint);
			CriManaPlugin.InitializeLibrary();
		}
		if (this.useDecrypter)
		{
			ulong key = (this.decrypterConfig.key.Length != 0) ? Convert.ToUInt64(this.decrypterConfig.key) : 0uL;
			string text2 = this.decrypterConfig.authenticationFile;
			if (CriWare.IsStreamingAssetsPath(text2))
			{
				text2 = Path.Combine(CriWare.streamingAssetsPath, text2);
			}
			CriWare.criWareUnity_SetDecryptionKey(key, text2, this.decrypterConfig.enableAtomDecryption, this.decrypterConfig.enableManaDecryption);
		}
		else
		{
			CriWare.criWareUnity_SetDecryptionKey(0uL, string.Empty, false, false);
		}
		if (this.dontDestroyOnLoad)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(CriWare.managerObject);
		}
	}

	private void OnDestroy()
	{
		CriWareInitializer.initializationCount--;
		if (CriWareInitializer.initializationCount != 0)
		{
			return;
		}
		if (this.initializesMana)
		{
			CriManaPlugin.FinalizeLibrary();
		}
		if (this.initializesAtom)
		{
			CriAtomPlugin.FinalizeLibrary();
		}
		if (this.initializesFileSystem)
		{
			CriFsPlugin.FinalizeLibrary();
		}
	}

	public static bool IsInitialized()
	{
		if (CriWareInitializer.initializationCount > 0)
		{
			return true;
		}
		CriWare.CheckBinaryVersionCompatibility();
		return false;
	}
}
