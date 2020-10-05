using System;

namespace App.Utility
{
	public struct Bitset32
	{
		private uint m_Value;

		public Bitset32(Bitset32 rhs)
		{
			this.m_Value = rhs.m_Value;
		}

		public Bitset32(uint x)
		{
			this.m_Value = x;
		}

		public override bool Equals(object o)
		{
			return true;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public bool Test(int pos)
		{
			return ((ulong)this.m_Value & (ulong)(1L << (pos & 31))) != 0uL;
		}

		public bool Any()
		{
			return this.m_Value != 0u;
		}

		public bool None()
		{
			return !this.Any();
		}

		public int Count()
		{
			uint num = 1u;
			int num2 = 0;
			for (int i = 32; i > 0; i--)
			{
				if ((this.m_Value & num) != 0u)
				{
					num2++;
				}
				num <<= 1;
			}
			return num2;
		}

		public Bitset32 Set(int pos)
		{
			this.m_Value |= 1u << pos;
			return this;
		}

		public Bitset32 Set(int pos, bool flag)
		{
			if (flag)
			{
				this.m_Value |= 1u << pos;
			}
			else
			{
				this.m_Value &= ~(1u << pos);
			}
			return this;
		}

		public Bitset32 Set()
		{
			this.m_Value = 4294967295u;
			return this;
		}

		public Bitset32 Reset(int pos)
		{
			this.m_Value ^= 1u << pos;
			return this;
		}

		public Bitset32 Reset()
		{
			this.m_Value = 0u;
			return this;
		}

		public Bitset32 Flip(int pos)
		{
			this.m_Value ^= 1u << pos;
			return this;
		}

		public Bitset32 Flip()
		{
			this.m_Value = ~this.m_Value;
			return this;
		}

		public uint to_ulong()
		{
			return this.m_Value;
		}

		public static bool operator ==(Bitset32 lhs, Bitset32 rhs)
		{
			return lhs.m_Value == rhs.m_Value;
		}

		public static bool operator !=(Bitset32 lhs, Bitset32 rhs)
		{
			return lhs.m_Value != rhs.m_Value;
		}

		public static Bitset32 operator &(Bitset32 lhs, Bitset32 rhs)
		{
			return new Bitset32(lhs.m_Value & rhs.m_Value);
		}

		public static Bitset32 operator |(Bitset32 lhs, Bitset32 rhs)
		{
			return new Bitset32(lhs.m_Value | rhs.m_Value);
		}
	}
}
