using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform playerTarget; 
    [SerializeField] GameObject enemyPrefab; 

    void Start()
    {
        StartCoroutine(spawnEnemiesProcess());
    }

    IEnumerator spawnEnemiesProcess()
    {
        yield return new WaitForSeconds(3); 
        while (true)
        {
            yield return new WaitForSeconds(1); 
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            Rigidbody enemyRb = newEnemy.GetComponent<Rigidbody>();
            Vector3 towardsPlayer = (playerTarget.position - enemyRb.position).normalized;
            enemyRb.linearVelocity = towardsPlayer * 0.5f; 
        }
    }

    void Update()
    {
        
    }
}
