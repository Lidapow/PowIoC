using UnityEngine;
using System.Collections;
using PowIoC;

public class OtherComponent : MonoBehaviour {
	[Inject(false)]
	ILogger logger;
	[Inject(false)]
	public SomeData data;

	void Awake () {
		Injector.Inject(this);
		logger.Context = this.GetType().ToString();
		logger.Log("Hello");
	}
	
	void Update () {
	
	}
}
