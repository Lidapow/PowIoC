using UnityEngine;
using System.Collections;
using PowIoC;

public class SomeComponent : MonoBehaviour {
	[Inject(false)]
	ILogger logger;
	[Inject]
	public SomeData data;
	[Inject("view")] IGUI viewer;
	[Inject("edit")] IGUI editor;

	void Awake () {
		Injector.Inject(this);
		logger.Context = this.GetType().ToString();
		logger.Log("Hello");
	}

	void Start () {
		
	}
	
	void Update () {
	
	}

	void OnGUI () {
		editor.OnGUI();
		viewer.OnGUI();
	}
}
