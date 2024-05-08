using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores a reference to important positions
/// </summary>
public class GlobalReferences : MonoBehaviour
{
    [Header("SCRIPTABLE OBJECTS:")]
    [Tooltip("The flyweight prefabs for the shapes and elements of the spells")]
    public SpellFlyweightsSO SpellFlyweightsSO;

    [Header("PLAYER:")]
    [Tooltip("The player : XR Origin (XR Rig)")]
    public Transform Player;

    [Header("WAND:")]
    [Tooltip("The center of the wand")]
    public Transform Wand;

    [Tooltip("The point at which the magic circle is instantiated : a meter or so in front of the wand")]
    public Transform MagicCircleSpawningPoint;

    [Tooltip("The point at which spell objects are instantiated : the tip of the wand")]
    public Transform SpellSpawningPoint;


    // --- Singleton management ---

    private static GlobalReferences instance;

    // This is the public reference to the instance
    public static GlobalReferences Instance
    {
        get
        {
            // If the instance is null, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<GlobalReferences>();

                // If it's still null, create a new GameObject and add the script
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GlobalReferences).Name);
                    instance = singletonObject.AddComponent<GlobalReferences>();
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
