using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to identify the components needed to save the game
/// </summary>
[System.Serializable]
public class GameData {
    
    // Name of the checkpoint
    public int checkPoint;

    // Number of lives left and the buildIndex
    public int lives;
    public int scene;

    // Position of BigHead
    public float headPositionX;
    public float headPositionY;
    public float headPositionZ;

    // Number of points
    public int points;

    // If the game was beated in the last game
    public bool gameBeated;
        
    // List of indexes of the weapons
    public List<int> weaponsIndex = new List<int>();

    // List of the ranking points with their names
    public List<float> rankingPoints = new List<float>();
    public List<string> rankingPlayerNames = new List<string>();

    public GameData(int lastCPIndex, Vector3 headPos, int numPoints, bool beated, int numLives, int sceneIndex, 
        List<int> listWeapons, List<float> listPoints, List<string> listNames)
    {
        checkPoint = lastCPIndex;
        headPositionX = headPos.x;
        headPositionY = headPos.y;
        headPositionZ = headPos.z;
        points = numPoints;
        gameBeated = beated;
        lives = numLives;
        scene = sceneIndex;
        for(int i = 0; i < listWeapons.Count; i++)
            weaponsIndex.Add(listWeapons[i]);
        rankingPoints = listPoints;
        rankingPlayerNames = listNames;
    }

}