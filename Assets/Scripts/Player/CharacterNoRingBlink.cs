using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class CharacterNoRingBlink : MonoBehaviour
	{
		private const string ShaderName = "ykChrLine_dme1";

		private const string ChangeParamName = "_OutlineColor";

		private const float BlinkTime = 0.5f;

		private readonly Color RedColor = Color.red;

		private Color m_defaultColor;

		private Color m_nowColor;

		private List<Material> m_materialList = new List<Material>();

		private float m_timer;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		public void SetEnable()
		{
			this.m_nowColor = this.RedColor;
			this.UpdateColor(this.m_nowColor);
			base.enabled = true;
		}

		public void SetDisable()
		{
			this.UpdateColor(this.m_defaultColor);
			base.enabled = false;
		}

		private void Update()
		{
			float num = 0.25f;
			this.m_timer += Time.deltaTime;
			if (this.m_timer < num)
			{
				this.m_nowColor = Color.Lerp(this.RedColor, this.m_defaultColor, Mathf.Clamp(this.m_timer / num, 0f, 1f));
			}
			else
			{
				this.m_nowColor = Color.Lerp(this.m_defaultColor, this.RedColor, Mathf.Clamp((this.m_timer - num) / num, 0f, 1f));
			}
			if (this.m_timer >= 0.5f)
			{
				this.m_timer = Mathf.Max(this.m_timer - 0.5f, 0f);
			}
			this.UpdateColor(this.m_nowColor);
		}

		public void Setup(GameObject model)
		{
			foreach (Transform transform in model.transform)
			{
				Renderer component = transform.GetComponent<Renderer>();
				if (component != null)
				{
					Material[] materials = component.materials;
					Material[] array = materials;
					for (int i = 0; i < array.Length; i++)
					{
						Material material = array[i];
						if (material.HasProperty("_OutlineColor"))
						{
							this.m_materialList.Add(material);
						}
					}
				}
			}
			if (this.m_materialList.Count > 0)
			{
				this.m_defaultColor = this.m_materialList[0].GetColor("_OutlineColor");
			}
		}

		public void UpdateColor(Color color)
		{
			foreach (Material current in this.m_materialList)
			{
				current.SetColor("_OutlineColor", color);
			}
		}
	}
}
