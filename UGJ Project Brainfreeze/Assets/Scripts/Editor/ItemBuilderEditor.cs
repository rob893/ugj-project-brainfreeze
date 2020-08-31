using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item), true)]
public class ItemBuilderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var myScript = (Item)target;
		if (GUILayout.Button("Set Item id to next available id"))
		{
			myScript.SetItemId();
		}
	}
}