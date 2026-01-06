using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Audio)]
[HutongGames.PlayMaker.Tooltip("Play a Sound in Master Audio")]
public class MasterAudioPlaySound : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("The GameObject to use for sound position.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[RequiredField]
	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

	[HutongGames.PlayMaker.Tooltip("Name of specific variation (optional)")]
	public FsmString variationName;

	[RequiredField]
	[HasFloatSlider(0f, 1f)]
	public FsmFloat volume = new FsmFloat(1f);

	[HasFloatSlider(0f, 10f)]
	public FsmFloat delaySound = new FsmFloat(0f);

	public FsmBool useThisLocation = new FsmBool(true);

	public FsmBool attachToGameObject = new FsmBool(false);

	public FsmBool useFixedPitch = new FsmBool(false);

	[HutongGames.PlayMaker.Tooltip("Fixed Pitch will be used only if 'Use Fixed Pitch' is checked above.")]
	[HasFloatSlider(-3f, 3f)]
	public FsmFloat fixedPitch = new FsmFloat(1f);

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
		Transform transform = ownerDefaultTarget.transform;
		string value = soundGroupName.Value;
		string value2 = variationName.Value;
		bool value3 = attachToGameObject.Value;
		bool value4 = useThisLocation.Value;
		float value5 = volume.Value;
		float value6 = delaySound.Value;
		float? pitch = fixedPitch.Value;
		if (!useFixedPitch.Value)
		{
			pitch = null;
		}
		if (string.IsNullOrEmpty(value2))
		{
			value2 = null;
		}
		if (!value4 && !value3)
		{
			MasterAudio.PlaySoundAndForget(value, value5, pitch, value6, value2);
		}
		else
		{
			MasterAudio.PlaySound3DAndForget(value, transform, value3, value5, pitch, value6, value2);
		}
		Finish();
	}

	public override void Reset()
	{
		gameObject = null;
		soundGroupName = new FsmString(string.Empty);
		variationName = new FsmString(string.Empty);
		volume = new FsmFloat(1f);
		delaySound = new FsmFloat(0f);
		useThisLocation = new FsmBool(true);
		attachToGameObject = new FsmBool(false);
		useFixedPitch = new FsmBool(false);
		fixedPitch = new FsmFloat(1f);
	}
}
