using GameScore;
using System;
using UnityEngine;

public class ObjFriendSign : SpawnableObject
{
	private enum Mode
	{
		Idle,
		Start,
		Rot,
		End
	}

	private const string ModelName = "obj_cmn_friendsign";

	private const float END_TIME = 5f;

	private const float START_SPEED = 100f;

	private ObjFriendSign.Mode m_mode;

	private float m_time;

	private float m_speed;

	private float m_rot;

	protected override string GetModelName()
	{
		return "obj_cmn_friendsign";
	}

	protected override ResourceCategory GetModelCategory()
	{
		return ResourceCategory.OBJECT_RESOURCE;
	}

	protected override void OnSpawned()
	{
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		switch (this.m_mode)
		{
		case ObjFriendSign.Mode.Start:
			this.m_rot = 0f;
			this.m_speed = 100f;
			this.m_mode = ObjFriendSign.Mode.Rot;
			break;
		case ObjFriendSign.Mode.Rot:
		{
			float num = 60f * deltaTime * this.m_speed;
			float num2 = this.m_rot + num;
			if (num2 < 360f)
			{
				this.m_rot += num;
			}
			else
			{
				num = 360f - this.m_rot;
				this.m_speed -= this.m_speed * 0.5f;
				this.m_rot = 0f;
				if (this.m_speed < 3f)
				{
					this.m_time = 0f;
					this.m_mode = ObjFriendSign.Mode.End;
				}
			}
			base.transform.rotation = Quaternion.Euler(0f, num, 0f) * base.transform.rotation;
			break;
		}
		case ObjFriendSign.Mode.End:
			this.m_time += deltaTime;
			if (this.m_time > 5f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				this.m_mode = ObjFriendSign.Mode.Idle;
			}
			break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				this.HitFriendSign();
			}
		}
	}

	private void HitFriendSign()
	{
		if (this.m_mode == ObjFriendSign.Mode.Idle)
		{
			ObjUtil.SendMessageAddScore(Data.FriendSign);
			ObjUtil.SendMessageScoreCheck(new StageScoreData(5, Data.FriendSign));
			ObjUtil.PlaySE("obj_item_friendsign", "SE");
			this.m_mode = ObjFriendSign.Mode.Start;
		}
	}

	public void ChangeTexture(Texture tex)
	{
		if (tex)
		{
			MeshRenderer meshRenderer = GameObjectUtil.FindChildGameObjectComponent<MeshRenderer>(base.gameObject, "obj_cmn_friendsignpicture");
			if (meshRenderer)
			{
				meshRenderer.material.mainTexture = tex;
			}
		}
	}
}
