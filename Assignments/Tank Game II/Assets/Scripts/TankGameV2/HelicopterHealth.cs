using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HelicopterHealth : MonoBehaviour
{
    public const int maxHealth = 20;
    public int currentHealth = maxHealth;
    public RectTransform healthbar;


    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image Content;

    /**
    * <summary>
    * Update once per frame to update the HandleBar
    * </summary>
    */
    void Update()
    {
        HandleBar();

        // Test code
        // Take damage when pressing 9
        //if (Input.GetKeyDown("9"))
        //{
        //    TakeDamage(1);
        //}
    }

    /**
    * <summary>
    * Will get the fill amount for the health bar to determine its mapping and will update accordingly
    * </summary>
    * 
    * <returns>Healthbar's fill amount</returns>
    */
    private void HandleBar()
    {
        Content.fillAmount = Map(currentHealth, 0, maxHealth, 0, 1);
    }

    /**
    * <summary>
    * Calculations to determine the 0 to 1 Unity asset and will calculate if it is greater than 1. It will scale it down between 0 to 1.
    * </summary>
    * 
    * <returns>Returns the Map</returns>
    */
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    /**
    * <summary>
    * Whenever the player takes damage, the health will go down.
    * </summary>
    * \par Pseudo Code
    * \par
    * Did I get hit, subtract one health. If I lose all my health, reload the scene
    * 
    * <returns>Returns player's health</returns>
    */
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
            //Destroy(GameObject.FindGameObjectWithTag("tankObject"));
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //healthbar.sizeDelta = new Vector2 (currentHealth, healthbar.sizeDelta.y);
    }// end take damage function


}