using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IncentiveWindowQueue : MonoBehaviour
{
	private List<IncentiveWindow> m_queue = new List<IncentiveWindow>();

	private ButtonEventResourceLoader m_resourceLoader;

	private static ButtonEventResourceLoader.CallbackIfNotLoaded __f__am_cache2;

	public bool SetUpped
	{
		get
		{
			return !(this.m_resourceLoader != null) || this.m_resourceLoader.IsLoaded;
		}
	}

	public void AddWindow(IncentiveWindow window)
	{
		this.m_queue.Add(window);
	}

	public void PlayStart()
	{
		if (this.IsEmpty())
		{
			return;
		}
		if (this.m_queue == null)
		{
			return;
		}
		this.m_queue[0].PlayStart();
	}

	public bool IsEmpty()
	{
		return this.m_queue.Count <= 0;
	}

	private void Start()
	{
		if (this.m_resourceLoader == null)
		{
			this.m_resourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
		}
		this.m_resourceLoader.LoadResourceIfNotLoadedAsync("item_get_Window", delegate
		{
			if (FontManager.Instance != null)
			{
				FontManager.Instance.ReplaceFont();
			}
			if (AtlasManager.Instance != null)
			{
				AtlasManager.Instance.ReplaceAtlasForMenu(true);
			}
		});
	}

	private void Update()
	{
		if (this.IsEmpty())
		{
			return;
		}
		if (!this.SetUpped)
		{
			return;
		}
		IncentiveWindow incentiveWindow = this.m_queue[0];
		if (incentiveWindow == null)
		{
			return;
		}
		incentiveWindow.Update();
		if (incentiveWindow.IsEnd)
		{
			this.m_queue.Remove(incentiveWindow);
			if (!this.IsEmpty())
			{
				this.m_queue[0].PlayStart();
			}
		}
	}
}
