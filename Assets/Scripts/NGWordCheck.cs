using DataTable;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class NGWordCheck
{
	private static List<NGWordData> m_wordData = new List<NGWordData>();

	private static bool m_debugDrawLocal = false;

	private static bool m_errorDraw = true;

	private static ResourceSceneLoader m_sceneLoader = null;

	public static void Load()
	{
		if (NGWordCheck.m_sceneLoader == null)
		{
			string name = "NGWordResourceSceneLoader";
			GameObject gameObject = GameObject.Find(name);
			if (gameObject == null)
			{
				gameObject = new GameObject(name);
			}
			if (gameObject != null)
			{
				NGWordCheck.m_sceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
				bool onAssetBundle = true;
				if (NGWordCheck.m_sceneLoader.AddLoadAndResourceManager("NGWordTable", onAssetBundle, ResourceCategory.ETC, true, false, null))
				{
					NGWordCheck.DebugDrawLocal("Load");
				}
			}
		}
	}

	public static bool IsLoaded()
	{
		return !(NGWordCheck.m_sceneLoader != null) || NGWordCheck.m_sceneLoader.Loaded;
	}

	public static void ResetData()
	{
		NGWordTable nGWordTable = GameObjectUtil.FindGameObjectComponent<NGWordTable>("NGWordTable");
		if (nGWordTable != null)
		{
			UnityEngine.Object.Destroy(nGWordTable.gameObject);
		}
		if (NGWordCheck.m_sceneLoader != null)
		{
			UnityEngine.Object.Destroy(NGWordCheck.m_sceneLoader.gameObject);
		}
		NGWordCheck.DebugDrawLocal("ResetData");
	}

	public static string Check(string target_word, UILabel uiLabel)
	{
		NGWordCheck.DebugDrawLocal("check target_word=" + target_word);
		if (NGWordCheck.isCheckUILabel(target_word, uiLabel))
		{
			NGWordCheck.ErrorDraw("target_word=" + target_word + " check error UILabel");
			return target_word;
		}
		if (NGWordCheck.isCheckEmoji(target_word))
		{
			NGWordCheck.ErrorDraw("target_word=" + target_word + " check error emoji");
			return target_word;
		}
		if (NGWordCheck.isCheckKisyuIzon(target_word))
		{
			NGWordCheck.ErrorDraw("target_word=" + target_word + " check error kisyu izon");
			return target_word;
		}
		NGWordCheck.SetupWordData();
		string text = NGWordCheck.convertKana(target_word);
		NGWordCheck.DebugDrawLocal("convertKana=" + text);
		int space_count = 0;
		string nospace_word = NGWordCheck.StrReplace(" ", string.Empty, text, ref space_count);
		return NGWordCheck.checkProc(text, nospace_word, space_count);
	}

	private static void SetupWordData()
	{
		if (NGWordCheck.m_wordData.Count == 0)
		{
			NGWordData[] dataTable = NGWordTable.GetDataTable();
			if (dataTable != null)
			{
				NGWordData[] array = dataTable;
				for (int i = 0; i < array.Length; i++)
				{
					NGWordData item = array[i];
					NGWordCheck.m_wordData.Add(item);
				}
			}
		}
		NGWordCheck.DebugDrawLocal("SetupWordData m_wordData.Count=" + NGWordCheck.m_wordData.Count);
	}

	private static string checkProc(string check_str, string nospace_word, int space_count)
	{
		NGWordCheck.DebugDrawLocal("checkProc check_str=" + check_str + " nospace_word=" + nospace_word);
		int num = 0;
		foreach (NGWordData current in NGWordCheck.m_wordData)
		{
			if (current.param == 0)
			{
				if (check_str.IndexOf(current.word) != -1)
				{
					NGWordCheck.ErrorDraw(string.Concat(new string[]
					{
						"0 check_str=",
						check_str,
						" checkProc index=",
						num.ToString(),
						" row.word=",
						current.word
					}));
					string word = current.word;
					return word;
				}
				if (nospace_word.IndexOf(current.word) != -1)
				{
					NGWordCheck.ErrorDraw(string.Concat(new string[]
					{
						"0 nospace_word=",
						nospace_word,
						" checkProc index=",
						num.ToString(),
						" row.word=",
						current.word
					}));
					string word = current.word;
					return word;
				}
			}
			else
			{
				if (check_str == current.word)
				{
					NGWordCheck.ErrorDraw(string.Concat(new string[]
					{
						"1 check_str=",
						check_str,
						" checkProc index=",
						num.ToString(),
						" row.word=",
						current.word
					}));
					string word = current.word;
					return word;
				}
				if (nospace_word == current.word)
				{
					NGWordCheck.ErrorDraw(string.Concat(new string[]
					{
						"1 nospace_word=",
						nospace_word,
						" checkProc index=",
						num.ToString(),
						" row.word=",
						current.word
					}));
					string word = current.word;
					return word;
				}
			}
			num++;
		}
		return null;
	}

	private static bool isCheckUILabel(string str, UILabel uiLabel)
	{
		if (str != null && uiLabel != null)
		{
			UIFont font = uiLabel.font;
			if (font != null)
			{
				BMFont bmFont = font.bmFont;
				if (bmFont != null)
				{
					for (int i = 0; i < str.Length; i++)
					{
						char c = str[i];
						if (!font.isDynamic)
						{
							if (bmFont.GetGlyph((int)c) == null)
							{
								NGWordCheck.DebugDrawLocal(string.Concat(new object[]
								{
									"isCheckUILabel BMGlyph str=",
									str,
									" i=",
									i,
									" c=",
									c
								}));
								return true;
							}
						}
						else
						{
							Font dynamicFont = font.dynamicFont;
							CharacterInfo characterInfo;
							if (dynamicFont != null && !dynamicFont.GetCharacterInfo(c, out characterInfo, font.dynamicFontSize, font.dynamicFontStyle))
							{
								NGWordCheck.DebugDrawLocal(string.Concat(new object[]
								{
									"isCheckUILabel dynamicFont str=",
									str,
									" i=",
									i,
									" c=",
									c
								}));
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	private static bool isCheckEmoji(string str)
	{
		string text = "[\\u2002-\\u2005]|\\u203C|\\u2049|\\u2122|\\u2139|[\\u2194-\\u2199]|\\u21A9|\\u21AA";
		if (NGWordCheck.PregMatch(text, str))
		{
			NGWordCheck.DebugDrawLocal("PregMatch " + text);
			return true;
		}
		string text2 = "\\u231A|\\u231B|[\\u23E9-\\u23EC]|\\u23F0|\\u23F3|\\u24C2|\\u25AA|\\u25AB|\\u25B6|\\u25C0|[\\u25FB-\\u25FE]";
		if (NGWordCheck.PregMatch(text2, str))
		{
			NGWordCheck.DebugDrawLocal("PregMatch " + text2);
			return true;
		}
		string text3 = "[\\u2600-\\u27FF]|\\u2934|\\u2935|[\\u2B05-\\u2B07]|\\u2B1B|\\u2B1C|\\u2B50|\\u2B55|\\u3030|\\u303D|\\u3297|\\u3299";
		if (NGWordCheck.PregMatch(text3, str))
		{
			NGWordCheck.DebugDrawLocal("PregMatch " + text3);
			return true;
		}
		string text4 = "[\\uE000-\\uF8FF]";
		if (NGWordCheck.PregMatch(text4, str))
		{
			NGWordCheck.DebugDrawLocal("PregMatch " + text4);
			return true;
		}
		string text5 = "[\\uD800-\\uE000]";
		if (NGWordCheck.PregMatch(text5, str))
		{
			NGWordCheck.DebugDrawLocal("PregMatch " + text5);
			return true;
		}
		return false;
	}

	private static bool isCheckKisyuIzon(string str)
	{
		string text = "～∥―－①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ㍉㌔㌢㍍㌘㌧㌃㌶㍑㍗㌍㌦㌣㌫㍊㌻㎜㎝㎞㎎㎏㏄㎡㍻〝〟№㏍℡㊤㊥㊦㊧㊨㈱㈲㈹㍾㍽㍼≒≡∫∮∑√⊥∠∟⊿∵∩∪ⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ￢￤＇＂㈱№℡∵";
		string text2 = text;
		for (int i = 0; i < text2.Length; i++)
		{
			char value = text2[i];
			if (str.IndexOf(value) != -1)
			{
				NGWordCheck.DebugDrawLocal("isCheckKisyuIzon=" + value.ToString());
				return true;
			}
		}
		return false;
	}

	private static string convertKana(string value)
	{
		string text = NGWordCheck.PregReplace("\\s+", " ", value);
		text = NGWordCheck.MbConvertKana_r(text);
		text = NGWordCheck.MbConvertKana_n(text);
		text = NGWordCheck.MbConvertKana_K(text);
		text = NGWordCheck.MbConvertKana_C(text);
		text = NGWordCheck.MbConvertKana_V(text);
		text = NGWordCheck.StrToLower(text);
		text = NGWordCheck.str2UpperKana(text);
		return NGWordCheck.replaceSpecialString(text);
	}

	private static string MbConvertKana_r(string str)
	{
		string text = "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ";
		string text2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		char[] array = text.ToCharArray();
		char[] array2 = text2.ToCharArray();
		if (array.Length == array2.Length)
		{
			for (int i = 0; i < array.Length; i++)
			{
				str = str.Replace(array[i], array2[i]);
			}
			char[] array3 = text.ToLower().ToCharArray();
			char[] array4 = text2.ToLower().ToCharArray();
			for (int j = 0; j < array3.Length; j++)
			{
				str = str.Replace(array3[j], array4[j]);
			}
		}
		return str;
	}

	private static string MbConvertKana_n(string str)
	{
		string text = "０１２３４５６７８９";
		string text2 = "0123456789";
		char[] array = text.ToCharArray();
		char[] array2 = text2.ToCharArray();
		if (array.Length == array2.Length)
		{
			for (int i = 0; i < array.Length; i++)
			{
				str = str.Replace(array[i], array2[i]);
			}
		}
		return str;
	}

	private static string MbConvertKana_K(string str)
	{
		string text = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン゛゜ァィゥェォャュョッ";
		string text2 = "ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝﾞﾟｧｨｩｪｫｬｭｮｯ";
		char[] array = text.ToCharArray();
		char[] array2 = text2.ToCharArray();
		if (array.Length == array2.Length)
		{
			for (int i = 0; i < array.Length; i++)
			{
				str = str.Replace(array2[i], array[i]);
			}
		}
		return str;
	}

	private static string MbConvertKana_C(string str)
	{
		string text = "ぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもゃやゅゆょよらりるれろゎわゐゑをん";
		string text2 = "ァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲン";
		char[] array = text.ToCharArray();
		char[] array2 = text2.ToCharArray();
		if (array.Length == array2.Length)
		{
			for (int i = 0; i < array.Length; i++)
			{
				str = str.Replace(array[i], array2[i]);
			}
		}
		return str;
	}

	private static string MbConvertKana_V(string str)
	{
		str = NGWordCheck.PregReplace("゛+", "゛", str);
		str = NGWordCheck.PregReplace("゜+", "゜", str);
		bool flag;
		do
		{
			flag = false;
			string str2 = str;
			str = NGWordCheck.MbConvertKana_V_bottom(str2, ref flag);
		}
		while (flag);
		return str;
	}

	private static string MbConvertKana_V_bottom(string str, ref bool change)
	{
		string text = "ウヴカガキギクグケゲコゴサザシジスズセゼソゾタダチヂツヅテデトドハバヒビフブヘベホボ";
		string text2 = "ヴヴガガギギググゲゲゴゴザザジジズズゼゼゾゾダダヂヂヅヅデデドドババビビブブベベボボ";
		char[] array = text2.ToCharArray();
		string text3 = "ハパヒピフプヘペホポ";
		string text4 = "パパピピププペペポポ";
		char[] array2 = text4.ToCharArray();
		change = false;
		if (str != null && str.Length > 0)
		{
			char[] array3 = str.ToCharArray();
			int num = str.Length - 1;
			for (int i = num; i >= 0; i--)
			{
				char c = array3[i];
				if (c == '゛')
				{
					int num2 = i - 1;
					if (num2 >= 0)
					{
						char value = array3[num2];
						int num3 = text.IndexOf(value, 0);
						if (num3 != -1 && num3 < array.Length)
						{
							string ptn = value.ToString() + c.ToString();
							change = true;
							return NGWordCheck.PregReplace(ptn, array[num3].ToString(), str);
						}
					}
				}
				else if (c == '゜')
				{
					int num4 = i - 1;
					if (num4 >= 0)
					{
						char value2 = array3[num4];
						int num5 = text3.IndexOf(value2, 0);
						if (num5 != -1 && num5 < array2.Length)
						{
							string ptn2 = value2.ToString() + c.ToString();
							change = true;
							return NGWordCheck.PregReplace(ptn2, array2[num5].ToString(), str);
						}
					}
				}
			}
		}
		return str;
	}

	private static string replaceSpecialString(string value)
	{
		string[] ptn = new string[]
		{
			"ー",
			"×",
			"○",
			"！",
			"．",
			"ー"
		};
		string[] newStr = new string[]
		{
			"-",
			"x",
			"0",
			"!",
			".",
			"-"
		};
		return NGWordCheck.PregReplace(ptn, newStr, value);
	}

	private static string str2UpperKana(string value)
	{
		string[] ptn = new string[]
		{
			"ァ",
			"ィ",
			"ゥ",
			"ェ",
			"ォ",
			"ッ",
			"ャ",
			"ュ",
			"ョ",
			"ヮ",
			"ヵ",
			"ヶ"
		};
		string[] newStr = new string[]
		{
			"ア",
			"イ",
			"ウ",
			"エ",
			"オ",
			"ツ",
			"ヤ",
			"ユ",
			"ヨ",
			"ワ",
			"カ",
			"ケ"
		};
		return NGWordCheck.PregReplace(ptn, newStr, value);
	}

	private static string StrToLower(string str)
	{
		return str.ToLower();
	}

	private static string StrReplace(string oldWord, string newWord, string str, ref int count)
	{
		string[] separator = new string[]
		{
			oldWord
		};
		string[] array = str.Split(separator, StringSplitOptions.None);
		count = array.Length - 1;
		return NGWordCheck.PregReplace(oldWord, newWord, str);
	}

	private static string PregReplace(string ptn, string newStr, string str)
	{
		return Regex.Replace(str, ptn, newStr);
	}

	private static string PregReplace(string[] ptn, string[] newStr, string str)
	{
		if (ptn.Length == newStr.Length)
		{
			for (int i = 0; i < ptn.Length; i++)
			{
				str = NGWordCheck.PregReplace(ptn[i], newStr[i], str);
			}
		}
		return str;
	}

	private static bool PregMatch(string ptn, string str)
	{
		return Regex.IsMatch(str, ptn);
	}

	private static void DebugDrawLocal(string str)
	{
	}

	private static void ErrorDraw(string str)
	{
	}
}
