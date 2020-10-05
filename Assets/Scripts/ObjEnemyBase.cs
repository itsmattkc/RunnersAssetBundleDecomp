using Message;
using Mission;
using System;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class ObjEnemyBase : SpawnableObject
{
	private bool m_rightAnimFlag;

	private bool m_setupAnimFlag;

	private bool m_destroyFlag;

	private bool m_heabyBomFlag;

	private bool m_end;

	private ObjEnemyUtil.EnemyType m_enmyType;

	protected override void OnSpawned()
	{
		if (StageComboManager.Instance != null && StageComboManager.Instance.IsChaoFlagStatus(StageComboManager.ChaoFlagStatus.ENEMY_DEAD))
		{
			this.SetBroken();
		}
	}

	private void Update()
	{
		if (this.m_end)
		{
			return;
		}
		if (!this.m_setupAnimFlag)
		{
			this.m_setupAnimFlag = this.SetupAnim();
		}
		if (this.m_destroyFlag)
		{
			this.m_end = true;
			this.CreateBrokenItem();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void OnMsgObjectDead(MsgObjectDead msg)
	{
		if (base.enabled)
		{
			this.SetBroken();
		}
	}

	public void OnMsgHeavyBombDead(float goldAnimalPercent)
	{
		if (base.enabled)
		{
			float num = UnityEngine.Random.Range(0f, 99.9f);
			if (goldAnimalPercent >= num)
			{
				this.m_heabyBomFlag = true;
			}
			this.SetBroken();
		}
	}

	protected override string GetModelName()
	{
		return ObjEnemyUtil.GetModelName(this.GetEnemyType(), this.GetModelFiles());
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.ENEMY_RESOURCE;
	}

	protected virtual ObjEnemyUtil.EnemyType GetOriginalType()
	{
		return ObjEnemyUtil.EnemyType.NORMAL;
	}

	protected virtual string[] GetModelFiles()
	{
		return null;
	}

	protected virtual int[] GetScoreTable()
	{
		return ObjEnemyUtil.GetDefaultScoreTable();
	}

	protected virtual ObjEnemyUtil.EnemyEffectSize GetEffectSize()
	{
		return ObjEnemyUtil.EnemyEffectSize.S;
	}

	protected virtual bool IsNormalMotion(float speed)
	{
		float y = base.transform.rotation.eulerAngles.y;
		return y <= 80f || y >= 100f;
	}

	protected void SetupEnemy(uint id, float speed)
	{
		this.m_enmyType = this.GetOriginalType();
		this.SetupRareCheck(id);
		this.SetupMetalCheck();
		if (!this.IsNormalMotion(speed))
		{
			this.m_rightAnimFlag = true;
		}
	}

	private void SetupRareCheck(uint id)
	{
		StageComboManager instance = StageComboManager.Instance;
		if (instance != null && instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_RARE_ENEMY))
		{
			this.m_enmyType = ObjEnemyUtil.EnemyType.RARE;
		}
		if (this.m_enmyType != ObjEnemyUtil.EnemyType.RARE)
		{
			GameObject gameObject = GameObjectUtil.FindGameObjectWithTag("GameModeStage", "GameModeStage");
			if (gameObject != null)
			{
				GameModeStage component = gameObject.GetComponent<GameModeStage>();
				if (component != null)
				{
					RareEnemyTable rareEnemyTable = component.GetRareEnemyTable();
					if (rareEnemyTable != null && rareEnemyTable.IsRareEnemy(id))
					{
						this.m_enmyType = ObjEnemyUtil.EnemyType.RARE;
					}
				}
			}
		}
	}

	private void SetupMetalCheck()
	{
		if (this.m_enmyType == ObjEnemyUtil.EnemyType.NORMAL)
		{
			StageComboManager instance = StageComboManager.Instance;
			if (instance != null && instance.IsActiveComboChaoAbility(ChaoAbility.COMBO_METAL_AND_METAL_SCORE))
			{
				this.m_enmyType = ObjEnemyUtil.EnemyType.METAL;
			}
		}
	}

	private bool SetupAnim()
	{
		Animator componentInChildren = base.GetComponentInChildren<Animator>();
		if (componentInChildren)
		{
			if (this.m_rightAnimFlag)
			{
				componentInChildren.Play("Idle_r");
			}
			return true;
		}
		return false;
	}

	private bool IsRare()
	{
		return this.m_enmyType == ObjEnemyUtil.EnemyType.RARE;
	}

	private bool IsMetal()
	{
		return this.m_enmyType == ObjEnemyUtil.EnemyType.METAL;
	}

	private int GetScore()
	{
		return ObjEnemyUtil.GetScore(this.m_enmyType, this.GetScoreTable());
	}

	private ObjEnemyUtil.EnemyType GetEnemyType()
	{
		return this.m_enmyType;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.m_destroyFlag)
		{
			return;
		}
		if (other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				AttackPower attack = AttackPower.PlayerSpin;
				if (this.m_enmyType == ObjEnemyUtil.EnemyType.METAL)
				{
					attack = AttackPower.PlayerSpin;
				}
				MsgHitDamage value = new MsgHitDamage(base.gameObject, attack);
				gameObject.SendMessage("OnDamageHit", value, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void OnDamageHit(MsgHitDamage msg)
	{
		if (this.m_destroyFlag)
		{
			return;
		}
		if (msg.m_sender)
		{
			GameObject gameObject = msg.m_sender.gameObject;
			if (gameObject)
			{
				bool flag = this.IsMetal();
				if (this.IsEnemyBroken(flag, msg.m_attackPower, msg.m_attackAttribute))
				{
					MsgHitDamageSucceed value = new MsgHitDamageSucceed(base.gameObject, 0, ObjUtil.GetCollisionCenterPosition(base.gameObject), base.transform.rotation);
					gameObject.SendMessage("OnDamageSucceed", value, SendMessageOptions.DontRequireReceiver);
					this.SetPlayerBroken(flag, msg.m_attackAttribute);
					ObjUtil.CreateBrokenBonus(base.gameObject, gameObject, msg.m_attackAttribute);
				}
				else if (flag && msg.m_attackPower > 0)
				{
					MsgAttackGuard value2 = new MsgAttackGuard(base.gameObject);
					gameObject.SendMessage("OnAttackGuard", value2, SendMessageOptions.DontRequireReceiver);
					this.PlayGuardEffect();
					ObjUtil.LightPlaySE("enm_metal_hit", "SE");
				}
			}
		}
	}

	private bool IsEnemyBroken(bool metal, int attackPower, uint attribute_state)
	{
		AttackPower attackPower2 = (!metal) ? AttackPower.PlayerSpin : AttackPower.PlayerPower;
		return attackPower >= (int)attackPower2 || (metal && attackPower == 2 && (attribute_state & 8u) > 0u);
	}

	private void PlayHitEffect()
	{
		EffectPlayType type = EffectPlayType.UNKNOWN;
		switch (this.GetEffectSize())
		{
		case ObjEnemyUtil.EnemyEffectSize.S:
			type = EffectPlayType.ENEMY_S;
			break;
		case ObjEnemyUtil.EnemyEffectSize.M:
			type = EffectPlayType.ENEMY_M;
			break;
		case ObjEnemyUtil.EnemyEffectSize.L:
			type = EffectPlayType.ENEMY_L;
			break;
		}
		if (StageEffectManager.Instance != null)
		{
			StageEffectManager.Instance.PlayEffect(type, ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
		}
	}

	private void PlayGuardEffect()
	{
		if (StageEffectManager.Instance != null)
		{
			StageEffectManager.Instance.PlayEffect(EffectPlayType.ENEMY_GUARD, ObjUtil.GetCollisionCenterPosition(base.gameObject), Quaternion.identity);
		}
	}

	private void CreateBrokenItem()
	{
		TimerType timerType = this.GetTimerType();
		if (timerType != TimerType.ERROR)
		{
			ObjTimerUtil.CreateTimer(base.gameObject, timerType);
		}
		else
		{
			this.CreateAnimal();
		}
	}

	private TimerType GetTimerType()
	{
		if (StageModeManager.Instance != null && StageModeManager.Instance.IsQuickMode() && ObjTimerUtil.IsEnableCreateTimer())
		{
			GameObject gameObject = GameObjectUtil.FindGameObjectWithTag("GameModeStage", "GameModeStage");
			if (gameObject != null)
			{
				GameModeStage component = gameObject.GetComponent<GameModeStage>();
				if (component != null)
				{
					EnemyExtendItemTable enemyExtendItemTable = component.GetEnemyExtendItemTable();
					if (enemyExtendItemTable != null)
					{
						int randomRange = ObjUtil.GetRandomRange100();
						int tableItemData = enemyExtendItemTable.GetTableItemData(EnemyExtendItemTableItem.BronzeTimer);
						int num = tableItemData + enemyExtendItemTable.GetTableItemData(EnemyExtendItemTableItem.SilverTimer);
						int num2 = num + enemyExtendItemTable.GetTableItemData(EnemyExtendItemTableItem.GoldTimer);
						if (randomRange < tableItemData)
						{
							return TimerType.BRONZE;
						}
						if (randomRange < num)
						{
							return TimerType.SILVER;
						}
						if (randomRange < num2)
						{
							return TimerType.GOLD;
						}
					}
				}
			}
		}
		return TimerType.ERROR;
	}

	private void CreateAnimal()
	{
		if (this.m_heabyBomFlag)
		{
			ObjAnimalUtil.CreateAnimal(base.gameObject, AnimalType.FLICKY);
			this.m_heabyBomFlag = false;
		}
		else
		{
			ObjAnimalUtil.CreateAnimal(base.gameObject);
		}
	}

	private void SetPlayerBroken(bool metal_type, uint attribute_state)
	{
		int num = this.GetScore();
		if (metal_type && StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.COMBO_METAL_AND_METAL_SCORE))
		{
			num = (int)StageAbilityManager.Instance.GetChaoAbilityExtraValue(ChaoAbility.COMBO_METAL_AND_METAL_SCORE);
		}
		if (StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.ENEMY_SCORE_SEVERALFOLD))
		{
			float chaoAbilityValue = StageAbilityManager.Instance.GetChaoAbilityValue(ChaoAbility.ENEMY_SCORE_SEVERALFOLD);
			float num2 = UnityEngine.Random.Range(0f, 99.9f);
			if (chaoAbilityValue >= num2)
			{
				num *= (int)StageAbilityManager.Instance.GetChaoAbilityExtraValue(ChaoAbility.ENEMY_SCORE_SEVERALFOLD);
				ObjUtil.RequestStartAbilityToChao(ChaoAbility.ENEMY_SCORE_SEVERALFOLD, false);
			}
		}
		List<ChaoAbility> list = new List<ChaoAbility>();
		list.Add(ChaoAbility.ENEMY_SCORE);
		ObjUtil.RequestStartAbilityToChao(ChaoAbility.ENEMY_SCORE, true);
		ObjUtil.GetChaoAbliltyPhantomFlag(attribute_state, ref list);
		num = ObjUtil.GetChaoAndEnemyScore(list, num);
		ObjUtil.SendMessageAddScore(num);
		ObjUtil.SendMessageScoreCheck(new StageScoreData(1, num));
		ObjUtil.SendMessageMission(Mission.EventID.ENEMYDEAD, 1);
		if (this.IsRare())
		{
			ObjUtil.SendMessageMission(Mission.EventID.GOLDENENEMYDEAD, 1);
		}
		ObjUtil.SendMessageTutorialClear(Tutorial.EventID.ENEMY);
		if (StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.ENEMY_COUNT_BOMB))
		{
			GameObjectUtil.SendMessageToTagObjects("StageManager", "OnChaoAbilityEnemyBreak", null, SendMessageOptions.DontRequireReceiver);
		}
		this.SetBroken();
	}

	private void SetBroken()
	{
		if (this.m_destroyFlag)
		{
			return;
		}
		this.PlayHitEffect();
		ObjUtil.LightPlaySE(ObjEnemyUtil.GetSEName(this.m_enmyType), "SE");
		this.m_destroyFlag = true;
	}
}
