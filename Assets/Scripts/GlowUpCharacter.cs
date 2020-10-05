using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class GlowUpCharacter : MonoBehaviour
{
	public delegate void GlowUpEndCallback();

	private sealed class _OnSetup_c__Iterator25 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal CharaType _charaType___0;

		internal UISprite _charaIcon___1;

		internal string _spriteName___2;

		internal UILabel _levelLabel___3;

		internal int _level___4;

		internal UILabel _charaNameLabel___5;

		internal string _charaName___6;

		internal UISprite _typeSprite___7;

		internal UISprite _teamTypeSprite___8;

		internal UISlider _baseSlider___9;

		internal UISlider _glowUpSlider___10;

		internal UILabel _expLabel___11;

		internal GlowUpExpBar.ExpInfo _startInfo___12;

		internal GameObject _abilityRootObject___13;

		internal int _PC;

		internal object _current;

		internal GlowUpCharacter __f__this;

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
				this._charaType___0 = this.__f__this.m_baseInfo.charaType;
				this._charaIcon___1 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.__f__this.gameObject, "img_player");
				if (this._charaIcon___1 != null)
				{
					this._spriteName___2 = HudUtility.MakeCharaTextureName(this._charaType___0, HudUtility.TextureType.TYPE_S);
					this._charaIcon___1.spriteName = this._spriteName___2;
				}
				this._levelLabel___3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.__f__this.gameObject, "Lbl_player_lv");
				if (this._levelLabel___3 != null)
				{
					this._level___4 = this.__f__this.m_baseInfo.level;
					this._levelLabel___3.text = TextUtility.GetTextLevel(this._level___4.ToString());
				}
				this._charaNameLabel___5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.__f__this.gameObject, "Lbl_player_name");
				if (this._charaNameLabel___5 != null)
				{
					this._charaName___6 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", CharaName.Name[(int)this._charaType___0]).text;
					this._charaNameLabel___5.text = this._charaName___6;
				}
				this._typeSprite___7 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.__f__this.gameObject, "img_player_speacies");
				if (this._typeSprite___7 != null)
				{
					this._typeSprite___7.spriteName = HudUtility.GetCharaAttributeSpriteName(this._charaType___0);
				}
				this._teamTypeSprite___8 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.__f__this.gameObject, "img_player_genus");
				if (this._teamTypeSprite___8 != null)
				{
					this._teamTypeSprite___8.spriteName = HudUtility.GetTeamAttributeSpriteName(this._charaType___0);
				}
				if (this.__f__this.m_expBar == null)
				{
					this.__f__this.m_expBar = this.__f__this.gameObject.AddComponent<GlowUpExpBar>();
					this._baseSlider___9 = GameObjectUtil.FindChildGameObjectComponent<UISlider>(this.__f__this.gameObject, "Pgb_b4exp");
					this._glowUpSlider___10 = GameObjectUtil.FindChildGameObjectComponent<UISlider>(this.__f__this.gameObject, "Pgb_exp");
					this._expLabel___11 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.__f__this.gameObject, "Lbl_price_number");
					if (this.__f__this.m_expBar != null)
					{
						this.__f__this.m_expBar.SetBaseSlider(this._baseSlider___9);
						this.__f__this.m_expBar.SetGlowUpSlider(this._glowUpSlider___10);
						this.__f__this.m_expBar.SetExpLabel(this._expLabel___11);
						this.__f__this.m_expBar.SetCallback(new GlowUpExpBar.LevelUpCallback(this.__f__this.ExpBarLevelUpCallback), new GlowUpExpBar.EndCallback(this.__f__this.ExpBarEndCallback));
					}
				}
				if (this.__f__this.m_expBar != null)
				{
					this._startInfo___12 = new GlowUpExpBar.ExpInfo();
					this._startInfo___12.level = this.__f__this.m_baseInfo.level;
					this._startInfo___12.cost = this.__f__this.m_baseInfo.levelUpCost;
					this._startInfo___12.exp = this.__f__this.m_baseInfo.currentExp;
					this.__f__this.m_expBar.SetStartExp(this._startInfo___12);
				}
				if (this.__f__this.m_abilityPanel == null)
				{
					this._abilityRootObject___13 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "ui_player_set_item_2_cell(Clone)");
					if (this._abilityRootObject___13 != null)
					{
						this.__f__this.m_abilityPanel = this._abilityRootObject___13.AddComponent<MenuPlayerSetAbilityButton>();
					}
				}
				this.__f__this.m_isEndSetup = true;
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

	private sealed class _OnPlayStart_c__Iterator26 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GlowUpCharaAfterInfo afterInfo;

		internal GlowUpExpBar.ExpInfo _endInfo___0;

		internal int _PC;

		internal object _current;

		internal GlowUpCharaAfterInfo ___afterInfo;

		internal GlowUpCharacter __f__this;

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
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this.__f__this.m_isEndSetup)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this.__f__this.m_afterInfo = this.afterInfo;
			this.__f__this.m_isEnd = false;
			if (this.__f__this.m_expBar != null)
			{
				this._endInfo___0 = new GlowUpExpBar.ExpInfo();
				this._endInfo___0.level = this.__f__this.m_afterInfo.level;
				this._endInfo___0.cost = this.__f__this.m_afterInfo.levelUpCost;
				this._endInfo___0.exp = this.__f__this.m_afterInfo.exp;
				this.__f__this.m_expBar.SetEndExp(this._endInfo___0);
				this.__f__this.m_expBar.PlayStart();
				this.__f__this.m_expBar.SetLevelUpCostList(this.__f__this.m_afterInfo.abilityListExp);
			}
			this._PC = -1;
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

	private GlowUpCharaBaseInfo m_baseInfo;

	private GlowUpCharaAfterInfo m_afterInfo;

	private GlowUpExpBar m_expBar;

	private MenuPlayerSetAbilityButton m_abilityPanel;

	private bool m_isEndSetup;

	private bool m_isEnd;

	public bool IsEndSetup
	{
		get
		{
			return this.m_isEndSetup;
		}
	}

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public void Setup(GlowUpCharaBaseInfo baseInfo)
	{
		this.m_baseInfo = baseInfo;
		if (!this.m_baseInfo.IsActive)
		{
			this.m_isEndSetup = true;
			this.m_isEnd = true;
			base.gameObject.SetActive(false);
			return;
		}
		base.StartCoroutine(this.OnSetup(baseInfo));
	}

	private IEnumerator OnSetup(GlowUpCharaBaseInfo baseInfo)
	{
		GlowUpCharacter._OnSetup_c__Iterator25 _OnSetup_c__Iterator = new GlowUpCharacter._OnSetup_c__Iterator25();
		_OnSetup_c__Iterator.__f__this = this;
		return _OnSetup_c__Iterator;
	}

	public void PlayStart(GlowUpCharaAfterInfo afterInfo)
	{
		if (this.m_isEnd)
		{
			return;
		}
		base.StartCoroutine(this.OnPlayStart(afterInfo));
	}

	public void PlaySkip()
	{
		if (this.m_expBar != null)
		{
			this.m_expBar.PlaySkip();
		}
		if (this.m_abilityPanel != null)
		{
			this.m_abilityPanel.SkipLevelUp();
		}
	}

	private IEnumerator OnPlayStart(GlowUpCharaAfterInfo afterInfo)
	{
		GlowUpCharacter._OnPlayStart_c__Iterator26 _OnPlayStart_c__Iterator = new GlowUpCharacter._OnPlayStart_c__Iterator26();
		_OnPlayStart_c__Iterator.afterInfo = afterInfo;
		_OnPlayStart_c__Iterator.___afterInfo = afterInfo;
		_OnPlayStart_c__Iterator.__f__this = this;
		return _OnPlayStart_c__Iterator;
	}

	private void ExpBarLevelUpCallback(int level)
	{
		SoundManager.SePlay("sys_buy", "SE");
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_player_lv");
		if (uILabel != null)
		{
			uILabel.text = TextUtility.GetTextLevel(level.ToString());
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_slot_mask");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		if (this.m_abilityPanel != null && this.m_afterInfo.abilityList.Count > 0)
		{
			AbilityType abilityType = this.m_afterInfo.abilityList[0];
			this.m_abilityPanel.Setup(this.m_baseInfo.charaType, abilityType);
			this.m_abilityPanel.LevelUp(new MenuPlayerSetAbilityButton.AnimEndCallback(this.LevelUpAnimationEndCallback));
			this.m_afterInfo.abilityList.Remove(abilityType);
		}
	}

	private void LevelUpAnimationEndCallback()
	{
	}

	private void ExpBarEndCallback()
	{
		this.m_isEnd = true;
	}
}
