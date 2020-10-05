using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	public UISprite target;

	public string normalSprite;

	public string hoverSprite;

	public string pressedSprite;

	public string disabledSprite;

	public bool isEnabled
	{
		get
		{
			Collider collider = GetComponent<Collider>();
			return collider && collider.enabled;
		}
		set
		{
			Collider collider = GetComponent<Collider>();
			if (!collider)
			{
				return;
			}
			if (collider.enabled != value)
			{
				collider.enabled = value;
				this.UpdateImage();
			}
		}
	}

	private void OnEnable()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
		this.UpdateImage();
	}

	private void UpdateImage()
	{
		if (this.target != null)
		{
			if (this.isEnabled)
			{
				this.target.spriteName = ((!UICamera.IsHighlighted(base.gameObject)) ? this.normalSprite : this.hoverSprite);
			}
			else
			{
				this.target.spriteName = this.disabledSprite;
			}
			this.target.MakePixelPerfect();
		}
	}

	private void OnHover(bool isOver)
	{
		if (this.isEnabled && this.target != null)
		{
			this.target.spriteName = ((!isOver) ? this.normalSprite : this.hoverSprite);
			this.target.MakePixelPerfect();
		}
	}

	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			this.target.spriteName = this.pressedSprite;
			this.target.MakePixelPerfect();
		}
		else
		{
			this.UpdateImage();
		}
	}
}
