using System;
using UnityEngine;

public class MileageMapTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_xml_data;

	private void Start()
	{
		MileageMapDataManager instance = MileageMapDataManager.Instance;
		if (instance != null)
		{
			instance.SetData(this.m_xml_data);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
	}
}
