using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the overall game logic
/// </summary>
public class MainController : MonoBehaviour
{
    [Header("PLAYER STATS:")]
    [Tooltip("The player's health")]
    public int PlayerHealth;

    private const int PLAYER_STARTING_HEALTH = 3;

    private void Start()
    {
        PlayerHealth = PLAYER_STARTING_HEALTH;
    }

    // --- Singleton management ---

    private static MainController instance;

    // This is the public reference to the instance
    public static MainController Instance
    {
        get
        {
            // If the instance is null, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<MainController>();

                // If it's still null, create a new GameObject and add the script
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(MainController).Name);
                    instance = singletonObject.AddComponent<MainController>();
                }
            }
            return instance;
        }
    }

    // Makes sure to prevent duplicates of the Singleton by destroying them
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
