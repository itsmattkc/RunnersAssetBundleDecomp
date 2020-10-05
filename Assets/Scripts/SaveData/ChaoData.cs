using System;
using System.Collections.Generic;

namespace SaveData
{
	public class ChaoData
	{
		public struct ChaoDataInfo
		{
			public int chao_id;

			public int level;
		}

		public int CHAO_MAX_NUM = 2;

		private ChaoData.ChaoDataInfo[] m_info;

		public ChaoData.ChaoDataInfo[] Info
		{
			get
			{
				return this.m_info;
			}
			set
			{
				this.m_info = value;
			}
		}

		public ChaoData()
		{
			this.m_info = new ChaoData.ChaoDataInfo[this.CHAO_MAX_NUM];
			for (int i = 0; i < this.CHAO_MAX_NUM; i++)
			{
				this.m_info[i].chao_id = -1;
				this.m_info[i].level = -1;
			}
		}

		public ChaoData(List<ServerChaoState> chaoStates)
		{
			int count = chaoStates.Count;
			if (count > 0)
			{
				this.m_info = new ChaoData.ChaoDataInfo[count];
				for (int i = 0; i < count; i++)
				{
					ServerChaoState serverChaoState = chaoStates[i];
					if (serverChaoState.Status > ServerChaoState.ChaoStatus.NotOwned)
					{
						ServerItem serverItem = new ServerItem((ServerItem.Id)serverChaoState.Id);
						this.m_info[i].chao_id = serverItem.chaoId;
						this.m_info[i].level = serverChaoState.Level;
					}
					else
					{
						this.m_info[i].chao_id = -1;
						this.m_info[i].level = -1;
					}
				}
				this.CHAO_MAX_NUM = count;
			}
		}

		public uint GetChaoCount()
		{
			uint num = 0u;
			ChaoData.ChaoDataInfo[] info = this.Info;
			for (int i = 0; i < info.Length; i++)
			{
				ChaoData.ChaoDataInfo chaoDataInfo = info[i];
				if (chaoDataInfo.chao_id != -1)
				{
					num += 1u;
				}
			}
			return num;
		}
	}
}
