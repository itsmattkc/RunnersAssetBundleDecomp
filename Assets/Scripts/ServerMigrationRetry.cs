using System;
using UnityEngine;

public class ServerMigrationRetry : ServerRetryProcess
{
	public string m_migrationId;

	public string m_migrationPassword;

	public ServerMigrationRetry(string migrationId, string migrationPassword, GameObject callbackObject) : base(callbackObject)
	{
		this.m_migrationId = migrationId;
		this.m_migrationPassword = migrationPassword;
	}

	public override void Retry()
	{
		ServerInterface serverInterface = GameObjectUtil.FindGameObjectComponent<ServerInterface>("ServerInterface");
		if (serverInterface != null)
		{
			serverInterface.RequestServerMigration(this.m_migrationId, this.m_migrationPassword, this.m_callbackObject);
		}
	}
}
