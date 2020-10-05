using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetBundleAsyncObjectLoader : MonoBehaviour
{
	private sealed class _Watch_c__IteratorBD : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal AssetBundleAsyncObjectLoader __f__this;

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
				this._current = this.__f__this.mAbRquest;
				this._PC = 1;
				return true;
			case 1u:
				this.__f__this.mAsyncLoadedCallback(this.__f__this.mAbRquest.asset);
				this.__f__this.mLoading = false;
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

	private UnityEngine.AssetBundleRequest mAbRquest;

	private AsyncLoadedObjectCallback mAsyncLoadedCallback;

	private bool mLoading;

	public AsyncLoadedObjectCallback asyncLoadedCallback
	{
		get
		{
			return this.mAsyncLoadedCallback;
		}
		set
		{
			this.mAsyncLoadedCallback = value;
		}
	}

	public UnityEngine.AssetBundleRequest assetBundleRequest
	{
		get
		{
			return this.mAbRquest;
		}
		set
		{
			this.mAbRquest = value;
		}
	}

	private void Awake()
	{
		this.mLoading = true;
	}

	private void Start()
	{
		base.StartCoroutine("Watch");
	}

	private void Update()
	{
		if (!this.mLoading)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private IEnumerator Watch()
	{
		AssetBundleAsyncObjectLoader._Watch_c__IteratorBD _Watch_c__IteratorBD = new AssetBundleAsyncObjectLoader._Watch_c__IteratorBD();
		_Watch_c__IteratorBD.__f__this = this;
		return _Watch_c__IteratorBD;
	}
}
