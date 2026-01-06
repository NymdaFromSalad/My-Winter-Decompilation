using System.Collections;
using SWS;
using UnityEngine;

public class MessageReceiver : MonoBehaviour
{
	private void MyMethod()
	{
	}

	private void PrintText(string text)
	{
		Debug.Log(text);
	}

	private void RotateSprite(float newRot)
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.y = newRot;
		base.transform.eulerAngles = eulerAngles;
	}

	private IEnumerator SetDestination(Object target)
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		navMove myMove = GetComponent<navMove>();
		GameObject tar = (GameObject)target;
		myMove.Pause(false);
		myMove.ChangeSpeed(4f);
		agent.SetDestination(tar.transform.position);
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
		yield return new WaitForSeconds(4f);
		myMove.ChangeSpeed(1.5f);
		myMove.Resume();
	}

	private IEnumerator ActivateForTime(Object target)
	{
		GameObject tar = (GameObject)target;
		tar.SetActive(true);
		yield return new WaitForSeconds(6f);
		tar.SetActive(false);
	}
}
