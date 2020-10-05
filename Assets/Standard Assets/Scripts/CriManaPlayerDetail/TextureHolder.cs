using System;
using System.Collections;
using UnityEngine;

namespace CriManaPlayerDetail
{
	public abstract class TextureHolder
	{
		public readonly int width;

		public readonly int height;

		public readonly bool isAlphaMode;

		private float offsetTexels;

		protected readonly uint texNumber;

		protected uint texIndex;

		protected TextureHolder(int argWidth, int argHeight, uint argTexNumber, bool alphaMode, float argOffsetTexels)
		{
			this.width = TextureHolder.next_pot_size(TextureHolder.ceiling64(argWidth));
			this.height = TextureHolder.next_pot_size(TextureHolder.ceiling16(argHeight));
			this.texNumber = argTexNumber;
			this.isAlphaMode = alphaMode;
			this.offsetTexels = argOffsetTexels;
		}

		public bool IsAvailable(int argWidth, int argHeight, uint argTexNumber, bool alphaMode)
		{
			return this.width >= argWidth && this.height >= argHeight && this.texNumber == argTexNumber && this.isAlphaMode == alphaMode;
		}

		public void SetTextureConfig(Material material, int argWidth, int argHeight, bool flipTopBottom, bool flipLeftRight)
		{
			float num = this.offsetTexels / (float)this.width;
			float num2 = this.offsetTexels / (float)this.height;
			float num3 = (float)argWidth / (float)this.width;
			float num4 = (float)argHeight / (float)this.height;
			float x;
			float x2;
			if (flipLeftRight)
			{
				x = num3 - num;
				x2 = -num3 + num;
			}
			else
			{
				x = 0f;
				x2 = num3 - num;
			}
			float y;
			float y2;
			if (flipTopBottom)
			{
				y = 0f;
				y2 = num4 - num2;
			}
			else
			{
				y = num4 - num2;
				y2 = -num4 + num2;
			}
			material.mainTextureScale = new Vector2(x2, y2);
			material.mainTextureOffset = new Vector2(x, y);
		}

		public abstract IEnumerator CreateTexture();

		public abstract bool UpdateTexture(Material material, int playerId, out CriManaPlayer.FrameInfo frameInfo);

		public abstract void DestroyTexture();

		private static uint next_pot_size(uint x)
		{
			x -= 1u;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1u;
		}

		private static int next_pot_size(int x)
		{
			return (int)TextureHolder.next_pot_size((uint)x);
		}

		private static int ceiling8(int x)
		{
			return x + 7 & -8;
		}

		private static int ceiling16(int x)
		{
			return x + 15 & -16;
		}

		private static int ceiling64(int x)
		{
			return x + 63 & -64;
		}

		private static int ceiling256(int x)
		{
			return x + 255 & -256;
		}

		public static TextureHolder Create(int reservedWidth, int reservedHeight, uint texNumber, bool alphaMode)
		{
			if (alphaMode)
			{
				return new TextureHolderByTexPtrWithAlpha(reservedWidth, reservedHeight, texNumber, alphaMode);
			}
			return new TextureHolderByTexPtr(reservedWidth, reservedHeight, texNumber, alphaMode);
		}
	}
}
