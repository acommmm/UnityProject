using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int hitCount; 
    private float Speed;
    private Vector3 Movement;

    private Animator animator;

    private bool onAttack;
    private bool onHit;
    private bool onRoll;
    private bool onJump;

    // 유니티 기본 제공 함수
    // 초기값 설정 할 때 사용
    void Start()
    {
        // 속도 초기화
        Speed = 5.0f;
        hitCount = 0;
        // Animator를 받아온다.
        animator = this.GetComponent<Animator>();

        onAttack = false;
        onHit = false;
        onRoll = false;
        onJump = false;
    }

    // 유니티 기본 제공 함수
    // 프레임마다 반복적으로 실행되는 함수.
    void Update()
    {
        // 실수 연산 IEEE 754 

        // Input.GeAxisRaw = -1, 0, 1 반환
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

        Movement.y += 1; 
    }

    private void SetJump()
    {
        onJump = false;
    }
    private void ResetCount()
    {
        hitCount = 0;
        animator.SetInteger("HitCount", hitCount);
    }       
}


