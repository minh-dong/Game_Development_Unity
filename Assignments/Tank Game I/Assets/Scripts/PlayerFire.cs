using UnityEngine;
using System.Collections;

public class PlayerFire : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform bulletEmitter;
	public float force = 100f;
	RaycastHit target;

    /**
    * <summary>
    * Update once per frame
    * </summary>
    */
    void Update ()
	{
		FireCannon ();
	}

    /**
    * <summary>
    * When the player presses the spacebar, fire the cannon.
    * </summary>
    * \par Pseudo Code
    * \par
    * Did I press the spacebar? If so, create the bullet, add some velocity, and fire!
    * 
    * <returns>Returns gameobject</returns>
    */
    void FireCannon()
	{
		if (Input.GetKeyDown("space")) {
			GameObject BulletInstance = Instantiate(bulletPrefab, bulletEmitter.position, bulletEmitter.rotation) as GameObject;
			Rigidbody launch = BulletInstance.GetComponent<Rigidbody>();
			launch.velocity = force * bulletEmitter.forward;
			Destroy (BulletInstance, 5f);
		}
	}
}