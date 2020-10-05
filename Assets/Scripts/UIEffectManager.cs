using System;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectManager : MonoBehaviour
{
	private static UIEffectManager m_instance;

	private List<UIObjectContainer>[] m_effectList;

	public static UIEffectManager Instance
	{
		get
		{
			return UIEffectManager.m_instance;
		}
		private set
		{
		}
	}

	public void AddEffect(UIObjectContainer container)
	{
		if (container == null)
		{
			return;
		}
		HudMenuUtility.EffectPriority priority = container.Priority;
		if (priority >= HudMenuUtility.EffectPriority.Num)
		{
			return;
		}
		this.m_effectList[(int)priority].Add(container);
	}

	public void RemoveEffect(UIObjectContainer container)
	{
		if (container == null)
		{
			return;
		}
		HudMenuUtility.EffectPriority priority = container.Priority;
		if (priority >= HudMenuUtility.EffectPriority.Num)
		{
			return;
		}
		this.m_effectList[(int)priority].Remove(container);
	}

	public void SetActiveEffect(HudMenuUtility.EffectPriority priority, bool isActive)
	{
		for (int i = 0; i <= (int)priority; i++)
		{
			foreach (UIObjectContainer current in this.m_effectList[i])
			{
				if (!(current == null))
				{
					current.SetActive(isActive);
				}
			}
		}
	}

	private void Awake()
	{
		if (UIEffectManager.m_instance == null)
		{
			UIEffectManager.m_instance = this;
			this.Init();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void Init()
	{
		this.m_effectList = new List<UIObjectContainer>[4];
		for (int i = 0; i < 4; i++)
		{
			this.m_effectList[i] = new List<UIObjectContainer>();
		}
	}
}
