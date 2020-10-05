using App;
using DataTable;
using LitJson;
using SaveData;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class NetUtil
{
	public static readonly float ConnectTimeOut = 15f;

	private static readonly TimeSpan ReloginStartTime = new TimeSpan(0, 30, 0);

	private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static string GetWebPageURL(InformationDataTable.Type type)
	{
		return NetBaseUtil.InformationServerURL + InformationDataTable.GetUrl(type);
	}

	public static string GetAssetBundleUrl()
	{
		string str = NetBaseUtil.AssetServerURL + "assetbundle/";
		return str + "android/";
	}

	public static string Base64Encode(string text)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
	}

	public static byte[] Base64EncodeToBytes(string text)
	{
		string s = NetUtil.Base64Encode(text);
		return Encoding.ASCII.GetBytes(s);
	}

	public static string Base64Decode(string text)
	{
		byte[] bytes = Convert.FromBase64String(text);
		return Encoding.UTF8.GetString(bytes);
	}

	public static string Base64DecodeFromBytes(byte[] byte_base64)
	{
		string @string = Encoding.ASCII.GetString(byte_base64);
		return NetUtil.Base64Decode(@string);
	}

	public static byte[] Xor(byte[] bytes)
	{
		byte[] array = new byte[bytes.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (bytes[i] ^ 255);
		}
		return array;
	}

	public static string CalcMD5String(string plainText)
	{
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
		string text = string.Empty;
		byte[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			byte b = array2[i];
			string text2 = b.ToString("X2");
			text += text2.ToLower();
		}
		return text;
	}

	public static void SyncSaveDataAndDataBase(ServerPlayerState playerState)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		ItemData itemData = instance.ItemData;
		if (itemData != null)
		{
			itemData.RingCount = (uint)playerState.m_numRings;
			itemData.RedRingCount = (uint)playerState.m_numRedRings;
			for (ItemType itemType = ItemType.INVINCIBLE; itemType < ItemType.NUM; itemType++)
			{
				ServerItem serverItem = new ServerItem(itemType);
				ServerItemState itemStateById = playerState.GetItemStateById((int)serverItem.id);
				if (itemStateById != null)
				{
					itemData.ItemCount[(int)itemType] = (uint)itemStateById.m_num;
				}
			}
		}
		PlayerData playerData = instance.PlayerData;
		if (playerData != null)
		{
			playerData.ProgressStatus = (uint)playerState.m_dailyMissionValue;
			playerData.TotalDistance = playerState.m_totalDistance;
			playerData.ChallengeCount = (uint)playerState.m_numEnergy;
			playerData.BestScore = playerState.m_totalHighScore;
			playerData.BestScoreQuick = playerState.m_totalHighScoreQuick;
			PlayerData arg_DA_0 = playerData;
			ServerItem serverItem2 = new ServerItem((ServerItem.Id)playerState.m_mainCharaId);
			arg_DA_0.MainChara = serverItem2.charaType;
			PlayerData arg_F5_0 = playerData;
			ServerItem serverItem3 = new ServerItem((ServerItem.Id)playerState.m_subCharaId);
			arg_F5_0.SubChara = serverItem3.charaType;
			playerData.Rank = (uint)playerState.m_numRank;
			PlayerData arg_11D_0 = playerData;
			ServerItem serverItem4 = new ServerItem((ServerItem.Id)playerState.m_mainChaoId);
			arg_11D_0.MainChaoID = serverItem4.chaoId;
			PlayerData arg_138_0 = playerData;
			ServerItem serverItem5 = new ServerItem((ServerItem.Id)playerState.m_subChaoId);
			arg_138_0.SubChaoID = serverItem5.chaoId;
			playerData.DailyMission.id = playerState.m_dailyMissionId;
			playerData.DailyMission.progress = (long)playerState.m_dailyMissionValue;
			playerData.DailyMission.missions_complete = playerState.m_dailyChallengeComplete;
			playerData.DailyMission.clear_count = playerState.m_numDailyChalCont;
		}
	}

	public static void SyncSaveDataAndDataBase(ServerCharacterState[] charaState)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		CharaData charaData = instance.CharaData;
		if (charaData != null)
		{
			for (int i = 0; i < 29; i++)
			{
				ServerCharacterState serverCharacterState = charaState[i];
				if (serverCharacterState != null)
				{
					CharaAbility charaAbility = charaData.AbilityArray[i];
					if (charaAbility != null)
					{
						for (int j = 0; j < serverCharacterState.AbilityLevel.Count; j++)
						{
							int id = 120000 + j;
							ServerItem serverItem = new ServerItem((ServerItem.Id)id);
							AbilityType abilityType = serverItem.abilityType;
							if (abilityType != AbilityType.NONE)
							{
								charaAbility.Ability[(int)abilityType] = (uint)serverCharacterState.AbilityLevel[j];
							}
						}
						if (serverCharacterState != null)
						{
							if (serverCharacterState.IsUnlocked)
							{
								charaData.Status[i] = 1;
							}
							else
							{
								charaData.Status[i] = 0;
							}
						}
					}
				}
			}
		}
	}

	public static void SyncSaveDataAndDataBase(List<ServerChaoState> chaoState)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		if (chaoState != null)
		{
			instance.ChaoData = new SaveData.ChaoData(chaoState);
		}
	}

	public static void SyncSaveDataAndDailyMission(ServerDailyChallengeState dailyChallengeState = null)
	{
		if (dailyChallengeState == null)
		{
			dailyChallengeState = ServerInterface.DailyChallengeState;
		}
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		PlayerData playerData = instance.PlayerData;
		if (playerData == null)
		{
			return;
		}
		playerData.DailyMission.clear_count = dailyChallengeState.m_numIncentiveCont;
		playerData.DailyMission.date = dailyChallengeState.m_numDailyChalDay;
		playerData.DailyMission.max = dailyChallengeState.m_maxDailyChalDay;
		playerData.DailyMission.reward_max = dailyChallengeState.m_maxIncentive;
		foreach (ServerDailyChallengeIncentive current in dailyChallengeState.m_incentiveList)
		{
			int num = current.m_numIncentiveCont - 1;
			UnityEngine.Debug.Log(string.Empty);
			int[] arg_C0_0 = playerData.DailyMission.reward_id;
			int arg_C0_1 = num;
			ServerItem serverItem = new ServerItem((ServerItem.Id)current.m_itemId);
			arg_C0_0[arg_C0_1] = serverItem.rewardType;
			playerData.DailyMission.reward_count[num] = current.m_num;
		}
	}

	public static bool IsAlreadySessionTimeOut(DateTime sessionTimeLimit, DateTime currentTime)
	{
		if (sessionTimeLimit <= currentTime)
		{
			return true;
		}
		TimeSpan t = new TimeSpan(0, 0, 0, (int)NetUtil.ConnectTimeOut);
		DateTime t2 = currentTime + t;
		return sessionTimeLimit <= t2;
	}

	public static bool IsSessionTimeOutSoon(DateTime sessionTimeLimit, DateTime currentTime)
	{
		return NetUtil.IsAlreadySessionTimeOut(sessionTimeLimit, currentTime + NetUtil.ReloginStartTime);
	}

	private static JsonData Find(JsonData from, string key)
	{
		return from[key];
	}

	public static bool IsExist(JsonData from, string key)
	{
		return from.ContainsKey(key);
	}

	public static JsonData GetJsonArray(JsonData jdata, string key)
	{
		if (jdata == null)
		{
			return null;
		}
		if (!NetUtil.IsExist(jdata, key))
		{
			return null;
		}
		JsonData jsonData = NetUtil.Find(jdata, key);
		if (jsonData == null)
		{
			return null;
		}
		if (jsonData.IsArray)
		{
			return jsonData;
		}
		UnityEngine.Debug.Log("GetJsonArray : There is not array : " + key);
		return null;
	}

	public static JsonData GetJsonArrayObject(JsonData jdata, string key, int index)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, key);
		if (jsonArray != null)
		{
			return jsonArray[index];
		}
		return null;
	}

	public static JsonData GetJsonObject(JsonData jdata, string key)
	{
		if (!jdata.ContainsKey(key))
		{
			return null;
		}
		JsonData jsonData = NetUtil.Find(jdata, key);
		if (jsonData != null && jsonData.IsObject)
		{
			return jsonData;
		}
		return null;
	}

	public static string GetJsonString(JsonData jdata, string key)
	{
		if (!jdata.ContainsKey(key))
		{
			return string.Empty;
		}
		JsonData jdata2 = NetUtil.Find(jdata, key);
		return NetUtil.GetJsonString(jdata2);
	}

	public static string GetJsonString(JsonData jdata)
	{
		string result = null;
		if (jdata != null)
		{
			if (jdata.IsString)
			{
				result = (string)jdata;
			}
			else if (jdata.IsInt)
			{
				result = ((int)jdata).ToString();
			}
			else if (jdata.IsLong)
			{
				result = ((long)jdata).ToString();
			}
			else
			{
				UnityEngine.Debug.Log("GetJsonIntValue : Illegal JSON Object");
			}
		}
		return result;
	}

	public static float GetJsonFloat(JsonData jdata, string key)
	{
		if (!jdata.ContainsKey(key))
		{
			return 0f;
		}
		JsonData jdata2 = NetUtil.Find(jdata, key);
		return NetUtil.GetJsonFloat(jdata2);
	}

	public static float GetJsonFloat(JsonData jdata)
	{
		float result = 0f;
		if (jdata != null)
		{
			if (jdata.IsDouble)
			{
				result = (float)(long)jdata;
			}
			else if (jdata.IsString)
			{
				string s = (string)jdata;
				if (!float.TryParse(s, out result))
				{
					result = 0f;
				}
			}
			else
			{
				UnityEngine.Debug.Log("GetJsonFloat : Illegal JSON Object");
			}
		}
		return result;
	}

	public static int GetJsonInt(JsonData jdata, string key)
	{
		if (!jdata.ContainsKey(key))
		{
			return 0;
		}
		JsonData jdata2 = NetUtil.Find(jdata, key);
		return NetUtil.GetJsonInt(jdata2);
	}

	public static int GetJsonInt(JsonData jdata)
	{
		int result = 0;
		if (jdata != null)
		{
			if (jdata.IsInt)
			{
				result = (int)jdata;
			}
			else if (jdata.IsString)
			{
				string s = (string)jdata;
				if (!int.TryParse(s, out result))
				{
					result = 0;
				}
			}
			else
			{
				UnityEngine.Debug.Log("GetJsonIntValue : Illegal JSON Object");
			}
		}
		return result;
	}

	public static long GetJsonLong(JsonData jdata, string key)
	{
		if (!jdata.ContainsKey(key))
		{
			return 0L;
		}
		JsonData jdata2 = NetUtil.Find(jdata, key);
		return NetUtil.GetJsonLong(jdata2);
	}

	public static long GetJsonLong(JsonData jdata)
	{
		long result = 0L;
		if (jdata != null)
		{
			if (jdata.IsLong)
			{
				result = (long)jdata;
			}
			else if (jdata.IsInt)
			{
				result = (long)(int)jdata;
			}
			else if (jdata.IsString)
			{
				string s = (string)jdata;
				if (!long.TryParse(s, out result))
				{
					result = 0L;
				}
			}
			else
			{
				UnityEngine.Debug.Log("GetJsonIntValue : Illegal JSON Object");
			}
		}
		return result;
	}

	public static bool GetJsonFlag(JsonData jdata, string key)
	{
		if (!jdata.ContainsKey(key))
		{
			return false;
		}
		JsonData jdata2 = NetUtil.Find(jdata, key);
		return NetUtil.GetJsonFlag(jdata2);
	}

	public static bool GetJsonFlag(JsonData jdata)
	{
		return NetUtil.GetJsonInt(jdata) != 0;
	}

	public static bool GetJsonBoolean(JsonData jdata, string key)
	{
		if (!jdata.ContainsKey(key))
		{
			return false;
		}
		JsonData jdata2 = NetUtil.Find(jdata, key);
		return NetUtil.GetJsonBoolean(jdata2);
	}

	public static bool GetJsonBoolean(JsonData jdata)
	{
		return jdata.IsBoolean && (bool)jdata;
	}

	public static void WriteValue(JsonWriter writer, string propertyName, object value)
	{
		if (writer == null)
		{
			return;
		}
		if (propertyName != null && string.Empty != propertyName)
		{
			writer.WritePropertyName(propertyName);
		}
		if (value is string)
		{
			writer.Write((string)value);
		}
		else if (value is int)
		{
			writer.Write(((int)value).ToString());
		}
		else if (value is long)
		{
			writer.Write(((long)value).ToString());
		}
		else if (value is ulong)
		{
			writer.Write(((ulong)value).ToString());
		}
		else if (value is float)
		{
			writer.Write(((float)value).ToString());
		}
		else if (value is bool)
		{
			writer.Write(((bool)value).ToString());
		}
	}

	public static void WriteObject(JsonWriter writer, string objectName, Dictionary<string, object> properties)
	{
		if (writer == null)
		{
			return;
		}
		if (properties == null)
		{
			return;
		}
		if (objectName != null && string.Empty != objectName)
		{
			writer.WritePropertyName(objectName);
		}
		writer.WriteObjectStart();
		foreach (KeyValuePair<string, object> current in properties)
		{
			string key = current.Key;
			object value = current.Value;
			NetUtil.WriteValue(writer, key, value);
		}
		writer.WriteObjectEnd();
	}

	public static void WriteArray(JsonWriter writer, string arrayName, List<object> objects)
	{
		if (writer == null)
		{
			return;
		}
		if (objects == null)
		{
			return;
		}
		writer.WritePropertyName(arrayName);
		writer.WriteArrayStart();
		int count = objects.Count;
		for (int i = 0; i < count; i++)
		{
			object value = objects[i];
			NetUtil.WriteValue(writer, string.Empty, value);
		}
		writer.WriteArrayEnd();
	}

	public static List<ServerDailyBattleDataPair> AnalyzeDailyBattleDataPairListJson(JsonData jdata, string key = "battleDataList")
	{
		List<ServerDailyBattleDataPair> list = new List<ServerDailyBattleDataPair>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, key);
		if (jsonArray == null)
		{
			return list;
		}
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jsonData = jsonArray[i];
			if (jsonData != null)
			{
				ServerDailyBattleDataPair item = NetUtil.AnalyzeDailyBattleDataPairJson(jsonData, "battleData", "rivalBattleData", "startTime", "endTime");
				list.Add(item);
			}
		}
		return list;
	}

	public static ServerDailyBattleDataPair AnalyzeDailyBattleDataPairJson(JsonData jdata, string myKey = "battleData", string rivalKey = "rivalBattleData", string startTimeKey = "startTime", string endTimeKey = "endTime")
	{
		ServerDailyBattleDataPair serverDailyBattleDataPair = new ServerDailyBattleDataPair();
		if (string.IsNullOrEmpty(myKey))
		{
			myKey = "battleData";
		}
		if (string.IsNullOrEmpty(rivalKey))
		{
			rivalKey = "rivalBattleData";
		}
		if (string.IsNullOrEmpty(startTimeKey))
		{
			startTimeKey = "startTime";
		}
		if (string.IsNullOrEmpty(endTimeKey))
		{
			endTimeKey = "endTime";
		}
		serverDailyBattleDataPair.starTime = NetUtil.AnalyzeDateTimeJson(jdata, startTimeKey);
		serverDailyBattleDataPair.endTime = NetUtil.AnalyzeDateTimeJson(jdata, endTimeKey);
		serverDailyBattleDataPair.myBattleData = NetUtil.AnalyzeDailyBattleDataJson(jdata, myKey);
		serverDailyBattleDataPair.rivalBattleData = NetUtil.AnalyzeDailyBattleDataJson(jdata, rivalKey);
		return serverDailyBattleDataPair;
	}

	public static ServerDailyBattleData AnalyzeDailyBattleDataJson(JsonData jdata, string key)
	{
		ServerDailyBattleData serverDailyBattleData = new ServerDailyBattleData();
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, key);
		if (jsonObject != null)
		{
			serverDailyBattleData.maxScore = NetUtil.GetJsonLong(jsonObject, "maxScore");
			serverDailyBattleData.league = NetUtil.GetJsonInt(jsonObject, "league");
			serverDailyBattleData.userId = NetUtil.GetJsonString(jsonObject, "userId");
			serverDailyBattleData.name = NetUtil.GetJsonString(jsonObject, "name");
			serverDailyBattleData.loginTime = NetUtil.GetJsonLong(jsonObject, "loginTime");
			serverDailyBattleData.mainChaoId = NetUtil.GetJsonInt(jsonObject, "mainChaoId");
			serverDailyBattleData.mainChaoLevel = NetUtil.GetJsonInt(jsonObject, "mainChaoLevel");
			serverDailyBattleData.subChaoId = NetUtil.GetJsonInt(jsonObject, "subChaoId");
			serverDailyBattleData.subChaoLevel = NetUtil.GetJsonInt(jsonObject, "subChaoLevel");
			serverDailyBattleData.numRank = NetUtil.GetJsonInt(jsonObject, "numRank");
			serverDailyBattleData.charaId = NetUtil.GetJsonInt(jsonObject, "charaId");
			serverDailyBattleData.charaLevel = NetUtil.GetJsonInt(jsonObject, "charaLevel");
			serverDailyBattleData.subCharaId = NetUtil.GetJsonInt(jsonObject, "subCharaId");
			serverDailyBattleData.subCharaLevel = NetUtil.GetJsonInt(jsonObject, "subCharaLevel");
			serverDailyBattleData.goOnWin = NetUtil.GetJsonInt(jsonObject, "goOnWin");
			serverDailyBattleData.isSentEnergy = NetUtil.GetJsonFlag(jsonObject, "energyFlg");
			serverDailyBattleData.language = (Env.Language)NetUtil.GetJsonInt(jsonObject, "language");
		}
		return serverDailyBattleData;
	}

	public static ServerDailyBattleStatus AnalyzeDailyBattleStatusJson(JsonData jdata, string key)
	{
		ServerDailyBattleStatus serverDailyBattleStatus = new ServerDailyBattleStatus();
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, key);
		if (jsonObject != null)
		{
			serverDailyBattleStatus.numWin = NetUtil.GetJsonInt(jsonObject, "numWin");
			serverDailyBattleStatus.numLose = NetUtil.GetJsonInt(jsonObject, "numLose");
			serverDailyBattleStatus.numDraw = NetUtil.GetJsonInt(jsonObject, "numDraw");
			serverDailyBattleStatus.numLoseByDefault = NetUtil.GetJsonInt(jsonObject, "numLoseByDefault");
			serverDailyBattleStatus.goOnWin = NetUtil.GetJsonInt(jsonObject, "goOnWin");
			serverDailyBattleStatus.goOnLose = NetUtil.GetJsonInt(jsonObject, "goOnLose");
		}
		return serverDailyBattleStatus;
	}

	public static List<ServerDailyBattlePrizeData> AnalyzeDailyBattlePrizeDataJson(JsonData jdata, string key)
	{
		List<ServerDailyBattlePrizeData> list = new List<ServerDailyBattlePrizeData>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, key);
		if (jsonArray == null)
		{
			return list;
		}
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jsonData = jsonArray[i];
			if (jsonData != null)
			{
				ServerDailyBattlePrizeData serverDailyBattlePrizeData = new ServerDailyBattlePrizeData();
				serverDailyBattlePrizeData.operatorData = NetUtil.GetJsonInt(jsonData, "operator");
				serverDailyBattlePrizeData.number = NetUtil.GetJsonInt(jsonData, "number");
				JsonData jsonArray2 = NetUtil.GetJsonArray(jsonData, "presentList");
				int count2 = jsonArray2.Count;
				for (int j = 0; j < count2; j++)
				{
					JsonData jsonData2 = jsonArray2[j];
					if (jsonData2 != null)
					{
						ServerItemState itemState = NetUtil.AnalyzeItemStateJson(jsonData2, string.Empty);
						serverDailyBattlePrizeData.AddItemState(itemState);
					}
				}
				list.Add(serverDailyBattlePrizeData);
			}
		}
		return list;
	}

	public static DateTime AnalyzeDateTimeJson(JsonData jdata, string key)
	{
		long jsonLong = NetUtil.GetJsonLong(jdata, key);
		return NetUtil.GetLocalDateTime(jsonLong);
	}

	public static ServerPlayerState AnalyzePlayerStateJson(JsonData jdata, string key)
	{
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, key);
		if (jsonObject == null)
		{
			return null;
		}
		ServerPlayerState serverPlayerState = new ServerPlayerState();
		NetUtil.AnalyzePlayerState_Scores(jsonObject, ref serverPlayerState);
		NetUtil.AnalyzePlayerState_Rings(jsonObject, ref serverPlayerState);
		NetUtil.AnalyzePlayerState_Enegies(jsonObject, ref serverPlayerState);
		NetUtil.AnalyzePlayerState_Messages(jsonObject, ref serverPlayerState);
		NetUtil.AnalyzePlayerState_DailyChallenge(jsonObject, ref serverPlayerState);
		ServerCharacterState[] array = NetUtil.AnalyzePlayerState_CharactersStates(jsonObject);
		ServerCharacterState[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ServerCharacterState serverCharacterState = array2[i];
			if (serverCharacterState != null)
			{
				serverPlayerState.SetCharacterState(serverCharacterState);
			}
		}
		List<ServerChaoState> list = NetUtil.AnalyzePlayerState_ChaoStates(jsonObject);
		foreach (ServerChaoState current in list)
		{
			if (current != null)
			{
				serverPlayerState.ChaoStates.Add(current);
			}
		}
		NetUtil.AnalyzePlayerState_ItemsStates(jsonObject, ref serverPlayerState);
		NetUtil.AnalyzePlayerState_EquipItemList(jsonObject, ref serverPlayerState);
		NetUtil.AnalyzePlayerState_Other(jsonObject, ref serverPlayerState);
		return serverPlayerState;
	}

	private static void AnalyzePlayerState_Scores(JsonData jdata, ref ServerPlayerState playerState)
	{
		playerState.m_totalHighScore = NetUtil.GetJsonLong(jdata, "totalHighScore");
		playerState.m_totalHighScoreQuick = NetUtil.GetJsonLong(jdata, "quickTotalHighScore");
		playerState.m_totalDistance = NetUtil.GetJsonLong(jdata, "totalDistance");
		playerState.m_maxDistance = NetUtil.GetJsonLong(jdata, "maximumDistance");
		playerState.m_leagueIndex = NetUtil.GetJsonInt(jdata, "rankingLeague");
		playerState.m_leagueIndexQuick = NetUtil.GetJsonInt(jdata, "quickRankingLeague");
	}

	private static void AnalyzePlayerState_Rings(JsonData jdata, ref ServerPlayerState playerState)
	{
		playerState.m_numFreeRings = NetUtil.GetJsonInt(jdata, "numRings");
		playerState.m_numBuyRings = NetUtil.GetJsonInt(jdata, "numBuyRings");
		playerState.m_numRings = playerState.m_numFreeRings + playerState.m_numBuyRings;
		playerState.m_numFreeRedRings = NetUtil.GetJsonInt(jdata, "numRedRings");
		playerState.m_numBuyRedRings = NetUtil.GetJsonInt(jdata, "numBuyRedRings");
		playerState.m_numRedRings = playerState.m_numFreeRedRings + playerState.m_numBuyRedRings;
	}

	private static void AnalyzePlayerState_Enegies(JsonData jdata, ref ServerPlayerState playerState)
	{
		playerState.m_numFreeEnergy = NetUtil.GetJsonInt(jdata, "energy");
		playerState.m_numBuyEnergy = NetUtil.GetJsonInt(jdata, "energyBuy");
		playerState.m_numEnergy = playerState.m_numFreeEnergy + playerState.m_numBuyEnergy;
		long jsonLong = NetUtil.GetJsonLong(jdata, "energyRenewsAt");
		playerState.m_energyRenewsAt = NetUtil.GetLocalDateTime(jsonLong);
	}

	private static void AnalyzePlayerState_Messages(JsonData jdata, ref ServerPlayerState playerState)
	{
		playerState.m_numUnreadMessages = NetUtil.GetJsonInt(jdata, "mumMessages");
	}

	private static void AnalyzePlayerState_DailyChallenge(JsonData jdata, ref ServerPlayerState playerState)
	{
		playerState.m_dailyMissionId = NetUtil.GetJsonInt(jdata, "dailyMissionId");
		playerState.m_dailyMissionValue = NetUtil.GetJsonInt(jdata, "dailyChallengeValue");
		playerState.m_dailyChallengeComplete = NetUtil.GetJsonFlag(jdata, "dailyChallengeComplete");
		long jsonLong = NetUtil.GetJsonLong(jdata, "dailyMissionEndTime");
		playerState.m_endDailyMissionDate = NetUtil.GetLocalDateTime(jsonLong);
		playerState.m_numDailyChalCont = NetUtil.GetJsonInt(jdata, "numDailyChalCont");
	}

	public static ServerCharacterState[] AnalyzePlayerState_CharactersStates(JsonData jdata, string arrayName)
	{
		ServerCharacterState[] array = new ServerCharacterState[29];
		for (int i = 0; i < 29; i++)
		{
			array[i] = new ServerCharacterState();
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, arrayName);
		if (jsonArray == null)
		{
			return array;
		}
		int count = jsonArray.Count;
		for (int j = 0; j < count; j++)
		{
			JsonData jsonData = jsonArray[j];
			int jsonInt = NetUtil.GetJsonInt(jsonData, "characterId");
			ServerItem serverItem = new ServerItem((ServerItem.Id)jsonInt);
			CharaType charaType = serverItem.charaType;
			ServerCharacterState serverCharacterState = array[(int)charaType];
			if (serverCharacterState != null)
			{
				serverCharacterState.Id = jsonInt;
				serverCharacterState.Status = (ServerCharacterState.CharacterStatus)NetUtil.GetJsonInt(jsonData, "status");
				serverCharacterState.OldStatus = serverCharacterState.Status;
				serverCharacterState.Level = NetUtil.GetJsonInt(jsonData, "level");
				serverCharacterState.Cost = NetUtil.GetJsonInt(jsonData, "numRings");
				serverCharacterState.OldCost = serverCharacterState.Cost;
				serverCharacterState.NumRedRings = NetUtil.GetJsonInt(jsonData, "numRedRings");
				JsonData jsonArray2 = NetUtil.GetJsonArray(jsonData, "abilityLevel");
				int count2 = jsonArray2.Count;
				serverCharacterState.AbilityLevel.Clear();
				for (int k = 0; k < count2; k++)
				{
					serverCharacterState.AbilityLevel.Add(NetUtil.GetJsonInt(jsonArray2[k]));
					serverCharacterState.OldAbiltyLevel.Add(NetUtil.GetJsonInt(jsonArray2[k]));
				}
				serverCharacterState.Condition = (ServerCharacterState.LockCondition)NetUtil.GetJsonInt(jsonData, "lockCondition");
				serverCharacterState.star = NetUtil.GetJsonInt(jsonData, "star");
				serverCharacterState.starMax = NetUtil.GetJsonInt(jsonData, "starMax");
				serverCharacterState.priceNumRings = NetUtil.GetJsonInt(jsonData, "priceNumRings");
				serverCharacterState.priceNumRedRings = NetUtil.GetJsonInt(jsonData, "priceNumRedRings");
				serverCharacterState.Exp = NetUtil.GetJsonInt(jsonData, "exp");
				serverCharacterState.OldExp = serverCharacterState.Exp;
				if (NetUtil.IsExist(jsonData, "campaignList"))
				{
					JsonData jsonArray3 = NetUtil.GetJsonArray(jsonData, "campaignList");
					if (jsonArray3 != null)
					{
						ServerCampaignData serverCampaignData = new ServerCampaignData();
						serverCampaignData.campaignType = Constants.Campaign.emType.CharacterUpgradeCost;
						serverCampaignData.id = serverCharacterState.Id;
						ServerInterface.CampaignState.RemoveCampaign(serverCampaignData);
						int count3 = jsonArray3.Count;
						for (int l = 0; l < count3; l++)
						{
							ServerCampaignData serverCampaignData2 = NetUtil.AnalyzeCampaignDataJson(jsonArray3[l], string.Empty);
							if (serverCampaignData2 != null)
							{
								serverCampaignData2.id = serverCharacterState.Id;
								ServerInterface.CampaignState.RegistCampaign(serverCampaignData2);
							}
						}
					}
				}
			}
		}
		return array;
	}

	public static ServerPlayCharacterState[] AnalyzePlayerState_PlayCharactersStates(JsonData jdata)
	{
		ServerCharacterState[] array = NetUtil.AnalyzePlayerState_CharactersStates(jdata, "playCharacterState");
		List<ServerPlayCharacterState> list = new List<ServerPlayCharacterState>();
		ServerCharacterState[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ServerCharacterState serverCharacterState = array2[i];
			if (serverCharacterState != null)
			{
				ServerPlayCharacterState serverPlayCharacterState = ServerPlayCharacterState.CreatePlayCharacterState(serverCharacterState);
				if (serverPlayCharacterState != null)
				{
					list.Add(serverPlayCharacterState);
				}
			}
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "playCharacterState");
		if (jsonArray == null)
		{
			return list.ToArray();
		}
		int count = jsonArray.Count;
		for (int j = 0; j < count; j++)
		{
			JsonData jsonData = jsonArray[j];
			if (jsonData != null)
			{
				if (j >= list.Count)
				{
					break;
				}
				int jsonInt = NetUtil.GetJsonInt(jsonArray[j], "characterId");
				ServerItem serverItem = new ServerItem((ServerItem.Id)jsonInt);
				CharaType charaType = serverItem.charaType;
				ServerPlayCharacterState serverPlayCharacterState2 = list[(int)charaType];
				if (serverPlayCharacterState2 != null)
				{
					serverPlayCharacterState2.star = NetUtil.GetJsonInt(jsonData, "star");
					serverPlayCharacterState2.starMax = NetUtil.GetJsonInt(jsonData, "starMax");
					serverPlayCharacterState2.priceNumRings = NetUtil.GetJsonInt(jsonData, "priceNumRings");
					serverPlayCharacterState2.priceNumRedRings = NetUtil.GetJsonInt(jsonData, "priceNumRedRings");
					JsonData jsonArray2 = NetUtil.GetJsonArray(jsonData, "abilityLevelup");
					int count2 = jsonArray2.Count;
					serverPlayCharacterState2.abilityLevelUp.Clear();
					for (int k = 0; k < count2; k++)
					{
						ServerItem serverItem2 = new ServerItem((ServerItem.Id)NetUtil.GetJsonInt(jsonArray2[k]));
						serverPlayCharacterState2.abilityLevelUp.Add((int)serverItem2.abilityType);
					}
					if (NetUtil.IsExist(jsonData, "abilityLevelupExp"))
					{
						JsonData jsonArray3 = NetUtil.GetJsonArray(jsonData, "abilityLevelupExp");
						int count3 = jsonArray3.Count;
						serverPlayCharacterState2.abilityLevelUpExp.Clear();
						for (int l = 0; l < count3; l++)
						{
							int jsonInt2 = NetUtil.GetJsonInt(jsonArray3[l]);
							serverPlayCharacterState2.abilityLevelUpExp.Add(jsonInt2);
						}
					}
				}
			}
		}
		return list.ToArray();
	}

	public static ServerCharacterState[] AnalyzePlayerState_CharactersStates(JsonData jdata)
	{
		return NetUtil.AnalyzePlayerState_CharactersStates(jdata, "characterState");
	}

	public static List<ServerChaoState> AnalyzePlayerState_ChaoStates(JsonData jdata)
	{
		List<ServerChaoState> list = new List<ServerChaoState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "chaoState");
		if (jsonArray == null)
		{
			jsonArray = NetUtil.GetJsonArray(jdata, "chaoStatus");
		}
		if (jsonArray == null)
		{
			return list;
		}
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			int jsonInt = NetUtil.GetJsonInt(jdata2, "chaoId");
			list.Add(new ServerChaoState
			{
				Id = jsonInt,
				Status = (ServerChaoState.ChaoStatus)NetUtil.GetJsonInt(jdata2, "status"),
				Level = NetUtil.GetJsonInt(jdata2, "level"),
				Dealing = (ServerChaoState.ChaoDealing)NetUtil.GetJsonInt(jdata2, "setStatus"),
				Rarity = NetUtil.GetJsonInt(jdata2, "rarity"),
				NumInvite = NetUtil.GetJsonInt(jdata2, "numInvite"),
				NumAcquired = NetUtil.GetJsonInt(jdata2, "acquired"),
				Hidden = NetUtil.GetJsonFlag(jdata2, "hidden")
			});
		}
		return list;
	}

	private static void AnalyzePlayerState_ItemsStates(JsonData jdata, ref ServerPlayerState playerState)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "items");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerItemState itemState = NetUtil.AnalyzeItemStateJson(jdata2, string.Empty);
			playerState.AddItemState(itemState);
		}
	}

	private static void AnalyzePlayerState_EquipItemList(JsonData jdata, ref ServerPlayerState playerState)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "equipItemList");
		for (int i = 0; i < playerState.m_equipItemList.Length; i++)
		{
			if (i < jsonArray.Count)
			{
				int jsonInt = NetUtil.GetJsonInt(jsonArray[i]);
				playerState.m_equipItemList[i] = jsonInt;
			}
			else
			{
				playerState.m_equipItemList[i] = -1;
			}
		}
	}

	private static void AnalyzePlayerState_Other(JsonData jdata, ref ServerPlayerState playerState)
	{
		playerState.m_numContinuesUsed = NetUtil.GetJsonInt(jdata, "numContinuesUsed");
		playerState.m_mainCharaId = NetUtil.GetJsonInt(jdata, "mainCharaID");
		playerState.m_subCharaId = NetUtil.GetJsonInt(jdata, "subCharaID");
		playerState.m_useSubCharacter = NetUtil.GetJsonFlag(jdata, "useSubCharacter");
		playerState.m_mainChaoId = NetUtil.GetJsonInt(jdata, "mainChaoID");
		playerState.m_subChaoId = NetUtil.GetJsonInt(jdata, "subChaoID");
		playerState.m_numPlaying = NetUtil.GetJsonInt(jdata, "numPlaying");
		playerState.m_numAnimals = NetUtil.GetJsonInt(jdata, "numAnimals");
		playerState.m_numRank = NetUtil.GetJsonInt(jdata, "numRank");
	}

	public static List<ServerWheelSpinInfo> AnalyzeWheelSpinInfo(JsonData jdata, string key)
	{
		List<ServerWheelSpinInfo> list = new List<ServerWheelSpinInfo>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, key);
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			list.Add(new ServerWheelSpinInfo
			{
				id = NetUtil.GetJsonInt(jdata2, "id"),
				start = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jdata2, "start")),
				end = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jdata2, "end")),
				param = NetUtil.GetJsonString(jdata2, "param")
			});
		}
		return list;
	}

	public static ServerWheelOptions AnalyzeWheelOptionsJson(JsonData jdata, string key)
	{
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, key);
		if (jsonObject == null)
		{
			return null;
		}
		ServerWheelOptions result = new ServerWheelOptions(null);
		NetUtil.AnalyzeWheelOptions_Items(jsonObject, ref result);
		NetUtil.AnalyzeWheelOptions_Other(jsonObject, ref result);
		return result;
	}

	private static void AnalyzeWheelOptions_Items(JsonData jdata, ref ServerWheelOptions wheelOptions)
	{
		wheelOptions.m_itemWon = NetUtil.GetJsonInt(jdata, "itemWon");
		wheelOptions.ResetItemList();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "itemList");
		if (jsonArray != null)
		{
			int count = jsonArray.Count;
			for (int i = 0; i < count; i++)
			{
				JsonData jdata2 = jsonArray[i];
				ServerItemState serverItemState = NetUtil.AnalyzeItemStateJson(jdata2, string.Empty);
				if (serverItemState != null)
				{
					wheelOptions.AddItemList(serverItemState);
				}
			}
		}
		JsonData jsonArray2 = NetUtil.GetJsonArray(jdata, "items");
		int count2 = jsonArray2.Count;
		for (int j = 0; j < count2; j++)
		{
			int jsonInt = NetUtil.GetJsonInt(jsonArray2[j]);
			wheelOptions.m_items[j] = jsonInt;
		}
		JsonData jsonArray3 = NetUtil.GetJsonArray(jdata, "item");
		int count3 = jsonArray3.Count;
		for (int k = 0; k < count3; k++)
		{
			int jsonInt2 = NetUtil.GetJsonInt(jsonArray3[k]);
			wheelOptions.m_itemQuantities[k] = jsonInt2;
		}
		JsonData jsonArray4 = NetUtil.GetJsonArray(jdata, "itemWeight");
		int count4 = jsonArray4.Count;
		for (int l = 0; l < count4; l++)
		{
			int jsonInt3 = NetUtil.GetJsonInt(jsonArray4[l]);
			wheelOptions.m_itemWeight[l] = jsonInt3;
		}
		if (wheelOptions.NumRequiredSpEggs > 0)
		{
			RouletteManager.Instance.specialEgg = RouletteManager.Instance.specialEgg + wheelOptions.NumRequiredSpEggs;
		}
	}

	private static void AnalyzeWheelOptions_Other(JsonData jdata, ref ServerWheelOptions wheelOptions)
	{
		long jsonLong = NetUtil.GetJsonLong(jdata, "nextFreeSpin");
		wheelOptions.m_nextFreeSpin = NetUtil.GetLocalDateTime(jsonLong);
		wheelOptions.m_spinCost = NetUtil.GetJsonInt(jdata, "spinCost");
		wheelOptions.m_rouletteRank = (RouletteUtility.WheelRank)NetUtil.GetJsonInt(jdata, "rouletteRank");
		wheelOptions.m_numRouletteToken = NetUtil.GetJsonInt(jdata, "numRouletteToken");
		wheelOptions.m_numJackpotRing = NetUtil.GetJsonInt(jdata, "numJackpotRing");
		wheelOptions.m_numRemaining = NetUtil.GetJsonInt(jdata, "numRemainingRoulette");
		if (NetUtil.IsExist(jdata, "campaignList"))
		{
			JsonData jsonArray = NetUtil.GetJsonArray(jdata, "campaignList");
			if (jsonArray != null)
			{
				int count = jsonArray.Count;
				for (int i = 0; i < count; i++)
				{
					ServerCampaignData serverCampaignData = NetUtil.AnalyzeCampaignDataJson(jsonArray[i], string.Empty);
					if (serverCampaignData != null)
					{
						ServerInterface.CampaignState.RegistCampaign(serverCampaignData);
					}
				}
			}
		}
		GeneralUtil.SetItemCount(ServerItem.Id.ROULLETE_TICKET_ITEM, (long)wheelOptions.m_numRouletteToken);
	}

	public static ServerWheelOptionsGeneral AnalyzeWheelOptionsGeneralJson(JsonData jdata, string key)
	{
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, key);
		if (jsonObject == null)
		{
			return null;
		}
		ServerWheelOptionsGeneral result = new ServerWheelOptionsGeneral();
		NetUtil.AnalyzeChaoWheelOptionsGeneral_Items(jsonObject, ref result);
		NetUtil.AnalyzeChaoWheelOptionsGeneral_Other(jsonObject, ref result);
		return result;
	}

	private static void AnalyzeChaoWheelOptionsGeneral_Items(JsonData jdata, ref ServerWheelOptionsGeneral chaoWheelOptions)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "items");
		JsonData jsonArray2 = NetUtil.GetJsonArray(jdata, "item");
		JsonData jsonArray3 = NetUtil.GetJsonArray(jdata, "itemWeight");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			int jsonInt = NetUtil.GetJsonInt(jsonArray[i]);
			int num = 1;
			int weight = 1;
			if (jsonArray2 != null && jsonArray2.Count > i)
			{
				num = NetUtil.GetJsonInt(jsonArray2[i]);
			}
			if (jsonArray3 != null && jsonArray3.Count > i)
			{
				weight = NetUtil.GetJsonInt(jsonArray3[i]);
			}
			chaoWheelOptions.SetupItem(i, jsonInt, weight, num);
		}
	}

	private static void AnalyzeChaoWheelOptionsGeneral_Other(JsonData jdata, ref ServerWheelOptionsGeneral wheelOptionsGeneral)
	{
		int jsonInt = NetUtil.GetJsonInt(jdata, "spinID");
		int jsonInt2 = NetUtil.GetJsonInt(jdata, "rouletteRank");
		int jsonInt3 = NetUtil.GetJsonInt(jdata, "numRemainingRoulette");
		int jsonInt4 = NetUtil.GetJsonInt(jdata, "numJackpotRing");
		int jsonInt5 = NetUtil.GetJsonInt(jdata, "numSpecialEgg");
		long jsonLong = NetUtil.GetJsonLong(jdata, "nextFreeSpin");
		if (RouletteManager.Instance != null)
		{
			RouletteManager.Instance.specialEgg = jsonInt5;
		}
		if (NetUtil.IsExist(jdata, "campaignList"))
		{
			JsonData jsonArray = NetUtil.GetJsonArray(jdata, "campaignList");
			if (jsonArray != null)
			{
				int count = jsonArray.Count;
				for (int i = 0; i < count; i++)
				{
					ServerCampaignData serverCampaignData = NetUtil.AnalyzeCampaignDataJson(jsonArray[i], string.Empty);
					if (serverCampaignData != null)
					{
						serverCampaignData.id = serverCampaignData.iSubContent;
						ServerInterface.CampaignState.RegistCampaign(serverCampaignData);
					}
				}
			}
		}
		wheelOptionsGeneral.SetupParam(jsonInt, jsonInt3, jsonInt4, jsonInt2, jsonInt5, NetUtil.GetLocalDateTime(jsonLong));
		wheelOptionsGeneral.ResetupCostItem();
		JsonData jsonArray2 = NetUtil.GetJsonArray(jdata, "costItemList");
		int count2 = jsonArray2.Count;
		for (int j = 0; j < count2; j++)
		{
			JsonData jdata2 = jsonArray2[j];
			int jsonInt6 = NetUtil.GetJsonInt(jdata2, "itemId");
			int jsonInt7 = NetUtil.GetJsonInt(jdata2, "numItem");
			int jsonInt8 = NetUtil.GetJsonInt(jdata2, "itemStock");
			if (jsonInt6 > 0)
			{
				wheelOptionsGeneral.AddCostItem(jsonInt6, jsonInt8, jsonInt7);
			}
		}
	}

	public static ServerChaoWheelOptions AnalyzeChaoWheelOptionsJson(JsonData jdata, string key)
	{
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, key);
		if (jsonObject == null)
		{
			return null;
		}
		ServerChaoWheelOptions result = new ServerChaoWheelOptions();
		NetUtil.AnalyzeChaoWheelOptions_Items(jsonObject, ref result);
		NetUtil.AnalyzeChaoWheelOptions_Other(jsonObject, ref result);
		if (NetUtil.IsExist(jsonObject, "campaignList"))
		{
			JsonData jsonArray = NetUtil.GetJsonArray(jsonObject, "campaignList");
			if (jsonArray != null)
			{
				int count = jsonArray.Count;
				for (int i = 0; i < count; i++)
				{
					ServerCampaignData serverCampaignData = NetUtil.AnalyzeCampaignDataJson(jsonArray[i], string.Empty);
					if (serverCampaignData != null)
					{
						serverCampaignData.id = serverCampaignData.iSubContent;
						ServerInterface.CampaignState.RegistCampaign(serverCampaignData);
					}
				}
			}
		}
		return result;
	}

	private static void AnalyzeChaoWheelOptions_Items(JsonData jdata, ref ServerChaoWheelOptions chaoWheelOptions)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "rarity");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			chaoWheelOptions.Rarities[i] = NetUtil.GetJsonInt(jsonArray[i]);
		}
		JsonData jsonArray2 = NetUtil.GetJsonArray(jdata, "itemWeight");
		int count2 = jsonArray2.Count;
		for (int j = 0; j < count2; j++)
		{
			chaoWheelOptions.ItemWeight[j] = NetUtil.GetJsonInt(jsonArray2[j]);
		}
	}

	private static void AnalyzeChaoWheelOptions_Other(JsonData jdata, ref ServerChaoWheelOptions chaoWheelOptions)
	{
		chaoWheelOptions.Cost = NetUtil.GetJsonInt(jdata, "spinCost");
		chaoWheelOptions.SpinType = (ServerChaoWheelOptions.ChaoSpinType)NetUtil.GetJsonInt(jdata, "chaoRouletteType");
		chaoWheelOptions.NumSpecialEggs = NetUtil.GetJsonInt(jdata, "numSpecialEgg");
		chaoWheelOptions.IsValid = (NetUtil.GetJsonInt(jdata, "rouletteAvailable") != 0);
		chaoWheelOptions.NumRouletteToken = NetUtil.GetJsonInt(jdata, "numChaoRouletteToken");
		chaoWheelOptions.IsTutorial = (NetUtil.GetJsonInt(jdata, "numChaoRoulette") == 0);
		if (RouletteManager.Instance != null)
		{
			RouletteManager.Instance.specialEgg = chaoWheelOptions.NumSpecialEggs;
		}
		GeneralUtil.SetItemCount(ServerItem.Id.ROULLETE_TICKET_PREMIAM, (long)chaoWheelOptions.NumRouletteToken);
	}

	public static ServerSpinResultGeneral AnalyzeSpinResultJson(JsonData jdata, string key)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, key);
		ServerSpinResultGeneral serverSpinResultGeneral = null;
		if (jsonArray != null)
		{
			int count = jsonArray.Count;
			if (count > 0)
			{
				int itemWon = 0;
				serverSpinResultGeneral = new ServerSpinResultGeneral();
				for (int i = 0; i < count; i++)
				{
					JsonData jsonData = jsonArray[i];
					if (jsonData != null)
					{
						ServerChaoData serverChaoData = NetUtil.AnalyzeChaoDataJson(jsonData, "getChao");
						if (serverChaoData != null)
						{
							serverSpinResultGeneral.AddChaoState(serverChaoData);
						}
						else
						{
							ServerChaoData serverChaoData2 = NetUtil.AnalyzeItemDataJson(jsonData, "getItem");
							if (serverChaoData2 != null)
							{
								serverSpinResultGeneral.AddChaoState(serverChaoData2);
							}
						}
						JsonData jsonArray2 = NetUtil.GetJsonArray(jsonData, "itemList");
						int count2 = jsonArray2.Count;
						for (int j = 0; j < count2; j++)
						{
							JsonData jdata2 = jsonArray2[j];
							ServerItemState serverItemState = NetUtil.AnalyzeItemStateJson(jdata2, string.Empty);
							if (serverItemState != null)
							{
								serverSpinResultGeneral.AddItemState(serverItemState);
							}
						}
						itemWon = NetUtil.GetJsonInt(jsonData, "itemWon");
					}
				}
				if (count > 1)
				{
					serverSpinResultGeneral.ItemWon = -1;
				}
				else
				{
					serverSpinResultGeneral.ItemWon = itemWon;
				}
			}
		}
		return serverSpinResultGeneral;
	}

	public static ServerSpinResultGeneral AnalyzeSpinResultGeneralJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		ServerSpinResultGeneral serverSpinResultGeneral = new ServerSpinResultGeneral();
		JsonData jsonArray = NetUtil.GetJsonArray(jsonData, "getChao");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerChaoData serverChaoData = NetUtil.AnalyzeChaoDataJson(jdata2, string.Empty);
			if (serverChaoData != null)
			{
				serverSpinResultGeneral.AddChaoState(serverChaoData);
			}
		}
		JsonData jsonArray2 = NetUtil.GetJsonArray(jsonData, "itemList");
		int count2 = jsonArray2.Count;
		for (int j = 0; j < count2; j++)
		{
			JsonData jdata3 = jsonArray2[j];
			ServerItemState serverItemState = NetUtil.AnalyzeItemStateJson(jdata3, string.Empty);
			if (serverItemState != null)
			{
				serverSpinResultGeneral.AddItemState(serverItemState);
			}
		}
		serverSpinResultGeneral.ItemWon = NetUtil.GetJsonInt(jsonData, "itemWon");
		return serverSpinResultGeneral;
	}

	public static ServerChaoSpinResult AnalyzeChaoSpinResultJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		ServerChaoSpinResult serverChaoSpinResult = new ServerChaoSpinResult();
		serverChaoSpinResult.AcquiredChaoData = NetUtil.AnalyzeChaoDataJson(jsonData, "getChao");
		serverChaoSpinResult.NumRequiredSpEggs = 0;
		JsonData jsonArray = NetUtil.GetJsonArray(jsonData, "itemList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerItemState serverItemState = NetUtil.AnalyzeItemStateJson(jdata2, string.Empty);
			if (serverItemState != null)
			{
				serverChaoSpinResult.AddItemState(serverItemState);
				if (serverItemState.m_itemId == 220000)
				{
					serverChaoSpinResult.NumRequiredSpEggs += serverItemState.m_num;
				}
			}
		}
		serverChaoSpinResult.ItemWon = NetUtil.GetJsonInt(jsonData, "itemWon");
		return serverChaoSpinResult;
	}

	public static ServerChaoData AnalyzeChaoDataJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerChaoData
		{
			Id = NetUtil.GetJsonInt(jsonData, "chaoId"),
			Level = NetUtil.GetJsonInt(jsonData, "level"),
			Rarity = NetUtil.GetJsonInt(jsonData, "rarity")
		};
	}

	public static ServerChaoData AnalyzeItemDataJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerChaoData
		{
			Id = NetUtil.GetJsonInt(jsonData, "itemId"),
			Level = NetUtil.GetJsonInt(jsonData, "level_after"),
			Rarity = NetUtil.GetJsonInt(jsonData, "rarity")
		};
	}

	public static ServerChaoRentalState AnalyzeChaoRentalStateJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerChaoRentalState
		{
			FriendId = NetUtil.GetJsonString(jsonData, "friendId"),
			Name = NetUtil.GetJsonString(jsonData, "name"),
			Url = NetUtil.GetJsonString(jsonData, "url"),
			ChaoData = NetUtil.AnalyzeChaoDataJson(jsonData, "chaoData"),
			RentalState = NetUtil.GetJsonInt(jsonData, "rentalFlg"),
			NextRentalAt = (long)NetUtil.GetJsonInt(jsonData, "nextRentalAt")
		};
	}

	public static ServerPrizeState AnalyzePrizeChaoWheelSpin(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "prizeList");
		if (jsonArray == null)
		{
			return null;
		}
		ServerPrizeState serverPrizeState = new ServerPrizeState();
		for (int i = 0; i < jsonArray.Count; i++)
		{
			serverPrizeState.AddPrize(new ServerPrizeData
			{
				itemId = NetUtil.GetJsonInt(jsonArray[i], "chao_id"),
				num = 1,
				weight = 1
			});
		}
		return serverPrizeState;
	}

	public static ServerPrizeState AnalyzePrizeWheelSpinGeneral(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "raidbossPrizeList");
		if (jsonArray == null)
		{
			return null;
		}
		ServerPrizeState serverPrizeState = new ServerPrizeState();
		for (int i = 0; i < jsonArray.Count; i++)
		{
			serverPrizeState.AddPrize(new ServerPrizeData
			{
				itemId = NetUtil.GetJsonInt(jsonArray[i], "itemId"),
				num = NetUtil.GetJsonInt(jsonArray[i], "numItem"),
				weight = NetUtil.GetJsonInt(jsonArray[i], "itemRate"),
				spinId = NetUtil.GetJsonInt(jsonArray[i], "spinId")
			});
		}
		return serverPrizeState;
	}

	public static ServerLeaderboardEntry AnalyzeLeaderboardEntryJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerLeaderboardEntry
		{
			m_hspId = NetUtil.GetJsonString(jsonData, "friendId"),
			m_score = NetUtil.GetJsonLong(jsonData, "rankingScore"),
			m_hiScore = NetUtil.GetJsonLong(jsonData, "maxScore"),
			m_userData = NetUtil.GetJsonInt(jsonData, "userData"),
			m_name = NetUtil.GetJsonString(jsonData, "name"),
			m_url = NetUtil.GetJsonString(jsonData, "url"),
			m_energyFlg = NetUtil.GetJsonFlag(jsonData, "energyFlg"),
			m_grade = NetUtil.GetJsonInt(jsonData, "grade"),
			m_gradeChanged = NetUtil.GetJsonInt(jsonData, "rankChanged"),
			m_expireTime = NetUtil.GetJsonLong(jsonData, "expireTime"),
			m_numRank = NetUtil.GetJsonInt(jsonData, "numRank"),
			m_loginTime = (long)NetUtil.GetJsonInt(jsonData, "loginTime"),
			m_charaId = NetUtil.GetJsonInt(jsonData, "charaId"),
			m_charaLevel = NetUtil.GetJsonInt(jsonData, "charaLevel"),
			m_subCharaId = NetUtil.GetJsonInt(jsonData, "subCharaId"),
			m_subCharaLevel = NetUtil.GetJsonInt(jsonData, "subCharaLevel"),
			m_mainChaoId = NetUtil.GetJsonInt(jsonData, "mainChaoId"),
			m_mainChaoLevel = NetUtil.GetJsonInt(jsonData, "mainChaoLevel"),
			m_subChaoId = NetUtil.GetJsonInt(jsonData, "subChaoId"),
			m_subChaoLevel = NetUtil.GetJsonInt(jsonData, "subChaoLevel"),
			m_leagueIndex = NetUtil.GetJsonInt(jsonData, "league"),
			m_language = (Env.Language)NetUtil.GetJsonInt(jsonData, "language")
		};
	}

	public static ServerMileageFriendEntry AnalyzeMileageFriendEntryJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerMileageFriendEntry
		{
			m_friendId = NetUtil.GetJsonString(jsonData, "friendId"),
			m_name = NetUtil.GetJsonString(jsonData, "name"),
			m_url = NetUtil.GetJsonString(jsonData, "url"),
			m_mapState = NetUtil.AnalyzeMileageMapStateJson(jsonData, "mapState")
		};
	}

	public static ServerMileageMapState AnalyzeMileageMapStateJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		ServerMileageMapState serverMileageMapState = new ServerMileageMapState();
		serverMileageMapState.m_episode = NetUtil.GetJsonInt(jsonData, "episode");
		serverMileageMapState.m_chapter = NetUtil.GetJsonInt(jsonData, "chapter");
		serverMileageMapState.m_point = NetUtil.GetJsonInt(jsonData, "point");
		serverMileageMapState.m_numBossAttack = NetUtil.GetJsonInt(jsonData, "numBossAttack");
		serverMileageMapState.m_stageTotalScore = NetUtil.GetJsonLong(jsonData, "stageTotalScore");
		serverMileageMapState.m_stageMaxScore = NetUtil.GetJsonLong(jsonData, "stageMaxScore");
		long unixTime = (long)NetUtil.GetJsonInt(jsonData, "chapterStartTime");
		serverMileageMapState.m_chapterStartTime = NetUtil.GetLocalDateTime(unixTime);
		return serverMileageMapState;
	}

	public static PresentItem AnalyzePresentItemJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new PresentItem
		{
			m_itemId = NetUtil.GetJsonInt(jsonData, "itemId"),
			m_numItem = NetUtil.GetJsonInt(jsonData, "numItem"),
			m_additionalInfo1 = NetUtil.GetJsonInt(jsonData, "additionalInfo1"),
			m_additionalInfo1 = NetUtil.GetJsonInt(jsonData, "additionalInfo2")
		};
	}

	public static ServerMessageEntry AnalyzeMessageEntryJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerMessageEntry
		{
			m_messageId = NetUtil.GetJsonInt(jsonData, "messageId"),
			m_messageType = (ServerMessageEntry.MessageType)NetUtil.GetJsonInt(jsonData, "messageType"),
			m_fromId = NetUtil.GetJsonString(jsonData, "friendId"),
			m_name = NetUtil.GetJsonString(jsonData, "name"),
			m_url = NetUtil.GetJsonString(jsonData, "url"),
			m_presentState = NetUtil.AnalyzePresentStateJson(jsonData, "item"),
			m_expireTiem = NetUtil.GetJsonInt(jsonData, "expireTime")
		};
	}

	public static ServerOperatorMessageEntry AnalyzeOperatorMessageEntryJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerOperatorMessageEntry
		{
			m_messageId = NetUtil.GetJsonInt(jsonData, "messageId"),
			m_content = NetUtil.GetJsonString(jsonData, "contents"),
			m_presentState = NetUtil.AnalyzePresentStateJson(jsonData, "item"),
			m_expireTiem = NetUtil.GetJsonInt(jsonData, "expireTime")
		};
	}

	public static ServerItemState AnalyzeItemStateJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerItemState
		{
			m_itemId = NetUtil.GetJsonInt(jsonData, "itemId"),
			m_num = NetUtil.GetJsonInt(jsonData, "numItem")
		};
	}

	public static ServerPresentState AnalyzePresentStateJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerPresentState
		{
			m_itemId = NetUtil.GetJsonInt(jsonData, "itemId"),
			m_numItem = NetUtil.GetJsonInt(jsonData, "numItem"),
			m_additionalInfo1 = NetUtil.GetJsonInt(jsonData, "additionalInfo1"),
			m_additionalInfo2 = NetUtil.GetJsonInt(jsonData, "additionalInfo2")
		};
	}

	public static ServerRedStarItemState AnalyzeRedStarItemStateJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		ServerRedStarItemState serverRedStarItemState = new ServerRedStarItemState();
		serverRedStarItemState.m_storeItemId = NetUtil.GetJsonInt(jsonData, "storeItemId");
		serverRedStarItemState.m_itemId = NetUtil.GetJsonInt(jsonData, "itemId");
		serverRedStarItemState.m_numItem = NetUtil.GetJsonInt(jsonData, "numItem");
		serverRedStarItemState.m_price = NetUtil.GetJsonInt(jsonData, "price");
		serverRedStarItemState.m_priceDisp = NetUtil.GetJsonString(jsonData, "priceDisp");
		serverRedStarItemState.m_productId = NetUtil.GetJsonString(jsonData, "productId");
		if (serverRedStarItemState.m_itemId == 900000)
		{
			ServerCampaignData serverCampaignData = new ServerCampaignData();
			serverCampaignData.id = serverRedStarItemState.m_storeItemId;
			serverCampaignData.campaignType = Constants.Campaign.emType.PurchaseAddRsrings;
			ServerInterface.CampaignState.RemoveCampaign(serverCampaignData);
			serverCampaignData.campaignType = Constants.Campaign.emType.PurchaseAddRsringsNoChargeUser;
			ServerInterface.CampaignState.RemoveCampaign(serverCampaignData);
		}
		if (NetUtil.IsExist(jsonData, "campaign"))
		{
			ServerCampaignData serverCampaignData2 = NetUtil.AnalyzeCampaignDataJson(jsonData, "campaign");
			if (serverCampaignData2 != null)
			{
				serverCampaignData2.id = serverRedStarItemState.m_storeItemId;
				ServerInterface.CampaignState.RegistCampaign(serverCampaignData2);
			}
		}
		return serverRedStarItemState;
	}

	public static ServerRingItemState AnalyzeRingItemStateJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		ServerRingItemState serverRingItemState = new ServerRingItemState();
		serverRingItemState.m_itemId = NetUtil.GetJsonInt(jsonData, "item_id");
		serverRingItemState.m_cost = NetUtil.GetJsonInt(jsonData, "price");
		if (NetUtil.IsExist(jsonData, "campaign"))
		{
			ServerCampaignData serverCampaignData = NetUtil.AnalyzeCampaignDataJson(jsonData, "campaign");
			if (serverCampaignData != null)
			{
				serverCampaignData.id = serverRingItemState.m_itemId;
				ServerInterface.CampaignState.RegistCampaign(serverCampaignData);
			}
		}
		return serverRingItemState;
	}

	public static ServerMileageIncentive AnalyzeMileageIncentiveJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		ServerMileageIncentive serverMileageIncentive = new ServerMileageIncentive();
		serverMileageIncentive.m_type = (ServerMileageIncentive.Type)NetUtil.GetJsonInt(jsonData, "type");
		serverMileageIncentive.m_itemId = NetUtil.GetJsonInt(jsonData, "itemId");
		serverMileageIncentive.m_num = NetUtil.GetJsonInt(jsonData, "numItem");
		serverMileageIncentive.m_pointId = NetUtil.GetJsonInt(jsonData, "pointId");
		if (jsonData.ContainsKey("friendId"))
		{
			serverMileageIncentive.m_friendId = NetUtil.GetJsonString(jsonData, "friendId");
		}
		return serverMileageIncentive;
	}

	public static ServerMileageEvent AnalyzeMileageEventJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerMileageEvent
		{
			Distance = NetUtil.GetJsonInt(jsonData, "distance"),
			EventType = (ServerMileageEvent.emEventType)NetUtil.GetJsonInt(jsonData, "eventType"),
			Content = NetUtil.GetJsonInt(jsonData, "content"),
			NumType = (ServerConstants.NumType)NetUtil.GetJsonInt(jsonData, "numType"),
			Num = NetUtil.GetJsonInt(jsonData, "numContent"),
			Level = NetUtil.GetJsonInt(jsonData, "level")
		};
	}

	public static List<ServerMileageFriendEntry> AnalyzeMileageFriendListJson(JsonData jdata, string key)
	{
		List<ServerMileageFriendEntry> list = new List<ServerMileageFriendEntry>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, key);
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			ServerMileageFriendEntry item = NetUtil.AnalyzeMileageFriendEntryJson(jsonArray[i], string.Empty);
			list.Add(item);
		}
		return list;
	}

	public static ServerDailyChallengeIncentive AnalyzeDailyMissionIncentiveJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerDailyChallengeIncentive
		{
			m_itemId = NetUtil.GetJsonInt(jsonData, "itemId"),
			m_num = NetUtil.GetJsonInt(jsonData, "numItem"),
			m_numIncentiveCont = NetUtil.GetJsonInt(jsonData, "numIncentiveCont")
		};
	}

	public static ServerCampaignData AnalyzeCampaignDataJson(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		return new ServerCampaignData
		{
			campaignType = (Constants.Campaign.emType)NetUtil.GetJsonInt(jsonData, "campaignType"),
			iContent = NetUtil.GetJsonInt(jsonData, "campaignContent"),
			iSubContent = NetUtil.GetJsonInt(jsonData, "campaignSubContent"),
			beginDate = NetUtil.GetJsonLong(jsonData, "campaignStartTime"),
			endDate = NetUtil.GetJsonLong(jsonData, "campaignEndTime")
		};
	}

	public static List<ServerRingExchangeList> AnalyzeRingExchangeList(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "itemList"))
		{
			return null;
		}
		List<ServerRingExchangeList> list = new List<ServerRingExchangeList>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "itemList");
		for (int i = 0; i < jsonArray.Count; i++)
		{
			ServerRingExchangeList serverRingExchangeList = new ServerRingExchangeList();
			serverRingExchangeList.m_ringItemId = NetUtil.GetJsonInt(jsonArray[i], "ringItemId");
			serverRingExchangeList.m_itemId = NetUtil.GetJsonInt(jsonArray[i], "itemId");
			serverRingExchangeList.m_itemNum = NetUtil.GetJsonInt(jsonArray[i], "numItem");
			serverRingExchangeList.m_price = NetUtil.GetJsonInt(jsonArray[i], "price");
			if (NetUtil.IsExist(jsonArray[i], "campaign"))
			{
				ServerCampaignData serverCampaignData = NetUtil.AnalyzeCampaignDataJson(jsonArray[i], "campaign");
				if (serverCampaignData != null)
				{
					serverCampaignData.id = serverRingExchangeList.m_itemId;
					ServerInterface.CampaignState.RegistCampaign(serverCampaignData);
				}
			}
			list.Add(serverRingExchangeList);
		}
		return list;
	}

	public static int AnalyzeRingExchangeListTotalItems(JsonData jdata)
	{
		if (NetUtil.GetJsonObject(jdata, "totalItems") == null)
		{
			return 0;
		}
		return NetUtil.GetJsonInt(jdata, "totalItems");
	}

	public static ServerLeagueData AnalyzeLeagueData(JsonData jdata, string key)
	{
		JsonData jsonData = jdata;
		if (key != null && string.Empty != key)
		{
			jsonData = NetUtil.GetJsonObject(jsonData, key);
		}
		if (jsonData == null)
		{
			return null;
		}
		ServerLeagueData serverLeagueData = new ServerLeagueData();
		serverLeagueData.mode = NetUtil.GetJsonInt(jdata, "mode");
		serverLeagueData.leagueId = NetUtil.GetJsonInt(jsonData, "leagueId");
		serverLeagueData.groupId = NetUtil.GetJsonInt(jsonData, "groupId");
		serverLeagueData.numGroupMember = NetUtil.GetJsonInt(jsonData, "numGroupMember");
		serverLeagueData.numLeagueMember = NetUtil.GetJsonInt(jsonData, "numLeagueMember");
		serverLeagueData.numUp = NetUtil.GetJsonInt(jsonData, "numUp");
		serverLeagueData.numDown = NetUtil.GetJsonInt(jsonData, "numDown");
		JsonData jsonArray = NetUtil.GetJsonArray(jsonData, "highScoreOpe");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerRemainOperator remainOperator = NetUtil.AnalyzeRemainOperator(jdata2);
			serverLeagueData.AddHighScoreRemainOperator(remainOperator);
		}
		JsonData jsonArray2 = NetUtil.GetJsonArray(jsonData, "totalScoreOpe");
		int count2 = jsonArray2.Count;
		for (int j = 0; j < count2; j++)
		{
			JsonData jdata3 = jsonArray2[j];
			ServerRemainOperator remainOperator2 = NetUtil.AnalyzeRemainOperator(jdata3);
			serverLeagueData.AddTotalScoreRemainOperator(remainOperator2);
		}
		return serverLeagueData;
	}

	public static ServerWeeklyLeaderboardOptions AnalyzeWeeklyLeaderboardOptions(JsonData jdata)
	{
		return new ServerWeeklyLeaderboardOptions
		{
			mode = NetUtil.GetJsonInt(jdata, "mode"),
			type = NetUtil.GetJsonInt(jdata, "type"),
			param = NetUtil.GetJsonInt(jdata, "param"),
			startTime = NetUtil.GetJsonInt(jdata, "startTime"),
			endTime = NetUtil.GetJsonInt(jdata, "endTime")
		};
	}

	public static List<ServerLeagueOperatorData> AnalyzeLeagueDatas(JsonData jdata, string key)
	{
		List<ServerLeagueOperatorData> list = new List<ServerLeagueOperatorData>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, key);
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerLeagueOperatorData serverLeagueOperatorData = new ServerLeagueOperatorData();
			serverLeagueOperatorData.leagueId = NetUtil.GetJsonInt(jdata2, "leagueId");
			serverLeagueOperatorData.numUp = NetUtil.GetJsonInt(jdata2, "numUp");
			serverLeagueOperatorData.numDown = NetUtil.GetJsonInt(jdata2, "numDown");
			JsonData jsonArray2 = NetUtil.GetJsonArray(jdata2, "highScoreOpe");
			int count2 = jsonArray2.Count;
			for (int j = 0; j < count2; j++)
			{
				JsonData jdata3 = jsonArray2[j];
				ServerRemainOperator remainOperator = NetUtil.AnalyzeRemainOperator(jdata3);
				serverLeagueOperatorData.AddHighScoreRemainOperator(remainOperator);
			}
			JsonData jsonArray3 = NetUtil.GetJsonArray(jdata2, "totalScoreOpe");
			int count3 = jsonArray3.Count;
			for (int k = 0; k < count3; k++)
			{
				JsonData jdata4 = jsonArray3[k];
				ServerRemainOperator remainOperator2 = NetUtil.AnalyzeRemainOperator(jdata4);
				serverLeagueOperatorData.AddTotalScoreRemainOperator(remainOperator2);
			}
			list.Add(serverLeagueOperatorData);
		}
		return list;
	}

	private static ServerRemainOperator AnalyzeRemainOperator(JsonData jdata)
	{
		ServerRemainOperator serverRemainOperator = new ServerRemainOperator();
		serverRemainOperator.operatorData = NetUtil.GetJsonInt(jdata, "operator");
		serverRemainOperator.number = NetUtil.GetJsonInt(jdata, "number");
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "presentList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerItemState itemState = NetUtil.AnalyzeItemStateJson(jdata2, string.Empty);
			serverRemainOperator.AddItemState(itemState);
		}
		return serverRemainOperator;
	}

	public static void GetResponse_CampaignList(JsonData jdata)
	{
		if (NetUtil.IsExist(jdata, "campaignList"))
		{
			JsonData jsonArray = NetUtil.GetJsonArray(jdata, "campaignList");
			if (jsonArray != null)
			{
				int count = jsonArray.Count;
				for (int i = 0; i < count; i++)
				{
					ServerCampaignData serverCampaignData = NetUtil.AnalyzeCampaignDataJson(jsonArray[i], string.Empty);
					if (serverCampaignData != null)
					{
						ServerInterface.CampaignState.RegistCampaign(serverCampaignData);
					}
				}
			}
		}
	}

	public static ServerFreeItemState AnalyzeFreeItemList(JsonData jdata)
	{
		ServerFreeItemState serverFreeItemState = new ServerFreeItemState();
		if (NetUtil.IsExist(jdata, "freeItemList"))
		{
			JsonData jsonArray = NetUtil.GetJsonArray(jdata, "freeItemList");
			if (jsonArray != null)
			{
				int count = jsonArray.Count;
				for (int i = 0; i < count; i++)
				{
					serverFreeItemState.AddItem(new ServerItemState
					{
						m_itemId = NetUtil.GetJsonInt(jsonArray[i], "itemId"),
						m_num = NetUtil.GetJsonInt(jsonArray[i], "numItem")
					});
				}
			}
		}
		return serverFreeItemState;
	}

	public static List<ServerConsumedCostData> AnalyzeConsumedCostDataList(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "consumedCostList"))
		{
			return null;
		}
		List<ServerConsumedCostData> list = new List<ServerConsumedCostData>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "consumedCostList");
		for (int i = 0; i < jsonArray.Count; i++)
		{
			list.Add(new ServerConsumedCostData
			{
				consumedItemId = NetUtil.GetJsonInt(jsonArray[i], "consumedItemId"),
				itemId = NetUtil.GetJsonInt(jsonArray[i], "itemId"),
				numItem = NetUtil.GetJsonInt(jsonArray[i], "numItem")
			});
		}
		return list;
	}

	public static List<ServerEventEntry> AnalyzeEventList(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventList"))
		{
			return null;
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "eventList");
		if (jsonArray == null)
		{
			return null;
		}
		List<ServerEventEntry> list = new List<ServerEventEntry>();
		for (int i = 0; i < jsonArray.Count; i++)
		{
			list.Add(new ServerEventEntry
			{
				EventId = NetUtil.GetJsonInt(jsonArray[i], "eventId"),
				EventType = NetUtil.GetJsonInt(jsonArray[i], "eventType"),
				EventStartTime = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jsonArray[i], "eventStartTime")),
				EventEndTime = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jsonArray[i], "eventEndTime")),
				EventCloseTime = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jsonArray[i], "eventCloseTime"))
			});
		}
		return list;
	}

	public static ServerEventState AnalyzeEventState(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventState"))
		{
			return null;
		}
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, "eventState");
		if (jsonObject == null)
		{
			return null;
		}
		return new ServerEventState
		{
			Param = NetUtil.GetJsonLong(jsonObject, "param"),
			RewardId = NetUtil.GetJsonInt(jsonObject, "rewardId")
		};
	}

	public static List<ServerEventReward> AnalyzeEventReward(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventRewardList"))
		{
			return null;
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "eventRewardList");
		if (jsonArray == null)
		{
			return null;
		}
		List<ServerEventReward> list = new List<ServerEventReward>();
		for (int i = 0; i < jsonArray.Count; i++)
		{
			list.Add(new ServerEventReward
			{
				RewardId = NetUtil.GetJsonInt(jsonArray[i], "rewardId"),
				Param = NetUtil.GetJsonLong(jsonArray[i], "param"),
				m_itemId = NetUtil.GetJsonInt(jsonArray[i], "itemId"),
				m_num = NetUtil.GetJsonInt(jsonArray[i], "numItem")
			});
		}
		return list;
	}

	public static ServerEventRaidBossState AnalyzeRaidBossState(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventRaidboss"))
		{
			return null;
		}
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, "eventRaidboss");
		if (jsonObject == null)
		{
			return null;
		}
		return new ServerEventRaidBossState
		{
			Id = NetUtil.GetJsonLong(jsonObject, "raidbossId"),
			Level = NetUtil.GetJsonInt(jsonObject, "raidbossLevel"),
			Rarity = NetUtil.GetJsonInt(jsonObject, "raidbossRarity"),
			HitPoint = NetUtil.GetJsonInt(jsonObject, "raidbossHitPoint"),
			MaxHitPoint = NetUtil.GetJsonInt(jsonObject, "raidbossMaxHitPoint"),
			Status = NetUtil.GetJsonInt(jsonObject, "raidbossStatus"),
			EscapeAt = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jsonObject, "raidbossEscapeAt")),
			EncounterName = NetUtil.GetJsonString(jsonObject, "encounterName"),
			Encounter = NetUtil.GetJsonBoolean(jsonObject, "encounterFlg"),
			Crowded = NetUtil.GetJsonBoolean(jsonObject, "crowdedFlg"),
			Participation = (NetUtil.GetJsonInt(jsonObject, "participateCount") != 0)
		};
	}

	public static List<ServerEventRaidBossState> AnalyzeRaidBossStateList(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventUserRaidbossList"))
		{
			return null;
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "eventUserRaidbossList");
		if (jsonArray == null)
		{
			return null;
		}
		List<ServerEventRaidBossState> list = new List<ServerEventRaidBossState>();
		for (int i = 0; i < jsonArray.Count; i++)
		{
			list.Add(new ServerEventRaidBossState
			{
				Id = NetUtil.GetJsonLong(jsonArray[i], "raidbossId"),
				Level = NetUtil.GetJsonInt(jsonArray[i], "raidbossLevel"),
				Rarity = NetUtil.GetJsonInt(jsonArray[i], "raidbossRarity"),
				HitPoint = NetUtil.GetJsonInt(jsonArray[i], "raidbossHitPoint"),
				MaxHitPoint = NetUtil.GetJsonInt(jsonArray[i], "raidbossMaxHitPoint"),
				Status = NetUtil.GetJsonInt(jsonArray[i], "raidbossStatus"),
				EscapeAt = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jsonArray[i], "raidbossEscapeAt")),
				EncounterName = NetUtil.GetJsonString(jsonArray[i], "encounterName"),
				Encounter = NetUtil.GetJsonBoolean(jsonArray[i], "encounterFlg"),
				Crowded = NetUtil.GetJsonBoolean(jsonArray[i], "crowdedFlg"),
				Participation = (NetUtil.GetJsonInt(jsonArray[i], "participateCount") != 0)
			});
		}
		return list;
	}

	public static ServerEventUserRaidBossState AnalyzeEventUserRaidBossState(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventUserRaidboss"))
		{
			return null;
		}
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, "eventUserRaidboss");
		if (jsonObject == null)
		{
			return null;
		}
		ServerEventUserRaidBossState serverEventUserRaidBossState = new ServerEventUserRaidBossState();
		int raidBossEnergy = 0;
		int raidbossEnergyBuy = 0;
		serverEventUserRaidBossState.NumRaidbossRings = NetUtil.GetJsonInt(jsonObject, "numRaidbossRings");
		raidBossEnergy = NetUtil.GetJsonInt(jsonObject, "raidbossEnergy");
		raidbossEnergyBuy = NetUtil.GetJsonInt(jsonObject, "raidbossEnergyBuy");
		serverEventUserRaidBossState.NumBeatedEncounter = NetUtil.GetJsonInt(jsonObject, "numBeatedEncounter");
		serverEventUserRaidBossState.NumBeatedEnterprise = NetUtil.GetJsonInt(jsonObject, "numBeatedEnterprise");
		serverEventUserRaidBossState.NumRaidBossEncountered = NetUtil.GetJsonInt(jsonObject, "numTotalEncountered");
		DateTime localDateTime = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jsonObject, "raidbossEnergyRenewsAt"));
		RaidEnergyStorage.CheckEnergy(ref raidBossEnergy, ref raidbossEnergyBuy, ref localDateTime);
		serverEventUserRaidBossState.RaidBossEnergy = raidBossEnergy;
		serverEventUserRaidBossState.RaidbossEnergyBuy = raidbossEnergyBuy;
		serverEventUserRaidBossState.EnergyRenewsAt = localDateTime;
		return serverEventUserRaidBossState;
	}

	public static List<ServerEventRaidBossUserState> AnalyzeEventRaidBossUserStateList(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventRaidbossUserList"))
		{
			return null;
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "eventRaidbossUserList");
		if (jsonArray == null)
		{
			return null;
		}
		List<ServerEventRaidBossUserState> list = new List<ServerEventRaidBossUserState>();
		for (int i = 0; i < jsonArray.Count; i++)
		{
			list.Add(new ServerEventRaidBossUserState
			{
				WrestleId = NetUtil.GetJsonString(jsonArray[i], "wrestleId"),
				UserName = NetUtil.GetJsonString(jsonArray[i], "name"),
				Grade = NetUtil.GetJsonInt(jsonArray[i], "grade"),
				NumRank = NetUtil.GetJsonInt(jsonArray[i], "numRank"),
				LoginTime = NetUtil.GetJsonInt(jsonArray[i], "loginTime"),
				CharaId = NetUtil.GetJsonInt(jsonArray[i], "charaId"),
				CharaLevel = NetUtil.GetJsonInt(jsonArray[i], "charaLevel"),
				SubCharaId = NetUtil.GetJsonInt(jsonArray[i], "subCharaId"),
				SubCharaLevel = NetUtil.GetJsonInt(jsonArray[i], "subCharaLevel"),
				MainChaoId = NetUtil.GetJsonInt(jsonArray[i], "mainChaoId"),
				MainChaoLevel = NetUtil.GetJsonInt(jsonArray[i], "mainChaoLevel"),
				SubChaoId = NetUtil.GetJsonInt(jsonArray[i], "subChaoId"),
				SubChaoLevel = NetUtil.GetJsonInt(jsonArray[i], "subChaoLevel"),
				Language = NetUtil.GetJsonInt(jsonArray[i], "language"),
				League = NetUtil.GetJsonInt(jsonArray[i], "league"),
				WrestleCount = NetUtil.GetJsonInt(jsonArray[i], "wrestleCount"),
				WrestleDamage = NetUtil.GetJsonInt(jsonArray[i], "wrestleDamage"),
				WrestleBeatFlg = NetUtil.GetJsonBoolean(jsonArray[i], "wrestleBeatFlg")
			});
		}
		return list;
	}

	public static List<ServerEventRaidBossDesiredState> AnalyzeEventRaidbossDesiredList(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventRaidbossDesiredList"))
		{
			return null;
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "eventRaidbossDesiredList");
		if (jsonArray == null)
		{
			return null;
		}
		List<ServerEventRaidBossDesiredState> list = new List<ServerEventRaidBossDesiredState>();
		for (int i = 0; i < jsonArray.Count; i++)
		{
			list.Add(new ServerEventRaidBossDesiredState
			{
				DesireId = NetUtil.GetJsonString(jsonArray[i], "desireId"),
				UserName = NetUtil.GetJsonString(jsonArray[i], "name"),
				NumRank = NetUtil.GetJsonInt(jsonArray[i], "numRank"),
				LoginTime = NetUtil.GetJsonInt(jsonArray[i], "loginTime"),
				CharaId = NetUtil.GetJsonInt(jsonArray[i], "charaId"),
				CharaLevel = NetUtil.GetJsonInt(jsonArray[i], "charaLevel"),
				SubCharaId = NetUtil.GetJsonInt(jsonArray[i], "subCharaId"),
				SubCharaLevel = NetUtil.GetJsonInt(jsonArray[i], "subCharaLevel"),
				MainChaoId = NetUtil.GetJsonInt(jsonArray[i], "mainChaoId"),
				MainChaoLevel = NetUtil.GetJsonInt(jsonArray[i], "mainChaoLevel"),
				SubChaoId = NetUtil.GetJsonInt(jsonArray[i], "subChaoId"),
				SubChaoLevel = NetUtil.GetJsonInt(jsonArray[i], "subChaoLevel"),
				Language = NetUtil.GetJsonInt(jsonArray[i], "language"),
				League = NetUtil.GetJsonInt(jsonArray[i], "league"),
				NumBeatedEnterprise = NetUtil.GetJsonInt(jsonArray[i], "numBeatedEnterprise")
			});
		}
		return list;
	}

	public static ServerEventRaidBossBonus AnalyzeEventRaidBossBonus(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "eventRaidbossBonus"))
		{
			return null;
		}
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, "eventRaidbossBonus");
		if (jsonObject == null)
		{
			return null;
		}
		return new ServerEventRaidBossBonus
		{
			EncounterBonus = NetUtil.GetJsonInt(jsonObject, "eventRaidbossEncounterBonus"),
			WrestleBonus = NetUtil.GetJsonInt(jsonObject, "eventRaidbossWrestleBonus"),
			DamageRateBonus = NetUtil.GetJsonInt(jsonObject, "eventRaidbossDamageRateBonus"),
			DamageTopBonus = NetUtil.GetJsonInt(jsonObject, "eventRaidbossDamageTopBonus"),
			BeatBonus = NetUtil.GetJsonInt(jsonObject, "eventRaidbossBeatBonus")
		};
	}

	public static List<ServerUserTransformData> AnalyzeUserTransformData(JsonData jdata)
	{
		if (!NetUtil.IsExist(jdata, "transformDataList"))
		{
			return null;
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "transformDataList");
		if (jsonArray == null)
		{
			return null;
		}
		List<ServerUserTransformData> list = new List<ServerUserTransformData>();
		for (int i = 0; i < jsonArray.Count; i++)
		{
			list.Add(new ServerUserTransformData
			{
				m_userId = NetUtil.GetJsonString(jsonArray[i], "userId"),
				m_facebookId = NetUtil.GetJsonString(jsonArray[i], "facebookId")
			});
		}
		return list;
	}

	public static DateTime GetLocalDateTime(long unixTime)
	{
		return NetUtil.UNIX_EPOCH.AddSeconds((double)unixTime).ToLocalTime();
	}

	public static long GetUnixTime(DateTime dateTime)
	{
		dateTime = dateTime.ToUniversalTime();
		return (long)(dateTime - NetUtil.UNIX_EPOCH).TotalSeconds;
	}

	public static int GetCurrentUnixTime()
	{
		DateTime utcNow = DateTime.UtcNow;
		return (int)(utcNow - NetUtil.UNIX_EPOCH).TotalSeconds;
	}

	public static DateTime GetCurrentTime()
	{
		return NetBase.GetCurrentTime();
	}

	public static bool IsServerTimeWithinPeriod(long start, long end)
	{
		return NetUtil.IsWithinPeriod(NetBase.LastNetServerTime, start, end);
	}

	public static bool IsWithinPeriod(long current, long start, long end)
	{
		if (start == 0L && end == 0L)
		{
			return true;
		}
		if (start == 0L)
		{
			if (current <= end)
			{
				return true;
			}
		}
		else if (end == 0L)
		{
			if (start <= current)
			{
				return true;
			}
		}
		else if (start <= current && current <= end)
		{
			return true;
		}
		return false;
	}
}
