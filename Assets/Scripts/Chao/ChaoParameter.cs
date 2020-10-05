using DataTable;
using SaveData;
using System;
using UnityEngine;

namespace Chao
{
	[Serializable]
	public class ChaoParameter : MonoBehaviour
	{
		[SerializeField]
		private int m_mainChaoId = -1;

		[SerializeField]
		private int m_mainChaoLevel;

		[SerializeField]
		private int m_subChaoId = -1;

		[SerializeField]
		private int m_subChaoLevel;

		public ChaoParameterData m_data;

		public ChaoParameterData Data
		{
			get
			{
				return this.m_data;
			}
			set
			{
				this.m_data = value;
			}
		}

		private void Start()
		{
			base.enabled = false;
		}

		public bool IsChaoParameterDataDebugFlag()
		{
			return false;
		}

		private void SetDebugChao()
		{
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				int setChaoId = this.GetSetChaoId(instance.PlayerData, true);
				if (setChaoId >= 0)
				{
					this.m_mainChaoId = setChaoId;
					this.m_mainChaoLevel = this.GetChaoLevel(instance.ChaoData, this.m_mainChaoId);
				}
				else
				{
					this.SetMainChao(instance.PlayerData, instance.ChaoData);
				}
				int setChaoId2 = this.GetSetChaoId(instance.PlayerData, false);
				if (setChaoId2 >= 0)
				{
					this.m_subChaoId = setChaoId2;
					this.m_subChaoLevel = this.GetChaoLevel(instance.ChaoData, this.m_subChaoId);
				}
				else
				{
					this.SetSubChao(instance.PlayerData, instance.ChaoData);
				}
			}
		}

		private void SetMainChao(PlayerData playerData, SaveData.ChaoData chaoData)
		{
			if (playerData != null)
			{
				playerData.MainChaoID = this.m_mainChaoId;
				this.m_mainChaoLevel = Mathf.Clamp(this.m_mainChaoLevel, 0, ChaoTable.ChaoMaxLevel());
				this.SetChaoLevel(chaoData, this.m_mainChaoId, this.m_mainChaoLevel);
			}
		}

		private void SetSubChao(PlayerData playerData, SaveData.ChaoData chaoData)
		{
			if (playerData != null)
			{
				playerData.SubChaoID = this.m_subChaoId;
				this.m_subChaoLevel = Mathf.Clamp(this.m_subChaoLevel, 0, ChaoTable.ChaoMaxLevel());
				this.SetChaoLevel(chaoData, this.m_subChaoId, this.m_subChaoLevel);
			}
		}

		private void SetChaoLevel(SaveData.ChaoData chaoData, int chaoId, int level)
		{
			if (chaoData != null)
			{
				for (int i = 0; i < chaoData.CHAO_MAX_NUM; i++)
				{
					if (chaoData.Info[i].chao_id == -1)
					{
						chaoData.Info[i].chao_id = chaoId;
						chaoData.Info[i].level = level;
						break;
					}
				}
			}
		}

		private int GetChaoLevel(SaveData.ChaoData chaoData, int chaoId)
		{
			if (chaoData != null)
			{
				for (int i = 0; i < chaoData.CHAO_MAX_NUM; i++)
				{
					if (chaoData.Info[i].chao_id == chaoId)
					{
						return chaoData.Info[i].level;
					}
				}
			}
			return 0;
		}

		private int GetSetChaoId(PlayerData playerData, bool mainChaoFlag)
		{
			if (playerData == null)
			{
				return -1;
			}
			if (mainChaoFlag)
			{
				return playerData.MainChaoID;
			}
			return playerData.SubChaoID;
		}
	}
}
