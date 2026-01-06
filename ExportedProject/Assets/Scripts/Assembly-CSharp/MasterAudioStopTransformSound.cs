using HutongGames.PlayMaker;
using UnityEngine;

[HutongGames.PlayMaker.Tooltip("Stop sounds made by a Transform in Master Audio")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioStopTransformSound : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("The Game Object to stop sounds made by")]
	[RequiredField]
	public FsmOwnerDefault gameObject = new FsmOwnerDefault();

	[HutongGames.PlayMaker.Tooltip("Check this to perform action on all Sound Groups")]
	public FsmBool allGroups;

	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

	public override void OnEnter()
	{
		if (!allGroups.Value && string.IsNullOrEmpty(soundGroupName.Value))
		{
			Debug.LogError("You must either check 'All Groups' or enter the Sound Group Name");
			return;
		}
		GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value.gameObject : base.Owner.gameObject);
		if (allGroups.Value)
		{
			MasterAudio.StopAllSoundsOfTransform(gameObject.transform);
		}
		else
		{
			if (string.IsNullOrEmpty(soundGroupName.Value))
			{
				Debug.LogError("You must either check 'All Groups' or enter the Sound Group Name");
			}
			MasterAudio.StopSoundGroupOfTransform(gameObject.transform, soundGroupName.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		gameObject = new FsmOwnerDefault();
		allGroups = new FsmBool(false);
		soundGroupName = new FsmString(string.Empty);
	}
}
