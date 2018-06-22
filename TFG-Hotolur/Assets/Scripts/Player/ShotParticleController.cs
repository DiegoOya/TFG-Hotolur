using UnityEngine;

/// <summary>
/// Script to control when the player hits the enemies 
/// </summary>
public class ShotParticleController : MonoBehaviour, IPooledObject {

    private ParticleSystem shotParticle;

    private void Awake()
    {
        shotParticle = GetComponent<ParticleSystem>();
    }

    // This method is called whenever this object spawns 
    public void OnObjectSpawn()
    {
        shotParticle.Play();
    }

}
