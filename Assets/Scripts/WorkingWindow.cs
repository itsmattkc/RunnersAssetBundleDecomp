using Message;
using System;
using UnityEngine;

public class WorkingWindow : MonoBehaviour
{
	private GameObject m_window_obj;

	private bool m_update_flag;

	private void Start()
	{
	}

	public void CreateWindow(string caption)
	{
		this.m_window_obj = GeneralWindow.Create(new GeneralWindow.CInfo
		{
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = caption,
			message = "         CLOSED \n DUE TO CONSTRUCTION",
			anchor_path = "Camera/menu_Anim/MainMenuUI4/Anchor_5_MC"
		});
		this.m_update_flag = true;
	}

	public void Update()
	{
		if (this.m_update_flag && this.m_window_obj != null && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
			this.m_window_obj = null;
			this.m_update_flag = false;
			this.SendMessage();
		}
	}

	public void SendMessage()
	{
		MsgMenuSequence msgMenuSequence = new MsgMenuSequence(MsgMenuSequence.SequeneceType.MAIN);
		if (msgMenuSequence != null)
		{
			GameObjectUtil.SendMessageFindGameObject("MainMenu", "OnMsgReceive", msgMenuSequence, SendMessageOptions.DontRequireReceiver);
		}
	}
}
