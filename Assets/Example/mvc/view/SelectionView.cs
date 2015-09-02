using UnityEngine;
using PowIoC;

public class SelectionView : MonoBehaviour {
	[Inject(false)]
	ILogger logger;
	[Inject]
	[SerializeField]
	AbstractSelectionController controller;
	public ParticleSystem particle;
	public AnimationClip coverFlowClip;
	public Texture2D[] games;

	void Awake () {
		Injector.Inject(this);
		logger.Context = this.GetType().ToString();

		controller.view = this;
		controller.transform = transform;
		controller.monoBehaviour = this;
		controller.particle = particle;
		controller.First();
	}

	void Start () {
		controller.Second();
	}

	void Update () {
		controller.Update();
	}

	void LateUpdate () {
		controller.LateUpdate();
	}

	void FixedUpdate () {
		controller.FixedUpdate();
	}

	void OnDestroy () {
		controller.OnDestroy();
	}
}