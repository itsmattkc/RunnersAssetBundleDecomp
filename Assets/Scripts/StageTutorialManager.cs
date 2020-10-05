using Message;
using System;
using Tutorial;
using UnityEngine;

public class StageTutorialManager : MonoBehaviour
{
	private enum Mode
	{
		Idle,
		PlayStart,
		PlayEnd
	}

	private enum EventState : uint
	{
		Clear = 1u,
		Damage,
		Miss = 4u
	}

	public bool m_debugDraw;

	private bool m_debugSkip;

	private static float RetryOffsetPos = 10f;

	private StageTutorialManager.Mode m_mode;

	private uint m_eventState;

	private int m_currentEventID;

	private bool m_complete;

	private Vector3 m_startPos = Vector3.zero;

	private TempTutorialScore m_tempScore;

	private static StageTutorialManager instance;

	public EventID CurrentEventID
	{
		get
		{
			return (EventID)this.m_currentEventID;
		}
	}

	public static StageTutorialManager Instance
	{
		get
		{
			return StageTutorialManager.instance;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	public void SetupTutorial()
	{
		this.DebugDraw("SetupTutorial");
		this.m_tempScore = new TempTutorialScore();
		EventID id = EventID.JUMP;
		this.StartTutorial(id);
		this.m_mode = StageTutorialManager.Mode.Idle;
	}

	private void StartTutorial(EventID id)
	{
		this.DebugDraw("StartTutorial id=" + id.ToString());
		this.m_currentEventID = (int)id;
		this.m_eventState = 0u;
		this.m_complete = false;
		if (this.m_currentEventID == 9)
		{
			this.CompleteTutorial();
		}
	}

	private void CompleteTutorial()
	{
		this.DebugDraw("CompleteTutorial");
		this.m_currentEventID = 9;
		this.m_complete = true;
	}

	public void OnMsgTutorialStart(MsgTutorialStart msg)
	{
		if (this.IsCompletedTutorial())
		{
			return;
		}
		if (this.m_mode == StageTutorialManager.Mode.Idle)
		{
			this.m_startPos = msg.m_pos;
			this.m_eventState = 0u;
			this.GetTempScore();
			MsgTutorialPlayStart value = new MsgTutorialPlayStart((EventID)this.m_currentEventID);
			GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialPlayStart", value, SendMessageOptions.DontRequireReceiver);
			this.DebugDraw("OnMsgTutorialStart m_startPos=" + this.m_startPos);
			this.m_mode = StageTutorialManager.Mode.PlayStart;
		}
	}

	public void OnMsgTutorialEnd(MsgTutorialEnd msg)
	{
		if (this.IsCompletedTutorial())
		{
			return;
		}
		if (this.m_mode == StageTutorialManager.Mode.PlayStart)
		{
			bool flag = this.CheckNextTutorial();
			Vector3 vector = (!flag) ? this.GetNextStartCollisionPosition() : this.m_startPos;
			vector -= Vector3.right * StageTutorialManager.RetryOffsetPos;
			Quaternion identity = Quaternion.identity;
			MsgTutorialPlayEnd value = new MsgTutorialPlayEnd(this.IsCompletedTutorial(), flag, (EventID)this.m_currentEventID, vector, identity);
			GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialPlayEnd", value, SendMessageOptions.DontRequireReceiver);
			this.DebugDraw("OnMsgTutorialEnd nextStartPos=" + vector);
			this.m_mode = StageTutorialManager.Mode.PlayEnd;
		}
	}

	public void OnMsgTutorialDebugEnd(MsgTutorialEnd msg)
	{
	}

	public void OnMsgTutorialNext(MsgTutorialNext msg)
	{
		if (this.m_mode == StageTutorialManager.Mode.PlayEnd)
		{
			if (this.IsCompletedTutorial())
			{
				GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnBossEnd", new MsgBossEnd(true), SendMessageOptions.DontRequireReceiver);
			}
			this.DebugDraw("OnMsgTutorialNext");
			this.m_mode = StageTutorialManager.Mode.Idle;
		}
	}

	public void OnMsgTutorialClear(MsgTutorialClear msg)
	{
		if (this.IsCompletedTutorial() && !this.IsPlayStart())
		{
			return;
		}
		foreach (MsgTutorialClear.Data current in msg.m_data)
		{
			if (current.eventid == (EventID)this.m_currentEventID)
			{
				this.DebugDraw("OnMsgTutorialClear id=" + current.eventid.ToString());
				this.SetEventState(StageTutorialManager.EventState.Clear);
			}
		}
	}

	public void OnMsgTutorialDamage(MsgTutorialDamage msg)
	{
		if (this.IsCompletedTutorial() && !this.IsPlayStart())
		{
			return;
		}
		this.SetEventState(StageTutorialManager.EventState.Damage);
		EventClearType eventClearType = Tutorial.EventData.GetEventClearType((EventID)this.m_currentEventID);
		if (eventClearType == EventClearType.NO_DAMAGE)
		{
			this.OnMsgTutorialEnd(new MsgTutorialEnd());
		}
	}

	public void OnMsgTutorialMiss(MsgTutorialMiss msg)
	{
		if (this.IsCompletedTutorial() && !this.IsPlayStart())
		{
			return;
		}
		this.SetEventState(StageTutorialManager.EventState.Miss);
		this.OnMsgTutorialEnd(new MsgTutorialEnd());
	}

	public bool IsCompletedTutorial()
	{
		return this.m_complete;
	}

	private bool IsClearTutorial()
	{
		if (this.IsCompletedTutorial())
		{
			return true;
		}
		if (this.m_currentEventID == 9)
		{
			return true;
		}
		switch (Tutorial.EventData.GetEventClearType((EventID)this.m_currentEventID))
		{
		case EventClearType.CLEAR:
			if (this.IsEventState(StageTutorialManager.EventState.Clear))
			{
				return true;
			}
			break;
		case EventClearType.NO_DAMAGE:
			if (!this.IsEventState(StageTutorialManager.EventState.Miss) && !this.IsEventState(StageTutorialManager.EventState.Damage))
			{
				return true;
			}
			break;
		case EventClearType.NO_MISS:
			if (!this.IsEventState(StageTutorialManager.EventState.Miss))
			{
				return true;
			}
			break;
		}
		return false;
	}

	private bool CheckNextTutorial()
	{
		bool result = false;
		int num = this.m_currentEventID;
		if (this.IsClearTutorial())
		{
			num++;
			if (num >= 8)
			{
				this.DebugDraw("CheckNextTutorial Complete!");
				this.CompleteTutorial();
			}
			else
			{
				this.DebugDraw("CheckNextTutorial Clear!");
				this.StartTutorial((EventID)num);
			}
		}
		else
		{
			this.DebugDraw("CheckNextTutorial Miss!");
			this.StartTutorial((EventID)num);
			result = true;
		}
		return result;
	}

	private void SetEventState(StageTutorialManager.EventState state)
	{
		this.m_eventState |= (uint)state;
	}

	private bool IsEventState(StageTutorialManager.EventState state)
	{
		return (this.m_eventState & (uint)state) != 0u;
	}

	private bool IsPlayStart()
	{
		return this.m_mode == StageTutorialManager.Mode.PlayStart;
	}

	private Vector3 GetNextStartCollisionPosition()
	{
		Vector3 vector = Vector3.zero;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Gimmick");
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject gameObject = array2[i];
			if (gameObject.GetComponent<ObjTutorialStartCollision>() && this.m_startPos.x + 1f < gameObject.transform.position.x)
			{
				if (vector == Vector3.zero)
				{
					vector = gameObject.transform.position;
				}
				else if (vector.x > gameObject.transform.position.x)
				{
					vector = gameObject.transform.position;
				}
			}
		}
		if (vector == Vector3.zero)
		{
			vector = this.m_startPos;
		}
		return vector;
	}

	private void GetTempScore()
	{
		if (this.m_tempScore == null)
		{
			return;
		}
		MsgTutorialGetRingNum msgTutorialGetRingNum = new MsgTutorialGetRingNum();
		GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgTutorialGetRingNum", msgTutorialGetRingNum, SendMessageOptions.DontRequireReceiver);
		this.m_tempScore.m_ring = msgTutorialGetRingNum.m_ring;
		StageScoreManager stageScoreManager = StageScoreManager.Instance;
		if (stageScoreManager != null)
		{
			this.m_tempScore.m_stkRing = (int)stageScoreManager.Ring;
			this.m_tempScore.m_score = (int)stageScoreManager.Score;
			this.m_tempScore.m_animal = (int)stageScoreManager.Animal;
		}
		PlayerInformation playerInformation = ObjUtil.GetPlayerInformation();
		if (playerInformation != null)
		{
			this.m_tempScore.m_distance = playerInformation.TotalDistance;
		}
		this.DebugDraw(string.Concat(new object[]
		{
			"GetTempScore score=",
			this.m_tempScore.m_score,
			" ring=",
			this.m_tempScore.m_ring,
			" animal=",
			this.m_tempScore.m_animal,
			"distance=",
			this.m_tempScore.m_distance
		}));
	}

	public void OnMsgTutorialResetForRetry(MsgTutorialResetForRetry msg)
	{
		if (this.m_tempScore == null)
		{
			return;
		}
		MsgTutorialResetForRetry value = new MsgTutorialResetForRetry(this.m_tempScore.m_ring, msg.m_blink);
		GameObjectUtil.SendMessageToTagObjects("Player", "OnMsgTutorialResetForRetry", value, SendMessageOptions.DontRequireReceiver);
		MsgResetScore msg2 = new MsgResetScore(this.m_tempScore.m_score, this.m_tempScore.m_animal, this.m_tempScore.m_stkRing);
		if (StageScoreManager.Instance != null)
		{
			StageScoreManager.Instance.ResetScore(msg2);
		}
		PlayerInformation playerInformation = ObjUtil.GetPlayerInformation();
		if (playerInformation != null)
		{
			playerInformation.TotalDistance = this.m_tempScore.m_distance - StageTutorialManager.RetryOffsetPos - 1.05f;
		}
		this.DebugDraw(string.Concat(new object[]
		{
			"SetTempScore score=",
			this.m_tempScore.m_score,
			" ring=",
			this.m_tempScore.m_ring,
			" animal=",
			this.m_tempScore.m_animal,
			"distance=",
			this.m_tempScore.m_distance
		}));
	}

	private void DebugDraw(string msg)
	{
	}

	private void OnMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
	}

	protected bool CheckInstance()
	{
		if (StageTutorialManager.instance == null)
		{
			StageTutorialManager.instance = this;
			return true;
		}
		if (this == StageTutorialManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (StageTutorialManager.instance == this)
		{
			StageTutorialManager.instance = null;
		}
	}
}
