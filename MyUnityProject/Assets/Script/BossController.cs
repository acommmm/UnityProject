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

        // �ƹ� �ൿ ���ϰ�������
        if(!Active)
        {
            CoolDown -= Time.deltaTime;
            SlideCool -= Time.deltaTime;
            // �ൿ�� ����
            if (CoolDown <= 0)
            {
                Active = true;
                choice = onController();
                CoolDown = CoolDefault;
            }
            
            //StartCoroutine(onCoolDown());
        }
        else
        {   // �� ���� �� �ൿ�� �Ʒ����� �����Ѵ�.
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
        // ** ���� �������� ���ϴ� ������ �÷��̾��� ��ġ�� ������������ ����
        EndPoint = Target.transform.position;
        float Distance = Vector3.Distance(EndPoint, transform.position);
        // ** �ൿ ���Ͽ� ���� ������ �߰�.
        

        // ** �ʱ�ȭ
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
        // * 1 : �̵�     STATE_WALK 
        // * 2 : ����     STATE_ATTACK
        // * 3 : �����̵� STATE_SLIDE 
        return BossAct; // ������ �ൿ�� �ǽ��Ѵ�.
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
            Anim.SetTrigger("Attack");// ����ü �߻�

            // �ҷ� ������ ������
            BulletPrefab = Resources.Load("Prefabs/BossBullet") as GameObject;
            // ������Ʈ ����
            GameObject Obj = Instantiate(BulletPrefab);

            // ������Ʈ ������ ������ �Ȱ���
            Obj.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;

            // ������Ʈ�� ��ġ�� ������ ��ġ
            Obj.transform.position = new Vector3(
                transform.position.x,
                transform.position.y - 1.0f,
                0.0f);

            // ������Ʈ�� �̸� ����
            Obj.transform.name = "BossBullet";

           

        }
        else
            Anim.SetTrigger("Attack");// �Ϲ� ���� ���



        Active = false;
    }

    private void onWalk()
    {
        // �ȴ´�.
        Walk = true;
        //Movement = new Vector3(Speed + 1.0f, 0.0f, 0.0f);

        // ** ������ ����
        float Distance = Vector3.Distance(Target.transform.position, transform.position);

        // ������ ������ �Ÿ��� �ִ�
        if(Distance > 0.5f)
        {
            print("Walk");

            // �ൿ ���� ���� ���������� ������Ʈ�� �Ÿ��� ���
            Vector3 Direction = (Target.transform.position -  transform.position).normalized;

            // ������ ���� �̵�.
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

        // ������ ������ �Ÿ��� �ִ�
        if (Distance > 0.5f)
        {
            print("Slide");
            if (Anim.GetBool("PreSlide") == false)
                Anim.SetBool("PreSlide", true);

            Anim.SetBool("Slide", true);
            // �ൿ ���� ���� ���������� ������Ʈ�� �Ÿ��� ���
            Vector3 Direction = (EndPoint - transform.position).normalized;

            // ������ ���� �̵�.
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


