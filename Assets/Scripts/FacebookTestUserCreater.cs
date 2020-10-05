using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FacebookTestUserCreater : MonoBehaviour
{
	public delegate void UpdateInfoCallback(WWW resultWWW);

	private sealed class _WaitWWW_c__IteratorD : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal WWW www;

		internal FacebookTestUserCreater.UpdateInfoCallback callback;

		internal int _PC;

		internal object _current;

		internal WWW ___www;

		internal FacebookTestUserCreater.UpdateInfoCallback ___callback;

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
			this.callback(this.www);
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

	private int m_requestCreateCount;

	public void Create(int createCount)
	{
		this.m_requestCreateCount = createCount;
		string empty = string.Empty;
		string empty2 = string.Empty;
		for (int i = 0; i < this.m_requestCreateCount; i++)
		{
			string text = "https://graph.facebook.com/";
			text += empty;
			text += "/accounts/test-users?installed=true&locale=ja_JP&permissions=read_stream&method=post&access_token=";
			text += empty2;
			global::Debug.Log("FacebookTestUserCreater.Create.query = " + text);
			WWW www = new WWW(text);
			base.StartCoroutine(this.WaitWWW(www, new FacebookTestUserCreater.UpdateInfoCallback(this.CreateTestUserCallback)));
		}
	}

	private void CreateTestUserCallback(WWW wwwResult)
	{
	}

	private IEnumerator WaitWWW(WWW www, FacebookTestUserCreater.UpdateInfoCallback callback)
	{
		FacebookTestUserCreater._WaitWWW_c__IteratorD _WaitWWW_c__IteratorD = new FacebookTestUserCreater._WaitWWW_c__IteratorD();
		_WaitWWW_c__IteratorD.www = www;
		_WaitWWW_c__IteratorD.callback = callback;
		_WaitWWW_c__IteratorD.___www = www;
		_WaitWWW_c__IteratorD.___callback = callback;
		return _WaitWWW_c__IteratorD;
	}
}
