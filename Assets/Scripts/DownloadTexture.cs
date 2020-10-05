using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class DownloadTexture : MonoBehaviour
{
	private sealed class _Start_c__Iterator4 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal WWW _www___0;

		internal UITexture _ut___1;

		internal int _PC;

		internal object _current;

		internal DownloadTexture __f__this;

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
				this._www___0 = new WWW(this.__f__this.url);
				this._current = this._www___0;
				this._PC = 1;
				return true;
			case 1u:
				this.__f__this.mTex = this._www___0.texture;
				if (this.__f__this.mTex != null)
				{
					this._ut___1 = this.__f__this.GetComponent<UITexture>();
					this._ut___1.mainTexture = this.__f__this.mTex;
					this._ut___1.MakePixelPerfect();
				}
				this._www___0.Dispose();
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

	public string url = "http://www.yourwebsite.com/logo.png";

	private Texture2D mTex;

	private IEnumerator Start()
	{
		DownloadTexture._Start_c__Iterator4 _Start_c__Iterator = new DownloadTexture._Start_c__Iterator4();
		_Start_c__Iterator.__f__this = this;
		return _Start_c__Iterator;
	}

	private void OnDestroy()
	{
		if (this.mTex != null)
		{
			UnityEngine.Object.Destroy(this.mTex);
		}
	}
}
