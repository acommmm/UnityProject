using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // 총알 속도
    private float Speed;
    //private SpriteRenderer spriteRenderer;
    //private Vector3 dst;
    //private float distance;

    // 총알이 가야할 방향
    public Vector3 Direction { get; set;} //과 동일
   
    private void Start()
    {
        //distance = 5.0f;
        //dst = transform.position;
        //spriteRenderer = this.GetComponent<SpriteRenderer>();

        // 속도 초기값
        Speed = 10.0f;
    }
    void Update()
    {
        //if (Mathf.Abs(dst.x - transform.position.x) >= distance)
            //Destroy(this.gameObject);

        //spriteRenderer.flipY = Direction.x > 0 ? false : true; 

        // 방향으로 속도만큼 위치를 변경
        transform.position += Direction * Speed * Time.deltaTime;
    }

    // 충돌체와 물리엔진이 포함된 오브젝트가 다른 충돌체와 충돌한다면 실행되는 함수.

    // Enter : 충돌된 순간 프레임 
    // Stay : 충돌된 다음 프레임부터 Exit프레임 직전까지
    // Exit : 충돌이 끝나기 직전 프레임

    // Trigger : 충돌 판정만하고 충돌체를 통과한다.
    // Collision : 충돌 판정을하고 충돌체와 충돌한다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyObject(this.gameObject);   
    }
}
