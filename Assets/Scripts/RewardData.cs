using System;

public struct RewardData
{
	public int serverId;

	public int reward_id;

	public int reward_count;

	public RewardData(int reward_id, int reward_count)
	{
		this.serverId = reward_id;
		ServerItem serverItem = new ServerItem((ServerItem.Id)reward_id);
		int num = reward_id;
		ServerItem.IdType idType = serverItem.idType;
		if (idType != ServerItem.IdType.RSRING)
		{
			if (idType != ServerItem.IdType.RING)
			{
				if (idType == ServerItem.IdType.EQUIP_ITEM)
				{
					num = serverItem.idIndex;
				}
			}
			else
			{
				num = 8;
			}
		}
		else
		{
			num = 9;
		}
		this.reward_id = num;
		this.reward_count = reward_count;
	}

	public void Set(RewardData src)
	{
		this.serverId = src.serverId;
		this.reward_id = src.reward_id;
		this.reward_count = src.reward_count;
	}
}
