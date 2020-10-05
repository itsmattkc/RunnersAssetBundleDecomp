using System;

public class ItemPriority
{
	private static readonly int[] ITEM_PRIORITY = new int[]
	{
		6,
		7,
		3,
		4,
		5,
		0,
		2,
		1
	};

	public static int GetItemPriority(ItemType type)
	{
		if (type < ItemType.NUM)
		{
			return ItemPriority.ITEM_PRIORITY[(int)type];
		}
		return 8;
	}
}
