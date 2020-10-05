using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HudDisplay
{
	public enum ObjType
	{
		Chao,
		NewItem,
		Main,
		Player,
		Option,
		Infomation,
		Roulette,
		Shop,
		PresentBox,
		DailyChallenge,
		DailyBattle,
		Ranking,
		Event,
		Mileage,
		NUM,
		NONE
	}

	private List<GameObject>[] m_obj_list = new List<GameObject>[14];

	public HudDisplay()
	{
		for (int i = 0; i < 14; i++)
		{
			this.m_obj_list[i] = new List<GameObject>();
		}
		GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
		if (menuAnimUIObject != null)
		{
			GameObject @object = this.GetObject(menuAnimUIObject, "ChaoSetUIPage");
			this.m_obj_list[0].Add(@object);
			this.m_obj_list[0].Add(this.GetObject(@object, "ChaoSetUI"));
			this.m_obj_list[6].Add(this.GetObject(menuAnimUIObject, "RouletteTopUI"));
			this.m_obj_list[2].Add(this.GetObject(menuAnimUIObject, "MainMenuUI4"));
			this.m_obj_list[3].Add(this.GetObject(menuAnimUIObject, "PlayerSet_3_UI"));
			GameObject object2 = this.GetObject(menuAnimUIObject, "ShopPage");
			this.m_obj_list[7].Add(object2);
			this.m_obj_list[7].Add(this.GetObject(object2, "ShopUI2"));
			this.m_obj_list[4].Add(this.GetObject(menuAnimUIObject, "OptionUI"));
			this.m_obj_list[5].Add(this.GetObject(menuAnimUIObject, "InformationUI"));
			this.m_obj_list[8].Add(this.GetObject(menuAnimUIObject, "PresentBoxUI"));
			this.m_obj_list[9].Add(this.GetObject(menuAnimUIObject, "DailyWindowUI"));
			this.m_obj_list[10].Add(this.GetObject(menuAnimUIObject, "DailyInfoUI"));
			this.m_obj_list[1].Add(this.GetObject(menuAnimUIObject, "ItemSet_3_UI"));
			this.m_obj_list[11].Add(this.GetObject(menuAnimUIObject, "ui_mm_ranking_page"));
			this.m_obj_list[13].Add(this.GetObject(menuAnimUIObject, "ui_mm_mileage2_page"));
		}
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject object3 = this.GetObject(cameraUIObject, "SpecialStageWindowUI");
			if (object3 != null)
			{
				this.m_obj_list[12].Add(object3);
			}
			GameObject object4 = this.GetObject(cameraUIObject, "RaidBossWindowUI");
			if (object4 != null)
			{
				this.m_obj_list[12].Add(object4);
			}
		}
	}

	public void SetAllDisableDisplay()
	{
		for (int i = 0; i < 14; i++)
		{
			if (this.m_obj_list[i] != null)
			{
				this.SetActiveListObj(this.m_obj_list[i], false);
			}
		}
	}

	public void SetDisplayHudObject(HudDisplay.ObjType obj_type)
	{
		for (int i = 0; i < 14; i++)
		{
			if (this.m_obj_list[i] != null)
			{
				bool active_flag = i == (int)obj_type;
				this.SetActiveListObj(this.m_obj_list[i], active_flag);
			}
		}
	}

	private GameObject GetObject(GameObject menu_anim_obj, string obj_name)
	{
		if (menu_anim_obj != null && obj_name != null)
		{
			Transform transform = menu_anim_obj.transform.Find(obj_name);
			if (transform != null)
			{
				return transform.gameObject;
			}
		}
		return null;
	}

	private void SetActiveListObj(List<GameObject> obj_list, bool active_flag)
	{
		if (obj_list != null)
		{
			foreach (GameObject current in obj_list)
			{
				if (current != null)
				{
					current.SetActive(active_flag);
					ButtonEvent.DebugInfoDraw("SetActive " + current.name + " " + active_flag.ToString());
				}
			}
		}
	}

	public static HudDisplay.ObjType CalcObjTypeFromSequenceType(MsgMenuSequence.SequeneceType seqType)
	{
		HudDisplay.ObjType result = HudDisplay.ObjType.Main;
		switch (seqType)
		{
		case MsgMenuSequence.SequeneceType.MAIN:
			result = HudDisplay.ObjType.Main;
			break;
		case MsgMenuSequence.SequeneceType.STAGE:
			result = HudDisplay.ObjType.NONE;
			break;
		case MsgMenuSequence.SequeneceType.PRESENT_BOX:
			result = HudDisplay.ObjType.PresentBox;
			break;
		case MsgMenuSequence.SequeneceType.DAILY_CHALLENGE:
			result = HudDisplay.ObjType.DailyChallenge;
			break;
		case MsgMenuSequence.SequeneceType.DAILY_BATTLE:
			result = HudDisplay.ObjType.DailyBattle;
			break;
		case MsgMenuSequence.SequeneceType.CHARA_MAIN:
			result = HudDisplay.ObjType.Player;
			break;
		case MsgMenuSequence.SequeneceType.CHAO:
			result = HudDisplay.ObjType.Chao;
			break;
		case MsgMenuSequence.SequeneceType.PLAY_ITEM:
		case MsgMenuSequence.SequeneceType.EPISODE_PLAY:
		case MsgMenuSequence.SequeneceType.QUICK:
		case MsgMenuSequence.SequeneceType.PLAY_AT_EPISODE_PAGE:
		case MsgMenuSequence.SequeneceType.MAIN_PLAY_BUTTON:
			result = HudDisplay.ObjType.NewItem;
			break;
		case MsgMenuSequence.SequeneceType.OPTION:
			result = HudDisplay.ObjType.Option;
			break;
		case MsgMenuSequence.SequeneceType.INFOMATION:
			result = HudDisplay.ObjType.Infomation;
			break;
		case MsgMenuSequence.SequeneceType.ROULETTE:
		case MsgMenuSequence.SequeneceType.CHAO_ROULETTE:
		case MsgMenuSequence.SequeneceType.ITEM_ROULETTE:
			result = HudDisplay.ObjType.Roulette;
			break;
		case MsgMenuSequence.SequeneceType.SHOP:
			result = HudDisplay.ObjType.Shop;
			break;
		case MsgMenuSequence.SequeneceType.EPISODE:
			result = HudDisplay.ObjType.Mileage;
			break;
		case MsgMenuSequence.SequeneceType.EPISODE_RANKING:
		case MsgMenuSequence.SequeneceType.QUICK_RANKING:
			result = HudDisplay.ObjType.Ranking;
			break;
		case MsgMenuSequence.SequeneceType.EVENT_TOP:
		case MsgMenuSequence.SequeneceType.EVENT_SPECIAL:
		case MsgMenuSequence.SequeneceType.EVENT_RAID:
		case MsgMenuSequence.SequeneceType.EVENT_COLLECT:
			result = HudDisplay.ObjType.Event;
			break;
		}
		return result;
	}
}
