using UnityEngine;

public interface ICustomEventReceiver
{
	void CheckForIllegalCustomEvents();

	void ReceiveEvent(string customEventName, Vector3 originPoint);

	bool SubscribesToEvent(string customEventName);

	void RegisterReceiver();

	void UnregisterReceiver();
}
