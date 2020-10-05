using System;
using UnityEngine;

public class UIDraggablePanelAssistance : MonoBehaviour
{
	private UIDraggablePanel m_draggablePanel;

	private SpringPanel m_spring;

	[SerializeField]
	private bool X_Axis = true;

	[SerializeField]
	private float X;

	[SerializeField]
	private bool Y_Axis = true;

	[SerializeField]
	private float Y;

	private bool autoCheck = true;

	private float checkTime = 0.5f;

	private long m_count;

	private bool m_isAssistanceEnable = true;

	private bool m_init;

	private float m_currentCheckTime;

	public bool isAssistanceEnable
	{
		get
		{
			bool result = false;
			if (this.m_draggablePanel != null && this.m_isAssistanceEnable)
			{
				result = true;
			}
			return result;
		}
		set
		{
			this.m_isAssistanceEnable = value;
		}
	}

	private void Start()
	{
		this.Init();
	}

	private void Update()
	{
		if (!this.m_init)
		{
			this.Init();
		}
		if (this.isAssistanceEnable)
		{
			this.Check();
		}
	}

	private void Check()
	{
		if (this.m_currentCheckTime <= 0f && !this.autoCheck)
		{
			return;
		}
		bool flag = false;
		if (this.autoCheck && this.m_count % 3L == 0L)
		{
			if (this.m_count % 3L == 0L)
			{
				flag = true;
			}
		}
		else if (this.m_currentCheckTime > 0f)
		{
			flag = true;
		}
		if (flag)
		{
			if (this.m_spring == null)
			{
				this.m_spring = base.gameObject.GetComponent<SpringPanel>();
			}
			if (this.m_spring != null && (this.X_Axis || this.Y_Axis))
			{
				float x = this.m_spring.target.x;
				float y = this.m_spring.target.y;
				float z = this.m_spring.target.z;
				bool flag2 = false;
				if (this.X_Axis && x != this.X)
				{
					this.m_spring.target = new Vector3(this.X, y, z);
					flag2 = true;
				}
				if (this.Y_Axis && y != this.Y)
				{
					this.m_spring.target = new Vector3(x, this.Y, z);
					flag2 = true;
				}
				if (flag2 && !this.m_spring.enabled)
				{
					this.m_spring.enabled = true;
				}
			}
			if (!this.autoCheck)
			{
				this.m_currentCheckTime -= Time.deltaTime;
			}
		}
		this.m_count += 1L;
	}

	private void Init()
	{
		this.m_draggablePanel = base.gameObject.GetComponent<UIDraggablePanel>();
		this.m_init = true;
	}

	public void CheckDraggable()
	{
		this.m_currentCheckTime = this.checkTime;
	}
}
