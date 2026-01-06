using System.IO;
using System.Threading;
using TanjentOGG;
using UnityEngine;

public class OggStream : MonoBehaviour
{
	private global::TanjentOGG.TanjentOGG tOgg;

	private Thread t1;

	private AudioSource audiosource;

	private bool trackLoadPending;

	public int trackIndex = 1;

	public string trackPath = "C:\\Dev\\Radio";

	public string trackPrefix = "TRACK";

	private static Mutex mutex;

	private void Start()
	{
		audiosource = GetComponent<AudioSource>();
		tOgg = new global::TanjentOGG.TanjentOGG();
		mutex = new Mutex();
		LoadNextTrack();
	}

	private void Update()
	{
		if (trackLoadPending && t1.ThreadState == ThreadState.Stopped)
		{
			t1 = null;
			audiosource.Stop();
			Object.Destroy(audiosource.clip);
			audiosource.clip = AudioClip.Create("StreamedOgg", tOgg.DecodedFloats.Length / 4, tOgg.Channels, tOgg.SampleRate, false);
			audiosource.clip.SetData(tOgg.DecodedFloats, 0);
			audiosource.Play();
			trackLoadPending = false;
		}
		if (Input.GetKeyUp("space"))
		{
			trackIndex++;
			LoadNextTrack();
		}
	}

	private void LoadNextTrack()
	{
		t1 = new Thread(LoadAudioFromThread);
		t1.Start();
		trackLoadPending = true;
	}

	private void LoadAudioFromThread()
	{
		mutex.WaitOne();
		try
		{
			byte[] fileBytes = File.ReadAllBytes(trackPath + "\\" + trackPrefix + trackIndex + ".ogg");
			tOgg.DecodeToFloats(fileBytes);
		}
		finally
		{
			mutex.ReleaseMutex();
		}
	}
}
