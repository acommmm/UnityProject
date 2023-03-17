using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    const int STATE_WALK = 1;
    const int STATE_ATTACK = 2;
    const int STATE_SLIDE = 3;

    private GameObject Target;
    private Animator Anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 Movement;
    private Vector3 EndPoint;

    private int HP;
    private int choice;

    private float CoolDown;
    private float Speed;

    private bool Active;
    private bool SkillAttack;
    private bool Attack;
    private bool Walk;

    private void Awake()
    {
        Target = GameObject.Find("Player");
        Anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        HP = 30000;

        CoolDown = 1.5f;
        Speed = 0.5f;

        Active = true;
        SkillAttack = false;
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

        if(Active)
        {
            Active = false;
            choice = onController();
            //StartCoroutine(onCoolDown());
        }
        else
        {
            switch (choice)
            {
                case STATE_ATTACK:
                    onAttack();
                    break;

                case STATE_WALK:
                    onWalk();
                    break;

                case STATE_SLIDE:
                    onSlide();
                    break;
            }
        }
    }

    private int onController()
    {
        // ** �ൿ ���Ͽ� ���� ������ �߰�.
        {
            // ** �ʱ�ȭ
            if(Walk)
            {
                Movement = new Vector3(0.0f, 0.0f, 0.0f);
                //Anim.SetFloat("Speed", Movement.x);
                Walk = false;
            }
            if(SkillAttack)
                SkillAttack = false;
            if(Attack)
                Attack = false;
        }

        // ** ����



        // ** ���� �������� ���ϴ� ������ �÷��̾��� ��ġ�� ������������ ����
        EndPoint = Target.transform.position;



        // * return
        // * 1 : ����     STATE_WALK 
        // * 2 : �̵�     STATE_ATTACK
        // * 3 : �����̵� STATE_SLIDE 
        return Random.Range(STATE_WALK, STATE_SLIDE + 1);
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

        Active = true;
    }

    private void onWalk()
    {
        Walk = true;
        //Movement = new Vector3(Speed + 1.0f, 0.0f, 0.0f);

        // ** ������ ����
        float Distance = Vector3.Distance(EndPoint, transform.position);

        if(Distance > 0.5f)
        {
            Vector3 Direction = (EndPoint -  transform.position).normalized;

            Movement = new Vector3(
                Speed * Direction.x,
                Speed * Direction.y,
                0.0f);

            transform.position += Movement * Time.deltaTime;
            Anim.SetFloat("Speed", Mathf.Abs(Movement.x));
        }
        else
            Active = true;
    }

    private void onSlide()
    {
        Active = true;
    }

    
}


