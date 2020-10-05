using Message;
using System;
using UnityEngine;

public class ObjTutorialEndCollision : ObjCollision
{
	private const float COLLIDER_X_SIZE = 2f;

	protected override void OnSpawned()
	{
		base.OnSpawned();
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (component != null && component.size.x < 2f)
		{
			component.size = new Vector3(2f, component.size.y, component.size.z);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObjectUtil.SendMessageFindGameObject("StageTutorialManager", "OnMsgTutorialEnd", new MsgTutorialEnd(), SendMessageOptions.DontRequireReceiver);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
