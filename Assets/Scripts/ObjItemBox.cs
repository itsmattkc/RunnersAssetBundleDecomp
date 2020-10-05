using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjItemBox")]
public class ObjItemBox : SpawnableObject
{
	private const string ModelName = "obj_cmn_itembox";

	private uint m_item_type;

	private GameObject m_item_obj;

	private bool m_end;

	protected override string GetModelName()
	{
		return "obj_cmn_itembox";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override void OnSpawned()
	{
	}

	public void CreateItem(ItemType item_type)
	{
		this.m_item_type = (uint)item_type;
		if (this.m_item_type < 8u)
		{
			string itemFileName = ItemTypeName.GetItemFileName((ItemType)this.m_item_type);
			if (itemFileName.Length > 0)
			{
				this.m_item_obj = base.AttachObject(ResourceCategory.OBJECT_RESOURCE, itemFileName, Vector3.zero, Quaternion.Euler(Vector3.zero));
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.m_end)
		{
			return;
		}
		if (other)
		{
			if (StageItemManager.Instance != null)
			{
				StageItemManager.Instance.OnAddItem(new MsgAddItemToManager((ItemType)this.m_item_type));
			}
			this.TakeItemBox();
		}
	}

	private void TakeItemBox()
	{
		this.m_end = true;
		ObjUtil.PlayEffectCollisionCenter(base.gameObject, "ef_com_itembox_open01", 1f, false);
		ObjUtil.PlaySE("obj_itembox", "SE");
		ObjUtil.SendGetItemIcon((ItemType)this.m_item_type);
		if (this.m_item_obj)
		{
			UnityEngine.Object.Destroy(this.m_item_obj);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
