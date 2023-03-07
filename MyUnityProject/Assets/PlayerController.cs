using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int hitCount;
    private float Speed;
    private float Fly;
    private Vector3 Movement;

    private Animator animator;

    private bool onAttack;
    private bool onHit;
    private bool onRoll;
    private bool onJump;
    private bool onDive;
    private bool onClimbing;

    // ����Ƽ �⺻ ���� �Լ�
    // �ʱⰪ ���� �� �� ���
    void Start()
    {
        // �ӵ� �ʱ�ȭ
        Speed = 5.0f;
        Fly = 0.0f;
        hitCount = 0;
        // Animator�� �޾ƿ´�.
        animator = this.GetComponent<Animator>();

        onAttack = false;
        onHit = false;
        onRoll = false;
        onJump = false;
        onDive = false;
        onClimbing = false;
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
            Ver * Time.deltaTime * Speed, 
            0.0f);

        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();

        if (Input.GetKey(KeyCode.Space))
            OnRoll();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            OnHit();

            if(hitCount >= 3)
                animator.SetInteger("HitCount", hitCount);
        }
        if (Input.GetKey(KeyCode.LeftAlt))
            OnJump();

        // ���������� y��ǥ �̵�
        if (onJump)
            Movement.y = 0.7f * Time.deltaTime * Speed;

        // ������ ������ ������ ������
        if (onDive)
        {
            Movement.y = -0.7f * Time.deltaTime * Speed;
            if (playerPosition().y <= 0) // ���� ������.(�浹 ����)
            {               
                SetDive();
                SetClimbing();
            }
            
        }
        
        if (playerPosition().x >= 1 && playerPosition().x <= 2) // �ö󰥼��ִ� ��ǥ�� ������
        {           
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


        animator.SetFloat("Fly", playerPosition().y);
        animator.SetFloat("Speed", Hor);
        transform.position += Movement;
  
    }

    private void OnAttack()
    {
        if (onAttack)
            return;

        onAttack = true;

        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
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
        hitCount = 0;
        animator.SetInteger("HitCount", hitCount);
    }      
    private Vector3 playerPosition()
    {     
        return this.gameObject.transform.position;
    }
}


