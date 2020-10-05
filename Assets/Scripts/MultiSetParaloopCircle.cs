using Message;
using System;
using UnityEngine;

public class MultiSetParaloopCircle : MultiSetBase
{
	protected override void OnSpawned()
	{
		base.OnSpawned();
	}

	protected override void OnCreateSetup()
	{
		this.SetActiveParaloopComponent(false);
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
					ObjUtil.PopCamera(CameraType.LOOP_TERRAIN, 2.5f);
				}
				else if (a == "Magnet")
				{
				}
			}
		}
	}

	private void SetActiveParaloopComponent(bool flag)
	{
		for (int i = 0; i < this.m_dataList.Count; i++)
		{
			GameObject obj = this.m_dataList[i].m_obj;
			if (obj)
			{
				MagnetControl component = obj.GetComponent<MagnetControl>();
				if (component)
				{
					component.enabled = flag;
				}
				SphereCollider component2 = obj.GetComponent<SphereCollider>();
				if (component2)
				{
					component2.enabled = flag;
				}
				BoxCollider component3 = obj.GetComponent<BoxCollider>();
				if (component3)
				{
					component3.enabled = flag;
				}
			}
		}
	}

	private void SetStartMagnet()
	{
		for (int i = 0; i < this.m_dataList.Count; i++)
		{
			GameObject obj = this.m_dataList[i].m_obj;
			if (obj)
			{
				MsgOnDrawingRings value = new MsgOnDrawingRings();
				obj.SendMessage("OnDrawingRings", value, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static bool IsNowParaloop()
	{
		PlayerInformation playerInformation = ObjUtil.GetPlayerInformation();
		return playerInformation != null && playerInformation.IsNowParaloop();
	}
}
