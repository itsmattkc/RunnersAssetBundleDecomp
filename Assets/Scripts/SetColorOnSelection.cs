using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Set Color on Selection"), ExecuteInEditMode, RequireComponent(typeof(UIWidget))]
public class SetColorOnSelection : MonoBehaviour
{
	private UIWidget mWidget;

	private static Dictionary<string, int> __f__switch_map0;

	private void OnSelectionChange(string val)
	{
		if (this.mWidget == null)
		{
			this.mWidget = base.GetComponent<UIWidget>();
		}
		switch (val)
		{
		case "White":
			this.mWidget.color = Color.white;
			break;
		case "Red":
			this.mWidget.color = Color.red;
			break;
		case "Green":
			this.mWidget.color = Color.green;
			break;
		case "Blue":
			this.mWidget.color = Color.blue;
			break;
		case "Yellow":
			this.mWidget.color = Color.yellow;
			break;
		case "Cyan":
			this.mWidget.color = Color.cyan;
			break;
		case "Magenta":
			this.mWidget.color = Color.magenta;
			break;
		}
	}
}
