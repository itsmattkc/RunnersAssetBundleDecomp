using Message;
using System;
using UnityEngine;

public class SendApollo : MonoBehaviour
{
	public enum State
	{
		Idle,
		Request,
		Succeeded,
		Failed
	}

	private bool m_debugDraw = true;

	private SendApollo.State m_state;

	private bool IsDebugDraw()
	{
		return false;
	}

	public SendApollo.State GetState()
	{
		return this.m_state;
	}

	public bool IsEnd()
	{
		return this.m_state != SendApollo.State.Request;
	}

	public void RequestServer(ApolloType type, string[] value)
	{
		if (this.IsDebugDraw())
		{
			global::Debug.Log("SendApollo RequestServer type=" + type.ToString());
			if (value != null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					string str = value[i];
					global::Debug.Log("SendApollo RequestServer value=" + str);
				}
			}
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerSendApollo((int)type, value, base.gameObject);
			this.m_state = SendApollo.State.Request;
		}
	}

	private void ServerSendApollo_Succeeded(MsgSendApolloSucceed msg)
	{
		if (this.IsDebugDraw())
		{
			global::Debug.Log("SendApollo ServerSendApollo_Succeeded");
		}
		this.m_state = SendApollo.State.Succeeded;
	}

	private void ServerSendApollo_Failed(MsgServerConnctFailed msg)
	{
		if (this.IsDebugDraw())
		{
			global::Debug.Log("SendApollo ServerSendApollo_Failed");
		}
		this.m_state = SendApollo.State.Failed;
	}

	public static SendApollo Create()
	{
		GameObject gameObject = new GameObject("SendApollo");
		return gameObject.AddComponent<SendApollo>();
	}

	public static SendApollo CreateRequest(ApolloType type, string[] value)
	{
		SendApollo sendApollo = SendApollo.Create();
		if (sendApollo != null)
		{
			sendApollo.RequestServer(type, value);
		}
		return sendApollo;
	}

	public static void GetTutorialValue(ApolloTutorialIndex index, ref string[] value)
	{
		if (value != null)
		{
			string[] arg_13_0 = value;
			int arg_13_1 = 0;
			int num = (int)index;
			arg_13_0[arg_13_1] = num.ToString();
		}
	}
}
