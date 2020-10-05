using System;
using Text;

public class ShopItemTable
{
	private static ShopItemData[] m_shopItemDataTable;

	public static ShopItemData[] GetDataTable()
	{
		if (ShopItemTable.m_shopItemDataTable == null)
		{
			ShopItemTable.m_shopItemDataTable = new ShopItemData[8];
			for (int i = 0; i < ShopItemTable.m_shopItemDataTable.Length; i++)
			{
				int num = i + 1;
				ShopItemTable.m_shopItemDataTable[i] = new ShopItemData();
				ShopItemTable.m_shopItemDataTable[i].number = num;
				ShopItemTable.m_shopItemDataTable[i].rings = 100;
				ShopItemTable.m_shopItemDataTable[i].SetName(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ShopItem", "name" + num).text);
				ShopItemTable.m_shopItemDataTable[i].SetDetails(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ShopItem", "details" + num).text);
			}
		}
		return ShopItemTable.m_shopItemDataTable;
	}

	public static ShopItemData GetShopItemData(int id)
	{
		ShopItemData[] dataTable = ShopItemTable.GetDataTable();
		for (int i = 0; i < dataTable.Length; i++)
		{
			ShopItemData shopItemData = dataTable[i];
			if (shopItemData.id == id)
			{
				return shopItemData;
			}
		}
		return null;
	}

	public static ShopItemData GetShopItemDataOfIndex(int index)
	{
		return ShopItemTable.GetDataTable()[index];
	}
}
