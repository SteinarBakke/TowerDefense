using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wavespawner : MonoBehaviour
{

    public static int totalWaves = 0;

    public float timeBetweenWaves = 10f;
    //This should be after all enemies destroyed

    [Header("UI Settings")]
    public Text waveCountDownText;
    public Text totalWavesText;
    private float countdown = 5f; //time before first wave

    [Header("difficulty")]
    public int IncreaseMultiplier = 10;
    public static float addedDifficulty;

    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
        //health?
    }

    public Wave[] waves;
    private int nextWave = 0;

    public SpawnState state = SpawnState.COUNTING;

    private float enemyDeadChecker = 1f;

    void Start()
    {
        addedDifficulty = 0f;
        totalWaves = 0;
    }

    void Update()
    {
        
        //Want to only check this every sec instead of every frame
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
                WaveCompleted();
            else
                return; // Don't want to go further if enemies alive

        }

        if (countdown <= 0f && state != SpawnState.SPAWNING)
        {
            waveCountDownText.text = "";
            StartCoroutine(SpawnWave(waves[nextWave]));
        }
        else if (state == SpawnState.COUNTING)
        {
            waveCountDownText.text = "Next Wave " + Mathf.Floor(countdown).ToString();
            countdown -= Time.deltaTime; //reduce each sec
        }
        enemyDeadChecker -= Time.deltaTime;

    }

    void WaveCompleted()
    {
        if (!GameManager.gameEnded) //don't play when game is over
        {
            totalWaves++;
            totalWavesText.text = "WAVE " + (totalWaves + 1).ToString();

            //After every round effect
            // maybe change this to 10 * (addedDificulty+1) ? To passively 10 until waves completed
            PlayerStats.Currency += totalWaves;
        }

        state = SpawnState.COUNTING;
        countdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
            GameCompleted();
        else
            nextWave++;
    }

    void GameCompleted()
    {
        Debug.Log("GAME WON - Looping with increased difficulty");
        nextWave = 0;
        addedDifficulty++;

    }

    bool EnemyIsAlive ()
    {
        enemyDeadChecker -= Time.deltaTime;
        if (enemyDeadChecker <= 0f)
        {
            enemyDeadChecker = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        for (int i = 0; i <_wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }


        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Instantiate(_enemy, transform.position, transform.rotation);
    }


}
