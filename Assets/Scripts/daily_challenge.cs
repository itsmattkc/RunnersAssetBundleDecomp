using AnimationOrTween;
using DataTable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class daily_challenge : MonoBehaviour
{
	[Serializable]
	private class InspectorUi
	{
		[SerializeField]
		public GameObject m_clearGameObject;

		[SerializeField]
		public Animation m_clearAnimation;

		[SerializeField]
		public GameObject m_dayObjectOrg;

		[SerializeField]
		public GameObject m_dayBigObjectOrg;

		[SerializeField]
		public GameObject m_dayObjectBase;

		[SerializeField]
		public List<Color> m_dayObjectColors;
	}

	public class DailyMissionInfo
	{
		private int _DayIndex_k__BackingField;

		private int _TodayMissionId_k__BackingField;

		private long _TodayMissionClearQuota_k__BackingField;

		private int[] _InceniveIdTable_k__BackingField;

		private int[] _InceniveNumTable_k__BackingField;

		private int _ClearMissionCount_k__BackingField;

		private bool _IsClearTodayMission_k__BackingField;

		private long _TodayMissionQuota_k__BackingField;

		private string _TodayMissionText_k__BackingField;

		private int _DayMax_k__BackingField;

		private long _TodayMissionClearQuotaBefore_k__BackingField;

		public int DayIndex
		{
			get;
			set;
		}

		public int TodayMissionId
		{
			get;
			set;
		}

		public long TodayMissionClearQuota
		{
			get;
			set;
		}

		public int[] InceniveIdTable
		{
			get;
			set;
		}

		public int[] InceniveNumTable
		{
			get;
			set;
		}

		public int ClearMissionCount
		{
			get;
			set;
		}

		public bool IsClearTodayMission
		{
			get;
			set;
		}

		public long TodayMissionQuota
		{
			get;
			set;
		}

		public string TodayMissionText
		{
			get;
			set;
		}

		public int DayMax
		{
			get;
			set;
		}

		public long TodayMissionClearQuotaBefore
		{
			get;
			set;
		}

		public bool IsMissionClearNotice
		{
			get
			{
				return this.TodayMissionClearQuotaBefore < this.TodayMissionClearQuota && this.TodayMissionClearQuota == this.TodayMissionQuota;
			}
		}

		public bool IsMissionEvent
		{
			get
			{
				return !this.IsClearTodayMission || this.IsMissionClearNotice;
			}
		}
	}

	private const float BAR_SPEED = 0.004f;

	private static bool s_isUpdateEffect;

	[SerializeField]
	private daily_challenge.InspectorUi m_inspectorUi;

	private bool m_isInitialized;

	private bool m_setUp;

	private float m_updateBarDelay;

	private float m_clearBarValue;

	private SoundManager.PlayId m_barSePlayId;

	private List<DayObject> m_days;

	private UIPlayAnimation m_windowAnime;

	private UIPlayAnimation m_windowBtnAnime;

	private UIPlayTween m_windowBtnTween;

	private daily_challenge.DailyMissionInfo m_info;

	public static bool isUpdateEffect
	{
		get
		{
			return daily_challenge.s_isUpdateEffect;
		}
	}

	public bool IsEndSetup
	{
		get
		{
			return this.m_setUp;
		}
	}

	public static daily_challenge.DailyMissionInfo GetInfoFromSaveData(long todayMissionClearQuotaBefore)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return null;
		}
		int id = instance.PlayerData.DailyMission.id;
		MissionData missionData = MissionTable.GetMissionData(id);
		if (missionData == null)
		{
			return null;
		}
		daily_challenge.DailyMissionInfo dailyMissionInfo = new daily_challenge.DailyMissionInfo
		{
			DayIndex = instance.PlayerData.DailyMission.date,
			DayMax = instance.PlayerData.DailyMission.max,
			TodayMissionId = instance.PlayerData.DailyMission.id,
			TodayMissionClearQuota = instance.PlayerData.DailyMission.progress,
			InceniveIdTable = new int[instance.PlayerData.DailyMission.reward_max],
			InceniveNumTable = new int[instance.PlayerData.DailyMission.reward_max],
			ClearMissionCount = instance.PlayerData.DailyMission.clear_count,
			IsClearTodayMission = instance.PlayerData.DailyMission.missions_complete,
			TodayMissionQuota = (long)missionData.quota,
			TodayMissionText = missionData.text
		};
		int reward_max = instance.PlayerData.DailyMission.reward_max;
		for (int i = 0; i < reward_max; i++)
		{
			if (i < instance.PlayerData.DailyMission.reward_id.Length)
			{
				dailyMissionInfo.InceniveIdTable[i] = instance.PlayerData.DailyMission.reward_id[i];
			}
			if (i < instance.PlayerData.DailyMission.reward_count.Length)
			{
				dailyMissionInfo.InceniveNumTable[i] = instance.PlayerData.DailyMission.reward_count[i];
			}
		}
		dailyMissionInfo.TodayMissionClearQuota = ((dailyMissionInfo.TodayMissionClearQuota >= dailyMissionInfo.TodayMissionQuota) ? dailyMissionInfo.TodayMissionQuota : dailyMissionInfo.TodayMissionClearQuota);
		dailyMissionInfo.TodayMissionClearQuotaBefore = ((todayMissionClearQuotaBefore == -1L) ? dailyMissionInfo.TodayMissionClearQuota : todayMissionClearQuotaBefore);
		if (todayMissionClearQuotaBefore != -1L && dailyMissionInfo.IsMissionClearNotice)
		{
			dailyMissionInfo.ClearMissionCount--;
		}
		return dailyMissionInfo;
	}

	public void Initialize(long todayMissionClearQuotaBefore)
	{
		this.m_info = daily_challenge.GetInfoFromSaveData(todayMissionClearQuotaBefore);
		this.m_updateBarDelay = 1f;
		this.m_barSePlayId = SoundManager.PlayId.NONE;
		GameObject gameObject = null;
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "DailyWindowUI");
		}
		if (gameObject != null)
		{
			this.m_windowAnime = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(gameObject, "blinder");
			if (this.m_windowAnime != null)
			{
				this.m_windowAnime.enabled = false;
			}
			this.m_windowBtnAnime = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(gameObject, "Btn_next");
			this.m_windowBtnTween = GameObjectUtil.FindChildGameObjectComponent<UIPlayTween>(gameObject, "Btn_next");
			if (this.m_windowBtnAnime != null)
			{
				this.m_windowBtnAnime.enabled = false;
			}
			if (this.m_windowBtnTween != null)
			{
				this.m_windowBtnTween.enabled = false;
			}
		}
		if (this.m_info != null)
		{
			this.m_clearBarValue = (float)this.m_info.TodayMissionClearQuotaBefore / (float)this.m_info.TodayMissionQuota;
			daily_challenge.DebugLog(string.Concat(new object[]
			{
				"UpdateBar ",
				this.m_info.TodayMissionClearQuotaBefore,
				"→",
				this.m_info.TodayMissionClearQuota,
				"/",
				this.m_info.TodayMissionQuota
			}));
			this.InitItem();
			this.m_isInitialized = true;
			daily_challenge.s_isUpdateEffect = true;
			this.UpdateView();
		}
		if (this.m_inspectorUi.m_clearGameObject != null)
		{
			this.m_inspectorUi.m_clearGameObject.SetActive(false);
		}
		this.m_setUp = true;
	}

	private void Update()
	{
		if (!this.m_isInitialized)
		{
			return;
		}
		this.UpdateBar(0.004f);
	}

	private void UpdateBar(float speed)
	{
		this.m_updateBarDelay = ((this.m_updateBarDelay <= Time.deltaTime) ? 0f : (this.m_updateBarDelay - Time.deltaTime));
		if (this.m_updateBarDelay > 0f)
		{
			return;
		}
		float num = (float)this.m_info.TodayMissionClearQuota / (float)this.m_info.TodayMissionQuota;
		if (this.m_clearBarValue < num)
		{
			if (this.m_barSePlayId == SoundManager.PlayId.NONE)
			{
				this.m_barSePlayId = SoundManager.SePlay("sys_gauge", "SE");
			}
			this.m_clearBarValue = Mathf.Min(this.m_clearBarValue + speed, num);
			if (this.m_info.IsMissionClearNotice && this.m_clearBarValue == 1f)
			{
				this.m_info.ClearMissionCount++;
				this.StopBarSe();
				if (this.m_inspectorUi.m_clearGameObject != null)
				{
					this.m_inspectorUi.m_clearGameObject.SetActive(true);
				}
				if (this.m_inspectorUi.m_clearAnimation != null)
				{
					ActiveAnimation.Play(this.m_inspectorUi.m_clearAnimation, Direction.Forward);
				}
				if (this.m_days.Count > this.m_info.ClearMissionCount - 1)
				{
					DayObject dayObject = this.m_days[this.m_info.ClearMissionCount - 1];
					dayObject.PlayGetAnimation();
				}
				SoundManager.SePlay("sys_dailychallenge", "SE");
			}
			if (this.m_clearBarValue == num)
			{
				daily_challenge.s_isUpdateEffect = false;
				this.m_info.TodayMissionClearQuotaBefore = this.m_info.TodayMissionClearQuota;
				this.StopBarSe();
			}
			else
			{
				daily_challenge.s_isUpdateEffect = true;
			}
			this.UpdateView();
		}
		else
		{
			daily_challenge.s_isUpdateEffect = false;
		}
	}

	private void UpdateView()
	{
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_days");
		if (uILabel != null)
		{
			uILabel.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "day").text, new Dictionary<string, string>
			{
				{
					"{DAY}",
					(this.m_info.DayMax - this.m_info.DayIndex).ToString()
				}
			});
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_daily_challenge");
		if (uILabel2 != null)
		{
			uILabel2.text = TextUtility.Replaces(this.m_info.TodayMissionText, new Dictionary<string, string>
			{
				{
					"{QUOTA}",
					this.m_info.TodayMissionQuota.ToString()
				}
			});
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_attainment_fg");
		if (gameObject != null)
		{
			gameObject.SetActive(this.m_clearBarValue > 0f);
		}
		UISlider uISlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(base.gameObject, "Pgb_attainment");
		if (uISlider != null)
		{
			uISlider.value = this.m_clearBarValue;
		}
		bool flag = this.m_info.IsClearTodayMission && this.m_clearBarValue == 1f;
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_percent_clear");
		if (uILabel3 != null)
		{
			if (flag)
			{
				uILabel3.text = string.Empty;
			}
			else
			{
				float num = this.m_clearBarValue * 100f;
				if (num > 100f)
				{
					num = 100f;
				}
				uILabel3.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "clear_percent").text, new Dictionary<string, string>
				{
					{
						"{PARAM}",
						((int)num).ToString()
					}
				});
			}
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "img_clear");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(flag);
		}
		for (int i = 0; i < this.m_info.InceniveIdTable.Length; i++)
		{
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "Lbl_day" + (i + 1));
			if (!(gameObject3 == null))
			{
				GameObject gameObject4 = GameObjectUtil.FindChildGameObject(gameObject3, "img_check");
				if (gameObject4 != null)
				{
					gameObject4.SetActive(i < this.m_info.ClearMissionCount);
				}
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject3, "img_daily_item");
				if (uISprite != null)
				{
					uISprite.spriteName = ((i >= this.m_info.InceniveIdTable.Length - 1) ? "ui_cmn_icon_rsring_L" : ("ui_cmn_icon_item_" + this.m_info.InceniveIdTable[i]));
				}
				UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject3.gameObject, "Lbl_count");
				if (uILabel4 != null)
				{
					uILabel4.text = this.m_info.InceniveNumTable[i].ToString();
				}
			}
		}
	}

	private void InitItem()
	{
		if (this.m_inspectorUi != null && this.m_inspectorUi.m_dayObjectBase != null && this.m_inspectorUi.m_dayObjectOrg != null && this.m_inspectorUi.m_dayBigObjectOrg != null)
		{
			GameObject dayObjectBase = this.m_inspectorUi.m_dayObjectBase;
			float num = 0f;
			float num2 = 0f;
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_inspectorUi.m_dayObjectOrg, "img_frame");
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_inspectorUi.m_dayBigObjectOrg, "img_frame");
			if (uISprite != null)
			{
				num = (float)uISprite.width;
			}
			if (uISprite2 != null)
			{
				num2 = (float)uISprite2.width;
			}
			float num3 = (float)(this.m_info.InceniveIdTable.Length - 1) * -0.5f * num - (num2 - num) * 0.5f;
			if (this.m_days != null && this.m_days.Count > 0)
			{
				for (int i = 0; i < this.m_days.Count; i++)
				{
					UnityEngine.Object.Destroy(this.m_days[i].m_clearGameObject);
				}
			}
			this.m_days = new List<DayObject>();
			for (int j = 0; j < this.m_info.InceniveIdTable.Length; j++)
			{
				Color color = new Color(1f, 1f, 1f, 1f);
				GameObject gameObject;
				if (j < this.m_info.InceniveIdTable.Length - 1)
				{
					gameObject = (UnityEngine.Object.Instantiate(this.m_inspectorUi.m_dayObjectOrg) as GameObject);
					if (this.m_inspectorUi.m_dayObjectColors != null && this.m_inspectorUi.m_dayObjectColors.Count > j)
					{
						color = this.m_inspectorUi.m_dayObjectColors[j];
					}
				}
				else
				{
					gameObject = (UnityEngine.Object.Instantiate(this.m_inspectorUi.m_dayBigObjectOrg) as GameObject);
				}
				if (gameObject != null)
				{
					gameObject.transform.parent = dayObjectBase.transform;
					float x = 0f;
					if (this.m_info.InceniveIdTable.Length > 1)
					{
						x = num3 + (float)j * num;
						if (j >= this.m_info.InceniveIdTable.Length - 1)
						{
							x = num3 + (float)j * num + (num2 - num) * 0.45f;
						}
					}
					gameObject.transform.localPosition = new Vector3(x, 0f, 0f);
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					DayObject dayObject = new DayObject(gameObject, color, j + 1);
					TweenPosition tweenPosition = GameObjectUtil.FindChildGameObjectComponent<TweenPosition>(dayObject.m_clearGameObject, "img_daily_item");
					if (tweenPosition != null)
					{
						tweenPosition.ignoreTimeScale = false;
						tweenPosition.Reset();
					}
					TweenScale tweenScale = GameObjectUtil.FindChildGameObjectComponent<TweenScale>(dayObject.m_clearGameObject, "img_daily_item");
					if (tweenScale != null)
					{
						tweenScale.ignoreTimeScale = false;
						tweenScale.Reset();
					}
					dayObject.SetItem(this.m_info.InceniveIdTable[j]);
					dayObject.count = this.m_info.InceniveNumTable[j];
					int num4 = this.m_info.InceniveIdTable.Length;
					if (num4 > 0)
					{
						if (this.m_info.ClearMissionCount < num4)
						{
							dayObject.SetAlready(j < this.m_info.ClearMissionCount);
						}
						else if (this.m_info.InceniveIdTable.Length - 1 > j)
						{
							dayObject.SetAlready(true);
						}
						else
						{
							dayObject.SetAlready(this.m_info.IsClearTodayMission);
						}
					}
					else
					{
						dayObject.SetAlready(false);
					}
					this.m_days.Add(dayObject);
				}
			}
		}
		else if (this.m_inspectorUi != null && this.m_inspectorUi.m_dayObjectBase != null)
		{
			List<GameObject> list = new List<GameObject>();
			for (int k = 1; k <= 7; k++)
			{
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "day" + k);
				if (gameObject2 != null)
				{
					list.Add(gameObject2);
				}
			}
			if (list.Count > 0)
			{
				int num5 = 0;
				foreach (GameObject current in list)
				{
					UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(current, "img_bg");
					UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(current, "img_check");
					UISprite uISprite5 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(current, "img_daily_item");
					UISprite uISprite6 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(current, "img_chara");
					UISprite uISprite7 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(current, "img_chao");
					UISprite uISprite8 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(current, "img_day_num");
					UISprite uISprite9 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(current, "img_hidden");
					if (uISprite3 != null)
					{
						uISprite3.enabled = (num5 == this.m_info.InceniveIdTable.Length - 1);
					}
					if (num5 < this.m_info.InceniveIdTable.Length)
					{
						if (uISprite4 != null)
						{
							uISprite4.enabled = (num5 < this.m_info.ClearMissionCount);
						}
						int num6 = this.m_info.InceniveIdTable[num5];
						int num7 = Mathf.FloorToInt((float)num6 / 100000f);
						if (uISprite6 != null)
						{
							uISprite6.alpha = 0f;
							uISprite6.enabled = true;
						}
						if (uISprite7 != null)
						{
							uISprite7.alpha = 0f;
							uISprite7.enabled = true;
						}
						if (uISprite5 != null)
						{
							uISprite5.alpha = 0f;
							uISprite5.enabled = true;
						}
						int num8 = num7;
						if (num8 != 3)
						{
							if (num8 != 4)
							{
								if (uISprite5 != null)
								{
									uISprite5.alpha = 1f;
									if (num6 >= 0)
									{
										uISprite5.spriteName = "ui_cmn_icon_item_" + num6;
									}
									else
									{
										uISprite5.spriteName = "ui_cmn_icon_item_9";
									}
								}
							}
							else if (uISprite7 != null)
							{
								uISprite7.alpha = 1f;
								uISprite7.spriteName = string.Format("ui_tex_chao_{0:D4}", num6 - 400000);
							}
						}
						else if (uISprite6 != null)
						{
							uISprite6.alpha = 1f;
							UISprite arg_611_0 = uISprite6;
							string arg_60C_0 = "ui_tex_player_";
							ServerItem serverItem = new ServerItem((ServerItem.Id)num6);
							arg_611_0.spriteName = arg_60C_0 + CharaTypeUtil.GetCharaSpriteNameSuffix(serverItem.charaType);
						}
						if (uISprite8 != null)
						{
							uISprite8.enabled = true;
							if (num5 == this.m_info.InceniveIdTable.Length - 1)
							{
								uISprite8.color = new Color(1f, 0.7529412f, 0f, 1f);
							}
							else
							{
								uISprite8.color = new Color(0.5019608f, 1f, 1f, 1f);
							}
						}
						if (uISprite9 != null)
						{
							uISprite9.enabled = (num5 < this.m_info.ClearMissionCount);
						}
					}
					else
					{
						if (uISprite4 != null)
						{
							uISprite4.enabled = false;
						}
						if (uISprite5 != null)
						{
							uISprite5.enabled = false;
						}
						if (uISprite8 != null)
						{
							uISprite8.enabled = false;
						}
						if (uISprite9 != null)
						{
							uISprite9.enabled = true;
						}
					}
					num5++;
				}
			}
		}
	}

	private void OnStartDailyMissionInMileageMap(long todayMissionClearQuotaBefore)
	{
		this.m_setUp = false;
		this.Initialize(todayMissionClearQuotaBefore);
	}

	private void OnStartDailyMissionInScreen()
	{
		this.Initialize(-1L);
		HudMenuUtility.SendStartInformaionDlsplay();
	}

	private void OnClickNextButton(GameObject dailyWindowGameObject)
	{
		float num = (float)this.m_info.TodayMissionClearQuota / (float)this.m_info.TodayMissionQuota;
		if (this.m_clearBarValue >= num)
		{
			if (this.m_windowBtnAnime != null)
			{
				this.m_windowBtnAnime.enabled = true;
			}
			if (this.m_windowBtnTween != null)
			{
				this.m_windowBtnTween.enabled = true;
			}
			if (this.m_windowAnime != null)
			{
				this.m_windowAnime.enabled = true;
				this.m_windowAnime.Play(false);
			}
		}
		else
		{
			this.UpdateBar(1f);
		}
		this.StopBarSe();
	}

	private void StopBarSe()
	{
		if (this.m_barSePlayId != SoundManager.PlayId.NONE)
		{
			SoundManager.SeStop(this.m_barSePlayId);
		}
		SoundManager.SeStop("sys_gauge", "SE");
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}
}
