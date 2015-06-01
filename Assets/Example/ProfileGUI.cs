using UnityEngine;
using System.Collections;
using PowIoC;

public class ProfileGUI : ScriptableObject, IGUI {
	[Inject]
	SomeData someData;

	public void OnGUI () {
		if(someData.edit)
			return;
		GUILayout.Label(someData.someStr);
		GUILayout.Label(someData.someInt.ToString());
		GUILayout.Label(someData.someFloat.ToString());
		someData.edit = GUILayout.Button("Edit");
	}
} 