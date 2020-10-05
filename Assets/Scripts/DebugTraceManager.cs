using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugTraceManager : MonoBehaviour
{
	public enum TraceType
	{
		ALL,
		SERVER,
		ASSETBUNDLE,
		UI,
		GAME,
		NUM,
		BEGIN = 0,
		END = 4
	}

	public static readonly string[] TypeName = new string[]
	{
		"All",
		"Server",
		"AssetBundle",
		"UI",
		"Game"
	};

	private static DebugTraceManager m_instance = null;

	private List<DebugTrace>[] m_traceList = new List<DebugTrace>[5];

	private StringBuilder[] m_textList = new StringBuilder[5];

	private DebugTraceMenu m_menu;

	public static DebugTraceManager Instance
	{
		get
		{
			return DebugTraceManager.m_instance;
		}
		private set
		{
		}
	}

	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
	}

	public string GetTraceText(DebugTraceManager.TraceType type)
	{
		return this.m_textList[(int)type].ToString();
	}

	public void AddTrace(DebugTraceManager.TraceType type, DebugTrace trace)
	{
		if (type != DebugTraceManager.TraceType.ALL)
		{
			this.m_traceList[(int)type].Add(trace);
			this.m_textList[(int)type].Append("+" + trace.text + "\n");
		}
		this.m_traceList[0].Add(trace);
		this.m_textList[0].Append("+" + trace.text + "\n");
	}

	public void ClearTrace(DebugTraceManager.TraceType type)
	{
		if (type == DebugTraceManager.TraceType.ALL)
		{
			for (int i = 0; i < 5; i++)
			{
				List<DebugTrace> list = this.m_traceList[i];
				if (list != null)
				{
					list.Clear();
					this.m_textList[i].Length = 0;
				}
			}
		}
		else
		{
			List<DebugTrace> list2 = this.m_traceList[(int)type];
			if (list2 != null)
			{
				list2.Clear();
			}
			this.m_textList[(int)type].Length = 0;
		}
	}

	public bool IsTracing()
	{
		return this.m_menu != null && this.m_menu.currentState == DebugTraceMenu.State.ON;
	}

	private void Init()
	{
		for (int i = 0; i < 5; i++)
		{
			this.m_traceList[i] = new List<DebugTrace>();
			this.m_textList[i] = new StringBuilder();
			this.m_textList[i].Capacity = 1048576;
		}
		this.m_menu = base.gameObject.AddComponent<DebugTraceMenu>();
	}
}
