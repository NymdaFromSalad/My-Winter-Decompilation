using System;

namespace TanjentOGG
{
	public class CPtr
	{
		public class BytePtr
		{
			public int offset;

			public byte[] bytes;

			public BytePtr(byte[] data)
			{
				offset = 0;
				bytes = data;
			}

			public BytePtr(BytePtr b)
			{
				offset = b.offset;
				bytes = b.bytes;
			}

			public BytePtr(BytePtr b, int offset)
			{
				this.offset = b.offset + offset;
				bytes = b.bytes;
			}

			public static BytePtr malloc(long size)
			{
				return new BytePtr(new byte[(int)size]);
			}

			public static BytePtr realloc(BytePtr b, int newSize)
			{
				if (b == null)
				{
					return malloc(newSize);
				}
				if (b.bytes == null)
				{
					return malloc(newSize);
				}
				if (newSize <= 0)
				{
					return null;
				}
				byte[] array = new byte[newSize];
				Array.Copy(b.bytes, b.offset, array, 0, Math.Min(b.bytes.Length - b.offset, newSize));
				return new BytePtr(array);
			}

			public static int memcmp(BytePtr ptr1, BytePtr ptr2, int num)
			{
				for (int i = 0; i < num; i++)
				{
					if (ptr1.bytes[ptr1.offset + i] != ptr2.bytes[ptr2.offset + i])
					{
						return ptr1.bytes[ptr1.offset + i].CompareTo(ptr2.bytes[ptr2.offset + i]);
					}
				}
				return 0;
			}

			public static BytePtr memchr(BytePtr ptr, int value, int num)
			{
				BytePtr bytePtr = new BytePtr(ptr);
				for (int i = 0; i < num; i++)
				{
					if (ptr.bytes[ptr.offset + i] == value)
					{
						bytePtr.offset = ptr.offset + i;
						return bytePtr;
					}
				}
				return null;
			}

			public static BytePtr memmove(BytePtr destination, BytePtr source, int num)
			{
				byte[] array = new byte[num];
				Array.Copy(source.bytes, source.offset, array, 0, num);
				Array.Copy(array, 0, destination.bytes, destination.offset, num);
				return destination;
			}

			public static BytePtr memcpy(BytePtr destination, BytePtr source, int num)
			{
				Array.Copy(source.bytes, source.offset, destination.bytes, destination.offset, num);
				return destination;
			}
		}

		public class FloatPtr
		{
			public int offset;

			public float[] floats;

			public FloatPtr(float[] data)
			{
				offset = 0;
				floats = data;
			}

			public FloatPtr(float[] data, int offset)
			{
				this.offset = offset;
				floats = data;
			}

			public FloatPtr(FloatPtr f)
			{
				offset = f.offset;
				floats = f.floats;
			}

			public FloatPtr(FloatPtr f, int offset)
			{
				this.offset = f.offset + offset;
				floats = f.floats;
			}

			public static FloatPtr memset(FloatPtr ptr, int value, long num)
			{
				for (int i = 0; i < num; i++)
				{
					ptr.floats[ptr.offset + i] = value;
				}
				return ptr;
			}
		}
	}
}
