using DataTable;
using System;
using Text;
using UI;
using UnityEngine;

public class HudUtility
{
	public enum TextureType
	{
		TYPE_S,
		TYPE_M,
		TYPE_L
	}

	private class PurchaseLimit
	{
		public int m_underAge;

		public int m_maxPurchaseOfMonth;

		public PurchaseLimit(int underAge, int maxMoney)
		{
			this.m_underAge = underAge;
			this.m_maxPurchaseOfMonth = maxMoney;
		}
	}

	private const int PURCHASE_AGE_0 = 13;

	private const int PURCHASE_MONEY_0 = 5000;

	private const int PURCHASE_AGE_1 = 20;

	private const int PURCHASE_MONEY_1 = 20000;

	private static HudUtility.PurchaseLimit[] s_purchaseLimits = new HudUtility.PurchaseLimit[]
	{
		new HudUtility.PurchaseLimit(13, 5000),
		new HudUtility.PurchaseLimit(20, 20000)
	};

	public static string GetFormatNumString<Type>(Type num)
	{
		string text = string.Format("{0:#,0}", num);
		return text.Replace(",", " ");
	}

	public static void SetInvalidNGUIMitiTouch()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			UICamera uICamera = GameObjectUtil.FindChildGameObjectComponent<UICamera>(gameObject, "Camera");
			if (uICamera != null)
			{
				uICamera.allowMultiTouch = false;
			}
		}
	}

	public static string GetEventStageName()
	{
		return HudUtility.GetEventStageName(EventManager.GetSpecificId());
	}

	public static string GetEventSpObjectName()
	{
		return HudUtility.GetEventSpObjectName(EventManager.GetSpecificId());
	}

	public static string GetEventStageName(int specificId)
	{
		string cellID = "sp_stage_name_" + specificId.ToString();
		return TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Name", cellID);
	}

	public static string GetEventSpObjectName(int specificId)
	{
		string cellID = "sp_object_name_" + specificId.ToString();
		return TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Name", cellID);
	}

	public static string MakeCharaTextureName(CharaType chara, HudUtility.TextureType texType)
	{
		string text = "ui_tex_player_";
		switch (texType)
		{
		case HudUtility.TextureType.TYPE_S:
			text += "set_";
			text += string.Format("{0:00}", (int)chara);
			text += "_";
			text += CharaName.PrefixName[(int)chara];
			break;
		case HudUtility.TextureType.TYPE_L:
			text += string.Format("{0:00}", (int)chara);
			text += "_";
			text += CharaName.PrefixName[(int)chara];
			break;
		}
		return text;
	}

	public static Vector2 GetScreenPosition(Camera camera, Vector3 worldPos)
	{
		Vector2 result = new Vector2(0f, 0f);
		if (camera == null)
		{
			return result;
		}
		result = camera.WorldToScreenPoint(worldPos);
		return result;
	}

	public static Vector2 GetScreenPosition(Camera camera, GameObject chaseObject)
	{
		if (chaseObject == null)
		{
			return new Vector2(0f, 0f);
		}
		return HudUtility.GetScreenPosition(camera, chaseObject.transform.localPosition);
	}

	public static GameObject LoadPrefab(string prefabName, string attachAnthorName)
	{
		GameObject gameObject = Resources.Load(prefabName) as GameObject;
		if (gameObject == null)
		{
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
		GameObject gameObject3 = GameObject.Find(attachAnthorName);
		if (gameObject3 == null)
		{
			return null;
		}
		Vector3 localPosition = gameObject2.transform.localPosition;
		Vector3 localScale = gameObject2.transform.localScale;
		gameObject2.transform.parent = gameObject3.transform;
		gameObject2.transform.localPosition = localPosition;
		gameObject2.transform.localScale = localScale;
		return gameObject2;
	}

	public static string GetCharaAttributeSpriteName(CharaType charaType)
	{
		CharacterDataNameInfo instance = CharacterDataNameInfo.Instance;
		if (instance == null)
		{
			return string.Empty;
		}
		CharacterDataNameInfo.Info dataByID = instance.GetDataByID(charaType);
		if (dataByID == null)
		{
			return string.Empty;
		}
		int attribute = (int)dataByID.m_attribute;
		return "ui_mm_player_species_" + attribute.ToString();
	}

	public static string GetTeamAttributeSpriteName(CharaType charaType)
	{
		CharacterDataNameInfo instance = CharacterDataNameInfo.Instance;
		if (instance == null)
		{
			return string.Empty;
		}
		CharacterDataNameInfo.Info dataByID = instance.GetDataByID(charaType);
		if (dataByID == null)
		{
			return string.Empty;
		}
		int teamAttribute = (int)dataByID.m_teamAttribute;
		return "ui_mm_player_genus_" + teamAttribute.ToString();
	}

	public static int GetMixedStringToInt(string s)
	{
		string text = string.Empty;
		if (s == null)
		{
			return 0;
		}
		for (int i = 0; i < s.Length; i++)
		{
			char c = s[i];
			if (c >= '0' && c <= '9')
			{
				text += c;
			}
		}
		int result = 0;
		int.TryParse(text, out result);
		return result;
	}

	public static int GetAge(DateTime birthDate, DateTime nowDate)
	{
		int num = nowDate.Year - birthDate.Year;
		if (nowDate.Month < birthDate.Month)
		{
			num--;
		}
		global::Debug.Log(string.Concat(new object[]
		{
			"age=",
			nowDate,
			"-",
			birthDate,
			"=",
			num
		}));
		return num;
	}

	public static bool CheckPurchaseOver(string birthday, int monthPurchase, int addPurchase)
	{
		RegionManager instance = RegionManager.Instance;
		if (instance != null && !instance.IsJapan())
		{
			return false;
		}
		int num = 1;
		if (!string.IsNullOrEmpty(birthday))
		{
			num = HudUtility.GetAge(DateTime.Parse(birthday), NetUtil.GetCurrentTime());
		}
		global::Debug.Log("CheckPurchaseOver.birthday" + birthday);
		global::Debug.Log("CheckPurchaseOver.monthPurchase" + monthPurchase.ToString());
		global::Debug.Log("CheckPurchaseOver.addPurchase" + addPurchase.ToString());
		global::Debug.Log("CheckPurchaseOver.age" + num.ToString());
		HudUtility.PurchaseLimit[] array = HudUtility.s_purchaseLimits;
		for (int i = 0; i < array.Length; i++)
		{
			HudUtility.PurchaseLimit purchaseLimit = array[i];
			if (num < purchaseLimit.m_underAge && monthPurchase + addPurchase > purchaseLimit.m_maxPurchaseOfMonth)
			{
				global::Debug.Log("CheckPurchaseOver.OverPurchase");
				return true;
			}
		}
		global::Debug.Log("CheckPurchaseOver.InPurchase");
		return false;
	}

	public static string GetChaoAbilityText(int chao_id, int level = -1)
	{
		ChaoData chaoData = ChaoTable.GetChaoData(chao_id);
		if (chaoData != null)
		{
			if (level == -1)
			{
				level = chaoData.level;
			}
			return chaoData.GetDetailLevelPlusSP(level);
		}
		return string.Empty;
	}

	public static string GetChaoGrowAbilityText(int chao_id, int level = -1)
	{
		ChaoData chaoData = ChaoTable.GetChaoData(chao_id);
		if (chaoData != null)
		{
			if (level == -1)
			{
				level = chaoData.level;
			}
			return chaoData.GetGrowDetailLevelPlusSP(level);
		}
		return string.Empty;
	}

	public static string GetChaoLoadingAbilityText(int chao_id)
	{
		ChaoData chaoData = ChaoTable.GetChaoData(chao_id);
		if (chaoData != null)
		{
			return chaoData.GetLoadingDetailsLevel(chaoData.level);
		}
		return string.Empty;
	}

	public static string GetChaoSPLoadingAbilityText(int chao_id)
	{
		ChaoData chaoData = ChaoTable.GetChaoData(chao_id);
		if (chaoData != null)
		{
			return chaoData.GetSPLoadingDetailsLevel(chaoData.level);
		}
		return string.Empty;
	}

	public static string GetChaoMenuAbilityText(int chao_id)
	{
		ChaoData chaoData = ChaoTable.GetChaoData(chao_id);
		if (chaoData != null)
		{
			string sPMainMenuDetailsLevel = chaoData.GetSPMainMenuDetailsLevel(chaoData.level);
			if (!string.IsNullOrEmpty(sPMainMenuDetailsLevel))
			{
				return sPMainMenuDetailsLevel;
			}
		}
		return string.Empty;
	}

	public static string GetChaoCountBonusText(float value)
	{
		return TextUtility.Replace(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoSet", "bonus_percent").text, "{BONUS}", value.ToString("F1"));
	}

	public static void SetChaoTexture(UITexture uiTex, int chaoId, bool refreshFlag)
	{
		if (uiTex == null)
		{
			return;
		}
		if (chaoId >= 0)
		{
			ChaoTextureManager instance = ChaoTextureManager.Instance;
			if (instance != null)
			{
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uiTex, null, true);
				ChaoTextureManager.Instance.GetTexture(chaoId, info);
			}
			uiTex.gameObject.SetActive(true);
		}
		else
		{
			uiTex.gameObject.SetActive(false);
		}
	}

	public static void SetupUILabelText(GameObject obj)
	{
		if (obj != null)
		{
			UILocalizeText component = obj.GetComponent<UILocalizeText>();
			if (component != null)
			{
				component.SetUILabelText();
			}
		}
	}
}
