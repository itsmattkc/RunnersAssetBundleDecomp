using System;
using UnityEngine;

[ExecuteInEditMode]
public class TenseEffect : MonoBehaviour
{
	[SerializeField]
	private string m_tenseTypeA = "DEFAULT";

	[SerializeField]
	private string m_tenseTypeB = "DEFAULT";

	private Color m_TenseColorA = Color.white;

	private Color m_TenseColorB = Color.white;

	private MaterialPropertyBlock m_MaterialProperty;

	private TenseEffectManager.Type m_tenseType;

	private void Start()
	{
		this.m_MaterialProperty = new MaterialPropertyBlock();
		this.m_TenseColorA = TenseEffectTable.GetItemData(this.m_tenseTypeA);
		this.m_TenseColorB = TenseEffectTable.GetItemData(this.m_tenseTypeB);
		if (TenseEffectManager.Instance != null)
		{
			this.m_tenseType = TenseEffectManager.Instance.GetTenseType();
		}
		Color color = (this.m_tenseType != TenseEffectManager.Type.TENSE_A) ? this.m_TenseColorB : this.m_TenseColorA;
		this.ModifyMaterialLightColor(color);
	}

	private void Update()
	{
	}

	private void ModifyMaterialLightColor(Color color)
	{
		if (this.m_MaterialProperty != null)
		{
			this.m_MaterialProperty.Clear();
			this.m_MaterialProperty.AddColor("_AmbientColor", color);
		}
		GetComponent<Renderer>().SetPropertyBlock(this.m_MaterialProperty);
	}
}
