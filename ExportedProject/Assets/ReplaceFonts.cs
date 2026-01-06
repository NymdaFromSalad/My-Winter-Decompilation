using UnityEditor;
using UnityEngine;

public class ReplaceFonts
{
	[MenuItem("Tools/Fix Broken Fonts")]
	static void FixFonts()
	{
		// CHANGE THIS PATH
		Font replacementFont = AssetDatabase.LoadAssetAtPath(
			"Assets/New Fonts/FugazOne-Regular.ttf",
			typeof(Font)
			) as Font;
		
		if (replacementFont == null)
		{
			Debug.LogError("Replacement font not found!");
			return;
		}
		
		string[] allAssets = AssetDatabase.GetAllAssetPaths();
		int replaced = 0;
		
		foreach (string path in allAssets)
		{
			if (!path.EndsWith("FugazOne-Regular.asset"))
				continue;
			
			Object asset = AssetDatabase.LoadMainAssetAtPath(path);
			if (asset == null)
				continue;
			
			SerializedObject so = new SerializedObject(asset);
			SerializedProperty prop = so.GetIterator();
			
			bool changed = false;
			
			while (prop.NextVisible(true))
			{
				if (prop.propertyType == SerializedPropertyType.ObjectReference &&
				    prop.objectReferenceValue == null &&
				    prop.type == "PPtr<$Font>")
				{
					prop.objectReferenceValue = replacementFont;
					changed = true;
					replaced++;
				}
			}
			
			if (changed)
				so.ApplyModifiedProperties();
		}
		
		AssetDatabase.SaveAssets();
		Debug.Log("Font fix complete. Replaced " + replaced + " references.");
	}
}
