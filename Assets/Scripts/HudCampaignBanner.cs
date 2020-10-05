using System;
using Text;
using UnityEngine;

public class HudCampaignBanner : MonoBehaviour
{
	private GameObject m_mainMenuObject;

	private GameObject m_textureObj;

	private UITexture m_replaceTex;

	private InformationWindow m_infoWindow;

	private long m_id = -1L;

	private bool m_quick;

	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (this.m_replaceTex != null)
		{
			this.m_replaceTex.mainTexture = null;
			this.m_replaceTex = null;
		}
	}

	private void Update()
	{
		if (this.m_infoWindow != null && this.m_infoWindow.IsEnd())
		{
			base.enabled = false;
		}
	}

	public void Initialize(GameObject mainMenuObject, bool quickMode)
	{
		this.m_quick = quickMode;
		if (mainMenuObject == null)
		{
			return;
		}
		this.m_mainMenuObject = mainMenuObject;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_mainMenuObject, "Anchor_5_MC");
		if (gameObject == null)
		{
			return;
		}
		string name = (!this.m_quick) ? "0_Endless" : "1_Quick";
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, name);
		if (gameObject2 == null)
		{
			return;
		}
		this.m_textureObj = GameObjectUtil.FindChildGameObject(gameObject2, "img_ad_tex");
		if (this.m_textureObj == null)
		{
			return;
		}
		UIButtonMessage component = this.m_textureObj.GetComponent<UIButtonMessage>();
		if (component != null)
		{
			component.enabled = true;
			component.trigger = UIButtonMessage.Trigger.OnClick;
			component.target = base.gameObject;
			component.functionName = "CampaignBannerClicked";
		}
		this.m_replaceTex = this.m_textureObj.GetComponent<UITexture>();
		this.m_textureObj.SetActive(false);
		this.UpdateView();
	}

	public void UpdateView()
	{
		bool flag = false;
		if (EventManager.Instance != null)
		{
			if (EventManager.Instance.Type == EventManager.EventType.QUICK)
			{
				flag = this.m_quick;
			}
			else if (EventManager.Instance.Type == EventManager.EventType.BGM)
			{
				EventStageData stageData = EventManager.Instance.GetStageData();
				if (stageData != null)
				{
					flag = ((!this.m_quick) ? stageData.IsEndlessModeBGM() : stageData.IsQuickModeBGM());
				}
			}
		}
		if (flag)
		{
			if (ServerInterface.NoticeInfo != null && ServerInterface.NoticeInfo.m_eventItems != null)
			{
				foreach (NetNoticeItem current in ServerInterface.NoticeInfo.m_eventItems)
				{
					if (this.m_id != current.Id)
					{
						this.m_id = current.Id;
						if (InformationImageManager.Instance != null)
						{
							InformationImageManager.Instance.Load(current.ImageId, true, new Action<Texture2D>(this.OnLoadCallback));
						}
						if (this.m_textureObj != null)
						{
							this.m_textureObj.SetActive(true);
						}
						break;
					}
				}
			}
		}
		else
		{
			if (this.m_textureObj != null)
			{
				this.m_textureObj.SetActive(false);
			}
			if (this.m_replaceTex != null && this.m_replaceTex.mainTexture != null)
			{
				this.m_replaceTex.mainTexture = null;
			}
		}
	}

	public void OnLoadCallback(Texture2D texture)
	{
		if (this.m_replaceTex != null && texture != null)
		{
			this.m_replaceTex.mainTexture = texture;
		}
	}

	private void CampaignBannerClicked()
	{
		if (this.m_mainMenuObject == null)
		{
			return;
		}
		this.m_infoWindow = base.gameObject.GetComponent<InformationWindow>();
		if (this.m_infoWindow == null)
		{
			this.m_infoWindow = base.gameObject.AddComponent<InformationWindow>();
		}
		if (this.m_infoWindow != null && ServerInterface.NoticeInfo != null && ServerInterface.NoticeInfo.m_eventItems != null)
		{
			foreach (NetNoticeItem current in ServerInterface.NoticeInfo.m_eventItems)
			{
				if (this.m_id == current.Id)
				{
					InformationWindow.Information info = default(InformationWindow.Information);
					info.pattern = InformationWindow.ButtonPattern.OK;
					info.imageId = current.ImageId;
					info.caption = TextUtility.GetCommonText("Informaion", "announcement");
					GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
					if (cameraUIObject != null)
					{
						GameObject newsWindowObj = GameObjectUtil.FindChildGameObject(cameraUIObject, "NewsWindow");
						this.m_infoWindow.Create(info, newsWindowObj);
						base.enabled = true;
						SoundManager.SePlay("sys_menu_decide", "SE");
					}
					break;
				}
			}
		}
	}
}
