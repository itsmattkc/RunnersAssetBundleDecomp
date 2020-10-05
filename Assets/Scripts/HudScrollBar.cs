using App;
using System;
using UnityEngine;

public class HudScrollBar : MonoBehaviour
{
	public delegate void PageChangeCallback(int prevPage, int currentPage);

	private UIScrollBar m_scrollBar;

	private float m_stepValue;

	private HudScrollBar.PageChangeCallback m_callback;

	private int m_currentPage;

	private bool m_isInit;

	private float m_lastScrollValue;

	private void Start()
	{
	}

	public void Setup(UIScrollBar scrollBar, int pageCount)
	{
		if (scrollBar == null)
		{
			return;
		}
		if (pageCount <= 1)
		{
			return;
		}
		this.m_scrollBar = scrollBar;
		this.m_stepValue = 1f / ((float)pageCount - 1f);
		EventDelegate.Add(this.m_scrollBar.onChange, new EventDelegate.Callback(this.OnChangeScrollBarValue));
	}

	public void SetPageChangeCallback(HudScrollBar.PageChangeCallback callback)
	{
		this.m_callback = callback;
	}

	public void LeftScroll(int pageCount)
	{
		float num = this.m_stepValue * (float)pageCount;
		float value = this.m_scrollBar.value;
		if (App.Math.NearZero(value - 1f, 1E-06f))
		{
			this.m_scrollBar.value = 1f - num;
		}
		else if (App.Math.NearZero(value - num, 1E-06f))
		{
			this.m_scrollBar.value = 0f;
			if (this.m_scrollBar.onDragFinished != null)
			{
				this.m_scrollBar.onDragFinished();
			}
		}
		else
		{
			this.m_scrollBar.value -= num;
		}
	}

	public void RightScroll(int pageCount)
	{
		float num = this.m_stepValue * (float)pageCount;
		float value = this.m_scrollBar.value;
		if (App.Math.NearZero(value, 1E-06f))
		{
			this.m_scrollBar.value = num;
		}
		else if (App.Math.NearZero(value + num - 1f, 1E-06f))
		{
			this.m_scrollBar.value = 1f;
			if (this.m_scrollBar.onDragFinished != null)
			{
				this.m_scrollBar.onDragFinished();
			}
		}
		else
		{
			this.m_scrollBar.value += num;
		}
	}

	public void PageJump(int pageIndex, bool isInit)
	{
		this.m_scrollBar.value = this.m_stepValue * (float)pageIndex;
		if (this.m_scrollBar.onDragFinished != null)
		{
			this.m_scrollBar.onDragFinished();
		}
		this.m_isInit = isInit;
	}

	private void LateUpdate()
	{
		int currentPage = this.GetCurrentPage();
		if (this.m_currentPage != currentPage || this.m_isInit)
		{
			bool flag = App.Math.NearEqual(this.m_lastScrollValue, this.m_scrollBar.value, 1E-06f);
			if (flag)
			{
				if (this.m_callback != null)
				{
					this.m_callback(this.m_currentPage, currentPage);
				}
				this.m_currentPage = currentPage;
				this.m_isInit = false;
			}
		}
		this.m_lastScrollValue = this.m_scrollBar.value;
	}

	private void OnDestroy()
	{
		EventDelegate.Remove(this.m_scrollBar.onChange, new EventDelegate.Callback(this.OnChangeScrollBarValue));
	}

	private void OnChangeScrollBarValue()
	{
	}

	private int GetCurrentPage()
	{
		float num = -this.m_stepValue * 0.5f;
		float num2 = this.m_stepValue * 0.5f;
		int num3 = 0;
		while (num > this.m_scrollBar.value || this.m_scrollBar.value > num2)
		{
			num += this.m_stepValue;
			num2 += this.m_stepValue;
			num3++;
		}
		return num3;
	}
}
