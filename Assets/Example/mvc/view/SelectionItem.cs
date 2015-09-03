using UnityEngine;
using PowIoC;

public class SelectionItem : MonoBehaviour {
	[Inject]
	Selection selection;
	int index;

	void Awake () {
		Injector.Inject(this);
	}

	void Start () {
		index = int.Parse(name.Replace("Item", ""));
		Destroy(collider);
		BoxCollider col = gameObject.AddComponent<BoxCollider>();
		col.size = new Vector3(7f, 1, 7f);
	}

	void OnMouseUpAsButton () {
		selection.SelectedIndex = index;
	}
}