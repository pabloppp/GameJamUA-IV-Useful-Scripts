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
		FALLING,
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
	public float translationAccel = 1;
	public float translationMaxSpeed = 10;
	float translationSpeed = 0;
	public bool forceRotation = false;
	public float rotationSpeed = 10;
	Vector3 rotationVector;
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode backwardKey = KeyCode.S;
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;

	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode attackKey = KeyCode.Mouse0;

	public bool lookAtTarget = true;
	public Transform lookTarget;

	public bool detectFloor = true;
	public bool anyFloor = true;
	public string[] floorTags;

	public bool antigravityJumps = true;
	public float jumpForce = 100;
	public float jumptime = 2;
	float currentJumpTime = 0;
	bool jumped = false;

	Vector3 localForward, localRight, localUp;
	float currentRot = 0;
	Animator myAnimator;
	


	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator>();
		localForward = transform.forward;
		localRight = transform.right;
		localUp = transform.up;
		rotationVector = Vector3.forward;
	}
	
	// Update is called once per frame
	void Update () {


		//transform.Translate(transform.forward*Time.deltaTime*10);

		if(useMainCamera && camera != Camera.main ) camera = Camera.main;
		if(forceTranslation && myAnimator.applyRootMotion) myAnimator.applyRootMotion = false;
		else if(!forceTranslation && !myAnimator.applyRootMotion) myAnimator.applyRootMotion = true;

		if(state == playerStates.IDLE){

			if(movementKeyDown()){
				myAnimator.SetBool("idle", false);
				myAnimator.SetTrigger("run");
				state = playerStates.RUNNING;
			}

			if(Input.GetKeyDown(jumpKey)){
				state = playerStates.JUMPING;
				myAnimator.SetTrigger("jump");
				currentJumpTime = Time.time;
			}

		}

		if(state == playerStates.FALLING){
			if(myAnimator.GetBool("floor")){
				state = playerStates.IDLE;
				myAnimator.SetTrigger("idle");
				if(antigravityJumps){
					Rigidbody rb = GetComponent<Rigidbody>();
					if(rb != null) rb.useGravity = true;
				}
			}
		}

		if(state == playerStates.JUMPING){

			if(!antigravityJumps && GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().AddForce(transform.up*jumpForce);

			if((Time.time-currentJumpTime) >= jumptime){
				state = playerStates.FALLING;
				myAnimator.SetTrigger("fall");
			}
		}

		if(state == playerStates.RUNNING){


			if(Input.GetKeyDown(jumpKey)){
				state = playerStates.JUMPING;
				myAnimator.SetTrigger("jump");
				currentJumpTime = Time.time;
			}

			//MECANIM CALLS
			if(Input.GetKeyDown(rightKey)){
				myAnimator.SetTrigger("right");
			}
			if(Input.GetKeyDown(leftKey)){
				myAnimator.SetTrigger("left");
			}
			if(Input.GetKeyDown(backwardKey)){
				myAnimator.SetTrigger("backward");
			}
			if(Input.GetKeyDown(forwardKey)){
				myAnimator.SetTrigger("forward");
			}

			if(Input.GetKey(rightKey)) myAnimator.SetBool("right", true);
			else myAnimator.SetBool("right", false);

			if(Input.GetKey(leftKey)) myAnimator.SetBool("left", true);
			else myAnimator.SetBool("left", false);

			if(Input.GetKey(backwardKey)) myAnimator.SetBool("backward", true);
			else myAnimator.SetBool("backward", false);

			if(Input.GetKey(forwardKey)) myAnimator.SetBool("forward", true);
			else myAnimator.SetBool("forward", false);
			//---

			if(!movementKey()){
				if(!forceTranslation || translationSpeed == 0){
					myAnimator.SetTrigger("idle");
					state = playerStates.IDLE;
				}
			}

			if(forceTranslation){
				if(forceRotation){

					if(movementKey()){
						if(translationSpeed < translationMaxSpeed) translationSpeed+=Time.deltaTime*translationAccel;
						if(translationSpeed >= translationMaxSpeed || translationAccel == 0) translationSpeed = translationMaxSpeed;
					}
					else{
						if(translationSpeed > 0) translationSpeed-=Time.deltaTime*translationAccel;
						if(translationSpeed <= 0 || translationAccel == 0) translationSpeed = 0;
					}

					transform.Translate(Vector3.forward*translationSpeed*Time.deltaTime);
				}
				else{

					if(movementKey()){
						translationSpeed = translationMaxSpeed;
					}
					else{
						translationSpeed = 0;
					}

					if(Input.GetKey(rightKey)){
						if(Input.GetKey(forwardKey)){
							rotationVector = (Vector3.right+Vector3.forward).normalized;
						}
						else if(Input.GetKey(backwardKey)){
							rotationVector = (Vector3.right-Vector3.forward).normalized;
						}
						else rotationVector = Vector3.right;
					}
					else if(Input.GetKey(leftKey)){
						if(Input.GetKey(forwardKey)){
							rotationVector = (-Vector3.right+Vector3.forward).normalized;
						}
						else if(Input.GetKey(backwardKey)){
							rotationVector = (-Vector3.right-Vector3.forward).normalized;
						}
						else rotationVector = -Vector3.right;
					}
					else if(Input.GetKey(forwardKey)){
						rotationVector = Vector3.forward;
					}
					else if(Input.GetKey(backwardKey)){
						rotationVector = -Vector3.forward;
					}
					transform.Translate(rotationVector*translationSpeed*Time.deltaTime);
				}
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
					Vector3 targetRot = new Vector3(transform.eulerAngles.x, localUp.y+currentRot, transform.eulerAngles.z);
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), Time.deltaTime*rotationSpeed);
				}
			}

		}

		myAnimator.SetBool("floor", false);

	}

	void OnCollisionEnter (Collision col)
	{
		myAnimator.SetTrigger("collide");
		foreach(ContactPoint contact in col.contacts){
			if(Vector3.Angle(contact.normal, transform.forward) > 135){
				myAnimator.SetTrigger("collide_front");
			}
		}
	}

	void OnCollisionStay(Collision col){
		myAnimator.SetBool("collide", true);

		foreach(ContactPoint contact in col.contacts){
			if(detectFloor){
				if(anyFloor || hasFloorTags(col.transform)){
					if(Vector3.Angle(contact.normal, transform.up) < 5){
						Debug.Log("On Floor");
						myAnimator.SetBool("floor", true);
					}
					//else myAnimator.SetBool("floor", false);
				}
			}
		}
	}

	void OnCollisionExit(Collision col){
		myAnimator.SetBool("collide", false);
		if(state == playerStates.JUMPING && antigravityJumps){
			Rigidbody rb = GetComponent<Rigidbody>();
			if(rb != null) rb.useGravity = false;
			jumped = true; 
		}
	}
	
	bool hasFloorTags(Transform tr){

		foreach(string s in floorTags) if(tr.CompareTag(s)) return true;
		return false;
	}

	void OnAnimatorIK(int layerIndex){
		if(lookTarget != null){
			if(lookAtTarget){

				myAnimator.SetLookAtPosition(lookTarget.position);
				myAnimator.SetLookAtWeight(1);
			}
			else myAnimator.SetLookAtWeight(0);
		}
	}

	public bool movementKey(){
		if(Input.GetKey(forwardKey) || Input.GetKey(backwardKey) || Input.GetKey(rightKey) || Input.GetKey(leftKey)) return true;
		else return false;
	}

	public bool movementKeyDown(){
		if(Input.GetKeyDown(forwardKey) || Input.GetKeyDown(backwardKey) || Input.GetKeyDown(rightKey) || Input.GetKeyDown(leftKey)) return true;
		else return false;
	}
}
