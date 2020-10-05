using System;
using UnityEngine;

public class ChaoPartsObjectMagnet : MonoBehaviour
{
	public enum HitType
	{
		RING,
		CRYSTAL
	}

	private enum Mode
	{
		Idle,
		Magnet,
		Respite
	}

	public float m_colliRadius = 2.5f;

	public float m_magnetRadius = 4f;

	public string m_effectName = string.Empty;

	public string m_hitLayer = string.Empty;

	public ChaoPartsObjectMagnet.HitType m_hitType;

	private SphereCollider m_collider;

	private GameObject m_magnetObj;

	private ChaoMagnet m_magnet;

	private GameObject m_effect;

	private Animator m_animator;

	private ChaoPartsObjectMagnet.Mode m_mode;

	private float m_time;

	private bool m_pauseFlag;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_pauseFlag)
		{
			return;
		}
		ChaoPartsObjectMagnet.Mode mode = this.m_mode;
		if (mode != ChaoPartsObjectMagnet.Mode.Magnet)
		{
			if (mode == ChaoPartsObjectMagnet.Mode.Respite)
			{
				this.m_time -= Time.deltaTime;
				if (this.m_time < 0f)
				{
					this.SetDisable();
					this.m_mode = ChaoPartsObjectMagnet.Mode.Idle;
				}
			}
		}
		else
		{
			this.m_time -= Time.deltaTime;
			if (this.m_time < 0f)
			{
				this.SetRespite();
				this.m_time = 1f;
				this.m_mode = ChaoPartsObjectMagnet.Mode.Respite;
			}
		}
	}

	public void Setup()
	{
		string layerName = "HitRing";
		ChaoPartsObjectMagnet.HitType hitType = this.m_hitType;
		if (hitType != ChaoPartsObjectMagnet.HitType.RING)
		{
			if (hitType == ChaoPartsObjectMagnet.HitType.CRYSTAL)
			{
				layerName = "HitCrystal";
			}
		}
		base.gameObject.layer = LayerMask.NameToLayer(layerName);
		this.m_animator = base.gameObject.GetComponent<Animator>();
		this.m_collider = base.gameObject.AddComponent<SphereCollider>();
		if (this.m_collider != null)
		{
			this.m_collider.radius = this.m_colliRadius;
			this.m_collider.isTrigger = true;
			this.m_collider.enabled = false;
		}
		this.m_magnetObj = new GameObject();
		if (this.m_magnetObj != null)
		{
			this.m_magnetObj.name = "magnet";
			this.m_magnetObj.transform.parent = base.gameObject.transform;
			this.m_magnetObj.layer = LayerMask.NameToLayer(layerName);
			this.m_magnet = this.m_magnetObj.AddComponent<ChaoMagnet>();
			if (this.m_magnet != null)
			{
				this.m_magnet.Setup(this.m_magnetRadius, this.m_hitLayer);
			}
		}
		base.enabled = false;
	}

	public void SetEnable(float time)
	{
		this.m_time = time;
		if (this.m_effect != null)
		{
			UnityEngine.Object.Destroy(this.m_effect);
			this.m_effect = null;
		}
		if (!string.IsNullOrEmpty(this.m_effectName))
		{
			this.m_effect = ObjUtil.PlayChaoEffect(base.gameObject, this.m_effectName, -1f);
		}
		if (this.m_collider != null)
		{
			this.m_collider.enabled = true;
		}
		if (this.m_magnet != null)
		{
			this.m_magnet.SetEnable(true);
		}
		SoundManager.SePlay("obj_magnet", "SE");
		this.m_mode = ChaoPartsObjectMagnet.Mode.Magnet;
		base.enabled = true;
		if (this.m_pauseFlag)
		{
			this.SetPause(this.m_pauseFlag);
		}
		else
		{
			this.SetAnimation(true);
		}
	}

	public void SetPause(bool flag)
	{
		this.m_pauseFlag = flag;
		ChaoPartsObjectMagnet.Mode mode = this.m_mode;
		if (mode != ChaoPartsObjectMagnet.Mode.Magnet)
		{
			if (mode != ChaoPartsObjectMagnet.Mode.Respite)
			{
			}
		}
		else
		{
			this.m_effect.SetActive(!this.m_pauseFlag);
			if (this.m_magnet != null)
			{
				this.m_magnet.SetEnable(!this.m_pauseFlag);
			}
			this.SetAnimation(!this.m_pauseFlag);
			if (!this.m_pauseFlag)
			{
				SoundManager.SePlay("obj_magnet", "SE");
			}
		}
	}

	private void SetRespite()
	{
		if (this.m_effect != null)
		{
			UnityEngine.Object.Destroy(this.m_effect);
			this.m_effect = null;
		}
		if (this.m_magnet != null)
		{
			this.m_magnet.SetEnable(false);
		}
		this.SetAnimation(false);
	}

	private void SetDisable()
	{
		if (this.m_collider != null)
		{
			this.m_collider.enabled = false;
		}
		this.SetAnimation(false);
		base.enabled = false;
	}

	private void SetAnimation(bool flag)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool("Ability", flag);
		}
	}
}
