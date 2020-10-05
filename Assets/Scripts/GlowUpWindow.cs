using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UI;
using UnityEngine;

public class GlowUpWindow : MonoBehaviour
{
	public enum ExpType
	{
		RUN_STAGE,
		BOSS_SUCCESS,
		BOSS_FAILED
	}

	private enum Type
	{
		None = -1,
		Main,
		Sub,
		Num
	}

	private enum State
	{
		None = -1,
		Idle,
		Setup,
		WaitSetup,
		OnInAnim,
		Playing,
		WaitTouchButton,
		OnOutAnim,
		End,
		Num
	}

	private sealed class _OnInAnimationEnd_c__Iterator27 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _blinder___0;

		internal GameObject _skipButtonObject___1;

		internal CharaType[] _charaTypeList___2;

		internal int _index___3;

		internal GlowUpCharacter _charaPlate___4;

		internal GlowUpCharaAfterInfo _afterInfo___5;

		internal CharaType _charaType___6;

		internal ServerCharacterState _charaState___7;

		internal ServerPlayCharacterState _playCharaState___8;

		internal List<AbilityType> _abilityList___9;

		internal List<int>.Enumerator __s_469___10;

		internal AbilityType _ability___11;

		internal List<int> _abilityLevelupList___12;

		internal List<int>.Enumerator __s_470___13;

		internal int _cost___14;

		internal int _PC;

		internal object _current;

		internal GlowUpWindow __f__this;

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
				this._current = new WaitForSeconds(0.5f);
				this._PC = 1;
				return true;
			case 1u:
				this._blinder___0 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "anime_blinder");
				if (this._blinder___0 != null)
				{
					this._blinder___0.SetActive(false);
				}
				this._skipButtonObject___1 = GameObjectUtil.FindChildGameObject(this.__f__this.gameObject, "Btn_skip");
				if (this._skipButtonObject___1 != null)
				{
					this._skipButtonObject___1.SetActive(true);
				}
				this._charaTypeList___2 = new CharaType[]
				{
					SaveDataManager.Instance.PlayerData.MainChara,
					SaveDataManager.Instance.PlayerData.SubChara
				};
				this._index___3 = 0;
				while (this._index___3 < 2)
				{
					this._charaPlate___4 = this.__f__this.m_charaPlate[this._index___3];
					if (!(this._charaPlate___4 == null))
					{
						this._afterInfo___5 = new GlowUpCharaAfterInfo();
						this._charaType___6 = this._charaTypeList___2[this._index___3];
						if (ServerInterface.LoggedInServerInterface != null)
						{
							this._charaState___7 = ServerInterface.PlayerState.CharacterState(this._charaType___6);
							if (this._charaState___7 != null)
							{
								this._afterInfo___5.level = this.__f__this.CalcCharacterTotalLevel(this._charaState___7.AbilityLevel);
								this._afterInfo___5.levelUpCost = this._charaState___7.Cost;
								this._afterInfo___5.exp = this._charaState___7.Exp;
							}
							this._playCharaState___8 = ServerInterface.PlayerState.PlayCharacterState(this._charaType___6);
							if (this._playCharaState___8 != null)
							{
								this._abilityList___9 = new List<AbilityType>();
								this.__s_469___10 = this._playCharaState___8.abilityLevelUp.GetEnumerator();
								try
								{
									while (this.__s_469___10.MoveNext())
									{
										this._ability___11 = (AbilityType)this.__s_469___10.Current;
										this._abilityList___9.Add(this._ability___11);
									}
								}
								finally
								{
									((IDisposable)this.__s_469___10).Dispose();
								}
								this._afterInfo___5.abilityList = this._abilityList___9;
								this._abilityLevelupList___12 = new List<int>();
								this.__s_470___13 = this._playCharaState___8.abilityLevelUpExp.GetEnumerator();
								try
								{
									while (this.__s_470___13.MoveNext())
									{
										this._cost___14 = this.__s_470___13.Current;
										this._abilityLevelupList___12.Add(this._cost___14);
									}
								}
								finally
								{
									((IDisposable)this.__s_470___13).Dispose();
								}
								this._afterInfo___5.abilityListExp = this._abilityLevelupList___12;
							}
						}
						this._charaPlate___4.PlayStart(this._afterInfo___5);
					}
					this._index___3++;
				}
				SoundManager.SePlay("sys_gauge", "SE");
				this.__f__this.m_state = GlowUpWindow.State.Playing;
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

	private static readonly string[] CharaPlateName = new string[]
	{
		"player_main",
		"player_sub"
	};

	private GlowUpWindow.State m_state = GlowUpWindow.State.Setup;

	private GlowUpCharacter[] m_charaPlate = new GlowUpCharacter[2];

	public bool IsPlayEnd
	{
		get
		{
			return this.m_state == GlowUpWindow.State.End;
		}
	}

	public void PlayStart(GlowUpWindow.ExpType expType)
	{
		base.gameObject.SetActive(true);
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, false);
		}
		CharaType[] array = new CharaType[]
		{
			SaveDataManager.Instance.PlayerData.MainChara,
			SaveDataManager.Instance.PlayerData.SubChara
		};
		bool flag = true;
		for (int i = 0; i < 2; i++)
		{
			string name = GlowUpWindow.CharaPlateName[i];
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, name);
			if (!(gameObject == null))
			{
				if (this.m_charaPlate[i] == null)
				{
					this.m_charaPlate[i] = gameObject.AddComponent<GlowUpCharacter>();
				}
				GlowUpCharaBaseInfo glowUpCharaBaseInfo = new GlowUpCharaBaseInfo();
				CharaType charaType = array[i];
				if (ServerInterface.LoggedInServerInterface != null)
				{
					ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(charaType);
					if (serverCharacterState != null)
					{
						glowUpCharaBaseInfo.charaType = charaType;
						glowUpCharaBaseInfo.level = this.CalcCharacterTotalLevel(serverCharacterState.OldAbiltyLevel);
						glowUpCharaBaseInfo.levelUpCost = serverCharacterState.OldCost;
						glowUpCharaBaseInfo.currentExp = serverCharacterState.OldExp;
						bool flag2 = false;
						ServerPlayCharacterState serverPlayCharacterState = ServerInterface.PlayerState.PlayCharacterState(charaType);
						if (serverPlayCharacterState != null)
						{
							flag2 = true;
						}
						glowUpCharaBaseInfo.IsActive = flag2;
						if (flag2 && serverCharacterState.OldStatus != ServerCharacterState.CharacterStatus.MaxLevel)
						{
							flag = false;
						}
					}
				}
				this.m_charaPlate[i].Setup(glowUpCharaBaseInfo);
			}
		}
		string text = string.Empty;
		string text2 = string.Empty;
		string str = string.Empty;
		if (EventManager.Instance.IsRaidBossStage())
		{
			bool flag3 = false;
			EventManager instance2 = EventManager.Instance;
			if (instance2 != null)
			{
				ServerEventRaidBossBonus raidBossBonus = instance2.RaidBossBonus;
				if (raidBossBonus != null && raidBossBonus.BeatBonus > 0)
				{
					flag3 = true;
				}
			}
			if (!flag3)
			{
				str = "ui_Lbl_player_exp_failed";
			}
			else if (flag)
			{
				str = "ui_Lbl_player_exp_level_max";
			}
			else
			{
				str = "ui_Lbl_player_exp_success_raid";
			}
		}
		else if (expType == GlowUpWindow.ExpType.BOSS_FAILED)
		{
			str = "ui_Lbl_player_exp_failed";
		}
		else if (flag)
		{
			str = "ui_Lbl_player_exp_level_max";
		}
		else if (expType == GlowUpWindow.ExpType.BOSS_SUCCESS)
		{
			str = "ui_Lbl_player_exp_success";
		}
		else
		{
			str = "ui_Lbl_player_exp";
		}
		text = str + "_caption";
		text2 = str + "_text";
		global::Debug.Log("ExpCaption: " + text);
		global::Debug.Log("ExpText: " + text2);
		GameObject labelObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Lbl_caption");
		this.SetupLabel(labelObject, "Result", text);
		GameObject labelObject2 = null;
		GameObject x = GameObjectUtil.FindChildGameObject(base.gameObject, "window_contents");
		if (x != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "body");
			if (gameObject2 != null)
			{
				labelObject2 = GameObjectUtil.FindChildGameObject(gameObject2, "Lbl_body");
			}
		}
		this.SetupLabel(labelObject2, "Result", text2);
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_ok");
		if (uIImageButton != null)
		{
			uIImageButton.isEnabled = false;
		}
		this.m_state = GlowUpWindow.State.WaitSetup;
	}

	private void Start()
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
		for (int i = 0; i < 2; i++)
		{
			this.m_charaPlate[i] = null;
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_skip");
		if (gameObject != null)
		{
			UIButtonMessage uIButtonMessage = gameObject.GetComponent<UIButtonMessage>();
			if (uIButtonMessage == null)
			{
				uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
			}
			if (uIButtonMessage != null)
			{
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "SkipButtonClickedCallback";
			}
			gameObject.SetActive(false);
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_ok");
		if (gameObject2 != null)
		{
			UIButtonMessage uIButtonMessage2 = gameObject2.GetComponent<UIButtonMessage>();
			if (uIButtonMessage2 == null)
			{
				uIButtonMessage2 = gameObject2.AddComponent<UIButtonMessage>();
			}
			if (uIButtonMessage2 != null)
			{
				uIButtonMessage2.target = base.gameObject;
				uIButtonMessage2.functionName = "ButtonClickedCallback";
			}
		}
		base.gameObject.SetActive(false);
		this.m_state = GlowUpWindow.State.Idle;
	}

	private void OnDestroy()
	{
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
	}

	private void Update()
	{
		switch (this.m_state)
		{
		case GlowUpWindow.State.WaitSetup:
		{
			bool flag = true;
			for (int i = 0; i < 2; i++)
			{
				GlowUpCharacter glowUpCharacter = this.m_charaPlate[i];
				if (!(glowUpCharacter == null))
				{
					if (!glowUpCharacter.IsEndSetup)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				Animation component = base.gameObject.GetComponent<Animation>();
				if (component != null)
				{
					ActiveAnimation activeAnimation = ActiveAnimation.Play(component, "ui_cmn_window_Anim2", Direction.Forward);
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.InAnimationEndCallback), true);
				}
				this.m_state = GlowUpWindow.State.OnInAnim;
			}
			break;
		}
		case GlowUpWindow.State.Playing:
		{
			bool flag2 = true;
			GlowUpCharacter[] charaPlate = this.m_charaPlate;
			for (int j = 0; j < charaPlate.Length; j++)
			{
				GlowUpCharacter glowUpCharacter2 = charaPlate[j];
				if (!(glowUpCharacter2 == null))
				{
					if (!glowUpCharacter2.IsEnd)
					{
						flag2 = false;
						break;
					}
				}
			}
			if (flag2)
			{
				SoundManager.SeStop("sys_gauge", "SE");
				GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_skip");
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
				UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_ok");
				if (uIImageButton != null)
				{
					uIImageButton.isEnabled = true;
				}
				this.m_state = GlowUpWindow.State.WaitTouchButton;
			}
			break;
		}
		}
	}

	private void InAnimationEndCallback()
	{
		base.StartCoroutine(this.OnInAnimationEnd());
	}

	private IEnumerator OnInAnimationEnd()
	{
		GlowUpWindow._OnInAnimationEnd_c__Iterator27 _OnInAnimationEnd_c__Iterator = new GlowUpWindow._OnInAnimationEnd_c__Iterator27();
		_OnInAnimationEnd_c__Iterator.__f__this = this;
		return _OnInAnimationEnd_c__Iterator;
	}

	private void OutAnimationEndCallback()
	{
		base.gameObject.SetActive(false);
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, true);
		}
		this.m_state = GlowUpWindow.State.End;
	}

	private void SkipButtonClickedCallback()
	{
		GlowUpWindow.State state = this.m_state;
		if (state == GlowUpWindow.State.Playing)
		{
			GlowUpCharacter[] charaPlate = this.m_charaPlate;
			for (int i = 0; i < charaPlate.Length; i++)
			{
				GlowUpCharacter glowUpCharacter = charaPlate[i];
				if (!(glowUpCharacter == null))
				{
					glowUpCharacter.PlaySkip();
				}
			}
		}
	}

	private void ButtonClickedCallback()
	{
		GlowUpWindow.State state = this.m_state;
		if (state == GlowUpWindow.State.WaitTouchButton)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			Animation component = base.gameObject.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(component, Direction.Reverse);
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OutAnimationEndCallback), true);
			}
			UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_ok");
			if (uIImageButton != null)
			{
				uIImageButton.isEnabled = false;
			}
			this.m_state = GlowUpWindow.State.OnOutAnim;
		}
	}

	private int CalcCharacterTotalLevel(List<int> abilityLevelList)
	{
		int num = 0;
		if (abilityLevelList == null)
		{
			return num;
		}
		foreach (int current in abilityLevelList)
		{
			num += current;
		}
		return num;
	}

	private void SetupLabel(GameObject labelObject, string groupName, string cellName)
	{
		if (labelObject == null)
		{
			return;
		}
		labelObject.SetActive(true);
		UILabel component = labelObject.GetComponent<UILabel>();
		if (component != null)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, groupName, cellName).text;
			component.text = text;
		}
		UILocalizeText component2 = labelObject.GetComponent<UILocalizeText>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
	}

	private void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		this.ButtonClickedCallback();
	}
}
