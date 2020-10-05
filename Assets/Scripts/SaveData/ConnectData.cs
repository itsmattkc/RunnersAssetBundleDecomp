using System;

namespace SaveData
{
	public class ConnectData
	{
		private bool m_replaceMessageBox;

		public bool ReplaceMessageBox
		{
			get
			{
				return this.m_replaceMessageBox;
			}
			set
			{
				this.m_replaceMessageBox = value;
			}
		}
	}
}
