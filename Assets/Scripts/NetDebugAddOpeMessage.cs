using LitJson;
using System;

public class NetDebugAddOpeMessage : NetBase
{
	public class OpeMsgInfo
	{
		public string userID;

		public int messageKind;

		public int infoId;

		public int itemId;

		public int numItem;

		public int additionalInfo1;

		public int additionalInfo2;

		public string msgTitle;

		public string msgContent;

		public string msgImageId;
	}

	private NetDebugAddOpeMessage.OpeMsgInfo paramOpeMsgInfo;

	public NetDebugAddOpeMessage(NetDebugAddOpeMessage.OpeMsgInfo info)
	{
		this.paramOpeMsgInfo = info;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/addOpeMessage");
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
		if (this.paramOpeMsgInfo != null)
		{
			base.WriteActionParamValue("messageKind", this.paramOpeMsgInfo.messageKind);
			base.WriteActionParamValue("infoid", this.paramOpeMsgInfo.infoId);
			base.WriteActionParamValue("item_id", this.paramOpeMsgInfo.itemId);
			base.WriteActionParamValue("num_item", this.paramOpeMsgInfo.numItem);
			base.WriteActionParamValue("additional_info_1", this.paramOpeMsgInfo.additionalInfo1);
			base.WriteActionParamValue("additional_info_2", this.paramOpeMsgInfo.additionalInfo2);
			base.WriteActionParamValue("msg_title", this.paramOpeMsgInfo.msgTitle);
			base.WriteActionParamValue("msg_content", this.paramOpeMsgInfo.msgContent);
			base.WriteActionParamValue("msg_image_id", this.paramOpeMsgInfo.msgImageId);
			base.WriteActionParamValue("hspToId", this.paramOpeMsgInfo.userID);
		}
	}
}
