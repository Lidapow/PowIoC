using UnityEngine;
using PowIoC;

public class SelectionItem : MonoBehaviour {
	[Inject]
	AbstractSelectionController controller;
	int index;

	void Awake () {
		Injector.Inject(this);
	}

	void Start () {
		index = int.Parse(name.Replace("Item", ""));
	}

	void OnMouseUpAsButton () {
		controller.Selected(index);
	}
}