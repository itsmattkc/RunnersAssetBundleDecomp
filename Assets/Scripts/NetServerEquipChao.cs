using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerEquipChao : NetBase
{
	private int _paramMainChaoId_k__BackingField;

	private int _paramSubChaoId_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public int paramMainChaoId
	{
		get;
		set;
	}

	public int paramSubChaoId
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerEquipChao() : this(0, 0)
	{
	}

	public NetServerEquipChao(int mainChaoId, int subChaoId)
	{
		this.paramMainChaoId = mainChaoId;
		this.paramSubChaoId = subChaoId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Chao/equipChao");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string equipChaoString = instance.GetEquipChaoString(this.paramMainChaoId, this.paramSubChaoId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(equipChaoString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_MainChaoId()
	{
		base.WriteActionParamValue("mainChaoId", this.paramMainChaoId);
	}

	private void SetParameter_SubChaoId()
	{
		base.WriteActionParamValue("subChaoId", this.paramSubChaoId);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
