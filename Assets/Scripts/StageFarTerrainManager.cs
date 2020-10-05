using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Game/Level")]
public class StageFarTerrainManager : MonoBehaviour
{
	private const float nextOffset = 1000f;

	private const float drawOffset = 1500f;

	private const float destroyOffset = 1700f;

	private const string defaultModelName = "_far";

	public GameObject m_originalFarModel;

	private List<GameObject> m_nowDrawingModel;

	private PlayerInformation m_playerInfo;

	private int m_nowSpawnedNumModels;

	private float m_nextSpawnOffset;

	private static readonly Vector3 originalPosition = new Vector3(-1500f, 0f, 7920f) * 0.1f;

	private static readonly Vector3 originalRotation = new Vector3(0f, 0f, 0f);

	private void Start()
	{
		this.m_playerInfo = GameObject.Find("PlayerInformation").GetComponent<PlayerInformation>();
		this.m_nowDrawingModel = new List<GameObject>();
	}

	public void SetupModel(string stageName)
	{
		string text = stageName + "_far";
		if (TenseEffectManager.Instance != null)
		{
			TenseEffectManager.Type tenseType = TenseEffectManager.Instance.GetTenseType();
			text += ((tenseType != TenseEffectManager.Type.TENSE_A) ? "B" : "A");
		}
		this.m_nowSpawnedNumModels = 0;
		this.m_originalFarModel = ResourceManager.Instance.GetGameObject(ResourceCategory.TERRAIN_MODEL, text);
		this.InstantiateModel(StageFarTerrainManager.originalPosition, Quaternion.Euler(StageFarTerrainManager.originalRotation));
		Vector3 position = StageFarTerrainManager.originalPosition;
		position.x += 1500f;
		this.InstantiateModel(position, Quaternion.Euler(StageFarTerrainManager.originalRotation));
		this.m_nextSpawnOffset = StageFarTerrainManager.originalPosition.x + 1000f;
	}

	private void Update()
	{
		if (this.m_playerInfo && this.m_originalFarModel && this.m_nowSpawnedNumModels > 0)
		{
			Vector3 position = this.m_playerInfo.Position;
			if (position.x > this.m_nextSpawnOffset)
			{
				Vector3 position2 = StageFarTerrainManager.originalPosition;
				position2.x += 1500f * (float)this.m_nowSpawnedNumModels;
				this.InstantiateModel(position2, Quaternion.Euler(StageFarTerrainManager.originalRotation));
				this.m_nextSpawnOffset += 1000f;
			}
			for (int i = this.m_nowDrawingModel.Count - 1; i >= 0; i--)
			{
				if (position.x - this.m_nowDrawingModel[i].transform.position.x > 1700f)
				{
					UnityEngine.Object.Destroy(this.m_nowDrawingModel[i]);
					this.m_nowDrawingModel.Remove(this.m_nowDrawingModel[i]);
				}
			}
		}
	}

	private void InstantiateModel(Vector3 position, Quaternion rotation)
	{
		if (this.m_originalFarModel == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(this.m_originalFarModel, position, rotation) as GameObject;
		if (gameObject)
		{
			gameObject.isStatic = true;
			gameObject.SetActive(true);
			this.m_nowDrawingModel.Add(gameObject);
			this.m_nowSpawnedNumModels++;
		}
	}

	private void OnMsgStageReplace(MsgStageReplace msg)
	{
		for (int i = this.m_nowDrawingModel.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.m_nowDrawingModel[i]);
		}
		this.m_nowDrawingModel.Clear();
		this.SetupModel(msg.m_stageName);
	}
}
