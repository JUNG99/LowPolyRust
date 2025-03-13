using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // AI 동작 모드를 정의하는 이넘
    public enum AIState
    {
        WonderMode,  // 랜덤 이동
        AttackMode   // 특정 타겟 추적
    }

    // 현재 AI 상태 (Inspector에서 변경 가능)
    public AIState currentState = AIState.WonderMode;

    // AttackMode에서 사용할 타겟 (예: 플레이어)
    public Transform target;

    // WonderMode 설정: 이동 반경과 재설정 시간
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    private float wanderTimerCounter;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimerCounter = wanderTimer;

        // 초기 상태에 따른 행동 설정
        if (currentState == AIState.WonderMode)
        {
            SetNewDestination();
        }
    }

    void Update()
    {
        // 현재 상태에 따라 행동 분기 처리
        if (currentState == AIState.WonderMode)
        {
            WanderUpdate();
            Debug.Log("Wandering");
        }
        else if (currentState == AIState.AttackMode)
        {
            AttackUpdate();
            Debug.Log("Attacking");
        }
    }

    // 랜덤 이동 동작 처리 (WonderMode)
    void WanderUpdate()
    {
        wanderTimerCounter += Time.deltaTime;
        if (wanderTimerCounter >= wanderTimer)
        {
            SetNewDestination();
            wanderTimerCounter = 0;
        }
    }

    // 타겟 추적 동작 처리 (AttackMode)
    void AttackUpdate()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    // 현재 위치를 기준으로 랜덤한 NavMesh 상의 위치를 찾아 목적지로 설정
    void SetNewDestination()
    {
        Vector3 newDestination = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newDestination);
    }

    // NavMesh 위에서 랜덤한 위치를 샘플링하는 함수
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}