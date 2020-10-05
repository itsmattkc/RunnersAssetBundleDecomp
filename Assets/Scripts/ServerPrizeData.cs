using System;
using Text;

public class ServerPrizeData
{
	private int m_itemId;

	public int weight;

	public int num;

	public int spinId;

	public int priority;

	public int itemId
	{
		get
		{
			return this.m_itemId;
		}
		set
		{
			this.m_itemId = value;
			if (this.m_itemId >= 300000 && this.m_itemId < 400000)
			{
				this.priority = 100;
			}
			else if (this.m_itemId >= 400000 && this.m_itemId < 500000)
			{
				if (this.m_itemId >= 402000)
				{
					this.priority = 2;
				}
				else if (this.m_itemId >= 401000)
				{
					this.priority = 1;
				}
				else
				{
					this.priority = 0;
				}
			}
			else if (this.m_itemId == 200002)
			{
				this.priority = -1;
			}
			else
			{
				this.priority = -100;
			}
		}
	}

	public ServerPrizeData()
	{
		this.itemId = 0;
		this.weight = 0;
		this.num = 1;
		this.spinId = 0;
		this.priority = -100;
	}

	public string GetItemName()
	{
		ServerItem serverItem = new ServerItem((ServerItem.Id)this.itemId);
		string result;
		if (serverItem.idType == ServerItem.IdType.CHARA)
		{
			result = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, RouletteUtility.GetChaoGroupName(this.itemId), RouletteUtility.GetChaoCellName(this.itemId)).text;
		}
		else if (serverItem.idType == ServerItem.IdType.CHAO)
		{
			result = TextManager.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, RouletteUtility.GetChaoGroupName(this.itemId), RouletteUtility.GetChaoCellName(this.itemId)).text;
		}
		else
		{
			result = serverItem.serverItemName;
		}
		return result;
	}

	public void CopyTo(ServerPrizeData to)
	{
		to.itemId = this.itemId;
		to.num = this.num;
		to.weight = this.weight;
		to.spinId = this.spinId;
	}
}
