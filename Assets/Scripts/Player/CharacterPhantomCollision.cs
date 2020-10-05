using System;
using UnityEngine;

namespace Player
{
	public class CharacterPhantomCollision : MonoBehaviour
	{
		private GameObject m_parent;

		private CharacterState m_state;

		private void Start()
		{
			this.m_parent = base.transform.parent.gameObject;
			if (this.m_parent != null)
			{
				this.m_state = this.m_parent.GetComponent<CharacterState>();
			}
		}

		private void OnAddRings(int numRing)
		{
			if (this.m_state != null)
			{
				this.m_state.OnAddRings(numRing);
			}
		}

		private void OnFallingDead()
		{
			if (this.m_state != null)
			{
				this.m_state.OnFallingDead();
			}
		}
	}
}
