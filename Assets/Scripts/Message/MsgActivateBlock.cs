using System;
using UnityEngine;

namespace Message
{
	public class MsgActivateBlock : MessageBase
	{
		public enum CheckPoint
		{
			None,
			First,
			Internal
		}

		public string m_stageName;

		public int m_blockNo;

		public int m_layerNo;

		public int m_activateID;

		public bool m_replaceStage;

		public Vector3 m_originPosition;

		public Quaternion m_originRotation;

		public MsgActivateBlock.CheckPoint m_checkPoint;

		public MsgActivateBlock(string stageName, int block, int layer, int activateID, Vector3 originPosition, Quaternion originrotation) : base(12299)
		{
			this.m_stageName = stageName;
			this.m_blockNo = block;
			this.m_layerNo = layer;
			this.m_activateID = activateID;
			this.m_originPosition = originPosition;
			this.m_originRotation = originrotation;
			this.m_replaceStage = false;
		}
	}
}
