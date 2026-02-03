using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _EnemyPrefab;
    [SerializeField]
    private GameObject _Enemycontainer;
    [SerializeField]
    private GameObject[] powerups;
    private bool _stopSpawning = false;
    
   

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    void Update()
    {
     
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_EnemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _Enemycontainer.transform;
            yield return new WaitForSeconds(2.0f);
        }
        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
