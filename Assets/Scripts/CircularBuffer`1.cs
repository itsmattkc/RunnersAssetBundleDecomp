using System;
using UnityEngine;

internal class CircularBuffer<T>
{
	private T[] m_Buffer;

	private int m_Cursor;

	private int m_Head;

	private int m_Tail;

	private int m_Size;

	public int Capacity
	{
		get
		{
			return this.m_Buffer.Length;
		}
	}

	public int Size
	{
		get
		{
			return this.m_Size;
		}
	}

	public int Head
	{
		get
		{
			return this.m_Head;
		}
	}

	public int Tail
	{
		get
		{
			return this.m_Tail;
		}
	}

	public CircularBuffer(int capacity)
	{
		this.m_Buffer = new T[capacity];
	}

	public T GetAt(int index)
	{
		return this.m_Buffer[index];
	}

	public void Add(T item)
	{
		this.m_Tail = this.m_Cursor;
		this.m_Buffer[this.m_Cursor] = item;
		this.m_Cursor = (this.m_Cursor + 1) % this.Capacity;
		this.m_Head = ((this.m_Size >= this.Capacity) ? this.m_Cursor : 0);
		this.m_Size = Mathf.Min(this.m_Size + 1, this.Capacity);
	}
}
