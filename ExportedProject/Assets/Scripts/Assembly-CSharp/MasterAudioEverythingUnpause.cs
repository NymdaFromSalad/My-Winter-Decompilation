using HutongGames.PlayMaker;

[Tooltip("Unpause all sound effects and Playlists in Master Audio.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioEverythingUnpause : FsmStateAction
{
	public override void OnEnter()
	{
		MasterAudio.UnpauseEverything();
		Finish();
	}

	public override void Reset()
	{
	}
}
