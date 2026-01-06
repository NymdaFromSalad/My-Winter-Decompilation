using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("It calculates frames/second over each updateInterval.It is also fairly accurate at very low FPS counts (<10).We do this not by simply counting frames per interval, butby accumulating FPS for each frame. This way we end up withcorrect overall FPS even if the interval renders something like 5.5 frames. credits: http://unifycommunity.com/wiki/index.php?title=FramesPerSecond")]
	[ActionCategory(ActionCategory.Time)]
	public class GetFPS : FsmStateAction
	{
		[Tooltip("Interval sampling")]
		public FsmFloat updateInterval = 0.5f;

		private float accum;

		private int frames;

		private float timeleft;

		[Tooltip("The current Frame per second")]
		public FsmFloat FPS;

		[Tooltip("The current Frame per second formated as string")]
		public FsmString FPS_asString;

		public override void Reset()
		{
			updateInterval = 0.5f;
			FPS = null;
		}

		public override void OnUpdate()
		{
			timeleft -= Time.deltaTime;
			accum += Time.timeScale / Time.deltaTime;
			frames++;
			if ((double)timeleft <= 0.0)
			{
				FPS.Value = accum / (float)frames;
				FPS_asString.Value = string.Empty + (accum / (float)frames).ToString("f2");
				timeleft = updateInterval.Value;
				accum = 0f;
				frames = 0;
			}
		}
	}
}
