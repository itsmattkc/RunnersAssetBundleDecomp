using App.Utility;
using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Game/Level")]
public class PlayerInformation : MonoBehaviour
{
	public enum Flags
	{
		OnGround,
		Damaged,
		Dead,
		EnableCharaChange,
		Paraloop,
		LastChance,
		Combo,
		MovementUpdated
	}

	private const int MAX_NUM_RINGS = 99999;

	private string m_mainCharacterName = "Sonic";

	private string m_subCharacterName;

	private int m_mainCharacterID;

	private int m_subCharacterID = -1;

	private CharacterAttribute m_mainCharaAttribute;

	private CharacterAttribute m_subCharaAttribute;

	private TeamAttribute m_mainTeamAttribute;

	private TeamAttribute m_subTeamAttribute;

	private PlayingCharacterType m_playingCharacterType;

	private CharacterAttribute m_attribute;

	private TeamAttribute m_teamAttr;

	private float m_totalDistance;

	private int m_numRings;

	private int m_lostRings;

	private Bitset32 m_flags;

	private Vector3 m_velocity;

	private Vector3 m_horzVelocity;

	private Vector3 m_vertVelocity;

	private float m_defaultSpeed;

	private float m_frontspeed;

	private float m_distanceFromGround;

	private Vector3 m_GravityDir;

	private Vector3 m_upDirection;

	private Vector3 m_pathSideViewPos;

	private Vector3 m_pathSideViewNormal;

	private PhantomType m_phantomType = PhantomType.NONE;

	[SerializeField]
	private PlayerSpeed m_speedLevel;

	[SerializeField]
	private bool m_drawInfo;

	private Rect m_window;

	public float TotalDistance
	{
		get
		{
			return this.m_totalDistance;
		}
		set
		{
			this.m_totalDistance = value;
		}
	}

	public int NumRings
	{
		get
		{
			return this.m_numRings;
		}
	}

	public int NumLostRings
	{
		get
		{
			return this.m_lostRings;
		}
	}

	public Vector3 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	public Quaternion Rotation
	{
		get
		{
			return base.transform.rotation;
		}
	}

	public float FrontSpeed
	{
		get
		{
			return this.m_frontspeed;
		}
	}

	public Vector3 Velocity
	{
		get
		{
			return this.m_velocity;
		}
	}

	public Vector3 HorizonVelocity
	{
		get
		{
			return this.m_horzVelocity;
		}
	}

	public Vector3 VerticalVelocity
	{
		get
		{
			return this.m_vertVelocity;
		}
	}

	public float DefaultSpeed
	{
		get
		{
			return this.m_defaultSpeed;
		}
	}

	public float DistanceFromGround
	{
		get
		{
			return this.m_distanceFromGround;
		}
	}

	public Vector3 GravityDir
	{
		get
		{
			return this.m_GravityDir;
		}
	}

	public Vector3 UpDirection
	{
		get
		{
			return this.m_upDirection;
		}
	}

	public PlayerSpeed SpeedLevel
	{
		get
		{
			return this.m_speedLevel;
		}
	}

	public Vector3 SideViewPathPos
	{
		get
		{
			return this.m_pathSideViewPos;
		}
	}

	public Vector3 SideViewPathNormal
	{
		get
		{
			return this.m_pathSideViewNormal;
		}
	}

	public string MainCharacterName
	{
		get
		{
			return this.m_mainCharacterName;
		}
	}

	public string SubCharacterName
	{
		get
		{
			return this.m_subCharacterName;
		}
	}

	public int MainCharacterID
	{
		get
		{
			return this.m_mainCharacterID;
		}
	}

	public int SubCharacterID
	{
		get
		{
			return this.m_subCharacterID;
		}
	}

	public CharacterAttribute PlayerAttribute
	{
		get
		{
			return this.m_attribute;
		}
	}

	public TeamAttribute PlayerTeamAttribute
	{
		get
		{
			return this.m_teamAttr;
		}
	}

	public PhantomType PhantomType
	{
		get
		{
			return this.m_phantomType;
		}
	}

	public CharacterAttribute MainCharacterAttribute
	{
		get
		{
			return this.m_mainCharaAttribute;
		}
	}

	public CharacterAttribute SubCharacterAttribute
	{
		get
		{
			return this.m_subCharaAttribute;
		}
	}

	public TeamAttribute MainTeamAttribute
	{
		get
		{
			return this.m_mainTeamAttribute;
		}
	}

	public TeamAttribute SubTeamAttribute
	{
		get
		{
			return this.m_subTeamAttribute;
		}
	}

	public PlayingCharacterType PlayingCharaType
	{
		get
		{
			return this.m_playingCharacterType;
		}
	}

	private void Start()
	{
		this.m_flags.Set(0, true);
		this.m_GravityDir = new Vector3(0f, -1f, 0f);
		this.m_upDirection = -this.m_GravityDir;
		if (SaveDataManager.Instance != null)
		{
			CharaType mainChara = SaveDataManager.Instance.PlayerData.MainChara;
			this.m_attribute = CharaTypeUtil.GetCharacterAttribute(mainChara);
			this.m_teamAttr = CharaTypeUtil.GetTeamAttribute(mainChara);
		}
	}

	public bool IsDead()
	{
		return this.m_flags.Test(2);
	}

	public bool IsDamaged()
	{
		return this.m_flags.Test(1);
	}

	public bool IsOnGround()
	{
		return this.m_flags.Test(0);
	}

	public bool IsNotCharaChange()
	{
		return this.m_flags.Test(3);
	}

	public bool IsNowParaloop()
	{
		return this.m_flags.Test(4);
	}

	public bool IsNowLastChance()
	{
		return this.m_flags.Test(5);
	}

	public bool IsNowCombo()
	{
		return this.m_flags.Test(6);
	}

	public bool IsMovementUpdated()
	{
		return this.m_flags.Test(7);
	}

	private void OnUpSpeedLevel(MsgUpSpeedLevel msg)
	{
		this.SetSpeedLevel(msg.m_level);
	}

	public void SetTransform(Transform input)
	{
		base.transform.position = input.position;
		base.transform.rotation = input.rotation;
	}

	public void SetVelocity(Vector3 velocity)
	{
		this.m_velocity = velocity;
	}

	public void SetHorzAndVertVelocity(Vector3 horzVelocity, Vector3 vertVelocity)
	{
		this.m_horzVelocity = horzVelocity;
		this.m_vertVelocity = vertVelocity;
	}

	public void SetDefautlSpeed(float speed)
	{
		this.m_defaultSpeed = speed;
	}

	public void SetFrontSpeed(float speed)
	{
		this.m_frontspeed = speed;
	}

	public void SetNumRings(int numRing)
	{
		this.m_numRings = Mathf.Clamp(numRing, 0, 99999);
	}

	public void AddNumRings(int addRings)
	{
		this.SetNumRings(this.NumRings + addRings);
	}

	public void LostRings()
	{
		this.m_lostRings += this.NumRings;
		this.SetNumRings(0);
	}

	public void SetDistanceToGround(float distance)
	{
		this.m_distanceFromGround = distance;
	}

	public void SetGravityDirection(Vector3 dir)
	{
		this.m_GravityDir = dir;
	}

	public void SetUpDirection(Vector3 dir)
	{
		this.m_upDirection = dir;
	}

	public void AddTotalDistance(float nowDistance)
	{
		this.m_totalDistance += nowDistance;
	}

	public void SetSideViewPath(Vector3 pos, Vector3 normal)
	{
		this.m_pathSideViewPos = pos;
		this.m_pathSideViewNormal = normal;
	}

	private void SetSpeedLevel(PlayerSpeed level)
	{
		this.m_speedLevel = level;
	}

	public void SetPhantomType(PhantomType type)
	{
		this.m_phantomType = type;
	}

	public void SetPlayerAttribute(CharacterAttribute attr, TeamAttribute teamAttr, PlayingCharacterType playingType)
	{
		this.m_attribute = attr;
		this.m_teamAttr = teamAttr;
		this.m_playingCharacterType = playingType;
	}

	public void SetDebugPlayerAttribute(CharaType charaType)
	{
	}

	public void SetDead(bool value)
	{
		this.m_flags.Set(2, value);
	}

	public void SetDamaged(bool value)
	{
		this.m_flags.Set(1, value);
	}

	public void SetOnGround(bool value)
	{
		this.m_flags.Set(0, value);
	}

	public void SetEnableCharaChange(bool value)
	{
		this.m_flags.Set(3, value);
	}

	public void SetParaloop(bool value)
	{
		this.m_flags.Set(4, value);
	}

	public void SetLastChance(bool value)
	{
		this.m_flags.Set(5, value);
	}

	public void SetCombo(bool value)
	{
		this.m_flags.Set(6, value);
	}

	public void SetMovementUpdated(bool value)
	{
		this.m_flags.Set(7, value);
	}

	public void SetPlayerCharacter(int main, int sub)
	{
		if (CharacterDataNameInfo.Instance)
		{
			CharacterDataNameInfo.Info dataByID = CharacterDataNameInfo.Instance.GetDataByID((CharaType)main);
			CharacterDataNameInfo.Info dataByID2 = CharacterDataNameInfo.Instance.GetDataByID((CharaType)sub);
			if (dataByID != null)
			{
				this.m_mainCharacterName = dataByID.m_name;
				this.m_mainCharacterID = (int)dataByID.m_ID;
				this.m_mainCharaAttribute = dataByID.m_attribute;
				this.m_mainTeamAttribute = dataByID.m_teamAttribute;
			}
			if (dataByID2 != null)
			{
				this.m_subCharacterName = dataByID2.m_name;
				this.m_subCharacterID = (int)dataByID2.m_ID;
				this.m_subCharaAttribute = dataByID2.m_attribute;
				this.m_subTeamAttribute = dataByID2.m_teamAttribute;
			}
		}
	}
}
