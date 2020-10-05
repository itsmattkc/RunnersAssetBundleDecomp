using System;
using UnityEngine;

[ExecuteInEditMode]
public class TenseEffectManager : MonoBehaviour
{
	public enum Type
	{
		TENSE_A,
		TENSE_B
	}

	[SerializeField]
	private TenseEffectManager.Type m_nowType;

	private bool m_notChangeTense;

	private static TenseEffectManager instance;

	public bool NotChangeTense
	{
		get
		{
			return this.m_notChangeTense;
		}
		set
		{
			this.m_notChangeTense = value;
		}
	}

	public static TenseEffectManager Instance
	{
		get
		{
			if (TenseEffectManager.instance == null)
			{
				TenseEffectManager.instance = GameObjectUtil.FindGameObjectComponent<TenseEffectManager>("TenseEffectManager");
			}
			return TenseEffectManager.instance;
		}
	}

	private void Awake()
	{
		this.CheckInstance();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetType(TenseEffectManager.Type t)
	{
		this.m_nowType = t;
	}

	public void FlipTenseType()
	{
		if (!this.NotChangeTense)
		{
			this.m_nowType = ((this.m_nowType != TenseEffectManager.Type.TENSE_A) ? TenseEffectManager.Type.TENSE_A : TenseEffectManager.Type.TENSE_B);
		}
	}

	public TenseEffectManager.Type GetTenseType()
	{
		return this.m_nowType;
	}

	protected bool CheckInstance()
	{
		if (TenseEffectManager.instance == null)
		{
			TenseEffectManager.instance = this;
			return true;
		}
		if (this == TenseEffectManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (TenseEffectManager.instance == this)
		{
			TenseEffectManager.instance = null;
		}
	}
}
