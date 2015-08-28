using UnityEngine;

namespace PowIoC 
{
	public interface IParsable {
		T Parse<T> (string rawData) where T : ScriptableObject;
		string ToRawString (object data);
	} 
}