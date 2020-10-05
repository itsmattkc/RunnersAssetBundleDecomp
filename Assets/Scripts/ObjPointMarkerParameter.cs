using System;

[Serializable]
public class ObjPointMarkerParameter : SpawnableParameter
{
	public int m_type;

	public ObjPointMarkerParameter() : base("ObjPointMarker")
	{
		this.m_type = 0;
	}
}
