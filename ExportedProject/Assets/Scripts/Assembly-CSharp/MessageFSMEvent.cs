using UnityEngine;

public class MessageFSMEvent : MonoBehaviour
{
	private PlayMakerFSM fsm;

	private void FsmOwner(Object owner)
	{
		fsm = (PlayMakerFSM)owner;
	}

	private void FsmEvent(string name)
	{
		if ((bool)fsm)
		{
			fsm.SendEvent(name);
		}
	}
}
