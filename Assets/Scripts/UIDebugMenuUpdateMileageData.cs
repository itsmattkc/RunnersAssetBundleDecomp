using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuUpdateMileageData : UIDebugMenuTask
{
	private enum TextType
	{
		EPISODE,
		CHAPTER,
		POINT,
		MAP_DISTANCE,
		NUM_BOSS_ATTACK,
		STAGE_DISTANCE,
		NUM
	}

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuButton m_decideButton;

	private UIDebugMenuTextField[] m_TextFields = new UIDebugMenuTextField[6];

	private string[] DefaultTextList = new string[]
	{
		"Story/話",
		"Chapter/章",
		"Point/ポイント",
		"Score On Map.",
		"Boss' Remain Life",
		"Target Score In Chapter"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 250f, 50f),
		new Rect(500f, 200f, 250f, 50f),
		new Rect(200f, 300f, 250f, 50f),
		new Rect(500f, 300f, 250f, 50f),
		new Rect(200f, 400f, 250f, 50f),
		new Rect(500f, 400f, 250f, 50f)
	};

	private string[] textFieldDefault = new string[]
	{
		"2",
		"1",
		"0",
		"0",
		"3",
		"0"
	};

	private NetDebugUpdMileageData m_updMileageData;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_decideButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_decideButton.Setup(new Rect(200f, 450f, 150f, 50f), "Decide", base.gameObject);
		for (int i = 0; i < 6; i++)
		{
			this.m_TextFields[i] = base.gameObject.AddComponent<UIDebugMenuTextField>();
			this.m_TextFields[i].Setup(this.RectList[i], this.DefaultTextList[i]);
			this.m_TextFields[i].text = this.textFieldDefault[i];
		}
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
		for (int i = 0; i < 6; i++)
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
		for (int i = 0; i < 6; i++)
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
			for (int i = 0; i < 6; i++)
			{
				UIDebugMenuTextField uIDebugMenuTextField = this.m_TextFields[i];
				if (!(uIDebugMenuTextField == null))
				{
					int num;
					if (!int.TryParse(uIDebugMenuTextField.text, out num))
					{
						return;
					}
				}
			}
			this.m_updMileageData = new NetDebugUpdMileageData(new ServerMileageMapState
			{
				m_episode = int.Parse(this.m_TextFields[0].text),
				m_chapter = int.Parse(this.m_TextFields[1].text),
				m_point = int.Parse(this.m_TextFields[2].text),
				m_stageTotalScore = (long)int.Parse(this.m_TextFields[3].text),
				m_numBossAttack = int.Parse(this.m_TextFields[4].text),
				m_stageMaxScore = (long)int.Parse(this.m_TextFields[5].text)
			});
			this.m_updMileageData.Request();
		}
	}
}
