using System;
using System.Collections.Generic;

namespace App
{
	public class Random
	{
		public static void Shuffle<T>(List<T> list)
		{
			System.Random random = new System.Random();
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = random.Next(i + 1);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		public static void ShuffleInt(int[] array)
		{
			System.Random random = new System.Random();
			int i = array.Length;
			while (i > 1)
			{
				i--;
				int num = random.Next(i + 1);
				int num2 = array[num];
				array[num] = array[i];
				array[i] = num2;
			}
		}
	}
}
