using System;
using UnityEngine;

public class ykKillTime : MonoBehaviour
{
	public enum ykKillType
	{
		destroy,
		hide,
		deactivate
	}

	private float passedTime;

	public float killTime = 1f;

	public ykKillTime.ykKillType killType;

	private void Start()
	{
		this.passedTime = 0f;
	}

	private void Update()
	{
		this.passedTime += Time.deltaTime;
		if (this.passedTime > this.killTime)
		{
			switch (this.killType)
			{
			case ykKillTime.ykKillType.destroy:
				UnityEngine.Object.Destroy(base.gameObject);
				break;
			case ykKillTime.ykKillType.hide:
			{
				Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
				Renderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Renderer renderer = array[i];
					renderer.enabled = false;
				}
				break;
			}
			case ykKillTime.ykKillType.deactivate:
				base.gameObject.SetActive(false);
				break;
			}
		}
	}
}
