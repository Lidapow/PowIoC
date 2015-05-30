using UnityEngine;
using System.Collections;
using PowIoC;

public class SomeComponent : MonoBehaviour {
	[Inject(false)]
	ILogger logger;
	[Inject]
	public SomeData data;

	void Awake () {
		Injector.Inject(this);
		logger.Context = this.GetType().ToString();
	}

	void Start () {
		data.someStr = "Hello";
		data.someInt = 2;
		data.someFloat = 3.4f;
	}
	
	void Update () {
	
	}
}
