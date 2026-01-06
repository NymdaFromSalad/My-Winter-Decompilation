using System;
using System.Collections;
using UnityEngine;

namespace SWS
{
	[AddComponentMenu("Simple Waypoint System/navMove")]
	[RequireComponent(typeof(NavMeshAgent))]
	public class navMove : MonoBehaviour
	{
		public enum LoopType
		{
			none = 0,
			loop = 1,
			pingPong = 2,
			random = 3
		}

		public PathManager pathContainer;

		public int currentPoint;

		public bool onStart;

		public bool moveToPath;

		public bool closeLoop;

		public bool updateRotation = true;

		[HideInInspector]
		public RangeValue[] delays;

		[HideInInspector]
		public Messages messages = new Messages();

		public LoopType loopType;

		[HideInInspector]
		public Transform[] waypoints;

		[HideInInspector]
		public bool repeat;

		private NavMeshAgent agent;

		private System.Random rand = new System.Random();

		private int rndIndex;

		private bool waiting;

		private void Start()
		{
			agent = GetComponent<NavMeshAgent>();
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
			waypoints = new Transform[pathContainer.waypoints.Length];
			Array.Copy(pathContainer.waypoints, waypoints, pathContainer.waypoints.Length);
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
			Stop(false);
			StartCoroutine(Move());
		}

		private IEnumerator Move()
		{
			agent.Resume();
			agent.updateRotation = updateRotation;
			if (moveToPath)
			{
				agent.SetDestination(waypoints[currentPoint].position);
				yield return StartCoroutine(WaitForDestination());
				moveToPath = false;
			}
			else
			{
				agent.Warp(waypoints[currentPoint].position);
			}
			if (loopType == LoopType.random)
			{
				StartCoroutine(ReachedEnd());
			}
			else
			{
				StartCoroutine(NextWaypoint());
			}
		}

		private IEnumerator NextWaypoint()
		{
			messages.Execute(this, currentPoint);
			yield return new WaitForEndOfFrame();
			if (delays[currentPoint] != null && delays[currentPoint].max > 0f)
			{
				yield return StartCoroutine(WaitDelay());
			}
			while (waiting)
			{
				yield return null;
			}
			Transform next = null;
			if (loopType == LoopType.pingPong && repeat)
			{
				currentPoint--;
			}
			else if (loopType == LoopType.random)
			{
				rndIndex++;
				currentPoint = int.Parse(waypoints[rndIndex].name.Replace("Waypoint ", string.Empty));
				next = waypoints[rndIndex];
			}
			else
			{
				currentPoint++;
			}
			currentPoint = Mathf.Clamp(currentPoint, 0, waypoints.Length - 1);
			if (next == null)
			{
				next = waypoints[currentPoint];
			}
			agent.SetDestination(next.position);
			yield return StartCoroutine(WaitForDestination());
			if ((loopType != LoopType.random && currentPoint == waypoints.Length - 1) || rndIndex == waypoints.Length - 1 || (repeat && currentPoint == 0))
			{
				StartCoroutine(ReachedEnd());
			}
			else
			{
				StartCoroutine(NextWaypoint());
			}
		}

		private IEnumerator WaitForDestination()
		{
			while (agent.pathPending)
			{
				yield return null;
			}
			float remain = agent.remainingDistance;
			while (remain == float.PositiveInfinity || remain - agent.stoppingDistance > float.Epsilon || agent.pathStatus != NavMeshPathStatus.PathComplete)
			{
				remain = agent.remainingDistance;
				yield return null;
			}
		}

		private IEnumerator WaitDelay()
		{
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
				messages.Execute(this, currentPoint);
				if (delays[currentPoint] != null && delays[currentPoint].max > 0f)
				{
					yield return StartCoroutine(WaitDelay());
				}
				yield break;
			case LoopType.loop:
				messages.Execute(this, currentPoint);
				if (delays[currentPoint] != null && delays[currentPoint].max > 0f)
				{
					yield return StartCoroutine(WaitDelay());
				}
				if (closeLoop)
				{
					agent.SetDestination(waypoints[0].position);
					yield return StartCoroutine(WaitForDestination());
				}
				else
				{
					agent.Warp(waypoints[0].position);
				}
				currentPoint = 0;
				break;
			case LoopType.pingPong:
				repeat = !repeat;
				break;
			case LoopType.random:
			{
				Array.Copy(pathContainer.waypoints, waypoints, pathContainer.waypoints.Length);
				int n = waypoints.Length;
				while (n > 1)
				{
					int k = rand.Next(n--);
					Transform temp = waypoints[n];
					waypoints[n] = waypoints[k];
					waypoints[k] = temp;
				}
				Transform first = pathContainer.waypoints[currentPoint];
				for (int i = 0; i < waypoints.Length; i++)
				{
					if (waypoints[i] == first)
					{
						Transform temp2 = waypoints[0];
						waypoints[0] = waypoints[i];
						waypoints[i] = temp2;
						break;
					}
				}
				rndIndex = 0;
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

		public void Stop()
		{
			Stop(false);
		}

		public void Stop(bool stopUpdates)
		{
			StopAllCoroutines();
			agent.Stop(stopUpdates);
		}

		public void ResetMove(bool reposition)
		{
			Stop(true);
			currentPoint = 0;
			if ((bool)pathContainer && reposition)
			{
				agent.Warp(pathContainer.waypoints[currentPoint].position);
			}
		}

		public void Pause()
		{
			Pause(false);
		}

		public void Pause(bool stopUpdates)
		{
			waiting = true;
			agent.Stop(stopUpdates);
		}

		public void Resume()
		{
			waiting = false;
			agent.Resume();
		}

		public void ChangeSpeed(float value)
		{
			agent.speed = value;
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
