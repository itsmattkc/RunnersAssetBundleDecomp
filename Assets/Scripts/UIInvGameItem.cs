using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIInvGameItem
{
	public enum Quality
	{
		Broken,
		Cursed,
		Damaged,
		Worn,
		Sturdy,
		Polished,
		Improved,
		Crafted,
		Superior,
		Enchanted,
		Epic,
		Legendary,
		_LastDoNotUse
	}

	[SerializeField]
	private int mBaseItemID;

	public UIInvGameItem.Quality quality = UIInvGameItem.Quality.Sturdy;

	public int itemLevel = 1;

	private UIInvBaseItem mBaseItem;

	public int baseItemID
	{
		get
		{
			return this.mBaseItemID;
		}
	}

	public UIInvBaseItem baseItem
	{
		get
		{
			if (this.mBaseItem == null)
			{
				this.mBaseItem = UIInvDatabase.FindByID(this.baseItemID);
			}
			return this.mBaseItem;
		}
	}

	public string name
	{
		get
		{
			if (this.baseItem == null)
			{
				return null;
			}
			return this.quality.ToString() + " " + this.baseItem.name;
		}
	}

	public float statMultiplier
	{
		get
		{
			float num = 0f;
			switch (this.quality)
			{
			case UIInvGameItem.Quality.Broken:
				num = 0f;
				break;
			case UIInvGameItem.Quality.Cursed:
				num = -1f;
				break;
			case UIInvGameItem.Quality.Damaged:
				num = 0.25f;
				break;
			case UIInvGameItem.Quality.Worn:
				num = 0.9f;
				break;
			case UIInvGameItem.Quality.Sturdy:
				num = 1f;
				break;
			case UIInvGameItem.Quality.Polished:
				num = 1.1f;
				break;
			case UIInvGameItem.Quality.Improved:
				num = 1.25f;
				break;
			case UIInvGameItem.Quality.Crafted:
				num = 1.5f;
				break;
			case UIInvGameItem.Quality.Superior:
				num = 1.75f;
				break;
			case UIInvGameItem.Quality.Enchanted:
				num = 2f;
				break;
			case UIInvGameItem.Quality.Epic:
				num = 2.5f;
				break;
			case UIInvGameItem.Quality.Legendary:
				num = 3f;
				break;
			}
			float num2 = (float)this.itemLevel / 50f;
			return num * Mathf.Lerp(num2, num2 * num2, 0.5f);
		}
	}

	public Color color
	{
		get
		{
			Color result = Color.white;
			switch (this.quality)
			{
			case UIInvGameItem.Quality.Broken:
				result = new Color(0.4f, 0.2f, 0.2f);
				break;
			case UIInvGameItem.Quality.Cursed:
				result = Color.red;
				break;
			case UIInvGameItem.Quality.Damaged:
				result = new Color(0.4f, 0.4f, 0.4f);
				break;
			case UIInvGameItem.Quality.Worn:
				result = new Color(0.7f, 0.7f, 0.7f);
				break;
			case UIInvGameItem.Quality.Sturdy:
				result = new Color(1f, 1f, 1f);
				break;
			case UIInvGameItem.Quality.Polished:
				result = NGUIMath.HexToColor(3774856959u);
				break;
			case UIInvGameItem.Quality.Improved:
				result = NGUIMath.HexToColor(2480359935u);
				break;
			case UIInvGameItem.Quality.Crafted:
				result = NGUIMath.HexToColor(1325334783u);
				break;
			case UIInvGameItem.Quality.Superior:
				result = NGUIMath.HexToColor(12255231u);
				break;
			case UIInvGameItem.Quality.Enchanted:
				result = NGUIMath.HexToColor(1937178111u);
				break;
			case UIInvGameItem.Quality.Epic:
				result = NGUIMath.HexToColor(2516647935u);
				break;
			case UIInvGameItem.Quality.Legendary:
				result = NGUIMath.HexToColor(4287627519u);
				break;
			}
			return result;
		}
	}

	public UIInvGameItem(int id)
	{
		this.mBaseItemID = id;
	}

	public UIInvGameItem(int id, UIInvBaseItem bi)
	{
		this.mBaseItemID = id;
		this.mBaseItem = bi;
	}

	public List<UIInvStat> CalculateStats()
	{
		List<UIInvStat> list = new List<UIInvStat>();
		if (this.baseItem != null)
		{
			float statMultiplier = this.statMultiplier;
			List<UIInvStat> stats = this.baseItem.stats;
			int i = 0;
			int count = stats.Count;
			while (i < count)
			{
				UIInvStat uIInvStat = stats[i];
				int num = Mathf.RoundToInt(statMultiplier * (float)uIInvStat.amount);
				if (num != 0)
				{
					bool flag = false;
					int j = 0;
					int count2 = list.Count;
					while (j < count2)
					{
						UIInvStat uIInvStat2 = list[j];
						if (uIInvStat2.id == uIInvStat.id && uIInvStat2.modifier == uIInvStat.modifier)
						{
							uIInvStat2.amount += num;
							flag = true;
							break;
						}
						j++;
					}
					if (!flag)
					{
						list.Add(new UIInvStat
						{
							id = uIInvStat.id,
							amount = num,
							modifier = uIInvStat.modifier
						});
					}
				}
				i++;
			}
			list.Sort(new Comparison<UIInvStat>(UIInvStat.CompareArmor));
		}
		return list;
	}
}
