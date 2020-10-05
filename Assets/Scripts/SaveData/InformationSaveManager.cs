using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using UnityEngine;

namespace SaveData
{
	public class InformationSaveManager : MonoBehaviour
	{
		public enum ErrorCode
		{
			NO_ERROR,
			FILE_NOT_EXIST,
			FILE_CANNOT_OPEN,
			DATA_INVALID
		}

		private const string INFORMATION_FILE_NAME = "ifrn";

		private const string EXTENSION = ".game";

		private const string XmlRootName = "InformationData";

		private InformationData m_informationData = new InformationData();

		private InformationSaveManager.ErrorCode m_errorcode;

		private static InformationSaveManager instance;

		public static InformationSaveManager Instance
		{
			get
			{
				if (InformationSaveManager.instance == null)
				{
					InformationSaveManager.instance = (UnityEngine.Object.FindObjectOfType(typeof(InformationSaveManager)) as InformationSaveManager);
				}
				return InformationSaveManager.instance;
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
			this.m_informationData.Init();
			if (!this.InformationFileCheck())
			{
				this.m_informationData.m_isDirty = true;
				this.SaveInformationData();
			}
			else
			{
				this.LoadInfomationData();
			}
		}

		public static InformationData GetInformationSaveData()
		{
			if (InformationSaveManager.instance == null)
			{
				return null;
			}
			return InformationSaveManager.instance.GetInformationData();
		}

		public InformationData GetInformationData()
		{
			return this.m_informationData;
		}

		private string getSavePath()
		{
			return Application.persistentDataPath;
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

		public void SaveInformationData()
		{
			if (!this.m_informationData.m_isDirty)
			{
				return;
			}
			string path = this.getSavePath() + "/ifrn.game";
			using (Stream stream = File.Open(path, FileMode.Create))
			{
				using (StreamWriter streamWriter = new StreamWriter(stream))
				{
					try
					{
						XmlDocument xmlDocument = this.CreateXmlData();
						string text = AESCrypt.Encrypt(xmlDocument.InnerXml);
						string hashData = this.GetHashData(text);
						streamWriter.Write(hashData + "\n");
						streamWriter.Write(text);
						this.m_errorcode = InformationSaveManager.ErrorCode.NO_ERROR;
					}
					catch
					{
						this.m_errorcode = InformationSaveManager.ErrorCode.FILE_CANNOT_OPEN;
					}
				}
			}
		}

		public void LoadInfomationData()
		{
			Stream stream = null;
			try
			{
				stream = File.Open(this.getSavePath() + "/ifrn.game", FileMode.Open);
			}
			catch
			{
				this.m_errorcode = InformationSaveManager.ErrorCode.FILE_NOT_EXIST;
				if (stream != null)
				{
					stream.Dispose();
				}
				return;
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
						this.m_errorcode = InformationSaveManager.ErrorCode.DATA_INVALID;
					}
					else
					{
						string streamData = AESCrypt.Decrypt(text2);
						this.ParseXmlData(streamData);
						this.m_errorcode = InformationSaveManager.ErrorCode.NO_ERROR;
					}
				}
				stream.Close();
				stream = null;
			}
			else
			{
				this.m_errorcode = InformationSaveManager.ErrorCode.FILE_NOT_EXIST;
			}
		}

		private XmlDocument CreateXmlData()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("InformationData");
			xmlDocument.AppendChild(xmlElement);
			int num = this.m_informationData.DataCount();
			for (int i = 0; i < num; i++)
			{
				this.CreateElementString(xmlDocument, xmlElement, "string", this.m_informationData.TextArray[i]);
			}
			return xmlDocument;
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

		private bool ParseXmlData(string streamData)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(streamData);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("InformationData");
			if (xmlNode == null)
			{
				return false;
			}
			XmlNodeList xmlNodeList = xmlNode.SelectNodes("string");
			if (xmlNodeList == null)
			{
				return false;
			}
			this.m_informationData.Init();
			int count = xmlNodeList.Count;
			for (int i = 0; i < count; i++)
			{
				this.m_informationData.TextArray.Add(xmlNodeList.Item(i).InnerText);
			}
			int num = this.m_informationData.DataCount();
			InformationImageManager informationImageManager = InformationImageManager.Instance;
			for (int j = 0; j < num; j++)
			{
				string data = this.m_informationData.GetData(j, InformationData.DataType.ID);
				if (data != InformationData.INVALID_PARAM && long.Parse(data) != (long)NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID && long.Parse(data) != (long)NetNoticeItem.OPERATORINFO_QUICKRANKINGRESULT_ID && long.Parse(data) != (long)NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID)
				{
					string text = this.m_informationData.GetData(j, InformationData.DataType.ADD_INFO);
					if (text.Length > 11)
					{
						text = "-1";
					}
					DateTime localDateTime = NetUtil.GetLocalDateTime(long.Parse(text));
					DateTime localDateTime2 = NetUtil.GetLocalDateTime((long)NetUtil.GetCurrentUnixTime());
					if (localDateTime2 > localDateTime)
					{
						if (informationImageManager != null)
						{
							string data2 = this.m_informationData.GetData(j, InformationData.DataType.IMAGE_ID);
							informationImageManager.DeleteImageData(data2);
						}
						this.m_informationData.Reset(j);
					}
				}
			}
			return true;
		}

		private string GetStringByXml(XmlNode rootNode)
		{
			XmlNode xmlNode = rootNode.SelectSingleNode("string");
			if (xmlNode != null)
			{
				return xmlNode.InnerText;
			}
			return null;
		}

		public bool InformationFileCheck()
		{
			return File.Exists(this.getSavePath() + "/ifrn.game");
		}

		public void DeleteInformationFile()
		{
			if (this.InformationFileCheck())
			{
				File.Delete(this.getSavePath() + "/ifrn.game");
			}
		}

		private InformationSaveManager.ErrorCode GetErrorCode()
		{
			return this.m_errorcode;
		}

		private void OnDestroy()
		{
			if (InformationSaveManager.instance == this)
			{
				InformationSaveManager.instance = null;
			}
		}

		private bool CheckInstance()
		{
			if (InformationSaveManager.instance == null)
			{
				InformationSaveManager.instance = this;
				return true;
			}
			if (this == InformationSaveManager.Instance)
			{
				return true;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return false;
		}
	}
}
