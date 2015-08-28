using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using PowIoC;

[Serializable]
public class InjectMap : ScriptableObject { 
	[SerializeField]
	public BindMap[] bind;
	[SerializeField]
	public PrimitiveMap[] primitive;
	[SerializeField]
	public PrimitiveArrayMap[] primitiveArray;
}

namespace PowIoC 
{
	[Serializable]
	public class BindMap {
		public string bind = "";
		public string to = "";
		public string scope = "";
		public string note = "";
	}

	[Serializable]
	public class PrimitiveMap 
	{
		public string fieldPath = "";
		public string fieldValue = "";
		public string note = "";
	}

	[Serializable]
	public class PrimitiveArrayMap
	{
		public string fieldPath = "";
		[System.Xml.Serialization.XmlArrayItem("value", typeof(string))]
		public string[] fieldValue;
		public string note = "";
	}
}