using App;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RaidBossUser
{
	public static SocialInterface s_socialInterface;

	private long _damage_k__BackingField;

	private long _destroyCount_k__BackingField;

	private bool _isDestroy_k__BackingField;

	private int _mapRank_k__BackingField;

	private int _rankIndex_k__BackingField;

	private int _rankIndexChanged_k__BackingField;

	private string _userName_k__BackingField;

	private bool _isFriend_k__BackingField;

	private bool _isSentEnergy_k__BackingField;

	private CharaType _charaType_k__BackingField;

	private CharaType _charaSubType_k__BackingField;

	private int _charaLevel_k__BackingField;

	private int _mainChaoId_k__BackingField;

	private int _subChaoId_k__BackingField;

	private int _mainChaoLevel_k__BackingField;

	private int _subChaoLevel_k__BackingField;

	private Env.Language _language_k__BackingField;

	private int _leagueIndex_k__BackingField;

	private string _id_k__BackingField;

	private DateTime _loginTime_k__BackingField;

	public long damage
	{
		get;
		set;
	}

	public long destroyCount
	{
		get;
		set;
	}

	public bool isDestroy
	{
		get;
		set;
	}

	public int mapRank
	{
		get;
		set;
	}

	public string dispMapRank
	{
		get
		{
			return (this.mapRank + 1).ToString("D3");
		}
	}

	public int rankIndex
	{
		get;
		set;
	}

	public int rankIndexChanged
	{
		get;
		set;
	}

	public string userName
	{
		get;
		set;
	}

	public bool isFriend
	{
		get;
		set;
	}

	public bool isSentEnergy
	{
		get;
		set;
	}

	public CharaType charaType
	{
		get;
		set;
	}

	public CharaType charaSubType
	{
		get;
		set;
	}

	public int charaLevel
	{
		get;
		set;
	}

	public int mainChaoId
	{
		get;
		set;
	}

	public int subChaoId
	{
		get;
		set;
	}

	public int mainChaoLevel
	{
		get;
		set;
	}

	public int subChaoLevel
	{
		get;
		set;
	}

	public Env.Language language
	{
		get;
		set;
	}

	public int leagueIndex
	{
		get;
		set;
	}

	public string id
	{
		get;
		set;
	}

	public DateTime loginTime
	{
		get;
		set;
	}

	public bool isRankTop
	{
		get
		{
			return this.rankIndex < 1;
		}
	}

	public int mainChaoRarity
	{
		get
		{
			return this.mainChaoId / 1000;
		}
	}

	public int subChaoRarity
	{
		get
		{
			return this.subChaoId / 1000;
		}
	}

	public RaidBossUser(ServerEventRaidBossUserState state)
	{
		this.isDestroy = state.WrestleBeatFlg;
		this.damage = (long)state.WrestleDamage;
		this.destroyCount = (long)state.WrestleCount;
		this.id = state.WrestleId;
		this.userName = state.UserName;
		this.rankIndex = state.Grade - 1;
		this.mapRank = state.NumRank;
		this.loginTime = NetUtil.GetLocalDateTime((long)state.LoginTime);
		ServerItem serverItem = new ServerItem((ServerItem.Id)state.CharaId);
		this.charaType = serverItem.charaType;
		ServerItem serverItem2 = new ServerItem((ServerItem.Id)state.SubCharaId);
		this.charaSubType = serverItem2.charaType;
		this.charaLevel = state.CharaLevel;
		ServerItem serverItem3 = new ServerItem((ServerItem.Id)state.MainChaoId);
		this.mainChaoId = serverItem3.chaoId;
		ServerItem serverItem4 = new ServerItem((ServerItem.Id)state.SubChaoId);
		this.subChaoId = serverItem4.chaoId;
		this.mainChaoLevel = state.MainChaoLevel;
		this.subChaoLevel = state.SubChaoLevel;
		this.leagueIndex = state.League;
		this.language = (Env.Language)state.Language;
		if (RaidBossUser.s_socialInterface == null)
		{
			RaidBossUser.s_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		}
		if (RaidBossUser.s_socialInterface != null)
		{
			this.isFriend = (SocialInterface.GetSocialUserDataFromGameId(RaidBossUser.s_socialInterface.FriendList, this.id) != null);
		}
		else
		{
			this.isFriend = (this.isSentEnergy || UnityEngine.Random.Range(0, 3) != 0);
		}
	}

	public RaidBossUser(ServerEventRaidBossDesiredState state)
	{
		this.isDestroy = false;
		this.damage = 0L;
		this.destroyCount = 0L;
		this.id = state.DesireId;
		this.userName = state.UserName;
		this.mapRank = state.NumRank;
		this.loginTime = NetUtil.GetLocalDateTime((long)state.LoginTime);
		ServerItem serverItem = new ServerItem((ServerItem.Id)state.CharaId);
		this.charaType = serverItem.charaType;
		ServerItem serverItem2 = new ServerItem((ServerItem.Id)state.SubCharaId);
		this.charaSubType = serverItem2.charaType;
		this.charaLevel = state.CharaLevel;
		ServerItem serverItem3 = new ServerItem((ServerItem.Id)state.MainChaoId);
		this.mainChaoId = serverItem3.chaoId;
		ServerItem serverItem4 = new ServerItem((ServerItem.Id)state.SubChaoId);
		this.subChaoId = serverItem4.chaoId;
		this.mainChaoLevel = state.MainChaoLevel;
		this.subChaoLevel = state.SubChaoLevel;
		this.leagueIndex = state.League;
		this.language = (Env.Language)state.Language;
		if (RaidBossUser.s_socialInterface == null)
		{
			RaidBossUser.s_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		}
		if (RaidBossUser.s_socialInterface != null)
		{
			this.isFriend = (SocialInterface.GetSocialUserDataFromGameId(RaidBossUser.s_socialInterface.FriendList, this.id) != null);
		}
		else
		{
			this.isFriend = (this.isSentEnergy || UnityEngine.Random.Range(0, 3) != 0);
		}
	}

	public RankingUtil.Ranker ConvertRankerData()
	{
		return new RankingUtil.Ranker(this);
	}
}
