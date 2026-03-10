using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public GameObject[] powerUpPrefabs;
    
    public float enemySpawnInterval = 5f;
    public float enemySpawnIntervalChange = 1f;
    public float waveLength = 20.0f;
    public float timeBetweenWaves = 10f;

    public float timeForBoss = 10f;

    public float powerUpSpawnInterval = 15f;


	void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(AdjustEnemySpawnInterval());
        StartCoroutine(SpawnPowerUps());
		Instantiate(enemyPrefab, GenerateSpawnLocation(), Quaternion.identity);
	}

    void Update()
    {
        
    }

    IEnumerator AdjustEnemySpawnInterval()
    {
        yield return new WaitForSeconds(waveLength); //Wellenlänge
        StopCoroutine(SpawnEnemies()); //Spawnpause zwischen Wellen

		if (enemySpawnInterval > 2) //Erhöhen des Spawnintervalls nach jeder Welle
		{
			yield return new WaitForSeconds(timeBetweenWaves);//Pausenlänge zwischen Wellen
			enemySpawnInterval -= enemySpawnIntervalChange; //Anpassen des Spawnintervalls
			StartCoroutine(AdjustEnemySpawnInterval()); //Wiederhole das Zeitnehmen und Anpassen der nächsten Welle
			StartCoroutine(SpawnEnemies()); //Starte Spawnen erneut
            yield break; //gehe vorzeitig aus der Coroutine raus, um Leistung zu sparen
		}
		else if (enemySpawnInterval == 2) //Boss spawnen 
        {
			enemySpawnInterval = 5f; //SpawnIntervall wieder hochsetzen
			yield return new WaitForSeconds(timeBetweenWaves);
			SpawnBoss();
			yield return new WaitForSeconds(timeForBoss);
			StartCoroutine(SpawnEnemies()); //Boss Unterstützung schicken
		}
	}

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(enemySpawnInterval);
        Instantiate(enemyPrefab, GenerateSpawnLocation(), Quaternion.identity);
        StartCoroutine(SpawnEnemies());
    }

    void SpawnBoss()
    {
        Vector3 bossSpawnPos = new Vector3(Random.Range(-12, 12), 2, 14); //Boss weiter oben spawnen auf Z-Achse

		Instantiate(bossPrefab, bossSpawnPos, Quaternion.identity);
    }

	Vector3 GenerateSpawnLocation()
    {
        //X12 Z6,5
        float posX = Random.Range(-12, 12);
        float posZ =  9;
        Vector3 spawnPos = new Vector3(posX, 2, posZ);
        return spawnPos;
    }

    IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(powerUpSpawnInterval); //
        int powerUpNumber = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[powerUpNumber], GenerateSpawnLocation(), Quaternion.identity);
        StartCoroutine(SpawnPowerUps());
    }
} 
