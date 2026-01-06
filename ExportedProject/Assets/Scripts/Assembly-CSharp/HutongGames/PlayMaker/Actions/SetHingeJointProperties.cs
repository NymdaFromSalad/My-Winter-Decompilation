using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Set some hinge joints properties")]
	public class SetHingeJointProperties : FsmStateAction
	{
		[Tooltip("JointSpring GameObject to control.")]
		[CheckForComponent(typeof(HingeJoint))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[CheckForComponent(typeof(Rigidbody))]
		[ActionSection("General")]
		public FsmGameObject connectedBody;

		public FsmFloat breakForce;

		public FsmFloat breakTorque;

		public FsmVector3 anchor;

		public FsmVector3 axis;

		[ActionSection("Spring")]
		public FsmBool useSpring;

		public FsmFloat spring;

		public FsmFloat damper;

		public FsmFloat targetPosition;

		[ActionSection("Motor")]
		public FsmBool useMotor;

		public FsmFloat targetVelocity;

		public FsmFloat force;

		public FsmBool freeSpin;

		[ActionSection("Limits")]
		public FsmBool useLimits;

		public FsmFloat min;

		public FsmFloat max;

		public FsmFloat minBounce;

		public FsmFloat maxBounce;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private HingeJoint _joint;

		public override void Reset()
		{
			gameObject = null;
			connectedBody = new FsmGameObject
			{
				UseVariable = true
			};
			breakForce = new FsmFloat
			{
				UseVariable = true
			};
			breakTorque = new FsmFloat
			{
				UseVariable = true
			};
			anchor = new FsmVector3
			{
				UseVariable = true
			};
			axis = new FsmVector3
			{
				UseVariable = true
			};
			useSpring = new FsmBool
			{
				UseVariable = true
			};
			spring = new FsmFloat
			{
				UseVariable = true
			};
			damper = new FsmFloat
			{
				UseVariable = true
			};
			targetPosition = new FsmFloat
			{
				UseVariable = true
			};
			useMotor = new FsmBool
			{
				UseVariable = true
			};
			targetVelocity = new FsmFloat
			{
				UseVariable = true
			};
			force = new FsmFloat
			{
				UseVariable = true
			};
			freeSpin = new FsmBool
			{
				UseVariable = true
			};
			useLimits = new FsmBool
			{
				UseVariable = true
			};
			min = new FsmFloat
			{
				UseVariable = true
			};
			max = new FsmFloat
			{
				UseVariable = true
			};
			minBounce = new FsmFloat
			{
				UseVariable = true
			};
			maxBounce = new FsmFloat
			{
				UseVariable = true
			};
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (gameObject == null)
			{
				LogWarning("Missing gameObject");
				return;
			}
			_joint = gameObject.GetComponent<HingeJoint>();
			if (_joint == null)
			{
				LogWarning("Missing HingeJoint");
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
			JointSpring jointSpring = _joint.spring;
			JointMotor motor = _joint.motor;
			JointLimits limits = _joint.limits;
			if (!connectedBody.IsNone)
			{
				_joint.connectedBody = connectedBody.Value.GetComponent<Rigidbody>();
			}
			if (!anchor.IsNone)
			{
				_joint.anchor = anchor.Value;
			}
			if (!axis.IsNone)
			{
				_joint.axis = axis.Value;
			}
			if (!useSpring.IsNone)
			{
				_joint.useSpring = useSpring.Value;
			}
			if (!spring.IsNone)
			{
				jointSpring.spring = spring.Value;
				_joint.spring = jointSpring;
			}
			if (!damper.IsNone)
			{
				jointSpring.damper = damper.Value;
				_joint.spring = jointSpring;
			}
			if (!targetPosition.IsNone)
			{
				jointSpring.targetPosition = targetPosition.Value;
				_joint.spring = jointSpring;
			}
			if (!useMotor.IsNone)
			{
				_joint.useMotor = useMotor.Value;
			}
			if (!targetVelocity.IsNone)
			{
				motor.targetVelocity = targetVelocity.Value;
				_joint.motor = motor;
			}
			if (!force.IsNone)
			{
				motor.force = force.Value;
				_joint.motor = motor;
			}
			if (!freeSpin.IsNone)
			{
				motor.freeSpin = freeSpin.Value;
				_joint.motor = motor;
			}
			if (!useLimits.IsNone)
			{
				_joint.useLimits = useLimits.Value;
			}
			if (!min.IsNone)
			{
				limits.min = min.Value;
				_joint.limits = limits;
			}
			if (!max.IsNone)
			{
				limits.max = max.Value;
				_joint.limits = limits;
			}
			if (!minBounce.IsNone)
			{
				limits.minBounce = minBounce.Value;
				_joint.limits = limits;
			}
			if (!maxBounce.IsNone)
			{
				limits.maxBounce = maxBounce.Value;
				_joint.limits = limits;
			}
		}
	}
}
