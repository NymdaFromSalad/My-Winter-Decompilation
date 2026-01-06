using HutongGames.PlayMaker;

[Tooltip("Stop all sound effects in Master Audio. Does not include Playlists.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioMixerStop : FsmStateAction
{
	public override void OnEnter()
	{
		MasterAudio.StopMixer();
		Finish();
	}

	public override void Reset()
	{
	}
}
