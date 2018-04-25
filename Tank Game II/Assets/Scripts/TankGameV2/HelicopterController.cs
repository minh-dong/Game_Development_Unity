using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour {

    // Transform Variables
    public Transform MainHelicopter; // the entire helicopter
    public GameObject mainRotor; // The rotor on top of the helicopter
    public GameObject tailRotor; // The rotor on the helicopter's tail

    // Helicopter Variables
    private float _RotorSpeed = 0.5f; // How fast the rotors can rotate
    private float _MinRotorSpeed = 0.5f; // The minimuim rotor speed
    private float _MaxRotorSpeed = 19.5f; // The maximum rotor speed
    private float _CurrentRotorSpeed = 0; // Determine the current throttle
    private float _ThrottleForce = 0.0f; // Determine the force to move up or down
    private float _Pitch = 0.5f; // rotate forward or rotate backward
    private float _Roll = 0.5f; // rotate left or rotate right
    private float _Spin = 0.25f; // rotate the entire helicopter left or right

    private Rigidbody rigidbody;

    AudioSource audioSource;
    bool AudioCheck = false;

    /**
        * <summary>
        * Initalize at the beginning of the game
        * </summary>
        */
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

    /**
        * <summary>
        * Update is called once per frame
        * </summary>
        */
    void Update () {
        HelicopterThrottle();

        // Turns on or off the audio based on the throttle
        if ((_CurrentRotorSpeed > 0) && (AudioCheck == false))
        {
            audioSource.mute = !audioSource.mute;
            AudioCheck = true;
        }
        else if ((_CurrentRotorSpeed == 0) && (AudioCheck == true))
        {
            audioSource.mute = !audioSource.mute;
            AudioCheck = false;
        }
    }// end Update function

    /**
        * <summary>
        * Fixed update per frame
        * </summary>
        */
    void FixedUpdate()
    {
        // to Cycle Logitudinal for W and S keys
        if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log("Moving Forward!");
            //HelicopterPitch(MainHelicopter);
            //_Pitch += _PitchRotationSpeed;
            rigidbody.AddRelativeForce(Vector3.forward * 1000000);
            //_Pitch += Input.GetAxis("Vertical") * Time.deltaTime;
            //_Pitch = Mathf.Clamp(_Pitch, 0, 90);
            rigidbody.transform.Rotate(_Pitch, 0, 0);
            //rigidbody.transform.localEulerAngles = new Vector3(_Pitch, _Spin, _Roll);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //Debug.Log("Moving Backward!");
            //HelicopterPitch(MainHelicopter);
            rigidbody.AddRelativeForce(Vector3.back * 1000000);
            rigidbody.transform.Rotate(-_Pitch, 0, 0);
        }

        // to cycle lateral for A and D keys
        if (Input.GetKey(KeyCode.A))
        {
            // code here
            rigidbody.AddRelativeForce(Vector3.left * 1000000);
            rigidbody.transform.Rotate(0, 0, _Roll);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // code here
            rigidbody.AddRelativeForce(Vector3.right * 1000000);
            rigidbody.transform.Rotate(0, 0, -_Roll);
        }

        // for collective up and down arrows
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Increase the force going up
            _ThrottleForce += 1000.0f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Decrease the force going down
            _ThrottleForce -= 1000.0f;
        }
        else
        {
            // If the keys are not being pressed anymore,
            // decrease the force back to zero slowly
            if (_ThrottleForce > 24000)
                _ThrottleForce -= 2500.0f;
            else if (_ThrottleForce < 0)
                _ThrottleForce += 2500.0f;
        }

        //Debug.Log(_ThrottleForce);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.transform.Rotate(0, -_Spin, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.transform.Rotate(0, _Spin, 0);
        }

        // Throttling for Z (decrease) and C (Increase) keys
        if (Input.GetKey(KeyCode.Z))
        {
            // Decrease Throttle Speed 
            if (_CurrentRotorSpeed >= _MinRotorSpeed)
            {
                _CurrentRotorSpeed -= _RotorSpeed;
                mainRotor.transform.Rotate(0, _CurrentRotorSpeed, 0);
                // Debug.Log(_CurrentRotorSpeed);
            }
            //else Debug.Log("Can no longer throttle! Minimum Rotor Speed: " + _CurrentRotorSpeed);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            // Increase Throttle Speed 
            if (_CurrentRotorSpeed <= _MaxRotorSpeed)
            {
                _CurrentRotorSpeed += _RotorSpeed;
                mainRotor.transform.Rotate(0, _CurrentRotorSpeed, 0);
                //Debug.Log(_CurrentRotorSpeed);
            }
            //else Debug.Log("Can no longer throttle! Maximum Rotor Speed: " + _CurrentRotorSpeed);
        }

        // These functionalites here will always keep updated based on the helicopter
        mainRotor.transform.Rotate(0, _CurrentRotorSpeed, 0);
        tailRotor.transform.Rotate(_CurrentRotorSpeed * 100, 0, 0);
    }

    /**
        * <summary>
        * Determine how much force to add to the throttle for up and down arrow keys
        * </summary>
        */
    void HelicopterThrottle()
    {
        rigidbody.AddRelativeForce(Vector3.up * _ThrottleForce * _CurrentRotorSpeed);
    }

    /**
        * <summary>
        * Determine the pitch (rotation forward/backward) where it will clamp up to a certain point
        * </summary>
        */
    void HelicopterPitch (Transform transform)
    {
        _Pitch += 2.0f * Time.deltaTime;
        _Pitch = Mathf.Clamp(_Pitch, -45, 45);
        //rigidbody.transform.Rotate(_Pitch, 0, 0);
        rigidbody.transform.localEulerAngles = new Vector3(_Pitch, 0, 0);
    }
}
