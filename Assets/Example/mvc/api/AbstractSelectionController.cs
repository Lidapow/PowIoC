using UnityEngine;
using PowIoC;

public abstract class AbstractSelectionController : ScriptableObject, IScriptableBehaviour, ISetup, IViewContainer {
	[Inject]
	protected Selection selection;
	public ParticleSystem particle { set; get; }
	public Transform transform { set; get; }
	public MonoBehaviour monoBehaviour { set; get; }
	public SelectionView view { set; get; }
	public virtual void First () {}
	public virtual void Second () {}
	public abstract void Update ();
	public abstract void LateUpdate ();
	public abstract void FixedUpdate ();
	public abstract void OnDestroy ();
	public virtual void Setup () {}
	protected abstract void OnSelected (int index);
	public void Selected (int index) {
		OnSelected(index);
		selection.SelectedIndex = index;
	}

	public void Register (IView view) { 
		this.view = view as SelectionView; 
	}
	public void Deregister (IView view) { 
		this.view = this.view == view ? null : this.view; 
	}

}