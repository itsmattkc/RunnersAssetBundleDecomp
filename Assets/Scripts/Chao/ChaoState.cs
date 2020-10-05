using App.Utility;
using DataTable;
using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Chao
{
	public class ChaoState : MonoBehaviour
	{
		private enum EventSignal
		{
			ENTER_TRIGGER = 100
		}

		private abstract class StateWork
		{
			private string name;

			public string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					this.name = value;
				}
			}

			public abstract void Destroy();
		}

		private enum FlagState
		{
			StatesStop,
			ReqestStop,
			RreviveProduct
		}

		private enum SubStateKillOut
		{
			Up,
			Forward,
			Wait
		}

		private enum AttackBossSubState
		{
			Up,
			Attack,
			After
		}

		private class AttackBossWork : ChaoState.StateWork
		{
			public GameObject m_attackCollision;

			public GameObject m_targetObject;

			public void DestroyAttackCollision()
			{
				if (this.m_attackCollision != null)
				{
					UnityEngine.Object.Destroy(this.m_attackCollision);
					this.m_attackCollision = null;
				}
			}

			public void Destroy()
			{
				this.DestroyAttackCollision();
				this.m_targetObject = null;
			}
		}

		private enum ChaoWalkerState
		{
			Action,
			Wait,
			SubAction,
			Out
		}

		private enum SubStateItemPresent
		{
			Action,
			Wait,
			SubAction,
			Out
		}

		private enum StockRingState
		{
			IDLE,
			CHANGE_MOVEMENT,
			PLAY_SPIN_MOTION,
			PLAY_EFECT,
			PLAY_EFFECT,
			WAIT_END
		}

		private sealed class _SetAnimationFlagForAbilityCourutine_c__Iterator9 : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal Animator _animator___0;

			internal bool value;

			internal int _wait___1;

			internal int _PC;

			internal object _current;

			internal bool ___value;

			internal ChaoState __f__this;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					if (this.__f__this.m_fsmBehavior.NowShutDown)
					{
						return false;
					}
					if (!(this.__f__this.m_modelObject != null))
					{
						goto IL_104;
					}
					this._animator___0 = this.__f__this.m_modelObject.GetComponent<Animator>();
					if (!(this._animator___0 != null))
					{
						goto IL_104;
					}
					this._animator___0.SetBool("Ability", this.value);
					if (!this.value)
					{
						goto IL_104;
					}
					this._wait___1 = 2;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				if (this._wait___1 > 0)
				{
					this._wait___1--;
					this._current = null;
					this._PC = 1;
					return true;
				}
				if (this._animator___0 != null && this._animator___0.enabled)
				{
					this._animator___0.SetBool("Ability", false);
				}
				IL_104:
				this._PC = -1;
				return false;
			}

			public void Dispose()
			{
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private sealed class _RingBank_c__IteratorA : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal int _waite_frame___0;

			internal int _stockRing___1;

			internal int _PC;

			internal object _current;

			internal ChaoState __f__this;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					ObjUtil.PlayChaoEffect(this.__f__this.gameObject, "ef_ch_combo_bank_r01", -1f);
					ObjUtil.LightPlaySE("act_chao_effect", "SE");
					this._waite_frame___0 = 1;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_100;
				default:
					return false;
				}
				if (this._waite_frame___0 > 0)
				{
					this._waite_frame___0--;
					this._current = null;
					this._PC = 1;
					return true;
				}
				this._stockRing___1 = 0;
				if (StageAbilityManager.Instance != null)
				{
					this._stockRing___1 = (int)StageAbilityManager.Instance.GetChaoAbilityExtraValue(ChaoAbility.COMBO_RING_BANK);
					if (StageScoreManager.Instance != null)
					{
						StageScoreManager.Instance.TransferRingForChaoAbility(this._stockRing___1);
					}
				}
				this._waite_frame___0 = 2;
				IL_100:
				if (this._waite_frame___0 > 0)
				{
					this._waite_frame___0--;
					this._current = null;
					this._PC = 2;
					return true;
				}
				GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnAddStockRing", new MsgAddStockRing(this._stockRing___1), SendMessageOptions.DontRequireReceiver);
				this._PC = -1;
				return false;
			}

			public void Dispose()
			{
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private const float ShaderLineOffsetMain = 5f;

		private const float ShaderLineOffsetSub = 15f;

		private const string ShaderName = "ykChrLine_dme1";

		private const string ChangeParamOutline = "_OutlineZOffset";

		private const string ChangeParamInnner = "_InnerZOffset";

		private const float InvalidExtremeSpeedRate = 1.5f;

		private const float EasySpeed_SpeedRate = 3f;

		private const float PhantomStartSpeedRate = 5f;

		private const string StockRingEffectName = "ef_ch_tornade_fog_";

		private const string StockRingEffectPost_SR = "sr01";

		private const string StockRingEffectPost_R = "r01";

		private const string StockRingEffectPost_N = "c01";

		private const int CHAOS_ID = 2014;

		private const int DEATH_EGG_ID = 2015;

		private const int PUYO_CARBUNCLE_ID = 2012;

		private const int PUYO_SUKETOUDARA_ID = 1018;

		private const int PUYO_PAPURISU_ID = 24;

		private const float ChaoWalkerSpeedRate = 2f;

		private const float m_hardwareAndCartridgeMoveTimer = 0.8f;

		private const float m_hardwareAndCartridgeWaitTimer = 2f;

		private const float HardwareCartridgeSpeedRate = 2f;

		private const float m_nightsMoveTimer = 1.5f;

		private const float m_nightsWaitTimer = 2f;

		private const float NightsSpeedRate = 2f;

		private const float ChaosMoveTimer = 1f;

		private const float ChaosWaitTimer = 2.2f;

		private const float ChaosSpeedRate = 2f;

		private const float PufferFishMoveTimer = 1f;

		private const float PufferFishWaitTimer = 0.5f;

		private const float PufferFishSubActionTimer = 1f;

		private const float PufferFishOutTimer = 0.5f;

		private const float PufferFishSpeedRate = 2.5f;

		private const float ItemPresentSpeedRate = 2f;

		private const float ItemPresentOutSpeedRate = 2f;

		private Bitset32 m_stateFlag;

		private TinyFsmBehavior m_fsmBehavior;

		private ChaoMovement m_movement;

		private ChaoType m_chao_type;

		private int m_chao_id = -1;

		private bool m_startEffect;

		private ChaoSetupParameterData m_setupdata = new ChaoSetupParameterData();

		private ChaoState.StateWork m_stateWork;

		private GameObject m_modelObject;

		private ChaoParameter m_parameter;

		private ChaoModelPostureController m_modelControl;

		private int m_attackCount;

		private int m_substate;

		private float m_stateTimer;

		private ItemType m_chaoItemType = ItemType.UNKNOWN;

		private static readonly Vector3 InvalidExtremeOffsetRate = new Vector3(0.5f, 0.7f, 0f);

		private static readonly ItemType[] ChaoAbilityItemTbl = new ItemType[]
		{
			ItemType.COMBO,
			ItemType.MAGNET,
			ItemType.INVINCIBLE,
			ItemType.BARRIER,
			ItemType.TRAMPOLINE
		};

		private static readonly ItemType[] ChaoAbilityItemTblPhantom = new ItemType[]
		{
			ItemType.COMBO,
			ItemType.MAGNET
		};

		private static readonly ItemType[] ChaoAbilityPhantomTbl = new ItemType[]
		{
			ItemType.LASER,
			ItemType.DRILL,
			ItemType.ASTEROID
		};

		private static readonly Vector3 AttackBossModelSpinSpeed = new Vector3(1440f, 0f, 0f);

		private ChaoState.ChaoWalkerState m_chaoWalkerState;

		private static readonly Vector3 ChaoWalkerOffsetRate = new Vector3(0.5f, 0.7f, 0f);

		private static readonly Vector3 HardwareCartridgeOffsetRate = new Vector3(0.5f, 0.5f, 0f);

		private static readonly Vector3 NightsOffsetRate = new Vector3(0.66f, 0.6f, 0f);

		private static readonly Vector3 RealaOffsetRate = new Vector3(0.33f, 0.6f, 0f);

		private static readonly Vector3 ChaosOffsetRate = new Vector3(0.5f, 0.5f, 0f);

		private static readonly Vector3 PufferFishOffsetRate = new Vector3(0.5f, 0.75f, 0f);

		private static readonly Vector3 ItemPresentOffsetRate = new Vector3(0.5f, 0.7f, 0f);

		private static readonly Vector3 CuebotItemOffsetRate = new Vector3(0.5f, 0.6f, 0f);

		private static readonly Vector3 OrbotItemtOffsetRate = new Vector3(0.5f, 0.78f, 0f);

		private Camera m_uiCamera;

		private GameObject m_effectObj;

		private GameObject m_itemBtnObj;

		private Vector3 m_effectScreenPos = Vector3.zero;

		private Vector3 m_targetScreenPos = Vector3.zero;

		private float m_distance;

		private bool m_presentFlag;

		private ChaoState.StockRingState m_stockRingState;

		private static readonly Vector3 StockRingModelSpinSpeed = new Vector3(0f, 0f, 360f);

		public GameObject ModelObject
		{
			get
			{
				return this.m_modelObject;
			}
		}

		public ChaoParameterData Parameter
		{
			get
			{
				if (this.m_parameter != null)
				{
					return this.m_parameter.Data;
				}
				return null;
			}
		}

		public ShaderType ShaderOffset
		{
			get
			{
				if (this.m_setupdata != null)
				{
					return this.m_setupdata.ShaderOffset;
				}
				return ShaderType.NORMAL;
			}
		}

		private void CreateCollider()
		{
			base.gameObject.AddComponent(typeof(SphereCollider));
			SphereCollider component = base.gameObject.GetComponent<SphereCollider>();
			if (component != null)
			{
				component.isTrigger = true;
				Vector3 colliCenter = this.m_setupdata.ColliCenter;
				component.center = colliCenter;
				component.radius = this.m_setupdata.ColliRadius;
			}
		}

		private void SetupModelAndParameter()
		{
			ResourceManager instance = ResourceManager.Instance;
			if (instance != null)
			{
				string name = null;
				GameObject gameObject = GameObjectUtil.FindChildGameObject(instance.gameObject, "ChaoModel" + this.m_chao_id.ToString("0000"));
				if (gameObject != null)
				{
					int childCount = gameObject.transform.childCount;
					for (int i = 0; i < childCount; i++)
					{
						GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
						if (gameObject2.name.IndexOf("cho_") == 0)
						{
							name = gameObject2.name;
						}
					}
					ChaoSetupParameter component = gameObject.GetComponent<ChaoSetupParameter>();
					if (component != null)
					{
						component.Data.CopyTo(this.m_setupdata);
					}
				}
				GameObject gameObject3 = instance.GetGameObject(ResourceCategory.CHAO_MODEL, name);
				this.CreateChildModelObject(gameObject3, true);
			}
		}

		private void SetupModelPostureController(GameObject modelObject)
		{
			this.m_modelControl = modelObject.AddComponent<ChaoModelPostureController>();
			this.m_modelControl.SetModelObject(modelObject);
		}

		private void CreateChildModelObject(GameObject src_obj, bool active)
		{
			if (src_obj != null)
			{
				Vector3 localPosition = src_obj.transform.localPosition;
				Quaternion localRotation = src_obj.transform.localRotation;
				GameObject gameObject = UnityEngine.Object.Instantiate(src_obj, localPosition, localRotation) as GameObject;
				if (gameObject != null)
				{
					gameObject.transform.parent = base.transform;
					gameObject.SetActive(active);
					gameObject.transform.localPosition = localPosition;
					gameObject.transform.localRotation = localRotation;
					gameObject.name = src_obj.name;
					this.OffAnimatorRootAnimation(gameObject);
					this.m_modelObject = gameObject;
					this.SetupModelPostureController(this.m_modelObject);
					float shaderOffsetValue = this.GetShaderOffsetValue();
					this.ChangeShaderOffsetChild(this.m_modelObject, shaderOffsetValue);
					ChaoPartsObjectMagnet component = gameObject.GetComponent<ChaoPartsObjectMagnet>();
					if (component != null)
					{
						component.Setup();
					}
				}
			}
		}

		private void OffAnimatorRootAnimation(GameObject modelObject)
		{
			if (modelObject != null)
			{
				Animator component = modelObject.GetComponent<Animator>();
				if (component != null)
				{
					component.applyRootMotion = false;
				}
			}
		}

		private float GetShaderOffsetValue()
		{
			ChaoType chaoType = ChaoUtility.GetChaoType(base.gameObject);
			float result = (chaoType != ChaoType.MAIN) ? 15f : 5f;
			if (this.m_setupdata.ShaderOffset != ShaderType.NORMAL)
			{
				ChaoType type = (chaoType != ChaoType.MAIN) ? ChaoType.MAIN : ChaoType.SUB;
				if (this.m_setupdata.ShaderOffset == ShaderType.MAIN)
				{
					if (ChaoUtility.GetChaoShaderType(base.gameObject.transform.parent.gameObject, type) == ShaderType.SUB)
					{
						result = 5f;
					}
				}
				else if (this.m_setupdata.ShaderOffset == ShaderType.SUB && ChaoUtility.GetChaoShaderType(base.gameObject.transform.parent.gameObject, type) == ShaderType.MAIN)
				{
					result = 15f;
				}
			}
			return result;
		}

		private void ChangeShaderOffsetChild(GameObject parent, float offset)
		{
			foreach (Transform transform in parent.transform)
			{
				this.ChangeShaderOffsetChild(transform.gameObject, offset);
				Renderer component = transform.GetComponent<Renderer>();
				if (component != null)
				{
					Material[] materials = component.materials;
					Material[] array = materials;
					for (int i = 0; i < array.Length; i++)
					{
						Material material = array[i];
						if (material.HasProperty("_OutlineZOffset"))
						{
							float @float = material.GetFloat("_OutlineZOffset");
							material.SetFloat("_OutlineZOffset", @float + offset);
						}
						if (material.HasProperty("_InnerZOffset"))
						{
							float float2 = material.GetFloat("_InnerZOffset");
							material.SetFloat("_InnerZOffset", float2 + offset);
						}
					}
				}
			}
		}

		public void Start()
		{
			this.SetChaoData();
			if (this.m_chao_id < 0)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.SetupModelAndParameter();
			this.CreateTinyFsm();
			this.SetChaoMovement();
			base.enabled = false;
		}

		private void OnDestroy()
		{
			if (this.m_fsmBehavior != null)
			{
				this.m_fsmBehavior.ShutDown();
				this.m_fsmBehavior = null;
			}
			this.DestroyStateWork();
		}

		private void CreateTinyFsm()
		{
			this.m_fsmBehavior = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
			if (this.m_fsmBehavior != null)
			{
				TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
				description.initState = new TinyFsmState(new EventFunction(this.StateInit));
				this.m_fsmBehavior.SetUp(description);
			}
		}

		private StageItemManager GetItemManager()
		{
			if (StageItemManager.Instance != null)
			{
				return StageItemManager.Instance;
			}
			return null;
		}

		public T GetCurrentMovement<T>() where T : FSMState<ChaoMovement>
		{
			return this.m_movement.GetCurrentState<T>();
		}

		private void SetChaoData()
		{
			this.m_chao_type = ChaoUtility.GetChaoType(base.gameObject);
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				ChaoType chao_type = this.m_chao_type;
				if (chao_type != ChaoType.MAIN)
				{
					if (chao_type == ChaoType.SUB)
					{
						this.m_chao_id = instance.PlayerData.SubChaoID;
					}
				}
				else
				{
					this.m_chao_id = instance.PlayerData.MainChaoID;
				}
			}
			GameObject gameObject = GameObject.Find("StageChao/ChaoParameter");
			if (gameObject != null)
			{
				this.m_parameter = gameObject.GetComponent<ChaoParameter>();
			}
		}

		private static ChaoData.Rarity GetRarity(int chaoId)
		{
			ChaoData chaoData = ChaoTable.GetChaoData(chaoId);
			if (chaoData != null)
			{
				return chaoData.rarity;
			}
			return ChaoData.Rarity.NONE;
		}

		private void RequestStartEffect()
		{
			if (!this.m_startEffect)
			{
				this.m_startEffect = true;
				StageAbilityManager instance = StageAbilityManager.Instance;
				if (instance != null)
				{
					instance.RequestPlayChaoEffect(ChaoAbility.ALL_BONUS_COUNT, this.m_chao_type);
					instance.RequestPlayChaoEffect(ChaoAbility.SCORE_COUNT, this.m_chao_type);
					instance.RequestPlayChaoEffect(ChaoAbility.RING_COUNT, this.m_chao_type);
					instance.RequestPlayChaoEffect(ChaoAbility.RED_RING_COUNT, this.m_chao_type);
					instance.RequestPlayChaoEffect(ChaoAbility.ANIMAL_COUNT, this.m_chao_type);
					instance.RequestPlayChaoEffect(ChaoAbility.RUNNIGN_DISTANCE, this.m_chao_type);
					instance.RequestPlayChaoEffect(ChaoAbility.RARE_ENEMY_UP, this.m_chao_type);
					instance.RequestPlayChaoEffect(ChaoAbility.ENEMY_SCORE, this.m_chao_type);
					instance.RequestPlayChaoEffect(ChaoAbility.COMBO_RECEPTION_TIME, this.m_chao_type);
				}
			}
		}

		private void SetChaoMovement()
		{
			this.m_movement = ChaoMovement.Create(base.gameObject, this.m_setupdata);
		}

		private void ChangeState(TinyFsmState nextState)
		{
			if (this.m_fsmBehavior != null)
			{
				this.m_fsmBehavior.ChangeState(nextState);
			}
		}

		private void ChangeMovement(MOVESTATE_ID state)
		{
			if (this.m_movement != null)
			{
				this.m_movement.ChangeState(state);
			}
		}

		private TinyFsmState GetCurrentState()
		{
			if (this.m_fsmBehavior != null)
			{
				return this.m_fsmBehavior.GetCurrentState();
			}
			return null;
		}

		private void OnMsgReceive(MsgChaoState message)
		{
			if (message != null && this.m_fsmBehavior != null)
			{
				TinyFsmEvent signal = TinyFsmEvent.CreateMessage(message);
				this.m_fsmBehavior.Dispatch(signal);
			}
		}

		private void OnMsgStageReplace(MsgStageReplace msg)
		{
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
		}

		private void OnMsgStartBoss()
		{
			this.m_attackCount = 0;
		}

		private void OnStartLastChance(MsgStartLastChance message)
		{
			StageAbilityManager instance = StageAbilityManager.Instance;
			if (instance != null && instance.HasChaoAbility(ChaoAbility.LAST_CHANCE, this.m_chao_type) && message != null && this.m_fsmBehavior != null)
			{
				MsgChaoState msg = new MsgChaoState(MsgChaoState.State.LAST_CHANCE);
				TinyFsmEvent signal = TinyFsmEvent.CreateMessage(msg);
				this.m_fsmBehavior.Dispatch(signal);
			}
		}

		private void OnEndLastChance(MsgEndLastChance message)
		{
			if (message != null && this.m_fsmBehavior != null)
			{
				MsgChaoState msg = new MsgChaoState(MsgChaoState.State.LAST_CHANCE_END);
				TinyFsmEvent signal = TinyFsmEvent.CreateMessage(msg);
				this.m_fsmBehavior.Dispatch(signal);
			}
		}

		private void OnPauseChangeLevel()
		{
			this.SetMagnetPause(true);
		}

		private void OnResumeChangeLevel()
		{
			this.SetMagnetPause(false);
		}

		private void OnMsgChaoAbilityStart(MsgChaoAbilityStart msg)
		{
			StageAbilityManager instance = StageAbilityManager.Instance;
			ChaoAbility[] ability = msg.m_ability;
			for (int i = 0; i < ability.Length; i++)
			{
				ChaoAbility ability2 = ability[i];
				if (instance != null && instance.HasChaoAbility(ability2, this.m_chao_type))
				{
					switch (ability2)
					{
					case ChaoAbility.ANIMAL_COUNT:
						if (this.m_chao_id == 2014)
						{
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_sp4_deatheggstar_flash_sr01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						break;
					case ChaoAbility.COMBO_ITEM_BOX:
						if (!this.IsNowLastChance())
						{
							if (this.m_stateFlag.Test(0))
							{
								this.m_stateFlag.Set(1, true);
							}
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateItemPresent)));
						}
						break;
					case ChaoAbility.COMBO_BARRIER:
						this.m_chaoItemType = ItemType.BARRIER;
						if (this.CheckItemPresent(this.m_chaoItemType))
						{
							PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
							if (playerInformation != null)
							{
								if (playerInformation.PhantomType == PhantomType.NONE)
								{
									ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_sp2_erazordjinn_magic_sr01", -1f);
									base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
								}
								StageItemManager itemManager = this.GetItemManager();
								if (itemManager != null)
								{
									MsgAddItemToManager value = new MsgAddItemToManager(this.m_chaoItemType);
									itemManager.SendMessage("OnAddItem", value, SendMessageOptions.DontRequireReceiver);
								}
							}
						}
						break;
					case ChaoAbility.COMBO_WIPE_OUT_ENEMY:
						if (this.m_stateFlag.Test(0))
						{
							this.m_stateFlag.Set(1, true);
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateKillOut)));
						break;
					case ChaoAbility.COMBO_COLOR_POWER:
						if (this.m_stateFlag.Test(0))
						{
							this.m_stateFlag.Set(1, true);
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StatePhantomPresent)));
						break;
					case ChaoAbility.COMBO_ALL_SPECIAL_CRYSTAL:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						if (this.m_chao_id == 1018)
						{
							ObjUtil.LightPlaySE("act_chao_puyo", "SE");
						}
						break;
					case ChaoAbility.COMBO_DESTROY_TRAP:
						ObjUtil.LightPlaySE("act_chao_mag", "SE");
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_trap_cry_c01", -1f);
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						GameObjectUtil.SendDelayedMessageToTagObjects("Gimmick", "OnMsgObjectDeadChaoCombo", new MsgObjectDead(ChaoAbility.COMBO_DESTROY_TRAP));
						break;
					case ChaoAbility.COMBO_DESTROY_AIR_TRAP:
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_brk_airtrap_c01", -1f);
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						if (this.m_chao_id == 24)
						{
							ObjUtil.LightPlaySE("act_chao_puyo", "SE");
						}
						GameObjectUtil.SendDelayedMessageToTagObjects("Gimmick", "OnMsgObjectDeadChaoCombo", new MsgObjectDead(ChaoAbility.COMBO_DESTROY_AIR_TRAP));
						break;
					case ChaoAbility.COMBO_RECOVERY_ALL_OBJ:
						if (this.m_chao_id == 2012)
						{
							ObjUtil.LightPlaySE("act_chao_puyo", "SE");
						}
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_sp3_carbuncle_magic_sr01", -1f);
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						break;
					case ChaoAbility.COMBO_STEP_DESTROY_GET_10_RING:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_brk_airfloor_r01", -1f);
						GameObjectUtil.SendDelayedMessageToTagObjects("Gimmick", "OnMsgStepObjectDead", new MsgObjectDead());
						ObjUtil.LightPlaySE("act_chao_killerwhale", "SE");
						break;
					case ChaoAbility.COMBO_EQUIP_ITEM:
						if (!this.IsNowLastChance())
						{
							if (this.m_stateFlag.Test(0))
							{
								this.m_stateFlag.Set(1, true);
							}
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePresentEquipItem)));
						}
						break;
					case ChaoAbility.COLOR_POWER_SCORE:
					case ChaoAbility.ASTEROID_SCORE:
					case ChaoAbility.DRILL_SCORE:
					case ChaoAbility.LASER_SCORE:
					case ChaoAbility.COLOR_POWER_TIME:
					case ChaoAbility.ASTEROID_TIME:
					case ChaoAbility.DRILL_TIME:
					case ChaoAbility.LASER_TIME:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateItemPhantom)));
						break;
					case ChaoAbility.BOSS_STAGE_TIME:
						ObjUtil.LightPlaySE("act_arthur", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						break;
					case ChaoAbility.COMBO_RECEPTION_TIME:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateGameHardware)));
						break;
					case ChaoAbility.BOSS_RED_RING_RATE:
					case ChaoAbility.BOSS_SUPER_RING_RATE:
						ObjUtil.LightPlaySE("act_ship_laser", "SE");
						break;
					case ChaoAbility.RECOVERY_RING:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateStockRing)));
						break;
					case ChaoAbility.BOSS_ATTACK:
						if ((float)this.m_attackCount < instance.GetChaoAbilityValue(ability2))
						{
							msg.m_flag = true;
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateAttackBoss)));
						}
						break;
					case ChaoAbility.CHECK_POINT_ITEM_BOX:
					{
						PlayerInformation playerInformation2 = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
						if (playerInformation2 != null)
						{
							bool flag = false;
							if (playerInformation2.PhantomType == PhantomType.NONE)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTbl[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTbl.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
							if (!flag)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
							if (flag)
							{
								if (this.m_stateFlag.Test(0))
								{
									this.m_stateFlag.Set(1, true);
								}
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateItemPresent2)));
							}
						}
						break;
					}
					case ChaoAbility.JUMP_RAMP_TRICK_SUCCESS:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_jumpboard_success_r01", -1f);
						break;
					case ChaoAbility.PURSUES_TO_BOSS_AFTER_ATTACK:
					{
						float num = UnityEngine.Random.Range(0f, 100f);
						if (num < instance.GetChaoAbilityValue(ability2))
						{
							msg.m_flag = true;
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursuesAttackBoss)));
						}
						break;
					}
					case ChaoAbility.SPECIAL_ANIMAL_PSO2:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.LightPlaySE("act_chao_rappy", "SE");
						break;
					case ChaoAbility.DAMAGE_TRAMPOLINE:
						if (ObjUtil.GetRandomRange100() < (int)instance.GetChaoAbilityValue(ability2))
						{
							this.m_chaoItemType = ItemType.TRAMPOLINE;
							PlayerInformation playerInformation3 = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
							if (playerInformation3 != null && playerInformation3.PhantomType == PhantomType.NONE)
							{
								ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_damaged_trampoline_r01", -1f);
								base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
								if (this.m_chao_id == 1018)
								{
									ObjUtil.LightPlaySE("act_chao_puyo", "SE");
								}
							}
							StageItemManager itemManager2 = this.GetItemManager();
							if (itemManager2 != null)
							{
								itemManager2.SendMessage("OnAddDamageTrampoline", null, SendMessageOptions.DontRequireReceiver);
							}
						}
						break;
					case ChaoAbility.RECOVERY_FROM_FAILURE:
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_sp4_deatheggstar_flash_sr01", -1f);
						ObjUtil.LightPlaySE("act_chao_deathegg", "SE");
						break;
					case ChaoAbility.ADD_COMBO_VALUE:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateGameCartridge)));
						break;
					case ChaoAbility.LOOP_COMBO_UP:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateNights)));
						break;
					case ChaoAbility.LOOP_MAGNET:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateReala)));
						break;
					case ChaoAbility.DAMAGE_DESTROY_ALL:
						if (ObjUtil.GetRandomRange100() < (int)instance.GetChaoAbilityValue(ability2))
						{
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_clb_pian_magic_r01", -1f);
							ObjUtil.LightPlaySE("act_chao_pian", "SE");
							MsgObjectDead value2 = new MsgObjectDead();
							GameObjectUtil.SendDelayedMessageToTagObjects("Gimmick", "OnMsgObjectDead", value2);
							GameObjectUtil.SendDelayedMessageToTagObjects("Enemy", "OnMsgObjectDead", value2);
						}
						break;
					case ChaoAbility.ITEM_REVIVE:
						if (!this.m_stateFlag.Test(2))
						{
							if (this.m_stateFlag.Test(0))
							{
								this.m_stateFlag.Set(1, true);
							}
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateReviveEquipItem)));
						}
						break;
					case ChaoAbility.TRANSFER_DOUBLE_RING:
						if (this.m_stateFlag.Test(0))
						{
							this.m_stateFlag.Set(1, true);
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateDoubleRing)));
						break;
					case ChaoAbility.CHAO_RING_MAGNET:
						if (this.m_stateFlag.Test(0))
						{
							this.m_stateFlag.Set(1, true);
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateRingMagnet)));
						break;
					case ChaoAbility.CHAO_CRYSTAL_MAGNET:
						if (this.m_stateFlag.Test(0))
						{
							this.m_stateFlag.Set(1, true);
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateCrystalMagnet)));
						break;
					case ChaoAbility.MAGNET_SPEED_TYPE_JUMP:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_typeactionS_magnet_r01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						break;
					case ChaoAbility.MAGNET_FLY_TYPE_JUMP:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_typeactionF_magnet_r01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						break;
					case ChaoAbility.MAGNET_POWER_TYPE_JUMP:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_typeactionP_magnet_r01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						break;
					case ChaoAbility.COMBO_DESTROY_AND_RECOVERY:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_brkall_magnetall_sr01", -1f);
						ObjUtil.LightPlaySE("act_chao_papaopa", "SE");
						ObjUtil.SendMessageOnObjectDead();
						GameObjectUtil.SendDelayedMessageToTagObjects("Gimmick", "OnMsgStepObjectDead", new MsgObjectDead());
						break;
					case ChaoAbility.COMBO_RING_BANK:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						base.StartCoroutine(this.RingBank());
						break;
					case ChaoAbility.ENEMY_COUNT_BOMB:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_enemy_brk_enemy_r01", -1f);
						ObjUtil.LightPlaySE("act_chao_heavybomb", "SE");
						if (StageAbilityManager.Instance != null)
						{
							float chaoAbilityExtraValue = StageAbilityManager.Instance.GetChaoAbilityExtraValue(ChaoAbility.ENEMY_COUNT_BOMB);
							GameObjectUtil.SendDelayedMessageToTagObjects("Enemy", "OnMsgHeavyBombDead", chaoAbilityExtraValue);
						}
						break;
					case ChaoAbility.ENEMY_SCORE_SEVERALFOLD:
						if (!this.IsPlayingAbilityAnimation())
						{
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_sp6_kingboombo_magic_sr01", -1f);
							ObjUtil.LightPlaySE("act_chao_effect", "SE");
						}
						break;
					case ChaoAbility.COMBO_METAL_AND_METAL_SCORE:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_enemy_m_r01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						break;
					case ChaoAbility.GUARD_DROP_RING:
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_damaged_keepring_r01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						break;
					case ChaoAbility.INVALIDI_EXTREME_STAGE:
						if (this.m_stateFlag.Test(0))
						{
							this.m_stateFlag.Set(1, true);
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateInvalidExtreme)));
						break;
					case ChaoAbility.COMBO_EQUIP_ITEM_EXTRA:
						if (!this.IsNowLastChance())
						{
							if (this.m_stateFlag.Test(0))
							{
								this.m_stateFlag.Set(1, true);
							}
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePresentSRareEquipItem)));
						}
						break;
					case ChaoAbility.EASY_SPEED:
						if (this.m_stateFlag.Test(0))
						{
							this.m_stateFlag.Set(1, true);
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateEasySpeed)));
						break;
					case ChaoAbility.COMBO_CHANGE_EQUIP_ITEM:
					{
						StageItemManager itemManager3 = this.GetItemManager();
						if (itemManager3 != null && itemManager3.IsEquipItem())
						{
							if (this.m_stateFlag.Test(0))
							{
								this.m_stateFlag.Set(1, true);
							}
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateChangeEquipItem)));
						}
						break;
					}
					case ChaoAbility.COMBO_COLOR_POWER_DRILL:
					{
						StageItemManager itemManager4 = this.GetItemManager();
						if (itemManager4 != null)
						{
							this.m_chaoItemType = ItemType.DRILL;
							itemManager4.SendMessage("OnAddColorItem", new MsgAddItemToManager(this.m_chaoItemType), SendMessageOptions.DontRequireReceiver);
							ObjUtil.LightPlaySE("act_chao_effect", "SE");
							ObjUtil.SendGetItemIcon(this.m_chaoItemType);
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_drill_r01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						break;
					}
					case ChaoAbility.COMBO_BONUS_UP:
						if (this.m_stateFlag.Test(0))
						{
							this.m_stateFlag.Set(1, true);
						}
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateComboBonusUp)));
						break;
					case ChaoAbility.COMBO_COLOR_POWER_LASER:
					{
						StageItemManager itemManager5 = this.GetItemManager();
						if (itemManager5 != null)
						{
							this.m_chaoItemType = ItemType.LASER;
							itemManager5.SendMessage("OnAddColorItem", new MsgAddItemToManager(this.m_chaoItemType), SendMessageOptions.DontRequireReceiver);
							ObjUtil.LightPlaySE("act_chao_effect", "SE");
							ObjUtil.SendGetItemIcon(this.m_chaoItemType);
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_laser_r01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						break;
					}
					case ChaoAbility.COMBO_COLOR_POWER_ASTEROID:
					{
						StageItemManager itemManager6 = this.GetItemManager();
						if (itemManager6 != null)
						{
							this.m_chaoItemType = ItemType.ASTEROID;
							itemManager6.SendMessage("OnAddColorItem", new MsgAddItemToManager(this.m_chaoItemType), SendMessageOptions.DontRequireReceiver);
							ObjUtil.LightPlaySE("act_chao_effect", "SE");
							ObjUtil.SendGetItemIcon(this.m_chaoItemType);
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_asteroid_r01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						break;
					}
					case ChaoAbility.COMBO_RANDOM_ITEM_MINUS_RING:
						if (!this.IsNowLastChance() && StageAbilityManager.Instance != null && StageScoreManager.Instance != null)
						{
							long costRing = (long)StageAbilityManager.Instance.GetChaoAbilityExtraValue(ChaoAbility.COMBO_RANDOM_ITEM_MINUS_RING);
							if (StageScoreManager.Instance.DefrayItemCostByRing(costRing))
							{
								if (this.m_stateFlag.Test(0))
								{
									this.m_stateFlag.Set(1, true);
								}
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOrbotItemPresent)));
							}
							else
							{
								ObjUtil.LightPlaySE("sys_error", "SE");
								ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_randombuyitem_fail_r01", -1f);
							}
						}
						break;
					case ChaoAbility.COMBO_RANDOM_ITEM:
						if (!this.IsNowLastChance())
						{
							float num2 = 0f;
							if (StageAbilityManager.Instance != null)
							{
								num2 = StageAbilityManager.Instance.GetChaoAbilityExtraValue(ChaoAbility.COMBO_RANDOM_ITEM);
							}
							if (UnityEngine.Random.Range(0f, 100f) < num2)
							{
								if (this.m_stateFlag.Test(0))
								{
									this.m_stateFlag.Set(1, true);
								}
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateCuebotItemPresent)));
							}
							else
							{
								if (this.m_stateFlag.Test(0))
								{
									this.m_stateFlag.Set(1, true);
								}
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateFailCuebotItemPresent)));
							}
						}
						break;
					case ChaoAbility.JUMP_DESTROY_ENEMY_AND_TRAP:
					{
						MsgObjectDead value3 = new MsgObjectDead();
						GameObjectUtil.SendDelayedMessageToTagObjects("Gimmick", "OnMsgObjectDead", value3);
						GameObjectUtil.SendDelayedMessageToTagObjects("Enemy", "OnMsgObjectDead", value3);
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_raid2_ganmen_atk_sr01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						break;
					}
					case ChaoAbility.JUMP_DESTROY_ENEMY:
						GameObjectUtil.SendDelayedMessageToTagObjects("Enemy", "OnMsgObjectDead", new MsgObjectDead());
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_nodamage_movetrap_r01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						break;
					case ChaoAbility.SUPER_RING_UP:
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_up_ring10_r01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						break;
					}
					return;
				}
			}
		}

		private bool CheckItemPresent(ItemType itemType)
		{
			if (this.IsNowLastChance())
			{
				return false;
			}
			StageItemManager itemManager = this.GetItemManager();
			if (itemManager != null)
			{
				MsgAskEquipItemUsed msgAskEquipItemUsed = new MsgAskEquipItemUsed(itemType);
				itemManager.SendMessage("OnAskEquipItemUsed", msgAskEquipItemUsed);
				if (msgAskEquipItemUsed.m_ok)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsNowLastChance()
		{
			PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
			return playerInformation != null && playerInformation.IsNowLastChance();
		}

		private void OnMsgChaoAbilityEnd(MsgChaoAbilityEnd msg)
		{
			StageAbilityManager instance = StageAbilityManager.Instance;
			ChaoAbility[] ability = msg.m_ability;
			for (int i = 0; i < ability.Length; i++)
			{
				ChaoAbility ability2 = ability[i];
				if (instance != null && instance.HasChaoAbility(ability2, this.m_chao_type) && instance != null && instance.HasChaoAbility(ability2, this.m_chao_type))
				{
					TinyFsmEvent signal = TinyFsmEvent.CreateMessage(msg);
					this.m_fsmBehavior.Dispatch(signal);
					return;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEventObject(100, GetComponent<Collider>());
			this.m_fsmBehavior.Dispatch(signal);
		}

		private void OnTriggerStay(Collider other)
		{
		}

		private void OnTriggerExit(Collider other)
		{
		}

		private void OnDamageSucceed(MsgHitDamageSucceed msg)
		{
			if (this.m_fsmBehavior != null)
			{
				TinyFsmEvent signal = TinyFsmEvent.CreateMessage(msg);
				this.m_fsmBehavior.Dispatch(signal);
			}
		}

		private T CreateStateWork<T>() where T : ChaoState.StateWork, new()
		{
			if (this.m_stateWork != null)
			{
				this.DestroyStateWork();
			}
			T t = Activator.CreateInstance<T>();
			t.Name = t.ToString();
			this.m_stateWork = t;
			return t;
		}

		private void DestroyStateWork()
		{
			if (this.m_stateWork != null)
			{
				this.m_stateWork.Destroy();
				this.m_stateWork = null;
			}
		}

		private T GetStateWork<T>() where T : ChaoState.StateWork
		{
			if (this.m_stateWork != null && this.m_stateWork is T)
			{
				return this.m_stateWork as T;
			}
			return (T)((object)null);
		}

		private TinyFsmState StateIdle(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				return TinyFsmState.End();
			case 1:
				return TinyFsmState.End();
			case 4:
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
				return TinyFsmState.End();
			case 5:
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateInit(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				return TinyFsmState.End();
			case 1:
				return TinyFsmState.End();
			case 4:
				this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
				return TinyFsmState.End();
			case 5:
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateComeIn(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				return TinyFsmState.End();
			case 1:
				this.m_stateFlag.Set(0, false);
				this.m_stateFlag.Set(1, false);
				this.ChangeMovement(MOVESTATE_ID.ComeIn);
				return TinyFsmState.End();
			case 4:
				if (this.m_movement != null && this.m_movement.NextState)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
				}
				return TinyFsmState.End();
			case 5:
			{
				MsgChaoState msgChaoState = fsmEvent.GetMessage as MsgChaoState;
				if (msgChaoState != null)
				{
					MsgChaoState.State state = msgChaoState.state;
					if (state == MsgChaoState.State.STOP)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateStop)));
					}
				}
				return TinyFsmState.End();
			}
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StatePursue(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				return TinyFsmState.End();
			case 1:
				this.m_stateFlag.Set(0, false);
				this.m_stateFlag.Set(1, false);
				this.RequestStartEffect();
				this.ChangeMovement(MOVESTATE_ID.Pursue);
				return TinyFsmState.End();
			case 4:
				return TinyFsmState.End();
			case 5:
			{
				MsgChaoState msgChaoState = fsmEvent.GetMessage as MsgChaoState;
				if (msgChaoState != null)
				{
					switch (msgChaoState.state)
					{
					case MsgChaoState.State.STOP:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateStop)));
						break;
					case MsgChaoState.State.LAST_CHANCE:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateLastChance)));
						break;
					}
				}
				return TinyFsmState.End();
			}
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateLastChance(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
			{
				StageAbilityManager instance = StageAbilityManager.Instance;
				if (instance != null)
				{
					instance.RequestStopChaoEffect(ChaoAbility.LAST_CHANCE);
				}
				return TinyFsmState.End();
			}
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.LastChance);
				StageAbilityManager instance2 = StageAbilityManager.Instance;
				if (instance2 != null)
				{
					instance2.RequestPlayChaoEffect(ChaoAbility.LAST_CHANCE, this.m_chao_type);
				}
				ObjUtil.LightPlaySE("act_chip_fly", "SE");
				return TinyFsmState.End();
			}
			case 4:
				return TinyFsmState.End();
			case 5:
			{
				MsgChaoState msgChaoState = fsmEvent.GetMessage as MsgChaoState;
				if (msgChaoState != null && msgChaoState.state == MsgChaoState.State.LAST_CHANCE_END)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateLastChanceEnd)));
				}
				return TinyFsmState.End();
			}
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateLastChanceEnd(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
				this.SetAnimationFlagForAbility(true);
				ObjUtil.LightPlaySE("act_chip_pose", "SE");
				this.ChangeMovement(MOVESTATE_ID.LastChanceEnd);
				return TinyFsmState.End();
			case 4:
				return TinyFsmState.End();
			case 5:
			{
				MsgChaoState msgChaoState = fsmEvent.GetMessage as MsgChaoState;
				if (msgChaoState != null)
				{
					if (msgChaoState.state == MsgChaoState.State.COME_IN)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
					}
					else if (msgChaoState.state == MsgChaoState.State.STOP_END)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateStopEnd)));
					}
					else if (msgChaoState.state == MsgChaoState.State.STOP)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateStop)));
					}
				}
				return TinyFsmState.End();
			}
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateStop(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.m_stateFlag.Set(0, false);
				return TinyFsmState.End();
			case 1:
				this.m_stateFlag.Set(0, true);
				this.m_stateFlag.Set(1, false);
				this.ChangeMovement(MOVESTATE_ID.Stop);
				return TinyFsmState.End();
			case 4:
				return TinyFsmState.End();
			case 5:
			{
				MsgChaoState msgChaoState = fsmEvent.GetMessage as MsgChaoState;
				if (msgChaoState != null)
				{
					switch (msgChaoState.state)
					{
					case MsgChaoState.State.COME_IN:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
						break;
					case MsgChaoState.State.STOP_END:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateStopEnd)));
						break;
					case MsgChaoState.State.LAST_CHANCE:
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateLastChance)));
						break;
					}
				}
				return TinyFsmState.End();
			}
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateStopEnd(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				return TinyFsmState.End();
			case 1:
				this.ChangeMovement(MOVESTATE_ID.StopEnd);
				return TinyFsmState.End();
			case 4:
				if (this.m_movement != null && this.m_movement.NextState)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
				}
				return TinyFsmState.End();
			case 5:
			{
				MsgChaoState msgChaoState = fsmEvent.GetMessage as MsgChaoState;
				if (msgChaoState != null)
				{
					MsgChaoState.State state = msgChaoState.state;
					if (state != MsgChaoState.State.COME_IN)
					{
						if (state == MsgChaoState.State.STOP)
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateStop)));
						}
					}
					else
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
					}
				}
				return TinyFsmState.End();
			}
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateInvalidExtreme(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget2);
				ChaoMoveGoCameraTargetUsePlayerSpeed currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTargetUsePlayerSpeed>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.InvalidExtremeOffsetRate, 1.5f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.8f;
				return TinyFsmState.End();
			}
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= getDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_magic_darkqueen_sr01", -1f);
						ObjUtil.LightPlaySE("act_sharla_magic", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 1.2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateComboBonusUp(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 1f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_bonus_comboscore_sr01", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 1f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateEasySpeed(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget2);
				ChaoMoveGoCameraTargetUsePlayerSpeed currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTargetUsePlayerSpeed>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.InvalidExtremeOffsetRate, 3f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.8f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_clb_nights_magic_sr02", -1f);
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 1.2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateOutItemPhantom(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.OutCameraTarget);
				ChaoMoveOutCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveOutCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				return TinyFsmState.End();
			}
			case 4:
				if (this.m_movement != null && this.m_movement.NextState)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateStop)));
				}
				else if (!this.m_stateFlag.Test(1))
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateStopEnd)));
				}
				return TinyFsmState.End();
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateItemPhantom(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					Vector3 screenOffsetRate = (this.m_chao_type != ChaoType.SUB) ? new Vector3(0.5f, 0.5f, 0f) : new Vector3(0.5f, 0.7f, 0f);
					currentMovement.SetParameter(screenOffsetRate, 5f);
				}
				this.SetAnimationFlagForAbility(true);
				this.m_stateTimer = 1f;
				return TinyFsmState.End();
			}
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				this.m_stateTimer -= getDeltaTime;
				if (this.m_stateTimer < 0f)
				{
					this.ChangeMovement(MOVESTATE_ID.Stay);
				}
				return TinyFsmState.End();
			}
			case 5:
				switch (fsmEvent.GetMessage.ID)
				{
				case 21760:
				{
					MsgChaoState msgChaoState = fsmEvent.GetMessage as MsgChaoState;
					if (msgChaoState != null && msgChaoState.state == MsgChaoState.State.COME_IN)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
						return TinyFsmState.End();
					}
					break;
				}
				case 21762:
					this.ChangeMovement(MOVESTATE_ID.Stay);
					return TinyFsmState.End();
				}
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateKillOut(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				if (StageAbilityManager.Instance != null)
				{
					StageAbilityManager.Instance.RequestStopChaoEffect(ChaoAbility.COMBO_WIPE_OUT_ENEMY);
				}
				if (StageComboManager.Instance != null)
				{
					StageComboManager.Instance.SetChaoFlagStatus(StageComboManager.ChaoFlagStatus.ENEMY_DEAD, false);
				}
				return TinyFsmState.End();
			case 1:
				this.ChangeMovement(MOVESTATE_ID.GoKillOut);
				this.m_stateTimer = 1f;
				this.m_stateFlag.Reset();
				this.m_substate = 0;
				if (StageAbilityManager.Instance != null)
				{
					StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.COMBO_WIPE_OUT_ENEMY, this.m_chao_type);
				}
				return TinyFsmState.End();
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				this.m_stateTimer -= getDeltaTime;
				switch (this.m_substate)
				{
				case 0:
					if (this.m_stateTimer < 0f)
					{
						this.m_stateTimer = 1f;
						ChaoMoveGoKillOut currentMovement = this.GetCurrentMovement<ChaoMoveGoKillOut>();
						if (currentMovement != null)
						{
							currentMovement.ChangeMode(ChaoMoveGoKillOut.Mode.Forward);
						}
						this.m_substate = 1;
					}
					break;
				case 1:
					if (this.m_stateTimer < 0f)
					{
						GameObjectUtil.SendDelayedMessageToTagObjects("Enemy", "OnMsgObjectDead", new MsgObjectDead());
						if (StageComboManager.Instance != null)
						{
							StageComboManager.Instance.SetChaoFlagStatus(StageComboManager.ChaoFlagStatus.ENEMY_DEAD, true);
						}
						ObjUtil.LightPlaySE("act_exp", "SE");
						Camera camera = GameObjectUtil.FindGameObjectComponentWithTag<Camera>("MainCamera", "GameMainCamera");
						if (camera != null)
						{
							Vector3 zero = Vector3.zero;
							zero.z = base.transform.position.z - camera.transform.position.z;
							ObjUtil.PlayChaoEffect(camera.gameObject, "ef_ch_bomber_atk_r01", zero, -1f);
						}
						this.m_substate = 2;
						this.m_stateTimer = StageComboManager.CHAO_OBJ_DEAD_TIME;
					}
					break;
				case 2:
					if (!this.m_stateFlag.Test(1) && this.m_stateTimer < 0f)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
					}
					break;
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private void SetAnimationFlagForAbility(bool value)
		{
			if (this.m_fsmBehavior.NowShutDown)
			{
				return;
			}
			if (this.m_modelObject != null)
			{
				Animator component = this.m_modelObject.GetComponent<Animator>();
				if (component != null && component.enabled)
				{
					component.SetBool("Ability", value);
				}
			}
		}

		private IEnumerator SetAnimationFlagForAbilityCourutine(bool value)
		{
			ChaoState._SetAnimationFlagForAbilityCourutine_c__Iterator9 _SetAnimationFlagForAbilityCourutine_c__Iterator = new ChaoState._SetAnimationFlagForAbilityCourutine_c__Iterator9();
			_SetAnimationFlagForAbilityCourutine_c__Iterator.value = value;
			_SetAnimationFlagForAbilityCourutine_c__Iterator.___value = value;
			_SetAnimationFlagForAbilityCourutine_c__Iterator.__f__this = this;
			return _SetAnimationFlagForAbilityCourutine_c__Iterator;
		}

		private bool IsPlayingAbilityAnimation()
		{
			if (this.m_modelObject != null)
			{
				Animator component = this.m_modelObject.GetComponent<Animator>();
				if (component != null)
				{
					return component.GetBool("Ability");
				}
			}
			return false;
		}

		private void SetMessageComeInOut(TinyFsmEvent fsmEvent)
		{
			MsgChaoState msgChaoState = fsmEvent.GetMessage as MsgChaoState;
			if (msgChaoState != null)
			{
				switch (msgChaoState.state)
				{
				case MsgChaoState.State.COME_IN:
					this.m_stateFlag.Set(1, false);
					break;
				case MsgChaoState.State.STOP:
					this.m_stateFlag.Set(1, true);
					break;
				case MsgChaoState.State.STOP_END:
					this.m_stateFlag.Set(1, false);
					break;
				}
			}
		}

		private TinyFsmState StateAttackBoss(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.DestroyStateWork();
				this.SetSpinModelControl(false);
				return TinyFsmState.End();
			case 1:
				this.ChangeMovement(MOVESTATE_ID.AttackBoss);
				this.CreateAttackBossWork();
				this.m_substate = 0;
				this.m_stateTimer = 0.8f;
				this.SetSpinModelControl(true);
				ObjUtil.LightPlaySE("act_sword_fly", "SE");
				return TinyFsmState.End();
			case 4:
			{
				ChaoState.AttackBossWork stateWork = this.GetStateWork<ChaoState.AttackBossWork>();
				if (stateWork == null)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
					return TinyFsmState.End();
				}
				if (stateWork.m_targetObject == null)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
					return TinyFsmState.End();
				}
				float getDeltaTime = fsmEvent.GetDeltaTime;
				if (stateWork != null)
				{
					this.UpdateAttackBoss(fsmEvent, getDeltaTime, stateWork);
				}
				return TinyFsmState.End();
			}
			case 5:
			{
				int iD = fsmEvent.GetMessage.ID;
				if (iD != 12323)
				{
					if (iD == 16385)
					{
						ChaoState.AttackBossWork stateWork2 = this.GetStateWork<ChaoState.AttackBossWork>();
						this.AttackBossCheckAndChanegStateOnDamageBossSucceed(fsmEvent, stateWork2);
						return TinyFsmState.End();
					}
					if (iD == 21762)
					{
						ChaoState.AttackBossWork stateWork3 = this.GetStateWork<ChaoState.AttackBossWork>();
						if (stateWork3 != null && this.m_substate < 2)
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							return TinyFsmState.End();
						}
					}
				}
				else
				{
					MsgBossCheckState msgBossCheckState = fsmEvent.GetMessage as MsgBossCheckState;
					if (msgBossCheckState != null && !msgBossCheckState.IsAttackOK())
					{
						ChaoState.AttackBossWork stateWork4 = this.GetStateWork<ChaoState.AttackBossWork>();
						if (stateWork4 != null && this.m_substate < 2)
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							return TinyFsmState.End();
						}
					}
				}
				return TinyFsmState.End();
			}
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StatePursuesAttackBoss(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.DestroyStateWork();
				this.SetSpinModelControl(false);
				return TinyFsmState.End();
			case 1:
				this.ChangeMovement(MOVESTATE_ID.AttackBoss);
				this.CreateAttackBossWork();
				this.m_substate = 0;
				this.m_stateTimer = 0.8f;
				this.SetSpinModelControl(true);
				ObjUtil.LightPlaySE("act_sword_fly", "SE");
				return TinyFsmState.End();
			case 4:
			{
				ChaoState.AttackBossWork stateWork = this.GetStateWork<ChaoState.AttackBossWork>();
				if (stateWork == null)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
					return TinyFsmState.End();
				}
				if (stateWork.m_targetObject == null)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
					return TinyFsmState.End();
				}
				float getDeltaTime = fsmEvent.GetDeltaTime;
				if (stateWork != null)
				{
					this.UpdatePursuesAttackBoss(fsmEvent, getDeltaTime, stateWork);
				}
				return TinyFsmState.End();
			}
			case 5:
			{
				int iD = fsmEvent.GetMessage.ID;
				if (iD != 12323)
				{
					if (iD == 16385)
					{
						ChaoState.AttackBossWork stateWork2 = this.GetStateWork<ChaoState.AttackBossWork>();
						this.PursuesAttackBossCheckAndChanegStateOnDamageBossSucceed(fsmEvent, stateWork2);
						return TinyFsmState.End();
					}
					if (iD == 21762)
					{
						ChaoState.AttackBossWork stateWork3 = this.GetStateWork<ChaoState.AttackBossWork>();
						if (stateWork3 != null && this.m_substate < 2)
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							return TinyFsmState.End();
						}
					}
				}
				else
				{
					MsgBossCheckState msgBossCheckState = fsmEvent.GetMessage as MsgBossCheckState;
					if (msgBossCheckState != null && !msgBossCheckState.IsAttackOK())
					{
						ChaoState.AttackBossWork stateWork4 = this.GetStateWork<ChaoState.AttackBossWork>();
						if (stateWork4 != null && this.m_substate < 2)
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							return TinyFsmState.End();
						}
					}
				}
				return TinyFsmState.End();
			}
			}
			return TinyFsmState.End();
		}

		private TinyFsmState UpdateAttackBoss(TinyFsmEvent fsmEvent, float deltaTime, ChaoState.AttackBossWork work)
		{
			switch (this.m_substate)
			{
			case 0:
				this.m_stateTimer -= deltaTime;
				if (this.m_stateTimer <= 0f)
				{
					this.m_stateTimer = 2f;
					this.SetChaoMoveAttackBoss(ChaoMoveAttackBoss.Mode.Homing);
					this.SetSpinModelControl(false);
					this.m_substate = 1;
				}
				break;
			case 1:
				this.m_stateTimer -= deltaTime;
				if (this.m_stateTimer <= 0f)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
					return TinyFsmState.End();
				}
				break;
			case 2:
				this.m_stateTimer -= deltaTime;
				if (this.m_stateTimer <= 0f)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
					return TinyFsmState.End();
				}
				break;
			}
			return TinyFsmState.End();
		}

		private TinyFsmState UpdatePursuesAttackBoss(TinyFsmEvent fsmEvent, float deltaTime, ChaoState.AttackBossWork work)
		{
			switch (this.m_substate)
			{
			case 0:
				this.m_stateTimer -= deltaTime;
				if (this.m_stateTimer <= 0f)
				{
					this.m_stateTimer = 2f;
					this.SetChaoMoveAttackBoss(ChaoMoveAttackBoss.Mode.Homing);
					this.SetSpinModelControl(false);
					this.SetAnimationFlagForAbility(true);
					this.m_substate = 1;
				}
				break;
			case 1:
				this.m_stateTimer -= deltaTime;
				if (this.m_stateTimer <= 0f)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
					return TinyFsmState.End();
				}
				break;
			case 2:
				this.m_stateTimer -= deltaTime;
				if (this.m_stateTimer <= 0f)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
					return TinyFsmState.End();
				}
				break;
			}
			return TinyFsmState.End();
		}

		private bool AttackBossCheckAndChanegStateOnDamageBossSucceed(TinyFsmEvent fsmEvent, ChaoState.AttackBossWork work)
		{
			MsgHitDamageSucceed msgHitDamageSucceed = fsmEvent.GetMessage as MsgHitDamageSucceed;
			if (msgHitDamageSucceed != null && work != null && msgHitDamageSucceed.m_sender == work.m_targetObject)
			{
				if (work != null && this.m_substate < 2)
				{
					ObjUtil.PlayChaoEffect("ef_ch_slash_sr01", msgHitDamageSucceed.m_position, 3f);
					ObjUtil.LightPlaySE("act_sword_attack", "SE");
					this.m_stateTimer = 0.5f;
					this.m_substate = 2;
					this.SetChaoMoveAttackBoss(ChaoMoveAttackBoss.Mode.AfterAttack);
					work.DestroyAttackCollision();
					this.SetSpinModelControl(true);
					this.m_attackCount++;
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
				}
				return true;
			}
			return false;
		}

		private bool PursuesAttackBossCheckAndChanegStateOnDamageBossSucceed(TinyFsmEvent fsmEvent, ChaoState.AttackBossWork work)
		{
			MsgHitDamageSucceed msgHitDamageSucceed = fsmEvent.GetMessage as MsgHitDamageSucceed;
			if (msgHitDamageSucceed != null && work != null && msgHitDamageSucceed.m_sender == work.m_targetObject)
			{
				if (work != null && this.m_substate < 2)
				{
					ObjUtil.PlayChaoEffect("ef_ch_raid1_moon_atk_sr01", msgHitDamageSucceed.m_position, 3f);
					ObjUtil.LightPlaySE("act_sword_attack", "SE");
					this.m_stateTimer = 0.5f;
					this.m_substate = 2;
					this.SetAnimationFlagForAbility(false);
					this.SetChaoMoveAttackBoss(ChaoMoveAttackBoss.Mode.AfterAttack);
					work.DestroyAttackCollision();
					this.SetSpinModelControl(true);
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
				}
				return true;
			}
			return false;
		}

		private void CreateAttackBossWork()
		{
			MsgBossInfo msgBossInfo = new MsgBossInfo();
			GameObjectUtil.SendMessageToTagObjects("Boss", "OnMsgBossInfo", msgBossInfo, SendMessageOptions.DontRequireReceiver);
			ChaoState.AttackBossWork attackBossWork = this.CreateStateWork<ChaoState.AttackBossWork>();
			if (msgBossInfo.m_succeed)
			{
				ChaoMoveAttackBoss currentMovement = this.GetCurrentMovement<ChaoMoveAttackBoss>();
				attackBossWork.m_targetObject = msgBossInfo.m_boss;
				if (currentMovement != null)
				{
					currentMovement.SetTarget(msgBossInfo.m_boss);
				}
			}
			attackBossWork.m_attackCollision = ChaoPartsAttackEnemy.Create(base.gameObject);
		}

		private void SetChaoMoveAttackBoss(ChaoMoveAttackBoss.Mode mode)
		{
			ChaoMoveAttackBoss currentMovement = this.GetCurrentMovement<ChaoMoveAttackBoss>();
			if (currentMovement != null)
			{
				currentMovement.ChangeMode(mode);
			}
		}

		private void SetSpinModelControl(bool flag)
		{
			if (this.m_modelControl != null)
			{
				if (flag)
				{
					this.m_modelControl.ChangeStateToSpin(ChaoState.AttackBossModelSpinSpeed);
				}
				else
				{
					this.m_modelControl.ChangeStateToReturnIdle();
				}
			}
		}

		private TinyFsmState StateRingMagnet(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ChaoWalkerOffsetRate, 2f);
				}
				this.m_chaoWalkerState = ChaoState.ChaoWalkerState.Action;
				this.m_stateTimer = 0.6f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.ChaoWalkerState chaoWalkerState = this.m_chaoWalkerState;
				if (chaoWalkerState != ChaoState.ChaoWalkerState.Action)
				{
					if (chaoWalkerState == ChaoState.ChaoWalkerState.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						this.SetEnableMagnetComponet(ChaoAbility.CHAO_RING_MAGNET);
						this.m_stateTimer = 1f;
						this.m_chaoWalkerState = ChaoState.ChaoWalkerState.Wait;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateCrystalMagnet(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ChaoWalkerOffsetRate, 2f);
				}
				this.m_chaoWalkerState = ChaoState.ChaoWalkerState.Action;
				this.m_stateTimer = 0.6f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.ChaoWalkerState chaoWalkerState = this.m_chaoWalkerState;
				if (chaoWalkerState != ChaoState.ChaoWalkerState.Action)
				{
					if (chaoWalkerState == ChaoState.ChaoWalkerState.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						this.SetEnableMagnetComponet(ChaoAbility.CHAO_CRYSTAL_MAGNET);
						this.m_stateTimer = 1f;
						this.m_chaoWalkerState = ChaoState.ChaoWalkerState.Wait;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private void SetEnableMagnetComponet(ChaoAbility ability)
		{
			float enable = 4f;
			if (StageAbilityManager.Instance != null)
			{
				enable = StageAbilityManager.Instance.GetChaoAbilityValue(ability);
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				ChaoPartsObjectMagnet component = child.GetComponent<ChaoPartsObjectMagnet>();
				if (component != null)
				{
					component.SetEnable(enable);
					break;
				}
			}
		}

		private void SetMagnetPause(bool flag)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				ChaoPartsObjectMagnet component = child.GetComponent<ChaoPartsObjectMagnet>();
				if (component != null)
				{
					component.SetPause(flag);
					break;
				}
			}
		}

		private TinyFsmState StateGameHardware(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.HardwareCartridgeOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.8f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("act_chao_megadrive", "SE");
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_time_combotime_r01", -1f);
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateGameCartridge(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.HardwareCartridgeOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.8f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						if (StageAbilityManager.Instance != null)
						{
							int addCombo = (int)StageAbilityManager.Instance.GetChaoAbilityValue(ChaoAbility.ADD_COMBO_VALUE);
							if (StageComboManager.Instance != null)
							{
								StageComboManager.Instance.AddComboForChaoAbilityValue(addCombo);
							}
						}
						ObjUtil.LightPlaySE("act_chao_cartridge", "SE");
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_up_combocount_r01", -1f);
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateNights(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.NightsOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 1.5f;
				StageItemManager itemManager = this.GetItemManager();
				if (itemManager != null)
				{
					itemManager.SendMessage("OnChaoAbilityLoopComboUp", null, SendMessageOptions.DontRequireReceiver);
				}
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("act_chao_nights", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_clb_nights_magic_sr01", -1f);
						this.m_stateTimer = 2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateReala(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.RealaOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 1.5f;
				StageItemManager itemManager = this.GetItemManager();
				if (itemManager != null)
				{
					itemManager.SendMessage("OnChaoAbilityLoopMagnet", null, SendMessageOptions.DontRequireReceiver);
				}
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("act_chao_reala", "SE");
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_clb_reala_magic_sr01", -1f);
						this.m_stateTimer = 2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private void SetItemBtnObjAndUICamera()
		{
			if (this.m_uiCamera != null && this.m_itemBtnObj != null)
			{
				return;
			}
			GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
			if (cameraUIObject != null)
			{
				this.m_uiCamera = cameraUIObject.GetComponent<Camera>();
				GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "HudCockpit");
				if (gameObject != null)
				{
					this.m_itemBtnObj = GameObjectUtil.FindChildGameObject(gameObject, "HUD_btn_item");
				}
			}
		}

		private void SetTargetScreenPos()
		{
			if (this.m_uiCamera != null && this.m_itemBtnObj != null)
			{
				this.m_targetScreenPos = this.m_uiCamera.WorldToScreenPoint(this.m_itemBtnObj.transform.position);
			}
		}

		private void SetPresentEquipItemPos()
		{
			if (Camera.main != null && this.m_uiCamera != null && this.m_itemBtnObj)
			{
				Vector3 position = Camera.main.WorldToScreenPoint(base.transform.position);
				Vector3 vector = this.m_uiCamera.WorldToScreenPoint(this.m_itemBtnObj.transform.position);
				position.x = vector.x;
				position.y = vector.y;
				Vector3 position2 = Camera.main.ScreenToWorldPoint(position);
				position2.z = 0f;
				this.m_effectObj.transform.position = position2;
			}
		}

		private TinyFsmState StateItemPresent(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				if (!this.m_presentFlag)
				{
					this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
					StageItemManager itemManager = this.GetItemManager();
					if (this.CheckItemPresent(this.m_chaoItemType) && itemManager != null)
					{
						MsgAddItemToManager value = new MsgAddItemToManager(this.m_chaoItemType);
						itemManager.SendMessage("OnAddItem", value, SendMessageOptions.DontRequireReceiver);
					}
				}
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				this.m_presentFlag = false;
				return TinyFsmState.End();
			}
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= getDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						bool flag = false;
						PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
						if (playerInformation != null)
						{
							if (playerInformation.PhantomType == PhantomType.NONE)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTbl[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTbl.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
							if (!flag)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
						}
						if (flag)
						{
							StageItemManager itemManager2 = this.GetItemManager();
							if (itemManager2 != null)
							{
								ItemType chaoItemType = this.m_chaoItemType;
								MsgAddItemToManager value2 = new MsgAddItemToManager(chaoItemType);
								itemManager2.SendMessage("OnAddItem", value2, SendMessageOptions.DontRequireReceiver);
								ObjUtil.LightPlaySE("obj_itembox", "SE");
								ObjUtil.LightPlaySE("act_sharla_magic", "SE");
								ObjUtil.SendGetItemIcon(chaoItemType);
								ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_magic_item_st_sr01", -1f);
								base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
								MsgAbilityEffectStart value3 = new MsgAbilityEffectStart(ChaoAbility.COMBO_ITEM_BOX, "ef_ch_magic_item_ht_sr01", false, true);
								GameObjectUtil.SendDelayedMessageToTagObjects("Player", "OnMsgAbilityEffectStart", value3);
							}
							this.m_presentFlag = true;
							this.m_stateTimer = 1.2f;
							this.m_substate = 1;
						}
						else if (this.m_stateFlag.Test(1))
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
						}
						else
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
						}
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateItemPresent2(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				if (!this.m_presentFlag)
				{
					this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
					StageItemManager itemManager = this.GetItemManager();
					if (this.CheckItemPresent(this.m_chaoItemType) && itemManager != null)
					{
						MsgAddItemToManager value = new MsgAddItemToManager(this.m_chaoItemType);
						itemManager.SendMessage("OnAddItem", value, SendMessageOptions.DontRequireReceiver);
					}
				}
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				this.m_presentFlag = false;
				return TinyFsmState.End();
			}
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= getDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						bool flag = false;
						PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
						if (playerInformation != null)
						{
							if (playerInformation.PhantomType == PhantomType.NONE)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTbl[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTbl.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
							if (!flag)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
						}
						if (flag)
						{
							StageItemManager itemManager2 = this.GetItemManager();
							if (itemManager2 != null)
							{
								MsgAddItemToManager value2 = new MsgAddItemToManager(this.m_chaoItemType);
								itemManager2.SendMessage("OnAddItem", value2, SendMessageOptions.DontRequireReceiver);
								ObjUtil.LightPlaySE("obj_itembox", "SE");
								ObjUtil.LightPlaySE("act_chao_quna", "SE");
								ObjUtil.SendGetItemIcon(this.m_chaoItemType);
								ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_clb_quna_item_st_sr01", -1f);
								base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
								MsgAbilityEffectStart value3 = new MsgAbilityEffectStart(ChaoAbility.CHECK_POINT_ITEM_BOX, "ef_ch_clb_quna_item_ht_sr01", false, true);
								GameObjectUtil.SendDelayedMessageToTagObjects("Player", "OnMsgAbilityEffectStart", value3);
							}
							this.m_stateTimer = 1.2f;
							this.m_substate = 1;
							this.m_presentFlag = true;
						}
						else if (this.m_stateFlag.Test(1))
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
						}
						else
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
						}
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StatePhantomPresent(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				if (!this.m_presentFlag)
				{
					StageItemManager itemManager = this.GetItemManager();
					if (itemManager != null)
					{
						MsgAddItemToManager value = new MsgAddItemToManager(this.m_chaoItemType);
						itemManager.SendMessage("OnAddColorItem", value, SendMessageOptions.DontRequireReceiver);
					}
				}
				return TinyFsmState.End();
			case 1:
			{
				PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
				if (playerInformation != null)
				{
					PhantomType phantomType = playerInformation.PhantomType;
					switch (phantomType + 1)
					{
					case PhantomType.LASER:
					{
						StageItemManager itemManager2 = this.GetItemManager();
						if (itemManager2 != null)
						{
							this.m_chaoItemType = itemManager2.GetPhantomItemType();
						}
						if (this.m_chaoItemType == ItemType.UNKNOWN)
						{
							this.m_chaoItemType = ChaoState.ChaoAbilityPhantomTbl[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityPhantomTbl.Length)];
						}
						break;
					}
					case PhantomType.DRILL:
						this.m_chaoItemType = ItemType.LASER;
						break;
					case PhantomType.ASTEROID:
						this.m_chaoItemType = ItemType.DRILL;
						break;
					case PhantomType.NUM_MAX:
						this.m_chaoItemType = ItemType.ASTEROID;
						break;
					}
				}
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				this.m_presentFlag = false;
				return TinyFsmState.End();
			}
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= getDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						StageItemManager itemManager3 = this.GetItemManager();
						if (itemManager3 != null)
						{
							MsgAddItemToManager value2 = new MsgAddItemToManager(this.m_chaoItemType);
							itemManager3.SendMessage("OnAddColorItem", value2, SendMessageOptions.DontRequireReceiver);
							ObjUtil.LightPlaySE("obj_itembox", "SE");
							ObjUtil.LightPlaySE("act_sharla_magic", "SE");
							ObjUtil.SendGetItemIcon(this.m_chaoItemType);
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_sp1_merlina_magic_sr01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						this.m_presentFlag = true;
						this.m_stateTimer = 1.2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateReviveEquipItem(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				this.m_stateFlag.Set(2, false);
				return TinyFsmState.End();
			case 1:
			{
				this.m_stateFlag.Set(2, true);
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ChaosOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 1f;
				this.SetItemBtnObjAndUICamera();
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("act_chao_chaosadv", "SE");
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_itemrecycle_sr01", -1f);
						ObjUtil.PlayChaoEffectForHUD(this.m_itemBtnObj, "ef_ch_itemrecycle_ht_sr01", -1f);
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 2.2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StatePresentSRareEquipItem(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				if (!this.m_presentFlag)
				{
					StageItemManager itemManager = this.GetItemManager();
					if (itemManager != null)
					{
						itemManager.SendMessage("OnAddEquipItem", null, SendMessageOptions.DontRequireReceiver);
						this.m_presentFlag = true;
					}
				}
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				this.SetItemBtnObjAndUICamera();
				this.m_presentFlag = false;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("act_sharla_magic", "SE");
						this.m_effectObj = ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_magic_darkqueen_st_sr01", -1f);
						ObjUtil.PlayChaoEffectForHUD(this.m_itemBtnObj, "ef_ch_magic_darkqueen_ht_sr01", -1f);
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 1.2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StatePresentEquipItem(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				this.m_effectObj = null;
				if (!this.m_presentFlag)
				{
					StageItemManager itemManager = this.GetItemManager();
					if (itemManager != null)
					{
						itemManager.SendMessage("OnAddEquipItem", null, SendMessageOptions.DontRequireReceiver);
						this.m_presentFlag = true;
					}
				}
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.PufferFishOffsetRate, 2.5f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 1f;
				this.SetItemBtnObjAndUICamera();
				this.SetTargetScreenPos();
				this.m_presentFlag = false;
				return TinyFsmState.End();
			}
			case 4:
				switch (this.m_substate)
				{
				case 0:
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("act_chao_effect", "SE");
						this.m_effectObj = ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_getitem_r01", 2.5f);
						if (this.m_effectObj != null && Camera.main != null)
						{
							this.m_effectScreenPos = Camera.main.WorldToScreenPoint(this.m_effectObj.transform.position);
							this.m_targetScreenPos.z = this.m_effectScreenPos.z;
						}
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 0.5f;
						this.m_substate = 1;
					}
					break;
				case 1:
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						this.m_distance = Vector3.Distance(this.m_effectScreenPos, this.m_targetScreenPos);
						this.m_stateTimer = 1f;
						this.m_substate = 2;
					}
					break;
				case 2:
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_effectObj != null)
					{
						this.m_effectScreenPos = Vector3.MoveTowards(this.m_effectScreenPos, this.m_targetScreenPos, this.m_distance * Time.deltaTime);
						if (Camera.main != null)
						{
							Vector3 position = Camera.main.ScreenToWorldPoint(this.m_effectScreenPos);
							position.z = -1.5f;
							this.m_effectObj.transform.position = position;
						}
					}
					if (this.m_stateTimer < 0f)
					{
						StageItemManager itemManager2 = this.GetItemManager();
						if (itemManager2 != null)
						{
							itemManager2.SendMessage("OnAddEquipItem", null, SendMessageOptions.DontRequireReceiver);
						}
						this.m_presentFlag = true;
						this.m_stateTimer = 0.5f;
						this.m_substate = 3;
					}
					break;
				case 3:
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						if (this.m_stateFlag.Test(1))
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
						}
						else
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
						}
					}
					break;
				}
				return TinyFsmState.End();
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StatePhantomPresentAsteroid(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.m_chaoItemType = ItemType.ASTEROID;
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						StageItemManager itemManager = this.GetItemManager();
						if (itemManager != null)
						{
							MsgAddItemToManager value = new MsgAddItemToManager(this.m_chaoItemType);
							itemManager.SendMessage("OnAddColorItem", value, SendMessageOptions.DontRequireReceiver);
							ObjUtil.LightPlaySE("act_chao_effect", "SE");
							ObjUtil.SendGetItemIcon(this.m_chaoItemType);
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_asteroid_r01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						this.m_stateTimer = 1.2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StatePhantomPresentDrill(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.m_chaoItemType = ItemType.DRILL;
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						StageItemManager itemManager = this.GetItemManager();
						if (itemManager != null)
						{
							MsgAddItemToManager value = new MsgAddItemToManager(this.m_chaoItemType);
							itemManager.SendMessage("OnAddColorItem", value, SendMessageOptions.DontRequireReceiver);
							ObjUtil.LightPlaySE("act_chao_effect", "SE");
							ObjUtil.SendGetItemIcon(this.m_chaoItemType);
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_drill_r01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						this.m_stateTimer = 1.2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StatePhantomPresentLaser(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.m_chaoItemType = ItemType.LASER;
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						StageItemManager itemManager = this.GetItemManager();
						if (itemManager != null)
						{
							MsgAddItemToManager value = new MsgAddItemToManager(this.m_chaoItemType);
							itemManager.SendMessage("OnAddColorItem", value, SendMessageOptions.DontRequireReceiver);
							ObjUtil.LightPlaySE("act_chao_effect", "SE");
							ObjUtil.SendGetItemIcon(this.m_chaoItemType);
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_laser_r01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						this.m_stateTimer = 1.2f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateChangeEquipItem(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				if (!this.m_presentFlag)
				{
					StageItemManager itemManager = this.GetItemManager();
					if (itemManager != null)
					{
						itemManager.SendMessage("OnChangeItem", null, SendMessageOptions.DontRequireReceiver);
					}
					this.m_presentFlag = true;
				}
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				this.SetItemBtnObjAndUICamera();
				this.m_presentFlag = false;
				return TinyFsmState.End();
			}
			case 4:
			{
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= fsmEvent.GetDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= fsmEvent.GetDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						StageItemManager itemManager2 = this.GetItemManager();
						if (itemManager2 != null)
						{
							itemManager2.SendMessage("OnChangeItem", null, SendMessageOptions.DontRequireReceiver);
							ObjUtil.LightPlaySE("act_chao_effect", "SE");
							ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_changeitem_r01", -1f);
							ObjUtil.PlayChaoEffectForHUD(this.m_itemBtnObj, "ef_ch_combo_changeitem_ht_r01", -1f);
							base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						}
						this.m_stateTimer = 1f;
						this.m_substate = 1;
						this.m_presentFlag = true;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateOrbotItemPresent(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				if (!this.m_presentFlag)
				{
					this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
					if (this.CheckItemPresent(this.m_chaoItemType) && StageItemManager.Instance != null)
					{
						StageItemManager.Instance.SendMessage("OnAddItem", new MsgAddItemToManager(this.m_chaoItemType), SendMessageOptions.DontRequireReceiver);
					}
				}
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.ItemPresentOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				this.m_presentFlag = false;
				return TinyFsmState.End();
			}
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				switch (this.m_substate)
				{
				case 0:
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						bool flag = false;
						PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
						if (playerInformation != null)
						{
							if (playerInformation.PhantomType == PhantomType.NONE)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTbl[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTbl.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
							if (!flag)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
						}
						if (flag)
						{
							if (StageItemManager.Instance != null)
							{
								ItemType chaoItemType = this.m_chaoItemType;
								MsgAddItemToManager value = new MsgAddItemToManager(chaoItemType);
								StageItemManager.Instance.SendMessage("OnAddItem", value, SendMessageOptions.DontRequireReceiver);
								ObjUtil.LightPlaySE("obj_itembox", "SE");
								ObjUtil.LightPlaySE("act_chao_effect", "SE");
								ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_randombuyitem_r01", -1f);
								ObjUtil.SendGetItemIcon(chaoItemType);
								base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
							}
							this.m_presentFlag = true;
							this.m_stateTimer = 0.8f;
							this.m_substate = 2;
						}
						else if (this.m_stateFlag.Test(1))
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
						}
						else
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
						}
					}
					break;
				case 1:
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						if (this.m_stateFlag.Test(1))
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
						}
						else
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
						}
					}
					break;
				case 2:
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						this.m_stateTimer += 1f;
						ObjUtil.LightPlaySE("act_ringspread", "SE");
						this.m_substate = 1;
					}
					break;
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateCuebotItemPresent(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				if (!this.m_presentFlag)
				{
					this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
					if (this.CheckItemPresent(this.m_chaoItemType) && StageItemManager.Instance != null)
					{
						StageItemManager.Instance.SendMessage("OnAddItem", new MsgAddItemToManager(this.m_chaoItemType), SendMessageOptions.DontRequireReceiver);
					}
				}
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.CuebotItemOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				this.m_presentFlag = false;
				return TinyFsmState.End();
			}
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= getDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						bool flag = false;
						PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
						if (playerInformation != null)
						{
							if (playerInformation.PhantomType == PhantomType.NONE)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTbl[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTbl.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
							if (!flag)
							{
								this.m_chaoItemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
								flag = this.CheckItemPresent(this.m_chaoItemType);
							}
						}
						if (flag)
						{
							if (StageItemManager.Instance != null)
							{
								ItemType chaoItemType = this.m_chaoItemType;
								MsgAddItemToManager value = new MsgAddItemToManager(chaoItemType);
								StageItemManager.Instance.SendMessage("OnAddItem", value, SendMessageOptions.DontRequireReceiver);
								ObjUtil.LightPlaySE("obj_itembox", "SE");
								ObjUtil.LightPlaySE("act_chao_effect", "SE");
								ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_randomitem_r01", -1f);
								ObjUtil.SendGetItemIcon(chaoItemType);
								base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
							}
							this.m_presentFlag = true;
							this.m_stateTimer = 1.8f;
							this.m_substate = 1;
						}
						else if (this.m_stateFlag.Test(1))
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
						}
						else
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
						}
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateFailCuebotItemPresent(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				this.SetAnimationFlagForAbility(false);
				return TinyFsmState.End();
			case 1:
			{
				this.ChangeMovement(MOVESTATE_ID.GoCameraTarget);
				ChaoMoveGoCameraTarget currentMovement = this.GetCurrentMovement<ChaoMoveGoCameraTarget>();
				if (currentMovement != null)
				{
					currentMovement.SetParameter(ChaoState.CuebotItemOffsetRate, 2f);
				}
				this.m_substate = 0;
				this.m_stateTimer = 0.6f;
				return TinyFsmState.End();
			}
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				ChaoState.SubStateItemPresent substate = (ChaoState.SubStateItemPresent)this.m_substate;
				if (substate != ChaoState.SubStateItemPresent.Action)
				{
					if (substate == ChaoState.SubStateItemPresent.Wait)
					{
						this.m_stateTimer -= getDeltaTime;
						if (this.m_stateTimer < 0f)
						{
							if (this.m_stateFlag.Test(1))
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StateOutItemPhantom)));
							}
							else
							{
								this.ChangeState(new TinyFsmState(new EventFunction(this.StatePursue)));
							}
						}
					}
				}
				else
				{
					this.m_stateTimer -= getDeltaTime;
					if (this.m_stateTimer < 0f)
					{
						ObjUtil.LightPlaySE("sys_error", "SE");
						ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_combo_randomitem_fail_r01", -1f);
						base.StartCoroutine(this.SetAnimationFlagForAbilityCourutine(true));
						this.m_stateTimer = 1.8f;
						this.m_substate = 1;
					}
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private ItemType GetOrbotCuebotItem()
		{
			ItemType itemType = ItemType.BARRIER;
			PlayerInformation playerInformation = GameObjectUtil.FindGameObjectComponentWithTag<PlayerInformation>("StageManager", "PlayerInformation");
			if (playerInformation != null)
			{
				bool flag = false;
				if (playerInformation.PhantomType == PhantomType.NONE)
				{
					itemType = ChaoState.ChaoAbilityItemTbl[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTbl.Length)];
					flag = this.CheckItemPresent(itemType);
				}
				if (!flag)
				{
					itemType = ChaoState.ChaoAbilityItemTblPhantom[UnityEngine.Random.Range(0, ChaoState.ChaoAbilityItemTblPhantom.Length)];
				}
			}
			return itemType;
		}

		private TinyFsmState StateStockRing(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
			{
				if (this.m_modelControl != null)
				{
					this.m_modelControl.ChangeStateToReturnIdle();
				}
				GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "ef_ch_tornade_fog_(Clone)");
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				return TinyFsmState.End();
			}
			case 1:
				this.m_stateTimer = 2f;
				this.m_stateFlag.Reset();
				this.m_stockRingState = ChaoState.StockRingState.CHANGE_MOVEMENT;
				return TinyFsmState.End();
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				this.m_stateTimer -= getDeltaTime;
				switch (this.m_stockRingState)
				{
				case ChaoState.StockRingState.CHANGE_MOVEMENT:
					this.ChangeMovement(MOVESTATE_ID.GoRingBanking);
					this.m_stockRingState = ChaoState.StockRingState.PLAY_SPIN_MOTION;
					break;
				case ChaoState.StockRingState.PLAY_SPIN_MOTION:
					if (this.m_modelControl != null)
					{
						this.m_modelControl.ChangeStateToSpin(ChaoState.StockRingModelSpinSpeed);
					}
					this.m_stockRingState = ChaoState.StockRingState.PLAY_EFECT;
					break;
				case ChaoState.StockRingState.PLAY_EFECT:
				{
					string text = "ef_ch_tornade_fog_";
					switch (ChaoState.GetRarity(this.m_chao_id))
					{
					case ChaoData.Rarity.NORMAL:
						text += "c01";
						break;
					case ChaoData.Rarity.RARE:
						text += "r01";
						break;
					case ChaoData.Rarity.SRARE:
						text += "sr01";
						ObjUtil.LightPlaySE("act_tornado_fly", "SE");
						break;
					}
					ObjUtil.PlayEffectChild(base.gameObject, text, Vector3.zero, Quaternion.identity, false);
					this.m_stockRingState = ChaoState.StockRingState.PLAY_EFFECT;
					break;
				}
				case ChaoState.StockRingState.PLAY_EFFECT:
					if (this.m_stateTimer < 1f && StageAbilityManager.Instance != null)
					{
						StageAbilityManager.Instance.RequestPlayChaoEffect(ChaoAbility.RECOVERY_RING, this.m_chao_type);
						this.m_stockRingState = ChaoState.StockRingState.WAIT_END;
					}
					break;
				case ChaoState.StockRingState.WAIT_END:
					if (this.m_stateTimer < 0f)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
						this.m_stockRingState = ChaoState.StockRingState.IDLE;
					}
					break;
				}
				return TinyFsmState.End();
			}
			case 5:
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private TinyFsmState StateDoubleRing(TinyFsmEvent fsmEvent)
		{
			int signal = fsmEvent.Signal;
			switch (signal + 4)
			{
			case 0:
				if (this.m_modelControl != null)
				{
					this.m_modelControl.ChangeStateToReturnIdle();
				}
				return TinyFsmState.End();
			case 1:
				this.m_stateTimer = 2.5f;
				this.m_stockRingState = ChaoState.StockRingState.CHANGE_MOVEMENT;
				return TinyFsmState.End();
			case 4:
			{
				float getDeltaTime = fsmEvent.GetDeltaTime;
				this.m_stateTimer -= getDeltaTime;
				switch (this.m_stockRingState)
				{
				case ChaoState.StockRingState.CHANGE_MOVEMENT:
					this.ChangeMovement(MOVESTATE_ID.GoRingBanking);
					this.m_stockRingState = ChaoState.StockRingState.PLAY_SPIN_MOTION;
					break;
				case ChaoState.StockRingState.PLAY_SPIN_MOTION:
					if (this.m_modelControl != null)
					{
						this.m_modelControl.ChangeStateToSpin(ChaoState.StockRingModelSpinSpeed);
					}
					this.m_stockRingState = ChaoState.StockRingState.PLAY_EFECT;
					break;
				case ChaoState.StockRingState.PLAY_EFECT:
					ObjUtil.PlayChaoEffect(base.gameObject, "ef_ch_up_bank_sr01", -1f);
					ObjUtil.LightPlaySE("act_chao_tornado2", "SE");
					this.m_stockRingState = ChaoState.StockRingState.WAIT_END;
					break;
				case ChaoState.StockRingState.WAIT_END:
					if (this.m_stateTimer < 0f)
					{
						if (this.m_stateFlag.Test(1))
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateStop)));
						}
						else
						{
							this.ChangeState(new TinyFsmState(new EventFunction(this.StateComeIn)));
						}
						this.m_stockRingState = ChaoState.StockRingState.IDLE;
					}
					break;
				}
				return TinyFsmState.End();
			}
			case 5:
				this.SetMessageComeInOut(fsmEvent);
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}

		private IEnumerator RingBank()
		{
			ChaoState._RingBank_c__IteratorA _RingBank_c__IteratorA = new ChaoState._RingBank_c__IteratorA();
			_RingBank_c__IteratorA.__f__this = this;
			return _RingBank_c__IteratorA;
		}
	}
}
