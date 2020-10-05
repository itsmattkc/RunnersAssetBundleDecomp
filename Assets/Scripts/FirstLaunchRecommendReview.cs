using Message;
using SaveData;
using System;
using Text;
using UnityEngine;

public class FirstLaunchRecommendReview : MonoBehaviour
{
	private enum EventSignal
	{
		PLAY_START = 100,
		REVIEW_END,
		INCENTIVE_CONNECT_END
	}

	private TinyFsmBehavior m_fsm;

	private bool m_isEndPlay;

	private string m_anchorPath;

	private SendApollo m_sendApollo;

	private IncentiveWindowQueue m_windowQueue = new IncentiveWindowQueue();

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
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSendApolloStart)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateSendApolloStart(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_sendApollo != null)
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
			}
			return TinyFsmState.End();
		case 1:
		{
			string[] value = new string[1];
			SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP8, ref value);
			this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_START, value);
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateRecommendReview)));
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRecommendReview(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
			info.caption = TextUtility.GetCommonText("FaceBook", "ui_Lbl_recommend_review_caption");
			string commonText = TextUtility.GetCommonText("FaceBook", "ui_Lbl_recommend_review_text", "{RED_STAR_RING_NUM}", "5");
			info.message = commonText;
			info.anchor_path = this.m_anchorPath;
			info.buttonType = GeneralWindow.ButtonType.YesNo;
			GeneralWindow.Create(info);
			return TinyFsmState.End();
		}
		case 4:
			if (GeneralWindow.IsYesButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUploadReview)));
			}
			else if (GeneralWindow.IsNoButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateUploadReview(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			Application.OpenURL(NetBaseUtil.RedirectInstallPageUrl);
			return TinyFsmState.End();
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSendApolloEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_sendApollo != null)
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
			}
			return TinyFsmState.End();
		case 1:
		{
			string[] value = new string[1];
			SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP8, ref value);
			this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_END, value);
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
			}
			return TinyFsmState.End();
		case 5:
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
			HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.RECOMMEND_REVIEW_END);
			this.m_isEndPlay = true;
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void ServerGetFacebookIncentive_Succeeded(MsgGetNormalIncentiveSucceed msg)
	{
		foreach (ServerPresentState current in msg.m_incentive)
		{
			if (current != null)
			{
				IncentiveWindow window = new IncentiveWindow(current.m_itemId, current.m_numItem, this.m_anchorPath);
				this.m_windowQueue.AddWindow(window);
			}
		}
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(102);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}
}
