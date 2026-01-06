using System;
using System.Collections.Generic;

namespace TanjentOGG
{
	public class TanjentOGG
	{
		public int SampleRate;

		public int Channels;

		public float[] DecodedFloats;

		private int decodedFloatsIndex;

		private Registry registry;

		private Ogg.ogg_sync_state oy;

		private Ogg.ogg_stream_state os;

		private Ogg.ogg_page og;

		private Ogg.ogg_packet op;

		private Codec.vorbis_info vi;

		private Codec.vorbis_comment vc;

		private Codec.vorbis_dsp_state vd;

		private Codec.vorbis_block vb;

		private CPtr.BytePtr buffer;

		private int bytes;

		private int convsize;

		private CPtr.BytePtr stdin;

		private int eos;

		public TanjentOGG()
		{
			registry = new Registry();
			oy = new Ogg.ogg_sync_state();
			os = new Ogg.ogg_stream_state();
			og = new Ogg.ogg_page();
			op = new Ogg.ogg_packet();
			vi = new Codec.vorbis_info();
			vc = new Codec.vorbis_comment();
			vd = new Codec.vorbis_dsp_state();
			vb = new Codec.vorbis_block();
		}

		public void DecodeToFloats(byte[] fileBytes)
		{
			DecodedFloats = new float[0];
			int num = 0;
			List<float[]> list = new List<float[]>();
			Framing.ogg_sync_init(oy);
			stdin = new CPtr.BytePtr(fileBytes);
			convsize = 4096;
			while (true)
			{
				eos = 0;
				buffer = Framing.ogg_sync_buffer(oy, 4096L);
				bytes = fread(buffer, 1, 4096, stdin);
				Framing.ogg_sync_wrote(oy, bytes);
				if (Framing.ogg_sync_pageout(oy, og) != 1)
				{
					if (bytes < 4096)
					{
						break;
					}
					return;
				}
				Framing.ogg_stream_init(os, Framing.ogg_page_serialno(og));
				Info.vorbis_info_init(vi);
				Info.vorbis_comment_init(vc);
				if (Framing.ogg_stream_pagein(os, og) < 0 || Framing.ogg_stream_packetout(os, op) != 1 || Info.vorbis_synthesis_headerin(registry, vi, vc, op) < 0)
				{
					return;
				}
				int i = 0;
				while (i < 2)
				{
					while (i < 2)
					{
						switch (Framing.ogg_sync_pageout(oy, og))
						{
						case 1:
							Framing.ogg_stream_pagein(os, og);
							for (; i < 2; i++)
							{
								int num2 = Framing.ogg_stream_packetout(os, op);
								if (num2 == 0)
								{
									break;
								}
								if (num2 < 0)
								{
									return;
								}
								num2 = Info.vorbis_synthesis_headerin(registry, vi, vc, op);
								if (num2 < 0)
								{
									return;
								}
							}
							continue;
						default:
							continue;
						case 0:
							break;
						}
						break;
					}
					buffer = Framing.ogg_sync_buffer(oy, 4096L);
					bytes = fread(buffer, 1, 4096, stdin);
					if (bytes == 0 && i < 2)
					{
						return;
					}
					Framing.ogg_sync_wrote(oy, bytes);
				}
				convsize = 4096 / vi.channels;
				Channels = vi.channels;
				if (Block.vorbis_synthesis_init(registry, vd, vi) != 0)
				{
					continue;
				}
				Block.vorbis_block_init(vd, vb);
				while (eos == 0)
				{
					while (eos == 0)
					{
						int num3 = Framing.ogg_sync_pageout(oy, og);
						if (num3 == 0)
						{
							break;
						}
						if (num3 < 0)
						{
							continue;
						}
						Framing.ogg_stream_pagein(os, og);
						while (true)
						{
							num3 = Framing.ogg_stream_packetout(os, op);
							if (num3 == 0)
							{
								break;
							}
							if (num3 < 0)
							{
								continue;
							}
							CPtr.FloatPtr[] array = null;
							if (Synthesis.vorbis_synthesis(registry, vb, op) == 0)
							{
								Block.vorbis_synthesis_blockin(registry, vd, vb);
							}
							CPtr.FloatPtr[][] array2 = new CPtr.FloatPtr[1][] { array };
							int num4;
							while ((num4 = Block.vorbis_synthesis_pcmout(vd, array2)) > 0)
							{
								array = array2[0];
								int num5 = ((num4 >= convsize) ? convsize : num4);
								float[] array3 = new float[num5 * vi.channels];
								for (i = 0; i < vi.channels; i++)
								{
									int num6 = i;
									int num7 = array[i].offset;
									while (num7 < array[i].offset + num5)
									{
										array3[num6] = array[i].floats[num7];
										num7++;
										num6 += vi.channels;
									}
								}
								num += array3.Length;
								list.Add(array3);
								Block.vorbis_synthesis_read(vd, num5);
							}
							SampleRate = (int)vi.rate;
						}
						if (Framing.ogg_page_eos(og) != 0)
						{
							eos = 1;
						}
					}
					if (eos == 0)
					{
						buffer = Framing.ogg_sync_buffer(oy, 4096L);
						bytes = fread(buffer, 1, 4096, stdin);
						Framing.ogg_sync_wrote(oy, bytes);
						if (bytes == 0)
						{
							eos = 1;
						}
					}
				}
			}
			DecodedFloats = new float[num];
			int num8 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				Array.Copy(list[j], 0, DecodedFloats, num8, list[j].Length);
				num8 += list[j].Length;
			}
		}

		public int DecodeToFloatSamples(byte[] fileBytes, float[] floatSamples)
		{
			int num = 0;
			if (floatSamples == null)
			{
				return num;
			}
			int num2 = floatSamples.Length;
			if (num2 <= 0)
			{
				return num;
			}
			if (stdin == null)
			{
				eos = 0;
				DecodedFloats = new float[0];
				Framing.ogg_sync_init(oy);
				stdin = new CPtr.BytePtr(fileBytes);
				buffer = Framing.ogg_sync_buffer(oy, 4096L);
				bytes = fread(buffer, 1, 4096, stdin);
				Framing.ogg_sync_wrote(oy, bytes);
				if (Framing.ogg_sync_pageout(oy, og) != 1)
				{
					if (bytes < 4096)
					{
						eos = 1;
						return num;
					}
					eos = 1;
					return -1;
				}
				Framing.ogg_stream_init(os, Framing.ogg_page_serialno(og));
				Info.vorbis_info_init(vi);
				Info.vorbis_comment_init(vc);
				if (Framing.ogg_stream_pagein(os, og) < 0)
				{
					eos = 1;
					return -1;
				}
				if (Framing.ogg_stream_packetout(os, op) != 1)
				{
					eos = 1;
					return -1;
				}
				if (Info.vorbis_synthesis_headerin(registry, vi, vc, op) < 0)
				{
					eos = 1;
					return -1;
				}
				int i = 0;
				while (i < 2)
				{
					while (i < 2)
					{
						switch (Framing.ogg_sync_pageout(oy, og))
						{
						case 1:
							Framing.ogg_stream_pagein(os, og);
							for (; i < 2; i++)
							{
								int num3 = Framing.ogg_stream_packetout(os, op);
								if (num3 == 0)
								{
									break;
								}
								if (num3 < 0)
								{
									eos = 1;
									return -1;
								}
								num3 = Info.vorbis_synthesis_headerin(registry, vi, vc, op);
								if (num3 < 0)
								{
									eos = 1;
									return -1;
								}
							}
							continue;
						default:
							continue;
						case 0:
							break;
						}
						break;
					}
					buffer = Framing.ogg_sync_buffer(oy, 4096L);
					bytes = fread(buffer, 1, 4096, stdin);
					if (bytes == 0 && i < 2)
					{
						eos = 1;
						return -1;
					}
					Framing.ogg_sync_wrote(oy, bytes);
				}
				convsize = 4096 / vi.channels;
				Channels = vi.channels;
				if (Block.vorbis_synthesis_init(registry, vd, vi) != 0)
				{
					eos = 1;
					return -1;
				}
				Block.vorbis_block_init(vd, vb);
			}
			while (eos == 0)
			{
				while (eos == 0)
				{
					if (decodedFloatsIndex < DecodedFloats.Length)
					{
						int val = Math.Min(num2, DecodedFloats.Length - decodedFloatsIndex);
						val = Math.Min(val, num2 - num);
						Array.Copy(DecodedFloats, decodedFloatsIndex, floatSamples, num, val);
						decodedFloatsIndex += val;
						num += val;
						if (num >= num2)
						{
							return num;
						}
					}
					int num4 = Framing.ogg_sync_pageout(oy, og);
					if (num4 == 0)
					{
						break;
					}
					if (num4 < 0)
					{
						continue;
					}
					Framing.ogg_stream_pagein(os, og);
					while (true)
					{
						num4 = Framing.ogg_stream_packetout(os, op);
						if (num4 == 0)
						{
							break;
						}
						if (num4 < 0)
						{
							continue;
						}
						CPtr.FloatPtr[] array = null;
						if (Synthesis.vorbis_synthesis(registry, vb, op) == 0)
						{
							Block.vorbis_synthesis_blockin(registry, vd, vb);
						}
						CPtr.FloatPtr[][] array2 = new CPtr.FloatPtr[1][] { array };
						int num5;
						while ((num5 = Block.vorbis_synthesis_pcmout(vd, array2)) > 0)
						{
							array = array2[0];
							int num6 = ((num5 >= convsize) ? convsize : num5);
							int num7 = 0;
							if (decodedFloatsIndex < DecodedFloats.Length)
							{
								num7 = DecodedFloats.Length - decodedFloatsIndex;
							}
							float[] array3 = new float[num6 * vi.channels + num7];
							Array.Copy(DecodedFloats, decodedFloatsIndex, array3, 0, num7);
							for (int i = 0; i < vi.channels; i++)
							{
								int num8 = num7 + i;
								int num9 = array[i].offset;
								while (num9 < array[i].offset + num6)
								{
									array3[num8] = array[i].floats[num9];
									num9++;
									num8 += vi.channels;
								}
							}
							decodedFloatsIndex = 0;
							DecodedFloats = array3;
							Block.vorbis_synthesis_read(vd, num6);
						}
						SampleRate = (int)vi.rate;
					}
					if (Framing.ogg_page_eos(og) != 0)
					{
						eos = 1;
					}
				}
				if (eos == 0)
				{
					buffer = Framing.ogg_sync_buffer(oy, 4096L);
					bytes = fread(buffer, 1, 4096, stdin);
					Framing.ogg_sync_wrote(oy, bytes);
					if (bytes == 0)
					{
						eos = 1;
					}
				}
			}
			if (eos == 1 && decodedFloatsIndex < DecodedFloats.Length)
			{
				int val2 = Math.Min(num2, DecodedFloats.Length - decodedFloatsIndex);
				val2 = Math.Min(val2, num2 - num);
				Array.Copy(DecodedFloats, decodedFloatsIndex, floatSamples, num, val2);
				decodedFloatsIndex += val2;
				num += val2;
				if (num >= num2)
				{
					return num;
				}
			}
			return num;
		}

		private int fread(CPtr.BytePtr ptr, int size, int count, CPtr.BytePtr stream)
		{
			for (int i = 0; i < size * count; i++)
			{
				if (stream.offset >= stream.bytes.Length)
				{
					return i;
				}
				ptr.bytes[ptr.offset + i] = stream.bytes[stream.offset];
				stream.offset++;
			}
			return size * count;
		}
	}
}
