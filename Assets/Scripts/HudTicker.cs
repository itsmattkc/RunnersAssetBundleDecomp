using System;
using System.Collections.Generic;
using UnityEngine;

public class HudTicker : MonoBehaviour
{
	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
		base.enabled = false;
	}

	public void OnUpdateTickerDisplay()
	{
		TickerWindow tickerWindow = this.GetTickerWindow();
		if (tickerWindow == null)
		{
			return;
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerTickerInfo tickerInfo = ServerInterface.TickerInfo;
			if (tickerInfo != null && tickerInfo.Data != null)
			{
				List<ServerTickerData> list = new List<ServerTickerData>();
				for (int i = 0; i < tickerInfo.Data.Count; i++)
				{
					ServerTickerData serverTickerData = tickerInfo.Data[i];
					if (serverTickerData != null && NetUtil.IsServerTimeWithinPeriod(serverTickerData.Start, serverTickerData.End))
					{
						list.Add(serverTickerData);
					}
				}
				if (list.Count > 0)
				{
					this.SetTickerData(list);
					return;
				}
			}
			List<ServerTickerData> list2 = new List<ServerTickerData>();
			ServerTickerData serverTickerData2 = new ServerTickerData();
			serverTickerData2.Init(0L, 0L, 0L, string.Empty);
			list2.Add(serverTickerData2);
			this.SetTickerData(list2);
			return;
		}
		List<ServerTickerData> list3 = new List<ServerTickerData>();
		ServerTickerData serverTickerData3 = new ServerTickerData();
		ServerTickerData serverTickerData4 = new ServerTickerData();
		serverTickerData3.Init(0L, 0L, 0L, "ログインしていません");
		serverTickerData4.Init(0L, 0L, 0L, "ログインしたらおしらせがひょうじされます");
		list3.Add(serverTickerData3);
		list3.Add(serverTickerData4);
		this.SetTickerData(list3);
	}

	public void OnTickerReset()
	{
		TickerWindow tickerWindow = this.GetTickerWindow();
		if (tickerWindow != null)
		{
			tickerWindow.ResetWindow();
		}
	}

	private GameObject GetLabelObject()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject == null)
		{
			return null;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "mainmenu_info_user");
		if (gameObject2 == null)
		{
			return null;
		}
		return GameObjectUtil.FindChildGameObject(gameObject2, "ticker");
	}

	private TickerWindow GetTickerWindow()
	{
		GameObject labelObject = this.GetLabelObject();
		if (labelObject == null)
		{
			return null;
		}
		return labelObject.GetComponent<TickerWindow>();
	}

	private void SetTickerData(List<ServerTickerData> tickerData)
	{
		TickerWindow tickerWindow = this.GetTickerWindow();
		if (tickerWindow != null)
		{
			tickerWindow.Setup(new TickerWindow.CInfo
			{
				tickerList = tickerData,
				labelName = "Lbl_ticker",
				moveSpeed = 1f,
				moveSpeedUp = 20f
			});
		}
	}
}
