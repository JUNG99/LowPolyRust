using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyRespawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;  // 리젠할 적 프리팹
    public List<Transform> spawnPoints; // 맵에 배치된 리젠 지점들
    public float respawnDelay = 5f;  // 리젠 딜레이 (초)
    public int maxEnemyCount = 10;   // 최대 리젠 수
    public float spawnRange = 2f;    // 각 스폰 포인트 주변의 스폰 범위 (반경)

    public Transform parentTransform;
    
    void Start()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnDelay);

            // Enemy 레이어 번호를 가져옵니다.
            int enemyLayer = LayerMask.NameToLayer("Enemy");
        
            // EnemyAI 컴포넌트를 가진 모든 오브젝트를 찾고, 해당 오브젝트의 레이어가 "Enemy"인지 확인합니다.
            EnemyAI[] enemyAIs = GameObject.FindObjectsOfType<EnemyAI>();
            int enemyCount = 0;
            for (int i = 0; i < enemyAIs.Length; i++)
            {
                if (enemyAIs[i].gameObject.layer == enemyLayer)
                {
                    enemyCount++;
                }
            }

            if (enemyCount < maxEnemyCount)
            {
                if (spawnPoints.Count > 0)
                {
                    int index = Random.Range(0, spawnPoints.Count);
                    Transform spawnPoint = spawnPoints[index];
                    // 스폰 포인트 내에서 랜덤한 위치를 구합니다.
                    Vector3 randomOffset = Random.insideUnitSphere * spawnRange;
                    randomOffset.y = 0f; // 높이 보정
                    GameObject spawnedEnemy = Instantiate (enemyPrefab, spawnPoint.position + randomOffset, spawnPoint.rotation, parentTransform);
                }
            }
        }
    }
    
    void OnDrawGizmos()
    {
        if (spawnPoints == null)
            return;

        Gizmos.color = Color.green;
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                // 각 스폰 포인트 주변에 spawnRange 반경의 와이어 스피어를 그림
                Gizmos.DrawWireSphere(spawnPoint.position, spawnRange);
            }
        }
    }
}