using GameScore;
using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjBreak")]
public class ObjBreak : SpawnableObject
{
	private class BreakParam
	{
		public float m_add_x;

		public float m_add_y;

		public float m_rot_speed;

		public BreakParam(float add_x, float add_y, float rot_speed)
		{
			this.m_add_x = add_x;
			this.m_add_y = add_y;
			this.m_rot_speed = rot_speed;
		}
	}

	private const float ADD_SPEED = 0.8f;

	private const float END_TIME = 2.5f;

	private const float BREAK_SPEED = 6f;

	private const float BREAK_GRAVITY = -6.1f;

	private GameObject m_modelObject;

	private static readonly Vector3[] MODEL_ROT_TBL = new Vector3[]
	{
		new Vector3(1f, 1f, 0f),
		new Vector3(0f, 1f, 1f),
		new Vector3(1f, 0f, 1f),
		new Vector3(1f, 1f, 0f)
	};

	private static readonly ObjBreak.BreakParam[] BREAK_PARAM = new ObjBreak.BreakParam[]
	{
		new ObjBreak.BreakParam(0.8f, 1.2f, 15f),
		new ObjBreak.BreakParam(1.5f, 1.4f, 5f),
		new ObjBreak.BreakParam(1f, 1f, 10f),
		new ObjBreak.BreakParam(0.7f, 1.6f, 2f)
	};

	private ObjBreak.BreakParam[] m_breakParam;

	private bool m_break;

	private uint m_model_count;

	private float m_time;

	private float m_move_speed;

	private GameObject m_break_obj;

	private bool m_setup;

	private string m_setup_name = string.Empty;

	public GameObject ModelObject
	{
		get
		{
			return this.m_modelObject;
		}
		set
		{
			if (this.m_modelObject == null)
			{
				this.m_modelObject = value;
				this.m_modelObject.SetActive(true);
			}
		}
	}

	protected override void OnSpawned()
	{
		float num = ObjUtil.GetPlayerAddSpeed();
		if (num < 0f)
		{
			num = 0f;
		}
		this.m_move_speed = 0.8f * num;
	}

	private void Update()
	{
		if (!this.m_setup)
		{
			this.m_setup = this.Setup(this.m_setup_name);
		}
		if (this.m_break)
		{
			this.m_time += Time.deltaTime;
			if (this.m_time > 2.5f)
			{
				this.m_break = false;
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}
	}

	public void SetObjName(string name)
	{
		this.m_setup_name = name;
	}

	private bool Setup(string name)
	{
		if (this.m_break_obj != null)
		{
			return true;
		}
		this.m_break_obj = ObjBreak.CreateBreakModel(base.gameObject, name);
		if (this.m_break_obj != null)
		{
			this.BreakModelVisible(false);
			this.m_model_count = 0u;
			if (this.m_break_obj)
			{
				Component[] componentsInChildren = this.m_break_obj.GetComponentsInChildren<MeshRenderer>(true);
				this.m_model_count = (uint)componentsInChildren.Length;
			}
			if ((ulong)this.m_model_count >= (ulong)((long)ObjBreak.MODEL_ROT_TBL.Length))
			{
				this.m_model_count = (uint)ObjBreak.MODEL_ROT_TBL.Length;
			}
			if ((ulong)this.m_model_count >= (ulong)((long)ObjBreak.BREAK_PARAM.Length))
			{
				this.m_model_count = (uint)ObjBreak.BREAK_PARAM.Length;
			}
			this.m_breakParam = new ObjBreak.BreakParam[this.m_model_count];
			int num = UnityEngine.Random.Range(0, ObjBreak.BREAK_PARAM.Length - 1);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)this.m_model_count))
			{
				int num3 = num + num2;
				if (num3 >= ObjBreak.BREAK_PARAM.Length)
				{
					num3 -= ObjBreak.BREAK_PARAM.Length;
				}
				this.m_breakParam[num2] = new ObjBreak.BreakParam(ObjBreak.BREAK_PARAM[num3].m_add_x + this.m_move_speed, ObjBreak.BREAK_PARAM[num3].m_add_y + this.m_move_speed, ObjBreak.BREAK_PARAM[num3].m_rot_speed);
				num2++;
			}
			return true;
		}
		return false;
	}

	public void OnMsgObjectDead(MsgObjectDead msg)
	{
		if (base.enabled)
		{
			this.SetBroken();
		}
	}

	public void OnMsgStepObjectDead(MsgObjectDead msg)
	{
		if (base.enabled)
		{
			this.SetBroken();
		}
	}

	private void SetPlayerBroken(uint attribute_state)
	{
		int num = Data.Break;
		List<ChaoAbility> abilityList = new List<ChaoAbility>();
		ObjUtil.GetChaoAbliltyPhantomFlag(attribute_state, ref abilityList);
		num = ObjUtil.GetChaoAndEnemyScore(abilityList, num);
		ObjUtil.SendMessageAddScore(num);
		ObjUtil.SendMessageScoreCheck(new StageScoreData(1, num));
		this.SetBroken();
	}

	private void SetBroken()
	{
		if (this.m_break)
		{
			return;
		}
		if (this.m_break_obj && this.m_modelObject)
		{
			this.RootModelVisible(false);
			this.BreakModelVisible(true);
			this.BreakStart();
			ObjUtil.PlayEffectCollisionCenter(base.gameObject, "ef_com_explosion_m01", 1f, false);
			ObjUtil.LightPlaySE("obj_brk", "SE");
			this.m_break = true;
		}
	}

	private void OnDamageHit(MsgHitDamage msg)
	{
		if (this.m_break)
		{
			return;
		}
		if (msg.m_attackPower >= 3 && msg.m_sender)
		{
			GameObject gameObject = msg.m_sender.gameObject;
			if (gameObject)
			{
				MsgHitDamageSucceed value = new MsgHitDamageSucceed(base.gameObject, 0, ObjUtil.GetCollisionCenterPosition(base.gameObject), base.transform.rotation);
				gameObject.SendMessage("OnDamageSucceed", value, SendMessageOptions.DontRequireReceiver);
				this.SetPlayerBroken(msg.m_attackAttribute);
				ObjUtil.CreateBrokenBonus(base.gameObject, gameObject, msg.m_attackAttribute);
			}
		}
	}

	private static GameObject CreateBreakModel(GameObject baseObj, string in_name)
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.STAGE_RESOURCE, in_name + "_brk");
		GameObject gameObject2 = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_PREFAB, "MotorThrow");
		if (gameObject != null && gameObject2 != null)
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject, baseObj.transform.position, baseObj.transform.rotation) as GameObject;
			if (gameObject3)
			{
				gameObject3.gameObject.SetActive(true);
				gameObject3.transform.parent = baseObj.transform;
				for (int i = 0; i < gameObject3.transform.childCount; i++)
				{
					GameObject gameObject4 = gameObject3.transform.GetChild(i).gameObject;
					if (gameObject4)
					{
						GameObject gameObject5 = UnityEngine.Object.Instantiate(gameObject2, baseObj.transform.position, baseObj.transform.rotation) as GameObject;
						if (gameObject5)
						{
							gameObject5.gameObject.SetActive(true);
							gameObject5.transform.parent = gameObject4.transform;
						}
					}
				}
				return gameObject3;
			}
		}
		return null;
	}

	private void BreakModelVisible(bool flag)
	{
		if (this.m_break_obj)
		{
			MeshRenderer component = this.m_break_obj.GetComponent<MeshRenderer>();
			if (component)
			{
				component.enabled = flag;
			}
			Component[] componentsInChildren = this.m_break_obj.GetComponentsInChildren<MeshRenderer>(true);
			Component[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				MeshRenderer meshRenderer = (MeshRenderer)array[i];
				meshRenderer.enabled = flag;
			}
		}
	}

	private void RootModelVisible(bool flag)
	{
		if (this.m_modelObject)
		{
			Component[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>(true);
			Component[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				MeshRenderer meshRenderer = (MeshRenderer)array[i];
				meshRenderer.enabled = flag;
				BoxCollider[] componentsInChildren2 = meshRenderer.gameObject.GetComponentsInChildren<BoxCollider>(true);
				BoxCollider[] array2 = componentsInChildren2;
				for (int j = 0; j < array2.Length; j++)
				{
					BoxCollider boxCollider = array2[j];
					boxCollider.enabled = flag;
				}
			}
		}
	}

	private void BreakStart()
	{
		if (this.m_break_obj)
		{
			for (int i = 0; i < this.m_break_obj.transform.childCount; i++)
			{
				GameObject gameObject = this.m_break_obj.transform.GetChild(i).gameObject;
				if (gameObject)
				{
					for (int j = 0; j < gameObject.transform.childCount; j++)
					{
						GameObject gameObject2 = gameObject.transform.GetChild(j).gameObject;
						if (gameObject2)
						{
							MotorThrow component = gameObject2.GetComponent<MotorThrow>();
							if (component && i < this.m_breakParam.Length)
							{
								component.Setup(new MotorThrow.ThrowParam
								{
									m_obj = gameObject,
									m_speed = 6f,
									m_gravity = -6.1f,
									m_add_x = this.m_breakParam[i].m_add_x,
									m_add_y = this.m_breakParam[i].m_add_y,
									m_rot_speed = this.m_breakParam[i].m_rot_speed,
									m_up = base.transform.up,
									m_forward = base.transform.right,
									m_rot_angle = ObjBreak.MODEL_ROT_TBL[i]
								});
								break;
							}
						}
					}
				}
			}
		}
	}
}
