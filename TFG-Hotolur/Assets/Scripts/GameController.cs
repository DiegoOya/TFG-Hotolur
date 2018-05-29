using UnityEngine;

/// <summary>
/// Script used to manage the game
/// </summary>
public class GameController : MonoBehaviour {

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        // Ignore the collisions between enemy weapons and the ground
        Physics.IgnoreLayerCollision(8, 9);
    }

    //void AudioManagement()
    //{

    //}

}
