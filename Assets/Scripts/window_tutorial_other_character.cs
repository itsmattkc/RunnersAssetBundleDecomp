using System;
using Text;
using UnityEngine;

public class window_tutorial_other_character : WindowBase
{
	[SerializeField]
	private GameObject m_closeBtn;

	[SerializeField]
	private GameObject m_nextBtn;

	[SerializeField]
	private UIImageButton m_rightImageBtn;

	[SerializeField]
	private UIImageButton m_leftImageBtn;

	[SerializeField]
	private UITexture m_texture;

	[SerializeField]
	private UILabel m_headerTextLabel;

	[SerializeField]
	private UILabel m_mainTextLabel;

	[SerializeField]
	private UILabel m_nextTextLabel;

	private ResourceSceneLoader m_loaderComponent;

	private bool m_isEnd;

	private bool m_isLoading;

	private bool m_playOpen;

	private int m_pageCount;

	private int m_pageIndex;

	private UIPlayAnimation m_uiAnimation;

	private window_tutorial.ScrollInfo m_scrollInfo;

	private string m_sceneName;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
		OptionMenuUtility.TranObj(base.gameObject);
		if (this.m_closeBtn != null)
		{
			UIPlayAnimation component = this.m_closeBtn.GetComponent<UIPlayAnimation>();
			if (component != null)
			{
				EventDelegate.Add(component.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), false);
			}
		}
		this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
		if (this.m_uiAnimation != null)
		{
			Animation component2 = base.gameObject.GetComponent<Animation>();
			this.m_uiAnimation.target = component2;
			this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
		}
		if (this.m_texture != null)
		{
			this.m_texture.enabled = false;
		}
	}

	public void SetScrollInfo(window_tutorial.ScrollInfo info)
	{
		this.m_scrollInfo = info;
		this.m_isLoading = false;
		this.m_isEnd = false;
		if (this.m_texture != null)
		{
			this.m_texture.enabled = false;
		}
		this.m_pageIndex = 0;
		this.m_pageCount = 0;
		if (this.m_scrollInfo != null)
		{
			this.m_pageCount = HudTutorial.GetTexuterPageCount(this.m_scrollInfo.HudId);
		}
		this.CheckLoadTexture();
	}

	public void PlayOpenWindow()
	{
		this.m_playOpen = true;
		base.enabled = true;
		base.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (this.m_playOpen)
		{
			this.SetupText();
			this.SetNextBtn();
			if (this.m_uiAnimation != null)
			{
				this.m_uiAnimation.Play(true);
			}
			SoundManager.SePlay("sys_window_open", "SE");
			this.m_playOpen = false;
		}
		if (this.m_isLoading && this.m_loaderComponent != null && this.m_loaderComponent.Loaded)
		{
			this.SetupTexture();
			this.m_loaderComponent = null;
			this.m_isLoading = false;
		}
		if (!this.m_playOpen && !this.m_isLoading)
		{
			base.enabled = false;
		}
	}

	private void OnClickCloseButton()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnClickLeftButton()
	{
		this.m_pageIndex--;
		if (this.m_pageIndex < 0)
		{
			this.m_pageIndex = 0;
		}
		this.SetupTexture();
		this.SetupText();
		this.SetPage();
		SoundManager.SePlay("sys_page_skip", "SE");
	}

	private void OnClickRightButton()
	{
		this.m_pageIndex++;
		if (this.m_pageIndex == this.m_pageCount)
		{
			this.m_pageIndex = this.m_pageCount - 1;
		}
		this.SetupTexture();
		this.SetupText();
		this.SetPage();
		SoundManager.SePlay("sys_page_skip", "SE");
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
	}

	private void CheckLoadTexture()
	{
		if (this.m_scrollInfo != null)
		{
			switch (this.m_scrollInfo.DispType)
			{
			case window_tutorial.DisplayType.QUICK:
				this.m_sceneName = HudTutorial.GetLoadQuickModeSceneName(this.m_scrollInfo.HudId);
				break;
			case window_tutorial.DisplayType.CHARA:
				this.m_sceneName = HudTutorial.GetLoadSceneName(this.m_scrollInfo.Chara);
				break;
			case window_tutorial.DisplayType.BOSS_MAP_1:
				this.m_sceneName = HudTutorial.GetLoadSceneName(BossType.MAP1);
				break;
			case window_tutorial.DisplayType.BOSS_MAP_2:
				this.m_sceneName = HudTutorial.GetLoadSceneName(BossType.MAP2);
				break;
			case window_tutorial.DisplayType.BOSS_MAP_3:
				this.m_sceneName = HudTutorial.GetLoadSceneName(BossType.MAP3);
				break;
			}
			if (!string.IsNullOrEmpty(this.m_sceneName))
			{
				GameObject x = GameObject.Find(this.m_sceneName);
				if (x == null)
				{
					this.LoadTexture(this.m_sceneName);
				}
				else
				{
					this.SetupTexture();
				}
			}
		}
	}

	private void SetupTexture()
	{
		if (this.m_scrollInfo != null && !string.IsNullOrEmpty(this.m_sceneName))
		{
			GameObject gameObject = GameObject.Find(this.m_sceneName);
			if (gameObject != null)
			{
				HudTutorialTexture component = gameObject.GetComponent<HudTutorialTexture>();
				if (component != null && this.m_texture != null)
				{
					uint num = (uint)component.m_texList.Length;
					if (this.m_pageIndex < (int)num)
					{
						this.m_texture.mainTexture = component.m_texList[this.m_pageIndex];
						this.m_texture.enabled = true;
					}
				}
			}
		}
	}

	private void LoadTexture(string sceneName)
	{
		if (!this.m_isLoading)
		{
			if (this.m_loaderComponent == null)
			{
				this.m_loaderComponent = base.gameObject.AddComponent<ResourceSceneLoader>();
			}
			if (this.m_loaderComponent != null)
			{
				this.m_loaderComponent.AddLoad(sceneName, true, false);
				this.m_isLoading = true;
			}
		}
	}

	private void SetupText()
	{
		if (this.m_scrollInfo != null)
		{
			string text = string.Empty;
			switch (this.m_scrollInfo.DispType)
			{
			case window_tutorial.DisplayType.QUICK:
			{
				string commonText = TextUtility.GetCommonText("Tutorial", "caption_quickmode_tutorial");
				text = TextUtility.GetCommonText("Tutorial", "caption_explan2", "{PARAM_NAME}", commonText);
				break;
			}
			case window_tutorial.DisplayType.CHARA:
			{
				string cellID = CharaName.Name[(int)this.m_scrollInfo.Chara];
				string commonText2 = TextUtility.GetCommonText("CharaName", cellID);
				text = TextUtility.GetCommonText("Option", "chara_operation_method", "{CHARA_NAME}", commonText2);
				break;
			}
			case window_tutorial.DisplayType.BOSS_MAP_1:
			{
				string textCommonBossName = BossTypeUtil.GetTextCommonBossName(BossType.MAP1);
				text = TextUtility.GetCommonText("Option", "boss_attack_method", "{BOSS_NAME}", textCommonBossName);
				break;
			}
			case window_tutorial.DisplayType.BOSS_MAP_2:
			{
				string textCommonBossName2 = BossTypeUtil.GetTextCommonBossName(BossType.MAP2);
				text = TextUtility.GetCommonText("Option", "boss_attack_method", "{BOSS_NAME}", textCommonBossName2);
				break;
			}
			case window_tutorial.DisplayType.BOSS_MAP_3:
			{
				string textCommonBossName3 = BossTypeUtil.GetTextCommonBossName(BossType.MAP3);
				text = TextUtility.GetCommonText("Option", "boss_attack_method", "{BOSS_NAME}", textCommonBossName3);
				break;
			}
			}
			if (this.m_headerTextLabel != null)
			{
				this.m_headerTextLabel.text = text;
			}
			if (this.m_mainTextLabel != null)
			{
				this.m_mainTextLabel.text = HudTutorial.GetExplainText(this.m_scrollInfo.HudId, this.m_pageIndex);
			}
		}
	}

	private void SetNextBtn()
	{
		if (this.m_scrollInfo != null && this.m_nextBtn != null)
		{
			bool flag = this.m_pageCount > 1;
			this.m_nextBtn.SetActive(flag);
			if (flag)
			{
				this.SetPage();
			}
		}
	}

	private void SetPage()
	{
		if (this.m_nextTextLabel != null)
		{
			int num = this.m_pageIndex + 1;
			this.m_nextTextLabel.text = num + "/" + this.m_pageCount;
		}
		if (this.m_rightImageBtn != null)
		{
			this.m_rightImageBtn.isEnabled = (this.m_pageCount != this.m_pageIndex + 1);
		}
		if (this.m_leftImageBtn != null)
		{
			this.m_leftImageBtn.isEnabled = (this.m_pageIndex != 0);
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		UIButtonMessage component = this.m_closeBtn.GetComponent<UIButtonMessage>();
		if (component != null)
		{
			component.SendMessage("OnClick");
		}
	}
}
