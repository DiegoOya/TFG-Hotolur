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

    [HideInInspector]
    public AudioClip menuAudio;
    [HideInInspector]
    public AudioClip gameAudio;
    
    [HideInInspector]
    public int lastCheckpoint;
    
    [HideInInspector]
    public bool doingSetup;
    
    private Vector3 headPosition;

    private TextMeshProUGUI livesText;

    private AudioSource audioSource;

    private List<Transform> checkpoints = new List<Transform>();

    private List<Weapon> weapons = new List<Weapon>();

    void Start ()
    {
        // Ignore the collisions between enemy weapons and the ground
        Physics.IgnoreLayerCollision(8, 9);

        GameObject[] counterTexts = GameObject.FindGameObjectsWithTag(Tags.counterText);
        if (counterTexts.Length != 0)
            livesText = counterTexts[1].GetComponent<TextMeshProUGUI>();
        
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // If livesText isn't null then show in the screen the number of lives left, if not then search for it
        if (livesText != null)
            livesText.text = lives.ToString();
        else
        {
            GameObject[] counterTexts = GameObject.FindGameObjectsWithTag(Tags.counterText);
            if(counterTexts.Length != 0)
                livesText = counterTexts[1].GetComponent<TextMeshProUGUI>();
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
    private void SaveGame()
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

        return new GameData(lastCheckpoint, headPosition, lives, sceneIndex, weaponsIndex);
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

            // Load the scene where the game was saved
            int buildIndex = gameData.scene;
            SceneManager.LoadScene(buildIndex);

            lives = gameData.lives;
            
            Vector3 headPos = new Vector3(gameData.headPositionX, gameData.headPositionY, gameData.headPositionZ);
            
            StartCoroutine(NewSceneLoaded(gameData.lives, headPos, gameData.checkPoint, gameData.weaponsIndex));
          
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

        if(livesAux >= 0)
        {
            SceneManager.LoadScene(buildIndex);

            StartCoroutine(NewSceneLoadedByDeath(livesAux));
            
            //lastCheckpoint.GetComponent<Checkpoint>().SetHasEntered(true);  **** This doesn't make sense if the player gets to win the level 

            // If it is needed some actions between scenes here is the place to write it
        }
        else
        {
            SceneManager.LoadScene(buildIndex);
            
            lastCheckpoint = checkpoints.Count - 1;

            livesAux = 3;

            StartCoroutine(NewSceneLoadedByDeath(livesAux));
        }
    }

    public void NewGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);

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
    private void AddWeapons()
    {
        GameObject[] weaponList = GameObject.FindGameObjectsWithTag(Tags.weapon);
        weapons.Clear(); // Clear the list in case it was already used
        for (int i = 0; i < weaponList.Length; i++)
        {
            weapons.Add((Weapon)weaponList[i].GetComponent<ItemPickUp>().GetItem());
        }
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

    private IEnumerator NewSceneLoadedByNewGame()
    {
        yield return new WaitForSeconds(1f);

        AudioManagement();

        AddCheckpoints();
        AddWeapons();

        Inventory.instance.SetItems(new List<Weapon>());
        Inventory.instance.Add(weapons[0]);

        PlayerShoot playerShoot = GameObject.FindGameObjectWithTag(Tags.player).GetComponentInChildren<PlayerShoot>();
        int maxLength = weapons.Count;
        playerShoot.EquipWeapon(weapons[0].maxDamage, weapons[0].range, weapons[0].fireRate, weapons[0].weaponType);

        lastCheckpoint = checkpoints.Count - 1;
        headPosition = GameObject.FindGameObjectWithTag(Tags.head).transform.position;

        doingSetup = false;
    }

    private IEnumerator NewSceneLoadedByDeath(int livesAux)
    {
        yield return new WaitForSeconds(1f);

        AddCheckpoints();
        
        Checkpoint CP = checkpoints[lastCheckpoint].GetComponent<Checkpoint>();
        if (CP != null)
            CP.SetHasEntered(true);

        if(lives - 1 < 0)
        {
            headPosition = GameObject.FindGameObjectWithTag(Tags.head).transform.position;

            AddWeapons();
            List<Weapon> initialWeapon = new List<Weapon>();
            initialWeapon.Add(weapons[0]);
            Inventory.instance.SetItems(initialWeapon);

            Inventory.instance.ChangeWeapon();
        }

        GameObject.FindGameObjectWithTag(Tags.player).transform.position = checkpoints[lastCheckpoint].position;
        
        GameObject.FindGameObjectWithTag(Tags.head).transform.position = headPosition;

        lives = livesAux;

        doingSetup = false;
    }

    private IEnumerator NewSceneLoaded(int livesAux, Vector3 headPos, int checkpointIndex, List<int> weaponsIndex)
    {
        yield return new WaitForSeconds(1f);

        AddCheckpoints();
        AddWeapons();

        lastCheckpoint = checkpointIndex;
        Checkpoint CP = checkpoints[lastCheckpoint].GetComponent<Checkpoint>();
        if (CP != null)
            CP.SetHasEntered(true);
        
        headPosition = headPos;
        
        List<Weapon> weaponList = new List<Weapon>();
        for (int i = 0; i < weaponsIndex.Count; i++)
        {
            weaponList.Add(weapons[weaponsIndex[i]]);
        }

        GameObject.FindGameObjectWithTag(Tags.player).transform.position = checkpoints[lastCheckpoint].position;
        GameObject.FindGameObjectWithTag(Tags.head).transform.position = headPosition;
        Inventory.instance.SetItems(weaponList);

        lives = 3;

        AudioManagement();

        doingSetup = false;
    }

}
