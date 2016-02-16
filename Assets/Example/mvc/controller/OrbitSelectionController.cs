using UnityEngine;
using System.Collections;
using PowIoC;

public class OrbitSelectionController : AbstractSelectionController {
	[Inject]
	ILogger logger;
	[Inject(false)]
	float factor;
	[Inject(false)]
	int elements;
	[Inject(false)]
	float distance;

	bool sliding;

	public override void Setup () {
		logger.Context = this.GetType().ToString();
	}

	public override void First () {
		logger.Log("Start");
		factor = factor == 0f ? 5f : factor;
		elements = elements == 0 ? view.games.Length : elements;
		distance = distance == 0f ? 7f : distance;
		Transform ncTransform;
		Vector3 newPosition;
		float unit = Mathf.PI / (elements / 2f);

		for(int i = 0; i < elements; i++){
			GameObject newCube = null;
			newCube = (GameObject)Instantiate(particle.gameObject);
			newCube.renderer.material.mainTexture = view.games[i];
			newCube.AddComponent<SelectionItem>();
			CapsuleCollider col = newCube.AddComponent<CapsuleCollider>();
			col.radius = 1.2f;
			col.height = 2.5f;
			col.direction = 2;
			ncTransform = newCube.transform;
			newCube.name = "Item" + i;
			newPosition = new Vector3(Mathf.Sin( i * unit ) * distance, 0, Mathf.Cos( i  * unit ) * distance);
			ncTransform.parent = transform;
			ncTransform.localPosition = newPosition;
		}
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

	protected override void OnSelected (int index) {
		logger.Log("Selected " + index);
		monoBehaviour.StartCoroutine(DampTo(index));
	}

	IEnumerator DampTo (int index) {
		float degree = Mathf.Abs(1 - (index + view.games.Length / 2 )) / (float)view.games.Length * 360f;
		Vector3 angle = Vector3.up * transform.localEulerAngles.y;
		float velocity = 0f, duration = 0.3f, time = 0f;

		logger.LogFormat("d: {0}, a: {1}", degree, angle);
		for(;;){
			time += Time.deltaTime;
			transform.localEulerAngles = angle;
			angle.y = Mathf.SmoothDampAngle(angle.y, degree, ref velocity, duration);
			yield return 0;
			if(time >= duration)
				break;
		}
		logger.Log("Done");
	}

	IEnumerator Job () {
		if(sliding)
			yield return 0;
		sliding = true;
		float velocity = 0f;
		float direction = 0f;
		Vector3 angle = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
		while(sliding){
			direction = Input.GetAxis("Mouse X") * factor;
			yield return 0;
			angle.y -= direction;
			transform.localEulerAngles = angle;
			if(Input.GetButtonUp("Fire1")){
				velocity = Mathf.Abs(direction - Input.GetAxis("Mouse X") * 10);
				velocity = Mathf.Min(velocity, 3);
				break;
			}
		}

		while(velocity > 0f){
			angle.y -= direction > 0 ? velocity : -velocity;
			velocity -= Time.deltaTime;
			transform.localEulerAngles = angle;
			if(Input.GetButtonDown("Fire1"))
				break;
			yield return 0;
		}
		sliding = false;
	}
}