using UnityEngine;

/// <summary>
/// Script used to manage the game
/// </summary>
public class GameController : MonoBehaviour {
    
	void Start ()
    {
        // Ignore the collisions between enemy weapons and the ground
        Physics.IgnoreLayerCollision(8, 9);
    }
}
