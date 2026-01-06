using UnityEngine.Audio;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the float value of an exposed parameter for a Unity Audio Mixer.")]
	[ActionCategory(ActionCategory.Audio)]
	public class AudioMixerSetFloatValue : FsmStateAction
	{
		[ObjectType(typeof(AudioMixer))]
		[RequiredField]
		[Tooltip("The Audio Mixer with the exposed parameter.")]
		public FsmObject theMixer;

		[RequiredField]
		[Tooltip("The name of the exposed parameter.")]
		[Title("Name of Parameter")]
		public FsmString exposedFloatName;

		[Title("Float Value")]
		public FsmFloat floatvalue;

		public bool everyFrame;

		public override void Reset()
		{
			theMixer = null;
			exposedFloatName = null;
			floatvalue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetMixerFloatValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetMixerFloatValue();
		}

		public void DoSetMixerFloatValue()
		{
			AudioMixer audioMixer = theMixer.Value as AudioMixer;
			if (audioMixer != null && !string.IsNullOrEmpty(exposedFloatName.Value))
			{
				string value = exposedFloatName.Value;
				float value2 = floatvalue.Value;
				audioMixer.SetFloat(value, value2);
			}
		}
	}
}
