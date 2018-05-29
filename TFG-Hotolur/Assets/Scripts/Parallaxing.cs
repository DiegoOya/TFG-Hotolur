using UnityEngine;

/// <summary>
/// Script to manage the parallax scrolling of the background and the foreground
/// </summary>
public class Parallaxing : MonoBehaviour {
    
    // The proportion of the camera's movement to move the backgrounds by
    private float[] parallaxScales;

    [SerializeField]
    private float smoothing = 1f;

    // Array of all the back and foregrounds to be parallaxed
    private Transform[] backgrounds;

    private Transform cam;

    // The position of the camera in the previous frame
    private Vector3 previousCamPos;

    // Assign the components needed
    private void Awake()
    {
        AssignComponents();
    }

    private void Update()
    {
        if (cam == null)
            AssignComponents();

        // For each background calculate its parallax movement and apply it
        for(int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Set the camera position in the actual frame to previousCamPos
        previousCamPos = cam.position;
    }

    private void AssignComponents()
    {
        cam = Camera.main.transform;

        previousCamPos = cam.position;

        // Search for all the backgrounds
        GameObject[] backgroundsGO = GameObject.FindGameObjectsWithTag(Tags.background);
        backgrounds = new Transform[backgroundsGO.Length];
        for (int i = 0; i < backgroundsGO.Length; i++)
        {
            backgrounds[i] = backgroundsGO[i].transform;
        }

        // Assigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
            parallaxScales[i] = backgrounds[i].position.z * (-1);
    }

}
