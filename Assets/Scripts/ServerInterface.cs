using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerInterface : MonoBehaviour
{
	public enum StatusCode
	{
		Ok,
		ServerSecurityError = -19001,
		VersionDifference = -19002,
		DecryptionFailure = -19003,
		ParamHashDifference = -19004,
		ServerNextVersion = -19990,
		ServerMaintenance = -19997,
		ServerBusyError = -19998,
		ServerSystemError = -19999,
		RequestParamError = -10100,
		NotAvailablePlayer = -10101,
		MissingPlayer = -10102,
		ExpirationSession = -10103,
		PassWordError = -10104,
		InvalidSerialCode = -10105,
		UsedSerialCode = -10106,
		HspWebApiError = -10110,
		ApolloWebApiError = -10115,
		DataMismatch = -30120,
		MasterDataMismatch = -10121,
		NotEnoughRedStarRings = -20130,
		NotEnoughRings = -20131,
		NotEnoughEnergy = -20132,
		RouletteUseLimit = -30401,
		RouletteBoardReset = -30411,
		CharacterLevelLimit = -20601,
		AllChaoLevelLimit = -20602,
		AlreadyInvitedFriend = -30801,
		AlreadyRequestedEnergy = -30901,
		AlreadySentEnergy = -30902,
		ReceiveFailureMessage = -30910,
		AlreadyExistedPrePurchase = -11001,
		AlreadyRemovedPrePurchase = -11002,
		InvalidReceiptData = -11003,
		AlreadyProcessedReceipt = -11004,
		EnergyLimitPurchaseTrigger = -21010,
		NotStartEvent = -10201,
		AlreadyEndEvent = -10202,
		VersionForApplication = -999002,
		TimeOut = -7,
		OtherError = -8,
		NotReachability = -10,
		InvalidResponse = -20,
		CliendError = -400,
		InternalServerError = -500,
		HspPurchaseError = -600,
		ServerBusy = -700
	}

	public ServerLeaderboardEntry m_myLeaderboardEntry;

	private static ServerSettingState s_settingState;

	private static ServerLoginState s_loginState;

	private static ServerNextVersionState s_nextState;

	private static ServerPlayerState s_playerState;

	private static ServerFreeItemState s_freeItemState;

	private static ServerLoginBonusData s_loginBonusData;

	private static ServerNoticeInfo s_noticeInfo;

	private static ServerTickerInfo s_tickerInfo;

	private static ServerWheelOptions s_wheelOptions;

	private static ServerChaoWheelOptions s_chaoWheelOptions;

	private static List<ServerRingExchangeList> s_ringExchangeList;

	private static List<ServerConsumedCostData> s_consumedCostList;

	private static List<ServerConsumedCostData> s_costList;

	private static ServerMileageMapState s_mileageMapState;

	private static List<ServerMileageReward> s_mileageRewardList;

	private static List<ServerMileageFriendEntry> s_mileageFriendList;

	private static List<ServerDistanceFriendEntry> s_distanceFriendEntry;

	private static ServerCampaignState s_campaignState;

	private static List<ServerMessageEntry> s_messageList;

	private static List<ServerOperatorMessageEntry> s_operatorMessageList;

	private static ServerLeaderboardEntries s_leaderboardEntries;

	private static ServerLeaderboardEntries s_leaderboardEntriesRivalHighScore;

	private static ServerLeaderboardEntry s_leaderboardEntryRivalHighScoreTop;

	private static ServerPrizeState s_premiumRoulettePrizeList;

	private static ServerPrizeState s_specialRoulettePrizeList;

	private static ServerPrizeState s_raidRoulettePrizeList;

	private static ServerEventState s_eventState;

	private static List<ServerEventEntry> s_eventEntryList;

	private static List<ServerEventReward> s_eventRewardList;

	private static List<ServerRedStarItemState> s_redStartItemState;

	private static List<ServerRedStarItemState> s_redStartExchangeRingItemState;

	private static List<ServerRedStarItemState> s_redStartExchangeEnergyItemState;

	private static List<ServerRedStarItemState> s_redStartExchangeRaidbossEnergyItemState;

	private static ServerDailyChallengeState s_dailyChallengeState;

	private static List<ServerUserTransformData> s_userTransformDataList;

	private static string s_migrationPassword;

	private static ServerLeagueData s_leagueData;

	private static bool s_isCreated;

	public static ServerSettingState SettingState
	{
		get
		{
			return ServerInterface.s_settingState;
		}
	}

	public static ServerLoginState LoginState
	{
		get
		{
			return ServerInterface.s_loginState;
		}
	}

	public static ServerNextVersionState NextVersionState
	{
		get
		{
			return ServerInterface.s_nextState;
		}
	}

	public static ServerFreeItemState FreeItemState
	{
		get
		{
			return ServerInterface.s_freeItemState;
		}
	}

	public static ServerLoginBonusData LoginBonusData
	{
		get
		{
			return ServerInterface.s_loginBonusData;
		}
	}

	public static ServerNoticeInfo NoticeInfo
	{
		get
		{
			return ServerInterface.s_noticeInfo;
		}
	}

	public static ServerTickerInfo TickerInfo
	{
		get
		{
			return ServerInterface.s_tickerInfo;
		}
	}

	public static ServerPlayerState PlayerState
	{
		get
		{
			return ServerInterface.s_playerState;
		}
	}

	public static ServerWheelOptions WheelOptions
	{
		get
		{
			return ServerInterface.s_wheelOptions;
		}
	}

	public static ServerChaoWheelOptions ChaoWheelOptions
	{
		get
		{
			return ServerInterface.s_chaoWheelOptions;
		}
	}

	public static List<ServerRingExchangeList> RingExchangeList
	{
		get
		{
			return ServerInterface.s_ringExchangeList;
		}
	}

	public static List<ServerConsumedCostData> ConsumedCostList
	{
		get
		{
			return ServerInterface.s_consumedCostList;
		}
	}

	public static List<ServerConsumedCostData> CostList
	{
		get
		{
			return ServerInterface.s_costList;
		}
	}

	public static ServerMileageMapState MileageMapState
	{
		get
		{
			return ServerInterface.s_mileageMapState;
		}
	}

	public static List<ServerMileageReward> MileageRewardList
	{
		get
		{
			return ServerInterface.s_mileageRewardList;
		}
	}

	public static List<ServerMileageFriendEntry> MileageFriendList
	{
		get
		{
			return ServerInterface.s_mileageFriendList;
		}
	}

	public static ServerCampaignState CampaignState
	{
		get
		{
			return ServerInterface.s_campaignState;
		}
	}

	public static List<ServerDistanceFriendEntry> DistanceFriendEntry
	{
		get
		{
			return ServerInterface.s_distanceFriendEntry;
		}
	}

	public static ServerLeaderboardEntries LeaderboardEntries
	{
		get
		{
			return ServerInterface.s_leaderboardEntries;
		}
	}

	public static ServerLeaderboardEntries LeaderboardEntriesRivalHighScore
	{
		get
		{
			return ServerInterface.s_leaderboardEntriesRivalHighScore;
		}
	}

	public static ServerLeaderboardEntry LeaderboardEntryRivalHighScoreTop
	{
		get
		{
			return ServerInterface.s_leaderboardEntryRivalHighScoreTop;
		}
	}

	public static ServerPrizeState PremiumRoulettePrizeList
	{
		get
		{
			return ServerInterface.s_premiumRoulettePrizeList;
		}
	}

	public static ServerPrizeState SpecialRoulettePrizeList
	{
		get
		{
			return ServerInterface.s_specialRoulettePrizeList;
		}
	}

	public static ServerPrizeState RaidRoulettePrizeList
	{
		get
		{
			return ServerInterface.s_raidRoulettePrizeList;
		}
	}

	public static List<ServerMessageEntry> MessageList
	{
		get
		{
			return ServerInterface.s_messageList;
		}
	}

	public static List<ServerOperatorMessageEntry> OperatorMessageList
	{
		get
		{
			return ServerInterface.s_operatorMessageList;
		}
	}

	public static ServerEventState EventState
	{
		get
		{
			return ServerInterface.s_eventState;
		}
	}

	public static List<ServerEventEntry> EventEntryList
	{
		get
		{
			return ServerInterface.s_eventEntryList;
		}
	}

	public static List<ServerEventReward> EventRewardList
	{
		get
		{
			return ServerInterface.s_eventRewardList;
		}
	}

	public static List<ServerRedStarItemState> RedStarItemList
	{
		get
		{
			return ServerInterface.s_redStartItemState;
		}
	}

	public static List<ServerRedStarItemState> RedStarExchangeRingItemList
	{
		get
		{
			return ServerInterface.s_redStartExchangeRingItemState;
		}
	}

	public static List<ServerRedStarItemState> RedStarExchangeEnergyItemList
	{
		get
		{
			return ServerInterface.s_redStartExchangeEnergyItemState;
		}
	}

	public static List<ServerRedStarItemState> RedStarExchangeRaidbossEnergyItemList
	{
		get
		{
			return ServerInterface.s_redStartExchangeRaidbossEnergyItemState;
		}
	}

	public static ServerDailyChallengeState DailyChallengeState
	{
		get
		{
			return ServerInterface.s_dailyChallengeState;
		}
	}

	public static List<ServerUserTransformData> UserTransformDataList
	{
		get
		{
			return ServerInterface.s_userTransformDataList;
		}
	}

	public static string MigrationPassword
	{
		get
		{
			return ServerInterface.s_migrationPassword;
		}
		set
		{
			ServerInterface.s_migrationPassword = value;
		}
	}

	public static ServerInterface LoggedInServerInterface
	{
		get
		{
			return (ServerInterface.LoginState == null || !ServerInterface.LoginState.IsLoggedIn) ? null : GameObjectUtil.FindGameObjectComponent<ServerInterface>("ServerInterface");
		}
	}

	private static void Init()
	{
		if (ServerInterface.s_isCreated)
		{
			return;
		}
		ServerInterface.s_settingState = new ServerSettingState();
		ServerInterface.s_loginState = new ServerLoginState();
		ServerInterface.s_nextState = new ServerNextVersionState();
		ServerInterface.s_playerState = new ServerPlayerState();
		ServerInterface.s_freeItemState = new ServerFreeItemState();
		ServerInterface.s_loginBonusData = new ServerLoginBonusData();
		ServerInterface.s_noticeInfo = new ServerNoticeInfo();
		ServerInterface.s_tickerInfo = new ServerTickerInfo();
		ServerInterface.s_wheelOptions = new ServerWheelOptions(null);
		ServerInterface.s_chaoWheelOptions = new ServerChaoWheelOptions();
		ServerInterface.s_ringExchangeList = new List<ServerRingExchangeList>();
		ServerInterface.s_consumedCostList = new List<ServerConsumedCostData>();
		ServerInterface.s_costList = new List<ServerConsumedCostData>();
		ServerInterface.s_mileageMapState = new ServerMileageMapState();
		ServerInterface.s_mileageRewardList = new List<ServerMileageReward>();
		ServerInterface.s_mileageFriendList = new List<ServerMileageFriendEntry>();
		ServerInterface.s_distanceFriendEntry = new List<ServerDistanceFriendEntry>();
		ServerInterface.s_campaignState = new ServerCampaignState();
		ServerInterface.s_leaderboardEntries = new ServerLeaderboardEntries();
		ServerInterface.s_leaderboardEntriesRivalHighScore = new ServerLeaderboardEntries();
		ServerInterface.s_leaderboardEntryRivalHighScoreTop = new ServerLeaderboardEntry();
		ServerInterface.s_premiumRoulettePrizeList = new ServerPrizeState();
		ServerInterface.s_specialRoulettePrizeList = new ServerPrizeState();
		ServerInterface.s_raidRoulettePrizeList = new ServerPrizeState();
		ServerInterface.s_messageList = new List<ServerMessageEntry>();
		ServerInterface.s_operatorMessageList = new List<ServerOperatorMessageEntry>();
		ServerInterface.s_eventState = new ServerEventState();
		ServerInterface.s_eventEntryList = new List<ServerEventEntry>();
		ServerInterface.s_eventRewardList = new List<ServerEventReward>();
		ServerInterface.s_redStartItemState = new List<ServerRedStarItemState>();
		ServerInterface.s_redStartExchangeRingItemState = new List<ServerRedStarItemState>();
		ServerInterface.s_redStartExchangeEnergyItemState = new List<ServerRedStarItemState>();
		ServerInterface.s_redStartExchangeRaidbossEnergyItemState = new List<ServerRedStarItemState>();
		ServerInterface.s_dailyChallengeState = new ServerDailyChallengeState();
		ServerInterface.s_userTransformDataList = new List<ServerUserTransformData>();
		ServerInterface.s_leagueData = new ServerLeagueData();
		ServerInterface.s_migrationPassword = string.Empty;
		ServerInterface.s_isCreated = true;
	}

	private static void Reset()
	{
		ServerInterface.Init();
	}

	private void Start()
	{
		if (!ServerInterface.s_isCreated)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			ServerInterface.Init();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void RequestServerLogin(string userId, string password, GameObject callbackObject)
	{
		base.StartCoroutine(ServerLogin.Process(userId, password, callbackObject));
	}

	public void RequestServerReLogin(GameObject callbackObject)
	{
		base.StartCoroutine(ServerReLogin.Process(callbackObject));
	}

	public void RequestServerMigration(string migrationID, string migrationPassword, GameObject callbackObject)
	{
		base.StartCoroutine(ServerMigration.Process(migrationID, migrationPassword, callbackObject));
	}

	public void RequestServerGetMigrationPassword(string userPassword, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetMigrationPassword.Process(userPassword, callbackObject));
	}

	public void RequestServerGetInformation(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetInformation.Process(callbackObject));
	}

	public void RequestServerGetVersion(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetVersion.Process(callbackObject));
	}

	public void RequestServerGetTicker(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetTicker.Process(callbackObject));
	}

	public void RequestServerGetCountry(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetCountry.Process(callbackObject));
	}

	public void RequestServerGetVariousParameter(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetVariousParameter.Process(callbackObject));
	}

	public void RequestServerLoginBonus(GameObject callbackObject)
	{
		base.StartCoroutine(ServerLoginBonus.Process(callbackObject));
	}

	public void RequestServerLoginBonusSelect(int rewardId, int rewardDays, int rewardSelect, int firstRewardDays, int firstRewardSelect, GameObject callbackObject)
	{
		base.StartCoroutine(ServerLoginBonusSelect.Process(rewardId, rewardDays, rewardSelect, firstRewardDays, firstRewardSelect, callbackObject));
	}

	public void RequestServerRetrievePlayerState(GameObject callbackObject)
	{
		base.StartCoroutine(ServerRetrievePlayerState.Process(callbackObject));
	}

	public void RequestServerGetCharacterState(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetCharacterState.Process(callbackObject));
	}

	public void RequestServerGetChaoState(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetChaoState.Process(callbackObject));
	}

	public void RequestServerSetUserName(string userName, GameObject callbackObject)
	{
		base.StartCoroutine(ServerSetUserName.Process(userName, callbackObject));
	}

	private void Event_ServerSetTutorialComplete(int tutorialId)
	{
	}

	public void RequestServerGetWheelOptions(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetWheelOptions.Process(callbackObject));
	}

	public void RequestServerGetWheelOptionsGeneral(int eventId, int spinId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetWheelOptionsGeneral.Process(eventId, spinId, callbackObject));
	}

	public void RequestServerGetWheelSpinInfo(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetWheelSpinInfo.Process(callbackObject));
	}

	public void RequestServerCommitWheelSpin(int count, GameObject callbackObject)
	{
		base.StartCoroutine(ServerCommitWheelSpin.Process(count, callbackObject));
	}

	public void RequestServerCommitWheelSpinGeneral(int eventId, int spinId, int spinCostItemId, int spinNum, GameObject callbackObject)
	{
		base.StartCoroutine(ServerCommitWheelSpinGeneral.Process(eventId, spinId, spinCostItemId, spinNum, callbackObject));
	}

	public void RequestServerGetDailyBattleStatus(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetDailyBattleStatus.Process(callbackObject));
	}

	public void RequestServerUpdateDailyBattleStatus(GameObject callbackObject)
	{
		base.StartCoroutine(ServerUpdateDailyBattleStatus.Process(callbackObject));
	}

	public void RequestServerPostDailyBattleResult(GameObject callbackObject)
	{
		base.StartCoroutine(ServerPostDailyBattleResult.Process(callbackObject));
	}

	public void RequestServerGetDailyBattleData(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetDailyBattleData.Process(callbackObject));
	}

	public void RequestServerGetPrizeDailyBattle(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetPrizeDailyBattle.Process(callbackObject));
	}

	public void RequestServerGetDailyBattleDataHistory(int count, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetDailyBattleDataHistory.Process(count, callbackObject));
	}

	public void RequestServerResetDailyBattleMatching(int type, GameObject callbackObject)
	{
		base.StartCoroutine(ServerResetDailyBattleMatching.Process(type, callbackObject));
	}

	public void RequestServerStartAct(List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, List<string> distanceFriendIdList, bool tutorial, int? eventId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerStartAct.Process(modifiersItem, modifiersBoostItem, distanceFriendIdList, tutorial, eventId, callbackObject));
	}

	public void RequestServerQuickModeStartAct(List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, bool tutorial, GameObject callbackObject)
	{
		base.StartCoroutine(ServerQuickModeStartAct.Process(modifiersItem, modifiersBoostItem, tutorial, callbackObject));
	}

	public void RequestServerActRetry(GameObject callbackObject)
	{
		base.StartCoroutine(ServerActRetry.Process(callbackObject));
	}

	public void RequestServerPostGameResults(ServerGameResults results, GameObject callbackObject)
	{
		base.StartCoroutine(ServerPostGameResults.Process(results, callbackObject));
	}

	public void RequestServerQuickModePostGameResults(ServerQuickModeGameResults results, GameObject callbackObject)
	{
		base.StartCoroutine(ServerQuickModePostGameResults.Process(results, callbackObject));
	}

	public void RequestServerGetMenuData(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetMenuData.Process(callbackObject));
	}

	public void RequestServerGetMileageReward(int episode, int chapter, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetMileageReward.Process(episode, chapter, callbackObject));
	}

	public void RequestServerGetCostList(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetCostList.Process(callbackObject));
	}

	public void RequestServerActRetryFree(GameObject callbackObject)
	{
		base.StartCoroutine(ServerActRetryFree.Process(callbackObject));
	}

	public void RequestServerGetFreeItemList(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetFreeItemList.Process(callbackObject));
	}

	public void RequestServerGetCampaignList(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetCampaignList.Process(callbackObject));
	}

	public void RequestServerGetMileageData(string[] distanceFriendList, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetMileageData.Process(distanceFriendList, callbackObject));
	}

	public void RequestServerGetDailyMissionData(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetDailyMissionData.Process(callbackObject));
	}

	public void RequestServerGetRingItemList(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetRingItemList.Process(callbackObject));
	}

	public void RequestServerUpgradeCharacter(int characterId, int abilityId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerUpgradeCharacter.Process(characterId, abilityId, callbackObject));
	}

	public void RequestServerUnlockedCharacter(CharaType charaType, ServerItem item, GameObject callbackObject)
	{
		base.StartCoroutine(ServerUnlockedCharacter.Process(charaType, item, callbackObject));
	}

	public void RequestServerChangeCharacter(int mainCharaId, int subCharaId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerChangeCharacter.Process(mainCharaId, subCharaId, callbackObject));
	}

	public void RequestServerUseSubCharacter(bool useFlag, GameObject callbackObject)
	{
		base.StartCoroutine(ServerUseSubCharacter.Process(useFlag, callbackObject));
	}

	public void RequestServerGetLeaderboardEntries(int mode, int first, int count, int index, int rankingType, int eventId, string[] friendIdList, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetLeaderboardEntries.Process(mode, first, count, index, rankingType, eventId, friendIdList, callbackObject));
	}

	public void RequestServerGetWeeklyLeaderboardOptions(int mode, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetWeeklyLeaderboardOptions.Process(mode, callbackObject));
	}

	public void RequestServerGetLeagueData(int mode, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetLeagueData.Process(mode, callbackObject));
	}

	public void RequestServerGetLeagueOperatorData(int mode, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetLeagueOperatorData.Process(mode, callbackObject));
	}

	private void Event_ServerGetFriendsList(int first, int count)
	{
	}

	private void Event_ServerGetGameFriendsList(int first, int count)
	{
	}

	public void RequestServerRequestEnergy(string friendId, GameObject gameObject)
	{
		base.StartCoroutine(ServerRequestEnergy.Process(friendId, gameObject));
	}

	public void RequestServerGetFacebookIncentive(int incentiveType, int achievementCount, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetFacebookIncentive.Process(incentiveType, achievementCount, callbackObject));
	}

	public void RequestServerSetFacebookScopedId(string userId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerSetFacebookScopedId.Process(userId, callbackObject));
	}

	public void RequestServerGetFriendUserIdList(List<string> friendFBIdList, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetFriendUserIdList.Process(friendFBIdList, callbackObject));
	}

	public void RequestServerSetInviteHistory(string facebookIdHash, GameObject callbackObject)
	{
		base.StartCoroutine(ServerSetInviteHistory.Process(facebookIdHash, callbackObject));
	}

	public void RequestServerSetInviteCode(string friendId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerSetInviteCode.Process(friendId, callbackObject));
	}

	private void Event_ServerSendInvite(string friendId)
	{
	}

	public void RequestServerSendEnergy(string friendId, GameObject gameObject)
	{
		base.StartCoroutine(ServerSendEnergy.Process(friendId, gameObject));
	}

	public void RequestServerUpdateMessage(List<int> messageIdList, List<int> operatorMessageIdList, GameObject callbackObject)
	{
		base.StartCoroutine(ServerUpdateMessage.Process(messageIdList, operatorMessageIdList, callbackObject));
	}

	public void RequestServerGetMessageList(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetMessageList.Process(callbackObject));
	}

	public void RequestServerPreparePurchase(int itemId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerPreparePurchase.Process(itemId, callbackObject));
	}

	public void RequestServerPurchase(bool isSuccess, GameObject callbackObject)
	{
		base.StartCoroutine(ServerPurchase.Process(isSuccess, callbackObject));
	}

	public void RequestServerGetRedStarExchangeList(int itemType, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetRedStarExchangeList.Process(itemType, callbackObject));
	}

	public void RequestServerRedStarExchange(int storeItemId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerRedStarExchange.Process(storeItemId, callbackObject));
	}

	public void RequestServerBuyIos(string receiptData, GameObject callbackObject)
	{
		base.StartCoroutine(ServerBuyIos.Process(receiptData, callbackObject));
	}

	public void RequestServerBuyAndroid(string receiptData, string signature, GameObject callbackObject)
	{
		base.StartCoroutine(ServerBuyAndroid.Process(receiptData, signature, callbackObject));
	}

	public void RequestServerGetRingExchangeList(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetRingExchangeList.Process(callbackObject));
	}

	public void RequestServerSetBirthday(string birthday, GameObject callbackObject)
	{
		base.StartCoroutine(ServerSetBirthday.Process(birthday, callbackObject));
	}

	public void RequestServerRingExchange(int itemId, int itemNum, GameObject callbackObject)
	{
		base.StartCoroutine(ServerRingExchange.Process(itemId, itemNum, callbackObject));
	}

	public void RequestServerGetItemStockNum(int eventId, List<int> itemId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetItemStockNum.Process(eventId, itemId, callbackObject));
	}

	public void RequestServerGetItemStockNum(int eventId, int itemId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetItemStockNum.Process(eventId, new List<int>
		{
			itemId
		}, callbackObject));
	}

	public void RequestServerGetItemStockNum(int eventId, ServerItem.Id itemId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetItemStockNum.Process(eventId, new List<int>
		{
			(int)itemId
		}, callbackObject));
	}

	public void RequestServerGetChaoWheelOptions(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetChaoWheelOptions.Process(callbackObject));
	}

	public void RequestServerGetPrizeChaoWheelSpin(int chaoWheelSpinType, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetPrizeChaoWheelSpin.Process(chaoWheelSpinType, callbackObject));
	}

	public void RequestServerGetPrizeWheelSpinGeneral(int eventId, int spinType, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetPrizeWheelSpinGeneral.Process(eventId, spinType, callbackObject));
	}

	public void RequestServerCommitChaoWheelSpin(int count, GameObject callbackObject)
	{
		base.StartCoroutine(ServerCommitChaoWheelSpin.Process(count, callbackObject));
	}

	public void RequestServerGetChaoRentalStates(string[] frindId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetChaoRentalStates.Process(frindId, callbackObject));
	}

	public void RequestServerEquipChao(int mainChaoId, int subChaoId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerEquipChao.Process(mainChaoId, subChaoId, callbackObject));
	}

	public void RequestServerGetFirstLaunchChao(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetFirstLaunchChao.Process(callbackObject));
	}

	public void RequestServerAddSpecialEgg(int numSpecialEgg, GameObject callbackObject)
	{
		base.StartCoroutine(ServerAddSpecialEgg.Process(numSpecialEgg, callbackObject));
	}

	public void RequestServerEquipItem(List<ItemType> items, GameObject callbackObject)
	{
		base.StartCoroutine(ServerEquipItem.Process(items, callbackObject));
	}

	public void RequestServerOptionUserResult(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetOptionUserResult.Process(callbackObject));
	}

	public void RequestServerAtomSerial(string campaignId, string serial, bool new_user, GameObject callbackObject)
	{
		base.StartCoroutine(ServerAtomSerial.Process(campaignId, serial, new_user, callbackObject));
	}

	public void RequestServerSendApollo(int type, string[] value, GameObject callbackObject)
	{
		base.StartCoroutine(ServerSendApollo.Process(type, value, callbackObject));
	}

	public void RequestServerSetNoahId(string noahId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerSetNoahId.Process(noahId, callbackObject));
	}

	public void RequestServerGetEventList(GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetEventList.Process(callbackObject));
	}

	public void RequestServerGetEventReward(int eventId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetEventReward.Process(eventId, callbackObject));
	}

	public void RequestServerEventStartAct(int eventId, int energyExpend, long raidBossId, List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, GameObject callbackObject)
	{
		base.StartCoroutine(ServerEventStartAct.Process(eventId, energyExpend, raidBossId, modifiersItem, modifiersBoostItem, callbackObject));
	}

	public void RequestServerEventUpdateGameResults(ServerEventGameResults results, GameObject callbackObject)
	{
		base.StartCoroutine(ServerEventUpdateGameResults.Process(results, callbackObject));
	}

	public void RequestServerEventPostGameResults(int eventId, int numRaidBossRings, GameObject callbackObject)
	{
		base.StartCoroutine(ServerEventPostGameResults.Process(eventId, numRaidBossRings, callbackObject));
	}

	public void RequestServerGetEventState(int eventId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetEventState.Process(eventId, callbackObject));
	}

	public void RequestServerGetEventUserRaidBossList(int eventId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetEventUserRaidBossList.Process(eventId, callbackObject));
	}

	public void RequestServerGetEventUserRaidBossState(int eventId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetEventUserRaidBossState.Process(eventId, callbackObject));
	}

	public void RequestServerGetEventRaidBossUserList(int eventId, long raidBossId, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetEventRaidBossUserList.Process(eventId, raidBossId, callbackObject));
	}

	public void RequestServerGetEventRaidBossDesiredList(int eventId, long raidBossId, List<string> friendIdList, GameObject callbackObject)
	{
		base.StartCoroutine(ServerGetEventRaidBossDesiredList.Process(eventId, raidBossId, friendIdList, callbackObject));
	}

	public void RequestServerDrawRaidBoss(int eventId, long score, GameObject callbackObject)
	{
		base.StartCoroutine(ServerDrawRaidBoss.Process(eventId, score, callbackObject));
	}

	private void Event_SPLoginUpgrade_Success()
	{
		ServerInterface.Reset();
	}

	public static bool IsRSREnable()
	{
		return ServerInterface.s_redStartItemState != null && ServerInterface.s_redStartItemState.Count > 0;
	}

	public static void DebugInit()
	{
		ServerInterface.Init();
	}
}
