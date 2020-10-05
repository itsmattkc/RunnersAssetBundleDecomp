using System;
using UnityEngine;

public class FriendSignCreateData
{
	public bool m_create;

	public Texture2D m_texture;

	public FriendSignCreateData(Texture2D texture)
	{
		this.m_create = false;
		this.m_texture = texture;
	}
}
