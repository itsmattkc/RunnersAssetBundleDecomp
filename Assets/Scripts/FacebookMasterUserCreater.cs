using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FacebookMasterUserCreater : MonoBehaviour
{
	public delegate void UpdateInfoCallback(WWW resultWWW);

	public delegate void UpdateInfoCallbackScheduled(WWW resultWWW1, WWW resultWWW2);

	private sealed class _WaitWWW_c__IteratorE : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal WWW www;

		internal FacebookMasterUserCreater.UpdateInfoCallback callback;

		internal int _PC;

		internal object _current;

		internal WWW ___www;

		internal FacebookMasterUserCreater.UpdateInfoCallback ___callback;

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

	private sealed class _WaitWWWScheduled_c__IteratorF : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string request1;

		internal WWW _www1___0;

		internal string request2;

		internal WWW _www2___1;

		internal FacebookMasterUserCreater.UpdateInfoCallbackScheduled callback;

		internal int _PC;

		internal object _current;

		internal string ___request1;

		internal string ___request2;

		internal FacebookMasterUserCreater.UpdateInfoCallbackScheduled ___callback;

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
				this._www1___0 = new WWW(this.request1);
				break;
			case 1u:
				break;
			case 2u:
				goto IL_74;
			default:
				return false;
			}
			if (!this._www1___0.isDone)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this._www2___1 = new WWW(this.request2);
			IL_74:
			if (!this._www2___1.isDone)
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			this.callback(this._www1___0, this._www2___1);
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

	private UserInfo m_meInfo;

	private List<UserInfo> m_appUserList = new List<UserInfo>();

	private Dictionary<string, UserInfo> m_friendList = new Dictionary<string, UserInfo>();

	private static readonly string AppId = "203227836537595";

	private static readonly string AppAccessToken = "203227836537595|PkqHXt8JyVfadw4sjwxAlqGyEig";

	public void AttachFriend(string userId, string userAccessToken)
	{
		this.m_appUserList.Clear();
		this.m_friendList.Clear();
		this.m_meInfo = new UserInfo();
		this.m_meInfo.id = userId;
		this.m_meInfo.accessToken = userAccessToken;
		string text = "https://graph.facebook.com/";
		text += FacebookMasterUserCreater.AppId;
		text += "/accounts/test-users?access_token=";
		text += FacebookMasterUserCreater.AppAccessToken;
		text += "&limit=5000";
		WWW www = new WWW(text);
		base.StartCoroutine(this.WaitWWW(www, new FacebookMasterUserCreater.UpdateInfoCallback(this.GetAllTestUserCallback)));
	}

	private void GetAllTestUserCallback(WWW wwwResult)
	{
		string text = wwwResult.text;
		global::Debug.Log(text);
		JsonData jsonData = JsonMapper.ToObject(text);
		if (jsonData != null)
		{
			JsonData jsonArray = NetUtil.GetJsonArray(jsonData, "data");
			if (jsonArray != null)
			{
				for (int i = 0; i < jsonArray.Count; i++)
				{
					UserInfo userInfo = new UserInfo();
					userInfo.id = NetUtil.GetJsonString(jsonArray[i], "id");
					userInfo.accessToken = NetUtil.GetJsonString(jsonArray[i], "access_token");
					userInfo.loginUrl = NetUtil.GetJsonString(jsonArray[i], "login_url");
					if (!string.IsNullOrEmpty(userInfo.id))
					{
						if (!string.IsNullOrEmpty(userInfo.accessToken))
						{
							this.m_appUserList.Add(userInfo);
						}
					}
				}
			}
		}
		string text2 = "https://graph.facebook.com/";
		text2 += this.m_meInfo.id;
		text2 += "/friends?access_token=";
		text2 += this.m_meInfo.accessToken;
		text2 += "&limit=5000";
		WWW www = new WWW(text2);
		base.StartCoroutine(this.WaitWWW(www, new FacebookMasterUserCreater.UpdateInfoCallback(this.GetMasterUserFriendsCallback)));
	}

	private void GetMasterUserFriendsCallback(WWW wwwResult)
	{
		string text = wwwResult.text;
		global::Debug.Log(text);
		JsonData jsonData = JsonMapper.ToObject(text);
		if (jsonData != null)
		{
			JsonData jsonArray = NetUtil.GetJsonArray(jsonData, "data");
			if (jsonArray != null)
			{
				for (int i = 0; i < jsonArray.Count; i++)
				{
					UserInfo userInfo = new UserInfo();
					userInfo.id = NetUtil.GetJsonString(jsonArray[i], "id");
					userInfo.accessToken = NetUtil.GetJsonString(jsonArray[i], "access_token");
					userInfo.loginUrl = NetUtil.GetJsonString(jsonArray[i], "login_url");
					this.m_friendList.Add(userInfo.id, userInfo);
				}
			}
		}
		foreach (UserInfo current in this.m_appUserList)
		{
			if (current != null)
			{
				if (this.m_meInfo != current)
				{
					if (!this.m_friendList.ContainsKey(current.id))
					{
						string text2 = "https://graph.facebook.com/";
						text2 += this.m_meInfo.id;
						text2 += "/friends/";
						text2 += current.id;
						text2 += "?access_token=";
						text2 += this.m_meInfo.accessToken;
						text2 += "&method=post";
						string text3 = "https://graph.facebook.com/";
						text3 += current.id;
						text3 += "/friends/";
						text3 += this.m_meInfo.id;
						text3 += "?access_token=";
						text3 += current.accessToken;
						text3 += "&method=post";
						base.StartCoroutine(this.WaitWWWScheduled(text2, text3, new FacebookMasterUserCreater.UpdateInfoCallbackScheduled(this.AttachFriendCallback)));
					}
				}
			}
		}
	}

	private void AttachFriendCallback(WWW wwwResult1, WWW wwwResult2)
	{
		global::Debug.Log(wwwResult1.text);
		global::Debug.Log(wwwResult2.text);
		wwwResult1.Dispose();
		wwwResult2.Dispose();
	}

	private IEnumerator WaitWWW(WWW www, FacebookMasterUserCreater.UpdateInfoCallback callback)
	{
		FacebookMasterUserCreater._WaitWWW_c__IteratorE _WaitWWW_c__IteratorE = new FacebookMasterUserCreater._WaitWWW_c__IteratorE();
		_WaitWWW_c__IteratorE.www = www;
		_WaitWWW_c__IteratorE.callback = callback;
		_WaitWWW_c__IteratorE.___www = www;
		_WaitWWW_c__IteratorE.___callback = callback;
		return _WaitWWW_c__IteratorE;
	}

	private IEnumerator WaitWWWScheduled(string request1, string request2, FacebookMasterUserCreater.UpdateInfoCallbackScheduled callback)
	{
		FacebookMasterUserCreater._WaitWWWScheduled_c__IteratorF _WaitWWWScheduled_c__IteratorF = new FacebookMasterUserCreater._WaitWWWScheduled_c__IteratorF();
		_WaitWWWScheduled_c__IteratorF.request1 = request1;
		_WaitWWWScheduled_c__IteratorF.request2 = request2;
		_WaitWWWScheduled_c__IteratorF.callback = callback;
		_WaitWWWScheduled_c__IteratorF.___request1 = request1;
		_WaitWWWScheduled_c__IteratorF.___request2 = request2;
		_WaitWWWScheduled_c__IteratorF.___callback = callback;
		return _WaitWWWScheduled_c__IteratorF;
	}
}
