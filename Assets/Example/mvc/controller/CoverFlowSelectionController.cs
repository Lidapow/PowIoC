using UnityEngine;
using System.Collections;
using PowIoC;

public class CoverFlowSelectionController : AbstractSelectionController {
	[Inject(false)]
	ILogger logger;
	[Inject(false)]
	float factor;
	[Inject(false)]
	int elements;
	[Inject(false)]
	float distance;

	bool sliding;
	float movement;

	private GameObject[] children;

	public override void First () {
		logger.Context = this.GetType().ToString();
		movement = 0.5f;

		factor = factor == 0f ? 50f : factor;
		elements = elements == 0 ? view.games.Length : elements;
		distance = distance == 0f ? 7f : distance;
		children = new GameObject[elements];

		selection.Selected += Clicked;

		for(int i = 0; i < elements; i++){
			GameObject newCube = null;
			newCube = GameObject.CreatePrimitive(PrimitiveType.Plane);
			newCube.renderer.sharedMaterial = particle.renderer.sharedMaterial;
			newCube.name = "Item" + i;
			newCube.transform.localScale = Vector3.one * 0.5f;
			newCube.renderer.material.mainTexture = view.games[i];
			newCube.AddComponent<SelectionItem>();
			children[i] = newCube;
			children[i].transform.parent = transform;
		}

		Setup(movement);
	}

	void Setup (float offset) {
		Vector3 newPosition;
		float evaluate = 0f;
		for(int i = 0; i < children.Length; i++) {
			evaluate = i/(float)elements + offset;
			evaluate = Mathf.Repeat(evaluate, 1f);
			children[i].SampleAnimation(view.coverFlowClip, evaluate);
		}
	}

	public override void Second () {

	}

	public override void Update () {
		if(Input.GetButtonDown("Fire1")){
			monoBehaviour.StartCoroutine(Job());
		}
	}

	public override void LateUpdate () {

	}

	public override void FixedUpdate () {

	}

	public override void OnDestroy () {

	}

	IEnumerator CheckClick () {
		float time = 0f;
		while (time < 0.1) {
			if(!sliding && Input.GetButtonUp("Fire1") && Input.GetAxis("Mouse X") < 0.01){
				selection.SelectedIndex = (int)(Mathf.Repeat(movement, 1f) * view.games.Length);
			}
			time += Time.deltaTime;
			yield return 0;
		}
	}

	IEnumerator Job () {
		// wait a frame, let previsou job done.
		if(sliding) {
			yield return 0; 
		}

		sliding = true;
		bool quit = false;
		float velocity = 0f;
		float direction = 0f;
		for(;;){
			direction = Input.GetAxis("Mouse X") / factor;
			movement = Mathf.Repeat(movement + direction, 1f);
			yield return 0;
			quit = Input.GetButtonUp("Fire1");
			Setup(movement);
			if(quit){
				velocity = Mathf.Abs(direction - Input.GetAxis("Mouse X") / factor);
				velocity = Mathf.Min(velocity, 0.05f);
				break;
			}
		}

		while(velocity > 0f){
			movement = Mathf.Repeat(movement + (direction > 0 ? velocity : -velocity), 1f);
			velocity -= Time.deltaTime / factor;
			Setup(movement);
			if(Input.GetButtonDown("Fire1"))
				break;
			yield return 0;
		}
		sliding = false;
	}

	IEnumerator SmoothDamp () {
		int index = selection.SelectedIndex, indexPrev = selection.SelectedIndexPrev;
		int length = view.games.Length;
		float destination = 1 - (index + length / 2f) / length;
		float velocity = 0f;
		float time = 0;

		while(movement != destination){
			movement = Mathf.SmoothDampAngle(movement * 360, destination * 360, ref velocity, 0.3f) / 360;
			Setup(movement);
			yield return 0;
			if(Mathf.Abs(movement - destination) < 0.001)
				break;
			if(index != selection.SelectedIndex)
				break;
			if(sliding)
				break;
		}
		movement = Mathf.Repeat(movement, 1f);
	}

	void Clicked () {
		monoBehaviour.StartCoroutine(SmoothDamp());
	}
}