using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class MenuPlayerSetAbilityButton : MonoBehaviour
{
	private enum State
	{
		IDLE,
		LEVELUP
	}

	public delegate void AnimEndCallback();

	private sealed class _HideEffectObject_c__Iterator47 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _effectObject___0;

		internal int _PC;

		internal object _current;

		internal MenuPlayerSetAbilityButton __f__this;

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
				this._effectObject___0 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "img_cursor_eff_set");
				if (this._effectObject___0 != null)
				{
					this._effectObject___0.SetActive(false);
				}
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

	private AbilityButtonParams m_params;

	private MenuPlayerSetLevelState m_currentState;

	private MenuPlayerSetAbilityButton.State m_state;

	private float m_levelUpAnimTime;

	private readonly float LevelUpAnimTotalTime = 1f;

	private MenuPlayerSetAbilityButton.AnimEndCallback m_animEndCallback;

	private bool m_animEnd = true;

	public bool AnimEnd
	{
		get
		{
			return this.m_animEnd;
		}
		private set
		{
		}
	}

	private void Start()
	{
	}

	private void LateUpdate()
	{
		MenuPlayerSetAbilityButton.State state = this.m_state;
		if (state != MenuPlayerSetAbilityButton.State.IDLE)
		{
			if (state == MenuPlayerSetAbilityButton.State.LEVELUP)
			{
				ImportAbilityTable instance = ImportAbilityTable.GetInstance();
				int maxLevel = instance.GetMaxLevel(this.m_params.Ability);
				int level = MenuPlayerSetUtil.GetLevel(this.m_params.Character, this.m_params.Ability);
				UISlider uISlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(base.gameObject, "Pgb_item_lv");
				if (uISlider != null)
				{
					this.m_levelUpAnimTime += Time.deltaTime;
					if (this.m_levelUpAnimTime >= this.LevelUpAnimTotalTime)
					{
						this.m_levelUpAnimTime = this.LevelUpAnimTotalTime;
						this.m_state = MenuPlayerSetAbilityButton.State.IDLE;
					}
					float num = (float)level - 1f + this.m_levelUpAnimTime / this.LevelUpAnimTotalTime;
					num /= (float)maxLevel;
					uISlider.value = num;
				}
			}
		}
	}

	public void Setup(CharaType charaType, AbilityType abilityType)
	{
		this.m_params = new AbilityButtonParams();
		this.m_params.Character = charaType;
		this.m_params.Ability = abilityType;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_cursor_eff_set");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		this.InitLabels();
		this.InitButtonState();
		UISlider uISlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(base.gameObject, "Pgb_item_lv");
		if (uISlider != null)
		{
			ImportAbilityTable instance = ImportAbilityTable.GetInstance();
			int maxLevel = instance.GetMaxLevel(this.m_params.Ability);
			int level = MenuPlayerSetUtil.GetLevel(this.m_params.Character, this.m_params.Ability);
			uISlider.value = (float)level / (float)maxLevel;
		}
		this.m_state = MenuPlayerSetAbilityButton.State.IDLE;
	}

	public void SetActive(bool isActive)
	{
		GameObject x = GameObjectUtil.FindChildGameObject(base.gameObject, "img_cursor");
		if (x != null)
		{
		}
	}

	public void LevelUp(MenuPlayerSetAbilityButton.AnimEndCallback callback)
	{
		this.m_animEndCallback = callback;
		this.m_animEnd = false;
		ImportAbilityTable instance = ImportAbilityTable.GetInstance();
		int maxLevel = instance.GetMaxLevel(this.m_params.Ability);
		int level = MenuPlayerSetUtil.GetLevel(this.m_params.Character, this.m_params.Ability);
		if (level >= maxLevel)
		{
			UnityEngine.Object.Destroy(this.m_currentState);
			this.m_currentState = base.gameObject.AddComponent<MenuPlayerSetLevelStateMax>();
			this.m_currentState.Setup(this.m_params);
			this.AwakeLevelMax();
		}
		this.SetPotentialText();
		this.ChangeLevelLabels();
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "Btn_toggle");
		if (animation != null)
		{
			animation.Stop();
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.LevelUpAnimationEndCallback), true);
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_cursor_eff_set");
			if (gameObject != null)
			{
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_effect_b4");
				if (uILabel != null)
				{
					uILabel.text = instance.GetAbilityPotential(this.m_params.Ability, level - 1).ToString();
				}
				UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_effect_af");
				if (uILabel2 != null)
				{
					uILabel2.text = instance.GetAbilityPotential(this.m_params.Ability, level).ToString();
				}
				gameObject.SetActive(true);
			}
			this.SetActive(false);
		}
		this.m_levelUpAnimTime = 0f;
		this.m_state = MenuPlayerSetAbilityButton.State.LEVELUP;
	}

	public void SkipLevelUp()
	{
		if (this.m_animEndCallback == null)
		{
			return;
		}
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "Btn_toggle");
		if (animation != null)
		{
			foreach (AnimationState animationState in animation)
			{
				if (!(animationState == null))
				{
					animationState.time = animationState.length * 0.99f;
				}
			}
		}
	}

	private void InitLabels()
	{
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_icon");
		if (uISprite != null)
		{
			string spriteName = "ui_mm_player_icon_" + ((int)this.m_params.Ability).ToString();
			uISprite.spriteName = spriteName;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_word_item_effect");
		if (uILabel != null)
		{
			string cellName = "abilitycaption" + ((int)(this.m_params.Ability + 1)).ToString();
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaStatus", cellName).text;
			uILabel.text = text;
		}
		this.SetPotentialText();
	}

	private void InitButtonState()
	{
		if (this.m_currentState != null)
		{
			return;
		}
		ImportAbilityTable instance = ImportAbilityTable.GetInstance();
		int maxLevel = instance.GetMaxLevel(this.m_params.Ability);
		int level = MenuPlayerSetUtil.GetLevel(this.m_params.Character, this.m_params.Ability);
		if (level >= maxLevel)
		{
			this.m_currentState = base.gameObject.AddComponent<MenuPlayerSetLevelStateMax>();
			this.AwakeLevelMax();
		}
		else
		{
			this.m_currentState = base.gameObject.AddComponent<MenuPlayerSetLevelStateNormal>();
		}
		this.m_currentState.Setup(this.m_params);
		this.ChangeLevelLabels();
	}

	private void SetPotentialText()
	{
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_item_effect");
		if (uILabel != null)
		{
			string cellName = "abilitypotential" + ((int)(this.m_params.Ability + 1)).ToString();
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaStatus", cellName);
			ImportAbilityTable instance = ImportAbilityTable.GetInstance();
			int level = MenuPlayerSetUtil.GetLevel(this.m_params.Character, this.m_params.Ability);
			text.ReplaceTag("{ABILITY_POTENTIAL}", instance.GetAbilityPotential(this.m_params.Ability, level).ToString());
			uILabel.text = text.text;
			UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_item_effect_sh");
			if (uILabel2 != null)
			{
				uILabel2.text = text.text;
			}
		}
	}

	private void ChangeLevelLabels()
	{
		if (this.m_currentState != null)
		{
			this.m_currentState.ChangeLabels();
		}
	}

	private void AwakeLevelMax()
	{
		UIImageButton component = base.gameObject.GetComponent<UIImageButton>();
		if (component != null)
		{
			component.isEnabled = false;
		}
	}

	private void LevelUpAnimationEndCallback()
	{
		this.m_animEnd = true;
		base.StartCoroutine(this.HideEffectObject());
		this.m_animEndCallback();
		this.m_animEndCallback = null;
	}

	private IEnumerator HideEffectObject()
	{
		MenuPlayerSetAbilityButton._HideEffectObject_c__Iterator47 _HideEffectObject_c__Iterator = new MenuPlayerSetAbilityButton._HideEffectObject_c__Iterator47();
		_HideEffectObject_c__Iterator.__f__this = this;
		return _HideEffectObject_c__Iterator;
	}
}
