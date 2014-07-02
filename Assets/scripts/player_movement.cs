using UnityEngine;
using System.Collections;

/// <summary>
/// Script for controlling, ina very general way, the player's movement and animation.
/// </summary>

[RequireComponent (typeof(Animator))]
public class player_movement : MonoBehaviour {

	public enum playerStates{
		NONE,  //Buggy state :P
		IDLE,
		RUNNING,   //trigger: run
		JUMPING,
		ATTACKING,
		FREEZED,
		DEAD
	}
	public playerStates state = playerStates.IDLE;
	public bool cameraDir = true;
	public float rotateToCameraSpeed = 10;
	public Camera camera;
	public bool useMainCamera = true;
	public bool forceTranslation = false;
	public float translationSpeed = 10;
	public bool forceRotation = false;
	public float rotationSpeed = 10;
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode backwardKey = KeyCode.S;
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	Vector3 localForward, localRight;
	float currentRot = 0;
	Animator myAnimator;


	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator>();
		localForward = transform.forward;
		localRight = transform.right;
	}
	
	// Update is called once per frame
	void Update () {

		//transform.Translate(transform.forward*Time.deltaTime*10);

		if(useMainCamera && camera != Camera.main ) camera = Camera.main;
		if(forceTranslation && myAnimator.applyRootMotion) myAnimator.applyRootMotion = false;
		else if(!forceTranslation && !myAnimator.applyRootMotion) myAnimator.applyRootMotion = true;

		if(state == playerStates.IDLE){
			if(Input.GetKeyDown(forwardKey) || Input.GetKeyDown(backwardKey) || Input.GetKeyDown(rightKey) || Input.GetKeyDown(leftKey)){
				myAnimator.SetTrigger("run");
				state = playerStates.RUNNING;
			}
		}

		if(state == playerStates.RUNNING){
			if(!Input.anyKey){
				myAnimator.SetTrigger("idle");
				state = playerStates.IDLE;
			}

			if(cameraDir){
				if(forceRotation){

					if(Input.GetKey(rightKey)){
						if(Input.GetKey(forwardKey)) currentRot = 45;
						else if(Input.GetKey(backwardKey)) currentRot = 135;
						else currentRot = 90;
					}
					else if(Input.GetKey(leftKey)){
						if(Input.GetKey(forwardKey)) currentRot = -45;
						else if(Input.GetKey(backwardKey)) currentRot = -135;
						else currentRot = -90;
					}
					else if(Input.GetKey(backwardKey)){
						currentRot = 180;
					}
					else if(Input.GetKey(forwardKey)){
						currentRot = 0;
					}
					Vector3 targetRot = new Vector3(transform.eulerAngles.x, camera.transform.eulerAngles.y+currentRot, transform.eulerAngles.z);
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), Time.deltaTime*rotationSpeed);
				}
				else{
					Vector3 targetRot = new Vector3(transform.eulerAngles.x, camera.transform.eulerAngles.y, transform.eulerAngles.z);
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), Time.deltaTime*rotateToCameraSpeed);
				}
			}
			else{
			}

		}

	}
}
