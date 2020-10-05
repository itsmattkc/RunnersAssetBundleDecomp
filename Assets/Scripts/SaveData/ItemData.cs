using System;
using System.Collections.Generic;

namespace SaveData
{
	public class ItemData
	{
		private uint m_ring_count;

		private uint m_red_ring_count;

		private int m_ring_count_offset;

		private int m_red_ring_count_offset;

		private uint[] m_item_count = new uint[8];

		private Dictionary<ServerItem.Id, long> m_etc_item_count;

		private Dictionary<ServerItem.Id, long> m_etc_item_count_offset;

		public uint RingCount
		{
			get
			{
				return this.m_ring_count;
			}
			set
			{
				this.m_ring_count = value;
			}
		}

		public int RingCountOffset
		{
			get
			{
				return this.m_ring_count_offset;
			}
			set
			{
				this.m_ring_count_offset = value;
			}
		}

		public int DisplayRingCount
		{
			get
			{
				return (int)(this.RingCount + (uint)this.RingCountOffset);
			}
		}

		public uint RedRingCount
		{
			get
			{
				return this.m_red_ring_count;
			}
			set
			{
				this.m_red_ring_count = value;
			}
		}

		public int RedRingCountOffset
		{
			get
			{
				return this.m_red_ring_count_offset;
			}
			set
			{
				this.m_red_ring_count_offset = value;
			}
		}

		public int DisplayRedRingCount
		{
			get
			{
				return (int)(this.RedRingCount + (uint)this.RedRingCountOffset);
			}
		}

		public uint[] ItemCount
		{
			get
			{
				return this.m_item_count;
			}
			set
			{
				this.m_item_count = value;
			}
		}

		public ItemData()
		{
			this.m_ring_count = 0u;
			this.m_red_ring_count = 0u;
			for (uint num = 0u; num < 8u; num += 1u)
			{
				this.m_item_count[(int)((UIntPtr)num)] = 0u;
			}
		}

		public uint GetItemCount(ItemType type)
		{
			if (this.IsValidType(type))
			{
				return this.m_item_count[(int)type];
			}
			return 0u;
		}

		public void SetItemCount(ItemType type, uint count)
		{
			if (this.IsValidType(type))
			{
				this.m_item_count[(int)type] = count;
			}
		}

		public uint GetAllItemCount()
		{
			uint num = 0u;
			for (ItemType itemType = ItemType.INVINCIBLE; itemType < ItemType.NUM; itemType++)
			{
				num += this.GetItemCount(itemType);
			}
			return num;
		}

		private bool IsValidType(ItemType type)
		{
			return ItemType.INVINCIBLE <= type && type < ItemType.NUM;
		}

		public void SetEtcItemCount(ServerItem.Id itemId, long count)
		{
			if (this.m_etc_item_count == null && this.m_etc_item_count_offset == null)
			{
				this.m_etc_item_count = new Dictionary<ServerItem.Id, long>();
				this.m_etc_item_count_offset = new Dictionary<ServerItem.Id, long>();
			}
			if (this.m_etc_item_count.ContainsKey(itemId))
			{
				this.m_etc_item_count[itemId] = count;
				this.m_etc_item_count_offset[itemId] = 0L;
			}
			else
			{
				this.m_etc_item_count.Add(itemId, count);
				this.m_etc_item_count_offset.Add(itemId, 0L);
			}
		}

		public void AddEtcItemCount(ServerItem.Id itemId, long count)
		{
			if (this.m_etc_item_count == null && this.m_etc_item_count_offset == null)
			{
				this.m_etc_item_count = new Dictionary<ServerItem.Id, long>();
				this.m_etc_item_count_offset = new Dictionary<ServerItem.Id, long>();
			}
			if (this.m_etc_item_count.ContainsKey(itemId))
			{
				Dictionary<ServerItem.Id, long> etc_item_count;
				Dictionary<ServerItem.Id, long> expr_43 = etc_item_count = this.m_etc_item_count;
				long num = etc_item_count[itemId];
				expr_43[itemId] = num + count;
				this.m_etc_item_count_offset[itemId] = 0L;
			}
			else
			{
				this.m_etc_item_count.Add(itemId, count);
				this.m_etc_item_count_offset.Add(itemId, 0L);
			}
		}

		public bool SetEtcItemCountOffset(ServerItem.Id itemId, long offset)
		{
			bool result = false;
			if (this.m_etc_item_count != null && this.m_etc_item_count_offset != null && this.m_etc_item_count_offset.ContainsKey(itemId))
			{
				this.m_etc_item_count_offset[itemId] = offset;
				result = true;
			}
			return result;
		}

		public long GetEtcItemCount(ServerItem.Id itemId)
		{
			long result = 0L;
			if (this.m_etc_item_count != null && this.m_etc_item_count.ContainsKey(itemId) && this.m_etc_item_count_offset.ContainsKey(itemId))
			{
				result = this.m_etc_item_count[itemId] + this.m_etc_item_count_offset[itemId];
			}
			return result;
		}

		public long GetEtcItemCountOffset(ServerItem.Id itemId)
		{
			long result = 0L;
			if (this.m_etc_item_count != null && this.m_etc_item_count.ContainsKey(itemId) && this.m_etc_item_count_offset.ContainsKey(itemId))
			{
				result = this.m_etc_item_count_offset[itemId];
			}
			return result;
		}

		public bool IsEtcItemCount(ServerItem.Id itemId)
		{
			bool result = false;
			if (this.m_etc_item_count != null && this.m_etc_item_count.ContainsKey(itemId) && this.m_etc_item_count_offset.ContainsKey(itemId))
			{
				result = true;
			}
			return result;
		}
	}
}
