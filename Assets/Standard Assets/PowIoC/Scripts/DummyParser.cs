using UnityEngine;
using PowIoC;

public class DummyParser : ScriptableObject, IParsable {
	public T Parse<T> (string rawData) where T : ScriptableObject {
		return default(T);
	}
	public string ToRawString (object data) {
		return "";
	}

}