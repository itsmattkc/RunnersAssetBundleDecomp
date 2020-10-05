using System;
using UnityEngine;

public class MultiSetParaloopItemPointCircle : MultiSetBase
{
	private int m_tblID;

	private bool m_setup;

	protected override void OnSpawned()
	{
		base.OnSpawned();
	}

	protected override void OnCreateSetup()
	{
		ObjItemPoint objItemPoint = this.GetObjItemPoint();
		if (objItemPoint != null)
		{
			objItemPoint.SetID(this.m_tblID);
		}
	}

	protected override void UpdateLocal()
	{
		if (!this.m_setup)
		{
			ObjItemPoint objItemPoint = this.GetObjItemPoint();
			if (objItemPoint != null && objItemPoint.IsCreateItemBox())
			{
				this.SetActiveParaloopComponent(false);
				this.m_setup = true;
			}
		}
	}

	public void SetID(int id)
	{
		this.m_tblID = id;
	}

	public void SucceedParaloop()
	{
		if (MultiSetParaloopCircle.IsNowParaloop())
		{
			this.SetActiveParaloopComponent(true);
			this.SetStartMagnet();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				string a = LayerMask.LayerToName(gameObject.layer);
				if (a == "Player")
				{
					this.SucceedParaloop();
					ObjUtil.PopCamera(CameraType.LOOP_TERRAIN, 5.5f);
				}
				else if (a == "Magnet")
				{
				}
			}
		}
	}

	private void SetActiveParaloopComponent(bool flag)
	{
		ObjItemBox objItemBox = this.GetObjItemBox();
		if (objItemBox != null)
		{
			MagnetControl magnetControl = objItemBox.gameObject.GetComponent<MagnetControl>();
			if (magnetControl == null)
			{
				magnetControl = objItemBox.gameObject.AddComponent<MagnetControl>();
			}
			if (magnetControl != null)
			{
				magnetControl.enabled = flag;
			}
			SphereCollider component = objItemBox.gameObject.GetComponent<SphereCollider>();
			if (component != null)
			{
				component.enabled = flag;
			}
		}
	}

	private void SetStartMagnet()
	{
		ObjItemBox objItemBox = this.GetObjItemBox();
		if (objItemBox != null)
		{
			ObjUtil.StartMagnetControl(objItemBox.gameObject, 0.5f);
		}
	}

	private ObjItemPoint GetObjItemPoint()
	{
		if (this.m_dataList.Count > 0)
		{
			GameObject obj = this.m_dataList[0].m_obj;
			if (obj != null)
			{
				return obj.GetComponent<ObjItemPoint>();
			}
		}
		return null;
	}

	private ObjItemBox GetObjItemBox()
	{
		ObjItemPoint objItemPoint = this.GetObjItemPoint();
		if (objItemPoint != null)
		{
			GameObject gameObject = objItemPoint.gameObject;
			if (gameObject != null)
			{
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					Transform child = gameObject.transform.GetChild(i);
					if (child != null)
					{
						ObjItemBox component = child.gameObject.GetComponent<ObjItemBox>();
						if (component != null)
						{
							return component;
						}
					}
				}
			}
		}
		return null;
	}
}
