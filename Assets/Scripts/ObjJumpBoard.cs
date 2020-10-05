using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjJumpBoard")]
public class ObjJumpBoard : SpawnableObject
{
	private enum State
	{
		Idle,
		Hit,
		Jump
	}

	private const string ModelName = "obj_cmn_jumpboard";

	private ObjJumpBoard.State m_state;

	private ObjJumpBoardParameter m_param;

	protected override string GetModelName()
	{
		return "obj_cmn_jumpboard";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override void OnSpawned()
	{
		ObjUtil.StopAnimation(base.gameObject);
	}

	public void SetObjJumpBoardParameter(ObjJumpBoardParameter param)
	{
		this.m_param = param;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.m_state == ObjJumpBoard.State.Idle)
		{
			MsgOnJumpBoardHit value = new MsgOnJumpBoardHit(base.transform.position, base.transform.rotation);
			other.gameObject.SendMessage("OnJumpBoardHit", value, SendMessageOptions.DontRequireReceiver);
			this.m_state = ObjJumpBoard.State.Hit;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (this.m_state == ObjJumpBoard.State.Hit)
		{
			Quaternion rot = Quaternion.Euler(0f, 0f, -this.m_param.m_succeedAngle) * base.transform.rotation;
			Quaternion rot2 = Quaternion.Euler(0f, 0f, -this.m_param.m_missAngle) * base.transform.rotation;
			Vector3 pos = base.transform.position + base.transform.up * 0.25f;
			MsgOnJumpBoardJump msgOnJumpBoardJump = new MsgOnJumpBoardJump(pos, rot, rot2, this.m_param.m_succeedFirstSpeed, this.m_param.m_missFirstSpeed, this.m_param.m_succeedOutOfcontrol, this.m_param.m_missOutOfcontrol);
			other.gameObject.SendMessage("OnJumpBoardJump", msgOnJumpBoardJump, SendMessageOptions.DontRequireReceiver);
			if (msgOnJumpBoardJump.m_succeed)
			{
				Animation componentInChildren = base.GetComponentInChildren<Animation>();
				if (componentInChildren)
				{
					componentInChildren.wrapMode = WrapMode.Once;
					componentInChildren.Play("obj_jumpboard_bounce");
				}
			}
			this.m_state = ObjJumpBoard.State.Jump;
		}
	}
}
