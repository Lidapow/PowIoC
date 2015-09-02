using UnityEngine;

public interface IScriptableBehaviour {
	Transform transform { set; get; }
	MonoBehaviour monoBehaviour { set; get; }
	void First ();
	void Second ();
	void Update ();
	void LateUpdate ();
	void FixedUpdate ();
	void OnDestroy ();
}