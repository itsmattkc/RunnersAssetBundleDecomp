using System;
using System.Collections.Generic;
using Text;

public class PresentBoxUtility
{
	public static string GetItemSpriteName(ServerItem serverItem)
	{
		string result = string.Empty;
		ServerItem.IdType idType = serverItem.idType;
		switch (idType)
		{
		case ServerItem.IdType.ROULLETE_TOKEN:
			result = "ui_cmn_icon_item_210000";
			return result;
		case ServerItem.IdType.EGG_ITEM:
			return result;
		case ServerItem.IdType.PREMIUM_ROULLETE_TICKET:
			result = "ui_cmn_icon_item_230000";
			return result;
		case ServerItem.IdType.ITEM_ROULLETE_TICKET:
			result = "ui_cmn_icon_item_240000";
			return result;
		case (ServerItem.IdType)25:
		case (ServerItem.IdType)26:
		case (ServerItem.IdType)27:
		case (ServerItem.IdType)28:
		case (ServerItem.IdType)29:
			IL_3F:
			switch (idType)
			{
			case ServerItem.IdType.RSRING:
				result = "ui_cmn_icon_item_9";
				return result;
			case ServerItem.IdType.RING:
				result = "ui_cmn_icon_item_8";
				return result;
			case ServerItem.IdType.ENERGY:
				result = "ui_cmn_icon_item_920000";
				return result;
			case ServerItem.IdType.ENERGY_MAX:
			case (ServerItem.IdType)94:
			case (ServerItem.IdType)95:
				IL_64:
				if (idType == ServerItem.IdType.EQUIP_ITEM)
				{
					result = "ui_cmn_icon_item_" + serverItem.idIndex.ToString();
					return result;
				}
				if (idType != ServerItem.IdType.CHAO)
				{
					return result;
				}
				result = "ui_tex_chao_" + serverItem.idIndex.ToString("D4");
				return result;
			case ServerItem.IdType.RAIDRING:
				result = "ui_cmn_icon_item_960000";
				return result;
			}
			goto IL_64;
		case ServerItem.IdType.CHARA:
			result = "ui_tex_player_" + serverItem.idIndex.ToString("D2") + "_" + CharaName.PrefixName[(int)serverItem.charaType];
			return result;
		}
		goto IL_3F;
	}

	public static string GetItemName(ServerItem serverItem)
	{
		return serverItem.serverItemName;
	}

	public static string GetItemInfo(PresentBoxUI.PresentInfo info)
	{
		string result = string.Empty;
		if (info != null)
		{
			switch (info.messageType)
			{
			case ServerMessageEntry.MessageType.SendEnergy:
				result = TextUtility.GetCommonText("PresentBox", "present_from_friend", "{FRIEND_NAME}", info.name);
				break;
			case ServerMessageEntry.MessageType.ReturnSendEnergy:
				result = TextUtility.GetCommonText("PresentBox", "remuneration_friend_present");
				break;
			case ServerMessageEntry.MessageType.InviteCode:
				result = TextUtility.GetCommonText("PresentBox", "remuneration_friend_invite");
				break;
			}
		}
		return result;
	}

	public static string GetReceivedTime(int expireTime)
	{
		string result = string.Empty;
		if (expireTime == 0)
		{
			result = TextUtility.GetCommonText("PresentBox", "unlimited_duration");
		}
		else
		{
			int num = expireTime - NetUtil.GetCurrentUnixTime();
			if (num >= 86400)
			{
				result = TextUtility.GetCommonText("PresentBox", "expire_days", "{DAYS}", (num / 86400).ToString());
			}
			else if (num >= 3600)
			{
				result = TextUtility.GetCommonText("PresentBox", "expire_hours", "{HOURS}", (num / 3600).ToString());
			}
			else
			{
				result = TextUtility.GetCommonText("PresentBox", "expire_minutes", "{MINUTES}", (num / 60).ToString());
			}
		}
		return result;
	}

	public static bool IsWithinTimeLimit(int expireTime)
	{
		int num = expireTime - NetUtil.GetCurrentUnixTime();
		return num > 0;
	}

	public static string GetPresetTextList(List<ServerPresentState> presentStateList)
	{
		string text = string.Empty;
		foreach (ServerPresentState current in presentStateList)
		{
			ServerItem serverItem = new ServerItem((ServerItem.Id)current.m_itemId);
			string itemName = PresentBoxUtility.GetItemName(serverItem);
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				itemName,
				"×",
				current.m_numItem.ToString(),
				"\n"
			});
		}
		return text;
	}
}
