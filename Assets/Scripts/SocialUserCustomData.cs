using System;

public class SocialUserCustomData
{
	private string m_actionId = string.Empty;

	private string m_objectId = string.Empty;

	private string m_gameId = string.Empty;

	public string ActionId
	{
		get
		{
			return this.m_actionId;
		}
		set
		{
			this.m_actionId = value;
		}
	}

	public string ObjectId
	{
		get
		{
			return this.m_objectId;
		}
		set
		{
			this.m_objectId = value;
		}
	}

	public string GameId
	{
		get
		{
			return this.m_gameId;
		}
		set
		{
			this.m_gameId = value;
		}
	}
}
