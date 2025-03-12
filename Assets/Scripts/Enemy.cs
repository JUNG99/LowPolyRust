using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // 적의 모드 정의
    public enum Mode
    {
        Angry,   // 플레이어를 공격, 추적할 수 있음
        Friendly, // 플레이어를 공격하지 않고, 추적하지 않음
        Custom    // 커스텀 모드: 개발자가 별도 조건을 구현
    }

    // 행동 상태 정의
    public enum ActionState
    {
        Idle,   // 정지
        Move,   // 탐색 (랜덤 이동 포함)
        Attack, // 공격
        Follow, // 따라오기
    }

    // 적의 기본 스텟 정보
    [System.Serializable]
    public class Stats
    {
        public float health = 100f;       // 체력
        public float moveSpeed = 3.5f;      // 이동 속도
        public float runSpeed = 5f;         // 뛰는 속도
        public float hunger = 0f;           // 배고픔 (필요에 따라 증가)
    }

    [Header("Enemy Stats")]
    public Stats stats = new Stats();

    [Header("Mode Settings")]
    public Mode currentMode = Mode.Angry;

    [Header("Detection & Action Distances")]
    [Tooltip("플레이어를 인식하는 시야각 (전체 각도)")]
    public float viewAngle = 120f;
    [Tooltip("플레이어 인식 감지 거리")]
    public float detectionDistance = 10f;
    [Tooltip("공격 사거리 (m)")]
    public float attackRange = 10f;
    [Tooltip("팔로우 사거리 (m)")]
    public float followRange = 20f;

    [Header("Wander Settings")]
    [Tooltip("랜덤 이동 범위")]
    public float wanderRadius = 10f;
    [Tooltip("랜덤 이동 목적지 갱신 주기 (초)")]
    public float wanderTimer = 5f;
    private float wanderTimerCounter;

    // 현재 행동 상태
    private ActionState currentAction = ActionState.Idle;

    // Unity 컴포넌트 참조
    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;

    void Start()
    {
        // NavMeshAgent, Animator 컴포넌트 초기화
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // 플레이어 태그를 가진 객체 탐색
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // 스텟에 맞게 NavMeshAgent 속도 설정
        if (agent != null)
        {
            agent.speed = stats.moveSpeed;
        }

        // 랜덤 이동 타이머 초기화
        wanderTimerCounter = wanderTimer;
    }

    void Update()
    {
        currentAction = ActionState.Move;
        if (agent != null)
        {
            agent.isStopped = false;
            Wander();
        }
        // 플레이어 탐지 체크
        bool playerDetected = CheckPlayerDetection();

        // 현재 모드에 따른 행동 처리
        switch (currentMode)
        {
            case Mode.Angry:
                HandleAttackMode(playerDetected);
                break;
            case Mode.Friendly:
                HandleFriendlyMode();
                break;
            case Mode.Custom:
                HandleCustomMode();
                break;
        }

        // 애니메이터 업데이트 (애니메이션 전환)
        UpdateAnimation();
    }

    // 플레이어 탐지 로직: 시야각, 감지 거리 체크 (추가로 엄폐물 등은 구현 필요)
    bool CheckPlayerDetection()
    {
        if (player == null)
            return false;

        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        // 감지 거리 체크
        if (distance > detectionDistance)
            return false;

        // 시야각 체크 (정면 기준 양쪽 viewAngle/2)
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > viewAngle * 0.5f)
            return false;

        // 추가: 플레이어가 뒤에서 접근하거나 엄폐물을 사용하면 false 처리 가능 (Raycast 등으로 구현 가능)

        return true;
    }

    // 공격 모드에서의 행동 처리
    void HandleAttackMode(bool playerDetected)
    {
        if (playerDetected)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                // 공격 상태: 플레이어 공격 (플레이어 HP 감소 로직 추가)
                currentAction = ActionState.Attack;
                Debug.Log("적이 플레이어를 공격");
                if (agent != null)
                    agent.isStopped = true;
            }
            else if (distanceToPlayer <= followRange)
            {
                // 따라오기 상태: 플레이어를 추적
                currentAction = ActionState.Follow;
                Debug.Log("적이 플레이어를 따라감");
                if (agent != null)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
            }
            else
            {
                // 탐색 상태: 플레이어는 인식하지만 아직 추적 범위에 들어오지 않음 → 랜덤 이동 실행
                currentAction = ActionState.Move;
                Debug.Log("적이 랜덤 이동 중");
                if (agent != null)
                {
                    agent.isStopped = false;
                    Wander();
                }
            }
        }
        else
        {
            // 플레이어를 인식하지 못한 경우: Idle 상태
            currentAction = ActionState.Idle;
            if (agent != null)
                agent.isStopped = true;
        }
    }

    // 친화 모드에서의 행동 처리: 플레이어를 공격하거나 추적하지 않음
    void HandleFriendlyMode()
    {
        currentAction = ActionState.Idle;
        if (agent != null)
            agent.isStopped = true;
        // (추가) 필요에 따라 랜덤 이동이나 다른 행동 패턴 구현 가능
    }

    // 커스텀 모드: 개발자가 별도 조건에 따라 행동을 제어
    void HandleCustomMode()
    {
        // 예시: 특별한 조건에 따라 행동 전환
        // 현재는 기본 Idle 상태로 설정
        currentAction = ActionState.Idle;
    }

    // 랜덤 이동 로직: wanderTimer에 따라 새로운 목적지 설정
    void Wander()
    {
        wanderTimerCounter += Time.deltaTime;
        // 목적지 도달 여부 또는 타이머가 만료되면 새로운 목적지 설정
        if (wanderTimerCounter >= wanderTimer || (agent.pathPending == false && agent.remainingDistance < 0.5f))
        {
            Vector3 newDestination = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newDestination);
            wanderTimerCounter = 0f;
        }
    }

    // NavMesh 내의 랜덤 위치를 반환하는 헬퍼 메서드
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    // 애니메이터를 통해 행동에 따른 애니메이션 전환 처리
    void UpdateAnimation()
    {
        if (animator == null)
            return;

        // 모든 행동 애니메이션 상태를 초기화
        animator.SetBool("Idle", false);
        animator.SetBool("Move", false);
        animator.SetTrigger("Attack");

        // 현재 행동 상태에 따른 애니메이션 활성화
        switch (currentAction)
        {
            case ActionState.Idle:
                animator.SetBool("Idle", true);
                break;
            case ActionState.Move:
                animator.SetBool("Move", true);
                break;
            case ActionState.Attack:
                animator.SetTrigger("Attack");
                break;
        }
    }
}