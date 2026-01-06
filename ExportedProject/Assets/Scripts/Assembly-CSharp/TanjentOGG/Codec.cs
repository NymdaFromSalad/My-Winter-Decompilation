namespace TanjentOGG
{
	public class Codec
	{
		public class vorbis_info
		{
			public int version;

			public int channels;

			public long rate;

			public long bitrate_upper;

			public long bitrate_nominal;

			public long bitrate_lower;

			public long bitrate_window;

			public codec_setup_info codec_setup;

			public void clear()
			{
				version = 0;
				channels = 0;
				rate = 0L;
				bitrate_upper = 0L;
				bitrate_nominal = 0L;
				bitrate_lower = 0L;
				bitrate_window = 0L;
			}
		}

		public class vorbis_look_floor
		{
		}

		public class vorbis_look_residue
		{
		}

		public class vorbis_info_mode
		{
			public int blockflag;

			public int windowtype;

			public int transformtype;

			public int mapping;
		}

		public class vorbis_info_floor
		{
		}

		public class vorbis_info_residue
		{
		}

		public class vorbis_info_mapping
		{
		}

		public class private_state
		{
			public int[] window = new int[2];

			public Mdct.mdct_lookup[,] transform = new Mdct.mdct_lookup[2, Registry.VI_TRANSFORMB];

			public int modebits;

			public vorbis_look_floor[] flr;

			public vorbis_look_residue[] residue;

			public long sample_count;
		}

		public class vorbis_dsp_state
		{
			public vorbis_info vi;

			public CPtr.FloatPtr[] pcm;

			public CPtr.FloatPtr[] pcmret;

			public int pcm_storage;

			public int pcm_current;

			public int pcm_returned;

			public int preextrapolate;

			public int eofflag;

			public long lW;

			public long W;

			public long nW;

			public long centerW;

			public long granulepos;

			public long sequence;

			public long glue_bits;

			public long time_bits;

			public long floor_bits;

			public long res_bits;

			public private_state backend_state;

			public void clear()
			{
				vi = null;
				pcm = null;
				pcmret = null;
				pcm_storage = 0;
				pcm_current = 0;
				pcm_returned = 0;
				preextrapolate = 0;
				eofflag = 0;
				lW = 0L;
				W = 0L;
				nW = 0L;
				centerW = 0L;
				granulepos = 0L;
				sequence = 0L;
				glue_bits = 0L;
				time_bits = 0L;
				floor_bits = 0L;
				res_bits = 0L;
				backend_state = null;
			}
		}

		public class vorbis_block
		{
			public CPtr.FloatPtr[] pcm;

			public Ogg.oggpack_buffer opb = new Ogg.oggpack_buffer();

			public long lW;

			public long W;

			public long nW;

			public int pcmend;

			public int mode;

			public int eofflag;

			public long granulepos;

			public long sequence;

			public vorbis_dsp_state vd;

			public long glue_bits;

			public long time_bits;

			public long floor_bits;

			public long res_bits;

			public int[] pinternal;

			public void clear()
			{
				pcm = null;
				opb.clear();
				lW = 0L;
				W = 0L;
				nW = 0L;
				pcmend = 0;
				mode = 0;
				eofflag = 0;
				granulepos = 0L;
				sequence = 0L;
				vd = null;
				glue_bits = 0L;
				time_bits = 0L;
				floor_bits = 0L;
				res_bits = 0L;
				pinternal = null;
			}
		}

		public class vorbis_comment
		{
			public CPtr.BytePtr[] user_comments;

			public int[] comment_lengths;

			public int comments;

			public CPtr.BytePtr vendor;

			public void clear()
			{
				user_comments = null;
				comment_lengths = null;
				comments = 0;
				vendor = null;
			}
		}

		public class codec_setup_info
		{
			public long[] blocksizes = new long[2];

			public int modes;

			public int maps;

			public int floors;

			public int residues;

			public int books;

			public vorbis_info_mode[] mode_param = new vorbis_info_mode[64];

			public int[] map_type = new int[64];

			public vorbis_info_mapping[] map_param = new vorbis_info_mapping[64];

			public int[] floor_type = new int[64];

			public vorbis_info_floor[] floor_param = new vorbis_info_floor[64];

			public int[] residue_type = new int[64];

			public vorbis_info_residue[] residue_param = new vorbis_info_residue[64];

			public Codebook.static_codebook[] book_param = new Codebook.static_codebook[256];

			public Codebook.codebook[] fullbooks;

			public int halfrate_flag;
		}

		public static int OV_FALSE = -1;

		public static int OV_EOF = -2;

		public static int OV_HOLE = -3;

		public static int OV_EREAD = -128;

		public static int OV_EFAULT = -129;

		public static int OV_EIMPL = -130;

		public static int OV_EINVAL = -131;

		public static int OV_ENOTVORBIS = -132;

		public static int OV_EBADHEADER = -133;

		public static int OV_EVERSION = -134;

		public static int OV_ENOTAUDIO = -135;

		public static int OV_EBADPACKET = -136;

		public static int OV_EBADLINK = -137;

		public static int OV_ENOSEEK = -138;
	}
}
