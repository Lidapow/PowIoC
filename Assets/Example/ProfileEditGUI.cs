using UnityEngine;
using System.Collections;
using PowIoC;

public class ProfileEditGUI : ScriptableObject, IGUI {
	[Inject]
	SomeData someData;

	public void OnGUI () {
		if(!someData.edit)
			return;
		someData.someStr = GUILayout.TextField(someData.someStr);
		someData.someInt = int.Parse(GUILayout.TextField(someData.someInt.ToString()));
		someData.someFloat = float.Parse(GUILayout.TextField(someData.someFloat.ToString()));
		someData.edit = !GUILayout.Button("Done");
	}
} 