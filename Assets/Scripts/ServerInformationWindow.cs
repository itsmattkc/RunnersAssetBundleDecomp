using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class ServerInformationWindow : MonoBehaviour
{
	private enum State
	{
		IDLE,
		INIT,
		SETUP,
		SETUP_END,
		PLAYING,
		WAIT_PLAY,
		END
	}

	private InformationWindow m_window;

	private GameObject m_windowObj;

	private List<NetNoticeItem> m_infos = new List<NetNoticeItem>();

	private int m_current_info;

	private bool m_playStartCue;

	private bool m_saveFlag;

	private ServerInformationWindow.State m_state;

	public bool IsReady
	{
		get
		{
			return this.m_state != ServerInformationWindow.State.IDLE && this.m_state != ServerInformationWindow.State.INIT && this.m_state != ServerInformationWindow.State.SETUP;
		}
		private set
		{
		}
	}

	public bool IsEnd()
	{
		return this.m_state == ServerInformationWindow.State.END;
	}

	private void Start()
	{
		if (ServerInterface.NoticeInfo != null)
		{
			List<NetNoticeItem> noticeItems = ServerInterface.NoticeInfo.m_noticeItems;
			if (noticeItems != null)
			{
				this.m_infos = new List<NetNoticeItem>();
				if (this.m_infos != null)
				{
					foreach (NetNoticeItem current in noticeItems)
					{
						if (current != null && current.Id != (long)NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID && current.Id != (long)NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID && !current.IsOnlyInformationPage() && !ServerInterface.NoticeInfo.IsChecked(current) && ServerInterface.NoticeInfo.IsOnTime(current))
						{
							this.m_infos.Add(current);
						}
					}
					if (this.m_infos.Count > 0)
					{
						this.m_state = ServerInformationWindow.State.INIT;
					}
				}
			}
		}
	}

	private void SetWindowData()
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			this.m_windowObj = GameObjectUtil.FindChildGameObject(cameraUIObject, "NewsWindow");
			if (this.m_windowObj != null)
			{
				this.m_windowObj.SetActive(false);
			}
		}
	}

	private void Update()
	{
		switch (this.m_state)
		{
		case ServerInformationWindow.State.INIT:
			this.SetWindowData();
			if (this.m_playStartCue)
			{
				this.m_playStartCue = false;
				this.CreateInformationWindow();
				this.m_state = ServerInformationWindow.State.PLAYING;
			}
			break;
		case ServerInformationWindow.State.PLAYING:
			if (this.m_window != null)
			{
				if (this.m_window.IsButtonPress(InformationWindow.ButtonType.LEFT))
				{
					this.m_state = ServerInformationWindow.State.WAIT_PLAY;
				}
				else if (this.m_window.IsButtonPress(InformationWindow.ButtonType.RIGHT))
				{
					this.m_state = ServerInformationWindow.State.WAIT_PLAY;
				}
				else if (this.m_window.IsButtonPress(InformationWindow.ButtonType.CLOSE))
				{
					this.m_state = ServerInformationWindow.State.WAIT_PLAY;
				}
			}
			break;
		case ServerInformationWindow.State.WAIT_PLAY:
			if (this.m_window != null && this.m_window.IsEnd())
			{
				this.UpdateInformaitionSaveData();
				if (this.HasNext())
				{
					this.PlayNext();
				}
				else
				{
					this.DestroyInformationWindow();
					this.m_state = ServerInformationWindow.State.END;
				}
			}
			break;
		}
	}

	public void Clear()
	{
		this.m_current_info = 0;
	}

	public bool HasNext()
	{
		if (this.m_infos != null)
		{
			int num = this.m_current_info + 1;
			if (num < this.m_infos.Count)
			{
				return true;
			}
		}
		return false;
	}

	public void PlayStart()
	{
		if (this.m_infos != null && this.m_infos.Count > 0)
		{
			if (this.m_state != ServerInformationWindow.State.INIT)
			{
				this.CreateInformationWindow();
				this.m_state = ServerInformationWindow.State.PLAYING;
			}
			else
			{
				this.m_playStartCue = true;
			}
		}
		else
		{
			this.m_state = ServerInformationWindow.State.END;
		}
	}

	public void PlayNext()
	{
		if (!this.HasNext())
		{
			return;
		}
		this.m_current_info++;
		this.CreateInformationWindow();
		this.m_state = ServerInformationWindow.State.PLAYING;
	}

	public void SetSaveFlag()
	{
		this.m_saveFlag = true;
	}

	private void CreateInformationWindow()
	{
		this.m_window = base.gameObject.GetComponent<InformationWindow>();
		if (this.m_window == null)
		{
			this.m_window = base.gameObject.AddComponent<InformationWindow>();
		}
		if (this.m_window != null)
		{
			NetNoticeItem netNoticeItem = this.m_infos[this.m_current_info];
			InformationWindow.Information info = default(InformationWindow.Information);
			if (netNoticeItem.WindowType == 0 || netNoticeItem.WindowType == 16)
			{
				info.pattern = InformationWindow.ButtonPattern.TEXT;
				info.imageId = "-1";
			}
			else
			{
				info.pattern = InformationWindow.ButtonPattern.OK;
				info.imageId = netNoticeItem.ImageId;
			}
			info.caption = TextUtility.GetCommonText("Informaion", "announcement");
			info.bodyText = netNoticeItem.Message;
			this.m_window.Create(info, this.m_windowObj);
		}
	}

	private void UpdateInformaitionSaveData()
	{
		ServerNoticeInfo noticeInfo = ServerInterface.NoticeInfo;
		if (noticeInfo != null)
		{
			noticeInfo.UpdateChecked(this.m_infos[this.m_current_info]);
			if (this.m_saveFlag)
			{
				noticeInfo.SaveInformation();
			}
		}
	}

	private void DestroyInformationWindow()
	{
		if (this.m_window != null)
		{
			UnityEngine.Object.Destroy(this.m_window);
			this.m_window = null;
		}
	}
}
