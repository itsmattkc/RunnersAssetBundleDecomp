using System;
using Text;
using UnityEngine;

namespace UI
{
	[AddComponentMenu("Scripts/UI/LocalizeText")]
	public class UILocalizeText : MonoBehaviour
	{
		private enum TagType
		{
			TAG_01,
			TAG_02,
			TAG_03,
			NUM
		}

		[Serializable]
		public class TextData
		{
			[SerializeField]
			public TextManager.TextType text_type;

			[SerializeField]
			public string group_id;

			[SerializeField]
			public string cell_id;
		}

		[Serializable]
		public class TagTextData
		{
			[SerializeField]
			public UILocalizeText.TextData text_data;

			[SerializeField]
			public string tag;
		}

		[SerializeField]
		public UILocalizeText.TextData m_main_text_data = new UILocalizeText.TextData();

		[SerializeField]
		public UILocalizeText.TagTextData[] m_tag_text_data = new UILocalizeText.TagTextData[3];

		private string m_main_text;

		public string MainText
		{
			get
			{
				return this.m_main_text;
			}
		}

		public UILocalizeText.TextData MainTextData
		{
			get
			{
				return this.m_main_text_data;
			}
			set
			{
				this.m_main_text_data = value;
			}
		}

		public UILocalizeText.TagTextData[] TagTextDatas
		{
			get
			{
				return this.m_tag_text_data;
			}
			set
			{
				this.m_tag_text_data = value;
			}
		}

		private void Start()
		{
			base.enabled = false;
			this.SetUILabelText();
		}

		public void SetUILabelText()
		{
			this.m_main_text = this.GetMainText();
			if (this.m_main_text != null)
			{
				UILabel component = base.gameObject.GetComponent<UILabel>();
				if (component != null)
				{
					component.text = this.m_main_text;
				}
			}
		}

		private string GetMainText()
		{
			if (this.m_main_text_data != null && this.m_main_text_data.group_id != null && this.m_main_text_data.cell_id != null && this.m_main_text_data.group_id != string.Empty && this.m_main_text_data.cell_id != string.Empty)
			{
				TextObject text = TextManager.GetText(this.m_main_text_data.text_type, this.m_main_text_data.group_id, this.m_main_text_data.cell_id);
				if (text != null && text.text != null)
				{
					this.ReplaceTag(ref text);
					return text.text;
				}
			}
			return null;
		}

		private string GetText(ref UILocalizeText.TextData text_data)
		{
			if (text_data != null && text_data.group_id != null && text_data.cell_id != null && text_data.group_id != string.Empty && text_data.cell_id != string.Empty)
			{
				TextObject text = TextManager.GetText(text_data.text_type, text_data.group_id, text_data.cell_id);
				if (text != null)
				{
					return text.text;
				}
			}
			return null;
		}

		private void ReplaceTag(ref TextObject text_obj)
		{
			if (this.m_tag_text_data != null && text_obj != null)
			{
				int num = this.m_tag_text_data.Length;
				for (int i = 0; i < num; i++)
				{
					if (this.m_tag_text_data[i] != null)
					{
						string text = this.GetText(ref this.m_tag_text_data[i].text_data);
						if (text != null)
						{
							text_obj.ReplaceTag(this.m_tag_text_data[i].tag, text);
						}
					}
				}
			}
		}
	}
}
