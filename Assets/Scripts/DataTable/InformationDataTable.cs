using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Text;
using UnityEngine;

namespace DataTable
{
	public class InformationDataTable : MonoBehaviour
	{
		public enum Type
		{
			COPYRIGHT,
			CREDIT,
			SHOP_LEGAL,
			HELP,
			TERMS_OF_SERVICE,
			FB_FEED_PICTURE_ANDROID,
			FB_FEED_PICTURE_IOS,
			INSTALL_PAGE_ANDROID,
			INSTALL_PAGE_IOS,
			MAINTENANCE_PAGE,
			NUM
		}

		private sealed class _LoadURL_c__IteratorB : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal float _oldTime___0;

			internal string url;

			internal WWWRequest _request___1;

			internal float _startTime___2;

			internal float _spendTime___3;

			internal float _loadTime___4;

			internal string _resultText___5;

			internal string _text_data___6;

			internal XmlSerializer _serializer___7;

			internal StringReader _sr___8;

			internal int _PC;

			internal object _current;

			internal string ___url;

			internal InformationDataTable __f__this;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					InformationDataTable.m_isError = false;
					this._oldTime___0 = Time.realtimeSinceStartup;
					if (this.__f__this.m_checkTime)
					{
						global::Debug.Log("LS:start install URL: " + this.url);
					}
					this._request___1 = new WWWRequest(this.url, false);
					this._request___1.SetConnectTime(20f);
					goto IL_F4;
				case 1u:
					this._spendTime___3 = Time.realtimeSinceStartup - this._startTime___2;
					if (this._spendTime___3 > 0.1f)
					{
						goto IL_F4;
					}
					break;
				default:
					return false;
				}
				IL_BF:
				this._current = null;
				this._PC = 1;
				return true;
				IL_F4:
				if (!this._request___1.IsEnd())
				{
					this._request___1.Update();
					if (!this._request___1.IsTimeOut())
					{
						this._startTime___2 = Time.realtimeSinceStartup;
						this._spendTime___3 = 0f;
						goto IL_BF;
					}
					this._request___1.Cancel();
				}
				if (this.__f__this.m_checkTime)
				{
					this._loadTime___4 = Time.realtimeSinceStartup;
					global::Debug.Log("LS:Load File: " + this.url + " Time is " + (this._loadTime___4 - this._oldTime___0).ToString());
				}
				if (this._request___1.IsTimeOut())
				{
					global::Debug.LogError("LoadURLKeyData TimeOut. ");
					if (this.__f__this.m_returnObject != null)
					{
						this.__f__this.m_returnObject.SendMessage("InformationDataLoad_Failed", SendMessageOptions.DontRequireReceiver);
					}
				}
				else if (this._request___1.GetError() != null)
				{
					global::Debug.LogError("LoadURLKeyData Error. " + this._request___1.GetError());
					if (this.__f__this.m_returnObject != null)
					{
						this.__f__this.m_returnObject.SendMessage("InformationDataLoad_Failed", SendMessageOptions.DontRequireReceiver);
					}
				}
				else
				{
					try
					{
						this._resultText___5 = this._request___1.GetResultString();
						if (this._resultText___5 != null)
						{
							this._text_data___6 = AESCrypt.Decrypt(this._resultText___5);
							this._serializer___7 = new XmlSerializer(typeof(InformationData[]));
							this._sr___8 = new StringReader(this._text_data___6);
							InformationDataTable.m_infoDataTable = (InformationData[])this._serializer___7.Deserialize(this._sr___8);
						}
						else
						{
							global::Debug.LogWarning("text load error www.text == null " + this.url);
							InformationDataTable.m_isError = true;
						}
					}
					catch
					{
						global::Debug.LogWarning("error " + this.url);
						InformationDataTable.m_isError = true;
					}
					if (this.__f__this.m_returnObject != null)
					{
						this.__f__this.m_returnObject.SendMessage("InformationDataLoad_Succeed", SendMessageOptions.DontRequireReceiver);
					}
				}
				this._request___1.Remove();
				this._PC = -1;
				return false;
			}

			public void Dispose()
			{
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private bool m_checkTime = true;

		private static string[] TypeName = new string[]
		{
			"copyright",
			"credit",
			"shop_legal",
			"help",
			"terms_of_service",
			"fb_feed_picture_android",
			"fb_feed_picture_ios",
			"install_page_android",
			"install_page_ios",
			"maintenance_page"
		};

		private static InformationData[] m_infoDataTable;

		private GameObject m_returnObject;

		private static InformationDataTable s_instance = null;

		private static bool m_isError = false;

		public static InformationDataTable Instance
		{
			get
			{
				return InformationDataTable.s_instance;
			}
		}

		public bool Loaded
		{
			get
			{
				return InformationDataTable.m_infoDataTable != null;
			}
		}

		public static void Create()
		{
			if (InformationDataTable.s_instance == null)
			{
				GameObject gameObject = new GameObject("InformationDataTable");
				gameObject.AddComponent<InformationDataTable>();
			}
		}

		public void Initialize(GameObject returnObject)
		{
			this.m_checkTime = false;
			this.m_returnObject = returnObject;
			base.StartCoroutine(this.LoadURL(NetBaseUtil.InformationServerURL + "InformationDataTable.bytes"));
		}

		public bool isError()
		{
			return InformationDataTable.m_isError;
		}

		private void Awake()
		{
			if (InformationDataTable.s_instance == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				InformationDataTable.s_instance = this;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void OnDestroy()
		{
			if (InformationDataTable.s_instance == this)
			{
				InformationDataTable.s_instance = null;
				InformationDataTable.m_infoDataTable = null;
			}
		}

		private IEnumerator LoadURL(string url)
		{
			InformationDataTable._LoadURL_c__IteratorB _LoadURL_c__IteratorB = new InformationDataTable._LoadURL_c__IteratorB();
			_LoadURL_c__IteratorB.url = url;
			_LoadURL_c__IteratorB.___url = url;
			_LoadURL_c__IteratorB.__f__this = this;
			return _LoadURL_c__IteratorB;
		}

		public static InformationData[] GetDataTable()
		{
			return InformationDataTable.m_infoDataTable;
		}

		public static string GetUrl(InformationDataTable.Type type)
		{
			if (InformationDataTable.m_infoDataTable != null && type < (InformationDataTable.Type)InformationDataTable.TypeName.Length)
			{
				InformationData[] infoDataTable = InformationDataTable.m_infoDataTable;
				for (int i = 0; i < infoDataTable.Length; i++)
				{
					InformationData informationData = infoDataTable[i];
					if (informationData.tag == InformationDataTable.TypeName[(int)type] && informationData.sfx == TextUtility.GetSuffixe())
					{
						global::Debug.Log(string.Concat(new string[]
						{
							"GetUrl type=",
							type.ToString(),
							" sfx=",
							informationData.sfx,
							" tag=",
							informationData.tag,
							" url=",
							informationData.url
						}));
						return informationData.url;
					}
				}
			}
			return string.Empty;
		}
	}
}
