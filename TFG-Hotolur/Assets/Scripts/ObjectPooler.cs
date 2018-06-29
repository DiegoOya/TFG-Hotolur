using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to manage the objects in the pool
/// </summary>
public class ObjectPooler : MonoBehaviour {

    // Class to manage the parameters needed of an object in the pool
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public static ObjectPooler instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion

    // List of pools and the dictionary
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Initialize variables
    private void Start()
    {
        // Create a dictionary which will store the pools
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        // Instantiate and store the pools
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // This will spawn an object from a pool stored in poolDictionary
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        // Whenever an object is spawned it can use the method OnObjectSpawn
        if (pooledObject != null)
            pooledObject.OnObjectSpawn();

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    // This will spawn an object from a pool stored in poolDictionary
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform player, Transform enemy, float range)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Search for SmallAttack in the objectToSpawn and if it exists then the variables player, enemy and range are passed to smallAttack
        LongRangeAttack smallAttack = objectToSpawn.GetComponent<LongRangeAttack>();

        if (smallAttack != null)
        {
            smallAttack.SetPlayer(player);
            smallAttack.SetRange(range);

            Physics.IgnoreCollision(objectToSpawn.GetComponent<Collider>(), enemy.GetComponent<Collider>());
        }

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        
        // Whenever an object is spawned it can use the method OnObjectSpawn
        if (pooledObject != null)
            pooledObject.OnObjectSpawn();

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
