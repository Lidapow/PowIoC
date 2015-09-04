using UnityEngine;
using PowIoC;

public class Selection : ScriptableObject {
	public delegate void SelectedDel ();
	public SelectedDel Selected { set; get; }
	public int SelectedIndex { 
		set {
			// if(value != selectedIndex) {
				selectedIndexPrev = selectedIndex;
				selectedIndex = value;
				if(Selected != null) {
					Selected();
				}
			// }
		}
		get {
			return selectedIndex;
		}
	}
	public int SelectedIndexPrev {
		get {
			return selectedIndexPrev;
		}
	}
	int selectedIndex;
	int selectedIndexPrev;
}