using DataTable;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class DailyInfoBattle : MonoBehaviour
{
	private const float UPDATE_LIMIT_TIME = 0.25f;

	private const float RELOAD_TIME = 3f;

	private DailyInfo m_parent;

	private DateTime m_currentEndTime;

	private int m_currentWinFlag;

	private ServerDailyBattleDataPair m_currentData;

	private ServerDailyBattleStatus m_currentStatus;

	private UILabel m_wins;

	private UILabel m_limitTime;

	private UIButton m_reloadBtn;

	private List<UIImageButton> m_matchingBtns;

	private GameObject m_myDataObject;

	private GameObject m_rivalDataObject;

	private GameObject m_noMatchingObject;

	private GameObject m_noScoreObject;

	private GameObject m_menuObject;

	private GameObject m_myWin;

	private GameObject m_myLose;

	private GameObject m_rivalWin;

	private GameObject m_rivalLose;

	private GameObject m_vsObject;

	private UIRectItemStorage m_myDataStorage;

	private UIRectItemStorage m_rivalDataStorage;

	private ui_ranking_scroll m_myData;

	private ui_ranking_scroll m_rivalData;

	private int m_matchingIndex;

	private float m_time;

	private float m_updateLimitTime;

	private float m_reloadTime;

	private string m_matchingOldUserId = string.Empty;

	private bool m_matchingResultEnd;

	private bool m_costError;

	private bool m_isEnableRematch = true;

	private ButtonInfoTable.ButtonType m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;

	private bool isEffectVS
	{
		get
		{
			bool result = true;
			if (GeneralWindow.Created || ranking_window.isActive)
			{
				result = false;
			}
			return result;
		}
	}

	private void Update()
	{
		if (this.m_time > 0f && base.gameObject.activeSelf)
		{
			float deltaTime = Time.deltaTime;
			if (this.m_reloadTime > 0f)
			{
				this.m_reloadTime -= deltaTime;
				if (this.m_matchingResultEnd && this.m_reloadTime <= 2.5f)
				{
					HudMenuUtility.SetConnectAlertSimpleUI(false);
					this.m_matchingResultEnd = false;
				}
				if (this.m_reloadTime <= 0f)
				{
					if (this.m_reloadBtn != null && !this.m_reloadBtn.isEnabled)
					{
						DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
						if (instance != null && instance.IsRequestResetMatching())
						{
							this.m_reloadBtn.isEnabled = true;
							this.m_reloadTime = 0f;
							HudMenuUtility.SetConnectAlertSimpleUI(false);
						}
						else
						{
							this.m_reloadTime = 3f;
						}
					}
					if (this.m_matchingBtns != null && this.m_matchingBtns.Count > 0)
					{
						foreach (UIImageButton current in this.m_matchingBtns)
						{
							if (!current.isEnabled)
							{
								DailyBattleManager instance2 = SingletonGameObject<DailyBattleManager>.Instance;
								if (instance2 != null && instance2.IsRequestResetMatching())
								{
									current.isEnabled = true;
									this.m_reloadTime = 0f;
									HudMenuUtility.SetConnectAlertSimpleUI(false);
								}
								else
								{
									this.m_reloadTime = 3f;
								}
							}
						}
					}
					this.m_reloadTime = 0f;
				}
			}
			this.m_updateLimitTime -= deltaTime;
			if (this.m_updateLimitTime <= 0f)
			{
				if (this.m_limitTime != null)
				{
					string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_limit_time");
					string timeLimitString = GeneralUtil.GetTimeLimitString(this.m_currentEndTime, false);
					this.m_limitTime.text = text.Replace("{TIME}", timeLimitString);
				}
				this.m_updateLimitTime = 0.25f;
			}
			this.m_time += Time.deltaTime;
		}
		if (this.m_vsObject != null)
		{
			this.m_vsObject.SetActive(this.isEffectVS);
		}
		if (this.m_matchingIndex > 0 && GeneralWindow.IsCreated("ShowMatchingInfo") && GeneralWindow.IsButtonPressed)
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				GeneralWindow.Close();
				global::Debug.Log("DailyInfoBattle.Update.Maching");
				this.Matching(this.m_matchingIndex);
			}
			else
			{
				this.m_matchingIndex = 0;
				this.m_isEnableRematch = true;
			}
		}
		if (this.m_costError)
		{
			if (GeneralWindow.IsYesButtonPressed)
			{
				HudMenuUtility.SendMenuButtonClicked(this.m_costErrorType, false);
			}
			this.m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;
			GeneralWindow.Close();
			this.m_costError = false;
			this.m_isEnableRematch = true;
		}
	}

	public void Setup(DailyInfo parent)
	{
		this.m_parent = parent;
		DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
		if (instance != null)
		{
			this.m_myDataObject = GameObjectUtil.FindChildGameObject(base.gameObject, "duel_mine_set");
			this.m_rivalDataObject = GameObjectUtil.FindChildGameObject(base.gameObject, "duel_adversary_set");
			this.m_noMatchingObject = GameObjectUtil.FindChildGameObject(base.gameObject, "no_matching_set");
			this.m_noScoreObject = GameObjectUtil.FindChildGameObject(base.gameObject, "no_score_set");
			this.m_menuObject = GameObjectUtil.FindChildGameObject(base.gameObject, "time");
			this.m_wins = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_wins");
			this.m_limitTime = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_timelimit");
			this.m_reloadBtn = null;
			this.m_matchingIndex = 0;
			this.m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;
			this.m_costError = false;
			if (this.m_matchingBtns != null)
			{
				this.m_matchingBtns.Clear();
			}
			this.m_matchingBtns = null;
			GeneralUtil.SetButtonFunc(base.gameObject, "Btn_battle_help", base.gameObject, "OnClickReward");
			if (this.m_wins != null)
			{
				this.m_wins.text = string.Empty;
			}
			if (this.m_limitTime != null)
			{
				this.m_limitTime.text = string.Empty;
			}
			if (this.m_menuObject != null)
			{
				this.m_menuObject.SetActive(false);
			}
			if (this.m_myDataObject != null)
			{
				this.m_myDataStorage = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.m_myDataObject, "score_mine");
				if (this.m_myDataStorage != null)
				{
					this.m_myDataStorage.maxItemCount = (this.m_myDataStorage.maxRows = 0);
					this.m_myDataStorage.Restart();
				}
				this.m_myWin = GameObjectUtil.FindChildGameObject(this.m_myDataObject, "duel_win_set");
				this.m_myLose = GameObjectUtil.FindChildGameObject(this.m_myDataObject, "duel_lose_set");
				if (this.m_myWin != null && this.m_myLose != null)
				{
					this.m_myWin.SetActive(false);
					this.m_myLose.SetActive(false);
				}
				this.m_myDataObject.SetActive(false);
			}
			if (this.m_rivalDataObject != null)
			{
				this.m_vsObject = GameObjectUtil.FindChildGameObject(this.m_rivalDataObject, "deco");
				this.m_rivalDataStorage = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.m_rivalDataObject, "score_adversary");
				if (this.m_rivalDataStorage != null)
				{
					this.m_rivalDataStorage.maxItemCount = (this.m_rivalDataStorage.maxRows = 0);
					this.m_rivalDataStorage.Restart();
				}
				this.m_rivalWin = GameObjectUtil.FindChildGameObject(this.m_rivalDataObject, "duel_win_set");
				this.m_rivalLose = GameObjectUtil.FindChildGameObject(this.m_rivalDataObject, "duel_lose_set");
				if (this.m_rivalWin != null && this.m_rivalLose != null)
				{
					this.m_rivalWin.SetActive(false);
					this.m_rivalLose.SetActive(false);
				}
				GeneralUtil.SetButtonFunc(base.gameObject, "Btn_matching_0", base.gameObject, "OnClickMatching1");
				GeneralUtil.SetButtonFunc(base.gameObject, "Btn_matching_1", base.gameObject, "OnClickMatching2");
				UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_matching_0");
				UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_matching_1");
				Dictionary<int, ServerConsumedCostData> resetCostList = instance.resetCostList;
				if (uIImageButton != null)
				{
					if (this.m_matchingBtns == null)
					{
						this.m_matchingBtns = new List<UIImageButton>();
					}
					this.m_matchingBtns.Add(uIImageButton);
					UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(uIImageButton.gameObject, "Lbl_bet_number");
					if (uILabel != null)
					{
						if (resetCostList != null && resetCostList.ContainsKey(1))
						{
							uILabel.text = "×" + resetCostList[1].numItem;
						}
						else
						{
							uILabel.text = "---";
						}
					}
				}
				if (uIImageButton2 != null)
				{
					if (this.m_matchingBtns == null)
					{
						this.m_matchingBtns = new List<UIImageButton>();
					}
					this.m_matchingBtns.Add(uIImageButton2);
					UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(uIImageButton2.gameObject, "Lbl_bet_number");
					if (uILabel2 != null)
					{
						if (resetCostList != null && resetCostList.ContainsKey(2))
						{
							uILabel2.text = "×" + resetCostList[2].numItem;
						}
						else
						{
							uILabel2.text = "---";
						}
					}
				}
				this.m_rivalDataObject.SetActive(false);
			}
			if (this.m_noMatchingObject != null)
			{
				GeneralUtil.SetButtonFunc(base.gameObject, "Btn_reload", base.gameObject, "OnClickMatching0");
				this.m_noMatchingObject.SetActive(false);
			}
			if (this.m_noScoreObject != null)
			{
				this.m_noScoreObject.SetActive(false);
			}
			this.m_time = 0f;
			this.m_updateLimitTime = 0f;
			if (instance.IsEndTimeOver())
			{
				instance.FirstSetup(new DailyBattleManager.CallbackSetup(this.DailyBattleManagerCallBack));
			}
			else
			{
				this.SetupParam();
			}
		}
	}

	private void DailyBattleManagerCallBack()
	{
		this.SetupParam();
	}

	private void SetupParam()
	{
		DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
		if (instance != null)
		{
			bool flag = true;
			bool flag2 = true;
			this.m_currentWinFlag = instance.currentWinFlag;
			this.m_currentEndTime = instance.currentEndTime;
			this.m_currentData = instance.currentDataPair;
			this.m_currentStatus = instance.currentStatus;
			this.m_matchingResultEnd = false;
			if (this.m_wins != null)
			{
				if (this.m_currentStatus != null && this.m_currentStatus.goOnWin > 1)
				{
					string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_continuous_win");
					this.m_wins.text = text.Replace("{PARAM}", this.m_currentStatus.goOnWin.ToString());
				}
				else
				{
					this.m_wins.text = string.Empty;
				}
			}
			if (this.m_limitTime != null)
			{
				string text2 = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_limit_time");
				string timeLimitString = GeneralUtil.GetTimeLimitString(this.m_currentEndTime, false);
				this.m_limitTime.text = text2.Replace("{TIME}", timeLimitString);
			}
			if (this.m_currentData != null && !string.IsNullOrEmpty(this.m_currentData.myBattleData.userId))
			{
				flag = false;
			}
			if (this.m_currentData != null && !string.IsNullOrEmpty(this.m_currentData.rivalBattleData.userId) && !flag)
			{
				flag2 = false;
			}
			if (this.m_matchingBtns != null && this.m_matchingBtns.Count > 0 && this.m_reloadTime <= 0f)
			{
				foreach (UIImageButton current in this.m_matchingBtns)
				{
					current.isEnabled = true;
				}
			}
			if (!flag && flag2)
			{
				this.m_reloadBtn = GameObjectUtil.FindChildGameObjectComponent<UIButton>(base.gameObject, "Btn_reload");
				if (this.m_reloadBtn != null && this.m_reloadTime <= 0f)
				{
					this.m_reloadBtn.isEnabled = true;
				}
			}
			else
			{
				this.m_reloadBtn = null;
			}
			if (this.m_menuObject != null)
			{
				this.m_menuObject.SetActive(true);
			}
			if (this.m_noScoreObject != null)
			{
				this.m_noScoreObject.SetActive(flag);
			}
			if (this.m_noMatchingObject != null)
			{
				this.m_noMatchingObject.SetActive(flag2 && !flag);
			}
			if (this.m_myDataObject != null)
			{
				if (this.m_myDataStorage != null && !flag)
				{
					this.m_myDataStorage.maxItemCount = (this.m_myDataStorage.maxRows = 1);
					this.m_myDataStorage.Restart();
					this.m_myData = GameObjectUtil.FindChildGameObjectComponent<ui_ranking_scroll>(this.m_myDataStorage.gameObject, "ui_ranking_scroll(Clone)");
					if (this.m_myData != null)
					{
						RankingUtil.Ranker ranker = new RankingUtil.Ranker(this.m_currentData.myBattleData);
						this.m_myData.UpdateView(RankingUtil.RankingScoreType.HIGH_SCORE, RankingUtil.RankingRankerType.RIVAL, ranker, true);
						this.m_myData.SetMyRanker(true);
					}
					if (this.m_myWin != null && this.m_myLose != null)
					{
						if (!flag2 && this.m_currentWinFlag > 0)
						{
							if (this.m_currentWinFlag >= 2)
							{
								this.m_myWin.SetActive(true);
								this.m_myLose.SetActive(false);
							}
							else
							{
								this.m_myWin.SetActive(false);
								this.m_myLose.SetActive(true);
							}
						}
						else
						{
							this.m_myWin.SetActive(false);
							this.m_myLose.SetActive(false);
						}
					}
				}
				this.m_myDataObject.SetActive(!flag);
			}
			if (this.m_rivalDataObject != null)
			{
				if (this.m_rivalDataStorage != null && !flag2)
				{
					this.m_rivalDataStorage.maxItemCount = (this.m_rivalDataStorage.maxRows = 1);
					this.m_rivalDataStorage.Restart();
					this.m_rivalData = GameObjectUtil.FindChildGameObjectComponent<ui_ranking_scroll>(this.m_rivalDataStorage.gameObject, "ui_ranking_scroll(Clone)");
					if (this.m_rivalData != null)
					{
						RankingUtil.Ranker ranker2 = new RankingUtil.Ranker(this.m_currentData.rivalBattleData);
						if (ranker2 != null && ranker2.isFriend)
						{
							ranker2.isSentEnergy = true;
						}
						this.m_rivalData.UpdateView(RankingUtil.RankingScoreType.HIGH_SCORE, RankingUtil.RankingRankerType.RIVAL, ranker2, true);
						this.m_rivalData.SendButtonDisable = true;
					}
				}
				if (this.m_rivalWin != null && this.m_rivalLose != null)
				{
					if (!flag2 && this.m_currentWinFlag > 0)
					{
						if (this.m_currentWinFlag >= 2)
						{
							this.m_rivalWin.SetActive(false);
							this.m_rivalLose.SetActive(true);
						}
						else
						{
							this.m_rivalWin.SetActive(true);
							this.m_rivalLose.SetActive(false);
						}
					}
					else
					{
						this.m_rivalWin.SetActive(false);
						this.m_rivalLose.SetActive(false);
					}
				}
				this.m_rivalDataObject.SetActive(!flag2);
			}
		}
		this.m_time = 0.0001f;
		this.m_updateLimitTime = 0.25f;
	}

	private void UpdateRectItemStorage(UIRectItemStorage storage, RankingUtil.Ranker ranker, bool myFlag)
	{
		storage.maxItemCount = (storage.maxRows = 1);
		storage.Restart();
		ui_ranking_scroll[] componentsInChildren = storage.GetComponentsInChildren<ui_ranking_scroll>(true);
		for (int i = 0; i < storage.maxItemCount; i++)
		{
			if (ranker != null)
			{
				componentsInChildren[i].UpdateView(RankingUtil.RankingScoreType.HIGH_SCORE, RankingUtil.RankingRankerType.ALL, ranker, true);
				componentsInChildren[i].SetMyRanker(myFlag);
			}
		}
	}

	private void OnClickReward()
	{
		this.Reward();
	}

	private void OnClickMatching0()
	{
		if (!this.m_isEnableRematch)
		{
			return;
		}
		this.m_isEnableRematch = false;
		if (this.m_parent != null)
		{
			this.m_matchingIndex = 0;
			global::Debug.Log("DailyInfoBattle.Update.OnClickMaching0");
			this.Matching(0);
		}
	}

	private void OnClickMatching1()
	{
		if (!this.m_isEnableRematch)
		{
			return;
		}
		this.m_isEnableRematch = false;
		if (this.m_parent != null)
		{
			this.m_matchingIndex = 1;
			this.ShowMatchingInfo();
		}
	}

	private void OnClickMatching2()
	{
		if (!this.m_isEnableRematch)
		{
			return;
		}
		this.m_isEnableRematch = false;
		if (this.m_parent != null)
		{
			this.m_matchingIndex = 2;
			this.ShowMatchingInfo();
		}
	}

	private void Reward()
	{
		DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
		if (instance != null)
		{
			List<ServerDailyBattlePrizeData> currentPrizeList = instance.currentPrizeList;
			if (currentPrizeList != null && currentPrizeList.Count > 0)
			{
				this.ShowReward(currentPrizeList);
			}
			else if (GeneralUtil.IsNetwork())
			{
				DailyBattleManager.CallbackGetPrize callback = new DailyBattleManager.CallbackGetPrize(this.CallbackGetPrizeFunc);
				if (!instance.RequestGetPrize(callback))
				{
					HudMenuUtility.SetConnectAlertSimpleUI(false);
				}
			}
			else
			{
				GeneralUtil.ShowNoCommunication("ShowNoCommunication");
			}
		}
	}

	private void Matching(int resetIndex)
	{
		if (GeneralUtil.IsNetwork())
		{
			this.m_matchingIndex = 0;
			this.m_matchingResultEnd = false;
			this.m_costErrorType = ButtonInfoTable.ButtonType.UNKNOWN;
			this.m_costError = false;
			DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
			if (instance != null)
			{
				Dictionary<int, ServerConsumedCostData> resetCostList = instance.resetCostList;
				if (resetCostList != null && resetCostList.ContainsKey(resetIndex))
				{
					ServerConsumedCostData serverConsumedCostData = resetCostList[resetIndex];
					if (serverConsumedCostData != null && serverConsumedCostData.itemId > 0)
					{
						ServerItem.Id itemId = (ServerItem.Id)serverConsumedCostData.itemId;
						long itemCount = GeneralUtil.GetItemCount(itemId);
						if (itemCount >= (long)serverConsumedCostData.numItem || resetIndex == 0)
						{
							this.m_matchingOldUserId = string.Empty;
							if (this.m_currentData != null && this.m_currentData.rivalBattleData != null)
							{
								this.m_matchingOldUserId = this.m_currentData.rivalBattleData.userId;
							}
							global::Debug.Log("DailyInfoBattle.Maching!");
							HudMenuUtility.SetConnectAlertSimpleUI(true);
							DailyBattleManager.CallbackResetMatching callback = new DailyBattleManager.CallbackResetMatching(this.CallbackResetMatchingFunc);
							if (instance.RequestResetMatching(resetIndex, callback))
							{
								if (this.m_reloadBtn != null)
								{
									this.m_reloadBtn.isEnabled = false;
									this.m_reloadTime = 3f;
								}
								if (this.m_matchingBtns != null)
								{
									foreach (UIImageButton current in this.m_matchingBtns)
									{
										current.isEnabled = false;
									}
									this.m_reloadTime = 3f;
								}
							}
							else
							{
								HudMenuUtility.SetConnectAlertSimpleUI(false);
							}
							SoundManager.SePlay("sys_menu_decide", "SE");
						}
						else if (itemId == ServerItem.Id.RSRING)
						{
							bool flag = ServerInterface.IsRSREnable();
							GeneralWindow.Create(new GeneralWindow.CInfo
							{
								caption = TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption"),
								message = ((!flag) ? TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption_text_2") : TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption_text")),
								buttonType = ((!flag) ? GeneralWindow.ButtonType.Ok : GeneralWindow.ButtonType.ShopCancel),
								finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowClosedCallback),
								name = "MatchingCostErrorRSRing",
								isPlayErrorSe = true
							});
							this.m_costErrorType = ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP;
						}
						else if (itemId == ServerItem.Id.RING)
						{
							GeneralWindow.Create(new GeneralWindow.CInfo
							{
								caption = TextUtility.GetCommonText("ItemRoulette", "gw_cost_caption"),
								message = TextUtility.GetCommonText("ItemRoulette", "gw_cost_text"),
								buttonType = GeneralWindow.ButtonType.ShopCancel,
								name = "MatchingCostErrorRing",
								finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowClosedCallback),
								isPlayErrorSe = true
							});
							this.m_costErrorType = ButtonInfoTable.ButtonType.RING_TO_SHOP;
						}
						else
						{
							global::Debug.Log("DailyInfoBattle Matching error   itemId:" + itemId + " !!!!!");
						}
					}
				}
			}
		}
		else
		{
			GeneralUtil.ShowNoCommunication("ShowNoCommunication");
		}
	}

	private void GeneralWindowClosedCallback()
	{
		this.m_costError = true;
		HudMenuUtility.SetConnectAlertSimpleUI(false);
	}

	private void CallbackGetPrizeFunc(List<ServerDailyBattlePrizeData> prizeList)
	{
		HudMenuUtility.SetConnectAlertSimpleUI(false);
		this.ShowReward(prizeList);
	}

	private void CallbackResetMatchingFunc(ServerPlayerState playerStatus, ServerDailyBattleDataPair dataPair, DateTime endTime)
	{
		HudMenuUtility.SetConnectAlertSimpleUI(false);
		this.m_isEnableRematch = true;
		if (playerStatus != null && dataPair != null)
		{
			this.m_currentData = dataPair;
			this.SetupParam();
			this.ShowMatchingResult(this.m_currentData);
		}
		else
		{
			global::Debug.Log("CallbackResetMatchingFunc error");
			this.SetupParam();
			this.ShowMatchingResult(null);
		}
	}

	private void ShowReward(List<ServerDailyBattlePrizeData> data)
	{
		if (data != null && data.Count > 0)
		{
			List<ServerRemainOperator> list = ServerDailyBattlePrizeData.ConvertRemainOperatorList(data);
			if (list != null)
			{
				int goOnWin = this.m_currentStatus.goOnWin;
				string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_win");
				string text2 = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_win_over");
				string itemText = RankingLeagueTable.GetItemText(list, text, text2, goOnWin - 1, false);
				if (!string.IsNullOrEmpty(itemText))
				{
					GeneralWindow.Create(new GeneralWindow.CInfo
					{
						name = "ShowReward",
						buttonType = GeneralWindow.ButtonType.Ok,
						caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_reward_caption"),
						message = itemText
					});
				}
			}
		}
		else
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "ShowReward",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_reward_caption"),
				message = "Reward Failed",
				isPlayErrorSe = true
			});
		}
	}

	private void ShowMatchingInfo()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "ShowMatchingInfo",
			buttonType = GeneralWindow.ButtonType.YesNo,
			caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_reset_caption"),
			message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_reset_text")
		});
	}

	private void ShowMatchingResult(ServerDailyBattleDataPair data)
	{
		bool flag = false;
		if (data != null && data.rivalBattleData != null)
		{
			flag = (!(this.m_matchingOldUserId == data.rivalBattleData.userId) && !string.IsNullOrEmpty(data.rivalBattleData.userId));
		}
		if (flag)
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "ShowMatchingResult",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_reset_result_caption"),
				message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_reset_result_succeed")
			});
		}
		else
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "ShowMatchingResult",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_reset_result_caption"),
				message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_reset_result_failure"),
				isPlayErrorSe = true
			});
		}
		this.m_matchingIndex = 0;
		this.m_matchingResultEnd = true;
		this.m_matchingOldUserId = string.Empty;
		HudMenuUtility.SetConnectAlertSimpleUI(false);
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}
}
