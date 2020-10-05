using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendSignManager : MonoBehaviour
{
	public bool m_debugDraw;

	public bool m_debugFriend;

	private Texture2D m_debugTexture;

	private static int CREATE_MAX = 10;

	private List<FriendSignData> m_data = new List<FriendSignData>();

	private static FriendSignManager instance;

	public static FriendSignManager Instance
	{
		get
		{
			if (FriendSignManager.instance == null)
			{
				FriendSignManager.instance = (UnityEngine.Object.FindObjectOfType(typeof(FriendSignManager)) as FriendSignManager);
			}
			return FriendSignManager.instance;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	public void SetupFriendSignManager()
	{
		this.DebugDraw("SetupFriendSignManager");
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		PlayerImageManager playerImageManager = GameObjectUtil.FindGameObjectComponent<PlayerImageManager>("PlayerImageManager");
		List<ServerDistanceFriendEntry> distanceFriendEntry = ServerInterface.DistanceFriendEntry;
		if (socialInterface != null && playerImageManager != null && socialInterface.FriendList != null)
		{
			int num = UnityEngine.Random.Range(0, socialInterface.FriendList.Count);
			int num2 = num;
			for (int i = 0; i < socialInterface.FriendList.Count; i++)
			{
				if (num2 >= socialInterface.FriendList.Count)
				{
					num2 = 0;
				}
				Texture2D playerImage = playerImageManager.GetPlayerImage(socialInterface.FriendList[num2].Id);
				int distance = this.GetDistance(distanceFriendEntry, socialInterface.FriendList[num2].CustomData);
				if (distance > 0)
				{
					FriendSignData item = new FriendSignData(this.m_data.Count, (float)distance, playerImage, false);
					this.m_data.Add(item);
					if (this.m_data.Count >= FriendSignManager.CREATE_MAX)
					{
						break;
					}
				}
				num2++;
			}
		}
	}

	public List<FriendSignData> GetFriendSignDataList()
	{
		return this.m_data;
	}

	public void SetAppear(int index)
	{
		if (index < this.m_data.Count)
		{
			this.m_data[index].m_appear = true;
			this.DebugDraw("SetAppear " + FriendSignManager.GetDebugDataString(this.m_data[index]));
		}
	}

	private void DebugDraw(string msg)
	{
	}

	public static string GetDebugDataString(FriendSignData data)
	{
		if (data != null)
		{
			string text = (!(data.m_texture == null)) ? "ok" : "null";
			return string.Concat(new object[]
			{
				"Friend(idx=",
				data.m_index,
				", dis=",
				data.m_distance,
				", tex=",
				text,
				", app=",
				data.m_appear.ToString(),
				")"
			});
		}
		return "Friend(null)";
	}

	private int GetDistance(List<ServerDistanceFriendEntry> friendEntry, SocialUserCustomData customData)
	{
		if (friendEntry != null && customData != null)
		{
			foreach (ServerDistanceFriendEntry current in friendEntry)
			{
				if (current != null)
				{
					this.DebugDraw(string.Concat(new object[]
					{
						"fe.m_friendId=",
						current.m_friendId,
						" customData.GameId=",
						customData.GameId,
						" fe.m_distance=",
						current.m_distance
					}));
					if (current.m_friendId == customData.GameId)
					{
						return current.m_distance;
					}
				}
			}
			return 0;
		}
		return 0;
	}

	private void OnMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
	}

	protected bool CheckInstance()
	{
		if (FriendSignManager.instance == null)
		{
			FriendSignManager.instance = this;
			return true;
		}
		if (this == FriendSignManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (FriendSignManager.instance == this)
		{
			FriendSignManager.instance = null;
		}
	}
}
