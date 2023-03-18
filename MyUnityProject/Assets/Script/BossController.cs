using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    const int STATE_WALK = 1;
    const int STATE_ATTACK = 2;
    const int STATE_SLIDE = 3;

    private GameObject BulletPrefab;
    private GameObject Target;
    private Animator Anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 Movement;
    private Vector3 EndPoint;

    private int HP;
    private int choice;

    private float CoolDown;
    private float CoolDefault;
    private float SlideCool;
    private float SCoolDefault;
    private float Speed;

    private bool Active;
    private bool Slide;
    private bool Attack;
    private bool Walk;

    private void Awake()
    {
        Target = GameObject.Find("Player");
        Anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        CoolDefault = 1.5f;
        SCoolDefault = 10.0f;
    }

    void Start()
    {
        HP = 30000;
        
        CoolDown = CoolDefault;
        SlideCool = SCoolDefault;
        Speed = 0.5f;

        Active = false;
        Slide = false;
        Attack = false;
        Walk = false;

        //StartCoroutine(onCoolDown());
    }

    void Update()
    {
        float result = Target.transform.position.x - transform.position.x;

        if (result < 0.0f)
            spriteRenderer.flipX = true;
        else if (result > 0.0f)
            spriteRenderer.flipX = false;

        if (ControllerManager.GetInstance().DirRight)
            transform.position -= new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;

        // 아무 행동 안하고있으면
        if(!Active)
        {
            CoolDown -= Time.deltaTime;
            SlideCool -= Time.deltaTime;
            // 행동을 고른다
            if (CoolDown <= 0)
            {
                Active = true;
                choice = onController();
                CoolDown = CoolDefault;
            }
            
            //StartCoroutine(onCoolDown());
        }
        else
        {   // 위 에서 고른 행동을 아래에서 실행한다.
            switch (choice)
            {
                case STATE_WALK:
                    onWalk();
                    break;

                case STATE_ATTACK:
                    onAttack();
                    break;

                case STATE_SLIDE:
                    onSlide();
                    break;
            }
        }
    }

    private int onController()
    {
        
        int BossAct = Random.Range(STATE_WALK, STATE_SLIDE + 1);
        // ** 어디로 움직일지 정하는 시점에 플레이어의 위치를 도착지점으로 세팅
        EndPoint = Target.transform.position;
        float Distance = Vector3.Distance(EndPoint, transform.position);
        // ** 행동 패턴에 대한 내용을 추가.
        

        // ** 초기화
        if(Walk)
        {
            Movement = new Vector3(0.0f, 0.0f, 0.0f);
            Anim.SetFloat("Speed", Movement.x);
            Walk = false;
        }
        if(Slide)
            Slide = false;
        if(Attack)
            Attack = false;

        if (Distance <= 0.5f)
            return STATE_ATTACK;
        else if (Distance <= 5.0f && BossAct == STATE_SLIDE)
            return STATE_WALK;


       
        // * return
        // * 1 : 이동     STATE_WALK 
        // * 2 : 공격     STATE_ATTACK
        // * 3 : 슬라이딩 STATE_SLIDE 
        return BossAct; // 랜덤한 행동을 실시한다.
    }

    private IEnumerator onCoolDown()
    {
        float fTime = CoolDown;

        while (fTime > 0.0f)
        {
            fTime -= Time.deltaTime;
            yield return null;
        }
    }

    private void onAttack()
    {
        Attack = true;
        float Distance = Vector3.Distance(EndPoint, transform.position);

        if (Distance > 5.0f)
        {
            Anim.SetTrigger("Attack");// 투사체 발사

            // 불렛 프리펩 가져옴
            BulletPrefab = Resources.Load("Prefabs/BossBullet") as GameObject;
            // 오브젝트 생성
            GameObject Obj = Instantiate(BulletPrefab);

            // 오브젝트 방향을 보스와 똑같이
            Obj.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;

            // 오브젝트의 위치는 보스의 위치
            Obj.transform.position = new Vector3(
                transform.position.x,
                transform.position.y - 1.0f,
                0.0f);

            // 오브젝트의 이름 설정
            Obj.transform.name = "BossBullet";

           

        }
        else
            Anim.SetTrigger("Attack");// 일반 공격 모션



        Active = false;
    }

    private void onWalk()
    {
        // 걷는다.
        Walk = true;
        //Movement = new Vector3(Speed + 1.0f, 0.0f, 0.0f);

        // ** 도착지 까지
        float Distance = Vector3.Distance(Target.transform.position, transform.position);

        // 도착지 까지의 거리가 멀다
        if(Distance > 0.5f)
        {
            print("Walk");

            // 행동 고를때 정한 도착지점과 오브젝트의 거리를 계산
            Vector3 Direction = (Target.transform.position -  transform.position).normalized;

            // 도착지 까지 이동.
            Movement = new Vector3(
                Speed * Direction.x,
                Speed * Direction.y,
                0.0f);

            transform.position += Movement * Time.deltaTime;
            Anim.SetFloat("Speed", Mathf.Abs(Movement.x));
        }
        else
            Active = false;
    }

    private void onSlide()
    {
        Slide = true;

        float Distance = Vector3.Distance(EndPoint, transform.position);

        // 도착지 까지의 거리가 멀다
        if (Distance > 0.5f)
        {
            print("Slide");
            if (Anim.GetBool("PreSlide") == false)
                Anim.SetBool("PreSlide", true);

            Anim.SetBool("Slide", true);
            // 행동 고를때 정한 도착지점과 오브젝트의 거리를 계산
            Vector3 Direction = (EndPoint - transform.position).normalized;

            // 도착지 까지 이동.
            Movement = new Vector3(
                Speed * Direction.x,
                Speed * Direction.y,
                0.0f);

            transform.position += Movement * Time.deltaTime * 1.0f;
        }
        else
        {
            print("EndSlide");
            Anim.SetBool("Slide", false);
            Anim.SetBool("PreSlide", false);
            SlideCool = SCoolDefault;
            Active = false;
        }
            
    }

    
}


