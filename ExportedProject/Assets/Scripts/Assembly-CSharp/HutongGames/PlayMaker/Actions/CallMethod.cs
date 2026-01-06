using System;
using System.Reflection;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	public class CallMethod : FsmStateAction
	{
		[Tooltip("Store the component in an Object variable.\nNOTE: Set theObject variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
		[ObjectType(typeof(MonoBehaviour))]
		public FsmObject behaviour;

		[Tooltip("Name of the method to call on the component")]
		public FsmString methodName;

		[Tooltip("Method paramters. NOTE: these must match the method's signature!")]
		public FsmVar[] parameters;

		[Tooltip("Store the result of the method call.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Store Result")]
		public FsmVar storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private UnityEngine.Object cachedBehaviour;

		private Type cachedType;

		private MethodInfo cachedMethodInfo;

		private ParameterInfo[] cachedParameterInfo;

		private object[] parametersArray;

		private string errorString;

		public override void OnEnter()
		{
			parametersArray = new object[parameters.Length];
			DoMethodCall();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMethodCall();
		}

		private void DoMethodCall()
		{
			if (behaviour.Value == null)
			{
				Finish();
				return;
			}
			if (cachedBehaviour != behaviour.Value)
			{
				errorString = string.Empty;
				if (!DoCache())
				{
					Debug.LogError(errorString);
					Finish();
					return;
				}
			}
			object obj = null;
			if (cachedParameterInfo.Length == 0)
			{
				obj = cachedMethodInfo.Invoke(cachedBehaviour, null);
			}
			else
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					FsmVar fsmVar = parameters[i];
					fsmVar.UpdateValue();
					parametersArray[i] = fsmVar.GetValue();
				}
				obj = cachedMethodInfo.Invoke(cachedBehaviour, parametersArray);
			}
			storeResult.SetValue(obj);
		}

		private bool DoCache()
		{
			cachedBehaviour = behaviour.Value as MonoBehaviour;
			if (cachedBehaviour == null)
			{
				errorString += "Behaviour is invalid!\n";
				Finish();
				return false;
			}
			cachedType = behaviour.Value.GetType();
			cachedMethodInfo = cachedType.GetMethod(methodName.Value);
			if (cachedMethodInfo == null)
			{
				errorString = errorString + "Method Name is invalid: " + methodName.Value + "\n";
				Finish();
				return false;
			}
			cachedParameterInfo = cachedMethodInfo.GetParameters();
			return true;
		}

		public override string ErrorCheck()
		{
			errorString = string.Empty;
			DoCache();
			if (!string.IsNullOrEmpty(errorString))
			{
				return errorString;
			}
			if (parameters.Length != cachedParameterInfo.Length)
			{
				return "Parameter count does not match method.\nMethod has " + cachedParameterInfo.Length + " parameters.\nYou specified " + parameters.Length + " paramaters.";
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				FsmVar fsmVar = parameters[i];
				Type realType = fsmVar.RealType;
				Type parameterType = cachedParameterInfo[i].ParameterType;
				if (!object.ReferenceEquals(realType, parameterType))
				{
					return string.Concat("Parameters do not match method signature.\nParameter ", i + 1, " (", realType, ") should be of type: ", parameterType);
				}
			}
			if (object.ReferenceEquals(cachedMethodInfo.ReturnType, typeof(void)))
			{
				if (!string.IsNullOrEmpty(storeResult.variableName))
				{
					return "Method does not have return.\nSpecify 'none' in Store Result.";
				}
			}
			else if (!object.ReferenceEquals(cachedMethodInfo.ReturnType, storeResult.RealType))
			{
				return "Store Result is of the wrong type.\nIt should be of type: " + cachedMethodInfo.ReturnType;
			}
			return string.Empty;
		}
	}
}
