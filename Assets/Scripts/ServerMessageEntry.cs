using System;

public class ServerMessageEntry
{
	public enum MessageState
	{
		Unread,
		Read,
		Used,
		Deleted
	}

	public enum MessageType
	{
		RequestEnergy,
		ReturnRequestEnergy,
		SendEnergy,
		ReturnSendEnergy,
		LentChao,
		InviteCode,
		Unknown = -1
	}

	public int m_messageId;

	public ServerMessageEntry.MessageType m_messageType;

	public string m_fromId;

	public string m_name;

	public string m_url;

	public ServerMessageEntry.MessageState m_messageState;

	public int m_expireTiem;

	public ServerPresentState m_presentState;

	public ServerMessageEntry()
	{
		this.m_messageId = 2346789;
		this.m_fromId = "0123456789abcdef";
		this.m_messageType = ServerMessageEntry.MessageType.SendEnergy;
		this.m_messageState = ServerMessageEntry.MessageState.Unread;
		this.m_name = "0123456789abcdef";
		this.m_url = "0123456789abcdef";
		this.m_expireTiem = 0;
		this.m_presentState = new ServerPresentState();
	}

	public void CopyTo(ServerMessageEntry to)
	{
		to.m_messageId = this.m_messageId;
		to.m_fromId = this.m_fromId;
		to.m_messageType = this.m_messageType;
		to.m_messageState = this.m_messageState;
		to.m_name = this.m_name;
		to.m_url = this.m_url;
		to.m_expireTiem = this.m_expireTiem;
		this.m_presentState.CopyTo(to.m_presentState);
	}
}
