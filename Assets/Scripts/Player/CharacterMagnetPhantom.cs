using App;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterMagnetPhantom : MonoBehaviour
	{
		private Vector3 m_defaultOffset;

		private float m_defaultRadius;

		private bool m_offDrawing;

		private void Awake()
		{
			SphereCollider component = base.GetComponent<SphereCollider>();
			if (component)
			{
				this.m_defaultOffset = component.center;
				this.m_defaultRadius = component.radius;
			}
		}

		private void Start()
		{
		}

		private void SetRadiusAndOffset(float radius, Vector3 offset)
		{
			SphereCollider component = base.GetComponent<SphereCollider>();
			if (component)
			{
				if (!App.Math.NearEqual(radius, component.radius, 1E-06f))
				{
					component.radius = radius;
				}
				if (!App.Math.NearZero((offset - component.center).sqrMagnitude, 1E-06f))
				{
					component.center = offset;
				}
				this.m_defaultOffset = component.center;
				this.m_defaultRadius = component.radius;
				if (StageAbilityManager.Instance != null)
				{
					float num = StageAbilityManager.Instance.GetChaoAbliltyValue(ChaoAbility.MAGNET_RANGE, 100f) / 100f;
					component.radius = this.m_defaultRadius * num;
					StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.MAGNET_RANGE);
				}
			}
		}

		public void SetDefaultRadiusAndOffset()
		{
			this.SetRadiusAndOffset(this.m_defaultRadius, this.m_defaultOffset);
		}

		public void SetOffDrawing(bool value)
		{
			this.m_offDrawing = value;
			SphereCollider component = base.GetComponent<SphereCollider>();
			if (component)
			{
				component.enabled = !this.m_offDrawing;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!this.m_offDrawing)
			{
				MsgOnDrawingRings value = new MsgOnDrawingRings();
				GameObjectUtil.SendDelayedMessageToGameObject(other.gameObject, "OnDrawingRings", value);
			}
		}
	}
}
