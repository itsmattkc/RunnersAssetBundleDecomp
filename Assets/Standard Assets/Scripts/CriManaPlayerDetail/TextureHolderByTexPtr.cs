using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CriManaPlayerDetail
{
	internal class TextureHolderByTexPtr : TextureHolder
	{
		private sealed class _CreateTexture_c__Iterator2 : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal int _i___0;

			internal int _PC;

			internal object _current;

			internal TextureHolderByTexPtr __f__this;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					this.__f__this.m_Texture_y = new Texture2D[this.__f__this.texNumber];
					this.__f__this.m_Texture_u = new Texture2D[this.__f__this.texNumber];
					this.__f__this.m_Texture_v = new Texture2D[this.__f__this.texNumber];
					this._i___0 = 0;
					goto IL_1BA;
				case 1u:
					this.__f__this.m_Texture_y[this._i___0] = new Texture2D(this.__f__this.width, this.__f__this.height, TextureFormat.Alpha8, false);
					this.__f__this.m_Texture_y[this._i___0].wrapMode = TextureWrapMode.Clamp;
					if ((long)(this._i___0 + 1) != (long)((ulong)this.__f__this.texNumber))
					{
						this._current = false;
						this._PC = 2;
						return true;
					}
					break;
				case 2u:
					break;
				default:
					return false;
				}
				this._i___0++;
				IL_1BA:
				if ((long)this._i___0 < (long)((ulong)this.__f__this.texNumber))
				{
					this.__f__this.m_Texture_u[this._i___0] = new Texture2D(this.__f__this.width / 2, this.__f__this.height / 2, TextureFormat.Alpha8, false);
					this.__f__this.m_Texture_u[this._i___0].wrapMode = TextureWrapMode.Clamp;
					this.__f__this.m_Texture_v[this._i___0] = new Texture2D(this.__f__this.width / 2, this.__f__this.height / 2, TextureFormat.Alpha8, false);
					this.__f__this.m_Texture_v[this._i___0].wrapMode = TextureWrapMode.Clamp;
					this._current = false;
					this._PC = 1;
					return true;
				}
				this._PC = -1;
				return false;
			}

			public void Dispose()
			{
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private const float constOffsetTexels = 1f;

		private Texture2D[] m_Texture_y;

		private Texture2D[] m_Texture_u;

		private Texture2D[] m_Texture_v;

		public TextureHolderByTexPtr(int argWidth, int argHeight, uint argTexNumber, bool alphaMode) : base(argWidth, argHeight, argTexNumber, false, 1f)
		{
		}

		public override IEnumerator CreateTexture()
		{
			TextureHolderByTexPtr._CreateTexture_c__Iterator2 _CreateTexture_c__Iterator = new TextureHolderByTexPtr._CreateTexture_c__Iterator2();
			_CreateTexture_c__Iterator.__f__this = this;
			return _CreateTexture_c__Iterator;
		}

		public override bool UpdateTexture(Material material, int playerId, out CriManaPlayer.FrameInfo frameInfo)
		{
			bool flag = CriManaPlugin.criManaUnityPlayer_UpdateTextureYuvByPtr(playerId, this.m_Texture_y[(int)((UIntPtr)this.texIndex)].GetNativeTexturePtr(), this.m_Texture_u[(int)((UIntPtr)this.texIndex)].GetNativeTexturePtr(), this.m_Texture_v[(int)((UIntPtr)this.texIndex)].GetNativeTexturePtr(), out frameInfo);
			if (flag)
			{
				material.SetTexture("Texture_y", this.m_Texture_y[(int)((UIntPtr)this.texIndex)]);
				material.SetTexture("Texture_u", this.m_Texture_u[(int)((UIntPtr)this.texIndex)]);
				material.SetTexture("Texture_v", this.m_Texture_v[(int)((UIntPtr)this.texIndex)]);
				this.texIndex = (this.texIndex + 1u) % this.texNumber;
			}
			return flag;
		}

		public override void DestroyTexture()
		{
			int num = 0;
			while ((long)num < (long)((ulong)this.texNumber))
			{
				UnityEngine.Object.Destroy(this.m_Texture_y[num]);
				UnityEngine.Object.Destroy(this.m_Texture_u[num]);
				UnityEngine.Object.Destroy(this.m_Texture_v[num]);
				num++;
			}
			this.m_Texture_y = null;
			this.m_Texture_u = null;
			this.m_Texture_v = null;
		}
	}
}
