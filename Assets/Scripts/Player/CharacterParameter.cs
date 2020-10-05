using System;
using UnityEngine;

namespace Player
{
	[Serializable]
	public class CharacterParameter : MonoBehaviour
	{
		public CharacterParameterData m_data;

		private void Start()
		{
			base.enabled = false;
		}

		public CharacterParameterData GetData()
		{
			return this.m_data;
		}

		public void CopyData(CharacterParameterData data)
		{
			this.m_data = data;
		}
	}
}
