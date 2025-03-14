using UnityEngine;
using UnityEngine.AI;

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

    // 적의 스텟
    public float health = 100f;
    public float moveSpeed = 3.5f;
    public float runSpeed = 7f;
    public float hunger = 100f;
    public float fieldOfView = 120f; // 시야각 (도 단위)
    public float attackRange = 1f;  // 공격 사거리
    public float watchRange = 10f;  // 주시 사거리 (외부 반경)
    
    // 적의 드랍 아이템
    public GameObject dropItem;

    // WonderMode 설정: 이동 반경과 재설정 시간
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    private float wanderTimerCounter;

    // 참조
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        agent.speed = moveSpeed; // 기본 이동 속도 설정
        wanderTimerCounter = wanderTimer;

        // 초기 상태에 따른 행동 설정
        if (currentState == AIState.WonderMode)
        {
            SetNewDestination();
        }
    }

    void Update()
    {
        // 자동 모드 전환 (toggle enabled via autoModeSwitch / false일 경우, currentState는 인스펙터에서 설정된 값으로 유지됨)
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
            Debug.Log("Attacking");
        }
        else if (currentState == AIState.WonderMode)
        {
            WanderUpdate();
            Debug.Log("Wandering");
        }
        
        UpdateAnimation();
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

    // 타겟 추적 및 공격 동작 처리 (AttackMode)
    void AttackUpdate()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            // 공격 사거리 내에 있으면 이동 멈추고 공격 실행
            agent.ResetPath();
            Attack();
        }
        else
        {
            // 공격 사거리 외에 있으면 타겟을 향해 이동 (추적)
            agent.SetDestination(target.position);
        }
    }

    // 적이 타겟한테 데미지 주기
    void Attack()
    {
        if (target != null)
        {
            Debug.Log("Enemy Attacks! Target takes damage.");
            animator.SetTrigger("Attack");
        }
        // >> 타겟의 HP를 감소시키는 로직을 추가 <<
    }
    
    // 적이 타겟한테 데미지 받기
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage. Health: " + health);
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
    
    void UpdateAnimation()
    {
        if (animator == null || agent == null)
            return;
    
        // NavMeshAgent의 속도를 이용해 현재 이동 중인지 확인
        float currentSpeed = agent.velocity.magnitude;
    
        // 이동 속도가 약간이라도 있다면(예: 0.1f 이상이면) 걷는 모션, 그렇지 않으면 Idle 모션
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