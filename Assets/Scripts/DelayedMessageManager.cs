using System;
using System.Collections.Generic;
using UnityEngine;

public class DelayedMessageManager : MonoBehaviour
{
	private class DelayMessageData
	{
		public string m_objectName;

		public string m_methodName;

		public GameObject m_object;

		public object m_option;

		public DelayMessageData(string objectName, string methodName, object option)
		{
			this.m_objectName = objectName;
			this.m_methodName = methodName;
			this.m_option = option;
		}

		public DelayMessageData(GameObject gameObject, string methodName, object option)
		{
			this.m_object = gameObject;
			this.m_methodName = methodName;
			this.m_option = option;
		}
	}

	private List<DelayedMessageManager.DelayMessageData> m_datas;

	private List<DelayedMessageManager.DelayMessageData> m_sendToTagDatas;

	private static DelayedMessageManager instance;

	public static DelayedMessageManager Instance
	{
		get
		{
			if (DelayedMessageManager.instance == null)
			{
				DelayedMessageManager.instance = GameObjectUtil.FindGameObjectComponent<DelayedMessageManager>("DelayedMessageManager");
			}
			return DelayedMessageManager.instance;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	private void Start()
	{
		this.m_datas = new List<DelayedMessageManager.DelayMessageData>();
		this.m_sendToTagDatas = new List<DelayedMessageManager.DelayMessageData>();
	}

	private void Update()
	{
		if (this.m_sendToTagDatas.Count > 0)
		{
			foreach (DelayedMessageManager.DelayMessageData current in this.m_sendToTagDatas)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag(current.m_objectName);
				GameObject[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					GameObject gameObject = array2[i];
					this.AddDelayedMessage(gameObject, current.m_methodName, current.m_option);
				}
			}
			this.m_sendToTagDatas.Clear();
		}
		if (this.m_datas.Count > 0)
		{
			for (int j = 0; j < this.m_datas.Count; j++)
			{
				DelayedMessageManager.DelayMessageData delayMessageData = this.m_datas[j];
				if (delayMessageData.m_object == null && delayMessageData.m_objectName != null)
				{
					delayMessageData.m_object = GameObject.Find(delayMessageData.m_objectName);
				}
				if (delayMessageData.m_object != null)
				{
					delayMessageData.m_object.SendMessage(delayMessageData.m_methodName, delayMessageData.m_option, SendMessageOptions.DontRequireReceiver);
				}
			}
			this.m_datas.Clear();
		}
	}

	public void AddDelayedMessage(string objectName, string methodName, object option)
	{
		DelayedMessageManager.DelayMessageData item = new DelayedMessageManager.DelayMessageData(objectName, methodName, option);
		this.m_datas.Add(item);
	}

	public void AddDelayedMessage(GameObject gameObject, string methodName, object option)
	{
		DelayedMessageManager.DelayMessageData item = new DelayedMessageManager.DelayMessageData(gameObject, methodName, option);
		this.m_datas.Add(item);
	}

	public void AddDelayedMessageToTag(string objectName, string methodName, object option)
	{
		DelayedMessageManager.DelayMessageData item = new DelayedMessageManager.DelayMessageData(objectName, methodName, option);
		this.m_sendToTagDatas.Add(item);
	}

	private void OnDestroy()
	{
		if (DelayedMessageManager.instance == this)
		{
			DelayedMessageManager.instance = null;
		}
	}

	protected bool CheckInstance()
	{
		if (DelayedMessageManager.instance == null)
		{
			DelayedMessageManager.instance = this;
			return true;
		}
		if (this == DelayedMessageManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(this);
		return false;
	}
}
