using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjPrism")]
public class ObjPrism : SpawnableObject
{
	private const string ModelName = "obj_cmn_prism";

	protected override string GetModelName()
	{
		return "obj_cmn_prism";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override void OnSpawned()
	{
		ObjUtil.StopAnimation(base.gameObject);
	}

	public void OnReturnFromPhantom(MsgReturnFromPhantom msg)
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		ObjUtil.PlayEffectCollisionCenter(base.gameObject, "ef_ph_laser_reflect01", 1f, false);
		Animation componentInChildren = base.GetComponentInChildren<Animation>();
		if (componentInChildren)
		{
			componentInChildren.wrapMode = WrapMode.Once;
			componentInChildren.Play("obj_prism_anim");
		}
		ObjUtil.PlaySE("phantom_laser_prism", "SE");
	}
}
