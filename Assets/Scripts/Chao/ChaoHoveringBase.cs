using System;
using UnityEngine;

namespace Chao
{
	public class ChaoHoveringBase : MonoBehaviour
	{
		public class CInfoBase
		{
			public ChaoMovement movement;

			protected CInfoBase(ChaoMovement movement_)
			{
				this.movement = movement_;
			}
		}

		private ChaoMovement m_movement;

		private Vector3 m_position;

		public Vector3 Position
		{
			get
			{
				return this.m_position;
			}
			protected set
			{
				this.m_position = value;
			}
		}

		public ChaoMovement Movement
		{
			get
			{
				return this.m_movement;
			}
		}

		public void Setup(ChaoHoveringBase.CInfoBase cinfo)
		{
			this.m_movement = cinfo.movement;
			this.SetupImpl(cinfo);
		}

		protected virtual void SetupImpl(ChaoHoveringBase.CInfoBase info)
		{
		}

		public virtual void Reset()
		{
		}
	}
}
