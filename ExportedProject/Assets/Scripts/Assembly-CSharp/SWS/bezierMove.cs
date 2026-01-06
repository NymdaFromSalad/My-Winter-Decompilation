using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UnityEngine;

namespace SWS
{
	[AddComponentMenu("Simple Waypoint System/bezierMove")]
	public class bezierMove : MonoBehaviour
	{
		public enum OrientToPathType
		{
			none = 0,
			to2DTopDown = 1,
			to2DSideScroll = 2,
			to3D = 3
		}

		public enum TimeValue
		{
			time = 0,
			speed = 1
		}

		public enum LoopType
		{
			none = 0,
			loop = 1,
			pingPong = 2
		}

		public BezierPathManager pathContainer;

		public PathType pathType = PathType.Curved;

		public bool onStart;

		public bool moveToPath;

		public OrientToPathType orientToPath;

		public float lookAhead;

		public float sizeToAdd;

		[HideInInspector]
		public Messages messages = new Messages();

		public TimeValue timeValue = TimeValue.speed;

		public float speed = 5f;

		public EaseType easeType;

		public AnimationCurve animEaseType;

		public LoopType loopType;

		private Vector3[] waypoints;

		public Axis lockAxis;

		public Axis lockPosition;

		public Tweener tween;

		private Vector3[] wpPos;

		private TweenParms tParms;

		private PlugVector3Path plugPath;

		private float positionOnPath = -1f;

		private float originSpeed;

		private void Start()
		{
			if (onStart)
			{
				StartMove();
			}
		}

		private void InitWaypoints()
		{
			wpPos = new Vector3[waypoints.Length];
			for (int i = 0; i < wpPos.Length; i++)
			{
				wpPos[i] = waypoints[i] + new Vector3(0f, sizeToAdd, 0f);
			}
		}

		public void StartMove()
		{
			if (pathContainer == null)
			{
				Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
				return;
			}
			waypoints = pathContainer.pathPoints;
			originSpeed = speed;
			Stop();
			StartCoroutine(Move());
		}

		private IEnumerator Move()
		{
			if (moveToPath)
			{
				yield return StartCoroutine(MoveToPath());
			}
			else
			{
				InitWaypoints();
				base.transform.position = waypoints[0] + new Vector3(0f, sizeToAdd, 0f);
			}
			CreateTween();
		}

		private IEnumerator MoveToPath()
		{
			wpPos = new Vector3[7];
			wpPos[0] = base.transform.position;
			wpPos[1] = 2f * waypoints[0] - waypoints[1] + new Vector3(0f, sizeToAdd, 0f);
			wpPos[2] = 2f * waypoints[0] - waypoints[2] + new Vector3(0f, sizeToAdd, 0f);
			wpPos[3] = waypoints[0] + new Vector3(0f, sizeToAdd, 0f);
			List<Vector3> unsmoothedList = new List<Vector3>();
			for (int i = 0; i < 4; i++)
			{
				unsmoothedList.Add(wpPos[i]);
			}
			Vector3[] smoothed = WaypointManager.SmoothCurve(unsmoothedList, 1).ToArray();
			for (int j = 0; j < 4; j++)
			{
				wpPos[j] = smoothed[j];
			}
			wpPos[4] = waypoints[1] + new Vector3(0f, sizeToAdd, 0f);
			wpPos[5] = pathContainer.bPoints[1].wp.position + new Vector3(0f, sizeToAdd, 0f);
			if (pathContainer.bPoints.Count > 2)
			{
				wpPos[6] = pathContainer.bPoints[2].wp.position + new Vector3(0f, sizeToAdd, 0f);
			}
			else
			{
				wpPos[6] = wpPos[5];
			}
			CreateTween();
			yield return StartCoroutine(tween.UsePartialPath(-1, 3).WaitForCompletion());
			moveToPath = false;
			tween.Kill();
			tween = null;
			InitWaypoints();
		}

		private void CreateTween()
		{
			plugPath = new PlugVector3Path(wpPos, true, pathType);
			if (orientToPath != OrientToPathType.none)
			{
				plugPath.OrientToPath(lookAhead, lockAxis);
			}
			if (orientToPath == OrientToPathType.to2DTopDown)
			{
				plugPath.Is2D();
			}
			else if (orientToPath == OrientToPathType.to2DSideScroll)
			{
				plugPath.Is2D(true);
			}
			if (lockPosition != Axis.None)
			{
				plugPath.LockPosition(lockPosition);
			}
			tParms = new TweenParms();
			tParms.Prop("position", plugPath);
			tParms.AutoKill(false);
			tParms.Loops(1);
			if (timeValue == TimeValue.speed)
			{
				tParms.SpeedBased();
				tParms.Ease(EaseType.Linear);
			}
			else if (easeType == EaseType.AnimationCurve)
			{
				tParms.Ease(animEaseType);
			}
			else
			{
				tParms.Ease(easeType);
			}
			if (!moveToPath)
			{
				tParms.OnUpdate(CheckPoint);
				tParms.OnComplete(ReachedEnd);
			}
			tween = HOTween.To(base.transform, originSpeed, tParms);
			if (originSpeed != speed)
			{
				ChangeSpeed(speed);
			}
		}

		private void CheckPoint()
		{
			float num = positionOnPath;
			positionOnPath = tween.fullElapsed / tween.fullDuration;
			for (int i = 0; i < messages.list.Count; i++)
			{
				if (num < messages.list[i].pos && positionOnPath >= messages.list[i].pos && num != positionOnPath)
				{
					messages.Execute(this, i);
				}
			}
		}

		public IEnumerator Wait(float value)
		{
			tween.Pause();
			float timer = Time.time + value;
			while (Time.time < timer)
			{
				yield return null;
			}
			if (positionOnPath < 1f && positionOnPath != -1f)
			{
				Resume();
			}
		}

		private void ReachedEnd()
		{
			positionOnPath = -1f;
			switch (loopType)
			{
			case LoopType.none:
				if (tween != null)
				{
					tween.Kill();
				}
				tween = null;
				break;
			case LoopType.loop:
				Stop();
				StartMove();
				break;
			case LoopType.pingPong:
			{
				if (tween != null)
				{
					tween.Kill();
				}
				tween = null;
				Vector3[] array = new Vector3[wpPos.Length];
				Array.Copy(wpPos, array, wpPos.Length);
				for (int i = 0; i < wpPos.Length; i++)
				{
					wpPos[i] = array[wpPos.Length - 1 - i];
				}
				MessageOptions[] array2 = new MessageOptions[messages.list.Count];
				messages.list.CopyTo(array2);
				for (int j = 0; j < messages.list.Count; j++)
				{
					messages.list[j].pos = 1f - messages.list[j].pos;
					messages.list[j] = array2[array2.Length - 1 - j];
				}
				CreateTween();
				break;
			}
			}
		}

		public void SetPath(BezierPathManager newPath)
		{
			Stop();
			pathContainer = newPath;
			StartMove();
		}

		public void Stop()
		{
			StopAllCoroutines();
			if (tween != null)
			{
				tween.Kill();
			}
			plugPath = null;
			tween = null;
		}

		public void ResetMove(bool reposition)
		{
			Stop();
			if ((bool)pathContainer && reposition)
			{
				base.transform.position = waypoints[0] + new Vector3(0f, sizeToAdd, 0f);
			}
		}

		public void Pause()
		{
			if (tween != null)
			{
				tween.Pause();
			}
		}

		public void Resume()
		{
			if (tween != null)
			{
				tween.Play();
			}
		}

		public void ChangeSpeed(float value)
		{
			float timeScale = ((timeValue != TimeValue.speed) ? (originSpeed / value) : (value / originSpeed));
			speed = value;
			if (tween != null)
			{
				tween.timeScale = timeScale;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.magenta;
			if (tween != null && !moveToPath)
			{
				for (int i = 0; i < messages.list.Count; i++)
				{
					Gizmos.DrawSphere(tween.GetPointOnPath(messages.list[i].pos), 0.2f);
				}
			}
		}
	}
}
