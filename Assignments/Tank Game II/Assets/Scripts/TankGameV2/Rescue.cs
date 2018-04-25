using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rescue : MonoBehaviour {

    public AudioClip win;
    private AudioSource source;

    /**
        * <summary>
        * Initalize the audio source
        * </summary>
        */
    void Start () {
        source = GetComponent<AudioSource>();
	}

    /**
        * <summary>
        * If the player collides with the hitbox, the player wins the game and the
        * level will reset.
        * </summary>
        */
    void OnCollisionEnter(Collision collision)
    {
        if (GameObject.FindGameObjectWithTag("helicopterPlayer"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            float vol = Random.Range(0.25f, 0.40f);
            source.PlayOneShot(win, vol);

            

        }

    }
}
