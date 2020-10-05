using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class ui_mm_news_page : MonoBehaviour
{
	private const int DISPLAY_MAX_ITEM_COUNT = 10;

	[SerializeField]
	private UIRectItemStorage m_itemStorage;

	[SerializeField]
	private UIScrollBar m_scrollBar;

	private List<ui_mm_new_page_ad_banner.BannerInfo> m_bannerInfoList = new List<ui_mm_new_page_ad_banner.BannerInfo>();

	private List<NetNoticeItem> m_noticeItems;

	private void Start()
	{
		base.enabled = false;
	}

	private void SetInfomation()
	{
		string commonText = TextUtility.GetCommonText("Informaion", "announcement");
		this.m_bannerInfoList.Clear();
		ServerNoticeInfo noticeInfo = ServerInterface.NoticeInfo;
		if (noticeInfo != null)
		{
			this.m_noticeItems = noticeInfo.m_noticeItems;
			if (this.m_noticeItems != null)
			{
				foreach (NetNoticeItem current in this.m_noticeItems)
				{
					bool flag = true;
					ui_mm_new_page_ad_banner.BannerInfo bannerInfo = new ui_mm_new_page_ad_banner.BannerInfo();
					bannerInfo.info = default(InformationWindow.Information);
					bannerInfo.info.pattern = (InformationWindow.ButtonPattern)current.WindowType;
					bannerInfo.info.bodyText = current.Message;
					bannerInfo.info.imageId = current.ImageId;
					bannerInfo.info.texture = null;
					bannerInfo.info.url = current.Adress;
					bannerInfo.item = current;
					if (current.Id == (long)NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID)
					{
						bannerInfo.info.rankingType = InformationWindow.RankingType.LEAGUE;
					}
					else if (current.Id == (long)NetNoticeItem.OPERATORINFO_QUICKRANKINGRESULT_ID)
					{
						bannerInfo.info.rankingType = InformationWindow.RankingType.QUICK_LEAGUE;
					}
					else if (current.Id == (long)NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID)
					{
						bannerInfo.info.rankingType = InformationWindow.RankingType.EVENT;
					}
					else
					{
						bannerInfo.info.caption = commonText;
						bannerInfo.info.rankingType = InformationWindow.RankingType.NON;
						flag = noticeInfo.IsOnTime(current);
					}
					if (flag)
					{
						this.m_bannerInfoList.Add(bannerInfo);
					}
				}
			}
		}
	}

	private void UpdatePage()
	{
		this.UpdateRectItemStorage();
		if (this.m_scrollBar != null)
		{
			this.m_scrollBar.value = 0f;
		}
	}

	private void UpdateRectItemStorage()
	{
		if (this.m_itemStorage != null)
		{
			int count = this.m_bannerInfoList.Count;
			this.m_itemStorage.maxItemCount = count;
			this.m_itemStorage.maxRows = count;
			this.m_itemStorage.Restart();
			ui_mm_new_page_ad_banner[] componentsInChildren = this.m_itemStorage.GetComponentsInChildren<ui_mm_new_page_ad_banner>(true);
			int num = componentsInChildren.Length;
			for (int i = 0; i < count; i++)
			{
				if (i < num)
				{
					componentsInChildren[i].UpdateView(this.m_bannerInfoList[i]);
				}
			}
		}
	}

	public void StartInformation()
	{
		this.SetInfomation();
		this.UpdatePage();
	}
}
