using System;
using System.Collections.Generic;
using UnityEngine;

namespace Text
{
	public class TextManager : MonoBehaviour
	{
		public enum TextType
		{
			TEXTTYPE_NONE = -1,
			TEXTTYPE_COMMON_TEXT,
			TEXTTYPE_MILEAGE_MAP_COMMON,
			TEXTTYPE_MILEAGE_MAP_EPISODE,
			TEXTTYPE_MILEAGE_MAP_PRE_EPISODE,
			TEXTTYPE_FIXATION_TEXT,
			TEXTTYPE_EVENT_COMMON_TEXT,
			TEXTTYPE_EVENT_SPECIFIC,
			TEXTTYPE_CHAO_TEXT,
			TEXTTYPE_END
		}

		private static TextManager m_instance;

		private Dictionary<TextManager.TextType, TextLoadImpl> m_textDictionary;

		private string m_languageName;

		private string m_suffixeName;

		public static void SetLanguageName()
		{
			TextManager instance = TextManager.GetInstance();
			instance.m_languageName = TextUtility.GetXmlLanguageDataType();
		}

		public static void SetSuffixeName()
		{
			TextManager instance = TextManager.GetInstance();
			instance.m_suffixeName = TextUtility.GetSuffixe();
		}

		private void Awake()
		{
			if (TextManager.m_instance == null)
			{
				TextManager.m_instance = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				Language.InitAppEnvLanguage();
				this.m_languageName = TextUtility.GetXmlLanguageDataType();
				this.m_suffixeName = TextUtility.GetSuffixe();
				this.m_textDictionary = new Dictionary<TextManager.TextType, TextLoadImpl>();
				this.SetupFixationText();
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
			if (TextManager.m_instance == this)
			{
				this.m_textDictionary.Clear();
				TextManager.m_instance = null;
			}
		}

		private void SetupFixationText()
		{
			TextManager.TextType key = TextManager.TextType.TEXTTYPE_FIXATION_TEXT;
			Dictionary<TextManager.TextType, TextLoadImpl> textDictionary = this.m_textDictionary;
			if (textDictionary.ContainsKey(key))
			{
				return;
			}
			textDictionary.Add(key, new TextLoadImpl());
			string fileName = "TextData/text_fixation_text_" + this.m_suffixeName;
			if (!textDictionary[key].IsSetup())
			{
				textDictionary[key].LoadResourcesSetup(fileName, this.m_languageName);
			}
		}

		public static void NotLoadSetupCommonText()
		{
			TextManager.NotLoadSetupText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "text_common_text");
			TextManager.SetupCommonText();
		}

		public static void NotLoadSetupChaoText()
		{
			TextManager.NotLoadSetupText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, "text_chao_text");
			TextManager.SetupChaoText();
		}

		public static void NotLoadSetupEventText()
		{
			TextManager.NotLoadSetupText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "text_event_common_text");
			TextManager.SetupEventText();
		}

		private static void NotLoadSetupText(TextManager.TextType textType, string baseName)
		{
			TextManager instance = TextManager.GetInstance();
			if (instance == null)
			{
				return;
			}
			Dictionary<TextManager.TextType, TextLoadImpl> textDictionary = instance.m_textDictionary;
			if (textDictionary.ContainsKey(textType))
			{
				GameObject gameObject = GameObject.Find(baseName + "_" + instance.m_suffixeName);
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				return;
			}
			textDictionary.Add(textType, new TextLoadImpl());
		}

		public static void LoadCommonText(ResourceSceneLoader sceneLoader)
		{
			TextManager.Load(sceneLoader, TextManager.TextType.TEXTTYPE_COMMON_TEXT, "text_common_text");
		}

		public static void LoadChaoText(ResourceSceneLoader sceneLoader)
		{
			TextManager.Load(sceneLoader, TextManager.TextType.TEXTTYPE_CHAO_TEXT, "text_chao_text");
		}

		public static void LoadEventText(ResourceSceneLoader sceneLoader)
		{
			TextManager.Load(sceneLoader, TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "text_event_common_text");
		}

		private static string GetEventProductionTextName(int specificId)
		{
			if (specificId > 0)
			{
				return "text_event_" + specificId.ToString() + "_text";
			}
			return null;
		}

		public static void LoadEventProductionText(ResourceSceneLoader sceneLoader)
		{
			if (EventManager.Instance != null && !EventUtility.IsExistSpecificEventText(EventManager.Instance.Id))
			{
				return;
			}
			int specificId = EventManager.GetSpecificId();
			string eventProductionTextName = TextManager.GetEventProductionTextName(specificId);
			if (!string.IsNullOrEmpty(eventProductionTextName))
			{
				TextManager.Load(sceneLoader, TextManager.TextType.TEXTTYPE_EVENT_SPECIFIC, eventProductionTextName);
			}
		}

		public static void SetupCommonText()
		{
			TextManager.Setup(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "text_common_text");
		}

		public static void SetupChaoText()
		{
			TextManager.Setup(TextManager.TextType.TEXTTYPE_CHAO_TEXT, "text_chao_text");
		}

		public static void SetupEventText()
		{
			TextManager.Setup(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "text_event_common_text");
		}

		public static void SetupEventProductionText()
		{
			if (EventManager.Instance != null && !EventUtility.IsExistSpecificEventText(EventManager.Instance.Id))
			{
				return;
			}
			int specificId = EventManager.GetSpecificId();
			string eventProductionTextName = TextManager.GetEventProductionTextName(specificId);
			if (!string.IsNullOrEmpty(eventProductionTextName))
			{
				TextManager.Setup(TextManager.TextType.TEXTTYPE_EVENT_SPECIFIC, eventProductionTextName);
			}
		}

		public static void Load(ResourceSceneLoader sceneLoader, TextManager.TextType textType, string fileName)
		{
			TextManager instance = TextManager.GetInstance();
			if (instance == null)
			{
				return;
			}
			Dictionary<TextManager.TextType, TextLoadImpl> textDictionary = instance.m_textDictionary;
			if (textDictionary.ContainsKey(textType))
			{
				return;
			}
			textDictionary.Add(textType, new TextLoadImpl(sceneLoader, fileName, instance.m_suffixeName));
		}

		public static void Setup(TextManager.TextType textType, string fileName)
		{
			TextManager instance = TextManager.GetInstance();
			if (instance == null)
			{
				return;
			}
			Dictionary<TextManager.TextType, TextLoadImpl> textDictionary = instance.m_textDictionary;
			if (!textDictionary.ContainsKey(textType))
			{
				return;
			}
			if (!textDictionary[textType].IsSetup())
			{
				textDictionary[textType].LoadSceneSetup(fileName, instance.m_languageName, instance.m_suffixeName);
			}
		}

		public static void UnLoad(TextManager.TextType textType)
		{
			TextManager instance = TextManager.GetInstance();
			if (instance == null)
			{
				return;
			}
			Dictionary<TextManager.TextType, TextLoadImpl> textDictionary = instance.m_textDictionary;
			if (!textDictionary.ContainsKey(textType))
			{
				return;
			}
			textDictionary.Remove(textType);
		}

		public static TextObject GetText(TextManager.TextType textType, string categoryName, string cellName)
		{
			TextManager instance = TextManager.GetInstance();
			if (instance == null)
			{
				return new TextObject(string.Empty);
			}
			Dictionary<TextManager.TextType, TextLoadImpl> textDictionary = instance.m_textDictionary;
			if (!textDictionary.ContainsKey(textType))
			{
				return new TextObject(string.Empty);
			}
			return new TextObject(textDictionary[textType].GetText(categoryName, cellName));
		}

		public static int GetCategoryCellCount(TextManager.TextType textType, string categoryName)
		{
			TextManager instance = TextManager.GetInstance();
			if (instance == null)
			{
				return -1;
			}
			Dictionary<TextManager.TextType, TextLoadImpl> textDictionary = instance.m_textDictionary;
			if (!textDictionary.ContainsKey(textType))
			{
				return -1;
			}
			return textDictionary[textType].GetCellCount(categoryName);
		}

		private static TextManager GetInstance()
		{
			if (TextManager.m_instance == null)
			{
				GameObject gameObject = new GameObject("TextManager");
				gameObject.AddComponent<TextManager>();
			}
			return TextManager.m_instance;
		}
	}
}
