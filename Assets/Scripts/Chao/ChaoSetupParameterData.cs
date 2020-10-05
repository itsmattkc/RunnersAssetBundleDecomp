using System;
using UnityEngine;

namespace Chao
{
	[Serializable]
	public class ChaoSetupParameterData
	{
		private const float DefaultHoverSpeedDegree = 180f;

		private const float DefaultHoverHeight = 0.2f;

		private const float DefaultHoverStartDegreeMain = 0f;

		private const float DefaultHoverStartDegreeSub = 160f;

		[SerializeField]
		private Vector3 m_mainOffset = new Vector3(-1.1f, 1.4f, 0f);

		[SerializeField]
		private Vector3 m_subOffset = new Vector3(-1.9f, 1f, 0f);

		[SerializeField]
		private float m_colliRadius = 0.3f;

		[SerializeField]
		private Vector3 m_colliCenter = new Vector3(0f, 0.13f, -0.05f);

		[SerializeField]
		private ChaoMovementType m_movementType;

		[SerializeField]
		private ChaoHoverType m_hoverType = ChaoHoverType.CHAO;

		[SerializeField]
		private bool m_useCustomHoverParam;

		[SerializeField]
		private float m_hoverSpeedDegree = 180f;

		[SerializeField]
		private float m_hoverHeight = 0.2f;

		[SerializeField]
		private float m_hoverStartDegreeMain;

		[SerializeField]
		private float m_hoverStartDegreeSub = 160f;

		[SerializeField]
		private ShaderType m_shaderType;

		public Vector3 MainOffset
		{
			get
			{
				return this.m_mainOffset;
			}
		}

		public Vector3 SubOffset
		{
			get
			{
				return this.m_subOffset;
			}
		}

		public float ColliRadius
		{
			get
			{
				return this.m_colliRadius;
			}
			private set
			{
				this.m_colliRadius = value;
			}
		}

		public Vector3 ColliCenter
		{
			get
			{
				return this.m_colliCenter;
			}
			private set
			{
				this.m_colliCenter = value;
			}
		}

		public ChaoMovementType MoveType
		{
			get
			{
				return this.m_movementType;
			}
			private set
			{
				this.m_movementType = value;
			}
		}

		public ChaoHoverType HoverType
		{
			get
			{
				return this.m_hoverType;
			}
		}

		public bool UseCustomHoverParam
		{
			get
			{
				return this.m_useCustomHoverParam;
			}
		}

		public float HoverSpeed
		{
			get
			{
				if (this.m_useCustomHoverParam)
				{
					return this.m_hoverSpeedDegree;
				}
				return 180f;
			}
		}

		public float HoverHeight
		{
			get
			{
				if (this.m_useCustomHoverParam)
				{
					return this.m_hoverHeight;
				}
				return 0.2f;
			}
		}

		public float HoverStartDegreeMain
		{
			get
			{
				if (this.m_useCustomHoverParam)
				{
					return this.m_hoverStartDegreeMain;
				}
				return 0f;
			}
		}

		public float HoverStartDegreeSub
		{
			get
			{
				if (this.m_useCustomHoverParam)
				{
					return this.m_hoverStartDegreeSub;
				}
				return 160f;
			}
		}

		public ShaderType ShaderOffset
		{
			get
			{
				return this.m_shaderType;
			}
		}

		public void CopyTo(ChaoSetupParameterData dst)
		{
			dst.ColliRadius = this.ColliRadius;
			dst.ColliCenter = this.ColliCenter;
			dst.MoveType = this.MoveType;
			dst.m_mainOffset = this.m_mainOffset;
			dst.m_subOffset = this.m_subOffset;
			dst.m_hoverType = this.m_hoverType;
			dst.m_useCustomHoverParam = this.m_useCustomHoverParam;
			dst.m_hoverSpeedDegree = this.m_hoverSpeedDegree;
			dst.m_hoverHeight = this.m_hoverHeight;
			dst.m_hoverStartDegreeMain = this.m_hoverStartDegreeMain;
			dst.m_hoverStartDegreeSub = this.m_hoverStartDegreeSub;
			dst.m_shaderType = this.m_shaderType;
		}
	}
}
