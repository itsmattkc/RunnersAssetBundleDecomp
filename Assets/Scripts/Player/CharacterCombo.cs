using System;
using UnityEngine;

namespace Player
{
	public class CharacterCombo : MonoBehaviour
	{
		private CharacterMovement m_movement;

		private GameObject m_effect;

		private PlayerInformation m_information;

		private float m_time;

		private bool m_requestEnd;

		private void Start()
		{
			this.m_movement = base.transform.parent.gameObject.GetComponent<CharacterMovement>();
			this.m_information = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
		}

		private void OnEnable()
		{
			this.SetCombo(true);
		}

		private void OnDisable()
		{
			this.SetCombo(false);
		}

		public void SetEnable()
		{
			base.gameObject.SetActive(true);
			this.m_time = -1f;
			this.m_effect = StateUtil.CreateEffect(this, "ef_pl_combobonus01", false);
			if (this.m_effect != null)
			{
				StateUtil.SetObjectLocalPositionToCenter(this, this.m_effect);
			}
			if (StageAbilityManager.Instance != null)
			{
				StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.COMBO_TIME);
				StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.ITEM_TIME);
			}
			SoundManager.SePlay("obj_combo_loop", "SE");
		}

		public void SetDisable()
		{
			StateUtil.SendMessageToTerminateItem(ItemType.COMBO);
			if (this.m_effect != null)
			{
				StateUtil.DestroyParticle(this.m_effect, 1f);
				this.m_effect = null;
			}
			base.gameObject.SetActive(false);
			this.m_requestEnd = false;
		}

		private void Update()
		{
			if (this.m_movement != null && this.m_movement.IsOnGround() && this.m_requestEnd)
			{
				this.SetDisable();
			}
			if (this.m_time > 0f)
			{
				this.m_time -= Time.deltaTime;
				if (this.m_time <= 0f)
				{
					this.RequestEnd();
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

		private void SetCombo(bool flag)
		{
			if (this.m_information == null)
			{
				this.m_information = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
			}
			if (this.m_information != null)
			{
				this.m_information.SetCombo(flag);
			}
		}
	}
}
