using System;
using UnityEngine;

namespace Player
{
	public class CharacterBlinkTimer : MonoBehaviour
	{
		private float m_timer;

		private CharacterState m_context;

		private void Start()
		{
		}

		private void OnDestroy()
		{
			this.End();
		}

		public void Setup(CharacterState ctx, float damageTime)
		{
			this.m_context = ctx;
			this.m_timer = damageTime;
			this.m_context.SetStatus(Status.Damaged, true);
			base.enabled = true;
		}

		public void End()
		{
			if (this.m_context)
			{
				this.m_context.SetVisibleBlink(false);
				this.m_context.SetStatus(Status.Damaged, false);
			}
			base.enabled = false;
		}

		private void FixedUpdate()
		{
			if (this.m_context == null)
			{
				return;
			}
			this.m_timer -= Time.deltaTime;
			if (this.m_timer <= 0f)
			{
				this.End();
				return;
			}
			if (Mathf.FloorToInt(this.m_timer * 100f) % 20 > 10)
			{
				this.m_context.SetVisibleBlink(true);
			}
			else
			{
				this.m_context.SetVisibleBlink(false);
			}
		}
	}
}
