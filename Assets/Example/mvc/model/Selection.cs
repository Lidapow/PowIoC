using UnityEngine;
using PowIoC;

public class Selection : ScriptableObject {
	public delegate void Selected ();
	public Selected selected { set; get; }
	public int SelectedIndex { 
		set {
			if(value != selectedIndex) {
				selectedIndexPrev = selectedIndex;
				selectedIndex = value;
				if(selected != null) {
					selected();
				}
			}
		}
		get {
			return selectedIndex;
		}
	}
	int selectedIndex;
	int selectedIndexPrev;
}