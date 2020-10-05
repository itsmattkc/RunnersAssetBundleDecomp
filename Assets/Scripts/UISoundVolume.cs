using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Sound Volume"), RequireComponent(typeof(UISlider))]
public class UISoundVolume : MonoBehaviour
{
	private UISlider mSlider;

	private void Awake()
	{
		this.mSlider = base.GetComponent<UISlider>();
		this.mSlider.value = NGUITools.soundVolume;
		EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.OnChange));
	}

	private void OnChange()
	{
		NGUITools.soundVolume = UISlider.current.value;
	}
}
