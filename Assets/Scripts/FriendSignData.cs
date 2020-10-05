using System;
using UnityEngine;

public class FriendSignData
{
	public int m_index;

	public float m_distance;

	public Texture2D m_texture;

	public bool m_appear;

	public FriendSignData(int index, float distance, Texture2D texture, bool appear)
	{
		this.m_index = index;
		this.m_distance = distance;
		this.m_texture = texture;
		this.m_appear = appear;
	}
}
