using HutongGames.PlayMaker;
using UnityEngine;

[HutongGames.PlayMaker.Tooltip("Change the pich of a variation (or all variations) in a Sound Group in Master Audio")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioVariationChangePitch : FsmStateAction
{
	private Transform trans;

	[RequiredField]
	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

	[HutongGames.PlayMaker.Tooltip("Name of specific variation (optional)")]
	public FsmString variationName;

	[RequiredField]
	public FsmBool changeAllVariations = new FsmBool(false);

	[HasFloatSlider(-3f, 3f)]
	public FsmFloat pitch = new FsmFloat(1f);

	[HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
	public bool everyFrame;

	public override void OnEnter()
	{
		ChangePitch();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		ChangePitch();
	}

	private void ChangePitch()
	{
		if (trans == null)
		{
			trans = base.Owner.transform;
		}
		string value = soundGroupName.Value;
		string value2 = variationName.Value;
		if (string.IsNullOrEmpty(value2))
		{
			value2 = null;
		}
		MasterAudio.ChangeVariationPitch(value, changeAllVariations.Value, value2, pitch.Value);
	}

	public override void Reset()
	{
		soundGroupName = new FsmString(string.Empty);
		variationName = new FsmString(string.Empty);
		changeAllVariations = new FsmBool(false);
		pitch = new FsmFloat(1f);
	}
}
