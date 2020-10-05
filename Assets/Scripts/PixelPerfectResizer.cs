using System;
using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectResizer : MonoBehaviour
{
	public Camera cam;

	private Transform _transform;

	private float _lastOrthographicSize;

	private float _lastPixelWidth;

	private float _lastPixelHeight;

	private void Awake()
	{
		this._transform = base.transform;
	}

	private void Update()
	{
		if (this.cam == null)
		{
			this.cam = Camera.main;
		}
		if (this.cam != null && (this.cam.orthographicSize != this._lastOrthographicSize || this.cam.pixelWidth != this._lastPixelWidth || this.cam.pixelHeight != this._lastPixelHeight))
		{
			this._transform.localScale = new Vector3((float)((int)(this.cam.orthographicSize * 2000f * this.cam.aspect / this.cam.pixelWidth)) / 1000f, (float)((int)(this.cam.orthographicSize * 2000f / this.cam.pixelHeight)) / 1000f, this._transform.localScale.z);
		}
	}
}
