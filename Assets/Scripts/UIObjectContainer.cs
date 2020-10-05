using System;
using UnityEngine;

public class UIObjectContainer : MonoBehaviour
{
	[SerializeField]
	private HudMenuUtility.EffectPriority m_priority;

	[SerializeField]
	private GameObject[] m_objects = new GameObject[0];

	private bool[] m_activeFlags;

	public HudMenuUtility.EffectPriority Priority
	{
		get
		{
			return this.m_priority;
		}
		set
		{
			this.m_priority = value;
		}
	}

	public GameObject[] Objects
	{
		get
		{
			return this.m_objects;
		}
		set
		{
			this.m_objects = value;
		}
	}

	public void SetActive(bool isActive)
	{
		if (this.m_activeFlags == null)
		{
			return;
		}
		if (this.m_objects.Length != this.m_activeFlags.Length)
		{
			return;
		}
		for (int i = 0; i < this.m_objects.Length; i++)
		{
			if (!(this.m_objects[i] == null))
			{
				if (isActive)
				{
					if (!this.m_objects[i].activeSelf && this.m_activeFlags[i])
					{
						this.m_objects[i].SetActive(true);
					}
				}
				else if (this.m_objects[i].activeSelf)
				{
					this.m_activeFlags[i] = true;
					this.m_objects[i].SetActive(false);
				}
			}
		}
	}

	private void Start()
	{
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.AddEffect(this);
		}
		if (this.m_objects.Length > 0)
		{
			this.m_activeFlags = new bool[this.m_objects.Length];
			if (this.m_activeFlags != null)
			{
				for (int i = 0; i < this.m_activeFlags.Length; i++)
				{
					this.m_activeFlags[i] = false;
				}
			}
		}
		base.enabled = false;
	}

	private void OnDestroy()
	{
		this.m_objects = null;
		this.m_activeFlags = null;
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.RemoveEffect(this);
		}
	}

	private void Update()
	{
	}
}
