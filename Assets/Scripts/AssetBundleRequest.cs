using Message;
using System;
using System.IO;
using UnityEngine;

public class AssetBundleRequest
{
	public enum Type
	{
		GAMEOBJECT,
		TEXTURE,
		TEXT,
		SCENE,
		OTHER
	}

	private enum State
	{
		INVALID = -1,
		EXECUTING,
		SUCCEEDED,
		FAILED,
		RETRY
	}

	private bool mUseCache;

	private global::AssetBundleRequest.State mState;

	private string mPath;

	private string mFileName;

	private GameObject mReturnObject;

	private WWW mWWW;

	private int mVersion;

	private uint mCRC;

	private global::AssetBundleRequest.Type mType;

	private string mURL;

	private int mTryCount;

	private int mMaxTryCount;

	private bool mIsLoaded;

	private AssetBundleResult mAssetbundleResult;

	private GameObject mDownloaderObject;

	public static readonly float DefaultTimeOut = 60f;

	private float mTimeOut;

	private float mElapsedTime;

	private bool mCancel;

	public bool useCache
	{
		get
		{
			return this.mUseCache;
		}
	}

	public bool isCancel
	{
		get
		{
			return this.mCancel;
		}
	}

	public WWW www
	{
		get
		{
			return this.mWWW;
		}
	}

	public int version
	{
		get
		{
			return this.mVersion;
		}
	}

	public uint crc
	{
		get
		{
			return this.mCRC;
		}
	}

	public global::AssetBundleRequest.Type type
	{
		get
		{
			return this.mType;
		}
	}

	public string Url
	{
		get
		{
			return this.mURL;
		}
	}

	public Texture2D Texture
	{
		get
		{
			return this.mAssetbundleResult.Texture;
		}
	}

	public string path
	{
		get
		{
			return this.mPath;
		}
	}

	public string FileName
	{
		get
		{
			return this.mFileName;
		}
	}

	public AssetBundleResult assetbundleResult
	{
		get
		{
			return this.mAssetbundleResult;
		}
	}

	public GameObject returnObject
	{
		get
		{
			return this.mReturnObject;
		}
	}

	public bool IsTypeTexture
	{
		get
		{
			return this.mType == global::AssetBundleRequest.Type.TEXTURE;
		}
	}

	public float TimeOut
	{
		get
		{
			return this.mTimeOut;
		}
		set
		{
			if (value > global::AssetBundleRequest.DefaultTimeOut * 3f)
			{
				value = global::AssetBundleRequest.DefaultTimeOut * 3f;
			}
			this.mTimeOut = value;
		}
	}

	public int TryCount
	{
		get
		{
			return this.mTryCount;
		}
	}

	public int MaxTryCount
	{
		get
		{
			return this.mMaxTryCount;
		}
	}

	public AssetBundleRequest(string path, int version, uint crc, global::AssetBundleRequest.Type type, GameObject returnObject) : this(path, version, crc, type, returnObject, false)
	{
	}

	public AssetBundleRequest(string path, int version, uint crc, global::AssetBundleRequest.Type type, GameObject returnObject, bool useCache)
	{
		this.mMaxTryCount = 1;
		this.mTimeOut = global::AssetBundleRequest.DefaultTimeOut;
		
		this.mUseCache = useCache;
		this.mState = global::AssetBundleRequest.State.INVALID;
		this.mPath = path;
		this.mFileName = Path.GetFileNameWithoutExtension(path);
		this.mReturnObject = returnObject;
		this.mWWW = null;
		this.mVersion = version;
		this.mCRC = crc;
		this.mType = type;
		this.mTryCount = 0;
		this.mCancel = false;
		this.mAssetbundleResult = null;
		this.mIsLoaded = false;
	}

	public AssetBundleRequest(global::AssetBundleRequest request)
	{
		this.mMaxTryCount = 1;
		this.mTimeOut = global::AssetBundleRequest.DefaultTimeOut;
		
		this.mUseCache = request.useCache;
		this.mState = global::AssetBundleRequest.State.INVALID;
		this.mPath = request.path;
		this.mFileName = request.mFileName;
		this.mReturnObject = request.returnObject;
		this.mWWW = null;
		this.mVersion = request.version;
		this.mCRC = request.crc;
		this.mType = request.type;
		this.mTryCount = 0;
		this.mCancel = false;
		this.mAssetbundleResult = null;
		this.mIsLoaded = false;
	}

	public void Load()
	{
		if (this.IsExecuting())
		{
			return;
		}
		if (this.mDownloaderObject == null)
		{
			global::Debug.Log("AssetBundleRequest:" + this.mFileName);
			this.mDownloaderObject = new GameObject("AssetBundleAsyncDownloader");
			AssetBundleAsyncDownloader assetBundleAsyncDownloader = this.mDownloaderObject.AddComponent<AssetBundleAsyncDownloader>();
			assetBundleAsyncDownloader.SetBundleRequest(this);
			assetBundleAsyncDownloader.asyncLoadedCallback = new AsyncDownloadCallback(this.LoadedCallback);
			this.mState = global::AssetBundleRequest.State.EXECUTING;
			this.mElapsedTime = 0f;
		}
	}

	public void Cancel()
	{
		this.mCancel = true;
	}

	public void Unload()
	{
		if (!AssetBundleManager.Instance.IsCancelRequest() && this.IsExecuting())
		{
			global::Debug.LogWarning("AssetBundleRequest.Unload : Now executing...");
			return;
		}
		if (this.mAssetbundleResult != null)
		{
			this.mAssetbundleResult.Clear();
			this.mAssetbundleResult = null;
		}
		if (this.mDownloaderObject != null)
		{
			UnityEngine.Object.Destroy(this.mDownloaderObject);
			this.mDownloaderObject = null;
		}
		if (this.mWWW != null)
		{
			this.mWWW.Dispose();
			this.mWWW = null;
		}
	}

	public bool IsInvalid()
	{
		return this.mState == global::AssetBundleRequest.State.INVALID;
	}

	public bool IsRetry()
	{
		return this.mState == global::AssetBundleRequest.State.RETRY;
	}

	public bool IsSucceeded()
	{
		return this.mState == global::AssetBundleRequest.State.SUCCEEDED;
	}

	public bool IsFailed()
	{
		return this.mState == global::AssetBundleRequest.State.FAILED;
	}

	public bool IsExecuting()
	{
		return this.mState == global::AssetBundleRequest.State.EXECUTING;
	}

	private void LoadedCallback(WWW www)
	{
		this.mWWW = www;
		this.mURL = this.mWWW.url;
		UnityEngine.Object.Destroy(this.mDownloaderObject);
		this.mDownloaderObject = null;
	}

	public bool IsLoaded()
	{
		if (this.mDownloaderObject != null)
		{
			return false;
		}
		if (this.mWWW == null)
		{
			return this.mIsLoaded;
		}
		return this.mWWW.isDone;
	}

	public bool IsLoading()
	{
		if (this.mDownloaderObject != null)
		{
			return true;
		}
		if (this.mWWW == null)
		{
			return !this.mIsLoaded;
		}
		return !this.mWWW.isDone;
	}

	public bool IsTimeOut()
	{
		return this.mTimeOut <= this.mElapsedTime;
	}

	public float UpdateElapsedTime(float addElapsedTime)
	{
		this.mElapsedTime += addElapsedTime;
		return 0.1f;
	}

	private bool IsErrorTexture()
	{
		if (this.mAssetbundleResult == null)
		{
			return false;
		}
		Texture2D texture = this.mAssetbundleResult.Texture;
		return !(null == texture) && !(string.Empty != texture.name) && texture.height == 8 && texture.width == 8 && texture.filterMode == FilterMode.Bilinear && texture.anisoLevel == 1 && texture.wrapMode == TextureWrapMode.Repeat && texture.mipMapBias == 0f;
	}

	public Texture2D MakeTexture()
	{
		Texture2D result = null;
		if (this.mType == global::AssetBundleRequest.Type.TEXTURE && this.mWWW.error == null)
		{
			result = this.mWWW.texture;
		}
		return result;
	}

	public string MakeText()
	{
		string result = null;
		if (this.mType == global::AssetBundleRequest.Type.TEXT && this.mWWW.error == null)
		{
			result = this.mWWW.text;
		}
		return result;
	}

	public void Result()
	{
		try
		{
			if (this.mCancel)
			{
				global::Debug.LogWarning("!!! AssetBundleRequest Cancel : " + this.mPath, DebugTraceManager.TraceType.ASSETBUNDLE);
				if (this.mWWW != null)
				{
					this.mWWW.Dispose();
					this.mWWW = null;
				}
				this.mState = global::AssetBundleRequest.State.FAILED;
			}
			else
			{
				bool flag = false;
				if (this.IsTimeOut())
				{
					global::Debug.Log("AssetBundle TimeOut : " + this.mPath, DebugTraceManager.TraceType.ASSETBUNDLE);
					flag = true;
				}
				else
				{
					if (this.mWWW == null)
					{
						return;
					}
					if (this.mUseCache)
					{
					}
					if (!this.mWWW.isDone)
					{
						return;
					}
				}
				bool flag2 = true;
				bool flag3 = false;
				if (this.mWWW != null && this.mWWW.error != null && !this.mWWW.error.Contains("Cannot load cached AssetBundle"))
				{
					flag3 = true;
				}
				if (flag3 || flag)
				{
					global::Debug.Log("!!!!! AssetBundle.Result : Error : " + ((this.mWWW != null) ? this.mWWW.error : "null"), DebugTraceManager.TraceType.ASSETBUNDLE);
					this.mTryCount++;
					if (this.mMaxTryCount > this.mTryCount)
					{
						global::Debug.LogWarning(string.Concat(new object[]
						{
							"AssetBundle.Result : Retry[",
							this.mTryCount,
							"/",
							this.mMaxTryCount,
							"]"
						}), DebugTraceManager.TraceType.ASSETBUNDLE);
						this.mState = global::AssetBundleRequest.State.RETRY;
						if (this.mWWW != null)
						{
							this.mWWW.Dispose();
							this.mWWW = null;
						}
						return;
					}
					global::Debug.LogWarning("AssetBundle.Result : Failed", DebugTraceManager.TraceType.ASSETBUNDLE);
					this.mState = global::AssetBundleRequest.State.FAILED;
					flag2 = false;
				}
				if (flag2)
				{
					this.mAssetbundleResult = this.CreateResult();
					this.mState = global::AssetBundleRequest.State.SUCCEEDED;
					global::Debug.Log("AssetBundle.Result : Success : " + this.mFileName);
					if (this.mReturnObject != null)
					{
						MsgAssetBundleResponseSucceed value = new MsgAssetBundleResponseSucceed(this, this.mAssetbundleResult);
						this.mReturnObject.SendMessage("AssetBundleResponseSucceed", value, SendMessageOptions.DontRequireReceiver);
					}
				}
				else
				{
					this.mState = global::AssetBundleRequest.State.FAILED;
					global::Debug.LogWarning("!!!!! AssetBundle.Result : Failure : " + ((this.mWWW == null) ? "-----" : this.mWWW.error), DebugTraceManager.TraceType.ASSETBUNDLE);
					if (this.mReturnObject != null)
					{
						MsgAssetBundleResponseFailed value2 = new MsgAssetBundleResponseFailed(this, this.mAssetbundleResult);
						this.mReturnObject.SendMessage("AssetBundleResponseFailed", value2, SendMessageOptions.DontRequireReceiver);
					}
				}
				if (this.mWWW != null)
				{
					this.mWWW.Dispose();
					this.mWWW = null;
				}
				this.mIsLoaded = true;
			}
		}
		catch (Exception ex)
		{
			global::Debug.Log("AssetBundleRequest.Result() Exception:Message = " + ex.Message + "ToString() = " + ex.ToString());
		}
	}

	public AssetBundleResult CreateResult()
	{
		AssetBundleResult result = null;
		if (this.mWWW == null)
		{
			byte[] bytes = null;
			string empty = string.Empty;
			result = new AssetBundleResult(this.mPath, bytes, empty);
			return result;
		}
		try
		{
			string text = (this.mWWW.error == null) ? null : (this.mWWW.error.Clone() as string);
			AssetBundle assetBundle = (text != null) ? null : this.mWWW.assetBundle;
			switch (this.mType)
			{
			case global::AssetBundleRequest.Type.GAMEOBJECT:
				result = new AssetBundleResult(this.mPath, assetBundle, text);
				break;
			case global::AssetBundleRequest.Type.TEXTURE:
			{
				Texture2D texture;
				if (assetBundle != null)
				{
					texture = (assetBundle.mainAsset as Texture2D);
				}
				else
				{
					texture = this.MakeTexture();
				}
				result = new AssetBundleResult(this.mPath, assetBundle, texture, text);
				break;
			}
			case global::AssetBundleRequest.Type.TEXT:
			{
				string text2 = null;
				if (assetBundle != null)
				{
					TextAsset textAsset = assetBundle.mainAsset as TextAsset;
					if (textAsset)
					{
						text2 = textAsset.text;
					}
				}
				else
				{
					text2 = this.MakeText();
				}
				result = new AssetBundleResult(this.mPath, assetBundle, text2, text);
				break;
			}
			case global::AssetBundleRequest.Type.SCENE:
				result = new AssetBundleResult(this.mPath, assetBundle, text);
				break;
			default:
			{
				byte[] bytes2 = this.mWWW.bytes.Clone() as byte[];
				result = new AssetBundleResult(this.mPath, bytes2, text);
				break;
			}
			}
		}
		catch (Exception ex)
		{
			global::Debug.Log("AssetBundleManager.CreateResult:Exception , Message = " + ex.Message + ", ToString() = " + ex.ToString());
		}
		return result;
	}
}
