using App;
using System;

public class ServerDailyBattleData
{
	public long maxScore;

	public int league;

	public string userId = string.Empty;

	public string name = string.Empty;

	public long loginTime;

	public int mainChaoId;

	public int mainChaoLevel;

	public int subChaoId;

	public int subChaoLevel;

	public int numRank;

	public int charaId;

	public int charaLevel;

	public int subCharaId;

	public int subCharaLevel;

	public int goOnWin;

	public bool isSentEnergy;

	public Env.Language language;

	public ServerDailyBattleData()
	{
		this.maxScore = 0L;
		this.league = 0;
		this.userId = string.Empty;
		this.name = string.Empty;
		this.loginTime = 0L;
		this.mainChaoId = 0;
		this.mainChaoLevel = 0;
		this.subChaoId = 0;
		this.subChaoLevel = 0;
		this.numRank = 0;
		this.charaId = 0;
		this.charaLevel = 0;
		this.subCharaId = 0;
		this.subCharaLevel = 0;
		this.goOnWin = 0;
		this.isSentEnergy = false;
		this.language = Env.Language.JAPANESE;
	}

	public bool CheckFriend()
	{
		bool result = false;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null && socialInterface.IsLoggedIn)
		{
			result = (SocialInterface.GetSocialUserDataFromGameId(socialInterface.FriendList, this.userId) != null);
		}
		return result;
	}

	public void Dump()
	{
		if (!string.IsNullOrEmpty(this.userId))
		{
			UnityEngine.Debug.Log(string.Format("ServerDailyBattleData  maxScore:{0} league:{1} userId:{2} name:{3} numRank:{4} goOnWin:{5}", new object[]
			{
				this.maxScore,
				this.league,
				this.userId,
				this.name,
				this.numRank,
				this.goOnWin
			}));
		}
		else
		{
			UnityEngine.Debug.Log("ServerDailyBattleData  null");
		}
	}

	public void CopyTo(ServerDailyBattleData dest)
	{
		dest.maxScore = this.maxScore;
		dest.league = this.league;
		dest.userId = this.userId;
		dest.name = this.name;
		dest.loginTime = this.loginTime;
		dest.mainChaoId = this.mainChaoId;
		dest.mainChaoLevel = this.mainChaoLevel;
		dest.subChaoId = this.subChaoId;
		dest.subChaoLevel = this.subChaoLevel;
		dest.numRank = this.numRank;
		dest.charaId = this.charaId;
		dest.charaLevel = this.charaLevel;
		dest.subCharaId = this.subCharaId;
		dest.subCharaLevel = this.subCharaLevel;
		dest.goOnWin = this.goOnWin;
		dest.isSentEnergy = this.isSentEnergy;
		dest.language = this.language;
	}
}
