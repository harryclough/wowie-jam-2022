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
    public int spawnCount = 0;
}

public class WaveController : MonoBehaviour
{
    [SerializeField] float mapRadius = 50f;
    [SerializeField] float spawnOffset = 5f;
    public float waveDuration = 40f;
    [SerializeField] float restDuration = 15f;
    public WaveState state;
    public float waveTimer;
    private float waveNumber = 0;


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
        waveTimer = 10f;
        beginPreWave();
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
        Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (mapRadius + spawnOffset);
        // Instantiate the enemy at the position
        for (int i = 0; i < amount; i++)
        {
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
        }
    }
}
