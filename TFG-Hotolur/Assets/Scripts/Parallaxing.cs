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
    [SerializeField]
    private Transform[] backgrounds;

    private Transform cam;

    // The position of the camera in the previous frame
    private Vector3 previousCamPos;

    // Get the reference of the camera Transform
    private void Awake()
    {
        cam = Camera.main.transform;
    }

    //Initialize the variables
    private void Start()
    {
        previousCamPos = cam.position;

        // Assigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for(int i = 0; i < backgrounds.Length; i++)
            parallaxScales[i] = backgrounds[i].position.z * (-1);
    }

    private void Update()
    {
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

}
