using System;
using UnityEngine;

namespace Player
{
	public class StateEdit : FSMState<CharacterState>
	{
		public float m_moveSpeed = 2f;

		public override void Enter(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.IgnoreCollision);
			Collider[] componentsInChildren = context.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Collider collider = array[i];
				collider.enabled = false;
			}
			Collider component = context.GetComponent<Collider>();
			if (component)
			{
				component.enabled = false;
			}
		}

		public override void Leave(CharacterState context)
		{
			Collider[] componentsInChildren = context.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Collider collider = array[i];
				collider.enabled = true;
			}
			Collider component = context.GetComponent<Collider>();
			if (component)
			{
				component.enabled = true;
			}
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			CharacterInput component = context.GetComponent<CharacterInput>();
			Vector3 b = component.GetStick() * this.m_moveSpeed * Time.deltaTime;
			context.transform.position += b;
			if (Input.GetButtonDown("ButtonX"))
			{
				this.m_moveSpeed *= 2f;
				if (this.m_moveSpeed >= 50f)
				{
					this.m_moveSpeed = 2f;
				}
			}
			if (Input.GetButtonDown("ButtonA"))
			{
				context.ChangeState(STATE_ID.Fall);
				return;
			}
		}

		public override void OnGUI(CharacterState context)
		{
			Rect position = new Rect(10f, 10f, 120f, 30f);
			string text = "Cursor Speed :" + this.m_moveSpeed;
			GUI.TextField(position, text);
		}
	}
}
