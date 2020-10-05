using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class DailyInfoHistory : MonoBehaviour
{
	private const int MAX_HISTORY_DATA = 30;

	private DailyInfo m_parent;

	private UILabel m_wins;

	private UILabel m_winStreak;

	private GameObject m_scrollObject;

	private UIRectItemStorage m_scrollStorage;

	private List<ui_daily_battle_scroll> m_historyList;

	public void Setup(DailyInfo parent)
	{
		this.m_parent = parent;
		global::Debug.Log("DailyInfoHistory Setup parent:" + (this.m_parent != null));
		DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
		if (instance != null)
		{
			this.m_wins = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_wins");
			this.m_winStreak = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_win_streak");
			if (this.m_historyList != null)
			{
				this.m_historyList.Clear();
			}
			this.m_scrollObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Scrollsystem");
			if (this.m_scrollObject != null)
			{
				this.m_scrollStorage = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.m_scrollObject, "slot");
				if (this.m_scrollStorage != null)
				{
					this.m_scrollStorage.maxItemCount = (this.m_scrollStorage.maxRows = 0);
					this.m_scrollStorage.Restart();
				}
			}
			if (this.m_wins != null)
			{
				this.m_wins.text = string.Empty;
			}
			if (this.m_winStreak != null)
			{
				this.m_winStreak.text = string.Empty;
			}
			bool flag = true;
			global::Debug.Log("currentDataPairList:" + ((instance.currentDataPairList == null) ? "null" : instance.currentDataPairList.Count.ToString()));
			if (instance.currentDataPairList != null)
			{
				if (instance.currentDataPairList.Count > 0)
				{
					if (!instance.IsReload(DailyBattleManager.REQ_TYPE.GET_DATA_HISTORY, 60.0))
					{
						flag = false;
					}
				}
				else if (instance.IsDataInit(DailyBattleManager.REQ_TYPE.GET_DATA_HISTORY) && !instance.IsReload(DailyBattleManager.REQ_TYPE.GET_DATA_HISTORY, 5.0))
				{
					flag = false;
				}
			}
			if (instance.IsEndTimeOver() || flag)
			{
				if (GeneralUtil.IsNetwork())
				{
					DailyBattleManager.CallbackGetDataHistory callback = new DailyBattleManager.CallbackGetDataHistory(this.CallbackHistory);
					if (instance.RequestGetDataHistory(30, callback))
					{
						HudMenuUtility.SetConnectAlertSimpleUI(true);
					}
					else
					{
						this.SetupParam();
					}
				}
				else
				{
					GeneralUtil.ShowNoCommunication("ShowNoCommunicationDailyInfoHistory");
				}
			}
			else
			{
				this.SetupParam();
			}
		}
	}

	private void CallbackHistory(List<ServerDailyBattleDataPair> list)
	{
		this.SetupParam();
	}

	private void SetupParam()
	{
		DailyBattleManager instance = SingletonGameObject<DailyBattleManager>.Instance;
		if (instance != null)
		{
			List<ServerDailyBattleDataPair> currentDataPairList = instance.currentDataPairList;
			if (currentDataPairList != null && currentDataPairList.Count > 0)
			{
				this.m_scrollStorage.maxItemCount = (this.m_scrollStorage.maxRows = currentDataPairList.Count);
				this.m_scrollStorage.Restart();
				this.m_historyList = GameObjectUtil.FindChildGameObjectsComponents<ui_daily_battle_scroll>(this.m_scrollStorage.gameObject, "ui_daily_battle_scroll(Clone)");
				if (this.m_historyList != null && this.m_historyList.Count > 0)
				{
					for (int i = 0; i < currentDataPairList.Count; i++)
					{
						if (currentDataPairList[i] != null && this.m_historyList.Count > i)
						{
							this.m_historyList[i].UpdateView(currentDataPairList[i]);
						}
					}
				}
			}
			if (instance.currentStatus != null)
			{
				if (this.m_wins != null)
				{
					string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_result");
					this.m_wins.text = TextUtility.Replaces(text, new Dictionary<string, string>
					{
						{
							"{WIN}",
							instance.currentStatus.numWin.ToString()
						},
						{
							"{LOSE}",
							instance.currentStatus.numLose.ToString()
						},
						{
							"{FAILURE}",
							instance.currentStatus.numLoseByDefault.ToString()
						}
					});
				}
				if (this.m_winStreak != null)
				{
					if (instance.currentStatus.goOnWin > 1)
					{
						string text2 = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_continuous_win");
						this.m_winStreak.text = text2.Replace("{PARAM}", instance.currentStatus.goOnWin.ToString());
					}
					else
					{
						this.m_winStreak.text = string.Empty;
					}
				}
			}
		}
		HudMenuUtility.SetConnectAlertSimpleUI(false);
	}
}
