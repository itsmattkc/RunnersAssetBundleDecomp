using App;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Text
{
	public class TextUtility
	{
		private static Dictionary<string, int> __f__switch_map7;

		public static string Replaces(string text, Dictionary<string, string> replaceDic)
		{
			if (text == null)
			{
				UnityEngine.Debug.LogWarning("TextUtility.Replaces() first argument is null");
				return string.Empty;
			}
			foreach (string current in replaceDic.Keys)
			{
				text = text.Replace(current, replaceDic[current]);
			}
			return text;
		}

		public static string Replace(string text, string srcText, string dstText)
		{
			return TextUtility.Replaces(text, new Dictionary<string, string>
			{
				{
					srcText,
					dstText
				}
			});
		}

		public static void SetText(UILabel label, TextManager.TextType type, string groupID, string cellID)
		{
			if (label != null)
			{
				TextObject text = TextManager.GetText(type, groupID, cellID);
				if (text != null && text.text != null)
				{
					label.text = text.text;
				}
			}
		}

		public static void SetText(UILabel label, TextManager.TextType type, string groupID, string cellID, string tag, string replace)
		{
			if (label != null)
			{
				TextObject text = TextManager.GetText(type, groupID, cellID);
				if (text != null && text.text != null)
				{
					text.ReplaceTag(tag, replace);
					label.text = text.text;
				}
			}
		}

		public static void SetCommonText(UILabel label, string groupID, string cellID)
		{
			TextUtility.SetText(label, TextManager.TextType.TEXTTYPE_COMMON_TEXT, groupID, cellID);
		}

		public static void SetCommonText(UILabel label, string groupID, string cellID, string tag, string replace)
		{
			TextUtility.SetText(label, TextManager.TextType.TEXTTYPE_COMMON_TEXT, groupID, cellID, tag, replace);
		}

		public static string GetText(TextManager.TextType type, string groupID, string cellID)
		{
			TextObject text = TextManager.GetText(type, groupID, cellID);
			if (text != null && text.text != null)
			{
				return text.text;
			}
			return null;
		}

		public static string GetText(TextManager.TextType type, string groupID, string cellID, string tag, string replace)
		{
			TextObject text = TextManager.GetText(type, groupID, cellID);
			if (text != null && text.text != null)
			{
				text.ReplaceTag(tag, replace);
				return text.text;
			}
			return null;
		}

		public static string GetCommonText(string groupID, string cellID)
		{
			return TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, groupID, cellID);
		}

		public static string GetCommonText(string groupID, string cellID, string tag, string replace)
		{
			return TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, groupID, cellID, tag, replace);
		}

		public static string GetChaoText(string groupID, string cellID)
		{
			return TextUtility.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, groupID, cellID);
		}

		public static string GetChaoText(string groupID, string cellID, string tag, string replace)
		{
			return TextUtility.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, groupID, cellID, tag, replace);
		}

		public static string GetXmlLanguageDataType()
		{
			switch (Env.language)
			{
			case Env.Language.JAPANESE:
				return "Japanese";
			case Env.Language.ENGLISH:
				return "English";
			case Env.Language.CHINESE_ZHJ:
				return "SimplifiedChinese";
			case Env.Language.CHINESE_ZH:
				return "TraditionalChinese";
			case Env.Language.KOREAN:
				return "Korean";
			case Env.Language.FRENCH:
				return "French";
			case Env.Language.GERMAN:
				return "German";
			case Env.Language.SPANISH:
				return "Spanish";
			case Env.Language.PORTUGUESE:
				return "Portuguese";
			case Env.Language.ITALIAN:
				return "Italian";
			case Env.Language.RUSSIAN:
				return "Russian";
			default:
				return "English";
			}
		}

		public static string GetSuffixe()
		{
			switch (Env.language)
			{
			case Env.Language.JAPANESE:
				return "ja";
			case Env.Language.ENGLISH:
				return "en";
			case Env.Language.CHINESE_ZHJ:
				return "zhj";
			case Env.Language.CHINESE_ZH:
				return "zh";
			case Env.Language.KOREAN:
				return "ko";
			case Env.Language.FRENCH:
				return "fr";
			case Env.Language.GERMAN:
				return "de";
			case Env.Language.SPANISH:
				return "es";
			case Env.Language.PORTUGUESE:
				return "pt";
			case Env.Language.ITALIAN:
				return "it";
			case Env.Language.RUSSIAN:
				return "ru";
			default:
				return "en";
			}
		}

		public static string GetSuffix(Env.Language language)
		{
			switch (language)
			{
			case Env.Language.JAPANESE:
				return "ja";
			case Env.Language.ENGLISH:
				return "en";
			case Env.Language.CHINESE_ZHJ:
				return "zhj";
			case Env.Language.CHINESE_ZH:
				return "zh";
			case Env.Language.KOREAN:
				return "ko";
			case Env.Language.FRENCH:
				return "fr";
			case Env.Language.GERMAN:
				return "de";
			case Env.Language.SPANISH:
				return "es";
			case Env.Language.PORTUGUESE:
				return "pt";
			case Env.Language.ITALIAN:
				return "it";
			case Env.Language.RUSSIAN:
				return "ru";
			default:
				return "en";
			}
		}

		public static bool IsSuffix(string languageCode)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(languageCode) && languageCode != null)
			{
				if (TextUtility.__f__switch_map7 == null)
				{
					TextUtility.__f__switch_map7 = new Dictionary<string, int>(22)
					{
						{
							"ja",
							0
						},
						{
							"en",
							1
						},
						{
							"de",
							2
						},
						{
							"es",
							3
						},
						{
							"fr",
							4
						},
						{
							"it",
							5
						},
						{
							"ko",
							6
						},
						{
							"pt",
							7
						},
						{
							"ru",
							8
						},
						{
							"zhj",
							9
						},
						{
							"zh",
							10
						},
						{
							"JA",
							11
						},
						{
							"EN",
							12
						},
						{
							"DE",
							13
						},
						{
							"ES",
							14
						},
						{
							"FR",
							15
						},
						{
							"IT",
							16
						},
						{
							"KO",
							17
						},
						{
							"PT",
							18
						},
						{
							"RU",
							19
						},
						{
							"ZHJ",
							20
						},
						{
							"ZH",
							21
						}
					};
				}
				int num;
				if (TextUtility.__f__switch_map7.TryGetValue(languageCode, out num))
				{
					switch (num)
					{
					case 0:
						result = true;
						break;
					case 1:
						result = true;
						break;
					case 2:
						result = true;
						break;
					case 3:
						result = true;
						break;
					case 4:
						result = true;
						break;
					case 5:
						result = true;
						break;
					case 6:
						result = true;
						break;
					case 7:
						result = true;
						break;
					case 8:
						result = true;
						break;
					case 9:
						result = true;
						break;
					case 10:
						result = true;
						break;
					case 11:
						result = true;
						break;
					case 12:
						result = true;
						break;
					case 13:
						result = true;
						break;
					case 14:
						result = true;
						break;
					case 15:
						result = true;
						break;
					case 16:
						result = true;
						break;
					case 17:
						result = true;
						break;
					case 18:
						result = true;
						break;
					case 19:
						result = true;
						break;
					case 20:
						result = true;
						break;
					case 21:
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public static string GetTextLevel(string levelNumber)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_LevelNumber").text;
			return TextUtility.Replace(text, "{PARAM}", levelNumber);
		}
	}
}
