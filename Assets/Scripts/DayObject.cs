using AnimationOrTween;
using System;
using UnityEngine;

public class DayObject
{
	public GameObject m_clearGameObject;

	private GameObject m_effect;

	private Animation m_clearAnimation;

	private UISprite m_imgCheck;

	private UISprite m_imgItem;

	private UISprite m_imgChara;

	private UITexture m_imgChao;

	private UISprite m_imgHidden;

	private UILabel m_lblCount;

	private int m_count;

	public int count
	{
		get
		{
			return this.m_count;
		}
		set
		{
			if (this.m_count != value)
			{
				this.m_count = value;
				if (this.m_lblCount != null)
				{
					this.m_lblCount.text = HudUtility.GetFormatNumString<int>(this.m_count);
				}
			}
		}
	}

	public DayObject(GameObject obj, Color color, int day)
	{
		this.m_clearGameObject = obj;
		this.m_effect = GameObjectUtil.FindChildGameObject(this.m_clearGameObject, "eff_4");
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_clearGameObject, "img_day_num");
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_clearGameObject, "img_frame_color");
		this.m_imgItem = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_clearGameObject, "img_daily_item");
		this.m_imgChara = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_clearGameObject, "img_chara");
		this.m_imgChao = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_clearGameObject, "img_chao");
		this.m_lblCount = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_clearGameObject, "Lbl_count");
		this.m_imgHidden = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_clearGameObject, "img_hidden");
		this.m_imgCheck = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_clearGameObject, "img_check");
		if (this.m_effect != null)
		{
			this.m_effect.SetActive(false);
		}
		if (uISprite2 != null)
		{
			uISprite2.color = color;
		}
		if (uISprite != null)
		{
			uISprite.spriteName = "ui_daily_num_" + day;
		}
		if (this.m_lblCount != null)
		{
			this.m_lblCount.text = "0";
		}
		if (this.m_imgItem != null)
		{
			this.m_imgItem.spriteName = string.Empty;
		}
		if (this.m_imgChara != null)
		{
			this.m_imgChara.spriteName = string.Empty;
			this.m_imgChara.alpha = 0f;
		}
		if (this.m_imgChao != null)
		{
			this.m_imgChao.mainTexture = null;
			this.m_imgChao.alpha = 0f;
		}
		if (this.m_imgHidden != null)
		{
			this.m_imgHidden.enabled = false;
		}
		if (this.m_imgCheck != null)
		{
			this.m_imgCheck.enabled = false;
		}
		this.m_clearAnimation = obj.GetComponentInChildren<Animation>();
	}

	public void SetAlready(bool already)
	{
		if (this.m_imgHidden != null)
		{
			this.m_imgHidden.enabled = already;
		}
		if (this.m_imgCheck != null)
		{
			this.m_imgCheck.enabled = already;
		}
	}

	public void PlayGetAnimation()
	{
		if (this.m_imgCheck != null)
		{
			this.m_imgCheck.enabled = true;
		}
		if (this.m_clearAnimation != null)
		{
			if (this.m_effect != null)
			{
				this.m_effect.SetActive(true);
			}
			ActiveAnimation.Play(this.m_clearAnimation, Direction.Forward);
		}
	}

	public bool SetItem(int id)
	{
		if (this.m_imgItem != null && this.m_imgChara != null && this.m_imgChao != null)
		{
			if (id >= 0)
			{
				int num = Mathf.FloorToInt((float)id / 100000f);
				int num2 = num;
				if (num2 != 3)
				{
					if (num2 != 4)
					{
						this.m_imgItem.alpha = 1f;
						this.m_imgChara.alpha = 0f;
						this.m_imgChao.alpha = 0f;
						this.m_imgItem.spriteName = "ui_cmn_icon_item_" + id;
					}
					else
					{
						this.m_imgItem.alpha = 0f;
						this.m_imgChara.alpha = 0f;
						this.m_imgChao.alpha = 1f;
						ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(this.m_imgChao, null, true);
						ChaoTextureManager.Instance.GetTexture(id - 400000, info);
					}
				}
				else
				{
					this.m_imgItem.alpha = 0f;
					this.m_imgChara.alpha = 1f;
					this.m_imgChao.alpha = 0f;
					UISprite arg_B1_0 = this.m_imgChara;
					string arg_AC_0 = "ui_tex_player_";
					ServerItem serverItem = new ServerItem((ServerItem.Id)id);
					arg_B1_0.spriteName = arg_AC_0 + CharaTypeUtil.GetCharaSpriteNameSuffix(serverItem.charaType);
				}
			}
			else
			{
				this.m_imgItem.spriteName = "ui_cmn_icon_rsring_L";
			}
			return true;
		}
		return false;
	}
}
