using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Audio)]
[Tooltip("Set the Playlist Master volume in Master Audio to a specific volume.")]
public class MasterAudioPlaylistSetVolume : FsmStateAction
{
	[Tooltip("Playlist New Volume")]
	[HasFloatSlider(0f, 1f)]
	[RequiredField]
	public FsmFloat newVolume;

	[Tooltip("Repeat every frame while the state is active.")]
	public bool everyFrame;

	public override void OnEnter()
	{
		SetVolume();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		SetVolume();
	}

	private void SetVolume()
	{
		MasterAudio.PlaylistMasterVolume = newVolume.Value;
	}

	public override void Reset()
	{
		newVolume = new FsmFloat();
	}
}
