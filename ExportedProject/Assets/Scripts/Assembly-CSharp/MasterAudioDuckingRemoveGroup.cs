using HutongGames.PlayMaker;

[Tooltip("Remove a Sound Group in Master Audio from the list of sounds that cause music ducking.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioDuckingRemoveGroup : FsmStateAction
{
	[RequiredField]
	[Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

	public override void OnEnter()
	{
		MasterAudio.RemoveSoundGroupFromDuckList(soundGroupName.Value);
		Finish();
	}

	public override void Reset()
	{
		soundGroupName = new FsmString(string.Empty);
	}
}
