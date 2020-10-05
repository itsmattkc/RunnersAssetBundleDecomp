using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerUpdateMessage : NetBase
{
	public const int INVALID_ID = -2147483648;

	private List<int> _paramMessageIdList_k__BackingField;

	private List<int> _paramOperatorMessageIdList_k__BackingField;

	private ServerMessageEntry.MessageState _paramMessageState_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private ServerCharacterState[] _resultCharacterState_k__BackingField;

	private List<ServerChaoState> _resultChaoState_k__BackingField;

	private List<ServerPresentState> _resultPresentStateList_k__BackingField;

	private List<int> _resultMissingMessageIdList_k__BackingField;

	private List<int> _resultMissingOperatorMessageIdList_k__BackingField;

	public List<int> paramMessageIdList
	{
		get;
		set;
	}

	public List<int> paramOperatorMessageIdList
	{
		get;
		set;
	}

	public ServerMessageEntry.MessageState paramMessageState
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public ServerCharacterState[] resultCharacterState
	{
		get;
		private set;
	}

	public List<ServerChaoState> resultChaoState
	{
		get;
		private set;
	}

	public int resultPresentStates
	{
		get
		{
			return (this.resultPresentStateList == null) ? 0 : this.resultPresentStateList.Count;
		}
	}

	public int resultMissingMessages
	{
		get
		{
			return (this.resultMissingMessageIdList == null) ? 0 : this.resultMissingMessageIdList.Count;
		}
	}

	public int resultMissingOperatorMessages
	{
		get
		{
			return (this.resultMissingOperatorMessageIdList == null) ? 0 : this.resultMissingOperatorMessageIdList.Count;
		}
	}

	private List<ServerPresentState> resultPresentStateList
	{
		get;
		set;
	}

	private List<int> resultMissingMessageIdList
	{
		get;
		set;
	}

	private List<int> resultMissingOperatorMessageIdList
	{
		get;
		set;
	}

	public NetServerUpdateMessage(List<int> messageIdList, List<int> operatorMessageIdList)
	{
		this.paramMessageIdList = messageIdList;
		this.paramOperatorMessageIdList = operatorMessageIdList;
	}

	protected override void DoRequest()
	{
		base.SetAction("Message/getMessage");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getMessageString = instance.GetGetMessageString(this.paramMessageIdList, this.paramOperatorMessageIdList);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getMessageString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_CharacterState(jdata);
		this.GetResponse_ChaoState(jdata);
		this.GetResponse_PresentStateList(jdata);
		this.GetResponse_MissingMessage(jdata);
		this.GetResponse_MissingOperatorMessage(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_MessageId()
	{
		if (this.paramMessageIdList == null)
		{
			if (ServerInterface.MessageList != null && ServerInterface.MessageList.Count > 0)
			{
				base.WriteActionParamValue("messageId", 0);
			}
		}
		else
		{
			List<object> list = new List<object>();
			if (list != null)
			{
				foreach (object item in this.paramMessageIdList)
				{
					list.Add(item);
				}
				if (list.Count != 0)
				{
					base.WriteActionParamArray("messageId", list);
				}
			}
		}
		if (this.paramOperatorMessageIdList == null)
		{
			if (ServerInterface.OperatorMessageList != null && ServerInterface.OperatorMessageList.Count > 0)
			{
				base.WriteActionParamValue("operationMessageId", 0);
			}
		}
		else
		{
			List<object> list2 = new List<object>();
			if (list2 != null)
			{
				foreach (object item2 in this.paramOperatorMessageIdList)
				{
					list2.Add(item2);
				}
				if (list2.Count != 0)
				{
					base.WriteActionParamArray("operationMessageId", list2);
				}
			}
		}
	}

	public ServerPresentState GetResultPresentState(int index)
	{
		if (0 <= index && this.resultPresentStates > index)
		{
			return this.resultPresentStateList[index];
		}
		return null;
	}

	public int GetResultMissingMessageId(int index)
	{
		if (0 <= index && this.resultMissingMessages > index)
		{
			return this.resultMissingMessageIdList[index];
		}
		return -2147483648;
	}

	public int GetResultMissingOperatorMessageId(int index)
	{
		if (0 <= index && this.resultMissingOperatorMessages > index)
		{
			return this.resultMissingOperatorMessageIdList[index];
		}
		return -2147483648;
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_CharacterState(JsonData jdata)
	{
		this.resultCharacterState = NetUtil.AnalyzePlayerState_CharactersStates(jdata);
	}

	private void GetResponse_ChaoState(JsonData jdata)
	{
		this.resultChaoState = NetUtil.AnalyzePlayerState_ChaoStates(jdata);
	}

	private void GetResponse_PresentStateList(JsonData jdata)
	{
		this.resultPresentStateList = new List<ServerPresentState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "presentList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerPresentState item = NetUtil.AnalyzePresentStateJson(jdata2, string.Empty);
			this.resultPresentStateList.Add(item);
		}
	}

	private void GetResponse_MissingMessage(JsonData jdata)
	{
		this.resultMissingMessageIdList = new List<int>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "notRecvMessageList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			int jsonInt = NetUtil.GetJsonInt(jsonArray[i]);
			this.resultMissingMessageIdList.Add(jsonInt);
		}
	}

	private void GetResponse_MissingOperatorMessage(JsonData jdata)
	{
		this.resultMissingOperatorMessageIdList = new List<int>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "notRecvOperatorMessageList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			int jsonInt = NetUtil.GetJsonInt(jsonArray[i]);
			this.resultMissingOperatorMessageIdList.Add(jsonInt);
		}
	}
}
