using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Reflection;
using PowIoC;

public class XMLParser : ScriptableObject, IParsable {
	[Inject(false)]
	ILogger logger;
	bool first = true;

	[XmlRoot("InjectMap")]
	public class _InjectMap {
		[XmlElement("BindMap")]
		public BindMap[] bind;
		[XmlElement("PrimitiveMap")]
		public PrimitiveMap[] primitive;
		[XmlElement("PrimitiveArrayMap")]
		public PrimitiveArrayMap[] primitiveArray;
	}

	public T Parse<T> (string rawdata) where T : ScriptableObject {
		if(first) {logger.Context = this.GetType().ToString(); first = !first; }
		XmlSerializer ser = new XmlSerializer(typeof(_InjectMap));
		_InjectMap map;
		using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(rawdata))){
			map = (_InjectMap)ser.Deserialize(ms);
		}

		T ret = ScriptableObject.CreateInstance<T>();
		int len;
		foreach(FieldInfo fi in ret.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)){
			switch(fi.Name){
				case "bind":
					len = map.bind.Length;
					(ret as InjectMap).bind = new BindMap[len];
					System.Array.Copy(map.bind, 0, (ret as InjectMap).bind, 0, len);

				break;
				case "primitive":
					len = map.primitive.Length;
					(ret as InjectMap).primitive = new PrimitiveMap[len];
					System.Array.Copy(map.primitive, 0, (ret as InjectMap).primitive, 0, len);

				break;
				case "primitiveArray":
					len = map.primitiveArray.Length;
					InjectMap target = ret as InjectMap;
					target.primitiveArray = new PrimitiveArrayMap[len];
					System.Array.Copy(map.primitiveArray, 0, (ret as InjectMap).primitiveArray, 0, len);
				break;
				default:
					//Not supported.
				break;
			}
		}
		return ret;
	}

	public string ToRawString (object data) { return ""; }
}