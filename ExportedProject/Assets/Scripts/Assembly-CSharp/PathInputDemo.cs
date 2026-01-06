using SWS;
using UnityEngine;

public class PathInputDemo : MonoBehaviour
{
	public float speedMultiplier = 10f;

	public float progress;

	private minimalMove move;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
		move = GetComponent<minimalMove>();
		move.StartMove();
		move.Pause();
		progress = 0f;
	}

	private void Update()
	{
		float num = speedMultiplier / 100f;
		if (Input.GetKey("right"))
		{
			progress += Time.deltaTime * num;
			progress = Mathf.Clamp01(progress);
			base.transform.position = move.tween.GetPointOnPath(progress);
			if (move.orientToPath == minimalMove.OrientToPathType.to3D)
			{
				float num2 = ((!(move.lookAhead > 0.01f)) ? 0.01f : move.lookAhead);
				base.transform.LookAt(move.tween.GetPointOnPath(Mathf.Clamp01(progress + num2)));
			}
		}
		if (Input.GetKey("left"))
		{
			progress -= Time.deltaTime * num;
			progress = Mathf.Clamp01(progress);
			base.transform.position = move.tween.GetPointOnPath(progress);
			if (move.orientToPath == minimalMove.OrientToPathType.to3D)
			{
				float num3 = ((!(move.lookAhead > 0.01f)) ? 0.01f : move.lookAhead);
				num3 = 0f - num3;
				base.transform.LookAt(move.tween.GetPointOnPath(Mathf.Clamp01(progress + num3)));
			}
		}
		if ((Input.GetKey("right") || Input.GetKey("left")) && progress != 0f && progress != 1f)
		{
			animator.SetFloat("Speed", move.speed);
		}
		else
		{
			animator.SetFloat("Speed", 0f);
		}
	}
}
