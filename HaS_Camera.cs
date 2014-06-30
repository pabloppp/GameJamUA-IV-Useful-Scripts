using UnityEngine;
using System.Collections;

public class HaS_Camera : MonoBehaviour {

	public enum modes{
		follow,
		cinematic
	}

	public Transform target;
	public modes mode = modes.follow;
	public bool fixedCamera = false;
	public bool restrictFloor = true;
	public float mouseSensitivity = 50;
	public float zoomSensitivity = 10;
	public float minZoomDist = 1;
	public float maxZoomDist = 10;
	Vector3 desf;

	// Use this for initialization
	void Start () {
		desf = transform.position-target.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(target != null){
			transform.position = target.position+desf;
			if(!fixedCamera){
				if(Input.GetMouseButton(1) ){
					if(!restrictFloor || (Input.GetAxis("Mouse Y") < 0 || transform.position.y > target.position.y)){
						transform.RotateAround(target.position, transform.up, Time.deltaTime*Input.GetAxis("Mouse X")*mouseSensitivity);
						transform.RotateAround(target.position, -transform.right, Time.deltaTime*Input.GetAxis("Mouse Y")*mouseSensitivity);
					}

					//transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.z, 0);
					//transform.LookAt(target.position);
				}
				if(Input.GetAxis("Mouse ScrollWheel") != 0){

					if(Vector3.Distance(transform.position, target.position) > minZoomDist && Input.GetAxis("Mouse ScrollWheel") > 0){
						transform.Translate( transform.forward*Time.deltaTime*zoomSensitivity*Input.GetAxis("Mouse ScrollWheel") );
					}
					if(Vector3.Distance(transform.position, target.position) < maxZoomDist && Input.GetAxis("Mouse ScrollWheel") < 0){
						transform.Translate( transform.forward*Time.deltaTime*zoomSensitivity*Input.GetAxis("Mouse ScrollWheel") );
					}
				}
			}

			//correcters
			if(restrictFloor && transform.position.y < target.position.y-0.1){
				transform.RotateAround(target.position, transform.right, Time.deltaTime*100);
			}
			transform.LookAt(target.position);
			desf = transform.position-target.position;
		}


	}
}
