using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	public Transform Player; // prefab for the player
	public float playerDistance; // will be measured later to check for distance
	public float rotationDamping; // how dampened the rotation should be

	public GameObject bulletPrefab;
	public Transform bulletEmitter;
	public float FireSpeed; // how fast the bullet should travel?
	public float FireFrequency; // how often they will fire?
	RaycastHit target;

	private float timer = 0;

	public const int maxHealth = 3;
	public int currentHealth = maxHealth;

    /**
    * <summary>
    * Update every per frame. Only function in here is the DistanceToLookAt.
    * </summary>
    * \par Pseudo Code
    * \par
    * Keeps updating the DistanceToLookAt function.
    * 
    * <returns>Distance Function</returns>
    */
    void Update () {
		DistanceToLookAt ();
	}

    /**
    * <summary>
    * When there is a collision detected by the player bullet, the enemy's life will go down. If down to 3, the enemy is dead.
    * </summary>
    * \par Pseudo Code
    * \par
    * Did I get hit by the player's bullet?
    * \par
    * If so, I lose a hit point
    * \par 
    * Did I lose all 3 lives?
    * \par
    * If so, destroy object
    * 
    * 
    * <param name="currentHealth">Enemy Health Variable</param>
    * 
    * <returns>Return if there is something collided with enemy and player's bullet</returns>
    */
    void OnCollisionEnter (Collision collision){

		if (GameObject.FindGameObjectWithTag ("helicopterBullet")) {
			Destroy (GameObject.FindGameObjectWithTag("helicopterBullet"));
			currentHealth -= 1;
		}

		if (currentHealth == 0)
			Destroy(gameObject);
	}

    /**
    * <summary>
    * Determine a function of how far the enemy looks at the player (distance), and when to shoot
    * at the player.
    * </summary>
    * \par Pseudo Code
    * \par
    * If the player is within looking distance, I will look at him wherever the player is.
    * \par
    * If the player steps into my danger zone, I am allowed to fire
    * 
    * <param name="timer">Incremental for time.deltatime</param>
    * <param name="playerDistance">Vector3.Distance</param>
    * 
    * <returns>Returns if I'm within the distance to look at the player</returns>
    */
    void DistanceToLookAt(){
		playerDistance = Vector3.Distance (Player.position, transform.position);

		timer += Time.deltaTime;

		if (playerDistance < 150f) {
			lookAtPlayer ();
			Debug.Log ("Looking at player!");
		}

		if (playerDistance < 75f) {
			if (timer > FireFrequency) {
				FireAtPlayer ();
				timer = 0;
			}
			Debug.Log ("Firing at the player!");
		}
	}

    /**
    * <summary>
    * Extensive math problem here ti determine the player position and enemy position.
    * </summary>
    * \par Pseudo Code
    * \par
    * I will rotate using Quaternion to look at the player position
    * 
    * <returns>Rotation</returns>
    */
    void lookAtPlayer(){
		Quaternion rotation = Quaternion.LookRotation (Player.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
	}

    /**
    * <summary>
    * Fire at the player
    * </summary>
    * \par Pseudo Code
    * \par
    * An object will be created with some velocity and will be destroyed at a certain amount of time.
    * <param name="BulletInstance">Create the bullet instance using the prefab stuff</param>
    * <param name="launch">Get the bullet instance/param>
    * <param name="velocity">Part of launch to determine the bullet speed</param>
    * <returns>Fire at the player</returns>
    */
    void FireAtPlayer(){
		GameObject BulletInstance = Instantiate(bulletPrefab, bulletEmitter.position, bulletEmitter.rotation) as GameObject;
		Rigidbody launch = BulletInstance.GetComponent<Rigidbody>();
		launch.velocity = FireSpeed * bulletEmitter.forward;
		Destroy (BulletInstance, 6f);
	}
}
