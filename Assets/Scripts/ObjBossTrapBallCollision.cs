using Message;
using System;
using UnityEngine;

public class ObjBossTrapBallCollision : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				MsgHitDamage value = new MsgHitDamage(base.gameObject, AttackPower.PlayerColorPower);
				gameObject.SendMessage("OnDamageHit", value, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void OnDrawGizmos()
	{
		SphereCollider component = base.GetComponent<SphereCollider>();
		if (component)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, component.radius);
		}
	}
}
