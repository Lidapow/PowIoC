using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using PowIoC;

public class UnityLogger : ScriptableObject, ILogger {
	[Inject]
	LoggerFilters filters;

	private string context;
	public string Context { 
		get { return context ?? ""; } 
		set { context = context ?? value; }
	}

	public void Log (object message) {
		_Log(message.ToString(), 2);
	}

	public void LogWarning (object message) {
		_LogWarning(message.ToString(), 2);
	}

	public void LogError (object message) {
		_LogError(message.ToString(), 2);
	}

	public void LogFormat (string format, params object[] messages) {
		_Log(string.Format(format, messages), 2);
	}

	public void LogWarningFormat (string format, params object[] messages) {
		_LogWarning(string.Format(format, messages), 2);
	}

	public void LogErrorFormat (string format, params object[] messages) {
		_LogError(string.Format(format, messages), 2);
	}

	void _Log (string message, int stackIndex = 2) {
		if(filters.Level <= 1) {
			if(!filters.IsFiltered(context, message)) {
				if(!string.IsNullOrEmpty(Context))
				Debug.Log(BuildMessage(message, MethodName(stackIndex + 1)));
			}
		}
	}

	void _LogWarning (string message, int stackIndex = 2) {
		if(filters.Level <= 2) {
			if(!filters.IsFiltered(context, message)) {
				if(!string.IsNullOrEmpty(Context))
				Debug.LogWarning(BuildMessage(message, MethodName(stackIndex + 1)));					
			}
		}
	}

	void _LogError (string message, int stackIndex = 2) {
		if(filters.Level <= 3) {
			if(!filters.IsFiltered(context, message)) {
				if(!string.IsNullOrEmpty(Context))
				Debug.LogError(BuildMessage(message, MethodName(stackIndex + 1)));					
			}
		}
	}

	string BuildMessage (string message, string method) {
		string msg = filters.LogPattern;
		if(Regex.IsMatch(msg, "%class"))
			msg = Regex.Replace(msg, "%class", context);
		if(Regex.IsMatch(msg, "%method"))
			msg = Regex.Replace(msg, "%method", method);
		if(Regex.IsMatch(msg, "%time"))
			msg = Regex.Replace(msg, "%time", DateTime.UtcNow.ToString("HH:mm:ss"));
		if(Regex.IsMatch(msg, "%date"))
			msg = Regex.Replace(msg, "%date", DateTime.Today.ToString("d"));
		if(Regex.IsMatch(msg, "%tick"))
			msg = Regex.Replace(msg, "%tick", DateTime.UtcNow.Ticks.ToString());
		if(Regex.IsMatch(msg, "%msg"))
			msg = Regex.Replace(msg, "%msg", message);
		return msg;
	}

	string MethodName (int stackIndex) {
		string methodName = "";
		if(Regex.IsMatch(filters.LogPattern, "%method")) {
			System.Diagnostics.StackFrame frame = new System.Diagnostics.StackTrace().GetFrames()[stackIndex];
			methodName = frame.GetMethod().Name;
		}
		return methodName;

	}
}