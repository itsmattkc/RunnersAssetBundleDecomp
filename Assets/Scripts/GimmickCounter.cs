using System;
using System.Collections.Generic;
using UnityEngine;

public class GimmickCounter : MonoBehaviour
{
	private int m_waitTimer;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_waitTimer == 5)
		{
			this.InitCoroutine();
		}
		this.m_waitTimer++;
	}

	private void InitCoroutine()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		GameObject gameObject = base.gameObject;
		int childCount = gameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = gameObject.transform.GetChild(i);
			if (!(child == null))
			{
				GameObject gameObject2 = child.gameObject;
				if (!(gameObject2 == null))
				{
					string name = gameObject2.name;
					int num3;
					if (name == "MultiSetLine" || name == "MultiSetSector" || name == "MultiSetCircle" || name == "MultiSetParaloopCircle")
					{
						int childCount2 = child.childCount;
						for (int j = 0; j < childCount2; j++)
						{
							Transform child2 = child.GetChild(j);
							if (!(child2 == null))
							{
								GameObject gameObject3 = child2.gameObject;
								if (!(gameObject3 == null))
								{
									string name2 = gameObject3.name;
									int num;
									if (!dictionary.TryGetValue(name2, out num))
									{
										dictionary.Add(name2, 1);
									}
									else
									{
										Dictionary<string, int> dictionary2;
										Dictionary<string, int> expr_118 = dictionary2 = dictionary;
										string text;
										string expr_11D = text = name2;
										int num2 = dictionary2[text];
										expr_118[expr_11D] = num2 + 1;
									}
								}
							}
						}
					}
					else if (!dictionary.TryGetValue(name, out num3))
					{
						dictionary.Add(name, 1);
					}
					else
					{
						Dictionary<string, int> dictionary3;
						Dictionary<string, int> expr_166 = dictionary3 = dictionary;
						string text;
						string expr_16B = text = name;
						int num2 = dictionary3[text];
						expr_166[expr_16B] = num2 + 1;
					}
				}
			}
		}
		global::Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
		string text2 = "BlockID = " + gameObject.name + "\n";
		foreach (KeyValuePair<string, int> current in dictionary)
		{
			string text = text2;
			text2 = string.Concat(new string[]
			{
				text,
				current.Key,
				":",
				current.Value.ToString(),
				"\n"
			});
		}
		global::Debug.Log(text2);
		global::Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
	}
}
