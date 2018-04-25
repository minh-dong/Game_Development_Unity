using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	// ------------ \\
	// Game Objects \\
	// ------------ \\
	GameObject Turret;
	GameObject Cannon;
	GameObject[] FrontWheels;
	GameObject[] BackWheels;
	GameObject[] FrontTurningWheels;
	public Transform SteeringLeft;
	public Transform SteeringRight;

	// --------------------------------- \\
	// Public Variables for use in Unity \\
	// --------------------------------- \\
	public float TankSpeed;	// how fast the tank will move
	public float RotationSpeed; // how fast the tank can rotate
	public float CannonSpeed; // how fast the cannon can rotate
	public float FrontWheelRotation; // how fast the front wheels rotate
	public float BackWheelRotation; // how fast the back wheels rotate
	public float horizontal;

    /**
        * <summary>
        * Initalize at the beginning of the game
        * </summary>
        */
    void Start (){
		Turret = GameObject.FindGameObjectWithTag ("TurretObject");
		Cannon = GameObject.FindGameObjectWithTag ("CannonObject");
		FrontWheels = GameObject.FindGameObjectsWithTag ("FrontWheelsObject");
		BackWheels = GameObject.FindGameObjectsWithTag ("BackWheelsObject");
		FrontTurningWheels = GameObject.FindGameObjectsWithTag ("FrontWheelsTurningObject");
	} // end start

    /**
        * <summary>
        * Update is called once per frame
        * </summary>
        */
    void Update () {
		// stuff that works
		HorizontalWheel ();
		WheelSteering ();
		FrontWheelMovement ();
		BackWheelMovement ();
		CannonRotation ();
		TurretRotation ();
		TankMovement ();
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	} // end void update


    // ----------------------------- \\
    //    Wheel MOVEMENT SECTION     \\
    // ----------------------------- \\

    /**
    * <summary>
    * The front wheels will go either rotate left or right by 45 degrees.
    * </summary>
    * \par Pseudo Code
    * \par
    * If the input key is D, rotate the front wheels by 45 degrees
    * \par
    * else if the input key is A, rotate by -45 degrees
    * 
    * <returns>Returns horizontal</returns>
    */
    void HorizontalWheel(){
		// fill this later
		if (Input.GetKey(KeyCode.D))
		{
			horizontal = Input.GetKey(KeyCode.D) ? 0.45f : 0;
		}
		else
			horizontal = Input.GetKey(KeyCode.A) ? -0.45f : 0;
	}

    /**
    * <summary>
    * The Front wheels should rotate forward/backward and should rotate with an angle of maxiumum of 45 to left and right
    * </summary>
    * 
    * <returns>Returns FrontWheel rotation/</returns>
    */
    void FrontWheelMovement(){

		// For moving forward or backward
		if (Input.GetKey (KeyCode.W)) {

			//Debug.Log ("W Key is pressed");

			var RotateForward = Time.deltaTime * FrontWheelRotation;

			FrontWheels[0].transform.Rotate(RotateForward, 0, 0);
			FrontWheels[1].transform.Rotate(RotateForward, 0, 0);
		} // end get key for W
		else if (Input.GetKey (KeyCode.S)) {

			//Debug.Log ("S Key is pressed");

			var RotateForward = Time.deltaTime * FrontWheelRotation;

			FrontWheels[0].transform.Rotate(-RotateForward, 0, 0);
			FrontWheels[1].transform.Rotate(-RotateForward, 0, 0);
		} // end get key for S
	} // end front wheel movement function

    /**
    * <summary>
    * The steering for left or right to determine how far it should also rotate.
    * </summary>
    * 
    * <returns>Returns Steering Left or Right rotation</returns>
    */
    void WheelSteering(){
		float rot = 100;
		float current_rot = rot * horizontal;

		Vector3 front_rotationAmount = transform.rotation.eulerAngles;
		front_rotationAmount.y += current_rot;

		SteeringLeft.rotation = Quaternion.Euler(front_rotationAmount);
		SteeringRight.rotation = Quaternion.Euler(front_rotationAmount);
	}

    /**
    * <summary>
    * The back wheels should rotate and also accelerate/decelerate the tank
    * </summary>
    * 
    * <returns>Returns Back Wheels Rotation</returns>
    */
    void BackWheelMovement(){
		if (Input.GetKey (KeyCode.W)) {

			//Debug.Log ("W Key is pressed");

			var RotateBackward = Time.deltaTime * BackWheelRotation;

			BackWheels[0].transform.Rotate(RotateBackward, 0, 0);
			BackWheels[1].transform.Rotate(RotateBackward, 0, 0);
		} // end get key for W
		else if (Input.GetKey (KeyCode.S)) {

			//Debug.Log ("S Key is pressed");

			var RotateBackward = Time.deltaTime * BackWheelRotation;

			BackWheels[0].transform.Rotate(-RotateBackward, 0, 0);
			BackWheels[1].transform.Rotate(-RotateBackward, 0, 0);
		} // end get key for S
	}


    // ----------------------------- \\
    //   CANNON MOVEMENT SECTION     \\
    // ----------------------------- \\

    /**
        * <summary>
        * How far the cannon should rotate up and to its rest position. Max for up position is vertical (90 degrees)
        * </summary>
        * 
        * <returns>Returns Cannon Rotation</returns>
        */
    void CannonRotation(){

		var tilt = Time.deltaTime * CannonSpeed;

		if (Input.GetKey (KeyCode.UpArrow)) {
			Debug.Log ("Up Arrow Key Pressed");

			float currentRotationZ = Cannon.transform.rotation.z * 100;

			//Debug.Log (currentRotationZ);

			if (currentRotationZ <= -70){

				//Debug.Log ("Input Up key is being called for first if");

				currentRotationZ = -70 / 100;
			} 
			else {
				//Debug.Log ("Input Up key is being called for second if");

				currentRotationZ /= 100;
				currentRotationZ -= tilt;
			}

			Cannon.transform.Rotate (0, 0, currentRotationZ);
		}
		else if (Input.GetKey (KeyCode.DownArrow)) {
			Debug.Log ("Down Arrow Key Pressed");

			float currentRotationZ = Cannon.transform.rotation.z * 100;

			//Debug.Log (currentRotationZ);

			if (currentRotationZ >= 0){

				//Debug.Log ("Input Down key is being called for first if");

				currentRotationZ = 0 / 100;
			} 
			else{
				//Debug.Log ("Input Down key is being called for second if");

				currentRotationZ /= 100;
				currentRotationZ -= tilt;
			}

			Cannon.transform.Rotate (0, 0, -currentRotationZ);
		}
	} // end cannon rotation function

    // ----------------------------- \\
    //   TURRET MOVEMENT SECTION     \\
    // ----------------------------- \\

    /**
    * <summary>
    * Will rotate the turret left or right when the user press left or right arrow keys
    * </summary>
    * 
    * <returns>Returns Turret Rotation</returns>
    */
    void TurretRotation(){

		if (Input.GetKey (KeyCode.LeftArrow)) {
			Debug.Log ("Left Arrow Key Pressed");

			var rotateLeft = Time.deltaTime * RotationSpeed;

			Turret.transform.Rotate (0, -rotateLeft, 0);
			//Cannon.transform.Rotate (0, -rotateLeft, 0);
		}
		else if (Input.GetKey (KeyCode.RightArrow)) {
			Debug.Log ("Right Arrow Key Pressed");

			var rotateRight = Time.deltaTime * RotationSpeed;

			Turret.transform.Rotate (0, rotateRight, 0);
			//Cannon.transform.Rotate (0, rotateRight, 0);
		}

	} // end turret rotation function

    // ----------------------------- \\
    //     TANK MOVEMENT SECTION     \\
    // ----------------------------- \\

    /**
    * <summary>
    *This is called in the update function. All funcitonality is done here for the tank movement like moving forward, backwards, left, or right.
    * </summary>
    * 
    * <returns>Returns translate or rotation</returns>
    */
    private void TankMovement(){
		// W or D keys
		if (Input.GetKey (KeyCode.W)) {

			Debug.Log ("W Key is pressed");

			var MoveForward = Time.deltaTime * TankSpeed;

			ForwardBackward (-MoveForward);

		} // end get key for W
		else if (Input.GetKey (KeyCode.S)) {

			Debug.Log ("S Key is pressed");

			var MoveBackward = Time.deltaTime * TankSpeed;

			ForwardBackward (MoveBackward);
		}

		// A or D keys
		if (Input.GetKey (KeyCode.A)) {

			Debug.Log ("A Key is pressed");

			var TurnLeft = Time.deltaTime * TankSpeed;

			LeftRight (-TurnLeft);
		}
		else if (Input.GetKey (KeyCode.D)) {

			Debug.Log ("D Key is pressed");

			var TurnRight = Time.deltaTime * TankSpeed;

			LeftRight (TurnRight);
		}
	}

    /**
    * <summary>
    * When the W or S key is pressed, the tank will move forward or backward respectively
    * </summary>
    * 
    * <returns>Returns translate</returns>
    */
    private void ForwardBackward (float movement){
		transform.Translate (movement, 0, 0);
	} // end ForwardBackward function

    /**
    * <summary>
    * When the A or D key is pressed, the tank will move left or right respectively
    * </summary>
    * 
    * <returns>Returns rotate</returns>
    */
    private void LeftRight (float movement){
		transform.Rotate (0, movement, 0);
	} // end LeftRight function
}
