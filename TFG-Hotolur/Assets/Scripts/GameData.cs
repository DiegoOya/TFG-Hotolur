using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to identify the components needed to save the game
/// </summary>
[System.Serializable]
public class GameData {
    
    // Name of the checkpoint
    public int checkPoint;

    // Position of BigHead
    public float headPositionX;
    public float headPositionY;
    public float headPositionZ;

    // Number of lives left and the buildIndex
    public int lives;
    public int scene;

    // List of indexes of the weapons
    public List<int> weaponsIndex = new List<int>();

    public GameData(int lastCPIndex, Vector3 headPos, int numLives, int sceneIndex, List<int> listWeapons)
    {
        checkPoint = lastCPIndex;
        headPositionX = headPos.x;
        headPositionY = headPos.y;
        headPositionZ = headPos.z;
        lives = numLives;
        scene = sceneIndex;
        for(int i = 0; i < listWeapons.Count; i++)
            weaponsIndex.Add(listWeapons[i]);
    }

}