using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using PowIoC;

public class SettingLoader : ScriptableObject, ISettingLoader {
	[Inject(false)]
	string fileName;
	[Inject(false)]
	string filePath;

	// Use this for initialization
	public string GetSettings () {
		StringBuilder sb = new StringBuilder();
		string path = filePath ?? Application.dataPath;
		Debug.Log(path);
		sb//.Append("file://")
		  .Append(path)
		  .Append("/")
		  .Append(fileName);
		return LoadSettingFile (sb.ToString());
	}

	string LoadSettingFile (string path) 
	{
		string retValue = "";
		if(File.Exists(path)) {
			byte[] data;
			using(Stream stream = new FileStream(path, FileMode.Open)){
				data = new byte[stream.Length];
				stream.Read(data, 0, (int)stream.Length);
			}
			retValue = Encoding.UTF8.GetString(data);
		} else {
			// logger.LogFormat("Setting file not exists. path: {0}", path);
		}
		return retValue;
	}
}
