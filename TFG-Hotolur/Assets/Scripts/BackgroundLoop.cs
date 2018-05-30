using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to control the loop of the backgrounds so there can be any number needed
/// </summary>
public class BackgroundLoop : MonoBehaviour {

    // Transform the camera
    private Transform cam;

    // SpriteRenderer of the backgrounds
    private List<SpriteRenderer[]> backgrounds = new List<SpriteRenderer[]>();

    // The width of the camera
    private float sizeCamX;

    private CameraController cameraController;

    private void Awake()
    {
        AssignComponents();
    }

    private void Update()
    {
        if (cam == null)
            AssignComponents();

        // Position of each side of the camera ([0]: "To the right"; [1]: "To the left") 
        float[] posCameraSides = new float[2];
        posCameraSides[0] = cam.position.x + sizeCamX;
        posCameraSides[1] = cam.position.x - sizeCamX;

        float camVelX = cameraController.cameraVelX;

        for (int i = 0; i < backgrounds.Count; i++)
        {
            SpriteRenderer[] background = backgrounds[i];

            // The camera is moving sideways, so when it points to an area where it is close
            // to be the end of the background in the camera plane, then move the other background
            for (int j = 0; j < background.Length; j++)
            {
                if (background[j].isVisible)
                {
                    // Position of each side of the background ([0]: "To the right"; [1]: "To the left") 
                    float[] posBGSides = new float[2];
                    posBGSides[0] = background[j].bounds.center.x + background[j].bounds.extents.x;
                    posBGSides[1] = background[j].bounds.center.x - background[j].bounds.extents.x;

                    // If the position of the right side of the camera is near to the 
                    // right end of the background then move the another background
                    if (posCameraSides[0] > posBGSides[0] - 5 && posCameraSides[0] < posBGSides[0] && camVelX > 0)
                    {
                        // Calculate the new position of the background
                        // ***The argument ([1 - j]) of background is only valid for 2 background
                        float newPosX = background[j].bounds.center.x + 2 * background[1 - j].bounds.extents.x;

                        background[1 - j].transform.position = new Vector3(newPosX,
                                                                            background[1 - j].transform.position.y,
                                                                            background[1 - j].transform.position.z);
                    }

                    // If the position of the left side of the camera is near to the 
                    // left end of the background then move the another background
                    if (posCameraSides[1] > posBGSides[1] && posCameraSides[1] < posBGSides[1] + 5 && camVelX < 0)
                    {
                        // Calculate the new position of the background
                        // ***The argument ([1 - j]) of background is only valid for 2 background
                        float newPosX = background[j].bounds.center.x - 2 * background[1 - j].bounds.extents.x;

                        background[1 - j].transform.position = new Vector3(newPosX,
                                                                            background[1 - j].transform.position.y,
                                                                            background[1 - j].transform.position.z);
                    }
                }
            }
        }
    }

    // Assign all the references 
    void AssignComponents()
    {
        cam = Camera.main.transform;
        sizeCamX = Camera.main.orthographicSize * 16 / 9;
        cameraController = cam.GetComponent<CameraController>();

        // Search for all the backgrounds in the scene and get the SpriteRenderers of each background
        GameObject[] backgroundsGO = GameObject.FindGameObjectsWithTag(Tags.background);

        backgrounds.Clear();
        for (int i = 0; i < backgroundsGO.Length; i++)
        {
            Plane[] planes = backgroundsGO[i].GetComponentsInChildren<Plane>();
            SpriteRenderer[] spriteRenderersBG = new SpriteRenderer[planes.Length];
            for (int j = 0; j < planes.Length; j++)
            {
                spriteRenderersBG[j] = planes[j].GetComponent<SpriteRenderer>();
            }
            backgrounds.Add(spriteRenderersBG);
        }
    }

}
