using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class BindingLink : IDisposable
{
	public const string PluginName = "UnmanagedProcess";

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern void CreateMD5String([MarshalAs(UnmanagedType.LPStr)] StringBuilder result, string loginKey, string userId, string password, int randomNum);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern bool IsEnableEncrypto();

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern void ResetResultScore();

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern NativeScoreResult GetResultScore();

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CheckHalfWayResultScore(PostGameResultNativeParam param, [In] [Out] StageScoreData[] scoreDatas, int dataSize);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CheckHalfWayQuickModeResultScore(QuickModePostGameResultNativeParam param, [In] [Out] StageScoreData[] scoreDatas, int dataSize);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CheckQuickModeResultTimer(QuickModeTimerNativeParam param);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcResponseString(string responseText, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern void GetSendString([MarshalAs(UnmanagedType.LPStr)] StringBuilder result, int stringSize);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern void GetResponseString([MarshalAs(UnmanagedType.LPStr)] StringBuilder result, string responseText, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcOnlySendBaseparamString([In] SendBaseNativeParam baseInfo, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcLoginString([In] SendBaseNativeParam baseInfo, string userId, string password, string migrationPassword, int platform, string device, int language, int salesLocale, int storeId, int platformSns, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcMigrationString([In] SendBaseNativeParam baseInfo, string userId, string migrationPassword, string migrationUserPassword, int platform, string device, int language, int salesLocale, int storeId, int platformSns, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetMigrationPasswordString([In] SendBaseNativeParam baseInfo, string userPassword, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcSetUserNameString([In] SendBaseNativeParam baseInfo, string userName, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcActStartString([In] SendBaseNativeParam baseInfo, [In] ActStartNativeParam actStartParam, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 50)] string[] distanceFriendList, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcQuickModeActStartString([In] SendBaseNativeParam baseInfo, [In] QuickModeActStartNativeParam actStartParam, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcActRetryString([In] SendBaseNativeParam baseInfo, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcPostGameResultString([In] SendBaseNativeParam baseInfo, [In] PostGameResultNativeParam param, [In] [Out] StageScoreData[] scoreDatas, int dataSize, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcQuickModePostGameResultString([In] SendBaseNativeParam baseInfo, [In] QuickModePostGameResultNativeParam param, [In] [Out] StageScoreData[] scoreDatas, int dataSize, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetMileageRewardString([In] SendBaseNativeParam baseInfo, int episode, int chapter, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcUpgradeCharacterString([In] SendBaseNativeParam baseInfo, int characterId, int abilityId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcUnlockedCharacterString([In] SendBaseNativeParam baseInfo, int characterId, int itemId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcChangeCharacterString([In] SendBaseNativeParam baseInfo, int mainCharacterId, int subCharacterId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetWeeklyLeaderboardEntryString([In] SendBaseNativeParam baseInfo, LeaderboardEntryNativeParam leaderboardEntryParam, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 50)] string[] friendIdList, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcLeagueOperatorDataString([In] SendBaseNativeParam baseInfo, int mode, int league, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcLeagueDataString([In] SendBaseNativeParam baseInfo, int mode, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcWeeklyLeaderboardOptionsString([In] SendBaseNativeParam baseInfo, int mode, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcInviteCodeString([In] SendBaseNativeParam baseInfo, string friendId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetFacebookIncentiveString([In] SendBaseNativeParam baseInfo, int type, int achievementCount, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcSetFacebookScopedIdString([In] SendBaseNativeParam baseInfo, string facebookId, string cryptoInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetFriendUserIdListString([In] SendBaseNativeParam baseInfo, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 50)] string[] friendIdList, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcSendEnergyString([In] SendBaseNativeParam baseInfo, string friendId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetMessageString([In] SendBaseNativeParam baseInfo, [In] [Out] int[] messageIdList, int messageIdSize, [In] [Out] int[] operationMessageIdList, int operationMessageIdSize, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetRedStarExchangeListString([In] SendBaseNativeParam baseInfo, int itemType, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcRedStarExchangeString([In] SendBaseNativeParam baseInfo, int itemId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcBuyIosString([In] SendBaseNativeParam baseInfo, string receiptData, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcBuyAndroidString([In] SendBaseNativeParam baseInfo, string receiptData, string receiptSignature, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcSetBirthdayString([In] SendBaseNativeParam baseInfo, string birthday, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcCommitWheelSpinString([In] SendBaseNativeParam baseInfo, int count, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcCommitChaoWheelSpinString([In] SendBaseNativeParam baseInfo, int count, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcEquipChaoString([In] SendBaseNativeParam baseInfo, int mainChaoId, int subChaoId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcAddSpecialEggString([In] SendBaseNativeParam baseInfo, int numSpecialEgg, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetPrizeChaoWheelSpinString([In] SendBaseNativeParam baseInfo, int chaoWheelSpinType, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetWheelSpinGeneralString([In] SendBaseNativeParam baseInfo, int eventId, int spinId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcCommitWheelSpinGeneralString([In] SendBaseNativeParam baseInfo, int eventId, int spinCostItemId, int spinId, int spinNum, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetPrizeWheelSpinGeneralString([In] SendBaseNativeParam baseInfo, int eventId, int spinType, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcSendAppoloString([In] SendBaseNativeParam baseInfo, int type, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 100)] string[] value, int paramSize, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcSetSerialCodeString([In] SendBaseNativeParam baseInfo, string campaignId, string serialCode, bool newUser, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcSetNoahIdString([In] SendBaseNativeParam baseInfo, string noahId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetEventRewardString([In] SendBaseNativeParam baseInfo, int eventId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcEventStateString([In] SendBaseNativeParam baseInfo, int eventId, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern int CalcGetItemStockNum([In] SendBaseNativeParam baseInfo, int eventId, [In] [Out] int[] itemIdList, int listSize, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern void BootGameAction();

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern void BeforeGameAction();

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern int CalcGetDailyBattleDataHistoryString([In] SendBaseNativeParam baseInfo, int count, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern int CalcResetDailyBattleMatchingString([In] SendBaseNativeParam baseInfo, int type, string encryptInitVector);

	[DllImport("UnmanagedProcess", CallingConvention = CallingConvention.Cdecl)]
	private static extern int CalcLoginBonusSelectString([In] SendBaseNativeParam baseInfo, int rewardId, int rewardDays, int rewardSelect, int firstRewardDays, int firstRewardSelect, string encryptInitVector);

	public void Dispose()
	{
	}

	public string GetMD5String(string loginKey, string userId, string password, int randomNum)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = 34;
		BindingLink.CreateMD5String(stringBuilder, loginKey, userId, password, randomNum);
		return stringBuilder.ToString();
	}

	public bool IsSecure()
	{
		return BindingLink.IsEnableEncrypto();
	}

	public void ResetNativeResultScore()
	{
		BindingLink.ResetResultScore();
	}

	public NativeScoreResult GetNativeResultScore()
	{
		NativeScoreResult resultScore = BindingLink.GetResultScore();
		UnityEngine.Debug.Log("Kenzan-score.stageScore = " + resultScore.stageScore.ToString(), DebugTraceManager.TraceType.SERVER);
		UnityEngine.Debug.Log("Kenzan-score.ringScore = " + resultScore.ringScore.ToString(), DebugTraceManager.TraceType.SERVER);
		UnityEngine.Debug.Log("Kenzan-score.animalScore = " + resultScore.animalScore.ToString(), DebugTraceManager.TraceType.SERVER);
		UnityEngine.Debug.Log("Kenzan-score.distanceScore = " + resultScore.distanceScore.ToString(), DebugTraceManager.TraceType.SERVER);
		UnityEngine.Debug.Log("Kenzan-score.finalScore = " + resultScore.finalScore.ToString());
		UnityEngine.Debug.Log("Kenzan-score.redStarRingCount = " + resultScore.redStarRingCount.ToString(), DebugTraceManager.TraceType.SERVER);
		return resultScore;
	}

	public void CheckNativeHalfWayResultScore(ServerGameResults gameResult)
	{
		PostGameResultNativeParam param = default(PostGameResultNativeParam);
		param.Init(gameResult);
		StageScoreManager instance = StageScoreManager.Instance;
		StageScorePool scorePool = instance.ScorePool;
		BindingLink.CheckHalfWayResultScore(param, scorePool.scoreDatas, scorePool.StoredCount);
	}

	public void CheckNativeHalfWayQuickModeResultScore(ServerQuickModeGameResults gameResult)
	{
		QuickModePostGameResultNativeParam param = default(QuickModePostGameResultNativeParam);
		param.Init(gameResult);
		StageScoreManager instance = StageScoreManager.Instance;
		StageScorePool scorePool = instance.ScorePool;
		BindingLink.CheckHalfWayQuickModeResultScore(param, scorePool.scoreDatas, scorePool.StoredCount);
	}

	public void CheckNativeQuickModeResultTimer(int gold, int silver, int bronze, int continueCount, int main, int sub, int total, long playTime)
	{
		QuickModeTimerNativeParam param = default(QuickModeTimerNativeParam);
		param.Init(gold, silver, bronze, continueCount, main, sub, total, playTime);
		BindingLink.CheckQuickModeResultTimer(param);
	}

	public void BootGameCheatCheck()
	{
		BindingLink.BootGameAction();
	}

	public void BeforeGameCheatCheck()
	{
		BindingLink.BeforeGameAction();
	}

	public string DecodeServerResponseText(string responseText)
	{
		int capacity = BindingLink.CalcResponseString(responseText, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = capacity;
		BindingLink.GetResponseString(stringBuilder, responseText, CryptoUtility.code);
		UnityEngine.Debug.Log("DecodeServerResponseText() = " + stringBuilder, DebugTraceManager.TraceType.SERVER);
		return stringBuilder.ToString();
	}

	public string GetOnlySendBaseParamString()
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcOnlySendBaseparamString(baseInfo, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetLoginString(string userId, string password, string migrationPassword, int platform, string device, int language, int salesLocale, int storeId, int platformSns)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcLoginString(baseInfo, userId, password, migrationPassword, platform, device, language, salesLocale, storeId, platformSns, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetMigrationString(string userId, string migrationPassword, string migrationUserPassword, int platform, string device, int language, int salesLocale, int storeId, int platformSns)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcMigrationString(baseInfo, userId, migrationPassword, migrationUserPassword, platform, device, language, salesLocale, storeId, platformSns, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetMigrationPasswordString(string userPassword)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetMigrationPasswordString(baseInfo, userPassword, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetSetUserNameString(string userName)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcSetUserNameString(baseInfo, userName, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetActStartString(List<int> modifire, List<string> distanceFriendList, bool tutorial, int eventId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		ActStartNativeParam actStartParam = default(ActStartNativeParam);
		actStartParam.Init(modifire, tutorial, eventId);
		string[] array = new string[50];
		for (int i = 0; i < 50; i++)
		{
			if (i < distanceFriendList.Count)
			{
				array[i] = distanceFriendList[i];
			}
			else
			{
				array[i] = null;
			}
		}
		int num = BindingLink.CalcActStartString(baseInfo, actStartParam, array, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetQuickModeActStartString(List<int> modifire, int tutorial)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		QuickModeActStartNativeParam actStartParam = default(QuickModeActStartNativeParam);
		actStartParam.Init(modifire, tutorial);
		int num = BindingLink.CalcQuickModeActStartString(baseInfo, actStartParam, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetActRetryString()
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcActRetryString(baseInfo, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetPostGameResultString(ServerGameResults gameResult)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		PostGameResultNativeParam param = default(PostGameResultNativeParam);
		param.Init(gameResult);
		StageScoreManager instance = StageScoreManager.Instance;
		StageScorePool scorePool = instance.ScorePool;
		int num = BindingLink.CalcPostGameResultString(baseInfo, param, scorePool.scoreDatas, scorePool.StoredCount, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetQuickModePostGameResultString(ServerQuickModeGameResults gameResult)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		QuickModePostGameResultNativeParam param = default(QuickModePostGameResultNativeParam);
		param.Init(gameResult);
		StageScoreManager instance = StageScoreManager.Instance;
		StageScorePool scorePool = instance.ScorePool;
		int num = BindingLink.CalcQuickModePostGameResultString(baseInfo, param, scorePool.scoreDatas, scorePool.StoredCount, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetMileageRewardString(int episode, int chapter)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetMileageRewardString(baseInfo, episode, chapter, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetUpgradeCharacterString(int characterId, int abilityId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcUpgradeCharacterString(baseInfo, characterId, abilityId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetUnlockedCharacterString(int characterId, int itemId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcUnlockedCharacterString(baseInfo, characterId, itemId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetChangeCharacterString(int mainCharacterId, int subCharacterId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcChangeCharacterString(baseInfo, mainCharacterId, subCharacterId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetWeeklyLeaderboardEntries(int mode, int first, int count, int type, List<string> friendIdList, int eventId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		LeaderboardEntryNativeParam leaderboardEntryParam = default(LeaderboardEntryNativeParam);
		leaderboardEntryParam.Init(mode, first, count, type, eventId);
		string[] array = new string[50];
		for (int i = 0; i < 50; i++)
		{
			if (i < friendIdList.Count)
			{
				array[i] = friendIdList[i];
			}
			else
			{
				array[i] = null;
			}
		}
		int num = BindingLink.CalcGetWeeklyLeaderboardEntryString(baseInfo, leaderboardEntryParam, array, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetLeagueOperatorDataString(int mode, int league)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcLeagueOperatorDataString(baseInfo, mode, league, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetLeagueDataString(int mode)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcLeagueDataString(baseInfo, mode, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetWeeklyLeaderboardOptionsString(int mode)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcWeeklyLeaderboardOptionsString(baseInfo, mode, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetSetInviteCodeString(string friendId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcInviteCodeString(baseInfo, friendId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetFacebookIncentiveString(int type, int achievementCount)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetFacebookIncentiveString(baseInfo, type, achievementCount, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetSetFacebookScopedIdString(string facebookId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcSetFacebookScopedIdString(baseInfo, facebookId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetFacebookFriendUserIdList(List<string> facebookIdList)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		string[] array = new string[50];
		for (int i = 0; i < 50; i++)
		{
			if (i < facebookIdList.Count)
			{
				array[i] = facebookIdList[i];
			}
			else
			{
				array[i] = null;
			}
		}
		int num = BindingLink.CalcGetFriendUserIdListString(baseInfo, array, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetSendEnergyString(string friendId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcSendEnergyString(baseInfo, friendId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetMessageString(List<int> messageId, List<int> operationMessageId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		List<int> list = new List<int>();
		if (messageId != null)
		{
			foreach (int current in messageId)
			{
				list.Add(current);
			}
		}
		List<int> list2 = new List<int>();
		if (operationMessageId != null)
		{
			foreach (int current2 in operationMessageId)
			{
				list2.Add(current2);
			}
		}
		int num = BindingLink.CalcGetMessageString(baseInfo, list.ToArray(), list.Count, list2.ToArray(), list2.Count, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetRedStarExchangeListString(int itemType)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetRedStarExchangeListString(baseInfo, itemType, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetRedStarExchangeString(int itemId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcRedStarExchangeString(baseInfo, itemId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetBuyIosString(string receiptData)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcBuyIosString(baseInfo, receiptData, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetBuyAndroidString(string receiptData, string receiptSignature)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcBuyAndroidString(baseInfo, receiptData, receiptSignature, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetSetBirthdayString(string birthday)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcSetBirthdayString(baseInfo, birthday, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetCommitWheelSpinString(int count)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcCommitWheelSpinString(baseInfo, count, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetCommitChaoWheelSpinString(int count)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcCommitChaoWheelSpinString(baseInfo, count, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetEquipChaoString(int mainChaoId, int subChaoId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcEquipChaoString(baseInfo, mainChaoId, subChaoId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetAddSpecialEggString(int numSpecialEgg)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcAddSpecialEggString(baseInfo, numSpecialEgg, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetPrizeChaoWheelSpinString(int chaoWheelSpinType)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetPrizeChaoWheelSpinString(baseInfo, chaoWheelSpinType, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetWheelSpinGeneralString(int eventId, int spinId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetWheelSpinGeneralString(baseInfo, eventId, spinId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetCommitWheelSpinGeneralString(int eventId, int spinCostItemId, int spinId, int spinNum)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcCommitWheelSpinGeneralString(baseInfo, eventId, spinCostItemId, spinId, spinNum, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetPrizeWheelSpinGeneralString(int eventId, int spinType)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetPrizeWheelSpinGeneralString(baseInfo, eventId, spinType, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetSendApolloString(int type, List<string> value)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcSendAppoloString(baseInfo, type, value.ToArray(), value.Count, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetSetSerialCodeString(string campaignId, string serialCode, bool newUser)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcSetSerialCodeString(baseInfo, campaignId, serialCode, newUser, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetSetNoahIdString(string noahId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcSetNoahIdString(baseInfo, noahId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetEventRewardString(int eventId)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetEventRewardString(baseInfo, eventId, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetGetItemStockNumString(int eventId, int[] itemIdList)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int listSize = 0;
		if (itemIdList != null)
		{
			listSize = itemIdList.Length;
		}
		int num = BindingLink.CalcGetItemStockNum(baseInfo, eventId, itemIdList, listSize, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string GetDailyBattleDataHistoryString(int count)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcGetDailyBattleDataHistoryString(baseInfo, count, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string ResetDailyBattleMatchingString(int type)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcResetDailyBattleMatchingString(baseInfo, type, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	public string LoginBonusSelectString(int rewardId, int rewardDays, int rewardSelect, int firstRewardDays, int firstRewardSelect)
	{
		SendBaseNativeParam baseInfo = default(SendBaseNativeParam);
		baseInfo.Init();
		int num = BindingLink.CalcLoginBonusSelectString(baseInfo, rewardId, rewardDays, rewardSelect, firstRewardDays, firstRewardSelect, CryptoUtility.code);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = num;
		BindingLink.GetSendString(stringBuilder, num);
		return stringBuilder.ToString();
	}

	private static void DebugLogCallbackFromCPlusPlus(string log)
	{
		UnityEngine.Debug.Log("From C++ [" + log + "]", DebugTraceManager.TraceType.SERVER);
	}
}
