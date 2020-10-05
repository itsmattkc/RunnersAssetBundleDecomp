using DataTable;
using Message;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class ranking_help : WindowBase
{
	[SerializeField]
	private GameObject page0;

	[SerializeField]
	private GameObject page1;

	[SerializeField]
	private UITable page1Table;

	private UILabel m_labelLeague;

	private UILabel m_labelLeagueEx;

	private UILabel m_labelBody;

	private Dictionary<string, UISprite> m_imgRanks;

	private UISprite m_imgLeagueIcon;

	private UISprite m_imgLeagueStar;

	private LeagueType m_currentLeague;

	private int m_upCount;

	private int m_dnCount;

	private bool m_rewardListInit;

	private bool m_open;

	private RankingUtil.RankingMode m_rankingMode;

	private void Start()
	{
		this.Setup();
	}

	public void Open(bool rewardListRest)
	{
		this.m_open = true;
		if (rewardListRest)
		{
			this.m_rewardListInit = false;
		}
		this.m_rankingMode = RankingUtil.currentRankingMode;
	}

	private void Setup()
	{
		int currentLeague = 0;
		RankingUtil.GetMyLeagueData(this.m_rankingMode, ref currentLeague, ref this.m_upCount, ref this.m_dnCount);
		this.m_currentLeague = (LeagueType)currentLeague;
		if (this.m_labelLeague == null)
		{
			this.m_labelLeague = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_league");
		}
		if (this.m_labelLeagueEx == null)
		{
			this.m_labelLeagueEx = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_league_ex");
		}
		if (this.m_labelBody == null)
		{
			this.m_labelBody = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_body");
		}
		if (this.m_imgRanks == null)
		{
			this.m_imgRanks = new Dictionary<string, UISprite>();
			foreach (string current in new List<string>
			{
				"F",
				"E",
				"D",
				"C",
				"B",
				"A",
				"S"
			})
			{
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_rank_" + current.ToLower());
				if (uISprite != null)
				{
					this.m_imgRanks.Add(current, uISprite);
				}
			}
		}
		if (this.m_imgLeagueIcon == null)
		{
			this.m_imgLeagueIcon = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_icon_league");
		}
		if (this.m_imgLeagueStar == null)
		{
			this.m_imgLeagueStar = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_icon_league_sub");
		}
		this.m_labelLeague.text = ranking_help.GetRankingCurrent(this.m_currentLeague);
		this.m_labelLeagueEx.text = ranking_help.GetRankingHelpText(this.m_rankingMode, this.m_currentLeague);
		this.m_labelBody.text = ranking_help.GetRankingHelpPresentText();
		if (this.page0 != null)
		{
			UIDraggablePanel uIDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(this.page0, "ScrollView");
			if (uIDraggablePanel != null)
			{
				uIDraggablePanel.ResetPosition();
			}
		}
		this.SetCurrentRankImage(this.m_currentLeague);
	}

	public void OnClose()
	{
		SoundManager.SePlay("sys_window_close", "SE");
		this.m_open = false;
	}

	private static string GetRankingHelpPresentText()
	{
		string result = string.Empty;
		if (SingletonGameObject<RankingManager>.Instance == null)
		{
			return result;
		}
		ServerLeagueData leagueData = SingletonGameObject<RankingManager>.Instance.GetLeagueData(RankingUtil.currentRankingMode);
		if (leagueData != null)
		{
			string itemText = RankingLeagueTable.GetItemText(leagueData.highScoreOpe, null, null, 0, false);
			string itemText2 = RankingLeagueTable.GetItemText(leagueData.totalScoreOpe, null, null, 0, false);
			result = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_help_1").text, new Dictionary<string, string>
			{
				{
					"{PARAM1}",
					itemText
				},
				{
					"{PARAM2}",
					itemText2
				}
			});
		}
		return result;
	}

	private bool SetCurrentRankImage(LeagueType leagueType)
	{
		if (this.m_imgRanks != null && this.m_imgRanks.Count > 0)
		{
			string leagueCategoryName = RankingUtil.GetLeagueCategoryName(leagueType);
			if (this.m_imgRanks.ContainsKey(leagueCategoryName))
			{
				Dictionary<string, UISprite>.KeyCollection keys = this.m_imgRanks.Keys;
				foreach (string current in keys)
				{
					this.m_imgRanks[current].gameObject.SetActive(false);
				}
				this.m_imgRanks[leagueCategoryName].gameObject.SetActive(true);
				this.m_imgLeagueIcon.spriteName = "ui_ranking_league_icon_" + leagueCategoryName.ToLower();
				this.m_imgLeagueStar.spriteName = "ui_ranking_league_icon_" + RankingUtil.GetLeagueCategoryClass(leagueType);
			}
		}
		return false;
	}

	private static string GetRankingCurrent(LeagueType leagueType)
	{
		string empty = string.Empty;
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_help_5").text;
		return TextUtility.Replaces(text, new Dictionary<string, string>
		{
			{
				"{PARAM_1}",
				RankingUtil.GetLeagueName(leagueType)
			}
		});
	}

	private static string GetRankingHelpText(RankingUtil.RankingMode rankingMode, LeagueType leagueType)
	{
		int num = 21;
		int num2 = (int)(LeagueType.NUM - leagueType);
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		RankingUtil.GetMyLeagueData(rankingMode, ref num5, ref num3, ref num4);
		string result = string.Empty;
		if (num2 == 1)
		{
			result = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_help_3").text, new Dictionary<string, string>
			{
				{
					"{PARAM_1}",
					num.ToString()
				},
				{
					"{PARAM_2}",
					num2.ToString()
				},
				{
					"{PARAM_4}",
					num4.ToString()
				}
			});
		}
		else if (num2 == 21 || num4 <= 0)
		{
			result = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_help_4").text, new Dictionary<string, string>
			{
				{
					"{PARAM_1}",
					num.ToString()
				},
				{
					"{PARAM_2}",
					num2.ToString()
				},
				{
					"{PARAM_3}",
					num3.ToString()
				}
			});
		}
		else
		{
			result = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_help_2").text, new Dictionary<string, string>
			{
				{
					"{PARAM_1}",
					num.ToString()
				},
				{
					"{PARAM_2}",
					num2.ToString()
				},
				{
					"{PARAM_3}",
					num3.ToString()
				},
				{
					"{PARAM_4}",
					num4.ToString()
				}
			});
		}
		return result;
	}

	private void OnClickToggle()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (!this.page1.activeSelf && !this.m_rewardListInit)
		{
			this.page1Table.repositionNow = false;
			List<GameObject> list = GameObjectUtil.FindChildGameObjects(this.page1Table.gameObject, "ui_ranking_reward(Clone)");
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GameObject parent = list[i];
					UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(parent, "img_icon_league");
					UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(parent, "img_icon_league_sub");
					UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_body");
					uILabel.text = string.Empty;
					uILabel.alpha = 0f;
					uISprite.alpha = 0f;
					uISprite2.alpha = 0f;
					string leagueCategoryName = RankingUtil.GetLeagueCategoryName((LeagueType)(list.Count - (i + 1)));
					uISprite.spriteName = "ui_ranking_league_icon_s_" + leagueCategoryName.ToLower();
					uISprite2.spriteName = "ui_ranking_league_icon_s_" + RankingUtil.GetLeagueCategoryClass((LeagueType)(list.Count - (i + 1)));
				}
			}
			if (ServerInterface.LoggedInServerInterface != null)
			{
				ServerInterface.LoggedInServerInterface.RequestServerGetLeagueOperatorData((int)RankingUtil.currentRankingMode, base.gameObject);
			}
			this.m_rewardListInit = true;
			this.page1Table.repositionNow = true;
		}
	}

	private void ServerGetLeagueOperatorData_Succeeded(MsgGetLeagueOperatorDataSucceed msg)
	{
		this.page1Table.repositionNow = false;
		List<GameObject> list = GameObjectUtil.FindChildGameObjects(this.page1Table.gameObject, "ui_ranking_reward(Clone)");
		if (list != null && list.Count > 0)
		{
			for (int i = 0; i < msg.m_leagueOperatorData.Count; i++)
			{
				int index = msg.m_leagueOperatorData.Count - (i + 1);
				ServerLeagueOperatorData serverLeagueOperatorData = msg.m_leagueOperatorData[index];
				if (serverLeagueOperatorData != null)
				{
					GameObject gameObject = list[i];
					UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_icon_league");
					UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_icon_league_sub");
					UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_body");
					uISprite.alpha = 1f;
					uISprite2.alpha = 1f;
					uILabel.alpha = 1f;
					string leagueCategoryName = RankingUtil.GetLeagueCategoryName((LeagueType)serverLeagueOperatorData.leagueId);
					uISprite.spriteName = "ui_ranking_league_icon_s_" + leagueCategoryName.ToLower();
					uISprite2.spriteName = "ui_ranking_league_icon_s_" + RankingUtil.GetLeagueCategoryClass((LeagueType)serverLeagueOperatorData.leagueId);
					string itemText = RankingLeagueTable.GetItemText(serverLeagueOperatorData.highScoreOpe, null, null, 0, false);
					string itemText2 = RankingLeagueTable.GetItemText(serverLeagueOperatorData.totalScoreOpe, null, null, 0, false);
					string text = string.Empty;
					text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_help_1").text, new Dictionary<string, string>
					{
						{
							"{PARAM1}",
							itemText
						},
						{
							"{PARAM2}",
							itemText2
						}
					});
					uILabel.text = text;
					gameObject.SendMessage("OnClickBg");
				}
			}
		}
		this.page1Table.repositionNow = true;
	}

	private void ServerGetLeagueOperatorData_Failed()
	{
		this.m_rewardListInit = false;
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (!this.m_open)
		{
			return;
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_close");
		if (gameObject != null)
		{
			UIButtonMessage component = gameObject.GetComponent<UIButtonMessage>();
			if (component != null)
			{
				component.SendMessage("OnClick");
			}
		}
	}
}
