using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class GlowUpExpBar : MonoBehaviour
{
	public class ExpInfo
	{
		public int level;

		public int cost;

		public int exp;
	}

	public delegate void LevelUpCallback(int level);

	public delegate void EndCallback();

	private UISlider m_baseSlider;

	private UISlider m_glowUpSlider;

	private GlowUpExpBar.ExpInfo m_startInfo = new GlowUpExpBar.ExpInfo();

	private GlowUpExpBar.ExpInfo m_endInfo = new GlowUpExpBar.ExpInfo();

	private GlowUpExpBar.LevelUpCallback m_levelUpCallback;

	private GlowUpExpBar.EndCallback m_endCallback;

	private HudInterpolateConstant m_interpolate = new HudInterpolateConstant();

	private bool m_isPlaying;

	private UILabel m_expLabel;

	private List<int> m_costList;

	private static readonly int RATIO_TO_VALUE = 10000;

	private static readonly float BAR_SPEED_PER_SEC = 0.333333343f * (float)GlowUpExpBar.RATIO_TO_VALUE;

	public void SetBaseSlider(UISlider slider)
	{
		if (slider == null)
		{
			return;
		}
		this.m_baseSlider = slider;
		this.m_baseSlider.value = 0f;
	}

	public void SetGlowUpSlider(UISlider slider)
	{
		if (slider == null)
		{
			return;
		}
		this.m_glowUpSlider = slider;
		this.m_glowUpSlider.value = 0f;
	}

	public void SetStartExp(GlowUpExpBar.ExpInfo startInfo)
	{
		if (startInfo == null)
		{
			return;
		}
		this.m_startInfo = startInfo;
		float num = GlowUpExpBar.CalcSliderValue(startInfo);
		if (this.m_baseSlider != null)
		{
			this.m_baseSlider.value = num;
		}
		if (this.m_glowUpSlider != null)
		{
			this.m_glowUpSlider.value = num;
		}
		int cost = this.m_startInfo.cost;
		int exp = (int)((float)cost * num);
		string text = GlowUpExpBar.CalcExpString(exp, cost);
		if (!this.m_expLabel.gameObject.activeSelf)
		{
			this.m_expLabel.gameObject.SetActive(true);
		}
		this.m_expLabel.text = text;
	}

	public void SetExpLabel(UILabel expLabel)
	{
		this.m_expLabel = expLabel;
	}

	public void SetEndExp(GlowUpExpBar.ExpInfo endInfo)
	{
		if (endInfo == null)
		{
			return;
		}
		this.m_endInfo = endInfo;
	}

	public void SetCallback(GlowUpExpBar.LevelUpCallback levelUpCallback, GlowUpExpBar.EndCallback endCallback)
	{
		this.m_levelUpCallback = levelUpCallback;
		this.m_endCallback = endCallback;
	}

	public void SetLevelUpCostList(List<int> expList)
	{
		if (expList == null)
		{
			return;
		}
		this.m_costList = new List<int>();
		foreach (int current in expList)
		{
			this.m_costList.Add(current);
		}
	}

	public void PlayStart()
	{
		int startValue = (int)(((float)this.m_startInfo.level + GlowUpExpBar.CalcSliderValue(this.m_startInfo)) * (float)GlowUpExpBar.RATIO_TO_VALUE);
		int endValue = (int)(((float)this.m_endInfo.level + GlowUpExpBar.CalcSliderValue(this.m_endInfo)) * (float)GlowUpExpBar.RATIO_TO_VALUE);
		this.m_interpolate.Setup(startValue, endValue, GlowUpExpBar.BAR_SPEED_PER_SEC);
		if (this.m_baseSlider != null)
		{
			this.m_baseSlider.value = GlowUpExpBar.CalcSliderValue(this.m_startInfo);
		}
		if (this.m_glowUpSlider != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "eff_thumb");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
		this.m_isPlaying = true;
	}

	public void PlaySkip()
	{
		if (this.m_interpolate == null)
		{
			return;
		}
		int currentValue = this.m_interpolate.CurrentValue;
		int num = currentValue / GlowUpExpBar.RATIO_TO_VALUE;
		int num2 = num + 1;
		int forceValue = num2 * GlowUpExpBar.RATIO_TO_VALUE - 1;
		this.m_interpolate.SetForceValue(forceValue);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.m_isPlaying)
		{
			return;
		}
		int num = this.m_interpolate.Update(Time.deltaTime);
		int prevValue = this.m_interpolate.PrevValue;
		float num2 = 0f;
		if (this.m_glowUpSlider != null)
		{
			float num3 = (float)num / (float)GlowUpExpBar.RATIO_TO_VALUE;
			int num4 = num / GlowUpExpBar.RATIO_TO_VALUE;
			float num5 = num3 - (float)num4;
			this.m_glowUpSlider.value = num5;
			num2 = num5;
		}
		int num6 = num / GlowUpExpBar.RATIO_TO_VALUE;
		int num7 = prevValue / GlowUpExpBar.RATIO_TO_VALUE;
		if (num6 > num7)
		{
			if (this.m_levelUpCallback != null)
			{
				this.m_levelUpCallback(num6);
			}
			if (this.m_baseSlider != null)
			{
				this.m_baseSlider.value = 0f;
			}
			if (this.m_costList != null && this.m_costList.Count > 0 && num7 != this.m_startInfo.level)
			{
				int item = this.m_costList[0];
				this.m_costList.Remove(item);
			}
		}
		if (this.m_interpolate.IsEnd)
		{
			this.m_isPlaying = false;
			if (this.m_endCallback != null)
			{
				this.m_endCallback();
			}
			if (this.m_glowUpSlider != null)
			{
				GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "eff_thumb");
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}
		if (this.m_expLabel != null)
		{
			string text = string.Empty;
			if (!this.m_isPlaying)
			{
				int cost = this.m_endInfo.cost;
				int exp = this.m_endInfo.exp;
				text = GlowUpExpBar.CalcExpString(exp, cost);
			}
			else if (num6 == this.m_startInfo.level)
			{
				int cost2 = this.m_startInfo.cost;
				int exp2 = (int)((float)cost2 * num2);
				text = GlowUpExpBar.CalcExpString(exp2, cost2);
			}
			else if (num6 == this.m_endInfo.level)
			{
				int cost3 = this.m_endInfo.cost;
				int exp3 = (int)((float)cost3 * num2);
				text = GlowUpExpBar.CalcExpString(exp3, cost3);
			}
			else if (this.m_costList != null && this.m_costList.Count > 0)
			{
				int num8 = this.m_costList[0];
				int exp4 = (int)((float)num8 * num2);
				text = GlowUpExpBar.CalcExpString(exp4, num8);
			}
			if (!this.m_expLabel.gameObject.activeSelf)
			{
				this.m_expLabel.gameObject.SetActive(true);
			}
			this.m_expLabel.text = text;
		}
	}

	private static float CalcSliderValue(GlowUpExpBar.ExpInfo info)
	{
		int cost = info.cost;
		int exp = info.exp;
		if (cost == 0)
		{
			return 0f;
		}
		return (float)exp / (float)cost;
	}

	private static string CalcExpString(int exp, int cost)
	{
		string formatNumString = HudUtility.GetFormatNumString<int>(exp);
		string text = HudUtility.GetFormatNumString<int>(cost);
		TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MileageMap", "point");
		text2.ReplaceTag("{VALUE}", formatNumString);
		string text3 = text2.text;
		string text4 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Item", "ring").text;
		text += text4;
		return text3 + " / " + text;
	}
}
