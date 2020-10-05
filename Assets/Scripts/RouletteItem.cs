using System;
using System.Collections.Generic;
using UnityEngine;

public class RouletteItem : MonoBehaviour
{
	[SerializeField]
	private GameObject m_common;

	[SerializeField]
	private GameObject m_egg;

	[SerializeField]
	private GameObject m_item;

	[SerializeField]
	private GameObject m_rank;

	[SerializeField]
	private GameObject m_campaign;

	[SerializeField]
	private List<GameObject> m_campaignList;

	private RouletteBoard m_parent;

	private int m_cellIndex;

	private Vector3 m_basePos = new Vector3(0f, 0f, 0f);

	public bool isRank
	{
		get
		{
			bool result = false;
			if (this.m_parent != null && this.m_parent.wheelData != null)
			{
				ServerWheelOptionsData wheelData = this.m_parent.wheelData;
				RouletteUtility.WheelType wheelType = wheelData.wheelType;
				if (wheelType != RouletteUtility.WheelType.Normal)
				{
					if (wheelType == RouletteUtility.WheelType.Rankup)
					{
						if (wheelData.GetCellItem(this.m_cellIndex).idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
						{
							result = true;
						}
					}
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}

	public void Setup(RouletteBoard parent, int cellIndex)
	{
		this.m_parent = parent;
		this.m_cellIndex = cellIndex;
		this.m_basePos = base.gameObject.transform.parent.localPosition;
		if (this.m_parent != null && this.m_parent.wheelData != null)
		{
			ServerWheelOptionsData wheelData = this.m_parent.wheelData;
			if (wheelData.isGeneral)
			{
				this.SetGeneral(wheelData);
			}
			else
			{
				RouletteUtility.WheelType wheelType = wheelData.wheelType;
				if (wheelType != RouletteUtility.WheelType.Normal)
				{
					if (wheelType == RouletteUtility.WheelType.Rankup)
					{
						this.SetItem(wheelData);
					}
				}
				else
				{
					this.SetEgg(wheelData);
				}
			}
			this.SetTweenDelay();
		}
	}

	private void SetTweenDelay()
	{
		TweenColor[] componentsInChildren = base.gameObject.GetComponentsInChildren<TweenColor>();
		TweenRotation[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<TweenRotation>();
		TweenAlpha[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<TweenAlpha>();
		TweenScale[] componentsInChildren4 = base.gameObject.GetComponentsInChildren<TweenScale>();
		TweenPosition[] componentsInChildren5 = base.gameObject.GetComponentsInChildren<TweenPosition>();
		if (componentsInChildren != null)
		{
			TweenColor[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				TweenColor tweenColor = array[i];
				tweenColor.delay = UnityEngine.Random.Range(0f, tweenColor.duration);
			}
		}
		if (componentsInChildren2 != null)
		{
			TweenRotation[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				TweenRotation tweenRotation = array2[j];
				tweenRotation.delay = UnityEngine.Random.Range(0f, tweenRotation.duration);
			}
		}
		if (componentsInChildren3 != null)
		{
			TweenAlpha[] array3 = componentsInChildren3;
			for (int k = 0; k < array3.Length; k++)
			{
				TweenAlpha tweenAlpha = array3[k];
				tweenAlpha.delay = UnityEngine.Random.Range(0f, tweenAlpha.duration);
			}
		}
		if (componentsInChildren4 != null)
		{
			TweenScale[] array4 = componentsInChildren4;
			for (int l = 0; l < array4.Length; l++)
			{
				TweenScale tweenScale = array4[l];
				tweenScale.delay = UnityEngine.Random.Range(0f, tweenScale.duration);
			}
		}
		if (componentsInChildren5 != null)
		{
			TweenPosition[] array5 = componentsInChildren5;
			for (int m = 0; m < array5.Length; m++)
			{
				TweenPosition tweenPosition = array5[m];
				tweenPosition.delay = UnityEngine.Random.Range(0f, tweenPosition.duration);
			}
		}
	}

	private void SetCampaign(ServerItem.Id itemId)
	{
		bool flag = false;
		bool flag2 = false;
		if (this.m_parent.wheelData.IsCampaign(Constants.Campaign.emType.PremiumRouletteOdds) && ServerInterface.CampaignState != null)
		{
			ServerCampaignData campaignInSession = ServerInterface.CampaignState.GetCampaignInSession(Constants.Campaign.emType.PremiumRouletteOdds, this.m_cellIndex);
			if (campaignInSession != null && campaignInSession.iContent > 0)
			{
				float cellWeight = this.m_parent.wheel.GetCellWeight(this.m_cellIndex);
				if ((float)campaignInSession.iContent > cellWeight)
				{
					flag = true;
				}
			}
		}
		if (this.m_parent.wheelData.IsCampaign(Constants.Campaign.emType.JackPotValueBonus) && ServerInterface.CampaignState != null)
		{
			ServerCampaignData campaignInSession2 = ServerInterface.CampaignState.GetCampaignInSession(Constants.Campaign.emType.JackPotValueBonus, this.m_cellIndex);
			if (campaignInSession2 != null && campaignInSession2.iContent > 0)
			{
				flag2 = true;
			}
		}
		if (this.m_campaign != null)
		{
			bool active = false;
			if ((flag || flag2) && this.m_campaignList != null && this.m_campaignList.Count > 0)
			{
				foreach (GameObject current in this.m_campaignList)
				{
					bool flag3 = false;
					if (current.name.IndexOf("jackpot") != -1)
					{
						if (itemId == ServerItem.Id.JACKPOT && flag2)
						{
							flag3 = true;
						}
					}
					else if (itemId != ServerItem.Id.JACKPOT && flag)
					{
						flag3 = true;
						float num = 50f / this.m_basePos.magnitude;
						float num2 = this.m_basePos.x * num;
						float num3 = this.m_basePos.y * num;
						num3 += 45f + num3 / 10f;
						if (num3 <= 20f && Mathf.Abs(num2) <= 60f)
						{
							if (num2 >= 0f)
							{
								num2 = 60f;
							}
							else
							{
								num2 = -60f;
							}
							num3 = 20f;
						}
						current.gameObject.transform.localPosition = new Vector3(num2, num3, 0f);
					}
					current.SetActive(flag3);
					if (flag3)
					{
						active = true;
						break;
					}
				}
			}
			this.m_campaign.SetActive(active);
		}
	}

	private void SetEgg(ServerWheelOptionsData data)
	{
		if (this.m_egg != null)
		{
			this.m_egg.SetActive(true);
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_egg, "main");
			uISprite.spriteName = "ui_roulette_chao_egg_" + data.GetCellEgg(this.m_cellIndex);
		}
		if (this.m_item != null)
		{
			this.m_item.SetActive(false);
		}
		if (this.m_rank != null)
		{
			this.m_rank.SetActive(false);
		}
		if (this.m_common != null)
		{
			this.m_common.SetActive(false);
		}
		this.SetCampaign(ServerItem.Id.CHAO_BEGIN);
	}

	private void SetItem(ServerWheelOptionsData data)
	{
		if (this.m_egg != null)
		{
			this.m_egg.SetActive(false);
		}
		if (this.m_item != null)
		{
			this.m_item.SetActive(false);
		}
		if (this.m_rank != null)
		{
			this.m_rank.SetActive(false);
		}
		if (this.m_common != null)
		{
			this.m_common.SetActive(false);
		}
		int num;
		ServerItem cellItem = data.GetCellItem(this.m_cellIndex, out num);
		if (cellItem.serverItemNum > 0)
		{
			num *= cellItem.serverItemNum;
		}
		ServerItem.IdType idType = cellItem.idType;
		switch (idType)
		{
		case ServerItem.IdType.RSRING:
		case ServerItem.IdType.RING:
		case ServerItem.IdType.ENERGY:
			break;
		default:
			if (idType != ServerItem.IdType.EQUIP_ITEM)
			{
				if (idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
				{
					if (this.m_rank != null)
					{
						base.transform.localScale = new Vector3(1f / base.transform.parent.transform.localScale.x, 1f / base.transform.parent.transform.localScale.x, 1f);
						this.m_rank.SetActive(true);
						UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rank, "jack");
						UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rank, "big");
						UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rank, "super");
						if (uISprite != null && uISprite2 != null && uISprite3 != null)
						{
							uISprite.gameObject.SetActive(false);
							uISprite2.gameObject.SetActive(false);
							uISprite3.gameObject.SetActive(false);
							switch (data.GetRouletteRank())
							{
							case RouletteUtility.WheelRank.Normal:
								uISprite2.gameObject.SetActive(true);
								break;
							case RouletteUtility.WheelRank.Big:
								uISprite3.gameObject.SetActive(true);
								break;
							case RouletteUtility.WheelRank.Super:
							{
								uISprite.gameObject.SetActive(true);
								UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(uISprite.gameObject, "ring");
								if (uILabel != null)
								{
									int num2 = RouletteManager.numJackpotRing;
									if (num2 <= 0)
									{
										num2 = 30000;
									}
									uILabel.text = HudUtility.GetFormatNumString<int>(num2);
								}
								break;
							}
							}
						}
					}
					goto IL_433;
				}
				if (idType != ServerItem.IdType.CHARA && idType != ServerItem.IdType.CHAO)
				{
					base.transform.localScale = new Vector3(1f, 1f, 1f);
					if (this.m_item != null)
					{
						this.m_item.SetActive(true);
						UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_item, "main");
						uISprite4.spriteName = "ui_cmn_icon_item_" + cellItem.id;
					}
					if (this.m_common != null)
					{
						this.m_common.SetActive(true);
						UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_common, "num");
						uILabel2.text = "×" + num;
					}
					goto IL_433;
				}
				if (this.m_egg != null)
				{
					this.m_egg.SetActive(true);
					UISprite uISprite5 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_egg, "main");
					uISprite5.spriteName = "ui_roulette_chao_egg_" + data.GetCellEgg(this.m_cellIndex);
				}
				goto IL_433;
			}
			break;
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		if (this.m_item != null)
		{
			this.m_item.SetActive(true);
			UISprite uISprite6 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_item, "main");
			uISprite6.spriteName = cellItem.serverItemSpriteNameRoulette;
		}
		if (this.m_common != null)
		{
			this.m_common.SetActive(true);
			UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_common, "num");
			uILabel3.text = "×" + num;
		}
		IL_433:
		this.SetCampaign(cellItem.id);
	}

	private void SetGeneral(ServerWheelOptionsData data)
	{
		if (this.m_egg != null)
		{
			this.m_egg.SetActive(false);
		}
		if (this.m_item != null)
		{
			this.m_item.SetActive(false);
		}
		if (this.m_rank != null)
		{
			this.m_rank.SetActive(false);
		}
		if (this.m_common != null)
		{
			this.m_common.SetActive(false);
		}
		int num;
		ServerItem cellItem = data.GetCellItem(this.m_cellIndex, out num);
		if (cellItem.serverItemNum > 0)
		{
			num *= cellItem.serverItemNum;
		}
		ServerItem.IdType idType = cellItem.idType;
		switch (idType)
		{
		case ServerItem.IdType.RSRING:
		case ServerItem.IdType.RING:
		case ServerItem.IdType.ENERGY:
			break;
		default:
			if (idType != ServerItem.IdType.EQUIP_ITEM)
			{
				if (idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
				{
					if (this.m_rank != null)
					{
						base.transform.localScale = new Vector3(1f / base.transform.parent.transform.localScale.x, 1f / base.transform.parent.transform.localScale.x, 1f);
						this.m_rank.SetActive(true);
						UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rank, "jack");
						UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rank, "big");
						UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_rank, "super");
						if (uISprite != null && uISprite2 != null && uISprite3 != null)
						{
							uISprite.gameObject.SetActive(false);
							uISprite2.gameObject.SetActive(false);
							uISprite3.gameObject.SetActive(false);
							switch (data.GetRouletteRank())
							{
							case RouletteUtility.WheelRank.Normal:
								uISprite2.gameObject.SetActive(true);
								break;
							case RouletteUtility.WheelRank.Big:
								uISprite3.gameObject.SetActive(true);
								break;
							case RouletteUtility.WheelRank.Super:
							{
								uISprite.gameObject.SetActive(true);
								UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(uISprite.gameObject, "ring");
								if (uILabel != null)
								{
									int num2 = RouletteManager.numJackpotRing;
									if (num2 <= 0)
									{
										num2 = 30000;
									}
									uILabel.text = HudUtility.GetFormatNumString<int>(num2);
								}
								break;
							}
							}
						}
					}
					goto IL_46B;
				}
				if (idType != ServerItem.IdType.CHARA && idType != ServerItem.IdType.CHAO)
				{
					base.transform.localScale = new Vector3(1f, 1f, 1f);
					if (this.m_item != null)
					{
						this.m_item.SetActive(true);
						UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_item, "main");
						uISprite4.spriteName = "ui_cmn_icon_item_" + cellItem.id;
					}
					if (this.m_common != null)
					{
						this.m_common.SetActive(true);
						UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_common, "num");
						uILabel2.text = "×" + num;
					}
					goto IL_46B;
				}
				if (this.m_egg != null)
				{
					this.m_egg.SetActive(true);
					UISprite uISprite5 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_egg, "main");
					if (uISprite5 != null)
					{
						if (cellItem.idType == ServerItem.IdType.CHARA)
						{
							uISprite5.spriteName = "ui_roulette_chao_egg_100";
						}
						else
						{
							int id = (int)cellItem.id;
							int num3 = id / 1000 % 10;
							uISprite5.spriteName = "ui_roulette_chao_egg_" + num3;
						}
					}
				}
				goto IL_46B;
			}
			break;
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		if (this.m_item != null)
		{
			this.m_item.SetActive(true);
			UISprite uISprite6 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_item, "main");
			uISprite6.spriteName = cellItem.serverItemSpriteNameRoulette;
		}
		if (this.m_common != null)
		{
			this.m_common.SetActive(true);
			UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_common, "num");
			uILabel3.text = "×" + num;
		}
		IL_46B:
		this.SetCampaign(cellItem.id);
	}
}
