using System;
using System.Collections.Generic;

public class HudContinueUtility
{
	public static int GetContinueCost()
	{
		int result = 1;
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerCampaignData continueCostCampaignData = HudContinueUtility.GetContinueCostCampaignData();
			if (continueCostCampaignData != null)
			{
				result = continueCostCampaignData.iContent;
			}
			else
			{
				List<ServerConsumedCostData> costList = ServerInterface.CostList;
				if (costList != null)
				{
					foreach (ServerConsumedCostData current in costList)
					{
						if (current != null)
						{
							if (current.consumedItemId == 950000)
							{
								result = current.numItem;
								break;
							}
						}
					}
				}
			}
		}
		return result;
	}

	public static bool IsInContinueCostCampaign()
	{
		return HudContinueUtility.GetContinueCostCampaignData() != null;
	}

	public static ServerCampaignData GetContinueCostCampaignData()
	{
		if (ServerInterface.CampaignState != null)
		{
			return ServerInterface.CampaignState.GetCampaignInSession(Constants.Campaign.emType.ContinueCost);
		}
		return null;
	}

	public static string GetContinueCostString()
	{
		return HudContinueUtility.GetContinueCost().ToString();
	}

	public static int GetRedStarRingCount()
	{
		return (int)SaveDataManager.Instance.ItemData.RedRingCount;
	}

	public static string GetRedStarRingCountString()
	{
		return HudContinueUtility.GetRedStarRingCount().ToString();
	}
}
