using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetBundleTestNoCache : MonoBehaviour
{
	private sealed class _WaitLoard_c__IteratorCA : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal WWW www;

		internal AssetBundle _myLoadedAssetBundle___0;

		internal UnityEngine.Object _asset___1;

		internal int _PC;

		internal object _current;

		internal WWW ___www;

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
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this.www.isDone)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.www.error != null)
			{
				global::Debug.LogError(this.www.error);
			}
			else
			{
				this._myLoadedAssetBundle___0 = this.www.assetBundle;
				this._asset___1 = this._myLoadedAssetBundle___0.mainAsset;
				UnityEngine.Object.Instantiate(this._asset___1);
				this._myLoadedAssetBundle___0.Unload(false);
			}
			this._PC = -1;
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

	private void Start()
	{
		WWW www = new WWW("http://web2/HikiData/Sonic_Runners/Soft/Asset/AssetBundles_Win/PrephabKnuckles.unity3d");
		base.StartCoroutine(this.WaitLoard(www));
	}

	private void Update()
	{
	}

	private IEnumerator WaitLoard(WWW www)
	{
		AssetBundleTestNoCache._WaitLoard_c__IteratorCA _WaitLoard_c__IteratorCA = new AssetBundleTestNoCache._WaitLoard_c__IteratorCA();
		_WaitLoard_c__IteratorCA.www = www;
		_WaitLoard_c__IteratorCA.___www = www;
		return _WaitLoard_c__IteratorCA;
	}
}
