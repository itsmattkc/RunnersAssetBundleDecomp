using System;
using System.Collections.Generic;
using UnityEngine;

public class RoulettePartsBase : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> m_effectList;

	protected RouletteTop m_parent;

	protected float m_roulettePartsTime;

	protected bool m_isWindow;

	protected bool m_isEffectLock;

	protected long m_partsUpdateCount;

	private float m_delayTime;

	private bool m_isEffect;

	public RouletteTop parent
	{
		get
		{
			return this.m_parent;
		}
	}

	public bool isDelay
	{
		get
		{
			return this.m_delayTime > 0f;
		}
	}

	public bool isSpin
	{
		get
		{
			return !(this.m_parent == null) && this.m_parent.isSpin;
		}
	}

	public int spinDecisionIndex
	{
		get
		{
			if (this.m_parent == null)
			{
				return -1;
			}
			return this.m_parent.spinDecisionIndex;
		}
	}

	public ServerWheelOptionsData wheel
	{
		get
		{
			if (this.m_parent == null)
			{
				return null;
			}
			return this.m_parent.wheelData;
		}
	}

	public List<GameObject> effectList
	{
		get
		{
			return this.m_effectList;
		}
	}

	public void SetDelayTime(float delay = 0.2f)
	{
		if (delay >= 0f)
		{
			if (delay > 10f)
			{
				delay = 10f;
			}
			this.m_delayTime = delay;
		}
		else
		{
			this.m_delayTime = 0f;
		}
	}

	private void Update()
	{
		this.UpdateParts();
		this.m_roulettePartsTime += Time.deltaTime;
		if (this.m_roulettePartsTime >= 3.40282347E+38f)
		{
			this.m_roulettePartsTime = 1000f;
		}
		if (this.m_delayTime > 0f)
		{
			this.m_delayTime -= Time.deltaTime;
			if (this.m_delayTime <= 0f)
			{
				this.m_delayTime = 0f;
			}
		}
		if (!this.m_isEffectLock)
		{
			if (GeneralWindow.Created || EventBestChaoWindow.Created || this.m_isWindow)
			{
				if (this.m_isEffect)
				{
					this.m_isEffect = false;
					if (this.m_effectList != null && this.m_effectList.Count > 0)
					{
						foreach (GameObject current in this.m_effectList)
						{
							current.SetActive(this.m_isEffect);
						}
					}
				}
			}
			else if (!this.m_isEffect)
			{
				this.m_isEffect = true;
				if (this.m_effectList != null && this.m_effectList.Count > 0)
				{
					foreach (GameObject current2 in this.m_effectList)
					{
						current2.SetActive(this.m_isEffect);
					}
				}
			}
		}
		else if (this.m_isEffect)
		{
			this.m_isEffect = false;
			if (this.m_effectList != null && this.m_effectList.Count > 0)
			{
				foreach (GameObject current3 in this.m_effectList)
				{
					current3.SetActive(this.m_isEffect);
				}
			}
		}
		this.m_partsUpdateCount += 1L;
	}

	protected virtual void UpdateParts()
	{
	}

	public virtual void UpdateEffectSetting()
	{
	}

	public virtual void Setup(RouletteTop parent)
	{
		this.m_isWindow = false;
		this.m_parent = parent;
		this.m_roulettePartsTime = 0f;
		this.m_delayTime = 0f;
		this.m_isEffect = true;
		if (this.m_isEffectLock)
		{
			this.m_isEffect = false;
		}
		if (this.m_effectList != null && this.m_effectList.Count > 0)
		{
			foreach (GameObject current in this.m_effectList)
			{
				current.SetActive(this.m_isEffect);
			}
		}
		this.m_partsUpdateCount = 0L;
	}

	public virtual void OnUpdateWheelData(ServerWheelOptionsData data)
	{
		this.m_roulettePartsTime = 0f;
	}

	public virtual void DestroyParts()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public virtual void OnSpinStart()
	{
	}

	public virtual void OnSpinSkip()
	{
	}

	public virtual void OnSpinDecision()
	{
	}

	public virtual void OnSpinDecisionMulti()
	{
	}

	public virtual void OnSpinEnd()
	{
	}

	public virtual void OnSpinError()
	{
	}

	public virtual void OnRouletteClose()
	{
	}

	public virtual void windowOpen()
	{
		this.m_isWindow = true;
	}

	public virtual void windowClose()
	{
		this.m_isWindow = false;
		if (!GeneralWindow.Created && !EventBestChaoWindow.Created && !this.m_isEffectLock)
		{
			this.m_isEffect = true;
			if (this.m_effectList != null && this.m_effectList.Count > 0)
			{
				foreach (GameObject current in this.m_effectList)
				{
					current.SetActive(this.m_isEffect);
				}
			}
		}
	}

	public virtual void PartsSendMessage(string mesage)
	{
	}
}
