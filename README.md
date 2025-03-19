# 3D 서바이벌 게임
플레이어가 자원을 수집하고 생존을 위해 적과 싸우는 게임


## 🖥️ 프로젝트 소개
스파르타 코딩클럽 내일배움캠프 "유니티 숙련 주차"의 팀 프로젝트 입니다.
러스트를 참고하여 제작했습니다.
<br>

## 🕰️ 개발 기간
* 25.03.12 ~ 23.3.18

### 🧑‍🤝‍🧑 맴버구성
 - 팀장 : 이정구
 - 팀원 : 김지현 / 김효재 / 심형진 / 오우택  

### ⚙️ 개발 환경
- `C#`
- `Unity 2022.3.17f`

### 🖥️시연 영상
[Youtubue](https://www.youtube.com/watch?v=6BjTQC2YuKI)
<br>
### 🖥️빌드 파일
<br>

## 📌 주요 기능
[플레이어 - 바로가기](#플레이어)
<br>
[적 - 바로가기](#적)
<br>
[빌딩 - 바로가기](#빌딩)
<br>
[아이템/인벤토리 - 바로가기](#아이템/인벤토리)
<br>
[맵 - 바로가기](#맵)
<br>
[사운드 - 바로가기](#사운드)
<br>
[날씨 - 바로가기](#날씨)
<br>

---
### 플레이어
-1인칭 시점 플레이어의 기본적인 움직임 (이동,점프,달리기,앉기)
<details>
<summary>
  PlayerController
</summary>

public class PlayerController : MonoBehaviour 



    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float crouchSpeed = 2f;
    public float crouchHeight = 0.5f;
    public float standHeight = 1f;

    private Rigidbody rb;
    private float moveSpeed;
    private bool isCrouching = false;

    public Transform cameraHolder;  // MainCamera를 여기 연결
    private float mouseSensitivity = 2f;
    private float xRotation = 0f;

    // 카메라 회전 제어 변수
    private bool canLook = true;

    // 플레이어 이동 제어 변수
    private bool canMove = true;

    // UI의 Aim 객체 참조
    public GameObject Aim;  // 인스펙터에서 Aim을 UI 이미지로 연결

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        moveSpeed = walkSpeed; // 기본 이동 속도 설정
        cameraHolder.localPosition += new Vector3(0f, 0f, 0.5f);
    }

    void Update()
    {
        if (canMove) // canMove가 true일 때만 이동
        {
            MovePlayer();
            Jump();
            Crouch();
        }

        if (canLook) LookAround();  // canLook이 true일 때만 LookAround 실행

        ToggleInventory();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            moveSpeed = sprintSpeed;
        }
        else if (isCrouching)
        {
            moveSpeed = crouchSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                StandUp();
            }
            else
            {
                CrouchDown();
            }
        }
    }

    void CrouchDown()
    {
        isCrouching = true;
        transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, crouchHeight, cameraHolder.localPosition.z);
        Debug.Log("앉기 완료");
    }

    void StandUp()
    {
        isCrouching = false;
        transform.localScale = new Vector3(transform.localScale.x, standHeight, transform.localScale.z);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, standHeight, cameraHolder.localPosition.z);
        Debug.Log("서기 완료");
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //MainCamera 회전
        transform.Rotate(Vector3.up * mouseX);
    }


</details>

-플레이어의 상태(Hp,배고픔,수분 등)에 따른 UI 업데이트
<details>
<summary>
  PlayerStats
</summary>
public class PlayerStats : MonoBehaviour


    public float maxHP = 100f;
    public float maxHunger = 100f;
    public float maxThirst = 100f;

    private float currentHP;
    private float currentHunger;
    private float currentThirst;

    public float hungerDecreaseRate = 1f;
    public float thirstDecreaseRate = 1.5f;
    public float hpDecreaseRate = 5f;

    private void Start()
    {
        currentHP = maxHP;
        currentHunger = maxHunger;
        currentThirst = maxThirst;

        UpdateUI();

        InvokeRepeating(nameof(DecreaseStatsOverTime), 1f, 1f);
    }

    private void DecreaseStatsOverTime()
    {
        if (currentHunger > 0)
        {
            currentHunger -= hungerDecreaseRate;
        }
        else
        {
            if (currentThirst > 0)
            {
                currentThirst -= thirstDecreaseRate;
            }
        }

        if (currentThirst <= 0 && currentHP > 0)
        {
            currentHP -= hpDecreaseRate;
        }
        
        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die(); // 체력이 0 이하가 되면 죽음 처리
        }
        UpdateUI(); // 체력 UI 업데이트
    }

    private void Die()
    {
        // 죽었을 때 처리
        Debug.Log("Player died");
    }
    private void UpdateUI()
    {
        UIManager.Instance.UpdateHP(currentHP, maxHP);
        UIManager.Instance.UpdateHunger(currentHunger, maxHunger);
        UIManager.Instance.UpdateThirst(currentThirst, maxThirst);
    }


</details>

-플레이어가 적을 공격,아이템 획득,자원을 채집


### 적

- Enemy AI : 적의 행동 모드가 Attack Mode와 Wonder Mode로 나뉘며, 플레이어와의 사거리에 따라 자동으로 전환됩니다. NavMeshAgent를 이용해 적이 자동으로 이동합니다.<br>
- Enemy 리스폰 : 지정한 수의 Enemy가, 미리 설정된 SpawnPoint(스폰 포인트)에서 랜덤한 위치에 생성됩니다.<br>

### 빌딩

### 아이템/인벤토리

### 맵 
 - 청크 별로 나무/돌/풀 생성
 - 나무 7분, 돌 5분, 풀 3분 마다 리젠
<br>

![Image](https://github.com/user-attachments/assets/0bbe8020-2e4b-444c-85a6-57f5da14c6b3)
<br>
##### Gizmo Colors
- 🌳 **Green Gizmo**: 나무  
- 🪨 **Blue Gizmo**: 돌  
- 🌿 **Yellow Gizmo**: 풀

<br>

 - 자세한 코드 / 설명은 하위 블로그 참고 ! 
    - [[개발일지] 청크별로 맵 오브젝트 생성](https://youcheachae.tistory.com/58)
 - 트러블 슈팅은 하위 블로그 참고 ! 
    - [[Unity] RaycastHit의 point와 trasform.position의 차이](https://youcheachae.tistory.com/59)

### 사운드
 - 오디오 소스를 플레이어의 맨 아래에 배치, 소리가 캐릭터의 위치에서 일관적으로 들리게 됨.
<br>

![Image](https://github.com/user-attachments/assets/ce00bc22-a823-4219-b1b4-4d17fb610f16)
<br>

 - 사운드 코드는 하위의 레포시토리 - 2.SoundManger 참고
    - [[kimYouChae]Unity-Utility](https://github.com/kimYouChae/Unity-Utility)
  
### 날씨
 - 맑은 날씨 / 눈 / 비 날씨가 반복적으로 바뀐다

![Image](https://github.com/user-attachments/assets/90b0243d-7341-49ce-b349-cac7210bbcda)

<br>

![Image](https://github.com/user-attachments/assets/e77c4deb-3f75-4734-ae9e-c7f832fe6b29)
