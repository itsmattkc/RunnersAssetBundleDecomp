using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class EventUtility : MonoBehaviour
{
	public static Texture GetBGTexture()
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "EventBGTexture");
			if (gameObject != null)
			{
				AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
				if (component != null)
				{
					return component.m_tex;
				}
			}
		}
		return null;
	}

	public static Texture GetLoadingFaceTexture()
	{
		GameObject gameObject = GameObject.Find("EventResourceLoadingTexture");
		if (gameObject != null)
		{
			AssetBundleTexture componentInChildren = gameObject.transform.GetComponentInChildren<AssetBundleTexture>();
			if (componentInChildren != null)
			{
				return componentInChildren.m_tex;
			}
		}
		return null;
	}

	private static int GetProgressEncountered(EventProductionData data, int encountered, int beatedEncountered)
	{
		int result = -1;
		if (data != null && data.startCollectCount != null)
		{
			for (int i = 0; i < data.startCollectCount.Length; i++)
			{
				if (data.startCollectCount[i] > encountered || data.startCollectCount[i] - 1 > beatedEncountered)
				{
					break;
				}
				result = data.startCollectCount[i];
			}
		}
		return result;
	}

	public static WindowEventData GetLoadingEventData()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null && EventManager.Instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			EventProductionData eventProductionData = null;
			if (EventManager.Instance.Type == EventManager.EventType.SPECIAL_STAGE)
			{
				eventProductionData = EventManager.Instance.GetPuductionData();
			}
			else if (EventManager.Instance.Type == EventManager.EventType.RAID_BOSS)
			{
				EventRaidProductionData raidProductionData = EventManager.Instance.GetRaidProductionData();
				ServerEventUserRaidBossState raidBossState = EventManager.Instance.RaidBossState;
				if (raidProductionData != null && raidBossState != null)
				{
					int numRaidBossEncountered = raidBossState.NumRaidBossEncountered;
					int numBeatedEncounter = raidBossState.NumBeatedEncounter;
					EventRaidProductionData raidProductionData2 = EventManager.Instance.GetRaidProductionData();
					if (raidProductionData2 != null)
					{
						int progressEncountered = EventUtility.GetProgressEncountered(raidProductionData2.mileagePage, numRaidBossEncountered, numBeatedEncounter);
						int progressEncountered2 = EventUtility.GetProgressEncountered(raidProductionData2.eventTop, numRaidBossEncountered, numBeatedEncounter);
						if (progressEncountered > progressEncountered2)
						{
							eventProductionData = raidProductionData2.mileagePage;
						}
						else
						{
							eventProductionData = raidProductionData2.eventTop;
						}
						if (systemdata != null && systemdata.pictureShowRaidBossFirstBattle == 0)
						{
							int progressEncountered3 = EventUtility.GetProgressEncountered(raidProductionData2.firstBattle, numRaidBossEncountered, numBeatedEncounter);
							if (progressEncountered <= progressEncountered3 && progressEncountered2 <= progressEncountered3)
							{
								eventProductionData = raidProductionData2.firstBattle;
							}
						}
					}
				}
			}
			if (systemdata != null && eventProductionData != null)
			{
				uint num = (uint)systemdata.pictureShowProgress;
				if (systemdata.pictureShowEventId != EventManager.Instance.Id)
				{
					num = 0u;
				}
				if ((ulong)num < (ulong)((long)eventProductionData.loadingWindowId.Length))
				{
					int texWindowId = eventProductionData.loadingWindowId[(int)((UIntPtr)num)];
					return EventManager.Instance.GetWindowEvenData(texWindowId);
				}
			}
		}
		return null;
	}

	public static int GetLoadingTextureResourceIndex()
	{
		int result = -1;
		WindowEventData loadingEventData = EventUtility.GetLoadingEventData();
		if (loadingEventData != null && loadingEventData.body != null && loadingEventData.body.Length > 0 && loadingEventData.body[0] != null && loadingEventData.body[0].product != null && loadingEventData.body[0].product.Length > 0 && loadingEventData.body[0].product[0] != null)
		{
			result = loadingEventData.body[0].product[0].face_id;
		}
		return result;
	}

	public static bool IsPictureEvent()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null && EventManager.Instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				if (systemdata.pictureShowEventId != EventManager.Instance.Id)
				{
					return true;
				}
				int pictureShowProgress = systemdata.pictureShowProgress;
				EventProductionData puductionData = EventManager.Instance.GetPuductionData();
				if (puductionData != null)
				{
					int num = 0;
					int[] startCollectCount = puductionData.startCollectCount;
					for (int i = 0; i < startCollectCount.Length; i++)
					{
						int num2 = startCollectCount[i];
						if (EventManager.Instance.CollectCount >= (long)num2 && num > pictureShowProgress)
						{
							return true;
						}
						num++;
					}
				}
			}
		}
		return false;
	}

	public static void SetDontDestroyLoadingFaceTexture()
	{
		string eventResourceLoadingTextureName = EventUtility.GetEventResourceLoadingTextureName();
		if (!string.IsNullOrEmpty(eventResourceLoadingTextureName))
		{
			GameObject gameObject = GameObject.Find(eventResourceLoadingTextureName);
			GameObject gameObject2 = new GameObject("EventResourceLoadingTexture");
			if (gameObject2 != null)
			{
				gameObject.transform.parent = gameObject2.transform;
				UnityEngine.Object.DontDestroyOnLoad(gameObject2);
			}
		}
	}

	public static void DestroyLoadingFaceTexture()
	{
		GameObject gameObject = GameObject.Find("EventResourceLoadingTexture");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public static string GetEventResourceLoadingTextureName()
	{
		int loadingTextureResourceIndex = EventUtility.GetLoadingTextureResourceIndex();
		if (loadingTextureResourceIndex >= 0)
		{
			return "ui_tex_event_" + loadingTextureResourceIndex.ToString("D4");
		}
		return null;
	}

	public static bool IsExistSpecificEventText(int eventId)
	{
		return EventManager.GetType(eventId) == EventManager.EventType.SPECIAL_STAGE || EventManager.GetType(eventId) == EventManager.EventType.RAID_BOSS;
	}

	public static string GetRaidBossName(int rarity)
	{
		TextManager.TextType type = TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT;
		string text = "bossname_";
		switch (rarity)
		{
		case 0:
			text = text + "n_" + EventManager.GetSpecificId().ToString();
			break;
		case 1:
			text = text + "r_" + EventManager.GetSpecificId().ToString();
			break;
		case 2:
			text = text + "p_" + EventManager.GetSpecificId().ToString();
			break;
		}
		return TextUtility.GetText(type, "EventBossName", text);
	}

	public static void UpdateCollectObjectCount()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsResultEvent())
		{
			StageScoreManager instance = StageScoreManager.Instance;
			if (instance == null)
			{
				return;
			}
			EventManager.EventType type = EventManager.Instance.Type;
			if (type == EventManager.EventType.SPECIAL_STAGE)
			{
				if (EventManager.Instance.IsSpecialStage())
				{
					EventManager.Instance.SetEventInfo();
				}
			}
			else if (type == EventManager.EventType.RAID_BOSS)
			{
				if (EventManager.Instance.IsRaidBossStage())
				{
					EventManager.Instance.CollectCount += instance.FinalCountData.raid_boss_ring;
					EventManager.Instance.SetEventInfo();
				}
			}
			else if (type == EventManager.EventType.COLLECT_OBJECT && EventManager.Instance.IsCollectEvent())
			{
				EventManager.Instance.SetEventInfo();
			}
		}
	}

	public static void SetEventIncentiveListToEventManager(List<ServerItemState> eventIncentiveList)
	{
	}

	public static void SetEventStateToEventManager(ServerEventState eventState)
	{
		if (EventManager.Instance != null)
		{
			EventManager.Instance.SetState(eventState);
		}
	}

	public static void SetRaidbossEntry(ServerEventRaidBossState raidData)
	{
		if (EventManager.Instance == null)
		{
			return;
		}
		if (EventManager.Instance.Type != EventManager.EventType.RAID_BOSS)
		{
			return;
		}
		if (raidData == null)
		{
			return;
		}
		if (raidData != null && !raidData.Encounter)
		{
			return;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null && !systemdata.raidEntryFlag && systemdata.currentRaidDrawIndex != raidData.Id)
			{
				systemdata.raidEntryFlag = true;
				instance.SaveSystemData();
			}
		}
	}

	public static ServerEventRaidBossState RaidbossDiscoverEntry()
	{
		if (EventManager.Instance == null)
		{
			return null;
		}
		if (EventManager.Instance.Type != EventManager.EventType.RAID_BOSS)
		{
			return null;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		ServerEventRaidBossState result = null;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null && EventManager.Instance.RaidBossInfo != null)
			{
				List<ServerEventRaidBossState> userRaidBossList = EventManager.Instance.UserRaidBossList;
				if (userRaidBossList != null)
				{
					bool flag = false;
					foreach (ServerEventRaidBossState current in userRaidBossList)
					{
						if (current.Encounter && systemdata.currentRaidDrawIndex != current.Id)
						{
							systemdata.currentRaidDrawIndex = current.Id;
							systemdata.raidEntryFlag = false;
							result = current;
							flag = true;
						}
					}
					if (flag)
					{
						instance.SaveSystemData();
					}
				}
			}
		}
		return result;
	}

	public static bool CheckRaidbossEntry()
	{
		if (EventManager.Instance == null)
		{
			return false;
		}
		if (EventManager.Instance.Type != EventManager.EventType.RAID_BOSS)
		{
			return false;
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null && EventManager.Instance.UserRaidBossList != null)
			{
				if (systemdata.raidEntryFlag)
				{
					return true;
				}
				List<ServerEventRaidBossState> userRaidBossList = EventManager.Instance.UserRaidBossList;
				if (userRaidBossList != null)
				{
					foreach (ServerEventRaidBossState current in userRaidBossList)
					{
						if (current.Encounter && systemdata.currentRaidDrawIndex != current.Id)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public static void SetRaidBossFirstBattle()
	{
		if (RaidBossInfo.currentRaidData != null && RaidBossInfo.currentRaidData.IsDiscoverer() && SystemSaveManager.Instance != null)
		{
			SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
			if (systemdata != null && systemdata.pictureShowRaidBossFirstBattle == -1)
			{
				systemdata.pictureShowRaidBossFirstBattle = 1;
			}
			SystemSaveManager.Instance.SaveSystemData();
		}
	}

	public static bool IsEnableRouletteUI()
	{
		bool result = false;
		if (EventManager.Instance != null && EventManager.Instance.IsInEvent())
		{
			if (EventManager.Instance.Type == EventManager.EventType.ADVERT)
			{
				if (EventManager.Instance.AdvertType == EventManager.AdvertEventType.ROULETTE)
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}
		}
		return result;
	}
}
