using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class RouletteInformationManager : MonoBehaviour
{
	public class InfoBannerRequest
	{
		private UITexture m_texture;

		public InfoBannerRequest(UITexture texture)
		{
			this.m_texture = texture;
		}

		public void LoadDone(Texture2D loadedTex)
		{
			if (loadedTex == null)
			{
				return;
			}
			if (this.m_texture == null)
			{
				return;
			}
			this.m_texture.mainTexture = loadedTex;
		}
	}

	private sealed class _LoadInfoBaner_c__AnonStoreyFE
	{
		internal RouletteInformationManager.InfoBannerRequest bannerRequest;

		internal void __m__6F(Texture2D tex)
		{
			this.bannerRequest.LoadDone(tex);
		}
	}

	private bool m_isSetuped;

	private Dictionary<RouletteCategory, InformationWindow.Information> m_rouletteInfo = new Dictionary<RouletteCategory, InformationWindow.Information>();

	private static RouletteInformationManager m_instance;

	public bool IsSetuped
	{
		get
		{
			return this.m_isSetuped;
		}
	}

	public static RouletteInformationManager Instance
	{
		get
		{
			return RouletteInformationManager.m_instance;
		}
	}

	public bool GetCurrentInfoParam(out Dictionary<RouletteCategory, InformationWindow.Information> infoParam)
	{
		if (this.m_isSetuped)
		{
			infoParam = new Dictionary<RouletteCategory, InformationWindow.Information>(this.m_rouletteInfo);
			return true;
		}
		infoParam = null;
		return false;
	}

	public void SetUp()
	{
		this.m_rouletteInfo.Clear();
		ServerNoticeInfo noticeInfo = ServerInterface.NoticeInfo;
		if (noticeInfo != null)
		{
			List<NetNoticeItem> rouletteItems = noticeInfo.m_rouletteItems;
			if (rouletteItems != null)
			{
				string commonText = TextUtility.GetCommonText("Informaion", "announcement");
				using (List<NetNoticeItem>.Enumerator enumerator = rouletteItems.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						NetNoticeItem current = enumerator.Current;
						InformationWindow.Information value = default(InformationWindow.Information);
						value.pattern = (InformationWindow.ButtonPattern)current.WindowType;
						value.bodyText = current.Message;
						value.imageId = current.ImageId;
						value.texture = null;
						value.caption = commonText;
						value.url = current.Adress;
						this.m_rouletteInfo.Add(RouletteCategory.PREMIUM, value);
					}
				}
				EventManager.EventType typeInTime = EventManager.Instance.TypeInTime;
				if (typeInTime == EventManager.EventType.RAID_BOSS)
				{
					InformationWindow.Information value2 = default(InformationWindow.Information);
					this.m_rouletteInfo.Add(RouletteCategory.RAID, value2);
				}
				this.m_isSetuped = true;
			}
		}
	}

	public void LoadInfoBaner(RouletteInformationManager.InfoBannerRequest bannerRequest, RouletteCategory category = RouletteCategory.PREMIUM)
	{
		if (this.m_isSetuped && this.m_rouletteInfo != null && this.m_rouletteInfo.ContainsKey(category))
		{
			InformationWindow.Information information = this.m_rouletteInfo[category];
			InformationImageManager instance = InformationImageManager.Instance;
			if (instance != null)
			{
				bool bannerFlag = true;
				instance.Load(information.imageId, bannerFlag, delegate(Texture2D tex)
				{
					bannerRequest.LoadDone(tex);
				});
			}
		}
	}

	private void Awake()
	{
		if (RouletteInformationManager.m_instance == null)
		{
			RouletteInformationManager.m_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
