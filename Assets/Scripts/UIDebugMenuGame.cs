using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuGame : UIDebugMenuTask
{
	private enum MenuType
	{
		W01_AFTERNOON,
		W01_NIGTH,
		W01_BOSS,
		W02_AFTERNOON,
		W02_NIGTH,
		W02_BOSS,
		W03_AFTERNOON,
		W03_NIGTH,
		W03_BOSS,
		W04_AFTERNOON,
		W05_AFTERNOON,
		W07_AFTERNOON,
		NUM
	}

	private static int BUTTON_W = 30;

	private List<string> MenuObjName = new List<string>
	{
		"w01_afternoon",
		"w01_night",
		"w01_boss",
		"w02_afternoon",
		"w02_night",
		"w02_boss",
		"w03_afternoon",
		"w03_night",
		"w03_boss",
		"w04_afternoon",
		"w05_afternoon",
		"w07_afternoon"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 180f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(400f, 180f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(600f, 180f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(200f, 220f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(400f, 220f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(600f, 220f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(200f, 260f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(400f, 260f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(600f, 260f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(200f, 300f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(400f, 300f, 150f, (float)UIDebugMenuGame.BUTTON_W),
		new Rect(600f, 300f, 150f, (float)UIDebugMenuGame.BUTTON_W)
	};

	private UIDebugMenuButtonList m_buttonList;

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuTextField m_mainCharacter;

	private UIDebugMenuTextField m_subCharacter;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_buttonList = base.gameObject.AddComponent<UIDebugMenuButtonList>();
		GameObject original = GameObjectUtil.FindChildGameObject(base.gameObject, "stage_object");
		for (int i = 0; i < 12; i++)
		{
			string name = this.MenuObjName[i];
			GameObject gameObject = UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;
			if (!(gameObject == null))
			{
				gameObject.name = name;
				this.m_buttonList.Add(this.RectList, this.MenuObjName, base.gameObject);
				this.m_mainCharacter = base.gameObject.AddComponent<UIDebugMenuTextField>();
				this.m_mainCharacter.Setup(new Rect(400f, 100f, 150f, 50f), "MainChara", string.Empty);
				this.m_subCharacter = base.gameObject.AddComponent<UIDebugMenuTextField>();
				this.m_subCharacter.Setup(new Rect(600f, 100f, 150f, 50f), "SubChara", string.Empty);
				if (CharacterDataNameInfo.Instance == null)
				{
					GameObject gameObject2 = new GameObject("ResourceSceneLoader");
					ResourceSceneLoader resourceSceneLoader = gameObject2.AddComponent<ResourceSceneLoader>();
					resourceSceneLoader.AddLoad("CharacterDataNameInfo", true, false);
				}
			}
		}
	}

	protected override void OnTransitionTo()
	{
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(false);
		}
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(false);
		}
		if (this.m_mainCharacter != null)
		{
			this.m_mainCharacter.SetActive(false);
		}
		if (this.m_subCharacter != null)
		{
			this.m_subCharacter.SetActive(false);
		}
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(true);
		}
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
		}
		if (this.m_mainCharacter != null)
		{
			this.m_mainCharacter.SetActive(true);
		}
		if (this.m_subCharacter != null)
		{
			this.m_subCharacter.SetActive(true);
		}
	}

	private void OnClicked(string name)
	{
		if (name == "Back")
		{
			base.TransitionToParent();
		}
		else
		{
			if (CharacterDataNameInfo.Instance != null && SaveDataManager.Instance != null)
			{
				CharacterDataNameInfo.Info dataByName = CharacterDataNameInfo.Instance.GetDataByName(this.m_mainCharacter.text);
				if (dataByName != null)
				{
					SaveDataManager.Instance.PlayerData.MainChara = dataByName.m_ID;
				}
				dataByName = CharacterDataNameInfo.Instance.GetDataByName(this.m_subCharacter.text);
				if (dataByName != null)
				{
					SaveDataManager.Instance.PlayerData.SubChara = dataByName.m_ID;
				}
			}
			StageInfo stageInfo = GameObjectUtil.FindGameObjectComponent<StageInfo>("StageInfo");
			if (stageInfo != null)
			{
				if (name.Contains("w01"))
				{
					stageInfo.SelectedStageName = StageInfo.GetStageNameByIndex(1);
					stageInfo.BossType = BossType.MAP1;
				}
				if (name.Contains("w02"))
				{
					stageInfo.SelectedStageName = StageInfo.GetStageNameByIndex(2);
					stageInfo.BossType = BossType.MAP2;
				}
				if (name.Contains("w03"))
				{
					stageInfo.SelectedStageName = StageInfo.GetStageNameByIndex(3);
					stageInfo.BossType = BossType.MAP3;
				}
				if (name.Contains("w04"))
				{
					stageInfo.SelectedStageName = StageInfo.GetStageNameByIndex(4);
				}
				if (name.Contains("w05"))
				{
					EventManager.Instance.Id = 100020000;
					EventManager.Instance.SetDebugParameter();
					EventManager.Instance.EventStage = true;
					stageInfo.EventStage = true;
					stageInfo.SelectedStageName = StageInfo.GetStageNameByIndex(5);
				}
				if (name.Contains("w07"))
				{
					EventManager.Instance.Id = 100010000;
					EventManager.Instance.SetDebugParameter();
					EventManager.Instance.EventStage = true;
					stageInfo.EventStage = true;
					stageInfo.SelectedStageName = StageInfo.GetStageNameByIndex(7);
				}
				if (name.Contains("afternoon") || name.Contains("boss"))
				{
					stageInfo.TenseType = TenseType.AFTERNOON;
				}
				else if (name.Contains("night"))
				{
					stageInfo.TenseType = TenseType.NIGHT;
				}
				if (name.Contains("boss"))
				{
					stageInfo.BossStage = true;
				}
				else
				{
					stageInfo.BossStage = false;
				}
				stageInfo.FromTitle = true;
				UnityEngine.SceneManagement.SceneManager.LoadScene("s_playingstage");
			}
		}
	}
}
