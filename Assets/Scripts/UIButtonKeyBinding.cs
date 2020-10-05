using System;
using UnityEngine;

[AddComponentMenu("Game/UI/Button Key Binding")]
public class UIButtonKeyBinding : MonoBehaviour
{
	public KeyCode keyCode;

	private void Update()
	{
		if (!UICamera.inputHasFocus)
		{
			if (this.keyCode == KeyCode.None)
			{
				return;
			}
			if (UnityEngine.Input.GetKeyDown(this.keyCode))
			{
				base.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
			}
			if (UnityEngine.Input.GetKeyUp(this.keyCode))
			{
				base.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
				base.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
