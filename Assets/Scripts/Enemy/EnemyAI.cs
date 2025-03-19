using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAI : MonoBehaviour
{
    // 적 AI 상태 정의
    public enum AIState
    {
        WonderMode,  // 탐색
        AttackMode   // 특정 타겟 추적 및 공격
    }
    
    // 기본 AI 상태 모드 (인스펙터에서 제어 가능하지만, 자동 모드 전환도 수행)
    public AIState currentState = AIState.WonderMode;
    public bool autoModeSwitch = true;
    
    // 적의 타겟
    public Transform target;
    private float lostTargetTimer = 0f;

    // 적의 스텟
    public float health = 100f;
    public float moveSpeed = 1.0f;
    public float fieldOfView = 120f; // 시야각 (도 단위)
    public float attackRange = 3f;  // 공격 사거리
    public float watchRange = 10f;  // 주시 사거리 (외부 반경)
    public float attackDamage = 10f;
    
    // 적 이동 반경과 재설정 시간
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    private float wanderTimerCounter;
    public float minWanderTime = 3f;
    public float maxWanderTime = 7f;
    
    // 적의 드랍 아이템
    public GameObject dropItem;
    
    public event Action OnAttack;

    // 참조
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        agent.speed = moveSpeed;
        wanderTimerCounter = wanderTimer;

        // 초기 상태에 따른 행동 설정
        if (currentState == AIState.WonderMode)
        {
            SetNewDestination();
        }
    }
    
    void OnEnable()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
                Debug.Log("Player target assigned in OnEnable: " + target.name);
            }
            else
            {
                Debug.LogWarning("Player not found in OnEnable.");
            }
        }
    }

    void Update()
    {
        // 자동 모드 전환 
        if (autoModeSwitch)
        {
            if (target != null && IsTargetVisible())
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance <= attackRange)
                {
                    currentState = AIState.AttackMode;
                }
                else if (distance > watchRange)
                {
                    currentState = AIState.WonderMode;
                }
            }
        }

        // 현재 상태에 따라 행동 분기 처리
        if (currentState == AIState.AttackMode)
        {
            AttackUpdate();
            Debug.Log("Enemy Attack Mode");
        }
        else if (currentState == AIState.WonderMode)
        {
            WanderUpdate();
            Debug.Log("Enemy Wander Mode");
        }
        
        UpdateAnimation();
    }

    // WonderMode
    void WanderUpdate()
    {
        wanderTimerCounter += Time.deltaTime;
        if (wanderTimerCounter >= wanderTimer)
        {
            SetNewDestination();
            wanderTimerCounter = 0;
            wanderTimer = UnityEngine.Random.Range(minWanderTime, maxWanderTime);
        }
    }

    // AttackMode
    void AttackUpdate()
    {
        if (target == null) return;
        
        float distance = Vector3.Distance(transform.position, target.position);
        
        if (distance > wanderRadius)
        {
            lostTargetTimer += Time.deltaTime;
            if (lostTargetTimer >= 10f)
            {
                currentState = AIState.WonderMode;
                lostTargetTimer = 0f;
                return;
            }
        }
        else
        {
            lostTargetTimer = 0f;
        }
        
        if (distance <= attackRange)
        {
            agent.ResetPath();
            animator.SetTrigger("Attack");
        }
        else
        {
            float baseSpeed = CalculateSpeed(distance, watchRange);
            agent.speed = baseSpeed * 2.5f;
            agent.SetDestination(target.position);
        }
    }

    // 적이 타겟한테 데미지 주기 (애니메이션으로 실행 가능)
    void Attack()
    {
        if (target != null)
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage);
            }
            
            Debug.Log($"Enemy Attacks! {attackDamage} damage.");
            
            OnAttack?.Invoke();
        }
    }
    
    // 적이 타겟한테 데미지 받기
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy took damage. Enemy Health: {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    // 타겟의 존재를 확인하는 함수
    bool IsTargetVisible()
    {
        if (target == null) return false;

        // 타겟 방향 및 각도 계산
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        // 타겟이 적의 시야각(fieldOfView) 내에 있으며, watchRange(주시 사거리) 내에 있는지 확인
        if (angle <= fieldOfView && directionToTarget.magnitude <= watchRange)
        {
            // watchRange 거리까지의 모든 객체에 대해 Raycast 실시
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            RaycastHit[] hits = Physics.RaycastAll(origin, directionToTarget.normalized, watchRange);

            bool targetFound = false;
            foreach (RaycastHit hit in hits)
            {
                // 타겟이 있는 경우
                if (hit.transform == target)
                {
                    targetFound = true;
                    // Debug.Log("Target Found");
                    break;
                }
            }

            // 타겟이 없는 경우
            if (!targetFound)
            {
                // Debug.Log("Target Not Found");
                return false;
            }
            
            return true;
        }
        return false;
    }

    // 현재 위치를 기준으로 랜덤한 NavMesh 상의 위치를 찾아 목적지로 설정 (WonderMode에서 사용)
    void SetNewDestination()
    {
        Vector3 newDestination = RandomNavSphere(transform.position, wanderRadius, -1);
        float distance = Vector3.Distance(transform.position, newDestination);
        agent.speed = CalculateSpeed(distance, wanderRadius);
        agent.SetDestination(newDestination);
    }

    // NavMesh 위에서 랜덤한 위치를 샘플링하는 함수
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    
    
    // 이동 거리에 따른 속도
    public float lowDistanceThreshold = 3f;
    float CalculateSpeed(float distance, float maxDistance)
    {
        // 1. 현재 거리를 최대 거리로 나누어 0~1 사이의 값 t를 만듭니다.
        float t = Mathf.Clamp01(distance / maxDistance);
        // 2. t 값을 제곱하여 짧은 거리에서의 민감도를 높입니다.
        t = Mathf.Pow(t, 2f);
        // 3. moveSpeed에서 moveSpeed * 2까지 선형 보간하여 기본 속도를 결정합니다.
        float baseSpeed = Mathf.Lerp(moveSpeed, moveSpeed * 2f, t);
        
        return baseSpeed;
    }
    
    void UpdateAnimation()
    {
        if (animator == null || agent == null)
        {
            return;
        }
        
        float currentSpeed = agent.velocity.magnitude;
        // 최대 이동 속도는 moveSpeed * 2, 정규화된 값(0~1)을 WalkScale에 할당
        float normalizedSpeed = Mathf.Clamp01(currentSpeed / (moveSpeed * 2f));
        animator.SetFloat("WalkScale", normalizedSpeed);
        
        // Move 파라미터: 속도가 0.1 이상이면 1 (이동 모션), 아니면 0 (Idle)
        if (currentSpeed > 0.1f)
        {
            animator.SetFloat("Move", 1f);
        }
        else
        {
            animator.SetFloat("Move", 0f);
        }
    }

    // 적의 사망 처리
    void Die()
    {
        Debug.Log("Enemy died.");
        // 드랍할 아이템 있다면, 현재 위치에 생성
        if (dropItem != null)
        {
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
        // 적 프리팹 제거
        Destroy(gameObject);
    }

    
    // 적의 사거리 시각화
    void OnDrawGizmosSelected()
    {
        // 공격 사거리 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 주시 사거리 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, watchRange);

        // 시야각을 나타내는 선 (파란색)
        Gizmos.color = Color.blue;
        Vector3 forward = transform.forward * watchRange;
        Quaternion leftRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        Quaternion rightRotation = Quaternion.AngleAxis(fieldOfView * 0.5f, Vector3.up);
        Vector3 leftBoundary = leftRotation * forward;
        Vector3 rightBoundary = rightRotation * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}