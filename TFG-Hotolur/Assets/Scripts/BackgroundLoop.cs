using UnityEngine;

/// <summary>
/// Script used to control the loop of the backgrounds so there can be any number needed
/// </summary>
public class BackgroundLoop : MonoBehaviour {

    // Transform the camera
    private Transform cam;

    // SpriteRenderer of the backgrounds
    private SpriteRenderer[] backgrounds;

    // The width of the camera
    private float sizeCamX;

    private CameraController cameraController;

    private void Awake()
    {
        cam = Camera.main.transform;
        sizeCamX = Camera.main.orthographicSize * 16 / 9;
        cameraController = cam.GetComponent<CameraController>();

        Plane[] planes = GetComponentsInChildren<Plane>();
        backgrounds = new SpriteRenderer[planes.Length];
        for(int i = 0; i < planes.Length; i++)
        {
            backgrounds[i] = planes[i].GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        // Position of each side of the camera ([0]: "To the right"; [1]: "To the left") 
        float[] posCameraSides = new float[2];
        posCameraSides[0] = cam.position.x + sizeCamX;
        posCameraSides[1] = cam.position.x - sizeCamX;

        float camVelX = cameraController.cameraVelX;

        // The camera is moving sideways, so when it points to an area where it is close
        // to be the end of the background in the camera plane
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if(backgrounds[i].isVisible)
            {
                // Position of each side of the background ([0]: "To the right"; [1]: "To the left") 
                float[] posBGSides = new float[2];
                posBGSides[0] = backgrounds[i].bounds.center.x + backgrounds[i].bounds.extents.x;
                posBGSides[1] = backgrounds[i].bounds.center.x - backgrounds[i].bounds.extents.x;

                // If the position of the right side of the camera is near to the 
                // right end of the background then move the another background
                if (posCameraSides[0] > posBGSides[0] - 5 && posCameraSides[0] < posBGSides[0] && camVelX > 0)
                {
                    // Calculate the new position of the background
                    // ***The argument ([1 - i]) of backgrounds is only valid for 2 backgrounds
                    float newPosX = backgrounds[i].bounds.center.x + 2 * backgrounds[1 - i].bounds.extents.x;

                    backgrounds[1 - i].transform.position = new Vector3(newPosX, 
                                                                        backgrounds[1 - i].transform.position.y, 
                                                                        backgrounds[1 - i].transform.position.z);
                }

                // If the position of the left side of the camera is near to the 
                // left end of the background then move the another background
                if (posCameraSides[1] > posBGSides[1] && posCameraSides[1] < posBGSides[1] + 5 && camVelX < 0)
                {
                    // Calculate the new position of the background
                    // ***The argument ([1 - i]) of backgrounds is only valid for 2 backgrounds
                    float newPosX = backgrounds[i].bounds.center.x - 2 * backgrounds[1 - i].bounds.extents.x;

                    backgrounds[1 - i].transform.position = new Vector3(newPosX,
                                                                        backgrounds[1 - i].transform.position.y,
                                                                        backgrounds[1 - i].transform.position.z);
                }
            }
        }
    }

}
