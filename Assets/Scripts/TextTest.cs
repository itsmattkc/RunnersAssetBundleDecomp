using System;
using Text;
using UnityEngine;

public class TextTest : MonoBehaviour
{
	public string m_labelName = "mission001";

	private UILabel m_label;

	private void Start()
	{
		this.m_label = GameObject.Find("TextTestLabel").GetComponent<UILabel>();
		if (this.m_label == null)
		{
			return;
		}
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "mission", this.m_labelName).text;
		this.m_label.text = text;
	}

	private void Update()
	{
	}
}
