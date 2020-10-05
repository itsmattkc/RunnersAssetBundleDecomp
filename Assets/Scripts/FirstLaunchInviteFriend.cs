using SaveData;
using System;
using Text;
using UnityEngine;

public class FirstLaunchInviteFriend : MonoBehaviour
{
	private enum EventSignal
	{
		PLAY_START = 100
	}

	private TinyFsmBehavior m_fsm;

	private float m_timer;

	private bool m_isEndPlay;

	private string m_anchorPath;

	public bool IsEndPlay
	{
		get
		{
			return this.m_isEndPlay;
		}
		private set
		{
		}
	}

	public void Setup(string anchorPath)
	{
		this.m_anchorPath = anchorPath;
	}

	public void PlayStart()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
		this.m_isEndPlay = false;
	}

	private void Start()
	{
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
	}

	private void Update()
	{
	}

	private TinyFsmState StateIdle(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInviteFriendWindow)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateInviteFriendWindow(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
			info.caption = TextUtility.GetCommonText("FaceBook", "ui_Lbl_facebook_invite_friend_caption");
			int num = 5;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				ServerSettingState settingState = ServerInterface.SettingState;
				if (settingState != null)
				{
					num = settingState.m_invitBaseIncentive.m_num;
				}
			}
			string commonText = TextUtility.GetCommonText("FaceBook", "ui_Lbl_facebook_invite_friend_text", "{RED_STAR_RING_NUM}", num.ToString());
			info.message = commonText;
			info.anchor_path = this.m_anchorPath;
			info.buttonType = GeneralWindow.ButtonType.Ok;
			GeneralWindow.Create(info);
			return TinyFsmState.End();
		}
		case 4:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateDisplayTutorialCursor)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateDisplayTutorialCursor(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.OPTION);
			this.m_timer = 3f;
			return TinyFsmState.End();
		case 4:
			this.m_timer -= Time.deltaTime;
			if (TutorialCursor.IsTouchScreen() || this.m_timer < 0f)
			{
				TutorialCursor.DestroyTutorialCursor();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.INVITE_FRIEND_SEQUENCE_END);
			this.m_isEndPlay = true;
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}
}
