using System;
using UnityEngine;

public class TextureRequestStagePicture : TextureRequest
{
	private int m_stageIndex = 1;

	private string m_fileName;

	private UITexture m_uiTex;

	public TextureRequestStagePicture(int stageIndex, UITexture uiTex)
	{
		if (stageIndex >= 1)
		{
			this.m_stageIndex = stageIndex;
			this.m_fileName = "ui_tex_mile_w" + this.m_stageIndex.ToString("D2") + "A";
			this.m_uiTex = uiTex;
		}
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
