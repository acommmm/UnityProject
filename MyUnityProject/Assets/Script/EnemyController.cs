using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private float Speed;
    public int HP;
    private Animator Anim;
    private Vector3 Movement;
    private GameObject Player;
    private GameObject SkillAttPrefab; 
    private float SkillCool;
    private float AttCool;
    private bool CoolDown;
    private bool SkillWalk;
    private bool AttCoolDown;
    //private bool SkillUse;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    void Start()
    {
        Player = GameObject.Find("Player").gameObject;
        SkillAttPrefab = Resources.Load("Prefabs/Enemy/EnemyBullet") as GameObject;
        CoolDown = true;
        AttCoolDown = true;
        SkillWalk = true;
        SkillCool = 10.0f;
        AttCool = 3.0f;

        Speed = 0.3f;
        Movement = new Vector3(Speed, 0.0f, 0.0f);
        HP = 3;
        //ddddddddddddddddSkillUse = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(
            Player.transform.position,
            transform.position);

        Movement = ControllerManager.GetInstance().DirRight ?
            new Vector3(Speed + 1.0f, 0.0f, 0.0f) : new Vector3(Speed, 0.0f, 0.0f);

        if (distance <= 2.0f)
        {
            if (Player.transform.position.x >= 0 && ControllerManager.GetInstance().DirRight)
                Movement = new Vector3(Speed + 1.0f, 0.0f, 0.0f);
            else
                Movement = new Vector3(0.0f, 0.0f, 0.0f);

            if (AttCoolDown == true)
            {
                Anim.SetTrigger("Attack");
                StartCoroutine(UseAtt());
            }
        }
        else if (distance < 5.0f && distance > 2.0f && SkillWalk == true)
        {
            if (Player.transform.position.x >= 0 && ControllerManager.GetInstance().DirRight)
                Movement = new Vector3(Speed + 1.0f, 0.0f, 0.0f);
            else
                Movement = new Vector3(0.0f, 0.0f, 0.0f);


            if (CoolDown == true)
            {
                Anim.SetTrigger("Skill");
                StartCoroutine(UseSkill());
            }
            
            
        }

        transform.position -= Movement * Time.deltaTime;
        Anim.SetFloat("Speed", Movement.x);
        Anim.SetFloat("Distance", distance);
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            --HP;
            if(HP <= 0)
            {
                Anim.SetTrigger("Die");
                GetComponent<CapsuleCollider2D>().enabled = false;
                //Destroy(gameObject, 0.016f);
            }
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject, 0.016f);
    }

    private void setSkill()
    {
        SkillWalk = false;
    }
    private void setSkillCool()
    {
        CoolDown = false;
    }

    private void attSkill()
    {
        print(123123123123);
        GameObject Obj = Instantiate(SkillAttPrefab);
        Obj.transform.position = new Vector3(Player.transform.position.x,
            Player.transform.position.y + 0.6f,
            0.0f);
    }

    private void setAttack()
    {
        AttCoolDown = false;
    }

    private IEnumerator UseSkill()
    {
        yield return new WaitForSeconds(SkillCool);
        SkillWalk = true;
        CoolDown = true;
    }
    private IEnumerator UseAtt()
    {
        yield return new WaitForSeconds(AttCool);
        print("ATTTT");
        AttCoolDown = true;
    }

}


