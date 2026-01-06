namespace TanjentOGG
{
	public class Ogg
	{
		public class oggpack_buffer
		{
			public long endbyte;

			public int endbit;

			public CPtr.BytePtr buffer;

			public CPtr.BytePtr ptr;

			public long storage;

			public void clear()
			{
				endbyte = 0L;
				endbit = 0;
				buffer = null;
				ptr = null;
				storage = 0L;
			}
		}

		public class ogg_page
		{
			public CPtr.BytePtr header;

			public long header_len;

			public CPtr.BytePtr body;

			public long body_len;
		}

		public class ogg_stream_state
		{
			public CPtr.BytePtr body_data;

			public long body_storage;

			public long body_fill;

			public long body_returned;

			public int[] lacing_vals;

			public long[] granule_vals;

			public long lacing_storage;

			public long lacing_fill;

			public long lacing_packet;

			public long lacing_returned;

			public byte[] header = new byte[282];

			public int header_fill;

			public int e_o_s;

			public int b_o_s;

			public long serialno;

			public long pageno;

			public long packetno;

			public long granulepos;

			public void clear()
			{
				body_data = null;
				body_storage = 0L;
				body_fill = 0L;
				body_returned = 0L;
				lacing_vals = null;
				granule_vals = null;
				lacing_storage = 0L;
				lacing_fill = 0L;
				lacing_packet = 0L;
				lacing_returned = 0L;
				for (int i = 0; i < header.Length; i++)
				{
					header[i] = 0;
				}
				header_fill = 0;
				e_o_s = 0;
				b_o_s = 0;
				serialno = 0L;
				pageno = 0L;
				packetno = 0L;
				granulepos = 0L;
			}
		}

		public class ogg_packet
		{
			public CPtr.BytePtr packet;

			public long bytes;

			public long b_o_s;

			public long e_o_s;

			public long granulepos;

			public long packetno;
		}

		public class ogg_sync_state
		{
			public CPtr.BytePtr data;

			public int storage;

			public int fill;

			public int returned;

			public int unsynced;

			public int headerbytes;

			public int bodybytes;

			public void clear()
			{
				data = null;
				storage = 0;
				fill = 0;
				returned = 0;
				unsynced = 0;
				headerbytes = 0;
				bodybytes = 0;
			}
		}
	}
}
