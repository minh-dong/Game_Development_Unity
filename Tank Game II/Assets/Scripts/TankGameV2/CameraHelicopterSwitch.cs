using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelicopterSwitch : MonoBehaviour {

    public Camera firstPCamera;
	public Camera thirdPCamera;
	public Camera overheadCamera;

    /**
    * <summary>
    * This is to intialize upon start of the game
    * </summary>
    * \par Pseudo Code
    * \par
    * Get third person camera and set it to true
    * \par
    * Get overhead camera and set it to false
    */
    void Start () {

        firstPCamera.GetComponent<Camera>().enabled = true;
		thirdPCamera.GetComponent<Camera>().enabled = false;
		overheadCamera.GetComponent<Camera>().enabled = false;
	}

    /**
    * <summary>
    * This will update every fame to check whether the user switch cams or not.
    * </summary>
    * \par Pseudo Code
    * \par
    * Did the user press F?
    * \par
    * If yes, check the switchCam statement, then reverse its statement, then another statement to make more statemenets to change the cameras
    * \par
    * If no, do nothing.
    * 
    * <param name="firstPCamera">First Person Camera</param>
    * <param name="thirdPCamera">Third Pereson Camera</param>
    * <param name="overheadCamera">Overhead Camera</param>
    * 
    * <returns>Swaps the statement to true or false for overhead and third person camera</returns>
    */
    void Update () {

		// check if the switch camera happened
		if (Input.GetKeyDown("1"))
        {
            firstPCamera.GetComponent<Camera>().enabled = true;
			thirdPCamera.GetComponent<Camera> ().enabled = false;
			overheadCamera.GetComponent<Camera> ().enabled = false;
		} 
		else if (Input.GetKeyDown("2"))
        {
            firstPCamera.GetComponent<Camera>().enabled = false;
            thirdPCamera.GetComponent<Camera> ().enabled = true;
			overheadCamera.GetComponent<Camera> ().enabled = false;
		}
        else if (Input.GetKeyDown("3"))
        {
            firstPCamera.GetComponent<Camera>().enabled = false;
            thirdPCamera.GetComponent<Camera>().enabled = false;
            overheadCamera.GetComponent<Camera>().enabled = true;
        }
	}
}
