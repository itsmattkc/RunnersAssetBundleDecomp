using App.Utility;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using UnityEngine;

namespace SaveData
{
	public class SystemSaveManager : MonoBehaviour
	{
		public enum ErrorCode
		{
			NO_ERROR,
			FILE_NOT_EXIST,
			FILE_CANNOT_OPEN,
			DATA_INVALID,
			INVALID_DEVICE_ID
		}

		private const string SYSTEM_FILE_NAME = "sfrn";

		private const string EXTENSION = ".game";

		private const string XmlRootName = "SystemData";

		private bool m_gameDataDirty;

		private bool m_errorOnStart;

		private SystemData m_SystemData = new SystemData();

		private GameIDData m_GameIDData = new GameIDData();

		private int m_SaveDataVersion = 1;

		private SystemSaveManager.ErrorCode m_errorcode;

		private static SystemSaveManager instance;

		public static SystemSaveManager Instance
		{
			get
			{
				if (SystemSaveManager.instance == null)
				{
					SystemSaveManager.instance = (UnityEngine.Object.FindObjectOfType(typeof(SystemSaveManager)) as SystemSaveManager);
				}
				return SystemSaveManager.instance;
			}
		}

		protected void Awake()
		{
			this.CheckInstance();
		}

		private void Start()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
			this.Init();
		}

		public void Init()
		{
			this.m_SystemData.Init(this.m_SaveDataVersion);
			this.m_GameIDData.Init();
			if (!this.SystemFileCheck())
			{
				string @string = PlayerPrefs.GetString("aa7329ab4330306fbdd6dbe9b85c96be");
				if (!string.IsNullOrEmpty(@string) && !@string.Equals("0") && this.GetGameIDDataFromPlayerPrefs())
				{
					this.m_errorOnStart = true;
				}
				this.m_GameIDData.device = SystemInfo.deviceUniqueIdentifier;
				this.CheckLightMode();
				this.SaveSystemData();
			}
			else
			{
				this.LoadSystemData();
				if (this.m_errorcode == SystemSaveManager.ErrorCode.INVALID_DEVICE_ID)
				{
					this.m_SystemData.Init(this.m_SaveDataVersion);
					this.m_GameIDData.Init();
					this.m_GameIDData.device = SystemInfo.deviceUniqueIdentifier;
					this.CheckLightMode();
					this.SaveSystemData();
				}
				else if (this.m_errorcode != SystemSaveManager.ErrorCode.NO_ERROR)
				{
					this.GetGameIDDataFromPlayerPrefs();
					this.m_errorOnStart = true;
				}
			}
		}

		public static SystemData GetSystemSaveData()
		{
			if (SystemSaveManager.instance == null)
			{
				return null;
			}
			return SystemSaveManager.instance.GetSystemdata();
		}

		public SystemData GetSystemdata()
		{
			return this.m_SystemData;
		}

		public static string GetGameID()
		{
			if (SystemSaveManager.instance == null)
			{
				return "0";
			}
			if (SystemSaveManager.instance.m_GameIDData == null)
			{
				return "0";
			}
			return SystemSaveManager.instance.m_GameIDData.id;
		}

		public static bool SetGameID(string id)
		{
			if (SystemSaveManager.instance == null)
			{
				return false;
			}
			SystemSaveManager.instance.m_GameIDData.id = id;
			SystemSaveManager.instance.m_gameDataDirty = true;
			return true;
		}

		public static string GetGamePassword()
		{
			if (SystemSaveManager.instance == null)
			{
				return string.Empty;
			}
			if (SystemSaveManager.instance.m_GameIDData == null)
			{
				return string.Empty;
			}
			return SystemSaveManager.instance.m_GameIDData.password;
		}

		public static bool SetGamePassword(string password)
		{
			if (SystemSaveManager.instance == null)
			{
				return false;
			}
			SystemSaveManager.instance.m_GameIDData.password = password;
			SystemSaveManager.instance.m_gameDataDirty = true;
			return true;
		}

		public static bool IsUserIDValid()
		{
			if (SystemSaveManager.instance == null)
			{
				return false;
			}
			string gameID = SystemSaveManager.GetGameID();
			return !string.IsNullOrEmpty(gameID) && !(gameID == "0");
		}

		public static string GetTakeoverID()
		{
			if (SystemSaveManager.instance == null)
			{
				return string.Empty;
			}
			if (SystemSaveManager.instance.m_GameIDData == null)
			{
				return string.Empty;
			}
			return SystemSaveManager.instance.m_GameIDData.takeoverId;
		}

		public static bool SetTakeoverID(string id)
		{
			if (SystemSaveManager.instance == null)
			{
				return false;
			}
			SystemSaveManager.instance.m_GameIDData.takeoverId = id;
			SystemSaveManager.instance.m_gameDataDirty = true;
			return true;
		}

		public static string GetTakeoverPassword()
		{
			if (SystemSaveManager.instance == null)
			{
				return string.Empty;
			}
			if (SystemSaveManager.instance.m_GameIDData == null)
			{
				return string.Empty;
			}
			return SystemSaveManager.instance.m_GameIDData.takeoverPassword;
		}

		public static bool SetTakeoverPassword(string pass)
		{
			if (SystemSaveManager.instance == null)
			{
				return false;
			}
			SystemSaveManager.instance.m_GameIDData.takeoverPassword = pass;
			SystemSaveManager.instance.m_gameDataDirty = true;
			return true;
		}

		public static string GetCountryCode()
		{
			if (SystemSaveManager.instance == null)
			{
				return string.Empty;
			}
			return SystemSaveManager.instance.m_SystemData.country;
		}

		public static bool SetCountryCode(string countrycode)
		{
			if (SystemSaveManager.instance == null)
			{
				return false;
			}
			SystemSaveManager.instance.m_SystemData.country = countrycode;
			return true;
		}

		public static bool IsIAPMessage()
		{
			if (SystemSaveManager.instance == null)
			{
				return false;
			}
			bool result = false;
			if (SystemSaveManager.instance.m_SystemData != null && !string.IsNullOrEmpty(SystemSaveManager.instance.m_SystemData.country) && SystemSaveManager.instance.m_SystemData.iap == 0)
			{
				result = true;
			}
			return result;
		}

		public static void CheckIAPMessage()
		{
			if (SystemSaveManager.instance == null)
			{
				return;
			}
			if (SystemSaveManager.instance.m_SystemData != null && !string.IsNullOrEmpty(SystemSaveManager.instance.m_SystemData.country))
			{
				if (RegionManager.Instance.IsNeedIapMessage())
				{
					SystemSaveManager.instance.m_SystemData.iap = 0;
				}
				else
				{
					SystemSaveManager.instance.m_SystemData.iap = 2;
				}
				SystemSaveManager.Save();
			}
		}

		public static void SetIAPMessageAlreadyRead()
		{
			if (SystemSaveManager.instance == null)
			{
				return;
			}
			if (SystemSaveManager.instance.m_SystemData != null && !string.IsNullOrEmpty(SystemSaveManager.instance.m_SystemData.country))
			{
				SystemSaveManager.instance.m_SystemData.iap = 1;
				SystemSaveManager.Save();
			}
		}

		public static void Save()
		{
			if (SystemSaveManager.instance == null)
			{
				return;
			}
			SystemSaveManager.instance.SaveSystemData();
		}

		public static bool Load()
		{
			return !(SystemSaveManager.instance == null) && SystemSaveManager.instance.LoadSystemData();
		}

		private string getSavePath()
		{
			return Application.persistentDataPath;
		}

		private string getDevelopmentPath()
		{
			return this.getSavePath() + "/../../SonicRunnerss";
		}

		private string GetHashData(string textdata)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(textdata);
			SHA256 sHA = new SHA256CryptoServiceProvider();
			byte[] array = sHA.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.AppendFormat("{0:X2}", array[i]);
			}
			sHA.Clear();
			return stringBuilder.ToString();
		}

		private bool WriteFile(string path, string cnvText)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			using (Stream stream = File.Open(path + "/sfrn.game", FileMode.Create))
			{
				using (StreamWriter streamWriter = new StreamWriter(stream))
				{
					try
					{
						string hashData = this.GetHashData(cnvText);
						streamWriter.Write(hashData + "\n");
						streamWriter.Write(cnvText);
						this.m_errorcode = SystemSaveManager.ErrorCode.NO_ERROR;
					}
					catch (Exception ex)
					{
						global::Debug.Log("WriteFile Error:" + ex.Message);
						this.m_errorcode = SystemSaveManager.ErrorCode.FILE_CANNOT_OPEN;
					}
				}
			}
			return this.m_errorcode == SystemSaveManager.ErrorCode.NO_ERROR;
		}

		public void SaveSystemData()
		{
			if (this.m_gameDataDirty)
			{
				this.SaveGameIDDataToPlayerPrefs();
				this.m_gameDataDirty = false;
			}
			XmlDocument xmlDocument = this.CreateXmlData();
			string cnvText = AESCrypt.Encrypt(xmlDocument.InnerXml);
			this.WriteFile(this.getSavePath(), cnvText);
		}

		public bool LoadSystemData()
		{
			Stream stream = null;
			try
			{
				stream = File.Open(this.getSavePath() + "/sfrn.game", FileMode.Open);
			}
			catch
			{
				this.m_errorcode = SystemSaveManager.ErrorCode.FILE_NOT_EXIST;
				if (stream != null)
				{
					stream.Dispose();
				}
				return false;
			}
			if (stream != null)
			{
				using (StreamReader streamReader = new StreamReader(stream))
				{
					string text = streamReader.ReadLine();
					string text2 = streamReader.ReadToEnd();
					string hashData = this.GetHashData(text2);
					if (!text.Equals(hashData))
					{
						global::Debug.Log("Data is Invalid.");
						this.m_errorcode = SystemSaveManager.ErrorCode.DATA_INVALID;
					}
					else
					{
						string data = AESCrypt.Decrypt(text2);
						this.ParseXmlData(data);
						this.m_errorcode = SystemSaveManager.ErrorCode.NO_ERROR;
					}
				}
				stream.Close();
				stream = null;
			}
			else
			{
				this.m_errorcode = SystemSaveManager.ErrorCode.FILE_NOT_EXIST;
			}
			return this.m_errorcode == SystemSaveManager.ErrorCode.NO_ERROR;
		}

		public bool ErrorOnStart()
		{
			return this.m_errorOnStart;
		}

		public bool SaveForStartingError()
		{
			this.m_gameDataDirty = false;
			this.SaveSystemData();
			this.m_errorOnStart = false;
			return this.m_errorcode == SystemSaveManager.ErrorCode.NO_ERROR;
		}

		public void CheckLightMode()
		{
			int width = Screen.width;
			int height = Screen.height;
			int num = width * height;
			if (SystemInfo.systemMemorySize > 512)
			{
				this.m_SystemData.lightMode = false;
				if (num > 2304000)
				{
					this.m_SystemData.highTexture = true;
				}
				else
				{
					this.m_SystemData.highTexture = false;
				}
			}
			else
			{
				this.m_SystemData.highTexture = false;
				this.m_SystemData.lightMode = true;
			}
		}

		private XmlDocument CreateXmlData()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("SystemData");
			xmlDocument.AppendChild(xmlElement);
			this.CleateElementInt(xmlDocument, xmlElement, "version", this.m_SystemData.version);
			this.CleateElementInt(xmlDocument, xmlElement, "bgmVolume", this.m_SystemData.bgmVolume);
			this.CleateElementInt(xmlDocument, xmlElement, "seVolume", this.m_SystemData.seVolume);
			this.CleateElementInt(xmlDocument, xmlElement, "achievementIncentiveCount", this.m_SystemData.achievementIncentiveCount);
			this.CleateElementInt(xmlDocument, xmlElement, "iap", this.m_SystemData.iap);
			this.CraeteElementBool(xmlDocument, xmlElement, "pushNotice", this.m_SystemData.pushNotice);
			this.CraeteElementBool(xmlDocument, xmlElement, "lightMode", this.m_SystemData.lightMode);
			this.CraeteElementBool(xmlDocument, xmlElement, "highTexture", this.m_SystemData.highTexture);
			this.CreateElementString(xmlDocument, xmlElement, "gameId", this.m_GameIDData.id);
			this.CreateElementString(xmlDocument, xmlElement, "password", this.m_GameIDData.password);
			this.CreateElementString(xmlDocument, xmlElement, "device", this.m_GameIDData.device);
			this.CreateElementString(xmlDocument, xmlElement, "takeoverId", this.m_GameIDData.takeoverId);
			this.CreateElementString(xmlDocument, xmlElement, "takeoverPassword", this.m_GameIDData.takeoverPassword);
			this.CreateElementString(xmlDocument, xmlElement, "noahId", this.m_SystemData.noahId);
			this.CreateElementString(xmlDocument, xmlElement, "purchasedRecipt", this.m_SystemData.purchasedRecipt);
			this.CreateElementString(xmlDocument, xmlElement, "country", this.m_SystemData.country);
			this.CreateElementString(xmlDocument, xmlElement, "facebookTime", this.m_SystemData.facebookTime);
			this.CreateElementString(xmlDocument, xmlElement, "gameStartTime", this.m_SystemData.gameStartTime);
			this.CreateElementUint(xmlDocument, xmlElement, "flags", this.m_SystemData.flags.to_ulong());
			this.CreateElementUint(xmlDocument, xmlElement, "itemTutorialFlags", this.m_SystemData.itemTutorialFlags.to_ulong());
			this.CreateElementUint(xmlDocument, xmlElement, "charaTutorialFlags", this.m_SystemData.charaTutorialFlags.to_ulong());
			this.CreateElementUint(xmlDocument, xmlElement, "actionTutorialFlags", this.m_SystemData.actionTutorialFlags.to_ulong());
			this.CreateElementUint(xmlDocument, xmlElement, "quickModeTutorialFlags", this.m_SystemData.quickModeTutorialFlags.to_ulong());
			this.CreateElementUint(xmlDocument, xmlElement, "pushNoticeFlags", this.m_SystemData.pushNoticeFlags.to_ulong());
			this.CreateElementString(xmlDocument, xmlElement, "deckData", this.m_SystemData.deckData);
			this.CleateElementInt(xmlDocument, xmlElement, "pictureShowEventId", this.m_SystemData.pictureShowEventId);
			this.CleateElementInt(xmlDocument, xmlElement, "pictureShowProgress", this.m_SystemData.pictureShowProgress);
			this.CleateElementInt(xmlDocument, xmlElement, "pictureShowEmergeRaidBossProgress", this.m_SystemData.pictureShowEmergeRaidBossProgress);
			this.CleateElementInt(xmlDocument, xmlElement, "pictureShowRaidBossFirstBattle", this.m_SystemData.pictureShowRaidBossFirstBattle);
			this.CreateElementLong(xmlDocument, xmlElement, "currentRaidDrawIndex", this.m_SystemData.currentRaidDrawIndex);
			this.CraeteElementBool(xmlDocument, xmlElement, "raidEntryFlag", this.m_SystemData.raidEntryFlag);
			this.CleateElementInt(xmlDocument, xmlElement, "chaoSortType01", this.m_SystemData.chaoSortType01);
			this.CleateElementInt(xmlDocument, xmlElement, "chaoSortType02", this.m_SystemData.chaoSortType02);
			this.CleateElementInt(xmlDocument, xmlElement, "playCount", this.m_SystemData.playCount);
			this.CreateElementString(xmlDocument, xmlElement, "loginRankigTime", this.m_SystemData.loginRankigTime);
			this.CleateElementInt(xmlDocument, xmlElement, "playGamesCancelCount", this.m_SystemData.achievementCancelCount);
			XmlElement xmlElement2 = this.StartArray(xmlDocument, "fbFriends");
			for (int i = 0; i < this.m_SystemData.fbFriends.Count; i++)
			{
				string text = this.m_SystemData.fbFriends[i];
				if (text != null)
				{
					string name = "friend" + (i + 1).ToString();
					this.CreateElementString(xmlDocument, xmlElement2, name, text);
				}
			}
			this.EndArray(xmlElement, xmlElement2);
			return xmlDocument;
		}

		private void CleateElementInt(XmlDocument doc, XmlElement rootElement, string name, int value)
		{
			XmlElement xmlElement = doc.CreateElement(name);
			XmlText newChild = doc.CreateTextNode(value.ToString());
			xmlElement.AppendChild(newChild);
			rootElement.AppendChild(xmlElement);
		}

		private void CreateElementUint(XmlDocument doc, XmlElement rootElement, string name, uint value)
		{
			XmlElement xmlElement = doc.CreateElement(name);
			XmlText newChild = doc.CreateTextNode(value.ToString());
			xmlElement.AppendChild(newChild);
			rootElement.AppendChild(xmlElement);
		}

		private void CreateElementLong(XmlDocument doc, XmlElement rootElement, string name, long value)
		{
			XmlElement xmlElement = doc.CreateElement(name);
			XmlText newChild = doc.CreateTextNode(value.ToString());
			xmlElement.AppendChild(newChild);
			rootElement.AppendChild(xmlElement);
		}

		private void CreateElementString(XmlDocument doc, XmlElement rootElement, string name, string value)
		{
			XmlElement xmlElement = doc.CreateElement(name);
			string text = value;
			if (value == null || value.Length == 0)
			{
				text = string.Empty;
			}
			XmlText newChild = doc.CreateTextNode(text);
			xmlElement.AppendChild(newChild);
			rootElement.AppendChild(xmlElement);
		}

		private void CraeteElementBool(XmlDocument doc, XmlElement rootElement, string name, bool value)
		{
			XmlElement xmlElement = doc.CreateElement(name);
			XmlText newChild = doc.CreateTextNode((!value) ? "false" : "true");
			xmlElement.AppendChild(newChild);
			rootElement.AppendChild(xmlElement);
		}

		private XmlElement StartArray(XmlDocument doc, string name)
		{
			return doc.CreateElement(name);
		}

		private void EndArray(XmlElement parentElement, XmlElement arrayRoot)
		{
			parentElement.AppendChild(arrayRoot);
		}

		private bool ParseXmlData(string data)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(data);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("SystemData");
			if (xmlNode == null)
			{
				return false;
			}
			this.m_SystemData.Init();
			this.m_GameIDData.Init();
			this.m_SystemData.version = this.GetIntByXml(xmlNode, "version");
			this.m_SystemData.bgmVolume = this.GetIntByXml(xmlNode, "bgmVolume");
			this.m_SystemData.seVolume = this.GetIntByXml(xmlNode, "seVolume");
			this.m_SystemData.achievementIncentiveCount = this.GetIntByXml(xmlNode, "achievementIncentiveCount");
			this.m_SystemData.iap = this.GetIntByXml(xmlNode, "iap");
			this.m_SystemData.pushNotice = this.GetBoolByXml(xmlNode, "pushNotice");
			this.m_SystemData.lightMode = this.GetBoolByXml(xmlNode, "lightMode");
			this.m_SystemData.highTexture = this.GetBoolByXml(xmlNode, "highTexture");
			this.m_GameIDData.id = this.GetStringByXml(xmlNode, "gameId");
			this.m_GameIDData.password = this.GetStringByXml(xmlNode, "password");
			this.m_GameIDData.device = this.GetStringByXml(xmlNode, "device");
			this.m_GameIDData.takeoverId = this.GetStringByXml(xmlNode, "takeoverId");
			this.m_GameIDData.takeoverPassword = this.GetStringByXml(xmlNode, "takeoverPassword");
			this.m_SystemData.noahId = this.GetStringByXml(xmlNode, "noahId");
			this.m_SystemData.purchasedRecipt = this.GetStringByXml(xmlNode, "purchasedRecipt");
			this.m_SystemData.country = this.GetStringByXml(xmlNode, "country");
			this.m_SystemData.facebookTime = this.GetStringByXml(xmlNode, "facebookTime");
			this.m_SystemData.gameStartTime = this.GetStringByXml(xmlNode, "gameStartTime");
			this.m_SystemData.flags = new Bitset32(this.GetUintByXml(xmlNode, "flags"));
			this.m_SystemData.itemTutorialFlags = new Bitset32(this.GetUintByXml(xmlNode, "itemTutorialFlags"));
			this.m_SystemData.charaTutorialFlags = new Bitset32(this.GetUintByXml(xmlNode, "charaTutorialFlags"));
			this.m_SystemData.actionTutorialFlags = new Bitset32(this.GetUintByXml(xmlNode, "actionTutorialFlags"));
			this.m_SystemData.quickModeTutorialFlags = new Bitset32(this.GetUintByXml(xmlNode, "quickModeTutorialFlags"));
			this.m_SystemData.pushNoticeFlags = new Bitset32(this.GetUintByXml(xmlNode, "pushNoticeFlags"));
			this.m_SystemData.deckData = this.GetStringByXml(xmlNode, "deckData");
			this.m_SystemData.pictureShowEventId = this.GetIntByXml(xmlNode, "pictureShowEventId");
			this.m_SystemData.pictureShowProgress = this.GetIntByXml(xmlNode, "pictureShowProgress");
			this.m_SystemData.pictureShowEmergeRaidBossProgress = this.GetIntByXml(xmlNode, "pictureShowEmergeRaidBossProgress");
			this.m_SystemData.pictureShowRaidBossFirstBattle = this.GetIntByXml(xmlNode, "pictureShowRaidBossFirstBattle");
			this.m_SystemData.currentRaidDrawIndex = (long)this.GetIntByXml(xmlNode, "currentRaidDrawIndex");
			this.m_SystemData.raidEntryFlag = this.GetBoolByXml(xmlNode, "raidEntryFlag");
			this.m_SystemData.chaoSortType01 = this.GetIntByXml(xmlNode, "chaoSortType01");
			this.m_SystemData.chaoSortType02 = this.GetIntByXml(xmlNode, "chaoSortType02");
			this.m_SystemData.playCount = this.GetIntByXml(xmlNode, "playCount");
			this.m_SystemData.loginRankigTime = this.GetStringByXml(xmlNode, "loginRankigTime");
			this.m_SystemData.achievementCancelCount = this.GetIntByXml(xmlNode, "playGamesCancelCount");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("fbFriends");
			if (xmlNode2 != null)
			{
				int num = 0;
				while (true)
				{
					string name = "friend" + (num + 1).ToString();
					string stringByXml = this.GetStringByXml(xmlNode2, name);
					if (stringByXml == null)
					{
						break;
					}
					this.m_SystemData.fbFriends.Add(stringByXml);
					num++;
				}
			}
			if (!this.m_SystemData.CheckDeck())
			{
				this.m_SystemData.deckData = SystemData.DeckAllDefalut();
			}
			return true;
		}

		private int GetIntByXml(XmlNode rootNode, string name)
		{
			XmlNode xmlNode = rootNode.SelectSingleNode(name);
			if (xmlNode != null)
			{
				string innerText = xmlNode.InnerText;
				if (innerText != null)
				{
					return int.Parse(innerText);
				}
			}
			return 0;
		}

		private uint GetUintByXml(XmlNode rootNode, string name)
		{
			XmlNode xmlNode = rootNode.SelectSingleNode(name);
			if (xmlNode != null)
			{
				string innerText = xmlNode.InnerText;
				if (innerText != null)
				{
					return uint.Parse(innerText);
				}
			}
			return 0u;
		}

		private string GetStringByXml(XmlNode rootNode, string name)
		{
			XmlNode xmlNode = rootNode.SelectSingleNode(name);
			if (xmlNode != null)
			{
				return xmlNode.InnerText;
			}
			return null;
		}

		private bool GetBoolByXml(XmlNode rootNode, string name)
		{
			XmlNode xmlNode = rootNode.SelectSingleNode(name);
			if (xmlNode != null)
			{
				string innerText = xmlNode.InnerText;
				if (innerText != null)
				{
					return innerText.Equals("true");
				}
			}
			return false;
		}

		public bool SystemFileCheck()
		{
			return File.Exists(this.getSavePath() + "/sfrn.game");
		}

		public void DeleteSystemFile()
		{
			if (this.SystemFileCheck())
			{
				File.Delete(this.getSavePath() + "/sfrn.game");
			}
			this.m_SystemData.Init();
			this.m_GameIDData.Init();
			this.SaveGameIDDataToPlayerPrefs();
		}

		public SystemSaveManager.ErrorCode GetErrorCode()
		{
			return this.m_errorcode;
		}

		public void SaveGameIDDataToPlayerPrefs()
		{
			if (this.m_GameIDData == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.m_GameIDData.id) && !this.m_GameIDData.id.Equals("0"))
			{
				string value = AESCrypt.Encrypt(this.m_GameIDData.id);
				PlayerPrefs.SetString("aa7329ab4330306fbdd6dbe9b85c96be", value);
			}
			else
			{
				PlayerPrefs.DeleteKey("aa7329ab4330306fbdd6dbe9b85c96be");
			}
			if (!string.IsNullOrEmpty(this.m_GameIDData.password))
			{
				string value2 = AESCrypt.Encrypt(this.m_GameIDData.password);
				PlayerPrefs.SetString("48521cd1266052bfc25718720e91fa83", value2);
			}
			else
			{
				PlayerPrefs.DeleteKey("48521cd1266052bfc25718720e91fa83");
			}
			PlayerPrefs.Save();
		}

		public bool GetGameIDDataFromPlayerPrefs()
		{
			if (this.m_GameIDData == null)
			{
				return false;
			}
			string @string = PlayerPrefs.GetString("aa7329ab4330306fbdd6dbe9b85c96be");
			if (!string.IsNullOrEmpty(@string))
			{
				this.m_GameIDData.id = AESCrypt.Decrypt(@string);
			}
			else
			{
				this.m_GameIDData.id = "0";
			}
			string string2 = PlayerPrefs.GetString("48521cd1266052bfc25718720e91fa83");
			if (!string.IsNullOrEmpty(string2))
			{
				this.m_GameIDData.password = AESCrypt.Decrypt(string2);
			}
			else
			{
				this.m_GameIDData.password = string.Empty;
			}
			return true;
		}

		public bool CheckCmSkipCount()
		{
			bool result = false;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				ServerSettingState settingState = ServerInterface.SettingState;
				if (settingState != null && this.m_SystemData.playCount >= settingState.m_cmSkipCount)
				{
					result = true;
				}
			}
			return result;
		}

		public void AddPlayCount()
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				ServerSettingState settingState = ServerInterface.SettingState;
				if (settingState != null)
				{
					int cmSkipCount = settingState.m_cmSkipCount;
					int num = this.m_SystemData.playCount;
					if (num < cmSkipCount)
					{
						num++;
						this.m_SystemData.playCount = num;
						global::Debug.Log(string.Concat(new object[]
						{
							"SystemSaveManager:AddPlayCount >>> ",
							num,
							"/",
							cmSkipCount
						}));
						SystemSaveManager.Save();
					}
				}
			}
		}

		private bool CheckInstance()
		{
			if (SystemSaveManager.instance == null)
			{
				SystemSaveManager.instance = this;
				return true;
			}
			if (this == SystemSaveManager.Instance)
			{
				return true;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return false;
		}
	}
}
