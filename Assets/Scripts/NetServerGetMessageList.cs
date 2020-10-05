using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetMessageList : NetBase
{
	private int _resultTotalMessages_k__BackingField;

	private int _resultTotalOperatorMessages_k__BackingField;

	private List<ServerMessageEntry> _resultMessageEntriesList_k__BackingField;

	private List<ServerOperatorMessageEntry> _resultOperatorMessageEntriesList_k__BackingField;

	public int resultTotalMessages
	{
		get;
		private set;
	}

	public int resultTotalOperatorMessages
	{
		get;
		private set;
	}

	public int resultMessages
	{
		get
		{
			return (this.resultMessageEntriesList == null) ? 0 : this.resultMessageEntriesList.Count;
		}
	}

	public int resultOperatorMessages
	{
		get
		{
			return (this.resultOperatorMessageEntriesList == null) ? 0 : this.resultOperatorMessageEntriesList.Count;
		}
	}

	private List<ServerMessageEntry> resultMessageEntriesList
	{
		get;
		set;
	}

	private List<ServerOperatorMessageEntry> resultOperatorMessageEntriesList
	{
		get;
		set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Message/getMessageList");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_MessageList(jdata);
		this.GetResponse_OperatorMessageList(jdata);
		this.GetResponse_TotalMessaga(jdata);
		this.GetResponse_TotalOperatorMessaga(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
		this.resultMessageEntriesList = new List<ServerMessageEntry>();
		this.resultOperatorMessageEntriesList = new List<ServerOperatorMessageEntry>();
		for (int i = 0; i < 2; i++)
		{
			ServerOperatorMessageEntry serverOperatorMessageEntry = new ServerOperatorMessageEntry();
			serverOperatorMessageEntry.m_messageId = i + 1;
			serverOperatorMessageEntry.m_content = "dummy content " + (i + 1);
			serverOperatorMessageEntry.m_presentState.m_itemId = 400000;
			serverOperatorMessageEntry.m_expireTiem = NetUtil.GetCurrentUnixTime() + 10000000;
			this.resultOperatorMessageEntriesList.Add(serverOperatorMessageEntry);
		}
		ServerMessageEntry serverMessageEntry = new ServerMessageEntry();
		serverMessageEntry.m_messageId = 1;
		serverMessageEntry.m_name = "dummy_0001";
		serverMessageEntry.m_url = (serverMessageEntry.m_fromId = "0123456789abcdefg1");
		serverMessageEntry.m_presentState.m_itemId = 900000;
		serverMessageEntry.m_expireTiem = NetUtil.GetCurrentUnixTime() + 10000000;
		serverMessageEntry.m_messageState = ServerMessageEntry.MessageState.Unread;
		serverMessageEntry.m_messageType = ServerMessageEntry.MessageType.SendEnergy;
		this.resultMessageEntriesList.Add(serverMessageEntry);
		serverMessageEntry = new ServerMessageEntry();
		serverMessageEntry.m_messageId = 2;
		serverMessageEntry.m_name = "dummy_0002";
		serverMessageEntry.m_url = (serverMessageEntry.m_fromId = "0123456789abcdefg2");
		serverMessageEntry.m_presentState.m_itemId = 900000;
		serverMessageEntry.m_expireTiem = NetUtil.GetCurrentUnixTime() + 10000000;
		serverMessageEntry.m_messageState = ServerMessageEntry.MessageState.Unread;
		serverMessageEntry.m_messageType = ServerMessageEntry.MessageType.ReturnSendEnergy;
		this.resultMessageEntriesList.Add(serverMessageEntry);
		serverMessageEntry = new ServerMessageEntry();
		serverMessageEntry.m_messageId = 3;
		serverMessageEntry.m_name = "dummy_0003";
		serverMessageEntry.m_url = (serverMessageEntry.m_fromId = "0123456789abcdefg3");
		serverMessageEntry.m_presentState.m_itemId = 900000;
		serverMessageEntry.m_expireTiem = NetUtil.GetCurrentUnixTime() + 10000000;
		serverMessageEntry.m_messageState = ServerMessageEntry.MessageState.Unread;
		serverMessageEntry.m_messageType = ServerMessageEntry.MessageType.RequestEnergy;
		this.resultMessageEntriesList.Add(serverMessageEntry);
		serverMessageEntry = new ServerMessageEntry();
		serverMessageEntry.m_messageId = 4;
		serverMessageEntry.m_name = "dummy_0004";
		serverMessageEntry.m_url = (serverMessageEntry.m_fromId = "0123456789abcdefg4");
		serverMessageEntry.m_presentState.m_itemId = 900000;
		serverMessageEntry.m_expireTiem = NetUtil.GetCurrentUnixTime() + 10000000;
		serverMessageEntry.m_messageState = ServerMessageEntry.MessageState.Unread;
		serverMessageEntry.m_messageType = ServerMessageEntry.MessageType.ReturnRequestEnergy;
		this.resultMessageEntriesList.Add(serverMessageEntry);
		serverMessageEntry = new ServerMessageEntry();
		serverMessageEntry.m_messageId = 5;
		serverMessageEntry.m_name = "dummy_0005";
		serverMessageEntry.m_url = (serverMessageEntry.m_fromId = "0123456789abcdefg5");
		serverMessageEntry.m_presentState.m_itemId = 900000;
		serverMessageEntry.m_expireTiem = NetUtil.GetCurrentUnixTime() + 10000000;
		serverMessageEntry.m_messageState = ServerMessageEntry.MessageState.Unread;
		serverMessageEntry.m_messageType = ServerMessageEntry.MessageType.LentChao;
		this.resultMessageEntriesList.Add(serverMessageEntry);
	}

	public ServerMessageEntry GetResultMessageEntry(int index)
	{
		if (0 <= index && this.resultMessages > index)
		{
			return this.resultMessageEntriesList[index];
		}
		return null;
	}

	public ServerOperatorMessageEntry GetResultOperatorMessageEntry(int index)
	{
		if (0 <= index && this.resultOperatorMessages > index)
		{
			return this.resultOperatorMessageEntriesList[index];
		}
		return null;
	}

	private void GetResponse_MessageList(JsonData jdata)
	{
		this.resultMessageEntriesList = new List<ServerMessageEntry>();
		int count = NetUtil.GetJsonArray(jdata, "messageList").Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jsonArrayObject = NetUtil.GetJsonArrayObject(jdata, "messageList", i);
			ServerMessageEntry item = NetUtil.AnalyzeMessageEntryJson(jsonArrayObject, string.Empty);
			this.resultMessageEntriesList.Add(item);
		}
	}

	private void GetResponse_TotalMessaga(JsonData jdata)
	{
		this.resultTotalMessages = NetUtil.GetJsonInt(jdata, "totalMessage");
	}

	private void GetResponse_OperatorMessageList(JsonData jdata)
	{
		this.resultOperatorMessageEntriesList = new List<ServerOperatorMessageEntry>();
		int count = NetUtil.GetJsonArray(jdata, "operatorMessageList").Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jsonArrayObject = NetUtil.GetJsonArrayObject(jdata, "operatorMessageList", i);
			ServerOperatorMessageEntry item = NetUtil.AnalyzeOperatorMessageEntryJson(jsonArrayObject, string.Empty);
			this.resultOperatorMessageEntriesList.Add(item);
		}
	}

	private void GetResponse_TotalOperatorMessaga(JsonData jdata)
	{
		this.resultTotalOperatorMessages = NetUtil.GetJsonInt(jdata, "totalOperatorMessage");
	}
}
