using Message;
using System;
using UnityEngine;

public class ObjBossBase : SpawnableObject
{
	protected override void OnSpawned()
	{
		if (ObjBossUtil.IsNowLastChance(ObjUtil.GetPlayerInformation()))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.SetOnlyOneObject();
		base.SetNotRageout(true);
	}

	private void OnMsgBossInfo(MsgBossInfo msg)
	{
		msg.m_boss = base.gameObject;
		msg.m_position = base.transform.position;
		msg.m_rotation = base.transform.rotation;
		msg.m_succeed = true;
	}
}
