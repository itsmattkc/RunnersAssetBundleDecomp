using System;
using UnityEngine;

namespace Player
{
	public class CharacterCheckTrickJump : MonoBehaviour
	{
		private bool m_touched;

		public bool IsTouched
		{
			get
			{
				return this.m_touched;
			}
		}

		private void Update()
		{
			CharacterInput component = base.GetComponent<CharacterInput>();
			if (component != null && component.IsTouched())
			{
				this.m_touched = true;
			}
		}

		public void Reset()
		{
			this.m_touched = false;
		}

		private void OnEnable()
		{
			this.Reset();
		}
	}
}
