using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private int hitCount;
    private float Speed;
    private float CoolTime;
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
    private GameObject BulletPrefab;

    // ������ FX ����
    private GameObject fxPrefab;

    // dictianary<string, GameObject> asd;

    // ���ȭ�� ���� ����
    public List<GameObject> stageBack = new List<GameObject>();
    // �÷��̾ ���������� �ٶ� ����
    private float Direction;

    [Header("����")]
    // ** �÷��̾ �ٶ� ����
    [Tooltip("����")]
    public bool DirLeft;
    [Tooltip("������")]
    public bool DirRight;

   
    // ����Ƽ �⺻ ���� �Լ�
    // �ʱⰪ ���� �� �� ���
    private void Awake()
    {
        // Animator�� �޾ƿ´�.
        animator = this.GetComponent<Animator>();
        // SpriteRenderer�� �޾ƿ´�.
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        // [Resources] �������� ����� ���ҽ��� ���´�. "Resources" ��� ������ �����ؾ���
        BulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        //fxPrefab = Resources.Load("Prefabs/FX/Smoke") as GameObject;
        fxPrefab = Resources.Load("Prefabs/FX/Hit") as GameObject;
        //for (int i = 0; i < 7; ++i)
        //    stageBack.Add(GameObject.Find(i.ToString()));
        //stageBack.Add(GameObject.Find("0"));
        //stageBack.Add(GameObject.Find("1"));
        //stageBack.Add(GameObject.Find("2"));
        //stageBack.Add(GameObject.Find("3"));
        //stageBack.Add(GameObject.Find("4"));
        //stageBack.Add(GameObject.Find("5"));
        //stageBack.Add(GameObject.Find("6"));
    }
    void Start()
    {
        EnemyManager.GetInstance.CreateTest();

        // �ӵ� �ʱ�ȭ
        Speed = 5.0f;
        hitCount = 0;
        Direction = 1.0f;
        CoolTime = 0.5f;
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
        float Ver = Input.GetAxisRaw("Vertical");

        Movement = new Vector3(
          Hor * Time.deltaTime * Speed,
          0.0f,
          0.0f);

        // Hor�� 0�̶�� �����ִ� �����̹Ƿ� ����ó��
        if (Hor != 0)
            Direction = Hor;


        transform.position += new Vector3(0.0f, Ver * Time.deltaTime * 2.5f, 0.0f);

        if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D))
        {            
            // ** �÷��̾��� ��ǥ�� 0.0 ���� ���� �� �÷��̾ �����δ�. 
            if (transform.position.x < 0)
                transform.position += Movement;
            else
            {
                ControllerManager.GetInstance().DirRight = true;
                ControllerManager.GetInstance().DirLeft = false;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.A))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = true;
            
            // ** �÷��̾��� ��ǥ�� -15.0���� Ŭ ��
            if(transform.position.x > -15.0f)
                // ** ���� �÷��̾ �����δ�.
                transform.position += Movement;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)
            || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = false;
        }

        
        // �ٶ󺸰��ִ� ���⿡ ���� �̹��� �ø�����
        if (Direction < 0)
            spriteRenderer.flipX = DirLeft = true;       
        else if (Direction > 0)
            spriteRenderer.flipX = false;

        


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
            StartCoroutine(OnAttack());
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

    IEnumerator OnAttack()
    {
        if (onAttack == false)
        { 
            while (true)
            {
                // ���ݸ�� �������� �ƴҰ��
                // ���ݻ��� Ȱ��ȭ
                onAttack = true;
                // ���� ��� ����
                animator.SetTrigger("Attack");

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

                yield return new WaitForSeconds(CoolTime);
            }
        }
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(123);
    }


}


