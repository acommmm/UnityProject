using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 Direction;
    private Vector3 Movement;

    private float speed;
    private void Awake()
    {
        targetPosition = GameObject.Find("Player").transform.position - new Vector3(0.0f, -0.3f, 0.0f); // 캐릭터 그림자 위치
    }
    void Start()
    {
        Direction = (targetPosition - transform.position).normalized;
        speed = 1.0f;
    }

    void Update()
    {
        Movement = new Vector3(
            speed * Direction.x,
            speed * Direction.y,
            0.0f);

        transform.position += Movement * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "wall" || collision.transform.tag == "Player")
            Destroy(gameObject);
    }
}
