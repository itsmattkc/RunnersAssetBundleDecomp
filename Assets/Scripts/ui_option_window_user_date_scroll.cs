using System;
using Text;
using UnityEngine;

public class ui_option_window_user_date_scroll : MonoBehaviour
{
	public enum ResultType
	{
		QUICK_HIGHT_SCORE,
		QUICK_TOTAL_SCORE,
		HIGHT_SCORE,
		TOTAL_SCORE,
		DISTANCE,
		CUMULATIVE_DISTANCE,
		PLAYING_NUM,
		RING,
		RED_RING,
		ANIMAL,
		CHAO_ROULETTE,
		ITEM_ROULETTE,
		JACK_POT_NUM,
		JACK_POT_RING,
		NUM
	}

	public class textInfo
	{
		public string grop;

		public string cell;
	}

	private readonly ui_option_window_user_date_scroll.textInfo[] m_textInfos = new ui_option_window_user_date_scroll.textInfo[]
	{
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "quick_high_score"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "quick_total_sum_high_score"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "endless_high_score"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "endless_total_sum_high_score"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "maximum_distance"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "cumulative_distance"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "playing_num"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "take_all_rings"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "take_all_red_rings"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "take_all_red_animals"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "chao_roulette_num"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "item_roulette_num"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "jack_pot_num"
		},
		new ui_option_window_user_date_scroll.textInfo
		{
			grop = "Option",
			cell = "jack_pot_ring"
		}
	};

	[SerializeField]
	private UILabel m_textLabel;

	[SerializeField]
	private UILabel m_socoreLabel;

	private ui_option_window_user_date_scroll.ResultType m_rusultType = ui_option_window_user_date_scroll.ResultType.HIGHT_SCORE;

	private void Start()
	{
		base.enabled = false;
	}

	public void UpdateView(ui_option_window_user_date_scroll.ResultType type, ServerOptionUserResult optionUserResult)
	{
		this.m_rusultType = type;
		this.TextInit(optionUserResult);
	}

	public void TextInit(ServerOptionUserResult optionUserResult)
	{
		if (this.m_rusultType < ui_option_window_user_date_scroll.ResultType.NUM)
		{
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, this.m_textInfos[(int)this.m_rusultType].grop, this.m_textInfos[(int)this.m_rusultType].cell);
			if (this.m_textLabel != null && text != null)
			{
				this.m_textLabel.text = text.text;
			}
		}
		if (this.m_socoreLabel != null)
		{
			this.m_socoreLabel.text = ui_option_window_user_date_scroll.GetScore(this.m_rusultType, optionUserResult);
		}
	}

	private static string GetScore(ui_option_window_user_date_scroll.ResultType type, ServerOptionUserResult optionUserResult)
	{
		long num = 0L;
		if (ServerInterface.PlayerState != null && optionUserResult != null)
		{
			switch (type)
			{
			case ui_option_window_user_date_scroll.ResultType.QUICK_HIGHT_SCORE:
				num = ServerInterface.PlayerState.m_totalHighScoreQuick;
				break;
			case ui_option_window_user_date_scroll.ResultType.QUICK_TOTAL_SCORE:
				num = optionUserResult.m_quickTotalSumHightScore;
				break;
			case ui_option_window_user_date_scroll.ResultType.HIGHT_SCORE:
				num = ServerInterface.PlayerState.m_totalHighScore;
				break;
			case ui_option_window_user_date_scroll.ResultType.TOTAL_SCORE:
				num = optionUserResult.m_totalSumHightScore;
				break;
			case ui_option_window_user_date_scroll.ResultType.DISTANCE:
				num = ServerInterface.PlayerState.m_maxDistance;
				break;
			case ui_option_window_user_date_scroll.ResultType.CUMULATIVE_DISTANCE:
				num = ServerInterface.PlayerState.m_totalDistance;
				break;
			case ui_option_window_user_date_scroll.ResultType.PLAYING_NUM:
				num = (long)ServerInterface.PlayerState.m_numPlaying;
				break;
			case ui_option_window_user_date_scroll.ResultType.RING:
				num = optionUserResult.m_numTakeAllRings;
				break;
			case ui_option_window_user_date_scroll.ResultType.RED_RING:
				num = optionUserResult.m_numTakeAllRedRings;
				break;
			case ui_option_window_user_date_scroll.ResultType.ANIMAL:
				num = (long)ServerInterface.PlayerState.m_numAnimals;
				break;
			case ui_option_window_user_date_scroll.ResultType.CHAO_ROULETTE:
				num = (long)optionUserResult.m_numChaoRoulette;
				break;
			case ui_option_window_user_date_scroll.ResultType.ITEM_ROULETTE:
				num = (long)optionUserResult.m_numItemRoulette;
				break;
			case ui_option_window_user_date_scroll.ResultType.JACK_POT_NUM:
				num = (long)optionUserResult.m_numJackPot;
				break;
			case ui_option_window_user_date_scroll.ResultType.JACK_POT_RING:
				num = (long)optionUserResult.m_numMaximumJackPotRings;
				break;
			}
		}
		if (num < 0L)
		{
			num = 0L;
		}
		else if (num > 999999999999L)
		{
			num = 999999999999L;
		}
		return HudUtility.GetFormatNumString<long>(num);
	}
}
