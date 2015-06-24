using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class virtualDetector : MonoBehaviour {


	public GameObject unit;
	public Vector2 stickLen;
	//public Vector3 scale;
	public Material lit;
	public Material dark;

	//which cameras does object detect?
	//public Camera[] cam;
	public Camera cam;

	private List<Transform> objects = new List<Transform> ();

	private GameObject clone;
	private SmoothFollow smoo;
	private Plane[] planes;

	public Shader shader1 ;
	public Shader shader2 ;
	
	// Use this for initialization
	void Start () {
		shader1 = Shader.Find("Particles/Alpha Blended");
		 shader2 = Shader.Find("Particles/Alpha Blended Premultiply");

		smoo = Camera.main.GetComponent<SmoothFollow>();
		//cam = Camera.allCameras;

		int i = 0;
		int j = 0;

		while (i < stickLen[0]) {
			while (j< stickLen[1]){

				clone = (GameObject)Instantiate(unit, new Vector3(i* 1.0F,j * 1.0F, 0), Quaternion.Euler (0, 180,0));
				clone.transform.parent = this.transform;
				j++;

			}

			i++;
			j=0;
		}
		/*
		while (i < stickLen) {
			clone = (GameObject)Instantiate(unit, new Vector3(0,i * 1.0F, 0), Quaternion.Euler (0, 180,0));
			clone.transform.parent = this.transform;
			i++;
		}
		*/
		//find the leds as topmost children
		FindLeaves (transform, objects);
		//calculate frustum at regular intervals
		InvokeRepeating ("calcFrustum", 0, .3f);
	}


	void calcFrustum() {

		//Debug.Log ("calculating Frustum");


			planes = GeometryUtility.CalculateFrustumPlanes(cam);


			/*
		foreach (Camera camx in cam){
			planes = GeometryUtility.CalculateFrustumPlanes(cam);
		}
		*/
	}
	
	void FindLeaves(Transform item, List<Transform> leafArray)
	{
		if (item.childCount == 0)
		{
			leafArray.Add(item);
		}
		else
		{
			foreach (Transform child in item)
			{
				FindLeaves(child, leafArray);
			}
		}
	}

	// Update is called once per frame
	void Update () {

		//for camera tracking
		if (Input.GetKeyDown(KeyCode.Space)) {
			smoo.enabled = !smoo.enabled;
		}

		foreach (Transform obT in objects) {
			GameObject ob = obT.gameObject;
			//Debug.Log(ob.name);
			if (ob.name != "Cube") {
			//foreach (Plane[] pl in planes){
				if (GeometryUtility.TestPlanesAABB(planes, ob.collider.bounds)){
					ob.renderer.material = lit;
					TimedTrailRenderer[] ttr = ob.GetComponents<TimedTrailRenderer>();
					ttr[0].emit = true;
					//StartCoroutine(switchEmitter(ttr[1], false));
					//ttr[1].emit = false;
					//ob.GetComponent<TimedTrailRenderer>().emit = true;
					
				//ob.GetComponent<TrailRenderer>().enabled = true;
					//Debug.Log(ob.name + " has been detected!");
				} else {
					//ob.GetComponent<TrailRenderer>().startWidth = 0.0f;
					//ob.GetComponent<TrailRenderer>().endWidth = 0.0f;
					//ob.GetComponent<TrailRenderer>().enabled = false;

					TimedTrailRenderer[] ttr = ob.GetComponents<TimedTrailRenderer>();
					ttr[0].emit = false;
					ttr[1].emit = true;
					//StartCoroutine(switchEmitter(ttr[0], false));
					//ttr[0].emit = false;

					//ob.GetComponent<TimedTrailRenderer>().emit = false;
					ob.renderer.material = dark;
					//Debug.Log("Nothing has been detected");
				}
			}
		}
	}

	IEnumerator switchEmitter(TimedTrailRenderer ttrEmit, bool tf) {

		yield return new WaitForSeconds(0.1f);
		ttrEmit.emit = tf;
	}





}
