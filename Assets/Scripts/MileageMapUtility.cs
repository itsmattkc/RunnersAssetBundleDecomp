using App;
using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class MileageMapUtility
{
	public static Texture GetBGTexture()
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		if (instance != null)
		{
			MileageMapData mileageMapData = instance.GetMileageMapData();
			if (mileageMapData != null)
			{
				int bg_id = mileageMapData.map_data.bg_id;
				GameObject gameObject = GameObject.Find(MileageMapBGDataTable.Instance.GetTextureName(bg_id));
				if (gameObject != null)
				{
					AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
					if (component != null)
					{
						return component.m_tex;
					}
				}
			}
		}
		return null;
	}

	public static Texture GetWindowBGTexture()
	{
		return MileageMapUtility.GetBGTexture();
	}

	public static string GetFaceTextureName(int face_id)
	{
		return "ui_tex_event_" + face_id.ToString("0000");
	}

	public static Texture GetFaceTexture(int face_id)
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		if (instance != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(instance.gameObject, "MileageMapFace");
			if (gameObject != null)
			{
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, MileageMapUtility.GetFaceTextureName(face_id));
				if (gameObject2 != null)
				{
					AssetBundleTexture component = gameObject2.GetComponent<AssetBundleTexture>();
					if (component != null)
					{
						return component.m_tex;
					}
				}
			}
		}
		return null;
	}

	public static BossType GetBossType()
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		if (instance != null)
		{
			BossEvent bossEvent = instance.GetMileageMapData().event_data.GetBossEvent();
			if (bossEvent.type_key == "Type1")
			{
				return BossType.MAP1;
			}
			if (bossEvent.type_key == "Type2")
			{
				return BossType.MAP2;
			}
			if (bossEvent.type_key == "Type3")
			{
				return BossType.MAP3;
			}
		}
		return BossType.MAP1;
	}

	public static int GetPointInterval()
	{
		int result = 500;
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		if (instance != null)
		{
			result = instance.GetMileageMapData().map_data.event_interval;
		}
		return result;
	}

	public static bool IsExistBoss()
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		return instance != null && instance.GetMileageMapData().event_data.IsBossEvent();
	}

	public static bool IsBossStage()
	{
		if (MileageMapUtility.IsExistBoss())
		{
			ServerMileageMapState mileageMapState = ServerInterface.MileageMapState;
			if (mileageMapState != null)
			{
				return mileageMapState.m_point == 5;
			}
		}
		return false;
	}

	public static CharacterAttribute[] GetCharacterAttribute(int stageIndex)
	{
		StageSuggestedDataTable instance = StageSuggestedDataTable.Instance;
		if (instance != null)
		{
			return instance.GetStageSuggestedData(stageIndex);
		}
		return null;
	}

	public static void SetMileageStageIndex(int episode, int chapter, PointType type)
	{
		if (MileageMapDataManager.Instance != null)
		{
			MileageMapData mileageMapData = MileageMapDataManager.Instance.GetMileageMapData(episode, chapter);
			if (mileageMapData != null)
			{
				StageData[] stage_data = mileageMapData.map_data.stage_data;
				if (type < (PointType)stage_data.Length)
				{
					MileageMapDataManager.Instance.MileageStageIndex = MileageMapUtility.GetStageIndex(stage_data[(int)type].key);
				}
			}
		}
	}

	public static string GetMileageStageName()
	{
		if (MileageMapDataManager.Instance != null)
		{
			return StageInfo.GetStageNameByIndex(MileageMapDataManager.Instance.MileageStageIndex);
		}
		return StageInfo.GetStageNameByIndex(1);
	}

	private bool IsRandom(string key)
	{
		return key.IndexOf("RANDOM") >= 0;
	}

	public static int GetStageIndex(string key)
	{
		int num = 1;
		if (key.IndexOf("STAGE") >= 0)
		{
			string[] array = key.Split(new char[]
			{
				'_'
			});
			string s = array[0].Replace("STAGE", string.Empty);
			num = int.Parse(s);
		}
		else if (key.IndexOf("RANDOM") >= 0)
		{
			int num2 = 3;
			if (key != "RANDOM")
			{
				string[] array2 = key.Split(new char[]
				{
					'_'
				});
				if (array2.Length > 1)
				{
					num2 = int.Parse(array2[1]);
				}
			}
			UnityEngine.Random.seed = NetUtil.GetCurrentUnixTime();
			num = UnityEngine.Random.Range(1, num2 + 1);
			if (num > num2)
			{
				num = num2;
			}
		}
		return num;
	}

	public static string GetEventStageName(string key)
	{
		return StageInfo.GetStageNameByIndex(MileageMapUtility.GetStageIndex(key));
	}

	public static TenseType GetTenseType(PointType point_type)
	{
		TenseType result = TenseType.NONE;
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		if (instance != null)
		{
			StageData[] stage_data = instance.GetMileageMapData().map_data.stage_data;
			if (point_type < (PointType)stage_data.Length)
			{
				result = MileageMapUtility.GetTenseType(stage_data[(int)point_type].key);
			}
		}
		return result;
	}

	public static TenseType GetTenseType(string key)
	{
		TenseType result = TenseType.NONE;
		if (key.IndexOf("AFTERNOON") > 0)
		{
			result = TenseType.AFTERNOON;
		}
		else if (key.IndexOf("NIGHT") > 0)
		{
			result = TenseType.NIGHT;
		}
		else
		{
			int[] array = new int[]
			{
				0,
				1
			};
			if (array != null)
			{
				App.Random.ShuffleInt(array);
				result = (TenseType)array[0];
			}
		}
		return result;
	}

	public static bool GetChangeTense(PointType point_type)
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		if (instance != null)
		{
			StageData[] stage_data = instance.GetMileageMapData().map_data.stage_data;
			if (point_type < (PointType)stage_data.Length)
			{
				return MileageMapUtility.GetChangeTense(stage_data[(int)point_type].key);
			}
		}
		return false;
	}

	public static bool GetChangeTense(string key)
	{
		return key.IndexOf("AFTERNOON") <= 0 && key.IndexOf("NIGHT") <= 0;
	}

	public static void AddReward(RewardType rewardType, int count)
	{
		bool flag = ServerInterface.LoggedInServerInterface != null;
		if (flag && rewardType < RewardType.RING)
		{
			return;
		}
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		switch (rewardType)
		{
		case RewardType.ITEM_INVINCIBLE:
		case RewardType.ITEM_BARRIER:
		case RewardType.ITEM_MAGNET:
		case RewardType.ITEM_TRAMPOLINE:
		case RewardType.ITEM_COMBO:
		case RewardType.ITEM_LASER:
		case RewardType.ITEM_DRILL:
		case RewardType.ITEM_ASTEROID:
			instance.ItemData.ItemCount[(int)rewardType] = MileageMapUtility.AddToUint(instance.ItemData.ItemCount[(int)rewardType], count);
			break;
		case RewardType.RING:
			if (flag)
			{
				instance.ItemData.RingCountOffset += count;
				if (instance.ItemData.RingCountOffset > 0)
				{
					instance.ItemData.RingCountOffset = 0;
				}
			}
			else
			{
				instance.ItemData.RingCount = MileageMapUtility.AddToUint(instance.ItemData.RingCount, count);
			}
			break;
		case RewardType.RSRING:
			if (flag)
			{
				instance.ItemData.RedRingCountOffset += count;
				if (instance.ItemData.RedRingCountOffset > 0)
				{
					instance.ItemData.RedRingCountOffset = 0;
				}
			}
			else
			{
				instance.ItemData.RedRingCount = MileageMapUtility.AddToUint(instance.ItemData.RedRingCount, count);
			}
			break;
		default:
			if (rewardType == RewardType.ENERGY)
			{
				if (flag)
				{
					instance.PlayerData.ChallengeCountOffset += count;
				}
				else
				{
					instance.PlayerData.ChallengeCount = MileageMapUtility.AddToUint(instance.PlayerData.ChallengeCount, count);
				}
			}
			break;
		}
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private static uint AddToUint(uint dst, int src)
	{
		return (src < 0) ? (dst - (uint)(-(uint)src)) : (dst + (uint)src);
	}

	public static string GetText(string cellName, Dictionary<string, string> dicReplaces = null)
	{
		string text = TextUtility.GetCommonText("MileageMap", cellName);
		if (dicReplaces != null)
		{
			text = TextUtility.Replaces(text, dicReplaces);
		}
		return text;
	}

	public static int GetDisplayOffset_FromResultData(ResultData resultData, ServerItem.Id id)
	{
		if (resultData != null && resultData.m_mileageIncentiveList != null)
		{
			int num = 0;
			foreach (ServerMileageIncentive current in resultData.m_mileageIncentiveList)
			{
				if (id == (ServerItem.Id)current.m_itemId)
				{
					num += current.m_num;
				}
			}
			return num;
		}
		return 0;
	}

	public static int GetMileageClearDisplayOffset_FromResultData(ResultData resultData, ServerItem.Id id)
	{
		if (resultData != null && resultData.m_mileageIncentiveList != null)
		{
			int num = 0;
			foreach (ServerMileageIncentive current in resultData.m_mileageIncentiveList)
			{
				if ((current.m_type == ServerMileageIncentive.Type.CHAPTER || current.m_type == ServerMileageIncentive.Type.EPISODE) && id == (ServerItem.Id)current.m_itemId)
				{
					num += current.m_num;
				}
			}
			return num;
		}
		return 0;
	}

	public static bool IsRankUp_FromResultData(ResultData resultData)
	{
		if (resultData != null && resultData.m_mileageIncentiveList != null)
		{
			foreach (ServerMileageIncentive current in resultData.m_mileageIncentiveList)
			{
				if (current.m_type == ServerMileageIncentive.Type.CHAPTER)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public static void SetDisplayOffset_FromResultData(ResultData resultData)
	{
		if (ServerInterface.LoggedInServerInterface != null)
		{
			int displayOffset_FromResultData = MileageMapUtility.GetDisplayOffset_FromResultData(resultData, ServerItem.Id.RSRING);
			int displayOffset_FromResultData2 = MileageMapUtility.GetDisplayOffset_FromResultData(resultData, ServerItem.Id.RING);
			bool flag = MileageMapUtility.IsRankUp_FromResultData(resultData);
			if (SaveDataManager.Instance != null)
			{
				if (flag)
				{
					PlayerData playerData = SaveDataManager.Instance.PlayerData;
					if (playerData != null)
					{
						playerData.RankOffset = -1;
					}
				}
				ItemData itemData = SaveDataManager.Instance.ItemData;
				if (itemData != null)
				{
					itemData.RedRingCountOffset = -displayOffset_FromResultData;
					itemData.RingCountOffset = -displayOffset_FromResultData2;
				}
			}
		}
	}
}
