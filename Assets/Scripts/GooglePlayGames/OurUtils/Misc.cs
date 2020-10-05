using System;
using System.Collections.Generic;

namespace GooglePlayGames.OurUtils
{
	public class Misc
	{
		public static bool BuffersAreIdentical(byte[] a, byte[] b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		public static void CopyList<T>(List<T> destList, List<T> sourceList)
		{
			destList.Clear();
			foreach (T current in sourceList)
			{
				destList.Add(current);
			}
		}

		public static byte[] GetSubsetBytes(byte[] array, int offset, int length)
		{
			if (offset == 0 && length == array.Length)
			{
				return array;
			}
			byte[] array2 = new byte[length];
			Array.Copy(array, offset, array2, 0, length);
			return array2;
		}
	}
}
