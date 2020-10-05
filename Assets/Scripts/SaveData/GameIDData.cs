using System;
using System.Runtime.CompilerServices;

namespace SaveData
{
	public class GameIDData
	{
		public const string NoUserID = "0";

		public const string KeyID = "aa7329ab4330306fbdd6dbe9b85c96be";

		public const string KeyPass = "48521cd1266052bfc25718720e91fa83";

		private string _id_k__BackingField;

		private string _password_k__BackingField;

		private string _device_k__BackingField;

		private string _takeoverId_k__BackingField;

		private string _takeoverPassword_k__BackingField;

		public string id
		{
			get;
			set;
		}

		public string password
		{
			get;
			set;
		}

		public string device
		{
			get;
			set;
		}

		public string takeoverId
		{
			get;
			set;
		}

		public string takeoverPassword
		{
			get;
			set;
		}

		public GameIDData()
		{
			this.Init();
		}

		public void Init()
		{
			this.id = "0";
			this.password = string.Empty;
			this.device = string.Empty;
			this.takeoverId = string.Empty;
			this.takeoverPassword = string.Empty;
		}
	}
}
