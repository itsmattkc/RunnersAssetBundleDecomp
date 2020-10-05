using System;
using UnityEngine;

public class MileageBonusInfo : MonoBehaviour
{
	public const string COMPONENT_NAME = "MileageBonusInfo";

	private MileageBonusData m_bonus_data;

	public MileageBonusData BonusData
	{
		get
		{
			return this.m_bonus_data;
		}
		set
		{
			this.m_bonus_data = value;
		}
	}

	public static MileageBonusInfo CreateMileageBonusInfo()
	{
		MileageBonusInfo mileageBonusInfo = GameObjectUtil.FindGameObjectComponent<MileageBonusInfo>("MileageBonusInfo");
		if (mileageBonusInfo)
		{
			UnityEngine.Object.Destroy(mileageBonusInfo.gameObject);
		}
		MileageBonusInfo mileageBonusInfo2 = null;
		GameObject gameObject = new GameObject("MileageBonusInfo");
		if (gameObject != null)
		{
			mileageBonusInfo2 = gameObject.AddComponent<MileageBonusInfo>();
			mileageBonusInfo2.ResetData();
		}
		return mileageBonusInfo2;
	}

	private void Start()
	{
		base.enabled = false;
	}

	public void ResetData()
	{
		this.m_bonus_data.type = MileageBonus.NULL_EFFECT;
		this.m_bonus_data.value = 0f;
	}
}
