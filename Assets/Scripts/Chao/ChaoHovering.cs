using System;
using UnityEngine;

namespace Chao
{
	public class ChaoHovering : ChaoHoveringBase
	{
		public class CInfo : ChaoHoveringBase.CInfoBase
		{
			public float height;

			public float speed;

			public float startAngle;

			public CInfo(ChaoMovement movement) : base(movement)
			{
			}
		}

		public const float PI_2 = 6.28318548f;

		private float m_angle;

		[SerializeField]
		private float m_hovering_speed = 3.14159274f;

		[SerializeField]
		private float m_hovering_height = 0.3f;

		private Vector3 m_hovering_pos = Vector3.zero;

		protected override void SetupImpl(ChaoHoveringBase.CInfoBase info)
		{
			ChaoHovering.CInfo cInfo = info as ChaoHovering.CInfo;
			this.m_hovering_height = cInfo.height;
			this.m_hovering_speed = cInfo.speed * 0.0174532924f;
			this.m_angle = cInfo.startAngle;
		}

		public override void Reset()
		{
			this.m_angle = 0f;
		}

		private void Start()
		{
		}

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			this.m_angle += this.m_hovering_speed * deltaTime;
			if (this.m_angle > 6.28318548f)
			{
				this.m_angle -= 6.28318548f;
			}
			this.m_hovering_pos.y = this.m_hovering_height * Mathf.Sin(this.m_angle);
			base.Position = this.m_hovering_pos;
		}
	}
}
