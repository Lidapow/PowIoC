using UnityEngine;
using System.Collections;
using PowIoC;

public class OtherComponent : MonoBehaviour {
	[Inject(false)]
	ILogger logger;
	[Inject]
	public SomeData data;

	void Awake () {
		Injector.Inject(this);
		logger.Context = this.GetType().ToString();
	}
	
	void Update () {
	
	}
}
