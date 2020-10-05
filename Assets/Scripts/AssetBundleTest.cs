using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetBundleTest : MonoBehaviour
{
	private sealed class _WaitLoard_c__IteratorC9 : IDisposable, IEnumerator, IEnumerator<object>
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
		WWW www = WWW.LoadFromCacheOrDownload("http://web2/HikiData/Sonic_Runners/Soft/Asset/AssetBundles_Win/PrephabKnuckles.unity3d", 5);
		base.StartCoroutine(this.WaitLoard(www));
	}

	private void Update()
	{
	}

	private IEnumerator WaitLoard(WWW www)
	{
		AssetBundleTest._WaitLoard_c__IteratorC9 _WaitLoard_c__IteratorC = new AssetBundleTest._WaitLoard_c__IteratorC9();
		_WaitLoard_c__IteratorC.www = www;
		_WaitLoard_c__IteratorC.___www = www;
		return _WaitLoard_c__IteratorC;
	}
}
