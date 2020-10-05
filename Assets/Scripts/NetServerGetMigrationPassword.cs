using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetMigrationPassword : NetBase
{
	private string _paramUserPassword_k__BackingField;

	private string _paramMigrationPassword_k__BackingField;

	public string paramUserPassword
	{
		private get;
		set;
	}

	public string paramMigrationPassword
	{
		get;
		set;
	}

	public NetServerGetMigrationPassword() : this(string.Empty)
	{
	}

	public NetServerGetMigrationPassword(string userPassword)
	{
		this.paramUserPassword = userPassword;
	}

	protected override void DoRequest()
	{
		base.SetAction("Login/getMigrationPassword");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getMigrationPasswordString = instance.GetGetMigrationPasswordString(this.paramUserPassword);
			base.WriteJsonString(getMigrationPasswordString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_MigrationPassword(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_UserPassword()
	{
		base.WriteActionParamValue("userPassword", this.paramUserPassword);
	}

	private void GetResponse_MigrationPassword(JsonData jdata)
	{
		this.paramMigrationPassword = NetUtil.GetJsonString(jdata, "password");
	}
}
