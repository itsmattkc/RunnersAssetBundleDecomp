using System;
using UnityEngine;

public class GetBonus : MonoBehaviour
{
	private GameObject m_bonus_mng_object;

	public void AddBonusMngObject(GameObject obj)
	{
		this.m_bonus_mng_object = obj;
	}

	public void SetBonusCount(GameObject obj)
	{
		if (this.m_bonus_mng_object)
		{
			this.m_bonus_mng_object.SendMessage("OnTake", obj, SendMessageOptions.DontRequireReceiver);
		}
	}
}
