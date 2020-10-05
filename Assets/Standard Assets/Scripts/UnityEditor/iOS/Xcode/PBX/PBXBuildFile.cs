using System;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class PBXBuildFile : PBXObject
	{
		public string fileRef;

		public string compileFlags;

		public bool weak;

		private static PropertyCommentChecker checkerData = new PropertyCommentChecker(new string[]
		{
			"fileRef/*"
		});

		internal override PropertyCommentChecker checker
		{
			get
			{
				return PBXBuildFile.checkerData;
			}
		}

		internal override bool shouldCompact
		{
			get
			{
				return true;
			}
		}

		public static PBXBuildFile CreateFromFile(string fileRefGUID, bool weak, string compileFlags)
		{
			PBXBuildFile pBXBuildFile = new PBXBuildFile();
			pBXBuildFile.guid = PBXGUID.Generate();
			pBXBuildFile.SetPropertyString("isa", "PBXBuildFile");
			pBXBuildFile.fileRef = fileRefGUID;
			pBXBuildFile.compileFlags = compileFlags;
			pBXBuildFile.weak = weak;
			return pBXBuildFile;
		}

		private PBXElementDict GetSettingsDict()
		{
			if (this.m_Properties.Contains("settings"))
			{
				return this.m_Properties["settings"].AsDict();
			}
			return this.m_Properties.CreateDict("settings");
		}

		public override void UpdateProps()
		{
			base.SetPropertyString("fileRef", this.fileRef);
			if (this.compileFlags != null && this.compileFlags != string.Empty)
			{
				this.GetSettingsDict().SetString("COMPILER_FLAGS", this.compileFlags);
			}
			if (this.weak)
			{
				PBXElementDict settingsDict = this.GetSettingsDict();
				PBXElementArray pBXElementArray = null;
				if (settingsDict.Contains("ATTRIBUTES"))
				{
					pBXElementArray = settingsDict["ATTRIBUTES"].AsArray();
				}
				else
				{
					pBXElementArray = settingsDict.CreateArray("ATTRIBUTES");
				}
				bool flag = false;
				foreach (PBXElement current in pBXElementArray.values)
				{
					if (current is PBXElementString && current.AsString() == "Weak")
					{
						flag = true;
					}
				}
				if (!flag)
				{
					pBXElementArray.AddString("Weak");
				}
			}
		}

		public override void UpdateVars()
		{
			this.fileRef = base.GetPropertyString("fileRef");
			this.compileFlags = null;
			this.weak = false;
			if (this.m_Properties.Contains("settings"))
			{
				PBXElementDict pBXElementDict = this.m_Properties["settings"].AsDict();
				if (pBXElementDict.Contains("COMPILER_FLAGS"))
				{
					this.compileFlags = pBXElementDict["COMPILER_FLAGS"].AsString();
				}
				if (pBXElementDict.Contains("ATTRIBUTES"))
				{
					PBXElementArray pBXElementArray = pBXElementDict["ATTRIBUTES"].AsArray();
					foreach (PBXElement current in pBXElementArray.values)
					{
						if (current is PBXElementString && current.AsString() == "Weak")
						{
							this.weak = true;
						}
					}
				}
			}
		}
	}
}
