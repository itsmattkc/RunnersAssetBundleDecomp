using System;
using Text;
using UnityEngine;

public class AgeVerification : MonoBehaviour
{
	private enum State
	{
		NONE = -1,
		IDLE,
		APOLLO_SEND_START,
		APOLLO_SEND_START_WAIT,
		CAUTION_AGE_VERIFICATION,
		CAUTION_AGE_VERIFICATION_WAIT,
		AGE_VERIFICATION,
		AGE_VERIFICATION_WAIT,
		APOLLO_SEND_END,
		APOLLO_SEND_END_WAIT,
		FINISHED_AGE_VERIFICATION,
		FINISHED_AGE_VERIFICATION_WAIT,
		END,
		COUNT
	}

	private SendApollo m_sendApollo;

	private string m_anchorPath;

	private AgeVerificationWindow m_ageVerification;

	private AgeVerification.State m_state;

	public static bool IsAgeVerificated
	{
		get
		{
			ServerSettingState serverSettingState = null;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				serverSettingState = ServerInterface.SettingState;
			}
			return !string.IsNullOrEmpty(serverSettingState.m_birthday);
		}
		private set
		{
		}
	}

	public bool IsEnd
	{
		get
		{
			return this.m_state == AgeVerification.State.END;
		}
	}

	public void Setup(string anchorPath)
	{
		this.m_anchorPath = anchorPath;
		this.m_ageVerification = base.gameObject.GetComponent<AgeVerificationWindow>();
		if (this.m_ageVerification == null)
		{
			this.m_ageVerification = base.gameObject.AddComponent<AgeVerificationWindow>();
		}
		this.m_ageVerification.Setup(anchorPath);
	}

	public void PlayStart()
	{
		base.gameObject.SetActive(true);
		this.m_state = AgeVerification.State.APOLLO_SEND_START;
	}

	private void Start()
	{
	}

	private void Update()
	{
		switch (this.m_state)
		{
		case AgeVerification.State.APOLLO_SEND_START:
		{
			string[] value = new string[1];
			SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP3, ref value);
			this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_START, value);
			this.m_state = AgeVerification.State.APOLLO_SEND_START_WAIT;
			break;
		}
		case AgeVerification.State.APOLLO_SEND_START_WAIT:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
				this.m_state = AgeVerification.State.CAUTION_AGE_VERIFICATION;
			}
			break;
		case AgeVerification.State.CAUTION_AGE_VERIFICATION:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextUtility.GetCommonText("Shop", "gw_age_verification_caption"),
				message = TextUtility.GetCommonText("Shop", "gw_age_verification_text"),
				buttonType = GeneralWindow.ButtonType.Ok
			});
			this.m_state = AgeVerification.State.CAUTION_AGE_VERIFICATION_WAIT;
			break;
		case AgeVerification.State.CAUTION_AGE_VERIFICATION_WAIT:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.m_state = AgeVerification.State.AGE_VERIFICATION;
			}
			break;
		case AgeVerification.State.AGE_VERIFICATION:
			if (this.m_ageVerification != null && this.m_ageVerification.IsReady)
			{
				this.m_ageVerification.PlayStart();
				this.m_state = AgeVerification.State.AGE_VERIFICATION_WAIT;
			}
			break;
		case AgeVerification.State.AGE_VERIFICATION_WAIT:
			if (this.m_ageVerification != null && this.m_ageVerification.IsEnd)
			{
				this.m_state = AgeVerification.State.APOLLO_SEND_END;
			}
			break;
		case AgeVerification.State.APOLLO_SEND_END:
		{
			string[] value2 = new string[1];
			SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP3, ref value2);
			this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_END, value2);
			this.m_state = AgeVerification.State.APOLLO_SEND_END_WAIT;
			break;
		}
		case AgeVerification.State.APOLLO_SEND_END_WAIT:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
				this.m_state = AgeVerification.State.FINISHED_AGE_VERIFICATION;
			}
			break;
		case AgeVerification.State.FINISHED_AGE_VERIFICATION:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextUtility.GetCommonText("Shop", "gw_age_verification_success_caption"),
				message = TextUtility.GetCommonText("Shop", "gw_age_verification_success_text"),
				anchor_path = this.m_anchorPath,
				buttonType = GeneralWindow.ButtonType.Ok
			});
			this.m_state = AgeVerification.State.FINISHED_AGE_VERIFICATION_WAIT;
			break;
		case AgeVerification.State.FINISHED_AGE_VERIFICATION_WAIT:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				base.gameObject.SetActive(false);
				this.m_state = AgeVerification.State.END;
			}
			break;
		}
	}
}
