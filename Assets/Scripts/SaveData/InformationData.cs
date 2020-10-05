using System;
using System.Collections.Generic;

namespace SaveData
{
	public class InformationData
	{
		public enum DataType
		{
			ID,
			SHOWED_TIME,
			ADD_INFO,
			IMAGE_ID,
			NUM
		}

		public const string RESET_PARAME = "-1,-1,-1,-1";

		private List<string> m_textArray = new List<string>();

		public bool m_isDirty;

		public static string INVALID_PARAM = "-1";

		public List<string> TextArray
		{
			get
			{
				return this.m_textArray;
			}
			set
			{
				this.m_textArray = value;
			}
		}

		public InformationData()
		{
			this.Init();
		}

		private bool CheckData(InformationData.DataType dataType)
		{
			return InformationData.DataType.ID <= dataType && dataType < InformationData.DataType.NUM;
		}

		public string GetData(string id, InformationData.DataType dataType)
		{
			if (this.m_textArray != null && this.CheckData(dataType))
			{
				for (int i = 0; i < this.m_textArray.Count; i++)
				{
					string[] array = this.m_textArray[i].Split(new char[]
					{
						','
					});
					if ((InformationData.DataType)array.Length > dataType && array[0] == id)
					{
						return array[(int)dataType];
					}
				}
			}
			return InformationData.INVALID_PARAM;
		}

		public string GetData(int index, InformationData.DataType dataType)
		{
			if (this.m_textArray != null && this.CheckData(dataType) && index < this.m_textArray.Count)
			{
				string[] array = this.m_textArray[index].Split(new char[]
				{
					','
				});
				if ((InformationData.DataType)array.Length > dataType)
				{
					return array[(int)dataType];
				}
			}
			return InformationData.INVALID_PARAM;
		}

		public string GetEventRankingData(string id, string saveKey, InformationData.DataType dataType)
		{
			if (this.m_textArray != null && this.CheckData(dataType))
			{
				for (int i = 0; i < this.m_textArray.Count; i++)
				{
					string[] array = this.m_textArray[i].Split(new char[]
					{
						','
					});
					if ((InformationData.DataType)array.Length > dataType && array[0] == id && array[2] == saveKey)
					{
						return array[(int)dataType];
					}
				}
			}
			return InformationData.INVALID_PARAM;
		}

		public void UpdateShowedTime(string id, string showedTime, string addInfo, string imageId)
		{
			if (this.m_textArray != null)
			{
				bool flag = false;
				for (int i = 0; i < this.m_textArray.Count; i++)
				{
					string[] array = this.m_textArray[i].Split(new char[]
					{
						','
					});
					if (array.Length > 0 && array[0] == id)
					{
						this.m_textArray[i] = string.Concat(new string[]
						{
							id,
							",",
							showedTime,
							",",
							addInfo,
							",",
							imageId
						});
						flag = true;
						this.m_isDirty = true;
						break;
					}
				}
				if (!flag)
				{
					this.m_textArray.Add(string.Concat(new string[]
					{
						id,
						",",
						showedTime,
						",",
						addInfo,
						",",
						imageId
					}));
					this.m_isDirty = true;
				}
			}
		}

		public void UpdateEventRankingShowedTime(string id, string showedTime, string addInfo, string imageId)
		{
			if (this.m_textArray != null)
			{
				bool flag = false;
				for (int i = 0; i < this.m_textArray.Count; i++)
				{
					string[] array = this.m_textArray[i].Split(new char[]
					{
						','
					});
					if (array.Length > 2 && array[0] == id && array[2] == addInfo)
					{
						this.m_textArray[i] = string.Concat(new string[]
						{
							id,
							",",
							showedTime,
							",",
							addInfo,
							",",
							imageId
						});
						flag = true;
						this.m_isDirty = true;
						break;
					}
				}
				if (!flag)
				{
					this.m_textArray.Add(string.Concat(new string[]
					{
						id,
						",",
						showedTime,
						",",
						addInfo,
						",",
						imageId
					}));
					this.m_isDirty = true;
				}
			}
		}

		public int DataCount()
		{
			if (this.m_textArray != null)
			{
				return this.m_textArray.Count;
			}
			return 0;
		}

		public void Reset(int index)
		{
			if (this.m_textArray != null && index < this.m_textArray.Count)
			{
				this.m_textArray.RemoveAt(index);
				this.m_isDirty = true;
			}
		}

		public void Init()
		{
			if (this.m_textArray != null)
			{
				for (int i = 0; i < this.m_textArray.Count; i++)
				{
					this.m_textArray[i] = "-1,-1,-1,-1";
				}
			}
			this.m_isDirty = false;
		}

		public void CopyTo(InformationData dst)
		{
			dst.TextArray = this.m_textArray;
			dst.m_isDirty = this.m_isDirty;
		}
	}
}
