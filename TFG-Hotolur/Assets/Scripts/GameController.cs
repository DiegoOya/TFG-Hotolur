using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to manage the game
/// </summary>
public class GameController : MonoBehaviour {

    public class Ranking
    {
        public string playerName;
        public float points;

        // Method to sort the Ranking list
        public static int SortAscending(Ranking r1, Ranking r2)
        {
            if (r1.points < r2.points)
                return 1;
            if (r1.points > r2.points)
                return -1;
            else
                return 0;
        }
    }

    #region Singleton
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
    #endregion

    // Number of lives the player will have, when it reaches zero the player has to start from the start
    public int lives = 3;

    //[HideInInspector]
    public AudioClip menuAudio;
    //[HideInInspector]
    public AudioClip gameAudio;

    [HideInInspector]
    public AudioSource audioSource;

    [HideInInspector]
    public int lastCheckpoint;
    [HideInInspector]
    public int points = 0;

    public float pointsToWin = 500f;
    
    [HideInInspector]
    public bool doingSetup;
    [HideInInspector]
    public bool gameBeated = false;
    
    private Vector3 headPosition;

    private TextMeshProUGUI livesText;
    private TextMeshProUGUI pointsText;
    private TextMeshProUGUI pointsToWinText;
    private TextMeshProUGUI rankingText;
    private TextMeshProUGUI addPointsText;
    
    private List<Transform> checkpoints = new List<Transform>();

    private List<Weapon> weapons = new List<Weapon>();

    private List<Ranking> ranking = new List<Ranking>();

    void Start ()
    {
        // Ignore the collisions between enemy weapons and the ground
        Physics.IgnoreLayerCollision(8, 9);
        Physics.IgnoreLayerCollision(9, 10);
        Physics.IgnoreLayerCollision(9, 11);

        audioSource = GetComponent<AudioSource>();
        
        // If it exists a save data file then load it
        if (File.Exists(Application.persistentDataPath + "/gameData.htlr"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.htlr", FileMode.Open);
            GameData gameData = (GameData)bf.Deserialize(file);
            file.Close();
            
            // Load the ranking saved
            for(int i = 0; i < gameData.rankingPlayerNames.Count; i++)
            {
                ranking.Add(new Ranking());
                ranking[i].playerName = gameData.rankingPlayerNames[i];
                ranking[i].points = gameData.rankingPoints[i];
            }
        }

        UpdateRanking();
    }

    private void Update()
    {
        // If livesText isn't null then show in the screen the number of lives left, if not then search for it
        if (livesText != null)
            livesText.text = lives.ToString();
        else
        {
            GameObject livesTextGO = GameObject.FindGameObjectWithTag(Tags.lifeCounterText);
            if (livesTextGO != null)
            {
                livesText = livesTextGO.GetComponent<TextMeshProUGUI>();
                livesText.text = lives.ToString();
            }
        }
    }

    // Updates the points and the text of the number of points
    public void UpdatePoints(int numPoints)
    {
        points += numPoints;

        // If addPointsTextGO isn't null then show in the screen the number of points obtained, if not then search for it
        GameObject addPointsTextGO = GameObject.FindGameObjectWithTag(Tags.addPointsText);
        if (addPointsTextGO != null && numPoints != 0)
        {
            addPointsText = addPointsTextGO.GetComponent<TextMeshProUGUI>();
            addPointsText.text = string.Concat("+", numPoints.ToString());
            addPointsText.color = new Color(0, 1, 0, 1);
            StartCoroutine(DeactivateText(addPointsText));
        }

        // If pointsTextGO isn't null then show in the screen the number of points, if not then search for it
        GameObject pointsTextGO = GameObject.FindGameObjectWithTag(Tags.pointsText);
        if (pointsTextGO != null)
        {
            pointsText = pointsTextGO.GetComponent<TextMeshProUGUI>();
            pointsText.text = string.Concat("Number of points: ", points.ToString());

            if(points > pointsToWin)
            {
                pointsText.color = Color.green;
            }
            else
            {
                pointsText.color = Color.red;
            }
        }

        // If pointsTextGO isn't null then show in the screen the number of points to win, if not then search for it
        GameObject pointsToWinTextGO = GameObject.FindGameObjectWithTag(Tags.pointsToWinText);
        if (pointsToWinTextGO != null)
        {
            pointsToWinText = pointsToWinTextGO.GetComponent<TextMeshProUGUI>();
            pointsToWinText.text = string.Concat("Points to win: ", pointsToWin.ToString());
        }
    }

    // Updates the ranking
    public void UpdateRanking()
    {
        GameObject rankinTextGO = GameObject.FindGameObjectWithTag(Tags.rankingText);
        if(rankinTextGO != null)
        {
            rankingText = rankinTextGO.GetComponent<TextMeshProUGUI>();
            rankingText.text = "";
            if(ranking.Count <= 0)
            {
                rankingText.text = "TODAVÍA NO HAS PUNTUADO";
            }
            else
            {
                for(int i = 0; i < ranking.Count; i++)
                rankingText.text = string.Concat(rankingText.text, (i + 1).ToString(), "º: ", ranking[i].playerName, "   ", ranking[i].points, "\n");
            }
        }
    }

    // When the player enters a checkpoint then save the game and the last checkpoint
    public void CheckpointEntered(Vector3 checkpoint)
    {
        int checkpointIndex = 0;
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i].position.Equals(checkpoint))
            {
                checkpointIndex = i;
                break;
            }
        }
        lastCheckpoint = checkpointIndex;
        SaveGame();
    }

    // Save the game so it can be possible to play again later
    public void SaveGame()
    {
        GameData gameData = CreateSaveData();
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.htlr");
        bf.Serialize(file, gameData);
        file.Close();

        Debug.Log("Game saved");
    }

    // Store the variables needed to save the game
    private GameData CreateSaveData()
    {
        headPosition = GameObject.FindGameObjectWithTag(Tags.head).transform.position;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        List<Weapon> weaponList = Inventory.instance.GetWeapons();
        List<int> weaponsIndex = new List<int>();
        for(int i = 0; i < weaponList.Count; i++)
        {
            for(int j = 0; j < weapons.Count; j++)
            {
                // As there isn't any weapon with the same values look if the player 
                // has the weapon equipped comparing their values
                if (weapons[j].maxDamage == weaponList[i].maxDamage && 
                    weapons[j].range == weaponList[i].range &&
                    weapons[j].fireRate == weaponList[i].fireRate)
                {
                    weaponsIndex.Add(j);
                    break;
                }
            }
        }

        List<float> listPoints = new List<float>();
        List<string> listPlayerNames = new List<string>();
        for (int i = 0; i < ranking.Count; i++)
        {
            listPoints.Add(ranking[i].points);
            listPlayerNames.Add(ranking[i].playerName);
        }

        return new GameData(lastCheckpoint, headPosition, points, gameBeated, lives, sceneIndex, weaponsIndex, listPoints, listPlayerNames);
    }

    // Load the game data so it can be possible to resume from the last play
    public bool LoadGame()
    {
        // If it exists a save data file then load it
        if (File.Exists(Application.persistentDataPath + "/gameData.htlr"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.htlr", FileMode.Open);
            GameData gameData = (GameData)bf.Deserialize(file);
            file.Close();
            
            gameBeated = gameData.gameBeated;
            if (gameBeated)
                return false;

            // Load the scene where the game was saved
            int buildIndex = gameData.scene;
            SceneManager.LoadScene(buildIndex);

            lives = gameData.lives;

            Vector3 headPos = new Vector3(gameData.headPositionX, gameData.headPositionY, gameData.headPositionZ);

            StartCoroutine(NewSceneLoaded(gameData.lives, headPos, gameData.points, gameData.checkPoint, gameData.weaponsIndex));

            return true;
        }
        else
        {
            return false;
        }
    }

    // Load a new scene and manage the actions after that
    public void LoadScene(int buildIndex)
    {
        int livesAux = lives - 1;

        if (buildIndex != 0)
        {
            if (livesAux >= 0)
            {
                SceneManager.LoadScene(buildIndex);

                StartCoroutine(NewSceneLoadedByDeath(livesAux));
            }
            else
            {
                SceneManager.LoadScene(buildIndex);

                lastCheckpoint = checkpoints.Count - 1;

                livesAux = 3;

                StartCoroutine(NewSceneLoadedByDeath(livesAux));
            }
        }
        else
        {
            SceneManager.LoadScene(buildIndex);
        }
    }

    public void NewGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);

        if(!audioSource.isPlaying)
            audioSource.Play();

        gameBeated = false;

        if(buildIndex != 0)
            StartCoroutine(NewSceneLoadedByNewGame());
    }

    public void AudioManagement()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioSource.clip = menuAudio;
            audioSource.Play();
        }

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            audioSource.clip = gameAudio;
            audioSource.Play();
        }
    }

    // Search for all the weapons in the scene and add them to a list
    private GameObject[] AddWeapons()
    {
        GameObject[] weaponList = GameObject.FindGameObjectsWithTag(Tags.weapon);
        weapons.Clear(); // Clear the list in case it was already used
        for (int i = 0; i < weaponList.Length; i++)
        {
            weapons.Add((Weapon)weaponList[i].GetComponent<ItemPickUp>().GetItem());
        }

        // Return the list of GameObjects of the weapons if it is needed
        return weaponList;
    }

    // Search for all the checkpoints in the scene and add them to a list
    private void AddCheckpoints()
    {
        GameObject[] checkpointList = GameObject.FindGameObjectsWithTag(Tags.checkpoint);
        checkpoints.Clear(); // Clear the list in case it was already used
        for(int i = 0; i < checkpointList.Length; i++)
        {
            checkpoints.Add(checkpointList[i].transform);
        }
    }

    // Adds a new player in the ranking
    public void AddInRanking(string playerName)
    {
        // If the ranking has 10 points then insert this new one if it is greater
        // If it has has less then add a new one in its corresponding place
        if (ranking.Count < 10)
        {
            ranking.Add(new Ranking());
            ranking[ranking.Count - 1].points = points;
            ranking[ranking.Count - 1].playerName = playerName;
        }
        else
        {
            // As the ranking is sorted if the last points is less than the actual points
            // then remove the last item and add the new one
            if(ranking[9].points < points)
            {
                ranking.RemoveAt(9);
                ranking.Add(new Ranking());
                ranking[ranking.Count - 1].points = points;
                ranking[ranking.Count - 1].playerName = playerName;
            }
        }

        // And finally sort the ranking
        ranking.Sort(Ranking.SortAscending);
    }

    private IEnumerator NewSceneLoadedByNewGame()
    {
        yield return new WaitForSeconds(1f);

        AudioManagement();

        AddCheckpoints();
        AddWeapons();

        Inventory.instance.SetItems(new List<Weapon>());
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].name == "Pistol")
            {
                Inventory.instance.Add(weapons[i]);
                break;
            }
        }
        
        int maxLength = weapons.Count;
        Inventory.instance.ChangeWeapon();

        lastCheckpoint = checkpoints.Count - 1;
        headPosition = GameObject.FindGameObjectWithTag(Tags.head).transform.position;

        points = 0;
        UpdatePoints(0);
        
        doingSetup = false;
    }

    private IEnumerator NewSceneLoadedByDeath(int livesAux)
    {
        yield return new WaitForSeconds(1f);

        AddCheckpoints();
        GameObject[] weaponsGO = AddWeapons();


        Checkpoint CP = checkpoints[lastCheckpoint].GetComponent<Checkpoint>();
        if (CP != null)
            CP.SetHasEntered(true);

        if(lives - 1 < 0)
        {
            headPosition = GameObject.FindGameObjectWithTag(Tags.head).transform.position;

            points = 0;
            UpdatePoints(0);
            
            List<Weapon> initialWeapon = new List<Weapon>();
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == "Pistol")
                {
                    initialWeapon.Add(weapons[i]);
                    break;
                }
            }
            Inventory.instance.SetItems(initialWeapon);

            Inventory.instance.ChangeWeapon();
        }

        GameObject.FindGameObjectWithTag(Tags.player).transform.position = checkpoints[lastCheckpoint].position;
        
        GameObject.FindGameObjectWithTag(Tags.head).transform.position = headPosition;

        lives = livesAux;

        Inventory.instance.EquipActualWeapon();

        // Look if there is any weapon equipped visible in the game, if there is any move it where it is not visible
        List<Weapon> weaponListInv = Inventory.instance.GetWeapons();
        for (int i = 0; i < weapons.Count; i++)
        {
            for (int j = 0; j < weaponListInv.Count; j++)
            {
                if (weapons[i].name == weaponListInv[j].name)
                {
                    weaponsGO[i].transform.position = new Vector3(-50f, 0f, 0f);
                    break;
                }
            }
        }

        UpdatePoints(0);

        doingSetup = false;
    }

    private IEnumerator NewSceneLoaded(int livesAux, Vector3 headPos, int numPoints, int checkpointIndex, List<int> weaponsIndex)
    {
        yield return new WaitForSeconds(1f);

        AddCheckpoints();
        GameObject[] weaponsGO = AddWeapons();

        lastCheckpoint = checkpointIndex;
        Checkpoint CP = checkpoints[lastCheckpoint].GetComponent<Checkpoint>();
        if (CP != null)
            CP.SetHasEntered(true);
        
        headPosition = headPos;

        points = numPoints;
        UpdatePoints(0);

        List<Weapon> weaponList = new List<Weapon>();
        for (int i = 0; i < weaponsIndex.Count; i++)
        {
            weaponList.Add(weapons[weaponsIndex[i]]);
            weaponsGO[weaponsIndex[i]].transform.position = new Vector3(-50f, 0f, 0f);
        }

        GameObject.FindGameObjectWithTag(Tags.player).transform.position = checkpoints[lastCheckpoint].position;
        GameObject.FindGameObjectWithTag(Tags.head).transform.position = headPosition;
        Inventory.instance.SetItems(weaponList);

        lives = 3;

        AudioManagement();

        doingSetup = false;
    }

    // "Deactivate" the addPointsText by making it transparent modifying the alpha component
    private IEnumerator DeactivateText(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(2f);
        
        text.color = new Color(0, 0, 0, 0);
    }

}
