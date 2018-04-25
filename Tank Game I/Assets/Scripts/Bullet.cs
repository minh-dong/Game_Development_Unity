using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    /**
        * <summary>
        * When there is a collision detection from the enemy bullet to
        * the player's tank. If it collides with each other, the player
        * will take damage
        * </summary>
        * \par Pseudo Code
        * \par
        * If there is a detected collision between two objects
        * \par
        * Get the health component for the tank
        * \par
        * If it is not equal to null, player takes 1 damage
        * \par
        * The enemy bullet object will be destroyed after
        * 
        * <param name="hit">Determine whether there was collision to get gameObject</param>
        * <param name="health">Get Health Component</param>
        * 
        * <returns>Returns if collision detected to update player's health and destroy enemy bullet</returns>
        */
    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(1);
        }

		if(GameObject.FindGameObjectWithTag("tankObject"))
       		Destroy(gameObject);
    }
}