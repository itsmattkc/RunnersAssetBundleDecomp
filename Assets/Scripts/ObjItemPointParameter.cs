using System;

[Serializable]
public class ObjItemPointParameter : SpawnableParameter
{
	public int m_tblID;

	public ObjItemPointParameter() : base("ObjItemPoint")
	{
		this.m_tblID = 0;
	}
}
