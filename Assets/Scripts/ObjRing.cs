using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common")]
public class ObjRing : SpawnableObject
{
	private const int m_add_ring_value = 1;

	private bool m_end;

	public static string GetRingModelName()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			return EventBossObjectTable.GetItemData(EventBossObjectTableItem.RingModel);
		}
		return "obj_cmn_ring";
	}

	protected override string GetModelName()
	{
		return ObjRing.GetRingModelName();
	}

	public static ResourceCategory GetRingModelCategory()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			return ResourceCategory.EVENT_RESOURCE;
		}
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ObjRing.GetRingModelCategory();
	}

	public static string GetRingEffect()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			return EventBossObjectTable.GetItemData(EventBossObjectTableItem.RingEffect);
		}
		return "ef_ob_get_ring01";
	}

	public static void SetPlayRingSE()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			ObjUtil.LightPlayEventSE(EventBossObjectTable.GetItemData(EventBossObjectTableItem.RingSE), EventManager.EventType.RAID_BOSS);
		}
		ObjUtil.LightPlaySE("obj_ring", "SE");
	}

	public static int GetScore()
	{
		return 10;
	}

	protected override void OnSpawned()
	{
		if (StageComboManager.Instance != null && (StageComboManager.Instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_RECOVERY_ALL_OBJ) || StageComboManager.Instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_DESTROY_AND_RECOVERY)))
		{
			this.OnDrawingRings(new MsgOnDrawingRings());
		}
		base.enabled = false;
	}

	public override void OnRevival()
	{
		MagnetControl component = base.GetComponent<MagnetControl>();
		if (component != null)
		{
			component.Reset();
		}
		SphereCollider component2 = base.GetComponent<SphereCollider>();
		if (component2)
		{
			component2.enabled = true;
		}
		BoxCollider component3 = base.GetComponent<BoxCollider>();
		if (component3)
		{
			component3.enabled = true;
		}
		this.m_end = false;
		this.OnSpawned();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.m_end)
		{
			return;
		}
		if (other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				string a = LayerMask.LayerToName(gameObject.layer);
				if (a == "Player")
				{
					if (gameObject.tag != "ChaoAttack")
					{
						gameObject.SendMessage("OnAddRings", 1, SendMessageOptions.DontRequireReceiver);
						this.TakeRing();
					}
				}
				else if (a == "HitRing" && gameObject.tag == "Chao")
				{
					GameObjectUtil.SendMessageToTagObjects("Player", "OnAddRings", 1, SendMessageOptions.DontRequireReceiver);
					this.TakeRing();
				}
			}
		}
	}

	private void OnDrawingRings(MsgOnDrawingRings msg)
	{
		ObjUtil.StartMagnetControl(base.gameObject);
	}

	private void OnDrawingRingsToChao(MsgOnDrawingRings msg)
	{
		if (msg != null)
		{
			ObjUtil.StartMagnetControl(base.gameObject, msg.m_target);
		}
	}

	private void OnDrawingRingsChaoAbility(MsgOnDrawingRings msg)
	{
		if (msg.m_chaoAbility == ChaoAbility.COMBO_RECOVERY_ALL_OBJ || msg.m_chaoAbility == ChaoAbility.COMBO_DESTROY_AND_RECOVERY)
		{
			this.OnDrawingRings(new MsgOnDrawingRings());
		}
	}

	private void TakeRing()
	{
		this.m_end = true;
		if (StageEffectManager.Instance != null)
		{
			StageEffectManager.Instance.PlayEffect(EffectPlayType.RING, ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
		}
		ObjRing.SetPlayRingSE();
		if (base.Share)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
