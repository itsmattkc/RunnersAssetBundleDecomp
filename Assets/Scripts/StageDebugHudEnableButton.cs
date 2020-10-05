using System;
using UnityEngine;

public class StageDebugHudEnableButton : MonoBehaviour
{
	private GameObject m_hudCockpit;

	private GameObject m_DebugTraceManager;

	private AllocationStatus m_allocationStatus;

	private Rect m_buttonRect;

	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Start()
	{
		this.m_hudCockpit = GameObject.Find("HudCockpit");
		this.m_DebugTraceManager = GameObject.Find("DebugTraceManager");
		this.m_allocationStatus = GameObjectUtil.FindGameObjectComponent<AllocationStatus>("AllocationStatus");
		float num = 100f;
		this.m_buttonRect = new Rect(((float)Screen.width - num) * 0.5f, 0f, num, num);
	}

	private void OnGUI()
	{
		if (GUI.Button(this.m_buttonRect, string.Empty, GUIStyle.none))
		{
			bool flag = true;
			if (this.m_hudCockpit != null)
			{
				flag = !this.m_hudCockpit.activeSelf;
				this.m_hudCockpit.SetActive(flag);
			}
			if (this.m_DebugTraceManager != null)
			{
				this.m_DebugTraceManager.SetActive(flag);
			}
			if (this.m_allocationStatus != null)
			{
				this.m_allocationStatus.show = flag;
			}
		}
	}
}
