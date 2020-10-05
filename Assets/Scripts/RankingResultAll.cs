using System;
using Text;
using UnityEngine;

public class RankingResultAll : MonoBehaviour
{
	private enum Mode
	{
		Idle,
		Wait,
		End
	}

	private RankingResultAll.Mode m_mode;

	private RankingServerInfoConverter m_rankingData;

	public void Setup(string serverMessageInfo)
	{
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_result_all_caption").text;
		this.m_rankingData = new RankingServerInfoConverter(serverMessageInfo);
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "RankingResultAll",
			caption = text,
			message = this.m_rankingData.rankingResultAllText,
			buttonType = GeneralWindow.ButtonType.Close
		});
		this.m_mode = RankingResultAll.Mode.Wait;
	}

	public bool IsEnd()
	{
		return this.m_mode != RankingResultAll.Mode.Wait;
	}

	private void Update()
	{
		RankingResultAll.Mode mode = this.m_mode;
		if (mode == RankingResultAll.Mode.Wait)
		{
			if (GeneralWindow.IsCreated("RankingResultAll") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.m_mode = RankingResultAll.Mode.End;
			}
		}
	}

	public static RankingResultAll Create(string serverMessageInfo)
	{
		GameObject gameObject = GameObject.Find("RankingResultAll");
		RankingResultAll rankingResultAll;
		if (gameObject == null)
		{
			gameObject = new GameObject("RankingResultAll");
			rankingResultAll = gameObject.AddComponent<RankingResultAll>();
		}
		else
		{
			rankingResultAll = gameObject.GetComponent<RankingResultAll>();
		}
		if (gameObject != null && rankingResultAll != null)
		{
			rankingResultAll.Setup(serverMessageInfo);
		}
		return rankingResultAll;
	}
}
