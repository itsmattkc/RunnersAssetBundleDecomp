using System;
using UnityEngine;

public class TextureRequestChara : TextureRequest
{
	private CharaType m_type;

	private string m_fileName;

	private UITexture m_uiTex;

	public TextureRequestChara(CharaType type, UITexture uiTex)
	{
		if (type != CharaType.UNKNOWN)
		{
			this.m_type = type;
			this.m_uiTex = uiTex;
			if (this.m_uiTex != null)
			{
				this.m_uiTex.SetTexture(TextureAsyncLoadManager.Instance.CharaDefaultTexture);
			}
			int num = (int)type;
			string[] prefixNameList = CharacterDataNameInfo.PrefixNameList;
			string str = prefixNameList[num];
			this.m_fileName = "ui_tex_player_" + num.ToString("D2") + "_" + str;
		}
	}

	public static void RemoveAllCharaTexture()
	{
		TextureAsyncLoadManager instance = TextureAsyncLoadManager.Instance;
		if (instance == null)
		{
			return;
		}
		for (int i = 0; i < 29; i++)
		{
			CharaType type = (CharaType)i;
			TextureRequestChara request = new TextureRequestChara(type, null);
			instance.Remove(request);
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
