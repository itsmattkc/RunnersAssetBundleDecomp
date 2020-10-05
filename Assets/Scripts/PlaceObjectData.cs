using System;
using UnityEngine;

public class PlaceObjectData
{
	public int x;

	public int y;

	public int count;

	public Bounds bound;

	public PlaceObjectData(int _x, int _y, int _count, Bounds _b)
	{
		this.x = _x;
		this.y = _y;
		this.count = _count;
		this.bound = _b;
	}
}
