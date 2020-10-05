using System;
using System.Runtime.CompilerServices;

namespace DataTable
{
	public class NGWordData
	{
		private string _word_k__BackingField;

		private int _param_k__BackingField;

		public string word
		{
			get;
			set;
		}

		public int param
		{
			get;
			set;
		}

		public NGWordData()
		{
		}

		public NGWordData(string word, int param)
		{
			this.word = word;
			this.param = param;
		}
	}
}
