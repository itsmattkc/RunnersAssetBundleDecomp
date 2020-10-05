using System;
using System.Diagnostics;
using Text;
using UnityEngine;

public class EasySnsFeed
{
	private enum Phase
	{
		START,
		WAIT_PRELOGIN,
		LOGIN,
		WAIT_LOGIN,
		FEED,
		WAIT_FEED,
		GET_INCENTIVE,
		WAIT_GET_INCENTIVE,
		VIEW_INCENTIVE,
		WAIT_VIEW_INCENTIVE,
		COMPLETED,
		FAILED
	}

	public enum Result
	{
		NONE,
		COMPLETED,
		FAILED
	}

	private GameObject m_snsLoginGameObject;

	private SettingPartsSnsLogin m_snsLogin;

	private EasySnsFeed.Phase m_phase;

	private EasySnsFeed.Result m_result;

	private GameObject m_gameObject;

	private string m_caption;

	private string m_text;

	private ui_mm_mileage_page m_mileage_page;

	private EasySnsFeedMonoBehaviour m_easySnsFeedMonoBehaviour;

	private bool m_isLoginOnly;

	public EasySnsFeed(GameObject gameObject, string anchorPath, string caption, string text, ui_mm_mileage_page mileage_page = null)
	{
		this.Init(gameObject, anchorPath, caption, text, mileage_page);
	}

	public EasySnsFeed(GameObject gameObject, string anchorPath)
	{
		this.m_isLoginOnly = true;
		this.Init(gameObject, anchorPath, null, null, null);
	}

	private void Init(GameObject gameObject, string anchorPath, string caption = null, string text = null, ui_mm_mileage_page mileage_page = null)
	{
		this.m_gameObject = gameObject;
		this.m_caption = caption;
		this.m_text = text;
		this.m_mileage_page = mileage_page;
		this.m_easySnsFeedMonoBehaviour = gameObject.GetComponent<EasySnsFeedMonoBehaviour>();
		if (this.m_easySnsFeedMonoBehaviour == null)
		{
			this.m_easySnsFeedMonoBehaviour = gameObject.AddComponent<EasySnsFeedMonoBehaviour>();
		}
		this.m_easySnsFeedMonoBehaviour.Init();
		this.m_snsLoginGameObject = GameObject.Find("EasySnsFeed.SnsLogin");
		if (this.m_snsLoginGameObject == null)
		{
			this.m_snsLoginGameObject = new GameObject("EasySnsFeed.SnsLogin");
		}
		if (this.m_snsLoginGameObject != null)
		{
			this.m_snsLogin = this.m_snsLoginGameObject.GetComponent<SettingPartsSnsLogin>();
			if (this.m_snsLogin == null)
			{
				this.m_snsLogin = this.m_snsLoginGameObject.AddComponent<SettingPartsSnsLogin>();
			}
		}
		if (this.m_snsLogin != null)
		{
			this.m_snsLogin.Setup(anchorPath);
			HudMenuUtility.SetConnectAlertSimpleUI(true);
		}
		else
		{
			this.m_result = EasySnsFeed.Result.FAILED;
		}
	}

	public EasySnsFeed.Result Update()
	{
		if (this.m_result == EasySnsFeed.Result.NONE)
		{
			switch (this.m_phase)
			{
			case EasySnsFeed.Phase.START:
			case EasySnsFeed.Phase.WAIT_PRELOGIN:
				this.m_phase = EasySnsFeed.Phase.LOGIN;
				break;
			case EasySnsFeed.Phase.LOGIN:
				HudMenuUtility.SetConnectAlertSimpleUI(false);
				this.m_snsLogin.SetCancelWindowUseFlag(!this.m_isLoginOnly);
				this.m_snsLogin.PlayStart();
				this.m_phase = EasySnsFeed.Phase.WAIT_LOGIN;
				break;
			case EasySnsFeed.Phase.WAIT_LOGIN:
				if (this.m_snsLogin.IsEnd)
				{
					this.m_phase = ((!this.m_snsLogin.IsCalceled) ? ((!this.m_isLoginOnly) ? EasySnsFeed.Phase.FEED : EasySnsFeed.Phase.COMPLETED) : EasySnsFeed.Phase.FAILED);
				}
				break;
			case EasySnsFeed.Phase.FEED:
			{
				SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
				if (socialInterface != null)
				{
					socialInterface.Feed(this.m_caption, this.m_text, this.m_gameObject);
				}
				this.m_phase = ((!(socialInterface != null)) ? EasySnsFeed.Phase.FAILED : EasySnsFeed.Phase.WAIT_FEED);
				break;
			}
			case EasySnsFeed.Phase.WAIT_FEED:
			{
				bool? isFeeded = this.m_easySnsFeedMonoBehaviour.m_isFeeded;
				if (isFeeded.HasValue)
				{
					this.m_phase = ((!(this.m_easySnsFeedMonoBehaviour.m_isFeeded == true)) ? EasySnsFeed.Phase.FAILED : EasySnsFeed.Phase.COMPLETED);
				}
				break;
			}
			case EasySnsFeed.Phase.GET_INCENTIVE:
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					loggedInServerInterface.RequestServerGetFacebookIncentive(2, 0, this.m_gameObject);
				}
				this.m_phase = ((!(loggedInServerInterface != null)) ? EasySnsFeed.Phase.FAILED : EasySnsFeed.Phase.WAIT_GET_INCENTIVE);
				break;
			}
			case EasySnsFeed.Phase.WAIT_GET_INCENTIVE:
				if (this.m_easySnsFeedMonoBehaviour.m_feedIncentiveList != null)
				{
					this.m_phase = EasySnsFeed.Phase.VIEW_INCENTIVE;
				}
				break;
			case EasySnsFeed.Phase.VIEW_INCENTIVE:
				if (this.m_easySnsFeedMonoBehaviour.m_feedIncentiveList.Count > 0)
				{
					ServerPresentState serverPresentState = this.m_easySnsFeedMonoBehaviour.m_feedIncentiveList[0];
					this.m_easySnsFeedMonoBehaviour.m_feedIncentiveList.RemoveAt(0);
					ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
					if (itemGetWindow != null)
					{
						itemGetWindow.Create(new ItemGetWindow.CInfo
						{
							caption = TextUtility.GetCommonText("SnsFeed", "gw_feed_incentive_caption"),
							serverItemId = serverPresentState.m_itemId,
							imageCount = TextUtility.GetCommonText("SnsFeed", "gw_feed_incentive_text", "{COUNT}", HudUtility.GetFormatNumString<int>(serverPresentState.m_numItem))
						});
					}
					HudMenuUtility.SendMsgUpdateSaveDataDisplay();
					this.m_phase = EasySnsFeed.Phase.WAIT_VIEW_INCENTIVE;
				}
				else
				{
					HudMenuUtility.SendMsgUpdateSaveDataDisplay();
					this.m_phase = EasySnsFeed.Phase.COMPLETED;
				}
				break;
			case EasySnsFeed.Phase.WAIT_VIEW_INCENTIVE:
			{
				ItemGetWindow itemGetWindow2 = ItemGetWindowUtil.GetItemGetWindow();
				if (itemGetWindow2 != null && itemGetWindow2.IsEnd)
				{
					itemGetWindow2.Reset();
					this.m_phase = EasySnsFeed.Phase.VIEW_INCENTIVE;
				}
				break;
			}
			case EasySnsFeed.Phase.COMPLETED:
				this.m_result = EasySnsFeed.Result.COMPLETED;
				break;
			case EasySnsFeed.Phase.FAILED:
				this.m_result = EasySnsFeed.Result.FAILED;
				break;
			}
		}
		return this.m_result;
	}

	[Conditional("DEBUG_INFO")]
	public static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}
}
