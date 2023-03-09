using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // �Ѿ� �ӵ�
    private float Speed;
    private float hp;

    public GameObject fxPrefab;
    //private SpriteRenderer spriteRenderer;
    //private Vector3 dst;
    //private float distance;

    // �Ѿ��� ������ ����
    public Vector3 Direction { get; set;} //�� ����
   
    private void Start()
    {
        hp = 1;
        //distance = 5.0f;
        //dst = transform.position;
        //spriteRenderer = this.GetComponent<SpriteRenderer>();

        // �ӵ� �ʱⰪ
        Speed = 10.0f;
    }
    void Update()
    {
        //if (Mathf.Abs(dst.x - transform.position.x) >= distance)
            //Destroy(this.gameObject);

        //spriteRenderer.flipY = Direction.x > 0 ? false : true; 

        // �������� �ӵ���ŭ ��ġ�� ����
        transform.position += Direction * Speed * Time.deltaTime;
    }

    // �浹ü�� ���������� ���Ե� ������Ʈ�� �ٸ� �浹ü�� �浹�Ѵٸ� ����Ǵ� �Լ�.

    // Enter : �浹�� ���� ������ 
    // Stay : �浹�� ���� �����Ӻ��� Exit������ ��������
    // Exit : �浹�� ���� �� �� ���� ������

    // Trigger : �浹 �������ϰ� �浹ü�� ����Ѵ�.
    // Collision : �浹 �������ϰ� �浹ü�� �浹�Ѵ�.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == ("wall"))
        {
            DestroyObject(this.gameObject);
            return;
        }
        --hp;

        GameObject Obj = Instantiate(fxPrefab);
        
        GameObject camera = new GameObject("Camera Test");
        camera.AddComponent<CameraController>();

        Obj.transform.position = transform.position;

        DestroyObject(collision.transform.gameObject);     
        print("Engter");

        if (hp == 0) { }
            //DestroyObject(this.gameObject);

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
