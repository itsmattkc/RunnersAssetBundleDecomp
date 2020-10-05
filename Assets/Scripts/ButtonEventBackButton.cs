using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventBackButton : MonoBehaviour
{
	public struct BtnData
	{
		public GameObject obj;

		public ButtonInfoTable.ButtonType btn_type;

		public BtnData(GameObject obj, ButtonInfoTable.ButtonType btn_type)
		{
			this.obj = obj;
			this.btn_type = btn_type;
		}
	}

	public delegate void ButtonClickedCallback(ButtonInfoTable.ButtonType buttonType);

	private ButtonInfoTable m_info_table = new ButtonInfoTable();

	private GameObject m_menu_anim_obj;

	private List<ButtonEventBackButton.BtnData> m_btn_obj_list;

	private ButtonEventBackButton.ButtonClickedCallback m_callback;

	public void Initialize(ButtonEventBackButton.ButtonClickedCallback callback)
	{
		this.m_callback = callback;
		this.m_menu_anim_obj = HudMenuUtility.GetMenuAnimUIObject();
		this.m_btn_obj_list = new List<ButtonEventBackButton.BtnData>();
		if (this.m_menu_anim_obj != null)
		{
			for (uint num = 0u; num < 49u; num += 1u)
			{
				this.SetupBackButton((ButtonInfoTable.ButtonType)num);
			}
		}
	}

	public void SetupBackButton(ButtonInfoTable.ButtonType buttonType)
	{
		for (int i = 0; i < this.m_btn_obj_list.Count; i++)
		{
			if (this.m_btn_obj_list[i].btn_type == buttonType)
			{
				return;
			}
		}
		if (string.IsNullOrEmpty(this.m_info_table.m_button_info[(int)buttonType].clickButtonPath))
		{
			return;
		}
		Transform transform = this.m_menu_anim_obj.transform.Find(this.m_info_table.m_button_info[(int)buttonType].clickButtonPath);
		if (transform == null)
		{
			return;
		}
		GameObject gameObject = transform.gameObject;
		if (gameObject == null)
		{
			return;
		}
		ButtonEventBackButton.BtnData item = new ButtonEventBackButton.BtnData(gameObject, buttonType);
		this.m_btn_obj_list.Add(item);
		UIButtonMessage component = gameObject.GetComponent<UIButtonMessage>();
		if (component == null)
		{
			gameObject.AddComponent<UIButtonMessage>();
			component = gameObject.GetComponent<UIButtonMessage>();
		}
		if (component != null)
		{
			component.enabled = true;
			component.trigger = UIButtonMessage.Trigger.OnClick;
			component.target = base.gameObject;
			component.functionName = "OnButtonClicked";
		}
		UIPlayAnimation[] components = gameObject.GetComponents<UIPlayAnimation>();
		if (components != null)
		{
			UIPlayAnimation[] array = components;
			for (int j = 0; j < array.Length; j++)
			{
				UIPlayAnimation uIPlayAnimation = array[j];
				uIPlayAnimation.target = null;
			}
		}
	}

	private void OnButtonClicked(GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this.m_callback == null)
		{
			return;
		}
		if (this.m_btn_obj_list == null)
		{
			return;
		}
		int count = this.m_btn_obj_list.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(obj != this.m_btn_obj_list[i].obj))
			{
				ButtonInfoTable.ButtonType btn_type = this.m_btn_obj_list[i].btn_type;
				this.m_callback(btn_type);
				this.m_info_table.PlaySE(btn_type);
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
