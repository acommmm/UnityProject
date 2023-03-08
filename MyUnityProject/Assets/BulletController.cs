using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // �Ѿ� �ӵ�
    private float Speed;
    //private SpriteRenderer spriteRenderer;
    //private Vector3 dst;
    //private float distance;

    // �Ѿ��� ������ ����
    public Vector3 Direction { get; set;} //�� ����
   
    private void Start()
    {
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
    // Exit : �浹�� ������ ���� ������

    // Trigger : �浹 �������ϰ� �浹ü�� ����Ѵ�.
    // Collision : �浹 �������ϰ� �浹ü�� �浹�Ѵ�.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyObject(this.gameObject);   
    }
}
