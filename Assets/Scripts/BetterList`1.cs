using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BetterList<T>
{
	private sealed class _GetEnumerator_c__Iterator6 : IDisposable, IEnumerator, IEnumerator<T>
	{
		internal int _i___0;

		internal int _PC;

		internal T _current;

		internal BetterList<T> __f__this;

		T IEnumerator<T>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				if (this.__f__this.buffer == null)
				{
					goto IL_89;
				}
				this._i___0 = 0;
				break;
			case 1u:
				this._i___0++;
				break;
			default:
				return false;
			}
			if (this._i___0 < this.__f__this.size)
			{
				this._current = this.__f__this.buffer[this._i___0];
				this._PC = 1;
				return true;
			}
			IL_89:
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public T[] buffer;

	public int size;

	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		BetterList<T>._GetEnumerator_c__Iterator6 _GetEnumerator_c__Iterator = new BetterList<T>._GetEnumerator_c__Iterator6();
		_GetEnumerator_c__Iterator.__f__this = this;
		return _GetEnumerator_c__Iterator;
	}

	private void AllocateMore()
	{
		T[] array = (this.buffer == null) ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)];
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new T[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
			}
		}
		else
		{
			this.buffer = null;
		}
	}

	public void Clear()
	{
		this.size = 0;
	}

	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		this.buffer[this.size++] = item;
	}

	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index < this.size)
		{
			for (int i = this.size; i > index; i--)
			{
				this.buffer[i] = this.buffer[i - 1];
			}
			this.buffer[index] = item;
			this.size++;
		}
		else
		{
			this.Add(item);
		}
	}

	public bool Contains(T item)
	{
		if (this.buffer == null)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		if (this.buffer != null && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
		}
	}

	public T Pop()
	{
		if (this.buffer != null && this.size != 0)
		{
			T result = this.buffer[--this.size];
			this.buffer[this.size] = default(T);
			return result;
		}
		return default(T);
	}

	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	public void Sort(Comparison<T> comparer)
	{
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = 1; i < this.size; i++)
			{
				if (comparer(this.buffer[i - 1], this.buffer[i]) > 0)
				{
					T t = this.buffer[i];
					this.buffer[i] = this.buffer[i - 1];
					this.buffer[i - 1] = t;
					flag = true;
				}
			}
		}
	}
}
