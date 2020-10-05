using System;
using UnityEngine;

public class ykTextureSheetAnimation : MonoBehaviour
{
	[SerializeField]
	private int x = 1;

	[SerializeField]
	private int y = 1;

	[SerializeField]
	private int first;

	[SerializeField]
	private int count;

	[SerializeField]
	private float speed = 1f;

	private float tOffset;

	private int nOffset;

	private Material _material;

	private void Start()
	{
		this._material = this.GetMaterial();
	}

	private void Update()
	{
		this.tOffset += Time.deltaTime * this.speed;
		if (this.count <= 0)
		{
			this.nOffset = (int)Mathf.Repeat(this.tOffset, (float)this.x * (float)this.y) + this.first;
		}
		else
		{
			this.nOffset = (int)Mathf.Repeat(this.tOffset, (float)this.count) + this.first;
		}
		float num = Mathf.Repeat((float)this.nOffset, (float)this.x);
		float num2 = (float)(this.y - Mathf.FloorToInt((float)((this.nOffset + this.x) / this.x)));
		Vector2 mainTextureScale = new Vector2(1f / (float)this.x, 1f / (float)this.y);
		this._material.mainTextureScale = mainTextureScale;
		Vector2 mainTextureOffset = new Vector2(mainTextureScale.x * num, mainTextureScale.y * num2);
		this._material.mainTextureOffset = mainTextureOffset;
	}

	protected virtual Material GetMaterial()
	{
		return GetComponent<Renderer>().material;
	}

	protected virtual bool IsValidChange()
	{
		return true;
	}

	public void SetSpeed(float in_speed)
	{
		if (this.IsValidChange())
		{
			this.speed = in_speed;
		}
	}
}
