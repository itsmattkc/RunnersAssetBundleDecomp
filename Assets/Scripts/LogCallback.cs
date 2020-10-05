using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LogCallback : MonoBehaviour
{
	public class LogData
	{
		public string m_condition;

		public string m_stackTrace;

		public string m_type;

		public LogData(string cond, string stack, string type)
		{
			this.m_condition = cond;
			this.m_stackTrace = stack;
			this.m_type = type;
		}
	}

	private List<LogCallback.LogData> m_logData = new List<LogCallback.LogData>();

	private Vector2 m_scrollViewVector = Vector2.zero;

	private string m_innerText;

	public bool m_saveLogFile = true;

	public bool m_offOnEditor = true;

	private static LogCallback instance;

	public static LogCallback Instance
	{
		get
		{
			return LogCallback.instance;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject, 0.1f);
	}

	private void OnEnable()
	{
		if (this.m_saveLogFile)
		{
			Application.RegisterLogCallback(new Application.LogCallback(this.CallbackSaveLog));
		}
		else
		{
			Application.RegisterLogCallback(new Application.LogCallback(this.HandleLog));
		}
	}

	private void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	private void HandleLog(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Log && !condition.StartsWith("LS:"))
		{
			return;
		}
		LogCallback.LogData item = new LogCallback.LogData(condition, stackTrace, type.ToString());
		this.m_logData.Add(item);
		if (this.m_logData.Count > 10)
		{
			this.m_logData.Remove(this.m_logData[0]);
		}
		this.m_innerText = null;
		foreach (LogCallback.LogData current in this.m_logData)
		{
			string innerText = this.m_innerText;
			this.m_innerText = string.Concat(new string[]
			{
				innerText,
				"condition : ",
				current.m_condition,
				"\nstackTrace : ",
				current.m_stackTrace,
				"\ntype : ",
				current.m_type,
				"\n\n"
			});
		}
	}

	private void CallbackSaveLog(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Log && !condition.StartsWith("LS:"))
		{
			return;
		}
		string value = string.Concat(new object[]
		{
			"condition : ",
			condition,
			"\nstackTrace : ",
			stackTrace,
			"\ntype : ",
			type,
			"\n\n"
		});
		using (Stream stream = this.StreamOpen(true))
		{
			using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
			{
				try
				{
					streamWriter.Write(value);
					streamWriter.Close();
				}
				catch (Exception ex)
				{
					global::Debug.Log("Callback SaveLog Error:" + ex.Message);
				}
			}
		}
	}

	private void OnGUI()
	{
		if (this.m_innerText == null)
		{
			return;
		}
		this.m_scrollViewVector = GUI.BeginScrollView(new Rect(600f, 80f, 400f, 400f), this.m_scrollViewVector, new Rect(0f, 0f, 350f, 10000f));
		this.m_innerText = GUI.TextArea(new Rect(0f, 0f, 350f, 10000f), this.m_innerText);
		GUI.EndScrollView();
	}

	private Stream StreamOpen(bool append)
	{
		FileMode mode = (!append) ? FileMode.Create : FileMode.Append;
		return File.Open(Application.persistentDataPath + "/ErrorLog.log", mode);
	}

	protected bool CheckInstance()
	{
		if (LogCallback.instance == null)
		{
			LogCallback.instance = this;
			return true;
		}
		if (this == LogCallback.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (this == LogCallback.instance)
		{
			LogCallback.instance = null;
		}
	}
}
