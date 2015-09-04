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
		Destroy(collider);
		BoxCollider col = gameObject.AddComponent<BoxCollider>();
		col.size = new Vector3(7f, 0.1f, 7f);
	}

	void OnMouseUpAsButton () {
		controller.Selected(index);
	}
}