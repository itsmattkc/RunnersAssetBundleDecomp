using System;
using UnityEngine;

namespace Player
{
	public class CharacterBarrier : MonoBehaviour
	{
		private GameObject m_effect;

		private bool m_bigSize;

		public bool IsBigSize
		{
			get
			{
				return this.m_bigSize;
			}
			set
			{
				this.m_bigSize = value;
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void SetEnable()
		{
			base.gameObject.SetActive(true);
			this.m_effect = StateUtil.CreateEffect(this, (!this.m_bigSize) ? "ef_pl_barrier_lv1_s01" : "ef_pl_barrier_lv1_l01", false);
			if (this.m_effect != null)
			{
				StateUtil.SetObjectLocalPositionToCenter(this, this.m_effect);
			}
			SoundManager.SePlay("obj_item_barrier", "SE");
		}

		public void SetDisable()
		{
			if (this.m_effect != null)
			{
				UnityEngine.Object.Destroy(this.m_effect);
				this.m_effect = null;
			}
			base.gameObject.SetActive(false);
		}

		private void Stop()
		{
			this.SetDisable();
			SoundManager.SePlay("obj_item_barrier_brk", "SE");
			GameObject gameObject = base.gameObject.transform.parent.gameObject;
			StateUtil.CreateEffect(this, gameObject, "ef_pl_barrier_cancel_s01", true, ResourceCategory.COMMON_EFFECT);
		}

		public void SetNotDraw(bool value)
		{
			if (this.m_effect != null)
			{
				this.m_effect.SetActive(!value);
			}
		}

		public void Damaged()
		{
			this.Stop();
		}
	}
}
