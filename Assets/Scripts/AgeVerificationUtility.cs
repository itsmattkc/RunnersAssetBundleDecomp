using System;
using UnityEngine;

public class AgeVerificationUtility : MonoBehaviour
{
	public static bool IsValidDate(int year, int month, int day)
	{
		DateTime minValue;
		try
		{
			minValue = new DateTime(1900, 1, 1);
		}
		catch (ArgumentOutOfRangeException ex)
		{
			global::Debug.Log(ex.ToString());
			minValue = DateTime.MinValue;
		}
		DateTime now = DateTime.Now;
		if (year < minValue.Year)
		{
			return false;
		}
		if (year > now.Year)
		{
			return false;
		}
		int num = DateTime.DaysInMonth(year, month);
		if (day > num)
		{
			return false;
		}
		DateTime t = new DateTime(year, month, day);
		int num2 = DateTime.Compare(t, now);
		return num2 <= 0;
	}

	public static DateTime CalcDateTime(int year, int month, int day, int deltaYear, int deltaMonth, int deltaDay, bool dummy)
	{
		DateTime dateTime = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
		DateTime dateTime2 = dateTime;
		DateTime dateTime3 = dateTime;
		DateTime dateTime4 = dateTime;
		try
		{
			dateTime3 = new DateTime(1900, 1, 1);
			dateTime4 = DateTime.Now;
		}
		catch (ArgumentOutOfRangeException ex)
		{
			global::Debug.Log("AgeVerificationUtility" + ex.ToString());
			dateTime3 = DateTime.MinValue;
			dateTime4 = DateTime.MaxValue;
		}
		try
		{
			dateTime2 = dateTime.AddYears(deltaYear);
			dateTime2 = dateTime2.AddMonths(deltaMonth);
			dateTime2 = dateTime2.AddDays((double)deltaDay);
			if (dateTime2 > dateTime4)
			{
				dateTime2 = dateTime4;
			}
			else if (dateTime2 < dateTime3)
			{
				dateTime2 = dateTime3;
			}
		}
		catch (ArgumentOutOfRangeException ex2)
		{
			global::Debug.Log("AgeVerificationUtility" + ex2.ToString());
			dateTime2 = dateTime;
		}
		return dateTime2;
	}
}
