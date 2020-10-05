using System;
using System.Collections.Generic;
using UnityEngine;

public class BackKeyManager : MonoBehaviour
{
	private static BackKeyManager instance;

	private List<GameObject> m_windowCallBackList = new List<GameObject>();

	private List<GameObject> m_eventCallBackList = new List<GameObject>();

	private List<GameObject> m_tutorialEventCallBackList = new List<GameObject>();

	private GameObject m_mileageCallBack;

	private GameObject m_mainMenuUICallBack;

	private float m_timer;

	private bool m_invalidFlag;

	private bool m_tutorialFlag;

	private bool m_transSceneFlag = true;

	private bool m_sequenceTransitionFlag;

	private bool m_touchedPrevFrame;

	public static BackKeyManager Instance
	{
		get
		{
			return BackKeyManager.instance;
		}
	}

	public static bool InvalidFlag
	{
		get
		{
			return BackKeyManager.instance != null && BackKeyManager.instance.m_invalidFlag;
		}
		set
		{
			if (BackKeyManager.instance != null)
			{
				BackKeyManager.instance.m_invalidFlag = value;
			}
		}
	}

	public static bool TutorialFlag
	{
		get
		{
			return BackKeyManager.instance != null && BackKeyManager.instance.m_tutorialFlag;
		}
		set
		{
			if (BackKeyManager.instance != null)
			{
				BackKeyManager.instance.m_tutorialFlag = value;
			}
		}
	}

	public static bool MenuSequenceTransitionFlag
	{
		get
		{
			return BackKeyManager.instance != null && BackKeyManager.instance.m_sequenceTransitionFlag;
		}
		set
		{
			if (BackKeyManager.instance != null)
			{
				BackKeyManager.instance.m_sequenceTransitionFlag = value;
			}
		}
	}

	public static void StartScene()
	{
		if (BackKeyManager.instance != null)
		{
			BackKeyManager.instance.m_invalidFlag = false;
			BackKeyManager.instance.m_tutorialFlag = false;
			BackKeyManager.instance.m_transSceneFlag = false;
			BackKeyManager.instance.m_sequenceTransitionFlag = false;
		}
	}

	public static void EndScene()
	{
		if (BackKeyManager.instance != null)
		{
			BackKeyManager.instance.m_windowCallBackList.Clear();
			BackKeyManager.instance.m_eventCallBackList.Clear();
			BackKeyManager.instance.m_tutorialEventCallBackList.Clear();
			BackKeyManager.instance.m_mileageCallBack = null;
			BackKeyManager.instance.m_mainMenuUICallBack = null;
			BackKeyManager.instance.m_transSceneFlag = true;
		}
	}

	public static void AddWindowCallBack(GameObject obj)
	{
		if (BackKeyManager.instance != null && !BackKeyManager.instance.m_windowCallBackList.Contains(obj))
		{
			BackKeyManager.instance.m_windowCallBackList.Add(obj);
		}
	}

	public static void RemoveWindowCallBack(GameObject obj)
	{
		if (BackKeyManager.instance != null && BackKeyManager.instance.m_windowCallBackList.Contains(obj))
		{
			BackKeyManager.instance.m_windowCallBackList.Remove(obj);
		}
	}

	public static void AddEventCallBack(GameObject obj)
	{
		if (BackKeyManager.instance != null && !BackKeyManager.instance.m_eventCallBackList.Contains(obj))
		{
			BackKeyManager.instance.m_eventCallBackList.Add(obj);
		}
	}

	public static void AddMainMenuUI(GameObject obj)
	{
		if (BackKeyManager.instance != null && BackKeyManager.instance.m_mainMenuUICallBack == null)
		{
			BackKeyManager.instance.m_mainMenuUICallBack = obj;
		}
	}

	public static void AddMileageCallBack(GameObject obj)
	{
		if (BackKeyManager.instance != null && BackKeyManager.instance.m_mileageCallBack == null)
		{
			BackKeyManager.instance.m_mileageCallBack = obj;
		}
	}

	public static void AddTutorialEventCallBack(GameObject obj)
	{
		if (BackKeyManager.instance != null && !BackKeyManager.instance.m_tutorialEventCallBackList.Contains(obj))
		{
			BackKeyManager.instance.m_tutorialEventCallBackList.Add(obj);
		}
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
	}

	private void LateUpdate()
	{
		if (this.m_timer > 0f)
		{
			this.m_timer -= RealTime.deltaTime;
		}
		bool flag = UICamera.touchCount > 0;
		if (UnityEngine.Input.GetKey(KeyCode.Escape) && this.m_timer <= 0f && !flag && !this.m_touchedPrevFrame)
		{
			this.m_timer = 0.6f;
			if (NetworkErrorWindow.Created)
			{
				NetworkErrorWindow.OnClickPlatformBackButton();
				return;
			}
			if (this.m_invalidFlag || this.m_transSceneFlag)
			{
				return;
			}
			if (this.CheckConnectNetwork())
			{
				return;
			}
			if (this.m_tutorialFlag)
			{
				WindowBase.BackButtonMessage backButtonMessage = new WindowBase.BackButtonMessage();
				if (this.m_sequenceTransitionFlag)
				{
					backButtonMessage.StaySequence();
				}
				this.SentWindowMessege(ref backButtonMessage);
				this.SentMileageMessege(ref backButtonMessage);
				if (!backButtonMessage.IsFlag(WindowBase.BackButtonMessage.Flags.STAY_SEQUENCE))
				{
					foreach (GameObject current in this.m_tutorialEventCallBackList)
					{
						if (current != null)
						{
							current.SendMessage("OnClickPlatformBackButtonTutorialEvent", null, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
				return;
			}
			WindowBase.BackButtonMessage backButtonMessage2 = new WindowBase.BackButtonMessage();
			if (this.m_sequenceTransitionFlag)
			{
				backButtonMessage2.StaySequence();
			}
			this.SentWindowMessege(ref backButtonMessage2);
			this.SendMainMenuUI(ref backButtonMessage2);
			this.SentMileageMessege(ref backButtonMessage2);
			if (!backButtonMessage2.IsFlag(WindowBase.BackButtonMessage.Flags.STAY_SEQUENCE))
			{
				foreach (GameObject current2 in this.m_eventCallBackList)
				{
					if (current2 != null)
					{
						current2.SendMessage("OnClickPlatformBackButtonEvent", null, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
		this.m_touchedPrevFrame = flag;
	}

	private void OnDestroy()
	{
		if (BackKeyManager.instance == this)
		{
			BackKeyManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (BackKeyManager.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			BackKeyManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private bool CheckConnectNetwork()
	{
		return NetMonitor.Instance != null && !NetMonitor.Instance.IsIdle();
	}

	private void SentWindowMessege(ref WindowBase.BackButtonMessage msg)
	{
		if (this.m_windowCallBackList.Count > 0)
		{
			foreach (GameObject current in this.m_windowCallBackList)
			{
				if (current.activeSelf)
				{
					current.SendMessage("OnClickPlatformBackButton", msg, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	private void SendMainMenuUI(ref WindowBase.BackButtonMessage msg)
	{
		if (!msg.IsFlag(WindowBase.BackButtonMessage.Flags.STAY_SEQUENCE) && this.m_mainMenuUICallBack != null)
		{
			this.m_mainMenuUICallBack.SendMessage("OnClickPlatformBackButton", msg, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void SentMileageMessege(ref WindowBase.BackButtonMessage msg)
	{
		if (!msg.IsFlag(WindowBase.BackButtonMessage.Flags.STAY_SEQUENCE) && this.m_mileageCallBack != null)
		{
			this.m_mileageCallBack.SendMessage("OnClickPlatformBackButton", msg, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void SendEventMessage()
	{
		foreach (GameObject current in this.m_eventCallBackList)
		{
			if (current != null)
			{
				current.SendMessage("OnPlatformBackButtonClicked", null, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
