using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MenuPlayerSetCharaPage : MonoBehaviour
{
	private sealed class _SetupCoroutine_c__Iterator48 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _buttonRoot___0;

		internal List<GameObject> _buttonList___1;

		internal int _index___2;

		internal GameObject _buttonObjectRoot___3;

		internal MenuPlayerSetAbilityButton _button___4;

		internal AbilityType _abilityType___5;

		internal AbilityType _nextLevelUpAbility___6;

		internal MenuPlayerSetAbilityButton _nextLevelUpButton___7;

		internal ServerPlayerState _playerState___8;

		internal ServerCharacterState _charaState___9;

		internal SaveDataManager _save_data_manager___10;

		internal CharaData _charaData___11;

		internal GameObject _playerSetRoot___12;

		internal UIButtonMessage _buttonMessage___13;

		internal int _PC;

		internal object _current;

		internal MenuPlayerSetCharaPage __f__this;

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
				this.__f__this.m_charaButton = this.__f__this.gameObject.AddComponent<MenuPlayerSetCharaButton>();
				if (this.__f__this.m_charaButton != null)
				{
					this.__f__this.m_charaButton.Setup(this.__f__this.m_charaType, this.__f__this.m_pageRoot);
				}
				this.__f__this.m_levelUpButton = this.__f__this.gameObject.AddComponent<MenuPlayerSetLevelUpButton>();
				if (this.__f__this.m_levelUpButton != null)
				{
					this.__f__this.m_levelUpButton.Setup(this.__f__this.m_charaType, this.__f__this.m_pageRoot);
					this.__f__this.m_levelUpButton.SetCallback(new MenuPlayerSetLevelUpButton.LevelUpCallback(this.__f__this.LevelUppedCallback));
				}
				this._buttonRoot___0 = GameObjectUtil.FindChildGameObject(this.__f__this.m_pageRoot, "slot");
				if (!(this._buttonRoot___0 != null))
				{
					goto IL_248;
				}
				this._buttonList___1 = null;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._buttonList___1 = GameObjectUtil.FindChildGameObjects(this._buttonRoot___0, "ui_player_set_item_2_cell(Clone)");
			if (this._buttonList___1 == null || this._buttonList___1.Count < 10)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this._buttonList___1 != null)
			{
				this._index___2 = 0;
				while (this._index___2 < this._buttonList___1.Count)
				{
					this._buttonObjectRoot___3 = this._buttonList___1[this._index___2];
					if (!(this._buttonObjectRoot___3 == null))
					{
						this._button___4 = this._buttonObjectRoot___3.AddComponent<MenuPlayerSetAbilityButton>();
						if (!(this._button___4 == null))
						{
							this._abilityType___5 = MenuPlayerSetUtil.AbilityLevelUpOrder[this._index___2];
							this._button___4.Setup(this.__f__this.m_charaType, this._abilityType___5);
							this.__f__this.m_abilityButton[(int)this._abilityType___5] = this._button___4;
						}
					}
					this._index___2++;
				}
			}
			IL_248:
			this._nextLevelUpAbility___6 = MenuPlayerSetUtil.GetNextLevelUpAbility(this.__f__this.m_charaType);
			this._nextLevelUpButton___7 = this.__f__this.m_abilityButton[(int)this._nextLevelUpAbility___6];
			if (this._nextLevelUpButton___7 != null)
			{
				this._nextLevelUpButton___7.SetActive(true);
			}
			if (ServerInterface.LoggedInServerInterface != null)
			{
				this._playerState___8 = ServerInterface.PlayerState;
				if (this._playerState___8 != null)
				{
					this._charaState___9 = this._playerState___8.CharacterState(this.__f__this.m_charaType);
					if (this._charaState___9 != null && !this._charaState___9.IsUnlocked)
					{
						this.__f__this.m_unlocked = this.__f__this.gameObject.AddComponent<MenuPlayerSetUnlockedChara>();
						this.__f__this.m_unlocked.Setup(this.__f__this.m_charaType, this.__f__this.m_pageRoot);
					}
				}
			}
			else
			{
				this._save_data_manager___10 = SaveDataManager.Instance;
				if (this._save_data_manager___10 != null)
				{
					this._charaData___11 = this._save_data_manager___10.CharaData;
					if (this._charaData___11.Status[(int)this.__f__this.m_charaType] == 0)
					{
						this.__f__this.m_unlocked = this.__f__this.gameObject.AddComponent<MenuPlayerSetUnlockedChara>();
						this.__f__this.m_unlocked.Setup(this.__f__this.m_charaType, this.__f__this.m_pageRoot);
					}
				}
			}
			this._playerSetRoot___12 = MenuPlayerSetUtil.GetPlayerSetRoot();
			if (this._playerSetRoot___12 != null)
			{
				this.__f__this.m_blinderObject = GameObjectUtil.FindChildGameObject(this._playerSetRoot___12, "blinder");
				if (this.__f__this.m_blinderObject != null)
				{
					this._buttonMessage___13 = this.__f__this.m_blinderObject.AddComponent<UIButtonMessage>();
					this._buttonMessage___13.target = this.__f__this.gameObject;
					this._buttonMessage___13.functionName = "OnClickedSkipButton";
				}
			}
			this.__f__this.m_isEndSetup = true;
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

	private static readonly string[] HideObjectName = new string[]
	{
		"Btn_lv_up",
		"Btn_player_main",
		"_guide",
		"slot"
	};

	private GameObject m_pageRoot;

	private CharaType m_charaType;

	private MenuPlayerSetUnlockedChara m_unlocked;

	private MenuPlayerSetCharaButton m_charaButton;

	private MenuPlayerSetLevelUpButton m_levelUpButton;

	private MenuPlayerSetAbilityButton[] m_abilityButton = new MenuPlayerSetAbilityButton[10];

	private GameObject m_blinderObject;

	private bool m_isEndSetup;

	public bool IsEndSetUp
	{
		get
		{
			return this.m_isEndSetup;
		}
		private set
		{
		}
	}

	public void Setup(GameObject pageRoot, CharaType charaType)
	{
		base.gameObject.SetActive(true);
		this.m_isEndSetup = false;
		this.m_pageRoot = pageRoot;
		this.m_charaType = charaType;
		if (this.m_pageRoot == null)
		{
			return;
		}
		string[] hideObjectName = MenuPlayerSetCharaPage.HideObjectName;
		for (int i = 0; i < hideObjectName.Length; i++)
		{
			string text = hideObjectName[i];
			if (text != null)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, text);
				if (!(gameObject == null))
				{
					gameObject.SetActive(true);
				}
			}
		}
		base.StartCoroutine(this.SetupCoroutine());
	}

	private IEnumerator SetupCoroutine()
	{
		MenuPlayerSetCharaPage._SetupCoroutine_c__Iterator48 _SetupCoroutine_c__Iterator = new MenuPlayerSetCharaPage._SetupCoroutine_c__Iterator48();
		_SetupCoroutine_c__Iterator.__f__this = this;
		return _SetupCoroutine_c__Iterator;
	}

	public void OnSelected()
	{
		if (this.m_charaButton != null)
		{
			this.m_charaButton.OnSelected();
		}
	}

	public void UpdateRibbon()
	{
		if (this.m_charaButton != null)
		{
			this.m_charaButton.UpdateRibbon();
		}
	}

	private void LevelUppedCallback(AbilityType levelUppedAbility)
	{
		if (this.m_charaButton != null)
		{
			this.m_charaButton.LevelUp(new MenuPlayerSetCharaButton.AnimEndCallback(this.LevelUpAnimationEndCallback));
		}
		if (this.m_blinderObject != null)
		{
			this.m_blinderObject.SetActive(true);
		}
		MenuPlayerSetAbilityButton menuPlayerSetAbilityButton = this.m_abilityButton[(int)levelUppedAbility];
		if (menuPlayerSetAbilityButton != null)
		{
			menuPlayerSetAbilityButton.LevelUp(new MenuPlayerSetAbilityButton.AnimEndCallback(this.LevelUpAnimationEndCallback));
		}
		AbilityType nextLevelUpAbility = MenuPlayerSetUtil.GetNextLevelUpAbility(this.m_charaType);
		MenuPlayerSetAbilityButton menuPlayerSetAbilityButton2 = this.m_abilityButton[(int)nextLevelUpAbility];
		if (menuPlayerSetAbilityButton2 != null)
		{
			menuPlayerSetAbilityButton2.SetActive(true);
		}
		if (this.m_levelUpButton != null)
		{
			this.m_levelUpButton.InitCostLabel();
		}
	}

	private void OnClickedSkipButton()
	{
		MenuPlayerSetAbilityButton[] abilityButton = this.m_abilityButton;
		for (int i = 0; i < abilityButton.Length; i++)
		{
			MenuPlayerSetAbilityButton menuPlayerSetAbilityButton = abilityButton[i];
			if (!(menuPlayerSetAbilityButton == null))
			{
				menuPlayerSetAbilityButton.SkipLevelUp();
			}
		}
		if (this.m_charaButton != null)
		{
			this.m_charaButton.SkipLevelUp();
		}
	}

	private void LevelUpAnimationEndCallback()
	{
		if (this.m_charaButton != null && !this.m_charaButton.AnimEnd)
		{
			return;
		}
		if (this.m_abilityButton != null)
		{
			MenuPlayerSetAbilityButton[] abilityButton = this.m_abilityButton;
			for (int i = 0; i < abilityButton.Length; i++)
			{
				MenuPlayerSetAbilityButton menuPlayerSetAbilityButton = abilityButton[i];
				if (!(menuPlayerSetAbilityButton == null))
				{
					if (!menuPlayerSetAbilityButton.AnimEnd)
					{
						return;
					}
				}
			}
		}
		if (this.m_blinderObject != null)
		{
			this.m_blinderObject.SetActive(false);
		}
		if (this.m_levelUpButton != null)
		{
			this.m_levelUpButton.OnLevelUpEnd();
		}
	}
}
