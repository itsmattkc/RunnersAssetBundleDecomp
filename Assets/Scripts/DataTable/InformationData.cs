using System;
using System.Runtime.CompilerServices;

namespace DataTable
{
	public class InformationData
	{
		private string _tag_k__BackingField;

		private string _url_k__BackingField;

		private string _sfx_k__BackingField;

		public string tag
		{
			get;
			set;
		}

		public string url
		{
			get;
			set;
		}

		public string sfx
		{
			get;
			set;
		}

		public InformationData()
		{
		}

		public InformationData(string tag, string url, string sfx)
		{
			this.tag = tag;
			this.url = url;
			this.sfx = sfx;
		}
	}
}
