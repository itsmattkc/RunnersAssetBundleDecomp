using App.Utility;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterMovement : MonoBehaviour
	{
		private enum Status
		{
			OnGround,
			OnRunPath,
			IgnoreCollision,
			ThroughBroken,
			RayPos_Dirty
		}

		public enum HitType
		{
			Down,
			Up,
			Front,
			NUM
		}

		private const float PlayerGravitySize = 45f;

		private const float maxLengthOnSearchGround = 30f;

		private Vector3[] m_dir = new Vector3[3];

		private Vector3[] m_rayOffset = new Vector3[3];

		private FSMSystem<CharacterMovement> m_fsm;

		private Vector3 m_velocity;

		private Vector3 m_displacement;

		private Vector3 m_prevRayPosition;

		private float m_distanceToGround;

		private StageBlockPathManager m_blockPathManager;

		private bool m_alreadySetup;

		private readonly float m_enableLandCos = Mathf.Cos(0.7853982f);

		public bool m_doneFixedUpdate;

		private PlayerInformation m_information;

		private bool m_dispInfo;

		private HitInfo[] m_hitInfo = new HitInfo[3];

		private HitInfo m_sweepHitInfo = default(HitInfo);

		private float m_gravitySize = 45f;

		private Vector3 m_gravityDir = -Vector3.up;

		private Vector3 m_groundUpDir = Vector3.up;

		private Bitset32 m_status;

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

		public Vector3 HorzVelocity
		{
			get
			{
				return this.m_velocity - Vector3.Project(this.m_velocity, base.transform.up);
			}
			set
			{
				this.m_velocity = value + this.VertVelocity;
			}
		}

		public Vector3 VertVelocity
		{
			get
			{
				return Vector3.Project(this.m_velocity, base.transform.up);
			}
			set
			{
				this.m_velocity = value + this.HorzVelocity;
			}
		}

		public float EnableLandCos
		{
			get
			{
				return this.m_enableLandCos;
			}
		}

		public Vector3 RaycastCheckPosition
		{
			get
			{
				return this.m_prevRayPosition;
			}
		}

		public Vector3 SideViewPathPos
		{
			get
			{
				return this.m_information.SideViewPathPos;
			}
		}

		public bool ThroughBreakable
		{
			get
			{
				return this.m_status.Test(3);
			}
			set
			{
				this.m_status.Set(3, value);
			}
		}

		public float DistanceToGround
		{
			get
			{
				return this.m_distanceToGround;
			}
		}

		public Vector3 GroundUpDirection
		{
			get
			{
				return this.m_groundUpDir;
			}
		}

		public CharacterParameterData Parameter
		{
			get
			{
				return base.GetComponent<CharacterParameter>().GetData();
			}
		}

		private void Start()
		{
			this.SetupOnStart();
		}

		public void SetupOnStart()
		{
			if (this.m_alreadySetup)
			{
				return;
			}
			this.m_prevRayPosition = base.transform.position;
			this.m_gravityDir = -Vector3.up;
			this.m_hitInfo = new HitInfo[3];
			this.m_sweepHitInfo = default(HitInfo);
			if (this.m_fsm == null)
			{
				this.m_fsm = new FSMSystem<CharacterMovement>();
				FSMStateFactory<CharacterMovement>[] stateTable = CharacterMovement.GetStateTable();
				FSMStateFactory<CharacterMovement>[] array = stateTable;
				for (int i = 0; i < array.Length; i++)
				{
					FSMStateFactory<CharacterMovement> stateFactory = array[i];
					this.m_fsm.AddState(stateFactory);
				}
				this.m_fsm.Init(this, 2);
			}
			if (this.m_blockPathManager == null)
			{
				this.m_blockPathManager = GameObjectUtil.FindGameObjectComponent<StageBlockPathManager>("StageBlockManager");
			}
			if (this.m_information == null)
			{
				GameObject gameObject = GameObject.Find("PlayerInformation");
				this.m_information = gameObject.GetComponent<PlayerInformation>();
			}
			this.m_alreadySetup = true;
		}

		private void LateUpdate()
		{
			this.m_information.SetMovementUpdated(false);
		}

		private void FixedUpdate()
		{
			this.m_sweepHitInfo.Reset();
			this.m_status.Set(4, false);
			Vector3 position = base.transform.position;
			if (this.m_fsm != null && this.m_fsm.CurrentState != null)
			{
				this.m_fsm.CurrentState.Step(this, Time.deltaTime);
			}
			this.m_displacement = base.transform.position - position;
			if (!this.m_status.Test(4))
			{
				this.m_prevRayPosition = base.transform.position + base.transform.up * 0.1f;
			}
			this.UpdateHitInfo();
			this.m_information.SetMovementUpdated(true);
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
			if (this.m_fsm != null && this.m_fsm.CurrentStateID != (StateID)state)
			{
				this.m_fsm.ChangeState(this, (int)state);
			}
		}

		public T GetCurrentState<T>() where T : FSMState<CharacterMovement>
		{
			if (this.m_fsm == null)
			{
				return (T)((object)null);
			}
			return this.m_fsm.CurrentState as T;
		}

		private void UpdateHitInfo()
		{
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			float num = 0.1f;
			float distance = 0.1f + num;
			CapsuleCollider component = base.GetComponent<CapsuleCollider>();
			float d = 0f;
			float d2 = 0f;
			Vector3 direction = Vector3.zero;
			if (component)
			{
				d = component.height;
				d2 = component.radius;
				direction = component.center;
			}
			this.m_dir[0] = -base.transform.up;
			this.m_dir[1] = up;
			this.m_dir[2] = base.transform.forward;
			this.m_rayOffset[0] = Vector3.zero;
			this.m_rayOffset[1] = this.m_dir[1] * d;
			this.m_rayOffset[2] = component.transform.TransformDirection(direction) + this.m_dir[2] * d2;
			for (int i = 0; i < 3; i++)
			{
				Vector3 vector = this.m_dir[i];
				Vector3 origin = position + this.m_rayOffset[i] - vector * num;
				RaycastHit hit;
				if (Physics.Raycast(origin, vector, out hit, distance))
				{
					this.m_hitInfo[i].Set(hit);
				}
				else
				{
					this.m_hitInfo[i].Reset();
				}
			}
			if (!this.m_hitInfo[0].IsValid() && this.m_sweepHitInfo.IsValid())
			{
				Vector3 rhs = up;
				float num2 = Vector3.Dot(this.m_sweepHitInfo.info.normal, rhs);
				if (num2 > this.m_enableLandCos)
				{
					this.m_hitInfo[0].Set(this.m_sweepHitInfo.info);
				}
			}
			this.m_status.Set(0, this.m_hitInfo[0].valid);
			if (this.IsOnGround())
			{
				this.m_groundUpDir = this.m_hitInfo[0].info.normal;
				this.m_distanceToGround = 0f;
			}
			else
			{
				this.m_groundUpDir = -this.GetGravityDir();
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position, this.GetGravityDir(), out raycastHit, 30f))
				{
					this.m_distanceToGround = raycastHit.distance;
				}
				else
				{
					this.m_distanceToGround = 30f;
				}
			}
		}

		public void SetRaycastCheckPosition(Vector3 pos)
		{
			this.m_prevRayPosition = pos;
			this.m_status.Set(4, true);
		}

		public bool GetHitInfo(CharacterMovement.HitType type, out HitInfo info)
		{
			info = this.m_hitInfo[(int)type];
			return info.valid;
		}

		public bool IsHit(CharacterMovement.HitType type)
		{
			return this.m_hitInfo[(int)type].IsValid();
		}

		public void SetSweepHitInfo(HitInfo info)
		{
			this.m_sweepHitInfo = info;
		}

		public bool GetGroundInfo(out HitInfo info)
		{
			info = this.m_hitInfo[0];
			return this.m_hitInfo[0].valid;
		}

		public bool IsOnGround()
		{
			return this.m_status.Test(0);
		}

		public void OffGround()
		{
			this.m_status.Set(0, false);
			this.m_hitInfo[0].Reset();
			this.m_groundUpDir = -this.GetGravityDir();
		}

		public void ResetPosition(Vector3 pos)
		{
			base.transform.position = pos;
			this.m_prevRayPosition = pos;
		}

		public void ResetRotation(Quaternion rot)
		{
			base.transform.rotation = rot;
		}

		public void SetLookRotation(Vector3 front, Vector3 up)
		{
			Quaternion identity = Quaternion.identity;
			identity.SetLookRotation(front, up);
			base.transform.rotation = identity;
		}

		public float GetVertVelocityScalar()
		{
			return Vector3.Dot(this.m_velocity, base.transform.up);
		}

		public float GetForwardVelocityScalar()
		{
			return Vector3.Dot(this.m_velocity, this.GetForwardDir());
		}

		public Vector3 GetForwardDir()
		{
			return base.transform.forward;
		}

		public Vector3 GetUpDir()
		{
			return base.transform.up;
		}

		public Vector3 GetGravity()
		{
			return this.m_gravityDir * this.m_gravitySize;
		}

		public Vector3 GetGravityDir()
		{
			return this.m_gravityDir;
		}

		public Vector3 GetDisplacement()
		{
			return this.m_displacement;
		}

		public StageBlockPathManager GetBlockPathManager()
		{
			return this.m_blockPathManager;
		}

		private static FSMStateFactory<CharacterMovement>[] GetStateTable()
		{
			return new FSMStateFactory<CharacterMovement>[]
			{
				new FSMStateFactory<CharacterMovement>(2, new CharacterMoveRun()),
				new FSMStateFactory<CharacterMovement>(3, new CharacterMoveAir()),
				new FSMStateFactory<CharacterMovement>(4, new CharacterMoveIgnoreCollision()),
				new FSMStateFactory<CharacterMovement>(5, new CharacterMoveOnPath()),
				new FSMStateFactory<CharacterMovement>(6, new CharacterMoveTarget()),
				new FSMStateFactory<CharacterMovement>(7, new CharacterMoveOnPathPhantom()),
				new FSMStateFactory<CharacterMovement>(8, new CharacterMoveTargetBoss()),
				new FSMStateFactory<CharacterMovement>(9, new CharacterMoveOnPathPhantomDrill()),
				new FSMStateFactory<CharacterMovement>(10, new CharacterMoveAsteroid())
			};
		}
	}
}
