using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set some CharacterMotor properties")]
	[ActionCategory("Character")]
	public class SetCharacterMotorProperties : FsmStateAction
	{
		//[CheckForComponent(typeof(CharacterMotor))]
		//[RequiredField]
		//[Tooltip("CharacterMotor GameObject to control.")]
		//public FsmOwnerDefault gameObject;

		[ActionSection("General")]
		public FsmBool canControl;

		[ActionSection("Movement")]
		public FsmFloat maxForwardSpeed;

		public FsmFloat maxSidewaysSpeed;

		public FsmFloat maxBackwardsSpeed;

		public FsmFloat maxGroundAcceleration;

		public FsmFloat gravity;

		public FsmFloat maxFallSpeed;

		[ActionSection("Jumping")]
		public FsmBool jumpEnabled;

		public FsmFloat baseHeight;

		public FsmFloat extraHeight;

		public FsmFloat perpAmount;

		public FsmFloat steepPerpAmount;

		[ActionSection("Moving Platform")]
		public FsmBool movingPlatformEnabled;

		[ActionSection("Sliding")]
		public FsmBool slidingEnabled;

		public FsmFloat slidingSpeed;

		public FsmFloat sidewaysControl;

		public FsmFloat speedControl;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		//private CharacterMotor _motor;

		public override void Reset()
		{
			//gameObject = null;
			canControl = new FsmBool
			{
				UseVariable = true
			};
			maxForwardSpeed = new FsmFloat
			{
				UseVariable = true
			};
			maxSidewaysSpeed = new FsmFloat
			{
				UseVariable = true
			};
			maxBackwardsSpeed = new FsmFloat
			{
				UseVariable = true
			};
			maxGroundAcceleration = new FsmFloat
			{
				UseVariable = true
			};
			gravity = new FsmFloat
			{
				UseVariable = true
			};
			maxFallSpeed = new FsmFloat
			{
				UseVariable = true
			};
			jumpEnabled = new FsmBool
			{
				UseVariable = true
			};
			baseHeight = new FsmFloat
			{
				UseVariable = true
			};
			extraHeight = new FsmFloat
			{
				UseVariable = true
			};
			perpAmount = new FsmFloat
			{
				UseVariable = true
			};
			steepPerpAmount = new FsmFloat
			{
				UseVariable = true
			};
			movingPlatformEnabled = new FsmBool
			{
				UseVariable = true
			};
			slidingEnabled = new FsmBool
			{
				UseVariable = true
			};
			slidingSpeed = new FsmFloat
			{
				UseVariable = true
			};
			sidewaysControl = new FsmFloat
			{
				UseVariable = true
			};
			speedControl = new FsmFloat
			{
				UseVariable = true
			};
			everyFrame = false;
		}

		public override void OnEnter()
		{
			//GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (true)//(gameObject == null)
			{
				LogWarning("Missing gameObject");
				return;
			}
			//_motor = gameObject.GetComponent<CharacterMotor>();
			if (true)//if (_motor == null)
			{
				LogWarning("Missing CharacterMotor");
				return;
			}
			DoSetProperties();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetProperties();
		}

		private void DoSetProperties()
		{
			if (!canControl.IsNone)
			{
				//_motor.canControl = canControl.Value;
			}
			if (!maxForwardSpeed.IsNone)
			{
				//_motor.movement.maxForwardSpeed = maxForwardSpeed.Value;
			}
			if (!maxSidewaysSpeed.IsNone)
			{
				//_motor.movement.maxSidewaysSpeed = maxSidewaysSpeed.Value;
			}
			if (!maxBackwardsSpeed.IsNone)
			{
				//_motor.movement.maxBackwardsSpeed = maxBackwardsSpeed.Value;
			}
			if (!maxGroundAcceleration.IsNone)
			{
				//_motor.movement.maxGroundAcceleration = maxGroundAcceleration.Value;
			}
			if (!gravity.IsNone)
			{
				//_motor.movement.gravity = gravity.Value;
			}
			if (!maxFallSpeed.IsNone)
			{
				//_motor.movement.maxFallSpeed = maxFallSpeed.Value;
			}
			if (!jumpEnabled.IsNone)
			{
				//_motor.jumping.enabled = jumpEnabled.Value;
			}
			if (!baseHeight.IsNone)
			{
				//_motor.jumping.baseHeight = baseHeight.Value;
			}
			if (!extraHeight.IsNone)
			{
				//_motor.jumping.extraHeight = extraHeight.Value;
			}
			if (!perpAmount.IsNone)
			{
				//_motor.jumping.perpAmount = perpAmount.Value;
			}
			if (!steepPerpAmount.IsNone)
			{
				//_motor.jumping.steepPerpAmount = steepPerpAmount.Value;
			}
			if (!movingPlatformEnabled.IsNone)
			{
				//_motor.movingPlatform.enabled = movingPlatformEnabled.Value;
			}
			if (true)//(!slidingEnabled.IsNone && _motor.sliding.enabled != slidingEnabled.Value)
			{
				//_motor.sliding.enabled = slidingEnabled.Value;
			}
			if (!slidingSpeed.IsNone)
			{
				//_motor.sliding.slidingSpeed = slidingSpeed.Value;
			}
			if (!sidewaysControl.IsNone)
			{
				//_motor.sliding.sidewaysControl = sidewaysControl.Value;
			}
			if (!speedControl.IsNone)
			{
				//_motor.sliding.speedControl = speedControl.Value;
			}
		}
	}
}
