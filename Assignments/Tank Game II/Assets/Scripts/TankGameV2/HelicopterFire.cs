using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterFire : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletEmiiterLeft;
    public Transform bulletEmitterRight;
    public float force = 100f;

    public float FireFrequency;
    private float timerLeft = 0;
    private float timerRight = 0;

	// Use this for initialization
	void Start () {
		
	}

    /**
        * <summary>
        * get the two teimers to determine when the player can shoot again. if left
        * mouse button is clicked, left guin will shoot. The right mouse button will shoot
        * right gun. Spacebar will shoot both guns.
        * </summary>
        */
    void Update () {
        timerLeft += Time.deltaTime;
        timerRight += Time.deltaTime;

        // Fire the weapons left (left weapon), right (right weapon) spacebar (both)
        if ((Input.GetKey(KeyCode.Mouse0)) && (timerLeft > FireFrequency))
        {
            FireLeft();
            timerLeft = 0;
        }
        if ((Input.GetKey(KeyCode.Mouse1)) && (timerRight > FireFrequency))
        {
            FireRight();
            timerRight = 0;
        }
        if ((Input.GetKey(KeyCode.Space)) && (timerLeft > FireFrequency) && (timerRight > FireFrequency))
        {
            FireLeft();
            FireRight();
            timerLeft = 0;
            timerRight = 0;
        }
    }

    /**
        * <summary>
        * Fire the left gun
        * </summary>
        */
    void FireLeft()
    {
        GameObject BulletInstance = Instantiate(bulletPrefab, bulletEmiiterLeft.position, bulletEmiiterLeft.rotation) as GameObject;
        Rigidbody launch = BulletInstance.GetComponent<Rigidbody>();
        launch.velocity = force * bulletEmiiterLeft.forward;
        Destroy(BulletInstance, 5f);
    }

    /**
        * <summary>
        * Fire the right gun
        * </summary>
        */
    void FireRight()
    {
        GameObject BulletInstance = Instantiate(bulletPrefab, bulletEmitterRight.position, bulletEmitterRight.rotation) as GameObject;
        Rigidbody launch = BulletInstance.GetComponent<Rigidbody>();
        launch.velocity = force * bulletEmitterRight.forward;
        Destroy(BulletInstance, 5f);
    }
}
