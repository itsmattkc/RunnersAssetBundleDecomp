using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetBundleLoadSceneTest : MonoBehaviour
{
	private sealed class _WaitLoard_c__IteratorC8 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal WWW www;

		internal string scenename;

		internal int _PC;

		internal object _current;

		internal WWW ___www;

		internal string ___scenename;

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
			if (this.www == null)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.www.error != null || this.www.error != null)
			{
				global::Debug.LogError(this.www.error);
			}
			else
			{
				Application.LoadLevelAdditive(this.scenename);
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
		WWW www = WWW.LoadFromCacheOrDownload("http://web2/HikiData/Sonic_Runners/Soft/Asset/AssetBundles_Win/ResourcesCommonPrefabs.unity3d", 5);
		base.StartCoroutine(this.WaitLoard(www, "ResourcesCommonPrefabs"));
		WWW www2 = WWW.LoadFromCacheOrDownload("http://web2/HikiData/Sonic_Runners/Soft/Asset/AssetBundles_Win/ResourcesCommonObject.unity3d", 5);
		base.StartCoroutine(this.WaitLoard(www2, "ResourcesCommonObject"));
	}

	private void Update()
	{
	}

	private IEnumerator WaitLoard(WWW www, string scenename)
	{
		AssetBundleLoadSceneTest._WaitLoard_c__IteratorC8 _WaitLoard_c__IteratorC = new AssetBundleLoadSceneTest._WaitLoard_c__IteratorC8();
		_WaitLoard_c__IteratorC.www = www;
		_WaitLoard_c__IteratorC.scenename = scenename;
		_WaitLoard_c__IteratorC.___www = www;
		_WaitLoard_c__IteratorC.___scenename = scenename;
		return _WaitLoard_c__IteratorC;
	}
}
