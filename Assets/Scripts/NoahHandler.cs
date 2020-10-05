using NoahUnity;
using System;
using System.Collections;
using UnityEngine;

public class NoahHandler : MonoBehaviour, NoahHandlerInterface
{
	public static string app_name = string.Empty;

	public static string consumer_key = string.Empty;

	public static string secret_key = string.Empty;

	public static string action_id = string.Empty;

	public static bool noahDebug;

	public static bool reconnect;

	private GameObject m_callbackObject;

	private static NoahHandler m_instance;

	private string m_guid;

	private bool m_isEndConnect;

	private bool m_isEndSetGUID;

	private static readonly int FailRetryCount = 3;

	private int m_connectFailCount;

	private int m_commitFailCount;

	public static NoahHandler Instance
	{
		get
		{
			return NoahHandler.m_instance;
		}
		private set
		{
		}
	}

	public bool IsEndConnect
	{
		get
		{
			return this.m_isEndConnect;
		}
		set
		{
			this.m_isEndConnect = value;
		}
	}

	public bool IsEndSetGUID
	{
		get
		{
			return this.m_isEndSetGUID;
		}
		set
		{
			this.m_isEndSetGUID = value;
		}
	}

	public void SetCallback(GameObject callbackObject)
	{
		this.m_callbackObject = callbackObject;
	}

	public void SetGUID(string guid)
	{
		this.m_guid = guid;
		global::Debug.Log("NoahHandler.SetGUID=" + ((guid != null) ? guid : "null"));
		if (this.m_isEndConnect && !this.m_isEndSetGUID)
		{
			Noah.Instance.SetGUID(this.m_guid);
		}
	}

	public string GetGUID()
	{
		return this.m_guid;
	}

	public void Awake()
	{
		if (NoahHandler.m_instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			if (Application.platform == RuntimePlatform.Android)
			{
				NoahHandler.consumer_key = "APP_65453158ce4ca288";
				NoahHandler.secret_key = "KEY_89353158ce4ca33f";
				NoahHandler.action_id = "OFF_42953158cf5ccd0c";
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				NoahHandler.consumer_key = "APP_20953158ca2030ad";
				NoahHandler.secret_key = "KEY_78053158ca20310c";
				NoahHandler.action_id = "OFF_60353158cb641b5c";
			}
			NoahHandler.m_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.transform.gameObject);
		}
	}

	public void Start()
	{
		Screen.sleepTimeout = 0;
		Noah.Instance.SetDebugMode(NoahHandler.noahDebug);
	}

	public void OnApplicationPause(bool flag)
	{
		if (flag)
		{
			Noah.Instance.CloseBanner();
			Noah.Instance.Suspend();
			this.m_isEndConnect = false;
		}
		else
		{
			this.m_connectFailCount = 0;
			this.m_commitFailCount = 0;
			Noah.Instance.Resume();
			if (NoahHandler.reconnect)
			{
				Noah.Instance.Connect(NoahHandler.consumer_key, NoahHandler.secret_key, NoahHandler.action_id);
			}
		}
	}

	public void OnDestroy()
	{
		Noah.Instance.Suspend();
		Noah.Instance.Close();
	}

	public void On15minutes()
	{
	}

	public void OnCommit(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string text = hashtable["result"].ToString();
		string str = hashtable["action_id"].ToString();
		switch (Noah.ConvertResultState(int.Parse(text)))
		{
		case Noah.ResultState.CommitOver:
			this.m_commitFailCount = 0;
			break;
		case Noah.ResultState.Success:
			this.m_commitFailCount = 0;
			break;
		case Noah.ResultState.Failure:
			if (this.m_commitFailCount < NoahHandler.FailRetryCount)
			{
				Noah.Instance.Commit(NoahHandler.action_id);
				global::Debug.Log("NoahHandler.OnConnect:Retry for failed");
			}
			this.m_commitFailCount++;
			break;
		}
		string text2 = "OnCommit\n" + Noah.ConvertResultState(int.Parse(text));
		text2 = text2 + "\n" + str;
		global::Debug.Log(text2);
		if (this.m_callbackObject != null)
		{
			this.m_callbackObject.SendMessage("OnCommitNoah", text, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnConnect(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string text = hashtable["result"].ToString();
		switch (Noah.ConvertResultState(int.Parse(text)))
		{
		case Noah.ResultState.Success:
			if (this.m_guid != null)
			{
				Noah.Instance.SetGUID(this.m_guid);
			}
			this.m_isEndConnect = true;
			NoahHandler.reconnect = true;
			this.m_connectFailCount = 0;
			break;
		case Noah.ResultState.Failure:
			if (this.m_connectFailCount < NoahHandler.FailRetryCount)
			{
				Noah.Instance.Connect(NoahHandler.consumer_key, NoahHandler.secret_key, NoahHandler.action_id);
				global::Debug.Log("NoahHandler.OnConnect:Retry for failed");
			}
			this.m_connectFailCount++;
			break;
		}
		string message = "OnConnect\n" + Noah.ConvertResultState(int.Parse(text));
		global::Debug.Log(message);
		if (this.m_callbackObject != null)
		{
			this.m_callbackObject.SendMessage("OnConnectNoah", text, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnDelete(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string s = hashtable["result"].ToString();
		switch (Noah.ConvertResultState(int.Parse(s)))
		{
		}
		string message = "OnDelete\n" + Noah.ConvertResultState(int.Parse(s));
		global::Debug.Log(message);
	}

	public void OnGetPoint(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string s = hashtable["result"].ToString();
		string str = hashtable["point"].ToString();
		switch (Noah.ConvertResultState(int.Parse(s)))
		{
		}
		string text = "OnGetPoint\n" + Noah.ConvertResultState(int.Parse(s));
		text = text + "\n" + str;
		global::Debug.Log(text);
	}

	public void OnGUID(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string text = hashtable["result"].ToString();
		global::Debug.Log("NoahHandler.OnGUID:" + text);
		switch (Noah.ConvertResultState(int.Parse(text)))
		{
		case Noah.ResultState.Success:
			this.m_isEndSetGUID = true;
			break;
		}
		string message = "OnGUID\n" + Noah.ConvertResultState(int.Parse(text));
		global::Debug.Log(message);
		if (this.m_callbackObject != null)
		{
			this.m_callbackObject.SendMessage("OnGUIDNoah", text, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnPurchased(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string s = hashtable["result"].ToString();
		string s2 = hashtable["goods_count"].ToString();
		switch (Noah.ConvertResultState(int.Parse(s)))
		{
		}
		string text = "OnPurchased\n" + Noah.ConvertResultState(int.Parse(s));
		if (int.Parse(s2) > 0)
		{
			for (int i = 0; i < int.Parse(s2); i++)
			{
				text = text + "\n" + hashtable["goods_id" + i].ToString();
			}
		}
		global::Debug.Log(text);
	}

	public void OnUsedPoint(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string s = hashtable["result"].ToString();
		string str = hashtable["point"].ToString();
		switch (Noah.ConvertResultState(int.Parse(s)))
		{
		}
		string text = "OnUsedPoint\n" + Noah.ConvertResultState(int.Parse(s));
		text = text + "\n" + str;
		global::Debug.Log(text);
	}

	public void OnBannerView(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string s = hashtable["result"].ToString();
		switch (Noah.ConvertResultState(int.Parse(s)))
		{
		}
		string message = "OnBannerView\n" + Noah.ConvertResultState(int.Parse(s));
		global::Debug.Log(message);
	}

	public void OnReview(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string s = hashtable["result"].ToString();
		switch (Noah.ConvertResultState(int.Parse(s)))
		{
		}
		string message = "OnReview\n" + Noah.ConvertResultState(int.Parse(s));
		global::Debug.Log(message);
	}

	public void OnRewardView(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string s = hashtable["result"].ToString();
		switch (Noah.ConvertResultState(int.Parse(s)))
		{
		}
		string message = "OnRewardView\n" + Noah.ConvertResultState(int.Parse(s));
		global::Debug.Log(message);
	}

	public void OnOffer(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string text = hashtable["result"].ToString();
		switch (Noah.ConvertResultState(int.Parse(text)))
		{
		}
		string message = "OnOffer\n" + Noah.ConvertResultState(int.Parse(text));
		global::Debug.Log(message);
		if (this.m_callbackObject != null)
		{
			this.m_callbackObject.SendMessage("OnOfferNoah", text, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnShop(string arg)
	{
		Hashtable hashtable = NewJSON.JsonDecode(arg) as Hashtable;
		if (hashtable == null)
		{
			return;
		}
		string s = hashtable["result"].ToString();
		switch (Noah.ConvertResultState(int.Parse(s)))
		{
		}
		string message = "OnShop\n" + Noah.ConvertResultState(int.Parse(s));
		global::Debug.Log(message);
	}

	public void NoahBannerWallViewControllerDidFnish(string arg)
	{
	}
}
