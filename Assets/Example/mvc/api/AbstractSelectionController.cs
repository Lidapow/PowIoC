using UnityEngine;
using PowIoC;

public abstract class AbstractSelectionController : ScriptableObject, IScriptableBehaviour {
	[Inject]
	protected Selection selection;
	public ParticleSystem particle { set; get; }
	public Transform transform { set; get; }
	public MonoBehaviour monoBehaviour { set; get; }
	public SelectionView view { set; get; }
	public abstract void First ();
	public abstract void Second ();
	public abstract void Update ();
	public abstract void LateUpdate ();
	public abstract void FixedUpdate ();
	public abstract void OnDestroy ();
}