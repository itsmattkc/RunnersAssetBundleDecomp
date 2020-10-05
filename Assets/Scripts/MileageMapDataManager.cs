using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class MileageMapDataManager : MonoBehaviour
{
	private Dictionary<string, MileageMapData> m_mileage_datas;

	private string m_current_key;

	private int m_mileageStageIndex = 1;

	private List<string> m_loadingFaceList = new List<string>();

	private List<string> m_keepList = new List<string>();

	private List<ServerMileageReward> m_rewardList = new List<ServerMileageReward>();

	private static MileageMapDataManager instance;

	public static MileageMapDataManager Instance
	{
		get
		{
			return MileageMapDataManager.instance;
		}
	}

	public int MileageStageIndex
	{
		get
		{
			return this.m_mileageStageIndex;
		}
		set
		{
			this.m_mileageStageIndex = value;
		}
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		if (this.m_mileage_datas == null)
		{
			this.m_mileage_datas = new Dictionary<string, MileageMapData>();
		}
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (MileageMapDataManager.instance == this)
		{
			MileageMapDataManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (MileageMapDataManager.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			MileageMapDataManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void SetData(TextAsset xml_data)
	{
		string s = AESCrypt.Decrypt(xml_data.text);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MileageMapData[]));
		StringReader textReader = new StringReader(s);
		MileageMapData[] array = (MileageMapData[])xmlSerializer.Deserialize(textReader);
		if (array[0] != null)
		{
			int episode = array[0].scenario.episode;
			int chapter = array[0].scenario.chapter;
			string key = this.GetKey(episode, chapter);
			if (!this.IsExist(key))
			{
				if (this.m_current_key == null)
				{
					this.m_current_key = key;
				}
				this.m_mileage_datas.Add(key, array[0]);
			}
		}
	}

	public void SetCurrentData(int episode, int chapter)
	{
		string key = this.GetKey(episode, chapter);
		if (this.IsExist(key))
		{
			this.m_current_key = key;
		}
	}

	public void DestroyData()
	{
		this.m_mileage_datas.Clear();
		this.DestroyFaceAndBGData(false);
		this.m_current_key = null;
	}

	public void DestroyFaceAndBGData(bool keepFlag = false)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "MileageMapFace");
		List<GameObject> list = new List<GameObject>();
		if (gameObject != null)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
				if (!this.m_loadingFaceList.Contains(gameObject2.name))
				{
					if (keepFlag)
					{
						if (!this.m_keepList.Contains(gameObject2.name))
						{
							list.Add(gameObject2);
						}
					}
					else
					{
						list.Add(gameObject2);
					}
				}
			}
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "MileageMapBG");
		if (gameObject3 != null)
		{
			for (int j = 0; j < gameObject3.transform.childCount; j++)
			{
				GameObject gameObject4 = gameObject3.transform.GetChild(j).gameObject;
				if (keepFlag)
				{
					if (!this.m_keepList.Contains(gameObject4.name))
					{
						list.Add(gameObject4);
					}
				}
				else
				{
					list.Add(gameObject4);
				}
			}
		}
		foreach (GameObject current in list)
		{
			UnityEngine.Object.Destroy(current);
		}
		if (!keepFlag)
		{
			this.m_keepList.Clear();
		}
	}

	public MileageMapData GetMileageMapData()
	{
		if (this.IsExist(this.m_current_key))
		{
			return this.m_mileage_datas[this.m_current_key];
		}
		return null;
	}

	public ServerMileageReward GetMileageReward(int episode, int chapter, int point)
	{
		foreach (ServerMileageReward current in this.m_rewardList)
		{
			if (current.m_episode == episode && current.m_chapter == chapter && current.m_point == point)
			{
				return current;
			}
		}
		return null;
	}

	public MileageMapData GetMileageMapData(int episode, int chapter)
	{
		string key = this.GetKey(episode, chapter);
		if (this.IsExist(key))
		{
			return this.m_mileage_datas[key];
		}
		return null;
	}

	public int GetRouteId(int episode, int chapter, int point)
	{
		string key = this.GetKey(episode, chapter);
		if (this.IsExist(key) && point < 5)
		{
			return this.m_mileage_datas[key].map_data.route_data[point].id;
		}
		return -1;
	}

	public void SetPointIncentiveData(int episode, int chapter, int point, RewardData src_reward)
	{
		string key = this.GetKey(episode, chapter);
		if (this.IsExist(key))
		{
			int num = this.m_mileage_datas[key].event_data.point.Length;
			if (point < num)
			{
				this.m_mileage_datas[key].event_data.point[point].reward.Set(src_reward);
			}
		}
	}

	public void SetChapterIncentiveData(int episode, int chapter, int index, RewardData src_reward)
	{
		string key = this.GetKey(episode, chapter);
		if (this.IsExist(key) && this.m_mileage_datas[key].map_data.reward != null)
		{
			int num = this.m_mileage_datas[key].map_data.reward.Length;
			if (index < num)
			{
				this.m_mileage_datas[key].map_data.reward[index].Set(src_reward);
			}
		}
	}

	public void SetEpisodeIncentiveData(int episode, int chapter, int index, RewardData src_reward)
	{
		string key = this.GetKey(episode, chapter);
		if (this.IsExist(key) && this.m_mileage_datas[key].scenario.reward != null)
		{
			int num = this.m_mileage_datas[key].scenario.reward.Length;
			if (index < num)
			{
				this.m_mileage_datas[key].scenario.reward[index].Set(src_reward);
			}
		}
	}

	public void SetRewardData(List<ServerMileageReward> rewardList)
	{
		this.m_rewardList.Clear();
		if (rewardList != null)
		{
			foreach (ServerMileageReward current in rewardList)
			{
				this.m_rewardList.Add(current);
			}
		}
	}

	public bool IsExist(string key)
	{
		return !string.IsNullOrEmpty(key) && this.m_mileage_datas != null && this.m_mileage_datas.ContainsKey(key);
	}

	public bool IsExist(int episode, int chapter)
	{
		string key = this.GetKey(episode, chapter);
		return this.IsExist(key);
	}

	private string GetKey(int episode, int chapter)
	{
		return episode.ToString("000") + chapter.ToString("00");
	}

	public void AddKeepList(string name)
	{
		foreach (string current in this.m_keepList)
		{
			if (name == current)
			{
				return;
			}
		}
		this.m_keepList.Add(name);
	}

	public void SetLoadingFaceId(List<int> loadingList)
	{
		this.m_loadingFaceList.Clear();
		foreach (int current in loadingList)
		{
			this.m_loadingFaceList.Add(MileageMapUtility.GetFaceTextureName(current));
		}
	}
}
