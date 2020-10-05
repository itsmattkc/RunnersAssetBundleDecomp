using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class StageDebugInformation : MonoBehaviour
{
	private enum StageDebugEditItem
	{
		RING_ADD_COUNT,
		ANIMAL_ADD_COUNT,
		DISTANCE_ADD,
		COMBO_ADD,
		SPCRYSTAL_ADD_COUNT,
		NUM
	}

	private PlayerInformation m_playerInformation;

	private StageBlockManager m_blockManager;

	private Rect m_window;

	private Rect m_window2;

	private Rect m_window3;

	private string[] m_window2text = new string[5];

	public bool m_showInformation;

	private static StageDebugInformation m_instance;

	public static StageDebugInformation Instance
	{
		get
		{
			return StageDebugInformation.m_instance;
		}
	}

	private void Awake()
	{
		if (!this.SetInstance())
		{
			return;
		}
		this.m_playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
		this.m_blockManager = GameObjectUtil.FindGameObjectComponentWithTag<StageBlockManager>("StageManager", "StageBlockManager");
		this.m_window = new Rect(10f, 160f, 250f, 120f);
		this.m_window2 = new Rect(10f, 290f, 280f, 150f);
		this.m_window3 = new Rect(300f, 160f, 280f, 260f);
		for (int i = 0; i < 5; i++)
		{
			this.m_window2text[i] = "0";
		}
		this.m_showInformation = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		if (StageDebugInformation.m_instance == this)
		{
			StageDebugInformation.m_instance = null;
		}
	}

	private void OnGUI()
	{
		if (this.m_showInformation)
		{
			this.m_window = GUI.Window(5, this.m_window, new GUI.WindowFunction(this.WindowFunction), "StageInformation");
			this.m_window2 = GUI.Window(7, this.m_window2, new GUI.WindowFunction(this.WindowFunction2), "StageEdit");
			this.m_window3 = GUI.Window(6, this.m_window3, new GUI.WindowFunction(this.WindowFunction3), "FriendSignInformation");
			if (GUI.Button(new Rect(10f, 120f, 150f, 30f), "Close Info"))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else if (GUI.Button(new Rect(10f, 120f, 150f, 30f), "Show Info"))
		{
			this.m_showInformation = true;
		}
	}

	private void WindowFunction(int windowID)
	{
		string text = string.Empty;
		Vector3 position = this.m_playerInformation.Position;
		string text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			"Position : ",
			position.x.ToString("F2"),
			" ",
			position.y.ToString("F2"),
			" ",
			position.z.ToString("F2"),
			"\n"
		});
		StageBlockManager.StageBlockInfo currenBlockInfo = this.m_blockManager.GetCurrenBlockInfo();
		if (currenBlockInfo != null)
		{
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"Block : ",
				currenBlockInfo.m_blockNo.ToString(),
				"  Layer : ",
				currenBlockInfo.m_layerNo.ToString(),
				"\n"
			});
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"Bl Length : ",
				currenBlockInfo.m_totalLength.ToString("F2"),
				"  Distance:",
				this.m_blockManager.GetBlockLocalDistance().ToString("F2"),
				"\n"
			});
			Vector3 blockLocalPosition = this.m_blockManager.GetBlockLocalPosition(position);
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"Bl LocalPos : ",
				blockLocalPosition.x.ToString("F2"),
				" ",
				blockLocalPosition.y.ToString("F2"),
				" ",
				blockLocalPosition.z.ToString("F2"),
				"\n"
			});
		}
		Vector3 sideViewPathPos = this.m_playerInformation.SideViewPathPos;
		text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			"Path     : ",
			sideViewPathPos.x.ToString("F2"),
			" ",
			sideViewPathPos.y.ToString("F2"),
			" ",
			sideViewPathPos.z.ToString("F2"),
			"\n"
		});
		GUIContent gUIContent = new GUIContent();
		gUIContent.text = text;
		Rect position2 = new Rect(5f, 20f, 240f, 120f);
		GUI.Label(position2, gUIContent);
	}

	private void WindowFunction2(int windowID)
	{
		int num = 5;
		int num2 = 20;
		int num3 = 100;
		int num4 = 70;
		int num5 = 50;
		int num6 = 20;
		int num7 = 0;
		Rect position = new Rect((float)num, (float)num2, (float)num3, (float)num6);
		Rect position2 = new Rect(position.xMax, position.yMin, (float)num4, (float)num6);
		Rect position3 = new Rect(position2.xMax, position.yMin, (float)num5, (float)num6);
		Rect position4 = new Rect(position3.xMax, position.yMin, (float)num5, (float)num6);
		GUI.Label(position, "RingCount");
		this.m_window2text[num7] = GUI.TextField(position2, this.m_window2text[num7]);
		if (GUI.Button(position3, "Add"))
		{
			int num8 = int.Parse(this.m_window2text[num7], NumberStyles.AllowLeadingSign);
			PlayerInformation playerInformation = ObjUtil.GetPlayerInformation();
			if (playerInformation != null)
			{
				playerInformation.SetNumRings(playerInformation.NumRings + num8);
			}
		}
		if (GUI.Button(position4, "Stock"))
		{
			ObjUtil.SendMessageTransferRing();
		}
		num2 += 20;
		int num9 = 1;
		Rect position5 = new Rect((float)num, (float)num2, (float)num3, (float)num6);
		Rect position6 = new Rect(position5.xMax, position5.yMin, (float)num4, (float)num6);
		Rect position7 = new Rect(position6.xMax, position5.yMin, (float)num5, (float)num6);
		GUI.Label(position5, "AnimalCount");
		this.m_window2text[num9] = GUI.TextField(position6, this.m_window2text[num9]);
		if (GUI.Button(position7, "Add"))
		{
			int addCount = int.Parse(this.m_window2text[num9], NumberStyles.AllowLeadingSign);
			ObjUtil.SendMessageAddAnimal(addCount);
		}
		num2 += 20;
		int num10 = 2;
		Rect position8 = new Rect((float)num, (float)num2, (float)num3, (float)num6);
		Rect position9 = new Rect(position8.xMax, position8.yMin, (float)num4, (float)num6);
		Rect position10 = new Rect(position9.xMax, position8.yMin, (float)num5, (float)num6);
		GUI.Label(position8, "Distance");
		this.m_window2text[num10] = GUI.TextField(position9, this.m_window2text[num10]);
		if (GUI.Button(position10, "Add"))
		{
			int num11 = int.Parse(this.m_window2text[num10], NumberStyles.AllowLeadingSign);
			GameObjectUtil.SendMessageToTagObjects("Player", "OnDebugAddDistance", num11, SendMessageOptions.DontRequireReceiver);
		}
		num2 += 20;
		int num12 = 3;
		Rect position11 = new Rect((float)num, (float)num2, (float)num3, (float)num6);
		Rect position12 = new Rect(position11.xMax, position11.yMin, (float)num4, (float)num6);
		Rect position13 = new Rect(position12.xMax, position11.yMin, (float)num5, (float)num6);
		GUI.Label(position11, "ComboCount");
		this.m_window2text[num12] = GUI.TextField(position12, this.m_window2text[num12]);
		if (GUI.Button(position13, "Add"))
		{
			int val = int.Parse(this.m_window2text[num12], NumberStyles.AllowLeadingSign);
			StageComboManager instance = StageComboManager.Instance;
			if (instance != null)
			{
				instance.DebugAddCombo(val);
			}
		}
		num2 += 20;
		if (EventManager.Instance != null && EventManager.Instance.IsSpecialStage())
		{
			int num13 = 4;
			Rect position14 = new Rect((float)num, (float)num2, (float)num3, (float)num6);
			Rect position15 = new Rect(position14.xMax, position14.yMin, (float)num4, (float)num6);
			Rect position16 = new Rect(position15.xMax, position14.yMin, (float)num5, (float)num6);
			GUI.Label(position14, "SPObjectCount");
			this.m_window2text[num13] = GUI.TextField(position15, this.m_window2text[num13]);
			if (GUI.Button(position16, "Add"))
			{
				int count = int.Parse(this.m_window2text[num13], NumberStyles.AllowLeadingSign);
				ObjUtil.SendMessageAddSpecialCrystal(count);
			}
			num2 += 20;
		}
		Rect position17 = new Rect((float)num, (float)num2, (float)num3, (float)num6);
		if (GUI.Button(position17, "BossDead"))
		{
			GameObjectUtil.SendMessageToTagObjects("Boss", "OnMsgDebugDead", null, SendMessageOptions.DontRequireReceiver);
		}
		num2 += 20;
	}

	private void WindowFunction3(int windowID)
	{
		int num = 5;
		int num2 = 20;
		int num3 = 300;
		int num4 = 20;
		FriendSignManager instance = FriendSignManager.Instance;
		if (instance)
		{
			List<FriendSignData> friendSignDataList = instance.GetFriendSignDataList();
			foreach (FriendSignData current in friendSignDataList)
			{
				Rect position = new Rect((float)num, (float)num2, (float)num3, (float)num4);
				GUI.Label(position, FriendSignManager.GetDebugDataString(current));
				num2 += num4;
			}
		}
	}

	public static void CreateActivateButton()
	{
		if (StageDebugInformation.Instance == null)
		{
			StageDebugInformation.Create();
		}
	}

	public static void DestroyActivateButton()
	{
		if (StageDebugInformation.Instance != null && !StageDebugInformation.Instance.m_showInformation)
		{
			UnityEngine.Object.Destroy(StageDebugInformation.Instance.gameObject);
		}
	}

	private static StageDebugInformation Create()
	{
		return null;
	}

	private bool SetInstance()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}
}
