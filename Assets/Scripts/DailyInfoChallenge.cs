using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class DailyInfoChallenge : MonoBehaviour
{
	private DailyInfo m_parent;

	private daily_challenge.DailyMissionInfo m_info;

	public void Setup(DailyInfo parent)
	{
		this.m_parent = parent;
		this.m_info = daily_challenge.GetInfoFromSaveData(-1L);
		if (this.m_info != null)
		{
			UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_detail");
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_days");
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "today_set");
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "tomorrow_set");
			if (uIButtonMessage != null)
			{
				uIButtonMessage.target = this.m_parent.gameObject;
				uIButtonMessage.functionName = "OnClickChallenge";
			}
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
			if (gameObject != null)
			{
				UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_clear");
				UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_daily_challenge");
				UISlider uISlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(gameObject, "Pgb_attainment");
				GameObject target = GameObjectUtil.FindChildGameObject(gameObject, "item_today");
				this.SetupItem(target, this.m_info.IsClearTodayMission);
				if (uISprite != null)
				{
					uISprite.gameObject.SetActive(this.m_info.IsClearTodayMission);
				}
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
				if (uISlider != null)
				{
					float num = (float)this.m_info.TodayMissionClearQuota / (float)this.m_info.TodayMissionQuota;
					if (num > 1f)
					{
						num = 1f;
					}
					else if (num < 0f)
					{
						num = 0f;
					}
					uISlider.value = num;
				}
			}
			if (gameObject2 != null)
			{
				GameObject target2 = GameObjectUtil.FindChildGameObject(gameObject2, "item_tomorrow");
				this.SetupItem(target2, false);
			}
		}
	}

	private void SetupItem(GameObject target, bool isClear)
	{
		if (target != null && this.m_info != null)
		{
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(target, "img_chao");
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(target, "img_chara");
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(target, "img_daily_item");
			UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(target, "img_check");
			if (uISprite3 != null)
			{
				uISprite3.gameObject.SetActive(isClear);
			}
			int num = this.m_info.DayIndex;
			if (num >= this.m_info.InceniveIdTable.Length)
			{
				num = this.m_info.InceniveIdTable.Length - 1;
			}
			int num2 = this.m_info.InceniveIdTable[num];
			int num3 = Mathf.FloorToInt((float)num2 / 100000f);
			if (uISprite2 != null)
			{
				uISprite2.gameObject.SetActive(num3 == 0);
				if (num3 == 0)
				{
					uISprite2.spriteName = ((num >= this.m_info.InceniveIdTable.Length - 1) ? "ui_cmn_icon_item_9" : ("ui_cmn_icon_item_" + num2));
				}
			}
			if (uISprite != null)
			{
				uISprite.gameObject.SetActive(num3 == 3);
				if (num3 == 3)
				{
					UISprite arg_14F_0 = uISprite;
					string arg_14A_0 = "ui_tex_player_";
					ServerItem serverItem = new ServerItem((ServerItem.Id)num2);
					arg_14F_0.spriteName = arg_14A_0 + CharaTypeUtil.GetCharaSpriteNameSuffix(serverItem.charaType);
				}
			}
			if (uITexture != null)
			{
				uITexture.gameObject.SetActive(num3 == 4);
				if (num3 == 4)
				{
					ChaoTextureManager instance = ChaoTextureManager.Instance;
					int chao_id = num2 - 400000;
					if (instance != null)
					{
						Texture loadedTexture = instance.GetLoadedTexture(chao_id);
						if (loadedTexture != null)
						{
							uITexture.mainTexture = loadedTexture;
						}
						else
						{
							ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
							instance.GetTexture(chao_id, info);
						}
					}
				}
			}
		}
	}
}
