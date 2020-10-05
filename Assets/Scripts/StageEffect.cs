using System;
using UnityEngine;

public class StageEffect : MonoBehaviour
{
	private GameObject m_stageEffect;

	private GameObject m_cameraObject;

	private bool m_resetPos;

	private void OnDestroy()
	{
		if (this.m_stageEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_stageEffect);
			this.m_stageEffect = null;
		}
	}

	private void Update()
	{
		if (this.m_stageEffect != null && this.m_cameraObject != null)
		{
			if (this.m_resetPos)
			{
				base.transform.localPosition = -this.m_cameraObject.transform.position;
			}
			else
			{
				base.transform.localPosition = new Vector3(0f, 0f, -this.m_cameraObject.transform.position.z);
			}
		}
	}

	public void Setup(GameObject originalObj)
	{
		if (this.m_stageEffect == null && originalObj != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(originalObj, Vector3.zero, Quaternion.identity) as GameObject;
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				gameObject.transform.parent = base.gameObject.transform;
				gameObject.transform.localPosition = originalObj.transform.localPosition;
				gameObject.transform.localRotation = originalObj.transform.localRotation;
				this.m_stageEffect = gameObject;
			}
		}
		if (this.m_cameraObject == null)
		{
			this.m_cameraObject = base.transform.parent.gameObject;
		}
	}

	public void ResetPos(bool reset)
	{
		this.m_resetPos = reset;
	}

	public static StageEffect CreateStageEffect(string stageName)
	{
		StageEffect stageEffect = null;
		if (ResourceManager.Instance != null)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.TERRAIN_MODEL, "ef_stage_" + stageName);
			if (gameObject != null)
			{
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("MainCamera");
				if (gameObject2 != null)
				{
					GameObject gameObject3 = new GameObject("StageEffect");
					if (gameObject3 != null)
					{
						gameObject3.SetActive(true);
						gameObject3.transform.parent = gameObject2.transform;
						gameObject3.transform.localPosition = Vector3.zero;
						gameObject3.transform.localRotation = Quaternion.identity;
						stageEffect = gameObject3.AddComponent<StageEffect>();
						if (stageEffect != null)
						{
							stageEffect.Setup(gameObject);
						}
					}
				}
			}
		}
		return stageEffect;
	}
}
