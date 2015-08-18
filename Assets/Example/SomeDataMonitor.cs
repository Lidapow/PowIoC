using UnityEngine;
using System.Threading;

public class SomeDataMonitor : SomeData {
	private Thread thread;
	private bool tRun;
	private int lastInt;

	void OnEnable () {
		tRun = true;
		lastInt = someInt;
		thread = new Thread(new ThreadStart(Job));
		thread.Start();
	}

	void Job () {
		while(tRun) {
			if(lastInt != someInt){
				Debug.Log("someInt changed");
				lastInt = someInt;
				Thread.Sleep(10);
			}
		}
	}

	void OnDisable () {
		tRun = false;
		thread.Join();
	}
}