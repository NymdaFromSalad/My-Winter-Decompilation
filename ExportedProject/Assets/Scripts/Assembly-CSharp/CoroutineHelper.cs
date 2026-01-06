using System.Collections;
using UnityEngine;

public static class CoroutineHelper
{
	public static IEnumerator WaitForActualSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return MasterAudio.EndOfFrameDelay;
		}
	}
}
