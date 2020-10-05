using App;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterMagnet : MonoBehaviour
	{
		private const string EffectNameS = "ef_pl_magnet_s01";

		private const string EffectNameL = "ef_pl_magnet_l01";

		private GameObject m_effect;

		private Vector3 m_defaultOffset;

		private float m_defaultRadius;

		private float m_time;

		private bool m_bigSize;

		[SerializeField]
		private bool m_forChaoAbility;

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

		public bool ForChaoAbility
		{
			get
			{
				return this.m_forChaoAbility;
			}
		}

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

		public void SetEnable()
		{
			base.gameObject.SetActive(true);
			this.m_time = -1f;
			if (this.m_forChaoAbility)
			{
				return;
			}
			this.m_effect = StateUtil.CreateEffect(this, (!this.m_bigSize) ? "ef_pl_magnet_s01" : "ef_pl_magnet_l01", false);
			if (this.m_effect != null)
			{
				StateUtil.SetObjectLocalPositionToCenter(this, this.m_effect);
			}
			if (StageAbilityManager.Instance != null)
			{
				StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.MAGNET_TIME);
				StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.ITEM_TIME);
				StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.MAGNET_RANGE);
			}
			SoundManager.SePlay("obj_magnet", "SE");
		}

		public void SetDisable()
		{
			if (!this.m_forChaoAbility)
			{
				StateUtil.SendMessageToTerminateItem(ItemType.MAGNET);
			}
			if (this.m_effect != null)
			{
				StateUtil.DestroyParticle(this.m_effect, 1f);
				this.m_effect = null;
			}
			base.gameObject.SetActive(false);
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
				}
			}
		}

		public void SetDefaultRadiusAndOffset()
		{
			this.SetRadiusAndOffset(this.m_defaultRadius, this.m_defaultOffset);
		}

		public void SetTime(float time)
		{
			this.m_time = time;
		}

		private void OnTriggerEnter(Collider other)
		{
			MsgOnDrawingRings value = new MsgOnDrawingRings();
			other.gameObject.SendMessage("OnDrawingRings", value, SendMessageOptions.DontRequireReceiver);
		}
	}
}
