using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class drawpath : MonoBehaviour {

	public float speed = 30.0f;
	public Transform sticker;
	static public List<Vector3> points = new List<Vector3>();
	static public List<Vector3> rots = new List<Vector3>();

	private LineRenderer lr;
	private Vector3 origin;
	private float dist;
	private float lineDrawSpeed;
	private bool its = false;

	private int ind = 0;

	void Awake() {
		//Application.persistentDataPath+"/filename.extension" if in Application.persistentDataPath (Library/Cache/Unity Technologies/Default company/)
		//else use "filename" <no extension> if file is located in /Assets/Resources
		List<Dictionary<string,object>> data = CSVReader.Read (Application.persistentDataPath+"/logView.csv");

		//"Time Stamp,Participant,Condition,Condition Name,Block,Group,Pos(x),Pos(y),Pos(z),Quat(x),Quat(y),Quat(z),Quat(w),Gyro(x),Gyro(y),Gyro(z),Touch-Left(x),Touch-Left(y),Touch-Right(x),Touch-Right(y),GameTime(s)");

		for(var i=0; i < data.Count; i++) {
			//Debug.Log (data[i]["Block"]);
			points.Add(new Vector3(Convert.ToSingle(data[i]["Pos(x)"]),Convert.ToSingle(data[i]["Pos(y)"]),Convert.ToSingle(data[i]["Pos(z)"])));
			rots.Add(new Vector3(Convert.ToSingle(data[i]["Rot(x)"]),Convert.ToSingle(data[i]["Rot(y)"]),Convert.ToSingle(data[i]["Rot(z)"])));

			  //foreach (KeyValuePair<string,object> pair in data[i]) {
				//Debug.Log (pair.Key.GetType() +" " + pair.Value.GetType());
				//Debug.Log (pair.Key +" " + pair.Value);
			//}


		}
		
	}
	
	// Use this for initialization
	void Start () {
		origin = new Vector3(0.0f, 0.0f, 0.0f);
		/*
		lr	= GetComponent<LineRenderer>();
		lr.SetPosition(0, origin);
		lr.SetVertexCount(points.Count);
		*/


		//dist = Vector3.Distance(origin, destination.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (!its){
			StartCoroutine(MoveOnPath(true));
			//StartCoroutine(moveStick(sticker, points, rots));
			its = true;
		}
	}

	IEnumerator MoveOnPath(bool loop)
	{
		do
		{
			ind = 0;
			foreach (Vector3 point in points){

				yield return StartCoroutine(MoveToPosition(point));
			}
		}
		while (loop);
	}

	IEnumerator MoveToPosition(Vector3 target)
	{	
		while (transform.position != target)
		{

			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			//THIS is always reporting 0,0,0.  Not sure why.
			//Debug.Log(rots[ind]);
			transform.eulerAngles = rots[ind];

			yield return 0;
		}
		ind++;
	}
	
	IEnumerator moveStick(Transform stick, List<Vector3> pts, List<Vector3> rts) {

		int ind = 0;
		foreach (Vector3 pt	in pts) 
		{
			//Debug.Log (rts[ind]);
			//lr.SetPosition(ind, pt);
			float step = speed * Time.deltaTime;
			stick.position = Vector3.MoveTowards(stick.position, pt, step);
			//stick.position = pt;
			//stick.eulerAngles = Vector3.RotateTowards(stick.eulerAngles, rts[ind], step);
			//stick.eulerAngles = rts[ind];

			ind++;

		}
		yield return null;
		
		
	}
	
	IEnumerator DrawLine(Vector3 orig, List<Vector3> pts, float d) {
		int ind = 0;
		foreach (Vector3 pt	in pts) 
		{
			//dist = Vector3.Distance(orig, pt);  
			//float counter = 0.0f;

			//while (counter < dist){
				//counter += 0.1f/lineDrawSpeed;
				//float x = Mathf.Lerp (0, dist, counter);
				//Vector3 pointAlongLine = x * Vector3.Normalize(pt - orig) + orig;
				//Debug.Log (ind);
				lr.SetPosition(ind, pt);

				//yield return null;
			//}     
			ind++;
	       //	orig = pt;
	    }
		yield return null;


	}


}