using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Audio)]
[Tooltip("Add a Sound Group in Master Audio to the list of sounds that cause music ducking.")]
public class MasterAudioDuckingAddGroup : FsmStateAction
{
	[RequiredField]
	[Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

	[HasFloatSlider(0f, 1f)]
	[Tooltip("Percentage of sound played to start unducking")]
	[RequiredField]
	public FsmFloat beginUnduck;

	public override void OnEnter()
	{
		MasterAudio.AddSoundGroupToDuckList(soundGroupName.Value, beginUnduck.Value);
		Finish();
	}

	public override void Reset()
	{
		soundGroupName = new FsmString(string.Empty);
		float num = 0.5f;
		MasterAudio instance = MasterAudio.Instance;
		if (instance != null)
		{
			num = instance.defaultRiseVolStart;
		}
		beginUnduck = new FsmFloat(num);
	}
}
