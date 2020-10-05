using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class UIDebugMenuUpdateMileageMapDataProduction : UIDebugMenuTask
{
	private enum TextType
	{
		EPISODE,
		CHAPTER,
		EVENT_FLAG,
		LOG,
		NUM
	}

	public class DebugData
	{
		public int m_episode;

		public int m_chapter;

		public int m_interval;

		public bool m_bossFlag;

		public DebugData(int episode, int chapter, int interval, bool bossFlag)
		{
			this.m_episode = episode;
			this.m_chapter = chapter;
			this.m_interval = interval;
			this.m_bossFlag = bossFlag;
		}

		public DebugData(string data)
		{
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array.Length == 4)
			{
				this.m_episode = int.Parse(array[0]);
				this.m_chapter = int.Parse(array[1]);
				this.m_interval = int.Parse(array[2]);
				this.m_bossFlag = (array[3] == "1");
			}
		}
	}

	private const int FAR_DISTANCE = 99999999;

	[SerializeField]
	private TextAsset m_dataXml;

	private MileageDebugData m_mileageDebugData;

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuButton m_decideButton;

	private UIDebugMenuTextField[] m_TextFields = new UIDebugMenuTextField[4];

	private string[] DefaultTextList = new string[]
	{
		"Story/話",
		"Chapter/章",
		"evntFlag 0 or 1",
		"log"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(100f, 150f, 150f, 50f),
		new Rect(100f, 250f, 150f, 50f),
		new Rect(300f, 150f, 150f, 50f),
		new Rect(300f, 300f, 250f, 50f)
	};

	private string[] textFieldDefault = new string[]
	{
		"2",
		"1",
		"0",
		string.Empty
	};

	private NetDebugUpdMileageData m_updMileageData;

	private List<UIDebugMenuUpdateMileageMapDataProduction.DebugData> m_debaguDatas = new List<UIDebugMenuUpdateMileageMapDataProduction.DebugData>();

	private void DataRead()
	{
		string text = this.m_dataXml.text;
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MileageDebugData[]));
		StringReader textReader = new StringReader(text);
		MileageDebugData[] array = (MileageDebugData[])xmlSerializer.Deserialize(textReader);
		this.m_mileageDebugData = array[0];
		for (int i = 0; i < this.m_mileageDebugData.data.Length; i++)
		{
			this.m_debaguDatas.Add(new UIDebugMenuUpdateMileageMapDataProduction.DebugData(this.m_mileageDebugData.data[i]));
		}
	}

	protected override void OnStartFromTask()
	{
		this.DataRead();
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 50f, 150f, 50f), "Back", base.gameObject);
		this.m_decideButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_decideButton.Setup(new Rect(400f, 50f, 150f, 50f), "Decide", base.gameObject);
		for (int i = 0; i < 4; i++)
		{
			this.m_TextFields[i] = base.gameObject.AddComponent<UIDebugMenuTextField>();
			this.m_TextFields[i].Setup(this.RectList[i], this.DefaultTextList[i]);
			this.m_TextFields[i].text = this.textFieldDefault[i];
		}
	}

	protected override void OnTransitionTo()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(false);
		}
		if (this.m_decideButton != null)
		{
			this.m_decideButton.SetActive(false);
		}
		for (int i = 0; i < 4; i++)
		{
			if (!(this.m_TextFields[i] == null))
			{
				this.m_TextFields[i].SetActive(false);
			}
		}
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
		}
		if (this.m_decideButton != null)
		{
			this.m_decideButton.SetActive(true);
		}
		for (int i = 0; i < 4; i++)
		{
			if (!(this.m_TextFields[i] == null))
			{
				this.m_TextFields[i].SetActive(true);
			}
		}
	}

	private void OnClicked(string name)
	{
		if (name == "Back")
		{
			base.TransitionToParent();
		}
		else if (name == "Decide")
		{
			for (int i = 0; i < 4; i++)
			{
				if (i != 3)
				{
					UIDebugMenuTextField uIDebugMenuTextField = this.m_TextFields[i];
					if (!(uIDebugMenuTextField == null))
					{
						int num;
						if (!int.TryParse(uIDebugMenuTextField.text, out num))
						{
							return;
						}
					}
				}
			}
			int episode = int.Parse(this.m_TextFields[0].text);
			int chapter = int.Parse(this.m_TextFields[1].text);
			if (!this.CheckExistData(episode, chapter))
			{
				this.m_TextFields[3].text = string.Concat(new string[]
				{
					"error!  not data [",
					episode.ToString(),
					"-",
					chapter.ToString(),
					"]"
				});
				return;
			}
			this.m_TextFields[3].text = "success";
			bool flag = int.Parse(this.m_TextFields[2].text) != 0;
			bool bossFlag = this.GetBossFlag(episode, chapter);
			int interval = this.GetInterval(episode, chapter);
			int num2 = interval * 5;
			int num3 = 5;
			if (flag && !bossFlag)
			{
				num2--;
				num3--;
			}
			ServerMileageMapState oldServerState = new ServerMileageMapState
			{
				m_episode = episode,
				m_chapter = chapter,
				m_point = ((!flag) ? 0 : num3),
				m_stageTotalScore = (long)((!flag) ? 0 : num2)
			};
			ServerMileageMapState serverMileageMapState = new ServerMileageMapState();
			if (flag)
			{
				int nextDataIndex = this.GetNextDataIndex(episode, chapter);
				if (nextDataIndex != -1 && nextDataIndex < this.m_debaguDatas.Count)
				{
					serverMileageMapState.m_episode = this.m_debaguDatas[nextDataIndex].m_episode;
					serverMileageMapState.m_chapter = this.m_debaguDatas[nextDataIndex].m_chapter;
					serverMileageMapState.m_point = 0;
					serverMileageMapState.m_stageTotalScore = 0L;
					serverMileageMapState.m_numBossAttack = 0;
					serverMileageMapState.m_stageMaxScore = 0L;
				}
			}
			else if (bossFlag)
			{
				serverMileageMapState.m_episode = episode;
				serverMileageMapState.m_chapter = chapter;
				serverMileageMapState.m_point = 5;
				serverMileageMapState.m_stageTotalScore = (long)num2;
				serverMileageMapState.m_numBossAttack = 0;
				serverMileageMapState.m_stageMaxScore = 0L;
			}
			else
			{
				int nextDataIndex2 = this.GetNextDataIndex(episode, chapter);
				if (nextDataIndex2 != -1 && nextDataIndex2 < this.m_debaguDatas.Count)
				{
					serverMileageMapState.m_episode = this.m_debaguDatas[nextDataIndex2].m_episode;
					serverMileageMapState.m_chapter = this.m_debaguDatas[nextDataIndex2].m_chapter;
					serverMileageMapState.m_point = 0;
					serverMileageMapState.m_stageTotalScore = 0L;
					serverMileageMapState.m_numBossAttack = 0;
					serverMileageMapState.m_stageMaxScore = 0L;
				}
			}
			bool bossDestroy = false;
			if (flag && bossFlag)
			{
				bossDestroy = true;
			}
			this.CreateResultInfo(oldServerState, serverMileageMapState, bossDestroy);
			this.m_updMileageData = new NetDebugUpdMileageData(serverMileageMapState);
			this.m_updMileageData.Request();
		}
	}

	private void CreateResultInfo(ServerMileageMapState oldServerState, ServerMileageMapState newServerState, bool bossDestroy)
	{
		ResultInfo resultInfo = ResultInfo.CreateResultInfo();
		resultInfo.ResetData();
		ResultData info = resultInfo.GetInfo();
		info.m_validResult = true;
		info.m_bossStage = bossDestroy;
		info.m_bossDestroy = bossDestroy;
		info.m_oldMapState = new MileageMapState
		{
			m_episode = oldServerState.m_episode,
			m_chapter = oldServerState.m_chapter,
			m_point = oldServerState.m_point,
			m_score = oldServerState.m_stageTotalScore
		};
		info.m_newMapState = new MileageMapState
		{
			m_episode = newServerState.m_episode,
			m_chapter = newServerState.m_chapter,
			m_point = newServerState.m_point,
			m_score = newServerState.m_stageTotalScore
		};
		resultInfo.SetInfo(info);
	}

	private bool CheckExistData(int episode, int chapter)
	{
		for (int i = 0; i < this.m_debaguDatas.Count; i++)
		{
			if (this.m_debaguDatas[i].m_episode == episode && this.m_debaguDatas[i].m_chapter == chapter)
			{
				return true;
			}
		}
		return false;
	}

	private int GetNextDataIndex(int episode, int chapter)
	{
		int num = -1;
		for (int i = 0; i < this.m_debaguDatas.Count; i++)
		{
			if (this.m_debaguDatas[i].m_episode == episode && this.m_debaguDatas[i].m_chapter == chapter)
			{
				num = i;
				break;
			}
		}
		if (num != -1 && num + 1 < this.m_debaguDatas.Count)
		{
			num++;
		}
		return num;
	}

	private int GetInterval(int episode, int chapter)
	{
		for (int i = 0; i < this.m_debaguDatas.Count; i++)
		{
			if (this.m_debaguDatas[i].m_episode == episode && this.m_debaguDatas[i].m_chapter == chapter)
			{
				return this.m_debaguDatas[i].m_interval;
			}
		}
		return 0;
	}

	private bool GetBossFlag(int episode, int chapter)
	{
		for (int i = 0; i < this.m_debaguDatas.Count; i++)
		{
			if (this.m_debaguDatas[i].m_episode == episode && this.m_debaguDatas[i].m_chapter == chapter)
			{
				return this.m_debaguDatas[i].m_bossFlag;
			}
		}
		return false;
	}
}
