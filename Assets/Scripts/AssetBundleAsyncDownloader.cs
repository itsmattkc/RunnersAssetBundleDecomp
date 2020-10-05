using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetBundleAsyncDownloader : MonoBehaviour
{
	private sealed class _Load_c__IteratorBE : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal WWW _www___0;

		internal int _PC;

		internal object _current;

		internal AssetBundleAsyncDownloader __f__this;

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
				this._www___0 = null;
				if (this.__f__this.mAbRquest.useCache)
				{
					if (this.__f__this.mAbRquest.crc > 0u)
					{
						this._www___0 = WWW.LoadFromCacheOrDownload(this.__f__this.mAbRquest.path, this.__f__this.mAbRquest.version, this.__f__this.mAbRquest.crc);
					}
					else
					{
						this._www___0 = WWW.LoadFromCacheOrDownload(this.__f__this.mAbRquest.path, this.__f__this.mAbRquest.version);
					}
				}
				else
				{
					this._www___0 = new WWW(this.__f__this.mAbRquest.path);
				}
				this._current = this._www___0;
				this._PC = 1;
				return true;
			case 1u:
				if (this.__f__this.mAsyncDownloadCallback != null)
				{
					this.__f__this.mAsyncDownloadCallback(this._www___0);
				}
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

	private global::AssetBundleRequest mAbRquest;

	private AsyncDownloadCallback mAsyncDownloadCallback;

	private bool mDownloading;

	public AsyncDownloadCallback asyncLoadedCallback
	{
		get
		{
			return this.mAsyncDownloadCallback;
		}
		set
		{
			this.mAsyncDownloadCallback = value;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
		if (this.mAbRquest == null)
		{
			return;
		}
		try
		{
			base.StartCoroutine(this.Load());
		}
		catch (Exception ex)
		{
			global::Debug.Log("AssetBundleAsyncDownloader.Start() ExceptionMessage = " + ex.Message + "ToString() = " + ex.ToString());
		}
	}

	public void SetBundleRequest(global::AssetBundleRequest request)
	{
		this.mAbRquest = request;
	}

	private void Update()
	{
	}

	private IEnumerator Load()
	{
		AssetBundleAsyncDownloader._Load_c__IteratorBE _Load_c__IteratorBE = new AssetBundleAsyncDownloader._Load_c__IteratorBE();
		_Load_c__IteratorBE.__f__this = this;
		return _Load_c__IteratorBE;
	}
}
