using System;

public class ServerOperatorMessageEntry
{
	public int m_messageId;

	public string m_content;

	public int m_expireTiem;

	public ServerPresentState m_presentState;

	public ServerOperatorMessageEntry()
	{
		this.m_messageId = 2346789;
		this.m_content = string.Empty;
		this.m_expireTiem = 0;
		this.m_presentState = new ServerPresentState();
	}

	public void CopyTo(ServerOperatorMessageEntry to)
	{
		to.m_messageId = this.m_messageId;
		to.m_content = this.m_content;
		to.m_expireTiem = this.m_expireTiem;
		this.m_presentState.CopyTo(to.m_presentState);
	}
}
