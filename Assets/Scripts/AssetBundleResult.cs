using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleResult
{
	private AssetBundle mAssetBundle;

	private GameObject mObjects;

	private AssetBundleAsyncObjectLoader mAbAsyncLoader;

	private List<AsyncLoadedObjectCallback> mAbAsyncLoaderCallback;

	private Texture2D mTexture;

	private string mText;

	private byte[] mBytes;

	private string mPath;

	private string mName;

	public string mError;

	private bool mIsValid;

	public Texture2D Texture
	{
		get
		{
			return this.mTexture;
		}
	}

	public string Text
	{
		get
		{
			return this.mText;
		}
	}

	public byte[] bytes
	{
		get
		{
			return this.mBytes;
		}
	}

	public string Error
	{
		get
		{
			return this.mError;
		}
	}

	public string Path
	{
		get
		{
			return this.mPath;
		}
	}

	public string Name
	{
		get
		{
			return this.mName;
		}
	}

	public bool isValid
	{
		get
		{
			return this.mIsValid;
		}
		set
		{
			this.mIsValid = value;
		}
	}

	public AssetBundleResult(string path, AssetBundle assetBundle, string err)
	{
		this.Initialize(path, assetBundle, null, null, null, err);
		this.mAbAsyncLoaderCallback = new List<AsyncLoadedObjectCallback>(2);
	}

	public AssetBundleResult(string path, AssetBundle assetBundle, Texture2D texture, string err)
	{
		this.Initialize(path, assetBundle, texture, null, null, err);
	}

	public AssetBundleResult(string path, AssetBundle assetBundle, TextAsset textAsset, string err)
	{
		this.Initialize(path, assetBundle, null, textAsset.text, null, err);
	}

	public AssetBundleResult(string path, AssetBundle assetBundle, string text, string err)
	{
		this.Initialize(path, assetBundle, null, text, null, err);
	}

	public AssetBundleResult(string path, byte[] bytes, string err)
	{
		this.Initialize(path, null, null, null, bytes, err);
	}

	public void Initialize(string path, AssetBundle assetBundle, Texture2D texture, string text, byte[] bytes, string err)
	{
		this.mAssetBundle = assetBundle;
		this.mAbAsyncLoader = null;
		this.mAbAsyncLoaderCallback = null;
		this.mObjects = null;
		this.mTexture = texture;
		this.mText = text;
		this.mBytes = bytes;
		this.mError = err;
		this.mPath = path;
		this.mName = System.IO.Path.GetFileNameWithoutExtension(path);
		this.mIsValid = true;
	}

	public GameObject LoadObject(string objectName)
	{
		if (null == this.mObjects)
		{
			if (null == this.mAssetBundle)
			{
				global::Debug.LogError("AssetBundleResult : LoadObject : mAssetBundle is null");
			}
			else
			{
				this.mObjects = (this.mAssetBundle.Load(objectName, typeof(GameObject)) as GameObject);
				this.mAssetBundle.Unload(false);
				this.mAssetBundle = null;
			}
		}
		return this.mObjects;
	}

	public void LoadGameObjectAsync(string objectName, AsyncLoadedObjectCallback callback)
	{
		if (null == this.mObjects)
		{
			if (null == this.mAssetBundle)
			{
				global::Debug.LogError("AssetBundleResult : LoadObject : mAssetBundle is null");
			}
			else
			{
				this.mAbAsyncLoaderCallback.Add(callback);
				if (null == this.mAbAsyncLoader)
				{
					GameObject gameObject = new GameObject("async load object");
					this.mAbAsyncLoader = gameObject.AddComponent<AssetBundleAsyncObjectLoader>();
					this.mAbAsyncLoader.assetBundleRequest = this.mAssetBundle.LoadAsync(objectName, typeof(GameObject));
					this.mAbAsyncLoader.asyncLoadedCallback = new AsyncLoadedObjectCallback(this.AsyncLoadCallback);
				}
			}
		}
		else if (callback != null)
		{
			callback(this.mObjects);
		}
	}

	private void AsyncLoadCallback(UnityEngine.Object loadedObject)
	{
		this.mObjects = (loadedObject as GameObject);
		int count = this.mAbAsyncLoaderCallback.Count;
		for (int i = 0; i < count; i++)
		{
			AsyncLoadedObjectCallback asyncLoadedObjectCallback = this.mAbAsyncLoaderCallback[i];
			if (asyncLoadedObjectCallback != null)
			{
				asyncLoadedObjectCallback(this.mObjects);
			}
		}
		this.mAbAsyncLoaderCallback.Clear();
		this.mAbAsyncLoaderCallback = null;
		this.mAssetBundle.Unload(false);
		this.mAssetBundle = null;
		this.mAbAsyncLoader = null;
	}

	public void Clear()
	{
		if (null != this.mAbAsyncLoader)
		{
			this.mAbAsyncLoader.asyncLoadedCallback = null;
		}
		bool flag = false;
		if (null != this.mAssetBundle)
		{
			flag = true;
			this.mAssetBundle.Unload(false);
		}
		if (null != this.mObjects)
		{
			UnityEngine.Object.DestroyImmediate(this.mObjects, true);
		}
		if (!flag && null != this.mTexture)
		{
			UnityEngine.Object.Destroy(this.mTexture);
		}
		this.mAbAsyncLoader = null;
		this.mAssetBundle = null;
		this.mObjects = null;
		this.mTexture = null;
		this.mText = null;
		this.mBytes = null;
		this.mPath = null;
		this.mError = null;
		this.mIsValid = false;
	}
}
