using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using PowIoC;

public class LoggerFilters : ScriptableObject {
	[Inject(false)]
	private int level;
	[Inject(false)]
	private string[] messageFilters;
	[Inject(false)]
	private string[] classFilters;
	[Inject(false)]
	private string logPattern;
	public int Level { get {return level;}}
	
	public string LogPattern { get {return logPattern;}}

	public bool IsFiltered (string context, string msg) {
		bool ignored = false;
		if(Array.IndexOf(classFilters, context) > -1)
			ignored = true;

		if(messageFilters != null && messageFilters.Length > 0) {
			foreach(string pattern in messageFilters) {
				if(Regex.IsMatch(msg, pattern)) {
					ignored = true;
					break;
				}
			}
		}

		return ignored;
	}
} 