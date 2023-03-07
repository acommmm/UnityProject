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

    // 유니티 기본 제공 함수
    // 초기값 설정 할 때 사용
    void Start()
    {
        // 속도 초기화
        Speed = 5.0f;
        Fly = 0.0f;
        hitCount = 0;
        // Animator를 받아온다.
        animator = this.GetComponent<Animator>();

        onAttack = false;
        onHit = false;
        onRoll = false;
        onJump = false;
        onDive = false;
        onClimbing = false;
    }

    // 유니티 기본 제공 함수
    // 프레임마다 반복적으로 실행되는 함수.
    void Update()
    {
        // 실수 연산 IEEE 754 

        // Input.GeAxisRaw = -1, 0, 1 반환a
        // Input.GeAxis = -1.0f ~ 1.0f 반환
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

        // 점프누르면 y좌표 이동
        if (onJump)
            Movement.y = 0.7f * Time.deltaTime * Speed;

        // 점프가 끝나면 땅으로 떨어짐
        if (onDive)
        {
            Movement.y = -0.7f * Time.deltaTime * Speed;
            if (playerPosition().y <= 0) // 땅에 닿으면.(충돌 조건)
            {               
                SetDive();
                SetClimbing();
            }
            
        }
        
        if (playerPosition().x >= 1 && playerPosition().x <= 2) // 올라갈수있는 좌표에 있으면
        {           
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


