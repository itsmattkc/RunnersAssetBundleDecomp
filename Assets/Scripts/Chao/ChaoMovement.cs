using App;
using Player;
using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMovement : MonoBehaviour
	{
		private FSMSystem<ChaoMovement> m_fsm;

		private PlayerInformation m_player_info;

		private ChaoParameter m_chao_param;

		private ChaoHoveringBase m_hoveringMove;

		[SerializeField]
		private Vector3 m_offset_pos = new Vector3(-0.5f, 0.8f, 0f);

		private Vector3 m_target_pos = new Vector3(0f, 0f, 0f);

		private Vector3 m_offsetRadicon = new Vector3(-1f, 0f, 0f);

		private Vector3 m_velocity = Vector3.zero;

		private float m_come_in_speed = 5f;

		private float m_target_access_speed = 5f;

		private bool m_next_state_flag;

		private Vector3 m_moved_velocity = Vector3.zero;

		private Vector3 m_prevPlayerPos = Vector3.zero;

		private bool m_fromComeIn;

		public static readonly Vector3 HorzDir = CharacterDefs.BaseFrontTangent;

		public static readonly Vector3 VertDir = Vector3.up;

		public Vector3 Position
		{
			get
			{
				return base.transform.position;
			}
			set
			{
				base.transform.position = value;
			}
		}

		public Vector3 Angles
		{
			get
			{
				return base.transform.localEulerAngles;
			}
			set
			{
				base.transform.localEulerAngles = value;
			}
		}

		public float ComeInSpeed
		{
			get
			{
				return this.m_come_in_speed;
			}
		}

		public float TargetAccessSpeed
		{
			get
			{
				return this.m_target_access_speed;
			}
		}

		public Vector3 Hovering
		{
			get
			{
				if (this.m_hoveringMove != null)
				{
					return this.m_hoveringMove.Position;
				}
				return Vector3.zero;
			}
		}

		public Vector3 OffsetPosition
		{
			get
			{
				return this.m_offset_pos;
			}
			protected set
			{
				this.m_offset_pos = value;
			}
		}

		public Vector3 TargetPosition
		{
			get
			{
				return this.m_target_pos;
			}
		}

		public PlayerInformation PlayerInfo
		{
			get
			{
				return this.m_player_info;
			}
		}

		public bool IsPlyayerMoved
		{
			get
			{
				return this.m_player_info != null && this.m_player_info.IsMovementUpdated();
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return this.m_velocity;
			}
			set
			{
				this.m_velocity = value;
			}
		}

		public Vector3 MovedVelocity
		{
			get
			{
				return this.m_moved_velocity;
			}
		}

		public Vector3 VertVelocity
		{
			get
			{
				return Vector3.Dot(this.m_velocity, Vector3.up) * Vector3.up;
			}
		}

		public Vector3 HorzVelocity
		{
			get
			{
				return this.m_velocity - this.VertVelocity;
			}
		}

		public bool NextState
		{
			get
			{
				return this.m_next_state_flag;
			}
			set
			{
				this.m_next_state_flag = value;
			}
		}

		public ChaoParameter Parameter
		{
			get
			{
				return this.m_chao_param;
			}
		}

		public ChaoParameterData ParameterData
		{
			get
			{
				return this.m_chao_param.Data;
			}
		}

		public Vector3 PlayerPosition
		{
			get
			{
				if (this.m_player_info != null)
				{
					return this.m_player_info.Position;
				}
				return Vector3.zero;
			}
		}

		public Vector3 PrevPlayerPosition
		{
			get
			{
				return this.m_prevPlayerPos;
			}
		}

		public bool FromComeIn
		{
			get
			{
				return this.m_fromComeIn;
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			if (App.Math.NearZero(deltaTime, 1E-06f))
			{
				return;
			}
			Vector3 position = this.Position;
			if (this.m_player_info != null)
			{
				this.m_target_pos = this.m_player_info.Position;
				this.m_target_pos.z = 0f;
			}
			if (this.m_fsm != null)
			{
				this.m_fsm.CurrentState.Step(this, deltaTime);
			}
			this.m_moved_velocity = (this.Position - position) / deltaTime;
			if (this.m_player_info != null)
			{
				this.m_prevPlayerPos = this.PlayerInfo.Position;
			}
		}

		private void OnDestroy()
		{
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.Leave(this);
				this.m_fsm = null;
			}
		}

		public void ChangeState(MOVESTATE_ID state)
		{
			this.m_next_state_flag = false;
			if (this.m_fsm != null && this.m_fsm.CurrentStateID != (StateID)state)
			{
				this.m_fromComeIn = (this.m_fsm.CurrentStateID == (StateID)3);
				this.m_fsm.ChangeState(this, (int)state);
			}
		}

		public static ChaoMovement Create(GameObject gameObject, ChaoSetupParameterData parameter)
		{
			ChaoMovement chaoMovement = gameObject.AddComponent<ChaoMovement>();
			FSMStateFactory<ChaoMovement>[] movementStateTable = MovementSetupChao.GetMovementStateTable();
			ChaoMovementType moveType = parameter.MoveType;
			if (moveType != ChaoMovementType.CHAO)
			{
				if (moveType == ChaoMovementType.RADICON)
				{
					movementStateTable = MovementSetupRadicon.GetMovementStateTable();
				}
			}
			chaoMovement.Setup(parameter, movementStateTable);
			return chaoMovement;
		}

		private void Setup(ChaoSetupParameterData parameter, FSMStateFactory<ChaoMovement>[] movementTable)
		{
			this.SetupBase(parameter);
			this.SetupFsm(movementTable);
			this.CreateHovering(parameter);
		}

		private void SetupFsm(FSMStateFactory<ChaoMovement>[] fsmtable)
		{
			this.m_fsm = new FSMSystem<ChaoMovement>();
			if (this.m_fsm != null && fsmtable != null)
			{
				for (int i = 0; i < fsmtable.Length; i++)
				{
					FSMStateFactory<ChaoMovement> fSMStateFactory = fsmtable[i];
					this.m_fsm.AddState(fSMStateFactory.stateID, fSMStateFactory.state);
				}
				this.m_fsm.Init(this, 3);
			}
		}

		private void SetupBase(ChaoSetupParameterData setupParameter)
		{
			this.m_player_info = ObjUtil.GetPlayerInformation();
			if (this.m_player_info != null)
			{
				this.m_prevPlayerPos = this.m_player_info.Position;
			}
			GameObject gameObject = GameObject.Find("StageChao/ChaoParameter");
			if (gameObject != null)
			{
				this.m_chao_param = gameObject.GetComponent<ChaoParameter>();
			}
			ChaoType chaoType = ChaoUtility.GetChaoType(base.gameObject);
			if (setupParameter != null)
			{
				ChaoType chaoType2 = chaoType;
				if (chaoType2 != ChaoType.MAIN)
				{
					if (chaoType2 == ChaoType.SUB)
					{
						this.OffsetPosition = setupParameter.SubOffset;
					}
				}
				else
				{
					this.OffsetPosition = setupParameter.MainOffset;
				}
				if (setupParameter.MoveType == ChaoMovementType.RADICON)
				{
					this.OffsetPosition += this.m_offsetRadicon;
				}
			}
		}

		private void CreateHovering(ChaoSetupParameterData setupParameter)
		{
			ChaoHoverType hoverType = setupParameter.HoverType;
			if (hoverType == ChaoHoverType.CHAO)
			{
				this.CreateChaoHover(setupParameter);
			}
		}

		private void CreateChaoHover(ChaoSetupParameterData setupParameter)
		{
			ChaoHovering chaoHovering = base.gameObject.AddComponent<ChaoHovering>();
			ChaoHovering.CInfo cInfo = new ChaoHovering.CInfo(this);
			cInfo.height = setupParameter.HoverHeight;
			cInfo.speed = setupParameter.HoverSpeed;
			ChaoType chaoType = ChaoUtility.GetChaoType(base.gameObject);
			ChaoType chaoType2 = chaoType;
			if (chaoType2 != ChaoType.MAIN)
			{
				if (chaoType2 == ChaoType.SUB)
				{
					cInfo.startAngle = setupParameter.HoverStartDegreeSub * 0.0174532924f;
				}
			}
			else
			{
				cInfo.startAngle = setupParameter.HoverStartDegreeMain * 0.0174532924f;
			}
			chaoHovering.Setup(cInfo);
			this.SetHoveringMove(chaoHovering);
		}

		public T GetCurrentState<T>() where T : FSMState<ChaoMovement>
		{
			if (this.m_fsm == null)
			{
				return (T)((object)null);
			}
			return this.m_fsm.CurrentState as T;
		}

		public float GetPlayerDisplacement()
		{
			return Vector3.Distance(this.PlayerPosition, this.PrevPlayerPosition);
		}

		private void SetHoveringMove(ChaoHoveringBase hovering)
		{
			this.m_hoveringMove = hovering;
		}

		public void ResetHovering()
		{
			if (this.m_hoveringMove != null)
			{
				this.m_hoveringMove.Reset();
			}
		}
	}
}
