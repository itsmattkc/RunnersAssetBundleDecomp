using System;

public class ServerUserTransformData
{
	public string m_userId;

	public string m_facebookId;

	public ServerUserTransformData()
	{
		this.m_userId = string.Empty;
		this.m_facebookId = string.Empty;
	}

	public void Dump()
	{
		UnityEngine.Debug.Log(string.Format("userId={0}, facebookId={1}", this.m_userId, this.m_facebookId));
	}
}
