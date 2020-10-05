using System;
using System.Collections.Generic;
using UnityEngine;

public class HudEventQuota : MonoBehaviour
{
	public delegate void PlayEndCallback();

	private List<QuotaInfo> m_quotaList = new List<QuotaInfo>();

	private GameObject m_rootObject;

	[SerializeField]
	private GameObject m_prefabObject;

	private Animation m_animation;

	private int m_currentAnimIndex;

	private bool m_isPlayEnd = true;

	private HudEventQuota.PlayEndCallback m_callback;

	public bool IsPlayEnd
	{
		get
		{
			return this.m_isPlayEnd;
		}
		private set
		{
		}
	}

	public void AddQuota(QuotaInfo info)
	{
		if (this.m_quotaList == null)
		{
			return;
		}
		this.m_quotaList.Add(info);
	}

	public void ClearQuota()
	{
		if (this.m_quotaList == null)
		{
			return;
		}
		this.m_quotaList.Clear();
	}

	public void Setup(GameObject rootObject, Animation animation, string swapAnimName1, string swapAnimName2)
	{
		this.m_rootObject = rootObject;
		this.m_animation = animation;
		if (this.m_prefabObject == null)
		{
			this.m_prefabObject = HudEventQuota.FindPrefabObject();
		}
		int count = this.m_quotaList.Count;
		if (count <= 0)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_rootObject, "next_arrow1");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			return;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_rootObject, "slot1");
		if (gameObject2 != null)
		{
			GameObject quotaPlate = HudEventQuota.CopyAttachPrefab(gameObject2, this.m_prefabObject);
			this.m_quotaList[0].Setup(quotaPlate, this.m_animation, string.Empty);
			this.m_quotaList[0].SetupDisplay();
		}
		if (count >= 2)
		{
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_rootObject, "slot2");
			if (gameObject3 != null)
			{
				GameObject quotaPlate2 = HudEventQuota.CopyAttachPrefab(gameObject3, this.m_prefabObject);
				this.m_quotaList[1].Setup(quotaPlate2, this.m_animation, swapAnimName1);
			}
			for (int i = 2; i < count; i++)
			{
				GameObject quotaPlate3 = this.m_quotaList[i - 2].QuotaPlate;
				if (!(quotaPlate3 == null))
				{
					string animClipName = string.Empty;
					if (i % 2 == 0)
					{
						animClipName = swapAnimName2;
					}
					else
					{
						animClipName = swapAnimName1;
					}
					this.m_quotaList[i].Setup(quotaPlate3, this.m_animation, animClipName);
				}
			}
		}
	}

	public void PlayStart(HudEventQuota.PlayEndCallback callback)
	{
		if (this.m_quotaList.Count <= 0)
		{
			this.m_isPlayEnd = true;
			callback();
			return;
		}
		this.m_callback = callback;
		this.m_isPlayEnd = false;
		this.m_currentAnimIndex = 0;
		this.m_quotaList[this.m_currentAnimIndex].PlayStart();
	}

	public void PlayStop()
	{
		this.m_isPlayEnd = true;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_isPlayEnd)
		{
			return;
		}
		if (this.m_quotaList.Count <= this.m_currentAnimIndex)
		{
			return;
		}
		QuotaInfo quotaInfo = this.m_quotaList[this.m_currentAnimIndex];
		if (quotaInfo == null)
		{
			return;
		}
		quotaInfo.Update();
		if (quotaInfo.IsPlayEnd())
		{
			this.m_currentAnimIndex++;
			if (this.m_quotaList.Count <= this.m_currentAnimIndex)
			{
				this.m_isPlayEnd = true;
				if (this.m_callback != null)
				{
					this.m_callback();
				}
			}
			else
			{
				this.m_quotaList[this.m_currentAnimIndex].PlayStart();
			}
		}
	}

	private static GameObject FindPrefabObject()
	{
		GameObject result = null;
		GameObject gameObject = GameObject.Find("ResourceManager");
		if (gameObject == null)
		{
			return result;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "EventResourceStage");
		if (gameObject2 == null)
		{
			return result;
		}
		return GameObjectUtil.FindChildGameObject(gameObject2, "ui_event_mission_scroll");
	}

	private static GameObject CopyAttachPrefab(GameObject parentObject, GameObject prefabObject)
	{
		GameObject gameObject = null;
		if (parentObject == null)
		{
			return gameObject;
		}
		if (prefabObject == null)
		{
			return gameObject;
		}
		gameObject = (GameObject)UnityEngine.Object.Instantiate(prefabObject);
		Vector3 localPosition = gameObject.transform.localPosition;
		Vector3 localScale = gameObject.transform.localScale;
		gameObject.transform.parent = parentObject.transform;
		gameObject.transform.localPosition = localPosition;
		gameObject.transform.localScale = localScale;
		gameObject.SetActive(true);
		return gameObject;
	}
}
