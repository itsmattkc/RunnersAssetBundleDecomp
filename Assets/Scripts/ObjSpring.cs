using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjSpring")]
public class ObjSpring : SpawnableObject
{
	private const string ModelName = "obj_cmn_spring";

	private float m_firstSpeed = 5f;

	private float m_outOfcontrol = 0.5f;

	protected override string GetModelName()
	{
		return "obj_cmn_spring";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected virtual string GetMotionName()
	{
		return "obj_spring_bounce";
	}

	protected override void OnSpawned()
	{
		ObjUtil.StopAnimation(base.gameObject);
		base.enabled = false;
	}

	public void SetObjSpringParameter(ObjSpringParameter param)
	{
		this.m_firstSpeed = param.firstSpeed;
		this.m_outOfcontrol = param.outOfcontrol;
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
				componentInChildren.Play(this.GetMotionName());
			}
			ObjUtil.PlaySE("obj_spring", "SE");
		}
	}
}
