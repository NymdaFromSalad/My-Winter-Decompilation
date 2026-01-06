using Holoville.HOTween;
using UnityEngine;

public class RotationHelper : MonoBehaviour
{
	public float duration;

	public int rotation;

	private void Start()
	{
		HOTween.To(base.transform, duration, new TweenParms().Prop("rotation", new Vector3(rotation, 0f, 0f)).Ease(EaseType.Linear).Loops(-1, LoopType.Incremental));
	}
}
