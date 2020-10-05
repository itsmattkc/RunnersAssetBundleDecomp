using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class StreamingDataLoader : MonoBehaviour
{
	private class Info
	{
		public string url;

		public string path;

		public bool downloaded;
	}

	private enum State
	{
		Init,
		WaitLoadServerKey,
		Idle,
		LoadReady,
		Loading,
		PrepareEnd,
		End
	}

	private sealed class _DownloadDatas_c__IteratorC5 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal bool _saveFlag___0;

		internal StreamingDataLoader.Info _info___1;

		internal string _fileName___2;

		internal string _serverKey___3;

		internal GameObject returnObject;

		internal int _PC;

		internal object _current;

		internal GameObject ___returnObject;

		internal StreamingDataLoader __f__this;

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
				this.__f__this.DebugDrawList("local server", "DownloadDatas START");
				this._current = null;
				this._PC = 1;
				return true;
			case 1u:
				this._saveFlag___0 = false;
				break;
			case 2u:
				this._info___1.downloaded = !this.__f__this.m_error;
				if (this.__f__this.m_error)
				{
					if (this.returnObject != null)
					{
						this.returnObject.SendMessage("StreamingDataLoad_Failed", SendMessageOptions.DontRequireReceiver);
					}
					GC.Collect();
					goto IL_22A;
				}
				this.__f__this.DebugDraw("Load End");
				this.__f__this.SetKey(ref this.__f__this.m_localKeyDataList, this._fileName___2, this._serverKey___3);
				GC.Collect();
				this.__f__this.m_loadEndCount++;
				break;
			default:
				return false;
			}
			while (this.__f__this.m_loadEndCount < this.__f__this.m_downloadList.Count)
			{
				this._info___1 = this.__f__this.m_downloadList[this.__f__this.m_loadEndCount];
				if (this._info___1 != null)
				{
					this._fileName___2 = Path.GetFileName(this._info___1.path);
					this._serverKey___3 = this.__f__this.GetKey(this.__f__this.m_serverKeyDataList, this._fileName___2);
					if (this.__f__this.IsNeedToLoad(this._info___1.path, this._fileName___2, this._serverKey___3))
					{
						this.__f__this.DebugDraw("Load !!");
						this.__f__this.m_error = false;
						this._current = this.__f__this.StartCoroutine(this.__f__this.UserInstallFile(this._info___1.url, this._info___1.path));
						this._PC = 2;
						return true;
					}
					this.__f__this.DebugDraw("No Load");
					this.__f__this.m_loadEndCount++;
				}
			}
			IL_22A:
			this.__f__this.SaveKeyData(SoundManager.GetDownloadedDataPath() + "StreamingDataList.txt", this.__f__this.m_localKeyDataList);
			if (!this.__f__this.m_error)
			{
				this.__f__this.DebugDrawList("local server", "DownloadDatas END");
				this.__f__this.m_downloadList.Clear();
				this.__f__this.m_state = StreamingDataLoader.State.PrepareEnd;
			}
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

	private sealed class _UserInstallFile_c__IteratorC6 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string url;

		internal string path;

		internal WWWRequest _request___0;

		internal float _startTime___1;

		internal float _spendTime___2;

		internal byte[] _rowdata___3;

		internal Stream _stream___4;

		internal Exception _ex___5;

		internal int _PC;

		internal object _current;

		internal string ___url;

		internal string ___path;

		internal StreamingDataLoader __f__this;

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
				if (this.__f__this.m_checkTime)
				{
					global::Debug.Log("LS:start install URL: " + this.url + " path:" + this.path);
				}
				this._request___0 = new WWWRequest(this.url, false);
				this._request___0.SetConnectTime(WWWRequest.DefaultConnectTime + WWWRequest.DefaultConnectTime * (float)this.__f__this.m_installFailedCount);
				break;
			case 1u:
				this._spendTime___2 = Time.realtimeSinceStartup - this._startTime___1;
				if (this._spendTime___2 <= 0.1f)
				{
					goto IL_DC;
				}
				break;
			default:
				return false;
			}
			if (!this._request___0.IsEnd())
			{
				this._request___0.Update();
				if (!this._request___0.IsTimeOut())
				{
					this._startTime___1 = Time.realtimeSinceStartup;
					this._spendTime___2 = 0f;
					goto IL_DC;
				}
				this._request___0.Cancel();
			}
			global::Debug.Log("UserInstallFile End. ");
			if (this._request___0.IsTimeOut())
			{
				this.__f__this.m_error = true;
				global::Debug.LogError("UserInstallFile TimeOut. ");
			}
			else if (this._request___0.GetError() != null)
			{
				this.__f__this.m_error = true;
				global::Debug.LogError("UserInstallFile Error. " + this._request___0.GetError());
			}
			else
			{
				this._rowdata___3 = this._request___0.GetResult();
				if (this._rowdata___3 != null)
				{
					this._stream___4 = File.Open(this.path, FileMode.Create);
					try
					{
						this._stream___4.Write(this._rowdata___3, 0, this._request___0.GetResultSize());
					}
					catch (Exception ex)
					{
						this._ex___5 = ex;
						this.__f__this.m_error = true;
						global::Debug.Log("UserInstallFile Write Error:" + this._ex___5.Message);
					}
					finally
					{
						if (this._stream___4 != null)
						{
							((IDisposable)this._stream___4).Dispose();
						}
					}
				}
			}
			this._request___0.Remove();
			this._PC = -1;
			return false;
			IL_DC:
			this._current = null;
			this._PC = 1;
			return true;
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

	private sealed class _LoadURLKeyData_c__IteratorC7 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string url;

		internal WWWRequest _request___0;

		internal float _startTime___1;

		internal float _spendTime___2;

		internal GameObject returnObject;

		internal string _resultText___3;

		internal int _PC;

		internal object _current;

		internal string ___url;

		internal GameObject ___returnObject;

		internal StreamingDataLoader __f__this;

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
				this.__f__this.DebugDraw("LoadURLKeyData url=" + this.url);
				this._request___0 = new WWWRequest(this.url, false);
				goto IL_C9;
			case 1u:
				this._spendTime___2 = Time.realtimeSinceStartup - this._startTime___1;
				if (this._spendTime___2 > 0.1f)
				{
					goto IL_C9;
				}
				break;
			default:
				return false;
			}
			IL_94:
			this._current = null;
			this._PC = 1;
			return true;
			IL_C9:
			if (!this._request___0.IsEnd())
			{
				this._request___0.Update();
				if (!this._request___0.IsTimeOut())
				{
					this._startTime___1 = Time.realtimeSinceStartup;
					this._spendTime___2 = 0f;
					goto IL_94;
				}
				this._request___0.Cancel();
			}
			global::Debug.Log("LoadURLKeyData End. ");
			if (this._request___0.IsTimeOut())
			{
				global::Debug.LogError("LoadURLKeyData TimeOut. ");
				if (this.returnObject != null)
				{
					this.returnObject.SendMessage("StreamingDataLoad_Failed", SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (this._request___0.GetError() != null)
			{
				global::Debug.LogError("LoadURLKeyData Error. " + this._request___0.GetError());
				if (this.returnObject != null)
				{
					this.returnObject.SendMessage("StreamingDataLoad_Failed", SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				try
				{
					this._resultText___3 = this._request___0.GetResultString();
					if (this._resultText___3 != null)
					{
						this.__f__this.DebugDraw("Draw WWW Text.\n" + this._resultText___3);
						this.__f__this.AddKeyData(this._resultText___3, ref this.__f__this.m_serverKeyDataList);
						this.__f__this.DebugDrawList("server", "LoadURLKeyData");
					}
					else
					{
						global::Debug.LogWarning("text load error www.text == null " + this.url);
					}
				}
				catch
				{
					global::Debug.LogWarning("error www.text.get " + this.url);
				}
				if (this.returnObject != null)
				{
					this.returnObject.SendMessage("StreamingDataLoad_Succeed", SendMessageOptions.DontRequireReceiver);
				}
				this.__f__this.m_state = StreamingDataLoader.State.Idle;
			}
			this._request___0.Remove();
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

	private const string DATALIST_NAME = "StreamingDataList.txt";

	private bool m_debugInfo;

	private bool m_checkTime = true;

	private List<StreamingKeyData> m_localKeyDataList = new List<StreamingKeyData>();

	private List<StreamingKeyData> m_serverKeyDataList = new List<StreamingKeyData>();

	private bool m_error;

	private StreamingDataLoader.State m_state;

	private List<StreamingDataLoader.Info> m_downloadList = new List<StreamingDataLoader.Info>();

	private int m_loadEndCount;

	private int m_installFailedCount;

	private GameObject m_returnObject;

	private static StreamingDataLoader instance;

	public bool Loaded
	{
		get
		{
			return this.m_state == StreamingDataLoader.State.End;
		}
	}

	public int NumInLoadList
	{
		get
		{
			return this.m_downloadList.Count;
		}
	}

	public int NumLoaded
	{
		get
		{
			return this.m_loadEndCount;
		}
	}

	public static StreamingDataLoader Instance
	{
		get
		{
			return StreamingDataLoader.instance;
		}
	}

	public void Initialize(GameObject returnObject)
	{
		this.m_localKeyDataList.Clear();
		this.m_serverKeyDataList.Clear();
		this.LoadKeyData(SoundManager.GetDownloadedDataPath() + "StreamingDataList.txt", ref this.m_localKeyDataList);
		this.LoadServerKey(returnObject);
		this.m_downloadList.Clear();
		this.m_loadEndCount = 0;
	}

	public void LoadServerKey(GameObject returnObject)
	{
		this.m_returnObject = returnObject;
		this.m_state = StreamingDataLoader.State.Init;
	}

	public bool IsEnableDownlad()
	{
		return this.m_state != StreamingDataLoader.State.Init && this.m_state != StreamingDataLoader.State.WaitLoadServerKey;
	}

	public void AddFileIfNotDownloaded(string url, string path)
	{
		string fileName = Path.GetFileName(path);
		string key = this.GetKey(this.m_serverKeyDataList, fileName);
		if (this.IsNeedToLoad(path, fileName, key))
		{
			StreamingDataLoader.Info info = new StreamingDataLoader.Info();
			info.url = url;
			info.path = path;
			info.downloaded = false;
			this.m_downloadList.Add(info);
		}
	}

	public void StartDownload(int tryCount, GameObject returnObject)
	{
		this.m_installFailedCount = tryCount;
		if (this.IsEnableDownlad())
		{
			this.DeleteLocalGarbage();
			this.m_returnObject = returnObject;
			this.m_state = StreamingDataLoader.State.LoadReady;
			this.m_loadEndCount = 0;
		}
	}

	public void GetLoadList(ref List<string> getData)
	{
		string text = "GetLoadList \n";
		foreach (StreamingKeyData current in this.m_serverKeyDataList)
		{
			getData.Add(current.m_name);
			text = text + current.m_name + "\n";
		}
		this.DebugDraw(text);
	}

	private IEnumerator DownloadDatas(GameObject returnObject)
	{
		StreamingDataLoader._DownloadDatas_c__IteratorC5 _DownloadDatas_c__IteratorC = new StreamingDataLoader._DownloadDatas_c__IteratorC5();
		_DownloadDatas_c__IteratorC.returnObject = returnObject;
		_DownloadDatas_c__IteratorC.___returnObject = returnObject;
		_DownloadDatas_c__IteratorC.__f__this = this;
		return _DownloadDatas_c__IteratorC;
	}

	private IEnumerator UserInstallFile(string url, string path)
	{
		StreamingDataLoader._UserInstallFile_c__IteratorC6 _UserInstallFile_c__IteratorC = new StreamingDataLoader._UserInstallFile_c__IteratorC6();
		_UserInstallFile_c__IteratorC.url = url;
		_UserInstallFile_c__IteratorC.path = path;
		_UserInstallFile_c__IteratorC.___url = url;
		_UserInstallFile_c__IteratorC.___path = path;
		_UserInstallFile_c__IteratorC.__f__this = this;
		return _UserInstallFile_c__IteratorC;
	}

	private void AddKeyData(string text, ref List<StreamingKeyData> outData)
	{
		string[] array = text.Split(new char[]
		{
			'\n'
		});
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text2 = array2[i];
			string[] array3 = text2.Split(new char[]
			{
				','
			});
			if (array3.Length == 2)
			{
				string name = array3[0].Trim();
				string key = array3[1].Trim();
				outData.Add(new StreamingKeyData(name, key));
			}
		}
	}

	private bool LoadKeyData(string filePath, ref List<StreamingKeyData> outData)
	{
		this.DebugDraw("LoadKeyData filePath=" + filePath);
		if (File.Exists(filePath))
		{
			Stream stream = null;
			try
			{
				stream = File.Open(filePath, FileMode.Open);
				using (StreamReader streamReader = new StreamReader(stream))
				{
					string text = streamReader.ReadToEnd();
					this.AddKeyData(text, ref outData);
				}
			}
			catch
			{
				bool result = false;
				return result;
			}
			if (stream != null)
			{
				stream.Close();
				this.DebugDrawList("local", "LoadKeyData");
				return true;
			}
			return false;
		}
		return false;
	}

	private IEnumerator LoadURLKeyData(string url, GameObject returnObject)
	{
		StreamingDataLoader._LoadURLKeyData_c__IteratorC7 _LoadURLKeyData_c__IteratorC = new StreamingDataLoader._LoadURLKeyData_c__IteratorC7();
		_LoadURLKeyData_c__IteratorC.url = url;
		_LoadURLKeyData_c__IteratorC.returnObject = returnObject;
		_LoadURLKeyData_c__IteratorC.___url = url;
		_LoadURLKeyData_c__IteratorC.___returnObject = returnObject;
		_LoadURLKeyData_c__IteratorC.__f__this = this;
		return _LoadURLKeyData_c__IteratorC;
	}

	private bool SaveKeyData(string filePath, List<StreamingKeyData> inData)
	{
		try
		{
			using (StreamWriter streamWriter = new StreamWriter(filePath, false, Encoding.GetEncoding("utf-8")))
			{
				streamWriter.NewLine = "\n";
				foreach (StreamingKeyData current in inData)
				{
					streamWriter.WriteLine(current.m_name + "," + current.m_key);
				}
				streamWriter.Close();
			}
		}
		catch
		{
			return false;
		}
		return true;
	}

	private bool IsNeedToLoad(string path, string fileName, string serverKey)
	{
		if (!File.Exists(path))
		{
			return true;
		}
		this.DebugDraw("IsNeedToLoad fileName=" + fileName + " serverKey=" + serverKey);
		if (serverKey == string.Empty)
		{
			global::Debug.LogWarning("error : NOT serverKey");
			return false;
		}
		string key = this.GetKey(this.m_localKeyDataList, fileName);
		this.DebugDraw("IsNeedToLoad localKey=" + key);
		return !(key == serverKey);
	}

	private string GetKey(List<StreamingKeyData> keyList, string nameData)
	{
		foreach (StreamingKeyData current in keyList)
		{
			if (nameData == current.m_name)
			{
				return current.m_key;
			}
		}
		return string.Empty;
	}

	private void SetKey(ref List<StreamingKeyData> keyList, string nameData, string key)
	{
		foreach (StreamingKeyData current in keyList)
		{
			if (nameData == current.m_name)
			{
				current.m_key = key;
				return;
			}
		}
		keyList.Add(new StreamingKeyData(nameData, key));
	}

	private bool IsStreamingKeyData(List<StreamingKeyData> keyList, string filename)
	{
		foreach (StreamingKeyData current in keyList)
		{
			if (filename == current.m_name)
			{
				return true;
			}
		}
		return false;
	}

	private void DeleteLocalGarbage()
	{
		this.DebugDrawList("local server", "DeleteLocalGarbage START----");
		List<StreamingKeyData> list = new List<StreamingKeyData>();
		List<string> list2 = new List<string>();
		foreach (StreamingKeyData current in this.m_localKeyDataList)
		{
			if (!this.IsStreamingKeyData(this.m_serverKeyDataList, current.m_name))
			{
				this.DebugDraw("deleteFileList.Add=" + current.m_name);
				list2.Add(current.m_name);
			}
			else
			{
				list.Add(current);
			}
		}
		if (list2.Count > 0)
		{
			foreach (string current2 in list2)
			{
				string text = SoundManager.GetDownloadedDataPath() + current2;
				if (File.Exists(text))
				{
					this.DebugDraw("Delete=" + text);
					File.Delete(text);
				}
			}
			this.m_localKeyDataList.Clear();
			this.m_localKeyDataList.AddRange(list);
			this.SaveKeyData(SoundManager.GetDownloadedDataPath() + "StreamingDataList.txt", this.m_localKeyDataList);
			this.DebugDrawList("local", "DeleteLocalGarbage END---");
		}
	}

	private void DebugDrawList(string type, string msg)
	{
	}

	private void DebugDraw(string msg)
	{
	}

	protected void Awake()
	{
		this.m_checkTime = false;
		this.CheckInstance();
	}

	public static void Create()
	{
		if (StreamingDataLoader.instance == null)
		{
			GameObject gameObject = new GameObject("StreamingDataLoader");
			gameObject.AddComponent<StreamingDataLoader>();
		}
	}

	protected bool CheckInstance()
	{
		if (StreamingDataLoader.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			StreamingDataLoader.instance = this;
			return true;
		}
		if (this == StreamingDataLoader.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (this == StreamingDataLoader.instance)
		{
			StreamingDataLoader.instance = null;
		}
	}

	private void Update()
	{
		switch (this.m_state)
		{
		case StreamingDataLoader.State.Init:
			base.StartCoroutine(this.LoadURLKeyData(SoundManager.GetDownloadURL() + "StreamingDataList.txt", this.m_returnObject));
			this.m_state = StreamingDataLoader.State.WaitLoadServerKey;
			break;
		case StreamingDataLoader.State.LoadReady:
			base.StartCoroutine(this.DownloadDatas(this.m_returnObject));
			this.m_state = StreamingDataLoader.State.Loading;
			break;
		case StreamingDataLoader.State.PrepareEnd:
			if (this.m_returnObject != null)
			{
				this.m_returnObject.SendMessage("StreamingDataLoad_Succeed", SendMessageOptions.DontRequireReceiver);
			}
			this.m_state = StreamingDataLoader.State.End;
			break;
		}
	}
}
