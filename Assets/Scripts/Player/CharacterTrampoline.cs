using System;
using UnityEngine;

namespace Player
{
	public class CharacterTrampoline : MonoBehaviour
	{
		private bool m_requestEnd;

		private GameObject m_effect;

		private float m_time;

		private void Start()
		{
		}

		private void OnEnable()
		{
			this.m_requestEnd = false;
			this.m_effect = StateUtil.CreateEffect(this, "ef_pl_trampoline_s01", false);
			StateUtil.SetObjectLocalPositionToCenter(this, this.m_effect);
		}

		private void OnDisable()
		{
			if (this.m_effect != null)
			{
				StateUtil.DestroyParticle(this.m_effect, 1f);
				this.m_effect = null;
			}
		}

		public void SetEnable()
		{
			base.gameObject.SetActive(true);
		}

		public void SetDisable()
		{
			StateUtil.SendMessageToTerminateItem(ItemType.TRAMPOLINE);
			if (this.m_effect != null)
			{
				StateUtil.DestroyParticle(this.m_effect, 1f);
				this.m_effect = null;
			}
			base.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (this.m_requestEnd)
			{
				base.gameObject.SetActive(false);
				return;
			}
			if (this.m_time > 0f)
			{
				this.m_time -= Time.deltaTime;
				if (this.m_time <= 0f)
				{
					base.gameObject.SetActive(false);
				}
			}
		}

		public void SetTime(float time)
		{
			this.m_time = time;
		}

		public void RequestEnd()
		{
			this.m_requestEnd = true;
		}
	}
}
