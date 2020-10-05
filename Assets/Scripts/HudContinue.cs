using Message;
using System;
using Text;
using UnityEngine;

public class HudContinue : MonoBehaviour
{
	private enum State
	{
		IDLE,
		SERVER_CONNECT_WAIT,
		ASK_CONTINUE_START,
		ASK_CONTINUE,
		BUY_RED_STAR_RING_START,
		BUY_RED_STAR_RING,
		PURCHASE_COMPLETED,
		WAIT_VIDEO_RESPONSE,
		NUM
	}

	private HudContinueWindow m_continueWindow;

	private HudContinueBuyRsRing m_buyRsRingWindow;

	private int m_cmWatchingMaxCount;

	private int m_watchingCount;

	private int m_beforeRsRingCount;

	private HudContinue.State m_state;

	private void Start()
	{
		if (ServerInterface.SettingState != null)
		{
			this.m_cmWatchingMaxCount = ServerInterface.SettingState.m_onePlayCmCount;
		}
	}

	public void SetTimeUp(bool timeUp)
	{
		if (this.m_continueWindow != null)
		{
			this.m_continueWindow.SetTimeUpObj(timeUp);
		}
	}

	public void PlayStart()
	{
		this.m_state = HudContinue.State.ASK_CONTINUE_START;
		this.m_beforeRsRingCount = HudContinue.GetCurrentRsRingCount();
	}

	public void PushBackKey()
	{
		if (this.m_state == HudContinue.State.ASK_CONTINUE)
		{
			if (this.m_continueWindow != null)
			{
				this.m_continueWindow.OnPushBackKey();
			}
		}
		else if (this.m_state == HudContinue.State.BUY_RED_STAR_RING && this.m_buyRsRingWindow != null)
		{
			this.m_buyRsRingWindow.OnPushBackKey();
		}
	}

	public void Setup(bool bossStage)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Continue_window");
		if (gameObject != null)
		{
			this.m_continueWindow = gameObject.AddComponent<HudContinueWindow>();
			this.m_continueWindow.Setup(bossStage);
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "simple_shop_window");
		if (gameObject2 != null)
		{
			this.m_buyRsRingWindow = gameObject2.AddComponent<HudContinueBuyRsRing>();
			this.m_buyRsRingWindow.Setup();
		}
	}

	private void Update()
	{
		switch (this.m_state)
		{
		case HudContinue.State.ASK_CONTINUE_START:
			if (this.m_continueWindow != null)
			{
				this.m_continueWindow.SetVideoButton(this.m_watchingCount < this.m_cmWatchingMaxCount);
				this.m_continueWindow.PlayStart();
			}
			this.m_state = HudContinue.State.ASK_CONTINUE;
			break;
		case HudContinue.State.ASK_CONTINUE:
			if (this.m_continueWindow != null)
			{
				int currentRsRingCount = HudContinue.GetCurrentRsRingCount();
				if (this.m_continueWindow.IsYesButtonPressed)
				{
					int redStarRingCount = HudContinueUtility.GetRedStarRingCount();
					int continueCost = HudContinueUtility.GetContinueCost();
					if (redStarRingCount >= continueCost)
					{
						ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
						if (loggedInServerInterface != null)
						{
							loggedInServerInterface.RequestServerActRetry(base.gameObject);
							this.m_state = HudContinue.State.SERVER_CONNECT_WAIT;
						}
						else
						{
							this.ServerActRetry_Succeeded(null);
						}
					}
					else
					{
						this.m_state = HudContinue.State.BUY_RED_STAR_RING_START;
					}
				}
				else if (this.m_continueWindow.IsNoButtonPressed)
				{
					MsgContinueResult value = new MsgContinueResult(false);
					GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnSendToGameModeStage", value, SendMessageOptions.DontRequireReceiver);
					this.m_state = HudContinue.State.IDLE;
				}
				else if (this.m_continueWindow.IsVideoButtonPressed)
				{
					this.m_state = HudContinue.State.WAIT_VIDEO_RESPONSE;
				}
				else if (this.m_beforeRsRingCount < currentRsRingCount)
				{
					GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
					info.caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "gw_item_caption").text;
					string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Item", "red_star_ring").text;
					text += " ";
					TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Shop", "gw_purchase_success_text");
					text2.ReplaceTag("{COUNT}", (currentRsRingCount - this.m_beforeRsRingCount).ToString());
					text += text2.text;
					info.message = text;
					info.anchor_path = "Camera/Anchor_5_MC";
					info.buttonType = GeneralWindow.ButtonType.Ok;
					info.name = "PurchaseCompleted";
					GeneralWindow.Create(info);
					this.m_beforeRsRingCount = currentRsRingCount;
					this.m_state = HudContinue.State.PURCHASE_COMPLETED;
				}
			}
			break;
		case HudContinue.State.BUY_RED_STAR_RING_START:
			if (ServerInterface.IsRSREnable())
			{
				if (this.m_buyRsRingWindow != null)
				{
					this.m_buyRsRingWindow.PlayStart();
				}
			}
			else
			{
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "ErrorRSRing",
					buttonType = GeneralWindow.ButtonType.Ok,
					caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "gw_cost_caption"),
					message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "gw_cost_caption_text_2"),
					isPlayErrorSe = true
				});
			}
			this.m_state = HudContinue.State.BUY_RED_STAR_RING;
			break;
		case HudContinue.State.BUY_RED_STAR_RING:
			if (ServerInterface.IsRSREnable())
			{
				if (this.m_buyRsRingWindow != null && this.m_buyRsRingWindow.IsEndPlay)
				{
					if (this.m_buyRsRingWindow.IsSuccess || this.m_buyRsRingWindow.IsCanceled)
					{
						this.m_state = HudContinue.State.ASK_CONTINUE_START;
					}
					else if (this.m_buyRsRingWindow.IsFailed)
					{
						this.m_state = HudContinue.State.BUY_RED_STAR_RING_START;
					}
				}
			}
			else if (GeneralWindow.IsCreated("ErrorRSRing") && GeneralWindow.IsButtonPressed)
			{
				this.m_state = HudContinue.State.ASK_CONTINUE_START;
				GeneralWindow.Close();
			}
			break;
		case HudContinue.State.PURCHASE_COMPLETED:
			if (GeneralWindow.IsCreated("PurchaseCompleted") && GeneralWindow.IsButtonPressed)
			{
				this.m_state = HudContinue.State.ASK_CONTINUE_START;
			}
			break;
		}
	}

	private void SendContinuResult(bool continueFlag)
	{
		MsgContinueResult value = new MsgContinueResult(continueFlag);
		GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnSendToGameModeStage", value, SendMessageOptions.DontRequireReceiver);
	}

	private void ServerActRetry_Succeeded(MsgActRetrySucceed msg)
	{
		int continueCost = HudContinueUtility.GetContinueCost();
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerInterface.PlayerState.m_numRedRings -= continueCost;
			SaveDataManager.Instance.ItemData.RedRingCount = (uint)ServerInterface.PlayerState.m_numRedRings;
		}
		this.SendContinuResult(true);
		this.m_state = HudContinue.State.IDLE;
	}

	private void ServerActRetryFree_Succeeded(MsgActRetryFreeSucceed msg)
	{
		this.SendContinuResult(true);
		this.m_state = HudContinue.State.IDLE;
	}

	private static int GetCurrentRsRingCount()
	{
		int result = 0;
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			result = ServerInterface.PlayerState.m_numRedRings;
		}
		else
		{
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				result = (int)instance.ItemData.RedRingCount;
			}
		}
		return result;
	}
}
