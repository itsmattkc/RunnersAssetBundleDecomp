using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckUtil
{
	public class DeckSet
	{
		public CharaType charaMain;

		public CharaType charaSub = CharaType.UNKNOWN;

		public int chaoMain = -1;

		public int chaoSub = -1;

		public bool isCurrentSelect;
	}

	private static int s_chaoSetCurrentStockIndex = -1;

	public static void SetFirstDeckData()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				SaveDataManager instance2 = SaveDataManager.Instance;
				if (instance2 != null)
				{
					systemdata.SaveDeckData(0, instance2.PlayerData.MainChara, instance2.PlayerData.SubChara, instance2.PlayerData.MainChaoID, instance2.PlayerData.SubChaoID);
				}
			}
		}
	}

	public static int GetDeckCurrentStockIndex()
	{
		int deckCurrentStockIndex = DeckUtil.s_chaoSetCurrentStockIndex;
		if (deckCurrentStockIndex < 0)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					SaveDataManager instance2 = SaveDataManager.Instance;
					if (instance2 != null)
					{
						deckCurrentStockIndex = systemdata.GetDeckCurrentStockIndex();
						DeckUtil.s_chaoSetCurrentStockIndex = deckCurrentStockIndex;
					}
				}
			}
		}
		return deckCurrentStockIndex;
	}

	public static void SetDeckCurrentStockIndex(int index)
	{
		if (index < 0 || index >= 6)
		{
			return;
		}
		DeckUtil.s_chaoSetCurrentStockIndex = index;
	}

	public static void CharaSetSaveAuto(int currentMainId, int currentSubId)
	{
		int deckCurrentStockIndex = DeckUtil.GetDeckCurrentStockIndex();
		DeckUtil.CharaSetSave(deckCurrentStockIndex, currentMainId, currentSubId);
	}

	private static bool CharaSetSave(int stock, int currentMainId, int currentSubId)
	{
		if (stock < 0 || stock >= 6)
		{
			return false;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				SaveDataManager instance2 = SaveDataManager.Instance;
				PlayerData arg_4A_0 = instance2.PlayerData;
				ServerItem serverItem = new ServerItem((ServerItem.Id)currentMainId);
				arg_4A_0.MainChara = serverItem.charaType;
				PlayerData arg_64_0 = instance2.PlayerData;
				ServerItem serverItem2 = new ServerItem((ServerItem.Id)currentSubId);
				arg_64_0.SubChara = serverItem2.charaType;
				instance2.SavePlayerData();
				systemdata.SaveDeckDataChara(stock);
				return true;
			}
		}
		return false;
	}

	public static void ChaoSetSaveAuto(int currentMainId, int currentSubId)
	{
		int deckCurrentStockIndex = DeckUtil.GetDeckCurrentStockIndex();
		DeckUtil.ChaoSetSave(deckCurrentStockIndex, currentMainId - 400000, currentSubId - 400000);
	}

	private static bool ChaoSetSave(int stock, int currentMainId, int currentSubId)
	{
		if (stock < 0 || stock >= 6)
		{
			return false;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				SaveDataManager instance2 = SaveDataManager.Instance;
				instance2.PlayerData.MainChaoID = currentMainId;
				instance2.PlayerData.SubChaoID = currentSubId;
				instance2.SavePlayerData();
				systemdata.SaveDeckDataChao(stock);
				return true;
			}
		}
		return false;
	}

	public static bool DeckReset(int stock)
	{
		if (stock < 0 || stock >= 6)
		{
			return false;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				systemdata.RestDeckData(stock);
			}
		}
		return true;
	}

	public static bool DeckSetSave(int stock, CharaType currentMainCharaType, CharaType currentSubCharaType, int currentMainId, int currentSubId)
	{
		if (stock < 0 || stock >= 6)
		{
			return false;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				systemdata.SaveDeckData(stock, currentMainCharaType, currentSubCharaType, currentMainId, currentSubId);
			}
		}
		return true;
	}

	public static bool DeckSetLoad(int stock, GameObject callbackObject)
	{
		if (stock < 0 || stock >= 6)
		{
			return false;
		}
		SaveDataManager instance = SaveDataManager.Instance;
		CharaType mainChara = instance.PlayerData.MainChara;
		CharaType subChara = instance.PlayerData.SubChara;
		int mainChaoID = instance.PlayerData.MainChaoID;
		int subChaoID = instance.PlayerData.SubChaoID;
		bool flag = DeckUtil.DeckSetLoad(stock, ref mainChara, ref subChara, ref mainChaoID, ref subChaoID, callbackObject);
		if (flag)
		{
			instance.PlayerData.MainChara = mainChara;
			instance.PlayerData.SubChara = subChara;
			instance.PlayerData.MainChaoID = mainChaoID;
			instance.PlayerData.SubChaoID = subChaoID;
		}
		return flag;
	}

	public static bool DeckSetLoad(int stock, ref CharaType currentMainCharaType, ref CharaType currentSubCharaType, ref int currentMainId, ref int currentSubId, GameObject callbackObject = null)
	{
		if (stock < 0 || stock >= 6)
		{
			return false;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				SaveDataManager instance2 = SaveDataManager.Instance;
				CharaType charaType = CharaType.SONIC;
				CharaType charaType2 = CharaType.UNKNOWN;
				int num = -1;
				int num2 = -1;
				systemdata.GetDeckData(stock, out charaType, out charaType2, out num, out num2);
				CharaType mainChara = instance2.PlayerData.MainChara;
				CharaType subChara = instance2.PlayerData.SubChara;
				int mainChaoID = instance2.PlayerData.MainChaoID;
				int subChaoID = instance2.PlayerData.SubChaoID;
				currentMainCharaType = charaType;
				currentSubCharaType = charaType2;
				currentMainId = num;
				currentSubId = num2;
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (num != mainChaoID || num2 != subChaoID)
				{
					if (loggedInServerInterface != null && callbackObject != null)
					{
						loggedInServerInterface.RequestServerEquipChao((int)ServerItem.CreateFromChaoId(num).id, (int)ServerItem.CreateFromChaoId(num2).id, callbackObject);
					}
				}
				else if (callbackObject != null)
				{
					callbackObject.SendMessage("ServerEquipChao_Dummy", SendMessageOptions.DontRequireReceiver);
				}
				if (charaType != mainChara || charaType2 != subChara)
				{
					if (loggedInServerInterface != null && callbackObject != null)
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
						loggedInServerInterface.RequestServerChangeCharacter(mainCharaId, subCharaId, callbackObject);
					}
				}
				else if (callbackObject != null)
				{
					callbackObject.SendMessage("RequestServerChangeCharacter_Dummy", SendMessageOptions.DontRequireReceiver);
				}
				return true;
			}
		}
		return false;
	}

	public static void GetDeckData(int stock, ref CharaType currentMainCharaType, ref CharaType currentSubCharaType, ref int currentMainId, ref int currentSubId)
	{
		if (stock < 0 || stock >= 6)
		{
			return;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance == null)
		{
			return;
		}
		SystemData systemdata = instance.GetSystemdata();
		if (systemdata == null)
		{
			return;
		}
		CharaType charaType = CharaType.SONIC;
		CharaType charaType2 = CharaType.UNKNOWN;
		int num = -1;
		int num2 = -1;
		systemdata.GetDeckData(stock, out charaType, out charaType2, out num, out num2);
		currentMainCharaType = charaType;
		currentSubCharaType = charaType2;
		currentMainId = num;
		currentSubId = num2;
	}

	public static void GetDeckData(int stock, ref CharaType currentMainCharaType, ref CharaType currentSubCharaType)
	{
		if (stock < 0 || stock >= 6)
		{
			return;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance == null)
		{
			return;
		}
		SystemData systemdata = instance.GetSystemdata();
		if (systemdata == null)
		{
			return;
		}
		CharaType charaType = CharaType.SONIC;
		CharaType charaType2 = CharaType.UNKNOWN;
		int num = -1;
		int num2 = -1;
		systemdata.GetDeckData(stock, out charaType, out charaType2, out num, out num2);
		currentMainCharaType = charaType;
		currentSubCharaType = charaType2;
	}

	public static void UpdateCharacters(CharaType mainChara, CharaType subChara)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.PlayerData.MainChara = mainChara;
		instance.PlayerData.SubChara = subChara;
	}

	public static void UpdateChaos(int mainChaoId, int subChaoId)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.PlayerData.MainChaoID = mainChaoId;
		instance.PlayerData.SubChaoID = subChaoId;
	}

	public static bool IsChaoSetSave(int stock)
	{
		bool result = true;
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				result = systemdata.IsSaveDeckData(stock);
			}
		}
		return result;
	}

	public static List<DeckUtil.DeckSet> GetDeckList()
	{
		List<DeckUtil.DeckSet> list = new List<DeckUtil.DeckSet>();
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				for (int i = 0; i < 6; i++)
				{
					DeckUtil.DeckSet deckSet = new DeckUtil.DeckSet();
					systemdata.GetDeckData(i, out deckSet.charaMain, out deckSet.charaSub, out deckSet.chaoMain, out deckSet.chaoSub);
					list.Add(deckSet);
				}
			}
		}
		int deckCurrentStockIndex = DeckUtil.GetDeckCurrentStockIndex();
		if (list.Count > deckCurrentStockIndex)
		{
			list[deckCurrentStockIndex].isCurrentSelect = true;
		}
		return list;
	}
}
