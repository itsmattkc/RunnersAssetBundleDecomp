using System;
using UnityEngine;

public class PnoteRegistration : MonoBehaviour
{
	private bool m_enable = true;

	private void Start()
	{
		this.m_enable = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (this.m_enable)
		{
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
