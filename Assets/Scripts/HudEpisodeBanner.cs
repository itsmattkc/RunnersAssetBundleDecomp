using System;
using UnityEngine;

public class HudEpisodeBanner
{
	private GameObject m_mainMenuObject;

	private int m_bannerId;

	public void Initialize(GameObject mainMenuObject)
	{
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
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "0_Endless");
		if (gameObject2 == null)
		{
			return;
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_2_mileage");
		if (gameObject3 == null)
		{
			return;
		}
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject3, "img_tex_ep");
		if (uITexture == null)
		{
			return;
		}
		TextureAsyncLoadManager instance = TextureAsyncLoadManager.Instance;
		if (instance == null)
		{
			return;
		}
		int bannerCount = TextureRequestEpisodeBanner.BannerCount;
		for (int i = 0; i < bannerCount; i++)
		{
			TextureRequestEpisodeBanner request = new TextureRequestEpisodeBanner(i, uITexture);
			if (instance.IsLoaded(request))
			{
				this.m_bannerId = i;
				instance.Request(request);
			}
		}
	}

	public void UpdateView()
	{
	}
}
