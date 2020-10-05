using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class PlayerGetPartsOverlap : ChaoGetPartsBase
{
	public enum IntroType
	{
		NORMAL,
		NO_EGG,
		NONE
	}

	private sealed class _FirstOverlap_c__Iterator23 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal ActiveAnimation _activeAnim___0;

		internal int _PC;

		internal object _current;

		internal PlayerGetPartsOverlap __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this.__f__this.SetupItem();
				this._activeAnim___0 = ActiveAnimation.Play(this.__f__this.m_animation, "ui_ro_PlayerGetWindowUI_2_Anim", Direction.Forward);
				EventDelegate.Add(this._activeAnim___0.onFinished, new EventDelegate.Callback(this.__f__this.GetAnimationFinishCallback), true);
				this.__f__this.m_animation = null;
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private int m_severId;

	private int m_rarity;

	private int m_level;

	private int m_currentAnimCount;

	private List<ServerItem> m_itemList;

	private List<int> m_itemListNum;

	private CharacterDataNameInfo.Info m_info;

	private Animation m_animation;

	private GameObject m_rootGameObject;

	private int m_playSeCount;

	private PlayerGetPartsOverlap.IntroType m_introType;

	private ChaoGetPartsBase.BtnType m_buttonType;

	private UILabel m_caption;

	public void Init(int severId, int rarity, int level, Dictionary<int, ServerItemState> itemList, PlayerGetPartsOverlap.IntroType introType = PlayerGetPartsOverlap.IntroType.NORMAL)
	{
		this.m_playSeCount = 0;
		this.m_severId = severId;
		this.m_rarity = rarity;
		this.m_level = level;
		this.m_introType = introType;
		this.m_itemListNum = null;
		this.m_itemList = null;
		if (itemList != null && itemList.Count > 0)
		{
			this.m_itemListNum = new List<int>();
			this.m_itemList = new List<ServerItem>();
			Dictionary<int, ServerItemState>.KeyCollection keys = itemList.Keys;
			foreach (int current in keys)
			{
				ServerItem item = new ServerItem((ServerItem.Id)current);
				this.m_itemList.Add(item);
				this.m_itemListNum.Add(itemList[current].m_num);
			}
		}
		this.m_currentAnimCount = 0;
		this.m_info = CharacterDataNameInfo.Instance.GetDataByServerID(this.m_severId);
	}

	public override void Setup(GameObject chaoGetObjectRoot)
	{
		this.m_rootGameObject = chaoGetObjectRoot;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(chaoGetObjectRoot, "eff_set");
		gameObject.SetActive(true);
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_egg_0");
		if (uISprite != null)
		{
			string spriteName = "ui_roulette_egg_" + (2 * this.m_rarity).ToString();
			uISprite.spriteName = spriteName;
		}
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_egg_1");
		if (uISprite2 != null)
		{
			string spriteName2 = "ui_roulette_egg_" + (2 * this.m_rarity + 1).ToString();
			uISprite2.spriteName = spriteName2;
		}
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(chaoGetObjectRoot, "img_player");
		UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_item");
		if (uITexture != null && this.m_info != null)
		{
			uITexture.gameObject.SetActive(true);
			TextureRequestChara request = new TextureRequestChara(this.m_info.m_ID, uITexture);
			TextureAsyncLoadManager.Instance.Request(request);
		}
		if (uISprite3 != null)
		{
			uISprite3.gameObject.SetActive(false);
		}
		UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_player_speacies");
		if (uISprite4 != null && this.m_info != null)
		{
			uISprite4.gameObject.SetActive(true);
			switch (this.m_info.m_attribute)
			{
			case CharacterAttribute.SPEED:
				uISprite4.spriteName = "ui_mm_player_species_0";
				break;
			case CharacterAttribute.FLY:
				uISprite4.spriteName = "ui_mm_player_species_1";
				break;
			case CharacterAttribute.POWER:
				uISprite4.spriteName = "ui_mm_player_species_2";
				break;
			}
		}
		UISprite uISprite5 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_player_genus");
		if (uISprite5 != null && this.m_info != null)
		{
			uISprite5.gameObject.SetActive(true);
			uISprite5.spriteName = HudUtility.GetTeamAttributeSpriteName(this.m_info.m_ID);
		}
		this.m_caption = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_caption");
		if (this.m_caption != null)
		{
			this.m_caption.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "PlayerSet", "ui_Lbt_get_captions").text;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_player_name");
		if (uILabel != null && this.m_info != null)
		{
			uILabel.gameObject.SetActive(true);
			uILabel.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", this.m_info.m_name.ToLower()).text;
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_player_lv");
		if (uILabel2 != null && this.m_info != null)
		{
			uILabel2.gameObject.SetActive(true);
			uILabel2.text = string.Format("Lv.{0:D3}", this.m_level);
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_player_attribute");
		if (uILabel3 != null && this.m_info != null)
		{
			uILabel3.gameObject.SetActive(true);
			ServerPlayerState playerState = ServerInterface.PlayerState;
			if (playerState != null)
			{
				ServerCharacterState serverCharacterState = playerState.CharacterState(this.m_info.m_ID);
				uILabel3.text = serverCharacterState.GetCharaAttributeText();
			}
		}
		UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_item_name");
		UILabel uILabel5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_item_info");
		UILabel uILabel6 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_item_number");
		if (uILabel4 != null)
		{
			uILabel4.gameObject.SetActive(false);
		}
		if (uILabel5 != null)
		{
			uILabel5.gameObject.SetActive(false);
		}
		if (uILabel6 != null)
		{
			uILabel6.gameObject.SetActive(false);
		}
	}

	private void SetupItem()
	{
		ServerItem serverItem = this.m_itemList[this.m_currentAnimCount];
		int num = this.m_itemListNum[this.m_currentAnimCount];
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rootGameObject, "img_player");
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rootGameObject, "img_item");
		if (uISprite != null && this.m_info != null)
		{
			uISprite.gameObject.SetActive(false);
		}
		if (uISprite2 != null && AtlasManager.Instance != null)
		{
			uISprite2.atlas = AtlasManager.Instance.ItemAtlas;
			uISprite2.spriteName = serverItem.serverItemSpriteName;
			uISprite2.gameObject.SetActive(true);
		}
		UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rootGameObject, "img_player_speacies");
		if (uISprite3 != null && this.m_info != null)
		{
			uISprite3.gameObject.SetActive(false);
		}
		UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rootGameObject, "img_player_genus");
		if (uISprite4 != null && this.m_info != null)
		{
			uISprite4.gameObject.SetActive(false);
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_rootGameObject, "Lbl_player_name");
		if (uILabel != null && this.m_info != null)
		{
			uILabel.gameObject.SetActive(false);
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_rootGameObject, "Lbl_player_lv");
		if (uILabel2 != null && this.m_info != null)
		{
			uILabel2.gameObject.SetActive(false);
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_rootGameObject, "Lbl_player_attribute");
		if (uILabel3 != null && this.m_info != null)
		{
			uILabel3.gameObject.SetActive(false);
		}
		UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_rootGameObject, "Lbl_item_name");
		UILabel uILabel5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_rootGameObject, "Lbl_item_info");
		UILabel uILabel6 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_rootGameObject, "Lbl_item_number");
		if (uILabel4 != null)
		{
			uILabel4.gameObject.SetActive(true);
			uILabel4.text = serverItem.serverItemName;
			if (this.m_caption != null)
			{
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "overlap_get_item_caption").text;
				if (string.IsNullOrEmpty(text))
				{
					this.m_caption.text = serverItem.serverItemName;
				}
				else
				{
					this.m_caption.text = text.Replace("{ITEM}", serverItem.serverItemName);
				}
			}
		}
		if (uILabel5 != null)
		{
			uILabel5.gameObject.SetActive(true);
			uILabel5.text = serverItem.serverItemComment;
			UIDraggablePanel uIDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(this.m_rootGameObject, "window_chaoset_alpha_clip");
			if (uIDraggablePanel != null)
			{
				uIDraggablePanel.ResetPosition();
			}
		}
		if (uILabel6 != null)
		{
			uILabel6.gameObject.SetActive(true);
			uILabel6.text = string.Format("× {0:0000}", num);
		}
	}

	public override void PlayGetAnimation(Animation anim)
	{
		if (anim == null)
		{
			return;
		}
		if (this.m_currentAnimCount == 0)
		{
			this.m_animation = anim;
			if (this.m_itemList != null && this.m_itemList.Count > 0)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(anim, "ui_ro_PlayerGetWindowUI_1_Anim", Direction.Forward);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.GetAnimationFinishCallback), true);
				this.m_buttonType = ChaoGetPartsBase.BtnType.NEXT;
			}
			else
			{
				string clipName = "ui_ro_PlayerGetWindowUI_intro_Anim";
				switch (this.m_introType)
				{
				case PlayerGetPartsOverlap.IntroType.NO_EGG:
				case PlayerGetPartsOverlap.IntroType.NONE:
					clipName = "ui_ro_PlayerGetWindowUI_noegg_intro_Anim";
					break;
				}
				ActiveAnimation activeAnimation2 = ActiveAnimation.Play(anim, clipName, Direction.Forward);
				EventDelegate.Add(activeAnimation2.onFinished, new EventDelegate.Callback(this.GetAnimationFinishCallback), true);
				this.m_buttonType = ChaoGetPartsBase.BtnType.EQUIP_OK;
			}
		}
		else
		{
			this.SetupItem();
			this.m_animation = null;
			ActiveAnimation activeAnimation3 = ActiveAnimation.Play(anim, "ui_ro_PlayerGetWindowUI_2_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation3.onFinished, new EventDelegate.Callback(this.GetAnimationFinishCallback), true);
			if (this.m_currentAnimCount >= this.m_itemList.Count - 1)
			{
				this.m_buttonType = ChaoGetPartsBase.BtnType.OK;
			}
			else
			{
				this.m_buttonType = ChaoGetPartsBase.BtnType.NEXT;
			}
		}
	}

	public override ChaoGetPartsBase.BtnType GetButtonType()
	{
		if (RouletteUtility.loginRoulette)
		{
			return ChaoGetPartsBase.BtnType.OK;
		}
		return this.m_buttonType;
	}

	public override void PlayEndAnimation(Animation anim)
	{
		if (anim == null)
		{
			return;
		}
		if (this.m_itemList != null && this.m_itemList.Count > 0)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(anim, "ui_ro_PlayerGetWindowUI_3_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.EndAnimationFinishCallback), true);
		}
		else
		{
			ActiveAnimation activeAnimation2 = ActiveAnimation.Play(anim, "ui_ro_PlayerGetWindowUI_outro_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation2.onFinished, new EventDelegate.Callback(this.EndAnimationFinishCallback), true);
		}
	}

	public override void PlaySE(string seType)
	{
		if (seType == ChaoWindowUtility.SeHatch)
		{
			if (this.m_playSeCount > 0)
			{
				SoundManager.SePlay("sys_roulette_itemget", "SE");
			}
			else
			{
				SoundManager.SePlay("sys_chao_hatch", "SE");
			}
			this.m_playSeCount++;
		}
		else if (seType == ChaoWindowUtility.SeBreak)
		{
			SoundManager.SePlay("sys_chao_birthS", "SE");
		}
	}

	public override EasySnsFeed CreateEasySnsFeed(GameObject rootObject)
	{
		string anchorPath = "Camera/menu_Anim/RouletteUI/Anchor_5_MC";
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "feed_chao_get_caption").text;
		TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "feed_chao_get_text");
		string text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", this.m_info.m_name.ToLower()).text;
		text2.ReplaceTag("{CHAO}", text3);
		return new EasySnsFeed(rootObject, anchorPath, text, text2.text, null);
	}

	private void GetAnimationFinishCallback()
	{
		if (this.m_currentAnimCount == 0 && this.m_itemList != null && this.m_itemList.Count > 0 && this.m_animation != null)
		{
			base.StartCoroutine(this.FirstOverlap());
		}
		else
		{
			this.m_currentAnimCount++;
			if (this.m_itemList == null || this.m_currentAnimCount >= this.m_itemList.Count)
			{
				this.m_callback(ChaoGetPartsBase.AnimType.GET_ANIM_FINISH);
			}
			else
			{
				this.m_callback(ChaoGetPartsBase.AnimType.GET_ANIM_CONTINUE);
			}
		}
	}

	private IEnumerator FirstOverlap()
	{
		PlayerGetPartsOverlap._FirstOverlap_c__Iterator23 _FirstOverlap_c__Iterator = new PlayerGetPartsOverlap._FirstOverlap_c__Iterator23();
		_FirstOverlap_c__Iterator.__f__this = this;
		return _FirstOverlap_c__Iterator;
	}

	private void EndAnimationFinishCallback()
	{
		this.m_callback(ChaoGetPartsBase.AnimType.OUT_ANIM);
	}
}
