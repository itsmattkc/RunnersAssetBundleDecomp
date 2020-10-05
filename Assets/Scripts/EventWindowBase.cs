using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventWindowBase : MonoBehaviour
{
	[SerializeField]
	protected List<GameObject> anchorObjects;

	protected Dictionary<string, UILabel> m_objectLabels;

	protected Dictionary<string, UISprite> m_objectSprites;

	protected Dictionary<string, UITexture> m_objectTextures;

	protected Dictionary<string, GameObject> m_objects;

	protected bool m_isSetObject;

	public bool enabledAnchorObjects
	{
		get
		{
			bool result = false;
			if (this.anchorObjects != null)
			{
				foreach (GameObject current in this.anchorObjects)
				{
					if (current.activeSelf)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
		set
		{
			if (this.anchorObjects != null)
			{
				foreach (GameObject current in this.anchorObjects)
				{
					current.SetActive(value);
				}
			}
		}
	}

	protected abstract void SetObject();

	public void AnimationFinishi()
	{
		this.enabledAnchorObjects = false;
	}
}
