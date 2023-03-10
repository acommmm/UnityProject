using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    private int hitCount;
    private float Speed;
    //private float checkRun;
    private Vector3 Movement;
    // 플레이어 Animator 구성요소
    private Animator animator;
    // 플레이어 SpriteRenderer 구성요소
    private SpriteRenderer spriteRenderer;

    // 상태 체크
    private bool onAttack; // 공격
    private bool onHit; // 피격
    private bool onRoll; // 구르기
    private bool onJump; // 점프
    private bool onDive; // 추락
    private bool onClimbing; // 등반

    // 복제된 총알의 저장 공간
    public List<GameObject> Bullets = new List<GameObject>(); 

    // 복사할 총알 원본
    public GameObject BulletPrefab;

    // 복사할 FX 원본
    public GameObject fxPrefab;

    public GameObject[] stageBack = new GameObject[7];
    // 플레이어가 마지막으로 바라본 방향
    private float Direction;

    public bool DirLeft;
    public bool DirRight;

    // 유니티 기본 제공 함수
    // 초기값 설정 할 때 사용
    private void Awake()
    {
        // Animator를 받아온다.
        animator = this.GetComponent<Animator>();
        // SpriteRenderer를 받아온다.
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        // 속도 초기화
        Speed = 5.0f;
        hitCount = 0;
        Direction = 1.0f;

        for (int i = 0; i < 7; ++i)
            stageBack[i] = GameObject.Find(i.ToString());

        // 초기값 세팅
        onAttack = false;
        onHit = false;
        onRoll = false;
        onJump = false;
        onDive = false;
        onClimbing = false;

        DirLeft = false;
        DirRight = false;
    }

    // 유니티 기본 제공 함수
    // 프레임마다 반복적으로 실행되는 함수.
    void Update()
    {
        // 실수 연산 IEEE 754 
        // Input.GeAxisRaw = -1, 0, 1 반환a
        // Input.GeAxis = -1.0f ~ 1.0f 반환
        float Hor = Input.GetAxisRaw("Horizontal");
        float Ver = 0;
        //float Ver = Input.GetAxisRaw("Vertical");



        // Hor이 0이라면 멈춰있는 상태이므로 예외처리
        if (Hor != 0)
            Direction = Hor;
        else
        {
            DirLeft = false;
            DirRight = false;
        }

        // 바라보고있는 방향에 따라 이미지 플립설정
        if (Direction < 0)
        {
            spriteRenderer.flipX = DirLeft = true;
            // 실제 플레이어를 움직인다.
            transform.position += Movement;
        }
            
        else if (Direction > 0)
        {
            spriteRenderer.flipX = false;
            DirRight = true;
            if(transform.position.x < 0)
            {
                transform.position += Movement;
            }
        }
        


        ////움직이고 있으면 Run애니메이션 실행
        //if (Hor < 0) // 왼쪽으로 가면 플립
        //{
        //    checkRun = 1.0f; // 움직이고 있다는 의미 Run 애니메이션 실행
        //    spriteRenderer.flipX = true;
        //}
        //else if (Hor > 0) // 오른쪽으로 가면 플립 해제
        //{
        //    checkRun = 1.0f; // 움직이고 있다는 의미 Run 애니메이션 실행
        //    spriteRenderer.flipX = false;
        //}
        //else
        //    checkRun = 0.0f;  // 좌우 변화가 없다는 의미 Idle 애니메이션 실행
        ////spriteRenderer.flipX = Hor < 0 ? true : false;

        // 입력받은 값으로 플레이어 움직임
        Movement = new Vector3(
           Hor * Time.deltaTime * Speed,
           Ver * Time.deltaTime * Speed,
           0.0f);

        // 좌측 컨트롤 키 입력시 
        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();// 공격

        // z키 입력시
        if (Input.GetKey(KeyCode.Z))
            OnRoll();// 구르기
        
        // 좌측 쉬프트 키 입력시
        if (Input.GetKey(KeyCode.LeftShift))
        {
            OnHit(); // 피격 발생

            if(hitCount >= 3)
                animator.SetInteger("HitCount", hitCount); // 목숨
        }
        // 좌측 알트 입력시
        if (Input.GetKey(KeyCode.LeftAlt))
        {//OnJump();// 점프
         }

        // 스페이스바 누르면
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 총알 원본을 복제
            GameObject Obj = Instantiate(BulletPrefab);
            //Obj.transform.name = "Bullet";
            // 복제된 총알 위치를 플레이어 위치로 설정
            Obj.transform.position = transform.position;
            // 총알의 BulletController 스크립트 받아옴
            BulletController Controller = Obj.AddComponent<BulletController>();
            
            // 총아알 의 SpriteRenderer를 받아옴
            SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

            // 총알 스크립트 내부의 방향 변수를 현재 플레이어의 방향 변수로 초기화
            Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

            // 총알 스크립트 내부의 Fx Prefab을 설정
            Controller.fxPrefab = fxPrefab;
            //Controller.Direction = spriteRenderer.flipX ? transform.right * -1 : transform.right;
            // 총알 이미지 반전상태를 플레이어의 이미지 반전 상태로 설정
            renderer.flipY = spriteRenderer.flipX;
   
            // 모든 설정 종료 시 저장소에 보관
            Bullets.Add(Obj);
        }
          
        // 점프누르면 y좌표 이동
        if (onJump)
            //Movement.y = 0.7f * Time.deltaTime * Speed;

        //  떨어지는 중이면 
        if (onDive)
        {
            //Movement.y = -0.7f * Time.deltaTime * Speed;
            //if (playerPosition().y <= 0) // 땅에 닿으면.(충돌 조건)
            {              
                // 멈춤
                //SetDive();
                //SetClimbing();
            }         
        }

        // 올라갈수있는 좌표에 있으면
        if (playerPosition().x >= 1 && playerPosition().x <= 2) 
        {
            Ver = Input.GetAxisRaw("Vertical");
            if(onJump)
            {
                SetJump();
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) // 올라갈 위치에서 위방향키누르면
            {
                OnClimbing(); // 애니메이션 실행
            }
        }
        else if(playerPosition().x > 2 || playerPosition().x < 1)
        {
            if(onClimbing)
                OnDive();
        }

        // 현재 Y좌표를 애니메이션에 전달
        animator.SetFloat("Fly", playerPosition().y);
        // 플레이어 움직임에 따라 이동 모션 실행
        animator.SetFloat("ChechRun", Hor);
        //animator.SetFloat("ChechRun", checkRun);

        // 실제 플레이어를 움직임
        



    }

    private void OnAttack()
    {
        // 공격모션 진행중일경우
        if (onAttack)
            return; // 함수 종료

        // 공격모션 진행중이 아닐경우
        // 공격상태 활성화
        onAttack = true;
        // 공격 모션 실행
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        // 함수가 실행되면 공격상태 비활성화
        // 함수는 애니메이션 클립의 이벤트 프레임으로 삽입된다.
        onAttack = false;
    }
    private void OnHit()
    {
        if (onHit)
            return;

        onHit = true;
        ++hitCount;
        
        animator.SetTrigger("Hit");
        
    }
    private void SetHit()
    {
        onHit = false;
    }
    private void OnRoll()
    {
        if (onRoll)
            return;

        onRoll = true;

        animator.SetTrigger("Roll");
    }

    private void SetRoll()
    {
        onRoll = false;
    }
    private void OnJump()
    {
        if (onJump)
            return;
        
        onJump = true;

        animator.SetTrigger("Jump");
    }

    private void SetJump()
    {     
        onJump = false;
    }
    private void OnDive()
    {
        if (onDive)
            return;
        
        onDive = true;

        animator.SetTrigger("Dive");
    }
    private void SetDive()
    {
        onDive = false;      
    }
    private void OnClimbing()
    {
        if (onClimbing)
            return;

        onClimbing = true;

        animator.SetTrigger("Climbing");
    }
    private void SetClimbing()
    {
        onClimbing = false;
    }
    private void ResetCount()
    {
        // 체력을 초기화
        hitCount = 0;
        // 초기화한 체력 애니메이션에 전달
        animator.SetInteger("HitCount", hitCount);
    }      
    private Vector3 playerPosition()
    {     
        // 플레이어의 위치를 받아온다.
        return this.gameObject.transform.position;
    }
    private void sholve()
    {
        // 총알 원본을 복제
        GameObject Obj = Instantiate(BulletPrefab);
        //Obj.transform.name = "Bullet";
        // 복제된 총알 위치를 플레이어 위치로 설정
        Obj.transform.position = transform.position;
        // 총알의 BulletController 스크립트 받아옴
        BulletController Controller = Obj.AddComponent<BulletController>();

        // 총아알 의 SpriteRenderer를 받아옴
        SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

        // 총알 스크립트 내부의 방향 변수를 현재 플레이어의 방향 변수로 초기화
        Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

        // 총알 스크립트 내부의 Fx Prefab을 설정
        Controller.fxPrefab = fxPrefab;
        //Controller.Direction = spriteRenderer.flipX ? transform.right * -1 : transform.right;
        // 총알 이미지 반전상태를 플레이어의 이미지 반전 상태로 설정
        renderer.flipY = spriteRenderer.flipX;




        // 모든 설정 종료 시 저장소에 보관
        Bullets.Add(Obj);
    }


}


