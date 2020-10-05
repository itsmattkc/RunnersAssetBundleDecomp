using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuServerAddOpeMessage : UIDebugMenuTask
{
	private enum TextType
	{
		MESSAGE_KIND,
		INFO_ID,
		ITEM_ID,
		NUM_ITEM,
		ADDITIONAL_INFO1,
		ADDITIONAL_INFO2,
		MSG_CONTENT,
		NUM
	}

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuButton m_decideButton;

	private UIDebugMenuTextField[] m_TextFields = new UIDebugMenuTextField[7];

	private string[] DefaultTextList = new string[]
	{
		"メッセージ種別( 0:全体 1:個別)",
		"お知らせID(全体へのプレゼント時の時に、入力可。個別時は0とする)",
		"運営配布のアイテムID",
		"運営配布のアイテム数",
		"アイテム追加情報_1(チャオであればLV)",
		"アイテム追加情報_2(チャオであればLVMAX時の取得スペシャルエッグ数)",
		"個別メッセージの内容"
	};

	private string[] DefaultParamTextList = new string[]
	{
		"1",
		"0",
		"120000",
		"1",
		"0",
		"0",
		string.Empty
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(100f, 100f, 200f, 50f),
		new Rect(310f, 100f, 500f, 50f),
		new Rect(100f, 175f, 250f, 50f),
		new Rect(360f, 175f, 250f, 50f),
		new Rect(100f, 250f, 500f, 50f),
		new Rect(100f, 325f, 500f, 50f),
		new Rect(100f, 475f, 500f, 50f)
	};

	private NetDebugAddOpeMessage m_addOpeMsg;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(100f, 20f, 150f, 50f), "Back", base.gameObject);
		this.m_decideButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_decideButton.Setup(new Rect(250f, 20f, 150f, 50f), "Decide", base.gameObject);
		for (int i = 0; i < 7; i++)
		{
			this.m_TextFields[i] = base.gameObject.AddComponent<UIDebugMenuTextField>();
			this.m_TextFields[i].Setup(this.RectList[i], this.DefaultTextList[i], this.DefaultParamTextList[i]);
		}
	}

	private string GetGameId()
	{
		return SystemSaveManager.GetGameID();
	}

	protected override void OnTransitionTo()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(false);
		}
		if (this.m_decideButton != null)
		{
			this.m_decideButton.SetActive(false);
		}
		for (int i = 0; i < 7; i++)
		{
			if (!(this.m_TextFields[i] == null))
			{
				this.m_TextFields[i].SetActive(false);
			}
		}
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
		}
		if (this.m_decideButton != null)
		{
			this.m_decideButton.SetActive(true);
		}
		for (int i = 0; i < 7; i++)
		{
			if (!(this.m_TextFields[i] == null))
			{
				this.m_TextFields[i].SetActive(true);
			}
		}
	}

	private void OnClicked(string name)
	{
		if (name == "Back")
		{
			base.TransitionToParent();
		}
		else if (name == "Decide")
		{
			this.m_addOpeMsg = new NetDebugAddOpeMessage(new NetDebugAddOpeMessage.OpeMsgInfo
			{
				userID = this.GetGameId(),
				messageKind = int.Parse(this.m_TextFields[0].text),
				infoId = int.Parse(this.m_TextFields[1].text),
				itemId = int.Parse(this.m_TextFields[2].text),
				numItem = int.Parse(this.m_TextFields[3].text),
				additionalInfo1 = int.Parse(this.m_TextFields[4].text),
				additionalInfo2 = int.Parse(this.m_TextFields[5].text),
				msgTitle = string.Empty,
				msgContent = this.m_TextFields[6].text,
				msgImageId = "0"
			});
			this.m_addOpeMsg.Request();
		}
	}
}
