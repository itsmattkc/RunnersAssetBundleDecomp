using System;
using UnityEngine;

public class TextureRequestEpisodeBanner : TextureRequest
{
	private static readonly int s_BannerCount = 2;

	private string m_fileName;

	private UITexture m_uiTex;

	public static int BannerCount
	{
		get
		{
			return TextureRequestEpisodeBanner.s_BannerCount;
		}
		private set
		{
		}
	}

	public TextureRequestEpisodeBanner(int textureIndex, UITexture uiTex)
	{
		this.m_fileName = "ui_tex_mm_ep_" + (textureIndex + 1).ToString("D3");
		this.m_uiTex = uiTex;
	}

	public override void LoadDone(Texture tex)
	{
		if (this.m_uiTex == null)
		{
			return;
		}
		this.m_uiTex.mainTexture = tex;
	}

	public override bool IsEnableLoad()
	{
		return !string.IsNullOrEmpty(this.GetFileName());
	}

	public override string GetFileName()
	{
		return this.m_fileName;
	}
}
