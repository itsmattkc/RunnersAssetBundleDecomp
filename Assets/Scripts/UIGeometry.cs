using System;
using UnityEngine;

public class UIGeometry
{
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	public BetterList<Color32> cols = new BetterList<Color32>();

	private BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();

	private Vector3 mRtpNormal;

	private Vector4 mRtpTan;

	public bool hasVertices
	{
		get
		{
			return this.verts.size > 0;
		}
	}

	public bool hasTransformed
	{
		get
		{
			return this.mRtpVerts != null && this.mRtpVerts.size > 0 && this.mRtpVerts.size == this.verts.size;
		}
	}

	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	public void ApplyTransform(Matrix4x4 widgetToPanel)
	{
		if (this.verts.size > 0)
		{
			this.mRtpVerts.Clear();
			int i = 0;
			int size = this.verts.size;
			while (i < size)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[i]));
				i++;
			}
			this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
			Vector3 normalized = widgetToPanel.MultiplyVector(Vector3.right).normalized;
			this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
		}
		else
		{
			this.mRtpVerts.Clear();
		}
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
		{
			if (n == null)
			{
				for (int i = 0; i < this.mRtpVerts.size; i++)
				{
					v.Add(this.mRtpVerts.buffer[i]);
					u.Add(this.uvs.buffer[i]);
					c.Add(this.cols.buffer[i]);
				}
			}
			else
			{
				for (int j = 0; j < this.mRtpVerts.size; j++)
				{
					v.Add(this.mRtpVerts.buffer[j]);
					u.Add(this.uvs.buffer[j]);
					c.Add(this.cols.buffer[j]);
					n.Add(this.mRtpNormal);
					t.Add(this.mRtpTan);
				}
			}
		}
	}

	public int WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t, int point)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
		{
			if (n == null)
			{
				for (int i = 0; i < this.mRtpVerts.size; i++)
				{
					if (v.size > point)
					{
						v[point] = this.mRtpVerts.buffer[i];
						u[point] = this.uvs.buffer[i];
						c[point] = this.cols.buffer[i];
					}
					else
					{
						v.Add(this.mRtpVerts.buffer[i]);
						u.Add(this.uvs.buffer[i]);
						c.Add(this.cols.buffer[i]);
					}
					point++;
				}
			}
			else
			{
				for (int j = 0; j < this.mRtpVerts.size; j++)
				{
					if (v.size > point)
					{
						v[point] = this.mRtpVerts.buffer[j];
						u[point] = this.uvs.buffer[j];
						c[point] = this.cols.buffer[j];
						n[point] = this.mRtpNormal;
						t[point] = this.mRtpTan;
					}
					else
					{
						v.Add(this.mRtpVerts.buffer[j]);
						u.Add(this.uvs.buffer[j]);
						c.Add(this.cols.buffer[j]);
						n.Add(this.mRtpNormal);
						t.Add(this.mRtpTan);
					}
					point++;
				}
			}
		}
		return point;
	}
}
