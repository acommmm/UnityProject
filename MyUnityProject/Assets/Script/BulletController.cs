using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // 총알 속도
    private float Speed;

    // 총알이 충돌한 횟수
    private float hp;

    // 이펙트효과 원본
    public GameObject fxPrefab;

    // 총알이 가야할 방향
    public Vector3 Direction { get; set;} //과 동일
   
    private void Start()
    {
        // 속도 초기값
        Speed = 10.0f;
        // 총알의 최대 충돌 횟수 지정
        hp = 2;
    }
    void Update()
    {
        // 방향으로 속도만큼 위치를 변경
        transform.position += Direction * Speed * Time.deltaTime;
    }

    // 충돌체와 물리엔진이 포함된 오브젝트가 다른 충돌체와 충돌한다면 실행되는 함수.

    // Enter : 충돌된 순간 프레임 
    // Stay : 충돌된 다음 프레임부터 Exit프레임 직전까지
    // Exit : 충돌이 끝난 후 그 다음 프레임

    // Trigger : 충돌 판정만하고 충돌체를 통과한다.
    // Collision : 충돌 판정을하고 충돌체와 충돌한다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌 횟수 차감.
        --hp;

        // 이펙트효과 복제
        GameObject Obj = Instantiate(fxPrefab);

        // 진동효과를 담당하는 관리자 생성
        GameObject camera = new GameObject("Camera Test");

        // 진동효과 생성
        camera.AddComponent<CameraController>();

        // 이펙트 효과의 위치를 지정
        Obj.transform.position = transform.position;

        // 충돌한 대상을 삭제
        if(collision.transform.tag != "wall" && collision.transform.tag != "BackGround")
            Destroy(collision.transform.gameObject);
        else
            Destroy(this.gameObject);
            
        if (hp == 0)
            Destroy(this.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        print("Stay");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print("Exit");
    }
}
