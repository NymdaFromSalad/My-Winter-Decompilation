using System;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UnityEngine;

namespace SWS
{
	[AddComponentMenu("Simple Waypoint System/minimalMove")]
	public class minimalMove : MonoBehaviour
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

		public PathManager pathContainer;

		public PathType pathType = PathType.Curved;

		public bool onStart;

		public bool moveToPath;

		public bool closeLoop;

		public OrientToPathType orientToPath;

		public float lookAhead;

		public TimeValue timeValue = TimeValue.speed;

		public float speed = 5f;

		public EaseType easeType;

		public AnimationCurve animEaseType;

		public LoopType loopType;

		[HideInInspector]
		public Vector3[] waypoints;

		[HideInInspector]
		public bool repeat;

		public Axis lockAxis;

		public Axis lockPosition;

		public Tweener tween;

		private TweenParms tParms;

		private PlugVector3Path plugPath;

		private float originSpeed;

		private void Start()
		{
			if (onStart)
			{
				StartMove();
			}
		}

		public void StartMove()
		{
			if (pathContainer == null)
			{
				Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
				return;
			}
			waypoints = pathContainer.GetPathPoints();
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
				base.transform.position = waypoints[0];
			}
			CreateTween();
		}

		private IEnumerator MoveToPath()
		{
			int max = ((waypoints.Length <= 4) ? waypoints.Length : 4);
			Vector3[] wpPos = pathContainer.GetPathPoints();
			waypoints = new Vector3[max];
			for (int i = 1; i < max; i++)
			{
				waypoints[i] = wpPos[i - 1];
			}
			waypoints[0] = base.transform.position;
			CreateTween();
			if (tween.isPaused)
			{
				tween.Play();
			}
			waypoints = pathContainer.GetPathPoints();
			yield return StartCoroutine(tween.UsePartialPath(-1, 1).WaitForCompletion());
			moveToPath = false;
			if (tween != null)
			{
				tween.Kill();
			}
			tween = null;
		}

		private void CreateTween()
		{
			plugPath = new PlugVector3Path(waypoints, true, pathType);
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
			if (loopType == LoopType.loop && closeLoop)
			{
				plugPath.ClosePath(true);
			}
			tParms = new TweenParms();
			tParms.Prop("position", plugPath);
			tParms.AutoKill(false);
			tParms.Loops(1);
			if (!moveToPath)
			{
				tParms.OnComplete(ReachedEnd);
			}
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
			tween = HOTween.To(base.transform, originSpeed, tParms);
			if (originSpeed != speed)
			{
				ChangeSpeed(speed);
			}
		}

		private void ReachedEnd()
		{
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
				tween.Restart();
				break;
			case LoopType.pingPong:
				if (tween != null)
				{
					tween.Kill();
				}
				tween = null;
				repeat = !repeat;
				Array.Reverse(waypoints);
				CreateTween();
				break;
			}
		}

		public void SetPath(PathManager newPath)
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
				base.transform.position = pathContainer.waypoints[0].position;
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
	}
}
