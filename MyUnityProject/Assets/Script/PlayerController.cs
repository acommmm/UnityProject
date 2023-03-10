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
    // �÷��̾� Animator �������
    private Animator animator;
    // �÷��̾� SpriteRenderer �������
    private SpriteRenderer spriteRenderer;

    // ���� üũ
    private bool onAttack; // ����
    private bool onHit; // �ǰ�
    private bool onRoll; // ������
    private bool onJump; // ����
    private bool onDive; // �߶�
    private bool onClimbing; // ���

    // ������ �Ѿ��� ���� ����
    public List<GameObject> Bullets = new List<GameObject>(); 

    // ������ �Ѿ� ����
    public GameObject BulletPrefab;

    // ������ FX ����
    public GameObject fxPrefab;

    public GameObject[] stageBack = new GameObject[7];
    // �÷��̾ ���������� �ٶ� ����
    private float Direction;

    public bool DirLeft;
    public bool DirRight;

    // ����Ƽ �⺻ ���� �Լ�
    // �ʱⰪ ���� �� �� ���
    private void Awake()
    {
        // Animator�� �޾ƿ´�.
        animator = this.GetComponent<Animator>();
        // SpriteRenderer�� �޾ƿ´�.
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        // �ӵ� �ʱ�ȭ
        Speed = 5.0f;
        hitCount = 0;
        Direction = 1.0f;

        for (int i = 0; i < 7; ++i)
            stageBack[i] = GameObject.Find(i.ToString());

        // �ʱⰪ ����
        onAttack = false;
        onHit = false;
        onRoll = false;
        onJump = false;
        onDive = false;
        onClimbing = false;

        DirLeft = false;
        DirRight = false;
    }

    // ����Ƽ �⺻ ���� �Լ�
    // �����Ӹ��� �ݺ������� ����Ǵ� �Լ�.
    void Update()
    {
        // �Ǽ� ���� IEEE 754 
        // Input.GeAxisRaw = -1, 0, 1 ��ȯa
        // Input.GeAxis = -1.0f ~ 1.0f ��ȯ
        float Hor = Input.GetAxisRaw("Horizontal");
        float Ver = 0;
        //float Ver = Input.GetAxisRaw("Vertical");



        // Hor�� 0�̶�� �����ִ� �����̹Ƿ� ����ó��
        if (Hor != 0)
            Direction = Hor;
        else
        {
            DirLeft = false;
            DirRight = false;
        }

        // �ٶ󺸰��ִ� ���⿡ ���� �̹��� �ø�����
        if (Direction < 0)
        {
            spriteRenderer.flipX = DirLeft = true;
            // ���� �÷��̾ �����δ�.
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
        


        ////�����̰� ������ Run�ִϸ��̼� ����
        //if (Hor < 0) // �������� ���� �ø�
        //{
        //    checkRun = 1.0f; // �����̰� �ִٴ� �ǹ� Run �ִϸ��̼� ����
        //    spriteRenderer.flipX = true;
        //}
        //else if (Hor > 0) // ���������� ���� �ø� ����
        //{
        //    checkRun = 1.0f; // �����̰� �ִٴ� �ǹ� Run �ִϸ��̼� ����
        //    spriteRenderer.flipX = false;
        //}
        //else
        //    checkRun = 0.0f;  // �¿� ��ȭ�� ���ٴ� �ǹ� Idle �ִϸ��̼� ����
        ////spriteRenderer.flipX = Hor < 0 ? true : false;

        // �Է¹��� ������ �÷��̾� ������
        Movement = new Vector3(
           Hor * Time.deltaTime * Speed,
           Ver * Time.deltaTime * Speed,
           0.0f);

        // ���� ��Ʈ�� Ű �Է½� 
        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();// ����

        // zŰ �Է½�
        if (Input.GetKey(KeyCode.Z))
            OnRoll();// ������
        
        // ���� ����Ʈ Ű �Է½�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            OnHit(); // �ǰ� �߻�

            if(hitCount >= 3)
                animator.SetInteger("HitCount", hitCount); // ���
        }
        // ���� ��Ʈ �Է½�
        if (Input.GetKey(KeyCode.LeftAlt))
        {//OnJump();// ����
         }

        // �����̽��� ������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �Ѿ� ������ ����
            GameObject Obj = Instantiate(BulletPrefab);
            //Obj.transform.name = "Bullet";
            // ������ �Ѿ� ��ġ�� �÷��̾� ��ġ�� ����
            Obj.transform.position = transform.position;
            // �Ѿ��� BulletController ��ũ��Ʈ �޾ƿ�
            BulletController Controller = Obj.AddComponent<BulletController>();
            
            // �Ѿƾ� �� SpriteRenderer�� �޾ƿ�
            SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

            // �Ѿ� ��ũ��Ʈ ������ ���� ������ ���� �÷��̾��� ���� ������ �ʱ�ȭ
            Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

            // �Ѿ� ��ũ��Ʈ ������ Fx Prefab�� ����
            Controller.fxPrefab = fxPrefab;
            //Controller.Direction = spriteRenderer.flipX ? transform.right * -1 : transform.right;
            // �Ѿ� �̹��� �������¸� �÷��̾��� �̹��� ���� ���·� ����
            renderer.flipY = spriteRenderer.flipX;
   
            // ��� ���� ���� �� ����ҿ� ����
            Bullets.Add(Obj);
        }
          
        // ���������� y��ǥ �̵�
        if (onJump)
            //Movement.y = 0.7f * Time.deltaTime * Speed;

        //  �������� ���̸� 
        if (onDive)
        {
            //Movement.y = -0.7f * Time.deltaTime * Speed;
            //if (playerPosition().y <= 0) // ���� ������.(�浹 ����)
            {              
                // ����
                //SetDive();
                //SetClimbing();
            }         
        }

        // �ö󰥼��ִ� ��ǥ�� ������
        if (playerPosition().x >= 1 && playerPosition().x <= 2) 
        {
            Ver = Input.GetAxisRaw("Vertical");
            if(onJump)
            {
                SetJump();
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) // �ö� ��ġ���� ������Ű������
            {
                OnClimbing(); // �ִϸ��̼� ����
            }
        }
        else if(playerPosition().x > 2 || playerPosition().x < 1)
        {
            if(onClimbing)
                OnDive();
        }

        // ���� Y��ǥ�� �ִϸ��̼ǿ� ����
        animator.SetFloat("Fly", playerPosition().y);
        // �÷��̾� �����ӿ� ���� �̵� ��� ����
        animator.SetFloat("ChechRun", Hor);
        //animator.SetFloat("ChechRun", checkRun);

        // ���� �÷��̾ ������
        



    }

    private void OnAttack()
    {
        // ���ݸ�� �������ϰ��
        if (onAttack)
            return; // �Լ� ����

        // ���ݸ�� �������� �ƴҰ��
        // ���ݻ��� Ȱ��ȭ
        onAttack = true;
        // ���� ��� ����
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        // �Լ��� ����Ǹ� ���ݻ��� ��Ȱ��ȭ
        // �Լ��� �ִϸ��̼� Ŭ���� �̺�Ʈ ���������� ���Եȴ�.
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
        // ü���� �ʱ�ȭ
        hitCount = 0;
        // �ʱ�ȭ�� ü�� �ִϸ��̼ǿ� ����
        animator.SetInteger("HitCount", hitCount);
    }      
    private Vector3 playerPosition()
    {     
        // �÷��̾��� ��ġ�� �޾ƿ´�.
        return this.gameObject.transform.position;
    }
    private void sholve()
    {
        // �Ѿ� ������ ����
        GameObject Obj = Instantiate(BulletPrefab);
        //Obj.transform.name = "Bullet";
        // ������ �Ѿ� ��ġ�� �÷��̾� ��ġ�� ����
        Obj.transform.position = transform.position;
        // �Ѿ��� BulletController ��ũ��Ʈ �޾ƿ�
        BulletController Controller = Obj.AddComponent<BulletController>();

        // �Ѿƾ� �� SpriteRenderer�� �޾ƿ�
        SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

        // �Ѿ� ��ũ��Ʈ ������ ���� ������ ���� �÷��̾��� ���� ������ �ʱ�ȭ
        Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

        // �Ѿ� ��ũ��Ʈ ������ Fx Prefab�� ����
        Controller.fxPrefab = fxPrefab;
        //Controller.Direction = spriteRenderer.flipX ? transform.right * -1 : transform.right;
        // �Ѿ� �̹��� �������¸� �÷��̾��� �̹��� ���� ���·� ����
        renderer.flipY = spriteRenderer.flipX;




        // ��� ���� ���� �� ����ҿ� ����
        Bullets.Add(Obj);
    }


}


