using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIInvDatabase : MonoBehaviour
{
	private static UIInvDatabase[] mList;

	private static bool mIsDirty = true;

	public int databaseID;

	public List<UIInvBaseItem> items = new List<UIInvBaseItem>();

	public UIAtlas iconAtlas;

	public static UIInvDatabase[] list
	{
		get
		{
			if (UIInvDatabase.mIsDirty)
			{
				UIInvDatabase.mIsDirty = false;
				UIInvDatabase.mList = NGUITools.FindActive<UIInvDatabase>();
			}
			return UIInvDatabase.mList;
		}
	}

	private void OnEnable()
	{
		UIInvDatabase.mIsDirty = true;
	}

	private void OnDisable()
	{
		UIInvDatabase.mIsDirty = true;
	}

	private UIInvBaseItem GetItem(int id16)
	{
		int i = 0;
		int count = this.items.Count;
		while (i < count)
		{
			UIInvBaseItem uIInvBaseItem = this.items[i];
			if (uIInvBaseItem.id16 == id16)
			{
				return uIInvBaseItem;
			}
			i++;
		}
		return null;
	}

	private static UIInvDatabase GetDatabase(int dbID)
	{
		int i = 0;
		int num = UIInvDatabase.list.Length;
		while (i < num)
		{
			UIInvDatabase uIInvDatabase = UIInvDatabase.list[i];
			if (uIInvDatabase.databaseID == dbID)
			{
				return uIInvDatabase;
			}
			i++;
		}
		return null;
	}

	public static UIInvBaseItem FindByID(int id32)
	{
		UIInvDatabase database = UIInvDatabase.GetDatabase(id32 >> 16);
		return (!(database != null)) ? null : database.GetItem(id32 & 65535);
	}

	public static UIInvBaseItem FindByName(string exact)
	{
		int i = 0;
		int num = UIInvDatabase.list.Length;
		while (i < num)
		{
			UIInvDatabase uIInvDatabase = UIInvDatabase.list[i];
			int j = 0;
			int count = uIInvDatabase.items.Count;
			while (j < count)
			{
				UIInvBaseItem uIInvBaseItem = uIInvDatabase.items[j];
				if (uIInvBaseItem.name == exact)
				{
					return uIInvBaseItem;
				}
				j++;
			}
			i++;
		}
		return null;
	}

	public static int FindItemID(UIInvBaseItem item)
	{
		int i = 0;
		int num = UIInvDatabase.list.Length;
		while (i < num)
		{
			UIInvDatabase uIInvDatabase = UIInvDatabase.list[i];
			if (uIInvDatabase.items.Contains(item))
			{
				return uIInvDatabase.databaseID << 16 | item.id16;
			}
			i++;
		}
		return -1;
	}
}
