using System;
using Text;
using UnityEngine;

public class MileageMapText : MonoBehaviour
{
	private static int m_start_episode = -1;

	private static int m_start_pre_episode = -1;

	private void Start()
	{
		base.enabled = false;
	}

	public static void Load(ResourceSceneLoader sceneLoader, int episode, int pre_episode)
	{
		TextManager.Load(sceneLoader, TextManager.TextType.TEXTTYPE_MILEAGE_MAP_COMMON, "text_mileage_map_common_text");
		int startEpisode = MileageMapText.GetStartEpisode(episode);
		if (MileageMapText.m_start_episode != -1 && MileageMapText.m_start_episode != startEpisode)
		{
			TextManager.UnLoad(TextManager.TextType.TEXTTYPE_MILEAGE_MAP_EPISODE);
		}
		TextManager.Load(sceneLoader, TextManager.TextType.TEXTTYPE_MILEAGE_MAP_EPISODE, MileageMapText.GetTextFileName(startEpisode));
		MileageMapText.m_start_episode = startEpisode;
		if (pre_episode != -1)
		{
			int startEpisode2 = MileageMapText.GetStartEpisode(pre_episode);
			if (startEpisode != startEpisode2)
			{
				TextManager.Load(sceneLoader, TextManager.TextType.TEXTTYPE_MILEAGE_MAP_PRE_EPISODE, MileageMapText.GetTextFileName(startEpisode2));
				MileageMapText.m_start_pre_episode = startEpisode2;
			}
		}
	}

	public static void Setup()
	{
		TextManager.Setup(TextManager.TextType.TEXTTYPE_MILEAGE_MAP_COMMON, "text_mileage_map_common_text");
		if (MileageMapText.m_start_episode != -1)
		{
			TextManager.Setup(TextManager.TextType.TEXTTYPE_MILEAGE_MAP_EPISODE, MileageMapText.GetTextFileName(MileageMapText.m_start_episode));
		}
		if (MileageMapText.m_start_pre_episode != -1)
		{
			TextManager.Setup(TextManager.TextType.TEXTTYPE_MILEAGE_MAP_PRE_EPISODE, MileageMapText.GetTextFileName(MileageMapText.m_start_pre_episode));
		}
	}

	public static void DestroyPreEPisodeText()
	{
		if (MileageMapText.m_start_pre_episode != -1)
		{
			TextManager.UnLoad(TextManager.TextType.TEXTTYPE_MILEAGE_MAP_PRE_EPISODE);
			MileageMapText.m_start_pre_episode = -1;
		}
	}

	public static string GetText(int episode, string label)
	{
		if (label.IndexOf("cmn_") == 0)
		{
			TextManager.TextType type = TextManager.TextType.TEXTTYPE_MILEAGE_MAP_COMMON;
			return TextUtility.GetText(type, "MileageMap", label);
		}
		int startEpisode = MileageMapText.GetStartEpisode(episode);
		if (MileageMapText.m_start_episode == startEpisode)
		{
			TextManager.TextType type2 = TextManager.TextType.TEXTTYPE_MILEAGE_MAP_EPISODE;
			return TextUtility.GetText(type2, "MileageMap", label);
		}
		if (MileageMapText.m_start_pre_episode == startEpisode)
		{
			TextManager.TextType type3 = TextManager.TextType.TEXTTYPE_MILEAGE_MAP_PRE_EPISODE;
			return TextUtility.GetText(type3, "MileageMap", label);
		}
		return null;
	}

	public static string GetMapCommonText(string label)
	{
		if (label.IndexOf("cmn_") == 0)
		{
			TextManager.TextType type = TextManager.TextType.TEXTTYPE_MILEAGE_MAP_COMMON;
			return TextUtility.GetText(type, "MileageMap", label);
		}
		return null;
	}

	public static string GetName(string name)
	{
		TextManager.TextType textType = TextManager.TextType.TEXTTYPE_MILEAGE_MAP_COMMON;
		TextObject text = TextManager.GetText(textType, "Name", name);
		if (text != null && text.text != null)
		{
			return text.text;
		}
		return null;
	}

	private static int GetStartEpisode(int episode)
	{
		if (episode > 0)
		{
			int num = episode / 10;
			if (episode % 10 == 0)
			{
				num--;
			}
			int num2 = num * 10;
			return num2 + 1;
		}
		return 1;
	}

	private static string GetTextFileName(int start_episode)
	{
		if (start_episode > 0)
		{
			int num = start_episode + 9;
			return string.Concat(new string[]
			{
				"text_mileage_map_episode_",
				start_episode.ToString("D2"),
				"_to_",
				num.ToString("D2"),
				"_text"
			});
		}
		return "text_mileage_map_episode_01_to_10_text";
	}
}
