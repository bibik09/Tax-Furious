using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform spawnPos;
    [SerializeField] Vector2 range;
    public GameObject enemy;
    public float timeToSpawn;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        
        yield return new WaitForSeconds(timeToSpawn);
        Vector2 pos = spawnPos.position + new Vector3(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y));
        Instantiate(enemy, pos, Quaternion.identity);

        RepeatSpawnEnemy();
    }

    void RepeatSpawnEnemy()
    {
        StartCoroutine(SpawnEnemy());
    }
}
