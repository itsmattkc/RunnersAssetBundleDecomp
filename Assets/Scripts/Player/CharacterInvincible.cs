using Message;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterInvincible : MonoBehaviour
	{
		private const string EffectNameS = "ef_pl_invincible_s01";

		private const string EffectNameL = "ef_pl_invincible_l01";

		private GameObject m_effect;

		private float m_time;

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
			this.m_effect = StateUtil.CreateEffect(this, (!this.m_bigSize) ? "ef_pl_invincible_s01" : "ef_pl_invincible_l01", false);
			if (base.transform.parent)
			{
				CapsuleCollider component = base.transform.parent.GetComponent<CapsuleCollider>();
				if (component)
				{
					base.transform.localPosition = component.center;
				}
			}
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
			if (this.m_time > 0f)
			{
				this.m_time -= Time.deltaTime;
				if (this.m_time <= 0f)
				{
					this.SetDisable();
				}
			}
		}

		public void SetEnable()
		{
			this.m_time = -1f;
		}

		public void SetActive(float time)
		{
			base.gameObject.SetActive(true);
			if (this.m_effect != null && !this.m_effect.activeInHierarchy)
			{
				this.m_effect.SetActive(true);
			}
			this.SetTime(time);
			MsgInvincible value = new MsgInvincible(MsgInvincible.Mode.Start);
			GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgInvincible", value, SendMessageOptions.DontRequireReceiver);
			if (StageAbilityManager.Instance != null)
			{
				StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.ITEM_TIME);
			}
		}

		public void SetDisable()
		{
			StateUtil.SendMessageToTerminateItem(ItemType.INVINCIBLE);
			base.gameObject.SetActive(false);
			GameObject gameObject = base.gameObject.transform.parent.gameObject;
			StateUtil.CreateEffect(this, gameObject, "ef_pl_invincible_cancel_s01", true, ResourceCategory.COMMON_EFFECT);
			MsgInvincible value = new MsgInvincible(MsgInvincible.Mode.End);
			GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgInvincible", value, SendMessageOptions.DontRequireReceiver);
		}

		public void SetNotDraw(bool value)
		{
			if (this.m_effect != null)
			{
				bool flag = !value;
				if (this.m_effect.activeInHierarchy != flag)
				{
					this.m_effect.SetActive(flag);
				}
			}
		}

		public void SetTime(float time)
		{
			this.m_time = time;
		}
	}
}
