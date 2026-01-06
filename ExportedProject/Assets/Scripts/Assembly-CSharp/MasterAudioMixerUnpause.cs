using HutongGames.PlayMaker;

[Tooltip("Unpause all sound effects in Master Audio. Does not include Playlists.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioMixerUnpause : FsmStateAction
{
	public override void OnEnter()
	{
		MasterAudio.UnpauseMixer();
		Finish();
	}

	public override void Reset()
	{
	}
}
