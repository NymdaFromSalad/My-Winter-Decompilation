using UnityEngine;

public class Popup
{
	private static int popupListHash = "PopupList".GetHashCode();

	public static bool List(Rect position, ref bool showList, ref int listEntry, string[] listContent)
	{
		int controlID = GUIUtility.GetControlID(popupListHash, FocusType.Passive);
		bool flag = false;
		switch (Event.current.GetTypeForControl(controlID))
		{
		case EventType.MouseDown:
			if (position.Contains(Event.current.mousePosition))
			{
				GUIUtility.hotControl = controlID;
				showList = true;
			}
			break;
		case EventType.MouseUp:
			if (showList)
			{
				flag = true;
			}
			break;
		}
		GUI.Label(position, listContent[listEntry]);
		if (showList)
		{
			Rect position2 = new Rect(position.x, position.y, position.width, (position.height + 20f) * (float)listContent.Length);
			GUI.Box(position2, string.Empty);
			listEntry = GUI.SelectionGrid(position2, listEntry, listContent, 1);
		}
		if (flag)
		{
			showList = false;
		}
		return flag;
	}
}
