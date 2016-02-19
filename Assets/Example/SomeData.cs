using UnityEngine;
using System;
using System.Collections;
using PowIoC;

[Serializable]
public class SomeData : ScriptableObject, ISetup {
	[Inject(false)] protected ILogger logger;
	[Inject(false)] public bool edit;
	[Inject(false)] public string someStr;
	[Inject(false)] public int someInt;
	[Inject(false)] public float someFloat;

	public void Setup () {
		logger.Context = this.GetType().ToString();
		logger.LogFormat("Data injected, someStr:{0}, someInt:{1}, someFloat:{2}", someStr, someInt, someFloat);
	}
} 