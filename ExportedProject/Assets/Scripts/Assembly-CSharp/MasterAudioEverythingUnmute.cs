using HutongGames.PlayMaker;

[Tooltip("Unmute all sound effects and Playlists in Master Audio.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioEverythingUnmute : FsmStateAction
{
	public override void OnEnter()
	{
		MasterAudio.UnmuteEverything();
		Finish();
	}

	public override void Reset()
	{
	}
}
