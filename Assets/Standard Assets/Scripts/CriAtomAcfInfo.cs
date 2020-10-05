using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CriAtomAcfInfo
{
	public class InfoBase
	{
		public string name = "dummyCueSheet";

		public int id;

		public string comment = string.Empty;
	}

	public class AcfInfo : CriAtomAcfInfo.InfoBase
	{
		public string acfPath = "dummyCueSheet.acf";

		public List<CriAtomAcfInfo.AcbInfo> acbInfoList = new List<CriAtomAcfInfo.AcbInfo>();

		public string atomGuid = string.Empty;

		public string dspBusSetting = "DspBusSetting_0";

		public List<string> aisacControlNameList = new List<string>();

		public AcfInfo(string n, int inId, string com, string inAcfPath, string _guid, string _dspBusSetting)
		{
			this.name = n;
			this.id = inId;
			this.comment = com;
			this.acfPath = inAcfPath;
			this.atomGuid = _guid;
			this.dspBusSetting = _dspBusSetting;
		}
	}

	public class AcbInfo : CriAtomAcfInfo.InfoBase
	{
		public string acbPath = "dummyCueSheet.acb";

		public string awbPath = "dummyCueSheet_streamfiles.awb";

		public string atomGuid = string.Empty;

		public Dictionary<int, CriAtomAcfInfo.CueInfo> cueInfoList = new Dictionary<int, CriAtomAcfInfo.CueInfo>();

		public AcbInfo(string n, int inId, string com, string inAcbPath, string inAwbPath, string _guid)
		{
			this.name = n;
			this.id = inId;
			this.comment = com;
			this.acbPath = inAcbPath;
			this.awbPath = inAwbPath;
			this.atomGuid = _guid;
		}
	}

	public class CueInfo : CriAtomAcfInfo.InfoBase
	{
		public CueInfo(string n, int inId, string com)
		{
			this.name = n;
			this.id = inId;
			this.comment = com;
		}
	}

	public static CriAtomAcfInfo.AcfInfo acfInfo;

	public static bool GetCueInfo(bool forceReload)
	{
		if (CriAtomAcfInfo.acfInfo == null || forceReload)
		{
		}
		if (CriAtomAcfInfo.acfInfo == null)
		{
			string[] files = Directory.GetFiles(Application.streamingAssetsPath);
			int num = 0;
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (Path.GetExtension(text.Replace("\\", "/")) == ".acf")
				{
					CriAtomAcfInfo.acfInfo = new CriAtomAcfInfo.AcfInfo(Path.GetFileNameWithoutExtension(text), 0, string.Empty, Path.GetFileName(text), string.Empty, string.Empty);
				}
			}
			if (CriAtomAcfInfo.acfInfo != null)
			{
				string[] array2 = files;
				for (int j = 0; j < array2.Length; j++)
				{
					string text2 = array2[j];
					if (Path.GetExtension(text2.Replace("\\", "/")) == ".acb")
					{
						CriAtomAcfInfo.AcbInfo acbInfo = new CriAtomAcfInfo.AcbInfo(Path.GetFileNameWithoutExtension(text2), num, string.Empty, Path.GetFileName(text2), string.Empty, string.Empty);
						CriAtomAcfInfo.CueInfo value = new CriAtomAcfInfo.CueInfo("DummyCue", 0, string.Empty);
						acbInfo.cueInfoList.Add(0, value);
						CriAtomAcfInfo.acfInfo.acbInfoList.Add(acbInfo);
						num++;
					}
				}
			}
		}
		return CriAtomAcfInfo.acfInfo != null;
	}
}
