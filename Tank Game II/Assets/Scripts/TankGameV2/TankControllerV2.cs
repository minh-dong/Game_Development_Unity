using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankControllerV2 : MonoBehaviour {
    // Speed of the tank
    float speed = 15; 

    // Ratio for how the wheels move (to keep them in sync)
    // The front wheels move a lot faster than the back wheels
    float backWheelRadius = 1.23f;
    float frontBackRatio = 1.5f;
    float moveSteerRatio = 0.01f;

    // To hold the rotational speed of the wheels
    float backWheelRotationSpeed;
    float frontWheelRotationSpeed;

    // The angle for steering and rotation speed
    float steerAngle;
    float steerRotationSpeed = 50.0f;

    // Rotational Speed for the turret
    float turretSpeed = 50.0f;

    // Rotational Speed and Min/Max Rotation for the cannon
    float cannonSpeed = 10.0f;
    float minimumCannonRotation = 0.0f;
    float maximumCannonRotation = 90.0f;

    // Cannon Stuff
    float cannonAngle;
    float cannonRotationSpeed = 20.0f;

    // Transform to add within Unity
    public Transform leftSteering;
    public Transform rightSteering;
    public Transform leftFrontWheel;
    public Transform rightFrontWheel;
    public Transform leftBackWheel;
    public Transform rightBackWheel;
    public Transform Cannon;
    public Transform Turret;

    // For use with Move function
    // Sole purpose is to set up the Artifical Intelligence
    public Transform Player;
    private bool InRange = false;
    [SerializeField] private float playerDistance;
    [SerializeField] private float rotationDamping;
    [SerializeField] private int MoveTimer = 0;
    [SerializeField] private float timer = 0;
    [SerializeField] private float MaximumLookAtDistance;
    [SerializeField] private float ShootPlayerDistance;
    [SerializeField] private float FireSpeed;
    [SerializeField] private float FireFrequency;
    [SerializeField] private float AI_Speed;
    private bool NPC = true;

    // Stuff for bullet fire
    public GameObject bulletPrefab;
    public Transform bulletEmitter;
    public float force = 100f;
    RaycastHit target;

    // For use with AI health
    public const int maxHealth = 10;
    public int currentHealth = maxHealth;

    /**
        * <summary>
        * Initalize at the beginning of the game
        * </summary>
        */
    void Start () {
        backWheelRotationSpeed = speed / backWheelRadius / 3.1415f * 180;
        frontWheelRotationSpeed = backWheelRotationSpeed * frontBackRatio;
    }

    /**
        * <summary>
        * the NPC will always be true. whenever the player feels like playing as the tank
        * he or she will be able to press F1 for tank control
        * </summary>
        */
    void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
            NPC = false;
        else if (Input.GetKeyDown(KeyCode.F2))
            NPC = true;

        if (NPC == false)
        {
            TankMovement();
            TurretRotation();
            //CannonRotation();
            CannonFire();
        }
        else
        {
            Move();
        }

        //Move();
    }
    /**
        * <summary>
        * If the tank is hit by the helicopter bullet, take damage.
        * </summary>
        */
    void OnCollisionEnter(Collision collision)
    {

        if (GameObject.FindGameObjectWithTag("helicopterBullet"))
        {
            Destroy(GameObject.FindGameObjectWithTag("helicopterBullet"));
            currentHealth -= 1;
        }

        if (currentHealth == 0)
            Destroy(gameObject);
    }

    /**
        * <summary>
        * Very simple AI that will move around and shoot at the player when the player
        * is within range.
        * </summary>
        */
    void Move(){
        // The Tank AI will move around like a regular AI would do
        // If the player is within range, only look at the player
        //  - Gather the positions of the player X Y and Z
        //  - Just look at the player regardless until out of range
        // If the player is within shooting range, fire at the player
        //  - If the player is at this range, a bullet will be fired at the player

        // Gather information about the player distance
        playerDistance = Vector3.Distance(Player.position, transform.position);

        if (playerDistance < MaximumLookAtDistance)
        {
            // Look at the player function
            //Debug.Log("Looking at player!");
            InRange = true;

            // Gather the positions of the helicopter for X, Y, and Z
            // Turret Rotation to Helicopter at X and Z
            // - Turret Rotation will use the X and Z
            //Quaternion rotation = Quaternion.LookRotation(Player.position - transform.position);
            //Turret.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);

            Turret.transform.LookAt(new Vector3(Player.position.x, 0, Player.position.z));


            // Cannon Rotation to Helicopter at Y
            // - Cannon Rotation will use the Y
            Cannon.transform.LookAt(new Vector3(Player.position.x, Player.position.y, Player.position.z));


            //float Y_Position = Player.transform.position.y;
            //Cannon.Rotate(-Y_Position, 0, 0);


        }
        else if (playerDistance > MaximumLookAtDistance)
        {
            //Debug.Log("Player isn't in range!");
            InRange = false;
        }

        if (playerDistance < ShootPlayerDistance)
        {
            timer += Time.deltaTime;

            if (timer > FireFrequency)
            {
                // Fire at the player function
                GameObject BulletInstance = Instantiate(bulletPrefab, bulletEmitter.position, bulletEmitter.rotation) as GameObject;
                Rigidbody launch = BulletInstance.GetComponent<Rigidbody>();
                launch.velocity = FireSpeed * bulletEmitter.forward;
                Destroy(BulletInstance, 4f);
                timer = 0;
            }
            //Debug.Log("Firing at the player!");
        }

        // Timer based Movement (If player is not within range)
        if (InRange == false)
            MoveTimer += 1;

        if((MoveTimer >= 95) && (MoveTimer <= 205) && (InRange == false))
        {
            //Debug.Log("MOVE FORWARD!");

            float verticalInput = AI_Speed;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * leftSteering.transform.localRotation, moveSteerRatio);
            transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);
            leftFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            leftBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
        }
        else if ((MoveTimer >= 306) && (MoveTimer <= 405) && (InRange == false))
        {
            //Debug.Log("MOVE BACKWARD!");

            float verticalInput = -AI_Speed;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * leftSteering.transform.localRotation, moveSteerRatio);
            transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);
            leftFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            leftBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
        }
        else if ((MoveTimer >= 506) && (MoveTimer <= 555) && (InRange == false))
        {
            //Debug.Log("MAKE A LEFT TURN!");

            Left_AI_SteeringTransform(leftSteering);
            Left_AI_SteeringTransform(rightSteering);
        }
        else if ((MoveTimer >= 566) && (MoveTimer <= 605) && (InRange == false))
        {
            //Debug.Log("MOVE FORWARD!");

            float verticalInput = AI_Speed;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * leftSteering.transform.localRotation, moveSteerRatio);
            transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);
            leftFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            leftBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
        }
        else if ((MoveTimer >= 606) && (MoveTimer <= 655) && (InRange == false))
        {
            //Debug.Log("MAKE A RIGHT TURN!");
            Right_AI_SteeringTransform(leftSteering);
            Right_AI_SteeringTransform(rightSteering);
        }
        else if((MoveTimer >= 656) && (MoveTimer <= 705) && (InRange == false))
        {
            //Debug.Log("MOVE FORWARD!");

            float verticalInput = AI_Speed;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * leftSteering.transform.localRotation, moveSteerRatio);
            transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);
            leftFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            leftBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
        }
        else if ((MoveTimer >= 806) && (MoveTimer <= 905) && (InRange == false))
        {
            //Debug.Log("MOVE BACKWARD! AGAIN");

            float verticalInput = -AI_Speed;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * leftSteering.transform.localRotation, moveSteerRatio);
            transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);
            leftFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            leftBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
        }
        else if ((MoveTimer >= 906) && (MoveTimer <= 1005) && (InRange == false))
        {
            //Debug.Log("MOVE FORWARD AGAIN!");

            float verticalInput = AI_Speed;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * leftSteering.transform.localRotation, moveSteerRatio);
            transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);
            leftFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            leftBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
        }
        else if ((MoveTimer >= 1006) && (InRange == false))
        {
            //Debug.Log("DO NOTHING!");
            MoveTimer = 0;
        }
    }

    /**
        * <summary>
        * The Tank Movement with all the key controls
        * </summary>
        */
    void TankMovement(){
        if (Input.GetKey(KeyCode.W))
        {
            float verticalInput = Input.GetAxis("Vertical");
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * leftSteering.transform.localRotation, moveSteerRatio);
            transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);
            leftFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            leftBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            float verticalInput = Input.GetAxis("Vertical");
            transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * Quaternion.Inverse(leftSteering.transform.localRotation), moveSteerRatio);
            transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);
            leftFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightFrontWheel.Rotate(verticalInput * frontWheelRotationSpeed * Time.deltaTime, 0, 0);
            leftBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
            rightBackWheel.Rotate(verticalInput * backWheelRotationSpeed * Time.deltaTime, 0, 0);
        }

        // Transform Functions
        //  - When the A or D keys are pressed, the wheels will rotate 
        //      as well as the tank will keep moving in that certain direction
        SteeringTransform(leftSteering);
        SteeringTransform(rightSteering);
        CannonTransform(Cannon);
    }

    /**
        * <summary>
        * The turret will rotate appropriately based on the user input
        * </summary>
        */
    void TurretRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            var rotateLeft = Time.deltaTime * turretSpeed;

            Turret.transform.Rotate(0, -rotateLeft, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            var rotateRight = Time.deltaTime * turretSpeed;

            Turret.transform.Rotate(0, rotateRight, 0);
        }
    }

    /**
        * <summary>
        * The cannon will rotate properly based on the user input
        * </summary>
        */
    void CannonTransform(Transform transform)
    {


        if (Input.GetKey(KeyCode.UpArrow))
        {
            cannonAngle += Input.GetAxis("Vertical") * cannonRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            cannonAngle += Input.GetAxis("Vertical") * cannonRotationSpeed * Time.deltaTime;
        }

        cannonAngle = Mathf.Clamp(cannonAngle, 0, 90);
        transform.localEulerAngles = new Vector3(-cannonAngle, 0, 0);
    }

    /**
        * <summary>
        * When the player presses the space bar, the cannon bullet will fire
        * </summary>
        */
    void CannonFire()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject BulletInstance = Instantiate(bulletPrefab, bulletEmitter.position, bulletEmitter.rotation) as GameObject;
            Rigidbody launch = BulletInstance.GetComponent<Rigidbody>();
            launch.velocity = force * bulletEmitter.forward;
            Destroy(BulletInstance, 5f);
        }
    }

    /**
        * <summary>
        * Transform to properly adjust the wheels
        * </summary>
        */
    void SteeringTransform(Transform transform)
    {
        
        if (Input.GetKey(KeyCode.A))
        {
            steerAngle += Input.GetAxis("Horizontal") * steerRotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            steerAngle += Input.GetAxis("Horizontal") * steerRotationSpeed * Time.deltaTime;
        }
        steerAngle = Mathf.Clamp(steerAngle, -45, 45);
        transform.localEulerAngles = new Vector3(0, steerAngle, 0);

        //Debug.Log("SteeringTransform function is being called!");
    }

    /**
        * <summary>
        * Transform to properly adjust the wheels (AI USAGE)
        * </summary>
        */
    void Left_AI_SteeringTransform(Transform transform)
    {
        steerAngle += (-10) * steerRotationSpeed * Time.deltaTime;
        steerAngle = Mathf.Clamp(steerAngle, -45, 45);
        transform.localEulerAngles = new Vector3(0, steerAngle, 0);
    }

    /**
        * <summary>
        * Transform to properly adjust the wheels (AI USAGE)
        * </summary>
        */
    void Right_AI_SteeringTransform(Transform transform)
    {
        steerAngle += (10) * steerRotationSpeed * Time.deltaTime;
        steerAngle = Mathf.Clamp(steerAngle, -45, 45);
        transform.localEulerAngles = new Vector3(0, steerAngle, 0);
    }
}
