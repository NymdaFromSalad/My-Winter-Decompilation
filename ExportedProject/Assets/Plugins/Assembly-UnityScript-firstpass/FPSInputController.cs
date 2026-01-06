using System;
using UnityEngine;

[Serializable]
[AddComponentMenu("Character/FPS Input Controller")]
[RequireComponent(typeof(CharacterMotor))]
public class FPSInputController : MonoBehaviour
{
	private CharacterMotor motor;

	public virtual void Awake()
	{
		motor = (CharacterMotor)GetComponent(typeof(CharacterMotor));
	}

	public virtual void Update()
	{
		Vector3 vector = new Vector3(cInput.GetAxis("PlayerHorizontal"), 0f, cInput.GetAxis("PlayerVertical"));
		if (vector != Vector3.zero)
		{
			float magnitude = vector.magnitude;
			vector /= magnitude;
			magnitude = Mathf.Min(1f, magnitude);
			magnitude *= magnitude;
			vector *= magnitude;
		}
		motor.inputMoveDirection = transform.rotation * vector;
		motor.inputJump = cInput.GetButton("Jump");
	}

	public virtual void Main()
	{
	}
}
