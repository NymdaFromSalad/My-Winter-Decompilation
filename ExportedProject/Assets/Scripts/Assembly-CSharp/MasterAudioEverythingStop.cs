using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Audio)]
[Tooltip("Stop all sound effects and Playlists in Master Audio.")]
public class MasterAudioEverythingStop : FsmStateAction
{
	public override void OnEnter()
	{
		MasterAudio.StopEverything();
		Finish();
	}

	public override void Reset()
	{
	}
}
