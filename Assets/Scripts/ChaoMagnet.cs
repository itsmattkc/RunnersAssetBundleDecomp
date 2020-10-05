using Message;
using System;
using UnityEngine;

public class ChaoMagnet : MonoBehaviour
{
	private SphereCollider m_collider;

	private string m_hitLayer = string.Empty;

	private void Start()
	{
		base.enabled = false;
	}

	public void Setup(float colliRadius, string hitLayer)
	{
		this.m_hitLayer = hitLayer;
		this.m_collider = base.gameObject.GetComponent<SphereCollider>();
		if (this.m_collider == null)
		{
			this.m_collider = base.gameObject.AddComponent<SphereCollider>();
		}
		if (this.m_collider != null)
		{
			this.m_collider.radius = colliRadius;
			this.m_collider.isTrigger = true;
			this.m_collider.enabled = false;
		}
	}

	public void SetEnable(bool flag)
	{
		if (this.m_collider != null)
		{
			this.m_collider.enabled = flag;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!string.IsNullOrEmpty(this.m_hitLayer) && other.gameObject.layer == LayerMask.NameToLayer(this.m_hitLayer))
		{
			MsgOnDrawingRings value = new MsgOnDrawingRings(base.gameObject);
			other.gameObject.SendMessage("OnDrawingRingsToChao", value, SendMessageOptions.DontRequireReceiver);
		}
	}
}
