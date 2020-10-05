using Message;
using System;
using UnityEngine;

public class HudAlertIcon : MonoBehaviour
{
	private const float X_OFFSET = 50f;

	private static GameObject m_prefabObject;

	private GameObject m_iconObject;

	private Camera m_camera;

	private GameObject m_chaseObject;

	private TinyFsmBehavior m_fsm;

	private float m_displayTime;

	private float m_currentTime;

	private bool m_isEnd;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public void Setup(Camera camera, GameObject chaseObject, float displayTime)
	{
		this.m_camera = camera;
		this.m_chaseObject = chaseObject;
		this.m_displayTime = displayTime;
		this.m_currentTime = 0f;
		this.m_isEnd = false;
	}

	public bool IsChasingObject(GameObject gameObject)
	{
		return this.m_chaseObject == gameObject;
	}

	private void Start()
	{
		if (HudAlertIcon.m_prefabObject == null)
		{
			HudAlertIcon.m_prefabObject = (Resources.Load("Prefabs/UI/ui_gp_icon_alert") as GameObject);
			if (HudAlertIcon.m_prefabObject == null)
			{
				return;
			}
		}
		this.m_iconObject = (UnityEngine.Object.Instantiate(HudAlertIcon.m_prefabObject, Vector3.zero, Quaternion.identity) as GameObject);
		this.m_iconObject.SetActive(false);
		GameObject gameObject = GameObject.Find("UI Root (2D)/Camera/Anchor_6_MR/");
		if (gameObject == null)
		{
			gameObject = new GameObject("Anchor_6_MR");
			GameObject gameObject2 = GameObject.Find("UI Root (2D)/Camera");
			if (gameObject2 != null)
			{
				gameObject.transform.parent = gameObject2.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			UIAnchor uIAnchor = gameObject.AddComponent<UIAnchor>();
			uIAnchor.side = UIAnchor.Side.Right;
			uIAnchor.halfPixelOffset = false;
			if (gameObject2 != null)
			{
				uIAnchor.uiCamera = gameObject2.GetComponent<Camera>();
			}
		}
		this.m_iconObject.transform.parent = gameObject.transform;
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm == null)
		{
			return;
		}
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StatePlay));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
	}

	private void Update()
	{
	}

	private TinyFsmState StatePlay(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
		{
			this.m_iconObject.SetActive(true);
			Vector2 screenPosition = HudUtility.GetScreenPosition(this.m_camera, this.m_chaseObject);
			screenPosition.x = -50f;
			screenPosition.y -= (float)Screen.height * 0.5f;
			Transform component = this.m_iconObject.GetComponent<Transform>();
			component.localPosition = new Vector3(screenPosition.x, screenPosition.y, 0f);
			this.m_currentTime += Time.deltaTime;
			if (this.m_currentTime >= this.m_displayTime)
			{
				this.m_isEnd = true;
				this.m_iconObject.SetActive(false);
				UnityEngine.Object.Destroy(this.m_iconObject);
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateIdle(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void OnMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
	}
}
