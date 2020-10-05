using System;
using UnityEngine;

namespace Player
{
	public class CharacterLoopEffect : MonoBehaviour
	{
		private string m_effectname;

		private string m_sename;

		private SoundManager.PlayId m_seID;

		private float m_stopTimer;

		private GameObject m_effect;

		private bool m_valid;

		private ResourceCategory m_category = ResourceCategory.COMMON_EFFECT;

		private void Start()
		{
			this.m_effect = StateUtil.CreateEffect(this, this.m_effectname, false, this.m_category);
		}

		private void OnEnable()
		{
			if (this.m_sename != null)
			{
				this.m_seID = SoundManager.SePlay(this.m_sename, "SE");
			}
		}

		private void OnDisable()
		{
			if (this.m_sename != null)
			{
				SoundManager.SeStop(this.m_seID);
				this.m_seID = SoundManager.PlayId.NONE;
			}
		}

		private void Update()
		{
			if (!this.m_valid)
			{
				if (this.m_stopTimer > 0f)
				{
					this.m_stopTimer -= Time.deltaTime;
				}
				else
				{
					base.gameObject.SetActive(false);
					this.m_stopTimer = 0f;
				}
			}
		}

		public void SetValid(bool valid)
		{
			if (valid && !this.m_valid)
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
				StateUtil.PlayParticle(this.m_effect);
			}
			else if (!valid && this.m_valid)
			{
				base.gameObject.SetActive(false);
			}
			this.m_valid = valid;
			this.m_stopTimer = 0f;
		}

		public void Stop(float stopTime)
		{
			if (this.m_effect != null && stopTime > 0f)
			{
				StateUtil.StopParticle(this.m_effect);
				this.m_stopTimer = stopTime;
			}
			this.m_valid = false;
		}

		public void Setup(string effectname, ResourceCategory category)
		{
			this.m_effectname = effectname;
			this.m_category = category;
		}

		public void SetSE(string sename)
		{
			this.m_sename = sename;
		}
	}
}
