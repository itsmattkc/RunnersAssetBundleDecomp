using System;
using System.Collections.Generic;

public class ButtonEventPageHistory
{
	private List<ButtonInfoTable.PageType> m_pageList = new List<ButtonInfoTable.PageType>();

	public void Push(ButtonInfoTable.PageType pageType)
	{
		if (pageType == ButtonInfoTable.PageType.NON)
		{
			return;
		}
		if (pageType >= ButtonInfoTable.PageType.NUM)
		{
			return;
		}
		if (pageType == ButtonInfoTable.PageType.MAIN)
		{
			this.Clear();
		}
		this.m_pageList.Add(pageType);
	}

	public ButtonInfoTable.PageType Pop()
	{
		if (this.m_pageList.Count <= 0)
		{
			return ButtonInfoTable.PageType.MAIN;
		}
		int index = this.m_pageList.Count - 1;
		ButtonInfoTable.PageType result = this.m_pageList[index];
		this.m_pageList.RemoveAt(index);
		return result;
	}

	public ButtonInfoTable.PageType Peek()
	{
		if (this.m_pageList.Count <= 0)
		{
			return ButtonInfoTable.PageType.MAIN;
		}
		int index = this.m_pageList.Count - 1;
		return this.m_pageList[index];
	}

	public void Clear()
	{
		this.m_pageList.Clear();
		this.m_pageList.Add(ButtonInfoTable.PageType.MAIN);
	}
}
