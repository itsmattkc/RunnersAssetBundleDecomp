using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjSuperRing")]
public class ObjSuperRing : SpawnableObject
{
	private bool m_end;

	public static string ModelName = "obj_cmn_superring10";

	public static string SEName = "obj_superring";

	public static string GetSuperRingModelName()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			return EventBossObjectTable.GetItemData(EventBossObjectTableItem.Ring10Model);
		}
		return ObjSuperRing.ModelName;
	}

	protected override string GetModelName()
	{
		return ObjSuperRing.GetSuperRingModelName();
	}

	public static ResourceCategory GetSuperRingModelCategory()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			return ResourceCategory.EVENT_RESOURCE;
		}
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ObjSuperRing.GetSuperRingModelCategory();
	}

	public static string GetSuperRingEffect()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			return EventBossObjectTable.GetItemData(EventBossObjectTableItem.Ring10Effect);
		}
		return "ef_ob_get_superring01";
	}

	public static void SetPlaySuperRingSE()
	{
		if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage())
		{
			ObjUtil.PlayEventSE(EventBossObjectTable.GetItemData(EventBossObjectTableItem.Ring10SE), EventManager.EventType.RAID_BOSS);
		}
		ObjUtil.PlaySE(ObjSuperRing.SEName, "SE");
	}

	protected override void OnSpawned()
	{
		if (StageComboManager.Instance != null && (StageComboManager.Instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_RECOVERY_ALL_OBJ) || StageComboManager.Instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_DESTROY_AND_RECOVERY)))
		{
			this.OnDrawingRings(new MsgOnDrawingRings());
		}
		base.enabled = true;
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
						gameObject.SendMessage("OnAddRings", ObjSuperRing.GetRingCount(), SendMessageOptions.DontRequireReceiver);
						this.TakeRing();
					}
				}
				else if (a == "HitRing" && gameObject.tag == "Chao")
				{
					GameObjectUtil.SendMessageToTagObjects("Player", "OnAddRings", ObjSuperRing.GetRingCount(), SendMessageOptions.DontRequireReceiver);
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
			StageEffectManager.Instance.PlayEffect(EffectPlayType.SUPER_RING, ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
		}
		ObjSuperRing.SetPlaySuperRingSE();
		if (base.Share)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public static int GetRingCount()
	{
		int num = 10;
		if (StageAbilityManager.Instance != null)
		{
			num += (int)StageAbilityManager.Instance.GetChaoAbilityValue(ChaoAbility.SUPER_RING_UP);
		}
		return num;
	}

	public static void AddSuperRing(GameObject obj)
	{
		if (obj)
		{
			obj.SendMessage("OnAddRings", ObjSuperRing.GetRingCount(), SendMessageOptions.DontRequireReceiver);
		}
	}
}
