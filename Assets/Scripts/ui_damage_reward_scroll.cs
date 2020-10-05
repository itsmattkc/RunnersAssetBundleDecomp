using System;
using Text;
using UnityEngine;

public class ui_damage_reward_scroll : MonoBehaviour
{
	[SerializeField]
	private UITexture m_faceTexture;

	[SerializeField]
	private UISprite m_scoreRankIconSprite;

	[SerializeField]
	private UISprite m_friendIconSprite;

	[SerializeField]
	private UISprite m_charaSprite;

	[SerializeField]
	private UITexture m_mainChaoIcon;

	[SerializeField]
	private UITexture m_subChaoIcon;

	[SerializeField]
	private UISprite m_chao1BgSprite;

	[SerializeField]
	private UISprite m_chao2BgSprite;

	[SerializeField]
	private UILabel m_nameLabel;

	[SerializeField]
	private UILabel m_damageLabel;

	[SerializeField]
	private UILabel m_damageRateLabel;

	[SerializeField]
	private UISlider m_damage;

	[SerializeField]
	private UISprite m_destroyIcon;

	[SerializeField]
	private UILabel m_destroyCountLabel;

	[SerializeField]
	private TweenColor[] m_myRanker = new TweenColor[2];

	private UILabel m_lvLabel;

	private RaidBossData m_bossData;

	private RaidBossUser m_user;

	private bool m_isImgLoad;

	private bool m_myCell;

	private static float s_updateLastTime;

	public void Start()
	{
	}

	private void LoadImage()
	{
		if (this.m_user != null && !this.m_isImgLoad && (this.m_myCell || this.m_user.isFriend))
		{
			this.m_faceTexture.mainTexture = RankingUtil.GetProfilePictureTexture(this.m_user.id, delegate(Texture2D _faceTexture)
			{
				this.m_faceTexture.mainTexture = _faceTexture;
				this.m_faceTexture.alpha = 1f;
			});
			if (this.m_faceTexture.mainTexture != null)
			{
				this.m_faceTexture.alpha = 1f;
			}
			this.m_isImgLoad = true;
		}
	}

	public void UpdateView(RaidBossUser user, RaidBossData bossData)
	{
		if (user == null)
		{
			return;
		}
		this.m_myCell = false;
		this.m_isImgLoad = false;
		this.m_bossData = bossData;
		this.m_user = user;
		if (this.m_lvLabel == null)
		{
			this.m_lvLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_lv");
		}
		this.SetChaoTexture(this.m_mainChaoIcon, this.m_user.mainChaoId);
		this.SetChaoTexture(this.m_subChaoIcon, this.m_user.subChaoId);
		if (this.m_user.isFriend)
		{
			this.LoadImage();
		}
		else
		{
			this.m_faceTexture.mainTexture = PlayerImageManager.GetPlayerDefaultImage();
		}
		this.m_scoreRankIconSprite.enabled = this.m_user.isRankTop;
		if (this.m_friendIconSprite != null)
		{
			this.m_friendIconSprite.spriteName = RankingUtil.GetFriendIconSpriteName(this.m_user);
		}
		if (this.m_user.charaType != CharaType.UNKNOWN)
		{
			if (AtlasManager.Instance != null)
			{
				AtlasManager.Instance.ReplacePlayerAtlasForRaidResult(this.m_charaSprite.atlas);
			}
			this.m_charaSprite.spriteName = "ui_tex_player_set_" + CharaTypeUtil.GetCharaSpriteNameSuffix(this.m_user.charaType);
		}
		if (this.m_lvLabel != null)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_LevelNumber").text;
			this.m_lvLabel.text = text.Replace("{PARAM}", user.charaLevel.ToString());
		}
		if (this.m_chao1BgSprite != null)
		{
			if (this.m_user.mainChaoId == -1)
			{
				this.m_chao1BgSprite.gameObject.SetActive(false);
			}
			else
			{
				this.m_chao1BgSprite.gameObject.SetActive(true);
				this.m_chao1BgSprite.spriteName = "ui_ranking_scroll_char_bg_" + this.m_user.mainChaoRarity;
			}
		}
		if (this.m_chao2BgSprite != null)
		{
			if (this.m_user.subChaoId == -1)
			{
				this.m_chao2BgSprite.gameObject.SetActive(false);
			}
			else
			{
				this.m_chao2BgSprite.gameObject.SetActive(true);
				this.m_chao2BgSprite.spriteName = "ui_ranking_scroll_char_bg_S" + this.m_user.subChaoRarity;
			}
		}
		this.m_nameLabel.text = this.m_user.userName;
		if (this.m_damage != null && this.m_damageLabel != null && this.m_damageRateLabel != null)
		{
			this.m_damageLabel.text = HudUtility.GetFormatNumString<long>(this.m_user.damage);
			float num = (float)this.m_user.damage / (float)this.m_bossData.hpMax;
			int num2;
			if (num < 0f)
			{
				num = 0f;
				num2 = 0;
			}
			else if (num > 1f)
			{
				num = 1f;
				num2 = 100;
			}
			else
			{
				int num3 = Mathf.FloorToInt(num * 10000f);
				if (num3 > 1)
				{
					num3--;
				}
				num = (float)num3 / 10000f;
				num2 = Mathf.CeilToInt(100f * num);
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			else if (num2 >= 100)
			{
				num2 = 100;
				if (this.m_user.damage < this.m_bossData.hpMax)
				{
					num2 = 99;
				}
			}
			string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "odds").text;
			this.m_damageRateLabel.text = TextUtility.Replace(text2, "{ODDS}", string.Empty + num2);
			this.m_damage.value = num;
			this.m_damage.ForceUpdate();
		}
		if (this.m_destroyCountLabel != null)
		{
			this.m_destroyCountLabel.text = HudUtility.GetFormatNumString<long>(this.m_user.destroyCount);
		}
		if (this.m_destroyIcon != null)
		{
			this.m_destroyIcon.enabled = this.m_user.isDestroy;
		}
	}

	public void SetMyRanker(bool myCell)
	{
		this.m_myCell = myCell;
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "Btn_damage_reward_top");
		if (uISprite != null)
		{
			if (uISprite.color.r < 1f || uISprite.color.g < 1f || uISprite.color.b < 1f)
			{
				uISprite.color = new Color(1f, 1f, 1f, 1f);
			}
			uISprite.spriteName = ((!myCell) ? "ui_event_raidboss_damage_reward_bar1" : "ui_event_raidboss_damage_reward_bar2");
		}
		for (int i = 0; i < this.m_myRanker.Length; i++)
		{
			if (this.m_myRanker[i] != null)
			{
				this.m_myRanker[i].enabled = myCell;
			}
		}
		if (myCell)
		{
			this.m_isImgLoad = false;
			this.LoadImage();
		}
	}

	public void UpdateSendChallenge(string id)
	{
		if (base.gameObject.activeSelf && this.m_user != null && this.m_user.id == id)
		{
			this.m_user.isSentEnergy = true;
			if (this.m_friendIconSprite != null)
			{
				this.m_friendIconSprite.spriteName = RankingUtil.GetFriendIconSpriteName(this.m_user);
			}
		}
	}

	private void SetChaoTexture(UITexture uiTex, int chaoId)
	{
		if (uiTex != null)
		{
			if (chaoId >= 0 && SingletonGameObject<RankingManager>.Instance != null)
			{
				SingletonGameObject<RankingManager>.Instance.GetChaoTexture(chaoId, uiTex, 0.2f, false);
				uiTex.gameObject.SetActive(true);
			}
			else
			{
				uiTex.gameObject.SetActive(false);
			}
		}
	}

	public void SetClickCollision(bool flag)
	{
		UIButtonOffset uIButtonOffset = GameObjectUtil.FindChildGameObjectComponent<UIButtonOffset>(base.gameObject, "Btn_damage_reward_top");
		if (uIButtonOffset != null)
		{
			uIButtonOffset.enabled = flag;
		}
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_damage_reward_top");
		if (uIButtonMessage != null)
		{
			uIButtonMessage.enabled = flag;
		}
	}

	private void OnClickUserScroll()
	{
		if (Mathf.Abs(ui_damage_reward_scroll.s_updateLastTime - Time.realtimeSinceStartup) > 1.5f)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			ranking_window.RaidOpen(this.m_user);
		}
	}
}
