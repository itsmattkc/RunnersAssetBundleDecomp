using System;

[Serializable]
public class UIInvStat
{
	public enum Identifier
	{
		Strength,
		Constitution,
		Agility,
		Intelligence,
		Damage,
		Crit,
		Armor,
		Health,
		Mana,
		Other
	}

	public enum Modifier
	{
		Added,
		Percent
	}

	public UIInvStat.Identifier id;

	public UIInvStat.Modifier modifier;

	public int amount;

	public static string GetName(UIInvStat.Identifier i)
	{
		return i.ToString();
	}

	public static string GetDescription(UIInvStat.Identifier i)
	{
		switch (i)
		{
		case UIInvStat.Identifier.Strength:
			return "Strength increases melee damage";
		case UIInvStat.Identifier.Constitution:
			return "Constitution increases health";
		case UIInvStat.Identifier.Agility:
			return "Agility increases armor";
		case UIInvStat.Identifier.Intelligence:
			return "Intelligence increases mana";
		case UIInvStat.Identifier.Damage:
			return "Damage adds to the amount of damage done in combat";
		case UIInvStat.Identifier.Crit:
			return "Crit increases the chance of landing a critical strike";
		case UIInvStat.Identifier.Armor:
			return "Armor protects from damage";
		case UIInvStat.Identifier.Health:
			return "Health prolongs life";
		case UIInvStat.Identifier.Mana:
			return "Mana increases the number of spells that can be cast";
		default:
			return null;
		}
	}

	public static int CompareArmor(UIInvStat a, UIInvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == UIInvStat.Identifier.Armor)
		{
			num -= 10000;
		}
		else if (a.id == UIInvStat.Identifier.Damage)
		{
			num -= 5000;
		}
		if (b.id == UIInvStat.Identifier.Armor)
		{
			num2 -= 10000;
		}
		else if (b.id == UIInvStat.Identifier.Damage)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == UIInvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == UIInvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	public static int CompareWeapon(UIInvStat a, UIInvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == UIInvStat.Identifier.Damage)
		{
			num -= 10000;
		}
		else if (a.id == UIInvStat.Identifier.Armor)
		{
			num -= 5000;
		}
		if (b.id == UIInvStat.Identifier.Damage)
		{
			num2 -= 10000;
		}
		else if (b.id == UIInvStat.Identifier.Armor)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == UIInvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == UIInvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}
}
