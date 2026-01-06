using System;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UnityEngine;

namespace SWS
{
	[AddComponentMenu("Simple Waypoint System/splineMove")]
	public class splineMove : MonoBehaviour
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
			pingPong = 2,
			random = 3
		}

		public PathManager pathContainer;

		public PathType pathType = PathType.Curved;

		public int currentPoint;

		public bool onStart;

		public bool moveToPath;

		public bool closeLoop;

		public OrientToPathType orientToPath;

		public bool local;

		public float lookAhead;

		public float sizeToAdd;

		[HideInInspector]
		public RangeValue[] delays;

		[HideInInspector]
		public Messages messages = new Messages();

		public TimeValue timeValue = TimeValue.speed;

		public float speed = 5f;

		public EaseType easeType;

		public AnimationCurve animEaseType;

		public LoopType loopType;

		[HideInInspector]
		public Transform[] waypoints;

		[HideInInspector]
		public bool repeat;

		public Axis lockAxis;

		public Axis lockPosition;

		public Tweener tween;

		private Vector3[] wpPos;

		private TweenParms tParms;

		private PlugVector3Path plugPath;

		private System.Random rand = new System.Random();

		private int[] rndArray;

		private int rndIndex;

		private bool waiting;

		private float originSpeed;

		private int startingPoint;

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
				wpPos[i] = waypoints[i].position + new Vector3(0f, sizeToAdd, 0f);
			}
		}

		public void StartMove()
		{
			if (pathContainer == null)
			{
				Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
				return;
			}
			waypoints = pathContainer.waypoints;
			originSpeed = speed;
			if (delays == null || delays.Length == 0)
			{
				delays = new RangeValue[waypoints.Length];
			}
			else if (delays.Length < waypoints.Length)
			{
				RangeValue[] array = new RangeValue[delays.Length];
				Array.Copy(delays, array, delays.Length);
				delays = new RangeValue[waypoints.Length];
				for (int i = 0; i < delays.Length; i++)
				{
					delays[i] = new RangeValue();
				}
				Array.Copy(array, delays, array.Length);
			}
			if (messages.list.Count > 0)
			{
				messages.Initialize(waypoints.Length);
			}
			Stop();
			if (currentPoint > 0)
			{
				Teleport(currentPoint);
			}
			else
			{
				StartCoroutine(Move());
			}
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
				if (currentPoint == waypoints.Length - 1)
				{
					base.transform.position = wpPos[currentPoint];
				}
				else
				{
					base.transform.position = wpPos[0];
				}
			}
			if (loopType == LoopType.random)
			{
				StartCoroutine(ReachedEnd());
				yield break;
			}
			CreateTween();
			StartCoroutine(NextWaypoint());
		}

		private IEnumerator MoveToPath()
		{
			int max = ((waypoints.Length <= 4) ? waypoints.Length : 4);
			wpPos = new Vector3[max];
			for (int i = 1; i < max; i++)
			{
				wpPos[i] = waypoints[i - 1].position + new Vector3(0f, sizeToAdd, 0f);
			}
			wpPos[0] = base.transform.position;
			CreateTween();
			if (tween.isPaused)
			{
				tween.Play();
			}
			yield return StartCoroutine(tween.UsePartialPath(-1, 1).WaitForCompletion());
			moveToPath = false;
			if (tween != null)
			{
				tween.Kill();
			}
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
			if (loopType == LoopType.loop && closeLoop)
			{
				plugPath.ClosePath(true);
			}
			tParms = new TweenParms();
			if (local)
			{
				tParms.Prop("localPosition", plugPath);
			}
			else
			{
				tParms.Prop("position", plugPath);
			}
			tParms.AutoKill(false);
			tParms.Pause(true);
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
			tween = HOTween.To(base.transform, originSpeed, tParms);
			if (originSpeed != speed)
			{
				ChangeSpeed(speed);
			}
		}

		private IEnumerator NextWaypoint()
		{
			for (int point = startingPoint; point < wpPos.Length - 1; point++)
			{
				messages.Execute(this, currentPoint);
				if (delays[currentPoint] != null && delays[currentPoint].max > 0f)
				{
					yield return StartCoroutine(WaitDelay());
				}
				if (tween == null)
				{
					yield break;
				}
				while (waiting)
				{
					yield return null;
				}
				tween.Play();
				yield return StartCoroutine(tween.UsePartialPath(point, point + 1).WaitForCompletion());
				if (loopType == LoopType.pingPong && repeat)
				{
					currentPoint--;
				}
				else if (loopType == LoopType.random)
				{
					rndIndex++;
					currentPoint = rndArray[rndIndex];
				}
				else
				{
					currentPoint++;
				}
			}
			if (loopType != LoopType.pingPong && loopType != LoopType.random)
			{
				messages.Execute(this, currentPoint);
				if (delays[currentPoint] != null && delays[currentPoint].max > 0f)
				{
					yield return StartCoroutine(WaitDelay());
				}
			}
			startingPoint = 0;
			StartCoroutine(ReachedEnd());
		}

		private IEnumerator WaitDelay()
		{
			tween.Pause();
			float rnd = UnityEngine.Random.Range(delays[currentPoint].min, delays[currentPoint].max);
			float timer = Time.time + rnd;
			while (!waiting && Time.time < timer)
			{
				yield return null;
			}
		}

		private IEnumerator ReachedEnd()
		{
			switch (loopType)
			{
			case LoopType.none:
				if (tween != null)
				{
					tween.Kill();
				}
				tween = null;
				yield break;
			case LoopType.loop:
				if (closeLoop)
				{
					tween.Play();
					yield return StartCoroutine(tween.UsePartialPath(currentPoint, -1).WaitForCompletion());
				}
				currentPoint = 0;
				break;
			case LoopType.pingPong:
				if (tween != null)
				{
					tween.Kill();
				}
				tween = null;
				if (!repeat)
				{
					repeat = true;
					for (int l = 0; l < wpPos.Length; l++)
					{
						wpPos[l] = waypoints[waypoints.Length - 1 - l].position + new Vector3(0f, sizeToAdd, 0f);
					}
				}
				else
				{
					InitWaypoints();
					repeat = false;
				}
				CreateTween();
				break;
			case LoopType.random:
			{
				rndIndex = 0;
				InitWaypoints();
				if (tween != null)
				{
					tween.Kill();
				}
				tween = null;
				rndArray = new int[wpPos.Length];
				for (int i = 0; i < rndArray.Length; i++)
				{
					rndArray[i] = i;
				}
				int n = wpPos.Length;
				while (n > 1)
				{
					int k = rand.Next(n--);
					Vector3 temp = wpPos[n];
					wpPos[n] = wpPos[k];
					wpPos[k] = temp;
					int tmpI = rndArray[n];
					rndArray[n] = rndArray[k];
					rndArray[k] = tmpI;
				}
				Vector3 first = wpPos[0];
				int rndFirst = rndArray[0];
				for (int j = 0; j < wpPos.Length; j++)
				{
					if (rndArray[j] == currentPoint)
					{
						rndArray[j] = rndFirst;
						wpPos[0] = wpPos[j];
						wpPos[j] = first;
					}
				}
				rndArray[0] = currentPoint;
				CreateTween();
				break;
			}
			}
			StartCoroutine(NextWaypoint());
		}

		public void SetPath(PathManager newPath)
		{
			SetPath(newPath, true);
		}

		public void SetPath(PathManager newPath, bool reset)
		{
			if (reset)
			{
				ResetMove(false);
			}
			else
			{
				Stop();
			}
			pathContainer = newPath;
			StartMove();
		}

		public void Teleport(int index)
		{
			if (loopType == LoopType.random)
			{
				Debug.LogWarning("Teleporting doesn't work with looptype set to 'random'. Resetting.");
				index = 0;
			}
			index = Mathf.Clamp(index, 0, waypoints.Length - 1);
			Resume();
			Stop();
			moveToPath = false;
			if (loopType == LoopType.loop && index == waypoints.Length - 1)
			{
				index = 0;
			}
			currentPoint = (startingPoint = index);
			StartCoroutine(Move());
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
			currentPoint = (startingPoint = 0);
			if ((bool)pathContainer && reposition)
			{
				base.transform.position = pathContainer.waypoints[currentPoint].position + new Vector3(0f, sizeToAdd, 0f);
			}
		}

		public void Pause()
		{
			waiting = true;
			if (tween != null)
			{
				tween.Pause();
			}
		}

		public void Resume()
		{
			waiting = false;
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

		public void SetDelay(int index, float min, float max)
		{
			if (delays == null)
			{
				delays = new RangeValue[waypoints.Length];
			}
			delays[index].min = min;
			delays[index].max = max;
		}
	}
}
