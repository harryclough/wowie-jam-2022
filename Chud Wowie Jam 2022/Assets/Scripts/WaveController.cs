using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Spawn {
    public GameObject enemy = null;
    public int maxSpawn = 20;
    public float minSpawnTimer = 3f;
    public float maxSpawnTimer = 7f;
    public int packSize = 1;
    public float spawnTimer = 0;
    [HideInInspector] public int spawnCount = 0;
}

public class WaveController : MonoBehaviour
{
    public static float mapRadius = 25f;
    [SerializeField] float spawnOffset = 2.5f;
    public float waveDuration = 45f;
    [SerializeField] float restDuration = 15f;
    public WaveState state;
    public float waveTimer;
    private float waveNumber = 0;
    public GameObject boundaries;
    private List<GameObject > liveSheep = new List<GameObject>();

    [SerializeField] List<Spawn> spawns = new List<Spawn>();
    [SerializeField] List<GameObject> flock = new List<GameObject>();

    public enum WaveState {
        PREWAVE, // Sheep/Scarecrows Spawn
        WAVE, // Enemies Spawn and Attack
        POSTWAVE, // Enemies Retreat
    }

    // Start is called before the first frame update
    void Start()
    {
        waveTimer = restDuration;
        setupBoundaries();
        beginPreWave();
    }

    // Function to setup the boundaries size, using the mapRadius
    void setupBoundaries() {
        boundaries.transform.localScale = new Vector3(mapRadius * 2, mapRadius * 2, 1);
        // Set the CircleCollider radius to be the mapRadius + spawnOffset * 2
        //boundaries.GetComponent<CircleCollider2D>().radius = mapRadius * 2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {
        waveTimer -= Time.fixedDeltaTime;
        if (waveTimer <= 0)
        {
            switch (state){
                case WaveState.PREWAVE:
                    beginWave();
                    break;
                case WaveState.WAVE:
                    beginPostWave();
                    break;
                case WaveState.POSTWAVE:
                    beginPreWave();
                    // Load the next scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
            }
        }
        switch (state)
        {
            case WaveState.PREWAVE:
                PreWave();
                break;
            case WaveState.WAVE:
                Wave();
                break;
            case WaveState.POSTWAVE:
                PostWave();
                break;
        }
    }

    // Spawns a random pack of enemies at the rim of a circle of radius mapRadius + SpawnOffset
    public void SpawnEnemy(GameObject spawn, int amount)
    {
        // Get a random angle
        float angle = Random.Range(0f, Mathf.PI * 2f);
        // Get a random position on the circle
        
        // Instantiate the enemy at the position
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = new Vector3(Mathf.Cos(angle+i*0.01f), Mathf.Sin(angle+i*0.01f), 0) * (mapRadius + spawnOffset);
            GameObject enemy = Instantiate(spawn, spawn.transform.position + pos, Quaternion.identity);
        }
    }

    // Update during prewave
    private void PreWave()
    {
        
    }

    // Update during wave
    private void Wave()
    {
        // Find how many sheep objects are alive
        if (allSheepDead()){
            //Restart the wave
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        };

        //Iterate through the list of spawns and deduct from their spawn timers. If they are 0, spawn an enemy and select a new random time.
        for (int i = 0; i < spawns.Count; i++)
        {
            spawns[i].spawnTimer -= Time.fixedDeltaTime;
            if (spawns[i].spawnTimer <= 0 && spawns[i].spawnCount < spawns[i].maxSpawn)
            {
                SpawnEnemy(spawns[i].enemy, spawns[i].packSize);
                spawns[i].spawnCount++;
                spawns[i].spawnTimer = Random.Range(spawns[i].minSpawnTimer, spawns[i].maxSpawnTimer);
            }
        }
    }

    // Update during postwave
    private void PostWave()
    {

    }


    private void beginPreWave()
    {
        state = WaveState.PREWAVE;
        waveTimer = restDuration/2;
        spawnSheep();
    }

    private void beginWave()
    {
        waveTimer = waveDuration;
        waveNumber++;
        state = WaveState.WAVE;
    }

    private void beginPostWave()
    {
        state = WaveState.POSTWAVE;
        waveTimer = restDuration/2;
        //find all enemies and call onWaveEnd()
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i=0; i<enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyController>().OnWaveEnd();
        }
    }

    // Spawns each sheep in the flock in the inner third of the circle
    private void spawnSheep()
    {
        for (int i = 0; i < flock.Count; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (mapRadius / 3);
            GameObject sheep = Instantiate(flock[i], flock[i].transform.position + pos, Quaternion.identity);
            liveSheep.Add(sheep);
        }
    }

    // Check if all sheep are dead
    public bool allSheepDead()
    {
        for (int i = 0; i < liveSheep.Count; i++)
        {
            if (liveSheep[i] != null)
            {
                return false;
            }
        }
        return true;
    }
}
