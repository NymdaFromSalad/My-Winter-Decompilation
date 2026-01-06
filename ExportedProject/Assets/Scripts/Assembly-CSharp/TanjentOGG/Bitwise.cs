namespace TanjentOGG
{
	public class Bitwise
	{
		private static long[] mask = new long[33]
		{
			0L, 1L, 3L, 7L, 15L, 31L, 63L, 127L, 255L, 511L,
			1023L, 2047L, 4095L, 8191L, 16383L, 32767L, 65535L, 131071L, 262143L, 524287L,
			1048575L, 2097151L, 4194303L, 8388607L, 16777215L, 33554431L, 67108863L, 134217727L, 268435455L, 536870911L,
			1073741823L, 2147483647L, 4294967295L
		};

		public static void oggpack_readinit(Ogg.oggpack_buffer b, CPtr.BytePtr buf, int bytes)
		{
			b.clear();
			b.buffer = (b.ptr = buf);
			b.storage = bytes;
		}

		public static long oggpack_look(Ogg.oggpack_buffer b, int bits)
		{
			if (bits < 0 || bits > 32)
			{
				return -1L;
			}
			long num = mask[bits];
			bits += b.endbit;
			if (b.endbyte >= b.storage - 4)
			{
				if (b.endbyte > b.storage - (bits + 7 >> 3))
				{
					return -1L;
				}
				if (bits == 0)
				{
					return 0L;
				}
			}
			long num2 = (b.ptr.bytes[b.ptr.offset] & 0xFF) >> b.endbit;
			if (bits > 8)
			{
				num2 |= (b.ptr.bytes[b.ptr.offset + 1] & 0xFF) << ((8 - b.endbit) & 0x1F);
				if (bits > 16)
				{
					num2 |= (b.ptr.bytes[b.ptr.offset + 2] & 0xFF) << ((16 - b.endbit) & 0x1F);
					if (bits > 24)
					{
						num2 |= (b.ptr.bytes[b.ptr.offset + 3] & 0xFF) << ((24 - b.endbit) & 0x1F);
						if (bits > 32 && b.endbit != 0)
						{
							num2 |= (b.ptr.bytes[b.ptr.offset + 4] & 0xFF) << ((32 - b.endbit) & 0x1F);
						}
					}
				}
			}
			return num & num2;
		}

		public static void oggpack_adv(Ogg.oggpack_buffer b, int bits)
		{
			bits += b.endbit;
			if (b.endbyte > b.storage - (bits + 7 >> 3))
			{
				b.ptr = null;
				b.endbyte = b.storage;
				b.endbit = 1;
			}
			b.ptr.offset += bits / 8;
			b.endbyte += bits / 8;
			b.endbit = bits & 7;
		}

		public static long oggpack_read(Ogg.oggpack_buffer b, int bits)
		{
			if (bits < 0 || bits > 32)
			{
				b.ptr = null;
				b.endbyte = b.storage;
				b.endbit = 1;
				return -1L;
			}
			long num = mask[bits];
			bits += b.endbit;
			if (b.endbyte >= b.storage - 4)
			{
				if (b.endbyte > b.storage - (bits + 7 >> 3))
				{
					b.ptr = null;
					b.endbyte = b.storage;
					b.endbit = 1;
					return -1L;
				}
				if (bits == 0)
				{
					return 0L;
				}
			}
			long num2 = (b.ptr.bytes[b.ptr.offset] & 0xFF) >> b.endbit;
			if (bits > 8)
			{
				num2 |= (b.ptr.bytes[b.ptr.offset + 1] & 0xFF) << ((8 - b.endbit) & 0x1F);
				if (bits > 16)
				{
					num2 |= (b.ptr.bytes[b.ptr.offset + 2] & 0xFF) << ((16 - b.endbit) & 0x1F);
					if (bits > 24)
					{
						num2 |= (b.ptr.bytes[b.ptr.offset + 3] & 0xFF) << ((24 - b.endbit) & 0x1F);
						if (bits > 32 && b.endbit != 0)
						{
							num2 |= (b.ptr.bytes[b.ptr.offset + 4] & 0xFF) << ((32 - b.endbit) & 0x1F);
						}
					}
				}
			}
			num2 &= num;
			b.ptr.offset += bits / 8;
			b.endbyte += bits / 8;
			b.endbit = bits & 7;
			return num2;
		}

		public static long oggpack_bytes(Ogg.oggpack_buffer b)
		{
			return b.endbyte + (b.endbit + 7) / 8;
		}
	}
}
