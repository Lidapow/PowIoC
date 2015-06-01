using UnityEngine;
using System;
using System.Collections;
using PowIoC;

[Serializable]
public class SomeData : ScriptableObject {
	[Inject(false)] public bool edit;
	[Inject(false)] public string someStr;
	[Inject(false)] public int someInt;
	[Inject(false)] public float someFloat;
} 