using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;

    [SerializeField] int _enemiesCount = 0;

    [SerializeField] GameObject[] _PowerUpPrefabs;

    [SerializeField] GameObject _shieldPrefab;

    [SerializeField] bool _stopSpawningEnemies = false;

    [SerializeField] bool _stopSpawningpowerUp = false;

    Enemy _enemy;

    void Start()
    {
        
        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnPowerUpRoutine());     
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (_stopSpawningEnemies == false) 
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemiesCount++;
            yield return new WaitForSeconds(5.0f);
           
        }

    }
    public void OnPlayerDeathStopSpawning()
    {
        _stopSpawningEnemies = true;
        _stopSpawningpowerUp = true;
      
    }

    public void DecreaseEnemyCount()
    {
        _enemiesCount--;
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawningpowerUp == false)
        {
            float powerUpID = Random.Range(0, 3);

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            switch (powerUpID)
            {
                case 0:
                    Instantiate(_PowerUpPrefabs[0], posToSpawn, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(_PowerUpPrefabs[1], posToSpawn, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(_PowerUpPrefabs[2], posToSpawn, Quaternion.identity);                 
                    break;
            }

            yield return new WaitForSeconds(Random.Range(3f, 8f));

        }
    }

}
