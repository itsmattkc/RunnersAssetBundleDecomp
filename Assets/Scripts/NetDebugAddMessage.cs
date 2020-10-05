using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugAddMessage : NetBase
{
	private string _paramFromHspId_k__BackingField;

	private string _paramToHspId_k__BackingField;

	private int _paramMessageType_k__BackingField;

	public string paramFromHspId
	{
		get;
		set;
	}

	public string paramToHspId
	{
		get;
		set;
	}

	public int paramMessageType
	{
		get;
		set;
	}

	public NetDebugAddMessage() : this(string.Empty, string.Empty, 0)
	{
	}

	public NetDebugAddMessage(string fromHspId, string toHspId, int messageType)
	{
		this.paramFromHspId = fromHspId;
		this.paramToHspId = toHspId;
		this.paramMessageType = messageType;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/addMessage");
		this.SetParameter_Message();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Message()
	{
		base.WriteActionParamValue("hspFromId", this.paramFromHspId);
		base.WriteActionParamValue("hspToId", this.paramToHspId);
		base.WriteActionParamValue("messageKind", this.paramMessageType);
	}
}
