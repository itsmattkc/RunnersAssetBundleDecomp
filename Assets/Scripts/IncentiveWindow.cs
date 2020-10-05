using System;
using Text;

public class IncentiveWindow
{
	private int m_serverItemId;

	private int m_itemNum;

	private bool m_isStart;

	private bool m_isEnd;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
		private set
		{
		}
	}

	public IncentiveWindow(int serverItemId, int itemNum, string anchor)
	{
		this.m_serverItemId = serverItemId;
		this.m_itemNum = itemNum;
	}

	public void PlayStart()
	{
		string imageCount = string.Empty;
		TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "gw_item_text");
		if (text != null)
		{
			text.ReplaceTag("{COUNT}", HudUtility.GetFormatNumString<int>(this.m_itemNum));
			imageCount = text.text;
		}
		string caption = string.Empty;
		text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_reward_caption");
		if (text != null)
		{
			caption = text.text;
		}
		ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
		if (itemGetWindow != null)
		{
			itemGetWindow.gameObject.SetActive(true);
			itemGetWindow.Create(new ItemGetWindow.CInfo
			{
				caption = caption,
				serverItemId = this.m_serverItemId,
				imageCount = imageCount
			});
		}
		this.m_isStart = true;
	}

	public void Update()
	{
		if (this.m_isStart)
		{
			ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
			if (itemGetWindow != null && itemGetWindow.IsEnd)
			{
				itemGetWindow.Reset();
				itemGetWindow.gameObject.SetActive(false);
				this.m_isEnd = true;
			}
		}
	}
}
