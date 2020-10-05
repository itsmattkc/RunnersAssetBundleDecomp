using Message;
using Player;
using System;
using UnityEngine;

public class CharacterContainer : MonoBehaviour
{
	private enum Type
	{
		MAIN,
		SUB
	}

	private int m_numCurrent;

	private bool[] m_recovered = new bool[2];

	private bool m_btnCharaChange;

	private bool m_enableChange;

	private bool m_requestChange;

	private GameObject[] m_character;

	private PlayerInformation m_playerInformation;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_requestChange)
		{
			int current = (this.m_numCurrent != 0) ? 0 : 1;
			this.ChangeCurrentCharacter(current, !this.m_enableChange);
			MsgChangeCharaSucceed msgChangeCharaSucceed = new MsgChangeCharaSucceed();
			msgChangeCharaSucceed.m_disabled = !this.m_enableChange;
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnChangeCharaSucceed", msgChangeCharaSucceed, SendMessageOptions.DontRequireReceiver);
			GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnChangeCharaSucceed", msgChangeCharaSucceed, SendMessageOptions.DontRequireReceiver);
			GameObjectUtil.SendMessageFindGameObject("StageComboManager", "OnChangeCharaSucceed", msgChangeCharaSucceed, SendMessageOptions.DontRequireReceiver);
			GameObjectUtil.SendDelayedMessageToTagObjects("Boss", "OnChangeCharaSucceed", msgChangeCharaSucceed);
			if (StageItemManager.Instance != null)
			{
				StageItemManager.Instance.OnChangeCharaStart(msgChangeCharaSucceed);
			}
			GameObjectUtil.SendDelayedMessageFindGameObject("StageItemManager", "OnChangeCharaSucceed", msgChangeCharaSucceed);
			this.m_requestChange = false;
		}
	}

	public void Init()
	{
		this.m_playerInformation = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
		this.m_character = new GameObject[2];
		this.m_character[0] = GameObjectUtil.FindChildGameObject(base.gameObject, "PlayerCharacterMain");
		this.m_character[1] = GameObjectUtil.FindChildGameObject(base.gameObject, "PlayerCharacterSub");
		if (this.m_playerInformation != null && this.m_playerInformation.SubCharacterName == null)
		{
			this.m_character[1] = null;
		}
		this.m_enableChange = (this.m_character[1] != null);
		this.m_requestChange = false;
		for (int i = 0; i < 2; i++)
		{
			this.m_recovered[i] = false;
		}
		this.m_btnCharaChange = false;
		MsgChangeCharaEnable value = new MsgChangeCharaEnable(this.m_enableChange);
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnChangeCharaEnable", value, SendMessageOptions.DontRequireReceiver);
		this.m_numCurrent = 0;
	}

	public void SetupCharacter()
	{
		int num = 0;
		GameObject[] character = this.m_character;
		for (int i = 0; i < character.Length; i++)
		{
			GameObject gameObject = character[i];
			if (gameObject != null)
			{
				CharacterState component = gameObject.GetComponent<CharacterState>();
				if (component != null)
				{
					component.SetPlayingType((PlayingCharacterType)num);
					component.SetupModelsAndParameter();
				}
			}
			num++;
		}
	}

	private GameObject GetCurrentCharacter()
	{
		return this.m_character[this.m_numCurrent];
	}

	private GameObject GetNonCurrentCharacter()
	{
		int num = (this.m_numCurrent != 0) ? 0 : 1;
		return this.m_character[num];
	}

	private void OnMsgChangeChara(MsgChangeChara msg)
	{
		GameObject currentCharacter = this.GetCurrentCharacter();
		if (currentCharacter != null && this.m_enableChange && !this.m_requestChange)
		{
			if (!currentCharacter.GetComponent<CharacterState>().IsEnableCharaChange(msg.m_changeByMiss))
			{
				return;
			}
			this.m_requestChange = true;
			if (msg.m_changeByMiss)
			{
				this.m_enableChange = false;
			}
			if (msg.m_changeByBtn)
			{
				this.m_btnCharaChange = true;
			}
			msg.m_succeed = true;
		}
	}

	private void ChangeCurrentCharacter(int current, bool dead)
	{
		GameObject currentCharacter = this.GetCurrentCharacter();
		this.m_numCurrent = current;
		for (int i = 0; i < this.m_character.Length; i++)
		{
			if (i != this.m_numCurrent)
			{
				this.DeactiveCharacter(i);
			}
		}
		for (int j = 0; j < this.m_character.Length; j++)
		{
			if (j == this.m_numCurrent)
			{
				this.ActivateCharacter(j, currentCharacter.transform.position, currentCharacter.transform.rotation, dead, true);
			}
		}
		if (dead)
		{
			MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.STOP_END);
		}
	}

	private void ActivateCharacter(int numPlayer, Vector3 plrPos, Quaternion plrRot, bool dead, bool trampoline)
	{
		Vector3 vector = plrPos;
		Vector3 sideViewPathPos = this.m_playerInformation.SideViewPathPos;
		Vector3 sideViewPathNormal = this.m_playerInformation.SideViewPathNormal;
		Vector3 lhs = plrPos - sideViewPathPos;
		if (Vector3.Dot(lhs, sideViewPathNormal) < 0f)
		{
			plrPos = sideViewPathPos + Vector3.up * 1f;
			plrRot = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		}
		global::Debug.Log(string.Concat(new object[]
		{
			"CharaChange Diactive POS: ",
			vector.x,
			" ",
			vector.y,
			" ",
			vector.z
		}));
		global::Debug.Log(string.Concat(new object[]
		{
			"CharaChange Active   POS: ",
			plrPos.x,
			" ",
			plrPos.y,
			" ",
			plrPos.z
		}));
		this.m_character[numPlayer].GetComponent<CharacterState>().ActiveCharacter(dead, dead, plrPos, plrRot);
		this.m_character[numPlayer].SetActive(true);
		if (trampoline)
		{
			ObjUtil.SendMessageAppearTrampoline();
		}
		if (!this.m_btnCharaChange)
		{
			ObjUtil.SendMessageOnObjectDead();
		}
		this.m_btnCharaChange = false;
	}

	private void DeactiveCharacter(int numPlayer)
	{
		this.m_character[numPlayer].GetComponent<CharacterState>().DeactiveCharacter();
		this.m_character[numPlayer].SetActive(false);
	}

	private bool IsNowLastChance(int numPlayer)
	{
		return this.m_character[numPlayer].GetComponent<CharacterState>().IsNowLastChance();
	}

	private void SetResetCharacterLastChance(int numPlayer)
	{
		this.m_character[numPlayer].GetComponent<CharacterState>().SetLastChance(false);
		if (this.m_playerInformation != null)
		{
			this.m_playerInformation.SetLastChance(false);
		}
	}

	private void SetResetCharacterDead()
	{
		if (this.m_playerInformation != null)
		{
			this.m_playerInformation.SetDead(false);
		}
	}

	public bool IsEnableChangeCharacter()
	{
		return this.m_enableChange;
	}

	public bool IsEnableRecovery()
	{
		return !this.m_recovered[this.m_numCurrent] && StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.RECOVERY_FROM_FAILURE);
	}

	public void PrepareRecovery(bool bossStage)
	{
		this.m_requestChange = false;
		this.m_recovered[this.m_numCurrent] = true;
		this.DeactiveCharacter(this.m_numCurrent);
		this.SetResetCharacterDead();
		this.ActivateCharacter(this.m_numCurrent, this.m_playerInformation.Position, this.m_playerInformation.Rotation, true, false);
		ObjUtil.SendMessageAppearTrampoline();
		MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.STOP_END);
		if (bossStage)
		{
			MsgPrepareContinue value = new MsgPrepareContinue(bossStage, false);
			GameObjectUtil.SendMessageToTagObjects("Boss", "OnMsgPrepareContinue", value, SendMessageOptions.DontRequireReceiver);
		}
		ObjUtil.RequestStartAbilityToChao(ChaoAbility.RECOVERY_FROM_FAILURE, false);
	}

	private void OnMsgPrepareContinue(MsgPrepareContinue msg)
	{
		bool flag = this.IsNowLastChance(this.m_numCurrent);
		ItemType itemType = ItemType.UNKNOWN;
		if (StageItemManager.Instance != null)
		{
			itemType = StageItemManager.Instance.GetPhantomItemType();
		}
		bool flag2 = msg.m_timeUp && itemType != ItemType.UNKNOWN && flag;
		this.m_enableChange = (this.m_character[1] != null);
		this.m_requestChange = false;
		for (int i = 0; i < 2; i++)
		{
			this.m_recovered[i] = false;
		}
		if (!flag2)
		{
			this.DeactiveCharacter(this.m_numCurrent);
		}
		this.SetResetCharacterLastChance(this.m_numCurrent);
		this.SetResetCharacterDead();
		if (msg.m_timeUp)
		{
			if (flag)
			{
				MsgEndLastChance value = new MsgEndLastChance();
				GameObjectUtil.SendMessageToTagObjects("Chao", "OnEndLastChance", value, SendMessageOptions.DontRequireReceiver);
				if (this.m_enableChange && this.m_numCurrent == 1)
				{
					this.m_numCurrent = 0;
				}
			}
		}
		else if (this.m_enableChange && this.m_numCurrent == 1)
		{
			this.m_numCurrent = 0;
		}
		if (flag2)
		{
			ObjUtil.SendMessageOnObjectDead();
		}
		else
		{
			this.ActivateCharacter(this.m_numCurrent, this.m_playerInformation.Position, this.m_playerInformation.Rotation, true, false);
		}
		bool flag3 = false;
		if (EventManager.Instance != null)
		{
			flag3 = EventManager.Instance.IsRaidBossStage();
		}
		StageScoreManager instance = StageScoreManager.Instance;
		if (instance != null)
		{
			ObjUtil.SendMessageTransferRingForContinue((!flag3) ? instance.ContinueRing : instance.ContinueRaidBossRing);
		}
		int numRings = this.m_playerInformation.NumRings;
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnAddStockRing", new MsgAddStockRing(numRings), SendMessageOptions.DontRequireReceiver);
		if (!msg.m_bossStage)
		{
			this.SendAddItem(ItemType.BARRIER);
			this.SendAddItem(ItemType.MAGNET);
			this.SendAddItem(ItemType.COMBO);
			this.SendAddDamageTrampoline();
			if (itemType == ItemType.UNKNOWN)
			{
				itemType = StageItemManager.GetRandomPhantomItem();
			}
			if (!this.SendAddItem(itemType))
			{
				this.SendAddColorItem(itemType);
				MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.STOP_END);
			}
		}
		else
		{
			this.SendAddItem(ItemType.INVINCIBLE);
			this.SendAddItem(ItemType.BARRIER);
			this.SendAddItem(ItemType.MAGNET);
			MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.STOP_END);
		}
		if (this.m_enableChange)
		{
			MsgChangeCharaEnable value2 = new MsgChangeCharaEnable(true);
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnChangeCharaEnable", value2, SendMessageOptions.DontRequireReceiver);
			MsgChangeCharaSucceed value3 = new MsgChangeCharaSucceed();
			GameObjectUtil.SendMessageFindGameObject("StageComboManager", "OnChangeCharaSucceed", value3, SendMessageOptions.DontRequireReceiver);
		}
	}

	private bool SendAddItem(ItemType itemType)
	{
		if (StageItemManager.Instance != null)
		{
			MsgAskEquipItemUsed msgAskEquipItemUsed = new MsgAskEquipItemUsed(itemType);
			StageItemManager.Instance.OnAskEquipItemUsed(msgAskEquipItemUsed);
			if (msgAskEquipItemUsed.m_ok)
			{
				StageItemManager.Instance.OnAddItem(new MsgAddItemToManager(itemType));
				return true;
			}
		}
		return false;
	}

	private void SendAddColorItem(ItemType itemType)
	{
		if (StageItemManager.Instance != null)
		{
			StageItemManager.Instance.OnAddColorItem(new MsgAddItemToManager(itemType));
		}
	}

	private void SendAddDamageTrampoline()
	{
		if (StageItemManager.Instance != null)
		{
			StageItemManager.Instance.OnAddDamageTrampoline();
		}
	}
}
