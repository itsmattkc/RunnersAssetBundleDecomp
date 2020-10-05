using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjTrampolineFloor")]
public class ObjTrampolineFloor : SpawnableObject
{
	private const string ModelName = "obj_cmn_trampolinefloor";

	private float m_firstSpeed;

	private float m_outOfcontrol;

	protected override string GetModelName()
	{
		return "obj_cmn_trampolinefloor";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override void OnSpawned()
	{
		ObjUtil.StopAnimation(base.gameObject);
		ObjUtil.PlayEffectCollisionCenter(base.gameObject, "ef_com_item_warp_s01", 1f, false);
		base.enabled = false;
	}

	public void SetParam(float first_speed, float out_of_control)
	{
		this.m_firstSpeed = first_speed;
		this.m_outOfcontrol = out_of_control;
	}

	private void OnTriggerEnter(Collider other)
	{
		MsgOnSpringImpulse msgOnSpringImpulse = new MsgOnSpringImpulse(base.transform.position, base.transform.rotation, this.m_firstSpeed, this.m_outOfcontrol);
		other.gameObject.SendMessage("OnSpringImpulse", msgOnSpringImpulse, SendMessageOptions.DontRequireReceiver);
		if (msgOnSpringImpulse.m_succeed)
		{
			Animation componentInChildren = base.GetComponentInChildren<Animation>();
			if (componentInChildren)
			{
				componentInChildren.wrapMode = WrapMode.Once;
				componentInChildren.Play("obj_trampolinefloor_bounce");
			}
			ObjUtil.PlaySE("obj_trampolinefloor", "SE");
		}
	}
}
