using Message;
using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDeckTab : MonoBehaviour
{
	private enum State
	{
		IDLE,
		DECK_CHANGING,
		NUM
	}

	private int m_currentDeckStock;

	private GameObject m_deckColliderObject;

	private MainMenuDeckTab.State m_state;

	public void UpdateView()
	{
		this.m_currentDeckStock = DeckUtil.GetDeckCurrentStockIndex();
		this.SetupTabView();
	}

	private void ChaoSetLoad(int stock)
	{
		if (this.m_state == MainMenuDeckTab.State.DECK_CHANGING)
		{
			return;
		}
		this.m_currentDeckStock = stock;
		DeckUtil.SetDeckCurrentStockIndex(this.m_currentDeckStock);
		this.m_state = MainMenuDeckTab.State.DECK_CHANGING;
		CharaType charaType = CharaType.UNKNOWN;
		CharaType charaType2 = CharaType.UNKNOWN;
		int num = -1;
		int num2 = -1;
		DeckUtil.GetDeckData(this.m_currentDeckStock, ref charaType, ref charaType2, ref num, ref num2);
		CharaType charaType3 = CharaType.UNKNOWN;
		CharaType charaType4 = CharaType.UNKNOWN;
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			charaType3 = instance.PlayerData.MainChara;
			charaType4 = instance.PlayerData.SubChara;
		}
		bool flag = false;
		if (charaType != CharaType.UNKNOWN && charaType != charaType3)
		{
			flag = true;
		}
		else if (charaType2 != charaType4)
		{
			flag = true;
		}
		if (flag)
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				int mainCharaId = -1;
				int subCharaId = -1;
				ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(charaType);
				ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(charaType2);
				if (serverCharacterState != null)
				{
					mainCharaId = serverCharacterState.Id;
				}
				if (serverCharacterState2 != null)
				{
					subCharaId = serverCharacterState2.Id;
				}
				loggedInServerInterface.RequestServerChangeCharacter(mainCharaId, subCharaId, base.gameObject);
				DeckUtil.UpdateCharacters(charaType, charaType2);
			}
			else
			{
				DeckUtil.UpdateCharacters(charaType, charaType2);
				this.ServerChangeCharacter_Succeeded(null);
			}
		}
		else
		{
			DeckUtil.UpdateCharacters(charaType, charaType2);
			this.ServerChangeCharacter_Succeeded(null);
		}
	}

	private void SetupTabView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_5_MC");
		if (gameObject == null)
		{
			return;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "2_Character");
		if (gameObject2 == null)
		{
			return;
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject2, "Deck_tab");
		if (gameObject3 == null)
		{
			return;
		}
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		list.Add("tab_1");
		list.Add("tab_2");
		list.Add("tab_3");
		list.Add("tab_4");
		list.Add("tab_5");
		list2.Add("OnClickTab1");
		list2.Add("OnClickTab2");
		list2.Add("OnClickTab3");
		list2.Add("OnClickTab4");
		list2.Add("OnClickTab5");
		this.m_deckColliderObject = GeneralUtil.SetToggleObject(base.gameObject, gameObject3, list2, list, this.m_currentDeckStock, true);
	}

	private void OnClickTab1()
	{
		if (this.m_currentDeckStock != 0)
		{
			this.ChaoSetLoad(0);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickTab2()
	{
		if (this.m_currentDeckStock != 1)
		{
			this.ChaoSetLoad(1);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickTab3()
	{
		if (this.m_currentDeckStock != 2)
		{
			this.ChaoSetLoad(2);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickTab4()
	{
		if (this.m_currentDeckStock != 3)
		{
			this.ChaoSetLoad(3);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickTab5()
	{
		if (this.m_currentDeckStock != 4)
		{
			this.ChaoSetLoad(4);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void ServerChangeCharacter_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		int num = -1;
		int num2 = -1;
		CharaType charaType = CharaType.SONIC;
		CharaType charaType2 = CharaType.TAILS;
		DeckUtil.GetDeckData(this.m_currentDeckStock, ref charaType, ref charaType2, ref num, ref num2);
		int num3 = -1;
		int num4 = -1;
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			num3 = instance.PlayerData.MainChaoID;
			num4 = instance.PlayerData.SubChaoID;
		}
		bool flag = false;
		if (num != num3)
		{
			flag = true;
		}
		else if (num2 != num4)
		{
			flag = true;
		}
		if (flag)
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				loggedInServerInterface.RequestServerEquipChao((int)ServerItem.CreateFromChaoId(num).id, (int)ServerItem.CreateFromChaoId(num2).id, base.gameObject);
				DeckUtil.UpdateChaos(num, num2);
			}
			else
			{
				this.ServerEquipChao_Succeeded(null);
				DeckUtil.UpdateChaos(num, num2);
			}
		}
		else
		{
			DeckUtil.UpdateChaos(num, num2);
			this.ServerEquipChao_Succeeded(null);
		}
	}

	private void ServerEquipChao_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		this.m_state = MainMenuDeckTab.State.IDLE;
	}

	private bool CheckExsitDeck()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				return systemdata.CheckExsitDeck();
			}
		}
		return false;
	}

	private void Start()
	{
		if (!this.CheckExsitDeck())
		{
			DeckUtil.SetFirstDeckData();
		}
		this.m_currentDeckStock = DeckUtil.GetDeckCurrentStockIndex();
		this.SetupTabView();
	}

	private void Update()
	{
		MainMenuDeckTab.State state = this.m_state;
		if (state != MainMenuDeckTab.State.IDLE)
		{
			if (state != MainMenuDeckTab.State.DECK_CHANGING)
			{
			}
		}
	}
}
