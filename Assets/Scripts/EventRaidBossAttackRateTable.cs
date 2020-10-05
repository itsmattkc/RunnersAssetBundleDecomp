using System;
using UnityEngine;

public class EventRaidBossAttackRateTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_xml_data;

	private void Start()
	{
		EventManager instance = EventManager.Instance;
		if (instance != null)
		{
			instance.SetRaidBossAttacRate(this.m_xml_data);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
	}
}
