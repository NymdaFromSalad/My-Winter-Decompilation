using HutongGames.PlayMaker;

[Tooltip("Pause all sound effects and Playlists in Master Audio.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioEverythingPause : FsmStateAction
{
	public override void OnEnter()
	{
		MasterAudio.PauseEverything();
		Finish();
	}

	public override void Reset()
	{
	}
}
